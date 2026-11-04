(function () {
    'use strict';

    // PMS Cost Control (Phase 1) - AngularJS 1.x controller.
    // Server methods live in /pms/costcontrol/default.aspx.cs.
    angular.module('pmsCostControlApp', [])
        .controller('CostControlController', ['$scope', '$http', function ($scope, $http) {
            var STORAGE_KEY = 'pmsCostControl.selection';

            $scope.tab = 'budget';
            $scope.busy = false;
            $scope.error = '';
            $scope.projects = [];
            $scope.costCodes = [];
            $scope.costTypes = ['Material', 'Labour', 'Subcontract', 'Equipment', 'Overhead', 'Other'];
            $scope.selection = { projectId: null };
            $scope.structure = { milestones: [], tasks: [] };
            $scope.budget = { versions: [], versionId: null, lines: [] };
            $scope.actuals = { entries: [], search: '' };
            $scope.report = { groupBy: 'COSTCODE', rows: [], header: null };
            $scope.codes = { search: '' };
            $scope.modal = { type: null, data: {}, error: '' };

            // ---------- server call helper ----------
            function call(method, payload) {
                return $http.post('default.aspx/' + method, payload || {}).then(function (response) {
                    var result = response && response.data ? response.data.d : null;
                    if (!result) throw new Error('No response from the server.');
                    if (result.Success === false) throw new Error(result.Message || 'The request failed.');
                    return result;
                }, function (response) {
                    var message = response && response.data && response.data.Message ? response.data.Message : 'The server request failed.';
                    throw new Error(message);
                });
            }
            function showError(err) { $scope.error = err && err.message ? err.message : String(err); }
            function toNumber(value) { var n = parseFloat(value); return isNaN(n) ? null : n; }
            function toIntOrNull(value) { var n = parseInt(value, 10); return isNaN(n) || n <= 0 ? null : n; }
            function pad(n) { return n < 10 ? '0' + n : '' + n; }
            // Business date only: build yyyy-MM-dd from the local calendar date (no time-zone shift).
            function toDateText(date) { return date ? date.getFullYear() + '-' + pad(date.getMonth() + 1) + '-' + pad(date.getDate()) : ''; }
            function fromDateText(text) {
                var parts = (text || '').split('-');
                return parts.length === 3 ? new Date(parseInt(parts[0], 10), parseInt(parts[1], 10) - 1, parseInt(parts[2], 10)) : null;
            }
            function saveSelection() {
                try { window.localStorage.setItem(STORAGE_KEY, JSON.stringify({ projectId: $scope.selection.projectId, tab: $scope.tab })); } catch (e) { }
            }
            function readSelection() {
                try { return JSON.parse(window.localStorage.getItem(STORAGE_KEY) || '{}') || {}; } catch (e) { return {}; }
            }

            // ---------- generic helpers used by the view ----------
            $scope.sum = function (rows, field) {
                var total = 0;
                angular.forEach(rows || [], function (row) { total += toNumber(row[field]) || 0; });
                return Math.round(total * 100) / 100;
            };
            $scope.totalConsumed = function () {
                var budget = $scope.sum($scope.report.rows, 'CurrentBudget');
                if (!budget) return '-';
                return (Math.round($scope.sum($scope.report.rows, 'Actual') * 10000 / budget) / 100).toFixed(2) + ' %';
            };
            $scope.tasksFor = function (milestoneId) {
                return ($scope.structure.tasks || []).filter(function (t) { return milestoneId && t.MilestoneId === milestoneId; });
            };
            $scope.versionLabel = function (v) {
                return 'V' + v.VersionNo + ' - ' + v.VersionType + ' (' + v.VersionStatus + ')';
            };
            $scope.currentVersion = function () {
                for (var i = 0; i < $scope.budget.versions.length; i++)
                    if ($scope.budget.versions[i].id === $scope.budget.versionId) return $scope.budget.versions[i];
                return null;
            };
            $scope.isDraft = function () { var v = $scope.currentVersion(); return !!v && v.VersionStatus === 'Draft'; };
            $scope.hasDraft = function () {
                return $scope.budget.versions.some(function (v) { return v.VersionStatus === 'Draft'; });
            };
            function matches(row, text, fields) {
                if (!text) return true;
                text = text.toLowerCase();
                return fields.some(function (f) { return String(row[f] == null ? '' : row[f]).toLowerCase().indexOf(text) >= 0; });
            }
            $scope.filteredEntries = function () {
                return $scope.actuals.entries.filter(function (e) {
                    return matches(e, $scope.actuals.search, ['EntryDate', 'CostCode', 'CostCodeName', 'MilestoneName', 'TaskName', 'ReferenceNo', 'Remarks']);
                });
            };
            $scope.filteredCodes = function () {
                return $scope.costCodes.filter(function (c) { return matches(c, $scope.codes.search, ['Code', 'Name', 'CostType', 'Description']); });
            };

            // ---------- loading ----------
            function loadLookups() {
                return call('GetLookups').then(function (result) {
                    $scope.projects = result.Projects || [];
                    $scope.costCodes = result.CostCodes || [];
                });
            }
            function loadStructure() {
                $scope.structure = { milestones: [], tasks: [] };
                if (!$scope.selection.projectId) return;
                return call('GetProjectStructure', { projectId: $scope.selection.projectId }).then(function (result) {
                    $scope.structure = { milestones: result.Milestones || [], tasks: result.Tasks || [] };
                });
            }
            $scope.loadBudget = function () {
                if (!$scope.selection.projectId) return;
                $scope.error = '';
                return call('GetBudget', { projectId: $scope.selection.projectId, versionId: $scope.budget.versionId || 0 }).then(function (result) {
                    $scope.budget.versions = result.Versions || [];
                    $scope.budget.lines = result.Lines || [];
                    if (!$scope.currentVersion()) $scope.budget.versionId = $scope.budget.versions.length ? $scope.budget.versions[0].id : null;
                }).catch(showError);
            };
            function loadEntries() {
                if (!$scope.selection.projectId) return;
                return call('GetCostEntries', { projectId: $scope.selection.projectId }).then(function (result) {
                    $scope.actuals.entries = result.Entries || [];
                }).catch(showError);
            }
            $scope.loadReport = function () {
                if (!$scope.selection.projectId) return;
                $scope.error = '';
                return call('GetBudgetVsActual', { projectId: $scope.selection.projectId, groupBy: $scope.report.groupBy }).then(function (result) {
                    $scope.report.rows = result.Rows || [];
                    $scope.report.header = result.Header || null;
                }).catch(showError);
            };
            function refreshTab() {
                if ($scope.tab === 'budget') return $scope.loadBudget();
                if ($scope.tab === 'actuals') return loadEntries();
                if ($scope.tab === 'report') return $scope.loadReport();
            }

            $scope.openTab = function (tab) {
                $scope.tab = tab;
                $scope.error = '';
                saveSelection();
                refreshTab();
            };
            $scope.projectChanged = function () {
                $scope.error = '';
                $scope.budget = { versions: [], versionId: null, lines: [] };
                $scope.actuals.entries = [];
                $scope.report.rows = [];
                $scope.report.header = null;
                saveSelection();
                var structure = loadStructure();
                if (structure && structure.catch) structure.catch(showError);
                refreshTab();
            };

            // ---------- modal handling ----------
            $scope.closeModal = function () { if (!$scope.busy) $scope.modal = { type: null, data: {}, error: '' }; };
            function openModal(type, data) { $scope.modal = { type: type, data: data, error: '' }; }

            $scope.openNewVersion = function () { openModal('version', { reason: '' }); };
            $scope.openBudgetLine = function (line) {
                line = line || {};
                openModal('line', {
                    Id: line.id || 0, CostCodeId: line.CostCodeId || null,
                    MilestoneId: line.MilestoneId || null, TaskId: line.TaskId || null,
                    Quantity: toNumber(line.Quantity), Unit: line.Unit || '', Rate: toNumber(line.Rate),
                    Amount: toNumber(line.Amount), Remarks: line.Remarks || ''
                });
            };
            $scope.recalculateLineAmount = function () {
                var d = $scope.modal.data;
                if (d.Quantity != null && d.Rate != null) d.Amount = Math.round(d.Quantity * d.Rate * 100) / 100;
            };
            $scope.openCostEntry = function (entry) {
                entry = entry || {};
                openModal('entry', {
                    Id: entry.id || 0, CostCodeId: entry.CostCodeId || null,
                    MilestoneId: entry.MilestoneId || null, TaskId: entry.TaskId || null,
                    EntryDateValue: entry.EntryDate ? fromDateText(entry.EntryDate) : new Date(),
                    Amount: toNumber(entry.Amount), ReferenceNo: entry.ReferenceNo || '', Remarks: entry.Remarks || ''
                });
            };
            $scope.openCostCode = function (code) {
                code = code || {};
                openModal('code', { Id: code.id || 0, Code: code.Code || '', Name: code.Name || '', CostType: code.CostType || null, Description: code.Description || '' });
            };

            function validateModal() {
                var d = $scope.modal.data;
                switch ($scope.modal.type) {
                    case 'version':
                        if ($scope.budget.versions.length && !(d.reason || '').trim()) return 'A reason is required for a revision.';
                        break;
                    case 'line':
                        if (!d.CostCodeId) return 'Select a cost code.';
                        if (d.Amount == null || d.Amount < 0) return 'Enter a budget amount of zero or more.';
                        break;
                    case 'entry':
                        if (!d.EntryDateValue) return 'Enter the cost date.';
                        if (!d.CostCodeId) return 'Select a cost code.';
                        if (!d.Amount) return 'Enter a non-zero amount.';
                        break;
                    case 'code':
                        if (!(d.Code || '').trim() || !(d.Name || '').trim()) return 'Code and name are required.';
                        if (!d.CostType) return 'Select a cost type.';
                        break;
                }
                return '';
            }

            $scope.saveModal = function () {
                var message = validateModal();
                if (message) { $scope.modal.error = message; return; }
                var d = $scope.modal.data, request, after;
                switch ($scope.modal.type) {
                    case 'version':
                        request = call('CreateBudgetVersion', { projectId: $scope.selection.projectId, reason: d.reason || '' });
                        after = function (result) { $scope.budget.versionId = result.Id; return $scope.loadBudget(); };
                        break;
                    case 'line':
                        request = call('SaveBudgetLine', { request: {
                            Id: d.Id, VersionId: $scope.budget.versionId, MilestoneId: toIntOrNull(d.MilestoneId), TaskId: toIntOrNull(d.TaskId),
                            CostCodeId: d.CostCodeId, Quantity: d.Quantity, Unit: d.Unit, Rate: d.Rate, Amount: d.Amount, Remarks: d.Remarks } });
                        after = function () { return $scope.loadBudget(); };
                        break;
                    case 'entry':
                        request = call('SaveCostEntry', { request: {
                            Id: d.Id, ProjectId: $scope.selection.projectId, MilestoneId: toIntOrNull(d.MilestoneId), TaskId: toIntOrNull(d.TaskId),
                            CostCodeId: d.CostCodeId, EntryDate: toDateText(d.EntryDateValue), Amount: d.Amount, ReferenceNo: d.ReferenceNo, Remarks: d.Remarks } });
                        after = loadEntries;
                        break;
                    case 'code':
                        request = call('SaveCostCode', { request: { Id: d.Id, Code: d.Code, Name: d.Name, CostType: d.CostType, Description: d.Description } });
                        after = loadLookups;
                        break;
                }
                $scope.busy = true;
                request.then(function (result) {
                    $scope.busy = false;
                    $scope.closeModal();
                    return after(result);
                }).catch(function (err) {
                    $scope.busy = false;
                    $scope.modal.error = err.message;
                });
            };

            function confirmAndRun(question, method, payload, after) {
                if (!window.confirm(question)) return;
                $scope.busy = true;
                $scope.error = '';
                call(method, payload).then(function () {
                    $scope.busy = false;
                    return after();
                }).catch(function (err) { $scope.busy = false; showError(err); });
            }
            $scope.lockVersion = function () {
                confirmAndRun('Lock this budget? A locked budget cannot be edited; later changes need a revision.',
                    'LockBudgetVersion', { versionId: $scope.budget.versionId }, $scope.loadBudget);
            };
            $scope.discardDraft = function () {
                confirmAndRun('Discard this draft budget and all its lines?', 'DiscardDraftVersion', { versionId: $scope.budget.versionId },
                    function () { $scope.budget.versionId = null; return $scope.loadBudget(); });
            };
            $scope.deleteBudgetLine = function (line) {
                confirmAndRun('Delete this budget line?', 'DeleteBudgetLine', { budgetLineId: line.id }, $scope.loadBudget);
            };
            $scope.deleteCostEntry = function (entry) {
                confirmAndRun('Delete this cost entry?', 'DeleteCostEntry', { costEntryId: entry.id }, loadEntries);
            };
            $scope.deleteCostCode = function (code) {
                confirmAndRun('Delete cost code ' + code.Code + '?', 'DeleteCostCode', { costCodeId: code.id }, loadLookups);
            };

            // ---------- start ----------
            var saved = readSelection();
            if (saved.tab) $scope.tab = saved.tab;
            loadLookups().then(function () {
                var exists = $scope.projects.some(function (p) { return p.id === saved.projectId; });
                if (exists) { $scope.selection.projectId = saved.projectId; $scope.projectChanged(); }
            }).catch(showError);
        }]);
})();
