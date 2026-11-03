<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowSetting.aspx.cs" Inherits="WorkflowSetting" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" ng-app="workflowApp">
<head runat="server">
    <title>QC Workflow</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" />

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.3/angular.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        [ng-cloak] { display:none !important; }
        body { background:#f6f8fb; font-family:Arial,sans-serif; }
        .page-wrap { padding:22px; }
        .card-box { background:#fff; border:1px solid #e5e8ee; border-radius:12px; box-shadow:0 2px 9px rgba(0,0,0,.04); margin-bottom:20px; }
        .card-head { padding:16px 18px; border-bottom:1px solid #edf0f4; display:flex; justify-content:space-between; align-items:center; gap:12px; }
        .card-body-custom { padding:18px; }
        .section-title { font-size:18px; font-weight:700; margin:0; }
        .small-muted { font-size:12px; color:#78828c; }
        .required:after { content:' *'; color:#dc3545; }
        .toolbar { display:flex; align-items:center; gap:8px; flex-wrap:wrap; }
        .workflow-grid-search { width:260px; }
        .workflow-grid-search .form-control { min-width:0; }
        .status-active { color:#198754; font-weight:700; }
        .status-inactive { color:#6c757d; font-weight:700; }
        .section-box { border:1px solid #dfe4eb; border-radius:10px; background:#fff; margin-bottom:16px; overflow:visible; }
        .section-head { padding:11px 14px; background:#f8fafc; border-bottom:1px solid #e7ebf0; display:flex; align-items:center; justify-content:space-between; gap:12px; }
        .section-title-row { display:flex; align-items:center; gap:10px; font-weight:700; }
        .type-pill { display:inline-block; padding:3px 10px; border-radius:14px; font-size:11px; font-weight:700; background:#eaf2ff; color:#1659b7; }
        .then-box { background:#fafbfd; border-top:1px solid #edf0f4; padding:14px; }
        .conditions-box { padding:14px; }
        .modal-xl { max-width:1320px; }
        .modal-body { max-height:76vh; overflow-y:auto; }
        .table td,.table th { vertical-align:middle; }
        .action-col { white-space:nowrap; }
        .branch-type-select { min-width:120px; }
        .delete-btn { min-width:72px; }
        .notification-options { display:flex; align-items:center; gap:18px; flex-wrap:wrap; }
        .notification-check { display:inline-flex; align-items:center; gap:6px; margin:0; cursor:pointer; white-space:nowrap; }
        .notification-check input { width:16px; height:16px; margin:0; }
        .bell-icon { font-size:18px; line-height:1; }
        .value-control { min-width:170px; }
        .top-links { display:flex; gap:8px; flex-wrap:wrap; justify-content:flex-end; }

        /* IN / NOT IN value picker - same pattern as Critical Workflow */
        .employee-picker { position:relative; }
        .employee-picker-control {
            min-height:31px;
            border:1px solid #ced4da;
            border-radius:.375rem;
            background:#fff;
            padding:4px 34px 4px 7px;
            display:flex;
            align-items:center;
            gap:5px;
            flex-wrap:wrap;
            cursor:pointer;
            position:relative;
        }
        .employee-picker-control:focus-within,
        .employee-picker-control.is-open {
            border-color:#86b7fe;
            box-shadow:0 0 0 .15rem rgba(13,110,253,.15);
        }
        .employee-picker-placeholder { color:#6c757d; padding:2px 4px; font-size:12px; }
        .employee-picker-arrow {
            position:absolute;
            right:10px;
            top:50%;
            transform:translateY(-50%);
            font-size:10px;
            color:#6c757d;
        }
        .employee-chip {
            display:inline-flex;
            align-items:center;
            gap:4px;
            max-width:150px;
            padding:2px 6px;
            border-radius:4px;
            background:#eef2f6;
            border:1px solid #dde3e8;
            font-size:11.5px;
            line-height:1.2;
        }
        .employee-chip-name {
            overflow:hidden;
            text-overflow:ellipsis;
            white-space:nowrap;
        }
        .employee-chip-remove {
            border:0;
            background:transparent;
            color:#6c757d;
            padding:0;
            line-height:1;
            font-size:14px;
            cursor:pointer;
        }
        .employee-picker-popup {
            position:absolute;
            z-index:1095;
            left:0;
            right:0;
            top:calc(100% + 4px);
            background:#fff;
            border:1px solid #ced4da;
            border-radius:.375rem;
            box-shadow:0 6px 18px rgba(0,0,0,.16);
            padding:8px;
            min-width:280px;
        }
        .employee-picker-popup.drop-up {
            top:auto;
            bottom:calc(100% + 4px);
        }
        .employee-picker-list {
            max-height:210px;
            overflow:auto;
            border:1px solid #eef0f3;
            border-radius:5px;
            margin-top:7px;
        }
        .employee-picker-item {
            display:flex;
            align-items:center;
            gap:8px;
            padding:7px 9px;
            cursor:pointer;
            margin:0;
            font-weight:400;
        }
        .employee-picker-item:hover { background:#f5f7fa; }
        .employee-picker-item input { margin:0; pointer-events:none; }
        .employee-picker-empty {
            padding:12px;
            color:#78828c;
            text-align:center;
            font-size:12px;
        }
        .employee-picker-footer {
            display:flex;
            align-items:center;
            justify-content:space-between;
            gap:8px;
            padding-top:7px;
            font-size:12px;
            color:#78828c;
        }

        /* QC condition picker is inside Bootstrap .table-responsive.
           Allow its popup to render outside the table instead of being clipped. */
        .qc-condition-table-responsive {
            overflow: visible !important;
        }

        /* Keep IN / NOT IN popup below the Value control, same as Critical Workflow screen. */
        .qc-condition-table-responsive .employee-picker-popup,
        .qc-condition-table-responsive .employee-picker-popup.drop-up {
            top: calc(100% + 4px) !important;
            bottom: auto !important;
        }

        /* Searchable Chosen dropdown: Department / Designation only */
        .chosen-container { width:100% !important; font-size:14px; }
        .chosen-container-single .chosen-single {
            height:31px;
            line-height:29px;
            border:1px solid #dee2e6;
            border-radius:.25rem;
            background:#fff;
            box-shadow:none;
        }
        .chosen-container-single .chosen-single div b { background-position:0 5px; }
        .chosen-container-active.chosen-with-drop .chosen-single {
            border-color:#86b7fe;
            box-shadow:0 0 0 .15rem rgba(13,110,253,.10);
            background:#fff;
        }
        .chosen-container .chosen-drop { z-index:1085; }
        .chosen-container-single .chosen-search input[type="text"] {
            width:100%;
            border:1px solid #ced4da;
            border-radius:4px;
            padding:6px 8px;
            background:#fff;
        }

        /* Keep Department / Designation search dropdown fully visible. */
        .workflow-action-table {
            overflow:visible !important;
        }
        .then-box {
            overflow:visible !important;
        }
        .chosen-container {
            position:relative;
            z-index:1090;
        }
        .chosen-container.chosen-with-drop {
            z-index:1100;
        }
        .chosen-container.chosen-drop-up .chosen-drop {
            top:auto !important;
            bottom:100% !important;
            margin-bottom:-1px;
            border-top:1px solid #aaa;
            border-bottom:0;
        }
    
        /* ===== Compact Workflow UI ===== */
        body {
            font-size: 13px;
        }

        .page-wrap {
            padding: 12px 16px;
        }

        h3 {
            font-size: 22px;
            margin-bottom: 4px;
        }

        .section-title {
            font-size: 16px;
        }

        .small-muted {
            font-size: 11px;
        }

        .card-box {
            margin-bottom: 12px;
            border-radius: 8px;
        }

        .card-head {
            padding: 10px 14px;
        }

        .card-body-custom {
            padding: 12px 14px;
        }

        .modal-header {
            padding: 10px 16px;
        }

        .modal-title {
            font-size: 20px;
        }

        .modal-body {
            padding: 10px 16px 12px;
        }

        .modal-footer {
            padding: 8px 16px;
        }

        .form-label {
            margin-bottom: 4px;
            font-size: 13px;
        }

        .form-control,
        .form-select {
            min-height: 34px;
            padding: 5px 10px;
            font-size: 13px;
        }

        .form-control-sm,
        .form-select-sm {
            min-height: 31px;
            padding: 4px 8px;
            font-size: 12.5px;
        }

        .row.g-3 {
            --bs-gutter-y: 0.65rem;
            --bs-gutter-x: 0.75rem;
        }

        .section-box {
            margin-bottom: 10px;
            border-radius: 8px;
        }

        .section-head {
            padding: 8px 12px;
        }

        .section-body {
            padding: 10px 12px;
        }

        .conditions-title,
        .actions-title {
            margin-bottom: 6px;
            font-size: 15px;
        }

        .table {
            font-size: 12.5px;
        }

        .table > :not(caption) > * > * {
            padding: 0.4rem 0.5rem;
        }

        .btn {
            font-size: 12.5px;
            padding: 5px 10px;
        }

        .btn-sm {
            font-size: 12px;
            padding: 4px 8px;
        }

        .notification-options {
            gap: 10px;
        }

        .notification-check {
            font-size: 12.5px;
        }

        .chosen-container {
            font-size: 12.5px;
        }

        .chosen-container-single .chosen-single {
            height: 31px;
            line-height: 29px;
        }

        .chosen-container-single .chosen-search input[type="text"] {
            padding: 5px 7px;
            font-size: 12.5px;
        }

        .workflow-grid-search {
            width: 230px;
        }

        .sticky-actions {
            padding: 8px 14px;
        }

        .form-check-label,
        .status-active,
        .status-inactive {
            font-size: 12.5px;
        }

        .branch-type-select {
            min-width: 110px;
        }

    
        /* ===== Extra Tight Spacing ===== */

        /* Header area */
        .card-head {
            padding: 7px 12px !important;
            min-height: 0 !important;
        }

        .card-body-custom {
            padding: 8px 12px !important;
        }

        .modal-header {
            padding: 7px 14px !important;
            min-height: 42px !important;
        }

        .modal-body {
            padding: 7px 14px 8px !important;
        }

        .modal-footer {
            padding: 6px 14px !important;
            min-height: 44px !important;
        }

        /* Workflow header controls */
        .row.g-3 {
            --bs-gutter-y: .35rem !important;
            --bs-gutter-x: .55rem !important;
        }

        .form-label {
            margin-bottom: 2px !important;
            line-height: 1.15 !important;
        }

        .form-control,
        .form-select {
            min-height: 30px !important;
            height: 30px !important;
            padding: 3px 8px !important;
            line-height: 1.2 !important;
        }

        textarea.form-control {
            height: auto !important;
        }

        /* Reduce large empty Description/header gaps */
        .workflow-header-box .card-body-custom,
        .workflow-header-box .card-body {
            padding-top: 7px !important;
            padding-bottom: 7px !important;
        }

        /* Main section spacing */
        .section-box {
            margin-bottom: 7px !important;
        }

        .section-head {
            padding: 6px 10px !important;
            min-height: 38px !important;
        }

        .section-body {
            padding: 7px 10px !important;
        }

        .conditions-title,
        .actions-title {
            margin: 0 0 4px 0 !important;
            padding: 0 !important;
            line-height: 1.2 !important;
        }

        /* Remove extra vertical whitespace around condition/action blocks */
        .conditions-block,
        .actions-block {
            margin-top: 0 !important;
            margin-bottom: 5px !important;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }

        /* Grid/table compact rows */
        .table {
            margin-bottom: 0 !important;
        }

        .table > :not(caption) > * > * {
            padding: .25rem .42rem !important;
            line-height: 1.15 !important;
        }

        .table thead th {
            padding-top: .3rem !important;
            padding-bottom: .3rem !important;
            white-space: nowrap;
        }

        .table tbody td {
            padding-top: .25rem !important;
            padding-bottom: .25rem !important;
        }

        .table .form-control,
        .table .form-select {
            height: 29px !important;
            min-height: 29px !important;
            padding: 2px 7px !important;
        }

        /* Buttons */
        .btn,
        .btn-sm {
            min-height: 29px !important;
            padding: 3px 8px !important;
            line-height: 1.2 !important;
        }

        /* Chosen controls */
        .chosen-container-single .chosen-single {
            height: 29px !important;
            line-height: 27px !important;
        }

        .chosen-container-single .chosen-single div b {
            background-position: 0 4px !important;
        }

        .chosen-container-single .chosen-search input[type="text"] {
            height: 29px !important;
            padding: 3px 6px !important;
        }

        /* Notification line */
        .notification-options {
            gap: 8px !important;
            min-height: 28px !important;
            align-items: center !important;
        }

        .notification-check {
            margin: 0 !important;
            line-height: 1 !important;
        }

        /* Reduce spacing before THEN Actions */
        .actions-title,
        .then-actions-title {
            margin-top: 4px !important;
        }

        /* Footer/sticky button area */
        .sticky-actions {
            padding: 6px 12px !important;
            min-height: 42px !important;
        }

        /* Existing workflow landing grid */
        .card-box .table-responsive {
            margin: 0 !important;
        }

    
        /* Workflow condition multi-select (IN / NOT IN) */
        .table .chosen-container-multi .chosen-choices {
            min-height: 29px !important;
            border: 1px solid #dee2e6 !important;
            border-radius: 4px !important;
            background: #fff !important;
            padding: 1px 4px !important;
        }

        .table .chosen-container-multi .chosen-choices li.search-field input[type="text"] {
            height: 25px !important;
            margin: 0 !important;
            font-size: 12px !important;
        }

        .table .chosen-container-multi .chosen-choices li.search-choice {
            margin: 2px 3px 2px 0 !important;
            padding-top: 3px !important;
            padding-bottom: 3px !important;
            font-size: 11px !important;
        }

    
        /* ===== User Actions configuration ===== */
        .user-actions-box {
            background:#fff;
            border-top:1px solid #edf0f4;
            padding:7px 10px;
            overflow:visible !important;
        }

        .user-actions-title {
            display:flex;
            align-items:center;
            gap:6px;
            flex-wrap:wrap;
        }

        .required-remark {
            display:inline-flex;
            align-items:center;
            gap:5px;
            white-space:nowrap;
            margin:0;
        }

        .required-remark input {
            width:15px;
            height:15px;
            margin:0;
        }


        /* Small Edit / Delete icon buttons only */
        .icon-btn-sm {
            width:26px !important;
            height:26px !important;
            min-width:26px !important;
            min-height:26px !important;
            padding:0 !important;
            display:inline-flex !important;
            align-items:center !important;
            justify-content:center !important;
            line-height:1 !important;
            border-radius:4px !important;
        }

        .icon-btn-sm .bi {
            font-size:12px !important;
            line-height:1 !important;
            margin:0 !important;
        }

        .delete-btn.icon-btn-sm {
            min-width:26px !important;
            width:26px !important;
        }

    </style>
</head>

<body ng-controller="WorkflowController as vm" ng-cloak>
<form id="form1" runat="server">
<div class="page-wrap container-fluid">

    <div class="d-flex justify-content-between align-items-center flex-wrap gap-2 mb-3">
        <div>
            <h3 class="mb-1">QC Workflow</h3>
           <%-- <div class="text-muted">Manage workflow rules from one landing page.</div>--%>
        </div>
        <div class="top-links">
            <a href="ActionMaster.aspx" class="btn btn-outline-secondary">Action Master</a>
            <%--<button type="button" class="btn btn-primary" ng-click="vm.openNew()">+ Add New</button>--%>
        </div>
    </div>

    <div class="alert alert-danger" ng-if="vm.error">{{vm.error}}</div>
    <div class="alert alert-success" ng-if="vm.success">{{vm.success}}</div>

    <!-- LANDING PAGE -->
    <div class="card-box">
        <div class="card-head">
            <div>
                <h4 class="section-title">Manage Workflows</h4>
                <%--<div class="small-muted mt-1">Add/Edit opens in popup. Active/Inactive can be changed directly from the list.</div>--%>
            </div>
            <div class="toolbar">
                <div class="input-group input-group-sm workflow-grid-search">
                    <span class="input-group-text"><i class="bi bi-search"></i></span>
                    <input type="text"
                           class="form-control"
                           ng-model="vm.gridSearch"
                           placeholder="Search workflow..." />
                    <button type="button"
                            class="btn btn-outline-secondary"
                            ng-if="vm.gridSearch"
                            ng-click="vm.gridSearch=''">
                        &times;
                    </button>
                </div>

                <button type="button" class="btn btn-sm btn-outline-secondary" ng-click="vm.loadWorkflows()">Refresh</button>
                <button type="button" class="btn btn-sm btn-primary" ng-click="vm.openNew()">+ Add New</button>
            </div>
        </div>

        <div class="card-body-custom">
            <div class="table-responsive">
                <table class="table table-hover table-bordered align-middle mb-0">
                    <thead class="table-light">
                        <tr>
                            <th style="width:70px">Sr No.</th>
                            <%--<th style="width:90px">Order No.</th>--%>
                            <th>Workflow</th>
                            <th style="width:130px">Apply On</th>
                            <%--<th style="width:105px">Conditions</th>
                            <th style="width:95px">Actions</th>--%>
                            <th style="width:125px">Active</th>
                            <th style="width:170px">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-if="vm.gridLoading">
                            <td colspan="5" class="text-center py-4">
                                <div class="d-inline-flex align-items-center text-muted">
                                    <div class="spinner-border spinner-border-sm text-primary me-2" role="status" aria-hidden="true"></div>
                                    <span>Loading workflows...</span>
                                </div>
                            </td>
                        </tr>

                        <tr ng-repeat="x in (vm.workflows | filter:vm.gridSearch) track by x.id">
                            <td>{{$index + 1}}</td>
                            <%--<td><strong>{{x.OrderNo}}</strong></td>--%>
                            <td>
                                <strong>{{x.WorkflowName}}</strong>
                                <div class="small-muted" ng-if="x.Description">{{x.Description}}</div>
                            </td>
                            <td>{{vm.levelText(x.ApplyOn)}}</td>
                            <%--<td>{{x.ConditionCount || 0}}</td>
                            <td>{{x.ActionCount || 0}}</td>--%>
                            <td>
                                <div class="form-check form-switch m-0">
                                    <input class="form-check-input" type="checkbox"
                                           ng-model="x.status"
                                           ng-change="vm.changeStatus(x)" />
                                    <%--<span ng-class="vm.toBool(x.status) ? 'status-active' : 'status-inactive'">
                                        {{vm.toBool(x.status) ? 'Active' : 'Inactive'}}
                                    </span>--%>
                                </div>
                            </td>
                            <td class="action-col">
                                <button type="button"
                                        class="btn btn-sm btn-outline-primary me-1 icon-btn-sm"
                                        ng-click="vm.openEdit(x.id)"
                                        ng-disabled="vm.editingId == x.id"
                                        title="Edit">
                                    <span ng-if="vm.editingId == x.id" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                    <i ng-if="vm.editingId != x.id" class="bi bi-pencil-square"></i>
                                </button>
                                <button type="button"
                                        class="btn btn-sm btn-outline-danger icon-btn-sm"
                                        ng-click="vm.deleteWorkflow(x)"
                                        ng-disabled="vm.deletingId == x.id"
                                        title="Delete">
                                    <span ng-if="vm.deletingId == x.id" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                    <i ng-if="vm.deletingId != x.id" class="bi bi-trash"></i>
                                </button>
                            </td>
                        </tr>
                        <tr ng-if="!vm.gridLoading && (vm.workflows | filter:vm.gridSearch).length === 0">
                            <td colspan="5" class="text-center text-muted py-4">
                                {{vm.gridSearch ? 'No matching workflow found.' : 'No workflow found.'}}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<!-- ADD / EDIT WORKFLOW POPUP -->
<div class="modal fade" id="workflowModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header">
                <div>
                    <h5 class="modal-title mb-0">{{vm.workflow.id > 0 ? 'Edit QC Workflow' : 'Add New QC Workflow'}}</h5>
                    <%--<div class="small-muted mt-1">IF is mandatory. ELSE IF and ELSE are optional.</div>--%>
                </div>
                <button type="button" class="btn-close" ng-click="vm.closeModal()"></button>
            </div>

            <div class="modal-body">
                <div class="card-box mb-3">
                    <div class="card-head"><h4 class="section-title">Workflow Header</h4></div>
                    <div class="card-body-custom">
                        <div class="row g-3">
                            <div class="col-md-5">
                                <label class="form-label required">Workflow Name</label>
                                <input type="text" maxlength="250" class="form-control" ng-model="vm.workflow.WorkflowName" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label required">Apply On</label>
                                <select class="form-select" ng-model="vm.workflow.ApplyOn">
                                    <option value="">Select</option>
                                    <option value="PROJECT">Project</option>
                                    <option value="MILESTONE">Milestone</option>
                                    <option value="TASK">Task</option>
                                    <option value="SUBTASK">Sub Task</option>
                                </select>
                            </div>
<div class="col-md-2" style="display:none;">
                                <label class="form-label">Order No.</label>
                                <input type="number" min="1" class="form-control" ng-model="vm.workflow.OrderNo" />
                            </div>
                            <div class="col-md-2">
                                <label class="form-label d-block">Active</label>
                                <div class="form-check form-switch pt-2">
                                    <input class="form-check-input" type="checkbox" id="wfActive" ng-model="vm.workflow.IsActive" />
                                    <label class="form-check-label" for="wfActive">{{vm.workflow.IsActive ? 'Active' : 'Inactive'}}</label>
                                </div>
                            </div>
                            <div class="col-md-12 mt-0">
                                <label class="form-label">Description</label>
                                <input type="text" maxlength="1000" class="form-control" ng-model="vm.workflow.Description" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="d-flex justify-content-between align-items-center mb-2">
                    <h4 class="section-title">Conditions & Actions</h4>
                    <div class="small-muted">
                        IF is required. ELSE IF and ELSE are optional.
                    </div>
                </div>

                <div class="section-box" ng-repeat="s in vm.workflow.Sections track by $index">
                    <div class="section-head">
                        <div class="d-flex align-items-center gap-2 flex-wrap">
                            <strong>Sr No. {{$index + 1}}</strong>

                            <select class="form-select form-select-sm branch-type-select"
                                    style="width:125px"
                                    ng-model="s.Type"
                                    ng-change="vm.branchTypeChanged(s,$index)"
                                    ng-disabled="$index === 0">
                                <option value="IF">IF</option>
                                <option value="ELSEIF">ELSE IF</option>
                                <option value="ELSE">ELSE</option>
                            </select>

                            <div class="d-flex align-items-center gap-2 ms-2"
                                 ng-if="$last && s.Type !== 'ELSE' && !vm.hasElse()">
                                <span class="small-muted">Add</span>
                                <select class="form-select form-select-sm"
                                        style="width:120px"
                                        ng-model="vm.newBranchType">
                                    <option value="ELSEIF">Else If</option>
                                    <option value="ELSE">Else</option>
                                </select>
                                <button type="button"
                                        class="btn btn-sm btn-outline-primary"
                                        ng-click="vm.addBranch(vm.newBranchType)">
                                    Add
                                </button>
                            </div>
                        </div>

                        <button type="button"
                                class="btn btn-sm btn-outline-danger icon-btn-sm"
                                ng-if="$index > 0"
                                ng-click="vm.removeSection($index)">
                             <i class="bi bi-trash"></i>
                        </button>
                    </div>

                    <div class="conditions-box" ng-if="s.Type !== 'ELSE'">
                        <div class="d-flex justify-content-between align-items-center mb-2">
                            <strong>Conditions {{s.Type === 'IF' ? 'IF' : 'ELSE IF'}}</strong>
                            <button type="button" class="btn btn-sm btn-primary" ng-click="vm.addCondition(s)">+ Add Condition</button>
                        </div>

                        <div class="table-responsive qc-condition-table-responsive">
                            <table class="table table-bordered align-middle mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th style="width:70px">Sr No.</th>
                                        <th>Field</th>
                                        <th style="width:150px">Operator</th>
                                        <th>Value</th>
                                        <th style="width:110px">Join</th>
                                        <th style="width:95px">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="c in s.Conditions track by $index">
                                        <td>{{$index + 1}}</td>
                                        <td>
                                            <select class="form-select form-select-sm" ng-model="c.FieldCode" ng-change="vm.fieldChanged(c)">
                                                <option value="">Select Field</option>
                                                <option value="STATUS">Status</option>
                                                <option value="PRIORITY">Priority</option>
                                                <option value="COMPLETION_PERCENT">Completion %</option>
                                                <option value="DURATION_PERCENT">Time Consumed %</option>
                                                <option value="PROGRESS_DIFFERENCE_PERCENT">Progress Difference %</option>
                                            </select>
                                        </td>
                                        <td>
                                            <select class="form-select form-select-sm"
                                                    ng-model="c.OperatorCode"
                                                    ng-change="vm.operatorChanged(c)">
                                                <option value="">Select</option>
                                                <option value="EQ">=</option>
                                                <option value="NE">!=</option>

                                                <!-- Status / Priority -->
                                                <option value="IN" ng-if="vm.isMasterField(c)">IN</option>
                                                <option value="NOT_IN" ng-if="vm.isMasterField(c)">NOT IN</option>

                                                <!-- Numeric fields -->
                                                <option value="GT" ng-if="!vm.isMasterField(c)">&gt;</option>
                                                <option value="GE" ng-if="!vm.isMasterField(c)">&gt;=</option>
                                                <option value="LT" ng-if="!vm.isMasterField(c)">&lt;</option>
                                                <option value="LE" ng-if="!vm.isMasterField(c)">&lt;=</option>
                                                <option value="BETWEEN" ng-if="!vm.isMasterField(c)">BETWEEN</option>
                                            </select>
                                        </td>
                                        <td>
                                            <!-- Status: single value -->
                                            <select class="form-select form-select-sm value-control"
                                                    ng-if="c.FieldCode === 'STATUS' && !vm.isMultiOperator(c)"
                                                    ng-model="c.CompareValue"
                                                    ng-options="(x.id + '') as x.ValueText for x in vm.statusMasters">
                                                <option value="">Select Status</option>
                                            </select>

                                            <!-- Priority: single value -->
                                            <select class="form-select form-select-sm value-control"
                                                    ng-if="c.FieldCode === 'PRIORITY' && !vm.isMultiOperator(c)"
                                                    ng-model="c.CompareValue"
                                                    ng-options="(x.id + '') as x.ValueText for x in vm.priorityMasters">
                                                <option value="">Select Priority</option>
                                            </select>

                                            <!-- Status / Priority: IN / NOT IN
                                                 Same searchable checkbox picker as Critical Workflow -->
                                            <div class="employee-picker value-control"
                                                 ng-if="vm.isMasterField(c) && vm.isMultiOperator(c)">
                                                <div class="employee-picker-control"
                                                     ng-class="{'is-open':c.ValuePickerOpen}"
                                                     ng-click="vm.toggleValuePicker(c,$event)">

                                                    <span class="employee-picker-placeholder"
                                                          ng-if="!c.SelectedValues.length">
                                                        Select values...
                                                    </span>

                                                    <span class="employee-chip"
                                                          ng-repeat="valueId in c.SelectedValues track by $index">
                                                        <span class="employee-chip-name">
                                                            {{vm.masterValueText(c,valueId)}}
                                                        </span>
                                                        <button type="button"
                                                                class="employee-chip-remove"
                                                                ng-click="vm.removeSelectedMasterValue(c,valueId,$event)">
                                                            &times;
                                                        </button>
                                                    </span>

                                                    <span class="employee-picker-arrow">&#9662;</span>
                                                </div>

                                                <div class="employee-picker-popup"
                                                     ng-class="{'drop-up':c.ValueDropUp}"
                                                     ng-if="c.ValuePickerOpen"
                                                     ng-click="$event.stopPropagation()">

                                                    <input type="text"
                                                           class="form-control form-control-sm"
                                                           ng-model="c.ValueSearch"
                                                           ng-click="$event.stopPropagation()"
                                                           placeholder="Search value..." />

                                                    <div class="employee-picker-list">
                                                        <label class="employee-picker-item"
                                                               ng-repeat="opt in (vm.masterValueOptions(c) | filter:{ValueText:c.ValueSearch}) track by opt.id"
                                                               ng-click="vm.toggleMasterValue(c,opt,$event)">
                                                            <input type="checkbox"
                                                                   ng-checked="vm.isMasterValueSelected(c,opt.id)" />
                                                            <span>{{opt.ValueText}}</span>
                                                        </label>

                                                        <div class="employee-picker-empty"
                                                             ng-if="!(vm.masterValueOptions(c) | filter:{ValueText:c.ValueSearch}).length">
                                                            No value found.
                                                        </div>
                                                    </div>

                                                    <div class="employee-picker-footer">
                                                        <span>{{c.SelectedValues.length}} value(s) selected</span>
                                                        <button type="button"
                                                                class="btn btn-primary btn-sm"
                                                                ng-click="vm.closeValuePicker(c,$event)">
                                                            Done
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- Numeric single value -->
                                            <input type="number"
                                                   class="form-control form-control-sm value-control"
                                                   ng-if="!vm.isMasterField(c) && c.OperatorCode !== 'BETWEEN'"
                                                   ng-model="c.CompareValue"
                                                   step="0.01"
                                                   placeholder="Value" />

                                            <!-- Numeric BETWEEN -->
                                            <div class="d-flex align-items-center gap-1"
                                                 ng-if="!vm.isMasterField(c) && c.OperatorCode === 'BETWEEN'">
                                                <input type="number"
                                                       class="form-control form-control-sm"
                                                       ng-model="c.CompareValue"
                                                       step="0.01"
                                                       placeholder="From" />
                                                <span class="small-muted">to</span>
                                                <input type="number"
                                                       class="form-control form-control-sm"
                                                       ng-model="c.CompareValue2"
                                                       step="0.01"
                                                       placeholder="To" />
                                            </div>
                                        </td>
                                        <td>
                                            <select class="form-select form-select-sm" ng-model="c.LogicalOperator" ng-disabled="$last">
                                                <option value="">--</option>
                                                <option value="AND">AND</option>
                                                <option value="OR">OR</option>
                                            </select>
                                        </td>
                                        <td>
                                            <button type="button" class="btn btn-sm btn-outline-danger delete-btn icon-btn-sm" ng-click="vm.removeCondition(s,$index)"> <i class="bi bi-trash"></i></button>
                                        </td>
                                    </tr>
                                    <tr ng-if="!s.Conditions.length">
                                        <td colspan="6" class="text-center text-muted py-3">No condition added.</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="then-box">
                        <div class="d-flex justify-content-between align-items-center mb-2">
                            <strong>THEN Action</strong>
                            <button type="button" class="btn btn-sm btn-primary" ng-click="vm.addAction(s)">+ Add Action</button>
                        </div>

                        <div class="table-responsive workflow-action-table">
                            <table class="table table-bordered align-middle mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th style="width:70px">Sr No.</th>
                                        <th>Take By</th>
                                        <th>Designation</th>
                                        <th style="width:330px">Notification By</th>
                                        <th style="width:95px">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="a in s.Actions track by $index">
                                        <td>{{$index + 1}}</td>
                                        <td>
                                            <select class="form-select form-select-sm"
                                                    chosen-select
                                                    chosen-placeholder="Select Department"
                                                    ng-model="a.TakenById">
                                                <option value="">Select Department</option>
                                                <option ng-repeat="x in vm.departmentMasters"
                                                        value="{{x.id}}">{{x.DepartmentName}}</option>
                                            </select>
                                        </td>

                                        <td>
                                            <select class="form-select form-select-sm"
                                                    chosen-select
                                                    chosen-placeholder="Select Designation"
                                                    ng-model="a.DesignationId">
                                                <option value="">Select Designation</option>
                                                <option ng-repeat="x in vm.designationMasters"
                                                        value="{{x.id}}">{{x.DesignationName}}</option>
                                            </select>
                                        </td>

                                        <td>
                                            <div class="notification-options">
                                                <label class="notification-check">
                                                    <input type="checkbox" ng-model="a.IsEmail" />
                                                    <span>Email</span>
                                                </label>

                                                <label class="notification-check">
                                                    <input type="checkbox" ng-model="a.IsWhatsApp" />
                                                    <span>WhatsApp</span>
                                                </label>

                                                <label class="notification-check" title="Bell Notification">
                                                    <input type="checkbox" ng-model="a.IsBell" />
                                                   
                                                    <span>Bell</span>
                                                </label>
                                            </div>
                                        </td>

                                        <td>
                                            <button type="button"
                                                    class="btn btn-sm btn-outline-danger delete-btn icon-btn-sm"
                                                    ng-click="vm.removeAction(s,$index)">
                                                <i class="bi bi-trash"></i>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr ng-if="!s.Actions.length">
                                        <td colspan="5" class="text-center text-muted py-3">No action added.</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                    <div class="user-actions-box">
                        <div class="d-flex justify-content-between align-items-center mb-1 flex-wrap gap-2">
                            <div class="user-actions-title">
                                <strong>Action Conditions</strong>
                                
                            </div>

                            <!-- First User Action is always IF -->
                            <button type="button"
                                    class="btn btn-sm btn-primary"
                                    ng-if="!s.UserActions.length"
                                    ng-click="vm.addUserAction(s,'IF')">
                                + Add Condition
                            </button>

                        </div>

                        <div class="table-responsive workflow-action-table">
                            <table class="table table-bordered align-middle mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th style="width:30px">S.No.</th>
                                        <th style="width:100px">Type</th>
                                        <th>QC Action</th>
                                        <th>Next Status</th>
                                        <th style="width:125px">Assign To Type</th>
                                        <th>Department</th>
                                        <th>Designation</th>
                                        <th style="width:220px">Notification By</th>
                                        <th style="width:80px">Remark <span class="text-danger">*</span></th>
                                        <th style="width:80px">Action</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <tr ng-repeat="ua in s.UserActions track by $index">
                                        <td>{{$index + 1}}</td>

                                        <td>
                                            <select class="form-select form-select-sm"
                                                    ng-model="ua.ActionType"
                                                    ng-change="vm.userActionTypeChanged(s,ua,$index)"
                                                    ng-disabled="$index === 0">
                                                <option value="IF">IF</option>
                                                <option value="ELSEIF">ELSE IF</option>
                                                <option value="ELSE">ELSE</option>
                                            </select>
                                        </td>

                                        <td>
                                            <select class="form-select form-select-sm"
                                                    chosen-select
                                                    chosen-placeholder="Select Action"
                                                    ng-model="ua.ActionId">
                                                <option value="">Select Action</option>
                                                <option ng-repeat="x in vm.actionMasters"
                                                        value="{{x.id}}">{{x.ActionName}}</option>
                                            </select>
                                        </td>

                                        <td>
                                            <select class="form-select form-select-sm"
                                                    chosen-select
                                                    chosen-placeholder="Select Next Status"
                                                    ng-model="ua.NextStatusId">
                                                <option value="">Select Status</option>
                                                <option ng-repeat="x in vm.statusMasters"
                                                        value="{{x.id}}">{{x.ValueText}}</option>
                                            </select>
                                        </td>

                                        <td>
                                            <select class="form-select form-select-sm"
                                                    ng-model="ua.AssignToType"
                                                    ng-change="vm.userAssignToTypeChanged(ua)">
                                                <option value="DEPARTMENT">Department</option>
                                                <option value="OWNER">Owner</option>
                                                <option value="NO_ASSIGNMENT">No Assign</option>
                                            </select>
                                        </td>

                                        <td>
                                            <div ng-if="ua.AssignToType === 'DEPARTMENT'">
                                                <select class="form-select form-select-sm"
                                                        chosen-select
                                                        chosen-placeholder="Select Department"
                                                        ng-model="ua.TakenById">
                                                    <option value="">Select Department</option>
                                                    <option ng-repeat="x in vm.departmentMasters"
                                                            value="{{x.id}}">{{x.DepartmentName}}</option>
                                                </select>
                                            </div>
                                            <span class="small-muted" ng-if="ua.AssignToType !== 'DEPARTMENT'">--</span>
                                        </td>

                                        <td>
                                            <div ng-if="ua.AssignToType === 'DEPARTMENT'">
                                                <select class="form-select form-select-sm"
                                                        chosen-select
                                                        chosen-placeholder="Select Designation"
                                                        ng-model="ua.DesignationId">
                                                    <option value="">Select Designation</option>
                                                    <option ng-repeat="x in vm.designationMasters"
                                                            value="{{x.id}}">{{x.DesignationName}}</option>
                                                </select>
                                            </div>
                                            <span class="small-muted" ng-if="ua.AssignToType !== 'DEPARTMENT'">--</span>
                                        </td>

                                        <td>
                                            <div class="notification-options">
                                                <label class="notification-check">
                                                    <input type="checkbox" ng-model="ua.IsEmail" />
                                                    <span>Email</span>
                                                </label>

                                                <label class="notification-check">
                                                    <input type="checkbox" ng-model="ua.IsWhatsApp" />
                                                    <span>WhatsApp</span>
                                                </label>

                                                <label class="notification-check">
                                                    <input type="checkbox" ng-model="ua.IsBell" />
                                                    <span>Bell</span>
                                                </label>
                                            </div>
                                        </td>

                                        <td>
                                            <div class="form-check form-switch m-0 d-flex align-items-center gap-2">
                                                <input class="form-check-input"
                                                       type="checkbox"
                                                       ng-model="ua.IsRemarkRequired" />
                                                <%--<span class="small-muted">
                                                    {{ua.IsRemarkRequired ? 'Required' : 'Optional'}}
                                                </span>--%>
                                            </div>
                                        </td>

                                        <td>
                                            <div class="d-flex align-items-center gap-1 flex-nowrap">
                                                <!-- Condition type is managed only from the Type column.
                                                     Add creates the next row as ELSE IF by default;
                                                     user can change that new row to ELSE from its Type dropdown. -->
                                                <button type="button"
                                                        class="btn btn-sm btn-outline-primary"
                                                        ng-if="$last && !vm.hasUserActionElse(s)"
                                                        ng-click="vm.addUserAction(s,'ELSEIF')"
                                                        title="Add next condition">
                                                    +
                                                </button>

                                                <button type="button"
                                                        class="btn btn-sm btn-outline-danger delete-btn icon-btn-sm"
                                                        ng-click="vm.removeUserAction(s,$index)"
                                                        title="Delete condition">
                                                     <i class="bi bi-trash"></i>
                                                </button>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr ng-if="!s.UserActions.length">
                                        <td colspan="10" class="text-center text-muted py-2">
                                            No user action added.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div class="small-muted mt-1">
                            User Action flow is evaluated as <strong>IF / ELSE IF / ELSE</strong>.
                            IF / ELSE IF / ELSE requires a QC Action value.
                            If <strong>Required Remark</strong> is checked, the user must enter a remark when taking that action.
                        </div>
                    </div>
                    </div>
                </div>
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-light" ng-click="vm.closeModal()">Cancel</button>
                <button type="button" class="btn btn-primary" ng-click="vm.saveWorkflow()" ng-disabled="vm.saving">
                    {{vm.saving ? 'Saving...' : (vm.workflow.id > 0 ? 'Update Workflow' : 'Save Workflow')}}
                </button>
            </div>
        </div>
    </div>
</div>
</form>

<script>
(function () {
    'use strict';

    angular.module('workflowApp', [])
        .directive('chosenSelect', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModelCtrl) {
                    var $el = jQuery(element);

                    function initChosen() {
                        if ($el.data('chosen')) {
                            $el.trigger('chosen:updated');
                            return;
                        }

                        $el.chosen({
                            width: '100%',
                            search_contains: true,
                            allow_single_deselect: true,
                            no_results_text: 'No result found',
                            placeholder_text_single: attrs.chosenPlaceholder || 'Select'
                        });

                        $el.on('change.chosenAngular', function () {
                            // IMPORTANT:
                            // For selects using ng-options Angular already converts the
                            // internal DOM value (for example "number:12") back to the
                            // real model value (12). Do not overwrite it with jQuery.val(),
                            // otherwise TakenById / DesignationId become null at save time.
                            if (attrs.ngOptions) {
                                return;
                            }

                            // Plain <option value="{{x.id}}"> selects still use the
                            // original Chosen-to-ngModel bridge.
                            scope.$evalAsync(function () {
                                var value = $el.val();
                                ngModelCtrl.$setViewValue(
                                    value === '' || value === null ? null : value
                                );
                            });
                        });

                        $el.on('chosen:showing_dropdown.chosenAngular', function () {
                            var $container = $el.next('.chosen-container');
                            var rect = $container[0].getBoundingClientRect();
                            var modalBody = $container.closest('.modal-body')[0];
                            var availableBottom = modalBody
                                ? modalBody.getBoundingClientRect().bottom - rect.bottom
                                : window.innerHeight - rect.bottom;

                            // Chosen results normally need around 220-260px.
                            if (availableBottom < 250) {
                                $container.addClass('chosen-drop-up');
                            } else {
                                $container.removeClass('chosen-drop-up');
                            }
                        });

                        $el.on('chosen:hiding_dropdown.chosenAngular', function () {
                            $el.next('.chosen-container').removeClass('chosen-drop-up');
                        });
                    }

                    $timeout(initChosen, 0);

                    scope.$watch(function () {
                        return element[0].options.length;
                    }, function () {
                        $timeout(initChosen, 0);
                    });

                    scope.$watch(attrs.ngModel, function () {
                        $timeout(function () {
                            if ($el.data('chosen')) {
                                $el.trigger('chosen:updated');
                            }
                        }, 0);
                    });

                    scope.$on('$destroy', function () {
                        $el.off('.chosenAngular');
                        if ($el.data('chosen')) {
                            $el.chosen('destroy');
                        }
                    });
                }
            };
        }])
        .directive('chosenMulti', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModelCtrl) {
                    var $el = jQuery(element);

                    function initChosenMulti() {
                        if (!$el.data('chosen')) {
                            $el.chosen({
                                width: '100%',
                                search_contains: true,
                                no_results_text: 'No result found',
                                placeholder_text_multiple: attrs.chosenPlaceholder || 'Select values'
                            });

                            $el.on('change.chosenMultiAngular', function () {
                                scope.$evalAsync(function () {
                                    ngModelCtrl.$setViewValue($el.val() || []);
                                });
                            });

                            $el.on('chosen:showing_dropdown.chosenMultiAngular', function () {
                                var $container = $el.next('.chosen-container');
                                var rect = $container[0].getBoundingClientRect();
                                var modalBody = $container.closest('.modal-body')[0];
                                var availableBottom = modalBody
                                    ? modalBody.getBoundingClientRect().bottom - rect.bottom
                                    : window.innerHeight - rect.bottom;

                                if (availableBottom < 250) {
                                    $container.addClass('chosen-drop-up');
                                } else {
                                    $container.removeClass('chosen-drop-up');
                                }
                            });

                            $el.on('chosen:hiding_dropdown.chosenMultiAngular', function () {
                                $el.next('.chosen-container').removeClass('chosen-drop-up');
                            });
                        }

                        $el.trigger('chosen:updated');
                    }

                    $timeout(initChosenMulti, 0);

                    scope.$watch(function () {
                        return element[0].options.length;
                    }, function () {
                        $timeout(initChosenMulti, 0);
                    });

                    scope.$watchCollection(attrs.ngModel, function () {
                        $timeout(function () {
                            if ($el.data('chosen')) {
                                $el.trigger('chosen:updated');
                            }
                        }, 0);
                    });

                    scope.$on('$destroy', function () {
                        $el.off('.chosenMultiAngular');
                        if ($el.data('chosen')) {
                            $el.chosen('destroy');
                        }
                    });
                }
            };
        }])
        .controller('WorkflowController', WorkflowController);

    WorkflowController.$inject = ['$http', '$timeout'];

    function WorkflowController($http, $timeout) {
        var vm = this;
        var modal = null;

        // Performance: workflow list is the landing-page data.
        // Heavy dropdown masters are loaded only when Add/Edit is opened,
        // and only once per page session.
        var mastersLoaded = false;
        var mastersLoading = null;
        var workflowsLoading = null;

        vm.workflows = [];
        vm.gridSearch = '';
        // Show loader until the landing-page workflow list is received.
        vm.gridLoading = true;

        vm.actionMasters = [];
        vm.statusMasters = [];
        vm.priorityMasters = [];
        vm.departmentMasters = [];
        vm.designationMasters = [];
        vm.workflow = newWorkflow();
        vm.error = '';
        vm.success = '';
        vm.saving = false;
        // Row action loaders: show spinner only on the clicked Edit/Delete button.
        vm.editingId = null;
        vm.deletingId = null;

        vm.loadMasters = loadMasters;
        vm.loadWorkflows = loadWorkflows;
        vm.openNew = openNew;
        vm.openEdit = openEdit;
        vm.closeModal = closeModal;
        vm.newBranchType = 'ELSEIF';
        vm.addBranch = addBranch;
        vm.branchTypeChanged = branchTypeChanged;
        vm.hasElse = hasElse;
        vm.removeSection = removeSection;
        vm.addCondition = addCondition;
        vm.removeCondition = removeCondition;
        vm.addAction = addAction;
        vm.removeAction = removeAction;
        vm.addUserAction = addUserAction;
        vm.removeUserAction = removeUserAction;
        vm.hasUserActionElse = hasUserActionElse;
        vm.userActionTypeChanged = userActionTypeChanged;
        vm.userAssignToTypeChanged = userAssignToTypeChanged;
        vm.fieldChanged = fieldChanged;
        vm.operatorChanged = operatorChanged;
        vm.isMasterField = isMasterField;
        vm.isMultiOperator = isMultiOperator;
        vm.masterValueOptions = masterValueOptions;
        vm.toggleValuePicker = toggleValuePicker;
        vm.closeValuePicker = closeValuePicker;
        vm.toggleMasterValue = toggleMasterValue;
        vm.isMasterValueSelected = isMasterValueSelected;
        vm.removeSelectedMasterValue = removeSelectedMasterValue;
        vm.masterValueText = masterValueText;
        vm.noValue = noValue;
        vm.saveWorkflow = saveWorkflow;
        vm.deleteWorkflow = deleteWorkflow;
        vm.changeStatus = changeStatus;
        vm.toBool = toBool;
        vm.levelText = levelText;

        angular.element(document).ready(function () {
            modal = new bootstrap.Modal(document.getElementById('workflowModal'), { backdrop:'static', keyboard:false });
        });

        // Load only the workflow grid on initial page load.
        // Masters are not needed until the popup is opened.
        loadWorkflows();

        // Close only the IN/NOT IN value picker when click happens outside.
        angular.element(document)
            .off('click.qcWorkflowValuePicker')
            .on('click.qcWorkflowValuePicker', function () {
                if (closeOtherValuePickers(null)) {
                    $timeout(function () { }, 0);
                }
            });

        function pagePost(method, payload) {
            return $http({
                method:'POST',
                url:'WorkflowSetting.aspx/' + method,
                data:payload || {},
                headers:{ 'Content-Type':'application/json; charset=utf-8' }
            }).then(function (response) {
                var raw = response.data ? response.data.d : null;
                return raw ? angular.fromJson(raw) : {};
            });
        }

        function blankCondition() {
            return {
                FieldCode:'',
                OperatorCode:'',
                CompareValue:'',
                CompareValue2:'',
                SelectedValues:[],
                ValuePickerOpen:false,
                ValueSearch:'',
                ValueDropUp:false,
                LogicalOperator:''
            };
        }

        function blankAction() {
            return { TakenById:null, DesignationId:null, IsEmail:false, IsWhatsApp:false, IsBell:false };
        }

        function blankUserAction(actionType) {
            return {
                ActionType:actionType || 'IF',
                ActionId:null,
                NextStatusId:null,
                AssignToType:'NO_ASSIGNMENT',
                TakenById:null,
                DesignationId:null,
                IsEmail:false,
                IsWhatsApp:false,
                IsBell:false,
                IsRemarkRequired:false
            };
        }

        function newSection(type) {
            return {
                Type:type,
                Conditions:type === 'ELSE' ? [] : [blankCondition()],
                Actions:[blankAction()],
                UserActions:[],
                NewUserActionType:'ELSEIF'
            };
        }

        function newWorkflow() {
            return {
                id:0,
                WorkflowName:'',
                ApplyOn:'',
                OrderNo:1,
                IsActive:true,
                Description:'',
                Sections:[newSection('IF')]
            };
        }

        function showSuccess(msg) {
            vm.success = msg || 'Saved successfully.';
            vm.error = '';
            $timeout(function(){ vm.success=''; },3500);
        }

        function showError(msg) {
            vm.error = msg || 'Something went wrong.';
            vm.success = '';
            $timeout(function(){ vm.error=''; },5500);
        }

        function normalizeValueMasters(items) {
            var out = [];

            angular.forEach(items || [], function (x) {
                if (!x) return;

                var rawId = null;

                if (x.id !== null && typeof x.id !== 'undefined') rawId = x.id;
                else if (x.ValueId !== null && typeof x.ValueId !== 'undefined') rawId = x.ValueId;
                else if (x.valueId !== null && typeof x.valueId !== 'undefined') rawId = x.valueId;
                else if (x.ID !== null && typeof x.ID !== 'undefined') rawId = x.ID;

                if (rawId === null || typeof rawId === 'undefined') return;

                var textValue =
                    x.ValueText ||
                    x.ProjectStatus ||
                    x.name ||
                    x.Name ||
                    '';

                out.push({
                    id: rawId,
                    ValueId: rawId,
                    ValueText: textValue
                });
            });

            return out;
        }

        function loadMasters(forceReload) {
            if (mastersLoaded && !forceReload) {
                return mastersLoading;
            }

            if (mastersLoading && !forceReload) {
                return mastersLoading;
            }

            mastersLoading = pagePost('GetMasters', {}).then(function(res){
                if (!res.success) {
                    mastersLoaded = false;
                    showError(res.message);
                    return res;
                }

                vm.actionMasters = res.actions || [];

                // Accept both DB result shapes:
                // Workflow master:  { id, ValueText }
                // Critical master:  { ValueId, ValueText }
                // This keeps existing single-value dropdowns and the new IN/NOT IN picker compatible.
                vm.statusMasters = normalizeValueMasters(res.statuses || []);
                vm.priorityMasters = normalizeValueMasters(res.priorities || []);

                vm.departmentMasters = res.departments || [];
                vm.designationMasters = res.designations || [];
                mastersLoaded = true;
                return res;
            }).catch(function(err){
                mastersLoaded = false;
                showError(httpMessage(err));
                throw err;
            }).finally(function(){
                mastersLoading = null;
            });

            return mastersLoading;
        }

        function loadWorkflows() {
            // Prevent duplicate list requests from double-click/refresh while one is already running.
            if (workflowsLoading) return workflowsLoading;

            vm.gridLoading = true;

            workflowsLoading = pagePost('GetWorkflows', {}).then(function(res){
                if (!res.success) { showError(res.message); return res; }
                vm.workflows = res.data || [];
                angular.forEach(vm.workflows,function(x){ x.status = toBool(x.status); });
                return res;
            }).catch(function(err){
                showError(httpMessage(err));
                throw err;
            }).finally(function(){
                vm.gridLoading = false;
                workflowsLoading = null;
            });

            return workflowsLoading;
        }

        function ensureModal() {
            if (!modal) modal = new bootstrap.Modal(document.getElementById('workflowModal'), { backdrop:'static', keyboard:false });
        }

        function openNew() {
            vm.workflow = newWorkflow();
            vm.newBranchType = 'ELSEIF';
            ensureModal();
            modal.show();

            // Masters are required only inside the popup. Load once, lazily.
            loadMasters(false);
        }

        function closeModal() {
            if (modal) modal.hide();
            vm.workflow = newWorkflow();
            vm.newBranchType = 'ELSEIF';
        }

        function addBranch(type) {
            type = type || 'ELSEIF';

            if (type !== 'ELSEIF' && type !== 'ELSE') {
                type = 'ELSEIF';
            }

            if (hasElse()) {
                showError('ELSE already exists. No condition can be added after ELSE.');
                return;
            }

            vm.workflow.Sections.push(newSection(type));
            vm.newBranchType = 'ELSEIF';
        }

        function branchTypeChanged(section, index) {
            if (!section) return;

            if (index === 0) {
                section.Type = 'IF';
                return;
            }

            if (section.Type === 'IF') {
                section.Type = 'ELSEIF';
            }

            if (section.Type === 'ELSE') {
                if (index !== vm.workflow.Sections.length - 1) {
                    section.Type = 'ELSEIF';
                    showError('ELSE must be the last condition part.');
                    return;
                }

                for (var i = 0; i < vm.workflow.Sections.length; i++) {
                    if (i !== index && vm.workflow.Sections[i].Type === 'ELSE') {
                        section.Type = 'ELSEIF';
                        showError('Only one ELSE is allowed.');
                        return;
                    }
                }

                section.Conditions = [];
            } else {
                section.Type = 'ELSEIF';
                if (!section.Conditions || !section.Conditions.length) {
                    section.Conditions = [blankCondition()];
                }
            }
        }

        function hasElse() {
            for (var i=0;i<vm.workflow.Sections.length;i++) {
                if (vm.workflow.Sections[i].Type === 'ELSE') return true;
            }
            return false;
        }

        function removeSection(index) {
            if (index <= 0) return;
            vm.workflow.Sections.splice(index,1);
        }

        function addCondition(section) {
            section.Conditions.push(blankCondition());
        }

        function removeCondition(section,index) {
            section.Conditions.splice(index,1);
        }

        function addAction(section) {
            section.Actions.push(blankAction());
        }

        function removeAction(section,index) {
            section.Actions.splice(index,1);
        }

        function addUserAction(section,type) {
            if (!section.UserActions) section.UserActions = [];

            // First nested user-action branch must always be IF.
            if (!section.UserActions.length) {
                section.UserActions.push(blankUserAction('IF'));
                section.NewUserActionType = 'ELSEIF';
                return;
            }

            if (hasUserActionElse(section)) {
                showError('User Actions already contains ELSE. No action can be added after ELSE.');
                return;
            }

            type = type || section.NewUserActionType || 'ELSEIF';

            if (type !== 'ELSEIF' && type !== 'ELSE') {
                type = 'ELSEIF';
            }

            section.UserActions.push(blankUserAction(type));
            section.NewUserActionType = 'ELSEIF';
        }

        function removeUserAction(section,index) {
            if (!section.UserActions) return;

            section.UserActions.splice(index,1);

            // If first row remains, it must always be IF.
            if (section.UserActions.length) {
                section.UserActions[0].ActionType = 'IF';
            }

            section.NewUserActionType = 'ELSEIF';
        }

        function hasUserActionElse(section) {
            if (!section || !section.UserActions) return false;

            for (var i=0; i<section.UserActions.length; i++) {
                if (section.UserActions[i].ActionType === 'ELSE') {
                    return true;
                }
            }

            return false;
        }

        function userActionTypeChanged(section,ua,index) {
            if (!section || !ua) return;

            if (index === 0) {
                ua.ActionType = 'IF';
                return;
            }

            if (ua.ActionType === 'IF') {
                ua.ActionType = 'ELSEIF';
            }

            if (ua.ActionType === 'ELSE') {
                // ELSE must be last.
                if (index !== section.UserActions.length - 1) {
                    ua.ActionType = 'ELSEIF';
                    showError('User Action ELSE must be the last item.');
                    return;
                }

                // Only one ELSE.
                for (var i=0; i<section.UserActions.length; i++) {
                    if (i !== index &&
                        section.UserActions[i].ActionType === 'ELSE') {
                        ua.ActionType = 'ELSEIF';
                        showError('Only one User Action ELSE is allowed.');
                        return;
                    }
                }

            }
        }

        function userAssignToTypeChanged(ua) {
            if (!ua) return;

            var assignType = String(ua.AssignToType || 'NO_ASSIGNMENT').toUpperCase();
            if (assignType !== 'DEPARTMENT' &&
                assignType !== 'OWNER' &&
                assignType !== 'NO_ASSIGNMENT') {
                assignType = 'NO_ASSIGNMENT';
            }

            ua.AssignToType = assignType;

            // Department / Designation IDs belong only to Department assignment.
            // Clearing them here prevents an old department value being saved
            // when the user changes the assignment type to Owner / No Assignment.
            if (assignType !== 'DEPARTMENT') {
                ua.TakenById = null;
                ua.DesignationId = null;
            }
        }

        function fieldChanged(c) {
            c.CompareValue = '';
            c.CompareValue2 = '';
            c.SelectedValues = [];
            c.OperatorCode = '';
            c.ValuePickerOpen = false;
            c.ValueSearch = '';
            c.ValueDropUp = false;
        }

        function operatorChanged(c) {
            c.CompareValue2 = '';
            c.ValuePickerOpen = false;
            c.ValueSearch = '';
            c.ValueDropUp = false;

            if (isMultiOperator(c)) {
                if ((!c.SelectedValues || !c.SelectedValues.length) && c.CompareValue) {
                    c.SelectedValues = String(c.CompareValue)
                        .split(',')
                        .filter(function (x) { return String(x).trim() !== ''; });
                }
                c.CompareValue = '';
            } else {
                if (c.SelectedValues && c.SelectedValues.length && !c.CompareValue) {
                    c.CompareValue = String(c.SelectedValues[0]);
                }
                c.SelectedValues = [];
            }
        }

        function isMasterField(c) {
            return c && (c.FieldCode === 'STATUS' || c.FieldCode === 'PRIORITY');
        }

        function isMultiOperator(c) {
            return c && (c.OperatorCode === 'IN' || c.OperatorCode === 'NOT_IN');
        }

        function masterValueOptions(c) {
            if (!c) return [];
            if (c.FieldCode === 'STATUS') return vm.statusMasters || [];
            if (c.FieldCode === 'PRIORITY') return vm.priorityMasters || [];
            return [];
        }

        function closeOtherValuePickers(currentCondition) {
            var changed = false;

            angular.forEach((vm.workflow && vm.workflow.Sections) || [], function (section) {
                angular.forEach(section.Conditions || [], function (condition) {
                    if (condition !== currentCondition && condition.ValuePickerOpen) {
                        condition.ValuePickerOpen = false;
                        condition.ValueSearch = '';
                        condition.ValueDropUp = false;
                        changed = true;
                    }
                });
            });

            return changed;
        }

        function toggleValuePicker(c, $event) {
            if ($event) $event.stopPropagation();

            closeOtherValuePickers(c);

            var opening = !c.ValuePickerOpen;
            c.ValuePickerOpen = opening;

            if (!opening) {
                c.ValueSearch = '';
                c.ValueDropUp = false;
                return;
            }

            // Keep this picker opening downward.
            // Auto drop-up inside the responsive table caused the popup
            // to be clipped, leaving only the footer visible.
            c.ValueDropUp = false;
        }

        function closeValuePicker(c, $event) {
            if ($event) $event.stopPropagation();
            if (!c) return;

            c.ValuePickerOpen = false;
            c.ValueSearch = '';
            c.ValueDropUp = false;
        }

        function isMasterValueSelected(c, valueId) {
            var id = String(valueId);
            var selected = c && c.SelectedValues ? c.SelectedValues : [];

            for (var i = 0; i < selected.length; i++) {
                if (String(selected[i]) === id) return true;
            }

            return false;
        }

        function toggleMasterValue(c, opt, $event) {
            if ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }

            if (!c.SelectedValues) c.SelectedValues = [];

            var id = String(opt.id);
            var index = -1;

            for (var i = 0; i < c.SelectedValues.length; i++) {
                if (String(c.SelectedValues[i]) === id) {
                    index = i;
                    break;
                }
            }

            if (index >= 0) c.SelectedValues.splice(index, 1);
            else c.SelectedValues.push(id);
        }

        function removeSelectedMasterValue(c, valueId, $event) {
            if ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }

            if (!c.SelectedValues) return;

            var id = String(valueId);

            for (var i = c.SelectedValues.length - 1; i >= 0; i--) {
                if (String(c.SelectedValues[i]) === id) {
                    c.SelectedValues.splice(i, 1);
                }
            }
        }

        function masterValueText(c, valueId) {
            var id = String(valueId);
            var options = masterValueOptions(c);

            for (var i = 0; i < options.length; i++) {
                if (String(options[i].id) === id) {
                    return options[i].ValueText || id;
                }
            }

            return id;
        }

        function noValue(c) {
            return false;
        }

        function validate() {
            if (!String(vm.workflow.WorkflowName || '').trim()) return 'Workflow Name is required.';
            if (!vm.workflow.ApplyOn) return 'Apply On is required.';
            if (!vm.workflow.OrderNo || parseInt(vm.workflow.OrderNo) < 1) return 'Order No. must be 1 or greater.';
            if (!vm.workflow.Sections || !vm.workflow.Sections.length || vm.workflow.Sections[0].Type !== 'IF') return 'IF is required.';

            var elseFound = false;
            for (var i=0;i<vm.workflow.Sections.length;i++) {
                var s = vm.workflow.Sections[i];
                if (s.Type === 'ELSE') {
                    if (elseFound || i !== vm.workflow.Sections.length-1) return 'ELSE must be the last item.';
                    elseFound = true;
                }
                if (s.Type !== 'ELSE') {
                    if (!s.Conditions || !s.Conditions.length) return 'Sr No. ' + (i+1) + ': At least one condition is required.';
                    for (var j=0;j<s.Conditions.length;j++) {
                        var c=s.Conditions[j];
                        if (!c.FieldCode) return 'Sr No. ' + (i+1) + ', Condition ' + (j+1) + ': Field is required.';
                        if (!c.OperatorCode)
                            return 'Sr No. ' + (i+1) + ', Condition ' + (j+1) + ': Operator is required.';

                        if (isMultiOperator(c)) {
                            if (!c.SelectedValues || !c.SelectedValues.length)
                                return 'Sr No. ' + (i+1) + ', Condition ' + (j+1) + ': Select at least one value.';
                        } else {
                            if (c.CompareValue === '' || c.CompareValue === null || typeof c.CompareValue === 'undefined')
                                return 'Sr No. ' + (i+1) + ', Condition ' + (j+1) + ': Value is required.';

                            if (c.OperatorCode === 'BETWEEN' &&
                                (c.CompareValue2 === '' || c.CompareValue2 === null || typeof c.CompareValue2 === 'undefined'))
                                return 'Sr No. ' + (i+1) + ', Condition ' + (j+1) + ': Second value is required for BETWEEN.';
                        }
                    }
                }
                if (!s.Actions || !s.Actions.length) return 'Sr No. ' + (i+1) + ': At least one action is required.';
                for (var k=0;k<s.Actions.length;k++) {
                    var act = s.Actions[k];
                    if (!act.TakenById && !act.DesignationId &&
                        !act.IsEmail && !act.IsWhatsApp && !act.IsBell) {
                        return 'Sr No. ' + (i+1) + ', Action ' + (k+1) +
                               ': Select Department, Designation or Notification By.';
                    }
                }

                var userActionElseFound = false;

                for (var u=0; u<(s.UserActions || []).length; u++) {
                    var ua = s.UserActions[u];

                    if (u === 0 && ua.ActionType !== 'IF') {
                        return 'Sr No. ' + (i+1) + ': First User Action must be IF.';
                    }

                    if (ua.ActionType !== 'IF' &&
                        ua.ActionType !== 'ELSEIF' &&
                        ua.ActionType !== 'ELSE') {
                        return 'Sr No. ' + (i+1) + ', User Action ' + (u+1) +
                               ': Invalid IF / ELSE IF / ELSE type.';
                    }

                    if (ua.ActionType === 'ELSE') {
                        if (userActionElseFound ||
                            u !== s.UserActions.length - 1) {
                            return 'Sr No. ' + (i+1) +
                                   ': User Action ELSE must be the last item.';
                        }

                        userActionElseFound = true;
                    }

                    if (!ua.ActionId) {
                        return 'Sr No. ' + (i+1) + ', User Action ' + (u+1) +
                               ': QC Action is required.';
                    }
                }
            }
            return '';
        }

        function toNullableInt(value) {
            if (value === null || value === '' || typeof value === 'undefined') return null;
            var n = parseInt(value, 10);
            return isNaN(n) ? null : n;
        }

        function buildSaveModel() {
            var model = {
                id:parseInt(vm.workflow.id || 0),
                WorkflowName:vm.workflow.WorkflowName,
                ApplyOn:vm.workflow.ApplyOn,
                OrderNo:parseInt(vm.workflow.OrderNo || 1),
                IsActive:!!vm.workflow.IsActive,
                Description:vm.workflow.Description || '',
                Conditions:[],
                Actions:[],
                UserActions:[]
            };

            angular.forEach(vm.workflow.Sections,function(s,sectionIndex){
                var srNo = sectionIndex + 1;
                angular.forEach(s.Conditions || [],function(c,index){
                    model.Conditions.push({
                        SrNo:srNo,
                        Type:s.Type,
                        FieldCode:c.FieldCode || '',
                        OperatorCode:c.OperatorCode || '',
                        CompareValue:isMultiOperator(c)
                            ? (c.SelectedValues || []).join(',')
                            : String(c.CompareValue == null ? '' : c.CompareValue),
                        CompareValue2:c.OperatorCode === 'BETWEEN'
                            ? String(c.CompareValue2 == null ? '' : c.CompareValue2)
                            : '',
                        LogicalOperator:index < s.Conditions.length-1 ? (c.LogicalOperator || 'AND') : ''
                    });
                });
                angular.forEach(s.Actions || [],function(a){
                    model.Actions.push({
                        SrNo:srNo,
                        Type:s.Type,
                        TakenById:toNullableInt(a.TakenById),
                        DesignationId:toNullableInt(a.DesignationId),
                        IsEmail:!!a.IsEmail,
                        IsWhatsApp:!!a.IsWhatsApp,
                        IsBell:!!a.IsBell
                    });
                });

                angular.forEach(s.UserActions || [],function(ua){
                    // Action Conditions use plain option values so Chosen returns the real master ID.
                    model.UserActions.push({
                        SrNo:srNo,
                        Type:s.Type,
                        ActionType:ua.ActionType || 'IF',
                        ActionId:toNullableInt(ua.ActionId),
                        NextStatusId:toNullableInt(ua.NextStatusId),
                        AssignToType:ua.AssignToType || 'NO_ASSIGNMENT',
                        TakenById:(ua.AssignToType === 'DEPARTMENT')
                            ? toNullableInt(ua.TakenById)
                            : null,
                        DesignationId:(ua.AssignToType === 'DEPARTMENT')
                            ? toNullableInt(ua.DesignationId)
                            : null,
                        IsEmail:!!ua.IsEmail,
                        IsWhatsApp:!!ua.IsWhatsApp,
                        IsBell:!!ua.IsBell,
                        IsRemarkRequired:!!ua.IsRemarkRequired
                    });
                });
            });
            return model;
        }

        function saveWorkflow() {
            if (vm.saving) return;
            var msg = validate();
            if (msg) { showError(msg); return; }
            debugger
            var saveModel = buildSaveModel();

            // Preserve UserActions in saveModel and send them separately as JSON.
            // Remove UserActions only from the COPY sent as WorkflowModel.
            // This prevents WebForms from trying to bind nested nullable values.
            var userActionsJson = angular.toJson(saveModel.UserActions || []);
            var workflowModel = angular.copy(saveModel);
            delete workflowModel.UserActions;

            vm.saving = true;
            pagePost('SaveWorkflow',{
                model: workflowModel,
                userActionsJson: userActionsJson
            }).then(function (res) {
                debugger
                vm.saving = false;
                if (!res.success) { showError(res.message); return; }
                var text = vm.workflow.id > 0 ? 'Workflow updated successfully.' : 'Workflow saved successfully.';
                closeModal();
                showSuccess(text);
                loadWorkflows();
                }).catch(function (err) {
                    debugger
                    vm.saving = false; showError(httpMessage(err));
                });
        }

        function openEdit(id) {
            id = parseInt(id);
            if (!id || vm.editingId === id) return;

            vm.editingId = id;

            // Start master loading in parallel with workflow detail loading.
            // This keeps Edit fast without delaying the landing-page grid.
            loadMasters(false);

            pagePost('GetWorkflow',{ workflowId:id }).then(function(res){
                if (!res.success) { showError(res.message); return; }
                var h = res.workflow && res.workflow.length ? res.workflow[0] : null;
                if (!h) { showError('Workflow not found.'); return; }

                var sections = {};
                angular.forEach(res.conditions || [],function(c){
                    var n=parseInt(c.SrNo || 1);
                    if (!sections[n]) sections[n]={ Type:c.Type || 'IF', Conditions:[], Actions:[], UserActions:[] };
                    var op = c.OperatorCode || '';
                    var fieldCode = c.FieldCode || '';
                    var compareValue = c.CompareValue == null ? '' : String(c.CompareValue);
                    var compareValue2 = c.CompareValue2 == null ? '' : String(c.CompareValue2);
                    var isMaster = fieldCode === 'STATUS' || fieldCode === 'PRIORITY';
                    var isMulti = op === 'IN' || op === 'NOT_IN';

                    sections[n].Conditions.push({
                        FieldCode:fieldCode,
                        OperatorCode:op,
                        CompareValue:isMulti
                            ? ''
                            : (isMaster
                                ? compareValue
                                : (compareValue === '' ? null : parseFloat(compareValue))),
                        CompareValue2:op === 'BETWEEN'
                            ? (compareValue2 === '' ? null : parseFloat(compareValue2))
                            : '',
                        SelectedValues:isMulti && compareValue
                            ? compareValue.split(',').filter(function (x) {
                                return String(x).trim() !== '';
                              })
                            : [],
                        ValuePickerOpen:false,
                        ValueSearch:'',
                        ValueDropUp:false,
                        LogicalOperator:c.LogicalOperator || ''
                    });
                });
                angular.forEach(res.actions || [],function(a){
                    var n=parseInt(a.SrNo || 1);
                    if (!sections[n]) sections[n]={ Type:a.Type || 'IF', Conditions:[], Actions:[], UserActions:[] };
                    sections[n].Actions.push({
                        // THEN Action uses plain option values with Chosen.
                        // Keep saved IDs as strings so Edit selects the correct option.
                        TakenById:(a.TakenById === null || typeof a.TakenById === 'undefined') ? null : String(a.TakenById),
                        DesignationId:(a.DesignationId === null || typeof a.DesignationId === 'undefined') ? null : String(a.DesignationId),
                        IsEmail:toBool(a.IsEmail),
                        IsWhatsApp:toBool(a.IsWhatsApp),
                        IsBell:toBool(a.IsBell)
                    });
                });

                angular.forEach(res.userActions || [],function(ua){
                    var n=parseInt(ua.SrNo || 1);
                    if (!sections[n]) {
                        sections[n]={
                            Type:ua.Type || 'IF',
                            Conditions:[],
                            Actions:[],
                            UserActions:[]
                        };
                    }

                    if (!sections[n].UserActions) sections[n].UserActions=[];

                    var assignToType = String(ua.AssignToType || '').toUpperCase();
                    if (assignToType !== 'DEPARTMENT' &&
                        assignToType !== 'OWNER' &&
                        assignToType !== 'NO_ASSIGNMENT') {
                        // Backward compatibility for rows saved before AssignToType existed.
                        assignToType = (ua.TakenById !== null && typeof ua.TakenById !== 'undefined') ||
                                       (ua.DesignationId !== null && typeof ua.DesignationId !== 'undefined')
                            ? 'DEPARTMENT'
                            : 'NO_ASSIGNMENT';
                    }

                    sections[n].UserActions.push({
                        ActionType:ua.ActionType || 'IF',
                        ActionId:(ua.ActionId === null || typeof ua.ActionId === 'undefined') ? null : String(ua.ActionId),
                        NextStatusId:(ua.NextStatusId === null || typeof ua.NextStatusId === 'undefined') ? null : String(ua.NextStatusId),
                        AssignToType:assignToType,
                        TakenById:assignToType === 'DEPARTMENT' && ua.TakenById !== null && typeof ua.TakenById !== 'undefined'
                            ? String(ua.TakenById) : null,
                        DesignationId:assignToType === 'DEPARTMENT' && ua.DesignationId !== null && typeof ua.DesignationId !== 'undefined'
                            ? String(ua.DesignationId) : null,
                        IsEmail:toBool(ua.IsEmail),
                        IsWhatsApp:toBool(ua.IsWhatsApp),
                        IsBell:toBool(ua.IsBell),
                        IsRemarkRequired:toBool(ua.IsRemarkRequired)
                    });
                });

                var list=[];
                Object.keys(sections).sort(function(a,b){ return parseInt(a)-parseInt(b); }).forEach(function(k){ list.push(sections[k]); });
                if (!list.length) list=[newSection('IF')];
                if (list[0].Type !== 'IF') list[0].Type='IF';
                angular.forEach(list,function(s){
                    if (!s.Actions) s.Actions=[];
                    if (!s.Actions.length) s.Actions=[blankAction()];
                    if (!s.UserActions) s.UserActions=[];

                    // Backward compatibility:
                    // old saved User Actions did not have nested IF/ELSE type.
                    angular.forEach(s.UserActions,function(ua,index){
                        if (!ua.ActionType) {
                            ua.ActionType = index === 0 ? 'IF' : 'ELSEIF';
                        }

                        if (!ua.AssignToType) {
                            ua.AssignToType = (ua.TakenById || ua.DesignationId)
                                ? 'DEPARTMENT'
                                : 'NO_ASSIGNMENT';
                        }
                    });

                    if (s.UserActions.length) {
                        s.UserActions[0].ActionType = 'IF';
                    }

                    s.NewUserActionType = 'ELSEIF';

                    if (s.Type !== 'ELSE' && !s.Conditions.length) s.Conditions=[blankCondition()];
                });

                vm.workflow={
                    id:parseInt(h.id || 0), WorkflowName:h.WorkflowName || '', ApplyOn:h.ApplyOn || '',
OrderNo:parseInt(h.OrderNo || 1), IsActive:toBool(h.status),
                    Description:h.Description || '', Sections:list
                };
                ensureModal();
                modal.show();
            }).catch(function(err){
                showError(httpMessage(err));
            }).finally(function(){
                vm.editingId = null;
            });
        }

        function deleteWorkflow(x) {
            if (!x || !x.id) return;
            var id = parseInt(x.id);
            if (!id || vm.deletingId === id) return;
            if (!window.confirm('Delete "' + (x.WorkflowName || 'this workflow') + '"?')) return;

            vm.deletingId = id;

            pagePost('DeleteWorkflow',{ workflowId:id }).then(function(res){
                if (!res.success) { showError(res.message); return; }
                showSuccess('Workflow deleted successfully.');
                // Keep the delete spinner visible until the refreshed grid is received.
                return loadWorkflows();
            }).catch(function(err){
                showError(httpMessage(err));
            }).finally(function(){
                vm.deletingId = null;
            });
        }

        function changeStatus(x) {
            var newStatus=toBool(x.status);
            pagePost('ChangeStatus',{ workflowId:parseInt(x.id), isActive:newStatus }).then(function(res){
                if (!res.success) { x.status=!newStatus; showError(res.message); return; }
                showSuccess(newStatus ? 'Workflow activated.' : 'Workflow deactivated.');
            }).catch(function(err){ x.status=!newStatus; showError(httpMessage(err)); });
        }

        function toBool(v) {
            return v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true';
        }

        function levelText(v) {
            if (v === 'SUBTASK') return 'Sub Task';
            if (v === 'MILESTONE') return 'Milestone';
            if (v === 'TASK') return 'Task';
            if (v === 'PROJECT') return 'Project';
            return v || '';
        }


        function httpMessage(err) {
            try {
                if (err && err.data) {
                    if (err.data.Message) return err.data.Message;
                    if (err.data.ExceptionMessage) return err.data.ExceptionMessage;
                    if (typeof err.data === 'string') return err.data;
                }
            } catch(e) {}
            return 'Server error.';
        }
    }
})();
</script>
</body>
</html>
