<%--<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="products_default" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/ang_popupmaster.master" AutoEventWireup="true" CodeFile="defaultnew.aspx.cs" Inherits="products_defaultnew" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script src="app/project.main.js?v=<%=DateTime.Now.Ticks %>"></script> 
    <script src="app/project.service.js?v=<%=DateTime.Now.Ticks %>"></script> 
    	<script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/fixedColumns.dataTables.js"></script>
    <style>
        .gantt-row-drag-handle{
    cursor:grab;
    color:#64748b;
    display:inline-flex;
    align-items:center;
    justify-content:center;
    width:18px;
    height:18px;
    margin-right:6px;
    user-select:none;
    -webkit-user-drag:none;
    touch-action:none;
}
.gantt-row-drag-handle:active{cursor:grabbing;}
.gantt-row-drag-handle *{pointer-events:none;}
.gantt-row-moving{opacity:.45;}
.gantt-row-over{background:#dbeafe !important;box-shadow:inset 0 3px 0 #2563eb;}
.gantt-row-invalid{background:#fee2e2 !important;box-shadow:inset 0 3px 0 #dc2626;}
.gantt-soft-ghost.invalid-drop{border-color:#dc2626;background:#fff1f2;color:#b91c1c;}
.gantt-soft-ghost{position:fixed;z-index:2147483647;pointer-events:none;opacity:.90;background:#fff;border:1px solid #b6c2d2;box-shadow:0 12px 30px rgba(15,23,42,.22);border-radius:7px;padding:7px 12px;font-size:12px;min-width:230px;max-width:420px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;}
body.gantt-soft-dragging{cursor:grabbing;user-select:none;}
    </style>
    <style>
        .small-desc-popup {
    font-size: 12px !important;
    border-radius: 8px !important;
}

.small-desc-title {
    font-size: 16px !important;
    padding: 5px 0 !important;
}

.small-desc-input {
    min-height: 80px !important;
    font-size: 12px !important;
}

.small-desc-btn {
    font-size: 12px !important;
    padding: 5px 12px !important;
}
    </style>
    <style>
        .gantt-bar {
    position: relative;
    cursor: move;
    user-select: none;
}

.resize-handle {
    position: absolute;
    top: 0;
    width: 8px;
    height: 100%;
    z-index: 5;
    cursor: ew-resize;
    background: rgba(0,0,0,0.15);
}

.resize-handle.left {
    left: 0;
}

.resize-handle.right {
    right: 0;
}
    </style>
    <style>
    .duration-box {
    display: flex;
    align-items: center;
    width: 100%;
    height: 28px;
    overflow: hidden;
    justify-content:space-around;
}

/* Inputs */
.duration-input {
    border: none !important;
    outline: none;
    height: 100%;
    font-size: 13px;
    background: transparent;
}
.duration-input:hover {border-bottom:1px solid #ddd !important;}
.duration-input:focus {border-bottom:1px solid #72075d !important;}
/* Left side */
.duration-input.left {
    width:49%;
    padding-right: 6px;
    text-align:right;
}

/* Right side */
.duration-input.right {
    width: 49%;
    padding-left: 6px;
}

/* Center slash */
.sls_2 {
    width: 10px;
    text-align: center;
}
    </style>
<script src="/pms/assets/js/dataTables.fixedColumns.min.js"></script>
   <style>
       .custom-popup {
    position: absolute;
    z-index: 99999;
}

.popup-box {
    background: #fff;
    border: 1px solid #ddd;
    padding: 8px;
    border-radius: 6px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    display: flex;
    gap: 10px;
}
   </style>
    <style>
        /*dropdown*/
        .no-caret::after {
    display: none !important;
}
        /*clander*/
       input[type="date"] {
    -webkit-appearance: none;
    -moz-appearance: textfield;
    appearance: none;
}
@-moz-document url-prefix() {
    input[type="date"] {
        color: transparent;
        text-shadow: 0 0 0 #000;
    }

    input[type="date"]::-moz-focus-inner {
        border: 0;
    }
}
input[type="date"]::-webkit-inner-spin-button,
input[type="date"]::-webkit-calendar-picker-indicator {
    display: none;
    -webkit-appearance: none;
}
    </style>
    <style>
       
.gantt-page-wrap{
    background:#fff;
    border:1px solid #dfe3ea;
    border-radius:10px;
    overflow:hidden;
}
.gantt-toolbar{
    display:flex;
    justify-content:space-between;
    align-items:center;
    padding:10px 12px;
    border-bottom:1px solid #e9edf3;
    background:#fafbfd;
}
.gantt-shell{
    display:flex;
    min-height:600px;
}
.gantt-left-panel{
    width:460px;
    min-width:460px;
    border-right:1px solid #e9edf3;
    background:#fff;
}
.gantt-right-panel{
    position: relative;
    overflow: auto;
    width: calc(100% - 460px);
    background: #fff;
}

.gantt-today-line{
    position: absolute;
    top: 84px; /* below 2 headers */
    bottom: 0;
    width: 2px;
    background: red;
    z-index: 20;
    pointer-events: none;
}
.gantt-left-head,
.gantt-left-row{
    display:grid;
    grid-template-columns: 1.8fr 1fr .9fr .6fr;
    align-items:center;
    min-height:42px;
    border-bottom:1px solid #eef1f5;
}
.gantt-left-head{
    background:#f7f9fc;
    font-weight:600;
    position:sticky;
    top:0;
    z-index:3;
}
.gantt-col-task,
.gantt-col-owner,
.gantt-col-status,
.gantt-col-progress{
    padding:8px 10px;
    font-size:12px;
}
.gantt-task-name{
    display:flex;
    align-items:center;
    gap:6px;
    white-space:nowrap;
    overflow:hidden;
    text-overflow:ellipsis;
}
.gantt-task-name.level-0{ padding-left:0; font-weight:600; }
.gantt-task-name.level-1{ padding-left:18px; }
.gantt-task-name.level-2{ padding-left:36px; }
.gantt-expand{
    cursor:pointer;
    width:16px;
    display:inline-flex;
    justify-content:center;
}
.gantt-expand-empty{
    width:16px;
    display:inline-block;
}
.gantt-type-badge{
    font-size:10px;
    text-transform:capitalize;
    padding:2px 6px;
    border-radius:10px;
    background:#eef3ff;
}
.gantt-type-badge.milestone{ background:#e8f1ff; color:#2563eb; }
.gantt-type-badge.task{ background:#ecfdf3; color:#16a34a; }
.gantt-type-badge.subtask{ background:#fff7ed; color:#ea580c; }

.gantt-status-pill{
    color:#fff;
    padding:3px 8px;
    border-radius:12px;
    font-size:11px;
    display:inline-block;
    min-width:60px;
    text-align:center;
}
.gantt-mini-user{
    display:inline-flex;
    align-items:center;
    justify-content:center;
    width:22px;
    height:22px;
    background:#e9eef8;
    font-size:10px;
    font-weight:600;
}

.gantt-header-top,
.gantt-header-bottom{
    display:flex;
    position:sticky;
    top:0;
    z-index:4;
    background:#fff;
    height:21px;
}
.gantt-header-top{
    border-bottom:0;
    background:#f7f9fc;
}
.gantt-header-bottom{
    border-bottom:0;
    background:#fbfcfe;
}
.gantt-header-group,
.gantt-cell-head{
    min-width:40px;
    text-align:center;
    padding:1px 4px;
    border-right:1px solid #edf1f5;
    font-size:11px;
    white-space:nowrap;
}
.gantt-body-row{
    position:relative;
    min-height:47px;
    border-bottom:1px solid #eef1f5;
}
.gantt-grid{
    display:flex;
    height:42px;
}
.gantt-grid-cell{
    height:47px;
    border-right:1px solid #f1f3f7;
    flex:0 0 auto;
}
.gantt-bar-wrap{
    position:absolute;
    top:11px;
    height:26px;
}
.gantt-bar{
    position:relative;
    height:26px;
    border-radius:6px;
    overflow:hidden;
    background:#60a5fa;
    box-shadow: inset 0 0 0 1px rgba(0,0,0,.05);
}
.gantt-bar.milestone{ background:#3b82f6; }
.gantt-bar.task{ background:#22c55e; }
.gantt-bar.subtask{ background:#f59e0b; }
.gantt-bar-progress{
    position:absolute;
    left:0;
    top:0;
    bottom:0;
    background:rgba(0,0,0,.18);
}
.gantt-bar-label{
    position:relative;
    z-index:2;
    display:block;
    color:#fff;
    font-size:11px;
    line-height:26px;
    padding:0 8px;
    white-space:nowrap;
    overflow:hidden;
    text-overflow:ellipsis;
}
.row-milestone{ background:#fcfdff; }

.tbl_view tr td {padding:0 !important}
.tbl_view tr td .progress-xl {
    height: 30px;
    border-radius:0px !important;
}
.tbl_view tr td .datepicker {line-height:30px;border:0 !important;padding:0 4px;}
.tbl_view tr td .dayswrapper {padding:6px;}
</style>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <!-- start page title -->
<div style="padding:0 15px;">
                        
<!-- end page title -->
<div class="customer_wrapper customer_form" ng-app="PMSerp" ng-controller="projectCtrl">
<div id="toast" class="erp-toast">
<div id="toastMessage"></div>
<div class="erp-progress">
<div id="toastBar" class="erp-progress-bar"></div>
</div>
</div>
  <div id="toasterror" class="erp-toasterror">
<div id="toastMessageerror"></div>
<div class="erp-progresserror">
<div id="toastBarerror" class="erp-progress-barerror"></div>
</div>
</div> 
 
<!-- showlist PANEL -->
<div ng-show="projectState.showlist" class="panel">
     <style>
a.owner-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:9px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}
a.owner-wrp {position:relative;}
a.owner-wrp:hover .owner-del {display:block !important;}
.files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.issue_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.dependency_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.pmsinput {border:0px;outline:none;background-color:transparent;padding:4px;border-bottom: 1px solid;
    border-color: transparent;
    line-height: 21px;text-align:left}
.pmsinput:hover {border-bottom:1px solid #ccc;}
.pmsinput:focus {border-bottom:1px solid #72075d;}
.loader-overlay {
position: fixed;
top: 0px; left: 0px;
width: 100%; height: 100%;
background: rgba(255,255,255,1);
display: flex;
justify-content: center;
align-items: center;
z-index: 9999;
}

.loader {
border: 8px solid #f3f3f3;
border-top: 2px solid #3498db;
border-radius: 50%;
width: 60px;
height: 60px;
animation: spin 1s linear infinite;
}

.flex-grow-1 {-webkit-box-flex: 1!important; -ms-flex-positive: 1!important;flex-grow: 1!important;}
.btn-gantt:hover {background:#eef3ff;}
</style>  
    <style>
   .action-menu {
    position: relative;
    display: inline-block;
}

.action-icon {
    cursor: pointer;
    font-size: 18px;
    padding: 6px;
}

.action-dropdown {
    position: absolute;
    top: 10px;
    left:12px;
    right:200;
    background: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding:5px;
    height:35px;
    z-index: 9999;
    box-shadow: 0 4px 12px rgba(0,0,0,0.1);

    display: none; 
}

.action-menu:hover .action-dropdown {
   display: block;
    
}


.menu-item {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 8px 10px;
    text-decoration: none;
    color: #333;
    font-size: 13px;
}

.menu-item:hover {
    background: #f5f5f5;
}

.menu-item i {
    font-size: 14px;
}

.count {
    margin-left: auto;
    font-size: 12px;
    color: #888;
}
.dropup-menu {
    bottom: 100% !important;
    top: auto !important;
    transform: translate3d(0px, -5px, 0px) !important;
}
    </style>

    <style>
.table-fix-wrap {
    width: 100%;
    overflow: auto;
    position: relative;


}
.project-table {
    min-width: 1400px; /* increase if needed */
    border-collapse: separate;
    border-spacing: 0;
}

.project-table th,
.project-table td {
    white-space: nowrap;
    vertical-align: middle;
    background: #fff;
    border-style:solid;
}

/* sticky header */
.project-table thead th {
    position: sticky;
    top: 0;
    z-index: 20;
    background: #f8f9fa;
    border: 1px solid #e5e7eb;
}

.project-table thead th.column-draggable {
    cursor: move;
    user-select: none;
}
.project-table thead th.column-draggable::after {
    content: " ↕";
    font-size: 11px;
    color: #94a3b8;
}
.project-table thead th.column-fixed {
    cursor: default;
}
.project-table thead th.dragging-col {
    opacity: .55;
    background: #f3e8ff !important;
}
.project-table thead th.drag-over-col {
    box-shadow: inset 0 -3px 0 #7c3aed;
}


/* first column */
.project-table .col-action {
    left: 0;
    min-width:50px;
    width: 50px;
    z-index: 15;
}

/* second column */
.project-table .col-milestone {
    left: 50px;
    min-width: 360px;
    width: 360px;
    z-index: 14;
}

/* third column */
.project-table .col-status {
    left: 410px; /* 80 + 540 */
    min-width: 50px;
    width: 50px;
    z-index: 13;
}

/* header sticky columns should stay above body */
.project-table thead .col-action,
.project-table thead .col-milestone,
.project-table thead .col-status {
    z-index: 25;
    background: #f8f9fa;
}

/* optional shadow line after fixed area */
.project-table .col-status {
    box-shadow: 4px 0 6px rgba(0,0,0,0.08);
}
.col-action { width: 50px; left: 0; }
.col-milestone { width: 360px; left: 50px; }
.col-status { width: 50px; left: 410px; }
.sticky-col {
    position: sticky;
    z-index:3;
    background: #fff;

}

thead .sticky-col {
    z-index: 3;
}
    </style>
    <style>
 #zoomBar input[type="range"] {
    -webkit-appearance: none;
    appearance: none;
    width: 150px;
    height: 4px;
    background: #d0d0d0;
    border-radius: 5px;
    outline: none;
}

/* ===== Chrome / Edge / Safari ===== */
#zoomBar input[type="range"]::-webkit-slider-runnable-track {
    height: 4px;
    background: #cfcfcf;
    border-radius: 5px;
}

#zoomBar input[type="range"]::-webkit-slider-thumb {
    -webkit-appearance: none !important;
    appearance: none !important;
    width: 14px;
    height: 14px;
    background: #888 !important;   /* 🔴 TEST COLOR (RED) */
    border: 0px solid #000;
    border-radius: 10px;
    cursor: pointer;
    margin-top: -6px;
}

/* ===== Firefox ===== */
#zoomBar input[type="range"]::-moz-range-thumb {
    width: 14px;
    height: 14px;
    background: #888 !important;   /* 🔴 TEST COLOR */
    border: 2px solid #000;
    border-radius: 10px;
    cursor: pointer;
}
        .zoom-bar {
    position: sticky;
    bottom: 0;
    background: #fff;
    border-top: 1px solid #ddd;
    padding: 8px 12px;
    display: flex;
    align-items: center;
    gap: 10px;
    z-index: 100;
}

.table-fix-wrap {
    overflow: auto;
}
    </style>
<div class="offcanvas-header border-bottom" style="padding:4px 24px;"  ng-if="isload=='1'">
        
        <input type="hidden" name="hdnT" id="hdnT">
        <input type="hidden" name="hdnST" id="hdnST">
        <h5 class="offcanvas-title" >{{projectlist.ProjectName}}</h5>       
        <div class="btn-group mt-4 mt-sm-0" style="padding-right:70px;" role="group" aria-label="Basic example">                                  
                                      
             
    <a href="javascript:void(0);" 
       ng-click="addRowUpDown_default()" 
       class="btn btn-outline-primary btn-icon" 
       data-bs-toggle="tooltip" 
       data-bs-placement="top" 
       data-bs-trigger="hover" 
       title="Add Milestone">
       <i class="ri-add-line"></i>
    </a>
            
             
          
                               
                                       
            
            <a href="javascript:void(0);" style="display:none" id="addview" onclick="addviewreset()" class="btn btn-outline-primary btn-icon"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Reset" data-bs-original-title="Reset"><i class="ri-add-line"></i></span></a>

                                           
                                       

                                        
                                           
                                        
                                        
                                      <div class="btn-group" role="group">
    <button id="btnGroupDrop1"
            type="button"
            class="btn btn-outline-primary btn-icon"
            data-bs-toggle="dropdown"
            aria-expanded="false">
        <span data-bs-toggle="tooltip"
              data-bs-placement="top"
              data-bs-trigger="hover"
              title="Change View">
            <i class="ri-grid-fill"></i>
        </span>
    </button>

    <ul class="dropdown-menu" aria-labelledby="btnGroupDrop1">
        <li>
            <a class="dropdown-item"
               href=""
               ng-click="changeView('table', $event)">
                List
            </a>
        </li>
        <li>
            <a class="dropdown-item"
               href=""
               ng-click="changeView('gantt', $event)">
                Gantt
            </a>
        </li>
    </ul>
</div>
                                     <a href="javascript:void(0);" id="btncopy" class="btn btn-outline-primary btn-icon" data-bs-toggle="modal" data-bs-target="#addcopy" ng-click="loadprojectlist(projectlist.id)" ng-if="(!projectlist.miles || projectlist.miles.length == 0) || projectlist.isadmin"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Copy From" data-bs-original-title="Copy From"><i class="bi bi-files"></i></span></a>
                                     <%--<a href="javascript:void(0);" style="display:none" id="btnreset" class="btn btn-outline-primary btn-icon"  ng-click="loadprojectlist_reset(projectlist.id)" ng-if="(projectlist.miles && projectlist.miles.length>0) || projectlist.isadmin"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Copy From" data-bs-original-title="Reset Previous"><i class="bi bi-arrow-repeat"></i></a>--%>
                                        
                                    </div>
        <div class="hstack gap-2 cls_btn" style="top:11px;">
           <%-- <button type="button" id="zoominidmulti" onclick="zoominmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutidmulti" onclick="zoomoutmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            --%><button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" ng-click="closepopupmainmulti();" aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
   

    <div  ng-if="isload=='1'">
<div ng-show="view == 'table'"  class="table-responsive">

<div class="table-fix-wrap" style="height: calc(100vh - 90px);overflow:auto;" id="projectTableContainer">

<table ng-hide="isLoading" border="1" style="width:100%;border-collapse:collapse; cursor:pointer;" 
    class="table align-middle table-striped table-hover table-nowrap table-bordered table-sm project-table tbl_view"
>
<thead>
<tr>   
   <%-- class="sticky-col col-status"--%>
<th style="width:50px;" class="sticky-col col-action">Action</th>
<th class="sticky-col col-milestone" ng-click="sortData('Milestone')" style="min-width:360px;text-align:center">Milestone {{ getSortClass('Milestone') }}</th>

<th  ng-click="sortData('P_Status')" style="min-width:50px;text-align:center">Status {{ getSortClass('P_Status') }}</th>
<th ng-click="sortData('Priority')" style="min-width:50px;text-align:center">Priority {{ getSortClass('Priority') }}</th>
<th style="min-width:130px;text-align:center">Completion</br> Status</th>
<th ng-click="sortData('txtdays')" style="min-width:50px;text-align:center">Dura.</br>Days</th>
<th ng-click="sortData('StartDate')"  style="min-width:50px;text-align:center">Start Date{{ getSortClass('StartDate') }}</th>
<th ng-click="sortData('TaskDuration')"  style="min-width:85px;text-align:center">Task</br>(Hrs/Min) {{ getSortClass('TaskDuration') }}</th>
<th style="min-width:156px;text-align:center">Timeline</th>
<th style="min-width:125px;text-align:center">Owner</th>
<th style="min-width:125px;text-align:center">Team</th>
<th ng-click="sortData('Weightage')" style="min-width:30px;text-align:center">Weightage {{ getSortClass('Weightage') }}</th>
<th style="min-width:70px;text-align:center">Issues</th> 
<th style="min-width:250px;text-align:center">Description</th>
</tr>
</thead>

<tbody>
<tr ng-repeat-start="c in projectlist.miles | filter:searchText track by $index" ng-init="c.isExpanded = c.isExpanded || false"  class="parent-row pms-row-drop" data-pms-row="milestone" data-pms-id="{{c.id}}" data-pms-parent-id="{{projectlist.id}}" data-pms-index="{{}}">
    <td class="sticky-col col-action">
    <span class="pms-row-drag-handle" draggable="false" title="Move milestone" ng-mousedown="startPmsRowDrag($event, 'milestone', c, projectlist.miles, projectlist)"><i class="ri-drag-move-2-line"></i></span>
    <a href="javascript:void(0);" style="display:" class="btn btn-info btn-xs" type="button"  data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="loadformedit(c,'milestone','editdetails','/pms/editmile.aspx?id='+c.id+'&pid='+c.ProjectId+'&ctype=ang','Milestone')"><i class="ri-edit-line"></i></a>
    <a href="javascript:void(0);" style="display:" class="btn btn-danger btn-xs" type="button" ng-click="deletedatamile($index,c)"><i class="ri-delete-bin-5-line"></i></a>

    </td>
<td style="padding:0px 5px 0px 0" class="sticky-col col-milestone">
    
    <div class="d-flex">
        <strong style="padding: 6px 0px 6px 5px;">{{$index + 1 }}.&nbsp;</strong>
        <div class="flex-grow-1" style="width:240px;padding-right:10px;">
     <input type="text"
         ng-model="c.Milestone"
        
        ng-blur="updatedetails_mile(c, 'Milestone')" class="pmsinput w-100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.Milestone}}" style="font-size:14px;font-weight:bold; background-color:{{projectlist.editcolor}}">
        </div>
        <div class="gap-1 flex-shrink-0 align-self-center" style="float:right;width:70px;text-align:right;">
    <div class="dropdown" style="display:inline-block;">
  <%--<button class="btn btn-link btn-sm p-0" type="button" 
          data-bs-toggle="dropdown" aria-expanded="false"
          tooltip="Options">
    <span class="btn-plus2">+</span>
  </button>--%>
        <a href=""
   tabindex="0"
   class="btn btn-link btn-sm p-0"
   ng-click="togglePopover($event, c, projectlist, 'milestone', $index); $event.preventDefault();">
   <span class="btn-plus2">+</span>
</a>

  <ul class="dropdown-menu dropdownmenu-secondary">
    <li style="display:inline-block;">
      <a href="javascript:void(0);" ng-click="addRowUpDown_mile($index,c,'up')" class="dropdown-item p-1">
        <i class="bi bi-arrow-up-square" tooltip="Add Above"></i>
      </a>
    </li>

    <li style="display:inline-block; margin:0 15px;">
      <a href="javascript:void(0);" ng-click="addRowUpDown_mile($index,c,'down')" class="dropdown-item p-1">
        <i class="bi bi-arrow-down-square" tooltip="Add Below"></i>
      </a>
    </li>

    <li style="display:inline-block;">
      <a href="javascript:void(0);" ng-if="c.istask==1" ng-click="addRowUpDown_taskdefault(c)" class="dropdown-item p-1">
        <i class="ri-list-unordered label-icon align-middle me-1" tooltip="Add Task"></i>
      </a>
    </li>
  </ul>
</div>
    <%--<a href="javascript:void(0);" ng-if="c.taskcount==1" ng-click="updatesrc($event,c.id)"><img src="/pms/assets/images/open.png" alt="open"></a>--%>
     <a href="javascript:void(0);"  ng-if="c.tasks&&c.tasks.length>0" ng-click="toggleMilestone(c)">
      <img ng-src="{{c.isExpanded?'/pms/assets/images/close.png' : '/pms/assets/images/open.png'}}" alt="{{c.isExpanded ? 'close' : 'open'}}" />
    </a>
            
<div class="action-menu p-0">
<span class="action-icon p-0" ng-click="togglePopover_action($event, c, projectlist, 'milestone', $index,projectlist.ProjectName); $event.preventDefault();" ><i class="bi bi-three-dots-vertical"></i></span>
   <%-- <div class="action-dropdown" >

    <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='modal' data-bs-target='#addfiles'  ng-click="loadSrcFiles(c,'milestone',$index)"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Upload Documents' style='font-size:12px;height:22px;width:22px;margin-left:9px'><i class='bi bi-file-earmark-arrow-up'></i></span><span class='files_count' >{{ c.viewfiles.length || 0 }}</span></a>
    <a href='javascript:void(0);'  class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_dependency' ng-click="loadform(c,'milestone','dependency','inner_popupD','/pms/AddSTDependency.aspx?id='+c.id+'&mprojectid='+c.ProjectId+'&type=milestone&pid='+c.ProjectId+'',' Dependency of '+projectlist.ProjectName+'')"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span><span class='dependency_count' >{{ c.totaldependency || 0 }}</span></a>
    <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_issue' ng-click="loadform(c,'milestone','issue','inner_popup','/pms/AddIssuesPopup.aspx?id='+c.ProjectId+'&milstoneid='+c.id+'&mprojectid='+c.ProjectId+'&type=milstone&pid='+c.ProjectId+'',' Issues of '+projectlist.ProjectName+'')"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Issues' style='font-size:12px;height:22px;width:22px;margin-left:9px'>I</span><span class='issue_count' >{{ c.pendingissue || 0 }}</span></a>
</div>--%>
        </div>
                </div>
 
       
</div>
       
</td>
    <%--class="sticky-col col-status"--%>
<td ng-style="{'background-color': c.status_color}"    style="position:relative">
   
    <div style="position:relative;">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c.status_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c,newstatus,'milestone'); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='bi bi-square-fill' ng-style="{'color': newstatus.colorname}" ></i> {{newstatus.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
    </div>   
</td>
<td ng-style="{'background-color':c.priority_color}">
   
    <div class='btn-group d-block'>
   <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c.priority_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style='display:'>
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name"  ng-click="selectPriority(c,newpriority,'milestone'); $event.preventDefault()">
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='bi bi-square-fill' ng-style="{'color': newpriority.colorname}" ></i> {{newpriority.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
</div>
</td>
<td>
  <div class="progress progress-xl cursor-pointer">
    <div class="progress-bar bg-success"
         role="progressbar"
        
        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Complete ({{c.complete_value}}%)"
         ng-style="{'width': c.complete_value + '%'}">
      {{ c.complete_value }}%
    </div>

    <div class="progress-bar bg-warning"
         role="progressbar"
        
        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Pending ({{c.pending_value}}%)"
         ng-style="{'width': c.pending_value + '%'}">
      <span ng-if="c.complete_value != '0'">
        {{ c.pending_value }}%
    </span>

    <span ng-if="c.complete_value == '0'">
        0%
    </span>

    </div>
  </div>
</td>
<td><input type="text" class="pmsinput" ng-model="c.txtdays" 
     ng-focus="c._oldtxtdays = c.txtdays"
   
     ng-blur="update_duration(c,'milestone','duration')"
    style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)"></td>
<td style="position:relative;">
  <input type="date"
         ng-model="c.StartDate"
         ng-focus="c._oldStartDate = c.StartDate"
         class="datepicker"
         
         ng-blur="update_duration(c,'milestone','')"
         style="border:1px solid #ddd; outline:none;" onclick="if(this.showPicker){this.showPicker();}">
   <i class="ri-refresh-line ms-2 text-primary"
   style="cursor:pointer;position:absolute; top:6px;right:5px; z-index:10"
  
       data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Reset Start Date"
   ng-click="resetStartDateHierarchy_mile(c,projectlist,$index)">
</i>

    <%-- <i class="ri-refresh-line ms-2 text-primary"
       style="cursor:pointer"
       title="Reset"
       ng-click="resetStartDateFromRow($index)">
    </i>--%>
</td>
<td>
   
   <%-- <input type="text" class="pmsinput"
           ng-model="c.TaskDuration"
        ui-mask="999:99"
       placeholder="000:00"
        ng-focus="c._oldTaskDuration = c.TaskDuration"
           ng-blur="validateTime(c,$index,'milestone');update_hrs(c,'milestone')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}" >--%>
   <div class="duration-box">
    
    <input type="text" class="duration-input left" ng-model="c.TaskDuration"  ng-focus="c._oldTaskDuration = c.TaskDuration"
           ng-blur="update_hrs(c,'milestone')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

    <div class="sls_2">/</div>

    <input type="text"
           class="duration-input right"          
           maxlength="2"
        ng-model="c.TaskDuration_min"  ng-focus="c._oldTaskDuration_min = c.TaskDuration_min"
           ng-blur="validateTime(c,$index,'milestone');update_hrs(c,'milestone')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

</div>

</td>

<td>
  <a href="javascript:void(0);" class="timesheet-wrap" 
     ng-click="addeventdateselect(c.ProjectId, c.id, c.StartDate, c.EndDate, 'm', $event)" style="width:100%">
     
    <div class="progress progress-xl bg-warning position-relative" 
         data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.leftdays}}">
      
      <!-- progress bar -->
      <div class="progress-bar bg-secondary" 
           role="progressbar" 
           ng-style="{'width': '100%'}"
          
          >
      </div>
      
      <!-- inner wrapper -->
      <div class="dayswrapper">
        <span class="fromtodate">{{ c.StartDate | date:'dd MMM yyyy' }} - {{ c.EndDate | date:'dd MMM yyyy' }}</span>
        <span class="totaldays">{{ c.txtdays }} days</span>
      </div>
      
    </div>
  </a>
</td>
   <td>
    <div class="dropdown w-100">

  <button class="btn btn-sm dropdown-toggle w-100 text-start no-caret"
          type="button"
          data-bs-toggle="dropdown" 
        ng-click="focusOwnerSearch($event, c)">

    {{ getOwnerName(c.Owner) || 'Select Owner' }}

  </button>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2"
           placeholder="Search..."
           ng-model="c.ownerSearch"
        >

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: c.ownerSearch"
         ng-class="{'active': c.Owner == o.id}"
         ng-click="selectOwner(c, o,'milestone')">

        {{o.name}}

      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="(owners | filter: c.ownerSearch).length == 0">
        No results found
      </div>

    </div>

  </div>

</div>   
<%--<div class="d-flex">
 <div class="flex-grow-1">
     
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newowner in c.owners|limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-original-title="{{newowner.name}}">
    <div class="avatar-xxs">
      <img ng-if="newowner.profileimage"
         ng-src="{{newowner.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newowner.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary" style="width:20px;height:20px;">
        {{newowner.shortname}}
      </div>
    </div>

    </div>
    <i class="ri-close-line owner-del" style="position:absolute; top:-6px; right:-2px; cursor:pointer;display:none;color:red;"
       ng-click="removeowners(c, newowner,'milestone')">
    </i>

</a>
  <div ng-if="c.owners && c.owners.length>3"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c.owners) }}" >

    <div class="avatar-xxs">
        <div class="avatar-title rounded-circle" style="width:20px;height:20px;">
        +{{ c.owners.length - 3 }}
            </div>
    </div>
</div>
</div>
     
 </div>
 <div class="gap-1 flex-shrink-0 align-self-center">
     <a href="javascript:void(0);" style="display:" type="button" 
         ng-click="loadSrcOwnersnew(c,'owner','milestone',$index)" 
         data-bs-toggle="modal" data-bs-target="#addowners">
         <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Owner">+</span></a>
</div>

</div>--%>


  
</td>
<td>
  <div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c.teams|limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-original-title="{{newteam.name}}">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary" style="width:20px;height:20px;">
        {{newteam.shortname}}
      </div>
    </div>

    </div>
    <i class="ri-close-line owner-del" style="position:absolute; top:-6px; right:-2px; cursor:pointer;display:none;color:red;"
      ng-click="removeteams(c,newteam,'milestone')">
    </i>

</a>
    <div ng-if="c.teams && c.teams.length > 3"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle" style="width:20px;height:20px;">
        +{{ c.teams.length - 3 }}
    </div>
    </div>
</div>
 
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center"><a href="javascript:void(0);"
     ng-click="loadSrcOwnersnew(c,'team','milestone',$index)" 
     data-bs-toggle="modal" data-bs-target="#addowners">
     <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
</td>
<td>
    <input type="text" class="pmsinput"
           ng-model="c.Weightage"
        ng-blur="updatedetails_mile(c, 'Weightage')"
          
           style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)">
</td>
<td>
   <div class="d-flex">
       <div class="flex-grow-1 align-self-center">
       <div class="progress progress-xl cursor-pointer">
           <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completeissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.completeissue_rate}}% [{{c.completeissue}} of {{c.totalissue}} Issues Completed]">{{c.completeissue}}</div>
           <div class="progress-bar bg-warning" role="progressbar" style="width:{{c.pendingissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.pendingissue_rate}}% [{{c.pendingissue}} of {{c.totalissue}} Issues Not Completed]">{{c.pendingissue}}</div>

       </div>
       </div>
       <div class="gap-1 flex-shrink-0 align-self-center">
           <a href="javascript:void(0);" style="display:" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_issue" ng-click="loadform(c,'milestone','issue','inner_popup','/pms/AddIssuesPopup.aspx?id='+c.ProjectId+'&milstoneid='+c.id+'&type=milstone','Issues of '+projectlist.ProjectName+' ')" >
               <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+Issue">+</span>

           </a>
       </div>
   </div>
</td>

<%--<td>
    <input type="text" class="pmsinput" ng-model="c.miledescription" ng-blur="updatedetails_mile(c, 'miledescription')"
           
           style="width:100%;background-color:{{projectlist.editcolor}}" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.miledescription}}">
</td>--%>
<td>
    <input type="text" class="pmsinput" ng-model="c.miledescription" ng-blur="!c.isDescPopupOpen && updatedetails_mile(c,'miledescription')" ng-dblclick="c.isDescPopupOpen=true; openDescriptionPopup($event,c,'miledescription','milestone')"
           
           style="width:100%;background-color:{{projectlist.editcolor}}" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.miledescription}}">
</td>
    <%--<td>
    <textarea
        class="pmsinput"
        ng-model="c.miledescription"
        ng-blur="!c.isDescPopupOpen && updatedetails_mile(c,'miledescription')"
        ng-dblclick="c.isDescPopupOpen=true; openDescriptionPopup($event,c,'miledescription','milestone')"

        style="width:100%;
               background-color:{{projectlist.editcolor}};
               min-height:28px;
               resize:vertical;
               overflow:auto;"

        data-bs-toggle="tooltip"
        data-bs-placement="top"
        data-bs-trigger="hover"
        title="{{c.miledescription}}">
    </textarea>
</td>--%>
</tr>
    <tr ng-repeat-start="c1 in c.tasks track by $index" class="child-row pms-row-drop" ng-if="c.isExpanded" data-pms-row="task" data-pms-id="{{c1.id}}" data-pms-parent-id="{{c.id}}" data-pms-index="{{}}">
   <td class="sticky-col col-action">
    <span class="pms-row-drag-handle" draggable="false" title="Move task" ng-mousedown="startPmsRowDrag($event, 'task', c1, c.tasks, c)"><i class="ri-drag-move-2-line"></i></span>
    <a href="javascript:void(0);" style="display:" class="btn btn-info btn-xs" type="button" ng-click="loadformedit(c1,'task','editdetails','/pms/edittask.aspx?id='+c1.id+'&mid='+c1.Milestoneid+'&ctype=ang','Task')" data-bs-toggle="offcanvas" data-bs-target="#addmile"><i class="ri-edit-line"></i></a>
    <a href="javascript:void(0);" style="display:" class="btn btn-danger btn-xs" type="button" ng-click="deletedatatask($index,c1,c)"><i class="ri-delete-bin-5-line"></i></a></td>     
<td style="padding:0px 5px 0px 0" class="sticky-col col-milestone">
    
    <div class="d-flex">
        <strong style="padding: 6px 0px 6px 12px;">{{$parent.$parent.$index+1}}.{{ $index + 1 }}&nbsp;</strong>
        <div class="flex-grow-1" style="width:240px;padding-right:10px">
     <input type="text"
         ng-model="c1.taskname"
        
        ng-blur="updatedetails_task(c1, 'taskname')" class="pmsinput w-100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.taskname}}" style="font-size:12px;font-weight:bold;background-color:{{projectlist.editcolor}}">
        </div>
        <div class="gap-1 flex-shrink-0 align-self-center" style="float:right;width:70px;text-align:right;">
    <div class="dropdown" style="display:inline-block;">
 <%-- <button class="btn btn-link btn-sm p-0" type="button" 
          data-bs-toggle="dropdown" aria-expanded="false"
          tooltip="Options">
    <span class="btn-plus2">+</span>
  </button>--%>
        <a href=""
   tabindex="0"
   class="btn btn-link btn-sm p-0"
   ng-click="togglePopover($event, c1, c, 'task', $index); $event.preventDefault();">
   <span class="btn-plus2">+</span>
</a>

  <ul class="dropdown-menu dropdownmenu-secondary">
    <li style="display:inline-block;">
      <a href="javascript:void(0);" ng-click="addRowUpDown_task($index,c,c1,'up')" class="dropdown-item p-1">
        <i class="bi bi-arrow-up-square" tooltip="Add Above"></i>
      </a>
    </li>

    <li style="display:inline-block; margin:0 15px;">
      <a href="javascript:void(0);" ng-click="addRowUpDown_task($index,c,c1,'down')" class="dropdown-item p-1">
        <i class="bi bi-arrow-down-square" tooltip="Add Below"></i>
      </a>
    </li>

    <li style="display:inline-block;">
      <a href="javascript:void(0);" ng-if="c1.issubtask===1" ng-click="addRowUpDown_subtaskdefault(c1)" class="dropdown-item p-1">
        <i class="ri-list-unordered label-icon align-middle me-1" tooltip="Add Sub Task"></i>
      </a>
    </li>
  </ul>
</div>
    <a href="javascript:void(0);" ng-if="c1.subtasks&&c1.subtasks.length>0" ng-click="toggleTask(c1)">
      <img ng-src="{{c1.isExpanded ? '/pms/assets/images/close.png' : '/pms/assets/images/open.png'}}" alt="{{c1.isExpanded ? 'close' : 'open'}}" />
    </a>
            <div class="action-menu p-0">
<span class="action-icon p-0" ng-click="togglePopover_action($event, c1,c, 'task', $index,projectlist.ProjectName); $event.preventDefault();" ><i class="bi bi-three-dots-vertical"></i></span>
    <%--<div class="action-dropdown">
    <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='modal' data-bs-target='#addfiles'  ng-click="loadSrcFiles(c1,'task',$index)"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Upload Documents' style='font-size:12px;height:22px;width:22px;margin-left:9px'><i class='bi bi-file-earmark-arrow-up'></i></span><span class='files_count' >{{ c1.viewfiles.length || 0 }}</span></a>
    <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_dependency' ng-click="loadform(c1,'task','dependency','inner_popupD','/pms/AddSTDependency.aspx?id='+c1.id+'&mid='+c1.MilestonId+'&mprojectid='+c1.ProjectId+'&type=task&pid='+c1.ProjectId+'','Dependency of '+projectlist.ProjectName+'')"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span><span class='dependency_count' >{{ c1.totaldependency || 0 }}</span></a>
    <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_issue' ng-click="loadform(c1,'task','issue','inner_popup','/pms/AddIssuesPopup.aspx?id='+c1.ProjectId+'&milstoneid='+c1.MilestonId+'&taskid='+c1.id+'&tasklistid=0&mprojectid='+c1.ProjectId+'&type=task','Issues of '+projectlist.ProjectName+'')"
        ><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Issues' style='font-size:12px;height:22px;width:22px;margin-left:9px'>I</span><span class='issue_count' >{{ c1.pendingissue || 0 }}</span></a>
</div>--%>
        </div>
</div>

        
        </div>
</td>
        <%--class="sticky-col col-status"--%>
<td ng-style="{'background-color': c1.status_color}" style="position:relative" >
   
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c1.status_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style='display:'>
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c1,newstatus,'task'); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='bi bi-square-fill' ng-style="{'color': newstatus.colorname}" ></i> {{newstatus.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
</td>
<td ng-style="{'background-color':c1.priority_color}">
   
    <div class='btn-group d-block'>
   <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c1.priority_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style='display:'>
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name"  ng-click="selectPriority(c1,newpriority,'task'); $event.preventDefault()">
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='bi bi-square-fill' ng-style="{'color': newpriority.colorname}" ></i> {{newpriority.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
</div>
</td>
<td>
  <div class="progress progress-xl cursor-pointer">
    <div class="progress-bar bg-success"
         role="progressbar"
        
        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Complete ({{c1.complete_value}}%)"
         ng-style="{'width': c1.complete_value + '%'}">
      {{ c1.complete_value }}%
    </div>

    <div class="progress-bar bg-warning"
         role="progressbar"
  
        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Pending ({{c1.pending_value}}%)"
         ng-style="{'width': c1.pending_value + '%'}">
      <span ng-if="c1.complete_value != '0'">
        {{ c1.pending_value }}%
    </span>

    <span ng-if="c1.complete_value == '0'">
        0%
    </span>
    </div>
  </div>
</td>
<td><input type="text" class="pmsinput" ng-model="c1.txtdays" 
   
     ng-blur="update_duration_task(c1,c,'')"
    style="width:100%;background-color:{{projectlist.editcolor}}"  ng-keypress="isnumeric($event)"></td>
<td style="position:relative;">
  <input type="date"
         ng-model="c1.Startdate"
      ng-focus="c1._oldStartdate = c1.Startdate"
         class="datepicker"

        
       ng-blur="update_duration_task(c1,c,'')"
         style="border:1px solid #ddd; outline:none;" onclick="if(this.showPicker){this.showPicker();}">
     <i class="ri-refresh-line ms-2 text-primary"
 style="cursor:pointer;position:absolute; top:6px;right:5px; z-index:10"
  data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Reset Start Date"
   ng-click="resetStartDateHierarchy_task(c1,c,$index)">
</i>
</td>
<td>
    <%--<input type="text" class="pmsinput"
           ng-model="c1.TaskDuration"
         ng-focus="c1._oldTaskDuration = c1.TaskDuration"
        ui-mask="999:99"  placeholder="000:00"
          ng-blur="validateTime(c1,$index,'task');update_hrs(c1,'task')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}">--%>
    <div class="duration-box">
    
    <input type="text" class="duration-input left" ng-model="c1.TaskDuration"  ng-focus="c1._oldTaskDuration = c1.TaskDuration"
           ng-blur="checkLimit(c1, c, 'task');"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

    <div class="sls_2">/</div>

    <input type="text"
           class="duration-input right"          
           maxlength="2"
           ng-model="c1.TaskDuration_min"  ng-focus="c1._oldTaskDuration_min = c1.TaskDuration_min"
           ng-blur="validateTime(c1,$index,'task');checkLimit(c1, c, 'task');"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

</div>
</td>

<td>
  <a href="javascript:void(0);" class="timesheet-wrap" 
     ng-click="addeventdateselect(c1.ProjectId, c1.id, c1.Startdate, c1.Targetdate, 't', $event)" style="width:100%">
     
    <div class="progress progress-xl bg-warning position-relative" 
         data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.leftdays}}">
      
      <!-- progress bar -->
      <div class="progress-bar bg-secondary" 
           role="progressbar" 
           ng-style="{'width': '100%'}">
      </div>
      
      <!-- inner wrapper -->
      <div class="dayswrapper">
        <span class="fromtodate">{{ c1.Startdate | date:'dd MMM yyyy' }} - {{ c1.Targetdate | date:'dd MMM yyyy' }}</span>
        <span class="totaldays">{{ c1.txtdays }} days</span>
      </div>
      
    </div>
  </a>
</td>
   <td>
       <div class="dropdown w-100">

  <button class="btn btn-sm dropdown-toggle w-100 text-start no-caret"
          type="button"
          data-bs-toggle="dropdown">

    {{ getOwnerName(c1.owner) || 'Select Owner' }}

  </button>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2"
           placeholder="Search..."
           ng-model="c1.ownerSearch">

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: c1.ownerSearch"
         ng-class="{'active': c1.owner == o.id}"
         ng-click="selectOwner(c1, o,'task')">

        {{o.name}}

      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="(owners | filter: c1.ownerSearch).length == 0">
        No results found
      </div>

    </div>

  </div>

</div>



  
</td>
<td>
  <div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c1.teams|limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-original-title="{{newteam.name}}">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary" style="width:20px;height:20px;">
        {{newteam.shortname}}
      </div>
    </div>

    </div>
    <i class="ri-close-line owner-del" style="position:absolute; top:-6px; right:-2px; cursor:pointer;display:none;color:red;"
       ng-click="removeteams(c1, newteam,'task')">
    </i>

</a>
   <div ng-if="c1.teams && c1.teams.length > 3"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c1.teams) }}">
       <div class="avatar-xxs">
    <div class="avatar-title rounded-circle" style="width:20px;height:20px;">
        +{{ c1.teams.length - 3 }}
    </div>
    </div>
