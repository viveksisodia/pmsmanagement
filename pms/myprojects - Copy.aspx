<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="myprojects.aspx.cs" Inherits="myprojects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/pms/angularmaster/jsapp/pmsapp.js?v=<%=DateTime.Now.Ticks %>"></script> 
<script src="/pms/projects/app/myproject.service.js?v=<%=DateTime.Now.Ticks %>"></script>
<script src="/pms/projects/app/myproject.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script> 
    <link rel="stylesheet" href="/pms/new/assets/css/daterangepicker.css" type="text/css" />
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
        .search_box {position:relative;}
.search_box i {position:absolute;top:4px;left:7px;font-size:16px;}
.search_box input.form-control {padding-left:28px;color:#000;border-radius:4px;height:32px;border: 1px solid #ddd !important;}
.search_box input.form-control:focus {border-color:#804574 !important;}
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
        .files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.issue_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.dependency_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}

    </style>
    <style>
        .owner-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:9px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <div class="main-wrapper"  ng-app="PMSerp" ng-controller="myprojectCtrl">

        <div class="sub-header">
            <div class="row">
                <div class="col-xxl-3 col-lg-3 col-md-4 col-sm-12">
                    <div class="sub-left">
                        <div class="sub-page-title">
                            Project
                        </div>
                        
                        <a href="javascript:void(0);" type="button" class="sub-soft-btn me-3" data-bs-toggle="offcanvas" data-bs-target="#addnewproject"><span data-bs-toggle="tooltip" title="+ Add Project"><i class="ri-add-line"></i></span></a>
                        <div class="d-flex align-items-center gap-1">
                            <!-- CALENDAR ICON -->
            <i class="ri-calendar-line text-muted ms-1"
               style="font-size:16px;line-height:1;"></i>

            <!-- DATE RANGE INPUT -->
            <input type="text" id="daterange" readonly
                   style="border:none; background:transparent;" />

            <!-- ARROW -->
            <i class="ri-arrow-down-s-line"
               style="font-size:18px;line-height:1;"></i>
                            <%--<i class="ri-calendar-line text-muted" style="font-size: 20px;"></i>
                            <span style="font-size: 15px;">2025-2026</span>
                            <i class="ri-arrow-down-s-line" style="font-size: 18px;"></i>--%>
                        </div>
                    </div>
                </div>
                <div class="col-xxl-6 col-lg-6 col-md-8 col-sm-12">
                    <ul class="top-list-sales">
                        <li>
                            <a href="#">
                                <div class="tile">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <div class="tile-icon"><i class="ri-folder-2-line"></i></div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>Total Project</span></div>
                                            <div class="tile-body">{{mytotalprojectlist}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        <li ng-repeat="c in myprojectlist_bystatus">
                            <a href="#">
                                <div class="tile">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <div class="tile-icon" ng-if="c.statusicon==''" ng-style="{'color':c.statuscolor }"><i class="ri-folder-2-line"></i></div>
                                            <div class="tile-icon" ng-if="c.statusicon" ng-style="{'color':c.statuscolor }"><i ng-class="c.statusicon"></i>
</div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>{{c.statusname}}</span></div>
                                            <div class="tile-body">{{c.total}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        
                    </ul>
                </div>
                <div class="col-xxl-3 col-lg-3 col-md-6 col-sm-12">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                       <div class="search_box me-2"  ng-show="freesearch=='1'">                           
                           <input type="text" class="form-control" ng-model="searchText" placeholder="Search..." />
                            <i class="ri-search-line"></i>
                        </div>
                        <a href="javascript:void(0);" type="button" class="sub-soft-btn" data-bs-toggle="tooltip" title="Search" ng-click="viewsearch()"><i class="ri-search-line"></i></a>
                        <a href="myprojects.html" class="sub-soft-btn active" data-bs-toggle="tooltip" title="List View"><i class="ri-list-check"></i></a>
                        <a href="myprojects-tiles.html" class="sub-soft-btn" data-bs-toggle="tooltip" title="Tiles View"><i class="ri-layout-grid-fill"></i></a>
                        <a href="myprojects-calendar.html" class="sub-soft-btn" data-bs-toggle="tooltip" title="Calendar"><i class="bx bx-calendar"></i></a>
                        <button class="sub-soft-btn" data-bs-toggle="tooltip" title="Filter"><i class="ri-filter-line"></i></button>
                        <div class="btn-group d-block">
                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="sub-soft-btn"><i class="ri-more-fill"></i></a>
                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                <ul class='sttus-list'>
                                    <li><a class='' href='#'><i class="bx bx-download fs-16 align-middle me-1"></i> Download Report</a></li>
                                    <li><a class='' href='#'><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Project</a></li>
                                    <li><a class='' href='#'><i class="bx bx-plus fs-16 align-middle me-1"></i> Add Project</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapper_inner p-0">
            <div class="table-responsive-new">
                <table id='tbDetails' class="table table-hover table-striped tbl_new project-table">
                    <thead>
                        <tr>
                            <th class="color_border"></th>
                            <th style="width: 40px;">S.No.</th>
                            <th>Client Name</th>
                            <th>ID</th>
                            <th style="min-width:250px;">Project Name</th>
                            <th>Timeline</th>
                            <th style="min-width:120px;">Status</th>
                            <th>Project Situation</th>
                            <th>Priority</th>
                            <th>Extended Timeline</th>
                            <th>Completion %</th>
                            <th style="min-width:120px;">Project Manager</th>
                            <th style="min-width:120px;">Team</th>
                            <th>Milestones</th>
                            <th>Task</th>
                            <th>Issues</th>
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Budget</th>
                            <th>Actual</th>
                            <th style="min-width:250px;">Project Address</th>
                            <th>Award Date</th>
                            <th>Approval</th>
                            <th>Approval Date</th>
                            <th>Files</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="c in projectlist|orderBy:sortColumn:reverseSort | filter:searchText | itemsPerPage: pageSize track by $index " current-page="currentPage">
                            <td class="color_border" style="border-left-color:#ffc300 !important;">
                                <div class="btn-group d-block">
                                    <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-more-2-fill"></i></a>
                                    <div class="dropdown-menu p-0" style="border-radius:0;">
                                        <ul class='sttus-list'>
                                            <li><a class='' href='#'><i class="ri-parent-line fs-16 align-middle me-1"></i> Dependency</a></li>
                                            <li><a class='' href='#'><i class="bx bx-error-circle fs-16 align-middle me-1"></i> Add Issue</a></li>
                                            <li><a class='' href='#'><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Documents</a></li>
                                            <li><a class='' href='#'><i class="bx bx-edit fs-16 align-middle me-1"></i> Edit Project</a></li>
                                            <li><a class='' href='#'><i class="bx bx-copy fs-16 align-middle me-1"></i> Clone</a></li>
                                            <li><a class='' href='javascript:void(0);' id="text"><i class="bx bx-palette fs-16 align-middle me-1"></i> Change Color</a></li>
                                            <li><a class='' href='#'><i class="bx bx-archive fs-16 align-middle me-1"></i> Hold</a></li>
                                            <li><a class='text-danger' href='#'><i class="bx bx-trash fs-16 align-middle me-1"></i> Delete</a></li>
                                        </ul>
                                        <input type="color" id="colorPicker">
                                    </div>
                                </div>
                            </td>
                            <td>{{ ((currentPage - 1) * pageSize) + $index + 1 }}</td>
                            <td>{{c.ClientName}}</td>
                            <td><a href="">{{c.ProjectID}}</a></td>
                            <td><a href="projects-unassign.html" style="color:#000 !important;"><b>{{c.ProjectName}}</b></a></td>
                            <td>{{ c.StartDate | date:'dd/MM/yyyy' }} - {{ c.TargetDate | date:'dd/MM/yyyy' }}</td>
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
                            <td class="text-danger">Critical</td>
                            <td>
                                <%--<div class="badge bg-danger-subtle text-danger priority">High</div>--%>
                                <div style="position:relative;">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;" ng-style="{'background-color': c.priority_color}"> {{ c.priority_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name" ng-click="selectPriority(c,newpriority); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='ri-checkbox-blank-fill' ng-style="{'color':newpriority.colorname}" ></i> {{newpriority.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
    </div> 
                            </td>
                            <td><div class="d-flex" style="width:100%">

    <!-- LEFT SIDE (Progress) -->
    <div class="flex-grow-1 align-self-center me-2">
        <div class="extended_time">
            
            <a href="javascript:void(0);" class="timesheet-wrap" ng-repeat="etl in c.extendedlists">

                <div class="progress progress-lg bg-dark position-relative me-1"  bs-tooltip
                     data-bs-toggle="tooltip"                    
                     data-bs-title="{{etl.extended_from}} - {{etl.extended_to}} ({{etl.delayed_days}} {{etl.extended_type}})">

                    <div class="progress-bar bg-secondary"
                         role="progressbar"
                         ng-style="{'width':'100%'}"
                         aria-valuemax="100">

                        {{etl.extended_days}} days
                    </div>

                </div>

            </a>

        </div>
    </div>

    <!-- RIGHT SIDE (+ BUTTON) -->
    <div class="gap-1 flex-shrink-0 align-self-center">

       
         <a href="javascript:void(0);"
       type="button"             
       data-bs-toggle="modal"
       data-bs-target="#ExtendedTimline"
       ng-click="Bindextenddate_Details(c,$index)">

      <span class="btn-plus2" bs-tooltip
            data-bs-toggle="tooltip"
            data-bs-title="+ Add Extended Day">
        +
      </span>
    </a>

    </div>

</div></td>
                            <td>
                                <div>


                                    <div class="d-block mb-0">
                                        <div style="font-size:11px !important;color:#777;">{{ c.complete_value }}%</div>
                                    </div>
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar bg-success" role="progressbar" style="width:{{ c.complete_value }}%;"></div>
                                    </div>
                                    
                                </div>
                            </td>
                            <td style="text-align: center;">
                                <%--<a href="javascript: void(0);" class="d-flex gap-2" style="display:inline-block;">
                                    <div class="avatar-xxs">
                                        <img src="/pms/new/assets/img/avatar.jpg" alt="" class="rounded-circle img-fluid">
                                    </div>
                                    <div style="color:#000;">Erica Kernan</div>
                                </a>--%>
                                <div class="dropdown w-100">

  <div class="dropdown"
         
          data-bs-toggle="dropdown" 
        ng-click="focusOwnerSearch($event, c)" style="cursor:pointer;">
      <div class="d-flex align-items-center gap-2">
   <div class="avatar-xxs">
       <img ng-src="{{getOwnerProfile(c.Owner)}}" onerror="this.onerror=null;this.src='Image/user.jpg';"  alt="" class="rounded-circle img-fluid">

   </div>                               
      <div>{{ getOwnerName(c.Owner) || 'Select Owner' }}</div>
      </div>
  </div>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2 search__input"
           placeholder="Search..."
           ng-model="c.ownerSearch">

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: c.ownerSearch"
         ng-class="{'active': c.Owner == o.id}"
         ng-click="selectOwner(c, o)">
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

                            </td>
                            <td>
                                <div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c.teams|limitTo:2" class="avatar-group-item material-shadow" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary" >
        {{newteam.shortname}}
      </div>
    </div>   
    <i class="ri-close-fill owner-del" ng-click="removeteams(c,newteam)"></i>

</a>
   <div ng-if="c.teams && c.teams.length > 2"
     class="avatar-group-item owner-wrp"     bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingOwners(c.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle bg-primary">
        +{{ c.teams.length - 2 }}
    </div>
    </div>
</div>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center"><a href="javascript:void(0);" style="display:" type="button"  ng-click="loadSrcOwnersnew(c,'team',$index)" data-bs-toggle="modal" data-bs-target="#addowners"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div style="min-width:85px;">
                                        <div class="d-block mb-0">
                                            <div style="font-size:11px !important;color:#777;">{{c.completemile}}/{{c.totalmile}}</div>
                                        </div>
                                        <div class="progress progress-md animated-progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completemile_rate}}%;"></div>
                                        </div>
                                    </div>
                                    <%--<div><a href="javascript:void(0);" class="click-link" ng-click="loadsrc(c,'inner_popup2','/pms/AddmilstonePopup.aspx?id='+c.id+'&type=Project','milestone')"><i class="ri-add-line"></i></a></div>--%>
                                </div>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div style="min-width:85px;">
                                        <div class="d-block mb-0">
                                            <div style="font-size:11px !important;color:#777;">{{c.completetask}}/{{c.totaltask}}</div>
                                        </div>
                                        <div class="progress progress-md animated-progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completetask_rate}}%;"></div>
                                        </div>
                                    </div>
                                    <%--<div><a href="javascript:void(0);" class="click-link"><i class="ri-add-line"></i></a></div>--%>
                                </div>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div style="min-width:85px;">
                                        <div class="d-block mb-0">
                                            <div style="font-size:11px !important;color:#777;">{{c.completeissue}}/{{c.totalissue}}</div>
                                        </div>
                                        <div class="progress progress-md animated-progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completeissue_rate}}%;"></div>
                                        </div>
                                    </div>
                                    <%--<div><a href="" class="click-link"><i class="ri-add-line"></i></a></div>--%>
                                </div>
                            </td>
                            <td>{{ c.StartDate | date:'dd/mm/yyyy' }}</td>
                            <td>{{ c.TargetDate | date:'dd/mm/yyyy' }}</td>
                            <td>{{c.Budget}}</td>
                            <td>{{c.Actual}}</td>
                            <td>{{c.Completeaddress}}</td>
                            <td>{{ c.AwardDate | date:'dd/MM/yyyy' }}</td>
                            <td>{{ c.ApprovalDrawing == '1' ? 'Yes' : 'No' }}</td>
                            <td>{{ c.DrawingApprovalDate | date:'dd/MM/yyyy' }}</td>
                            <td>
                                <%--<a href="javascript:void(0);" style="font-size:22px;" data-bs-toggle="offcanvas" data-bs-target="#uploaddocuments"><i class="ri-file-text-line"></i></a>--%>
                                <a href="javascript:void(0);" style="font-size:22px;" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadprojectfiles(c,$index)" class="position-relative"><span class="btn-plus2" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="Upload Documents" ><i class="ri-file-text-line"></i></span><span class="files_count">{{ c.viewfiles.length || 0 }}</span></a>
                                
                            </td>
                        </tr>
                        
                    </tbody>
                </table>
            </div>
            <div ng-show="isLoading" class="loader-overlay">
<div class="loader"></div>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="d-flex align-items-center gap-2" style="padding-left:10px;">
                        <select class="form-select" ng-model="pageSize"  ng-options="size for size in pageSizes" style="width:55px;"></select>
                        <span class="text-muted" style="font-size:11px;">Records per page</span>
                    </div>
                </div>
                <div class="col-sm-4 text-center" style="padding-top:7px;">
                    <%--<span class="text-muted" style="font-size:11px;">Showing 1 to 2 of 2 entries</span>--%>
                </div>
                <div class="col-sm-4">
                    <div class="pull-right" style="padding-right:10px;">
                        <dir-pagination-controls
max-size="8" 
direction-links="true"
boundary-links="true">
</dir-pagination-controls>
                    </div>
                </div>
            </div>
             <!-- Paging Controls -->


        </div>
        <div class="offcanvas offcanvas-end" tabindex="-1" id="addnewproject">
        <div class="offcanvas-header justify-content-between border-bottom p-2">
            <h5 class="offcanvas-title">Add Project</h5>
            <!--<button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>-->
            <div class="hstack gap-1">
                <button type="button" onclick="fsFunction()" class="sub-soft-btn fsr fs-18"><i class="ri-fullscreen-fill"></i></button>
                <button type="button" onclick="fseFunction()" class="sub-soft-btn fsre fs-18"><i class="ri-fullscreen-exit-fill"></i></button>
                <button type="button" class="sub-soft-btn danger fs-18" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ri-close-fill"></i></button>
            </div>
        </div>
        <div class="offcanvas-body p-0">
            <div data-simplebar style="height: calc(100vh - 45px);">
                <div class="p-3">
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="input-line">
                                <input type="text" placeholder="Project ID" ng-model="newproject.ProjectID">
                            </div>
                        </div>
                        <div class="col-sm-9">
                            <div class="input-line">
                                <input type="text" placeholder="Project Name" ng-model="newproject.ProjectName">
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1">
                            <label style="font-weight:600;">Client Details</label>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <%--<div class="search__wrap position-relative" ng-click="$event.stopPropagation()">
                                                                                        <input type="text" id="clientid"
                                                                                            class="form-control search__input"
                                                                                            placeholder="Client Name"
                                                                                            autocomplete="off"
                                                                                            ng-model="customerlist.ClientName"                                                                                            
                                                                                            ng-change="dropdownTyping = true"
                                                                                            dropdown-auto-clear
                                                                                            selected-value="customerlist.customerid"                                                                                            
                                                                                            display-field="ClientName"
                                                                                            dropdown-visible="showUnitDropdown">
                                                                                        <span class="search__arrow" ng-click="showUnitDropdown = !showUnitDropdown">
                                                                                            <i class="ri-arrow-down-s-fill"></i>
                                                                                        </span>
                                                                                        <input type="hidden" ng-model="customerlist.customerid" />
                                                                                        <div class="dropdown_bl search__dropdown shadow-sm"
                                                                                            ng-show="showUnitDropdown">

                                                                                            <a href=""
                                                                                                class="dropdown_bl__link"
                                                                                                ng-repeat="c in customerlist | filter:filterUnit"
                                                                                                ng-click="$event.preventDefault()" >{{customerlist.ClientName}}
                                                                                            </a>

                                                                                            <div class="dropdown_bl__no-record"
                                                                                                ng-if="(customerlist | filter:filterUnit).length == 0">
                                                                                                No record found
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>--%>
                                        <div class="search__wrap position-relative">

    <input type="text"
           class="form-control search__input"
           placeholder="Client Name"
           ng-model="ClientName"
           autocomplete="off"   ng-focus="showqcustomersDropdown = true;"
                                                            ng-click="showqcustomersDropdown = true;">

    <span class="search__arrow">
        <i class="ri-arrow-down-s-fill"></i>
    </span>

    <div class="dropdown_bl search__dropdown shadow-sm"  ng-show="showqcustomersDropdown"
                                                            ng-mousedown="$event.preventDefault(); $event.stopPropagation();">

        <a href="javascript:void(0);"
           class="dropdown_bl__link"
           ng-repeat="c in customerlist | filter:{ClientName: ClientName}"
           ng-click="selectclient(c)">
            {{c.ClientName}}
        </a>

        <div class="dropdown_bl__no-record"
             ng-if="(customerlist | filter:{ClientName: ClientName}).length == 0">
            No record found
        </div>

    </div>

</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">

                                        <div class="search__wrap position-relative">
                                            <input type="text" class="form-control search__input" placeholder="Sales Order" value="" autocomplete="off">
                                            <span class="search__arrow"><i class="ri-arrow-down-s-fill"></i></span>
                                            <div class="dropdown_bl search__dropdown shadow-sm">
                                                <a href="javascript:void(0);" class="dropdown_bl__link" ng-repeat="c in solist " ng-click="selectso(c.salesorderid);">{{c.Estimateno}}</a>
                                               
                                                <div class="dropdown_bl__no-record d-none">No record found</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Milestone</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                            <div class="d-flex align-items-center gap-2 mt-2" style="display:none !important;">
                                <span>Same weightage</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Task</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                            <div class="d-flex align-items-center gap-2 mt-2" style="display:none !important;">
                                <span>Same weightage</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Subtask</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                            <div class="d-flex align-items-center gap-2 mt-2" style="display:none !important;">
                                <span>Same weightage</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-12 mt-3">
                            <label style="font-weight:600;">Priority</label>
                            <div class="hstack gap-1 flex-wrap">
                                <div ng-repeat="c in prioritylists">
    <input type="radio"
           class="btn-check"
           name="options-priority"
           id="rbt_{{c.id}}"
           ng-model="$parent.selectedPriority"
           ng-value="c.id">

    <label class="btn btn-stts"
           for="rbt_{{c.id}}"
           ng-style="{
                'background-color':c.colorname,    'color': '#fff'
           }">
        {{c.name}}
    </label>
</div>
                            </div>
                        </div>
                        <div class="col-sm-12 mt-2">
                            <label style="font-weight:600;">Status</label>
                            <div class="hstack gap-1 flex-wrap">
                                <input type="radio"
                                       class="btn-check" name="options-status"
                                       id="Status1" checked>
                                <label class="btn-outline-sp btn-stts" for="Status1" style="background:#ddfbf9;color:#18e0d3;"><i class="ri-check-line me-1"></i> WIP</label>
                                <input type="radio"
                                       class="btn-check" name="options-status"
                                       id="Status2">
                                <label class="btn-outline-sp btn-stts" for="Status2" style="background:#fef4e4;color:#f7ba4b;"><i class="ri-check-line me-1"></i> To be Started</label>
                                <input type="radio"
                                       class="btn-check" name="options-status"
                                       id="Status3">
                                <label class="btn-stts" for="Status3" style="background:#f6dff1;color:#ff06c9;"><i class="ri-check-line me-1"></i> Ready to Live</label>
                                <input type="radio"
                                       class="btn-check" name="options-status"
                                       id="Status4">
                                <label class="btn-stts" for="Status4" style="background:#d7e3fa;color:#3577f1;"><i class="ri-check-line me-1"></i> Go Live</label>
                                <input type="radio"
                                       class="btn-check" name="options-status"
                                       id="Status5">
                                <label class="btn-stts" for="Status5" style="background:#d9fad7;color:#0cda7d;"><i class="ri-check-line me-1"></i> Completed</label>
                            </div>
                        </div>
                        <div class="col-sm-12 mt-2">
                            <div class="input-line">
                                <div class="search__wrap position-relative">
                                    <input type="text" class="form-control search__input" placeholder="Project Manager" value="" autocomplete="off">
                                    <span class="search__arrow"><i class="ri-arrow-down-s-fill"></i></span>
                                    <div class="dropdown_bl search__dropdown shadow-sm">
                                        <a href="#" class="dropdown_bl__link">Avneesh Thakur</a>
                                        <a href="#" class="dropdown_bl__link">Vivek Sisodia</a>
                                        <div class="dropdown_bl__no-record d-none">No record found</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1">
                            <div class="input-line">
                                <div class="search__wrap position-relative">
                                    <div class="tag-input">
                                        <input type="text" id="" class="search__input" placeholder="Team" />
                                        <span class="search__arrow"><i class="ri-arrow-down-s-fill "></i></span>
                                    </div>
                                    <div class="dropdown_bl search__dropdown">
                                        <a href="#" class="dropdown_bl__link">Amit Kumar</a>
                                        <a href="#" class="dropdown_bl__link">Sachin Gupta</a>
                                        <a href="#" class="dropdown_bl__link">Ashok Sharma</a>
                                        <a href="#" class="dropdown_bl__link">Arun Kumar</a>
                                        <a href="#" class="dropdown_bl__link">Bablu Kumar</a>
                                        <div class="dropdown_bl__no-record d-none">No record found</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="input-line">
                                <label for="StartDate">Start Date</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="StartDate" class="form-control search__input" placeholder="dd/mm/yyyy" value="" autocomplete="off">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="input-line">
                                <label for="EndDate">End Date</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="EndDate" class="form-control search__input" placeholder="dd/mm/yyyy" value="" autocomplete="off">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="input-line">
                                <label for="TargetDate">Target Date</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="TargetDate" class="form-control search__input" placeholder="dd/mm/yyyy" value="" autocomplete="off">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 mt-1 mb-1">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="input-line">
                                        <label class="mb-2">Project Address</label>
                                        <input type="text" placeholder="Address Line 1">
                                    </div>
                                    <div class="input-line">
                                        <input type="text" placeholder="Address Line 2">
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <input type="text" placeholder="City">
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <div class="search__wrap position-relative">
                                            <input type="text" class="form-control search__input" placeholder="State" value="" autocomplete="off">
                                            <span class="search__arrow"><i class="ri-arrow-down-s-fill"></i></span>
                                            <div class="dropdown_bl search__dropdown">
                                                <a href="#" class="dropdown_bl__link">Andaman and Nicobar Islands</a>
                                                <a href="#" class="dropdown_bl__link">Andhra Pradesh</a>
                                                <a href="#" class="dropdown_bl__link">Arunachal Pradesh</a>
                                                <a href="#" class="dropdown_bl__link">Assam</a>
                                                <a href="#" class="dropdown_bl__link">Bihar</a>
                                                <a href="#" class="dropdown_bl__link">Chandigarh</a>
                                                <a href="#" class="dropdown_bl__link">Chhattisgarh</a>
                                                <a href="#" class="dropdown_bl__link">Dadra and Nagar Haveli</a>
                                                <a href="#" class="dropdown_bl__link">Daman and Diu</a>
                                                <a href="#" class="dropdown_bl__link">Delhi</a>
                                                <a href="#" class="dropdown_bl__link">Goa</a>
                                                <a href="#" class="dropdown_bl__link">Gujarat</a>
                                                <a href="#" class="dropdown_bl__link">Haryana</a>
                                                <a href="#" class="dropdown_bl__link">Himachal Pradesh</a>
                                                <a href="#" class="dropdown_bl__link">Jammu and Kashmir</a>
                                                <a href="#" class="dropdown_bl__link">Jharkhand</a>
                                                <a href="#" class="dropdown_bl__link">Karnataka</a>
                                                <a href="#" class="dropdown_bl__link">Kerala</a>
                                                <a href="#" class="dropdown_bl__link">Ladakh</a>
                                                <a href="#" class="dropdown_bl__link">Lakshadweep Islands</a>
                                                <a href="#" class="dropdown_bl__link">Madhya Pradesh</a>
                                                <a href="#" class="dropdown_bl__link">Maharashtra</a>
                                                <a href="#" class="dropdown_bl__link">Manipur</a>
                                                <a href="#" class="dropdown_bl__link">Meghalaya</a>
                                                <a href="#" class="dropdown_bl__link">Mizoram</a>
                                                <a href="#" class="dropdown_bl__link">Nagaland</a>
                                                <a href="#" class="dropdown_bl__link">Odisha</a>
                                                <a href="#" class="dropdown_bl__link">Pondicherry</a>
                                                <a href="#" class="dropdown_bl__link">Punjab</a>
                                                <a href="#" class="dropdown_bl__link">Rajasthan</a>
                                                <a href="#" class="dropdown_bl__link">Sikkim</a>
                                                <a href="#" class="dropdown_bl__link">Tamil Nadu</a>
                                                <a href="#" class="dropdown_bl__link">Telangana</a>
                                                <a href="#" class="dropdown_bl__link">Tripura</a>
                                                <a href="#" class="dropdown_bl__link">UP</a>
                                                <a href="#" class="dropdown_bl__link">Uttar Pradesh</a>
                                                <a href="#" class="dropdown_bl__link">Uttarakhand</a>
                                                <a href="#" class="dropdown_bl__link">West Bengal</a>
                                                <div class="dropdown_bl__no-record d-none">No record found</div>
                                                <!--<a href="#" class="dropdown_bl__link search-more">Search more…</a>-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <input type="text" placeholder="Zip/Postal Code">
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <div class="search__wrap position-relative">
                                            <input type="text" class="form-control search__input" placeholder="Country" value="" autocomplete="off">
                                            <span class="search__arrow"><i class="ri-arrow-down-s-fill"></i></span>
                                            <div class="dropdown_bl search__dropdown">
                                                <a href="#" class="dropdown_bl__link">Afghanistan</a>
                                                <a href="#" class="dropdown_bl__link">Albania</a>
                                                <a href="#" class="dropdown_bl__link">Algeria</a>
                                                <a href="#" class="dropdown_bl__link">Andorra</a>
                                                <a href="#" class="dropdown_bl__link">Angola</a>
                                                <a href="#" class="dropdown_bl__link">Antigua and Barbuda</a>
                                                <a href="#" class="dropdown_bl__link">Argentina</a>
                                                <a href="#" class="dropdown_bl__link">Armenia</a>
                                                <a href="#" class="dropdown_bl__link">Australia</a>
                                                <a href="#" class="dropdown_bl__link">Austria</a>
                                                <a href="#" class="dropdown_bl__link">Azerbaijan</a>
                                                <a href="#" class="dropdown_bl__link">Bahamas</a>
                                                <a href="#" class="dropdown_bl__link">Bahrain</a>
                                                <a href="#" class="dropdown_bl__link">Bangladesh</a>
                                                <a href="#" class="dropdown_bl__link">Barbados</a>
                                                <a href="#" class="dropdown_bl__link">Belarus</a>
                                                <a href="#" class="dropdown_bl__link">Belgium</a>
                                                <a href="#" class="dropdown_bl__link">Belize</a>
                                                <a href="#" class="dropdown_bl__link">Benin</a>
                                                <a href="#" class="dropdown_bl__link">Bhutan</a>
                                                <a href="#" class="dropdown_bl__link">Bolivia</a>
                                                <a href="#" class="dropdown_bl__link">Bosnia and Herzegovina</a>
                                                <a href="#" class="dropdown_bl__link">Botswana</a>
                                                <a href="#" class="dropdown_bl__link">Brazil</a>
                                                <a href="#" class="dropdown_bl__link">Brunei Darussalam</a>
                                                <a href="#" class="dropdown_bl__link">Bulgaria</a>
                                                <a href="#" class="dropdown_bl__link">Burkina Faso</a>
                                                <a href="#" class="dropdown_bl__link">Burundi</a>
                                                <a href="#" class="dropdown_bl__link">Cabo Verde</a>
                                                <a href="#" class="dropdown_bl__link">Cambodia</a>
                                                <a href="#" class="dropdown_bl__link">Cameroon</a>
                                                <a href="#" class="dropdown_bl__link">Canada</a>
                                                <a href="#" class="dropdown_bl__link">Central African Republic</a>
                                                <a href="#" class="dropdown_bl__link">Chad</a>
                                                <a href="#" class="dropdown_bl__link">Chile</a>
                                                <a href="#" class="dropdown_bl__link">China</a>
                                                <a href="#" class="dropdown_bl__link">Colombia</a>
                                                <a href="#" class="dropdown_bl__link">Comoros</a>
                                                <a href="#" class="dropdown_bl__link">Congo</a>
                                                <a href="#" class="dropdown_bl__link">Costa Rica</a>
                                                <a href="#" class="dropdown_bl__link">Côte d'Ivoire</a>
                                                <a href="#" class="dropdown_bl__link">Croatia</a>
                                                <a href="#" class="dropdown_bl__link">Cuba</a>
                                                <a href="#" class="dropdown_bl__link">Cyprus</a>
                                                <a href="#" class="dropdown_bl__link">Czech Republic</a>
                                                <a href="#" class="dropdown_bl__link">Democratic People's Republic of Korea (North Korea)</a>
                                                <a href="#" class="dropdown_bl__link">Democratic Republic of the Cong</a>
                                                <a href="#" class="dropdown_bl__link">Denmark</a>
                                                <a href="#" class="dropdown_bl__link">Djibouti</a>
                                                <a href="#" class="dropdown_bl__link">Dominica</a>
                                                <a href="#" class="dropdown_bl__link">Dominican Republic</a>
                                                <a href="#" class="dropdown_bl__link">Ecuador</a>
                                                <a href="#" class="dropdown_bl__link">Egypt</a>
                                                <a href="#" class="dropdown_bl__link">El Salvador</a>
                                                <a href="#" class="dropdown_bl__link">Equatorial Guinea</a>
                                                <a href="#" class="dropdown_bl__link">Eritrea</a>
                                                <a href="#" class="dropdown_bl__link">Estonia</a>
                                                <a href="#" class="dropdown_bl__link">Ethiopia</a>
                                                <a href="#" class="dropdown_bl__link">Fiji</a>
                                                <a href="#" class="dropdown_bl__link">Finland</a>
                                                <a href="#" class="dropdown_bl__link">France</a>
                                                <a href="#" class="dropdown_bl__link">Gabon</a>
                                                <a href="#" class="dropdown_bl__link">Gambia</a>
                                                <a href="#" class="dropdown_bl__link">Georgia</a>
                                                <a href="#" class="dropdown_bl__link">Germany</a>
                                                <a href="#" class="dropdown_bl__link">Ghana</a>
                                                <a href="#" class="dropdown_bl__link">Greece</a>
                                                <a href="#" class="dropdown_bl__link">Grenada</a>
                                                <a href="#" class="dropdown_bl__link">Guatemala</a>
                                                <a href="#" class="dropdown_bl__link">Guinea</a>
                                                <a href="#" class="dropdown_bl__link">Guinea-Bissau</a>
                                                <a href="#" class="dropdown_bl__link">Guyana</a>
                                                <a href="#" class="dropdown_bl__link">Haiti</a>
                                                <a href="#" class="dropdown_bl__link">Honduras</a>
                                                <a href="#" class="dropdown_bl__link">Hungary</a>
                                                <a href="#" class="dropdown_bl__link">Iceland</a>
                                                <a href="#" class="dropdown_bl__link">India</a>
                                                <a href="#" class="dropdown_bl__link">Indonesia</a>
                                                <a href="#" class="dropdown_bl__link">Iran</a>
                                                <a href="#" class="dropdown_bl__link">Iraq</a>
                                                <a href="#" class="dropdown_bl__link">Ireland</a>
                                                <a href="#" class="dropdown_bl__link">Israel</a>
                                                <a href="#" class="dropdown_bl__link">Italy</a>
                                                <a href="#" class="dropdown_bl__link">Jamaica</a>
                                                <a href="#" class="dropdown_bl__link">Japan</a>
                                                <a href="#" class="dropdown_bl__link">Jordan</a>
                                                <a href="#" class="dropdown_bl__link">Kazakhstan</a>
                                                <a href="#" class="dropdown_bl__link">Kenya</a>
                                                <a href="#" class="dropdown_bl__link">Kiribati</a>
                                                <a href="#" class="dropdown_bl__link">Kuwait</a>
                                                <a href="#" class="dropdown_bl__link">Kyrgyzstan</a>
                                                <a href="#" class="dropdown_bl__link">Lao People's Democratic Republic (Laos)</a>
                                                <a href="#" class="dropdown_bl__link">Latvia</a>
                                                <a href="#" class="dropdown_bl__link">Lebanon</a>
                                                <a href="#" class="dropdown_bl__link">Lesotho</a>
                                                <a href="#" class="dropdown_bl__link">Liberia</a>
                                                <a href="#" class="dropdown_bl__link">Libya</a>
                                                <a href="#" class="dropdown_bl__link">Liechtenstein</a>
                                                <a href="#" class="dropdown_bl__link">Lithuania</a>
                                                <a href="#" class="dropdown_bl__link">Luxembourg</a>
                                                <a href="#" class="dropdown_bl__link">Macedonia</a>
                                                <a href="#" class="dropdown_bl__link">Madagascar</a>
                                                <a href="#" class="dropdown_bl__link">Malawi</a>
                                                <a href="#" class="dropdown_bl__link">Malaysia</a>
                                                <a href="#" class="dropdown_bl__link">Maldives</a>
                                                <a href="#" class="dropdown_bl__link">Mali</a>
                                                <a href="#" class="dropdown_bl__link">Malta</a>
                                                <a href="#" class="dropdown_bl__link">Marshall Islands</a>
                                                <a href="#" class="dropdown_bl__link">Mauritania</a>
                                                <a href="#" class="dropdown_bl__link">Mauritius</a>
                                                <a href="#" class="dropdown_bl__link">Mexico</a>
                                                <a href="#" class="dropdown_bl__link">Micronesia (Federated States of)</a>
                                                <a href="#" class="dropdown_bl__link">Monaco</a>
                                                <a href="#" class="dropdown_bl__link">Mongolia</a>
                                                <a href="#" class="dropdown_bl__link">Montenegro</a>
                                                <a href="#" class="dropdown_bl__link">Morocco</a>
                                                <a href="#" class="dropdown_bl__link">Mozambique</a>
                                                <a href="#" class="dropdown_bl__link">Myanmar</a>
                                                <a href="#" class="dropdown_bl__link">Namibia</a>
                                                <a href="#" class="dropdown_bl__link">Nauru</a>
                                                <a href="#" class="dropdown_bl__link">Nepal</a>
                                                <a href="#" class="dropdown_bl__link">Netherlands</a>
                                                <a href="#" class="dropdown_bl__link">New Zealand</a>
                                                <a href="#" class="dropdown_bl__link">Nicaragua</a>
                                                <a href="#" class="dropdown_bl__link">Niger</a>
                                                <a href="#" class="dropdown_bl__link">Nigeria</a>
                                                <a href="#" class="dropdown_bl__link">Norway</a>
                                                <a href="#" class="dropdown_bl__link">Oman</a>
                                                <a href="#" class="dropdown_bl__link">Pakistan</a>
                                                <a href="#" class="dropdown_bl__link">Palau</a>
                                                <a href="#" class="dropdown_bl__link">Panama</a>
                                                <a href="#" class="dropdown_bl__link">Papua New Guinea</a>
                                                <a href="#" class="dropdown_bl__link">Paraguay</a>
                                                <a href="#" class="dropdown_bl__link">Peru</a>
                                                <a href="#" class="dropdown_bl__link">Philippines</a>
                                                <a href="#" class="dropdown_bl__link">Poland</a>
                                                <a href="#" class="dropdown_bl__link">Portugal</a>
                                                <a href="#" class="dropdown_bl__link">Qatar</a>
                                                <a href="#" class="dropdown_bl__link">Republic of Korea (South Korea)</a>
                                                <a href="#" class="dropdown_bl__link">Republic of Moldova</a>
                                                <a href="#" class="dropdown_bl__link">Romania</a>
                                                <a href="#" class="dropdown_bl__link">Russian Federation</a>
                                                <a href="#" class="dropdown_bl__link">Rwanda</a>
                                                <a href="#" class="dropdown_bl__link">Saint Kitts and Nevis</a>
                                                <a href="#" class="dropdown_bl__link">Saint Lucia</a>
                                                <a href="#" class="dropdown_bl__link">Saint Vincent and the Grenadines</a>
                                                <a href="#" class="dropdown_bl__link">Samoa</a>
                                                <a href="#" class="dropdown_bl__link">San Marino</a>
                                                <a href="#" class="dropdown_bl__link">Sao Tome and Principe</a>
                                                <a href="#" class="dropdown_bl__link">Saudi Arabia</a>
                                                <a href="#" class="dropdown_bl__link">Senegal</a>
                                                <a href="#" class="dropdown_bl__link">Serbia</a>
                                                <a href="#" class="dropdown_bl__link">Seychelles</a>
                                                <a href="#" class="dropdown_bl__link">Sierra Leone</a>
                                                <a href="#" class="dropdown_bl__link">Singapore</a>
                                                <a href="#" class="dropdown_bl__link">Slovakia</a>
                                                <a href="#" class="dropdown_bl__link">Slovenia</a>
                                                <a href="#" class="dropdown_bl__link">Solomon Islands</a>
                                                <a href="#" class="dropdown_bl__link">Somalia</a>
                                                <a href="#" class="dropdown_bl__link">South Africa</a>
                                                <a href="#" class="dropdown_bl__link">South Sudan</a>
                                                <a href="#" class="dropdown_bl__link">Spain</a>
                                                <a href="#" class="dropdown_bl__link">Sri Lanka</a>
                                                <a href="#" class="dropdown_bl__link">Sudan</a>
                                                <a href="#" class="dropdown_bl__link">Suriname</a>
                                                <a href="#" class="dropdown_bl__link">Swaziland</a>
                                                <a href="#" class="dropdown_bl__link">Sweden</a>
                                                <a href="#" class="dropdown_bl__link">Switzerland</a>
                                                <a href="#" class="dropdown_bl__link">Syrian Arab Republic</a>
                                                <a href="#" class="dropdown_bl__link">Tajikistan</a>
                                                <a href="#" class="dropdown_bl__link">Thailand</a>
                                                <a href="#" class="dropdown_bl__link">Timor-Leste</a>
                                                <a href="#" class="dropdown_bl__link">Togo</a>
                                                <a href="#" class="dropdown_bl__link">Tonga</a>
                                                <a href="#" class="dropdown_bl__link">Trinidad and Tobago</a>
                                                <a href="#" class="dropdown_bl__link">Tunisia</a>
                                                <a href="#" class="dropdown_bl__link">Turkey</a>
                                                <a href="#" class="dropdown_bl__link">Turkmenistan</a>
                                                <a href="#" class="dropdown_bl__link">Tuvalu</a>
                                                <a href="#" class="dropdown_bl__link">Uganda</a>
                                                <a href="#" class="dropdown_bl__link">Ukraine</a>
                                                <a href="#" class="dropdown_bl__link">United Arab Emirates</a>
                                                <a href="#" class="dropdown_bl__link">United Kingdom of Great Britain and Northern Ireland</a>
                                                <a href="#" class="dropdown_bl__link">United Republic of Tanzania</a>
                                                <a href="#" class="dropdown_bl__link">United States of America</a>
                                                <a href="#" class="dropdown_bl__link">Uruguay</a>
                                                <a href="#" class="dropdown_bl__link">Uzbekistan</a>
                                                <a href="#" class="dropdown_bl__link">Vanuatu</a>
                                                <a href="#" class="dropdown_bl__link">Venezuela</a>
                                                <a href="#" class="dropdown_bl__link">Vietnam</a>
                                                <a href="#" class="dropdown_bl__link">Yemen</a>
                                                <a href="#" class="dropdown_bl__link">Zambia</a>
                                                <a href="#" class="dropdown_bl__link">Zimbabwe</a>
                                                <div class="dropdown_bl__no-record d-none">No record found</div>
                                                <!--<a href="#" class="dropdown_bl__link search-more">Search more…</a>-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Document Required</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Approval of Document</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" />
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-4 mt-2">
                            <div class="input-line">
                                <label for="AwardDate">Date of Award</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="AwardDate" class="form-control search__input" placeholder="dd/mm/yyyy" value="" autocomplete="off">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1">
                            <label style="font-weight:600;">Upload Document</label>
                            <div class="row">
                                <div class="col-sm-12 mt-1">
                                    <div id="dropzone" class="dropzone">
                                        <input type="file" id="fileInput" multiple hidden>
                                        <div class="mb-2">
                                            <i class="display-4 text-muted ri-upload-cloud-2-fill"></i>
                                        </div>
                                        <h3>Drop files here or click to upload</h3>
                                    </div>
                                    <table class="table table-hover table-striped tbl_new tbl_fileup">
                                        <thead>
                                            <tr>
                                                <th>File</th>
                                                <th>Uploaded by</th>
                                                <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody id="fileTable"></tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="offcanvas-foorter border-top p-2 text-center">
            <button type="button" class="sub-soft-btn fs-18" style="display:inline;"><i class="ri-save-3-line"></i></button>
        </div>
    </div>
<div class="offcanvas offcanvas-end" tabindex="-1" id="addfiles">
        <div class="offcanvas-header justify-content-between border-bottom p-2">
            <h5 class="offcanvas-title">Upload Documents</h5>
            <!--<button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>-->
            <div class="hstack gap-1">
                <button type="button" onclick="fs1Function()" class="sub-soft-btn fsr fs-18"><i class="ri-fullscreen-fill"></i></button>
                <button type="button" onclick="fse1Function()" class="sub-soft-btn fsre fs-18"><i class="ri-fullscreen-exit-fill"></i></button>
                <button type="button" class="sub-soft-btn danger fs-18" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ri-close-fill"></i></button>
            </div>
        </div>
        <div class="offcanvas-body p-0 overflow-hidden">
            <div data-simplebar style="height: calc(100vh - 10px);">
                <div class="p-3">
                    <input type="file" id="fileupload" multiple hidden  onchange="angular.element(this).scope().filesSelected(this.files)">

                    <div id="dropZone" class="dropzone"
                     ng-click="openFilePicker($event)"
                     ng-class="{'highlight': isDragging}">
                    <i class="ri-upload-cloud-2-fill fs-6xl"></i><br />
                    Drag & Drop files here or click to upload
                </div>
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
                    <div class="row" style="display:none">
                    <div class="col-4"></div>
                    <div class="col-4"></div>
                <div class="col-4" style="text-align:right;">
                    <span class="form-switch form-switch-lg"
      data-bs-toggle="tooltip"
      title="view all files">

    <input type="checkbox"
           class="form-check-input"
           ng-model="chkall"
           ng-change="viewallfiles()"
           role="switch">
</span></div>
                    </div>
                    <div style="max-height:400px;overflow-x:auto;">
                    <table class='table align-middle table-striped table-hover table-bordered table-sm datatable tbl_new tbl_fileup'>
<thead>
                                            <tr>
                                                <th></th>
                                                <th>File</th>
                                                <th>Uploaded by</th>
                                                <th>Action</th>
                                            </tr>
                                        </thead>
                     
<tr ng-repeat="file in showfileslist | orderBy:'-id' track by file.id"
    ng-if="file.ftype=='project' || file.isshow">

                          <td width="45px">
                              <div class="attch_icon" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}">

                                  <i class="ri"
                                      ng-class="{
                                      'ri-image-2-fill': file.fext && (file.fext.toLowerCase() == '.jpg' || file.fext.toLowerCase() == '.png'),
                                      ' ri-file-pdf-fill': file.fext && file.fext.toLowerCase() == '.pdf',
                                      'ri-file-excel-fill': file.fext && (file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx'),
                                      'ri-file-word-fill': file.fext && (file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.doc')
         }"></i>
                              </div>
                          </td>

                          <td><span bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}"> {{ file.filedes.replace(file.fext,'').length > 40? (file.filedes.replace(file.fext,'') | limitTo:40) + '...': file.filedes.replace(file.fext,'')}}</span></td>
    <td>{{file.Uploadby}}</td>
                          <td class='text-right' style='width:30px;vertical-align:middle;'>
                             
                              <a href="javascript:void(0);" class="text-dark" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}" ng-href="{{ (file.fext.toLowerCase() == '.pdf' || file.fext.toLowerCase() == '.doc' || file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx') 
                      ? '/pms/ValidateDocument.aspx?isview=1&count=1&id=' + file.id 
                      : file.fileurl }}" 
       target="_blank"><i class="ri-eye-line" data-bs-toggle="tooltip"
         data-bs-placement="top"
         data-bs-trigger="hover"
         title="View"></i></a>
                              <a href="javascript:void(0);" class="text-dark" ng-click="downloadFile(file)"><i class="ri-download-2-line"></i></a>
                              <a href="javascript:void(0);" data-index="0" class="delete text-danger" ng-if ="file.ftype=='project'" ng-click="removedocumentnew(file.id)"><i class="ri-delete-bin-line" data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='Delete'></i></a>
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
            <div class="modal-body p-3 pb-2" ng-click="closelist()">                
               <div  id="ownerDropdown" class="dropdown" style="position:relative; width:100%;" click-outside="closelist()">

    <!-- Input box showing tags -->
    <div class="input-tags" ng-click="closelist()">
        <span class="tag" ng-repeat="owner in getSelectedOwners()">
            {{owner.name}} <span class="remove" ng-click="removeOwner(owner.id); $event.stopPropagation()"><i class="ri-close-fill"></i></span>
        </span>
        
    </div>
<input type="text" class="search__input"
               ng-model="searchOwner"
               placeholder="Select teams..."
    ng-click="openTagDropdown($event);stopTagEvent($event)"
               ng-focus="showOwnerDropdown = true"
               style="width:100%;" />
    <!-- Dropdown -->
    <div class="dropdown-menu"
         ng-show="showOwnerDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

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
.dropdown-menu div:hover {
    background-color: #e0e0e0;
}
                </style>

            </div>
        </div>
    </div>
</div>
    </div>

    <script src="/pms/new/assets/js/jquery.min.js"></script>
    <script src="/pms/new/assets/js/moment.min.js"></script>
    <script src="/pms/new/assets/js/daterangepicker.min.js"></script>

       <script>
    function fsFunction(){
        var element = document.getElementById("addnewproject");
        element.classList.add("canvas-fullscreen");
    }
    function fseFunction(){
        var element = document.getElementById("addnewproject");
        element.classList.remove("canvas-fullscreen");
    }
    </script>
    <script>
    function fs1Function(){
        var element = document.getElementById("addfiles");
        element.classList.add("canvas-fullscreen");
    }
    function fse1Function(){
        var element = document.getElementById("addfiles");
        element.classList.remove("canvas-fullscreen");
    }
    </script>
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
    <script src="/pms/assets/js/jscolor.js"></script>
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
                    input.placeholder = hasTags ? 'Search tag' : originalPlaceholder;
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
    <script>

function initSearchDropdown() {

document.querySelectorAll('.search__wrap').forEach(wrapper => {

const input = wrapper.querySelector('.search__input');
const dropdown = wrapper.querySelector('.search__dropdown');
const arrow = wrapper.querySelector('.search__arrow');
const items = wrapper.querySelectorAll('.dropdown_bl__link:not(.search-more)');

function openDropdown() {
dropdown.style.display = 'block';
wrapper.classList.add('open');
}

function closeDropdown() {
dropdown.style.display = 'none';
wrapper.classList.remove('open');
}

input.addEventListener('focus', openDropdown);

if (arrow) {
arrow.addEventListener('click', e => {
e.stopPropagation();
dropdown.style.display === 'block' ? closeDropdown() : openDropdown();
input.focus();
});
}

items.forEach(item => {
item.addEventListener('click', e => {
e.preventDefault();
closeDropdown();
});
});

document.addEventListener('click', e => {
if (!wrapper.contains(e.target)) {
closeDropdown();
}
});

});

}
</script>
</asp:Content>

