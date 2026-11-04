# Construction PMS — ERP Capability Research & Requirements

**Research date:** 2026-11-03
**Scope:** Research and requirements definition only. No software, prototype or implementation code is part of this document.
**Method:** One research agent per ERP (8 agents) browsed current vendor documentation, release notes and official marketplace listings; the lead agent reconciled conflicts and wrote this report. Third-party / community sources are marked as such.

**How to read this document**

| Marker | Meaning |
|---|---|
| **[Fact]** | Stated in vendor documentation, release notes or official marketplace listing (URL given). |
| **[Claim]** | Vendor marketing or third-party listing, not confirmed in product documentation. |
| **[Unverified]** | Could not be confirmed from public sources; must be validated in demo / sandbox. |
| **[Analysis]** | Lead-agent interpretation of the facts. |
| **[Recommendation]** | Lead-agent recommendation. |

---

> **Addendum (2026-11-03), consistency with the requirements package**
> - **This file is research evidence only.** Approved and proposed business decisions are in `business-decisions.md`. Requirements are in `requirements.md` (v1.0-draft, *Draft for business and finance validation*).
> - **Superseded sections:**
>   - §7 (conceptual blueprint) and §8 (feature priorities) are superseded by requirements.md §1–§13.
>   - The double-counting formula in §1 finding 2 is refined in requirements.md §3.1–3.5: three separate reconciliations, and ETC excludes remaining commitments.
> - **§9 shortlist:** this is a research scoring exercise under the assumptions in §9.4. It is **not an ERP selection**. The business direction (Q17-P1) is to extend or develop our PMS and integrate it with an accounting ERP, subject to feasibility. ERP selection requires explicit instruction.
> - **Corrections applied:**
>   - CR-3: "not found in public documentation" is now distinguished from "not supported" (§1 finding 5, §5 legend and cells, §7.3 R-B5).
>   - The country, hosting and intercompany research added later is in `statutory-research-india-uae.md` (Parts A–G) and `hosting-research.md`.
> - **Vendor conflicts:** the conflicts in §10.2 are now checks VD-01…VD-16 in `vendor-demo-checklist.md`, which has 32 checks in total, all Pending verification.

## 1. Executive summary and key findings

**Objective.** Determine how eight ERPs support a construction company running many projects and subprojects with reliable budget-versus-actual (BvA) reporting, and turn the findings into requirements for our own construction PMS.

**Key findings**

1. **Construction-native or construction-edition products are clearly deeper on cost control than horizontal ERPs.** CMiC, IFS Cloud, Infor CloudSuite Engineering & Construction (LN) and Acumatica Construction Edition all document a distinct *committed* bucket that is relieved as cost becomes actual, plus a forecast-at-completion formula. Odoo, Zoho and Business Central either lack a committed column in project reports or lack a forecast/EAC field. [Analysis]
2. **The correct BvA formula is consistent across the deep products** [Analysis, derived from vendor formulas]:
   `Forecast at completion = Actual + (Revised commitment − Commitment already converted to actual) + Estimate to complete for uncommitted scope`
   In the documentation reviewed for the five vendors that describe commitment relief (IFS, Infor, SAP, CMiC and Acumatica, see §6), double counting is avoided by *relieving* the commitment by the amount that becomes actual. The other three vendors' documentation does not describe this mechanism at the same level.
3. **Subcontract management with progress claims, retention and variations is the biggest differentiator.** It is native in CMiC, IFS Cloud, Infor LN (Services Procurement) and Acumatica Construction Edition; basic in SAP (service entry sheets + down-payment chains, with partner add-ons for running accounts); absent in Odoo, Zoho and Business Central without third-party apps. [Fact, see §4]
4. **Change orders that flow into budget, commitments and billing** are native in CMiC (four PCI classes), IFS (Contract Change Orders), Acumatica (change orders/change requests), Infor (Extensions + Budget Adjustments) and SAP (budget supplements/returns/transfers + CPM change management). [Fact]
5. **A controlled contingency-drawdown object was not found in the public documentation reviewed for any of the eight vendors.** This is a finding about the documentation, not proof that the capability is missing. What was found for each vendor:
   - **Infor:** documents budget-line contingency as "for information only".
   - **IFS:** describes "Estimated Future Commitment" in the EAC Workbench as covering contingencies. Whether there is a reserve object is Unverified.
   - **CMiC:** no distinct object was found (Unverified).
   - **SAP and Acumatica:** no dedicated object was found. Research agents suggested modelling contingency as a budget line or WBS element. That is an analyst workaround, not vendor documentation.
   - **Odoo, Zoho and Business Central:** no contingency feature was found.
   
   All of these must be confirmed in demos (vendor-demo-checklist VD-02 for IFS; others to be added if those vendors are shortlisted). [Fact for what was found; Analysis for the conclusion]
   *Corrected 2026-11-03: the earlier wording overstated this as a uniform modelling practice.*
6. **Payroll is the weakest common area outside North America.** CMiC and Acumatica payroll are US/Canada; IFS follows an integration strategy; Infor LN has no native payroll; Business Central needs localisation apps. Plan for payroll integration rather than replacement. [Fact/Unverified per country]
7. **Pricing:** only Odoo, Zoho and Business Central publish list prices. SAP, Infor, IFS, CMiC and Acumatica are **Quote required**. [Fact]

**Shortlist (best fit for construction cost control):** **1. CMiC**, **2. IFS Cloud**, **3. Infor CloudSuite Engineering & Construction (LN)**. SAP S/4HANA (PS) and Acumatica Construction Edition are close runners-up. See §9 for criteria, evidence and trade-offs. [Recommendation]

---

## 2. Construction business requirements and terminology

### 2.1 Business context (assumed)
A contractor executing multiple concurrent projects, each split into subprojects (blocks, towers, sites, packages), using own labour, subcontractors, purchased materials and owned/rented equipment, billing the client by progress, and needing reliable monthly cost reports (budget vs committed vs actual vs forecast) per project, subproject and cost code.

### 2.2 Terminology

| Term | Meaning used in this report |
|---|---|
| **WBS** (Work Breakdown Structure) | Hierarchical decomposition of project scope: project → subproject → phase → work package/activity → task. |
| **CBS / Cost code** | Cost breakdown structure: classification of cost (e.g., 03-Concrete, 05-Steel) independent of WBS. Cost is usually captured at the intersection **WBS × cost code × cost type**. |
| **Cost type / category** | Material, Labour, Equipment, Subcontract, Overhead/Other. |
| **Original budget** | Budget approved at contract award (baseline). Never overwritten. |
| **Revised / current budget** | Original + approved changes ± transfers. |
| **Budget transfer** | Move budget between lines without changing project total. |
| **Contingency** | Reserve budget held for risk; drawn down via approved transfers. |
| **Change order (CO) / Variation** | Approved scope change with client (external) or internal re-allocation. Often preceded by a Potential Change Order (PCO) / Change Request. |
| **Commitment** | Contracted but not yet incurred cost: open PO, subcontract or rental agreement. |
| **Open commitment** | Commitment − amount already converted to actual (received/invoiced/certified). |
| **Actual cost** | Cost incurred and posted: issued materials, approved timesheets, certified subcontract claims, posted vendor invoices, equipment charges. |
| **Accrual** | Cost incurred but not yet invoiced (e.g., goods received not invoiced, work done not certified). |
| **ETC** (Estimate to Complete) | Forecast remaining cost. |
| **EAC / FAC** (Estimate / Forecast at Completion) | Actual + open commitments + ETC for uncommitted scope. |
| **Variance at completion (VAC)** | Revised budget − EAC. |
| **Progress claim / Payment application / Valuation** | Periodic claim of work done by a subcontractor (payable) or to the client (receivable). |
| **Retention / Retainage** | Percentage withheld from progress payments until completion / defects period. |
| **WIP / Over-under billing** | Difference between revenue earned (by % complete) and amount billed. |
| **Earned value (EV)** | BCWS (planned value), BCWP (earned value), ACWP (actual cost); CPI/SPI. |
| **AIA G702/G703** | US standard progress billing forms (schedule of values). |
| **BoQ** | Bill of Quantities — priced scope used in estimating, client contract and subcontracts. |
| **DPR** | Daily Progress Report from site. |

### 2.3 Core business requirements (summary)
1. Multi-project, multi-subproject hierarchy with consistent roll-up.
2. Budget lifecycle with versioning, approvals, transfers, contingency and change orders.
3. Analytical dimensions: project, subproject, site, department, cost centre, cost code, cost type.
4. Material lifecycle from estimate to consumption/return/wastage at site.
5. Labour: crews, timesheets, rates, overtime, payroll interface, cost allocation.
6. Subcontract lifecycle: contract, commitment, claim, certification, retention, variation, payment.
7. Equipment: allocation, utilisation, internal and external rental charges.
8. Cost control: committed, actual, accrual, ETC, EAC, variance — without double counting.
9. Client billing: milestones, progress/valuation billing, retention, revenue recognition.
10. Governance: roles, approval workflows, audit trail, reporting.

---

## 3. Selection of the three additional ERPs

**Preliminary research** (comparison sites, analyst-style roundups; mostly third-party with commercial interest, therefore treated as directional only) repeatedly named: CMiC, Trimble Viewpoint Vista, Acumatica Construction Edition, Sage Intacct Construction, IFS Cloud, Oracle (NetSuite / Construction & Engineering), Procore (PM platform, not ERP).
Sources: https://erpresearch.com/en-us/top-10-construction-erp-software-report ; https://blog.ifs.com/ifs-cloud-the-ideal-erp-solution-for-construction-and-engineering-companies/ ; https://www.netatwork.com/resource/construction-erp-comparison-acumatica-trimble-viewpoint