</div>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center"><a href="javascript:void(0);" style="display:" type="button"  ng-click="loadSrcOwnersnew(c1,'team','task',$index)" data-bs-toggle="modal" data-bs-target="#addowners"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
</td>
<td>
    <input type="text" class="pmsinput"
           ng-model="c1.Weightage"
        ng-blur="updatedetails_task(c1, 'Weightage')"
          
           style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)">
</td>
<td>
    <div class="d-flex">
       <div class="flex-grow-1 align-self-center">
       <div class="progress progress-xl cursor-pointer">
           <div class="progress-bar bg-success" role="progressbar" style="width:{{c1.completeissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.completeissue_rate}}% [{{c1.completeissue}} of {{c1.totalissue}} Issues Completed]">{{c1.completeissue}}</div>
           <div class="progress-bar bg-warning" role="progressbar" style="width:{{c1.pendingissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.pendingissue_rate}}% [{{c1.pendingissue}} of {{c1.totalissue}} Issues Not Completed]">{{c1.pendingissue}}</div>

       </div>
       </div>
       <div class="gap-1 flex-shrink-0 align-self-center">
           <a href="javascript:void(0);" style="display:" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_issue" ng-click="loadform(c1,'task','issue','inner_popup','/pms/AddIssuesPopup.aspx?id='+c1.ProjectId+'&milstoneid='+c1.Milestoneid+'&tasklistid=0&taskid='+c1.id+'&type=task','Issues of '+projectlist.ProjectName+'')" >
               <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+Issue">+</span>

           </a>
       </div>
   </div>
