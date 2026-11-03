<%@ Page Title="Issues List" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pms_issues_default" %>
<asp:Content ID="IssuesHead" ContentPlaceHolderID="head" Runat="Server">
    <script>
        window.IssuesPage = {
            regid: <%= pageRegId %>,
            employeeId: <%= pageEmployeeId %>,
            isadmin: <%= isadmin %>
        };
    </script>
    <script src="app/issues.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <style>
        [ng-cloak]{display:none!important}.issues-page{padding:28px 18px 20px;background:#f4f6f8;min-height:calc(100vh - 110px);color:#252a31}.issues-card{position:relative;background:#fff;border:1px solid #e7e9ed;border-radius:7px;box-shadow:0 1px 3px rgba(16,24,40,.04);overflow:visible}.issues-titlebar{display:flex;align-items:center;justify-content:space-between;min-height:46px;padding:9px 14px;border-bottom:1px solid #eceef1}.issues-title{margin:0;font-size:18px;font-weight:600}.issues-mode{font-size:11px;font-weight:600;padding:4px 9px;border-radius:12px;background:#efe8fb;color:#6f42c1}.issues-filterbar{display:grid;grid-template-columns:150px 300px 180px 230px auto;gap:9px;padding:10px 14px;align-items:center;border-bottom:1px solid #eceef1}.issues-filterbar select,.issues-gridsearch input{min-height:36px;border:1px solid #d9dde3;border-radius:5px;padding:6px 9px;background:#fff;font-size:12px;width:100%}.issues-actions-top{display:flex;justify-content:flex-end}.issues-btn{border:0;border-radius:5px;min-height:35px;padding:6px 13px;font-size:12px;font-weight:600;cursor:pointer}.issues-btn-light{background:#f1f3f5;color:#343a40}.issues-gridsearch{display:flex;align-items:center;gap:8px;padding:8px 14px;border-bottom:1px solid #eceef1}.issues-gridsearch input{flex:1;min-width:200px;height:33px}.issues-count{font-size:11px;color:#7a828c;white-space:nowrap}.issues-alert{margin:9px 14px 0;padding:8px 10px;border-radius:5px;font-size:12px;background:#fff1f0;border:1px solid #ffd7d4;color:#a61b1b}.issues-tablewrap{position:relative;min-height:120px;max-height:calc(100vh - 290px);overflow:auto;scrollbar-gutter:stable;-webkit-overflow-scrolling:touch}.issues-tablewrap::-webkit-scrollbar{width:10px;height:10px}.issues-tablewrap::-webkit-scrollbar-thumb{background:#b8bec6;border-radius:8px;border:2px solid #fff}.issues-table{width:100%;border-collapse:collapse;margin:0;min-width:1250px}.issues-table th{position:sticky;top:0;z-index:1;background:#f8f9fb;color:#5e6672;font-size:11px;font-weight:600;white-space:nowrap;padding:8px;border-bottom:1px solid #e5e7eb;text-align:left}.issues-table td{font-size:12px;color:#2e3338;padding:8px;border-bottom:1px solid #edf0f2;vertical-align:middle;line-height:1.3}.issues-table tbody tr:hover{background:#fbfcfd}.issues-name{font-weight:600;color:#25282d;min-width:180px;white-space:normal}.issues-type,.issues-status{display:inline-block;padding:3px 7px;border-radius:12px;font-size:10px;font-weight:600;white-space:nowrap}.issues-type-project{background:#eaf2ff;color:#285ca8}.issues-type-milestone{background:#fff5df;color:#8a5b08}.issues-type-task{background:#edf8ef;color:#327744}.issues-type-subtask{background:#f2edfb;color:#6741a5}.issues-status-pending{background:#fff3cd;color:#7a5d00}.issues-status-complete{background:#dff5e5;color:#23743c}.issues-empty{padding:35px 12px!important;text-align:center!important;color:#7a828c;font-size:12px!important}.issues-loading{display:inline-flex;align-items:center;gap:7px;color:#6c757d}.issues-spinner{width:14px;height:14px;border:2px solid #d4d7db;border-top-color:#7b0b69;border-radius:50%;animation:issuespin .7s linear infinite}.issues-refreshline{position:absolute;top:0;left:0;right:0;height:2px;z-index:5;overflow:hidden;background:rgba(123,11,105,.10)}.issues-refreshline span{display:block;width:34%;height:100%;background:#7b0b69;animation:issuesloadline 1s ease-in-out infinite}.issues-table tbody{transition:opacity .16s ease}.issues-tablewrap.refreshing .issues-table tbody{opacity:.58}.issues-paging{display:flex;align-items:center;justify-content:space-between;gap:12px;padding:9px 14px;border-top:1px solid #eceef1;background:#fff;font-size:11px;color:#68717c}.issues-paging select,.issues-pagebutton{height:29px;border:1px solid #d7dce2;border-radius:4px;padding:3px 7px;background:#fff;color:#424a53;font-size:11px}.issues-pagebuttons{display:flex;align-items:center;gap:4px}.issues-pagebutton{cursor:pointer}.issues-pagebutton.active{border-color:#7b0b69;background:#7b0b69;color:#fff}.issues-pagebutton:disabled{opacity:.5;cursor:not-allowed}.issues-employee{position:relative;min-width:0}.issues-employee-trigger{width:100%;min-height:36px;max-height:74px;border:1px solid #d9dde3;border-radius:5px;padding:4px 8px;background:#fff;text-align:left;color:#343a40;font-size:12px;display:flex;justify-content:space-between;align-items:center;gap:8px;overflow-y:auto;cursor:text}.issues-employee:focus-within .issues-employee-trigger{border-color:#7b0b69}.issues-employee-inline-search{flex:1 1 100px;min-width:90px;width:100%;height:25px;padding:3px 2px;border:0!important;outline:0;background:transparent;font-size:12px;color:#343a40}.issues-employee-trigger-content{display:flex;flex:1 1 auto;flex-wrap:wrap;align-items:center;gap:4px;min-width:0}.issues-employee-trigger .issues-employee-chip{padding:3px 5px}.issues-employee-trigger-arrow{flex:0 0 auto;align-self:center;color:#59636e}.issues-employee-popup{position:absolute;left:0;top:calc(100% + 4px);z-index:25;width:min(340px,85vw);min-width:100%;box-sizing:border-box;padding:8px;background:#fff;border:1px solid #d9dde3;border-radius:6px;box-shadow:0 8px 24px rgba(20,30,40,.16)}.issues-employee-search{width:100%;height:32px;border:1px solid #d9dde3;border-radius:4px;padding:5px 8px;margin-bottom:7px;font-size:12px}.issues-employee-selected{display:flex;flex-wrap:wrap;gap:5px;padding:0 0 7px;max-height:76px;overflow:auto}.issues-employee-chip{display:inline-flex;align-items:center;gap:6px;max-width:100%;padding:4px 6px;border-radius:4px;background:#eaf2fb;color:#315d82;font-size:11px}.issues-employee-chip span{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.issues-employee-chip button{border:0;background:transparent;color:#526b7f;font-size:14px;line-height:12px;padding:0 1px;cursor:pointer}.issues-employee-options{max-height:230px;overflow:auto}.issues-employee-option{display:flex;align-items:center;justify-content:space-between;gap:8px;width:100%;border:0;background:#fff;text-align:left;padding:7px 8px;font-size:12px;color:#343a40;cursor:pointer}.issues-employee-option:hover,.issues-employee-option:focus,.issues-employee-option.active{background:#e7f1fb;outline:0}.issues-employee-option-mark{color:#315d82;font-weight:700}.issues-employee-empty{padding:12px 5px;text-align:center;color:#7a828c;font-size:11px}.issues-employee-footer{display:flex;justify-content:space-between;gap:8px;padding-top:8px;margin-top:6px;border-top:1px solid #eceef1}.issues-employee-error{color:#a61b1b;font-size:11px;padding:7px 4px}@keyframes issuespin{to{transform:rotate(360deg)}}@keyframes issuesloadline{0%{transform:translateX(-110%)}100%{transform:translateX(310%)}}@media(max-width:1100px){.issues-filterbar{grid-template-columns:1fr 1fr}}@media(max-width:650px){.issues-filterbar{grid-template-columns:1fr}.issues-page{padding:8px 10px 14px}.issues-tablewrap{max-height:60vh}.issues-gridsearch{flex-wrap:wrap}.issues-count{width:100%}}
    </style>
</asp:Content>
<asp:Content ID="IssuesContent" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="issues-page" ng-app="pmsIssuesApp" ng-controller="IssuesController" ng-cloak ng-click="closeEmployeeMenu($event)">
        <div class="issues-card">
            <div class="issues-titlebar">
                <h3 class="issues-title">Issues List</h3>
                <span class="issues-mode">{{isadmin==1 ? 'Admin - All Issues' : 'My Issues'}}</span>
            </div>
            <div class="issues-filterbar">
                <select aria-label="Filter by issue hierarchy type" ng-model="filter.EntityType" ng-options="x.Id as x.Name for x in entityTypes" ng-change="filterChanged()"></select>
                <div class="issues-employee">
                    <div class="issues-employee-trigger" role="combobox" tabindex="0" ng-click="openEmployeeMenu($event)" ng-keydown="($event.keyCode===13 || $event.keyCode===32) && openEmployeeMenu($event)" aria-haspopup="listbox" aria-expanded="{{employeeMenuOpen}}" aria-label="Filter by employee">
                        <span class="issues-employee-trigger-content">
                            <span class="issues-employee-chip" ng-repeat="employee in getSelectedEmployees() track by employee.Id" ng-click="$event.stopPropagation()">
                                <span>{{employee.Name}}</span><button type="button" aria-label="Remove {{employee.Name}}" ng-click="unselectEmployee(employee.Id,$event)">&times;</button>
                            </span>
                            <input id="employeeSearchInput" type="search" class="issues-employee-inline-search" ng-model="employeeSearch" ng-change="resetEmployeeActiveIndex()" ng-keydown="employeeSearchKeydown($event)" placeholder="Search employees..." autocomplete="off" aria-label="Search employees" ng-click="$event.stopPropagation();openEmployeeMenu($event)" ng-focus="openEmployeeMenu($event)" />
                        </span><span class="issues-employee-trigger-arrow" aria-hidden="true">&#9662;</span>
                    </div>
                    <div class="issues-employee-popup" ng-if="employeeMenuOpen" ng-click="$event.stopPropagation()">
                        <div class="issues-employee-error" ng-if="employeeError">{{employeeError}}</div>
                        <div class="issues-employee-options" ng-if="!employeeError">
                            <button type="button" role="option" id="employee-option-{{$index}}" class="issues-employee-option" ng-class="{'active':$index===employeeActiveIndex}" ng-repeat="employee in getFilteredEmployeeOptions() track by employee.Id" ng-click="toggleEmployee(employee.Id,$event)">
                                <span>{{employee.Name}}</span>
                            </button>
                            <div class="issues-employee-empty" ng-if="!getFilteredEmployeeOptions().length">{{selectedEmployeeIds.length === employeeOptions.length && employeeOptions.length ? 'All employees selected' : 'No matching employees'}}</div>
                        </div>
                        <div class="issues-employee-footer"><button type="button" class="issues-btn issues-btn-light" ng-click="clearEmployeeFilter()">Clear</button><button type="button" class="issues-btn" style="background:#7b0b69;color:#fff" ng-click="applyEmployeeFilter()">Apply</button></div>
                    </div>
                </div>
                <select aria-label="Filter by date" ng-model="filter.DateFilter" ng-options="x.Id as x.Name for x in dateFilters" ng-change="dateFilterChanged()"></select>
                <select aria-label="Filter by issue completion" ng-model="filter.IssueStatus" ng-options="x.Id as x.Name for x in issueStatusFilters" ng-change="filterChanged()"></select>
                <div class="issues-actions-top"><button type="button" class="issues-btn issues-btn-light" ng-click="clearFilters()" ng-disabled="loading">Clear Filters</button></div>
            </div>
            <div class="issues-custom-date" ng-if="filter.DateFilter=='CUSTOM'" style="display:flex;align-items:center;gap:8px;padding:9px 14px;border-bottom:1px solid #eceef1;background:#fcfcfd">
                <label style="margin:0;font-size:11px;color:#707782">From</label><input type="date" ng-model="filter.FromDate" />
                <label style="margin:0;font-size:11px;color:#707782">To</label><input type="date" ng-model="filter.ToDate" />
                <button type="button" class="issues-btn" style="background:#7b0b69;color:#fff" ng-click="filterChanged()">Apply</button>
            </div>
            <div class="issues-alert" ng-if="error">{{error}}</div>
            <div class="issues-gridsearch">
                <input type="search" ng-model="searchText" placeholder="Search issues on this page..." autocomplete="off" aria-label="Search issues" />
                <span class="issues-count" ng-if="totalRows>0">{{filteredRows.length}} visible / {{totalRows}} issues</span>
                <button type="button" class="issues-btn issues-btn-light" ng-click="searchText=''" ng-disabled="!searchText">Clear Search</button>
            </div>
            <div class="issues-tablewrap" ng-class="{'refreshing':loading && rows.length>0}" aria-busy="{{loading}}">
                <div class="issues-refreshline" ng-if="loading && rows.length>0"><span></span></div>
                <table class="issues-table">
                    <thead><tr><th>#</th><th>Sr. No.</th><th>Type</th><th>Issue</th><th>Owner</th><th>Team</th><th>Status</th><th>Priority</th><th>Assign To</th><th>Create Date</th><th>Target Date</th><th>Issue Type</th><th>Project</th><th>Milestone</th></tr></thead>
                    <tbody>
                        <tr ng-if="loading && rows.length==0"><td colspan="14" class="issues-empty"><span class="issues-loading"><span class="issues-spinner"></span> Loading issues...</span></td></tr>
                        <tr ng-repeat="row in filteredRows track by row.Id">
                            <td>{{((currentPage-1)*pageSize)+$index+1}}</td><td>{{row.HierarchySrNo || '-'}}</td>
                            <td><span class="issues-type" ng-class="entityClass(row.EntityType)">{{row.EntityType}}</span></td>
                            <td class="issues-name" title="{{row.Description}}">{{row.IssueName || ('Issue #'+row.Id)}}</td><td>{{row.OwnerName || '-'}}</td><td>{{row.TeamNames || '-'}}</td>
                            <td><span class="issues-status" ng-class="statusClass(row.IsComplete)">{{row.StatusName || '-'}}</span></td><td>{{row.PriorityName || '-'}}</td><td>{{row.AssigneeName || '-'}}</td><td>{{row.CreateDate || '-'}}</td><td>{{row.TargetDate || '-'}}</td><td>{{row.IssueTypeName || '-'}}</td><td>{{row.ProjectName || '-'}}</td><td>{{row.MilestoneName || '-'}}</td>
                        </tr>
                        <tr ng-if="!loading && totalRows==0"><td colspan="14" class="issues-empty">No issues found for the selected filters.</td></tr>
                        <tr ng-if="!loading && totalRows>0 && filteredRows.length==0"><td colspan="14" class="issues-empty">No issues match your search on this page.</td></tr>
                    </tbody>
                </table>
            </div>
            <div class="issues-paging" ng-if="totalRows>0">
                <div style="display:flex;align-items:center;gap:7px"><select ng-model="pageSize" ng-options="size for size in pageSizeOptions" ng-change="changePageSize()"></select><span>Records per page</span><span>{{startRow}}-{{endRow}} of {{totalRows}}</span></div>
                <div class="issues-pagebuttons"><button type="button" class="issues-pagebutton" ng-click="goToPage(currentPage-1)" ng-disabled="currentPage==1">Previous</button><button type="button" class="issues-pagebutton" ng-repeat="pageNo in pageNumbers track by pageNo" ng-class="{'active':pageNo==currentPage}" ng-click="goToPage(pageNo)">{{pageNo}}</button><button type="button" class="issues-pagebutton" ng-click="goToPage(currentPage+1)" ng-disabled="currentPage==totalPages">Next</button></div>
            </div>
        </div>
    </div>
</asp:Content>