**Selection criteria:** (a) an ERP (not a PM-only tool); (b) construction-specific job costing, commitments and subcontract management documented by the vendor; (c) publicly accessible documentation so claims can be verified; (d) coverage of different market segments; (e) no overlap with the five named vendors.

| Selected | Why |
|---|---|
| **CMiC (CMiC Platform R12 / "Nexus")** | Construction-native, single-database ERP for mid-to-large contractors. Large public documentation portal (docs.cmicglobal.com) documents committed-cost logic, four-class change management, subcontract retention/compliance, equipment charge-out and WIP. Represents the "built for construction" category. |
| **Acumatica Cloud ERP — Construction Edition** | First-party construction edition on a general mid-market ERP; documented commitments, change orders, AIA billing, retainage and cost projections; unlimited-user pricing model. Represents the "general ERP + vendor's own construction edition" category. |
| **IFS Cloud** | Project-based ERP used by engineering/EPC contractors internationally; documented value-stage model (planned committed → committed → used → actual), EAC Workbench, sub-contract valuations with retention, contract change orders and project inventory. Represents the "global project-based enterprise ERP" category. |

**Considered but not selected:** Trimble Viewpoint Vista and Sage Intacct Construction — strong job costing but largely US-centric (payroll, AIA, certified payroll) with more gated documentation; Oracle NetSuite / Oracle Construction & Engineering — portfolio split between ERP and separate construction products (Aconex, Primavera, Textura) that would complicate like-for-like comparison; Procore — project management platform that relies on an ERP for accounting. [Analysis]

---

## 4. Detailed findings per ERP

> Labels used: **Native**, **First-party add-on**, **Third-party extension**, **Customization required**, **Not supported** (only where vendor documentation states the limitation), **Not found in public docs** (no evidence either way), **Unverified**.
> *Corrected 2026-11-03:* matrix cells that said "Not supported" without a documented vendor statement are relabelled "Not found in public docs".

### 4.1 Odoo

**Product / edition / deployment.** Odoo 19.0 (saas-19.x and 20.0 docs also exist); Community (LGPL) vs Enterprise. Deployment: Odoo Online, Odoo.sh, on-prem. Third-party Apps Store modules cannot run on Odoo Online — they require Odoo.sh or on-prem (Custom plan). [Fact] https://www.odoo.com/pricing
First-party **"General Contractor" industry package** (`construction`, Enterprise, OEEL-1) mostly *configures* standard apps (Project, Timesheets, Planning, Purchase, Inventory, Sales, Invoicing, Field Service, Documents…). [Fact] https://apps.odoo.com/apps/modules/saas-19.1/construction

**Relevant modules.** Project, Accounting (analytic plans, Budgets), Purchase, Inventory, Timesheets, Planning, Payroll, Expenses, Fleet, Maintenance, Approvals, Studio.

**Key findings**
- Hierarchy: project → stages → tasks → sub-tasks, milestones. No native subproject/WBS code object. [Fact] https://www.odoo.com/documentation/19.0/applications/services/project/project_management/project_milestones.html
- Budgets: analytic/financial budgets with Draft/Open/Revised; "Revise" creates a new revision. No approval, transfer, contingency or change-order object documented. [Fact] https://www.odoo.com/documentation/19.0/applications/finance/accounting/reporting/budget.html
- Dimensions: multiple analytic plans (project, department, site, cost code), percentage distribution, mandatory rules. Strong. [Fact] https://www.odoo.com/documentation/19.0/applications/finance/accounting/reporting/analytic_accounting.html
- Cost control: Budget report shows **Budgeted, Committed (Achieved + confirmed unbilled POs), Achieved**. PO over-budget is highlighted (warning, not blocking). No EAC, no project accruals. [Fact]
- Subcontracts, retention, progress billing, equipment rental charging: **Third-party** (e.g., SDLC Construction Management, Probuse/Grevlin retention). [Claim] https://apps.odoo.com/apps/modules/19.0/sdlc_construction_management

**Strengths:** flexible analytic dimensions; native "Committed" in budgets; low cost; one database.
**Limitations:** no subproject/WBS codes; no CO/contingency/transfer; no subcontract claims; no EAC/WIP; commitment covers POs only.
**Dependencies:** Enterprise edition; Odoo.sh/on-prem for third-party modules; quality of third-party modules varies.
**Pricing:** Public, geo-localised. Fetched in INR: Standard ₹580/user/month (yearly), Custom ₹1,080/user/month (yearly). US figures from third-party trackers: Unverified. Implementation: Quote required. https://www.odoo.com/pricing
**Risks to test [Unverified]:** storable material may hit the project twice (vendor-bill analytic + consumption analytic); payroll analytic + timesheet cost could double-count labour.

### 4.2 Zoho (multiple applications)

**Product.** No construction ERP. A bundle of SaaS apps, region-specific editions (US, IN, UAE…). Zoho's construction pages market general apps. [Fact] https://www.zoho.com/books/industry/construction-accounting-software/

| App | Contribution |
|---|---|
| Zoho Projects | WBS (phases → task lists → tasks → multi-level subtasks), project/phase/task budgets (cost, revenue, hours), labour-based forecast, timesheets. |
| Zoho Books | Ledger; its own *separate* project record; line-level project tagging of POs/bills/expenses/journals; reporting tags (up to 5-level hierarchy); Locations add-on; GL budgets; progress invoices; retainer invoices; payment retention; fixed assets; multi-level approvals. |
| Zoho Inventory | Warehouses, transfer orders with approval. No project consumption concept. |
| Zoho People / Payroll / Expense | Timesheets & attendance; payroll journals to Books with tags; expense claims to Books. |
| Zoho Analytics | Cross-app BI — where a BvA + committed report must be built. |
| Zoho Creator / Flow | Low-code: subcontracts, variations, equipment logs must be built here. |
| Zoho ERP (India, Jan 2026) | No project/construction module documented. [Unverified for projects] |

**Integration map.** Projects → Books "Finance Suite": projects, tasks, users and **only approved billable** timesheets sync one way; requires a primary customer; Projects budgets/phases are **not documented** to sync to Books project budgets (two disconnected project models). [Fact] https://help.zoho.com/portal/en/kb/projects/integration/zoho-apps/articles/zoho-finance-integration

**Key findings**
- Cost control: Books Project Cost Summary counts bills, vendor credits, expenses, journals and timesheet "staff cost"; **POs are excluded** → committed cost is invisible natively. [Fact] https://www.zoho.com/en-fr/books/help/projects/functions.html
- Forecast in Projects = ((100 − % complete) × planned cost)/100 + actual — labour-centric. [Fact] https://help.zoho.com/portal/kb/articles/project-budget
- Billing: progress invoices from quotes, retainers, payment retention (retention shown as early-access in one help version — conflict). [Fact/Conflict] https://www.zoho.com/books/help/payment-retention/

**Strengths:** very low cost; good dimensions (tags/locations); construction-friendly billing; approvals.
**Limitations:** no subprojects, CO, transfers, contingency, commitments, subcontract claims, equipment costing, material-to-WBS consumption.
**Dependencies:** Projects Premium+ and Books Professional+ for integration; Analytics and Creator for construction objects.
**Pricing:** Books US $15–$240/org/month (annual) by plan; Locations $10/location/month; Zoho One $37/user/month (all-employee) or $90 (flexible). Projects/Creator/Analytics prices not captured from official pages → Unverified. https://www.zoho.com/us/books/pricing/ ; https://www.zoho.com/one/pricing.html
**Double-counting risk:** labour counted both as timesheet staff cost and as tagged payroll journals.

### 4.3 SAP — S/4HANA (Private Cloud / on-prem) with Project System

**Product selected:** **SAP S/4HANA (RISE Private Cloud or on-prem) with Project System (PS), Commercial Project Management (CPM), Controlling, MM, SD, FI, EBRR, CATS.** Chosen because only this edition has networks/activities, full PS budget lifecycle and FI down-payment chains with retention. [Analysis]
**Alternative:** S/4HANA Cloud Public Edition "Enterprise Projects" — documented gaps: no networks; milestones not linked to billing plan (KBA 3547457); no customer down-payment invoices (KBA 3725325); customer retention via G/L workaround (KBA 3731931). [Fact] https://userapps.support.sap.com/sap/support/knowledge/en/3547457
**EC&O:** "SAP for Engineering, Construction & Operations" is an industry portfolio, not a separate licensed module. [Unverified]
**Partner add-ons (Third-party):** Syntax Progress Billing, Syntax Project Change Order Management, MotiveMinds Myra (measurement books, running accounts, retention), LTIMindtree Project PayChain. [Claim] https://www.sap.com/products/financial-management/partners/syntax-systems-usa-lp-progress-billing.html

**Key findings**
- Hierarchy: project definition → multi-level WBS → networks/activities/milestones. [Fact]
- Budget: original, supplements, returns, transfers; release status; **availability control** (warning / mail / hard error) on actual + commitments. [Fact] https://userapps.support.sap.com/sap/support/knowledge/en/3458767
- Commitments from PR/PO assigned to WBS; relieved on valuated GR or on IR. [Fact]
- Subcontracting: service POs + service entry sheets as progress acceptance; down-payment chains with retention (on-prem FI). Running-account bills via partners. [Fact/Third-party]
- Revenue recognition: Results Analysis (legacy) or Event-Based Revenue Recognition with POC/WIP. [Fact] https://blogs.sap.com/2022/10/05/event-based-revenue-recognition-for-universal-parallel-accounting-in-sap-s-4hana-2022/
- Equipment/plant hire: no construction module — activity allocation from cost centres; **Customization required**. [Fact]