</td>
<td>
    <input type="text" class="pmsinput" ng-model="c1.taskdescripton" ng-blur="updatedetails_task(c1, 'taskdescripton')"
           
           style="width:100%;background-color:{{projectlist.editcolor}}" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.taskdescripton}}">
</td>

</tr>
    <tr ng-repeat="c2 in c1.subtasks track by $index" class="child-row pms-row-drop" ng-if="c1.isExpanded" data-pms-row="subtask" data-pms-id="{{c2.id}}" data-pms-parent-id="{{c1.id}}" data-pms-index="{{}}">
   <td class="sticky-col col-action">
    <span class="pms-row-drag-handle" draggable="false" title="Move sub task" ng-mousedown="startPmsRowDrag($event, 'subtask', c2, c1.subtasks, c1)"><i class="ri-drag-move-2-line"></i></span>
    <a href="javascript:void(0);" style="display:" class="btn btn-info btn-xs" type="button" ng-click="loadformedit(c2,'subtask','editdetails','/pms/addnewsubtask.aspx?id='+c2.id+'&tid='+c2.Task+'&ctype=ang','Sub Task')" data-bs-toggle="offcanvas" data-bs-target="#addmile"><i class="ri-edit-line"></i></a>
    <a href="javascript:void(0);" style="display:" class="btn btn-danger btn-xs" type="button" ng-click="deletedatasubtask($index,c2,c1)"><i class="ri-delete-bin-5-line"></i></a></td>     
