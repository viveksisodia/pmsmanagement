<%@ Page Title="PMS Manager Dashboard" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pms_manager_dashboard" MasterPageFile="~/pms/angularmaster/angular_head.master" %>
<asp:Content ID="ManagerDashboardHead" ContentPlaceHolderID="head" runat="server">
<script src="/pms/new/assets/js/chart.js"></script>
<style>
.manager-dashboard .md-report-link{font-size:11px;color:#3876dd;white-space:nowrap;margin-left:auto;margin-right:10px}
.manager-dashboard .md-tag-scroll,.manager-dashboard .md-task-chart-scroll{scrollbar-width:none;-ms-overflow-style:none}
.manager-dashboard .md-tag-scroll::-webkit-scrollbar,.manager-dashboard .md-task-chart-scroll::-webkit-scrollbar{display:none;width:0;height:0}
.manager-dashboard .md-tag-scroll{max-height:270px;overflow:auto}
.manager-dashboard .md-tag-scroll .badge{color:#000!important}
.manager-dashboard .md-tag-scroll thead th{position:sticky;top:0;z-index:1;background:#eaf2f6}
.manager-dashboard .md-task-chart-scroll{overflow-x:auto;overflow-y:hidden;width:100%;min-width:0}
.manager-dashboard .dashboard2>.card{min-width:0}
.manager-dashboard .md-loading-status{min-height:24px;margin-bottom:8px;display:flex;align-items:center;gap:8px}
.manager-dashboard .md-loading-spinner{display:inline-block;width:14px;height:14px;border:2px solid #d6e1e8;border-top-color:#3876dd;border-radius:50%;animation:md-loading-spin .75s linear infinite;margin-right:6px;vertical-align:middle}
.manager-dashboard.md-is-loading .card{opacity:.6;pointer-events:none}
.manager-dashboard .card{transition:opacity .15s ease}
@keyframes md-loading-spin{to{transform:rotate(360deg)}}
@media(prefers-reduced-motion:reduce){.manager-dashboard .md-loading-spinner{animation:none}.manager-dashboard .card{transition:none}}

        .manager-dashboard {
            background-color: #f1f5f7;
        }
    

        .manager-dashboard .dashboard1 {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 16px;
            margin-bottom: 16px;
        }

        .manager-dashboard .dashboard2 {
            display: grid;
            grid-template-columns: 1fr 2fr;
            gap: 16px;
            margin-bottom: 16px;
        }

        .manager-dashboard .dashboard3 {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 16px;
            margin-bottom: 16px;
        }

        .manager-dashboard .card {
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.08);
            padding:12px;
            margin:0px;
        }

        .manager-dashboard .card-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
            padding:0;
        }

            .manager-dashboard .card-header h3 {
                font-size: 16px;
                color: #000;
                margin:0;
            }

        .manager-dashboard select {
            padding: 3px 5px;
            border: 1px solid #ddd;
            border-radius: 0px;
            outline: none;
            background:#fff;
            cursor:pointer;
        }

        .manager-dashboard .chart-wrapper {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
        }

        .manager-dashboard .chart-box {
            width: 160px;
            height: 160px;
            position: relative;
            overflow: hidden;
        }

        .manager-dashboard .bar-box {
            height: 300px;
            position: relative;
            overflow: hidden;
        }

        .manager-dashboard canvas {
            display: block !important;
            width: 100% !important;
            height: 100% !important;
        }

        .manager-dashboard .legend {
            list-style: none;
            font-size: 14px;
            line-height: 1.9;
            padding: 0;
        }

            .manager-dashboard .legend li {
                display: flex;
                align-items: center;
                gap: 4px;
                font-size: 9px;
            }

        .manager-dashboard .dot {
            width: 6px;
            height: 6px;
            border-radius: 50%;
        }

        .manager-dashboard .wide {
            grid-template-columns: 1fr 2fr;
        }

        @media(max-width:1100px) {
            .manager-dashboard .dashboard1, .manager-dashboard .dashboard2, .manager-dashboard .dashboard3 {
                grid-template-columns: repeat(2,1fr);
            }

            .manager-dashboard .wide {
                grid-column: span 2;
            }
        }

        @media(max-width:767px) {
            .manager-dashboard .dashboard1, .manager-dashboard .dashboard2, .manager-dashboard .dashboard3 {
                grid-template-columns: 1fr;
            }

            .manager-dashboard .wide {
                grid-column: span 1;
            }

            .manager-dashboard .chart-wrapper {
                flex-direction: column;
            }
        }
    

.manager-dashboard .md-date{display:flex;align-items:center;gap:7px;position:relative;font-size:12px}
.manager-dashboard .md-range-trigger{height:32px;min-width:206px;padding:4px 9px;border:1px solid #d7dce2;border-radius:4px;background:#fff;color:#3d4650;font-size:11px;text-align:left;display:flex;align-items:center;justify-content:space-between;gap:9px}
.manager-dashboard .md-range-popup{position:absolute;z-index:1050;top:38px;right:0;width:142px;padding:8px;background:#fff;border:1px solid #dfe3e8;border-radius:5px;box-shadow:0 6px 20px rgba(28,39,50,.16)}
.manager-dashboard .md-range-popup.custom-open{width:342px;display:grid;grid-template-columns:132px minmax(0,1fr);gap:8px}
.manager-dashboard .md-presets{display:flex;flex-direction:column}
.manager-dashboard .md-preset{border:0;border-radius:0;background:#fff;color:#202633;padding:5px;text-align:left;font-size:11px;line-height:16px}
.manager-dashboard .md-preset:hover,.manager-dashboard .md-preset.active{background:#0788c9;color:#fff}
.manager-dashboard .md-custom label{display:block;color:#111;font-size:13px;font-weight:600;margin:0 0 4px}
.manager-dashboard .md-custom input{box-sizing:border-box;width:100%;height:34px;padding:4px 8px;margin:0 0 8px;border:1px solid #cfd5dc;border-radius:4px;background:#fff;font-size:12px}
.manager-dashboard .md-apply{width:100%;height:34px;border:0;border-radius:3px;background:#42133f;color:#fff;font-size:12px}
.manager-dashboard .md-range-error{color:#c0392b;font-size:11px;margin-top:7px}
@media(max-width:600px){.manager-dashboard .md-range-popup.custom-open{width:min(94vw,342px);grid-template-columns:122px minmax(0,1fr)}}
.manager-dashboard .md-employee{position:relative;display:inline-block;font-size:12px}
.manager-dashboard .md-employee-popup{position:absolute;top:36px;left:0;width:260px;max-width:90vw;z-index:1050;padding:8px;border:1px solid #dfe3e8;border-radius:5px;background:#fff;box-shadow:0 6px 20px rgba(28,39,50,.16)}
.manager-dashboard .md-employee-popup input{width:100%;box-sizing:border-box;padding:7px;border:1px solid #d7dce2;border-radius:4px;margin-bottom:6px}
.manager-dashboard .md-employee-list{max-height:230px;overflow:auto}
.manager-dashboard .md-employee-option{display:block;width:100%;padding:7px;text-align:left;background:#fff;border:0;font-size:12px}
.manager-dashboard .md-employee-option.active{background:#0788c9;color:#fff}
.manager-dashboard .md-pager{display:flex;align-items:center;justify-content:space-between;flex-wrap:wrap;gap:8px;padding-top:10px;margin-top:10px;border-top:1px solid #e5e7eb;font-size:12px}
.manager-dashboard .md-pager-controls{display:flex;align-items:center;gap:6px}
.manager-dashboard .md-pager button,.manager-dashboard .md-pager select{border:1px solid #cfd5dc;border-radius:4px;padding:4px 8px;background:#fff;color:#111}
.manager-dashboard .md-pager button:disabled{opacity:.45;cursor:default}
.manager-dashboard .md-list-search{width:210px;max-width:100%;padding:5px 8px;border:1px solid #cfd5dc;border-radius:4px;font-size:12px}
.manager-dashboard .md-team-avatars{display:flex;align-items:center;padding-left:0;min-width:85px}
.manager-dashboard .md-team-member{position:relative;display:inline-flex;cursor:default;margin-left:-8px}
.manager-dashboard .md-team-member:first-child{margin-left:0}
.manager-dashboard .md-team-member .avatar-xxs{width:26px;height:26px}
.manager-dashboard .md-team-member .avatar-title{display:flex;align-items:center;justify-content:center;width:26px;height:26px;border:2px solid #fff;color:#fff;font-size:10px;font-weight:600;box-sizing:border-box}
.manager-dashboard .md-team-member:hover,.manager-dashboard .md-team-member:focus{z-index:3}
.manager-dashboard .md-main-filter-row{display:flex;align-items:center;gap:10px;flex-wrap:wrap}
.manager-dashboard .md-project-dropdown{min-width:160px;max-width:240px}
.manager-dashboard .md-project-dropdown .md-range-trigger{width:100%}
.manager-dashboard #mdProjectsissue .md-employee-popup{left:auto;right:0;width:260px;max-width:calc(100vw - 32px);box-sizing:border-box}
.manager-dashboard #mdProjectsissue .md-employee-list{overflow-x:hidden;overscroll-behavior:contain}
.manager-dashboard #mdProjectsissue .md-employee-option{white-space:normal;overflow-wrap:anywhere}
.manager-dashboard .md-situation-scroll{height:auto!important;max-height:none!important;min-height:0!important;overflow-x:auto!important;overflow-y:hidden!important}
.md-dashboard-team-tooltip{position:fixed;z-index:10800;max-width:260px;padding:6px 9px;border-radius:4px;background:#222;color:#fff;white-space:normal;overflow-wrap:anywhere;font-size:12px;line-height:1.4;pointer-events:none;box-shadow:0 2px 8px rgba(0,0,0,.2)}
.manager-dashboard .md-milestone-track{display:flex!important;align-items:center;gap:0;min-width:0;flex-wrap:nowrap;height:12px!important}
.manager-dashboard .md-milestone-segment{flex:1 1 0!important;min-width:0!important;padding:0!important;height:12px!important;border-right:1px solid #777;background:transparent!important}
.manager-dashboard .md-milestone-segment:last-child{border-right:0}
.manager-dashboard .md-milestone-label{display:flex;justify-content:space-between;gap:4px;font-size:10px;line-height:16px;color:#333}
.manager-dashboard .md-milestone-count{font-size:10px;line-height:16px;color:#666}
.manager-dashboard .md-milestone-segment .progress{height:12px!important;margin:0!important;border-radius:0!important;background:#edf0f4}
.md-dashboard-team-tooltip{white-space:pre-line}
</style>
<script src="<%= ResolveUrl("~/pms/dashboard/app/dashboard.main.js") %>?v=<%= System.IO.File.GetLastWriteTimeUtc(Server.MapPath("~/pms/dashboard/app/dashboard.main.js")).Ticks %>"></script>
<script src="<%= ResolveUrl("~/pms/dashboard/app/dashboard.service.js") %>?v=<%= System.IO.File.GetLastWriteTimeUtc(Server.MapPath("~/pms/dashboard/app/dashboard.service.js")).Ticks %>"></script>
</asp:Content>
<asp:Content ID="ManagerDashboardContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main-wrapper manager-dashboard" ng-app="pmsManagerDashboardApp" ng-controller="ManagerDashboardController" ng-class="{'md-is-loading':projectLoading}" aria-busy="{{projectLoading}}">
        <div class="sub-header">
            <div class="row">
                <div class="col-xxl-3 col-lg-3 col-md-3 col-sm-12">
                    <div class="sub-left">
                        <div class="sub-page-title">Dashboard <small style="font-size:10px;color:#777;">
                            </small></div>
                    </div>
                </div>
                <div class="col-xxl-6 col-lg-6 col-md-6 col-sm-12">
                    <div class="md-main-filter-row"><div class="md-employee md-project-dropdown" id="mdProjectsmain" ng-click="$event.stopPropagation()">
                        <button type="button" class="md-range-trigger" ng-click="toggleProjectDropdown('main')" aria-expanded="{{projectDropdowns.main.open}}" aria-label="Main project filter"><span>{{projectDropdowns.main.selected.ProjectName}}</span><i class="ri-arrow-down-s-line"></i></button>
                        <div class="md-employee-popup" ng-if="projectDropdowns.main.open" ng-keydown="projectKeydown('main',$event)">
                            <input id="mdProjectSearchmain" type="text" aria-label="Search projects" placeholder="Search projects..." ng-model="projectDropdowns.main.search" ng-change="filterProjectOptions('main')" autocomplete="off" />
                            <div class="md-employee-list" role="listbox"><button type="button" role="option" class="md-employee-option" ng-repeat="project in projectDropdowns.main.options track by project.id" ng-class="{'active':$index===projectDropdowns.main.index}" ng-mouseenter="projectDropdowns.main.index=$index" ng-click="selectProjectOption('main',project)">{{project.ProjectName}}</button><div ng-if="!projectDropdowns.main.options.length">No projects found</div></div>
                        </div></div>
                    <div class="md-employee" ng-click="$event.stopPropagation()" style="display:none;">
                        <button type="button" class="md-range-trigger" ng-click="toggleEmployees()" ng-disabled="employeeLoading || employeeError" aria-expanded="{{employeeOpen}}"><span>{{employeeLoading ? 'Loading employees...' : selectedEmployee.name}}</span><i class="ri-arrow-down-s-line"></i></button>
                        <div class="md-employee-popup" ng-if="employeeOpen" ng-keydown="employeeKeydown($event)">
                            <input id="mdEmployeeSearch" type="text" aria-label="Search employee" placeholder="Search employee..." ng-model="employeeState.search" ng-change="filterEmployees(employeeState.search)" autocomplete="off" />
                            <div class="md-employee-list" role="listbox"><button type="button" role="option" aria-selected="{{selectedEmployee.id===employee.id}}" class="md-employee-option" ng-repeat="employee in employeeOptions track by employee.id" ng-class="{'active':$index===employeeState.index}" ng-mouseenter="employeeState.index=$index" ng-click="selectEmployee(employee)">{{employee.name}}</button><div ng-if="!employeeOptions.length">No employees found</div></div>
                        </div>
                        <span class="text-danger" ng-if="employeeError">{{employeeError}}</span>
                    </div>
                    </div>
                </div>
                <div class="col-xxl-3 col-lg-3 col-md-3 col-sm-12">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                        <div class="md-date" ng-click="$event.stopPropagation()">
                            <i class="ri-calendar-line"></i>
                            <button type="button" class="md-range-trigger" ng-click="rangeMenuOpen=!rangeMenuOpen" aria-haspopup="true" aria-expanded="{{rangeMenuOpen}}"><span>{{rangeDisplay}}</span><i class="ri-arrow-down-s-line"></i></button>
                            <div class="md-range-popup" ng-if="rangeMenuOpen" ng-class="{'custom-open':customMode}">
                                <div class="md-presets"><button type="button" ng-repeat="preset in datePresets" class="md-preset" ng-class="{'active':datePreset===preset}" ng-click="selectPreset(preset)">{{preset === 'Custom' ? 'Custom Range' : preset}}</button></div>
                                <div class="md-custom" ng-if="customMode">
                                    <label for="mdFrom">From Date</label><input id="mdFrom" type="date" ng-model="dateState.from" ng-click="openCalendar($event)" />
                                    <label for="mdTo">To Date</label><input id="mdTo" type="date" ng-model="dateState.to" ng-click="openCalendar($event)" />
                                    <button type="button" class="md-apply" ng-click="applyCustomRange()">Apply</button>
                                    <div class="md-range-error" ng-if="dateError">{{dateError}}</div>
                                </div>
                            </div>
                        </div>
                        <div class="btn-group d-block">
                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="sub-soft-btn"><i class="ri-more-fill"></i></a>
                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                <ul class='sttus-list'>
                                    <li><a class='' href='#'><i class="bx bx-download fs-16 align-middle me-1"></i> Download Report</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapper_inner p-3">
            <div class="md-loading-status">
                <span ng-if="projectLoading" role="status" aria-live="polite"><span class="md-loading-spinner" aria-hidden="true"></span>Updating dashboard...</span>
                <span class="text-danger" role="alert" ng-if="projectError">{{projectError}}</span><span class="text-danger" ng-if="chartError">{{chartError}}</span>
            </div>
            <div class="dashboard1">
                <!-- Projects -->
                <div class="card">
                    <div class="card-header">
                        <h3>Projects</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('projects',projectState.group)}}">View Report</a>
                        <select ng-model="projectState.group" ng-change="bindProjectGroups()"><option>Status</option><option>Priority</option><option>Situation</option></select>
                    </div>
                    <div class="chart-wrapper">
                        <div class="chart-box">
                            <canvas id="projectChart"></canvas>
                        </div>
                        <ul class="legend"><li ng-repeat="g in projectLegend track by $index"><span class="dot" ng-style="{'background':g.color}"></span> {{g.count}} {{g.name}} ({{g.percentage}}%)</li><li ng-if="!projects.length && !projectLoading">No projects found.</li></ul>
                    </div>
                </div>

                <!-- Milestones -->
                <div class="card">
                    <div class="card-header">
                        <h3>Milestones</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('milestones',itemState.Milestone)}}">View Report</a>
                        <select ng-model="itemState.Milestone" ng-change="bindItemCharts()"><option>Status</option><option>Priority</option><option>Situation</option></select>
                    </div>
                    <div class="chart-wrapper">
                        <div class="chart-box">
                            <canvas id="milestoneChart"></canvas>
                        </div>
                        <ul class="legend"><li ng-repeat="g in itemLegends.Milestone track by $index"><span class="dot" ng-style="{'background':g.color}"></span> {{g.count}} {{g.name}} ({{g.percentage}}%)</li></ul>
                    </div>
                </div>

                <!-- Task -->
                <div class="card">
                    <div class="card-header">
                        <h3>Task</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('tasks',itemState.Task)}}">View Report</a>
                        <select ng-model="itemState.Task" ng-change="bindItemCharts()"><option>Status</option><option>Priority</option><option>Situation</option></select>
                    </div>
                    <div class="chart-wrapper">
                        <div class="chart-box">
                            <canvas id="taskChart"></canvas>
                        </div>
                        <ul class="legend"><li ng-repeat="g in itemLegends.Task track by $index"><span class="dot" ng-style="{'background':g.color}"></span> {{g.count}} {{g.name}} ({{g.percentage}}%)</li></ul>
                    </div>
                </div>

                <!-- Subtask -->
                <div class="card">
                    <div class="card-header">
                        <h3>Subtask</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('subtasks',itemState.Subtask)}}">View Report</a>
                        <select ng-model="itemState.Subtask" ng-change="bindItemCharts()"><option>Status</option><option>Priority</option><option>Situation</option></select>
                    </div>
                    <div class="chart-wrapper">
                        <div class="chart-box">
                            <canvas id="subtaskChart"></canvas>
                        </div>
                        <ul class="legend"><li ng-repeat="g in itemLegends.Subtask track by $index"><span class="dot" ng-style="{'background':g.color}"></span> {{g.count}} {{g.name}} ({{g.percentage}}%)</li></ul>
                    </div>
                </div>
            </div>
            <div class="dashboard2">
                <!-- Bar Chart -->
                <div class="card">
                    <div class="card-header">
                        <h3>Project Percentage Completion</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('completion','Completion')}}">View Report</a>
                    </div>
                    <div class="bar-box" style="height:200px;">
                        <canvas id="barChart"></canvas>
                    </div>
                </div>

                <!-- Priority Chart -->
                <div class="card">
                    <div class="card-header">
                        <h3>Project with Milestone</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('milestoneprogress','Project')}}">View Report</a>
                        <input type="search" class="md-list-search" aria-label="Search projects and milestones" placeholder="Search projects or milestones..." ng-model="listSearch.milestone" ng-change="refreshPage('milestone',true)" />
                    </div>
                    <div class="mt-2">
                        <div class="project-wrapper" ng-repeat="p in milestonePageRows track by p.id">
                            <div class="project-name-in">{{p.ProjectName}}</div>
                            <div class="pro-progress-bar md-milestone-track">
                                <div class="step-milestone md-milestone-segment" ng-repeat="m in p.milestones track by m.id" tabindex="0" ng-mouseenter="showTeamTooltip($event,m.Tooltip)" ng-mouseleave="hideTeamTooltip()" ng-focus="showTeamTooltip($event,m.Tooltip)" ng-blur="hideTeamTooltip()" aria-label="{{m.Tooltip}}">
                                    
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="{{m.Completion}}" ng-style="{'width':m.Completion+'%','background':m.Color}"></div>
                                    </div>
                                    
                                </div>
                                <span ng-if="!p.milestones.length">No milestones</span>
                            </div>
                        </div>
                        <div ng-if="!milestonePageRows.length && !projectLoading">No projects found.</div>
                    </div>
                    <div class="md-pager" aria-label="Milestone projects pagination">
                        <span>Showing {{paging.milestone.from || 0}} to {{paging.milestone.to || 0}} of {{paging.milestone.total}}</span>
                        <div class="md-pager-controls"><label for="mdSizemilestone">Rows</label><select id="mdSizemilestone" ng-model="paging.milestone.size" ng-options="size for size in pageSizes" ng-change="refreshPage('milestone',true)"></select>
                        <button type="button" ng-click="goPage('milestone',paging.milestone.page-1)" ng-disabled="projectLoading || paging.milestone.page<=1">Previous</button>
                        <span aria-live="polite">Page {{paging.milestone.page}} of {{paging.milestone.pages}}</span>
                        <button type="button" ng-click="goPage('milestone',paging.milestone.page+1)" ng-disabled="projectLoading || paging.milestone.page>=paging.milestone.pages">Next</button></div>
                    </div>
                </div>
            </div>
            <div class="dashboard3">
                <div class="card">
                    <div class="card-header">
                        <h3>Projects by Situation</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('situation','Situation')}}">View Report</a>
                        <input type="search" class="md-list-search" aria-label="Search projects by situation" placeholder="Search projects, owner, team..." ng-model="listSearch.situation" ng-change="refreshPage('situation',true)" /><select ng-model="projectState.situationId" ng-options="s.id as s.Name for s in situationOptions" ng-change="bindCriticalProjects()"><option value="">All Situations</option></select>
                    </div>
                    <div class="table-responsive md-situation-scroll">
                        <table class="table align-middle tbl-tag mt-1 mb-1">
                            <thead>
                                <tr>
                                    <th>Project Name</th>
                                    <th>Project Manager</th>
                                    <th>Team</th>
                                    <th>Deadline</th>
                                    <th>Completion %</th>
                                    <th>Situation</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="p in situationPageRows track by p.id">
                                    <td style="font-weight:600;">{{p.ProjectName}}</td><td>{{p.OwnerName}}</td><td><div class="mile_st"><div class="avatar-group md-team-avatars">
                                        <span class="avatar-group-item material-shadow md-team-member" ng-repeat="member in p.teamAvatars | limitTo:3 track by $index" tabindex="0" aria-label="{{member.name}}" ng-mouseenter="showTeamTooltip($event,member.name)" ng-mouseleave="hideTeamTooltip()" ng-focus="showTeamTooltip($event,member.name)" ng-blur="hideTeamTooltip()">
                                            <span class="avatar-xxs"><span class="avatar-title rounded-circle" ng-style="{'background':member.color}">{{member.initials}}</span></span>
                                        </span>
                                        <span class="avatar-group-item material-shadow md-team-member" ng-if="p.teamAvatars.length>3" tabindex="0" aria-label="{{p.moreTeamNames}}" ng-mouseenter="showTeamTooltip($event,p.moreTeamNames)" ng-mouseleave="hideTeamTooltip()" ng-focus="showTeamTooltip($event,p.moreTeamNames)" ng-blur="hideTeamTooltip()"><span class="avatar-xxs"><span class="avatar-title rounded-circle bg-primary">+{{p.teamAvatars.length-3}}</span></span></span>
                                        <span ng-if="!p.teamAvatars.length">-</span>
                                    </div></div></td><td>{{p.Deadline ? p.Deadline.split('-').reverse().join('/') : '-'}}</td>
                                    <td>{{p.Completion | number:1}}%<div class="progress progress-md"><div class="progress-bar bg-warning" ng-style="{'width':p.Completion+'%'}"></div></div></td>
                                    <td ng-style="{'color':p.SituationColor,'font-weight':600}">{{p.SituationName}}</td>
                                </tr><tr ng-if="!situationPageRows.length && !projectLoading"><td colspan="6">No projects found.</td></tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="md-pager" aria-label="Situation projects pagination">
                        <span>Showing {{paging.situation.from || 0}} to {{paging.situation.to || 0}} of {{paging.situation.total}}</span>
                        <div class="md-pager-controls"><label for="mdSizesituation">Rows</label><select id="mdSizesituation" ng-model="paging.situation.size" ng-options="size for size in pageSizes" ng-change="refreshPage('situation',true)"></select>
                        <button type="button" ng-click="goPage('situation',paging.situation.page-1)" ng-disabled="projectLoading || paging.situation.page<=1">Previous</button>
                        <span aria-live="polite">Page {{paging.situation.page}} of {{paging.situation.pages}}</span>
                        <button type="button" ng-click="goPage('situation',paging.situation.page+1)" ng-disabled="projectLoading || paging.situation.page>=paging.situation.pages">Next</button></div>
                    </div>
                </div>
                <!-- Issues -->
                <div class="card">
                    <div class="card-header">
                        <h3>Issues</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('issues','Status')}}">View Report</a>
                        <div class="md-employee md-project-dropdown" id="mdProjectsissue" ng-click="$event.stopPropagation()">
                        <button type="button" class="md-range-trigger" ng-click="toggleProjectDropdown('issue')" aria-expanded="{{projectDropdowns.issue.open}}" aria-label="Issues project filter"><span>{{projectDropdowns.issue.selected.ProjectName}}</span><i class="ri-arrow-down-s-line"></i></button>
                        <div class="md-employee-popup" ng-if="projectDropdowns.issue.open" ng-keydown="projectKeydown('issue',$event)">
                            <input id="mdProjectSearchissue" type="text" aria-label="Search projects" placeholder="Search projects..." ng-model="projectDropdowns.issue.search" ng-change="filterProjectOptions('issue')" autocomplete="off" />
                            <div class="md-employee-list" role="listbox"><button type="button" role="option" class="md-employee-option" ng-repeat="project in projectDropdowns.issue.options track by project.id" ng-class="{'active':$index===projectDropdowns.issue.index}" ng-mouseenter="projectDropdowns.issue.index=$index" ng-click="selectProjectOption('issue',project)">{{project.ProjectName}}</button><div ng-if="!projectDropdowns.issue.options.length">No projects found</div></div>
                        </div></div>
                    </div>

                    <div class="chart-wrapper">
                        <div class="chart-box">
                            <canvas id="issuesChart"></canvas>
                        </div>

                        <ul class="legend"><li ng-repeat="g in issueLegend track by g.name"><span class="dot" ng-style="{'background':g.color}"></span> {{g.count}} {{g.name}}</li></ul>
                    </div>
                </div>
            </div>
            <div class="dashboard2">
                <!-- Bar Chart -->
                <div class="card">
                    <div class="card-header">
                        <h3>Task Tag</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('tags','Tag')}}">View Report</a>
                    </div>
                    <div class="table-responsive md-tag-scroll" tabindex="0" role="region" aria-label="Task tags">
                        <table class="table align-middle tbl-tag mt-1 mb-1">
                            <thead>
                                <tr>
                                    <th>Tag Name</th>
                                    <th>Number of Items</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="tag in tags track by tag.id">
                                    <td><span class="badge priority" ng-style="{'background':tag.Color}">{{tag.Name}}</span></td>
                                    <td>{{tag.Count}}</td>
                                </tr>
                                <tr ng-if="!tags.length && !projectLoading"><td colspan="2">No tags found.</td></tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <!-- Horizontal Progress -->
                <div class="card">
                    <div class="card-header">
                        <h3>Project {{itemState.taskProjectGroup}}</h3><a class="md-report-link" target="_blank" rel="noopener noreferrer" ng-href="{{reportUrl('taskprojects',itemState.taskProjectGroup)}}">View Report</a>
                        <select ng-model="itemState.taskProjectGroup" ng-change="bindItemCharts()"><option>Priority</option><option>Status</option></select>
                    </div>
                    <div class="md-task-chart-scroll" tabindex="0" role="region" aria-label="Total tasks chart"><div class="bar-box" style="height:230px;" ng-style="{'width':taskChartWidth+'px','min-width':'100%'}">
                        <canvas id="priorityChart"></canvas>
                    </div></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