**Strengths:** hard budget enforcement; integrated commitments; real-time POC/WIP; strong audit and authorisations.
**Limitations:** no native construction progress-claim/payment-certificate workflow or AIA; claim management deprecated → CPM; no plant hire; heavy, partner-led implementation.
**Dependencies:** HCM/SuccessFactors payroll; CPM; SAC; partner add-ons.
**Pricing:** **Quote required.**

### 4.4 Microsoft Dynamics 365 Business Central

**Product.** Business Central 2026 release wave 2 (v29.0, Oct 2026); "Jobs" renamed "Projects". Essentials covers all project functions (Premium adds manufacturing/service only). SaaS or on-prem. [Fact] https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/whatsnew/whatsnew-update-29-0
Construction AppSource extensions (Third-party): ProjectPro/Construction365 (Netsmartz), Construction-Centric (RSM), NB Projects (Cetas), NavBiz, VanRoey Construction & Installation, COSMO Project Construction. "Clarity Construction", "Construction Cloud", "BuildingPoint" — **not found** for BC. [Claim/Unverified]

**Key findings**
- Hierarchy: Project → Project Tasks (Posting/Heading/Total/Begin-Total/End-Total) → Planning Lines. No subproject object. [Fact] https://learn.microsoft.com/en-us/dynamics365/business-central/projects-how-create-jobs
- Budget: planning lines (Budget / Billable / Both); Project Archive versions (dimensions not archived). No native CO, contingency, transfer, budget approval. [Fact] https://learn.microsoft.com/en-us/dynamics365/release-plan/2024wave1/smb/dynamics365-business-central/archive-projects
- Dimensions: 2 global + 8 shortcut, unlimited dimensions, mandatory rules. [Fact] https://learn.microsoft.com/en-us/dynamics365/business-central/finance-dimensions
- Materials: project POs, receipts written off to project, warehouse picks, transfers, Usage Link. Project ledger entry created **only when invoice is posted**. [Fact] https://learn.microsoft.com/en-us/dynamics365/business-central/projects-how-manage-project-supplies
- Cost control: task lines show Outstanding Orders and Amt. Rcd. Not Invoiced; Project Actual to Budget reports; **no EAC/ETC field**. [Fact]
- Billing: five WIP methods (Cost Value, Cost of Sales, Sales Value, POC, Completed Contract). **No retention, no AIA**. [Fact] https://learn.microsoft.com/en-us/dynamics365/business-central/projects-understanding-wip
- Payroll: not native. [Fact]

**Strengths:** mature ledger-based job costing; strong dimensions; WIP methods; Power BI Projects app; affordable.
**Limitations:** two-level WBS; no CO/contingency/subcontract/retention/EAC natively; no payroll.
**Pricing:** Essentials $80, Premium $110, Team Members $8 per user/month (US list, annual). AppSource apps: Quote required. https://www.microsoft.com/en-us/dynamics-365/products/business-central/pricing

### 4.5 Infor — CloudSuite Engineering & Construction (on Infor LN)

**Product selected:** **Infor CloudSuite Engineering & Construction (CSEC)**, multi-tenant SaaS, core application **Infor LN** (Project TP module, Project Contract Management, Services Procurement), with Infor OS/ION and Birst. Infor's own docs name LN as the core; other Infor ERPs (CloudSuite Industrial, M3, SunSystems) are not positioned for E&C and were not evaluated. [Fact] https://docs.infor.com/csec/2026.x/en-us/cseclib/csecpov/oak1623337857554.html
Note: Infor EAM was sold to Hexagon (2021) → HxGN EAM is **Third-party**. [Fact] https://hexagon.com/company/newsroom/press-releases/2021/hexagon-acquire-infors-eam-business

**Key findings**
- Hierarchy: main projects/subprojects; element structure and/or activity structure (WBS element, control account, work package, milestone). [Fact] https://docs.infor.com/ln/10.8/en-us/lnolh/tpolh/tpom000024.html
- Budget: frozen budgets changed via coded Budget Adjustments (reason, e-signature); BCA version snapshots; change orders as **Extensions**; contingency "for information only"; no explicit transfer function found. [Fact/Unverified] https://docs.infor.com/ln/latest/en-us/lnolh/tpbudug/edc1631356681441.html
- Subcontracting: Subcontracting Procurement Orders with **Application for Payment (applied vs assessed, payless)**, retention % with cap and staged release, variations, certificate of payment. Requires Services Procurement concept activation. [Fact] https://docs.infor.com/ln/2026.x/en-us/lnolh/procspoug/cover.html
- Cost control: **soft commitments → hard commitments → actuals**, commitment offset by same amount as cost; EAC/ETC/EEC forecast methods, VAC, EVM. Labour has no commitments. [Fact] https://docs.infor.com/ln/2022.x/en-us/lnolh/tpolh/tpom000096.html
- Billing: cost-plus, unit-rate, progress, installments, holdback (retention), POC revenue recognition. [Fact] https://docs.infor.com/ln/2026.x/en-us/lnolh/tpolh/tpom000339.html
- Equipment: equipment cost objects, budget lines, cost transactions, Equipment Procurement Orders for hire. [Fact]
- Payroll: LN has no native payroll ("Data to Payroll" export). [Fact]

**Strengths:** construction-specific subcontract cycle; soft/hard commitment logic; forecasting framework with EVM; Birst analytics included.
**Limitations:** contingency informational only; no explicit transfer; crews unverified; complex parameter-driven configuration; better suited to large/EPC contractors [Claim, third-party].
**Pricing:** **Quote required.**

### 4.6 CMiC (CMiC Platform R12 / Nexus)

**Product.** CMiC Construction ERP ("CMiC Platform"), release line R12, current releases branded **Nexus** (Nexus 02, hotfixes Aug–Sep 2026). Multi-tenant SaaS hosted by CMiC on AWS per CMiC page; on-prem/private cloud only from third-party sources → Unverified. "Nova" not found. [Fact/Unverified] https://docs.cmicglobal.com/portal/Content/E_Reference_Material/New_Functionality/Patch_Releases/Patch_Releases.htm
Modules: Job Costing, Job Billing, Change Management, Subcontract Management, Contract Forecasting/GC Monitor, PO, Requisitions, Inventory, Equipment Costing, Payroll (US & Canada), Resource Planning, Workflow, BI — single database.

**Key findings**
- Hierarchy: job → phase/cost code → category; master job/sub-jobs via controlling job; up to 4 WBS codes per company. [Fact] https://docs.cmicglobal.com/portal/Content/E_Reference_Material/JC_-_Job_Costing/Reference/Using_WBS_Capabilities/Assigning_WBS_Codes.htm
- Budget/CO: PCI classes — **Original Budget, Internal, External (owner), Transfer** (can be forced to net zero). Original budget only changes via OBC PCIs. [Fact] https://docs.cmicglobal.com/portal/Content/E_Reference_Material/CM_-_Change_Management/Reference/Local_Tables/PCI_Types.htm
- Subcontracts: SOV, posting creates commitment, Requests for Payment → AP vouchers, retainage by line/vendor, compliance holds (insurance, lien waivers), pay-when-paid. [Fact] https://docs.cmicglobal.com/portal/Content/E_Reference_Material/SC_-_Subcontract_Management/Reference/Overview/SC_Lifecycle.htm
- Equipment: owned/leased/rented; automatic charge-out (daily/weekly/monthly/28-day, caps); utilisation report EM201; rental invoice matching. [Fact] https://docs.cmicglobal.com/portal/Content/E_Reference_Material/EM_-_Equipment_Costing/Reference/Transactions/Automatic_Charge_out.htm
- Cost control: GC Monitor — Actual, Open Commitment, Spent & Committed, Cost to Complete, Forecasted Cost, Cost Variance; projected-final override; freeze. [Fact] https://docs.cmicglobal.com/portal/Content/E_Reference_Material/GC_Monitor/Reference/GC_Monitor_Screen/Summary_Tab.htm
- Labour: union rates, prevailing wage, certified payroll, crew timesheets, burden to JC. [Fact]
- Billing: AIA G702/G703 (customer AIA licence needed), retainage, WIP (GAAP/IFRS), over/under billing. Milestone billing Unverified. [Fact]
- Multi-currency and multi-company native. [Fact]

**Strengths:** deepest documented construction cost model; full equipment costing; workflow with audit.
**Limitations:** steep learning curve; WBS limited to 4 codes; no contingency object, wastage, return-to-vendor documented; payroll US/Canada only.
**Pricing:** **Quote required.** https://cmicglobal.com/pricing

### 4.7 Acumatica Cloud ERP — Construction Edition

**Product.** Acumatica 2026 R2 (GA ~2026-10-01); Construction Edition = base platform + Construction feature (CS100000). Docs largely sourced from MYOB Acumatica mirror (same code base) because help.acumatica.com was not retrievable. Deployment options (SaaS/private/on-prem) not explicitly confirmed → Unverified. [Fact/Unverified]