<td style="padding:0px 5px 0px 0" class="sticky-col col-milestone">
    
    <div class="d-flex">
        <label style="padding: 6px 0px 6px 22px;">{{$parent.$parent.$parent.$index+1}}.{{$parent.$parent.$index+1}}.{{ $index + 1 }}&nbsp;</label>
        <div class="flex-grow-1" style="width:240px;padding-right:10px;">
     <input type="text"
         ng-model="c2.Subtaskname"
        
        ng-blur="updatedetails_subtask(c2, 'Subtaskname')" class="pmsinput w-100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.Subtaskname}}" style="background-color:{{projectlist.editcolor}}">
        <%--font-size:10px;font-weight:bold;--%>
        </div>
        <div class="gap-1 flex-shrink-0 align-self-center" style="float:right;width:70px;text-align:right;">
    <div class="dropdown" style="display:inline-block;">
  <%--<button class="btn btn-link btn-sm p-0" type="button" 
          data-bs-toggle="dropdown" aria-expanded="false"
          tooltip="Options">
    <span class="btn-plus2">+</span>
  </button>--%>
        <a href=""
   tabindex="0"
   class="btn btn-link btn-sm p-0"
   ng-click="togglePopover($event, c2,c1,'subtask',$index); $event.preventDefault();">
   <span class="btn-plus2">+</span>
