<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular_user.master" AutoEventWireup="true" CodeFile="mytaskuser.aspx.cs" Inherits="mytaskuser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/pms/angularmaster/jsapp/pmsapp.js?v=<%=DateTime.Now.Ticks %>"></script> 
<script src="/pms/projects/app/usertask.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <script src="/pms/projects/app/usertask.service.js?v=<%=DateTime.Now.Ticks %>"></script>
     <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script> 
    
   <link rel="stylesheet" href="/pms/new/assets/css/daterangepicker.css" type="text/css" />
     <style>
         .tag-popup {
    width: 220px;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0 3px 15px rgba(0,0,0,.18);
    padding: 10px;
}

.tag-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 15px;
}

.tag-header span {
    font-size: 14px;
    color: #7b8794;
}

.tag-actions {
    display: flex;
    gap: 6px;
}

.icon-btn {
    width: 22px;
    height: 22px;
    border-radius: 50%;
    border: 1px solid #d7dce2;
    background: #fff;
    cursor: pointer;
    padding: 0;
}

.icon-btn i {
    font-size: 12px;
}

.tag-list {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
}

.tag-item {
    padding: 7px 14px;
    border-radius: 20px;
    font-size: 13px;
    cursor: pointer;
    display: inline-block;
}

.tag-yellow {
    background: #ffc400;
    color: #000;
}

.tag-red {
    background: #ff0000;
    color: #fff;
}
     </style>
    <style>
        .badge-border {	border-left: 2px solid}
        .badge-label {margin-right:5px;border-radius:20px 4px 4px 20px;	position:relative}
        body {background-color: #f1f5f7;}
        input#daterange {
            border: 0px !important;
            padding: 3px 0 0 0;
            width: 145px;
        }
        input#daterange:focus {
            outline: 0;
        }