**Key findings**
- Hierarchy: Project → Tasks × Cost Codes × Account Groups × Items; **parent-child projects new in 2026 R2**. [Fact] https://community.acumatica.com/acumatica-news-updates-2/introducing-acumatica-cloud-erp-2026r2-37191
- Budget/CO: original vs revised; change orders on cost, revenue and commitments; 2026 R1 owner-approval statuses; no transfer form (offsetting CO lines); no contingency object. [Fact] https://help.myob.com.au/advanced/Docs/Published/ReleaseNotes/RN_2026_R1_ProjectsConstr_Streamlined_Change_Order_Workflow.html
- Materials: project-specific inventory (3 tracking modes), requisitions, drop-ship to project, return leftovers to free stock. Wastage Unverified. [Fact]
- Subcontracts (CE): subcontracts as commitments, subcontract COs, AP/AR retainage, compliance docs, lien waivers, joint payments, vendor portal (2026 R2). [Fact] https://help.myob.com.au/advanced/Docs/Published/UserGuide/Construction__con_Overview_Construction_Processes.html
- Cost control: committed original/CO/revised/received/invoiced/open; Cost Projection by Date (qty × unit rate 2026 R1, S-curve 2026 R2). **No native GRNI accrual to project actuals.** [Fact] https://help.myob.com.au/advanced/Docs/Published/ReleaseNotes/RN_2026_R1_ProjectsConstr_Cost_Projection_Qty_Unit_Rate.html
- Billing: pro forma, AIA, retainage with cap, POC revenue recognition, WIP adjustment (2025 R2). [Fact] https://help.myob.com.au/advanced/Docs/Published/UserGuide/Construction_Reports_AIA.html
- Labour: time cards, weekly crew time entry, union/prevailing wage; payroll US/Canada; resource scheduling weak. [Fact/Claim]
- Equipment: equipment time cards to project; no utilisation analytics/rental fleet. [Fact]

**Strengths:** strong commitment model; AIA/retainage; frequent construction releases; unlimited-user pricing.
**Limitations:** immature parent-child hierarchy; no contingency/transfer object; weak resource planning; North-America-centric features.
**Pricing:** **Quote required** (resource-based, unlimited users). https://www.acumatica.com/pricing/

### 4.8 IFS Cloud

**Product.** IFS Cloud, single codebase, twice-yearly releases (docs 24R1–25R2, some 26R1). Deployment: IFS-managed cloud or Remote (customer-hosted); same functionality. [Fact] https://docs.ifs.com/techdocs/25r2/010_overview/020_high_level_architecture
Components: Project Planning & Execution, Budgeting & Forecasting, EAC Workbench, Performance Analysis, Project Accounting, Sales Contract Management + Application for Payment, Sub Contract Management + Valuation, Contract Change Management, Schedule of Work, Procurement, Project Inventory, Rental Management, Maintenance, Time Reporting, Resource Planning, Analysis Models/Lobbies.

**Key findings**
- Hierarchy: project → nestable sub projects → activities → tasks; CBS; Primavera/MS Project integration. [Fact] https://docs.ifs.com/ifsclouddocs/24r2/ProjectManagement/AboutProjectManagement.htm
- Budget: multiple forecast versions, one active budget; Preliminary → Reviewed → Approval in Progress → Approved with routing; **logged budget transfers**; baselines; CCO feeds ETC. Contingency object Unverified. [Fact] https://docs.ifs.com/ifsclouddocs/25r2/ProjectManagement/AboutProjBnF.htm
- Dimensions: code-string part as base for project cost/revenue elements; posting controls. [Fact] https://docs.ifs.com/ifsclouddocs/25r2/ProjectManagement/AboutProjectCostElements.htm
- Materials: Schedule of Work (BoQ-like) → contracts/subcontracts; project inventory with transfers; requisitions committed at reservation, used at issue. [Fact] https://docs.ifs.com/ifsclouddocs/24r1/MaintainInventory/ProcessTransferProjectInventory.htm
- Subcontracts: call-offs (planned committed → committed), cumulative valuations application → certification, retention with caps, B2B portal. [Fact] https://docs.ifs.com/ifsclouddocs/25r2/SubConValuation/AboutSubContractValuation.htm
- Equipment: Rental Management incl. "Manage Rent to Project". [Fact] https://docs.ifs.com/ifsclouddocs/24r1/ProcessModels/Process_Model/managerenttoproject.htm
- Cost control: value stages **estimated, planned, baseline, planned committed, committed, used, actual**; EAC Workbench snapshot; earned value with CPI/SPI. [Fact] https://docs.ifs.com/ifsclouddocs/24r2/ProjectManagement/AboutEACWorkbench.htm
- Billing: sales contracts with retention, cumulative Applications for Payment, POC / event-based revenue recognition, WIP. [Fact] https://docs.ifs.com/ifsclouddocs/25r2/ApplicationForPayment/AboutApplicationForPayment.htm
- Payroll: integration strategy (e.g., ADP); limited native country payroll. [Claim, community]

**Strengths:** most complete documented value-stage model; full subcontract & client valuation cycles; budget approvals and transfers; global deployment.
**Limitations:** cost elements are company-wide and must be designed up front; payroll via integration; crews/wastage/contingency not explicit.
**Pricing:** **Quote required.**
**Doc conflict:** one IFS worked example keeps committed unchanged after used increases — validate in sandbox.

---

## 5. Capability matrix

Product evaluated per column: **Odoo** 19 Enterprise · **Zoho** (Projects + Books + Analytics) · **SAP** S/4HANA PS (Private/on-prem) · **BC** Business Central v29 · **Infor** CSEC/LN · **CMiC** R12/Nexus · **Acumatica** Construction Edition 2026 R2 · **IFS** Cloud 25R2.

| Capability | Odoo | Zoho | SAP S/4 PS | Business Central | Infor CSEC (LN) | CMiC | Acumatica CE | IFS Cloud |
|---|---|---|---|---|---|---|---|---|
| **Project hierarchy / WBS** | Native (partial) — tasks/sub-tasks; no subproject/WBS codes | Native (partial) — Projects app only; no subprojects | Native — WBS + networks | Native (partial) — tasks + planning lines; no subproject | Native — main/sub projects, element & activity structures | Native — job/sub-job, cost code, 4 WBS codes | Native — parent-child new in 2026 R2 | Native — nestable sub projects, activities |
| **Original budget & revisions** | Native — revisions | Native (basic) — no versioning | Native | Native (partial) — archive versions | Native — frozen + adjustments, BCA snapshots | Native — OBC PCIs | Native — original vs revised | Native — versions, baselines |
| **Budget approvals** | Customization required | Customization required | Native — status/release control | Customization required (Power Automate/AL) | Native — e-signature on adjustments, ION | Native — workflow | Unverified for original budget; Native for COs | Native — approval routing |
| **Budget transfers** | Customization required | Customization required | Native | Customization required | Unverified | Native — Transfer PCI (net zero) | Customization required — offsetting CO lines | Native — logged transfers |
| **Contingency** | Customization required | Customization required | Customization required | Customization required | Native (informational only) | Unverified — model as cost code | Customization required | Unverified |
| **Change orders** | Third-party extension | Customization required | First-party add-on (CPM) | Third-party extension | Native — Extensions | Native — 4 PCI classes | Native | Native — Contract Change Orders |
| **Financial dimensions** | Native — analytic plans | Native — reporting tags/locations | Native — WBS/CC/PC/plant | Native — dimensions | Native — dimension types | Native — dept/WBS/cost code | Native — subaccounts/cost codes | Native — code string parts |
| **Materials (PO → receipt → stock → site → consumption)** | Native (partial) — analytic costs on moves | Native (procurement/stock); Customization for consumption | Native — project stock | Native — project POs/usage link | Native — PRP, project warehouse | Native — PO/receipt/issue/transfer | Native — project-specific inventory | Native — project inventory |
| **Material estimates / BoQ** | Third-party extension | Customization required | Native (cost planning) | Native (planning lines) | Native (budget lines/estimate) | Unverified — partners | Native (budget lines) | Native — Schedule of Work |
| **Returns & wastage** | Native moves; project posting Unverified | Customization required | Native (movements); wastage no dedicated function | Native returns (limits); wastage Customization | Unverified | Returns native; wastage Unverified | Returns native; wastage Unverified | Unverified |
| **Timesheets & labour rates** | Native | Native (Projects/People) | Native (CATS + activity types) | Native | Native (People) | Native — union/prevailing wage | Native — crew time entry | Native |
| **Crews** | Customization required | Customization required | Unverified | Native (resource groups, partial) | Unverified | Native — crew timesheets | Native — weekly crew time | Unverified |
| **Overtime costing** | Unverified | Customization required | First-party add-on (HCM) | Native — alternate cost/work types | Native — labour types | Native | Native | Unverified (daily only per community) |
| **Payroll integration** | Native app (country-dependent; Unverified) | Native — Payroll → Books journals | First-party add-on (HCM/SuccessFactors) | Third-party extension | First-party add-on/Unverified (export) | Native — US/Canada only | First-party add-on — US/Canada | Third-party (integration strategy) |
| **Resource planning** | Native (Planning) | Native (workload) | Native | Native (capacity) | Native | Native | Third-party (weak natively) | Native |
| **Subcontract commitments** | Customization required (service POs) | Customization required | Native (service POs) | Customization required (POs) | Native | Native | Native | Native |
| **Progress claims / valuations** | Third-party extension | Third-party extension (Creator) | Native (SES, basic); Third-party for running accounts | Third-party extension | Native — Application for Payment | Native — Requests for Payment | Native — bills vs subcontract lines | Native — valuations with certification |
| **Subcontract retention** | Third-party extension | Native (payment retention, basic) | Native (down-payment chains, on-prem) | Third-party extension | Native — cap + staged release | Native | Native | Native — caps |
| **Subcontract variations** | Third-party extension | Customization required | Native (PO change) / Third-party | Third-party extension | Native | Native | Native | Native |
| **Compliance (insurance, lien waivers)** | Third-party extension | Customization required | Customization required | Third-party extension | Unverified | Native — compliance hold | Native | Unverified |
| **Equipment allocation & rental charges** | Customization required | Customization required | Customization required | Native (resources) / Customization for FA | Native — equipment cost objects, hire POs | Native — auto charge-out | Native (time cards); rental fleet Third-party | Native — Rental Management |
| **Equipment utilisation** | Customization required | Customization required | Customization required | Native — utilization report (resources) | Unverified | Native — EM201 | Not found in public docs (native); third-party options exist | Unverified |
| **Committed cost visibility** | Native — Committed in budget report (POs only) | Not supported in project cost reports — Zoho docs list cost sources without POs | Native | Native (partial) — outstanding orders | Native — soft/hard | Native | Native | Native — planned committed/committed |
| **Accruals (received not invoiced)** | Not found in public docs (project level) | Not found in public docs | Native (valuated GR / GR-IR) | Native (partial) — Amt. Rcd. Not Invoiced | Native (via receipts) | Unverified (WIP accrual only) | Not found in public docs (GRNI to project actuals; VD-04) | Native — "used" stage |
| **ETC / EAC forecast** | Customization required | Native (labour-only) | Native (PS); PCE Unverified | Customization required (Power BI) | Native — EAC/ETC/EEC + EVM | Native — GC Monitor | Native — cost projections | Native — EAC Workbench + EVM |
| **Budget vs actual reporting** | Native | Native (GL-level) / Analytics | Native | Native | Native (Birst) | Native | Native | Native |
| **Milestone billing** | Native | Native (Books) | Native | Native (billable lines) | Native (installments) | Unverified | Native (via tasks) | Unverified |
| **Progress billing** | Third-party extension | Native — progress invoices | Native (PS) / Third-party (PCE) | Third-party extension | Native | Native — AIA | Native — AIA | Native — Application for Payment |
| **Client retention** | Third-party extension (first-party claim Unverified) | Native (early access conflict) | Native (down-payment chains) | Third-party extension | Native — holdback | Native | Native — retainage cap | Native |
| **Revenue recognition / WIP** | Customization required | Not found in public docs | Native — EBRR/RA | Native — 5 WIP methods | Native — POC | Native — GAAP/IFRS WIP | Native — WIP adjustment | Native — POC/event |
| **Roles & permissions** | Native | Native | Native | Native | Native | Native | Native | Native |
| **Approval workflows** | Native (Approvals/PO double validation); multi-level Third-party | Native — up to 10 levels | Native | Native (templates) | Native (ION) | Native | Native (approval maps) | Native |
| **Audit trail** | Native (chatter); full audit Unverified | Native (approval history); system audit Unverified | Native — change documents | Native — Change Log | Native (audit fields); field-level Unverified | Native — User Audit/history | Native — field audit | Native — history logging |