</a>

  <ul class="dropdown-menu dropdownmenu-secondary">
    <li style="display:inline-block;">
      <a href="javascript:void(0);" ng-click="addRowUpDown_subtask($index,c1,c2,'up')" class="dropdown-item p-1">
        <i class="bi bi-arrow-up-square" tooltip="Add Above"></i>
      </a>
    </li>

    <li style="display:inline-block; margin:0 15px;">
      <a href="javascript:void(0);" ng-click="addRowUpDown_subtask($index,c1,c2,'down')" class="dropdown-item p-1">
        <i class="bi bi-arrow-down-square" tooltip="Add Below"></i>
      </a>
    </li>

    
  </ul>
</div>
    <div class="action-menu p-0">
<span class="action-icon p-0" ng-click="togglePopover_action($event, c2,c1, 'subtask', $index,projectlist.ProjectName); $event.preventDefault();" ><i class="bi bi-three-dots-vertical"></i></span>
    <%--<div class="action-dropdown">
    <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='modal' data-bs-target='#addfiles'  ng-click="loadSrcFiles(c2,'subtask',$index)"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='Upload Documents' style='font-size:12px;height:22px;width:22px;margin-left:9px'><i class='bi bi-file-earmark-arrow-up'></i></span><span class='files_count' >{{ c2.viewfiles.length || 0 }}</span></a>
    <a href='javascript:void(0);' class="position-relative" type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_dependency' ng-click="loadform(c2,'subtask','dependency','inner_popupD','/pms/AddSTDependency.aspx?id='+c2.id+'&tid='+c2.task+'&mid='+c2.MilestonId+'&pid='+c2.ProjectId+'&mprojectid='+c2.ProjectId+'&type=subtask','Dependency of '+projectlist.ProjectName+'')"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span><span class='dependency_count' >{{ c2.totaldependency || 0 }}</span></a>
    <a href='javascript:void(0);' class="position-relative" type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_issue' ng-click="loadform(c2,'subtask','issue','inner_popup','/pms/AddIssuesPopup.aspx?id='+c2.ProjectId+'&taskid='+c2.task+'&milstoneid='+c.MilestonId+'&subtaskid='+c2.id+'&tasklistid=0&mprojectid='+c.ProjectId+'&type=subtask',' Issues of '+projectlist.ProjectName+'')"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Issues' style='font-size:12px;height:22px;width:22px;margin-left:9px'>I</span><span class='issue_count' >{{ c2.pendingissue || 0 }}</span></a>
</div>--%>
        </div></div>
</div>
       
</td>
        <%--class="sticky-col col-status"--%>
<td ng-style="{'background-color': c2.status_color}" style="position:relative" >
   
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c2.status_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style='display:'>
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c2,newstatus,'subtask'); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='bi bi-square-fill' ng-style="{'color': newstatus.colorname}" ></i> {{newstatus.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
</td>
<td ng-style="{'background-color':c2.priority_color}">
   
    <div class='btn-group d-block'>
   <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c2.priority_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style='display:'>
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name"  ng-click="selectPriority(c2,newpriority,'subtask'); $event.preventDefault()">
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='bi bi-square-fill' ng-style="{'color': newpriority.colorname}" ></i> {{newpriority.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
</div>
</td>
<td>
  <div class="progress progress-xl cursor-pointer">
    <div class="progress-bar bg-success"
         role="progressbar"

        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Complete ({{c2.complete_value}}%)"
         ng-style="{'width': c2.complete_value + '%'}">
      {{ c2.complete_value }}%
    </div>

    <div class="progress-bar bg-warning"
         role="progressbar"
         
        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Pending ({{c2.pending_value}}%)"
         ng-style="{'width': c2.pending_value + '%'}">
       <span ng-if="c2.complete_value != '0'">
        {{ c2.pending_value }}%
    </span>

    <span ng-if="c2.complete_value == '0'">
        0%
    </span>
    </div>
  </div>
</td>
<td><input type="text" class="pmsinput" ng-model="c2.txtdays" 
    
     ng-blur="update_duration_subtask(c2,c1,'duration')"
    style="width:100%;background-color:{{projectlist.editcolor}}"  ng-keypress="isnumeric($event)"></td>
<td style="position:relative;">
  <input type="date"
         ng-model="c2.Startdate"
      ng-focus="c2._oldStartdate = c2.Startdate"
         class="datepicker"         
       ng-blur="update_duration_subtask(c2,c1,'')"
         style="border:1px solid #ddd; outline:none;" onclick="if(this.showPicker){this.showPicker();}">
    <i class="ri-refresh-line ms-2 text-primary"
    style="cursor:pointer;position:absolute; top:6px;right:5px; z-index:10"
   data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-title="Reset Start Date"
   ng-click="resetStartDateHierarchy_subtask(c2,c1,$index)">
</i>
</td>
<td>
    <%--<input type="text" class="pmsinput"
           ng-model="c2.TaskDuration"  ui-mask="999:99"  placeholder="000:00"
         ng-focus="c2._oldTaskDuration = c2.TaskDuration"
          ng-blur="validateTime(c2,$index,'subtask');update_hrs(c2,'subtask')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}">--%>
    <div class="duration-box">
    
    <input type="text" class="duration-input left" ng-model="c2.TaskDuration"  ng-focus="c2._oldTaskDuration = c2.TaskDuration"
           ng-blur="checkLimit(c2, c1, 'subtask');"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

    <div class="sls_2">/</div>

    <input type="text"
           class="duration-input right"          
           maxlength="2"
           ng-model="c2.TaskDuration_min"  ng-focus="c2._oldTaskDuration_min = c2.TaskDuration_min"
           ng-blur="validateTime(c2,$index,'subtask');checkLimit(c2, c1, 'subtask');"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

</div>
</td>

<td>
  <a href="javascript:void(0);" class="timesheet-wrap" 
     ng-click="addeventdateselect(c2.ProjectId, c2.id, c2.Startdate, c2.Targetdate, 's', $event)" style="width:100%">
     
    <div class="progress progress-xl bg-warning position-relative" 
        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.leftdays}}">
      
      <!-- progress bar -->
      <div class="progress-bar bg-secondary" 
           role="progressbar" 
           ng-style="{'width': '100%'}">
      </div>
      
      <!-- inner wrapper -->
      <div class="dayswrapper">
        <span class="fromtodate">{{ c2.Startdate | date:'dd MMM yyyy' }} - {{ c2.Targetdate | date:'dd MMM yyyy' }}</span>
        <span class="totaldays">{{ c2.txtdays }} days</span>
      </div>
      
    </div>
  </a>
</td>
   <td>
       <div class="dropdown w-100">

  <button class="btn btn-sm dropdown-toggle w-100 text-start no-caret"
          type="button"
          data-bs-toggle="dropdown">

    {{ getOwnerName(c2.owner) || 'Select Owner' }}

  </button>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2"
           placeholder="Search..."
           ng-model="c2.ownerSearch">

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: c2.ownerSearch"
         ng-class="{'active': c2.owner == o.id}"
         ng-click="selectOwner(c2, o,'subtask')">

        {{o.name}}

      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="(owners | filter: c1.ownerSearch).length == 0">
        No results found
      </div>

    </div>

  </div>

</div>
<%--<div class="d-flex">
 <div class="flex-grow-1">
     
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newowner in c2.owners| limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-original-title="{{newowner.name}}">
    <div class="avatar-xxs">
      <img ng-if="newowner.profileimage"
         ng-src="{{newowner.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newowner.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary" style="width:20px;height:20px;">
        {{newowner.shortname}}
      </div>
    </div>

    </div>
    <i class="ri-close-line owner-del" style="position:absolute; top:-6px; right:-2px; cursor:pointer;display:none;color:red;"
       ng-click="removeowners(c2, newowner,'subtask')">
    </i>

</a>
  <div ng-if="c2.owners && c2.owners.length > 3"
     class="avatar-xxs"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c2.owners) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle" style="width:20px;height:20px;">
        +{{ c2.owners.length - 3 }}
    </div>
    </div>
</div>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center"><a href="javascript:void(0);" style="display:" type="button"  ng-click="loadSrcOwnersnew(c2,'owner','subtask',$index)" data-bs-toggle="modal" data-bs-target="#addowners"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Owner">+</span></a>
</div>

</div>--%>


  
</td>
<td>
  <div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c2.teams|limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-original-title="{{newteam.name}}">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary" style="width:20px;height:20px;">
        {{newteam.shortname}}
      </div>
    </div>

    </div>
    <i class="ri-close-line owner-del" style="position:absolute; top:-6px; right:-2px; cursor:pointer;display:none;color:red;"
       ng-click="removeteams(c2, newteam,'subtask')">
    </i>

</a>
   <div ng-if="c2.teams && c2.teams.length > 3"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c2.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle" style="width:20px;height:20px;">
        +{{ c2.teams.length - 3 }}
    </div>
    </div>
</div>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center"><a href="javascript:void(0);" style="display:" type="button"  ng-click="loadSrcOwnersnew(c2,'team','subtask',$index)" data-bs-toggle="modal" data-bs-target="#addowners"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
</td>
<td>
    <input type="text" class="pmsinput"
           ng-model="c2.Weightage"
        ng-blur="updatedetails_subtask(c2, 'Weightage')"
          
           style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)">