.avatar-group .avatar-group-item:nth-child(1) .avatar-title {background:#fef4e4!important;color:#f7b84b!important;}
.avatar-group .avatar-group-item:nth-child(2) .avatar-title {background:#ddfbf9!important;color:#18ddcd!important;}
.avatar-group .avatar-group-item:nth-child(3) .avatar-title {background:#f8f0f7!important;color:#804574!important;}
    </style>
    <style>
      

.spent-time-cell .start-timer{
    cursor:pointer;
    display:inline-flex;
    align-items:center;
    gap:6px;
}

.spent-time-cell .running-timer{
    position:relative;
    display:inline-flex;
    align-items:center;
    gap:6px;
}

.spent-time-cell .timer-text{
    color:#ff6a4d;
    font-weight:500;
}
.spent-time-cell .timer-text-green{
    color:#0cd71a !important;
    font-weight:500;
}

.spent-time-cell .timer-controls{
    position:absolute;
    top:50%;
    
    transform:translateY(-50%);
    margin-left:10px;

    background:#222;
    border-radius:6px;
    padding:3px 4px;

    display:none;
    align-items:center;
    gap:10px;

    box-shadow:0 2px 10px rgba(0,0,0,.2);
    z-index:99;
}

.spent-time-cell .running-timer:hover .timer-controls{
    display:flex;
}

.spent-time-cell .timer-controls button{
    border:0;
    background:none;
    padding:0;
}

.spent-time-cell .timer-controls i{
    font-size:22px;
    cursor:pointer;
}

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
    </style>
    
    <style>
        .preview-item {
    width: 30px;
    height: 30px;
    border-radius: 50%;
    object-fit: cover;
    margin: 5px;
    display: inline-block;
    box-shadow: 0 0 2px rgba(0,0,0,0.2);
    text-align:center;
    line-height:30px;
    float: left;
    overflow:hidden;
    transition: transform 0.3s ease;
}
        .preview-item:hover {transform: scale(1.25);}
.preview-item img {width: 30px;height: 30px;border-radius: 50%;}
        .tag-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:8px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}
a.tag-wrp {position:relative;}
.tag-wrp:hover .tag-del {display:block;}
    </style>
    <style>
        .search_box {position:relative;}
.search_box i {position:absolute;top:4px;left:7px;font-size:16px;}
.search_box input.form-control {padding-left:28px;color:#000;border-radius:4px;height:32px;border: 1px solid #ddd !important;}
.search_box input.form-control:focus {border-color:#804574 !important;}
    </style>
    <style>
  .table-fix-wrap {
    width: 100%;
    overflow: auto;
    position: relative;


}

.project-table th,
.project-table td {
    white-space: nowrap;
    vertical-align: middle;
    background: #fff;
    border-style:solid;
}

.project-table thead th {
   
    background: #f8f9fa;
    border: 1px solid #e5e7eb;
}

  
    </style>
    
    <style>

.custom-range-box{
    width:260px;
    float:left;
    padding:10px;
}

.custom-range-box .form-control{
    height:34px;
    font-size:13px;
}

.custom-range-box label{
    display:block;
    margin-bottom:4px;
    font-weight:600;
}

</style>
    <style>
        .project-table td {
    white-space: nowrap;
    vertical-align: middle;
    background: #fff;
    border-style:solid;
}
.project-table thead th {
   
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

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main-wrapper" ng-app="PMSerp" ng-controller="usertaskCtrl">
        <div class="sub-header">
            <div class="row">
                <%--<div class="col-xxl-3 col-lg-3 col-md-4 col-sm-12">
                    <div class="sub-left">
                        <div class="sub-page-title">Task</div>
                        <div class="d-flex align-items-center gap-1 ms-3">
                            
                           <div class="d-flex align-items-center">
                                <i class="ri-calendar-line text-muted me-1" style="font-size:16px;line-height: 1;"></i>
                                <input type="text" id="daterange" readonly/>
                                <i class="ri-arrow-down-s-line" style="font-size:18px;line-height: 1;"></i>
                            </div> 
                            
                        </div>
                    </div>
                </div>--%>
                <div class="col-xxl-3 col-lg-3 col-md-4 col-sm-12">
    <div class="sub-left">

        <div class="sub-page-title">Task</div>

        <div class="d-flex align-items-center gap-2 ms-3">

          <select class="form-select" ng-model="pageSize"  ng-options="size for size in pageSizes" style="width:55px;"></select>

            <!-- CALENDAR ICON -->
            <i class="ri-calendar-line text-muted ms-1"
               style="font-size:16px;line-height:1;"></i>

            <!-- DATE RANGE INPUT -->
            <input type="text" id="daterange" readonly
                   style="border:none; background:transparent;cursor:pointer" />

            <!-- ARROW -->
            <i class="ri-arrow-down-s-line"
               style="font-size:18px;line-height:1;"></i>

        </div>
    </div>
</div>
                <div class="col-xxl-6 col-lg-6 col-md-8 col-sm-12">
                    <ul class="top-list-sales">
                        <li ng-repeat="c in mytasklist_bystatus">
                            <a href="#">
                                <div class="tile">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <%--<div class="tile-icon" ng-style="{'background-color':getContrastColor(c.statuscolor),'color':c.statuscolor }"><i class="ri-folder-2-line"></i></div>--%>
                                            <div class="tile-icon" ng-if="c.statusicon==''" ng-style="{'color':c.statuscolor }"><i class="ri-folder-2-line"></i></div>
                                            <div class="tile-icon" ng-if="c.statusicon" ng-style="{'color':c.statuscolor }"><i ng-class="c.statusicon"></i>
</div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>{{c.statusname}}</span></div>
                                            <%--<div class="tile-body" ng-style="{'color': c.statuscolor}">{{c.total}}</div>--%>
                                            <div class="tile-body">{{c.total}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        
                    </ul>
                </div>
                <div class="col-xxl-3 col-lg-3 col-md-6 col-sm-12" ng-show="multiselect=='1'">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                        <div class="btn-group d-block mt-1">
                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="d-block" aria-expanded="false" ng-style="{'color':(selectedStatus.length > 0 &&selectedStatus[selectedStatus.length-1].colorname &&selectedStatus[selectedStatus.length-1].colorname != '#FFFFFF')? selectedStatus[selectedStatus.length-1].colorname: '#000'}">

    {{ selectedStatus.length >0? selectedStatus[selectedStatus.length-1].name: 'Status' }}<i class="ri-arrow-down-s-line"></i></a>
                            <div class="dropdown-menu p-0" style="border-radius: 0px;">
                                <ul class="sttus-list">
                                     <li ng-repeat="newstatus in statuslists|filter:name" >
                 <div class='dropdown-item p-0'>
                    <%--<a class='' href='javascript:void(0);'  ng-style="{'color':(newstatus.colorname && newstatus.colorname != '') ?newstatus.colorname:'#000'}" ng-click="selectStatus_multi(newstatus); $event.preventDefault()">
                        <i class='bx bxs-circle' ></i>{{newstatus.name }}
                    </a>--%>
                     <a href="javascript:void(0);"
   ng-style="{
      'color':
      (!newstatus.colorname || newstatus.colorname == '' || newstatus.colorname == '#FFFFFF')
      ? '#000'
      : newstatus.colorname
   }"
   ng-click="selectStatus_multi(newstatus); $event.preventDefault()">

    <i class="bx bxs-circle"></i>{{newstatus.name}}
</a>
                    </div>
    </li>
                                </ul>
                            </div>
                        </div>
                       
                        <div class="btn-group d-block mt-1">
                            <a href="javascript:void(0);" type="button" data-bs-boundary="window" class="d-block" aria-expanded="false"  data-bs-toggle="modal" data-bs-target="#addtags_multi" ng-click="clearselectedtag()">Tag<i class="ri-arrow-down-s-line"></i></a>
                            
                        </div>
                       
                    </div>
                </div>

                <div class="col-xxl-3 col-lg-3 col-md-6 col-sm-12" ng-show="multiselect=='0'">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                       <div class="search_box me-2"  ng-show="freesearch=='1'">                           
                           <input type="text" class="form-control" ng-model="searchText" placeholder="Search..." />
                            <i class="ri-search-line"></i>
                        </div>

                        <button class="sub-soft-btn" data-bs-toggle="tooltip" title="Search" ng-click="viewsearch()"><i class="ri-search-line"></i></button>
                        <a href="mytask.html" class="sub-soft-btn active" data-bs-toggle="tooltip" title="List View"><i class="ri-list-check"></i></a>
                        <a href="mytask-kanban.html" class="sub-soft-btn" data-bs-toggle="tooltip" title="Kanban View"><i class="las la-chart-bar2"></i></a>
                        <a href="mytask-timesheet.html" class="sub-soft-btn" data-bs-toggle="tooltip" title="Timesheet"><i class="bx bx-server"></i></a>
                        <a href="mytask-weekly.html" class="sub-soft-btn" data-bs-toggle="tooltip" title="Weekly View"><i class="ri-time-line"></i></a>
                        <a href="mytask-calendar.html" class="sub-soft-btn" data-bs-toggle="tooltip" title="Calendar"><i class="bx bx-calendar"></i></a>
                        <button class="sub-soft-btn" data-bs-toggle="tooltip" title="Filter"><i class="ri-filter-line"></i></button>
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
        
        <div  ng-if="isload=='1'">
        <div class="wrapper_inner p-0">
            <div class="table-responsive-new">
                <table id='tbDetails' class="table table-hover table-striped tbl_new project-table">
                    <thead>
                        <tr>
                            <th class="color_border" style="text-align:right;">
                                  <input type="checkbox" class="form-check-input" id="chkAll"   onclick="angular.element(this).scope().selectall(this.checked)">

                            </th>
                            <th>Task</th>
                            <th>Description</th>
                            <th>Project Name</th>
                            <th>Priority</th>
                            <th>Status</th>
                            <th>Total Completion</th>
                            <th>Tags</th>
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Timeline</th>
                            <th>Assigned Time</th>
                            <th style="min-width:160px;">Spent Time</th>
                            <th>Project Head</th>
                            <th style="min-width:120px;">Team</th>
                        </tr>
                    </thead>
                    <tbody>
                       
                       <%--<tr dir-paginate="c in mytasklist|orderBy:sortColumn:reverseSort| itemsPerPage: pageSize | filter:searchText track by $index" current-page="currentPage">--%>
                        <%--<tr dir-paginate="c in (mytasklist | filter:searchText | orderBy:sortColumn:reverseSort)| itemsPerPage: pageSize track by $index" current-page="currentPage">--%>
       <tr dir-paginate="
        c in mytasklist
        | filter:searchText
        | orderBy:sortColumn:reverseSort
        | itemsPerPage:pageSize
        "
    current-page="currentPage">

                            <td class="color_border" style="border-left-color:#ffc300 !important;" >
                                <input class="form-check-input" type="checkbox" ng-model="c.selected" ng-change="checkselect(c)">
                            </td>
                            <td><span style="display:none">{{ ((currentPage - 1) * pageSize) + $index + 1 }}</span>{{c.Subtaskname}}</td>
                            <td>{{c.SubTaskDescription}}</td>
                            <td>{{c.ProjectName}}</td>
                            
            <td style="text-align:center;" ng-style="{'background-color': c.priority_color}">
   <div class="badge priority" >{{c.priority_name}}</div>

            </td>
       

                           
                          
                            <td>
                                <div class="btn-group d-block">
                                    <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="d-block" ng-style="{'color':c.status_color}" ng-click="
      c._oldcomplete_value = c.complete_value;
      c._oldpending_value = c.pending_value;
      
   "><i class='bx bxs-circle'></i>{{c.status_name }}</a>
                                    <div class="dropdown-menu p-0" style="border-radius:0;">
                                        <ul class='sttus-list'>
                                            <%--<li><a class='text-warning' href='#'><i class='bx bxs-circle' ng-style="{'color':c.status_color}"></i>{{c.status_name }}</a></li>--%>
                                           <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c,newstatus); $event.preventDefault()" >
                 <div class='dropdown-item p-0'>
                    <%--<a class='' href='javascript:void(0);' ng-style="{'color':(newstatus.colorname && newstatus.colorname != '') ? newstatus.colorname:'#000'}" ng-click="selectStatus(c,newstatus); $event.preventDefault()">
                        <i class='bx bxs-circle' ></i>{{newstatus.name }}
                    </a>--%>
                     <a href="javascript:void(0);"
   ng-style="{
      'color':
      (!newstatus.colorname || newstatus.colorname == '' || newstatus.colorname == '#FFFFFF')
      ? '#000'
      : newstatus.colorname
   }"
   ng-click="selectStatus(c,newstatus); $event.preventDefault()">

    <i class="bx bxs-circle"></i>{{newstatus.name}}
</a>
                    </div>
    </li>
                                        </ul>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div class="d-block mb-0">
                                        <div style="font-size:11px !important;color:#777;">{{c.complete_value}}%</div>
                                    </div>
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar bg-success" role="progressbar" style="width:{{c.complete_value}}%;"></div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <%--<span class="badge bg-danger priority" style="background:#ff6a00;">Important</span>--%>
                               <%-- <a ng-repeat="tag in c.tags|limitTo:1" ng-click="viewtags(c,'subtask'); $event.preventDefault()" class="badge priority tag-wrp"  ng-style="{'background':'#' + tag.tagcolor}">
                                </a>--%>
                                <a ng-repeat="tag in c.tags | limitTo:1"
   class="badge priority tag-wrp"
   ng-style="{
      'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
    {{tag.tagname}}
                                    <i class="ri-close-line tag-del" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="Remove" ng-click="removetags(c,tag.id)"></i>
</a>
                                
                              
                                <span  ng-if="c.tags.length>1" class="cursor-pointer" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingtags(c.tags) }}">{{c.tags.length-1}}</span>
                               
                                

                                <a href="javascript:void(0);" ng-click="Addtags(c,c.tasktype)" data-bs-toggle="modal" data-bs-target="#addtags"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Tags'>+</span></a>
                            </td>
                            
                           <td>{{ c.Startdate | date:'dd/MM/yyyy' }}</td>
                            <td>{{ c.Targetdate | date:'dd/MM/yyyy' }}</td>
                            <td>{{ c.Startdate | date:'dd/MM/yyyy' }}-{{ c.Targetdate | date:'dd/MM/yyyy' }}</td>
                            <td>{{ c.TaskDuration}} hrs</td>
                            <td class="spent-time-cell">

    <!-- before start -->
  
    <div class="start-timer"
         ng-if="!c.isrun"
         ng-click="timerstart(c)">

        <i class="bx bx-play-circle text-success"
           style="font-size:18px;margin-top:2px;"></i>

        <span class="text-success">
            Start Timer
        </span>

    </div>

    <!-- after start -->
    <div class="running-timer"
         ng-if="c.isrun">

        <!-- timer icon -->
        <i class="bx bx-stopwatch text-danger"
           style="font-size:18px;"></i>

        <!-- timer text -->
         <%--<span class="timer-text">
            {{c.timerText || '00:00:00'}}
        </span>--%>
        
        <span ng-if="c.isexceed == 1" class="timer-text">
    {{c.timerText || '00:00:00'}}
</span>

<span ng-if="c.isexceed == 0" class="timer-text-green">
    {{c.timerText || '00:00:00'}}
</span>

        <!-- hover buttons -->
        <div class="timer-controls">

    <!-- timer text inside hover -->
    <span class="text-white fw-bold"
          style="font-size:13px;">
        {{c.timerText || '00:00:00'}}
    </span>

    <!-- pause -->
    <button type="button"
            ng-if="!c.ispause"
            ng-click="timerpause(c)" data-bs-toggle="modal" data-bs-target="#addnotes">

        <i class="ri-pause-circle-fill text-warning"
           title="Pause"></i>

    </button>

    <!-- play -->
    <button type="button"
            ng-if="c.ispause"
            ng-click="timerresume(c)">

        <i class="ri-play-circle-fill text-success"
           title="Play"></i>

    </button>

    <!-- stop -->
    <button type="button"
            ng-click="timersstop(c)" data-bs-toggle="modal" data-bs-target="#timer_stop">

        <i class="ri-stop-circle-fill text-danger"
           title="Stop"></i>

    </button>

</div>

    </div>
<!-- view history -->
                                <div class="start-history" style="display:inline !important" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="History">
   <a href="javascript:void(0);"  ng-click="showhistory(c.id,c.tasktype)" data-bs-toggle="modal" data-bs-target="#timerhistory">
   <i class="bx bx-history"></i>
  </a>
</div>
                                </br> 
                                 <span ng-if="c.isend==1"
      ng-class="c.isexceed == 1 ? 'timer-text' : 'timer-text-green'" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="Spent Time">
    {{c.timerText_spent || '00:00:00'}}
</span>

                                
</td>
                            <td style="text-align: center;">
                                <a href="javascript: void(0);" ng-if="c.Powner" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getOwnerName(c.Powner)|| '' }}" style="display:inline-block;">
                                    <div class="avatar-xxs">
                                        <%--<img src="/pms/new/assets/img/avatar.jpg" alt="" class="rounded-circle img-fluid">--%>
                                        <img ng-src="{{getOwnerProfile(c.Powner)}}"  alt="" class="rounded-circle img-fluid" />
                                    </div>
                                </a>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div class="avatar-group">
                                        <a href="javascript: void(0);"  ng-repeat="newteam in c.teams|limitTo:2"  class="avatar-group-item material-shadow" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}">
                                            <%--<div class="avatar-xxs">

                                                <div class="avatar-title rounded-circle bg-danger">
                                                    D
                                                </div>
                                                </div>--%>
                                                <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle img-fluid" />

    <!-- Initial -->
   
      <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary" >
        {{newteam.shortname}}
      </div>
    

    </div>

                                            
                                        </a>
                                        <div ng-if="c.teams && c.teams.length > 2"
     class="avatar-group-item owner-wrp"
     bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingOwners(c.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle bg-primary">
        +{{ c.teams.length - 2 }}
    </div>
    </div>
</div>
                                       <%-- <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="Lee Winton">
                                            <div class="avatar-xxs">
                                                <img src="/pms/new/assets/img/avatar-2.jpg" alt="" class="rounded-circle img-fluid">
                                            </div>
                                        </a>
                                        <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="Johnny Shorter">
                                            <div class="avatar-xxs">
                                                <img src="/pms/new/assets/img/avatar-3.jpg" alt="" class="rounded-circle img-fluid">
                                            </div>
                                        </a>--%>
                                        <%--<a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="2 More">
                                            <div class="avatar-xxs">
                                                <div class="avatar-title rounded-circle bg-primary">
                                                    +2
                                                </div>
                                            </div>
                                        </a>--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                       
                    </tbody>
                </table>
            </div>
            
        </div>
            <div ng-show="isLoading" class="loader-overlay">
<div class="loader"></div>
            </div>
             <!-- Paging Controls -->
<div class="pull-right">
<dir-pagination-controls
max-size="8" 
direction-links="true"
boundary-links="true">
</dir-pagination-controls>
</div>
        <input type="hidden" id="hdnid" value="0" />
        <div id="timerhistory" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title"> {{mytasklist_history.taskname}} (Total Spent Time -{{mytasklist_history.spent}})</h5>
                <button type="button" class="btn-close" id="btnclshistory" data-bs-dismiss="modal" aria-label="Close"></button>
                
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                        <%--{{mytasklist_history.taskname}}--%>
<div class="table-responsive-new">
                <table class="table table-hover table-striped">
                    <thead>
                        <tr>
                            
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Spent Time</th>
                            <th>Remark</th>
                            <%--<th>Status</th> --%>                           
                            
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="history in mytasklist_history.history track by $index">
                            <td>{{ history.startdate | date:'dd/MM/yyyy'}}</td>
                            <td>{{ history.enddate | date:'dd/MM/yyyy'}}</td>
                            <td>{{ history.spenttime}}</td>
                            <td>{{ history.remark}}</td>

                        </tr>
                        </tbody>
                    </table>
    </div>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</div>
<div id="addnotes" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Notes</h5>
                <button type="button" class="btn-close" id="btnclsNotes" data-bs-dismiss="modal" aria-label="Close" ng-click="closepause();"></button>
                <button type="button" style="display:none" class="btn-close" id="btnclsNotes2" data-bs-dismiss="modal" aria-label="Close" ></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                        <div class="col-sm-12 mb-1">
                            <textarea id="Notes" runat="server" class="form-control"></textarea>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <div id="pnlimgpause" style="display:none;">
                        <img src="/pms/Image/loading8.gif" runat="server" />
                    </div>
                <div class="btn-group" id="divpause" style="display: block;">
               <button type="button" id="btnpausesaveNotes" class="btn btn-primary"  ng-click="SavePauseDetials();">Submit</button>
             </div>

            </div>
        </div>
    </div>
</div>

<div id="timer_stop" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Timer has stopped</h5>
                <button type="button" class="btn-close" id="btnclsTimer" data-bs-dismiss="modal" aria-label="Close" ng-click="closestop();"></button>
                <button type="button" style="display:none" class="btn-close" id="btnclsTimer2" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <%--<h5 class="text-center">Timer has stopped</h5>--%>

                    <asp:HiddenField ID="hdnhourcount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnminutecount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnsecondcount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnmilisecondcount" runat="server" Value="0" />
                   

                    <div class="row"> 
                        <div class="col-sm-12 mb-2">
                            <div class="d-flex align-items-center">
                            </div>
                            <div class="timer-stopped">
                                <div class="startendhrs">
                                    <h4 class="mb-0"><span id="spnhr">{{currentlist.timerText}}</span></h4>
                                    <span>Hours</span>
                                </div>
                                <div class="startendtime" style="display:none">
                                   Start Time - <span id="spnStarttime"></span><br />
                                    End Time - <span id="spnEndtime"></span>

                                </div>
                                
                            </div>
                        </div>
                        <div class="col-xxl-3 col-lg-3" style="display:none">
                            <div class="form-check form-radio-primary">
                                <label class="form-check-label">
                                    <input value="" name="Billable" type="radio" id="" class="form-check-input" checked="checked">
                                    Billable
                                </label>
                            </div>
                        </div>
                        <div class="col-xxl-3 col-lg-3" style="display:none">
                            <div class="form-check form-radio-primary">
                                <label class="form-check-label">
                                    <input value="" name="Billable" type="radio" id="" class="form-check-input">
                                    Non Billable
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1 mt-2">
                            <div class="d-flex align-items-center">
                                <label class="form-label flex-grow-1">Notes</label>
                            </div>
                      
                                <textarea id="TimerNotes" runat="server" class="form-control"></textarea>

                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                 <div id="pnlimgstop" style="display:none;">
                        <img src="/pms/Image/loading8.gif" runat="server" />
                    </div>
                <div class="btn-group" id="divstop" style="display: block;">
               <button type="button" id="btnsaveTimer" class="btn btn-primary" ng-click="SaveStopDetials();">Submit</button>
               <%-- <button type="button" id="btnRemoveTimer" style="display:none" onclick="RemoveDataTimer('removeTimer');" class="btn btn-dark">Remove Timer</button>--%>
          
             </div>
                  </div>

        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
            <div id="addtags" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            
            <div class="modal-header d-flex justify-content-between align-items-center" style="border-bottom:1px solid #ddd;padding:10px;">
                    <h4 style="font-size:18px;margin:0;">Add Tags</h4>
                    <button type="button" id="btnclstags" class="sub-soft-btn danger" data-bs-dismiss="modal" aria-label="Close" ng-click="closetagwindow();"><i class="ri-close-line"></i></button>
                
                </div>
           
             <div class="modal-body p-3 pb-3" click-outside="closelist()">
                 <div  id="tagDropdown" class="input-line" style="position:relative; width:100%;" >

                 <div class="input-tags" ng-click="closelist()">
        <span class="tag" ng-repeat="tag in getSelectedTags()" ng-style="{'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}} <span class="remove" ng-click="removetagsnew(tag.id); $event.stopPropagation()"><i class="ri-close-fill"></i></span>
        </span>
        
    </div>
                 <div style="width:100%;">
                                <input type="text" class="search__input"
               ng-model="searchTag"
               placeholder="Select Tag..."
               ng-click="openTagDropdown($event);stopTagEvent($event)"
               ng-change="filterTagList()"
               style="width:100%;" />                                
                            </div>
            
    <div class="dropdown-menu"
         ng-show="showTagDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

        <!-- Owner list -->
        <div ng-repeat="tag in filteredTagList | filter:searchTag"
             ng-click="toggleTag(tag)"
             ng-class="{'selected-tag': selectedtagIds.includes(tag.id)}"
             style="padding:3px; cursor:pointer;"
            ng-style="{
      'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}}
        </div>

        <div ng-if="(mytasktaglist | filter:searchTag).length == 0" style="padding:3px; color:#888;">
            No owners found
        </div>
    </div>

</div>
             </div>
            <div class="modal-body p-3 pb-2" id="dvnewtag" style="display:none">
                <asp:HiddenField ID="hdntagprjid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntagshtml" runat="server" Value="0" />
<div class="d-flex gap-2 align-items-center justify-content-between">
                        <div class="d-flex gap-2 justify-content-between" style="width:100%;">
                            <div style="width:100%;">
                               
                                 <input type="text" id="tags" placeholder="Tag Name" runat="server" class="form-control" required/>
                            </div>
                            <div>
                                <div class="input-group">
                                    <span class="input-group-text"><i class="bx bx-color-fill"></i></span>
                                   <%-- <input type="text" data-jscolor="{}" class="jscolor form-control">--%>
                                    <input id="tagscolor" class="form-control">
                                </div>
                            </div>
                        </div>
                        <div class="d-flex gap-1">
                            
                       <button type="button" id="btnsaveTags" class="sub-soft-btn" data-bs-toggle="tooltip" title="+ Add" ng-click="SvaeadTags();"><i class="ri-add-line"></i></button>
                            <button type="button" class="sub-soft-btn danger" ng-click="closenewtabgdiv()"><i class="ri-close-line"></i></button>
                            </div>
                    </div>
<div class="pt-3 pb-2">
                        <%--<div class="d-flex gap-1">
                            <span class="badge priority" style="background:#ff6a00;">Important</span>
                            <span class="badge priority" style="background:#3978de;">Urgent</span>
                        </div>--%>
                    </div>
                
            </div>
            
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
                <style>
                   .custom-dropdown{
    display:block;
    position:absolute;
    top:100%;
    left:0;
    right:0;
    background:#fff;
    border:1px solid #ddd;
    border-radius:6px;
    max-height:250px;
    overflow-y:auto;
    z-index:9999;
    box-shadow:0 2px 10px rgba(0,0,0,.1);
}

.dropdown-item-custom{
    padding:8px 12px;
    cursor:pointer;
}

.dropdown-item-custom:hover{
    background:#f5f5f5;
}

.selected-tag{
    background:#e7f1ff;
    font-weight:600;
}

.no-record{
    padding:10px;
    color:#888;
}

.input-tags{
    display:flex;
    flex-wrap:wrap;
    gap:5px;
    margin-bottom:5px;
}

.input-tags .tag{
    background:#eef2ff;
    border-radius:20px;
    padding:4px 10px;
    font-size:12px;
}

.input-tags .tag .remove {
    margin-left:5px;
    cursor:pointer;
    font-weight:bold;
}
.dropdown-menu div:hover {
    background-color: #e0e0e0;
}
                </style>
</div>
            <div id="addtags_multi" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            
            <div class="modal-header d-flex justify-content-between align-items-center" style="border-bottom:1px solid #ddd;padding:10px;">
                    <h4 style="font-size:18px;margin:0;">Add Tags</h4>
                    <button type="button" id="btnclstags2" class="sub-soft-btn danger" data-bs-dismiss="modal" aria-label="Close" ng-click="closetagwindow();"><i class="ri-close-line"></i></button>
                
                </div>
           
             <div class="modal-body p-3 pb-3" click-outside="closelist2()">
                 <div  id="tagDropdown2" class="input-line" style="position:relative; width:100%;" >

                 <div class="input-tags" ng-click="closelist()">
        <span class="tag" ng-repeat="tag in getSelectedTags()" ng-style="{'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}} <span class="remove" ng-click="removetagsnew_multi(tag.id); $event.stopPropagation()"><i class="ri-close-fill"></i></span>
        </span>
        
    </div>
                 <div style="width:100%;">
                                <input type="text" class="search__input"
               ng-model="searchTag"
               placeholder="Select Tag..."
               ng-click="openTagDropdown($event);stopTagEvent($event)"
               ng-change="filterTagList()"
               style="width:100%;" />
                                
                            </div>
            
    <div class="dropdown-menu"
         ng-show="showTagDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

        <!-- Owner list -->
        <div ng-repeat="tag in filteredTagList | filter:searchTag"
             ng-click="toggleTag_multi(tag)"
             ng-class="{'selected-tag': selectedtagIds.includes(tag.id)}"
             style="padding:3px; cursor:pointer;"
            ng-style="{
      'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}}
        </div>

        <div ng-if="(mytasktaglist | filter:searchTag).length == 0" style="padding:3px; color:#888;">
            No owners found
        </div>
    </div>

</div>
             </div>
            <div class="modal-body p-3 pb-2" id="dvnewtag2" style="display:none">
                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                          <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
<div class="d-flex gap-2 align-items-center justify-content-between">
                        <div class="d-flex gap-2 justify-content-between" style="width:100%;">
                            <div style="width:100%;">
                               
                                 <input type="text" id="tags2" placeholder="Tag Name" runat="server" class="form-control" required/>
                            </div>
                            <div>
                                <div class="input-group">
                                    <span class="input-group-text"><i class="bx bx-color-fill"></i></span>
                                   <%-- <input type="text" data-jscolor="{}" class="jscolor form-control">--%>
                                    <input id="tagscolor2" class="form-control">
                                </div>
                            </div>
                        </div>
                        
                    </div>

                
            </div>
            <div class="d-flex justify-content-center p-2">                            
                       <button type="button" id="btnsaveTags2" class="btn btn-primary" data-bs-toggle="tooltip" title="Save" ng-click="SvaeadTags_Multi();">Save</button>
                            
                            </div>
            
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
                <style>
                   .custom-dropdown{
    display:block;
    position:absolute;
    top:100%;
    left:0;
    right:0;
    background:#fff;
    border:1px solid #ddd;
    border-radius:6px;
    max-height:250px;
    overflow-y:auto;
    z-index:9999;
    box-shadow:0 2px 10px rgba(0,0,0,.1);
}

.dropdown-item-custom{
    padding:8px 12px;
    cursor:pointer;
}

.dropdown-item-custom:hover{
    background:#f5f5f5;
}

.selected-tag{
    background:#e7f1ff;
    font-weight:600;
}

.no-record{
    padding:10px;
    color:#888;
}

.input-tags{
    display:flex;
    flex-wrap:wrap;
    gap:5px;
    margin-bottom:5px;
}

.input-tags .tag{
    background:#eef2ff;
    border-radius:20px;
    padding:4px 10px;
    font-size:12px;
}

.input-tags .tag .remove {
    margin-left:5px;
    cursor:pointer;
    font-weight:bold;
}
.dropdown-menu div:hover {
    background-color: #e0e0e0;
}
                </style>
</div>
    </div>
        
    <script src="/pms/new/assets/js/jquery.min.js"></script>
    <script src="/pms/new/assets/js/moment.min.js"></script>
    <script src="/pms/new/assets/js/daterangepicker.min.js"></script>
      
        <script>

$(function () {

    // =========================================
    // MAIN DATERANGE
    // =========================================
    $('#daterange').daterangepicker({

    autoUpdateInput: true,
    autoApply: true,

    // DEFAULT THIS MONTH
    startDate: moment().startOf('month'),
    endDate: moment().endOf('month'),

    locale: {
        format: 'DD/MM/YYYY'
    },

    
     ranges: {

            'All': [moment('2020-01-01'), moment()],
            'Today': [moment(), moment()],
            'This Week': [moment().startOf('week'), moment().endOf('week')],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'This Quarter': [moment().startOf('quarter'), moment().endOf('quarter')],
            'This Year': [moment().startOf('year'), moment().endOf('year')],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Previous Week': [moment().subtract(1, 'week').startOf('week'), moment().subtract(1, 'week').endOf('week')],
            'Previous Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],            
            'Previous Quarter': [moment().subtract(1, 'quarter').startOf('quarter'), moment().subtract(1, 'quarter').endOf('quarter')],            
            'Previous Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
        }
});




    // =========================================
    // GET PICKER
    // =========================================
    var picker = $('#daterange').data('daterangepicker');




    // =========================================
    // HIDE DEFAULT CALENDAR
    // =========================================
    picker.container
        .find('.drp-calendar')
        .hide();




    // =========================================
    // DEFAULT VALUE
    // =========================================
    $('#daterange').val(

        picker.startDate.format('DD/MM/YYYY') +
        ' - ' +
        picker.endDate.format('DD/MM/YYYY')

    );




    // =========================================
    // NORMAL RANGE APPLY
    // =========================================
    $('#daterange').on(
        'apply.daterangepicker',
        function (ev, picker) {

            var fromdate =
                picker.startDate.format('DD/MM/YYYY');

            var todate =
                picker.endDate.format('DD/MM/YYYY');

            $(this).val(
                fromdate + ' - ' + todate
            );

            // CUSTOM RANGE SKIP
            if (
                picker.chosenLabel !== 'Custom Range'
            ) {

                callAngularBind(
                    fromdate,
                    todate
                );
                initCustomCalendar();
            }

        }
    );




    // =========================================
    // CUSTOM RANGE HTML
    // =========================================
    var customHtml = `

        <div class="custom-range-box p-2"
             style="display:none;width:200px;">

            <div class="mb-2">

                <label class="mb-1 d-block">
                    From Date
                </label>

                <input type="text"
                       id="customFromDate"
                       class="form-control"
                       placeholder="DD/MM/YYYY"
                       maxlength="10">

            </div>

            <div class="mb-2">

                <label class="mb-1 d-block">
                    To Date
                </label>

                <input type="text"
                       id="customToDate"
                       class="form-control"
                       placeholder="DD/MM/YYYY"
                       maxlength="10">

            </div>

            <button type="button"
                    id="btnCustomApply"
                    class="btn btn-primary btn-sm w-100">

                Apply

            </button>

        </div>

    `;

    picker.container.append(customHtml);




    // =========================================
    // RANGE CLICK
    // =========================================
    picker.container.find('.ranges li').on(
        'click',
        function () {

            var label =
                $(this).attr('data-range-key');



            // =====================================
            // CUSTOM RANGE
            // =====================================
            if (label === 'Custom Range') {

                // HIDE MAIN CALENDAR
                picker.container
                    .find('.drp-calendar')
                    .hide();

                // HIDE DEFAULT BUTTON
                picker.container
                    .find('.drp-buttons')
                    .hide();

                // SHOW CUSTOM BOX
                picker.container
                    .find('.custom-range-box')
                    .show();

                // DEFAULT VALUE
                $('#customFromDate').val(
                    picker.startDate.format(
                        'DD/MM/YYYY'
                    )
                );

                $('#customToDate').val(
                    picker.endDate.format(
                        'DD/MM/YYYY'
                    )
                );



                // =====================================
                // FROM DATE PICKER
                // =====================================
                if (
                    !$('#customFromDate')
                        .data('daterangepicker')
                ) {

                    $('#customFromDate')
                        .daterangepicker({

                            singleDatePicker: true,
                            autoApply: true,
                            autoUpdateInput: true,
                            showDropdowns: true,
                            parentEl: '.custom-range-box',

                            locale: {
                                format: 'DD/MM/YYYY'
                            }

                        });

                    $('#customFromDate').on(
                        'apply.daterangepicker',
                        function (ev, p) {

                            $(this).val(
                                p.startDate.format(
                                    'DD/MM/YYYY'
                                )
                            );

                        }
                    );
                }



                // =====================================
                // TO DATE PICKER
                // =====================================
                if (
                    !$('#customToDate')
                        .data('daterangepicker')
                ) {

                    $('#customToDate')
                        .daterangepicker({

                            singleDatePicker: true,
                            autoApply: true,
                            autoUpdateInput: true,
                            showDropdowns: true,
                            parentEl: '.custom-range-box',

                            locale: {
                                format: 'DD/MM/YYYY'
                            }

                        });

                    $('#customToDate').on(
                        'apply.daterangepicker',
                        function (ev, p) {

                            $(this).val(
                                p.startDate.format(
                                    'DD/MM/YYYY'
                                )
                            );

                        }
                    );
                }

            }



            // =====================================
            // NORMAL RANGE
            // =====================================
            else {

                // HIDE CUSTOM BOX
                picker.container
                    .find('.custom-range-box')
                    .hide();

                // HIDE CALENDAR
                picker.container
                    .find('.drp-calendar')
                    .hide();

                // SHOW BUTTONS
                picker.container
                    .find('.drp-buttons')
                    .hide();
            }

        }
    );




    // =========================================
    // ALLOW ONLY DATE CHARACTERS
    // =========================================
    $(document).on(
        'input',
        '#customFromDate, #customToDate',
        function () {

            this.value =
                this.value.replace(
                    /[^0-9\/]/g,
                    ''
                );

        }
    );




    // =========================================
    // CUSTOM APPLY
    // =========================================
    $(document).on(
        'click',
        '#btnCustomApply',
        function () {

            var fromdate =
                $('#customFromDate')
                    .val()
                    .trim();

            var todate =
                $('#customToDate')
                    .val()
                    .trim();



            // EMPTY CHECK
            if (
                fromdate === '' ||
                todate === ''
            ) {

                alert(
                    'Please enter both dates'
                );

                return;
            }



            // FORMAT CHECK
            if (
                !isValidDateFormat(
                    fromdate
                )
            ) {

                alert(
                    'Invalid From Date. Use DD/MM/YYYY'
                );

                $('#customFromDate')
                    .focus();

                return;
            }

            if (
                !isValidDateFormat(
                    todate
                )
            ) {

                alert(
                    'Invalid To Date. Use DD/MM/YYYY'
                );

                $('#customToDate')
                    .focus();

                return;
            }



            // DATE CHECK
            if (
                toDateObj(fromdate) >
                toDateObj(todate)
            ) {

                alert(
                    'From Date cannot be greater than To Date'
                );

                return;
            }



            // SET VALUE
            $('#daterange').val(

                fromdate +
                ' - ' +
                todate

            );



            // UPDATE PICKER
            picker.setStartDate(
                moment(
                    fromdate,
                    'DD/MM/YYYY'
                )
            );

            picker.setEndDate(
                moment(
                    todate,
                    'DD/MM/YYYY'
                )
            );



            // CLOSE
            picker.hide();



            // CALL ANGULAR
            callAngularBind(
                fromdate,
                todate
            );
            initCustomCalendar();
        }
    );

});




// =========================================
// VALIDATE DATE
// =========================================
function isValidDateFormat(dateString) {

    var regex =
        /^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/\d{4}$/;

    if (!regex.test(dateString)) {

        return false;
    }

    var parts =
        dateString.split('/');

    var day =
        parseInt(parts[0], 10);

    var month =
        parseInt(parts[1], 10) - 1;

    var year =
        parseInt(parts[2], 10);

    var date =
        new Date(year, month, day);

    return (

        date.getFullYear() === year &&
        date.getMonth() === month &&
        date.getDate() === day

    );
}




// =========================================
// STRING TO DATE
// =========================================
function toDateObj(str) {

    var p = str.split('/');

    return new Date(

        parseInt(p[2]),
        parseInt(p[1]) - 1,
        parseInt(p[0])

    );
}




// =========================================
// CALL ANGULAR
// =========================================
function callAngularBind(
    fromdate,
    todate
) {

    var scope =
        angular.element(
            $('#daterange')
        ).scope();



    // FALLBACK
    if (!scope) {

        scope = angular.element(
            document.querySelector(
                '[ng-controller]'
            )
        ).scope();
    }



    // CALL FUNCTION
    if (scope) {

        scope.$applyAsync(function () {

            scope.fromdate = fromdate;

            scope.todate = todate;



            if (
                typeof scope.bindtasklist2
                === 'function'
            ) {

                scope.bindtasklist2(
                    fromdate,
                    todate
                );
            }

        });
    }
    initCustomCalendar();
}
function initCustomCalendar() {

    $('#customFromDate, #customToDate').daterangepicker({
        singleDatePicker: true,
        autoUpdateInput: false,
        locale: {
            format: 'DD/MM/YYYY'
        }
    });

    // set value on selection
    $('#customFromDate').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY'));
    });

    $('#customToDate').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY'));
    });
}
</script>
        
  <%--ranges: {

            'All': [moment('2020-01-01'), moment()],

            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],

            'This Week': [moment().startOf('week'), moment().endOf('week')],
            'Previous Week': [moment().subtract(1, 'week').startOf('week'), moment().subtract(1, 'week').endOf('week')],

            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],

            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Previous Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],

            'This Quarter': [moment().startOf('quarter'), moment().endOf('quarter')],
            'Previous Quarter': [moment().subtract(1, 'quarter').startOf('quarter'), moment().subtract(1, 'quarter').endOf('quarter')],

            'This Year': [moment().startOf('year'), moment().endOf('year')],
            'Previous Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
        }--%>  
        