---

## 6. Budget-versus-actual workflow comparison

### 6.1 Reference flow
**Budget → Commitment (PO / subcontract) → Consumption (material issue, timesheet, equipment, subcontract certification) → Posted actual / accrual → Forecast (ETC/EAC) → Variance.**

### 6.2 Per ERP

| Stage | Odoo | Zoho | SAP S/4 PS | Business Central |
|---|---|---|---|---|
| Budget | Analytic budget lines per plan; revision creates "Rev" budget | Projects budget *or* Books project/GL budget (not synced) | WBS cost plan → original budget → supplements/returns/transfers; AVC active | Budget planning lines on posting tasks; archive versions |
| Commitment | Confirmed unbilled PO lines with analytic distribution | Books PO tagged to project — **not in cost reports** | PR/PO on WBS writes commitment; AVC checks actual + commitment | PO lines with project/task → Outstanding Orders |
| Consumption | Stock moves (if Analytic Costs on), timesheets at HR cost | Timesheet "staff cost" memo | GI to WBS, CATS activity allocation, SES | Receipt immediately consumed; time sheets → project journal |
| Actual / accrual | Posted bills, timesheets, expenses; **no project accrual** | Bills, expenses, journals; no accrual | Valuated GR posts actual; IR posts actual for non-valuated; GR/IR accrual | Project ledger only on invoice posting; Amt. Rcd. Not Invoiced shown separately |
| Forecast | None native | Labour % complete formula | Actual + open commitment + remaining plan (PS cost forecast) | None native (build Actual + Remaining or + Outstanding + RNI) |
| Variance | Budgeted − Achieved / Committed | Planned − actual (labour); GL BvA | AVC/cost reports, SAC | Project Actual to Budget, Power BI |
| **Double-counting control** | PO stays Committed until billed, then moves to Achieved. Risk: material counted at bill *and* at consumption; payroll + timesheet labour | POs never in cost → no double count, but commitments invisible. Risk: labour via timesheets and tagged payroll | One open-commitment balance per PO line consumed by GR/SES/IR. Risk: non-valuated GR + GR/IR accrual | Mutually exclusive buckets: Outstanding → Rcd. Not Invoiced → Ledger. Risk: summing Usage-Link remaining *and* open PO for the same line |

| Stage | Infor CSEC (LN) | CMiC | Acumatica CE | IFS Cloud |
|---|---|---|---|---|
| Budget | Estimate → frozen element/activity budget; Budget Adjustments + Extensions; BCA versions | JC budget lines per cost code/category; OBC/Internal/External/Transfer PCIs | Original vs revised budget per task × cost code × account group; change orders | Budget version → Approved → Active Budget; transfers; baselines |
| Commitment | PRP/PO/subcontract → soft commitment; on delivery → hard commitment (labour excluded) | Posted PO/subcontract creates commitment ('O') transactions, no GL entry | PO/subcontract → Original Committed; CO → Committed CO / Revised Committed | PR/planned PO = planned committed; released PO/call-off/reservation = committed |
| Consumption | Issue/receipt to project; People hours; equipment cost entries | Receipts, payroll, equipment charge-out, inventory issues | Receipts raise Committed Received; time & equipment cards post usage | Receipts, issues, confirmed time, rentals, certified valuations = used |
| Actual / accrual | Commitment offset by same amount as cost; costs approved then to Financials | AP vouchers linked to SC/PO lines; WIP accrual | AP/subcontract bill → Committed Invoiced + Actual; **no GRNI to actuals** | Voucher row replaces used value; actual = GL-posted subset |
| Forecast | EAC/ETC/EEC; ETC = EAC − actuals; EVM | Forecasted Cost = Actual + Open Commitments + In/Out-of-scope changes + ETC | Projected Cost at Completion = ETC + Actual + Revised Committed − Committed Invoiced | EAC = Planned + Estimated Future Commitment + unplanned CCO; ETC = EAC − (Committed + Used) |
| Variance | VAC = BAC − EAC | Cost Variance = Projected Cost Budget − Forecasted Cost | Revised Budget − Projected Cost at Completion | Monitoring/Performance Analysis; CPI/SPI |
| **Double-counting control** | Commitment relieved by exactly the cost amount; soft converts to hard (not added). Risk: forecast lines not cleared for committed scope | Open Commitment = commitments − cost posted against them; requires vouchers linked to SC/PO lines (utility for unlinked) | Invoiced commitment subtracted before adding actual. Risk: "Actual + Open" understates GRNI — use Revised − Invoiced | One holder per stage; committed is remaining quantity only; used replaced by voucher. Rule: exposure = committed + used, never used + actual |

### 6.3 Cross-ERP conclusions [Analysis]
1. **Relief principle:** every robust design reduces the commitment by the exact amount that becomes actual (or "used"), at the line level (PO line / subcontract SOV line).
2. **Where relief happens matters:** on receipt (SAP valuated GR, IFS used, CMiC PO receipt) or on invoice (Odoo, BC, Acumatica). If relief is on invoice, goods received not invoiced must be shown as a separate accrual bucket or the forecast understates exposure.
3. **Labour has no commitment** in most systems (explicit in Infor); labour exposure comes from ETC, not commitments.
4. **Single labour-cost source:** either timesheet × rate (management cost) or payroll allocation (GL cost) — never both in the same report without reconciliation (Odoo/Zoho risk).
5. **Forecast must exclude scope already committed** (Infor warning) — ETC applies only to uncommitted remaining scope.

---

## 7. Conceptual requirements blueprint for our construction PMS

> Requirements documentation only — no implementation. Where relevant, mapped to concepts already present in our current PMS (Project → Milestone → Task → Subtask, statuses, dependencies, QC workflow, critical labels, timers, weightage roll-up).

### 7.1 Project hierarchy
- R-H1 Company → Project → Subproject (unlimited nesting, at least 3 levels) → Phase / Work package → Activity/Task → Subtask. Current Milestone/Task/Subtask levels map to Phase / Activity / Subtask.
- R-H2 Each WBS node has: code, name, owner, dates, status, weightage, budget-control flag, and the "cost-collecting" flag (only leaf or flagged nodes accept cost).
- R-H3 Roll-up of budget, commitment, actual, forecast and progress at every level (existing weightage roll-up extended to cost).
- R-H4 Project templates and copy-project including WBS and budget skeleton.
- R-H5 Link to schedule (Gantt, dependencies) and to BoQ items.