</td>
<td>
    <div class="d-flex">
       <div class="flex-grow-1 align-self-center">
       <div class="progress progress-xl cursor-pointer">
           <div class="progress-bar bg-success" role="progressbar" style="width:{{c2.completeissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.completeissue_rate}}% [{{c2.completeissue}} of {{c2.totalissue}} Issues Completed]">{{c2.completeissue}}</div>
           <div class="progress-bar bg-warning" role="progressbar" style="width:{{c2.pendingissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.pendingissue_rate}}% [{{c2.pendingissue}} of {{c2.totalissue}} Issues Not Completed]">{{c2.pendingissue}}</div>

       </div>
       </div>
       <div class="gap-1 flex-shrink-0 align-self-center">
           <a href="javascript:void(0);" style="display:" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_issue" ng-click="loadform(c2,'subtask','issue','inner_popup','/pms/AddIssuesPopup.aspx?id='+c2.ProjectId+'&milstoneid='+c2.Milestoneid+'&tasklistid=0&taskid='+c2.task+'&subtaskid='+c2.id+'&type=subtask','Issues of '+projectlist.ProjectName+'')" >
               <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+Issue">+</span>

           </a>
       </div>
   </div>
</td>
<td>
    <input type="text" class="pmsinput" ng-model="c2.SubTaskDescription" ng-blur="updatedetails_subtask(c2, 'SubTaskDescription')"
           
           style="width:100%;background-color:{{projectlist.editcolor}}" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.SubTaskDescription}}">
</td>

</tr>
    <tr ng-repeat-end></tr>
   <tr ng-repeat-end></tr>
   
 
    
</tbody>

</table>

    </div>
</div>

<div ng-show="view == 'gantt'" class="gantt-page-wrap" style="height: calc(100vh - 60px);overflow:auto;">
    <div class="gantt-toolbar">
        <div class="gantt-title">
            <strong>{{projectlist.ProjectName}}</strong>
        </div>

        <div class="gantt-actions">
            <select class="form-select form-select-sm d-inline-block"
                    style="width:120px;"
                    ng-model="ganttScale"
                    ng-change="changeGanttScale(ganttScale)">
                <option value="day">Day</option>
                <option value="week">Week</option>
                <option value="month">Month</option>
            </select>
        </div>
    </div>

    <div class="gantt-shell" ng-if="ganttRows.length">
        <!-- left side -->
        <div class="gantt-left-panel">
            <div class="gantt-left-head">
                <div class="gantt-col-task">Task Name</div>
                <div class="gantt-col-owner">Owner</div>
                <div class="gantt-col-status">Status</div>
                <div class="gantt-col-progress">% </div>
            </div>

            <%--<div class="gantt-left-row"
                 ng-repeat="row in ganttRows track by row.uid"
                 ng-class="{'row-milestone': row.type=='milestone', 'row-task': row.type=='task', 'row-subtask': row.type=='subtask'}">
               --%> 
            <div class="gantt-left-row"
     ng-repeat="row in ganttRows track by row.uid"
     data-gantt-row="{{row.uid}}"
     data-row-index="{{$index}}"
     data-row-type="{{row.type}}"
     ng-class="{
        'row-milestone': row.type=='milestone',
        'row-task': row.type=='task',
        'row-subtask': row.type=='subtask',
        'gantt-row-moving': ganttDraggingRow && ganttDraggingRow.uid == row.uid,
        'gantt-row-over': ganttDragOverRow && ganttDragOverRow.uid == row.uid,
        'gantt-row-invalid': ganttInvalidDropRow && ganttInvalidDropRow.uid == row.uid
     }">
                <div class="gantt-col-task">
                    
                    <div class="gantt-task-name level-{{row.level}}">
                        <span class="gantt-row-drag-handle"
                              draggable="false"
                              title="Drag row"
                              ng-mousedown="startSoftGanttRowDrag($event,row)"
                              ng-touchstart="startSoftGanttRowDrag($event,row)">
                            <i class="ri-drag-move-2-line"></i>
                        </span>

                        <span class="gantt-expand"
                              ng-if="row.hasChildren"
                              ng-click="toggleGanttRow(row, $event)">
                            <i class="ri-arrow-right-s-line" ng-if="!row.expanded"></i>
                            <i class="ri-arrow-down-s-line" ng-if="row.expanded"></i>
                        </span>
                        <span class="gantt-expand-empty" ng-if="!row.hasChildren"></span>
                        <span class="gantt-type-badge {{row.type}}" >{{ row.type.charAt(0).toUpperCase()}}</span>
                      
                         <input type="text"
       ng-model="row.name"
       class="pmsinput w-100"
       title="{{row.name}}"
       data-bs-toggle="tooltip"
       data-bs-placement="top" ng-blur="updatedetails_gant(row)">
                    </div>
                </div>

                <div class="gantt-col-owner">
                  <div class="dropdown w-100">

  <button class="btn btn-sm dropdown-toggle w-100 text-start no-caret"
          type="button"
          data-bs-toggle="dropdown" 
        ng-click="focusOwnerSearch($event, row)">

    {{ getOwnerName(getSelectedOwnerId(row)) || 'Select Owner' }}

  </button>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2"
           placeholder="Search..."
           ng-model="row.ownerSearch"
        >

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: row.ownerSearch"
         ng-class="{'active':row.Owner == o.id}"
         ng-click="selectOwner_gantt(row, o,row.type)">

        {{o.name}}

      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="(owners | filter: row.ownerSearch).length == 0">
        No results found
      </div>

    </div>

  </div>

</div>  
                    <%--<div class="d-flex">
 <div class="flex-grow-1">
     
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newowner in row.owners| limitTo:2" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="{{newowner.name}}">
    <div >
      <img ng-if="newowner.profileimage"
         ng-src="{{newowner.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newowner.profileimage" >
      <div class="avatar-title rounded-circle bg-light text-primary" style="width:20px;height:20px;margin-right:2px;">
        {{newowner.shortname}}
      </div>
    </div>

    </div>
    <i class="ri-close-line owner-del" style="position:absolute; top:-6px; right:-2px;cursor:pointer;display:none;color:red;"
       ng-click="removeowners(row, newowner,row.type)">
    </i>

</a>
  <div ng-if="row.owners && row.owners.length > 2"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(row.owners) }}" >

    <div class="avatar-xxs">
        <div class="avatar-title rounded-circle" style="width:20px;height:20px;">
        +{{ row.owners.length - 2 }}
            </div>
    </div>
</div>
</div>
     
 </div>
 <div class="gap-1 flex-shrink-0 align-self-center"><a href="javascript:void(0);" style="display:" type="button"  ng-click="loadSrcOwnersnew(row,'owner',row.type,$index)" data-bs-toggle="modal" data-bs-target="#addowners"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Owner">+</span></a>
</div>

