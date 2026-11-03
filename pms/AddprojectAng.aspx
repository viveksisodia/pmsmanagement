<%@ Page Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="AddprojectAng.aspx.cs" Inherits="AddprojectAng" ValidateRequest="false" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
  <script src="/pms/angularmaster/js/angular.min.js"></script>
<script src="/pms/angularmaster/js/angular-sanitize.js"></script>
<script src="/pms/angularmaster/js/dirPagination.js"></script>  
<script src="/pms/angularmaster/jsapp/pmsapp.js?v=<%=DateTime.Now.Ticks %>"></script> 
<script src="/pms/projects/app/mproject.service.js?v=<%=DateTime.Now.Ticks %>"></script>
<script src="/pms/projects/app/mproject.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script> 
    <style>
        .form-switch-lg .form-check-input {
    width: 45px;
    height: 22px;
    cursor: pointer;
}

.form-switch-lg .form-check-input:checked {
    background-color: #28a745;
    border-color: #28a745;
}

.form-switch-lg .form-check-input:focus {
    box-shadow: none;
}
    </style>
    <style>
        .files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.issue_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.dependency_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}

    </style>
    <style>
/*.dataTables_scrollBody {min-height:300px;}*/
.dataTables_scrollBody {height: calc(100vh - 370px) !important;}
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
.owner-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:9px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}

a.owner-wrp {position:relative;}
a.owner-wrp:hover .owner-del {display:block !important;}

.tbl_view tr td {padding:0 !important}
.tbl_view tr td .progress-xl {
    height: 30px;
    border-radius:0px !important;
}
.tbl_view tr td .datepicker {line-height:30px;border:0 !important;padding:0 4px;}
.tbl_view tr td .dayswrapper {padding:6px;}
.no-caret::after {display: none !important;}
</style>
   <style>
        

#dropZone {border:2px dashed #ccc;padding:20px;text-align:center;border-radius:10px;cursor:pointer;transition: 0.3s;}
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
.project-table thead .col-sn,
.project-table thead .col-action,
.project-table thead .col-client
.project-table thead .col-projectid
.project-table thead .col-projectname
{
    z-index: 25;
    background: #f8f9fa;
}

.col-sn {min-width:30px; left: 0; }
.col-action { min-width:50px; left:30px; }
.col-client { min-width:200px; left: 80px; }
.col-projectid { min-width:100px; left:280px; }
.col-projectname {min-width:200px; left:380px; }
.sticky-col {
    position: sticky;
    background: #fff;
    z-index: 5;
    border-right: 1px solid #ddd;
}
.project-table thead th {
   
    background: #f8f9fa;
    border: 1px solid #e5e7eb;
}

  
    </style>
    <style>
        .search_box {position:relative;}
