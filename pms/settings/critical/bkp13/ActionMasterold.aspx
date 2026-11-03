<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionMasterold.aspx.cs" Inherits="ActionMasterold" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" ng-app="actionApp">
<head runat="server">
    <title>PMS Action Master</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.3/angular.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <style>
        [ng-cloak] { display:none !important; }
        body { background:#f6f8fb; font-family:Arial,sans-serif; }
        .page-wrap { padding:22px; }
        .card-box { background:#fff; border:1px solid #e5e8ee; border-radius:12px; box-shadow:0 2px 9px rgba(0,0,0,.04); margin-bottom:20px; }
        .card-head { padding:16px 18px; border-bottom:1px solid #edf0f4; display:flex; justify-content:space-between; align-items:center; gap:10px; }
        .card-body-custom { padding:18px; }
        .section-title { font-size:18px; font-weight:700; margin:0; }
        .small-muted { font-size:12px; color:#78828c; }
        .table td,.table th { vertical-align:middle; }
        .badge-yes { background:#e8f7ee; color:#198754; border:1px solid #bfe5cd; padding:3px 8px; border-radius:12px; font-size:11px; font-weight:700; }
        .badge-no { background:#f1f3f5; color:#6c757d; border:1px solid #dee2e6; padding:3px 8px; border-radius:12px; font-size:11px; font-weight:700; }
        .status-active { color:#198754; font-weight:700; }
        .status-inactive { color:#dc3545; font-weight:700; }
        .modal-header { background:#f8fafc; }
        .required:after { content:' *'; color:#dc3545; }
        .switch-row { border:1px solid #e9ecef; border-radius:8px; padding:10px 12px; height:100%; }
        .form-check-input { cursor:pointer; }
    </style>
</head>
<body ng-controller="ActionController as vm" ng-cloak>
<form id="form1" runat="server">
<div class="page-wrap container-fluid">

    <div class="d-flex justify-content-between align-items-center flex-wrap gap-2 mb-3">
        <div>
            <h3 class="mb-1">Action Master</h3>
            <div class="text-muted">Maintain workflow actions such as Done, Rework, Back To Assign, Approve and Reject.</div>
        </div>
        <button type="button" class="btn btn-primary" ng-click="vm.openAdd()">+ Add Action</button>
    </div>

    <div class="alert alert-danger" ng-if="vm.error">{{vm.error}}</div>
    <div class="alert alert-success" ng-if="vm.success">{{vm.success}}</div>

    <div class="card-box">
        <div class="card-head">
            <div>
                <h4 class="section-title">Existing Actions</h4>
                <div class="small-muted mt-1">Edit or soft-delete workflow actions from this list.</div>
            </div>
            <div class="d-flex gap-2">
                <input type="text" class="form-control form-control-sm" style="width:220px" ng-model="vm.searchText" placeholder="Search action..." />
                <button type="button" class="btn btn-sm btn-outline-secondary" ng-click="vm.loadActions()">Refresh</button>
            </div>
        </div>

        <div class="card-body-custom">
            <div class="table-responsive">
                <table class="table table-hover table-bordered mb-0">
                    <thead class="table-light">
                        <tr>
                            <th style="width:65px">Sr.</th>
                            <th>Action Name</th>
                            <th>Action Code</th>
                            <th style="width:120px">Remark</th>
                            <th style="width:120px">Recipient</th>
                            <th style="width:125px">Department</th>
                            <th style="width:115px">Final</th>
                            <th style="width:85px">Order</th>
                            <th style="width:95px">Status</th>
                            <th style="width:160px">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="x in (vm.actions | filter:vm.searchText) track by x.id">
                            <td>{{$index + 1}}</td>
                            <td><strong>{{x.ActionName}}</strong></td>
                            <td><code>{{x.ActionCode}}</code></td>
                            <td><span ng-class="vm.toBool(x.RequiresRemark) ? 'badge-yes' : 'badge-no'">{{vm.toBool(x.RequiresRemark) ? 'Yes' : 'No'}}</span></td>
                            <td><span ng-class="vm.toBool(x.RequiresRecipient) ? 'badge-yes' : 'badge-no'">{{vm.toBool(x.RequiresRecipient) ? 'Yes' : 'No'}}</span></td>
                            <td><span ng-class="vm.toBool(x.RequiresDepartment) ? 'badge-yes' : 'badge-no'">{{vm.toBool(x.RequiresDepartment) ? 'Yes' : 'No'}}</span></td>
                            <td><span ng-class="vm.toBool(x.IsFinalAction) ? 'badge-yes' : 'badge-no'">{{vm.toBool(x.IsFinalAction) ? 'Yes' : 'No'}}</span></td>
                            <td>{{x.SortOrder}}</td>
                            <td>
                                <span ng-class="vm.toBool(x.status) ? 'status-active' : 'status-inactive'">
                                    {{vm.toBool(x.status) ? 'Active' : 'Inactive'}}
                                </span>
                            </td>
                            <td>
                                <button type="button" class="btn btn-sm btn-outline-primary me-1" ng-click="vm.openEdit(x)">Edit</button>
                                <button type="button" class="btn btn-sm btn-outline-danger" ng-click="vm.deleteAction(x)">Delete</button>
                            </td>
                        </tr>
                        <tr ng-if="!(vm.actions | filter:vm.searchText).length">
                            <td colspan="10" class="text-center text-muted py-4">No actions found.</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<!-- Add / Edit Popup -->
<div class="modal fade" id="actionModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <div>
                    <h5 class="modal-title mb-0">{{vm.editId > 0 ? 'Edit Action' : 'Add Action'}}</h5>
                    <div class="small-muted mt-1">Define how this action behaves in workflow configuration.</div>
                </div>
                <button type="button" class="btn-close" ng-click="vm.closeModal()"></button>
            </div>

            <div class="modal-body">
                <div class="row g-3">
                    <div class="col-md-6">
                        <label class="form-label required">Action Name</label>
                        <input type="text" maxlength="100" class="form-control" ng-model="vm.action.ActionName" ng-change="vm.syncCode()" placeholder="e.g. Rework" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label required">Action Code</label>
                        <input type="text" maxlength="100" class="form-control text-uppercase" ng-model="vm.action.ActionCode" placeholder="e.g. REWORK" />
                        <div class="small-muted mt-1">Unique technical code. Example: BACK_TO_ASSIGN</div>
                    </div>

                    <div class="col-md-3">
                        <label class="form-label">Sort Order</label>
                        <input type="number" min="0" class="form-control" ng-model="vm.action.SortOrder" />
                    </div>

                    <div class="col-md-3">
                        <div class="switch-row d-flex align-items-center justify-content-between mt-md-4">
                            <span>Active</span>
                            <div class="form-check form-switch m-0">
                                <input class="form-check-input" type="checkbox" ng-model="vm.action.IsActive" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="switch-row d-flex align-items-center justify-content-between mt-md-4">
                            <span>Final Action</span>
                            <div class="form-check form-switch m-0">
                                <input class="form-check-input" type="checkbox" ng-model="vm.action.IsFinalAction" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3"></div>

                    <div class="col-12"><hr class="my-1" /></div>

                    <div class="col-md-4">
                        <div class="switch-row d-flex align-items-center justify-content-between">
                            <div>
                                <div class="fw-semibold">Requires Remark</div>
                                <div class="small-muted">Show remark field</div>
                            </div>
                            <div class="form-check form-switch m-0">
                                <input class="form-check-input" type="checkbox" ng-model="vm.action.RequiresRemark" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="switch-row d-flex align-items-center justify-content-between">
                            <div>
                                <div class="fw-semibold">Requires Recipient</div>
                                <div class="small-muted">Owner / Team / Employee</div>
                            </div>
                            <div class="form-check form-switch m-0">
                                <input class="form-check-input" type="checkbox" ng-model="vm.action.RequiresRecipient" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="switch-row d-flex align-items-center justify-content-between">
                            <div>
                                <div class="fw-semibold">Requires Department</div>
                                <div class="small-muted">Show department selector</div>
                            </div>
                            <div class="form-check form-switch m-0">
                                <input class="form-check-input" type="checkbox" ng-model="vm.action.RequiresDepartment" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-light" ng-click="vm.closeModal()">Cancel</button>
                <button type="button" class="btn btn-primary" ng-click="vm.saveAction()" ng-disabled="vm.saving">
                    {{vm.saving ? 'Saving...' : (vm.editId > 0 ? 'Update Action' : 'Save Action')}}
                </button>
            </div>
        </div>
    </div>
</div>

</form>

<script>
(function () {
    'use strict';

    angular.module('actionApp', []).controller('ActionController', ActionController);
    ActionController.$inject = ['$http', '$timeout'];

    function ActionController($http, $timeout) {
        var vm = this;
        var modal = null;

        vm.actions = [];
        vm.action = newAction();
        vm.editId = 0;
        vm.searchText = '';
        vm.error = '';
        vm.success = '';
        vm.saving = false;

        vm.loadActions = loadActions;
        vm.openAdd = openAdd;
        vm.openEdit = openEdit;
        vm.closeModal = closeModal;
        vm.saveAction = saveAction;
        vm.deleteAction = deleteAction;
        vm.syncCode = syncCode;
        vm.toBool = toBool;

        angular.element(document).ready(function () {
            modal = new bootstrap.Modal(document.getElementById('actionModal'), {
                backdrop: 'static',
                keyboard: false
            });
        });

        loadActions();

        function pagePost(method, payload) {
            return $http({
                method: 'POST',
                url: 'ActionMaster.aspx/' + method,
                data: payload || {},
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            }).then(function (response) {
                var raw = response.data ? response.data.d : null;
                return raw ? angular.fromJson(raw) : {};
            });
        }

        function newAction() {
            return {
                id: 0,
                ActionName: '',
                ActionCode: '',
                RequiresRemark: false,
                RequiresRecipient: false,
                RequiresDepartment: false,
                IsFinalAction: false,
                SortOrder: 0,
                IsActive: true
            };
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

        function loadActions() {
            pagePost('GetActions', {}).then(function (res) {
                if (!res.success) { showError(res.message); return; }
                vm.actions = res.data || [];
            }).catch(function (err) {
                showError(httpMessage(err));
            });
        }

        function openAdd() {
            vm.editId = 0;
            vm.action = newAction();
            if (!modal) modal = new bootstrap.Modal(document.getElementById('actionModal'), { backdrop:'static', keyboard:false });
            modal.show();
        }

        function openEdit(x) {
            vm.editId = parseInt(x.id || 0);
            vm.action = {
                id: vm.editId,
                ActionName: x.ActionName || '',
                ActionCode: x.ActionCode || '',
                RequiresRemark: toBool(x.RequiresRemark),
                RequiresRecipient: toBool(x.RequiresRecipient),
                RequiresDepartment: toBool(x.RequiresDepartment),
                IsFinalAction: toBool(x.IsFinalAction),
                SortOrder: parseInt(x.SortOrder || 0),
                IsActive: toBool(x.status)
            };
            if (!modal) modal = new bootstrap.Modal(document.getElementById('actionModal'), { backdrop:'static', keyboard:false });
            modal.show();
        }

        function closeModal() {
            if (modal) modal.hide();
            vm.editId = 0;
            vm.action = newAction();
        }

        function syncCode() {
            if (vm.editId > 0) return;
            var name = String(vm.action.ActionName || '').trim();
            vm.action.ActionCode = name
                .replace(/[^a-zA-Z0-9]+/g, '_')
                .replace(/^_+|_+$/g, '')
                .toUpperCase();
        }

        function saveAction() {
            if (vm.saving) return;

            if (!vm.action.ActionName || !String(vm.action.ActionName).trim()) {
                showError('Action Name is required.');
                return;
            }

            if (!vm.action.ActionCode || !String(vm.action.ActionCode).trim()) {
                showError('Action Code is required.');
                return;
            }

            vm.action.ActionCode = String(vm.action.ActionCode)
                .trim()
                .replace(/\s+/g, '_')
                .toUpperCase();

            vm.saving = true;
            var editId = parseInt(vm.editId || 0);

            pagePost('SaveAction', { editId: editId, model: vm.action }).then(function (res) {
                vm.saving = false;
                if (!res.success) { showError(res.message); return; }

                var msg = (res.data && res.data.length && res.data[0].Message)
                    ? res.data[0].Message
                    : (editId > 0 ? 'Action updated successfully.' : 'Action saved successfully.');

                closeModal();
                showSuccess(msg);
                loadActions();
            }).catch(function (err) {
                vm.saving = false;
                showError(httpMessage(err));
            });
        }

        function deleteAction(x) {
            if (!x || !x.id) return;
            if (!window.confirm('Delete "' + (x.ActionName || 'this action') + '"?')) return;

            pagePost('DeleteAction', { id: parseInt(x.id) }).then(function (res) {
                if (!res.success) { showError(res.message); return; }

                if (res.data && res.data.length && parseInt(res.data[0].Result || 0) === 0) {
                    showError(res.data[0].Message || 'Action cannot be deleted.');
                    return;
                }

                showSuccess((res.data && res.data.length && res.data[0].Message)
                    ? res.data[0].Message
                    : 'Action deleted successfully.');
                loadActions();
            }).catch(function (err) {
                showError(httpMessage(err));
            });
        }

        function toBool(v) {
            return v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true';
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