</div>--%>
                </div>

                <div class="gantt-col-status">
                    <span class="gantt-status-pill"
                          ng-style="{'background-color': row.status_color || '#ddd'}">
                       
                        <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge p-0' > {{ row.status_name|| '-' }}</a>
   <div class='dropdown-menu p-0' style='display:'>
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus_gantt(row,newstatus,row.type); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='bi bi-square-fill' ng-style="{'color': newstatus.colorname}" ></i> {{newstatus.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
                    </span>
                </div>


                <div class="gantt-col-progress">
                   
                    <div class="d-flex">
       <div class="flex-grow-1">
      {{row.progress || 0}}
        </div>
        <div class="gap-1 flex-shrink-0 align-self-center" style="float:right;">
    
    <div class="action-menu p-0" >
<span class="action-icon p-0" ><i class="bi bi-three-dots-vertical"></i></span>
 <div class="action-dropdown" style="width:100px!important">
     
   <a href="javascript:void(0);" ng-if="row.type=='milestone'" style="display:" class="btn btn-info btn-xs" type="button"  data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="loadformedit_gantt(row,'milestone','editdetails','/pms/editmile.aspx?id='+row.id+'&pid='+row.ProjectId+'&ctype=ang','Milestone')"><i class="ri-edit-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Edit"></i></a>
   <a href="javascript:void(0);" ng-if="row.type=='task'" style="display:" class="btn btn-info btn-xs" type="button"  data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="loadformedit_gantt(row,'task','editdetails','/pms/edittask.aspx?id='+row.id+'&pid='+row.ProjectId+'&ctype=ang','Task')"><i class="ri-edit-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Edit"></i></a>
   <a href="javascript:void(0);" ng-if="row.type=='subtask'" style="display:" class="btn btn-info btn-xs" type="button"  data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="loadformedit_gantt(row,'subtask','editdetails','/pms/addnewsubtask.aspx?id='+row.id+'&pid='+row.ProjectId+'&ctype=ang','Sub Task')"><i class="ri-edit-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Edit"></i></a>
    <a href="javascript:void(0);" style="display:" class="btn btn-danger btn-xs" type="button" ng-click="deletedata_gantt(row,row.type)"><i class="ri-delete-bin-5-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Delete"></i></a>
     <div class="dropdown" style="display:inline-block;">
    <button class="btn btn-link btn-sm p-0" type="button" 
          data-bs-toggle="dropdown" aria-expanded="false"
          tooltip="Options" >
    <span class="btn-plus2" style="margin-left:20px;width:16px;height:17px;font-size:14px !important" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add">+</span>
  </button>
      

  <div class="dropdown-menu dropdownmenu-secondary p-0" style="min-width: 85px;">
      <div class="d-flex align-item-center">
    <div ng-if="row.type=='milestone'">
      <a href="javascript:void(0);" ng-click="addRowUpDown_mile_gantt($index,row,'up')" class="btn-sm btn-gantt btn-icon">
        <i class="bi bi-arrow-up-square" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Above"></i>
      </a>
    </div>
    <div ng-if="row.type=='milestone'">
      <a href="javascript:void(0);" ng-click="addRowUpDown_mile_gantt($index,row,'down')" class="btn-sm btn-gantt btn-icon">
        <i class="bi bi-arrow-down-square" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Below"></i>
      </a>
    </div>
    <div ng-if="row.type=='task'">
      <a href="javascript:void(0);" ng-click="addRowUpDown_task_gantt($index,row,'up')" class="btn-sm btn-gantt btn-icon">
        <i class="bi bi-arrow-up-square" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Above"></i>
      </a>
    </div>
    <div ng-if="row.type=='task'">
      <a href="javascript:void(0);" ng-click="addRowUpDown_task_gantt($index,row,'down')" class="btn-sm btn-gantt btn-icon">
        <i class="bi bi-arrow-down-square" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Below"></i>
      </a>
    </div>
    <div ng-if="row.type=='subtask'">
      <a href="javascript:void(0);" ng-click="addRowUpDown_subtask_gantt($index,row,'up')" class="btn-sm btn-gantt btn-icon">
        <i class="bi bi-arrow-up-square" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Above"></i>
      </a>
    </div>
    <div ng-if="row.type=='subtask'">
      <a href="javascript:void(0);" ng-click="addRowUpDown_subtask_gantt($index,row,'down')" class="btn-sm btn-gantt btn-icon">
        <i class="bi bi-arrow-down-square" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Below"></i>
      </a>
    </div>
    <div ng-if="row.type=='milestone'">
      <a href="javascript:void(0);"  ng-click="adddefault_gantt(row,row.type)" class="btn-sm btn-gantt btn-icon">
        <i class="ri-list-unordered label-icon align-middle me-1" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Task"></i>
      </a>
    </div>
    <div ng-if="row.type=='task'">
      <a href="javascript:void(0);"  ng-click="adddefault_gantt(row,row.type)" class="btn-sm btn-gantt btn-icon">
        <i class="ri-list-unordered label-icon align-middle me-1" data-bs-toggle="tooltip" 
       data-bs-placement="bottom" 
       data-bs-trigger="hover" 
       title="Add Sub Task"></i>
      </a>
    </div>
          </div>
  </div>
</div>
</div>
        </div>
            

                </div>
 
       
</div>
                </div>
            </div>
        </div>

        <!-- right side -->
        <div class="gantt-right-panel" id="ganttRightPanel">
            <!-- header row 1 -->
            <div class="gantt-header-top" style="display:flex;width:max-content;">
                <div class="gantt-header-group" style="text-align:center;font-weight: bold;"
                     ng-repeat="grp in ganttHeaderGroups track by $index"
                     ng-style="{'width': grp.width + 'px'}">
                    {{grp.label}}
                </div>
            </div>

            <!-- header row 2 -->
            <div class="gantt-header-bottom" style="display:flex;width:max-content;">
                <div class="gantt-cell-head" style="text-align:center;border: 1px solid #eee;flex-shrink: 0;"
                     ng-repeat="cell in ganttCells track by $index"
                     ng-style="{'width': ganttCellWidth + 'px'}">
                    {{cell.label}}
                </div>
            </div>

            <!-- body -->
            <div class="gantt-body-row"
                 ng-repeat="row in ganttRows track by row.uid"
                 data-gantt-row="{{row.uid}}"
                 data-row-index="{{$index}}"
                 data-row-type="{{row.type}}"
                 ng-class="{
                    'row-milestone': row.type=='milestone',
                    'row-task': row.type=='task',
                    'row-subtask': row.type=='subtask',
                    'gantt-row-moving': ganttDraggingRow && ganttDraggingRow.uid == row.uid,
                    'gantt-row-over': ganttDragOverRow && ganttDragOverRow.uid == row.uid,
                    'gantt-row-invalid': ganttInvalidDropRow && ganttInvalidDropRow.uid == row.uid
                 }">

                <div class="gantt-grid">
                    <div class="gantt-grid-cell"
                         ng-repeat="cell in ganttCells track by $index"
                         ng-style="{'width': ganttCellWidth + 'px'}">
                    </div>
                </div>

                <div class="gantt-bar-wrap"
                     ng-if="row.start && row.end"
                     ng-style="getBarStyle(row)">

                    <div class="gantt-bar {{row.type}}" ng-mousedown="startDragBar($event,row)">
                         <span class="resize-handle left"
          ng-mousedown="startResizeBar($event,row,'left')">
    </span>
                        <div class="gantt-bar-progress"
                             ng-style="{'width': (row.progress || 0) + '%'}"></div>

                        <span class="gantt-bar-label">{{row.name}}</span>
                        <span class="resize-handle right"
          ng-mousedown="startResizeBar($event,row,'right')">
    </span>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--<div class="text-center p-4" ng-if="!ganttRows.length">
        No timeline data found
    </div>--%>
</div>


 

<div ng-show="isLoading" class="loader-overlay">
<div class="loader"></div>
</div>
</div>
   <div ng-if="view === 'table'" class="zoom-bar" id="zoomBar" style="float:right;">
    <span>-</span>
    <input type="range" min="50" max="150"
           ng-model="zoomLevel"
           ng-change="applyZoom ()" />
    <span>+</span>
    <span>{{zoomLevel}}%</span>
</div>
    </div>




<div ng-show="projectState.showAddForm" class="panel">

    
 
                                 

</div>
    <style>
        

#dropZone {border:2px dashed #ccc;padding:20px;text-align:center;border-radius:10px;cursor:pointer;transition: 0.3s;}
    </style>
    <div id="addfiles" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
    <div class="modal-dialog modal-lg">
        <div class="modal-content" style="height:500px;">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title" id="lblfiles">{{ modalTitle }}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                
                <!-- Drop Zone -->
                <div id="dropZone" class="drop-zone"
                     ng-click="openFilePicker($event)"
                     ng-class="{'highlight': isDragging}">
                    <i class="ri-upload-cloud-2-fill fs-6xl"></i><br />
                    Drag & Drop files here or click to upload
                </div>

                <!-- Hidden file input for multiple files -->
                <input type="file" id="fileupload" multiple style="position:absolute; left:-9999px"
                       onchange="angular.element(this).scope().filesSelected(this.files)">

                <!-- File previews -->
                <%--<div class="preview-container" style="margin-top:10px; display:flex; flex-wrap:wrap; gap:8px;">--%>
                <div class="preview-container" style="margin-top:10px; display:none; flex-wrap:wrap; gap:8px;">
                    <div ng-repeat="file in uploadedFiles" class="file-preview" style="width:60px; text-align:center;">
                        <img ng-if="file.type.startsWith('image/')" ng-src="{{file.data}}" 
                             style="width:60px; height:60px; object-fit:cover; border-radius:4px;">
                        <div ng-if="file.type==='application/pdf'" 
                             style="width:60px; height:60px; display:flex; justify-content:center; align-items:center; font-size:24px; background:#f0f0f0; border-radius:4px;">
                            📄
                        </div>
                        <span style="font-size:10px; display:block; overflow:hidden; text-overflow:ellipsis;">{{file.name}}</span>
                    </div>
                </div>

                <div style="max-height:300px;overflow-y:auto;">
                  <table class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>
  <tr ng-repeat="file in showfileslist|orderBy:'-id' track by file.id">

                          <td width="45px">
                              <div class="attch_icon">

                                  <i class="bi"
                                      ng-class="{
           'bi-file-earmark-image': file.fext && (file.fext.toLowerCase() == '.jpg' || file.fext.toLowerCase() == '.png'),
           'bi-file-earmark-pdf-fill': file.fext && file.fext.toLowerCase() == '.pdf',
                                      'bi bi-file-earmark-excel-fill': file.fext && (file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx'),
                                      'bi bi-file-earmark-word-fill': file.fext && (file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.doc')
         }"></i>
                              </div>
                          </td>

                          <td>{{ file.filedes.replace(file.fext,'') }}</td>

                          <td class='text-right' style='width:30px;vertical-align:middle;'>
                              <a ng-href="{{ (file.fext.toLowerCase() == '.pdf' || file.fext.toLowerCase() == '.doc' || file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx') 
                      ? '/pms/ValidateDocument.aspx?isview=1&count=1&id=' + file.id 
                      : file.fileurl }}" 
       target="_blank" 
       class="btn btn-success btn-icon btn-sm">
      <i class="ri-eye-line"
         data-bs-toggle="tooltip"
         data-bs-placement="top"
         data-bs-trigger="hover"
         title="View"></i>
    </a>
                              <a href="" ng-click="removedocumentnew(file.id)" class='btn btn-danger btn-icon btn-sm ms-1 me-1'><i class='ri-delete-bin-5-line' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='Delete'></i></a>
                          </td>

                      </tr>
</table>
                </div>

            </div>
        </div>
    </div>
</div>
   <div id="addowners" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
    <div class="modal-dialog modal-samll">
        <div class="modal-content" style="height:225px;">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title" id="lblowners">{{ modalTitle }}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body p-3 pb-2" >                
               <div  id="ownerDropdown" class="dropdown" style="position:relative; width:100%;" click-outside="closelist()">

    <!-- Input box showing tags -->
    <div class="input-tags" >
        <span class="tag" ng-repeat="owner in getSelectedOwners()">
            {{owner.name}} <span class="remove" ng-click="removeOwner(owner.id); $event.stopPropagation()"><i class="bi bi-x-lg"></i></span>
        </span>
        
    </div>
<input type="text" class="pmsinput"
               ng-model="searchOwner"
               placeholder="Select owners..."
               ng-focus="showOwnerDropdown = true"
               style="width:100%;" />
    <!-- Dropdown -->
    <div class="dropdown-menu"
         ng-show="showOwnerDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

        <!-- Search input -->
        <%--<input type="text"
               ng-model="searchOwner"
               placeholder="Search..."
             ng-keydown="handleOwnerKeydown($event)" ng-focus="openOwnerDropdown()" 
           placeholder="Search owners..."
               style="border:0px; width:100%; margin-bottom:5px; padding:3px;" />--%>

        <!-- Owner list -->
        <div ng-repeat="owner in owners | filter:searchOwner"
             ng-click="toggleOwner(owner)"
             ng-class="{'selected-owner': selectedOwnerIds.includes(owner.id)}"
             style="padding:3px; cursor:pointer;">
            {{owner.name}}
        </div>


        <div ng-if="(owners | filter:searchOwner).length == 0" style="padding:3px; color:#888;">
            No owners found
        </div>
    </div>
