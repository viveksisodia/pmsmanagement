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
        .cc-link:disabled{color:#8a9099;cursor:not-allowed;text-decoration:none}
        .cc-subhead{margin:10px 12px 4px;font-size:14px;font-weight:600}
        .cc-measures{display:flex;flex-wrap:wrap;gap:8px;padding:8px 12px;border-bottom:1px solid #eceef1}
        .cc-measures>div{min-width:150px;padding:6px 10px;border:1px solid #e3e9ee;border-radius:6px;background:#fafbfd}
        .cc-measures span{display:block;font-size:11px;color:#5e6672}
        .cc-measures b{font-size:14px}
        .cc-badge-approved{background:#dff5e5;color:#23743c}.cc-badge-rejected{background:#fbe9e9;color:#a61b1b}.cc-badge-muted{background:#f1f3f5;color:#5e6672}
        .cc-mini{min-width:0!important}.cc-mini td,.cc-mini th{padding:3px 5px!important}
        .cc-modal{width:760px}
        /* Supporting documents (Phase 4) */
        .cc-docs{margin-top:4px}
        .cc-drop{position:relative;display:flex!important;flex-direction:column;align-items:center;gap:2px;margin:4px 0 0!important;padding:12px;border:1px dashed #b98ab2;border-radius:6px;background:#fbf7fb;color:#6b1e63!important;font-size:13px!important;cursor:pointer;text-align:center}
        .cc-drop:hover,.cc-drop:focus-within{background:#f4ebf3;border-color:#6b1e63}
        .cc-drop input[type=file]{position:absolute;inset:0;width:100%;height:100%;opacity:0;cursor:pointer}
        .cc-drop-off{opacity:.55;cursor:not-allowed}.cc-drop-off input[type=file]{cursor:not-allowed}
        .cc-doclist{display:grid;grid-template-columns:repeat(auto-fill,minmax(200px,1fr));gap:8px;margin-top:8px}
        .cc-doc{position:relative;display:flex;align-items:center;gap:8px;min-width:0;padding:7px 8px;border:1px solid #e3e9ee;border-radius:6px;background:#fff}
        .cc-doc-new{border-style:dashed;background:#fafbfd}
        .cc-doc-icon{flex:none;width:34px;height:34px;border-radius:5px;display:flex;align-items:center;justify-content:center;font-size:10px;font-weight:700;color:#fff;background:#6b737d}
        .cc-doc-pdf{background:#c0392b}.cc-doc-img{background:#2e7d4f}.cc-doc-doc{background:#285ca8}.cc-doc-xls{background:#1d6f42}
        .cc-doc-name{flex:1;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;font-size:12px}
        .cc-doc-size{position:absolute;left:50px;bottom:2px;font-size:10px;color:#8a9099;white-space:nowrap;overflow:hidden;max-width:calc(100% - 58px)}
        .cc-doc-name{align-self:flex-start;margin-top:2px}
        .cc-doc-tools{position:absolute;inset:0;display:flex;align-items:center;justify-content:center;gap:10px;border-radius:6px;background:rgba(40,20,40,.62);opacity:0;transition:opacity .12s}
        .cc-doc:hover .cc-doc-tools,.cc-doc:focus-within .cc-doc-tools{opacity:1}
        @media (hover:none){.cc-doc-tools{position:static;background:none;opacity:1;gap:4px}}
        .cc-doc-btn{width:32px;height:32px;border:0;border-radius:50%;background:#fff;color:#333;font-size:15px;line-height:1;cursor:pointer;box-shadow:0 1px 3px rgba(0,0,0,.25)}
        .cc-doc-btn:hover,.cc-doc-btn:focus{background:#f4ebf3;outline:2px solid #6b1e63}
        .cc-doc-del:hover,.cc-doc-del:focus{background:#fbe9e9;color:#a61b1b;outline-color:#a61b1b}
        .cc-ico{width:17px;height:17px;fill:none;stroke:currentColor;stroke-width:1.9;stroke-linecap:round;stroke-linejoin:round;vertical-align:middle}
        .cc-clip{margin-left:6px;border:1px solid #d6c3d3;border-radius:10px;background:#fbf7fb;color:#6b1e63;font-size:11px;padding:0 6px;cursor:pointer}
        .cc-clip:hover{background:#f4ebf3}
        .cc-reason-label{display:flex;align-items:center;gap:8px;margin:9px 0 3px;font-size:12px;color:#4a525c}
        .cc-hist-btn{display:inline-flex;align-items:center;gap:4px;padding:2px 8px;border:1px solid #d6c3d3;border-radius:12px;background:#fbf7fb;color:#6b1e63;font-size:11px;cursor:pointer}
        .cc-hist-btn:hover,.cc-hist-btn:focus{background:#f4ebf3;outline:2px solid #6b1e63;outline-offset:1px}
        .cc-hist-btn .cc-ico{width:14px;height:14px}
        .cc-hist-on{background:#6b1e63;color:#fff;border-color:#6b1e63}.cc-hist-on:hover{background:#5a1853}
        .cc-hist{margin-bottom:6px}
        .cc-hist-list{max-height:150px;overflow:auto;border:1px solid #e3e9ee;border-radius:6px;background:#fafbfd}
        .cc-hist-row{padding:6px 9px;border-bottom:1px solid #eceef1;font-size:12px}
        .cc-hist-row:last-child{border-bottom:0}
        .cc-hist-head{color:#4a525c;word-break:break-word}
        .cc-hist-reason{margin-top:2px;color:#6b1e63;font-weight:600;word-break:break-word}
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
                    <span ng-if="currentVersion().SubmittedOn">Submitted by {{currentVersion().SubmittedByName || 'employee ' + currentVersion().SubmittedBy}} on {{currentVersion().SubmittedOn | ccDate}}</span>
                    <span ng-if="currentVersion().LockedOn">Approved by {{currentVersion().LockedByName}} on {{currentVersion().LockedOn | ccDate}}</span>
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
                        <thead><tr><th>Date</th><th>Cost Code</th><th>Milestone</th><th>Task</th><th>Site</th><th>Source Doc</th><th>Reference</th><th>Change</th><th class="cc-num">Amount</th><th>Remarks</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredEntries().length"><td colspan="11" class="cc-empty">No cost entries.</td></tr>
                            <tr ng-repeat="entry in filteredEntries() track by entry.id">
                                <td>{{entry.EntryDate | ccDate}}</td>
                                <td ng-if="!entry.splits.length">{{entry.CostCode}} - {{entry.CostCodeName}}</td>
                                <td ng-if="entry.splits.length" colspan="3" class="cc-diff"><b>Split ({{entry.splits.length}})</b><div ng-repeat="s in entry.splits track by s.id">{{s.CostCodeLabel}}{{s.MilestoneName ? ' / ' + s.MilestoneName : ''}}: {{s.Amount | number:2}} ({{s.SplitPercent | number:2}} %)</div></td>
                                <td ng-if="!entry.splits.length">{{entry.MilestoneName || 'Project level'}}</td><td ng-if="!entry.splits.length">{{entry.TaskName}}</td>
                                <td>{{entry.SiteName}}</td>
                                <td>{{entry.SourceDocType}}<button type="button" class="cc-clip" ng-if="entry.documents.length" ng-click="openEntryDocs(entry)" title="View attached documents" aria-label="{{entry.documents.length}} attached document(s)"><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true" style="width:13px;height:13px"><path d="M20 11.5l-8.2 8.2a5 5 0 0 1-7.1-7.1l8.5-8.5a3.4 3.4 0 0 1 4.8 4.8l-8.4 8.4a1.7 1.7 0 0 1-2.4-2.4l7.7-7.7"/></svg> {{entry.documents.length}}</button></td>
                                <td>{{entry.ReferenceNo}}</td>
                                <td>{{entry.ChangeNo ? '#' + entry.ChangeNo : ''}}<span class="cc-warn" ng-if="entry.IsAtRisk==1"> at risk</span></td>
                                <td class="cc-num" ng-class="{'cc-neg':entry.Amount<0}">{{entry.Amount | number:2}}</td>
                                <td>{{entry.Remarks}}</td>
                                <td class="cc-actions">
                                    <span class="cc-lock" ng-if="entry.IsPeriodClosed==1">Month closed</span>
                                    <button type="button" class="cc-btn" ng-if="access.CanEnter && entry.SourceType==='Manual' && entry.IsPeriodClosed!=1" ng-click="openCostEntry(entry)">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanEnter && entry.SourceType==='Manual' && entry.IsPeriodClosed!=1" ng-click="openDeleteEntry(entry)">Delete</button>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="filteredEntries().length"><tr><td colspan="8">Total</td><td class="cc-num">{{sum(filteredEntries(),'Amount') | number:2}}</td><td colspan="2"></td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= FORECAST (ETC) ================= -->
            <div ng-if="tab==='forecast' && selection.projectId">
                <div class="cc-toolbar">
                    <span class="cc-meta">Estimate to complete (ETC) = cost still expected for the remaining work. <b>Until purchase orders and subcontracts are recorded in Cost Control, include ordered-but-not-yet-received value in ETC.</b> EAC = Actual + ETC. VAC = Revised budget (RB) - EAC.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter" ng-click="openEtc()" ng-disabled="busy">Add ETC</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('etc')" ng-disabled="!forecast.rows.length">Export Excel</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Cost Code</th><th>Milestone</th><th class="cc-num">Revised Budget (RB)</th><th class="cc-num">Actual (manual entries)</th><th class="cc-num">ETC</th><th class="cc-num">EAC</th><th class="cc-num">VAC</th><th>As of</th><th>Remarks</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!forecast.rows.length"><td colspan="10" class="cc-empty">No budget, cost or estimate yet.</td></tr>
                            <tr ng-repeat="row in forecast.rows track by $index">
                                <td>{{row.CostCode}} - {{row.CostCodeName}}</td><td>{{row.MilestoneName || 'Project level'}}</td>
                                <td class="cc-num">{{row.CurrentBudget | number:2}}</td><td class="cc-num">{{row.Actual | number:2}}</td>
                                <td class="cc-num">{{row.EtcAmount===null ? '-' : (row.EtcAmount | number:2)}}</td>
                                <td class="cc-num">{{eacOf(row) | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.CurrentBudget - eacOf(row) < 0}">{{row.CurrentBudget - eacOf(row) | number:2}}</td>
                                <td>{{row.AsOfDate | ccDate}}</td><td>{{row.Remarks}}</td>
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
                    <label style="margin:0;font-size:12px">As at</label>
                    <cc-search-select class="cc-ss-version" ng-model="report.snapshotId" items="snapshotOptions" value-key="id" label-key="Label" ng-change="loadReport()"></cc-search-select>
                    <label style="margin:0;font-size:12px">Group by</label>
                    <cc-search-select class="cc-ss-small" ng-model="report.groupBy" items="groupByOptions" value-key="id" label-key="Name" ng-change="loadReport()"></cc-search-select>
                    <label class="cc-meta" style="display:flex;align-items:center;gap:6px;margin:0"><input type="checkbox" class="cc-check" ng-model="report.showBreakdown" /> Show budget breakdown</label>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadReport()" ng-disabled="busy">Refresh</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('report')" ng-disabled="!report.rows.length">Export Excel</button>
                </div>
                <div class="cc-toolbar cc-meta">
                    <span ng-if="report.header.SnapshotId" class="cc-badge cc-badge-locked">Snapshot {{monthLabel(report.header.PeriodStart)}} v{{report.header.SnapshotVersion}} - frozen at month close</span>
                    <span ng-if="!report.header.SnapshotId">Live figures.</span>
                    <span ng-if="report.header.CurrentVersionNo">Baseline = approved version {{report.header.CurrentVersionNo}}.</span>
                    <span class="cc-warn" ng-if="!report.header.SnapshotId && report.header.DraftCount>0">A draft or submitted revision is not included until approved.</span>
                    <span class="cc-warn" ng-if="!report.header.SnapshotId && report.header.OpenMovements>0">{{report.header.OpenMovements}} budget movement(s) waiting - not included until approved.</span>
                </div>
                <div class="cc-measures" ng-if="report.header">
                    <div><span>Revised budget (ΣRB)</span><b>{{sum(report.rows,'CurrentBudget') | number:2}}</b></div>
                    <div><span>Reserves (RSV)</span><b>{{report.header.RSV | number:2}}</b></div>
                    <div><span>Total available (ΣRB + RSV)</span><b>{{sum(report.rows,'CurrentBudget') + (report.header.RSV||0) | number:2}}</b></div>
                    <div><span>EAC</span><b>{{sum(report.rows,'Eac') | number:2}}</b></div>
                    <div><span>Pending changes (PCE)</span><b>{{report.header.PCE | number:2}}</b></div>
                    <div><span>Probable pending (PPC)</span><b>{{report.header.PPC | number:2}}</b></div>
                    <div><span>EAC incl. probable (EACp)</span><b>{{sum(report.rows,'Eac') + (report.header.PPC||0) | number:2}}</b></div>
                    <div><span>VACp (ΣRB + RSV − EACp)</span><b ng-class="{'cc-neg':vacP()<0}">{{vacP() | number:2}}</b></div>
                    <div><span>At-risk exposure</span><b ng-class="{'cc-warn':report.header.AtRiskExposure>0}">{{report.header.AtRiskExposure | number:2}}</b></div>
                    <div><span>Unrecovered at-risk</span><b ng-class="{'cc-neg':report.header.Unrecovered>0}">{{report.header.Unrecovered | number:2}}</b></div>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>{{groupLabel()}}</th><th ng-if="report.groupBy==='COSTCODE'">Type</th><th class="cc-num">Original (OB)</th>
                            <th class="cc-num" ng-if="report.showBreakdown">Revisions</th><th class="cc-num" ng-if="report.showBreakdown">Corrections (ADJ)</th>
                            <th class="cc-num" ng-if="report.showBreakdown">Approved changes (ACH)</th><th class="cc-num" ng-if="report.showBreakdown">Net transfers (NTR)</th>
                            <th class="cc-num">Revised Budget (RB)</th><th class="cc-num">Actual (manual entries)</th><th class="cc-num">Remaining</th><th class="cc-num">ETC</th><th class="cc-num">EAC</th><th class="cc-num">VAC</th><th class="cc-num">Consumed %</th></tr></thead>
                        <tbody>
                            <tr ng-if="!report.rows.length"><td colspan="14" class="cc-empty">No approved budget, cost entries or estimates yet.</td></tr>
                            <tr ng-repeat="row in report.rows track by row.GroupId">
                                <td>{{row.GroupName}}</td><td ng-if="report.groupBy==='COSTCODE'">{{row.CostType}}</td>
                                <td class="cc-num">{{row.OriginalBudget | number:2}}</td>
                                <td class="cc-num" ng-if="report.showBreakdown">{{row.RevisionAdj | number:2}}</td><td class="cc-num" ng-if="report.showBreakdown">{{row.Corrections | number:2}}</td>
                                <td class="cc-num" ng-if="report.showBreakdown">{{row.ApprovedChanges | number:2}}</td><td class="cc-num" ng-if="report.showBreakdown">{{row.NetTransfers | number:2}}</td>
                                <td class="cc-num">{{row.CurrentBudget | number:2}}</td>
                                <td class="cc-num">{{row.Actual | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.Variance<0}">{{row.Variance | number:2}}</td>
                                <td class="cc-num">{{row.Etc | number:2}}</td><td class="cc-num">{{row.Eac | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.Vac<0}">{{row.Vac | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.ConsumedPercent>100}">{{row.ConsumedPercent===null ? '-' : (row.ConsumedPercent | number:2) + ' %'}}</td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="report.rows.length"><tr>
                            <td ng-attr-colspan="{{report.groupBy==='COSTCODE' ? 2 : 1}}">Total</td>
                            <td class="cc-num">{{sum(report.rows,'OriginalBudget') | number:2}}</td>
                            <td class="cc-num" ng-if="report.showBreakdown">{{sum(report.rows,'RevisionAdj') | number:2}}</td><td class="cc-num" ng-if="report.showBreakdown">{{sum(report.rows,'Corrections') | number:2}}</td>
                            <td class="cc-num" ng-if="report.showBreakdown">{{sum(report.rows,'ApprovedChanges') | number:2}}</td><td class="cc-num" ng-if="report.showBreakdown">{{sum(report.rows,'NetTransfers') | number:2}}</td>
                            <td class="cc-num">{{sum(report.rows,'CurrentBudget') | number:2}}</td>
                            <td class="cc-num">{{sum(report.rows,'Actual') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(report.rows,'Variance')<0}">{{sum(report.rows,'Variance') | number:2}}</td>
                            <td class="cc-num">{{sum(report.rows,'Etc') | number:2}}</td><td class="cc-num">{{sum(report.rows,'Eac') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(report.rows,'Vac')<0}">{{sum(report.rows,'Vac') | number:2}}</td>
                            <td class="cc-num">{{totalConsumed(report.rows)}}</td>
                        </tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= MOVEMENTS & RESERVES ================= -->
            <div ng-if="tab==='movements' && selection.projectId">
                <div class="cc-toolbar">
                    <span class="cc-meta">Changes to an approved budget: transfers between lines (must net to zero), reserve additions, releases and returns (ΣRB + RSV stays the same), and corrections. Each movement needs a reason and another person's approval.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-if="access.CanEnter" ng-click="openReserve()" ng-disabled="busy">Add Reserve</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter" ng-click="openMovement()" ng-disabled="busy">New Movement</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('reserves')" ng-disabled="!movements.reserves.length">Export Reserves</button>
                </div>
                <h5 class="cc-subhead">Reserves <span class="cc-meta">(held at project level; never receive cost)</span></h5>
                <div class="cc-tablewrap" style="max-height:none">
                    <table class="cc-table" style="min-width:700px">
                        <thead><tr><th>Type</th><th>Name</th><th>Visibility</th><th class="cc-num">Additions</th><th class="cc-num">Releases</th><th class="cc-num">Returns</th><th class="cc-num">Remaining</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!movements.reserves.length"><td colspan="8" class="cc-empty">No reserves. Add one, then record its amount with a "Reserve addition" movement.</td></tr>
                            <tr ng-repeat="r in movements.reserves track by r.id">
                                <td>{{reserveTypeLabel(r.ReserveType)}}</td><td>{{r.Name}}</td><td>{{r.IsInternal==1 ? 'Internal only' : 'Per contract'}}</td>
                                <td class="cc-num">{{r.Additions | number:2}}</td><td class="cc-num">{{r.Releases | number:2}}</td><td class="cc-num">{{r.Returns | number:2}}</td>
                                <td class="cc-num"><b>{{r.Remaining | number:2}}</b></td>
                                <td class="cc-actions"><button type="button" class="cc-btn" ng-if="access.CanEnter" ng-click="openReserve(r)">Edit</button></td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="movements.reserves.length"><tr><td colspan="6">Total reserves (RSV)</td><td class="cc-num">{{sum(movements.reserves,'Remaining') | number:2}}</td><td></td></tr></tfoot>
                    </table>
                </div>
                <h5 class="cc-subhead">Budget movements</h5>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>No.</th><th>Type</th><th>Status</th><th>Reason</th><th>Lines</th><th class="cc-num">Budget net</th><th class="cc-num">Reserve net</th><th>Submitted</th><th>Approved</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!movements.rows.length"><td colspan="10" class="cc-empty">No budget movements.</td></tr>
                            <tr ng-repeat="m in movements.rows track by m.id">
                                <td>#{{m.MovementNo}}</td><td>{{movementTypeLabel(m.MovementType)}}<span class="cc-meta" ng-if="m.ChangeNo"> (change #{{m.ChangeNo}})</span></td>
                                <td><span class="cc-badge" ng-class="statusBadge(m.MovementStatus)">{{m.MovementStatus}}</span></td>
                                <td>{{m.Reason}}<div class="cc-warn" ng-if="m.MovementStatus==='Draft' && m.RejectReason">Returned: {{m.RejectReason}}</div></td>
                                <td class="cc-diff"><div ng-repeat="l in m.lines track by l.id">{{l.LineKind==='Reserve' ? 'Reserve: ' + l.ReserveName : l.CostCodeLabel + (l.MilestoneName ? ' / ' + l.MilestoneName : '')}} <b ng-class="{'cc-neg':l.Amount<0}">{{l.Amount | number:2}}</b></div></td>
                                <td class="cc-num">{{m.BudgetNet | number:2}}</td><td class="cc-num">{{m.ReserveNet | number:2}}</td>
                                <td>{{(m.SubmittedOn ? m.SubmittedByName + ', ' + m.SubmittedOn : '') | ccDate}}</td><td>{{(m.ApprovedOn ? m.ApprovedByName + ', ' + m.ApprovedOn : '') | ccDate}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn" ng-if="access.CanEnter && m.MovementStatus==='Draft'" ng-click="openMovement(m)">Edit</button>
                                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter && m.MovementStatus==='Draft'" ng-click="movementAction(m,'Submit')">Submit</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanEnter && m.MovementStatus==='Draft'" ng-click="movementAction(m,'Discard')">Discard</button>
                                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanApprove && m.MovementStatus==='Submitted'" ng-disabled="m.SubmittedBy===access.EmployeeId" title="{{m.SubmittedBy===access.EmployeeId ? 'You submitted it; another approver must approve.' : 'Approve'}}" ng-click="movementAction(m,'Approve')">Approve</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanApprove && m.MovementStatus==='Submitted'" ng-disabled="m.SubmittedBy===access.EmployeeId" ng-click="movementAction(m,'Reject')">Return</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- ================= CHANGE REGISTER ================= -->
            <div ng-if="tab==='changes' && selection.projectId">
                <div class="cc-toolbar">
                    <span class="cc-meta">Four independent statuses: internal cost authorisation, client approval, subcontract approval and budget posting. None sets another. Pending changes appear as PCE / PPC only; budget changes only when the posting movement is approved.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter" ng-click="openChange()" ng-disabled="busy">New Change</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('changes')" ng-disabled="!changes.rows.length">Export Excel</button>
                </div>
                <div class="cc-measures">
                    <div><span>Pending changes (PCE)</span><b>{{changeTotals().pce | number:2}}</b></div>
                    <div><span>Probable pending (PPC)</span><b>{{changeTotals().ppc | number:2}}</b></div>
                    <div><span>At-risk cost booked</span><b ng-class="{'cc-warn':changeTotals().atRisk>0}">{{changeTotals().atRisk | number:2}}</b></div>
                    <div><span>Approved client value</span><b>{{changeTotals().clientApproved | number:2}}</b></div>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table" style="min-width:1500px">
                        <thead><tr><th>No.</th><th>Title</th><th>Class</th><th class="cc-num">Est. cost</th><th class="cc-num">In ETC</th><th>Probable</th><th>Internal</th><th>Client</th><th>Subcontract</th><th>Budget posting</th><th>At risk</th><th class="cc-num">Cost booked</th><th class="cc-num">Approved margin</th><th>Record</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!changes.rows.length"><td colspan="15" class="cc-empty">No changes.</td></tr>
                            <tr ng-repeat="c in changes.rows track by c.id">
                                <td>#{{c.ChangeNo}}</td><td>{{c.Title}}<div class="cc-diff" ng-repeat="a in c.allocations track by a.id">{{a.CostCodeLabel}}{{a.MilestoneName ? ' / ' + a.MilestoneName : ''}}: {{a.Amount | number:2}}</div></td>
                                <td>{{changeClassLabel(c.ChangeClass)}}</td>
                                <td class="cc-num">{{c.EstimatedCost | number:2}}</td><td class="cc-num">{{c.AmountInEtc | number:2}}</td><td>{{c.IsProbable==1 ? 'Yes' : 'No'}}</td>
                                <td><span class="cc-badge" ng-class="statusBadge(c.InternalStatus)">{{c.InternalStatus}}</span><div class="cc-meta" ng-if="c.InternalDecisionOn">{{c.InternalDecisionByName}}</div></td>
                                <td><span class="cc-badge" ng-class="statusBadge(c.ClientStatus)">{{c.ClientStatus}}</span><div class="cc-meta" ng-if="c.ClientValueApproved!==null">{{c.ClientValueApproved | number:2}} on {{c.ClientDecisionOn | ccDate}}</div><div class="cc-meta" ng-if="c.ClientValueApproved===null && c.ClientValueProposed!==null">proposed {{c.ClientValueProposed | number:2}}</div></td>
                                <td><span class="cc-badge" ng-class="statusBadge(c.SubcontractStatus)">{{c.SubcontractStatus}}</span></td>
                                <td><span class="cc-badge" ng-class="statusBadge(c.BudgetPostingStatus)">{{c.BudgetPostingStatus}}</span><div class="cc-meta" ng-if="c.MovementStatus && c.BudgetPostingStatus!=='Posted'">movement {{c.MovementStatus}}</div></td>
                                <td>{{c.IsAtRisk==1 ? (c.AtRiskAuthorisedOn ? 'Authorised' : 'Awaiting authorisation') : 'No'}}</td>
                                <td class="cc-num">{{c.CostIncurred | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':c.ApprovedMargin<0}">{{c.ClientValueApproved!==null ? (c.ApprovedMargin | number:2) : '-'}}</td>
                                <td>{{c.RecordStatus}}</td>
                                <td class="cc-actions">
                                    <button type="button" class="cc-btn" ng-if="access.CanEnter && c.RecordStatus==='Open' && c.BudgetPostingStatus!=='Posted'" ng-click="openChange(c)">Edit</button>
                                    <button type="button" class="cc-btn" ng-if="access.CanEnter && c.RecordStatus==='Open'" ng-click="openChangeStatus(c)">Client / Subcontract</button>
                                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanApprove && c.RecordStatus==='Open' && c.InternalStatus==='Pending'" ng-disabled="c.cby===access.EmployeeId" title="{{c.cby===access.EmployeeId ? 'You raised it; another approver must authorise.' : ''}}" ng-click="changeAction(c,'Internal','Approved')">Authorise cost</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanApprove && c.RecordStatus==='Open' && c.InternalStatus==='Pending'" ng-disabled="c.cby===access.EmployeeId" ng-click="changeAction(c,'Internal','Rejected')">Reject</button>
                                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanApprove && c.IsAtRisk==1 && !c.AtRiskAuthorisedOn && c.RecordStatus==='Open'" ng-disabled="c.cby===access.EmployeeId" ng-click="changeAction(c,'AtRisk','Authorise')">Authorise at-risk</button>
                                    <button type="button" class="cc-btn cc-btn-primary" ng-if="access.CanEnter && c.RecordStatus==='Open' && c.InternalStatus==='Approved' && c.BudgetPostingStatus!=='Posted' && !c.MovementStatus" ng-click="postChange(c)">Post to budget</button>
                                    <button type="button" class="cc-btn cc-btn-danger" ng-if="access.CanEnter && c.RecordStatus==='Open' && c.BudgetPostingStatus!=='Posted'" ng-click="changeAction(c,'Record','Withdrawn')">Withdraw</button>
                                    <button type="button" class="cc-btn" ng-if="access.CanEnter && c.RecordStatus==='Open' && c.BudgetPostingStatus==='Posted'" ng-click="changeAction(c,'Record','Closed')">Close</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- ================= PENDING APPROVALS ================= -->
            <div ng-if="tab==='approvals'">
                <div class="cc-toolbar">
                    <span class="cc-meta">Everything waiting for a decision in the projects you can see: budgets, budget movements, change cost authorisations and at-risk authorisations. You cannot approve your own submissions.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadApprovals()" ng-disabled="busy">Refresh</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Type</th><th>Project</th><th>Item</th><th class="cc-num">Amount</th><th>Requested by</th><th>Requested on</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-if="!approvals.rows.length"><td colspan="7" class="cc-empty">Nothing is waiting for approval.</td></tr>
                            <tr ng-repeat="a in approvals.rows track by $index">
                                <td>{{a.ItemType==='AtRisk' ? 'At-risk work' : a.ItemType}}</td><td>{{a.ProjectName}}</td><td>{{a.Title}}</td>
                                <td class="cc-num">{{a.Amount | number:2}}</td><td>{{a.RequestedByName}}<span class="cc-warn" ng-if="a.IsOwn==1"> (you)</span></td><td>{{a.RequestedOn | ccDate}}</td>
                                <td class="cc-actions"><button type="button" class="cc-btn cc-btn-primary" ng-click="openApproval(a)">Open</button></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- ================= PROJECT SUMMARY ================= -->
            <div ng-if="tab==='summary'">
                <div class="cc-toolbar">
                    <span class="cc-meta">All projects you can see that have a budget, cost or estimate. Click a project to open its Budget vs Actual.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadSummary()" ng-disabled="busy">Refresh</button>
                    <button type="button" class="cc-btn" ng-click="exportFile('summary')" ng-disabled="!summary.rows.length">Export Excel</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table" style="min-width:1300px">
                        <thead><tr><th>Project</th><th>Budget</th><th class="cc-num">Revised Budget (RB)</th><th class="cc-num">Reserves (RSV)</th><th class="cc-num">Actual (manual entries)</th><th class="cc-num">ETC</th><th class="cc-num">EAC</th><th class="cc-num">VAC</th><th class="cc-num">PCE</th><th class="cc-num">EACp</th><th class="cc-num">VACp</th><th class="cc-num">At-risk</th><th class="cc-num">Consumed %</th></tr></thead>
                        <tbody>
                            <tr ng-if="!summary.rows.length"><td colspan="13" class="cc-empty">No project has Cost Control data yet.</td></tr>
                            <tr ng-repeat="row in summary.rows track by row.ProjectId">
                                <td><button type="button" class="cc-link" ng-click="openProjectReport(row.ProjectId)">{{row.ProjectName}}</button></td>
                                <td>{{row.CurrentVersionNo ? 'V' + row.CurrentVersionNo + ' approved' : 'No approved budget'}}<span class="cc-warn" ng-if="row.OpenVersionStatus"> ({{row.OpenVersionStatus}} revision open)</span></td>
                                <td class="cc-num">{{row.CurrentBudget | number:2}}</td><td class="cc-num">{{row.RSV | number:2}}</td><td class="cc-num">{{row.Actual | number:2}}</td>
                                <td class="cc-num">{{row.Etc | number:2}}</td><td class="cc-num">{{row.Eac | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.Vac<0}">{{row.Vac | number:2}}</td>
                                <td class="cc-num">{{row.PCE | number:2}}</td><td class="cc-num">{{row.EacP | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.VacP<0}">{{row.VacP | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-warn':row.AtRiskExposure>0}">{{row.AtRiskExposure | number:2}}</td>
                                <td class="cc-num" ng-class="{'cc-neg':row.ConsumedPercent>100}">{{row.ConsumedPercent===null ? '-' : (row.ConsumedPercent | number:2) + ' %'}}</td>
                            </tr>
                        </tbody>
                        <tfoot ng-if="summary.rows.length"><tr><td colspan="2">Total</td>
                            <td class="cc-num">{{sum(summary.rows,'CurrentBudget') | number:2}}</td><td class="cc-num">{{sum(summary.rows,'RSV') | number:2}}</td><td class="cc-num">{{sum(summary.rows,'Actual') | number:2}}</td>
                            <td class="cc-num">{{sum(summary.rows,'Etc') | number:2}}</td><td class="cc-num">{{sum(summary.rows,'Eac') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(summary.rows,'Vac')<0}">{{sum(summary.rows,'Vac') | number:2}}</td>
                            <td class="cc-num">{{sum(summary.rows,'PCE') | number:2}}</td><td class="cc-num">{{sum(summary.rows,'EacP') | number:2}}</td>
                            <td class="cc-num" ng-class="{'cc-neg':sum(summary.rows,'VacP')<0}">{{sum(summary.rows,'VacP') | number:2}}</td>
                            <td class="cc-num">{{sum(summary.rows,'AtRiskExposure') | number:2}}</td>
                            <td class="cc-num">{{totalConsumed(summary.rows)}}</td></tr></tfoot>
                    </table>
                </div>
            </div>

            <!-- ================= PERIODS ================= -->
            <div ng-if="tab==='periods' && selection.projectId">
                <div class="cc-toolbar">
                    <span class="cc-meta">Closing a month blocks cost entries dated in it and freezes a snapshot of the figures (budget, actual to month end, ETC, reserves, pending changes). Reopening needs a reason and keeps the snapshot; closing again saves version n+1.</span>
                    <span class="cc-spacer"></span>
                    <button type="button" class="cc-btn" ng-click="loadPeriods()" ng-disabled="busy">Refresh</button>
                </div>
                <div class="cc-tablewrap">
                    <table class="cc-table">
                        <thead><tr><th>Month</th><th>Status</th><th class="cc-num">Entries</th><th class="cc-num">Amount</th><th>Closed</th><th>Last reopened</th><th>Snapshots</th><th class="cc-actions"></th></tr></thead>
                        <tbody>
                            <tr ng-repeat="p in periods.rows track by p.PeriodStart">
                                <td>{{monthLabel(p.PeriodStart)}}</td>
                                <td><span class="cc-badge" ng-class="p.IsClosed==1 ? 'cc-badge-locked' : 'cc-badge-draft'">{{p.IsClosed==1 ? 'Closed' : 'Open'}}</span></td>
                                <td class="cc-num">{{p.EntryCount}}</td><td class="cc-num">{{p.EntryTotal | number:2}}</td>
                                <td>{{(p.ClosedOn ? p.ClosedByName + ', ' + p.ClosedOn : '') | ccDate}}</td>
                                <td>{{(p.ReopenedOn ? p.ReopenedByName + ', ' + p.ReopenedOn + ' - ' + p.ReopenReason : '') | ccDate}}</td>
                                <td><button type="button" class="cc-link" ng-if="p.SnapshotCount>0" ng-click="viewSnapshot(p.LatestSnapshotId)">{{p.SnapshotCount}} version(s) - view latest</button></td>
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
                                <td>{{h.ActionTime | ccDate}}</td><td>{{h.UserName}}</td><td>{{entityLabel(h.EntityType)}}{{h.EntityId ? ' #' + h.EntityId : ''}}</td>
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
                        <thead><tr><th>Employee</th><th>View</th><th>Enter</th><th>Approve</th><th>Projects</th><th></th></tr></thead>
                        <tbody>
                            <tr ng-if="!filteredAccess().length"><td colspan="6" class="cc-empty">No employees.</td></tr>
                            <tr ng-repeat="a in filteredAccess() track by a.EmployeeId">
                                <td>{{a.EmployeeName}}</td>
                                <td><input type="checkbox" class="cc-check" ng-model="a.view" ng-change="saveAccess(a,'view')" ng-disabled="a.saving || a.enter || a.approve" aria-label="View" /></td>
                                <td><input type="checkbox" class="cc-check" ng-model="a.enter" ng-change="saveAccess(a,'enter')" ng-disabled="a.saving" aria-label="Enter" /></td>
                                <td><input type="checkbox" class="cc-check" ng-model="a.approve" ng-change="saveAccess(a,'approve')" ng-disabled="a.saving" aria-label="Approve" /></td>
                                <td><button type="button" class="cc-link" ng-click="openAccessProjects(a)" ng-disabled="!a.view">Choose projects</button></td>
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
                    <label class="cc-meta" style="display:flex;align-items:center;gap:6px"><input type="checkbox" class="cc-check" ng-model="modal.data.IsSplit" ng-change="splitToggled()" /> Split this cost across several cost codes / milestones</label>
                    <div ng-if="!modal.data.IsSplit">
                        <label>Cost Code *</label>
                        <cc-search-select ng-model="modal.data.CostCodeId" items="postingCodeOptions" value-key="id" label-key="Label" placeholder="-- Select --" add-new-label="{{access.IsAdmin ? '+ Add new cost code' : ''}}" on-add-new="startQuickCode()"></cc-search-select>
                        <div class="cc-quickadd" ng-if="quickCode.open" ng-include="'quickCode.html'"></div>
                        <div class="cc-grid2">
                            <div><label>Milestone</label>
                                <cc-search-select ng-model="modal.data.MilestoneId" items="structure.milestones" value-key="id" label-key="MilestoneName" empty-label="Project level" ng-change="milestoneChanged()"></cc-search-select></div>
                            <div><label>Task</label>
                                <cc-search-select ng-model="modal.data.TaskId" items="modalTasks" value-key="id" label-key="TaskName" empty-label="-- None --" is-disabled="!modal.data.MilestoneId"></cc-search-select></div>
                        </div>
                    </div>
                    <div ng-if="modal.data.IsSplit" class="cc-quickadd">
                        <table class="cc-table cc-mini">
                            <thead><tr><th>Cost Code *</th><th>Milestone</th><th class="cc-num">%</th><th class="cc-num">Amount</th><th></th></tr></thead>
                            <tbody>
                                <tr ng-repeat="s in modal.data.Splits track by $index">
                                    <td><cc-search-select ng-model="s.CostCodeId" items="postingCodeOptions" value-key="id" label-key="Label" placeholder="-- Select --"></cc-search-select></td>
                                    <td><cc-search-select ng-model="s.MilestoneId" items="structure.milestones" value-key="id" label-key="MilestoneName" empty-label="Project level"></cc-search-select></td>
                                    <td class="cc-num"><input type="number" step="0.01" style="width:80px" ng-model="s.Percent" ng-change="splitPercentChanged(s)" /></td>
                                    <td class="cc-num"><input type="number" step="0.01" style="width:120px" ng-model="s.Amount" ng-change="splitAmountChanged(s)" /></td>
                                    <td><button type="button" class="cc-btn cc-btn-danger" ng-click="removeSplit($index)" ng-disabled="modal.data.Splits.length<=2">x</button></td>
                                </tr>
                            </tbody>
                            <tfoot><tr><td colspan="2"><button type="button" class="cc-btn" ng-click="addSplit()">+ Add row</button> <button type="button" class="cc-btn" ng-click="balanceSplits()">Put remainder on last row</button></td>
                                <td class="cc-num">{{splitTotals().percent | number:2}} %</td><td class="cc-num" ng-class="{'cc-neg':!splitBalanced()}">{{splitTotals().amount | number:2}}</td><td></td></tr></tfoot>
                        </table>
                        <div class="cc-hint" ng-class="{'cc-neg':!splitBalanced()}">The rows must total exactly the entry amount ({{modal.data.Amount | number:2}}) = 100 %. Reports use the split rows; the entry keeps the link to them.</div>
                    </div>
                    <div class="cc-grid2">
                        <div><label>Source Document</label>
                            <cc-search-select ng-model="modal.data.SourceDocType" items="sourceDocOptions" value-key="id" label-key="Name" empty-label="-- None --"></cc-search-select></div>
                        <div><label>Reference No.</label><input type="text" maxlength="100" ng-model="modal.data.ReferenceNo" placeholder="Bill / voucher / PO number" /></div>
                    </div>
                    <div class="cc-docs" ng-if="showDocArea(modal.data)">
                        <label>Documents <span class="cc-meta">({{modal.data.SourceDocType || 'select the source document type'}})</span></label>
                        <label class="cc-drop" ng-class="{'cc-drop-off':!modal.data.SourceDocType}">
                            <input type="file" multiple accept="{{docAccept}}" cc-file-picker="addFiles(files)" ng-disabled="!modal.data.SourceDocType" />
                            <span><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M12 16V4M7 8.5l5-5 5 5M5 20h14"/></svg> Click to choose files (you can select several)</span>
                            <span class="cc-hint">PDF, images, Word, Excel, CSV or text; up to 10 MB each, 20 per entry. Files are uploaded when you click Save.</span>
                        </label>
                        <div class="cc-hint cc-neg" ng-if="modal.data.FileError">{{modal.data.FileError}}</div>
                        <div class="cc-doclist" ng-if="modal.data.Documents.length || modal.data.NewFiles.length">
                            <div class="cc-doc" ng-repeat="doc in modal.data.Documents track by doc.id" title="{{doc.FileName}}">
                                <span class="cc-doc-icon cc-doc-{{docKind(doc.FileName)}}">{{docKindLabel(doc.FileName)}}</span>
                                <span class="cc-doc-name">{{doc.FileName}}</span><span class="cc-doc-size">{{fileSize(doc.FileSize)}}</span>
                                <span class="cc-doc-tools">
                                    <button type="button" class="cc-doc-btn" ng-click="viewSavedDoc(doc)" title="View" aria-label="View {{doc.FileName}}"><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M1.5 12S5.5 4.5 12 4.5 22.5 12 22.5 12 18.5 19.5 12 19.5 1.5 12 1.5 12z"/><circle cx="12" cy="12" r="3.2"/></svg></button>
                                    <button type="button" class="cc-doc-btn cc-doc-del" ng-click="removeSavedDoc($index)" title="Remove" aria-label="Remove {{doc.FileName}}"><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M4 7h16M9 7V4.5h6V7M6.5 7l1 13h9l1-13M10 11v6M14 11v6"/></svg></button>
                                </span>
                            </div>
                            <div class="cc-doc cc-doc-new" ng-repeat="f in modal.data.NewFiles track by $index" title="{{f.name}}">
                                <span class="cc-doc-icon cc-doc-{{docKind(f.name)}}">{{docKindLabel(f.name)}}</span>
                                <span class="cc-doc-name">{{f.name}}</span><span class="cc-doc-size">New &middot; {{fileSize(f.size)}}</span>
                                <span class="cc-doc-tools">
                                    <button type="button" class="cc-doc-btn" ng-click="viewNewFile(f)" ng-if="f.url" title="View" aria-label="View {{f.name}}"><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M1.5 12S5.5 4.5 12 4.5 22.5 12 22.5 12 18.5 19.5 12 19.5 1.5 12 1.5 12z"/><circle cx="12" cy="12" r="3.2"/></svg></button>
                                    <button type="button" class="cc-doc-btn cc-doc-del" ng-click="removeNewFile($index)" title="Remove" aria-label="Remove {{f.name}}"><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M4 7h16M9 7V4.5h6V7M6.5 7l1 13h9l1-13M10 11v6M14 11v6"/></svg></button>
                                </span>
                            </div>
                        </div>
                        <div class="cc-hint" ng-repeat="doc in modal.data.RemovedDocs track by doc.id">
                            <s>{{doc.FileName}}</s> will be removed when you save. <a href="" ng-click="undoRemoveDoc($index)">Undo</a>
                        </div>
                    </div>
                    <label>Site / Location</label><input type="text" maxlength="100" ng-model="modal.data.SiteName" placeholder="Site where the cost was incurred" />
                    <label>Change / at-risk work</label>
                    <cc-search-select ng-model="modal.data.ChangeId" items="changeOptions" value-key="id" label-key="Label" empty-label="-- Not linked to a change --"></cc-search-select>
                    <div class="cc-hint">Link cost of work done for a change. At-risk work appears here only after its at-risk authorisation; the cost stays in project actuals either way.</div>
                    <label>Remarks</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Remarks"></textarea>
                    <div ng-if="modal.data.Id" class="cc-reason-label">Reason for change *
                        <button type="button" class="cc-hist-btn" ng-class="{'cc-hist-on':modal.data.ShowHistory}" ng-click="toggleEntryHistory()"
                                title="{{modal.data.ShowHistory ? 'Hide previous changes' : 'Show previous changes and their reasons'}}" aria-expanded="{{!!modal.data.ShowHistory}}" aria-label="Previous changes">
                            <svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M3.5 12a8.5 8.5 0 1 0 2.5-6M3.5 3.5V8H8M12 7.5V12l3 2"/></svg>
                            <span>Previous changes</span>
                        </button>
                    </div>
                    <div ng-if="modal.data.Id && modal.data.ShowHistory" class="cc-hist">
                        <div class="cc-hint" ng-if="modal.data.History === null">Loading...</div>
                        <div class="cc-hint cc-neg" ng-if="modal.data.HistoryError">{{modal.data.HistoryError}}</div>
                        <div class="cc-hint" ng-if="modal.data.History && !modal.data.History.length && !modal.data.HistoryError">No earlier changes recorded.</div>
                        <div class="cc-hist-list" ng-if="modal.data.History.length">
                            <div class="cc-hist-row" ng-repeat="h in modal.data.History track by h.id">
                                <div class="cc-hist-head"><b>{{h.ActionTime | ccDate}}</b> &middot; {{h.UserName || 'Unknown user'}} &middot; {{h.Summary}}</div>
                                <div ng-if="h.Reason" class="cc-hist-reason">Reason: {{h.Reason}}</div>
                            </div>
                        </div>
                    </div>
                    <div ng-if="modal.data.Id"><textarea rows="2" maxlength="1000" ng-model="modal.data.Reason" placeholder="Why is this saved entry being changed?"></textarea></div>
                </div>

                <div ng-if="modal.type==='docs'">
                    <h4>Documents &middot; {{modal.data.entry.EntryDate | ccDate}} &middot; {{modal.data.entry.SourceDocType}} {{modal.data.entry.ReferenceNo}}</h4>
                    <div class="cc-doclist">
                        <div class="cc-doc" ng-repeat="doc in modal.data.entry.documents track by doc.id" title="{{doc.FileName}}">
                            <span class="cc-doc-icon cc-doc-{{docKind(doc.FileName)}}">{{docKindLabel(doc.FileName)}}</span>
                            <span class="cc-doc-name">{{doc.FileName}}</span><span class="cc-doc-size">{{fileSize(doc.FileSize)}} &middot; {{doc.UploadedBy}} {{doc.cdate | ccDate}}</span>
                            <span class="cc-doc-tools">
                                <button type="button" class="cc-doc-btn" ng-click="viewSavedDoc(doc)" title="View" aria-label="View {{doc.FileName}}"><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M1.5 12S5.5 4.5 12 4.5 22.5 12 22.5 12 18.5 19.5 12 19.5 1.5 12 1.5 12z"/><circle cx="12" cy="12" r="3.2"/></svg></button>
                                <button type="button" class="cc-doc-btn" ng-click="downloadSavedDoc(doc)" title="Download" aria-label="Download {{doc.FileName}}"><svg class="cc-ico" viewBox="0 0 24 24" aria-hidden="true"><path d="M12 4v11M7 10.5l5 5 5-5M5 20h14"/></svg></button>
                            </span>
                        </div>
                    </div>
                    <div class="cc-hint" ng-if="access.CanEnter && modal.data.entry.IsPeriodClosed!=1">To add or remove documents, edit the cost entry.</div>
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

                <div ng-if="modal.type==='reserve'">
                    <h4>{{modal.data.Id ? 'Edit' : 'Add'}} Reserve</h4>
                    <label>Reserve type *</label>
                    <cc-search-select ng-model="modal.data.ReserveType" items="reserveTypeOptions" value-key="id" label-key="Name" placeholder="-- Select --"></cc-search-select>
                    <div class="cc-hint">Contractor contingency is internal only (never shown to the client). Client allowances and provisional sums follow the contract. The amount is set with a "Reserve addition" movement, which needs approval. No default percentage is applied.</div>
                    <label>Name *</label><input type="text" maxlength="200" ng-model="modal.data.Name" placeholder="e.g. Design development contingency" />
                    <label>Remarks</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Remarks"></textarea>
                </div>

                <div ng-if="modal.type==='movement'">
                    <h4>{{modal.data.Id ? 'Edit' : 'New'}} Budget Movement</h4>
                    <div class="cc-grid2">
                        <div><label>Type *</label><cc-search-select ng-model="modal.data.MovementType" items="movementTypeOptions" value-key="id" label-key="Name" placeholder="-- Select --" is-disabled="modal.data.ChangeId" ng-change="movementTypeChanged()"></cc-search-select></div>
                        <div><label>Risk / document reference</label><input type="text" maxlength="100" ng-model="modal.data.RiskReference" /></div>
                    </div>
                    <div class="cc-hint">{{movementTypeHint(modal.data.MovementType)}}</div>
                    <label>Reason *</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Reason"></textarea>
                    <table class="cc-table cc-mini" style="margin-top:8px">
                        <thead><tr><th>Line</th><th>Cost Code / Reserve</th><th>Milestone</th><th class="cc-num">Amount (+ / -)</th><th></th></tr></thead>
                        <tbody>
                            <tr ng-if="!modal.data.Lines.length"><td colspan="5" class="cc-empty">Add lines below.</td></tr>
                            <tr ng-repeat="l in modal.data.Lines track by $index">
                                <td>{{l.LineKind}}</td>
                                <td>
                                    <cc-search-select ng-if="l.LineKind==='Budget'" ng-model="l.CostCodeId" items="postingCodeOptions" value-key="id" label-key="Label" placeholder="-- Cost code --"></cc-search-select>
                                    <cc-search-select ng-if="l.LineKind==='Reserve'" ng-model="l.ReserveId" items="reserveOptions" value-key="id" label-key="Label" placeholder="-- Reserve --"></cc-search-select>
                                </td>
                                <td><cc-search-select ng-if="l.LineKind==='Budget'" ng-model="l.MilestoneId" items="structure.milestones" value-key="id" label-key="MilestoneName" empty-label="Project level"></cc-search-select></td>
                                <td class="cc-num"><input type="number" step="0.01" style="width:130px" ng-model="l.Amount" /></td>
                                <td><button type="button" class="cc-btn cc-btn-danger" ng-click="modal.data.Lines.splice($index,1)">x</button></td>
                            </tr>
                        </tbody>
                        <tfoot><tr><td colspan="3">
                            <button type="button" class="cc-btn" ng-if="movementAllows('Budget')" ng-click="addMovementLine('Budget')">+ Budget line</button>
                            <button type="button" class="cc-btn" ng-if="movementAllows('Reserve')" ng-click="addMovementLine('Reserve')">+ Reserve line</button></td>
                            <td class="cc-num">Budget {{movementTotals().budget | number:2}}<br />Reserve {{movementTotals().reserve | number:2}}<br /><b ng-class="{'cc-neg':movementTotals().net!==0 && movementMustNet()}">Net {{movementTotals().net | number:2}}</b></td><td></td></tr></tfoot>
                    </table>
                </div>

                <div ng-if="modal.type==='change'">
                    <h4>{{modal.data.Id ? 'Edit' : 'New'}} Change</h4>
                    <label>Title *</label><input type="text" maxlength="200" ng-model="modal.data.Title" />
                    <div class="cc-grid2">
                        <div><label>Class *</label><cc-search-select ng-model="modal.data.ChangeClass" items="changeClassOptions" value-key="id" label-key="Name" placeholder="-- Select --"></cc-search-select></div>
                        <div><label>Estimated cost *</label><input type="number" step="0.01" ng-model="modal.data.EstimatedCost" /></div>
                    </div>
                    <div class="cc-grid2">
                        <div><label>Part already in ETC</label><input type="number" step="0.01" min="0" ng-model="modal.data.AmountInEtc" /><div class="cc-hint">So probable pending cost (PPC) does not double count (rule F-2).</div></div>
                        <div><label style="display:flex;align-items:center;gap:6px;margin-top:28px"><input type="checkbox" class="cc-check" ng-model="modal.data.IsProbable" /> Probable (counts in PPC / EACp)</label></div>
                    </div>
                    <div class="cc-grid2">
                        <div><label>Client value proposed</label><input type="number" step="0.01" ng-model="modal.data.ClientValueProposed" /></div>
                        <div><label>Subcontract value</label><input type="number" step="0.01" ng-model="modal.data.SubcontractValue" /></div>
                    </div>
                    <label style="display:flex;align-items:center;gap:6px"><input type="checkbox" class="cc-check" ng-model="modal.data.IsAtRisk" /> At-risk work: work will start before client approval (needs a separate at-risk authorisation)</label>
                    <label>Description</label><textarea rows="2" maxlength="2000" ng-model="modal.data.Description"></textarea>
                    <label>Cost allocations (used when the change is posted to the budget)</label>
                    <table class="cc-table cc-mini">
                        <thead><tr><th>Cost Code *</th><th>Milestone</th><th class="cc-num">Amount</th><th></th></tr></thead>
                        <tbody>
                            <tr ng-repeat="a in modal.data.Allocations track by $index">
                                <td><cc-search-select ng-model="a.CostCodeId" items="postingCodeOptions" value-key="id" label-key="Label" placeholder="-- Select --"></cc-search-select></td>
                                <td><cc-search-select ng-model="a.MilestoneId" items="structure.milestones" value-key="id" label-key="MilestoneName" empty-label="Project level"></cc-search-select></td>
                                <td class="cc-num"><input type="number" step="0.01" style="width:130px" ng-model="a.Amount" /></td>
                                <td><button type="button" class="cc-btn cc-btn-danger" ng-click="modal.data.Allocations.splice($index,1)">x</button></td>
                            </tr>
                        </tbody>
                        <tfoot><tr><td colspan="2"><button type="button" class="cc-btn" ng-click="modal.data.Allocations.push({CostCodeId:null,MilestoneId:null,Amount:null})">+ Allocation</button></td>
                            <td class="cc-num" ng-class="{'cc-warn':sum(modal.data.Allocations,'Amount')!==(modal.data.EstimatedCost||0)}">{{sum(modal.data.Allocations,'Amount') | number:2}}</td><td></td></tr></tfoot>
                    </table>
                </div>

                <div ng-if="modal.type==='changeStatus'">
                    <h4>Change #{{modal.data.ChangeNo}}: client / subcontract status</h4>
                    <div class="cc-hint">Each status is independent. Client approval does not authorise internal cost or post the budget.</div>
                    <div class="cc-grid2">
                        <div><label>Which status *</label><cc-search-select ng-model="modal.data.Field" items="statusFieldOptions" value-key="id" label-key="Name" ng-change="statusFieldChanged()"></cc-search-select></div>
                        <div><label>New value *</label><cc-search-select ng-model="modal.data.Value" items="statusValueOptions" value-key="id" label-key="Name" placeholder="-- Select --"></cc-search-select></div>
                    </div>
                    <div class="cc-grid2" ng-if="modal.data.Value==='Approved' || modal.data.Value==='Rejected'">
                        <div><label>Decision date *</label><input type="date" ng-model="modal.data.DecisionDateValue" /></div>
                        <div ng-if="modal.data.Field==='Client' && modal.data.Value==='Approved'"><label>Approved client value *</label><input type="number" step="0.01" ng-model="modal.data.Amount" /></div>
                    </div>
                    <label>Reason{{modal.data.Value==='Rejected' ? ' *' : ''}}</label><textarea rows="2" maxlength="1000" ng-model="modal.data.Reason"></textarea>
                </div>

                <div ng-if="modal.type==='accessProjects'">
                    <h4>Projects for {{modal.data.EmployeeName}}</h4>
                    <div class="cc-hint">Tick the projects this employee may use in Cost Control. Leave all unticked for access to every project.</div>
                    <input type="search" ng-model="modal.data.search" placeholder="Search projects..." style="width:100%;margin:8px 0" />
                    <div style="max-height:300px;overflow:auto">
                        <label ng-repeat="p in projects | filter:{ProjectName: modal.data.search} track by p.id" style="display:flex;align-items:center;gap:8px;margin:4px 0">
                            <input type="checkbox" class="cc-check" ng-model="modal.data.selected[p.id]" /> {{p.ProjectName}}</label>
                    </div>
                </div>

                <div class="cc-modal-footer">
                    <button type="button" class="cc-btn" ng-click="closeModal()" ng-disabled="busy">{{modal.type==='docs' ? 'Close' : 'Cancel'}}</button>
                    <button type="button" class="cc-btn cc-btn-primary" ng-if="modal.type!=='docs'" ng-click="saveModal()" ng-disabled="busy">{{busy ? 'Saving...' : (modal.type==='reason' ? modal.data.button : 'Save')}}</button>
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
