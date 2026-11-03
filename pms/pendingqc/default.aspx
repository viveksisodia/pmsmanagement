<%@ Page Title="QC Action" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pms_qcaction_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        window.QCActionPage = {
            regid: <%= pageRegId %>,
            employeeId: <%= pageEmployeeId %>,
            isadmin: <%= isadmin %>,
            apiBase: '<%= System.Web.HttpUtility.JavaScriptStringEncode(qcApiBase) %>'
        };
    </script>

    <script src="/pms/pendingqc/app/qcaction.main.js?v=<%=DateTime.Now.Ticks %>"></script>

    <style>
        [ng-cloak] { display:none !important; }
        .qc-page { padding:37px 18px 20px; background:#f4f6f8; min-height:calc(100vh - 110px); }
        .qc-card { margin:0 auto 10px; }
        .qc-card { background:#fff; border:1px solid #e7e9ed; border-radius:7px; box-shadow:0 1px 3px rgba(16,24,40,.04); overflow:hidden; }
        .qc-titlebar { display:flex; align-items:center; justify-content:space-between; gap:12px; min-height:42px; padding:9px 14px; border-bottom:1px solid #eceef1; }
        .qc-title { margin:0; font-size:18px; font-weight:600; color:#20242a; }
        .qc-mode { font-size:11px; font-weight:600; padding:4px 9px; border-radius:12px; background:#f1f3f5; color:#495057; }
        .qc-mode-admin { background:#efe8fb; color:#6f42c1; }
        .qc-filterbar { display:grid; grid-template-columns:160px 170px 170px auto; gap:9px; padding:10px 14px; align-items:center; border-bottom:1px solid #eceef1; }
        .qc-filterbar select,.qc-filterbar input { min-height:36px; border:1px solid #d9dde3; border-radius:5px; padding:6px 9px; background:#fff; font-size:12px; width:100%; }
        .qc-date-custom { display:flex; gap:8px; padding:9px 14px; align-items:center; border-bottom:1px solid #eceef1; background:#fcfcfd; }
        .qc-date-custom label { margin:0; font-size:11px; color:#707782; }
        .qc-date-custom input { width:150px; min-height:34px; border:1px solid #d9dde3; border-radius:5px; padding:5px 8px; font-size:12px; }
        .qc-btn { border:0; border-radius:5px; min-height:35px; padding:6px 13px; font-size:12px; font-weight:600; cursor:pointer; }
        .qc-btn-primary { background:#7b0b69; color:#fff; }
        .qc-btn:disabled,.qc-modal-close:disabled,.qc-modal-remark:disabled { opacity:.6; cursor:not-allowed; }
        .qc-btn-light { background:#f1f3f5; color:#343a40; }
        .qc-btn:disabled { opacity:.65; cursor:not-allowed; }
        .qc-actions-top { display:flex; gap:7px; justify-content:flex-end; }
        .qc-gridsearch { display:flex; align-items:center; gap:8px; padding:8px 14px; border-bottom:1px solid #eceef1; background:#fff; }
        .qc-gridsearch input { flex:1; min-width:220px; height:32px; border:1px solid #d9dde3; border-radius:5px; padding:5px 9px; font-size:12px; background:#fff; }
        .qc-gridsearch-count { font-size:11px; color:#7a828c; white-space:nowrap; }
        .qc-alert { margin:9px 14px 0; padding:8px 10px; border-radius:5px; font-size:12px; }
        .qc-alert-error { background:#fff1f0; border:1px solid #ffd7d4; color:#a61b1b; }
        .qc-alert-success { background:#effaf3; border:1px solid #ccebd6; color:#23743c; }
        /* Keep the pager visible and scroll only the grid rows when page size is large. */
        .qc-tablewrap {
            position:relative;
            min-height:110px;
            max-height:calc(100vh - 310px);
            overflow-x:auto;
            overflow-y:auto;
            scrollbar-gutter:stable;
            -webkit-overflow-scrolling:touch;
        }
        .qc-tablewrap::-webkit-scrollbar { width:10px; height:10px; }
        .qc-tablewrap::-webkit-scrollbar-thumb { background:#b8bec6; border-radius:8px; border:2px solid #fff; }
        .qc-tablewrap::-webkit-scrollbar-track { background:#f2f4f6; }
        .qc-table tbody { transition:opacity .16s ease; }
        .qc-tablewrap.qc-refreshing .qc-table tbody { opacity:.58; }
        .qc-refreshline { position:absolute; top:0; left:0; right:0; height:2px; z-index:5; overflow:hidden; background:rgba(123,11,105,.10); }
        .qc-refreshline span { display:block; width:34%; height:100%; background:#7b0b69; animation:qcloadline 1s ease-in-out infinite; }
        @keyframes qcloadline { 0% { transform:translateX(-110%); } 100% { transform:translateX(310%); } }
        .qc-table { width:100%; border-collapse:collapse; margin:0; min-width:1050px; }
        .qc-table th { position:sticky; top:0; z-index:1; background:#f8f9fb; color:#5e6672; font-size:11px; font-weight:600; white-space:nowrap; padding:7px 8px; border-bottom:1px solid #e5e7eb; text-align:left; }
        .qc-table td { font-size:12px; color:#2e3338; padding:6px 8px; border-bottom:1px solid #edf0f2; vertical-align:middle; line-height:1.25; }
        .qc-table tbody tr:hover { background:#fbfcfd; }
        .qc-entity { font-weight:600; color:#25282d; }
        .qc-sub { display:block; margin-top:2px; font-size:10px; color:#8a9199; font-weight:400; }
        .qc-badge { display:inline-block; padding:3px 7px; border-radius:12px; font-size:10px; font-weight:600; white-space:nowrap; }
        .qc-type-project { background:#eaf2ff; color:#285ca8; }
        .qc-type-milestone { background:#fff5df; color:#8a5b08; }
        .qc-type-task { background:#edf8ef; color:#327744; }
        .qc-type-subtask { background:#f2edfb; color:#6741a5; }
        .qc-status-pending { background:#fff3cd; color:#7a5d00; }
        .qc-status-done { background:#dff5e5; color:#23743c; }
        .qc-status-closed { background:#eceff2; color:#59616b; }
        .qc-actionbox { min-width:150px; max-width:170px; }
        .qc-actionrow { display:flex; gap:6px; align-items:center; }
        .qc-actionrow select { width:150px; min-width:150px; height:28px; border:1px solid #d7dce2; border-radius:4px; padding:2px 6px; font-size:11px; background:#fff; }
        .qc-remark { width:100%; margin-top:6px; min-height:32px; border:1px solid #d7dce2; border-radius:4px; padding:6px 7px; font-size:11px; resize:vertical; }
        .qc-modal-backdrop { position:fixed; inset:0; z-index:99999; background:rgba(0,0,0,.38); display:flex; align-items:center; justify-content:center; padding:18px; }
        .qc-modal { width:100%; max-width:750px; background:#fff; border-radius:8px; box-shadow:0 18px 45px rgba(0,0,0,.25); overflow:hidden; }
        .qc-modal-header { display:flex; align-items:center; justify-content:space-between; padding:14px 16px; border-bottom:1px solid #eceef1; }
        .qc-modal-title { margin:0; font-size:16px; font-weight:600; color:#20242a; }
        .qc-modal-close { border:0; background:transparent; font-size:22px; line-height:1; color:#6c757d; cursor:pointer; }
        .qc-modal-body { padding:16px; }
        .qc-modal-row { margin-bottom:12px; font-size:12px; color:#555d66; }
        .qc-modal-row strong { color:#25282d; }
        .qc-modal-label { display:block; margin-bottom:5px; font-size:12px; font-weight:600; color:#343a40; }
        .qc-modal-remark { width:100%; min-height:95px; border:1px solid #d7dce2; border-radius:5px; padding:8px 9px; font-size:12px; resize:vertical; }
        .qc-required { color:#c92a2a; }
        .qc-modal-footer { display:flex; justify-content:flex-end; gap:8px; padding:12px 16px; border-top:1px solid #eceef1; background:#fafbfc; }
        .qc-taken { font-size:11px; line-height:1.5; color:#3e4650; }
        .qc-taken strong { color:#1f2429; }
        .qc-remark-icon { display:inline-block; margin-left:6px; padding:3px 8px; border:0; border-radius:12px; background:#fff3cd; color:#8a5b08; box-shadow:none; font-size:10px; font-weight:700; line-height:1.2; cursor:help; vertical-align:middle; transition:background-color .15s ease,color .15s ease; }
        .qc-history-icon { cursor:pointer; }
        .qc-remark-icon:hover { background:#ffe69c; color:#664d03; }
        .qc-remark-icon:focus { outline:2px solid #7b0b69; outline-offset:2px; }
        .qc-empty { padding:35px 12px; text-align:center; color:#7a828c; font-size:12px; }
        .qc-loading { display:inline-flex; align-items:center; gap:6px; color:#6c757d; font-size:11px; }
        .qc-spinner { width:13px; height:13px; border:2px solid #d4d7db; border-top-color:#7b0b69; border-radius:50%; animation:qcspin .7s linear infinite; }
        .qc-paging { display:flex; align-items:center; justify-content:space-between; gap:12px; padding:9px 14px; border-top:1px solid #eceef1; background:#fff; font-size:11px; color:#68717c; }
        .qc-pagesize { display:flex; align-items:center; gap:7px; white-space:nowrap; }
        .qc-pagesize select { height:29px; min-width:62px; border:1px solid #d7dce2; border-radius:4px; padding:3px 6px; background:#fff; font-size:11px; }
        .qc-pagebuttons { display:flex; align-items:center; gap:4px; }
        .qc-pagebutton { min-width:29px; height:29px; border:1px solid #d7dce2; border-radius:4px; padding:3px 7px; background:#fff; color:#424a53; font-size:11px; cursor:pointer; }
        .qc-pagebutton:hover:not(:disabled) { border-color:#7b0b69; color:#7b0b69; }
        .qc-pagebutton.active { border-color:#7b0b69; background:#7b0b69; color:#fff; }
        .qc-pagebutton:disabled { opacity:.5; cursor:not-allowed; }
        @keyframes qcspin { to { transform:rotate(360deg); } }
        @media(max-width:1100px) { .qc-filterbar { grid-template-columns:1fr 1fr; } }
        @media(max-width:650px) { .qc-filterbar { grid-template-columns:1fr; } .qc-page { padding:8px 10px 14px; } .qc-tablewrap { max-height:60vh; } }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="qc-page" ng-app="qcActionApp" ng-controller="qcActionCtrl" ng-cloak>
        <div class="qc-card">
            <div class="qc-titlebar">
                <div>
                    <h3 class="qc-title">QC Action List</h3>
                   <%-- <span class="qc-sub">{{totalRows}} record(s)</span>--%>
                </div>
                <%--<span class="qc-mode" ng-class="{'qc-mode-admin':isadmin==1}">
                    {{isadmin==1 ? 'Admin - All Company QC' : 'My QC Actions'}}
                </span>--%>
            </div>

            <div class="qc-filterbar">
                <!-- Type filter: Project / Milestone / Task / Sub Task -->
                <select ng-model="filter.EntityType"
                        ng-options="x.Id as x.Name for x in entityTypes"
                        ng-change="serverFilterChanged()"></select>

                <!-- Time filter like All Dates dropdown -->
                <select ng-model="filter.DateFilter"
                        ng-options="x.Id as x.Name for x in dateFilters"
                        ng-change="onDateFilterChanged()"></select>

                <select ng-model="filter.QCStatus"
                        ng-options="x.Id as x.Name for x in statusFilters"
                        ng-change="serverFilterChanged()"></select>

                <div class="qc-actions-top">
                    <button type="button" class="qc-btn qc-btn-light" ng-click="clearFilters()" ng-disabled="loading">Clear Filters</button>
                </div>
            </div>

            <div class="qc-date-custom" ng-if="filter.DateFilter=='CUSTOM'">
                <label>From</label>
                <input type="date" ng-model="filter.FromDate" />
                <label>To</label>
                <input type="date" ng-model="filter.ToDate" />
                <button type="button" class="qc-btn qc-btn-primary" ng-click="serverFilterChanged()">Apply</button>
            </div>

            <div class="qc-alert qc-alert-success" ng-if="success">{{success}}</div>

            <!-- Free search works only on already loaded grid rows; no server/API call -->
            <div class="qc-gridsearch">
                <input type="text"
                       ng-model="gridSearchText"
                       placeholder="Search in current QC grid..."
                       autocomplete="off" />
               <%-- <span class="qc-gridsearch-count" ng-if="rows.length>0">
                    {{filteredRows.length || 0}} / {{rows.length}}
                </span>--%>
                <button type="button" class="qc-btn qc-btn-light"
                        ng-click="clearGridSearch()"
                        ng-disabled="!gridSearchText">Clear Search</button>
            </div>

            <div class="qc-tablewrap" ng-class="{'qc-refreshing':loading && rows.length>0}" aria-busy="{{loading}}">
                <div class="qc-refreshline" ng-if="loading && rows.length>0"><span></span></div>
                <table class="qc-table">
                    <thead>
                        <tr>
                            <th style="width:45px;">#</th>
                            <th style="width:70px;">Sr. No.</th>
                            <th>Date</th>
                            <th>Type</th>
                            <th>Task Name</th>
                            <th>Project</th>
                            <%--<th>Workflow</th>--%>
                            <th ng-if="isadmin==1">Notified To</th>
                            <th>QC Status</th>
                            <th style="width:170px; min-width:170px;">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-if="loading && rows.length==0">
                            <td ng-attr-colspan="{{isadmin==1 ? 9 : 8}}" class="qc-empty">
                                <span class="qc-loading"><span class="qc-spinner"></span> Loading QC list...</span>
                            </td>
                        </tr>

                        <tr ng-repeat="row in pagedRows track by $index">
                            <td>{{((currentPage-1)*pageSize)+$index+1}}</td>
                            <td>{{row.HierarchySrNo || '-'}}</td>
                            <td>
                                {{row.QCDate}}
                                <%--<span class="qc-sub">Runtime #{{row.RuntimeId}} / Cycle {{row.CycleNo}}</span>--%>
                            </td>
                            <td>
                                <span class="qc-badge" ng-class="entityClass(row.EntityType)">{{row.EntityType}}</span>
                            </td>
                            <td>
                                <span class="qc-entity">{{row.EntityName || ('ID: '+row.EntityId)}}</span>
                                <%--<span class="qc-sub">Entity ID: {{row.EntityId}}</span>--%>
                            </td>
                            <td>{{row.ProjectName || '-'}}</td>
                            <%--<td>
                                {{row.WorkflowName || ('Workflow #'+row.WorkflowId)}}
                                <span class="qc-sub">Branch {{row.BranchNo}}</span>
                            </td>--%>
                            <td ng-if="isadmin==1">{{row.NotifiedTo || '-'}}</td>
                            <td>
                                <span class="qc-badge" ng-class="statusClass(row.QCStatus)">{{row.QCStatus=='PENDING' ? 'Pending' : (row.TakenActionName || row.QCStatus)}}</span>
                                <span class="qc-remark-icon"
                                      ng-if="row.QCStatus!='PENDING' && row.ActionRemark"
                                      data-bs-toggle="tooltip" data-bs-placement="top"
                                      ng-attr-data-bs-title="{{row.ActionRemark}}"
                                      aria-label="Action remark" tabindex="0">R</span>
                                <button type="button" class="qc-remark-icon qc-history-icon" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="QC action history" aria-label="QC action history" ng-click="openHistory(row)"><svg width="7" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" aria-hidden="true" style="vertical-align:middle"><path d="M3 11a9 9 0 1 1 2 7M3 4v7h7"/><path d="M12 7v5l3 2"/></svg></button>
                            </td>
                            <td>
                                <select ng-if="row.PendingRuntimes.length>1" ng-model="row.SelectedRuntimeId"
                                        ng-change="selectPendingRuntime(row)" ng-disabled="row.ActionBusy"
                                        ng-options="r.RuntimeId as (r.WorkflowName + ' / Cycle ' + r.CycleNo + ' / QC ' + r.RuntimeId) for r in row.PendingRuntimes"></select>
                                <div class="qc-actionbox" ng-if="row.RuntimeStatus=='OPEN' && row.Actions.length>0">
                                    <div class="qc-actionrow">
                                        <select ng-model="row.SelectedActionId"
                                                ng-change="openActionPopup(row)"
                                                ng-disabled="row.ActionBusy"
                                                ng-options="a.ActionId as a.ActionName for a in row.Actions">
                                            <option value="">Select Action</option>
                                        </select>
                                    </div>
                                </div>

                                <span ng-if="row.RuntimeStatus=='OPEN' && row.Actions.length==0" class="qc-sub">No Action Configured</span>
                                <span ng-if="row.RuntimeStatus!='OPEN'" class="qc-sub">Completed</span>
                            </td>
                        </tr>

                        <tr ng-if="!loading && rows.length==0">
                            <td ng-attr-colspan="{{isadmin==1 ? 9 : 8}}" class="qc-empty">No QC records found for selected filters.</td>
                        </tr>
                        <tr ng-if="!loading && rows.length>0 && filteredRows.length==0">
                            <td ng-attr-colspan="{{isadmin==1 ? 9 : 8}}" class="qc-empty">No QC records match your search.</td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="qc-paging" ng-if="totalRows>0">
                <div class="qc-pagesize">
                    <select ng-model="pageSize"
                            ng-options="size for size in pageSizeOptions"
                            ng-change="changePageSize()"></select>
                    <span>Records per page</span>
                    <span>{{((currentPage-1)*pageSize)+1}}-{{((currentPage-1)*pageSize)+rows.length}} of {{totalRows}}</span>
                </div>

                <div class="qc-pagebuttons">
                    <button type="button" class="qc-pagebutton"
                            ng-click="goToPage(currentPage-1)"
                            ng-disabled="currentPage==1">Previous</button>

                    <button type="button" class="qc-pagebutton"
                            ng-repeat="pageNo in getPageNumbers() track by pageNo"
                            ng-class="{'active':pageNo==currentPage}"
                            ng-click="goToPage(pageNo)">{{pageNo}}</button>

                    <button type="button" class="qc-pagebutton"
                            ng-click="goToPage(currentPage+1)"
                            ng-disabled="currentPage==totalPages">Next</button>
                </div>
            </div>
        </div>

        <div class="qc-modal-backdrop" ng-if="historyPopup.Show">
            <div class="qc-modal" role="dialog" aria-modal="true" aria-label="QC action history">
                <div class="qc-modal-header">
                    <h4 class="qc-modal-title">QC History - {{historyPopup.Row.EntityName}}</h4>
                    <button type="button" class="qc-modal-close" ng-click="historyPopup.Show=false">&times;</button>
                </div>
                <div class="qc-modal-body" style="max-height:65vh;overflow:auto">
                    <table class="table"><thead><tr><th>Sr No</th><th>Remark</th><th>Action</th></tr></thead>
                        <tbody><tr ng-repeat="h in historyPopup.Rows track by $index">
                            <td>{{$index+1}}</td><td style="white-space:pre-wrap;overflow-wrap:anywhere">{{h.ActionRemark || '-'}}</td>
                            <td><strong>{{h.TakenActionName || 'Action Taken'}}</strong>
                                <div>{{h.ActionTakenDate}}</div><div>{{h.ActionTakenByName || ('User '+h.ActionTakenBy)}}</div></td>
                        </tr><tr ng-if="!historyPopup.Rows.length"><td colspan="3">No actions taken yet.</td></tr></tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- QC Action Remark Popup: opens immediately after selecting an action -->
        <div class="qc-modal-backdrop" ng-if="actionPopup.Show">
            <div class="qc-modal" role="dialog" aria-modal="true">
                <div class="qc-modal-header">
                    <h4 class="qc-modal-title">Take QC Action</h4>
                    <button type="button" class="qc-modal-close" ng-click="closeActionPopup(true)" ng-disabled="actionPopup.Row.ActionBusy">&times;</button>
                </div>

                <div class="qc-modal-body">
                    <div class="qc-modal-row">
                        <strong>Entity:</strong> {{actionPopup.Row.EntityName || actionPopup.Row.EntityType}}
                        <span class="qc-sub">{{actionPopup.Row.EntityType}} #{{actionPopup.Row.EntityId}}</span>
                    </div>
                    <div class="qc-modal-row">
                        <strong>Selected Action:</strong> {{actionPopup.Action.ActionName}}
                    </div>

                    <label class="qc-modal-label">
                        Remark
                        <span class="qc-required" ng-if="actionPopup.Action.IsRemarkRequired">*</span>
                        <span ng-if="!actionPopup.Action.IsRemarkRequired">(Optional)</span>
                    </label>
                    <textarea class="qc-modal-remark"
                              ng-model="actionPopup.Remark"
                              ng-disabled="actionPopup.Row.ActionBusy"
                              maxlength="2000"
                              placeholder="Enter remark..."></textarea>
                    <span class="qc-sub" ng-if="actionPopup.Action.IsRemarkRequired">Remark is required for this action.</span>
                </div>

                <div class="qc-modal-footer">
                    <button type="button" class="qc-btn qc-btn-light"
                            ng-click="closeActionPopup(true)"
                            ng-disabled="actionPopup.Row.ActionBusy">Cancel</button>
                    <button type="button" class="qc-btn qc-btn-primary"
                            ng-click="submitActionPopup()"
                            ng-disabled="actionPopup.Row.ActionBusy">
                        {{actionPopup.Row.ActionBusy ? 'Saving...' : 'Take Action'}}
                    </button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