.search_box i {position:absolute;top:4px;left:7px;font-size:16px;}
.search_box input.form-control {padding-left:28px;color:#000;border-radius:4px;height:32px;border: 1px solid #ddd !important;}
.search_box input.form-control:focus {border-color:#804574 !important;}
.form-select {padding:5px;border-color: #ddd;background-position: right .3rem center;}
.form-select:focus {outline:none;box-shadow:none;border-color:#804574;}
    </style>
    <style>
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
    <style>
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
    top: 5px;
    left:12px;
    right:200;
    background: #fff;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding:5px;
    height:35px;
    z-index: 9999;
    box-shadow: 0 4px 4px rgba(0,0,0,0.1);

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
    <div class="page-content" style="padding-bottom:0px;">
     
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <div class="inner-head">
                                        <div class="opp-ico orange">
                                            <i class="ri-pencil-ruler-2-line"></i>
                                        </div>
                                        <div class="head-txt">
                                            <p><%=fns.getltext("Project Management", "2")%></p>                                                    
                                            <a href="javascript:void(0);" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" data-bs-auto-close="outside" style="display:none;">
                                                <h2><label id="ContentPlaceHolder1_lblowner">All Projects</label> <i class="bi bi-caret-down-fill"></i></h2>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end" style="display:none;">
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(0)"><span class="align-middle">All Projects</span></a>                        
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(1)"><span class="align-middle">Under Negotiationaa</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(2)"><span class="align-middle">Working</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(3)"><span class="align-middle">Stuck</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(4)"><span class="align-middle">Done</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(5)"><span class="align-middle">under progress</span></a>
                                                <a class="dropdown-item fs-sm" href="#" onclick="GetStatussrch(6)"><span class="align-middle">ug process</span></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm mt-1">
                                    <div class="d-flex justify-content-sm-end">
                                    <div class="btn-group mt-4 mt-sm-0" role="group" aria-label="Basic example">
                                      <%--<%if (ischeckCreate == 1)
                                          {%>--%>
                                        <a href="javascript:void(0);" class="btn btn-outline-primary btn-icon" data-bs-toggle="offcanvas" data-bs-target="#addproject" onclick="loadsrc('inner_projectpopup','/pms/AddprojectPopup.aspx')"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add New"><i class="ri-add-line"></i></span></a>
                                       <%-- <%} %>--%>
                                      
                                        <%-- <a href="javascript:void(0);" style="display:none;" type="button" class="btn btn-outline-primary btn-icon" data-bs-toggle="modal" data-bs-target="#import_mdl"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Import"><i class="ri-download-2-line"></i></span></a>--%>


                                        <div class="modal fade" id="import_mdl" data-bs-backdrop="static"
                                            data-bs-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true">
                                            <div class="modal-dialog modal-dialog-centered" role="document">
                                                <div class="modal-content">
                                                    <div class="modal-body text-center p-3">
                                                        <div class="mt-4">
                                                            <h4 class="mb-1">Import Project</h4>
                                                            <p class="text-muted mb-3">Download Our <a href="Downloadexcel.aspx"><b>Sample File</b></a> To Get Started.</p>
                                                            
                                                            <div class="input-group mb-3">
                                                        <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server"  />
                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator4" SetFocusOnError="true" 
                               ValidationGroup="gi"  runat="server" ControlToValidate="FileUpload1"
                                ErrorMessage="Accepts Excel Files Only !" ValidationExpression="^.*\.xls[xm]?$"
                                Display="Dynamic"></asp:RegularExpressionValidator>

                                                            </div>
                                                            <div class="hstack gap-2 justify-content-center">
                                                                <a href="javascript:void(0);" class="btn btn-danger" data-bs-dismiss="modal"><i class="ri-close-line me-1 align-middle"></i> Close</a>
                                                            </div>
                                                             <div id="dvbutton" style="display:none">
                                              
                                                </div>
                                            <div style="display:none">
                                             
                                                </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                         <%-- excel import data end--%>



                                        
                                    </div>
                                    </div>
                                </div>                                
                            </div> 
                        </div>
                       
                        <div class="card-body" ng-app="PMSerp" ng-controller="mprojectCtrl">
                            <div class="sub-header">
    <div class="row mb-2">
        <div class="col-lg-6 col-md-6">
            <select class="form-select" ng-model="pageSize" ng-options="size for size in pageSizes" style="width:55px;"></select>                
            <%--<div class="sub-left">               

                <div class="sub-page-title">Customers</div>
                <a href="" ng-click="openAddCustomer('new')" class="sub-soft-btn" data-bs-toggle="tooltip" title="+ New"><i class="ri-add-line"></i></a>
                <a href="" data-bs-toggle="tooltip" title="List View" ng-click="setView('table')" ng-class="{'active': view == 'table'}" class="sub-soft-btn"><i class="ri-list-check"></i></a>
                <a href="" data-bs-toggle="tooltip" title="Tiles View" ng-click="setView('tile')" ng-class="{'active': view == 'tile'}" class="btn_top_addnew"><i class="ri-grid-fill"></i></a>
                <a class="sub-soft-btn" data-bs-toggle="tooltip" title="Download Excel" ng-click="downloadExcel()"><i class="ri-file-excel-2-line"></i></a>
                <a class="sub-soft-btn" data-bs-toggle="tooltip" title="Download PDF" ng-click="downloadPDF()"><i class="las la-file-pdf"></i></a>
            </div>--%>
        </div>
        <div class="col-lg-6 col-md-6">
            <div class="d-flex justify-content-end align-items-center gap-2">
                <div class="search_box">
                    <input type="text" class="form-control" ng-model="searchText" placeholder="Search..." />
                    <i class="ri-search-line"></i>
                </div>
                
            </div>
        </div>
    </div>
</div>
                            <div class="table-responsive" style="height: calc(100vh - 350px);overflow:auto;">
                              
                                <table id='tbDetails' ng-hide="isLoading" style='width:100%;margin-bottom:0px;' cellpadding='0' cellspacing='0' border='0' class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable project-table tbl_view' >
       
    <thead>
        <tr>
        <th class="sticky-col col-sn">S.No.</th>
        <th class="sticky-col col-action">Action</th>
        <th class="sticky-col col-client" ng-click="sortData('ClientName')" >Client Name{{ getSortClass('ClientName') }}
       </th>
        <th class="sticky-col col-projectid" ng-click="sortData('ProjectID')">Project ID{{ getSortClass('ProjectID') }}</th>
        <th class="sticky-col col-projectname" ng-click="sortData('ProjectName')">Project Name{{ getSortClass('ProjectName') }}</th>
        <th style='min-width:100px;'>Status</th>
        <th style='min-width:80px;'>Priority</th>
        <th style='min-width:170px;'>Timeline</th>
        <th style="min-width:140px;">Extended Timeline</th>
        <th style="min-width:140px;">Completion Status</th>
        <th style='min-width:30px;'>Project</br> Head</th>
        <th style='min-width:120px;'>Team</th>
        <th style="min-width:100px;">Milestone</th>       
        <th style="min-width:100px;">Task</th>        
        <th style="min-width:100px;">Issues</th>       
        <th style="min-width:80px;">Start Date</th>
        <th style="min-width:80px;">End Date</th>       
        <%--<th style='min-width:120px;'>Tags</th>--%>
        <th style="min-width:80px;">Budget</th>
        <th style="min-width:80px;">Actual</th>
        <th style="min-width:200px;">Project Address</th>
        <th style="min-width:80px;">Award Date</th>   
        <th style="min-width:50px;">Approval</th>
		 <th  style="min-width:80px;text-align:center">Approval</br>Date</th>
         <th  style="min-width:50px;">Files</th>       
        </tr>
        </thead>
        <tbody>
		<tr dir-paginate="c in projectlist|orderBy:sortColumn:reverseSort | filter:searchText | itemsPerPage: pageSize track by $index " current-page="currentPage">
            <%--<td class="sticky-col col-sn" style="text-align:center;min-width:30px;max-width:30px;">{{$index + 1 }}</td>--%>
            <td class="sticky-col col-sn" style="text-align:center;min-width:30px;max-width:30px;">{{ ((currentPage - 1) * pageSize) + $index + 1 }}</td>
            <td class="sticky-col col-action" style="text-align:center"><a href="javascript:void(0);" style="display:" class="btn btn-info btn-xs" type="button" ng-click="loadformedit(c,'inner_projectpopup','/pms/AddprojectPopup.aspx?id='+c.id+' ')" data-bs-toggle="offcanvas" data-bs-target="#addproject"><i class="ri-edit-line"></i></a>
    <a href="javascript:void(0);" style="display:" class="btn btn-danger btn-xs" type="button" ng-click="deletedataproject(c)"><i class="ri-delete-bin-5-line"></i></a></td> </td>
            <td class="sticky-col col-client" style="text-align:left;padding-left: 5px !important;padding-right: 5px !important;">{{c.ClientName}}</td>
            <td class="sticky-col col-projectid" style="text-align:left;padding-left: 5px !important;padding-right: 5px !important;">{{c.ProjectID}}</td>
            <td class="sticky-col col-projectname" style="text-align:left;padding-left: 5px !important;padding-right: 5px !important;">
          
  
                            
<div class="d-flex" style="display:none;"><div class="flex-grow-1"><a href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup_multi" ng-click ="loadsrc_ang('inner_popup_multi','/pms/projects/?id=' +c.id+'')" >{{c.ProjectName}}</a> </div>
    <div class="gap-1 flex-shrink-0 align-self-center">
      <div class="action-menu p-0">
<span class="action-icon p-0" ng-click="togglePopover_action($event, c, projectlist, 'milestone', $index,projectlist.ProjectName); $event.preventDefault();" ><i class="bi bi-three-dots-vertical"></i></span>
   <div class="action-dropdown" >

    <a href="javascript:void(0);" class="position-relative" style="display:" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup2" ng-click="loadsrc(c,'inner_popup2','/pms/AddSTDependency.aspx?id='+c.id+'&mprojectid='+c.id+'&type=Project&ctype=ang','dependency')"><span class="btn-plus2" bs-tooltip data-bs-toggle="tooltip"  data-bs-title="+ Dependency" style="font-size:12px;height:22px;width:22px;margin-right:2px">D</span>
          <%--<span class="dependency_count">{{c.totaldependency}}</span>--%>
      </a>
    <a href="javascript:void(0);" class="position-relative" style="display:" type="button" data-bs-toggle="offcanvas" data-bs-target="#canvas_popup2" ng-click="loadsrc(c,'inner_popup2','/pms/AddIssuesPopup.aspx?id='+c.id+'&type=Project&ctype=ang','issue')"><span class="btn-plus2" bs-tooltip data-bs-toggle="tooltip"  data-bs-title="+ Issue" style="font-size:12px;height:22px;width:22px;">I</span>
            <%--<span class="issue_count">{{c.totalissue}}</span>--%>
        </a>
</div>
        </div>

    </div></div>
            </td>
           <td ng-style="{'background-color': c.status_color}"    style="position:relative">
   
    <div style="position:relative;">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c.status_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c,newstatus); $event.preventDefault()" >
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
            <td ng-style="{'background-color': c.priority_color}"   style="position:relative">
   
    <div style="position:relative;">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;"> {{ c.priority_name }}</a>
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name" ng-click="selectPriority(c,newpriority); $event.preventDefault()" >
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
  <a href="javascript:void(0);" class="timesheet-wrap"  style="width:100%">
     
    <div class="progress progress-xl bg-warning position-relative" bs-tooltip
     data-bs-toggle="tooltip"
     data-bs-title="{{c.leftdays}}">
      
      <!-- progress bar -->
      <div class="progress-bar bg-secondary" 
           role="progressbar" 
           ng-style="{'width': '100%'}"
          
          >
      </div>
      
      <!-- inner wrapper -->
      <div class="dayswrapper">
        <span class="fromtodate">{{ c.StartDate | date:'dd MMM yyyy' }} - {{ c.TargetDate | date:'dd MMM yyyy' }}</span>
        <span class="totaldays">{{ c.txtdays }} days</span>
      </div>
      
    </div>
  </a>

</td>
            <td style="min-width:140px;">
<div class="d-flex" style="width:100%">

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

</div>

            </td>
            <td>
  <div class="progress progress-xl cursor-pointer">
    <div class="progress-bar bg-success"
         role="progressbar"
        
        bs-tooltip
     data-bs-toggle="tooltip"
     data-bs-title="Complete ({{c.complete_value}}%)"
         ng-style="{'width': c.complete_value + '%'}">
      {{ c.complete_value }}%
    </div>

    <div class="progress-bar bg-warning"
         role="progressbar"
        
        bs-tooltip
     data-bs-toggle="tooltip" data-bs-title="Pending ({{c.pending_value}}%)"
         ng-style="{'width': c.pending_value + '%'}">
      <span ng-if="c.complete_value != '0'">
        {{ c.pending_value }}%
    </span>
<div ng-if="c.complete_value == '0'" bs-tooltip
     data-bs-toggle="tooltip"
     data-bs-title="Complete (0%)">
    <span>
        0%
    </span>
    </div>
    </div>
  </div>
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
<a href="javascript: void(0);"  ng-repeat="newteam in c.teams|limitTo:3" class="avatar-group-item owner-wrp" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}">
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
   <%-- <i class="ri-close-line owner-del" style="position:absolute; top:-6px; right:-2px; cursor:pointer;display:none;color:red;"
      ng-click="removeteams(c,newteam,'milestone')">
    </i>--%>
    <i class="bi bi-x-lg owner-del" ng-click="removeteams(c,newteam)"></i>

</a>
   <div ng-if="c.teams && c.teams.length > 3"
     class="avatar-group-item owner-wrp"
     bs-tooltip data-bs-toggle="tooltip"     
     data-bs-title="{{ getRemainingOwners(c.teams) }}">
       <div class="avatar-xxs">
    <div class="avatar-title rounded-circle" style="width:20px;height:20px;">
        +{{ c.teams.length - 3 }}
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
               <div class="d-flex">

  <!-- Progress Section -->
  <div class="flex-grow-1 align-self-center">
    <div class="progress progress-xl cursor-pointer">

      <!-- Completed -->
      <div ng-if="(c.completemile || 0) > 0" class="progress-bar bg-success" bs-tooltip 
           role="progressbar"
           ng-style="{'width': c.completemile_rate + '%'}"
           aria-valuemin="0"
           aria-valuemax="100"
           data-bs-toggle="tooltip"           
           data-bs-title="{{c.completemile_rate}}% [{{c.completemile}} of {{c.totalmile}} Milestone Completed]">

        {{c.completemile}}
      </div>

      <!-- Pending -->
      <div class="progress-bar bg-warning" bs-tooltip 
           role="progressbar"
           ng-style="{'width': c.pendingmile_rate + '%'}"
           aria-valuemin="0"
           aria-valuemax="100"
           data-bs-toggle="tooltip"           
           data-bs-title="{{c.pendingmile_rate}}% [{{c.pendingmile}} of {{c.totalmile}} Milestone Not Completed]">

        {{c.pendingmile}}
      </div>

    </div>
  </div>

  <!-- Add Button -->
  <div class="gap-1 flex-shrink-0 align-self-center">
    <a href="javascript:void(0);"
       type="button"
       data-bs-toggle="offcanvas"
       data-bs-target="#canvas_popup2"
       ng-click="loadsrc(c,'inner_popup2','/pms/AddmilstonePopup.aspx?id='+c.id+'&type=Project','milestone')">

      <span class="btn-plus2" bs-tooltip
            data-bs-toggle="tooltip"
            data-bs-title="+ Add Milestone">
        +
      </span>
    </a>
  </div>

</div>
   
</td>
           
            <td>
               <div class="d-flex">

  <!-- Progress Section -->
  <div class="flex-grow-1 align-self-center">
    <div class="progress progress-xl cursor-pointer">

      <!-- Completed -->
      <div ng-if="(c.completetask || 0) > 0"  class="progress-bar bg-success" bs-tooltip 
           role="progressbar"
           ng-style="{'width': c.completetask_rate + '%'}"
           aria-valuemin="0"
           aria-valuemax="100"
           data-bs-toggle="tooltip"           
           data-bs-title="{{c.completetask_rate}}% [{{c.completetask}} of {{c.totaltask}} Task Completed]">

        {{c.completetask}}
      </div>

      <!-- Pending -->
      <div class="progress-bar bg-warning" bs-tooltip 
           role="progressbar"
           ng-style="{'width': c.pendingtask_rate + '%'}"
           aria-valuemin="0"
           aria-valuemax="100"
           data-bs-toggle="tooltip"           
           data-bs-title="{{c.pendingtask_rate}}% [{{c.pendingtask}} of {{c.totaltask}} Task Not Completed]">

        {{c.pendingtask}}
      </div>

    </div>
  </div>

  <!-- Add Button -->
  <div class="gap-1 flex-shrink-0 align-self-center">
    <a href="javascript:void(0);"
       type="button"
       data-bs-toggle="offcanvas"
       data-bs-target="#canvas_popup2"
       ng-click="loadsrc(c,'inner_popup2','/pms/AddTaskPopup.aspx?id='+c.id+'&type=Project','task')">

      <span class="btn-plus2" bs-tooltip
            data-bs-toggle="tooltip"
            data-bs-title="+ Add Task">
        +
      </span>
    </a>
  </div>

</div>
   
</td>
            <td>
               <div class="d-flex">

  <!-- Progress Section -->
  <div class="flex-grow-1 align-self-center">
    <div class="progress progress-xl cursor-pointer">

      <!-- Completed -->
      <div ng-if="(c.completeissue || 0) > 0" class="progress-bar bg-success" bs-tooltip 
           role="progressbar"
           ng-style="{'width': c.completeissue_rate + '%'}"
           aria-valuemin="0"
           aria-valuemax="100"
           data-bs-toggle="tooltip"           
           data-bs-title="{{c.completeissue_rate}}% [{{c.completeissue}} of {{c.totalissue}} Issue Completed]">

        {{c.completeissue}}
      </div>

      <!-- Pending -->
      <div class="progress-bar bg-warning" bs-tooltip 
           role="progressbar"
           ng-style="{'width': c.pendingissue_rate + '%'}"
           aria-valuemin="0"
           aria-valuemax="100"
           data-bs-toggle="tooltip"           
           data-bs-title="{{c.pendingissue_rate}}% [{{c.pendingissue}} of {{c.totalissue}} Issue Not Completed]">

        {{c.pendingissue}}
      </div>

    </div>
  </div>

  <!-- Add Button -->
  <div class="gap-1 flex-shrink-0 align-self-center">
    <a href="javascript:void(0);"
       type="button"
       data-bs-toggle="offcanvas"
       data-bs-target="#canvas_popup2"
       ng-click="loadsrc(c,'inner_popup2','/pms/AddIssuesPopup.aspx?id='+c.id+'&type=Project','issue')">

      <span class="btn-plus2" bs-tooltip
            data-bs-toggle="tooltip"
           data-bs-title="+ Add Issue">
        +
      </span>
    </a>
  </div>

</div>
   
</td>
            <td style="text-align:center">{{ c.StartDate | date:'dd/MM/yyyy' }}</td>
            <td style="text-align:center">{{ c.TargetDate | date:'dd/MM/yyyy' }}</td>
          <%--  <td><div class="d-flex">

  <!-- TAG LIST -->
  <div class="flex-grow-1 align-self-center">

    <div class="d-flex">
      <div class="flex-grow-1 align-self-center">
        <div class="flex-wrap gap-2 d-flex">

          <!-- Show first 2 tags -->
          <a href="javascript:void(0);"
             ng-repeat="tag in c.tags | limitTo:1"
             data-bs-toggle="offcanvas"
             data-bs-target="#canvas_popup"
             ng-click="loadtags(c,'inner_popup','/pms/AddTagsPopup.aspx?id='+c.id+'&type=Project')">

            <span class="badge badge-label" bs-tooltip data-bs-toggle="tooltip" data-bs-title="{{tag.tag_name}}"
                  ng-style="{'background': tag.tag_color || '#eee'}">
              <i class="ti ti-point-filled"></i>
              {{tag.tag_name}}
            </span>
               
          </a>

          <!-- +More -->
          <span class="cursor-pointer" bs-tooltip
                ng-if="c.tags && c.tags.length> 1"
                data-bs-toggle="tooltip"
                data-bs-html="true"                
                data-bs-title="{{ getRemainingTags(c.tags) }}">

            +{{c.tags.length - 1}}
          </span>

        </div>
      </div>
    </div>

  </div>

  <!-- ADD TAG BUTTON -->
  <div class="gap-1 flex-shrink-0 align-self-center">
    <a href="javascript:void(0);"
       data-bs-toggle="modal"
       data-bs-target="#addtags"
       ng-click="openAddTagsModal(c)">

      <span class="btn-plus2" bs-tooltip
            data-bs-toggle="tooltip"
            data-bs-title="+ Add Tags">
        +
      </span>

    </a>
  </div>

</div></td>--%>
            <td style="text-align:center">{{c.Budget}}</td>
            <td style="text-align:center">{{c.Actual}}</td>
            
            <td style="text-align:left;padding-left: 5px !important;padding-right: 5px !important;">{{c.Completeaddress}}</td>
           <td style="text-align:center">{{ c.AwardDate | date:'dd/MM/yyyy' }}</td>
            <td style="text-align:center">{{ c.ApprovalDrawing == '1' ? 'Yes' : 'No' }}</td>
             <td style="text-align:center">{{ c.DrawingApprovalDate | date:'dd/MM/yyyy' }}</td>
            <td>
                <a href="javascript:void(0);" ng-click="loadprojectfiles(c,$index)" class="position-relative" data-bs-toggle="modal" data-bs-target="#addfiles"> <span class="btn-plus2" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="Upload Documents" style="font-size:12px;height:22px;width:22px;margin-left:9px"><i class="bi bi-file-earmark-arrow-up"></i></span><span class="files_count">{{ c.viewfiles.length || 0 }}</span></a>
            </td>
		</tr>
		</tbody>
		</table>

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
                
                
    <div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopup" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomin" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomout" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>
                                <div class="offcanvas offcanvas-end canvas_lg_screen" id="canvas_popup2" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="offcanvas-title" id="lblctitle">{{ modalTitle }}</h5>
                <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" ng-click="closepopup_canvas2()"   aria-label="Close"><i class="ph-x-bold"></i></button>
            </div>
                                    <div style="display:none;">
     <button type="button" id="btnclosemainpopup2" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomin2" onclick="fs2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomout2" onclick="fse2Function()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup2' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
</div>
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
                <div style="max-height:300px;overflow-y:auto;">

                  <table class='table align-middle table-striped table-hover table-nowrap table-bordered table-sm datatable'>
<tr ng-repeat="file in showfileslist | orderBy:'-id' track by file.id"
    ng-if="file.ftype=='project' || file.isshow">

                          <td width="45px">
                              <div class="attch_icon" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}">

                                  <i class="bi"
                                      ng-class="{
           'bi-file-earmark-image': file.fext && (file.fext.toLowerCase() == '.jpg' || file.fext.toLowerCase() == '.png'),
           'bi-file-earmark-pdf-fill': file.fext && file.fext.toLowerCase() == '.pdf',
                                      'bi bi-file-earmark-excel-fill': file.fext && (file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx'),
                                      'bi bi-file-earmark-word-fill': file.fext && (file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.doc')
         }"></i>
                              </div>
                          </td>

                          <td><span bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}"> {{ file.filedes.replace(file.fext,'') }}</span></td>

                          <td class='text-right' style='width:30px;vertical-align:middle;'>
                              <a bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}" ng-href="{{ (file.fext.toLowerCase() == '.pdf' || file.fext.toLowerCase() == '.doc' || file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx') 
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
                              <a href=""  ng-if ="file.ftype=='project'" ng-click="removedocumentnew(file.id)" class='btn btn-danger btn-icon btn-sm ms-1 me-1'><i class='ri-delete-bin-5-line' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='Delete'></i></a>
                          </td>

                      </tr>
</table>
                </div>

            </div>
        </div>
    </div>
</div>
                             
    <div id="ExtendedTimline" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
                                 <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="btnclsextended" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <%--<asp:Button ID="Button2" runat="server" Text="" OnClick="Button2_Click" />--%>
   </div>
                      <asp:HiddenField ID="hdnextendid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnextnhtml" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <input type="date" class="form-control datepicker" ng-model="extendObj.fromdate"    onclick="if(this.showPicker){this.showPicker();}">
                                   
                                        
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    
                                    <input type="date" class="form-control datepicker" ng-model="extendObj.todate" onclick="if(this.showPicker){this.showPicker();}">
                                         
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <a href="javascript:void(0);"  ng-click="SvaeExtended()" class='btn btn-primary'>Save</a>
                
            </div>
        </div>
    </div>
</div>   
                            <div class="offcanvas offcanvas-end" id="addproject" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
                                <asp:HiddenField ID="hdnnewprojectid" runat="server" />
                                <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
        <h5 class="offcanvas-title" id="offcanvasRightLabel">
    {{projectTitle}}
</h5>
        <div class="hstack gap-2">
             <div style="display:"><button type="button" id="btncls" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close" ><i class="ph-x-bold"></i></button></div>
            <div style="display:none"><button type="button" id="btncls2" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close" ng-click="closedata()"><i class="ph-x-bold"></i></button></div>
            <div style="display:none"><button type="button" id="btncls3" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close" ng-click="closedatanew()"><i class="ph-x-bold"></i></button></div>
            <div style="display:none"><button type="button" id="btncls4" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios"  ng-click="closedatanew()"></button></div>
        </div>
    </div>
    <iframe id='inner_projectpopup' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
  </div>
                          
               <div class="offcanvas offcanvas-end canvas_lg_screen canvas-fullscreen" id="canvas_popup_multi" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopupmulti" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoominmulti" onclick="fse3Functionmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomoutmulti" onclick="fs3Functionmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup_multi' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
   </div>                 
                                
                                <div ng-show="isLoading" class="loader-overlay">
<div class="loader"></div>
</div>

                            </div>
                            <!-- Paging Controls -->
<div class="pull-right">
<dir-pagination-controls
max-size="8" 
direction-links="true"
boundary-links="true">
</dir-pagination-controls>
</div>
                        </div>
                    </div>
                </div>
            </div>           
        </div>
    </div>
    

    <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script>
     <script src="/pms/assets/libs/sortablejs/Sortable.min.js"></script>
    <script src="/pms/assets/js/pages/nestable.init.js"></script>
    <link href="/pms/assets/js/select2.min.css" rel="stylesheet" type="text/css"> 
    <%--<script src="/pms/assets/js/jquery-3.7.0.js"></script>--%>
<script src="/pms/assets/js/select2.min.js"></script> 
    <script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/jscolor.js"></script>


    <script src="/pms/assets/js/fixedColumns.dataTables.js"></script>
<script src="/pms/assets/js/dataTables.fixedColumns.min.js"></script>


</asp:Content>