### 7.2 Dimensions and cost centres
- R-D1 Mandatory dimensions on every cost transaction: Project, WBS node, Cost code, Cost type.
- R-D2 Optional/organisational dimensions: Site/location, Department, Cost centre, Profit centre, Branch/company, Equipment, Employee/crew, Vendor/subcontract.
- R-D3 Cost code library (hierarchical, company-wide, with project-level enable/disable) separate from WBS.
- R-D4 Validation rules (mandatory/blocked combinations) as in Odoo/BC dimension rules.
- R-D5 Dimension values must be carried consistently to the ERP/GL integration.

### 7.3 Budgeting and change control
- R-B1 Budget line key = WBS × cost code × cost type (optionally × resource/item), with quantity, unit rate and amount.
- R-B2 Budget versions: Estimate → Original (locked at approval) → Revised (current) → Forecast versions; baselines retained.
- R-B3 Budget states with approval routing (Draft → Submitted → Approved → Active), based on amount thresholds and roles.
- R-B4 Budget transfers between lines with zero-net validation, reason code, approval and log (CMiC Transfer PCI / IFS logged transfers).
- R-B5 Contingency as an explicit reserve line per project/subproject with drawdown only via approved transfers, and a contingency-remaining report (no such object found in the public documentation reviewed; see §1 finding 5).
- R-B6 Change management: Potential CO → Change Request → Approved CO with classes: Original budget, Internal, External (client), Transfer; each CO may change cost budget, revenue budget and commitments.
- R-B7 Budget control: warning / hard stop when commitments + actuals exceed available budget at a configurable level (SAP availability control model).

### 7.4 Materials
- R-M1 Material estimate (BoQ quantities × rates) per WBS/cost code.
- R-M2 Material requisition from site → approval → PO (budget check) → receipt at site/central store.
- R-M3 Project/site stores with transfers (store → site, site → site, site → store).
- R-M4 Issue/consumption to WBS/cost code; returns to store; wastage/scrap with reason codes and wastage % vs norm.
- R-M5 Reconciliation: estimated vs requisitioned vs received vs consumed vs balance at site.

### 7.5 Manpower
- R-L1 Resource and crew definitions (crew = set of trades/employees, crew rates).
- R-L2 Resource planning by WBS (planned hours per trade/period), capacity view.
- R-L3 Daily timesheets / crew time entry per WBS × cost code, with approval (current task timers can feed this).
- R-L4 Labour rate tables by trade/employee/project with effective dates; overtime and holiday multipliers.
- R-L5 Payroll integration (export approved hours; import actual payroll cost for true-up) — choose one cost source for reporting.
- R-L6 Labour productivity: planned vs actual hours per quantity.

### 7.6 Subcontractors
- R-S1 Subcontract with schedule of values / BoQ lines linked to WBS × cost code; approval; becomes commitment on approval.
- R-S2 Subcontract variations (linked to project COs where applicable).
- R-S3 Periodic progress claims (cumulative), site measurement, assessment/certification, retention %, retention cap, advance recovery, deductions/back-charges.
- R-S4 Retention release (practical completion / defects liability).
- R-S5 Compliance documents (insurance, licences, bonds) with expiry and payment hold.
- R-S6 Payment certificate → AP invoice in ERP; relieve commitment by certified amount.

### 7.7 Equipment
- R-E1 Equipment register (owned / rented), rate cards (hourly/daily/monthly), operator link.
- R-E2 Allocation to projects/sites with dates; equipment log (hours, idle, breakdown).
- R-E3 Internal charge-out to WBS/cost code; external rental as commitment (rental agreement/PO).
- R-E4 Utilisation report (available vs used vs idle).

### 7.8 Cost control and forecasting
- R-C1 Cost buckets per budget line: Original budget, Approved changes, Revised budget, Committed (total), Open commitment, Received/certified not invoiced (accrual), Actual, ETC, EAC, Variance.
- R-C2 Relief rule: each commitment line is reduced by exactly the amount converted to actual/accrual — no separate re-entry.
- R-C3 EAC = Actual + Accrual + Open commitment + ETC(uncommitted scope); ETC entered per line or calculated (budget remaining, unit-rate, % complete), with manual override and freeze.
- R-C4 Monthly cost-report snapshot/cut-off with period comparison.
- R-C5 Earned value (planned value, earned value, actual cost, CPI, SPI) using progress from the schedule.
- R-C6 Critical labels / alerts (existing critical-label engine) driven by variance thresholds.

### 7.9 Billing (client side)
- R-R1 Client contract with billing items (BoQ / schedule of values / milestones).
- R-R2 Milestone billing and progress/valuation billing (cumulative), certification by client.
- R-R3 Client retention and advance recovery.
- R-R4 Revenue recognition (POC by cost or by physical progress) and over/under billing report.

### 7.10 Approvals and governance
- R-G1 Role-based access per company/project (PM, site engineer, QS, procurement, accounts, management).
- R-G2 Configurable approval workflows (existing QC workflow engine extended) for budgets, transfers, COs, requisitions, POs, subcontracts, claims, timesheets.
- R-G3 Immutable audit trail on budget, commitment and cost changes (who, when, old/new value, reason).
- R-G4 Server-side permission enforcement on every data operation (see current-system concerns in memory notes).

### 7.11 Reporting
- R-P1 Project cost report (budget → committed → actual → forecast → variance) by WBS and by cost code, drillable to documents.
- R-P2 Portfolio dashboard across projects/subprojects.
- R-P3 Change order log, contingency log, subcontract ledger, retention ledger, material reconciliation, labour productivity, equipment utilisation, cash-flow forecast.
- R-P4 Export to Excel/PDF; scheduled monthly reports.

---

## 8. Prioritised feature list

### Must-have (MVP for reliable budget vs actual)
1. Project → subproject → WBS hierarchy with roll-up (R-H1–H3).
2. Cost code library and mandatory dimensions (R-D1–D4).
3. Budget lines (WBS × cost code × cost type) with original/revised and approval (R-B1–B3).
4. Change orders (internal/external/transfer) and budget transfers (R-B4, R-B6).
5. Commitments from POs and subcontracts with line-level relief (R-C1–C2).
6. Actuals from material issues, timesheets, certified subcontract claims and invoices (R-M4, R-L3, R-S3).
7. Forecast (ETC/EAC) with override and monthly snapshot (R-C3–C4).
8. Project cost report and portfolio dashboard (R-P1–P2).
9. Role-based access, approval workflows, audit trail, server-side authorisation (R-G1–G4).

### Should-have
1. Contingency reserve and drawdown log (R-B5).
2. Budget control warnings/hard stops (R-B7).
3. Material requisition → PO → site store → transfers → returns/wastage reconciliation (R-M2–M5).
4. Subcontract retention, variations, compliance holds (R-S2–S5).
5. Labour rates, overtime multipliers, crew entry, payroll export (R-L1, R-L4–L5).
6. Equipment register, allocation, charge-out (R-E1–E3).
7. Client milestone and progress billing with retention (R-R1–R3).
8. Accrual bucket for received/certified-not-invoiced (R-C1).

### Future
1. Earned value (CPI/SPI) and S-curves (R-C5).
2. Revenue recognition / over-under billing (R-R4).
3. Equipment utilisation analytics (R-E4).
4. Labour productivity analytics (R-L6).
5. Subcontractor / client portals for claims and approvals.
6. BoQ/estimating import, BIM/schedule integration (Primavera/MS Project).
7. AI-assisted forecasting and anomaly detection (as marketed by CMiC Nexus / Acumatica 2026 R1 — Claims).

---

## 9. Shortlist — best three ERP fits

### 9.1 Evaluation criteria (weights)
Weights favour construction suitability and cost-control depth, not popularity. Scores 0–5 are lead-agent judgement based on the documented labels in §5 (5 = native and documented in depth; 3 = native but partial; 1 = third-party/customization; 0 = not supported).

| Criterion | Weight |
|---|---|
| Cost control (commitments, relief, accruals, EAC) | 20 |
| Budgeting & change orders (versions, approvals, transfers, COs) | 15 |
| Subcontract management (claims, retention, variations) | 15 |
| Project hierarchy / WBS | 10 |
| Materials | 10 |
| Manpower | 10 |
| Financial dimensions | 5 |
| Equipment | 5 |
| Project billing | 5 |
| Governance | 5 |

### 9.2 Scores [Analysis]

| Criterion (weight) | Odoo | Zoho | SAP PS | BC | Infor | CMiC | Acumatica | IFS |
|---|---|---|---|---|---|---|---|---|
| Cost control (20) | 2 | 1 | 5 | 3 | 5 | 5 | 4 | 5 |
| Budget & CO (15) | 2 | 1 | 4 | 2 | 4 | 5 | 4 | 5 |
| Subcontracts (15) | 1 | 1 | 3 | 1 | 5 | 5 | 5 | 5 |
| Hierarchy (10) | 3 | 3 | 5 | 3 | 5 | 4 | 3 | 5 |
| Materials (10) | 3 | 2 | 5 | 4 | 4 | 4 | 4 | 4 |
| Manpower (10) | 3 | 2 | 4 | 3 | 4 | 5 | 4 | 3 |
| Dimensions (5) | 5 | 4 | 5 | 5 | 4 | 4 | 4 | 4 |
| Equipment (5) | 1 | 1 | 2 | 3 | 4 | 5 | 3 | 4 |
| Billing (5) | 2 | 3 | 4 | 3 | 5 | 5 | 5 | 5 |
| Governance (5) | 3 | 3 | 5 | 4 | 4 | 5 | 4 | 4 |
| **Weighted score (/100)** | **46** | **35** | **85** | **56** | **90** | **95** | **81** | **91** |

### 9.3 Shortlist [Recommendation]

