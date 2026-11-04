# RT-05b — Hosting, Data Separation and Resilience: ERP Vendors and Public Clouds

**Research date:** 2026-11-03

**Nature of this document:** research only. It is **not a vendor recommendation**, and it is not an ERP selection. It lists what each vendor documents publicly. Anything not documented is marked **Not found** or **Unverified**. Every item must be confirmed in writing by the vendor (vendor-demo-checklist VD-30).

**Access limits during research:**
- SAP Trust Center pages returned HTTP 403, and IFS PDFs returned HTTP 429. For these two vendors, some facts come from search snippets of official documents and are flagged "(snippet)".
- Per-legal-entity region selection is **not assumed** for any product (Q16-P2).

## Key findings
1. **India and UAE regions are documented for only two vendors.** Of the eight ERPs, only **Business Central** and **Zoho** publicly document both an India region and a UAE region. Odoo documents India but not the UAE. For SAP, Infor, CMiC, Acumatica and IFS, no product-specific India or UAE region was found in public documentation.
2. **Business Central is the only one documenting environments in different geographies within one tenant.** Environment location follows its country localisation (snippet). No other vendor documents multi-region within one customer.
3. **Odoo Online stores backups outside the region.** For the "Middle East & Southern Asia" hosting region, backups are stored in Europe and Canada, and the customer cannot choose or restrict this.
   - This interacts with the India books-backup rule, if that rule applies (statutory-research F.1 and sign-off item C1). Professional validation is needed.
4. **Published availability figures** (where found):

   | Vendor / service | Figure | Note |
   |---|---|---|
   | Business Central | 99.9 % financially backed | — |
   | SAP Public Edition (GROW supplement) | 99.9 % | — |
   | SAP RISE Private Edition | 99.7 % | 2022 supplement |
   | Infor | 99.7 % | — |
   | IFS optional High Availability | 99.95 % | — |
   | Odoo | 99.9 % target | Stated as not legally binding |

   Published RPO/RTO were found only for:
   - **Odoo:** RPO 24 h / RTO 24 h for data-centre loss.
   - **SAP's optional DR service:** RPO 30 min / RTO 12 h long-distance; RPO 0 / RTO 12 h short-distance.

## Table A — Regions, isolation, resilience

| Product | India / UAE region documented | Region choice | Multi-region in one customer | Isolation | Backup / DR | SLA / RPO / RTO |
|---|---|---|---|---|---|---|
| Odoo Online | India: yes. UAE: no. (Region "Middle East & Southern Asia": India and Saudi Arabia) | Database region chosen; backup region cannot be chosen | Not documented | Dedicated database per customer | 14 backups ≥ 3 months, ≥ 3 data centres plus cold storage; ME & S. Asia backups stored in Europe and Canada | 99.9 % target (not binding); RPO/RTO 24 h (DC loss) |
| Odoo.sh | India (Mumbai): yes. UAE: no (Dammam, Saudi Arabia listed) | Chosen at project creation; cannot be changed later | Not documented | Not stated | Production backed up every 24 h, 14 backups; location not stated | Refers to Odoo SLA |
| Zoho | India (Mumbai/Chennai): yes. UAE (Dubai/Abu Dhabi): yes | Data centre set by sign-up country; migration on request (not all apps) | One org across data centres: not documented, and evidence suggests no | Logical (multi-tenant) | Daily incremental + weekly full, 3 months, same data centre; replicated to secondary site | No numeric SLA / RPO / RTO found |
| SAP S/4HANA Cloud Public | Unverified for this product (SAP list names Dubai and Abu Dhabi, not by product; Mumbai only from a community answer) | Some services allow data-centre selection; unconfirmed for Public | Not documented | Unverified | Secondary data centre in same region (SAP statement) | 99.9 % (GROW supplement v12-2024, snippet) |
| SAP RISE / Private Edition | Hyperscaler regions current for India/UAE: Unverified | Data centre fixed in contract | Not documented | Single-tenant | Older SLA: daily full + log backups, 30 days (may be outdated); optional DR | 99.7 %; optional DR RPO 30 min / RTO 12 h (snippet) |
| Business Central online | India: yes. UAE: yes (partner localisation) | Per environment, via its localisation | **Yes**, documented (snippet); limits Unverified | Isolated database per tenant | 28-day point-in-time restore; geo-redundant within same geography | 99.9 % financially backed; no RPO/RTO published |
| Infor CloudSuite | India: Unverified (press reports a 2026 India data centre). UAE: not found | Not documented | Not documented | Multi-tenant | Not published | 99.7 % |
| CMiC | Not found (AWS stated, but an Oracle story says OCI — conflict) | Not documented | Not documented | Multi-tenant | Not documented | Not documented |
| Acumatica SaaS | Not found (only a Canadian region option documented) | Not documented | Not documented | Separate database per subscriber | Daily + log backups; tiered retention to 1 year; second geographic zone | No figure found; RPO/RTO not found |
| IFS Cloud | Unverified (Azure; no current region list) | Not documented; **Remote deployment** lets production run at a customer-chosen location (snippet) | Not documented | Single-tenant | Formal backup policy; secondary Azure site (snippet) | Optional HA 99.95 %; standard SLA/RPO/RTO not published |

## Table B — Support access, security, exit, mobile