</div>
                <style>
                    .input-tags {
    border: px solid #ccc;
    padding: 3px;
    min-height:30px;
    display:flex;
    flex-wrap:wrap;
    cursor:text;
}
.input-tags .tag {
    background:#007bff;
    color:#fff;
    padding:3px 6px;
    border-radius:3px;
    margin:2px;
    display:flex;
    align-items:center;
}
.input-tags .tag .remove {
    margin-left:5px;
    cursor:pointer;
    font-weight:bold;
}
.selected-owner {
    background:#f0f0f0;
}
.dropdown-menu div:hover {background-color: #e0e0e0;}

                </style>

            </div>
        </div>
    </div>
</div>
    <div class="modal fade" id="confirmCopyModal" tabindex="-1">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">

      <div class="modal-header">
        <h5 class="modal-title">Confirm Copy</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
      </div>

      <div class="modal-body">
        Are you sure you want to copy data from selected project?
      </div>

      <div class="modal-footer">
        <button type="button" class="btn btn-secondary"
                data-bs-dismiss="modal">Cancel</button>

        <button type="button" class="btn btn-primary"
                ng-click="confirmCopy()">
          Yes, Copy
        </button>
      </div>

    </div>
  </div>
</div>
   <div id="addcopy" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
    <div class="modal-dialog modal-samll" style="height:150px;width:300px!important">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Copy From</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" id="btnclosecopy"></button>
            </div>
            <div class="modal-body p-3 pb-2" >                
               <div class="dropdown w-100 ">
                   <button class="btn btn-sm dropdown-toggle w-100 text-start no-caret"
        type="button"
        data-bs-toggle="dropdown">

    {{ selectedprojectname || 'Select Project' }}

</button>
<div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control mb-2"
           placeholder="Search..."
           ng-model="searchText">

    <!-- LIST -->
    <div style="max-height:200px; overflow:auto;">

        <a href="javascript:void(0);"
           class="dropdown-item"
           ng-repeat="p in projectmasterlist | filter:searchText"
           ng-click="selectProject(p)">

            {{p.ProjectName}}

        </a>

        <!-- NO RESULT -->
        <div class="text-muted p-2"
             ng-if="(projectmasterlist | filter:searchText).length == 0">
            No results found
        </div>

    </div>

</div>

</div>
                <div class="row" style="text-align:center;margin-top:20px">
                    <div class="col-4"></div>
                    <div class="col-4">
                        <a type="button" class="btn btn-primary" ng-click="confirmCopy()">Submit</a>

                    </div>
                    <div class="col-4"></div>
                    
                </div>
            </div>
        </div>
    </div>
</div>
  <input type="hidden" id="hdneditid" value="0" />
    <div class="offcanvas offcanvas-end" id="addmile" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title">
             <label id="lbltitlemile">{{ modalTitle }}</label>  

        </h5>
        <div class="hstack gap-2">
            <button style="display:none" type="button" onclick="fsFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button style="display:none" type="button" onclick="fseFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" id="btncloseedit"  ng-click="closemile()" aria-label="Close"><i class="ph-x-bold"></i></button>
            <div style="display:none"><button type="button" id="btnclsmile" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
        </div>
    </div>
    <iframe id='editdetails' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      

</div>
    

    <div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup_issue" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="offcanvas-title" id="lblctitle">{{ modalTitle }}</h5>
                <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" ng-click="closepopup_canvas_issue()"   aria-label="Close"><i class="ph-x-bold"></i></button>
            </div>

        <div style="display:none;">
     <button type="button" id="btnclosemainpopup_issue" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>
    <div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup_dependency" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="offcanvas-title" id="lblctitle">{{ modalTitle }}</h5>
                <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" ng-click="closepopup_canvas_dependency()"   aria-label="Close"><i class="ph-x-bold"></i></button>
            </div>

        <div style="display:none;">
     <button type="button" id="btnclosemainpopupD" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoominD" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomoutD" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popupD' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>
    <div class="offcanvas offcanvas-end" id="addissue" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title">
             <label id="Label3" runat="server">Add Issue</label>  

        </h5>
        <div class="hstack gap-2">
            <button style="display:none" type="button" onclick="fsFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button style="display:none" type="button" onclick="fseFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  ng-click="closeissue()" aria-label="Close"><i class="ph-x-bold"></i></button>
            <div style="display:none"><button type="button" id="btnclsissue" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ph-x-bold"></i></button></div>
        </div>
    </div>
   
   	   <div class="offcanvas-body p-0 overflow-hidden">
        <div data-simplebar style="height: calc(100vh - 112px);">
            <div class="acitivity-timeline p-4">
                <div class="row gy-4">
                    <div class="col-lg-12">
                        <div class="lead_form_body p-0">
                            <div class="row">
                               

                                 <div class="col-xxl-12 col-lg-12" id="divproject">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Project", "2")%></label>
                                    </div>
                                 
                                      <select class="form-control"
            ng-model="newissueData.projectId"
            ng-options="p.id as p.name for p in projectList">
        <option value="">-- Select Project --</option>
    </select>

                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divmilestone">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Milestone", "2")%></label>
                                    </div>

                                   
                                       <cc1:DropDownListChosen ID="Milestoneid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Milestone ..." AllowSingleDeselect="true" AutoPostBack="true" >
                                                </cc1:DropDownListChosen>
                                            



                                </div>

                                 <div class="col-xxl-12 col-lg-12" id="divptasklist" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task List", "2")%></label>
                                    </div>
                                
                                    
                                       <cc1:DropDownListChosen ID="tasklistid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="task list ..." AllowSingleDeselect="true" AutoPostBack="true" >
                                                </cc1:DropDownListChosen>
                                            


                                </div>

                               <div class="col-xxl-12 col-lg-12" id="divtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Task", "2")%></label>
                                    </div>
                                  

                                    
                                       <cc1:DropDownListChosen ID="taskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Task ..." AllowSingleDeselect="true" AutoPostBack="true" >
                                                </cc1:DropDownListChosen>
                                            


                                </div>


                                <div class="col-xxl-12 col-lg-12" id="divsubtask">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Sub Task", "2")%></label>
                                    </div>
                                  

                                   
                                       <cc1:DropDownListChosen ID="subtaskid" CssClass="form-control" runat="server" width="450px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Sub Task ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>
                                           


                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Issue", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator9" 
                                                ControlToValidate="Issues" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                    <input type="text" id="Issues" runat="server" placeholder="Issues" class="form-control">
                                </div>
                                
                              


                                 <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Description", "2")%></label>
                                    </div>
                                    <textarea id="Description" runat="server" placeholder="Description" class="form-control"></textarea>
                                </div>


                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Owner", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="Owner" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Owner ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnowner" runat="server" Value="0" />
                                      

                                </div>



                                  <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Team", "2")%></label>
                                    </div>
                                   
                                      <asp:DropDownList ID="team" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Team ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnteam" runat="server" Value="0" />
                                      

                                </div> 


                                <div class="col-xxl-12 col-lg-12"  id="divreminder">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder", "2")%>  &nbsp;&nbsp;</label>
                                    </div>
                                   
                                     
                                              <cc1:DropDownListChosen ID="Reminder" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true" onchange="GetReminderDtls()">
                                         <asp:ListItem  value="0">-- Select --</asp:ListItem>
                                          <asp:ListItem  value="1">Based on Due Date</asp:ListItem>
                                          <asp:ListItem  value="2">Specific Date</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                              

                                </div>

                                <div class="col-xxl-12 col-lg-12" id="divremindertype">
                                <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Reminder Type", "2")%> &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator10" 
                                                ControlToValidate="ReminderType" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>

                                    
                                     

                                              <cc1:DropDownListChosen ID="ReminderType" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Access ..." AllowSingleDeselect="true" onchange="GetRemindertypeDtls()">
                                         <asp:ListItem  value="1">Daily</asp:ListItem>
                                          <asp:ListItem  value="2">On due date</asp:ListItem>
                                          <asp:ListItem  value="3">Day(s) before due date</asp:ListItem>
                                                </cc1:DropDownListChosen>

                                              

                                     </div>

                                      <div class="col-xxl-12 col-lg-12" id="divDays">
                                        <div class="d-flex align-items-center">
                                            <label class="form-label flex-grow-1">Enter Day(s)</label>
                                        </div>
                                        <input type="text" id="Days" runat="server" placeholder="Days" class="form-control">
                                    </div>

                                <div class="col-xxl-6 col-lg-6" id="divChooseDate" style="display:none">
                                <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Choose Date</label>
                                    </div>
                                
                                      
                                    
                                      <input type="date" class="form-control"
           ng-model="newissueData.ChooseDate" onclick="if(this.showPicker){this.showPicker();}">   


                                </div>

                                   <div class="col-xxl-6 col-lg-6" id="Divtime" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Time</label>
                                    </div>

                                   
                                    <input type="text" id="Time" runat="server" class="form-control">
                                             
                                       </div>


                                  <div class="col-xxl-12 col-lg-12"  id="divNotifyusers">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Notify users", "2")%></label>
                                    </div>
                                
                                      <asp:DropDownList ID="Notifyusers" CssClass="form-control" runat="server" width="450px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Notify User ..." AllowSingleDeselect="true">
                                                    </asp:DropDownList>

                                             <asp:HiddenField ID="hdnNotifyusers" runat="server" Value="0" />
                                     
                                    </div>
                               

                                

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Assign To &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator5" 
                                                ControlToValidate="AssignTo" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                            
                                    

                                             <cc1:DropDownListChosen ID="AssignTo" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="Assign To ..." AllowSingleDeselect="true">
                                                </cc1:DropDownListChosen>

                                               



                                </div>



                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Start Date</label>
                                    </div>
                                  
                                    <input type="date" class="form-control"
           ng-model="newissueData.startDate" onclick="if(this.showPicker){this.showPicker();}">
                                         
                                </div>
                                <div class="col-xxl-6 col-lg-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">End Date</label>
                                    </div>
                                  
                                    <input type="date" class="form-control"
           ng-model="newissueData.endDate" onclick="if(this.showPicker){this.showPicker();}">
                                            
                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Status &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator6" 
                                                ControlToValidate="IssueStatus" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                     

                                             <cc1:DropDownListChosen ID="IssueStatus" CssClass="form-control" runat="server" Width="220px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Status..." AllowSingleDeselect="true" onchange="AddNewIssuestatus()">                                                        
                                                    </cc1:DropDownListChosen>
                                            

                                         <div id="dvnewissuestatus" style="display:none">
                                          <input type="text" id="txtnewissustatus" class="form-control" placeholder="Status"  runat="server" />
                                           <input  data-jscolor="{}" id="stustprintColor" class="jscolor form-control" runat="server">
                                             </div>
                                            <asp:HiddenField ID="hdnIssueStatus" runat="server" Value="0" />
                                           


                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Priority", "2")%>  &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" 
                                                ControlToValidate="Priority" InitialValue="0" Display="Dynamic" ValidationGroup="btnsave" 
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                      

                                     <cc1:DropDownListChosen ID="Priority" CssClass="form-control" runat="server" width="220px"
                                                    NoResultsText="No results match."
                                                    DataPlaceHolder="priority ..." AllowSingleDeselect="true" onchange="AddNewpriority()">
                                         </cc1:DropDownListChosen>
                                             
                                                 <div id="dvnewpriority" style="display:none">
                                          <input type="text" id="txtnewpriority" class="form-control" placeholder="priority"  runat="server" />
                                            <input  data-jscolor="{}" id="txtpriortycolor" class="jscolor form-control" runat="server">
                                                     </div>
                                               <asp:HiddenField ID="hdnPriority" runat="server" Value="0" />
                                          

                                </div>


                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Issue Type &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator7" 
                                                ControlToValidate="IssueType" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                 
                                   


                                               <cc1:DropDownListChosen ID="IssueType" CssClass="form-control" runat="server" Width="220px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Issue Type..." AllowSingleDeselect="true" onchange="AddNewIssueType()">                                                        
                                                    </cc1:DropDownListChosen>
                                              
                                         <div id="dvnewissueTypes" style="display:none">
                                          <input type="text" id="txtnewissutype" class="form-control" placeholder="Issue Type"  runat="server" />
                                             <input  data-jscolor="{}" id="txtIssuescolor" class="jscolor form-control" runat="server">
                                              </div>
                                               <asp:HiddenField ID="hdnIssueType" runat="server" Value="0" />
                                         

                                </div>

                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Replicability &nbsp;&nbsp;<asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator8" 
                                                ControlToValidate="Replicability" InitialValue="0" ValidationGroup="btnsave"
                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                
                                    

                                              <cc1:DropDownListChosen ID="Replicability" CssClass="form-control" runat="server" Width="220px"
                                                        NoResultsText="No results match."
                                                        DataPlaceHolder="Select Replicability..." AllowSingleDeselect="true" onchange="AddNewReplicability()">                                                        
                                                    </cc1:DropDownListChosen>
                                              
                                         <div id="dvnewReplicability" style="display:none">
                                          <input type="text" id="txtReplicability" class="form-control" placeholder="Replicability"  runat="server" />
                                              <input  data-jscolor="{}" id="txtReplicabilitycolor" class="jscolor form-control" runat="server">
                                              </div>
                                               <asp:HiddenField ID="hdnReplicability" runat="server" Value="0" />
                                          


                                </div>


                                <div class="col-xxl-12 col-lg-12" style="display:none">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">File(s) Upload</label>
                                    </div>
                                    

                                        <div style="display:none;">
                                        <input type="file" id="UploadImage" runat="server" class="form-control" style="border-color:#777;border-radius:0px;" aria-label="Upload">
                                        <button class="btn btn-success" style="border-radius:0px;"  type="button">Upload</button>  
                                            </div>
                                        
                                        <asp:FileUpload ID="fileupload1" CssClass="form-control" runat="server" OnChange="ShowPreviewnew();"/>
                                        <a href="#" id="imgpath" target="_blank" style="display:none"> <img src="/pms/assets/images/user.jpg" id="empimg" style="height:50px;width:50px"/></a>
                                    <div id="previewContainer1" style="margin-top: 10px;"></div>

                                </div>


                                                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
   
    <div class="offcanvas-foorter border p-3 pt-1 pb-1 text-center"> 
         <div id="pnlimg" style="display: none;">
                        <img src="/Image/loading8.gif" runat="server" id="img2" />
                    </div>
                    <div class="btn-group">
                      
                        <div class="btn-group" id="divsub" style="display: block;">
                            <a  href="javascript:void(0);" class="btn btn-success" id="btnsave" onclick="SvaeData('save');" aria-label="Save">Save</a>
                            <a href="javascript:void(0);" class="btn btn-secondary" id="btnsaveandmore" onclick="SvaeData('savemore');">Save & Add More</a>
                            
                        </div>
                    </div>
      <%--  <a href="javascript:void(0);" class="btn btn-success">Save</a>--%>
          <a  href="javascript:void(0);" class="btn btn-danger" onclick="showdivimg()"  aria-label="Close">Close</a>
        <a href="javascript:void(0);" class="btn btn-danger" id="btnclose" style="display:none" data-bs-dismiss="offcanvas" aria-label="Close">Close</a>
    </div>             

    
      
    

</div>
  
    


</div>
    </div>
    <style>
        .col-status { width: 50px; left: 410px; }
    </style>
    
 
    <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script>
</asp:Content>

