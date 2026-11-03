<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActionMaster.aspx.cs" Inherits="ActionMaster" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" ng-app="ActionMasterApp">
<head runat="server">
    <title>Action Master</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
        <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.3/angular.min.js"></script>
   <style>
       .tdp { padding:1px 2px  !important;}
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
    <style>
        [ng-cloak] { display:none !important; }
        body { background:#f6f8fb; font-family:Arial,sans-serif; }
        .page-wrap { padding:22px; }
        .card-box {
            background:#fff;
            border:1px solid #e5e8ee;
            border-radius:12px;
            box-shadow:0 2px 9px rgba(0,0,0,.04);
            margin-bottom:20px;
        }
        .card-head {
            padding:16px 18px;
            border-bottom:1px solid #edf0f4;
        }
        .card-body-custom { padding:18px; }
        .section-title { font-size:18px; font-weight:700; margin:0; }
        .table td,.table th { vertical-align:middle; }
        .master-form { max-width:720px; }
    </style>
</head>

<body ng-controller="ActionMasterController as vm" ng-cloak>
<form id="form1" runat="server">

<div class="page-wrap container-fluid">

    <div class="d-flex justify-content-between align-items-center flex-wrap gap-2 mb-3">
        <div>
            <h3 class="mb-1">Action Master</h3>
            <div class="text-muted">Manage workflow action master.</div>
        </div>
        <a href="WorkflowSetting.aspx" class="btn btn-outline-secondary">
            Back to Workflow
        </a>
    </div>

    <div class="alert alert-danger" ng-if="vm.error">{{vm.error}}</div>
    <div class="alert alert-success" ng-if="vm.success">{{vm.success}}</div>

    <!-- Only one input: Master Name -->
    <div class="card-box">
        <div class="card-head">
            <h4 class="section-title">{{vm.editId > 0 ? 'Update Action' : 'Create Action'}}</h4>
        </div>

        <div class="card-body-custom">
            <div class="row">
                <div class="col-6">
            <label class="form-label">Action Name</label>

            <div class="d-flex gap-2">
                <input type="text"
                       class="form-control"
                       maxlength="150"
                       ng-model="vm.masterName"
                       ng-keydown="vm.keyDown($event)"
                       placeholder="Enter action name" />

                <button type="button"
                        class="btn btn-primary"
                        ng-click="vm.saveMaster()"
                        ng-disabled="vm.saving">
                    {{vm.saving ? 'Saving...' : (vm.editId > 0 ? 'Update' : 'Save')}}
                </button>

                <button type="button"
                        class="btn btn-light"
                        ng-if="vm.editId > 0"
                        ng-click="vm.cancelEdit()">
                    Cancel
                </button>
                </div>
            </div>
                </div>
        </div>
    </div>

    <div class="card-box">
        <div class="card-head d-flex justify-content-between align-items-center flex-wrap gap-2">
            <h4 class="section-title">Action Master List</h4>

            <div class="d-flex align-items-center gap-2">
                <div class="input-group input-group-sm" style="width:260px;">
                    <span class="input-group-text bg-white">
                        <i class="bi bi-search"></i>
                    </span>
                    <input type="text"
                           class="form-control"
                           ng-model="vm.searchText"
                           placeholder="Search action..." />
                    <button type="button"
                            class="btn btn-outline-secondary"
                            ng-if="vm.searchText"
                            ng-click="vm.searchText=''"
                            title="Clear search">
                        <i class="bi bi-x-lg"></i>
                    </button>
                </div>

                <button type="button"
                        class="btn btn-sm btn-outline-secondary"
                        ng-click="vm.loadMasters()">
                    Refresh
                </button>
            </div>
        </div>

        <div class="card-body-custom">
            <div class="table-responsive">
                <table class="table table-hover table-bordered mb-0">
                    <thead class="table-light">
                        <tr>
                            <th style="width:80px">Sr. No.</th>
                            <th>Name</th>
                            <%--<th style="width:190px">Date</th>--%>
                            <th style="width:180px">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="x in filteredMasters = (vm.masters | filter:{MasterName:vm.searchText}) track by x.id">
                            <td class="tdp">{{$index + 1}}</td>
                            <td class="tdp">{{x.MasterName}}</td>
                            <!-- DisplayDate is already formatted on SQL side.
                                 Do not pass it through JavaScript Date(), so browser timezone cannot shift it. -->
                            <%--<td>{{x.DisplayDate}}</td>--%>
                            <td class="tdp">
                                <button type="button"
                                        class="btn btn-sm btn-outline-primary me-1"
                                        ng-click="vm.editMaster(x)">
                                   <i class="bi bi-pencil-square"></i>
                                </button>

                                <button type="button"
                                        class="btn btn-sm btn-outline-danger"
                                        ng-click="vm.deleteMaster(x)">
                                     <i class="bi bi-trash"></i>
                                </button>
                            </td>
                        </tr>

                        <tr ng-if="!filteredMasters.length">
                            <td colspan="4" class="text-center text-muted py-4">
                                {{vm.searchText ? 'No matching action found.' : 'No master found.'}}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

</div>
</form>

<script>
(function () {
    'use strict';

    angular.module('ActionMasterApp', [])
        .controller('ActionMasterController', ActionMasterController);

    ActionMasterController.$inject = ['$http', '$timeout'];

    function ActionMasterController($http, $timeout) {
        var vm = this;

        vm.masterName = '';
        vm.editId = 0;
        vm.masters = [];
        vm.searchText = '';
        vm.error = '';
        vm.success = '';
        vm.saving = false;

        vm.loadMasters = loadMasters;
        vm.saveMaster = saveMaster;
        vm.editMaster = editMaster;
        vm.deleteMaster = deleteMaster;
        vm.cancelEdit = cancelEdit;
        vm.keyDown = keyDown;

        loadMasters();

        function pagePost(method, payload) {
            return $http({
                method: 'POST',
                url: 'ActionMaster.aspx/' + method,
                data: payload || {},
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                }
            }).then(function (response) {
                var raw = response.data ? response.data.d : null;
                return raw ? angular.fromJson(raw) : {};
            });
        }

        function showSuccess(msg) {
            vm.success = msg || 'Saved successfully.';
            vm.error = '';

            $timeout(function () {
                vm.success = '';
            }, 3500);
        }

        function showError(msg) {
            vm.error = msg || 'Something went wrong.';
            vm.success = '';

            $timeout(function () {
                vm.error = '';
            }, 5000);
        }

        function loadMasters() {
            pagePost('GetMasters', {})
                .then(function (res) {
                    if (!res.success) {
                        showError(res.message);
                        return;
                    }

                    vm.masters = res.data || [];
                })
                .catch(function (err) {
                    showError(httpMessage(err));
                });
        }

        function saveMaster() {
            if (vm.saving) return;

            var name = String(vm.masterName || '').trim();

            if (!name) {
                showError('Master Name is required.');
                return;
            }

            vm.saving = true;
            var editId = parseInt(vm.editId || 0);

            pagePost('SaveMaster', {
                editId: editId,
                masterName: name
            })
            .then(function (res) {
                vm.saving = false;

                if (!res.success) {
                    showError(res.message);
                    return;
                }

                var message = editId > 0
                    ? 'Master updated successfully.'
                    : 'Master saved successfully.';

                if (res.data && res.data.length && res.data[0].Message) {
                    message = res.data[0].Message;
                }

                resetForm();
                showSuccess(message);
                loadMasters();
            })
            .catch(function (err) {
                vm.saving = false;
                showError(httpMessage(err));
            });
        }

        function editMaster(x) {
            if (!x) return;

            vm.editId = parseInt(x.id || 0);
            vm.masterName = x.MasterName || '';
            window.scrollTo({ top: 0, behavior: 'smooth' });
        }

        function deleteMaster(x) {
            if (!x || !x.id) return;

            if (!window.confirm('Delete "' + (x.MasterName || 'this master') + '"?')) {
                return;
            }

            pagePost('DeleteMaster', {
                id: parseInt(x.id)
            })
            .then(function (res) {
                if (!res.success) {
                    showError(res.message);
                    return;
                }

                var message = 'Master deleted successfully.';

                if (res.data && res.data.length && res.data[0].Message) {
                    message = res.data[0].Message;
                }

                if (vm.editId === parseInt(x.id)) {
                    resetForm();
                }

                showSuccess(message);
                loadMasters();
            })
            .catch(function (err) {
                showError(httpMessage(err));
            });
        }

        function cancelEdit() {
            resetForm();
        }

        function resetForm() {
            vm.masterName = '';
            vm.editId = 0;
        }

        function keyDown(e) {
            if (e && e.keyCode === 13) {
                e.preventDefault();
                saveMaster();
            }
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