| Product | Support-access location | Security features documented | Export / exit | Offline / mobile |
|---|---|---|---|---|
| Odoo | Staff access controlled and audited; location not stated | AES-256 at rest including backups; TLS; OIDC/OAuth; customer 2FA and audit log not officially documented | Manual backup download at any time | Not documented |
| Zoho | Least-privilege, logged access with 2FA; location not stated | TLS 1.2/1.3; AES-256; SAML SSO; MFA; service audit logs | Self-export; deletion cycles after termination | Unverified |
| SAP | Not found | Not verified | Not verified | Not verified |
| Business Central | Microsoft staff with Customer Lockbox approval; location not stated | Entra ID; MFA / Conditional Access; TDE and encrypted backups; customer-managed key; Purview audit log (180 days default) | Database export (BACPAC) to customer storage; exports logged | Not documented; Copilot may process data outside the geography (consent toggle) |
| Infor | Not documented | Not documented | Data returned in native form within 30 days of contract end, on request | Not documented |
| CMiC | Not documented | SOC 3; access controls; SSO/MFA/encryption not stated | Not documented | "CONSTRUCT" suite claims low-connectivity working (marketing) |
| Acumatica | Not documented | TLS; built-in MFA; SSO via a module; at-rest encryption not stated | Database copy on request within 10 days of expiry; restorable backups (fee) | Offline mobile entry Unverified |
| IFS | Not documented | Azure single-tenant; other details not retrievable | Not documented | Not documented |

## Public cloud regions (from official region pages)

| Cloud | India | UAE / nearby |
|---|---|---|
| AWS | ap-south-1 Mumbai (default); ap-south-2 Hyderabad (opt-in) | me-central-1 UAE (opt-in); me-south-1 Bahrain (opt-in) |
| Azure | Central India (Pune); South India (Chennai); West India (Mumbai); India South Central (Hyderabad) | UAE North (Dubai); UAE Central (Abu Dhabi, restricted to DR scenarios); Qatar Central |
| Google Cloud | asia-south1 Mumbai; asia-south2 Delhi | **No UAE region listed**; Doha, Dammam (special access terms), Tel Aviv |
| Oracle OCI | Mumbai; Hyderabad | Dubai; Abu Dhabi |

## Implications for requirements (analysis; not approved)
- **NF-01 / NF-03 (data location):** per-entity data location must be verified product by product (VD-30). Public documentation supports it clearly for only one ERP. For most others, written vendor confirmation is required.
- **Backup location is a separate question from production region:**
  - Odoo Online's backups for the Middle East & South Asia region are stored offshore.
  - Zoho keeps backups in the same data centre.
  - Business Central keeps backups within the same geography.
  - Backup location must be checked against the India books-backup rule (if applicable), CERT-In log rules and contract clauses (statutory-research F.3).
- **PMS hosting:** if the PMS is extended or developed by us (Q17-P1), the public-cloud regions above show that India and UAE hosting is *available* from AWS, Azure and Oracle. Google lists no UAE region.
  - Region availability is not a design decision. Design is not started.
- **RPO/RTO (NF-10):** few vendors publish figures. Targets must come from the business (Q16-O2) and then be agreed contractually.

## Sources
- https://www.odoo.com/cloud-sla
- https://www.odoo.com/security
- https://www.odoo.com/privacy
- https://www.odoo.sh/faq
- https://www.zoho.com/know-your-datacenter.html
- https://www.zoho.com/security.html
- https://help.zoho.com/portal/en/kb/accounts/manage-your-zoho-account/articles/dc-migration
- https://www.zoho.com/books/kb/users-and-roles/user-domain.html
- https://www.sap.com/about/trust-center/data-center.html (403)
- SAP agreement PDFs:
  - data-centre list v9-2026
  - GROW supplement v12-2024
  - RISE supplement v9-2022
  - DR service description v7-2023
  - third-party DC list v8-2020

  (assets.cdn.sap.com / sap.com; snippets)
- https://community.sap.com/t5/enterprise-resource-planning-q-a/data-centre-location-request/m-p/12695872
- https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/compliance/apptest-countries-and-translations
- https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/service-overview
- https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/security/security-online
- https://learn.microsoft.com/en-us/azure/azure-sovereign-clouds/public/sovereign-controls-d365-business-central (snippet)
- https://learn.microsoft.com/en-za/dynamics365/business-central/dev-itpro/administration/tenant-admin-center-database-export
- https://www.infor.com/mea/customer-center/customer-bill-of-rights
- https://www.infor.com/pl-pl/partners/aws (snippet)
- Press: https://www.newsbytesapp.com/news/business/infor-amplifies-india-investment-with-new-data-centre/tldr
- https://cmicglobal.com/resources/article/CMiC-the-leading-next-generation-cloud-based-construction-ERP
- https://www.oracle.com/customers/cmic-construction-enterprise
- https://www.acumatica.com/acumatica-saas-faq/
- https://www.acumatica.com/blog/mfa-sso-setup-guide-licensing/
- https://www.ifs.com/about/trust-center (snippet)
- IFS cloud-security fact sheet 2023 (429; snippet)
- IFS Additional Services fact sheet (snippet)
- https://docs.ifs.com/techdocs/23r2/070_remote_deploy/010_installing_fresh_system/010_planning_installation/003_overview_remote (snippet)
- https://community.ifs.com/ifs-cloud-new-customers-365/implications-of-moving-the-azure-region-our-cloud-and-mwm-instances-are-in-66314
- https://docs.aws.amazon.com/global-infrastructure/latest/regions/aws-regions.html
- https://learn.microsoft.com/en-us/azure/reliability/regions-list
- https://docs.cloud.google.com/compute/docs/regions-zones
- https://docs.oracle.com/en-us/iaas/Content/General/Concepts/regions.htm
