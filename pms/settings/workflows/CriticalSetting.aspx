<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CriticalSetting.aspx.cs" Inherits="CriticalSetting" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" ng-app="criticalApp">
<head runat="server">
    <title>Critical Workflow</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.3/angular.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <style>

        :root {
            --cw-bg: #f4f7fb;
            --cw-card: #ffffff;
            --cw-border: #dde5ef;
            --cw-text: #243447;
            --cw-muted: #6c7a89;
            --cw-if: #0d6efd;
            --cw-elseif: #fd7e14;
            --cw-else: #6f42c1;
            --cw-soft: #f8fbff;
        }

        [ng-cloak] { display:none !important; }

        body {
            background: var(--cw-bg);
            font-family: "Segoe UI", Arial, sans-serif;
            color: var(--cw-text);
            font-size: 12px;
            line-height: 1.42;
        }

        .page-wrap { padding: 20px; }

        .table {
            margin-bottom: 0 !important;
            font-size: 12.45px;
        }

        .table > :not(caption) > * > * {
            padding: .45rem .55rem !important;
            vertical-align: middle;
        }

        .table thead th {
            padding-top: .55rem !important;
            padding-bottom: .55rem !important;
            white-space: nowrap;
            font-size: 12.75px;
            font-weight: 700;
            color: #304255;
            background: #f7f9fc !important;
        }

        .table tbody td {
            padding-top: .42rem !important;
            padding-bottom: .42rem !important;
        }

        .table .form-control,
        .table .form-select,
        .form-control,
        .form-select {
            min-height: 34px !important;
            padding: .3rem .58rem !important;
            font-size: 12.9px !important;
            border-color: #ced7e2;
        }

        textarea.form-control { min-height: auto !important; }

        .btn {
            font-size: 12.6px;
            padding: 5px 11px;
            border-radius: 8px;
        }

        .btn-sm {
            font-size: 11.9px;
            padding: 4px 9px;
        }

        .form-label {
            font-size: 12.45px;
            font-weight: 700;
            color: #435466;
            margin-bottom: .2rem;
        }

        .card-box {
            background: var(--cw-card);
            border: 1px solid var(--cw-border);
            border-radius: 14px;
            box-shadow: 0 6px 22px rgba(36,52,71,.06);
            margin-bottom: 20px;
        }

        .card-head {
            padding: 12px 18px;
            border-bottom: 1px solid #edf1f5;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
            background: linear-gradient(180deg, #ffffff 0%, #fbfcfe 100%);
            border-radius: 14px 14px 0 0;
        }

        .card-body-custom { padding: 12px 14px !important; }
        .section-title { font-size: 17px; font-weight: 700; margin: 0; color: #203245; }
        .small-muted { font-size: 12px; color: var(--cw-muted); }
        .toolbar { display:flex; align-items:center; gap:8px; flex-wrap:wrap; }
        .toolbar .form-select { min-width:180px; }
        .rule-summary { white-space: normal; min-width: 220px; }
        .action-col { white-space: nowrap; }
        .empty-box {
            border: 1px dashed #cfd8e3;
            border-radius: 12px;
            padding: 12px;
            text-align: center;
            color: #6b7785;
            background: #fbfcff;
        }

        .condition-help,
        .workflow-legend {
            background: #f8fbff;
            border: 1px solid #e3ebf5;
            border-radius: 10px;
            padding: 10px 12px;
        }

        .workflow-legend {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            margin-bottom: 14px;
        }

        .legend-pill,
        .branch-pill {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            border-radius: 999px;
            padding: 4px 11px;
            font-size: 12px;
            font-weight: 700;
        }

        .legend-pill.if,
        .branch-pill.if { background: rgba(13,110,253,.10); color: var(--cw-if); }
        .legend-pill.elseif,
        .branch-pill.elseif { background: rgba(253,126,20,.12); color: var(--cw-elseif); }
        .legend-pill.else,
        .branch-pill.else { background: rgba(111,66,193,.12); color: var(--cw-else); }

        .modal-xl { --bs-modal-width: 1360px; }

        #ruleModal .modal-dialog {
            width: calc(100vw - 24px);
            max-width: 1650px !important;
            height: calc(100vh - 24px);
            margin: 12px auto;
        }

        #ruleModal .modal-content {
            height: 100%;
            border-radius: 16px;
            overflow: hidden;
            border: 1px solid #d9e2ee;
            box-shadow: 0 10px 35px rgba(0,0,0,.14);
        }

        #ruleModal .modal-body {
            background: #fbfcfe;
            overflow-y: auto;
            padding: 12px;
        }

        #ruleModal .modal-header,
        #ruleModal .modal-footer {
            position: sticky;
            z-index: 5;
            background: #fff;
        }

        #ruleModal .modal-header {
            top: 0;
            border-bottom: 1px solid #e8edf4;
            padding: 14px 18px;
        }

        #ruleModal .modal-footer {
            bottom: 0;
            border-top: 1px solid #e8edf4;
            padding: 12px 18px;
        }

        .branch-box {
            border: 1px solid #dbe4ef;
            border-left-width: 4px;
            border-radius: 14px;
            background: #fff;
            margin-bottom: 18px;
            overflow: visible;
            box-shadow: 0 4px 14px rgba(28,44,64,.05);
        }

        .branch-box.branch-if { border-left-color: var(--cw-if); }
        .branch-box.branch-elseif { border-left-color: var(--cw-elseif); }
        .branch-box.branch-else { border-left-color: var(--cw-else); }

        .branch-head {
            padding: 12px 14px;
            background: #fbfcfe;
            border-bottom: 1px solid #e8edf4;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
        }

        .branch-body { padding: 6px 8px; }

        .condition-row,
        .action-row {
            border: 1px solid #e4eaf2;
            background: #fff;
            border-radius: 12px;
            padding: 6px 8px;
            margin-bottom: 6px;
            box-shadow: 0 2px 7px rgba(0,0,0,.03);
        }

        .action-row { background: #fcfdff; }
        .if-box {
            font-weight: 700;
            min-width: 58px;
            display: flex;
            align-items: center;
            justify-content: center;
            background: #f3f6fa !important;
        }

        .action-heading { font-weight: 700; font-size: 13.8px; color: #223548; }
        .branch-type-select { min-width: 135px; }
        .branch-head .btn { white-space: nowrap; }
        .add-condition-main-btn { padding: 4px 10px !important; min-height: 32px !important; font-size: 11.8px !important; line-height: 1.15; border-radius: 8px; min-width: 82px; }
        .bracket-pair { display: flex; gap: 4px; }
        .bracket-select { min-width: 0; width: 50%; padding-left: .35rem; padding-right: 1.35rem; }
        .condition-row-actions {
            display: flex;
            align-items: flex-end;
            justify-content: flex-start;
            gap: 5px;
            min-height: 100%;
        }
        .condition-action-btn,
        .condition-delete-btn {
            white-space: nowrap;
            min-width: 32px;
            height: 34px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            padding-left: .7rem;
            padding-right: .7rem;
            margin-top: 18px;
        }
        .condition-action-btn i,
        .condition-delete-btn i { font-size: 12px; }
        .condition-row-actions.compact { justify-content: flex-start; }
        .action-label { margin-bottom: .22rem; }
        .branch-head { padding: 10px 12px; }
        .card-head { padding: 10px 14px; }
                .legend-pill,
        .branch-pill { padding: 3px 10px; font-size: 11.6px; }
        .condition-row .row.g-2,
        .action-row .row.g-2 {
            --bs-gutter-x: .45rem;
            --bs-gutter-y: .35rem;
        }
        .condition-row .form-label,
        .action-row .form-label { margin-bottom: .18rem; }
        .condition-row hr { margin: .45rem 0; }

        .employee-picker { position: relative; }
        .employee-picker-control {
            min-height: 38px;
            border: 1px solid #ced4da;
            border-radius: .55rem;
            background: #fff;
            padding: 5px 34px 5px 7px;
            display: flex;
            align-items: center;
            gap: 5px;
            flex-wrap: wrap;
            cursor: pointer;
            position: relative;
        }

        .employee-picker-control:focus-within,
        .employee-picker-control.is-open {
            border-color: #86b7fe;
            box-shadow: 0 0 0 .25rem rgba(13,110,253,.14);
        }

        .employee-picker-placeholder { color: #6c757d; padding: 2px 4px; }
        .employee-picker-arrow { position: absolute; right: 10px; top: 50%; transform: translateY(-50%); font-size: 10px; color: #6c757d; }
        .employee-chip {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            max-width: 180px;
            background: #eef3f8;
            border: 1px solid #d7e0ea;
            border-radius: 14px;
            padding: 3px 8px;
            font-size: 12px;
            line-height: 1.2;
        }

        .employee-chip-name { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
        .employee-chip-remove { border: 0; background: transparent; color: #6c757d; padding: 0; line-height: 1; font-size: 15px; cursor: pointer; }
        .employee-picker-popup {
            position: absolute;
            z-index: 1095;
            left: 0;
            right: 0;
            top: calc(100% + 4px);
            background: #fff;
            border: 1px solid #ced4da;
            border-radius: .55rem;
            box-shadow: 0 8px 22px rgba(0,0,0,.16);
            padding: 8px;
            min-width: 280px;
        }

        .employee-picker-popup.drop-up { top: auto; bottom: calc(100% + 4px); }
        .employee-picker-popup .form-control { position: relative; z-index: 1; }
        .employee-picker-list { max-height: 220px; overflow: auto; border: 1px solid #eef0f3; border-radius: 8px; margin-top: 7px; }
        .employee-picker-item { display: flex; align-items: center; gap: 8px; padding: 8px 10px; cursor: pointer; margin: 0; font-weight: 400; }
        .employee-picker-item:hover { background: #f5f8fc; }
        .employee-picker-item input { margin: 0; pointer-events: none; }
        .employee-picker-empty { padding: 12px; color: #78828c; text-align: center; font-size: 12px; }
        .employee-picker-footer { display: flex; align-items: center; justify-content: space-between; gap: 8px; padding-top: 7px; font-size: 12px; color: #78828c; }

        @media (max-width: 991.98px) {
            #ruleModal .modal-dialog {
                width: calc(100vw - 10px);
                height: calc(100vh - 10px);
                margin: 5px auto;
            }

            .card-head,
            .branch-head {
                align-items: flex-start;
                flex-direction: column;
            }

            .workflow-legend { flex-direction: column; }
        }
    </style>
</head>
<body ng-controller="CriticalController as vm" ng-cloak>
<form id="form1" runat="server">
<div class="page-wrap container-fluid">

    <div class="d-flex justify-content-between align-items-center flex-wrap gap-2 mb-3">
        <div>
            <h3 class="mb-1">Critical Workflow</h3>
            <%--<div class="text-muted">Manage Project / Milestone / Task / Sub Task critical Workflow from one landing page.</div>--%>
        </div>
        <div class="d-flex gap-2">
            <a href="CriticalLabelMaster.aspx" class="btn btn-outline-secondary">Label Master</a>
            <%--<button type="button" class="btn btn-primary" ng-click="vm.openNewRule()">+ Add New</button>--%>
        </div>
    </div>

    <div class="alert alert-danger" ng-if="vm.error">{{vm.error}}</div>
    <div class="alert alert-success" ng-if="vm.success">{{vm.success}}</div>

    <div class="card-box">
        <div class="card-head">
            <div>
                <h4 class="section-title">Manage Critical Workflow</h4>
                <%--<div class="small-muted mt-1">Add and edit Workflow in popup. After save/update, this grid refreshes automatically.</div>--%>
            </div>
            <div class="toolbar">
                <div class="row">
                    <div class="col-7">
                <div class="input-group input-group-sm" style="width:260px;">
                    <span class="input-group-text bg-white"><i class="bi bi-search"></i></span>
                    <input type="text"
                           class="form-control"
                           ng-model="vm.searchText"
                           placeholder="Search workflow..." />
                    <button type="button"
                            class="btn btn-outline-secondary"
                            ng-if="vm.searchText"
                            ng-click="vm.searchText=''">
                        <i class="bi bi-x-lg"></i>
                    </button>
                   
                    </div>
                         </div>
                    <div class="col-5" style="display:none;">
 <select class="form-select form-select-sm"
                        ng-model="vm.ruleFilterLevel"
                        ng-change="vm.loadRules()"
                        ng-options="x.id as x.LevelName for x in vm.ruleLevelOptions">
                </select>
                    </div>
                                
                </div>
                
                <button type="button" class="btn btn-sm btn-outline-secondary" ng-click="vm.loadRules()">Refresh</button>
                <button type="button" class="btn btn-sm btn-primary" ng-click="vm.openNewRule()">+ Add New</button>
            </div>
        </div>

        <div class="card-body-custom">
            <div class="table-responsive">
                <table class="table table-hover table-bordered align-middle mb-0">
                    <thead class="table-light">
                        <tr>
                            <th style="width:70px">Sr No.</th>
                            <%--<th style="width:95px">Rule Priority</th>--%>
                            <th>Rule</th>
                            <th style="width:125px">Apply On</th>
                            <%--<th style="width:150px">Label</th>--%>
                            <%--<th style="width:105px">Conditions</th>class="tdp"
                            <th style="width:105px">Child Workflow</th>--%>
                            <%--<th style="width:80px">Stop</th>--%>
                            <th style="width:95px;">Active</th>
                            <th style="width:100px">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-if="vm.rulesLoading">
                            <td colspan="10" class="text-center py-4">
                                <div class="d-flex align-items-center justify-content-center gap-2 text-muted">
                                    <div class="spinner-border spinner-border-sm text-primary" role="status" aria-hidden="true"></div>
                                    <span>Loading workflows...</span>
                                </div>
                            </td>
                        </tr>
                        <tr ng-if="!vm.rulesLoading" ng-repeat="r in (vm.rules | filter:vm.searchText)">
                            <td >{{$index + 1}}</td>
                            <%--<td><strong>{{r.RulePriority}}</strong></td>--%>
                            <td class="rule-summary">
                                <strong>{{r.RuleName}}</strong>
                                <div class="small-muted" ng-if="r.Description">{{r.Description}}</div>
                            </td>
                            <td >{{r.LevelName}}</td>
                           <%-- <td >
                                <span class="badge-label" ng-if="r.CriticalLabel" ng-style="{'background-color': r.ColorCode || '#6c757d'}">{{r.CriticalLabel}}</span>
                                <span class="text-muted" ng-if="!r.CriticalLabel">Action based</span>
                            </td>--%>
                            <%--<td>{{r.ConditionCount}}</td>
                            <td>{{r.ChildConditionCount || 0}}</td>--%>
                            <%--<td >{{r.StopProcessing ? 'Yes' : 'No'}}</td>--%>
                            <td>
                                <div class="form-check form-switch m-0">
                                    <input class="form-check-input" type="checkbox" ng-model="r.status" ng-change="vm.changeStatus(r)" />
                                </div>
                            </td>
                            <td class="action-col">
                                <button type="button"
                                        class="btn btn-sm btn-outline-primary me-1"
                                        ng-click="vm.editRule(r)"
                                        ng-disabled="r._editLoading">
                                    <span ng-if="r._editLoading" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                    <i ng-if="!r._editLoading" class="bi bi-pencil-square"></i>
                                </button>
                                <button type="button"
                                        class="btn btn-sm btn-outline-danger"
                                        ng-click="vm.deleteRule(r)"
                                        ng-disabled="r._deleteLoading">
                                    <span ng-if="r._deleteLoading" class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                    <i ng-if="!r._deleteLoading" class="bi bi-trash"></i>
                                </button>
                            </td>
                        </tr>
                        <tr ng-if="!vm.rulesLoading && (vm.rules | filter:vm.searchText).length === 0">
                            <td colspan="10" class="text-center text-muted py-4">
                                {{vm.searchText ? 'No matching Workflow found.' : 'No Workflow found.'}}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<!-- ADD / EDIT RULE POPUP -->
<div class="modal fade" id="ruleModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header">
                <div>
                    <h5 class="modal-title">{{vm.rule.RuleId ? 'Edit Critical Workflow' : 'Add New Critical Workflow'}}</h5>
                    <%--<div class="small-muted">Configure IF / ELSE IF / ELSE labels and the actions to perform when a label is true.</div>--%>
                </div>
                <button type="button" class="btn-close" aria-label="Close" ng-click="vm.closeRulePopup()"></button>
            </div>

            <div class="modal-body">
                <div class="alert alert-danger" ng-if="vm.popupError">{{vm.popupError}}</div>

                <div class="card-box mb-3">
                    <div class="card-head"><h4 class="section-title">Workflow Header</h4></div>
                    <div class="card-body-custom">
                        <div class="row g-3">
                            <div class="col-lg-5 col-md-6">
                                <label class="form-label">Workflow Name <span class="text-danger">*</span></label>
                                <input type="text" class="form-control" ng-model="vm.rule.RuleName" placeholder="" />
                            </div>

                            <div class="col-lg-2 col-md-6">
                                <label class="form-label">Apply On <span class="text-danger">*</span></label>
                                <select class="form-select" ng-model="vm.rule.LevelId" ng-change="vm.levelChanged()"
                                        ng-options="x.id as x.LevelName for x in vm.masters.levels">
                                    <option value="">Select</option>
                                </select>
                            </div>

                            <div class="col-lg-2 col-md-3" style="display:none">
                                <label class="form-label">Rule Order</label>
                                <input type="number" min="1" class="form-control" ng-model="vm.rule.RulePriority" />
                            </div>

                            <div class="col-lg-3 col-md-3" style="display:none">
                                <label class="form-label d-block">Stop Processing</label>
                                <div class="form-check form-switch pt-2">
                                    <input class="form-check-input" type="checkbox" ng-model="vm.rule.StopProcessing" />
                                    <label class="form-check-label">{{vm.rule.StopProcessing ? 'Yes' : 'No'}}</label>
                                </div>
                            </div>

                            <div class="col-12">
                                <label class="form-label">Description</label>
                                <textarea class="form-control" rows="2" ng-model="vm.rule.Description" placeholder="Optional rule description"></textarea>
                            </div>
                        </div>

                        <%--<div class="condition-help mt-3">
                            <strong>Hierarchy:</strong>
                            Project can depend on Milestone / Task / Sub Task; Milestone can depend on Task / Sub Task; Task can depend on Sub Task; Sub Task uses Self conditions only.
                        </div>--%>
                    </div>
                </div>

                <div class="card-box mb-0">
                    <div class="card-head">
                        <div>
                            <h4 class="section-title">Conditions &amp; Actions</h4>
                            <div class="small-muted mt-1">IF is required. ELSE IF and ELSE are optional and selectable. Every label can have its own conditions and actions.</div>
                        </div>
                        <div class="d-flex gap-2 align-items-center">
                            <select class="form-select form-select-sm branch-type-select" ng-model="vm.newBranchType">
                                <option value="ELSEIF">Else If</option>
                                <option value="ELSE">Else</option>
                            </select>
                            <button type="button" class="btn btn-sm btn-outline-primary" ng-click="vm.addBranch(vm.newBranchType)">Add</button>
                        </div>
                    </div>

                    <div class="card-body-custom">
                        <%--<div class="workflow-legend">
                            <span class="legend-pill if">IF = first condition block checked</span>
                            <span class="legend-pill elseif">ELSE IF = checked only when previous block does not match</span>
                            <span class="legend-pill else">ELSE = fallback block when no previous block matches</span>
                        </div>--%>

                        <div class="branch-box"
                             ng-class="{'branch-if': b.BranchType === 'IF', 'branch-elseif': b.BranchType === 'ELSEIF', 'branch-else': b.BranchType === 'ELSE'}"
                             ng-repeat="b in vm.rule.Branches track by b.ClientId">
                            <div class="branch-head">
                                <div class="d-flex align-items-center gap-2 flex-wrap">
                                    <strong>Label {{$index + 1}}</strong>
                                    <span class="branch-pill"
                                          ng-class="{'if': b.BranchType === 'IF', 'elseif': b.BranchType === 'ELSEIF', 'else': b.BranchType === 'ELSE'}">
                                        {{b.BranchType === 'IF' ? 'IF' : (b.BranchType === 'ELSEIF' ? 'ELSE IF' : 'ELSE')}}
                                    </span>
                                    <select class="form-select form-select-sm branch-type-select"
                                            ng-model="b.BranchType"
                                            ng-change="vm.branchTypeChanged(b, $index)"
                                            ng-disabled="$index === 0">
                                        <option value="IF">IF</option>
                                        <option value="ELSEIF">ELSE IF</option>
                                        <option value="ELSE">ELSE</option>
                                    </select>
                                </div>
                                <div class="d-flex align-items-center gap-2 ms-auto">
                                    <button type="button"
                                            class="btn btn-sm btn-outline-primary"
                                            ng-click="vm.addBranchAfter($index, 'ELSEIF')">
                                        + ELSE IF
                                    </button>

                                    <button type="button"
                                            class="btn btn-sm btn-outline-secondary"
                                            ng-if="!vm.hasElseBranch()"
                                            ng-click="vm.addBranchAfter($index, 'ELSE')">
                                        + ELSE
                                    </button>

                                    <button type="button"
                                            class="btn btn-sm btn-outline-danger"
                                            ng-if="$index > 0"
                                            ng-click="vm.removeBranch($index)">
                                        <i class="bi bi-trash"></i>
                                    </button>
                                </div>
                            </div>

                            <div class="branch-body">
                                <!-- CONDITIONS FOR IF / ELSE IF / ELSE -->
                                <div>
                                    <div class="d-flex justify-content-between align-items-center mb-2" >
                                        <div style="display:none">
                                            <span class="fw-bold">{{b.BranchType === 'IF' ? 'IF Conditions' : (b.BranchType === 'ELSEIF' ? 'ELSE IF Conditions' : 'ELSE Conditions')}}</span>
                                            <div class="small-muted" ng-if="b.BranchType !== 'ELSE'">Dependency = Self for this level's own Status / Priority / Completion / Time. Dependency = Child / Descendant when this level depends on a lower level's Critical Label (Project → Milestone/Task/Sub Task, Milestone → Task/Sub Task, Task → Sub Task). For “any critical child”, use Count >= 1.</div>
                                            <div class="small-muted" ng-if="b.BranchType === 'ELSE'">Condition is optional. By default ELSE runs only its action. Click + Add Condition only when an ELSE condition is required.</div>
                                        </div>
                                        <button type="button" class="btn btn-sm btn-outline-primary add-condition-main-btn" ng-click="vm.addCondition(b)" title="Add Condition" style="display:none">+ Add</button>
                                    </div>

                                    <div class="condition-row" ng-repeat="c in b.Conditions track by c.ClientId">
                                        <div class="row g-2 align-items-end">
                                            <div class="col-lg-1 col-md-2">
                                                <label class="form-label">{{$index === 0 ? (b.BranchType === 'IF' ? 'IF' : (b.BranchType === 'ELSEIF' ? 'ELSE IF' : 'ELSE')) : 'Join'}}</label>
                                                <select class="form-select" ng-if="$index > 0" ng-model="c.JoinOperatorId"
                                                        ng-options="x.id as x.OperatorName for x in vm.masters.logicalOperators"></select>
                                                <div class="form-control bg-light if-box" ng-if="$index === 0">{{b.BranchType === 'IF' ? 'IF' : (b.BranchType === 'ELSEIF' ? 'ELSE IF' : 'ELSE')}}</div>
                                            </div>

                                            <div class="col-lg-1 col-md-2">
                                                <label class="form-label">Dependency</label>
                                                <select class="form-select" ng-model="c.ConditionSource" ng-change="vm.sourceChanged(c)">
                                                    <option value="SELF">Self</option>
                                                    <option value="CHILD" ng-if="vm.hasChildLevels()">Child</option>
                                                </select>
                                            </div>

                                            <div class="col-lg-2 col-md-4" ng-if="c.ConditionSource === 'SELF'">
                                                <label class="form-label">Field</label>
                                                <select class="form-select" ng-model="c.FieldId" ng-change="vm.fieldChanged(c)"
                                                        ng-options="x.id as x.FieldName for x in vm.masters.fields">
                                                    <option value="">Select Field</option>
                                                </select>
                                            </div>

                                            <div class="col-lg-2 col-md-4" ng-if="c.ConditionSource === 'CHILD'">
                                                <label class="form-label">Depends On</label>
                                                <select class="form-select" ng-model="c.ChildLevelId" ng-change="vm.childLevelChanged(c)"
                                                        ng-options="x.id as x.LevelName for x in vm.childLevelsForRule()">
                                                    <option value="">Select Level</option>
                                                </select>
                                            </div>

                                            <div class="col-lg-2 col-md-4" ng-if="c.ConditionSource === 'CHILD'">
                                                <label class="form-label">Critical Label</label>
                                                <select class="form-select" ng-model="c.ChildCriticalLabelId"
                                                        ng-options="x.id as x.CriticalLabel for x in vm.masters.labels">
                                                    <option value="">Select Label</option>
                                                </select>
                                            </div>

                                            <div class="col-lg-1 col-md-3" ng-if="c.ConditionSource === 'CHILD'">
                                                <label class="form-label">Measure</label>
                                                <select class="form-select" ng-model="c.ChildMeasure" ng-change="vm.childMeasureChanged(c)">
                                                    <option value="COUNT">Count</option>
                                                    <option value="PERCENTAGE">%</option>
                                                </select>
                                            </div>

                                            <div class="col-lg-1 col-md-3">
                                                <label class="form-label">Operator</label>
                                                <select class="form-select" ng-model="c.OperatorId" ng-change="vm.operatorChanged(c)"
                                                        ng-disabled="(c.ConditionSource === 'SELF' && !c.FieldId) || (c.ConditionSource === 'CHILD' && !c.ChildLevelId)"
                                                        ng-options="x.id as x.OperatorSymbol for x in vm.allowedOperators(c)">
                                                    <option value="">Op</option>
                                                </select>
                                            </div>

                                            <div class="col-lg-2 col-md-4">
                                                <label class="form-label">Value</label>
                                                <select class="form-select"
                                                        ng-if="c.ConditionSource === 'SELF' && vm.isMasterField(c) && !vm.isMultiOperator(c)"
                                                        ng-model="c.CompareValue"
                                                        ng-options="(x.ValueId + '') as x.ValueText for x in vm.valueOptions(c)">
                                                    <option value="">Select Value</option>
                                                </select>

                                                <div class="employee-picker"
                                                     ng-if="c.ConditionSource === 'SELF' && vm.isMasterField(c) && vm.isMultiOperator(c)">
                                                    <div class="employee-picker-control"
                                                         ng-class="{'is-open':c.ValuePickerOpen}"
                                                         ng-click="vm.toggleValuePicker(c,$event)">
                                                        <span class="employee-picker-placeholder" ng-if="!c.SelectedValues.length">
                                                            Select values...
                                                        </span>

                                                        <span class="employee-chip"
                                                              ng-repeat="valueId in c.SelectedValues track by $index">
                                                            <span class="employee-chip-name">{{vm.masterValueText(c,valueId)}}</span>
                                                            <button type="button" class="employee-chip-remove"
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
                                                        <input type="text" class="form-control form-control-sm"
                                                               ng-model="c.ValueSearch"
                                                               ng-click="$event.stopPropagation()"
                                                               placeholder="Search value..." />

                                                        <div class="employee-picker-list">
                                                            <label class="employee-picker-item"
                                                                   ng-repeat="opt in (vm.valueOptions(c) | filter:{ValueText:c.ValueSearch}) track by opt.ValueId"
                                                                   ng-click="vm.toggleMasterValue(c,opt,$event)">
                                                                <input type="checkbox"
                                                                       ng-checked="vm.isMasterValueSelected(c,opt.ValueId)" />
                                                                <span>{{opt.ValueText}}</span>
                                                            </label>

                                                            <div class="employee-picker-empty"
                                                                 ng-if="!(vm.valueOptions(c) | filter:{ValueText:c.ValueSearch}).length">
                                                                No value found.
                                                            </div>
                                                        </div>

                                                        <div class="employee-picker-footer">
                                                            <span>{{c.SelectedValues.length}} value(s) selected</span>
                                                            <button type="button" class="btn btn-primary btn-sm"
                                                                    ng-click="vm.closeValuePicker(c,$event)">Done</button>
                                                        </div>
                                                    </div>
                                                </div>

                                                <input type="number" step="0.01" class="form-control"
                                                       ng-if="c.ConditionSource === 'CHILD' || (c.ConditionSource === 'SELF' && vm.dataType(c) === 'NUMBER')"
                                                       ng-model="c.CompareValue"
                                                       ng-attr-min="{{c.ConditionSource === 'CHILD' ? 0 : undefined}}"
                                                       ng-attr-max="{{c.ConditionSource === 'CHILD' && c.ChildMeasure === 'PERCENTAGE' ? 100 : undefined}}"
                                                       placeholder="Value" />

                                                <input type="text" class="form-control"
                                                       ng-if="c.ConditionSource === 'SELF' && vm.dataType(c) !== 'NUMBER' && !vm.isMasterField(c)"
                                                       ng-model="c.CompareValue" placeholder="Value" />
                                            </div>

                                            <div class="col-md-3" ng-if="vm.operatorSymbol(c) === 'BETWEEN'">
                                                <label class="form-label">Second Value</label>
                                                <input type="number" step="0.01" class="form-control" ng-model="c.CompareValue2"
                                                       ng-attr-min="{{c.ConditionSource === 'CHILD' ? 0 : undefined}}"
                                                       ng-attr-max="{{c.ConditionSource === 'CHILD' && c.ChildMeasure === 'PERCENTAGE' ? 100 : undefined}}"
                                                       placeholder="Second value" />
                                            </div>

                                            <div class="col-lg-1 col-md-2" style="display:none;">
                                                <label class="form-label">Bracket</label>
                                                <div class="bracket-pair">
                                                    <select class="form-select bracket-select"
                                                            ng-model="c.OpenBracket"
                                                            ng-options="x.Value as x.Text for x in vm.openBracketOptions"
                                                            title="Open bracket">
                                                    </select>
                                                    <select class="form-select bracket-select"
                                                            ng-model="c.CloseBracket"
                                                            ng-options="x.Value as x.Text for x in vm.closeBracketOptions"
                                                            title="Close bracket">
                                                    </select>
                                                </div>
                                            </div>

                                            <div class="col-lg-1 col-md-2 col-12">
                                                <label class="form-label action-label">Action</label>
                                                <div class="condition-row-actions compact">
                                                    <button type="button" class="btn btn-sm btn-outline-primary condition-action-btn" ng-click="vm.addCondition(b)" title="Add new condition below"><i class="bi bi-plus-lg"></i></button>
                                                    <button type="button" class="btn btn-sm btn-outline-danger condition-delete-btn" ng-click="vm.removeCondition(b,$index)" title="Delete this condition"><i class="bi bi-trash"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <hr />

                                <!-- ACTIONS FOR THIS BRANCH -->
                                <div class="d-flex justify-content-between align-items-center mb-2">
                                    <div>
                                        <span class="action-heading">THEN Actions</span>
                                        <div class="small-muted">Actions below run only when this {{b.BranchType === 'IF' ? 'IF' : (b.BranchType === 'ELSEIF' ? 'ELSE IF' : 'ELSE')}} label is selected.</div>
                                    </div>
                                    <button type="button" class="btn btn-sm btn-outline-primary" ng-click="vm.addAction(b)">+ Add Action</button>
                                </div>

                                <div class="empty-box mb-2" ng-if="!b.Actions.length">No action added.</div>

                                <div class="action-row" ng-repeat="a in b.Actions track by a.ClientId">
                                    <div class="row g-2 align-items-end">
                                        <div class="col-lg-1 col-md-2">
                                            <label class="form-label" ng-if="$index === 0">THEN</label>
                                            <div class="form-control bg-light if-box">{{$index === 0 ? '' : 'AND'}}</div>
                                        </div>

                                        <div class="col-lg-3 col-md-5">
                                            <label class="form-label" ng-if="$index === 0">Action Name</label>
                                            <select class="form-select" ng-model="a.ActionId" ng-change="vm.actionChanged(a)"
                                                    ng-options="x.id as x.ActionName for x in vm.workflowActions()">
                                                <option value="">Select Action</option>
                                            </select>
                                        </div>

                                        <div class="col-lg-3 col-md-5" ng-if="vm.actionCode(a) === 'SET_CRITICAL_LABEL'">
                                            <label class="form-label">Set Label</label>
                                            <select class="form-select" ng-model="a.ActionValue"
                                                    ng-options="(x.id + '') as x.CriticalLabel for x in vm.masters.labels">
                                                <option value="">Select Critical Label</option>
                                            </select>
                                        </div>

                                        <div class="col-lg-3 col-md-6" ng-if="vm.actionCode(a) === 'SEND_NOTIFICATION'">
                                            <label class="form-label">Recipient (Multiple)</label>
                                            <div class="employee-picker recipient-picker">
                                                <div class="employee-picker-control"
                                                     ng-class="{'is-open': a.RecipientPickerOpen}"
                                                     ng-click="vm.toggleRecipientPicker(a,$event)">

                                                    <span class="employee-picker-placeholder" ng-if="!a.SelectedRecipients.length">
                                                        Select recipients...
                                                    </span>

                                                    <span class="employee-chip"
                                                          ng-repeat="recipientType in a.SelectedRecipients track by recipientType">
                                                        <span class="employee-chip-name">{{vm.recipientName(recipientType)}}</span>
                                                        <button type="button" class="employee-chip-remove"
                                                                title="Remove"
                                                                ng-click="vm.removeSelectedRecipient(a,recipientType,$event)">&times;</button>
                                                    </span>

                                                    <span class="employee-picker-arrow">&#9662;</span>
                                                </div>

                                                <div class="employee-picker-popup"
                                                     ng-class="{'drop-up': a.RecipientDropUp}"
                                                     ng-if="a.RecipientPickerOpen"
                                                     ng-click="$event.stopPropagation()">
                                                    <div class="employee-picker-list" style="margin-top:0;">
                                                        <label class="employee-picker-item"
                                                               ng-repeat="recipient in vm.recipientOptions track by recipient.Value"
                                                               ng-click="vm.toggleRecipient(a,recipient,$event)">
                                                            <input type="checkbox"
                                                                   ng-checked="vm.isRecipientSelected(a,recipient.Value)" />
                                                            <span>{{recipient.Text}}</span>
                                                        </label>
                                                    </div>

                                                    <div class="employee-picker-footer">
                                                        <span>{{a.SelectedRecipients.length}} recipient(s) selected</span>
                                                        <button type="button" class="btn btn-sm btn-primary"
                                                                ng-click="vm.closeRecipientPicker(a,$event)">Done</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-4 col-md-6" ng-if="vm.actionCode(a) === 'SEND_NOTIFICATION' && vm.isRecipientSelected(a,'CUSTOM')">
                                            <label class="form-label">Employees</label>
                                            <div class="employee-picker">
                                                <div class="employee-picker-control"
                                                     ng-class="{'is-open': a.EmployeePickerOpen}"
                                                     ng-click="vm.toggleEmployeePicker(a,$event)">

                                                    <span class="employee-picker-placeholder" ng-if="!a.SelectedEmployees.length">
                                                        Select employees...
                                                    </span>

                                                    <span class="employee-chip"
                                                          ng-repeat="empId in a.SelectedEmployees track by empId">
                                                        <span class="employee-chip-name">{{vm.employeeName(empId)}}</span>
                                                        <button type="button" class="employee-chip-remove"
                                                                title="Remove"
                                                                ng-click="vm.removeSelectedEmployee(a,empId,$event)">&times;</button>
                                                    </span>

                                                    <span class="employee-picker-arrow">&#9662;</span>
                                                </div>

                                                <div class="employee-picker-popup"
                                                     ng-class="{'drop-up': a.EmployeeDropUp}"
                                                     ng-if="a.EmployeePickerOpen"
                                                     ng-click="$event.stopPropagation()">
                                                    <input type="text" class="form-control form-control-sm"
                                                           ng-model="a.EmployeeSearch"
                                                           placeholder="Search employee..." />

                                                    <div class="employee-picker-list">
                                                        <label class="employee-picker-item"
                                                               ng-repeat="emp in (vm.masters.employees | filter:{DisplayName:a.EmployeeSearch}) track by emp.id"
                                                               ng-click="vm.toggleEmployee(a,emp,$event)">
                                                            <input type="checkbox"
                                                                   ng-checked="vm.isEmployeeSelected(a,emp.id)" />
                                                            <span>{{emp.DisplayName}}</span>
                                                        </label>

                                                        <div class="employee-picker-empty"
                                                             ng-if="!(vm.masters.employees | filter:{DisplayName:a.EmployeeSearch}).length">
                                                            No employee found.
                                                        </div>
                                                    </div>

                                                    <div class="employee-picker-footer">
                                                        <span>{{a.SelectedEmployees.length}} employee(s) selected</span>
                                                        <button type="button" class="btn btn-sm btn-primary"
                                                                ng-click="vm.closeEmployeePicker(a,$event)">Done</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-3 col-md-6" ng-if="vm.actionCode(a) === 'SEND_NOTIFICATION'">
                                            <label class="form-label d-block">Notification By</label>
                                            <div class="d-flex align-items-center flex-wrap gap-3" style="min-height:38px;">
                                                <label class="form-check d-flex align-items-center gap-1 mb-0">
                                                    <input class="form-check-input mt-0" type="checkbox" ng-model="a.IsEmail" />
                                                    <span>Email</span>
                                                </label>
                                                <label class="form-check d-flex align-items-center gap-1 mb-0">
                                                    <input class="form-check-input mt-0" type="checkbox" ng-model="a.IsWhatsApp" />
                                                    <span>WhatsApp</span>
                                                </label>
                                                <label class="form-check d-flex align-items-center gap-1 mb-0">
                                                    <input class="form-check-input mt-0" type="checkbox" ng-model="a.IsBell" />
                                                    <span>Bell</span>
                                                </label>
                                            </div>
                                        </div>

                                        <div class="col-lg-2 col-md-5" style="display:none" ng-if="vm.actionCode(a) === 'SEND_NOTIFICATION'">
                                            <label class="form-label">Notification Text</label>
                                            <input type="text" class="form-control" ng-model="a.ActionValue" placeholder="Optional notification message" />
                                        </div>

                                        <div class="col-lg-1 col-md-2 ms-auto">
                                            <label class="form-label d-block" ng-if="$index === 0">Delete</label>
                                            <button type="button" class="btn btn-sm btn-outline-danger" ng-click="vm.removeAction(b,$index)"><i class="bi bi-trash"></i></button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal-footer">
                <%--<button type="button" class="btn btn-light" ng-click="vm.resetRule()">Reset</button>--%>
                <button type="button" class="btn btn-outline-secondary" ng-click="vm.closeRulePopup()">Cancel</button>
                <button type="button" class="btn btn-primary" ng-click="vm.saveRule()" ng-disabled="vm.saving">
                    {{vm.saving ? 'Saving...' : (vm.rule.RuleId ? 'Update Workflow' : 'Save Workflow')}}
                </button>
            </div>
        </div>
    </div>
</div>
</form>

<script>
(function () {
    'use strict';

    angular.module('criticalApp', [])
        .controller('CriticalController', CriticalController);

    CriticalController.$inject = ['$http', '$timeout', '$document', '$scope'];

    function CriticalController($http, $timeout, $document, $scope) {
        var vm = this;
        // CriticalSetting ELSE fix version: 20260820-1200
        var clientSeed = 1;

        vm.masters = {
            labels: [], levels: [], fields: [], operators: [], actions: [],
            logicalOperators: [], statuses: [], priorities: [], employees: []
        };
        vm.rules = [];
        vm.ruleFilterLevel = 0;
        vm.ruleLevelOptions = [{ id: 0, LevelName: 'All Levels' }];
        vm.rule = newRule();
        vm.newBranchType = 'ELSEIF';
        vm.openBracketOptions = [
            { Value: '', Text: '-' },
            { Value: '(', Text: '(' },
            { Value: '((', Text: '((' }
        ];
        vm.closeBracketOptions = [
            { Value: '', Text: '-' },
            { Value: ')', Text: ')' },
            { Value: '))', Text: '))' }
        ];
        vm.recipientOptions = [
            { Value: 'OWNER', Text: 'Owner' },
            { Value: 'TEAM', Text: 'Team' },
            { Value: 'PROJECT_MANAGER', Text: 'Project Manager' },
            { Value: 'CUSTOM', Text: 'Custom Employee' }
        ];

        // Close recipient / employee multi-select on ANY click/tap outside it.
        // Capture phase is used because Bootstrap/modal controls can stop normal click bubbling.
        document.addEventListener('mousedown', onDocumentPointerDown, true);
        document.addEventListener('touchstart', onDocumentPointerDown, true);
        $scope.$on('$destroy', function () {
            document.removeEventListener('mousedown', onDocumentPointerDown, true);
            document.removeEventListener('touchstart', onDocumentPointerDown, true);
        });
        vm.searchText = '';
        // Initial page load first fetches masters and then the workflow list.
        // Keep the grid loader visible until that complete chain finishes.
        vm.rulesLoading = true;
        vm.error = '';
        vm.success = '';
        vm.popupError = '';
        vm.saving = false;
        vm.ruleModal = null;

        vm.loadRules = loadRules;
        vm.openNewRule = openNewRule;
        vm.closeRulePopup = closeRulePopup;
        vm.addBranch = addBranch;
        vm.addBranchAfter = addBranchAfter;
        vm.hasElseBranch = hasElseBranch;
        vm.removeBranch = removeBranch;
        vm.branchTypeChanged = branchTypeChanged;
        vm.addCondition = addCondition;
        vm.removeCondition = removeCondition;
        vm.addAction = addAction;
        vm.removeAction = removeAction;
        vm.actionChanged = actionChanged;
        vm.recipientChanged = recipientChanged;
        vm.toggleRecipientPicker = toggleRecipientPicker;
        vm.closeRecipientPicker = closeRecipientPicker;
        vm.toggleRecipient = toggleRecipient;
        vm.isRecipientSelected = isRecipientSelected;
        vm.removeSelectedRecipient = removeSelectedRecipient;
        vm.recipientName = recipientName;
        vm.toggleEmployeePicker = toggleEmployeePicker;
        vm.closeEmployeePicker = closeEmployeePicker;
        vm.toggleEmployee = toggleEmployee;
        vm.isEmployeeSelected = isEmployeeSelected;
        vm.removeSelectedEmployee = removeSelectedEmployee;
        vm.employeeName = employeeName;
        vm.toggleValuePicker = toggleValuePicker;
        vm.closeValuePicker = closeValuePicker;
        vm.toggleMasterValue = toggleMasterValue;
        vm.isMasterValueSelected = isMasterValueSelected;
        vm.removeSelectedMasterValue = removeSelectedMasterValue;
        vm.masterValueText = masterValueText;
        vm.actionCode = actionCode;
        vm.workflowActions = workflowActions;
        vm.levelChanged = levelChanged;
        vm.sourceChanged = sourceChanged;
        vm.fieldChanged = fieldChanged;
        vm.childLevelChanged = childLevelChanged;
        vm.childMeasureChanged = childMeasureChanged;
        vm.operatorChanged = operatorChanged;
        vm.saveRule = saveRule;
        vm.resetRule = resetRule;
        vm.editRule = editRule;
        vm.deleteRule = deleteRule;
        vm.changeStatus = changeStatus;
        vm.getField = getField;
        vm.dataType = dataType;
        vm.sourceType = sourceType;
        vm.operatorSymbol = operatorSymbol;
        vm.allowedOperators = allowedOperators;
        vm.isMasterField = isMasterField;
        vm.isMultiOperator = isMultiOperator;
        vm.valueOptions = valueOptions;
        vm.childLevelsForRule = childLevelsForRule;
        vm.hasChildLevels = hasChildLevels;

        init();

        function nextClientId(prefix) { return (prefix || 'x') + (clientSeed++); }

        function init() { loadMasters(); }

        function pagePost(method, payload) {
            return $http({
                method: 'POST',
                url: 'CriticalSetting.aspx/' + method,
                data: payload || {},
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            }).then(function (response) {
                var raw = response.data ? response.data.d : null;
                return raw ? angular.fromJson(raw) : {};
            });
        }

        function showSuccess(msg) {
            vm.success = msg || 'Saved successfully.';
            vm.error = '';
            $timeout(function () { vm.success = ''; }, 3500);
        }

        function showError(msg) {
            vm.error = msg || 'Something went wrong.';
            vm.success = '';
            $timeout(function () { vm.error = ''; }, 5000);
        }

        function showPopupError(msg) {
            vm.popupError = msg || 'Please check rule data.';
            $timeout(function () { vm.popupError = ''; }, 5000);
        }

        function newCondition() {
            var defaultLogical = vm.masters.logicalOperators.length ? vm.masters.logicalOperators[0].id : null;
            return {
                ClientId: nextClientId('c'), SequenceNo: 0, ConditionSource: 'SELF', OpenBracket: '',
                FieldId: null, ChildLevelId: null, ChildCriticalLabelId: null, ChildMeasure: 'COUNT',
                OperatorId: null, CompareValue: '', CompareValue2: '', SelectedValues: [],
                ValuePickerOpen: false, ValueSearch: '', ValueDropUp: false,
                LogicalOperatorId: null, JoinOperatorId: defaultLogical, CloseBracket: '', GroupNo: 1
            };
        }

        function newAction() {
            return {
                ClientId: nextClientId('a'), ActionId: null, ActionValue: '', RecipientType: '',
                RecipientValue: '', IsEmail: false, IsWhatsApp: false, IsBell: false,
                SelectedRecipients: [], RecipientPickerOpen: false, RecipientDropUp: false,
                SelectedEmployees: [], EmployeePickerOpen: false, EmployeeSearch: '', EmployeeDropUp: false, SequenceNo: 0
            };
        }

        function newBranch(type) {
            var t = String(type || 'IF').toUpperCase();
            var b = { ClientId: nextClientId('b'), BranchType: t, Conditions: [], Actions: [] };

            // IF / ELSE IF keep the existing mandatory default condition.
            // ELSE starts action-only. Optional condition can be added with + Add Condition.
            if (t !== 'ELSE')
                b.Conditions.push(newCondition());

            b.Actions.push(newAction());
            return b;
        }

        function newRule() {
            return {
                RuleId: 0, RuleName: '', LevelId: null, CriticalLabelId: null,
                RulePriority: 1, StopProcessing: false, Description: '',
                Branches: [newBranch('IF')]
            };
        }

        function getModal() {
            var el = document.getElementById('ruleModal');
            if (!el || typeof bootstrap === 'undefined') return null;
            vm.ruleModal = bootstrap.Modal.getOrCreateInstance(el, { backdrop: 'static', keyboard: false });
            return vm.ruleModal;
        }

        function openRulePopup() {
            vm.popupError = '';
            var modal = getModal();
            if (modal) modal.show();
        }

        function closeRulePopup() {
            vm.popupError = '';
            var modal = getModal();
            if (modal) modal.hide();
        }

        function openNewRule() { resetRule(); openRulePopup(); }

        function loadMasters() {
            pagePost('GetMasters', {}).then(function (res) {
                if (!res.success) {
                    vm.rulesLoading = false;
                    showError(res.message);
                    return;
                }
                vm.masters.labels = res.labels || [];
                vm.masters.levels = res.levels || [];
                vm.ruleLevelOptions = [{ id: 0, LevelName: 'All Levels' }].concat(vm.masters.levels);
                vm.ruleFilterLevel = 0;
                vm.masters.fields = res.fields || [];
                vm.masters.operators = res.operators || [];
                vm.masters.logicalOperators = res.logicalOperators || [];
                vm.masters.actions = res.actions || [];
                vm.masters.statuses = res.statuses || [];
                vm.masters.priorities = res.priorities || [];
                vm.masters.employees = res.employees || [];
                loadRules();
            }).catch(function (err) {
                vm.rulesLoading = false;
                showError(httpMessage(err));
            });
        }

        function addBranch(type) {
            var t = String(type || 'ELSEIF').toUpperCase();
            if (t === 'ELSE' && hasElseBranch()) {
                showPopupError('Only one ELSE label is allowed.');
                return;
            }
            if (hasElseBranch()) {
                showPopupError('ELSE must be the last label. Delete ELSE first to add another ELSE IF.');
                return;
            }
            vm.rule.Branches.push(newBranch(t));
        }

        function addBranchAfter(index, type) {
            var t = String(type || 'ELSEIF').toUpperCase();

            if (t === 'ELSE') {
                if (hasElseBranch()) {
                    showPopupError('Only one ELSE label is allowed.');
                    return;
                }

                // ELSE must always remain the final label.
                vm.rule.Branches.push(newBranch('ELSE'));
                return;
            }

            // ELSE IF is inserted immediately after the current label.
            // If current label is ELSE, insert before ELSE so ELSE stays last.
            var insertIndex = index + 1;

            if (vm.rule.Branches[index] &&
                vm.rule.Branches[index].BranchType === 'ELSE') {
                insertIndex = index;
            }

            vm.rule.Branches.splice(insertIndex, 0, newBranch('ELSEIF'));
        }

        function removeBranch(index) {
            if (index <= 0) return;
            vm.rule.Branches.splice(index, 1);
        }

        function hasElseBranch() {
            return vm.rule.Branches.some(function (b) { return b.BranchType === 'ELSE'; });
        }

        function branchTypeChanged(b, index) {
            if (index === 0) { b.BranchType = 'IF'; return; }
            if (b.BranchType === 'IF') b.BranchType = 'ELSEIF';
            if (b.BranchType === 'ELSE') {
                for (var i = 0; i < vm.rule.Branches.length; i++) {
                    if (i !== index && vm.rule.Branches[i].BranchType === 'ELSE') {
                        b.BranchType = 'ELSEIF';
                        showPopupError('Only one ELSE label is allowed.');
                        return;
                    }
                }

                // ELSE is action-only by default.
                // Any condition can be added later only with + Add Condition.
                b.Conditions = [];

                // ELSE must remain the final branch.
                if (index !== vm.rule.Branches.length - 1) {
                    vm.rule.Branches.splice(index, 1);
                    vm.rule.Branches.push(b);
                }
            }
            // IF / ELSE IF must keep one condition row.
            // ELSE may remain with zero conditions and only its action.
            if (b.BranchType !== 'ELSE' && !b.Conditions.length)
                b.Conditions.push(newCondition());
        }

        function addCondition(branch) { branch.Conditions.push(newCondition()); }

        function removeCondition(branch, index) {
            branch.Conditions.splice(index, 1);

            // Preserve old behavior for IF / ELSE IF only.
            // For ELSE, deleting the last condition must really remove it.
            if (branch.BranchType !== 'ELSE' && !branch.Conditions.length)
                branch.Conditions.push(newCondition());
        }

        function addAction(branch) { branch.Actions.push(newAction()); }

        function removeAction(branch, index) {
            branch.Actions.splice(index, 1);
            if (!branch.Actions.length) branch.Actions.push(newAction());
        }

        function workflowActions() {
            return vm.masters.actions.filter(function (x) {
                var code = String(x.ActionCode || '').toUpperCase();
                return code === 'SET_CRITICAL_LABEL' || code === 'SEND_NOTIFICATION';
            });
        }

        function actionCode(a) {
            if (!a || !a.ActionId) return '';
            for (var i = 0; i < vm.masters.actions.length; i++) {
                if (String(vm.masters.actions[i].id) === String(a.ActionId))
                    return String(vm.masters.actions[i].ActionCode || '').toUpperCase();
            }
            return '';
        }

        function criticalBool(value) {
            if (value === true || value === 1) return true;
            var v = String(value === null || typeof value === 'undefined' ? '' : value).toLowerCase();
            return v === '1' || v === 'true' || v === 'yes';
        }

        function actionChanged(a) {
            a.ActionValue = '';
            a.RecipientType = '';
            a.RecipientValue = '';
            a.SelectedRecipients = [];
            a.RecipientPickerOpen = false;
            a.RecipientDropUp = false;
            a.SelectedEmployees = [];
            a.IsEmail = false;
            a.IsWhatsApp = false;
            a.IsBell = false;
            a.EmployeePickerOpen = false;
            a.EmployeeSearch = '';
            a.EmployeeDropUp = false;
        }

        // Kept for backward compatibility with any old markup/custom script.
        function recipientChanged(a) {
            if (!a.SelectedRecipients) a.SelectedRecipients = [];
            if (a.RecipientType && !isRecipientSelected(a, a.RecipientType))
                a.SelectedRecipients.push(String(a.RecipientType));
            if (!isRecipientSelected(a, 'CUSTOM')) clearCustomEmployees(a);
        }

        function clearCustomEmployees(a) {
            a.RecipientValue = '';
            a.SelectedEmployees = [];
            a.EmployeePickerOpen = false;
            a.EmployeeSearch = '';
            a.EmployeeDropUp = false;
        }

        function isRecipientSelected(a, recipientType) {
            var type = String(recipientType || '').toUpperCase();
            var selected = a && a.SelectedRecipients ? a.SelectedRecipients : [];
            for (var i = 0; i < selected.length; i++) {
                if (String(selected[i]).toUpperCase() === type) return true;
            }
            return false;
        }

        function recipientName(recipientType) {
            var type = String(recipientType || '').toUpperCase();
            for (var i = 0; i < vm.recipientOptions.length; i++) {
                if (vm.recipientOptions[i].Value === type) return vm.recipientOptions[i].Text;
            }
            return recipientType || '';
        }

        function toggleRecipient(a, recipient, $event) {
            if ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            if (!a.SelectedRecipients) a.SelectedRecipients = [];
            var type = String(recipient.Value || recipient).toUpperCase();
            var index = -1;
            for (var i = 0; i < a.SelectedRecipients.length; i++) {
                if (String(a.SelectedRecipients[i]).toUpperCase() === type) { index = i; break; }
            }
            if (index >= 0) {
                a.SelectedRecipients.splice(index, 1);
                if (type === 'CUSTOM') clearCustomEmployees(a);
            } else {
                a.SelectedRecipients.push(type);
            }
        }

        function removeSelectedRecipient(a, recipientType, $event) {
            if ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            if (!a.SelectedRecipients) return;
            var type = String(recipientType || '').toUpperCase();
            for (var i = a.SelectedRecipients.length - 1; i >= 0; i--) {
                if (String(a.SelectedRecipients[i]).toUpperCase() === type)
                    a.SelectedRecipients.splice(i, 1);
            }
            if (type === 'CUSTOM') clearCustomEmployees(a);
        }

        function toggleRecipientPicker(a, $event) {
            if ($event) $event.stopPropagation();
            // Recipient and employee popups share the same action row; keep only one open.
            a.EmployeePickerOpen = false; a.EmployeeSearch = ''; a.EmployeeDropUp = false;
            closeOtherEmployeePickers(a);
            var opening = !a.RecipientPickerOpen;
            a.RecipientPickerOpen = opening;
            if (!opening) { a.RecipientDropUp = false; return; }

            a.RecipientDropUp = false;
            try {
                var control = $event && $event.currentTarget ? $event.currentTarget : null;
                var modalBody = control ? angular.element(control).closest('.modal-body')[0] : null;
                if (control && modalBody) {
                    var controlRect = control.getBoundingClientRect();
                    var bodyRect = modalBody.getBoundingClientRect();
                    var roomBelow = bodyRect.bottom - controlRect.bottom;
                    var roomAbove = controlRect.top - bodyRect.top;
                    a.RecipientDropUp = roomBelow < 230 && roomAbove > roomBelow;
                }
            } catch (e) { a.RecipientDropUp = false; }
        }

        function closeRecipientPicker(a, $event) {
            if ($event) $event.stopPropagation();
            a.RecipientPickerOpen = false;
            a.RecipientDropUp = false;
        }

        function closeOtherEmployeePickers(currentAction) {
            angular.forEach(vm.rule.Branches || [], function (b) {
                angular.forEach(b.Actions || [], function (x) {
                    if (x !== currentAction) {
                        x.EmployeePickerOpen = false; x.EmployeeDropUp = false;
                        x.RecipientPickerOpen = false; x.RecipientDropUp = false;
                    }
                });
                angular.forEach(b.Conditions || [], function (c) {
                    c.ValuePickerOpen = false; c.ValueSearch = ''; c.ValueDropUp = false;
                });
            });
        }

        function closeOtherValuePickers(currentCondition) {
            angular.forEach(vm.rule.Branches || [], function (b) {
                angular.forEach(b.Conditions || [], function (c) {
                    if (c !== currentCondition) { c.ValuePickerOpen = false; c.ValueSearch = ''; c.ValueDropUp = false; }
                });
                angular.forEach(b.Actions || [], function (a) {
                    a.EmployeePickerOpen = false; a.EmployeeSearch = ''; a.EmployeeDropUp = false;
                    a.RecipientPickerOpen = false; a.RecipientDropUp = false;
                });
            });
        }

        function isInsideEmployeePicker(target) {
            var node = target;
            while (node && node !== document) {
                if (node.classList && node.classList.contains('employee-picker')) return true;
                node = node.parentNode;
            }
            return false;
        }

        function onDocumentPointerDown(event) {
            var target = event && event.target ? event.target : null;

            // Keep the selector open for clicks on its control, search, list, checkbox, chips or Done.
            if (target && isInsideEmployeePicker(target)) return;

            var changed = false;
            angular.forEach(vm.rule.Branches || [], function (b) {
                angular.forEach(b.Actions || [], function (a) {
                    if (a.EmployeePickerOpen) {
                        a.EmployeePickerOpen = false;
                        a.EmployeeSearch = '';
                        a.EmployeeDropUp = false;
                        changed = true;
                    }
                    if (a.RecipientPickerOpen) {
                        a.RecipientPickerOpen = false;
                        a.RecipientDropUp = false;
                        changed = true;
                    }
                });
                angular.forEach(b.Conditions || [], function (c) {
                    if (c.ValuePickerOpen) {
                        c.ValuePickerOpen = false;
                        c.ValueSearch = '';
                        c.ValueDropUp = false;
                        changed = true;
                    }
                });
            });

            if (changed) {
                $scope.$evalAsync();
            }
        }

        function toggleEmployeePicker(a, $event) {
            if ($event) $event.stopPropagation();
            // Recipient and employee popups share the same action row; keep only one open.
            a.RecipientPickerOpen = false; a.RecipientDropUp = false;
            closeOtherEmployeePickers(a);

            var opening = !a.EmployeePickerOpen;
            a.EmployeePickerOpen = opening;

            if (!opening) {
                a.EmployeeSearch = '';
                a.EmployeeDropUp = false;
                return;
            }

            // Open upward when there is not enough visible room below the control.
            // This keeps the complete employee list visible inside the scrollable rule popup.
            a.EmployeeDropUp = false;
            try {
                var control = $event && $event.currentTarget ? $event.currentTarget : null;
                var modalBody = control ? angular.element(control).closest('.modal-body')[0] : null;
                if (control && modalBody) {
                    var controlRect = control.getBoundingClientRect();
                    var bodyRect = modalBody.getBoundingClientRect();
                    var roomBelow = bodyRect.bottom - controlRect.bottom;
                    var roomAbove = controlRect.top - bodyRect.top;
                    a.EmployeeDropUp = roomBelow < 300 && roomAbove > roomBelow;
                }
            } catch (e) {
                a.EmployeeDropUp = false;
            }
        }

        function closeEmployeePicker(a, $event) {
            if ($event) $event.stopPropagation();
            a.EmployeePickerOpen = false;
            a.EmployeeSearch = '';
            a.EmployeeDropUp = false;
        }

        function isEmployeeSelected(a, employeeId) {
            var id = String(employeeId);
            var selected = a && a.SelectedEmployees ? a.SelectedEmployees : [];
            for (var i = 0; i < selected.length; i++) {
                if (String(selected[i]) === id) return true;
            }
            return false;
        }

        function toggleEmployee(a, emp, $event) {
            if ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            if (!a.SelectedEmployees) a.SelectedEmployees = [];
            var id = String(emp.id);
            var index = -1;
            for (var i = 0; i < a.SelectedEmployees.length; i++) {
                if (String(a.SelectedEmployees[i]) === id) { index = i; break; }
            }
            if (index >= 0) a.SelectedEmployees.splice(index, 1);
            else a.SelectedEmployees.push(id);
        }

        function removeSelectedEmployee(a, employeeId, $event) {
            if ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }
            if (!a.SelectedEmployees) return;
            var id = String(employeeId);
            for (var i = a.SelectedEmployees.length - 1; i >= 0; i--) {
                if (String(a.SelectedEmployees[i]) === id) a.SelectedEmployees.splice(i, 1);
            }
        }

        function employeeName(employeeId) {
            var id = String(employeeId);
            for (var i = 0; i < vm.masters.employees.length; i++) {
                if (String(vm.masters.employees[i].id) === id)
                    return vm.masters.employees[i].DisplayName || id;
            }
            return id;
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

            c.ValueDropUp = false;
            try {
                var control = $event && $event.currentTarget ? $event.currentTarget : null;
                var modalBody = control ? angular.element(control).closest('.modal-body')[0] : null;
                if (control && modalBody) {
                    var controlRect = control.getBoundingClientRect();
                    var bodyRect = modalBody.getBoundingClientRect();
                    var roomBelow = bodyRect.bottom - controlRect.bottom;
                    var roomAbove = controlRect.top - bodyRect.top;
                    c.ValueDropUp = roomBelow < 260 && roomAbove > roomBelow;
                }
            } catch (e) {
                c.ValueDropUp = false;
            }
        }

        function closeValuePicker(c, $event) {
            if ($event) $event.stopPropagation();
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
            var id = String(opt.ValueId);
            var index = -1;
            for (var i = 0; i < c.SelectedValues.length; i++) {
                if (String(c.SelectedValues[i]) === id) { index = i; break; }
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
                if (String(c.SelectedValues[i]) === id) c.SelectedValues.splice(i, 1);
            }
        }

        function masterValueText(c, valueId) {
            var id = String(valueId);
            var options = valueOptions(c) || [];
            for (var i = 0; i < options.length; i++) {
                if (String(options[i].ValueId) === id) return options[i].ValueText || id;
            }
            return id;
        }

        function getLevel(levelId) {
            for (var i = 0; i < vm.masters.levels.length; i++) {
                if (String(vm.masters.levels[i].id) === String(levelId)) return vm.masters.levels[i];
            }
            return null;
        }

        function getRuleLevelCode() {
            var level = getLevel(vm.rule.LevelId);
            return level ? String(level.LevelCode || '').toUpperCase() : '';
        }

        function childLevelsForRule() {
            var parent = getRuleLevelCode();
            var allowed = [];
            if (parent === 'PROJECT') allowed = ['MILESTONE', 'TASK', 'SUBTASK'];
            else if (parent === 'MILESTONE') allowed = ['TASK', 'SUBTASK'];
            else if (parent === 'TASK') allowed = ['SUBTASK'];
            return vm.masters.levels.filter(function (x) {
                return allowed.indexOf(String(x.LevelCode || '').toUpperCase()) >= 0;
            });
        }

        function hasChildLevels() { return childLevelsForRule().length > 0; }

        function levelChanged() {
            var allowedIds = childLevelsForRule().map(function (x) { return String(x.id); });
            angular.forEach(vm.rule.Branches, function (b) {
                angular.forEach(b.Conditions, function (c) {
                    if (c.ConditionSource === 'CHILD') {
                        if (!hasChildLevels()) resetAsSelf(c);
                        else if (c.ChildLevelId && allowedIds.indexOf(String(c.ChildLevelId)) < 0) {
                            c.ChildLevelId = null; c.ChildCriticalLabelId = null; c.OperatorId = null;
                            c.CompareValue = ''; c.CompareValue2 = '';
                        }
                    }
                });
            });
        }

        function resetAsSelf(c) {
            c.ConditionSource = 'SELF'; c.FieldId = null; c.ChildLevelId = null;
            c.ChildCriticalLabelId = null; c.ChildMeasure = 'COUNT'; c.OperatorId = null;
            c.CompareValue = ''; c.CompareValue2 = ''; c.SelectedValues = []; c.ValuePickerOpen = false; c.ValueSearch = ''; c.ValueDropUp = false;
        }

        function sourceChanged(c) {
            c.FieldId = null; c.ChildLevelId = null; c.ChildCriticalLabelId = null; c.ChildMeasure = 'COUNT';
            c.OperatorId = null; c.CompareValue = ''; c.CompareValue2 = ''; c.SelectedValues = []; c.ValuePickerOpen = false; c.ValueSearch = ''; c.ValueDropUp = false;
        }

        function getField(c) {
            if (!c || !c.FieldId) return null;
            for (var i = 0; i < vm.masters.fields.length; i++)
                if (String(vm.masters.fields[i].id) === String(c.FieldId)) return vm.masters.fields[i];
            return null;
        }

        function dataType(c) { var f = getField(c); return f ? String(f.DataType || '').toUpperCase() : ''; }
        function sourceType(c) { var f = getField(c); return f ? String(f.SourceType || '').toUpperCase() : ''; }
        function isMasterField(c) { return c.ConditionSource === 'SELF' && dataType(c) === 'MASTER'; }
        function valueOptions(c) {
            if (sourceType(c) === 'PRIORITY') return vm.masters.priorities;
            if (sourceType(c) === 'STATUS') return vm.masters.statuses;
            return [];
        }

        function operatorSymbol(c) {
            if (!c || !c.OperatorId) return '';
            for (var i = 0; i < vm.masters.operators.length; i++)
                if (String(vm.masters.operators[i].id) === String(c.OperatorId))
                    return String(vm.masters.operators[i].OperatorSymbol || '').toUpperCase();
            return '';
        }

        function isMultiOperator(c) { var s = operatorSymbol(c); return s === 'IN' || s === 'NOT IN'; }

        function allowedOperators(c) {
            var masterAllowed = ['=', '!=', 'IN', 'NOT IN'];
            var numericAllowed = ['=', '!=', '>', '<', '>=', '<=', 'BETWEEN'];
            var allowed;
            if (!c) return [];
            if (c.ConditionSource === 'CHILD') {
                if (!c.ChildLevelId) return [];
                allowed = numericAllowed;
            } else {
                if (!c.FieldId) return [];
                allowed = isMasterField(c) ? masterAllowed : numericAllowed;
            }
            return vm.masters.operators.filter(function (x) {
                return allowed.indexOf(String(x.OperatorSymbol || '').toUpperCase()) >= 0;
            });
        }

        function fieldChanged(c) {
            c.CompareValue = ''; c.CompareValue2 = ''; c.SelectedValues = []; c.OperatorId = null;
            var ops = allowedOperators(c); if (ops.length) c.OperatorId = ops[0].id;
        }

        function childLevelChanged(c) {
            c.CompareValue = ''; c.CompareValue2 = ''; c.OperatorId = null;
            var ops = allowedOperators(c); if (ops.length) c.OperatorId = ops[0].id;
        }

        function childMeasureChanged(c) { c.CompareValue = ''; c.CompareValue2 = ''; }

        function operatorChanged(c) {
            c.CompareValue2 = '';
            c.ValuePickerOpen = false;
            c.ValueSearch = '';
            c.ValueDropUp = false;
            if (c.ConditionSource === 'SELF' && isMultiOperator(c)) {
                c.SelectedValues = c.SelectedValues || [];
                if (c.CompareValue && !c.SelectedValues.length)
                    c.SelectedValues = String(c.CompareValue).split(',').filter(function (x) { return x !== ''; });
            } else {
                if (c.ConditionSource === 'SELF' && (!c.CompareValue || c.CompareValue === '') && c.SelectedValues && c.SelectedValues.length)
                    c.CompareValue = String(c.SelectedValues[0]);
                c.SelectedValues = [];
            }
        }

        // ELSE condition is optional. This helper identifies only the untouched
        // blank placeholder row. If the user starts selecting any condition value,
        // normal condition validation still applies.
        function hasConditionInput(c) {
            if (!c) return false;

            if (String(c.ConditionSource || 'SELF').toUpperCase() === 'CHILD') return true;
            if (c.FieldId) return true;
            if (c.ChildLevelId || c.ChildCriticalLabelId) return true;
            if (c.OperatorId) return true;
            if (c.OpenBracket || c.CloseBracket) return true;
            if (c.SelectedValues && c.SelectedValues.length) return true;

            if (c.CompareValue !== '' && c.CompareValue !== null && typeof c.CompareValue !== 'undefined')
                return true;
            if (c.CompareValue2 !== '' && c.CompareValue2 !== null && typeof c.CompareValue2 !== 'undefined')
                return true;

            return false;
        }

        function validateCondition(c, branchNo, n) {
            if (!c.ConditionSource) c.ConditionSource = 'SELF';
            var prefix = 'Label ' + branchNo + ', Condition ' + n + ': ';
            if (c.ConditionSource === 'CHILD') {
                var allowedChildIds = childLevelsForRule().map(function (x) { return String(x.id); });
                if (!hasChildLevels()) return prefix + 'Sub Task cannot have Child conditions.';
                if (!c.ChildLevelId) return prefix + 'Depends On level is required.';
                if (allowedChildIds.indexOf(String(c.ChildLevelId)) < 0) return prefix + 'Selected dependency level is not valid.';
                if (!c.ChildCriticalLabelId) return prefix + 'Child Critical Label is required.';
                if (c.ChildMeasure !== 'COUNT' && c.ChildMeasure !== 'PERCENTAGE') return prefix + 'Child Measure is required.';
                if (!c.OperatorId) return prefix + 'Operator is required.';
                if (c.CompareValue === '' || c.CompareValue === null || typeof c.CompareValue === 'undefined') return prefix + 'Value is required.';
                if (isNaN(parseFloat(c.CompareValue)) || parseFloat(c.CompareValue) < 0) return prefix + 'Child value must be a valid number.';
                if (c.ChildMeasure === 'PERCENTAGE' && parseFloat(c.CompareValue) > 100) return prefix + 'Percentage cannot be greater than 100.';
            } else {
                if (!c.FieldId) return prefix + 'Field is required.';
                if (!c.OperatorId) return prefix + 'Operator is required.';
                if (isMultiOperator(c)) {
                    if (!c.SelectedValues || !c.SelectedValues.length) return prefix + 'Select at least one value.';
                } else if (c.CompareValue === '' || c.CompareValue === null || typeof c.CompareValue === 'undefined') {
                    return prefix + 'Value is required.';
                }
            }
            if (operatorSymbol(c) === 'BETWEEN' && (c.CompareValue2 === '' || c.CompareValue2 === null || typeof c.CompareValue2 === 'undefined'))
                return prefix + 'Second value is required for BETWEEN.';
            return '';
        }

        function validateAction(a, branchNo, n) {
            var prefix = 'Label ' + branchNo + ', Action ' + n + ': ';
            if (!a.ActionId) return prefix + 'Action is required.';
            var code = actionCode(a);
            if (code === 'SET_CRITICAL_LABEL') {
                if (!a.ActionValue) return prefix + 'Critical Label is required.';
            } else if (code === 'SEND_NOTIFICATION') {
                if (!a.SelectedRecipients || !a.SelectedRecipients.length) return prefix + 'Select at least one Recipient.';
                if (isRecipientSelected(a, 'CUSTOM') && (!a.SelectedEmployees || !a.SelectedEmployees.length))
                    return prefix + 'Select at least one custom employee.';
                if (!a.IsEmail && !a.IsWhatsApp && !a.IsBell)
                    return prefix + 'Select at least one Notification By option (Email / WhatsApp / Bell).';
            } else {
                return prefix + 'Unsupported action selected.';
            }
            return '';
        }

        function validateRule() {
            if (!vm.rule.RuleName || !String(vm.rule.RuleName).trim()) return 'Rule Name is required.';
            if (!vm.rule.LevelId) return 'Apply On is required.';
            if (!vm.rule.RulePriority || parseInt(vm.rule.RulePriority) < 1) return 'Rule Order must be 1 or greater.';
            if (!vm.rule.Branches || !vm.rule.Branches.length) return 'At least one IF label is required.';
            if (vm.rule.Branches[0].BranchType !== 'IF') return 'First label must be IF.';

            var seenElse = false;
            for (var i = 0; i < vm.rule.Branches.length; i++) {
                var b = vm.rule.Branches[i];
                if (i > 0 && b.BranchType === 'IF') return 'Only first label can be IF.';
                if (b.BranchType === 'ELSE') {
                    if (seenElse) return 'Only one ELSE label is allowed.';
                    seenElse = true;
                    if (i !== vm.rule.Branches.length - 1) return 'ELSE must be the last label.';
                } else if (seenElse) return 'No label can appear after ELSE.';

                // IF and ELSE IF require a condition.
                // ELSE is a fallback branch, so its condition is optional.
                // If the user starts configuring an ELSE condition, validate it normally.
                if (b.BranchType !== 'ELSE') {
                    if (!b.Conditions || !b.Conditions.length)
                        return 'Label ' + (i + 1) + ': At least one condition is required.';

                    for (var c = 0; c < b.Conditions.length; c++) {
                        var ce = validateCondition(b.Conditions[c], i + 1, c + 1);
                        if (ce) return ce;
                    }
                } else {
                    var elseHasAnyCondition = false;
                    angular.forEach(b.Conditions || [], function (condition) {
                        if (hasConditionInput(condition)) elseHasAnyCondition = true;
                    });

                    if (elseHasAnyCondition) {
                        for (var ec = 0; ec < b.Conditions.length; ec++) {
                            var elseCondition = b.Conditions[ec];

                            // Do not silently ignore an extra blank row when ELSE
                            // already contains a real condition.
                            if (!hasConditionInput(elseCondition))
                                return 'Label ' + (i + 1) + ', Condition ' + (ec + 1) + ': Complete or delete this condition.';

                            var ece = validateCondition(elseCondition, i + 1, ec + 1);
                            if (ece) return ece;
                        }
                    }
                }

                if (!b.Actions || !b.Actions.length) return 'Label ' + (i + 1) + ': At least one action is required.';
                for (var a = 0; a < b.Actions.length; a++) {
                    var ae = validateAction(b.Actions[a], i + 1, a + 1); if (ae) return ae;
                }
            }
            return '';
        }

        function prepareCondition(c, branchNo, branchType, index, conditions) {
            var out = angular.copy(c);
            out.BranchNo = branchNo;
            out.BranchType = branchType;
            out.SequenceNo = index + 1;
            out.LogicalOperatorId = index < conditions.length - 1 ? (conditions[index + 1].JoinOperatorId || out.JoinOperatorId || null) : null;
            var sym = operatorSymbol(c);
            if (out.ConditionSource === 'SELF') {
                out.ChildLevelId = null; out.ChildCriticalLabelId = null; out.ChildMeasure = '';
                if (sym === 'IN' || sym === 'NOT IN') { out.CompareValue = (out.SelectedValues || []).join(','); out.CompareValue2 = ''; }
                else {
                    if (out.CompareValue !== null && typeof out.CompareValue !== 'undefined') out.CompareValue = String(out.CompareValue);
                    if (out.CompareValue2 !== null && typeof out.CompareValue2 !== 'undefined') out.CompareValue2 = String(out.CompareValue2);
                }
            } else {
                out.FieldId = null; out.SelectedValues = [];
                if (out.CompareValue !== null && typeof out.CompareValue !== 'undefined') out.CompareValue = String(out.CompareValue);
                if (out.CompareValue2 !== null && typeof out.CompareValue2 !== 'undefined') out.CompareValue2 = String(out.CompareValue2);
            }
            delete out.ClientId; delete out.SelectedValues; delete out.JoinOperatorId;
            delete out.ValuePickerOpen; delete out.ValueSearch; delete out.ValueDropUp;
            return out;
        }

        function prepareAction(a, branchNo, branchType, index, recipientType) {
            var out = angular.copy(a);
            out.BranchNo = branchNo;
            out.BranchType = branchType;
            out.SequenceNo = index + 1;
            if (actionCode(a) === 'SEND_NOTIFICATION') {
                out.RecipientType = recipientType || '';
                out.RecipientValue = out.RecipientType === 'CUSTOM' ? (a.SelectedEmployees || []).join(',') : '';
                out.IsEmail = !!a.IsEmail;
                out.IsWhatsApp = !!a.IsWhatsApp;
                out.IsBell = !!a.IsBell;
            } else {
                out.RecipientType = '';
                out.RecipientValue = '';
                out.IsEmail = false;
                out.IsWhatsApp = false;
                out.IsBell = false;
            }
            delete out.ClientId;
            delete out.SelectedRecipients; delete out.RecipientPickerOpen; delete out.RecipientDropUp;
            delete out.SelectedEmployees; delete out.EmployeePickerOpen; delete out.EmployeeSearch; delete out.EmployeeDropUp;
            return out;
        }

        function saveRule() {
            var validation = validateRule();
            if (validation) { showPopupError(validation); return; }

            vm.saving = true;
            var isEdit = !!vm.rule.RuleId;
            var model = {
                RuleId: vm.rule.RuleId || 0,
                RuleName: vm.rule.RuleName,
                LevelId: vm.rule.LevelId,
                CriticalLabelId: null,
                PriorityId: null,
                RulePriority: vm.rule.RulePriority,
                StopProcessing: vm.rule.StopProcessing,
                Description: vm.rule.Description,
                Conditions: [],
                Actions: []
            };

            angular.forEach(vm.rule.Branches, function (b, bi) {
                var branchNo = bi + 1;

                // An ELSE branch may be action-only. Do not send its untouched
                // placeholder condition to SQL. Configured ELSE conditions are
                // still saved exactly as before.
                var conditionsToSave = (b.Conditions || []).filter(function (c) {
                    return b.BranchType !== 'ELSE' || hasConditionInput(c);
                });

                angular.forEach(conditionsToSave, function (c, ci) {
                    model.Conditions.push(prepareCondition(c, branchNo, b.BranchType, ci, conditionsToSave));
                });

                angular.forEach(b.Actions || [], function (a, ai) {
                    if (actionCode(a) === 'SEND_NOTIFICATION') {
                        // Keep the existing DB/service contract: one RecipientType per action row.
                        // A multi-recipient UI action is expanded into multiple ordinary action rows
                        // with the same SequenceNo, so existing workflow execution remains unchanged.
                        angular.forEach(a.SelectedRecipients || [], function (recipientType) {
                            model.Actions.push(prepareAction(a, branchNo, b.BranchType, ai, recipientType));
                        });
                    } else {
                        model.Actions.push(prepareAction(a, branchNo, b.BranchType, ai, ''));
                    }
                });
            });

            pagePost('SaveRule', { model: model }).then(function (res) {
                vm.saving = false;
                if (!res.success) { showPopupError(res.message); return; }
                closeRulePopup();
                showSuccess(isEdit ? 'Critical workflow updated successfully.' : 'Critical workflow saved successfully.');
                resetRule();
                loadRules();
            }).catch(function (err) {
                vm.saving = false;
                showPopupError(httpMessage(err));
            });
        }

        function resetRule() {
            vm.popupError = '';
            vm.newBranchType = 'ELSEIF';
            vm.rule = newRule();
        }

        function loadRules() {
            vm.rulesLoading = true;

            pagePost('GetRules', { levelId: vm.ruleFilterLevel || 0 }).then(function (res) {
                if (!res.success) {
                    showError(res.message);
                    return;
                }

                vm.rules = res.data || [];
                angular.forEach(vm.rules, function (r) {
                    r.status = r.status === true || r.status === 1 || r.status === '1';
                    r.StopProcessing = r.StopProcessing === true || r.StopProcessing === 1 || r.StopProcessing === '1';
                });
            }).catch(function (err) {
                showError(httpMessage(err));
            }).finally(function () {
                vm.rulesLoading = false;
            });
        }

        function findBranch(branches, branchNo, branchType) {
            for (var i = 0; i < branches.length; i++)
                if (branches[i].BranchNo === branchNo) return branches[i];
            var b = { ClientId: nextClientId('b'), BranchNo: branchNo, BranchType: branchType || (branchNo === 1 ? 'IF' : 'ELSEIF'), Conditions: [], Actions: [] };
            branches.push(b);
            return b;
        }

        function editRule(rule) {
            var ruleId = (rule && typeof rule === 'object') ? rule.id : rule;
            if (!ruleId) return;
            if (rule && typeof rule === 'object') {
                if (rule._editLoading) return;
                rule._editLoading = true;
            }

            pagePost('GetRule', { ruleId: ruleId }).then(function (res) {
                if (!res.success) { showError(res.message); return; }
                if (!res.rule || !res.rule.length) { showError('Workflow not found.'); return; }

                var h = res.rule[0];
                var branches = [];
                angular.forEach(res.conditions || [], function (c, index) {
                    var branchNo = parseInt(c.BranchNo || 1);
                    var branchType = String(c.BranchType || (branchNo === 1 ? 'IF' : 'ELSEIF')).toUpperCase();
                    var b = findBranch(branches, branchNo, branchType);
                    var source = String(c.ConditionSource || 'SELF').toUpperCase();
                    var isChild = source === 'CHILD';
                    var cond = {
                        ClientId: nextClientId('c'), SequenceNo: parseInt(c.SequenceNo || index + 1),
                        ConditionSource: isChild ? 'CHILD' : 'SELF', OpenBracket: c.OpenBracket || '',
                        FieldId: c.FieldId ? parseInt(c.FieldId) : null,
                        ChildLevelId: c.ChildLevelId ? parseInt(c.ChildLevelId) : null,
                        ChildCriticalLabelId: c.ChildCriticalLabelId ? parseInt(c.ChildCriticalLabelId) : null,
                        ChildMeasure: c.ChildMeasure || 'COUNT', OperatorId: c.OperatorId ? parseInt(c.OperatorId) : null,
                        CompareValue: c.CompareValue === null ? '' : (isChild || String(c.DataType || '').toUpperCase() === 'NUMBER' ? parseFloat(c.CompareValue) : String(c.CompareValue)),
                        CompareValue2: c.CompareValue2 === null ? '' : (isChild || String(c.DataType || '').toUpperCase() === 'NUMBER' ? parseFloat(c.CompareValue2) : String(c.CompareValue2)),
                        SelectedValues: [], ValuePickerOpen: false, ValueSearch: '', ValueDropUp: false,
                        LogicalOperatorId: c.LogicalOperatorId ? parseInt(c.LogicalOperatorId) : null,
                        JoinOperatorId: null, CloseBracket: c.CloseBracket || '', GroupNo: parseInt(c.GroupNo || 1)
                    };
                    var sym = String(c.OperatorSymbol || '').toUpperCase();
                    if (!isChild && (sym === 'IN' || sym === 'NOT IN')) {
                        cond.SelectedValues = c.CompareValue ? String(c.CompareValue).split(',') : [];
                        cond.CompareValue = '';
                    }
                    b.Conditions.push(cond);
                });

                // Existing SQL stores one RecipientType per action row. For the new UI,
                // notification rows having the same branch/action/sequence/channels are merged
                // back into one visual action with multiple selected recipients.
                var loadedNotificationActions = {};
                angular.forEach(res.actions || [], function (a, index) {
                    var branchNo = parseInt(a.BranchNo || 1);
                    var branchType = String(a.BranchType || (branchNo === 1 ? 'IF' : 'ELSEIF')).toUpperCase();
                    var b = findBranch(branches, branchNo, branchType);
                    var actionId = a.ActionId ? parseInt(a.ActionId) : null;
                    var sequenceNo = parseInt(a.SequenceNo || index + 1);
                    var storedRecipientTypes = a.RecipientType ? String(a.RecipientType).split(',') : [];
                    var code = actionCode({ ActionId: actionId });

                    if (code === 'SEND_NOTIFICATION') {
                        var groupKey = [
                            branchNo, branchType, actionId || '', sequenceNo,
                            a.ActionValue === null ? '' : String(a.ActionValue),
                            criticalBool(a.IsEmail) ? 1 : 0,
                            criticalBool(a.IsWhatsApp) ? 1 : 0,
                            criticalBool(a.IsBell) ? 1 : 0
                        ].join('|');

                        var uiAction = loadedNotificationActions[groupKey];
                        if (!uiAction) {
                            uiAction = {
                                ClientId: nextClientId('a'), ActionId: actionId,
                                ActionValue: a.ActionValue === null ? '' : String(a.ActionValue),
                                RecipientType: '', RecipientValue: '',
                                IsEmail: criticalBool(a.IsEmail), IsWhatsApp: criticalBool(a.IsWhatsApp), IsBell: criticalBool(a.IsBell),
                                SelectedRecipients: [], RecipientPickerOpen: false, RecipientDropUp: false,
                                SelectedEmployees: [], EmployeePickerOpen: false, EmployeeSearch: '', EmployeeDropUp: false,
                                SequenceNo: sequenceNo
                            };
                            loadedNotificationActions[groupKey] = uiAction;
                            b.Actions.push(uiAction);
                        }

                        angular.forEach(storedRecipientTypes, function (recipientType) {
                            var type = String(recipientType || '').trim().toUpperCase();
                            if (!type || isRecipientSelected(uiAction, type)) return;
                            uiAction.SelectedRecipients.push(type);
                        });

                        if (storedRecipientTypes.some(function (x) { return String(x || '').trim().toUpperCase() === 'CUSTOM'; }) && a.RecipientValue) {
                            angular.forEach(String(a.RecipientValue).split(','), function (empId) {
                                empId = String(empId || '').trim();
                                if (empId && !isEmployeeSelected(uiAction, empId)) uiAction.SelectedEmployees.push(empId);
                            });
                        }
                    } else {
                        b.Actions.push({
                            ClientId: nextClientId('a'), ActionId: actionId,
                            ActionValue: a.ActionValue === null ? '' : String(a.ActionValue),
                            RecipientType: '', RecipientValue: '',
                            IsEmail: false, IsWhatsApp: false, IsBell: false,
                            SelectedRecipients: [], RecipientPickerOpen: false, RecipientDropUp: false,
                            SelectedEmployees: [], EmployeePickerOpen: false, EmployeeSearch: '', EmployeeDropUp: false,
                            SequenceNo: sequenceNo
                        });
                    }
                });

                branches.sort(function (a, b) { return a.BranchNo - b.BranchNo; });
                if (!branches.length) branches.push(newBranch('IF'));
                branches[0].BranchType = 'IF';
                angular.forEach(branches, function (b, bi) {
                    if (b.BranchType === 'ELSE') {
                        // Remove old blank placeholder rows that may exist from
                        // earlier versions. Keep only actually configured ELSE conditions.
                        b.Conditions = (b.Conditions || []).filter(function (c) {
                            return hasConditionInput(c);
                        });
                    } else if (!b.Conditions.length) {
                        // Existing IF / ELSE IF behavior remains unchanged.
                        b.Conditions.push(newCondition());
                    }

                    if (!b.Actions.length) b.Actions.push(newAction());
                    angular.forEach(b.Conditions, function (c, ci) {
                        if (ci === 0) c.JoinOperatorId = vm.masters.logicalOperators.length ? vm.masters.logicalOperators[0].id : null;
                        else c.JoinOperatorId = b.Conditions[ci - 1].LogicalOperatorId || (vm.masters.logicalOperators.length ? vm.masters.logicalOperators[0].id : null);
                    });
                });

                vm.rule = {
                    RuleId: parseInt(h.id), RuleName: h.RuleName, LevelId: parseInt(h.LevelId), CriticalLabelId: null,
                    RulePriority: parseInt(h.RulePriority || 1),
                    StopProcessing: h.StopProcessing === true || h.StopProcessing === 1 || h.StopProcessing === '1',
                    Description: h.Description || '', Branches: branches
                };
                levelChanged();
                openRulePopup();
            }).catch(function (err) {
                showError(httpMessage(err));
            }).finally(function () {
                if (rule && typeof rule === 'object') rule._editLoading = false;
            });
        }

        function deleteRule(rule) {
            var ruleId = (rule && typeof rule === 'object') ? rule.id : rule;
            if (!ruleId) return;
            if (!window.confirm('Delete this critical workflow?')) return;

            if (rule && typeof rule === 'object') {
                if (rule._deleteLoading) return;
                rule._deleteLoading = true;
            }

            pagePost('DeleteRule', { ruleId: ruleId }).then(function (res) {
                if (!res.success) { showError(res.message); return; }
                showSuccess('Workflow deleted successfully.'); loadRules();
            }).catch(function (err) {
                showError(httpMessage(err));
            }).finally(function () {
                if (rule && typeof rule === 'object') rule._deleteLoading = false;
            });
        }

        function changeStatus(r) {
            pagePost('ChangeRuleStatus', { ruleId: r.id, status: !!r.status }).then(function (res) {
                if (!res.success) { showError(res.message); r.status = !r.status; return; }
                showSuccess('Workflow status updated.');
            }).catch(function (err) { r.status = !r.status; showError(httpMessage(err)); });
        }

        function httpMessage(err) {
            try {
                if (err && err.data) {
                    if (err.data.Message) return err.data.Message;
                    if (err.data.ExceptionMessage) return err.data.ExceptionMessage;
                    if (typeof err.data === 'string') return err.data;
                }
            } catch (e) { }
            return 'Server error.';
        }
    }
})();
</script>
</body>
</html>
