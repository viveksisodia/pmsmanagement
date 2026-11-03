<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="products_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>angular.module('myTeamTaskApp', []);</script>
    <link href="/pms/new/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/pms/new/assets/css/icons.min.css" rel="stylesheet" />
    <link href="/pms/new/assets/css/style.css" rel="stylesheet" />
    <script src="/pms/new/assets/js/moment.min.js"></script>
    <script src="/pms/new/assets/js/interact.min.js"></script>
    <script src="/pms/new/assets/js/chart.js"></script>
    <script src="/pms/myteamtask/app/myteam.service.js?v=<%=DateTime.Now.Ticks %>"></script>
    <script src="/pms/myteamtask/app/myteam.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <style>

body{background:#fff;font-size:13px;overflow-x:hidden}
.mt-page{background:#fff;min-height:0;padding:0}
#myTeamTaskToolbar{height:44px!important;min-height:44px!important;margin:0!important;padding:5px 16px!important;border:0!important;border-bottom:1px solid #dfe3e7!important;border-radius:0!important;background:#fff!important;box-shadow:none!important;display:flex!important;visibility:visible!important;opacity:1!important;align-items:center!important;justify-content:space-between!important;gap:12px!important;position:relative!important;z-index:25!important;overflow:visible!important}
#myTeamTaskToolbar .mt-toolbar-title{display:flex;align-items:center;gap:8px;white-space:nowrap;font-size:16px;color:#222;font-weight:600}
#myTeamTaskToolbar .mt-filters{display:flex!important;gap:5px;align-items:center;justify-content:flex-end;flex-wrap:nowrap;visibility:visible!important;position:relative}
#myTeamTaskToolbar .mt-filter-field{display:flex;flex-direction:column;gap:0}
#myTeamTaskToolbar .mt-filter-label{display:none}
#myTeamTaskToolbar select:not([chosen-select]),#myTeamTaskToolbar input{display:block!important;visibility:visible!important;opacity:1!important;height:34px!important;background-color:#fff!important;color:#222!important;border-radius:4px!important}
#myTeamTaskToolbar .project-quick-select{width:220px;min-width:220px;border:0!important;box-shadow:none!important;text-align:center;text-align-last:center;padding:4px 24px 4px 8px;background-color:transparent!important;font-size:13px}
/* Angular searchable project dropdown - no external Chosen dependency */
.project-search-native{position:absolute!important;width:1px!important;height:1px!important;min-width:1px!important;opacity:0!important;pointer-events:none!important;left:-9999px!important}
.angular-project-select{position:relative;width:100%;font-size:13px;z-index:80}
.angular-project-select-toggle{width:100%;height:34px;border:1px solid #dfe3e7;border-radius:4px;background:#fff;color:#222;display:flex;align-items:center;justify-content:space-between;gap:8px;padding:4px 8px 4px 10px;text-align:left;cursor:pointer}
.angular-project-select-label{display:block;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;flex:1}
.angular-project-select-arrow{font-size:18px;line-height:1;transition:transform .15s ease}
.angular-project-select.open .angular-project-select-arrow{transform:rotate(180deg)}
.angular-project-select-menu{display:none;position:absolute;top:calc(100% + 2px);left:0;right:0;background:#fff;border:1px solid #d7dde3;border-radius:4px;box-shadow:0 6px 16px rgba(30,32,37,.16);z-index:2000;padding:6px}
.angular-project-select.open .angular-project-select-menu{display:block}
.angular-project-search-wrap{position:relative;margin-bottom:5px}
.angular-project-search-wrap>i{position:absolute;left:8px;top:50%;transform:translateY(-50%);font-size:14px;color:#7b8490;pointer-events:none}
.angular-project-search-input{display:block!important;width:100%!important;min-width:0!important;height:32px!important;border:1px solid #cfd6dd!important;border-radius:4px!important;background:#fff!important;color:#222!important;padding:5px 8px 5px 28px!important;box-sizing:border-box!important;outline:none}
.angular-project-search-input:focus{border-color:#a8b4c0!important;box-shadow:0 0 0 2px rgba(128,69,116,.08)}
.angular-project-results{max-height:280px;overflow-y:auto;overflow-x:hidden}
.angular-project-option{padding:7px 9px;border-radius:3px;cursor:pointer;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;color:#222}
.angular-project-option:hover,.angular-project-option.active{background:#eceaf0}
.angular-project-option.selected{font-weight:600;background:#f3f0f4}
.angular-project-option.selected.active{background:#e7e1e8}
.angular-project-empty{padding:10px;color:#7b8490;text-align:center}
#myTeamTaskToolbar .angular-project-select{width:240px;min-width:240px}
#myTeamTaskToolbar .angular-project-select-toggle{border:0;background:transparent;height:34px}
#myTeamTaskToolbar .angular-project-select-menu{text-align:left}
#assignTaskModal .task-panel .angular-project-select{width:100%;margin-bottom:8px;z-index:2100}
#assignTaskModal .task-panel .angular-project-select-menu{z-index:2200}
#myTeamTaskToolbar .chosen-container,#myTeamTaskToolbar .chzn-container{width:240px!important;min-width:240px;font-size:13px;z-index:70}
#myTeamTaskToolbar .chosen-container-single .chosen-single,#myTeamTaskToolbar .chzn-container-single .chzn-single{height:34px;line-height:32px;border:0!important;border-radius:4px;background:#fff;box-shadow:none;padding:0 26px 0 9px;color:#222}
#myTeamTaskToolbar .chosen-container-single .chosen-single span,#myTeamTaskToolbar .chzn-container-single .chzn-single span{line-height:32px;text-align:left}
#myTeamTaskToolbar .chosen-container-single .chosen-single div b,#myTeamTaskToolbar .chzn-container-single .chzn-single div b{background-position:0 7px}
#myTeamTaskToolbar .chosen-container .chosen-drop,#myTeamTaskToolbar .chzn-container .chzn-drop{z-index:1200;text-align:left}
#myTeamTaskToolbar .chosen-container-single .chosen-search input,#myTeamTaskToolbar .chzn-container-single .chzn-search input{display:block!important;width:100%!important;height:32px!important;min-width:0!important;padding:5px 8px!important;border:1px solid #dfe3e7!important;border-radius:4px!important;box-sizing:border-box}
#myTeamTaskToolbar .chosen-container .chosen-results,#myTeamTaskToolbar .chzn-container .chzn-results{max-height:260px;text-align:left}

.filter-icon-btn{width:30px;height:30px;border:0;border-radius:6px;background:#f8f0f7;color:#804574;display:inline-flex;align-items:center;justify-content:center;font-size:18px;padding:0}
.filter-icon-btn:hover,.filter-icon-btn.active{background:#eadcea;color:#5b1747}
.quick-search{width:0;overflow:hidden;transition:width .2s ease}
.quick-search.open{width:190px}
.quick-search input{width:190px!important;min-width:190px!important;border:1px solid #dfe3e7!important;padding:5px 9px!important}
.filter-popover{position:absolute;right:34px;top:36px;width:230px;padding:10px;background:#fff;border:1px solid #dfe3e7;border-radius:5px;box-shadow:0 5px 14px rgba(30,32,37,.14);z-index:50}
.filter-popover label{display:block;font-size:11px;color:#667085;margin-bottom:4px}
.filter-popover input{width:100%!important;min-width:0!important;border:1px solid #dfe3e7!important;padding:5px 9px!important}
.more-menu{min-width:145px;font-size:13px}

#myTeamTaskToolbar .mt-date-control{display:flex;align-items:center;gap:4px}
#myTeamTaskToolbar .mt-date-control input{min-width:145px!important;width:145px!important}
#myTeamTaskToolbar .mt-date-control button{height:34px;width:34px;border:1px solid #cfd6dd;background:#f8f9fa;color:#333}

.mt-date-header{display:flex;align-items:center;justify-content:center;gap:3px;white-space:nowrap}
.mt-date-header button{border:0;background:transparent;padding:2px 4px;color:#111;font-size:19px}
.mt-date-header .calendar-wrap{position:relative;display:flex;align-items:center;gap:5px;color:#111;font-size:13px;min-width:105px;justify-content:center;cursor:pointer}
.mt-date-header .calendar-wrap i{color:#748095;font-size:15px}
.mt-date-header .calendar-date-input{position:absolute!important;width:1px!important;height:1px!important;opacity:0!important;pointer-events:none!important;left:-9999px!important;top:auto!important}

.mt-layout{display:grid;grid-template-columns:220px 175px minmax(0,1fr);height:calc(100vh - 260px);min-height:400px;max-height:510px;border:1px solid #dfe3e8;background:#fff;overflow:auto}
.mt-table{width:100%;border-collapse:collapse;table-layout:fixed}
.mt-table th,.mt-table td{height:50px;padding:2px 10px;white-space:nowrap;border-bottom:1px solid #e5e7eb;vertical-align:middle}
.mt-table th{background:#fff;font-weight:500;text-align:center}
.mt-left,.mt-summary{position:sticky;background:#fff;z-index:3}
.mt-left{left:0;border-right:1px solid #e5e7eb}
.mt-summary{left:220px;right:auto;border-left:0;border-right:1px solid #e5e7eb}

.employee{display:flex;align-items:center;gap:10px;padding:0}
.employee img{width:34px;height:34px;border-radius:50%;object-fit:cover}
.employee .name{font-size:14px;font-weight:500;color:#111827}
.emp-id{font-size:12px;color:#6b7280}

.timeline-scroll{overflow-x:auto;overflow-y:hidden}
.timeline-table{width:max-content;min-width:auto !important}
.time-head,.time-row{position:relative;min-width:0}
.time-head{height:50px;display:grid;border-bottom:1px solid #e5e7eb;background:#fff}
.time-head span{display:flex;align-items:center;justify-content:center;height:100%;text-align:center;border-right:1px solid #edf0f2;box-sizing:border-box;font-size:13px;color:#111}
.time-row{height:50px;border-bottom:1px solid #e5e7eb;background:#fff;overflow:hidden}
.shift-strip{position:absolute;top:12px;height:28px;border:1px solid #d6dde5;background:repeating-linear-gradient(to right,#fff 0,#fff 14px,#e3eaf0 14px,#e3eaf0 16px);z-index:1;overflow:hidden;box-sizing:border-box}
.planned-block{position:absolute;background:#bcccdc;border:1px solid #9eb4c7;border-radius:2px;overflow:hidden;white-space:nowrap;padding:0 4px;color:#273746;z-index:2;box-sizing:border-box}
.planned-block.availability{top:15px;height:22px;cursor:default}
.planned-view-pill{display:flex;align-items:center;gap:4px;height:18px;line-height:18px;max-width:100%;background:#fff;border-radius:3px;padding:0 6px;box-sizing:border-box;overflow:hidden;margin-top:1px}
.planned-view-pill:before{content:'\22EE';color:#9eb0bd;font-weight:bold;flex:0 0 auto;font-size:14px;line-height:18px}
.assignment-remove{cursor:pointer!important;display:inline-flex;align-items:center;justify-content:center;flex:0 0 auto}
.assignment-remove i{cursor:pointer!important}
.planned-title{display:block;min-width:0;overflow:hidden;text-overflow:ellipsis;font-size:11px;line-height:18px;color:#111827}
.tooltip.team-task-tooltip .tooltip-inner,.team-task-tooltip .tooltip-inner{text-align:left;width:300px;max-width:300px;background:#d9f2d9!important;color:#000!important;white-space:normal;line-height:1.45;padding:8px 10px}
.planned-block.pending{background:#d7c7f4}
.planned-block.changed{outline:2px solid #6f42c1}
.resize-handle{position:absolute;right:0;top:0;width:7px;height:100%;cursor:e-resize}

.mt-summary .mt-table th:nth-child(1),.mt-summary .mt-table td:nth-child(1){width:115px;padding-left:8px;padding-right:8px;text-align:center}
.mt-summary .mt-table th:nth-child(2),.mt-summary .mt-table td:nth-child(2){width:60px;text-align:center}
.mt-summary .mt-table th+th,.mt-summary .mt-table td+td{border-left:0!important}
.available-cell{text-align:center;font-size:14px;color:#111827}
.assign-btn{border:0;background:transparent;color:#3978de;font-size:20px}
.legend{height:42px;padding:5px 10px;display:flex;align-items:center;gap:20px;border-bottom:1px solid #ddd}
.legend i{font-size:14px}
.empty{padding:35px;text-align:center;color:#8795a1}

.team-card-view{height:calc(100vh - 260px);min-height:400px;max-height:510px;overflow:auto;padding:12px;background:#f7f9fb;border:1px solid #dfe3e8}
.team-card-date{display:flex;align-items:center;justify-content:space-between;margin-bottom:10px;background:#fff;border:1px solid #e5e7eb;padding:5px 10px}
.team-card-date .mt-date-header{height:34px}
.team-card-grid{display:grid;grid-template-columns:repeat(4,minmax(210px,1fr));gap:12px}
.team-member-card{background:#fff;border:1px solid #e1e6eb;border-radius:5px;min-height:180px;padding:12px;box-shadow:0 1px 3px rgba(30,41,59,.05)}
.team-card-head{display:flex;align-items:center;gap:9px;padding-bottom:10px;border-bottom:1px solid #edf0f2}
.team-card-head img{width:38px;height:38px;border-radius:50%;object-fit:cover}
.team-card-head .name{font-size:14px;font-weight:500;color:#111827}
.team-card-head .id{font-size:11px;color:#6b7280}
.team-card-availability{display:flex;align-items:center;justify-content:space-between;padding:10px 0;font-size:12px}
.team-card-availability strong{font-size:14px;font-weight:500}
.team-card-tasks{min-height:62px}
.team-task-chip{background:#cbd5e1;color:#273746;border-radius:2px;padding:5px 7px;margin-bottom:5px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;font-size:11px}
.team-card-footer{display:flex;justify-content:flex-end;border-top:1px solid #edf0f2;padding-top:7px}
.team-card-footer .assign-btn{line-height:24px}
@media(max-width:1200px){.team-card-grid{grid-template-columns:repeat(3,minmax(210px,1fr))}}
@media(max-width:850px){.team-card-grid{grid-template-columns:repeat(2,minmax(210px,1fr))}}

#assignTaskModal .modal-dialog{max-width:1140px;width:calc(100vw - 36px)}
#assignTaskModal .modal-content{height:503px;max-height:calc(100vh - 36px);border:0;border-radius:4px;overflow:hidden}
#assignTaskModal .modal-header{height:50px;min-height:50px;padding:10px 18px;border-bottom:1px solid #ddd}
#assignTaskModal .modal-title{font-size:18px;font-weight:500;color:#111}
#assignTaskModal .modal-body{min-height:0;overflow:hidden}
.assign-layout{display:grid;grid-template-columns:220px minmax(0,1fr) 280px;height:100%}
.assign-user-panel{background:#fff;border-right:1px solid #d9dde2}
.assign-user-head{height:50px;border-bottom:1px solid #d9dde2;display:flex;align-items:center;justify-content:center}
.assign-user-row{height:50px;border-bottom:1px solid #d9dde2;display:flex;align-items:center;gap:9px;padding:7px 12px}
.assign-user-row img{width:30px;height:30px;border-radius:50%;object-fit:cover}
.assign-user-row .name{font-size:13px;color:#333}
.assign-user-row .code{font-size:11px;color:#7d8793}
.assign-board{overflow:auto;background:#fff}
.assign-board .time-head{position:sticky;top:0;z-index:5;background:#fff}
.assign-help{padding:16px;color:#68758a;font-size:12px;border-top:1px solid #edf0f2}
.task-panel{padding:10px;overflow:auto;border-left:1px solid #d9dde2;background:#fff}
.task-panel .project-select{height:36px;margin-bottom:10px}
.task-panel .task-heading{font-size:13px;color:#333;margin:8px 0 6px;font-weight:600}
.task-card{display:flex;align-items:center;gap:7px;border:1px solid #b9d2e8;background:#fff;padding:5px 7px;margin:3px 0 5px;border-radius:2px;cursor:grab;min-height:32px;box-sizing:border-box}
.task-card .title{flex:1;min-width:0}
.task-card .title>div:first-child{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
.task-meta{font-size:11px;color:#77838f}
.assignment-tree{margin-top:6px;padding:0 2px 10px 0}
.tree-node{margin:0;padding:0;box-sizing:border-box}
.tree-row{width:100%;border:1px solid #c4d8ea;background:#fff;display:flex;align-items:center;gap:5px;min-height:30px;padding:4px 6px;text-align:left;color:#303640;border-radius:2px;font-size:12px;line-height:18px;box-sizing:border-box;margin-bottom:5px}
.tree-row:hover{background:#f7fbff;border-color:#9fc2df}
.tree-row i:first-child{width:15px;flex:0 0 15px;color:#536577;font-size:15px;text-align:center}
.tree-row .tree-icon{width:16px;flex:0 0 16px;color:#8b6f88;font-size:14px;text-align:center}
.tree-row .tree-name{min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;flex:1}
.tree-duration{margin-left:auto;white-space:nowrap;flex:0 0 auto;font-size:11px;font-weight:600;color:#2f3540}
.tree-subtask .tree-duration{font-weight:600;color:#2f3540}
.tree-project>.tree-row{font-weight:600;color:#2f3540;margin-left:0;width:100%}
.tree-project>.tree-row .tree-icon{color:#746173}
.tree-milestone{margin-left:0}
.tree-milestone>.tree-row{font-weight:600;width:100%}
.tree-task{margin-left:12px}
.tree-task>.tree-row{width:calc(100% - 12px);color:#414b56}
.tree-subtasks{margin-left:24px;padding:0}
.tree-subtask{position:relative;width:calc(100% - 24px)}
.tree-subtask .tree-name{min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;flex:1}
.tree-subtask .drag-icon{color:#6f7f8d;font-size:14px;flex:0 0 auto}
.tree-only-subtask-note{font-size:11px;line-height:16px;color:#7b8490;margin:4px 0 8px}
#assignTaskModal .external-subtask{user-select:none;-webkit-user-select:none}
#assignTaskModal .external-assign-parent{cursor:grab;user-select:none;-webkit-user-select:none;touch-action:none}
#assignTaskModal .external-assign-parent:active{cursor:grabbing}
#assignTaskModal .external-assign-parent.parent-drag-source{opacity:.55}
#assignTaskModal .external-subtask.subtask-drag-source{opacity:.38}
.myteam-subtask-drag-preview{position:fixed!important;z-index:2147483647!important;pointer-events:none!important;display:flex;align-items:center;gap:7px;width:220px;min-height:32px;padding:5px 7px;box-sizing:border-box;background:#fff;border:1px solid #9fc2df;border-radius:2px;box-shadow:0 8px 22px rgba(15,23,42,.20);color:#263746;font-size:12px;opacity:.96;overflow:hidden}
.myteam-subtask-drag-preview .drag-icon{color:#6f7f8d;font-size:14px;flex:0 0 auto}
.myteam-subtask-drag-preview .preview-text{min-width:0;flex:1;overflow:hidden}
.myteam-subtask-drag-preview .preview-title{font-size:12px;line-height:16px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
.myteam-subtask-drag-preview .preview-meta{font-size:10px;line-height:14px;color:#77838f;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
.loading-mask{position:fixed;inset:0;background:rgba(255,255,255,.65);z-index:2000;display:flex;align-items:center;justify-content:center}
.invalid-day{background:#fff4f4}
.toast-msg{position:fixed;right:20px;bottom:20px;z-index:3000;min-width:280px}

/* Page-only master-layout correction. Shared CSS files remain unchanged. */
#myTeamTaskRoot .mt-page{margin-top:44px!important}
#myTeamTaskRoot #myTeamTaskToolbar{position:relative!important;top:0!important;z-index:40!important}
#myTeamTaskRoot .mt-layout{align-items:stretch!important}
#myTeamTaskRoot .mt-left,#myTeamTaskRoot .timeline-scroll,#myTeamTaskRoot .mt-summary{align-self:start!important;height:auto!important;min-height:0!important}
#myTeamTaskRoot .mt-left .mt-table,#myTeamTaskRoot .mt-summary .mt-table,#myTeamTaskRoot .mt-left .mt-table tbody,#myTeamTaskRoot .mt-summary .mt-table tbody{height:auto!important;min-height:0!important}
#myTeamTaskRoot .mt-left .mt-table tr,#myTeamTaskRoot .mt-summary .mt-table tr{height:50px!important;min-height:50px!important;max-height:50px!important}
#myTeamTaskRoot .mt-left .mt-table th,#myTeamTaskRoot .mt-left .mt-table td,#myTeamTaskRoot .mt-summary .mt-table th,#myTeamTaskRoot .mt-summary .mt-table td{height:50px!important;min-height:50px!important;max-height:50px!important;padding-top:2px!important;padding-bottom:2px!important}
#myTeamTaskRoot .timeline-table,#myTeamTaskRoot .time-head,#myTeamTaskRoot .time-row{height:auto}
#myTeamTaskRoot .time-head,#myTeamTaskRoot .time-row{height:50px!important;min-height:50px!important;max-height:50px!important}
@media(max-width:1100px){#myTeamTaskToolbar{height:auto!important;min-height:44px!important;align-items:flex-start!important;flex-direction:column!important}#myTeamTaskToolbar .mt-filters{justify-content:flex-start}.mt-layout{grid-template-columns:210px 175px minmax(620px,1fr);height:calc(100vh - 275px)}.mt-summary{left:210px;right:auto}}

    
        /* Assign Task popup: keep the original Assign Projects style.
           This is intentionally scoped so the Manage Team Tasks overview display is unchanged. */
        #assignTaskModal .shift-strip{top:12px;height:26px;border:1px solid #e5e7eb;background:repeating-linear-gradient(to right,#fff 0,#fff 14px,#e5ecf1 14px,#e5ecf1 16px);overflow:hidden;box-sizing:border-box}
        #assignTaskModal .planned-block{top:12px;height:26px;background:#cbd5e1;border:0;border-radius:0;padding:4px 7px;color:#273746;cursor:move;box-sizing:border-box}
        #assignTaskModal .planned-view-pill{height:18px;line-height:18px;margin:0;background:#fff;border-radius:3px;padding:0 6px;box-sizing:border-box}
        #assignTaskModal .planned-title{line-height:18px;font-size:11px}

        /* Main page stays in the compact display requested for the overview. */
        #overviewScroller .shift-strip{top:12px;height:28px}
        #overviewScroller .planned-block.availability{top:15px;height:22px;padding:0 4px;border:1px solid #9eb4c7;border-radius:2px}
        #overviewScroller .planned-view-pill{height:18px;line-height:18px;margin-top:1px;padding:0 6px}

    .tooltip.team-task-tooltip{opacity:1!important}
.team-task-tooltip .tooltip-inner *{color:#000!important}
.team-task-tooltip .tooltip-inner strong{font-weight:700}
.team-task-tooltip.bs-tooltip-top .tooltip-arrow::before{border-top-color:#d9f2d9!important}
.team-task-tooltip.bs-tooltip-bottom .tooltip-arrow::before{border-bottom-color:#d9f2d9!important}
#overviewScroller .planned-block{cursor:move;touch-action:none}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="myTeamTaskRoot" ng-app="myTeamTaskApp" ng-controller="myTeamController as vm">
<div class="mt-page">
    <div id="myTeamTaskToolbar">
        <div class="mt-toolbar-title"><a href="/pms/headview/myteam.html" title="Back to Team"><i class="ri-arrow-left-line fs-5"></i></a><span>Manage Team Tasks</span></div>
        <div class="mt-filters">
            <select class="form-select form-select-sm project-quick-select"
                    aria-label="Project filter"
                    ng-model="vm.projectId"
                    project-search-select
                    project-search-options="vm.projects"
                    project-search-placeholder="Search project"
                    ng-options="p.Id as p.Name for p in vm.projects"
                    ng-change="vm.reload()"></select>
            <div class="quick-search" ng-class="{open:vm.showSearch}"><input class="form-control form-control-sm" ng-model="vm.search" placeholder="Search employee" /></div>
            <button type="button" class="filter-icon-btn" ng-class="{active:vm.showSearch}" title="Search employee" ng-click="vm.showSearch=!vm.showSearch"><i class="ri-search-line"></i></button>
            <button type="button" class="filter-icon-btn" ng-class="{active:vm.viewMode==='list'}" title="List view" ng-click="vm.viewMode='list'"><i class="ri-list-check"></i></button>
            <button type="button" class="filter-icon-btn" ng-class="{active:vm.viewMode==='grid'}" title="Grid view" ng-click="vm.viewMode='grid'"><i class="ri-layout-grid-fill"></i></button>
            <button type="button" class="filter-icon-btn" ng-class="{active:vm.showFilterPanel}" title="Filters" ng-click="vm.showFilterPanel=!vm.showFilterPanel"><i class="ri-filter-line"></i></button>
            <div class="filter-popover" ng-if="vm.showFilterPanel"><label>Employee</label><input class="form-control form-control-sm" ng-model="vm.search" placeholder="Search employee" /></div>
            <div class="dropdown">
                <button type="button" class="filter-icon-btn" title="More options" data-bs-toggle="dropdown"><i class="ri-more-fill"></i></button>
                <ul class="dropdown-menu dropdown-menu-end more-menu"><li><button class="dropdown-item" type="button" ng-click="vm.search='';vm.projectId=0;vm.reload()">Clear filters</button></li></ul>
            </div>
        </div>
    </div>

    <div class="mt-layout wrapperteamassign" ng-show="vm.viewMode==='list'">
        <section class="mt-left left-section">
            <table class="mt-table"><thead><tr><th><div class="mt-date-header"><button type="button" ng-click="vm.changeDate(-1)" title="Previous date"><i class="ri-arrow-left-s-line"></i></button><span class="calendar-wrap m-0" title="Select work date" ng-click="vm.openDatePicker($event)"><i class="ri-calendar-line"></i><span>{{vm.displayDate()}}</span></span><input class="calendar-date-input" type="date" ng-model="vm.selectedDate" ng-change="vm.onDateSelected()" /><button type="button" ng-click="vm.changeDate(1)" title="Next date"><i class="ri-arrow-right-s-line"></i></button></div></th></tr></thead><tbody>
                <tr ng-repeat="u in (vm.filteredUsers=(vm.users|filter:vm.search)) track by u.EmployeeId"><td>
                    <div class="employee"><img ng-src="{{u.Photo || '/pms/Image/user.jpg'}}" onerror="this.onerror=null;this.src='/pms/Image/user.jpg';"/><div><div class="name">{{u.Name}}</div><div class="emp-id id">{{u.EmployeeCode || u.EmployeeId}}</div></div></div>
                </td></tr><tr ng-if="!vm.filteredUsers.length"><td class="empty">No team member</td></tr>
            </tbody></table>
        </section>

        <section class="mt-summary right-section">
            <table class="mt-table"><thead><tr><th title="Employee shift hours minus assigned task hours">Available Hrs</th><th>Assign</th></tr></thead><tbody>
                <tr ng-repeat="u in vm.filteredUsers track by u.EmployeeId">
                    <td class="available-cell available-time" title="Shift: {{u.TotalShiftText}} | Left: {{u.AvailableText}}">{{u.AvailableText}}</td>
                    <td class="text-center assign"><button type="button" class="assign-btn m-0 p-0" title="Assign task" ng-disabled="!u.IsWorkingDay || u.AvailableMinutes<=0" ng-click="vm.openAssign(u)"><i class="ri-user-add-line"></i></button></td>
                </tr>
            </tbody></table>
        </section>

        <section class="timeline-scroll timeline-section" id="overviewScroller"><div class="timeline-table">
            <div class="time-head" ng-style="vm.headerGridStyle"><span ng-repeat="h in vm.hours track by $index">{{h}}</span></div>
            <div class="time-row" data-employee-id="{{u.EmployeeId}}" ng-repeat="u in vm.filteredUsers track by u.EmployeeId" ng-class="{'invalid-day':!u.IsWorkingDay}" ng-style="{'width':vm.timelineWidth+'px','min-width':vm.timelineWidth+'px'}">
                <div class="shift-strip time-grid" ng-if="u.IsWorkingDay" ng-style="vm.slotStyle(u.ShiftStart,u.ShiftEnd)"></div>
                <div class="planned-block availability" data-employee-id="{{u.EmployeeId}}" data-key="{{a.ClientKey}}" ng-repeat="a in u.Assignments track by a.ClientKey" ng-style="vm.slotStyle(a.StartTime,a.EndTime)" data-bs-toggle="tooltip" data-bs-html="true" data-bs-custom-class="team-task-tooltip" data-bs-placement="top" data-bs-title="{{a.TooltipHtml}}">
                    <span class="planned-view-pill"><span class="planned-title">{{a.Title}}</span></span>
                </div>
            </div>
        </div></section>
    </div>
    <div class="team-card-view" ng-if="vm.viewMode==='grid'">
        <div class="team-card-date"><strong>Team members</strong><div class="mt-date-header"><button type="button" ng-click="vm.changeDate(-1)" title="Previous date"><i class="ri-arrow-left-s-line"></i></button><span class="calendar-wrap" title="Select work date" ng-click="vm.openDatePicker($event)"><i class="ri-calendar-line"></i><span>{{vm.displayDate()}}</span></span><input class="calendar-date-input" type="date" ng-model="vm.selectedDate" ng-change="vm.onDateSelected()" /><button type="button" ng-click="vm.changeDate(1)" title="Next date"><i class="ri-arrow-right-s-line"></i></button></div></div>
        <div class="team-card-grid">
            <article class="team-member-card" ng-repeat="u in (vm.filteredUsers=(vm.users|filter:vm.search)) track by u.EmployeeId">
                <div class="team-card-head"><img ng-src="{{u.Photo || '/pms/Image/user.jpg'}}" onerror="this.onerror=null;this.src='/pms/Image/user.jpg';"/><div><div class="name">{{u.Name}}</div><div class="id">{{u.EmployeeCode || u.EmployeeId}}</div></div></div>
                <div class="team-card-availability"><span>Available Hrs</span><strong title="Shift: {{u.TotalShiftText}}">{{u.AvailableText}}</strong></div>
                <div class="team-card-tasks"><div class="team-task-chip" ng-repeat="a in u.Assignments track by a.ClientKey" title="{{a.TypeLabel}}: {{a.Title}} | {{a.StartTime}} - {{a.EndTime}}">{{a.Title}} · {{a.StartTime}} - {{a.EndTime}}</div><div class="empty p-2" ng-if="!u.Assignments.length">No assigned task</div></div>
                <div class="team-card-footer"><button type="button" class="assign-btn m-0 p-0" title="Assign task" ng-disabled="!u.IsWorkingDay || u.AvailableMinutes<=0" ng-click="vm.openAssign(u)"><i class="ri-user-add-line"></i></button></div>
            </article>
            <div class="empty" ng-if="!vm.filteredUsers.length">No team member</div>
        </div>
    </div>
    <div class="legend"><span><i class="ri-checkbox-blank-fill" style="color:#cbd9e5"></i> Planned</span><span><i class="ri-checkbox-blank-line" style="color:#bbb"></i> Available</span><%--<span><i class="ri-checkbox-blank-fill" style="color:#d7c7f4"></i> Pending change</span>--%></div>
</div>

<div class="modal fade" id="assignTaskModal" tabindex="-1">
 <div class="modal-dialog modal-dialog-centered modal-xl"><div class="modal-content">
  <div class="modal-header"><h5 class="modal-title">Assign Task</h5><button type="button" class="btn-close" data-bs-dismiss="modal"></button></div>
  <div class="modal-body p-0"><div class="assign-layout">
   <section class="assign-user-panel">
    <div class="assign-user-head"><div class="mt-date-header"><button type="button" ng-click="vm.changeDate(-1)" title="Previous date"><i class="ri-arrow-left-s-line"></i></button><span class="calendar-wrap" title="Select work date" ng-click="vm.openDatePicker($event)"><i class="ri-calendar-line"></i><span>{{vm.displayDate()}}</span></span><input class="calendar-date-input" type="date" ng-model="vm.selectedDate" ng-change="vm.onDateSelected()" /><button type="button" ng-click="vm.changeDate(1)" title="Next date"><i class="ri-arrow-right-s-line"></i></button></div></div>
    <div class="assign-user-row" ng-if="vm.activeUser"><img ng-src="{{vm.activeUser.Photo || '/pms/Image/user.jpg'}}" onerror="this.onerror=null;this.src='/pms/Image/user.jpg';"/><div><div class="name">{{vm.activeUser.Name}}</div><div class="code">{{vm.activeUser.EmployeeCode || vm.activeUser.EmployeeId}}</div></div></div>
   </section>
   <div class="assign-board">
    <div class="time-head" ng-style="vm.headerGridStyle"><span ng-repeat="h in vm.hours track by $index">{{h}}</span></div>
    <div class="time-row" id="assignRow" ng-if="vm.activeUser" ng-style="{'width':vm.timelineWidth+'px','min-width':vm.timelineWidth+'px'}">
      <div class="shift-strip" ng-style="vm.slotStyle(vm.activeUser.ShiftStart,vm.activeUser.ShiftEnd)"></div>
      <div class="planned-block" ng-repeat="a in vm.editAssignments track by a.ClientKey" ng-class="{pending:a.IsNew,changed:a.IsChanged}" ng-style="vm.slotStyle(a.StartTime,a.EndTime)" data-key="{{a.ClientKey}}" data-bs-toggle="tooltip" data-bs-html="true" data-bs-custom-class="team-task-tooltip" data-bs-placement="top" data-bs-title="{{a.TooltipHtml}}">
        <span class="planned-view-pill"><span class="planned-title">{{a.Title}}</span><span class="ms-auto assignment-remove" ng-click="vm.removeAssignment(a,$event)"><i class="ri-close-line text-danger"></i></span></span><span class="resize-handle"></span>
      </div>
    </div>
    <div class="assign-help">Drag a milestone, task or subtask from the right and drop it in available time. Milestone/Task assigns only its child subtasks.</div>
   </div>
   <aside class="task-panel">
    <select class="form-select form-select-sm project-select"
             ng-model="vm.modalProjectId"
             project-search-select
             project-search-options="vm.assignableProjects"
             project-search-placeholder="Search project"
             ng-options="p.Id as p.Name for p in vm.assignableProjects"
             ng-change="vm.loadAssignableTasks()"></select>

   <%-- <div class="task-heading">Available Subtasks</div>
    <div class="tree-only-subtask-note">Only Subtask can be dragged. Project, Milestone and Task are for hierarchy view only.</div>
  --%>  <input class="form-control form-control-sm mb-2"
           ng-model="vm.taskSearch"
           placeholder="Search project/milestone/task/subtask" />

    <div class="assignment-tree">
      <%-- Project remains the data/filter scope only. The visible hierarchy starts at Milestone. --%>
      <div class="tree-node tree-project"
           ng-repeat="p in vm.assignmentTree track by p.Id"
           ng-if="vm.projectTreeVisible(p)">

        <div class="tree-node tree-milestone"
             ng-repeat="m in p.Milestones track by m.Id"
             ng-if="vm.milestoneTreeVisibleInProject(m,p)">
          <button type="button"
                  class="tree-row external-assign-parent external-milestone"
                  data-type="milestone"
                  data-id="{{m.Id}}"
                  data-project-id="{{p.Id}}"
                  ng-click="vm.toggleTreeNode(m,$event)">
            <i ng-class="vm.treeExpanded(m)?'ri-arrow-down-s-line':'ri-arrow-right-s-line'"></i>
            <span class="tree-name"><%--{{m.OrderNo}} --%>{{m.Name}}</span>
            <span class="tree-duration" ng-if="m.Duration">({{vm.hierarchyDurationText(m.Duration)}} hrs)</span>
          </button>

          <div ng-show="vm.treeExpanded(m)">
            <div class="tree-node tree-task"
                 ng-repeat="t in m.Tasks track by t.Id"
                 ng-if="vm.taskTreeVisibleInContext(t,m,p)">
              <button type="button"
                      class="tree-row external-assign-parent external-task"
                      data-type="task"
                      data-id="{{t.Id}}"
                      data-project-id="{{p.Id}}"
                      ng-click="vm.toggleTreeNode(t,$event)">
                <i ng-class="vm.treeExpanded(t)?'ri-arrow-down-s-line':'ri-arrow-right-s-line'"></i>
                <span class="tree-name"><%--{{t.OrderNo}} --%>{{t.Name}}</span>
                <span class="tree-duration" ng-if="t.Duration">({{vm.hierarchyDurationText(t.Duration)}} hrs)</span>
              </button>

              <div class="tree-subtasks" ng-show="vm.treeExpanded(t)">
                <div class="task-card external-subtask tree-subtask"
                     ng-repeat="s in t.Subtasks track by s.Key"
                     ng-if="vm.subtaskTreeVisibleInContext(s,t,m,p)"
                     data-key="{{s.Key}}"
                     data-type="subtask"
                     data-id="{{s.EntityId}}"
                     data-project-id="{{p.Id}}"
                     title="Drag {{s.Name}} to {{vm.activeUser.Name}} free time">
                  <div class="title d-flex align-items-center w-100">
                    <span class="tree-name"><%--{{s.OrderNo}} --%>{{s.Name}}</span>
                    <span class="tree-duration" ng-if="s.Duration">({{vm.hierarchyDurationText(s.Duration)}} hrs)</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="empty" ng-if="!vm.allAvailableTasks.length">No assignable subtask</div>
   </aside>
  </div></div>
 </div></div>
</div>
<div class="loading-mask" ng-if="vm.loading"><div class="spinner-border text-primary"></div></div>
<div class="alert toast-msg" ng-class="vm.message.ok?'alert-success':'alert-danger'" ng-if="vm.message.text">{{vm.message.text}}</div>
</div>
</asp:Content>