<script src="/pms/assets/js/jscolor.js"></script>

<script>

$(document).on('shown.bs.modal', '#addtags', function () {

    if (!document.getElementById('tagscolor').jscolor) {

        new jscolor(document.getElementById('tagscolor'));

    }

});
$(document).on('shown.bs.modal', '#addtags_multi', function () {

    if (!document.getElementById('tagscolor2').jscolor) {

        new jscolor(document.getElementById('tagscolor2'));

    }

});
</script>

<script>
        document.addEventListener('DOMContentLoaded', function () {

            document.querySelectorAll('.search__wrap').forEach(wrapper => {

                const tagInput = wrapper.querySelector('.tag-input'); // optional
                const input = wrapper.querySelector('.search__input');
                const dropdown = wrapper.querySelector('.search__dropdown');
                const arrow = wrapper.querySelector('.search__arrow');
                const items = [...wrapper.querySelectorAll('.dropdown_bl__link:not(.search-more)')];
                const noRecord = wrapper.querySelector('.dropdown_bl__no-record');

                /* ✅ PLACEHOLDER FIX */
                const originalPlaceholder = input.getAttribute('placeholder');
                function updatePlaceholder() {
                    if (!tagInput) return;
                    const hasTags = tagInput.querySelectorAll('.tag').length > 0;
                    input.placeholder = hasTags ? 'Search' : originalPlaceholder;
                }

                function openDropdown() {
                    dropdown.style.display = 'block';
                }

                function closeDropdown() {
                    dropdown.style.display = 'none';
                }

                function updateNoRecord() {
                    const visible = items.some(i => i.style.display !== 'none');
                    if (noRecord) {
                        noRecord.classList.toggle('d-none', visible);
                    }
                }

                function toggleTag(value) {
                    if (!tagInput) return false;

                    const existing = [...tagInput.querySelectorAll('.tag')]
                        .find(tag => tag.dataset.value === value);

                    const item = items.find(i => i.textContent.trim() === value);

                    /* 🔁 REMOVE TAG */
                    if (existing) {
                        existing.remove();
                        if (item) item.style.display = 'block';
                        updateNoRecord();
                        updatePlaceholder(); // ✅
                        return true;
                    }

                    /* ➕ ADD TAG */
                    const tag = document.createElement('span');
                    tag.className = 'tag';
                    tag.dataset.value = value;
                    tag.innerHTML = `${value} <i class="ri-close-line"></i>`;

                    tag.querySelector('i').onclick = () => {
                        tag.remove();
                        if (item) item.style.display = 'block';
                        updateNoRecord();
                        updatePlaceholder(); // ✅
                    };

                    tagInput.insertBefore(tag, input);
                    if (item) item.style.display = 'none';

                    updateNoRecord();
                    updatePlaceholder(); // ✅
                    return true;
                }

                /* Open dropdown */
                (tagInput || input).addEventListener('click', () => {
                    input.focus();
                    openDropdown();
                });

                if (arrow) {
                    arrow.addEventListener('click', e => {
                        e.stopPropagation();
                        dropdown.style.display === 'block' ? closeDropdown() : openDropdown();
                    });
                }

                /* 🔍 Search filter */
                input.addEventListener('input', () => {
                    const value = input.value.toLowerCase();
                    let visibleCount = 0;

                    items.forEach(item => {
                        if (item.style.display === 'none') return;

                        const match = item.textContent.toLowerCase().includes(value);
                        item.style.display = match ? 'block' : 'none';
                        if (match) visibleCount++;
                    });

                    if (noRecord) {
                        noRecord.classList.toggle('d-none', visibleCount !== 0);
                    }
                });

                /* Select / Unselect */
                items.forEach(item => {
                    item.addEventListener('click', e => {
                        e.preventDefault();
                        const value = item.textContent.trim();

                        if (tagInput) {
                            toggleTag(value);
                            input.value = '';
                            input.dispatchEvent(new Event('input'));
                        } else {
                            input.value = value;
                            closeDropdown();
                        }
                    });
                });

                /* Click outside */
                document.addEventListener('click', e => {
                    if (!wrapper.contains(e.target)) {
                        closeDropdown();
                    }
                });

                updatePlaceholder(); // ✅ initial state

            });

        });
    </script>
</asp:Content>