**1. CMiC (95)**
- Evidence: four-class PCI change model incl. net-zero transfers; documented Open Commitment and Forecasted Cost formulas; subcontract SOV/retainage/compliance holds; equipment automatic charge-out and utilisation; union/certified payroll; single database (§4.6).
- Trade-offs: payroll US/Canada only; WBS limited to 4 codes per company; SaaS-only per vendor page; steep learning curve; Quote required.
- Best as: the reference model for construction cost-control workflows.

**2. IFS Cloud (91)**
- Evidence: value-stage model (planned committed → committed → used → actual) with documented relief; EAC Workbench with snapshots and EVM; budget approval routing and logged transfers; sub-contract valuations and client Applications for Payment with retention; project inventory; rental to project; global deployment (cloud or remote) (§4.8).
- Trade-offs: payroll via integration; cost elements must be designed company-wide up front; crews/contingency not explicit; Quote required.
- Best as: international/EPC contractors needing one global project-based ERP.

**3. Infor CloudSuite Engineering & Construction / LN (90)**
- Evidence: main/sub projects with element and activity structures; soft/hard commitments offset by cost; EAC/ETC/EEC and EVM; Services Procurement with Application for Payment, retention cap, variations; holdback and POC billing (§4.5).
- Trade-offs: contingency informational only; budget transfer not found; no native payroll; EAM now Hexagon (third-party); complex configuration; Quote required.
- Best as: large contractors already on or open to the Infor stack.

**Runners-up:** SAP S/4HANA PS (85) — strongest budget enforcement and WIP, but subcontract claims and plant hire depend on partners and implementation is heavy. Acumatica Construction Edition (81) — strong commitments/AIA/retainage with unlimited users, but immature parent-child hierarchy and North-America-centric features.

**Not recommended for construction cost control without significant extension:** Business Central (56), Odoo (46), Zoho (35) — all lack native subcontract claims/retention and EAC; Zoho excludes POs from project cost entirely.

### 9.4 Assumptions
- Mid-to-large contractor, multiple concurrent projects, mix of own labour and subcontractors.
- Region not confirmed — scores do not penalise US/Canada payroll limits; if operating in India/GCC, CMiC and Acumatica payroll/billing fit drops and IFS/Infor/SAP rise relatively.
- Scores reflect public documentation as of 2026-11-03, not hands-on testing. Gated docs (Infor, SAP community, Acumatica help) were partly read via mirrors/summaries.
- Pricing excluded from scores (mostly Quote required).

---

## 10. Open questions and sources

### 10.1 Open questions requiring business input
1. **Countries of operation** and statutory requirements (GST/VAT, e-invoicing, payroll, labour law) — affects payroll and billing fit.
2. **Company size:** number of concurrent projects, subprojects per project, users (office vs site), transaction volumes.
3. **Build vs buy:** is the goal to extend our existing PMS (and integrate with an ERP for accounting), or to select one of these ERPs? Which ERP/accounting system is the financial system of record?
4. **Hierarchy depth** needed (project → subproject → phase → package → activity → task?) and whether subprojects can belong to different companies/sites.
5. **Cost code standard** (in-house, CSI MasterFormat, client-specific BoQ structure)?
6. **When cost becomes actual:** on goods receipt / certification, or on vendor invoice? (Drives accrual design.)
7. **Labour cost source:** timesheet × standard rate, actual payroll cost, or both with true-up?
8. **Contingency policy:** who owns it, approval thresholds, reporting to client?
9. **Change order process:** classes, approval levels, link to client variations and subcontract variations.
10. **Subcontract practice:** measurement method, retention %, caps, release triggers, advance payments, back-charges, compliance documents required.
11. **Equipment:** owned vs rented share, charging basis (hour/day/month), operator costing.
12. **Material practice:** central store vs site stores, wastage norms, return handling.
13. **Client billing:** milestone, BoQ/valuation-based, cost-plus? Retention terms? Revenue recognition standard (Ind AS 115 / IFRS 15 / ASC 606)?
14. **Approval matrix** by document type and amount.
15. **Reporting cadence** (monthly cost report cut-off, forecast review cycle) and required report formats.
16. **Deployment and data-residency** constraints (cloud vs on-prem).
17. **Budget** for licences and implementation; tolerance for partner-led implementation.

### 10.2 Items to validate in vendor demos / sandboxes
- IFS: committed-relief worked-example conflict; contingency handling.
- Acumatica: Revised Committed definition conflict (pending vs released COs); GRNI handling.
- Odoo: material double count (bill + consumption analytic); first-party retention claim.
- Zoho: retention availability by region; timesheet sync scope conflict.
- SAP: Public Cloud milestone-billing roadmap (KBA vs community).
- CMiC: deployment options; milestone billing; accruals.
- Infor: budget transfers; crews; payroll route.
- BC: third-party app features and prices.

### 10.3 Consolidated source list (accessed 2026-11-03)

**Preliminary selection**
- https://erpresearch.com/en-us/top-10-construction-erp-software-report (third-party)
- https://www.netatwork.com/resource/construction-erp-comparison-acumatica-trimble-viewpoint (partner)
- https://blog.ifs.com/ifs-cloud-the-ideal-erp-solution-for-construction-and-engineering-companies/ (vendor marketing)

**Odoo**
- https://www.odoo.com/documentation/19.0/applications/finance/accounting/reporting/budget.html
- https://www.odoo.com/documentation/20.0/applications/finance/accounting/reporting/budget.html
- https://www.odoo.com/documentation/19.0/applications/finance/accounting/reporting/analytic_accounting.html
- https://www.odoo.com/documentation/19.0/applications/services/project/project_management/project_profitability.html
- https://www.odoo.com/documentation/19.0/applications/services/project/project_management/project_milestones.html
- https://www.odoo.com/documentation/19.0/applications/services/timesheets.html
- https://www.odoo.com/pricing
- https://apps.odoo.com/apps/modules/saas-19.1/construction
- https://apps.odoo.com/apps/modules/19.0/sdlc_construction_management (third-party)
- https://apps.odoo.com/apps/modules/19.0/retention_construction_contracting_projects (third-party)
- https://apps.odoo.com/apps/modules/17.0/grev_od_construction_retention (third-party)
- https://apps.odoo.com/apps/modules/19.0/purchase_analytic_budget_monitor (third-party)
- https://apps.odoo.com/apps/modules/19.0/grev_od_purchase_budget_control (third-party)
- https://apps.odoo.com/apps/modules/19.0/us_job_cost_timesheet_payroll_allocation (third-party)
- https://apps.odoo.com/apps/modules/19.0/mst_purchase_multi_level_approval (third-party)

**Zoho**
- https://help.zoho.com/portal/kb/articles/project-budget
- https://help.zoho.com/portal/en/kb/projects/zoho-projects-overview/articles/work-breakdown-structure-in-zoho-projects
- https://help.zoho.com/portal/en/kb/projects/integration/zoho-apps/articles/zoho-finance-integration
- https://www.zoho.com/en-fr/books/help/integrations/projects-integration.html
- https://www.zoho.com/en-fr/books/help/projects/functions.html
- https://www.zoho.com/books/help/accountant/budgets.html
- https://www.zoho.com/books/help/settings/reporting-tag.html
- https://www.zoho.com/books/kb/locations
- https://www.zoho.com/inventory/help/warehouses/transfer-orders.html
- https://www.zoho.com/books/help/payment-retention/
- https://www.zoho.com/bh/books/help/quote/progress-invoice.html
- https://www.zoho.com/books/help/accountant/fixed-assets.html
- https://www.zoho.com/books/help/transaction-approval-new-flow/transaction-approval-workflow.html
- https://help.zoho.com/portal/en/kb/projects/settings-in-zoho-projects/user-management/articles/resources
- https://www.zoho.com/ae/payroll/kb/employer/zoho-books-integration/reporting-tag.html
- https://www.zoho.com/books/industry/construction-accounting-software/
- https://www.zoho.com/en-in/erp/
- https://www.zoho.com/us/books/pricing/
- https://www.zoho.com/one/pricing.html
- https://www.zoho.com/projects/pricing.html
- https://ecosire.com/apps/zoho-one/zoho-construction-project-management-app (third-party)

**SAP**
- https://userapps.support.sap.com/sap/support/knowledge/en/3547457
- https://userapps.support.sap.com/sap/support/knowledge/en/3725325
- https://userapps.support.sap.com/sap/support/knowledge/en/3731931
- https://userapps.support.sap.com/sap/support/knowledge/en/3606983
- https://userapps.support.sap.com/sap/support/knowledge/en/3458767
- https://userapps.support.sap.com/sap/support/knowledge/en/2916914
- https://userapps.support.sap.com/sap/support/knowledge/en/3710740
- https://userapps.support.sap.com/sap/support/knowledge/en/3709113
- https://userapps.support.sap.com/sap/support/knowledge/en/3511965
- https://userapps.support.sap.com/sap/support/knowledge/en/3688854
- https://userapps.support.sap.com/sap/support/knowledge/en/3770277
- https://learning.sap.com/courses/project-financials-control-in-sap-s-4hana/executing-a-project
- https://learning.sap.com/courses/project-financials-control-in-sap-s-4hana/performing-internal-activities
- https://learning.sap.com/courses/implementing-sap-s-4hana-cloud-public-edition-sourcing-and-procurement/creating-purchase-orders-with-retention_d1eb3aa4-60a7-441c-bf64-2e4703a52956
- https://help.sap.com/saphelp_46C/helpdata/EN/89/a3a9375d79fb7de10000009b38f8cf/content.htm (legacy)
- https://help.sap.com/saphelp_globext607_10/helpdata/en/d9/b6033ce78e1205e10000000a114084/content.htm (legacy)
- https://blogs.sap.com/2021/03/06/sap-s-4hana-project-issue-and-change-management-in-project-management/
- https://blogs.sap.com/2022/10/05/event-based-revenue-recognition-for-universal-parallel-accounting-in-sap-s-4hana-2022/
- https://blog.sap-press.com/billing-with-sap-project-system-in-sap-s4hana
- https://www.sap.com/products/financial-management/partners/syntax-systems-usa-lp-progress-billing.html (third-party)
- https://www.sap.com/products/erp/partners/syntax-systems-usa-lp-project-change-order-management.html (third-party)
- https://www.sap.com/products/financial-management/partners/motiveminds-consulting-private-limited-myra.html (third-party)
- https://www.sap.com/finland/products/erp/partners/ltimindtree-limited-project-paychain.html (third-party)

