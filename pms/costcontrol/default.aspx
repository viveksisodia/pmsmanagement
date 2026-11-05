<%@ Page Title="Cost Control" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pms_costcontrol_default" %>
<asp:Content ID="CostControlHead" ContentPlaceHolderID="head" runat="Server">
    <script>
        window.CostControlPage = { regid: <%= pageRegId %>, employeeId: <%= pageEmployeeId %>, isadmin: <%= isadmin %>, canEnter: <%= canEnter %>, canApprove: <%= canApprove %> };
    </script>
    <script src="app/costcontrol.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <style>
        [ng-cloak]{display:none!important}
        .cc-page{padding:37px 2px 14px;background:#f1f5f7;min-height:calc(100vh - 110px);color:#252a31}
        .cc-card{background:#fff;border:1px solid #e3e9ee}
        .cc-titlebar{display:flex;align-items:center;justify-content:space-between;gap:10px;min-height:38px;padding:7px 12px;border-bottom:1px solid #eceef1;flex-wrap:wrap}
        .cc-title{margin:0;font-size:18px;font-weight:600}
        .cc-project{display:flex;align-items:center;gap:8px}
        .cc-project label{margin:0;font-size:12px;color:#5e6672}
        .cc-project select{min-width:280px}
        .cc-page select,.cc-page input,.cc-page textarea{min-height:32px;border:1px solid #d9dde3;border-radius:4px;padding:4px 8px;font-size:12px;background:#fff}
        .cc-page input,.cc-page input:hover{border-bottom:1px solid #d9dde3!important}
        /* searchable drop-down (cc-search-select) */
        cc-search-select{display:block;min-width:0}
        .cc-ss{position:relative;width:100%}
        .cc-ss-trigger{display:flex;align-items:center;justify-content:space-between;gap:8px;width:100%;min-height:32px;padding:4px 8px;border:1px solid #d9dde3;border-radius:4px;background:#fff;font-size:12px;color:#252a31;text-align:left;cursor:pointer}
        .cc-ss-trigger:focus,.cc-ss-open .cc-ss-trigger{outline:0;border-color:#7b0b69}
        .cc-ss-trigger:disabled{background:#f4f5f7;color:#8a9099;cursor:not-allowed}
        .cc-ss-text{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
        .cc-ss-placeholder{color:#7a828c}
        .cc-ss-arrow{flex:0 0 auto;color:#59636e;font-size:11px}
        .cc-ss-popup{position:absolute;left:0;top:calc(100% + 3px);z-index:10050;width:100%;min-width:240px;padding:7px;background:#fff;border:1px solid #d9dde3;border-radius:6px;box-shadow:0 8px 24px rgba(20,30,40,.18)}
        .cc-page .cc-ss-search,.cc-page .cc-ss-search:hover{width:100%;height:34px;padding:5px 10px;margin-bottom:6px;background:#ffff00;border:1px solid #333!important;border-radius:6px;font-size:13px;outline:0}
        .cc-ss-list{max-height:230px;overflow-y:auto}
        .cc-ss-option{display:block;width:100%;padding:8px 10px;border:0;border-radius:4px;background:#fff;text-align:left;font-size:13px;color:#252a31;cursor:pointer}
        .cc-ss-option.cc-ss-active{background:#f3edf6}
        .cc-ss-option.cc-ss-selected{font-weight:700;color:#7b0b69}
        .cc-ss-option.cc-ss-emptyopt{color:#5e6672}
        .cc-ss-option.cc-ss-addnew{margin-top:4px;border-top:1px solid #eceef1;border-radius:0;color:#7b0b69;font-weight:600}
        .cc-ss-none{padding:9px 10px;font-size:12px;color:#7a828c}
        .cc-ss-project{width:300px}
        .cc-ss-version{width:240px}
        .cc-ss-small{width:150px}
        .cc-tabs{display:flex;gap:2px;padding:0 12px;border-bottom:1px solid #eceef1}
        .cc-tab{border:0;background:transparent;padding:9px 14px;font-size:13px;color:#5e6672;cursor:pointer;border-bottom:2px solid transparent}
        .cc-tab.active{color:#7b0b69;border-bottom-color:#7b0b69;font-weight:600}
        .cc-toolbar{display:flex;align-items:center;gap:8px;padding:8px 12px;border-bottom:1px solid #eceef1;flex-wrap:wrap}
        .cc-spacer{flex:1}
        .cc-btn{border:0;border-radius:4px;min-height:32px;padding:5px 12px;font-size:12px;font-weight:600;cursor:pointer;background:#f1f3f5;color:#343a40}
        .cc-btn-primary{background:#7b0b69;color:#fff}
        .cc-btn-danger{background:#fbe9e9;color:#a61b1b}
        .cc-btn:disabled{opacity:.5;cursor:not-allowed}
        .cc-badge{display:inline-block;padding:3px 8px;border-radius:10px;font-size:11px;font-weight:600}
        .cc-badge-draft{background:#fff3cd;color:#7a5d00}
        .cc-badge-locked{background:#dff5e5;color:#23743c}
        .cc-alert{margin:8px 12px 0;padding:8px 10px;border-radius:4px;font-size:12px;background:#fff1f0;border:1px solid #ffd7d4;color:#a61b1b}
        .cc-info{margin:8px 12px 0;padding:8px 10px;border-radius:4px;font-size:12px;background:#eef6ff;border:1px solid #cfe3fb;color:#285ca8}
        .cc-tablewrap{overflow:auto;max-height:calc(100vh - 300px)}
        .cc-table{width:100%;border-collapse:collapse;min-width:900px}
        .cc-table th{position:sticky;top:0;background:#eaf3f7;color:#111;font-size:12px;font-weight:500;padding:6px 7px;border:1px solid #e0eaf1;text-align:left;white-space:nowrap}
        .cc-table td{font-size:12px;padding:5px 7px;border:1px solid #e0eaf1;vertical-align:middle}
        .cc-table tbody tr:nth-child(even){background:#fafbfd}
        .cc-table tfoot td{font-weight:700;background:#f4f7f9}
        .cc-num{text-align:right!important;white-space:nowrap}
        .cc-neg{color:#a61b1b;font-weight:600}
        .cc-empty{padding:28px!important;text-align:center!important;color:#7a828c}
        .cc-actions{white-space:nowrap;width:1%}
        .cc-modal-backdrop{position:fixed;inset:0;background:rgba(0,0,0,.4);z-index:10000;display:flex;align-items:center;justify-content:center}
        .cc-modal{background:#fff;border-radius:8px;padding:18px;width:560px;max-width:95vw;max-height:90vh;overflow:auto}
        .cc-modal h4{margin:0 0 10px;font-size:16px}
        .cc-modal label{display:block;margin:9px 0 3px;font-size:12px;color:#4a525c}
        .cc-modal input,.cc-modal select,.cc-modal textarea{width:100%}
        .cc-grid2{display:grid;grid-template-columns:1fr 1fr;gap:0 12px}
        .cc-grid3{display:grid;grid-template-columns:1fr 1fr 1fr;gap:0 12px}
        .cc-modal-footer{display:flex;gap:8px;justify-content:flex-end;margin-top:14px}
        .cc-quickadd{margin-top:8px;padding:8px 10px;border:1px dashed #b98ab2;border-radius:6px;background:#fbf7fb}
        .cc-hint{font-size:11px;color:#7a828c;margin-top:3px}
        @media(max-width:650px){.cc-project select{min-width:0;width:100%}.cc-project{width:100%}.cc-ss-project,.cc-ss-version{width:100%}.cc-grid2,.cc-grid3{grid-template-columns:1fr}}
        .cc-badge-submitted{background:#e6efff;color:#285ca8}
        .cc-tabs{overflow-x:auto;white-space:nowrap}
        .cc-meta{font-size:12px;color:#5e6672}
        .cc-warn{color:#7a5d00}
        .cc-lock{color:#8a9099;font-size:11px;white-space:nowrap}
        .cc-indent-1{padding-left:18px!important}.cc-indent-2{padding-left:36px!important}
        .cc-level{display:inline-block;padding:1px 6px;border-radius:8px;font-size:10px;font-weight:600;background:#f1f3f5;color:#5e6672}
        .cc-check{width:auto!important;min-height:0!important;margin:0}
        .cc-diff{font-size:11px;color:#4a525c;white-space:normal;max-width:520px}
        .cc-link{border:0;background:none;color:#285ca8;cursor:pointer;padding:0;font-size:12px;text-align:left}
        .cc-link:hover{text-decoration:underline}
    </style>
</asp:Content>
<asp:Content ID="CostControlContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="cc-page" ng-app="pmsCostControlApp" ng-controller="CostControlController" ng-cloak onkeydown="if(event.keyCode===13 && event.target.tagName!=='TEXTAREA'){event.preventDefault();}">
        <div class="cc-card">
            <div class="cc-titlebar">
                <h3 class="cc-title">Cost Control</h3>
                <div class="cc-project" ng-if="needsProject()">
                    <label>Project</label>
                    <cc-search-select class="cc-ss-project" ng-model="selection.projectId" items="projects" value-key="id" label-key="ProjectName" placeholder="-- Select project --" ng-change="projectChanged()"></cc-search-select>
                </div>
            </div>

            <div class="cc-tabs" role="tablist">
                <button type="button" class="cc-tab" ng-repeat="t in visibleTabs() track by t.id" ng-class="{active:tab===t.id}" ng-click="openTab(t.id)">{{t.name}}</button>
            </div>

            <div class="cc-alert" ng-if="error">{{error}}</div>
            <div class="cc-info" ng-if="notice">{{notice}}</div>
            <div class="cc-info" ng-if="needsProject() && !selection.projectId">Select a project to continue.</div>
            <div class="cc-info" ng-if="needsProject() && selection.projectId && !postingCodeOptions.length && (tab==='budget' || tab==='actuals' || tab==='forecast')">No Element-level cost codes yet. {{access.IsAdmin ? 'Add them in the Cost Codes tab, or choose "+ Add new cost code" in the Cost Code drop-down.' : 'Ask an administrator to add cost codes.'}}</div>

            <!-- ================= BUDGET ================= -->
            <div ng-if="tab==='budget' && selection.projectId">
                <div class="cc-toolbar">
                    <label style="margin:0;font-size:12px">Version</label>
                    <cc-search-select class="cc-ss-version" ng-model="budget.versionId" items="versionOptions" value-key="id" label-key="Label" placeholder="-- No version --" ng-change="loadBudget()" is-disabled="!budget.versions.length"></cc-search-select>
                    <span class="cc-badge" ng-if="currentVersion()" ng-class="{'cc-badge-draft':currentVersion().VersionStatus==='Draft','cc-badge-submitted':currentVersion().VersionStatus==='Submitted','cc-badge-locked':currentVersion().VersionStatus==='Locked'}">{{currentVersion().VersionStatus==='Locked' ? 'Approved / Locked' : currentVersion().VersionStatus}}</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-if="access.CanEnter" ng-click="openNewVersion()" ng-disabled="busy || hasOpenVersion()">{{budget.versions.length ? 'New Revision' : 'Create Original Budget'}}</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter && isDraft()" ng-click="openBudgetLine()" ng-disabled="busy">Add Line</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter && isDraft()" ng-click="submitVersion()" ng-disabled="busy || !budget.lines.length">Submit for Approval</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanApprove && isSubmitted()" ng-click="approveVersion()" ng-disabled="busy || isOwnSubmission()" title="{{isOwnSubmission() ? 'You submitted this budget; another approver must approve it.' : 'Approve and lock'}}">Approve &amp; Lock</button>
                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanApprove && isSubmitted()" ng-click="openReject()" ng-disabled="busy">Return to Draft</button>
                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanEnter && isDraft()" ng-click="discardDraft()" ng-disabled="busy">Discard Draft</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('budget')" ng-disabled="!budget.versions.length">Export Excel</button>
                </div>
                <div class="cc-toolbar cc-meta" ng-if="currentVersion()">
                    <span ng-if="currentVersion().Reason">Reason: {{currentVersion().Reason}}</span>
                    <span ng-if="currentVersion().SubmittedOn">Submitted by {{currentVersion().SubmittedByName || 'employee ' + currentVersion().SubmittedBy}} on {{currentVersion().SubmittedOn}}</span>
                    <span ng-if="currentVersion().LockedOn">Approved by {{currentVersion().LockedByName}} on {{currentVersion().LockedOn}}</span>
                    <span class="cc-warn" ng-if="currentVersion().VersionStatus==='Draft' && currentVersion().RejectReason">Returned: {{currentVersion().RejectReason}}</span>
                    <span class="cc-warn" ng-if="isOwnSubmission() && access.CanApprove">You submitted this version, so another approver must approve it.</span>
                    <span ng-if="isSubmitted() && !access.CanApprove">Waiting for approval.</span>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Cost Code</th><th>Type</th><th>Milestone</th><th>Task</th><th class="cc-num">Qty</th><th>Unit</th><th class="cc-num">Rate</th><th class="cc-num">Amount</th><th>Remarks</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!budget.versions.length"><td colspan="10" class="cc-empty">No budget yet.{{access.CanEnter ? ' Click "Create Original Budget".' : ''}}</td></tr>
                            <tr ng-if="budget.versions.length && !budget.lines.length"><td colspan="10" class="cc-empty">No lines in this version.</td></tr>
                            <tr ng-repeat="line in budget.lines track by line.id">
                                <td>{{line.CostCode}} - {{line.CostCodeName}}</td><td>{{line.CostType}}</td>
                                <td>{{line.MilestoneName || 'Project level'}}</td><td>{{line.TaskName}}</td>
                                <td class="cc-num">{{line.Quantity | number:2}}</td><td>{{line.Unit}}</td>
                                <td class="cc-num">{{line.Rate | number:2}}</td><td class="cc-num">{{line.Amount | number:2}}</td>
                                <td>{{line.Remarks}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn" ng-if="access.CanEnter && isDraft()" ng-click="openBudgetLine(line)">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanEnter && isDraft()" ng-click="deleteBudgetLine(line)">Delete</button>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="budget.lines.length"><tr><td colspan="7">Total</td><td class="cc-num">{{sum(budget.lines,'Amount') | number:2}}</td><td colspan="2"></td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= COST ENTRIES ================= -->
            <div ng-if="tab==='actuals' && selection.projectId">
                <div class="cc-toolbar">
                    <input type="search" ng-model="actuals.search" placeholder="Search entries..." aria-label="Search cost entries" style="min-width:240px" />
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter" ng-click="openCostEntry()" ng-disabled="busy">Add Cost Entry</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('entries')" ng-disabled="!actuals.entries.length">Export Excel</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Date</th><th>Cost Code</th><th>Milestone</th><th>Task</th><th>Site</th><th>Source Doc</th><th>Reference</th><th class="cc-num">Amount</th><th>Remarks</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredEntries().length"><td colspan="10" class="cc-empty">No cost entries.</td></tr>
                            <tr ng-repeat="entry in filteredEntries() track by entry.id">
                                <td>{{entry.EntryDate}}</td><td>{{entry.CostCode}} - {{entry.CostCodeName}}</td>
                                <td>{{entry.MilestoneName || 'Project level'}}</td><td>{{entry.TaskName}}</td>
                                <td>{{entry.SiteName}}</td><td>{{entry.SourceDocType}}</td><td>{{entry.ReferenceNo}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':entry.Amount<0}">{{entry.Amount | number:2}}</td>
                                <td>{{entry.Remarks}}</td>
                                <td class="cc-actions">
                                    <span class="cc-lock" ng-if="entry.IsPeriodClosed==1">Month closed</span>
                                    <button type="button" class="cc-btn" ng-if="access.CanEnter && entry.SourceType==='Manual' && entry.IsPeriodClosed!=1" ng-click="openCostEntry(entry)">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanEnter && entry.SourceType==='Manual' && entry.IsPeriodClosed!=1" ng-click="openDeleteEntry(entry)">Delete</button>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="filteredEntries().length"><tr><td colspan="7">Total</td><td class="cc-num">{{sum(filteredEntries(),'Amount') | number:2}}</td><td colspan="2"></td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= FORECAST (ETC) ================= -->
            <div ng-if="tab==='forecast' && selection.projectId">
                <div class="cc-toolbar">
                    <span class="cc-meta">Estimate to complete (ETC) = cost still expected for the remaining work. EAC = Actual + ETC. VAC = Current budget - EAC.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter" ng-click="openEtc()" ng-disabled="busy">Add ETC</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('etc')" ng-disabled="!forecast.rows.length">Export Excel</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Cost Code</th><th>Milestone</th><th class="cc-num">Current Budget</th><th class="cc-num">Actual</th><th class="cc-num">ETC</th><th class="cc-num">EAC</th><th class="cc-num">VAC</th><th>As of</th><th>Remarks</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!forecast.rows.length"><td colspan="10" class="cc-empty">No budget, cost or estimate yet.</td></tr>
                            <tr ng-repeat="row in forecast.rows track by $index">
                                <td>{{row.CostCode}} - {{row.CostCodeName}}</td><td>{{row.MilestoneName || 'Project level'}}</td>
                                <td class="cc-num">{{row.CurrentBudget | number:2}}</td><td class="cc-num">{{row.Actual | number:2}}</td>
                                <td class="cc-num">{{row.EtcAmount===null ? '-' : (row.EtcAmount | number:2)}}</td>
                                <td class="cc-num">{{eacOf(row) | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.CurrentBudget - eacOf(row) < 0}">{{row.CurrentBudget - eacOf(row) | number:2}}</td>
                                <td>{{row.AsOfDate}}</td><td>{{row.Remarks}}</td>
                                <td class="cc-actions"><button type="button" class="cc-btn" ng-if="access.CanEnter" ng-click="openEtc(row)">{{row.EtcId ? 'Edit' : 'Set ETC'}}</button></td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="forecast.rows.length"><tr><td colspan="2">Total</td>
                            <td class="cc-num">{{sum(forecast.rows,'CurrentBudget') | number:2}}</td><td class="cc-num">{{sum(forecast.rows,'Actual') | number:2}}</td>
                            <td class="cc-num">{{sum(forecast.rows,'EtcAmount') | number:2}}</td>
                            <td class="cc-num">{{sum(forecast.rows,'Actual') + sum(forecast.rows,'EtcAmount') | number:2}}</td>
                            <td class="cc-num">{{sum(forecast.rows,'CurrentBudget') - sum(forecast.rows,'Actual') - sum(forecast.rows,'EtcAmount') | number:2}}</td>
                            <td colspan="3"></td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= BUDGET VS ACTUAL ================= -->
            <div ng-if="tab==='report' && selection.projectId">
                <div class="cc-toolbar">
                    <label style="margin:0;font-size:12px">Group by</label>
                    <cc-search-select class="cc-ss-small" ng-model="report.groupBy" items="groupByOptions" value-key="id" label-key="Name" ng-change="loadReport()"></cc-search-select>
                    <span class="cc-meta" ng-if="report.header.CurrentVersionNo">Current budget = locked version {{report.header.CurrentVersionNo}}</span>
                    <span class="cc-warn" style="font-size:12px" ng-if="report.header.DraftCount>0">A draft or submitted revision exists and is not included until it is approved.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadReport()" ng-disabled="busy">Refresh</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('report')" ng-disabled="!report.rows.length">Export Excel</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>{{groupLabel()}}</th><th ng-if="report.groupBy==='COSTCODE'">Type</th><th class="cc-num">Original Budget</th><th class="cc-num">Current Budget</th><th class="cc-num">Actual</th><th class="cc-num">Remaining Budget</th><th class="cc-num">ETC</th><th class="cc-num">EAC</th><th class="cc-num">VAC</th><th class="cc-num">Consumed %</th></tr></thead>
                        <tbody>
                            <tr ng-if="!report.rows.length"><td colspan="10" class="cc-empty">No approved budget, cost entries or estimates yet.</td></tr>
                            <tr ng-repeat="row in report.rows track by row.GroupId">
                                <td>{{row.GroupName}}</td><td ng-if="report.groupBy==='COSTCODE'">{{row.CostType}}</td>
                                <td class="cc-num">{{row.OriginalBudget | number:2}}</td><td class="cc-num">{{row.CurrentBudget | number:2}}</td>
                                <td class="cc-num">{{row.Actual | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.Variance<0}">{{row.Variance | number:2}}</td>
                                <td class="cc-num">{{row.Etc | number:2}}</td><td class="cc-num">{{row.Eac | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.Vac<0}">{{row.Vac | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.ConsumedPercent>100}">{{row.ConsumedPercent===null ? '-' : (row.ConsumedPercent | number:2) + ' %'}}</td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="report.rows.length"><tr>
                            <td ng-attr-colspan="{{report.groupBy==='COSTCODE' ? 2 : 1}}">Total</td>
                            <td class="cc-num">{{sum(report.rows,'OriginalBudget') | number:2}}</td><td class="cc-num">{{sum(report.rows,'CurrentBudget') | number:2}}</td>
                            <td class="cc-num">{{sum(report.rows,'Actual') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(report.rows,'Variance')<0}">{{sum(report.rows,'Variance') | number:2}}</td>
                            <td class="cc-num">{{sum(report.rows,'Etc') | number:2}}</td><td class="cc-num">{{sum(report.rows,'Eac') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(report.rows,'Vac')<0}">{{sum(report.rows,'Vac') | number:2}}</td>
                            <td class="cc-num">{{totalConsumed(report.rows)}}</td>
                        </tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= PROJECT SUMMARY ================= -->
            <div ng-if="tab==='summary'">
                <div class="cc-toolbar">
                    <span class="cc-meta">All projects with a budget, cost or estimate. Click a project to open its Budget vs Actual.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadSummary()" ng-disabled="busy">Refresh</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('summary')" ng-disabled="!summary.rows.length">Export Excel</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Project</th><th>Budget</th><th class="cc-num">Current Budget</th><th class="cc-num">Actual</th><th class="cc-num">ETC</th><th class="cc-num">EAC</th><th class="cc-num">VAC</th><th class="cc-num">Consumed %</th></tr></thead>
                        <tbody>
                            <tr ng-if="!summary.rows.length"><td colspan="8" class="cc-empty">No project has Cost Control data yet.</td></tr>
                            <tr ng-repeat="row in summary.rows track by row.ProjectId">
                                <td><button type="button" class="cc-link" ng-click="openProjectReport(row.ProjectId)">{{row.ProjectName}}</button></td>
                                <td>{{row.CurrentVersionNo ? 'V' + row.CurrentVersionNo + ' locked' : 'No approved budget'}}<span class="cc-warn" ng-if="row.OpenVersionStatus"> ({{row.OpenVersionStatus}} revision open)</span></td>
                                <td class="cc-num">{{row.CurrentBudget | number:2}}</td><td class="cc-num">{{row.Actual | number:2}}</td>
                                <td class="cc-num">{{row.Etc | number:2}}</td><td class="cc-num">{{row.Eac | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.Vac<0}">{{row.Vac | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.ConsumedPercent>100}">{{row.ConsumedPercent===null ? '-' : (row.ConsumedPercent | number:2) + ' %'}}</td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="summary.rows.length"><tr><td colspan="2">Total</td>
                            <td class="cc-num">{{sum(summary.rows,'CurrentBudget') | number:2}}</td><td class="cc-num">{{sum(summary.rows,'Actual') | number:2}}</td>
                            <td class="cc-num">{{sum(summary.rows,'Etc') | number:2}}</td><td class="cc-num">{{sum(summary.rows,'Eac') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(summary.rows,'Vac')<0}">{{sum(summary.rows,'Vac') | number:2}}</td>
                            <td class="cc-num">{{totalConsumed(summary.rows)}}</td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= PERIODS ================= -->
            <div ng-if="tab==='periods' && selection.projectId">
                <div class="cc-toolbar">
                    <span class="cc-meta">A closed month blocks adding, changing or deleting cost entries dated in that month. Reopening needs a reason.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadPeriods()" ng-disabled="busy">Refresh</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Month</th><th>Status</th><th class="cc-num">Entries</th><th class="cc-num">Amount</th><th>Closed</th><th>Last reopened</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-repeat="p in periods.rows track by p.PeriodStart">
                                <td>{{monthLabel(p.PeriodStart)}}</td>
                                <td><span class="cc-badge" ng-class="p.IsClosed==1 ? 'cc-badge-locked' : 'cc-badge-draft'">{{p.IsClosed==1 ? 'Closed' : 'Open'}}</span></td>
                                <td class="cc-num">{{p.EntryCount}}</td><td class="cc-num">{{p.EntryTotal | number:2}}</td>
                                <td>{{p.ClosedOn ? p.ClosedByName + ', ' + p.ClosedOn : ''}}</td>
                                <td>{{p.ReopenedOn ? p.ReopenedByName + ', ' + p.ReopenedOn + ' - ' + p.ReopenReason : ''}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanApprove && p.IsClosed!=1" ng-click="closePeriod(p)" ng-disabled="busy">Close Month</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanApprove && p.IsClosed==1" ng-click="openReopen(p)" ng-disabled="busy">Reopen</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- ================= HISTORY ================= -->
            <div ng-if="tab==='history' && (selection.projectId || history.companyLevel)">
                <div class="cc-toolbar">
                    <label class="cc-meta" ng-if="access.IsAdmin" style="display:flex;align-items:center;gap:6px;margin:0"><input type="checkbox" class="cc-check" ng-model="history.companyLevel" ng-change="loadHistory()" /> Company-level history (cost codes, access)</label>
                    <input type="search" ng-model="history.search" placeholder="Search history..." aria-label="Search history" style="min-width:220px" />
                    <span class="cc-spacer"></span>
                    <span class="cc-meta">Latest 500 actions. History cannot be edited or deleted.</span>
                    <button type="button" class="cc-btn" ng-click="loadHistory()" ng-disabled="busy">Refresh</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>When</th><th>User</th><th>Record</th><th>Action</th><th>Reason</th><th>Changes</th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredHistory().length"><td colspan="6" class="cc-empty">No history yet.</td></tr>
                            <tr ng-repeat="h in filteredHistory() track by h.id">
                                <td>{{h.ActionTime}}</td><td>{{h.UserName}}</td><td>{{entityLabel(h.EntityType)}}{{h.EntityId ? ' #' + h.EntityId : ''}}</td>
                                <td>{{h.Action}}</td><td>{{h.Reason}}</td><td class="cc-diff">{{h.Changes}}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- ================= COST CODES (admin) ================= -->
            <div ng-if="tab==='codes'">
                <div class="cc-toolbar">
                    <input type="search" ng-model="codes.search" placeholder="Search cost codes..." aria-label="Search cost codes" style="min-width:240px" />
                    <span class="cc-meta">Division &gt; Section &gt; Element. Only Element codes are used in budgets, cost entries and forecasts. A code that is in use can only have its description changed.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn cc-btn-primary" ng-click="openCostCode()" ng-disabled="busy">Add Cost Code</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Code</th><th>Level</th><th>Name</th><th>Cost Type</th><th>Parent</th><th>Description</th><th>In Use</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredCodes().length"><td colspan="8" class="cc-empty">No cost codes.</td></tr>
                            <tr ng-repeat="code in filteredCodes() track by code.id">
                                <td ng-class="{'cc-indent-1':code.CodeLevel==='Section','cc-indent-2':code.CodeLevel==='Element' && code.ParentId}">{{code.Code}}</td>
                                <td><span class="cc-level">{{code.CodeLevel}}</span></td><td>{{code.Name}}</td><td>{{code.CostType}}</td>
                                <td>{{code.ParentCode ? code.ParentCode + ' - ' + code.ParentName : ''}}</td><td>{{code.Description}}</td>
                                <td>{{code.IsUsed==1 ? 'Yes' : 'No'}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn" ng-click="openCostCode(code)">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-click="deleteCostCode(code)" ng-disabled="code.IsUsed==1 || code.HasChildren==1" title="{{code.IsUsed==1 ? 'Used in budgets, cost entries or forecasts' : (code.HasChildren==1 ? 'Has child codes' : 'Delete')}}">Delete</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- ================= ACCESS (admin) ================= -->
            <div ng-if="tab==='access'">
                <div class="cc-toolbar">
                    <input type="search" ng-model="accessList.search" placeholder="Search employees..." aria-label="Search employees" style="min-width:240px" />
                    <span class="cc-meta">View: see Cost Control. Enter: budgets, cost entries, forecasts. Approve: approve budgets and close months (the submitter can never approve their own budget). Administrators can view and enter, but approve only when ticked here.</span>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table" style="min-width:600px">
                        <thead><tr><th>Employee</th><th>View</th><th>Enter</th><th>Approve</th><th></th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredAccess().length"><td colspan="5" class="cc-empty">No employees.</td></tr>
                            <tr ng-repeat="a in filteredAccess() track by a.EmployeeId">
                                <td>{{a.EmployeeName}}</td>
                                <td><input type="checkbox" class="cc-check" ng-model="a.view" ng-change="saveAccess(a,'view')" ng-disabled="a.saving || a.enter || a.approve" aria-label="View" /></td>
                                <td><input type="checkbox" class="cc-check" ng-model="a.enter" ng-change="saveAccess(a,'enter')" ng-disabled="a.saving" aria-label="Enter" /></td>
                                <td><input type="checkbox" class="cc-check" ng-model="a.approve" ng-change="saveAccess(a,'approve')" ng-disabled="a.saving" aria-label="Approve" /></td>
                                <td class="cc-meta">{{a.saving ? 'Saving...' : a.message}}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- ================= MODALS ================= -->
        <div class="cc-modal-backdrop" ng-if="modal.type" ng-keydown="$event.keyCode===27 && closeModal()">
            <div class="cc-modal" role="dialog" aria-modal="true">
                <div class="cc-alert" style="margin:0 0 8px" ng-if="modal.error">{{modal.error}}</div>

                <div ng-if="modal.type==='version'">
                    <h4>{{budget.versions.length ? 'New Budget Revision' : 'Create Original Budget'}}</h4>
                    <div class="cc-hint" ng-if="budget.versions.length">The revision starts as a copy of the last approved version. A reason is required.</div>
                    <label>Reason{{budget.versions.length ? ' *' : ''}}</label>
                    <textarea rows="3" ng-model="modal.data.reason" maxlength="1000"></textarea>
                </div>

                <div ng-if="modal.type==='reason'">
                    <h4>{{modal.data.title}}</h4>
                    <div class="cc-hint">{{modal.data.hint}}</div>
                    <label>Reason *</label>
                    <textarea rows="3" ng-model="modal.data.reason" maxlength="1000"></textarea>
                </div>

                <div ng-if="modal.type==='line'">
                    <h4>{{modal.data.Id ? 'Edit' : 'Add'}} Budget Line</h4>
                    <label>Cost Code *</label>
                    <cc-search-select ng-model="modal.data.CostCodeId" items="postingCodeOptions" value-key="id" label-key="Label" placeholder="-- Select --" add-new-label="{{access.IsAdmin ? '+ Add new cost code' : ''}}" on-add-new="startQuickCode()"></cc-search-select>
                    <div class="cc-quickadd" ng-if="quickCode.open" ng-include="'quickCode.html'"></div>
                    <div class="cc-grid2">
                        <div><label>Milestone</label>
                            <cc-search-select ng-model="modal.data.MilestoneId" items="structure.milestones" value-key="id" label-key="MilestoneName" empty-label="Project level" ng-change="milestoneChanged()"></cc-search-select></div>
                        <div><label>Task</label>
                            <cc-search-select ng-model="modal.data.TaskId" items="modalTasks" value-key="id" label-key="TaskName" empty-label="-- None --" is-disabled="!modal.data.MilestoneId"></cc-search-select></div>
                    </div>
                    <div class="cc-grid3">
                        <div><label>Quantity</label><input type="number" step="0.0001" ng-model="modal.data.Quantity" ng-change="recalculateLineAmount()" /></div>
                        <div><label>Unit</label><cc-search-select ng-model="modal.data.UnitId" items="units" value-key="id" label-key="UnitName" empty-label="-- Select --" add-new-label="+ Add new unit" on-add-new="startQuickUnit()"></cc-search-select></div>
                        <div><label>Rate</label><input type="number" step="0.0001" ng-model="modal.data.Rate" ng-change="recalculateLineAmount()" /></div>
                    </div>
                    <div class="cc-quickadd" ng-if="quickUnit.open"><label style="margin-top:0">New Unit Name *</label><input type="text" maxlength="50" ng-model="quickUnit.Name" placeholder="e.g. Bag, Cum, Sqm" /><div class="cc-hint">Added to the common unit master (tbl_unitmaster), so it is also available in other ERP screens.</div><div class="cc-alert" style="margin:6px 0 0" ng-if="quickUnit.error">{{quickUnit.error}}</div><div class="cc-modal-footer" style="margin-top:8px"><button type="button" class="cc-btn" ng-click="cancelQuickUnit()" ng-disabled="quickUnit.saving">Cancel</button><button type="button" class="cc-btn cc-btn-primary" ng-click="saveQuickUnit()" ng-disabled="quickUnit.saving">{{quickUnit.saving ? 'Adding...' : 'Add Unit'}}</button></div></div>
                    <label>Amount *</label>
                    <input type="number" step="0.01" min="0" ng-model="modal.data.Amount" ng-disabled="modal.data.Quantity!=null && modal.data.Rate!=null" />
                    <div class="cc-hint">When quantity and rate are both entered, amount = quantity x rate.</div>
                    <label>Remarks</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Remarks"></textarea>
                </div>

                <div ng-if="modal.type==='entry'">
                    <h4>{{modal.data.Id ? 'Edit' : 'Add'}} Cost Entry</h4>
                    <div class="cc-grid2">
                        <div><label>Date *</label><input type="date" ng-model="modal.data.EntryDateValue" /></div>
                        <div><label>Amount *</label><input type="number" step="0.01" ng-model="modal.data.Amount" /><div class="cc-hint">Use a negative amount for a reversal.</div></div>
                    </div>
                    <label>Cost Code *</label>
                    <cc-search-select ng-model="modal.data.CostCodeId" items="postingCodeOptions" value-key="id" label-key="Label" placeholder="-- Select --" add-new-label="{{access.IsAdmin ? '+ Add new cost code' : ''}}" on-add-new="startQuickCode()"></cc-search-select>
                    <div class="cc-quickadd" ng-if="quickCode.open" ng-include="'quickCode.html'"></div>
                    <div class="cc-grid2">
                        <div><label>Milestone</label>
                            <cc-search-select ng-model="modal.data.MilestoneId" items="structure.milestones" value-key="id" label-key="MilestoneName" empty-label="Project level" ng-change="milestoneChanged()"></cc-search-select></div>
                        <div><label>Task</label>
                            <cc-search-select ng-model="modal.data.TaskId" items="modalTasks" value-key="id" label-key="TaskName" empty-label="-- None --" is-disabled="!modal.data.MilestoneId"></cc-search-select></div>
                    </div>
                    <div class="cc-grid2">
                        <div><label>Source Document</label>
                            <cc-search-select ng-model="modal.data.SourceDocType" items="sourceDocOptions" value-key="id" label-key="Name" empty-label="-- None --"></cc-search-select></div>
                        <div><label>Reference No.</label><input type="text" maxlength="100" ng-model="modal.data.ReferenceNo" placeholder="Bill / voucher / PO number" /></div>
                    </div>
                    <label>Site / Location</label><input type="text" maxlength="100" ng-model="modal.data.SiteName" placeholder="Site where the cost was incurred" />
                    <label>Remarks</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Remarks"></textarea>
                    <div ng-if="modal.data.Id"><label>Reason for change *</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Reason" placeholder="Why is this saved entry being changed?"></textarea></div>
                </div>

                <div ng-if="modal.type==='etc'">
                    <h4>Estimate to Complete</h4>
                    <label>Cost Code *</label>
                    <cc-search-select ng-model="modal.data.CostCodeId" items="postingCodeOptions" value-key="id" label-key="Label" placeholder="-- Select --" is-disabled="modal.data.fixedKey"></cc-search-select>
                    <label>Milestone</label>
                    <cc-search-select ng-model="modal.data.MilestoneId" items="structure.milestones" value-key="id" label-key="MilestoneName" empty-label="Project level" is-disabled="modal.data.fixedKey"></cc-search-select>
                    <div class="cc-hint" ng-if="modal.data.fixedKey">Current budget {{modal.data.CurrentBudget | number:2}}, actual to date {{modal.data.Actual | number:2}}.</div>
                    <div class="cc-grid2">
                        <div><label>ETC Amount *</label><input type="number" step="0.01" min="0" ng-model="modal.data.EtcAmount" /></div>
                        <div><label>As of *</label><input type="date" ng-model="modal.data.AsOfValue" /></div>
                    </div>
                    <div class="cc-hint" ng-if="modal.data.EtcAmount!=null && modal.data.fixedKey">EAC will be {{(modal.data.Actual||0) + (modal.data.EtcAmount||0) | number:2}}, VAC {{(modal.data.CurrentBudget||0) - (modal.data.Actual||0) - (modal.data.EtcAmount||0) | number:2}}.</div>
                    <label>Remarks</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Remarks" placeholder="Basis of the estimate"></textarea>
                </div>

                <div ng-if="modal.type==='code'">
                    <h4>{{modal.data.Id ? 'Edit' : 'Add'}} Cost Code</h4>
                    <div class="cc-info" style="margin:0 0 6px" ng-if="modal.data.IsUsed">This code is already used. Only its description can be changed; create a new code for other changes.</div>
                    <div class="cc-grid2">
                        <div><label>Level *</label><cc-search-select ng-model="modal.data.CodeLevel" items="levelOptions" value-key="id" label-key="Name" ng-change="codeLevelChanged()" is-disabled="modal.data.IsUsed || modal.data.HasChildren"></cc-search-select></div>
                        <div><label>Parent{{modal.data.CodeLevel==='Section' ? ' (Division) *' : (modal.data.CodeLevel==='Element' ? ' (Section)' : '')}}</label><cc-search-select ng-model="modal.data.ParentId" items="parentOptions" value-key="id" label-key="Label" empty-label="-- None --" is-disabled="modal.data.IsUsed || modal.data.CodeLevel==='Division'"></cc-search-select></div>
                    </div>
                    <div class="cc-grid2">
                        <div><label>Code *</label><input type="text" maxlength="30" ng-model="modal.data.Code" ng-disabled="modal.data.IsUsed" /></div>
                        <div><label>Cost Type *</label><cc-search-select ng-model="modal.data.CostType" items="costTypeOptions" value-key="id" label-key="Name" placeholder="-- Select --" is-disabled="modal.data.IsUsed"></cc-search-select></div>
                    </div>
                    <label>Name *</label><input type="text" maxlength="200" ng-model="modal.data.Name" ng-disabled="modal.data.IsUsed" />
                    <label>Description</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Description"></textarea>
                </div>

                <div class="cc-modal-footer">
                    <button type="button" class="cc-btn" ng-click="closeModal()" ng-disabled="busy">Cancel</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-click="saveModal()" ng-disabled="busy">{{busy ? 'Saving...' : (modal.type==='reason' ? modal.data.button : 'Save')}}</button>
                </div>
            </div>
        </div>

        <!-- Quick add of an Element cost code inside the budget-line and cost-entry forms (administrators). -->
        <script type="text/ng-template" id="quickCode.html">
            <div class="cc-grid3">
                <div><label style="margin-top:0">New Code *</label><input type="text" maxlength="30" ng-model="quickCode.Code" /></div>
                <div><label style="margin-top:0">Name *</label><input type="text" maxlength="200" ng-model="quickCode.Name" /></div>
                <div><label style="margin-top:0">Cost Type *</label><cc-search-select ng-model="quickCode.CostType" items="costTypeOptions" value-key="id" label-key="Name" placeholder="-- Select --"></cc-search-select></div>
            </div>
            <label>Section (optional)</label>
            <cc-search-select ng-model="quickCode.ParentId" items="sectionOptions" value-key="id" label-key="Label" empty-label="-- None --"></cc-search-select>
            <div class="cc-hint">Creates an Element-level code.</div>
            <div class="cc-alert" style="margin:6px 0 0" ng-if="quickCode.error">{{quickCode.error}}</div>
            <div class="cc-modal-footer" style="margin-top:8px"><button type="button" class="cc-btn" ng-click="cancelQuickCode()" ng-disabled="quickCode.saving">Cancel</button><button type="button" class="cc-btn cc-btn-primary" ng-click="saveQuickCode()" ng-disabled="quickCode.saving">{{quickCode.saving ? 'Adding...' : 'Add Cost Code'}}</button></div>
        </script>
    </div>
</asp:Content>
