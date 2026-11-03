<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CriticalLabelMaster.aspx.cs" Inherits="CriticalLabelMaster" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" ng-app="labelApp">
<head runat="server">
    <title>Critical Label Master</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
     <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
   
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.3/angular.min.js"></script>
    <style>
        .tdp { padding:1px 2px  !important;}
        [ng-cloak] { display:none !important; }
        body { background:#f6f8fb; font-family:Arial,sans-serif; }
        .page-wrap { padding:22px; }
        .card-box { background:#fff; border:1px solid #e5e8ee; border-radius:12px; box-shadow:0 2px 9px rgba(0,0,0,.04); margin-bottom:20px; }
        .card-head { padding:3px 15px; border-bottom:1px solid #edf0f4; display:flex; justify-content:space-between; align-items:center; gap:10px; }
        .card-body-custom { padding:6px; }
        .section-title { font-size:18px; font-weight:700; margin:0; }
        .small-muted { font-size:12px; color:#78828c; }
        .badge-label { display:inline-block; color:#fff; border-radius:14px; padding:4px 10px; font-size:12px; font-weight:700; }
        .label-dot { width:18px; height:18px; border-radius:50%; border:1px solid #ccc; display:inline-block; vertical-align:middle; }
        .color-input { width:52px; height:38px; padding:2px; }
        .table td,.table th { vertical-align:middle; }
    </style>
</head>
<body ng-controller="LabelController as vm" ng-cloak>
<form id="form1" runat="server">
<div class="page-wrap container-fluid">
    <div class="d-flex justify-content-between align-items-center flex-wrap gap-2 mb-3">
        <div>
            <h3 class="mb-1">Critical Label Master</h3>
            <%--<div class="text-muted">Maintain Under Control / Moderate / High / Critical / Overdue labels separately from rule configuration.</div>--%>
        </div>
        <a href="CriticalSetting.aspx" class="btn btn-outline-secondary">Back to Workflow</a>
    </div>

    <div class="alert alert-danger" ng-if="vm.error">{{vm.error}}</div>
    <div class="alert alert-success" ng-if="vm.success">{{vm.success}}</div>

    <div class="card-box">
        <div class="card-head">
            <div>
                <h4 class="section-title">{{vm.editId > 0 ? 'Edit Label' : 'Add Label'}}</h4>
                <%--<div class="small-muted mt-1">A label cannot be deleted when it is used by a rule or child condition.</div>--%>
            </div>
            <button type="button" class="btn btn-sm btn-outline-secondary" ng-if="vm.editId > 0" ng-click="vm.cancelEdit()">Cancel Edit</button>
        </div>

        <div class="card-body-custom">
            <div class="row g-3">
                <div class="col-md-4">
                    <label class="form-label">Label Name</label>
                    <input type="text" class="form-control" ng-model="vm.label.CriticalLabel" placeholder="" />
                </div>

                <div class="col-md-3">
                    <label class="form-label">Color</label>
                    <div class="d-flex gap-2">
                        <input type="color" class="form-control form-control-color color-input" ng-model="vm.label.ColorCode" />
                        <input type="text" class="form-control" ng-model="vm.label.ColorCode" placeholder="#28a745" />
                    </div>
                </div>

                <div class="col-md-2" style="display:none">
                    <label class="form-label">Severity</label>
                    <input type="number" min="1" class="form-control" ng-model="vm.label.Severity" />
                </div>

                <div class="col-md-1" style="display:none">
                    <label class="form-label">Order</label>
                    <input type="number" min="0" class="form-control" ng-model="vm.label.DisplayOrder" />
                </div>

                <div class="col-md-2 d-flex align-items-end gap-2">
                    <button type="button" class="btn btn-primary flex-grow-1" ng-click="vm.saveLabel()">
                        {{vm.editId > 0 ? 'Update' : 'Save'}}
                    </button>
                    <button type="button" class="btn btn-light" ng-click="vm.reset()">Reset</button>
                </div>
            </div>

            <div class="mt-3">
                <span class="small-muted me-2">Preview:</span>
                <span class="badge-label" ng-style="{'background-color': vm.label.ColorCode || '#28a745'}">{{vm.label.CriticalLabel || 'Critical Label'}}</span>
            </div>
        </div>
    </div>

    <div class="card-box">
        <div class="card-head">
            <h4 class="section-title">Existing Critical Labels</h4>
            <div class="d-flex align-items-center gap-2">
                <div class="input-group input-group-sm" style="width:260px;">
                    <span class="input-group-text bg-white"><i class="bi bi-search"></i></span>
                    <input type="text"
                           class="form-control"
                           ng-model="vm.searchText"
                           placeholder="Search critical label..." />
                    <button type="button"
                            class="btn btn-outline-secondary"
                            ng-if="vm.searchText"
                            ng-click="vm.searchText=''">
                        <i class="bi bi-x-lg"></i>
                    </button>
                </div>
                <button type="button"
                        class="btn btn-sm btn-outline-secondary"
                        ng-click="vm.loadLabels()">
                    Refresh
                </button>
            </div>
        </div>

        <div class="card-body-custom">
            <div class="table-responsive">
                <table class="table table-hover table-bordered mb-0">
                    <thead class="table-light">
                        <tr>
                            <th style="width:70px">Sr No.</th>
                            <th>Label</th>
                            <th style="width:170px">Color</th>
                            <th style="width:110px;display:none">Severity</th>
                            <th style="width:100px;display:none">Order</th>
                            <th style="width:180px">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="x in (vm.labels | filter:vm.searchText)">
                            <td class="tdp">{{$index + 1}}</td>
                            <td class="tdp"><span class="badge-label" ng-style="{'background-color': x.ColorCode}">{{x.CriticalLabel}}</span></td>
                            <td class="tdp"><span class="label-dot me-2" ng-style="{'background-color': x.ColorCode}"></span>{{x.ColorCode}}</td>
                            <td style="display:none">{{x.Severity}}</td>
                            <td style="display:none">{{x.DisplayOrder}}</td>
                            <td class="tdp">
                                <button type="button" class="btn btn-sm btn-outline-primary me-1" ng-click="vm.editLabel(x)"><i class="bi bi-pencil-square"></i></button>
                                <button type="button" class="btn btn-sm btn-outline-danger" ng-click="vm.deleteLabel(x)"><i class="bi bi-trash"></i></button>
                            </td>
                        </tr>
                        <tr ng-if="(vm.labels | filter:vm.searchText).length === 0">
                            <td colspan="6" class="text-center text-muted py-4">No labels found.</td>
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

    angular.module('labelApp', []).controller('LabelController', LabelController);
    LabelController.$inject = ['$http', '$timeout'];

    function LabelController($http, $timeout) {
        var vm = this;
        vm.labels = [];
        vm.label = newLabel();
        vm.editId = 0;
        vm.error = '';
        vm.success = '';
        vm.searchText = '';

        vm.loadLabels = loadLabels;
        vm.saveLabel = saveLabel;
        vm.editLabel = editLabel;
        vm.deleteLabel = deleteLabel;
        vm.cancelEdit = cancelEdit;
        vm.reset = reset;

        loadLabels();

        function pagePost(method, payload) {
            return $http({
                method: 'POST',
                url: 'CriticalLabelMaster.aspx/' + method,
                data: payload || {},
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            }).then(function (response) {
                var raw = response.data ? response.data.d : null;
                return raw ? angular.fromJson(raw) : {};
            });
        }

        function newLabel() {
            return { id:0, CriticalLabel:'', ColorCode:'#28a745', Severity:1, DisplayOrder:0 };
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

        function loadLabels() {
            pagePost('GetLabels', {}).then(function (res) {
                if (!res.success) { showError(res.message); return; }
                vm.labels = res.data || [];
            }).catch(function (err) { showError(httpMessage(err)); });
        }

        function saveLabel() {
            if (!vm.label.CriticalLabel || !String(vm.label.CriticalLabel).trim()) {
                showError('Critical Label is required.');
                return;
            }

            var editId = parseInt(vm.editId || 0);
            pagePost('SaveLabel', { editId: editId, model: vm.label }).then(function (res) {
                if (!res.success) { showError(res.message); return; }
                showSuccess(editId > 0 ? 'Critical label updated successfully.' : 'Critical label saved successfully.');
                reset();
                loadLabels();
            }).catch(function (err) { showError(httpMessage(err)); });
        }

        function editLabel(x) {
            vm.editId = parseInt(x.id || 0);
            vm.label = {
                id: parseInt(x.id || 0),
                CriticalLabel: x.CriticalLabel || '',
                ColorCode: x.ColorCode || '#28a745',
                Severity: parseInt(x.Severity || 1),
                DisplayOrder: parseInt(x.DisplayOrder || 0)
            };
            window.scrollTo({ top:0, behavior:'smooth' });
        }

        function deleteLabel(x) {
            if (!x || !x.id) return;
            if (!window.confirm('Delete "' + (x.CriticalLabel || 'this label') + '"?\n\nIf it is used in any rule, deletion will be blocked.')) return;

            pagePost('DeleteLabel', { id: parseInt(x.id) }).then(function (res) {
                if (!res.success) { showError(res.message); return; }
                if (res.data && res.data.length && parseInt(res.data[0].Result || 0) === 0) {
                    showError(res.data[0].Message || 'Critical label cannot be deleted.');
                    return;
                }
                showSuccess((res.data && res.data.length && res.data[0].Message) ? res.data[0].Message : 'Critical label deleted successfully.');
                if (vm.editId === parseInt(x.id)) reset();
                loadLabels();
            }).catch(function (err) { showError(httpMessage(err)); });
        }

        function cancelEdit() { reset(); }

        function reset() {
            vm.label = newLabel();
            vm.editId = 0;
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