**Microsoft Dynamics 365 Business Central**
- https://learn.microsoft.com/en-us/dynamics365/business-central/projects-how-create-jobs
- https://learn.microsoft.com/en-us/dynamics365/business-central/projects-how-manage-project-supplies
- https://learn.microsoft.com/en-us/dynamics365/business-central/projects-understanding-wip
- https://learn.microsoft.com/en-us/dynamics365/business-central/projects-how-invoice-jobs
- https://learn.microsoft.com/en-us/dynamics365/business-central/projects-how-setup-resources
- https://learn.microsoft.com/en-us/dynamics365/business-central/projects-how-setup-time-sheets
- https://learn.microsoft.com/en-us/dynamics365/business-central/finance-dimensions
- https://learn.microsoft.com/en-us/dynamics365/business-central/across-workflow
- https://learn.microsoft.com/en-us/dynamics365/business-central/projects-analytics-overview
- https://learn.microsoft.com/en-us/dynamics365/business-central/project-reports
- https://learn.microsoft.com/en-us/dynamics365/release-plan/2024wave1/smb/dynamics365-business-central/archive-projects
- https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/whatsnew/whatsnew-update-29-0
- https://www.microsoft.com/en-us/dynamics-365/products/business-central/pricing
- https://appsource.microsoft.com/en-us/product/web-apps/rsmproductsalesllc1604685958273.constructioncentric (third-party)
- https://marketplace.microsoft.com/en-us/product/dynamics-365-business-central/pubid.netsmartz%7Caid.projectproconstruction%7Cpappid.a3c03ddf-3f59-46a5-9644-a1f4a6b1d289?tab=overview (third-party)

**Infor**
- https://www.infor.com/industries/construction
- https://docs.infor.com/csec/2026.x/en-us/cseclib/csecpov/oak1623337857554.html
- https://docs.infor.com/ln/latest/en-us/lnolh/tpbudug/edc1631356681441.html
- https://docs.infor.com/ln/2025.x/en-us/lnolh/tpolh/help/tp/ptc/tpptc0170m000.html
- https://docs.infor.com/ln/10.8/en-us/lnolh/tpolh/tpom000024.html
- https://docs.infor.com/ln/2022.x/en-us/lnolh/tpolh/tpom000096.html
- https://docs.infor.com/ln/latest/en-us/lnolh/tpolh/tpom000094.html
- https://docs.infor.com/ln/2022.x/en-us/lnolh/tpolh/tpom000326.html
- https://docs.infor.com/ln/2026.x/en-us/lnolh/procspoug/cover.html
- https://docs.infor.com/ln/2026.x/en-us/lnolh/tdolh/help/td/pur/tdpur6620m000.html
- https://docs.infor.com/ln/latest/en-us/lnolh/tpolh/help/tp/pdm/tppdm6512m000.html
- https://docs.infor.com/ln/2026.x/en-us/lnolh/tpolh/tpom000339.html
- https://docs.infor.com/ln/2022.x/en-us/lnolh/tpolh/tpom000414.html
- https://hexagon.com/company/newsroom/press-releases/2021/hexagon-acquire-infors-eam-business

**CMiC**
- https://cmicglobal.com/products/nexus
- https://cmicglobal.com/pricing
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/New_Functionality/Patch_Releases/Patch_Releases.htm
- https://docs.cmicglobal.com/portal/Content/F_Onboarding/Docs/Project_Controls/Job_Costing/JCJobs.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/JC_-_Job_Costing/Reference/Using_WBS_Capabilities/Assigning_WBS_Codes.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/CM_-_Change_Management/Reference/Local_Tables/PCI_Types.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/CM_-_Change_Management/Reference/PCI_Basics/Transfer.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/GC_Monitor/Reference/GC_Monitor_Screen/Summary_Tab.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/SC_-_Subcontract_Management/Reference/Overview/SC_Lifecycle.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/SC_-_Subcontract_Management/Reference/Payments/Releasing_Retainage.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/EM_-_Equipment_Costing/Reference/Transactions/Automatic_Charge_out.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/EM_-_Equipment_Costing/Reference/Reports/Detailed_Utilization_Report.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/PYU_-_US_Payroll/Reference/Pay_Rates/Union_Pay_Rates.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/JB_-_Job_Billing/Reference/Setup/Control_File/Invoice_Tab.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/JC_-_Job_Costing/Reference/JC_-_R12/Accounting_Methods.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/WKF_-_Workflow/Reference/Workflow_Management.htm
- https://docs.cmicglobal.com/portal/Content/E_Reference_Material/SD_-_System_Data/Reference/Security/User_Audit.htm
- https://www.oracle.com/customers/cmic-construction-enterprise (conflicting hosting claim)

**Acumatica**
- https://www.acumatica.com/pricing/
- https://www.acumatica.com/media/2025/02/Project_Accounting-DS-GBE-20261013.pdf
- https://www.acumatica.com/corporate-newsroom/press-releases/2026-r1-equips-construction-firms-to-scale/
- https://community.acumatica.com/acumatica-news-updates-2/introducing-acumatica-cloud-erp-2026r2-37191
- https://help.myob.com.au/advanced/Docs/Published/UserGuide/Construction__con_Overview_Construction_Processes.html (MYOB Acumatica mirror)
- https://help.myob.com.au/advanced/Docs/Published/UserGuide/PM_65_83_00.html
- https://help.myob.com.au/advanced/Docs/Published/ReleaseNotes/RN_2026_R1_ProjectsConstr_Streamlined_Change_Order_Workflow.html
- https://help.myob.com.au/advanced/Docs/Published/ReleaseNotes/RN_2026_R1_ProjectsConstr_Cost_Projection_Qty_Unit_Rate.html
- https://help.myob.com.au/advanced/Docs/Published/ReleaseNotes/RN_2025_R2_ProjectsConstr_WIP_Adjustments.html
- https://help.myob.com.au/advanced/Docs/Published/UserGuide/Construction_Reports_AIA.html
- https://help.myob.com.au/advanced/Docs/Published/UserGuide/Construction_Lien_Waivers_Process_Activity.html
- https://help.myob.com.au/advanced/Docs/Published/UserGuide/Projects_Inventory_Tracking_To_Track_Quantity_and_Cost.html
- https://help.myob.com.au/advanced/Docs/Published/UserGuide/TimeExpenses_Entering_Employee_Time_Time_Cards.html
- https://community.acumatica.com/construction-120/forecasting-in-acumatica-596 (community)
- https://www.projectmanager.com/blog/acumatica-construction (competitor opinion)

**IFS Cloud**
- https://docs.ifs.com/techdocs/25r2/010_overview/020_high_level_architecture
- https://docs.ifs.com/ifsclouddocs/24r2/ProjectManagement/AboutProjectManagement.htm
- https://docs.ifs.com/ifsclouddocs/25r2/ProjectManagement/AboutProjBnF.htm
- https://docs.ifs.com/ifsclouddocs/25r2/ProjectManagement/AboutProjectCostElements.htm
- https://docs.ifs.com/ifsclouddocs/24r2/ProjectManagement/AboutReportCostHrsAndRevToActivity.htm
- https://docs.ifs.com/ifsclouddocs/24r2/ProjectManagement/AboutEACWorkbench.htm
- https://docs.ifs.com/ifsclouddocs/24r1/ProjectManagement/ActivitySetBaseline.htm
- https://docs.ifs.com/ifsclouddocs/25r2/ProjectManagement/ActivityPerformProjectAnalysis.htm
- https://docs.ifs.com/ifsclouddocs/25r2/SubConValuation/AboutSubContractValuation.htm
- https://docs.ifs.com/ifsclouddocs/24r2/SubConManagement/AboutContractRetentions.htm
- https://docs.ifs.com/ifsclouddocs/26r1/lang/en/ContractChangeManagement/AboutContractChangeManagement.htm
- https://docs.ifs.com/ifsclouddocs/25r1/ContractManagement/AboutSalesContractManagement.htm
- https://docs.ifs.com/ifsclouddocs/25r2/ApplicationForPayment/AboutApplicationForPayment.htm
- https://docs.ifs.com/ifsclouddocs/24r1/MaintainInventory/ProcessTransferProjectInventory.htm
- https://docs.ifs.com/ifsclouddocs/24r1/ProcessModels/Process_Model/managerenttoproject.htm
- https://docs.ifs.com/ifsclouddocs/24r1/TimeReporting/ActivityRegJobHoursAndTimeCardInfo.htm
- https://community.ifs.com/questions/50147 (community)

---
*End of research report. No software has been built.*
