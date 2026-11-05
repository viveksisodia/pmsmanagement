(function () {
    'use strict';

    // PMS Cost Control (Phase 1) - AngularJS 1.x controller.
    // Server methods live in /pms/costcontrol/default.aspx.cs.
    angular.module('pmsCostControlApp', [])
        /*
         * <cc-search-select> - searchable drop-down used by every list on this page.
         *   ng-model       selected value (works with ng-change)
         *   items          array of objects
         *   value-key      property holding the value      (default "id")
         *   label-key      property holding the text       (default "Name")
         *   placeholder    text when nothing is selected
         *   empty-label    optional first choice that clears the value (e.g. "Project level")
         *   add-new-label  optional LAST choice, e.g. "+ Add new unit"; calls on-add-new()
         *   is-disabled    expression
         * Keyboard: Enter/Space/ArrowDown opens, ArrowUp/ArrowDown move, Enter selects, Esc closes.
         */
        .directive('ccSearchSelect', ['$document', '$timeout', function ($document, $timeout) {
            return {
                restrict: 'E',
                require: 'ngModel',
                scope: {
                    items: '<', valueKey: '@', labelKey: '@', placeholder: '@',
                    emptyLabel: '@', addNewLabel: '@', onAddNew: '&', isDisabled: '<'
                },
                template:
                    '<div class="cc-ss" ng-class="{\'cc-ss-open\':open,\'cc-ss-disabled\':isDisabled}">' +
                    '<button type="button" class="cc-ss-trigger" ng-click="toggle()" ng-disabled="isDisabled" ng-keydown="triggerKey($event)" aria-haspopup="listbox" aria-expanded="{{open}}">' +
                    '<span class="cc-ss-text" ng-class="{\'cc-ss-placeholder\':!currentLabel}">{{currentLabel || placeholder || emptyLabel || \'-- Select --\'}}</span>' +
                    '<span class="cc-ss-arrow" aria-hidden="true">&#9662;</span></button>' +
                    '<div class="cc-ss-popup" ng-if="open">' +
                    '<input type="text" class="cc-ss-search" ng-model="state.search" ng-change="rebuild(true)" ng-keydown="searchKey($event)" placeholder="Type to search..." autocomplete="off" aria-label="Type to search" />' +
                    '<div class="cc-ss-list" role="listbox">' +
                    '<button type="button" role="option" class="cc-ss-option" ng-repeat="entry in entries track by $index" ' +
                    'ng-class="{\'cc-ss-active\':$index===state.active,\'cc-ss-selected\':entry.selected,\'cc-ss-addnew\':entry.kind===\'add\',\'cc-ss-emptyopt\':entry.kind===\'empty\'}" ' +
                    'ng-mouseenter="state.active=$index" ng-click="choose(entry)">{{entry.label}}</button>' +
                    '<div class="cc-ss-none" ng-if="!hasMatches">No matching records</div>' +
                    '</div></div></div>',
                link: function (scope, element, attrs, ngModel) {
                    var valueKey = scope.valueKey || 'id', labelKey = scope.labelKey || 'Name';
                    scope.open = false;
                    scope.state = { search: '', active: 0 };
                    scope.entries = [];
                    scope.currentLabel = '';

                    function same(a, b) { return a !== null && a !== undefined && b !== null && b !== undefined && String(a) === String(b); }
                    function refreshLabel() {
                        var value = ngModel.$viewValue, label = '';
                        angular.forEach(scope.items || [], function (item) { if (!label && same(item[valueKey], value)) label = item[labelKey]; });
                        scope.currentLabel = label;
                    }
                    // Entries are rebuilt only when items/search change, so the view sees a stable array.
                    scope.rebuild = function (resetActive) {
                        var text = (scope.state.search || '').toLowerCase().trim(), list = [], value = ngModel.$viewValue;
                        if (scope.emptyLabel && !text) list.push({ kind: 'empty', label: scope.emptyLabel, value: null, selected: value === null || value === undefined || value === '' });
                        angular.forEach(scope.items || [], function (item) {
                            var label = String(item[labelKey] == null ? '' : item[labelKey]);
                            if (!text || label.toLowerCase().indexOf(text) >= 0)
                                list.push({ kind: 'item', label: label, value: item[valueKey], selected: same(item[valueKey], value) });
                        });
                        scope.hasMatches = list.some(function (e) { return e.kind === 'item'; }) || (!text && list.length > 0);
                        if (scope.addNewLabel) list.push({ kind: 'add', label: scope.addNewLabel });
                        scope.entries = list;
                        if (resetActive) scope.state.active = 0;
                    };
                    function scrollActiveIntoView() {
                        $timeout(function () {
                            var active = element[0].querySelector('.cc-ss-active');
                            if (active && active.scrollIntoView) active.scrollIntoView({ block: 'nearest' });
                        }, 0, false);
                    }
                    scope.openPopup = function () {
                        if (scope.isDisabled) return;
                        scope.open = true;
                        scope.state.search = '';
                        scope.rebuild(false);
                        var selectedIndex = -1;
                        scope.entries.forEach(function (e, i) { if (selectedIndex < 0 && e.selected) selectedIndex = i; });
                        scope.state.active = selectedIndex >= 0 ? selectedIndex : 0;
                        $timeout(function () {
                            var input = element[0].querySelector('.cc-ss-search');
                            if (input) input.focus();
                            scrollActiveIntoView();
                        }, 0, false);
                    };
                    scope.close = function (focusTrigger) {
                        scope.open = false;
                        if (focusTrigger) $timeout(function () { var b = element[0].querySelector('.cc-ss-trigger'); if (b) b.focus(); }, 0, false);
                    };
                    scope.toggle = function () { if (scope.open) scope.close(false); else scope.openPopup(); };
                    scope.choose = function (entry) {
                        if (!entry) return;
                        scope.close(true);
                        if (entry.kind === 'add') { scope.onAddNew(); return; }
                        if (!same(entry.value, ngModel.$viewValue) || (entry.value === null && ngModel.$viewValue !== null)) {
                            ngModel.$setViewValue(entry.value);
                        }
                        refreshLabel();
                    };
                    scope.triggerKey = function (event) {
                        var key = event.key || '';
                        if (key === 'Enter' || key === ' ' || key === 'ArrowDown') { event.preventDefault(); scope.openPopup(); }
                    };
                    scope.searchKey = function (event) {
                        var key = event.key || '', count = scope.entries.length;
                        if (key === 'ArrowDown') { event.preventDefault(); if (count) scope.state.active = (scope.state.active + 1) % count; scrollActiveIntoView(); }
                        else if (key === 'ArrowUp') { event.preventDefault(); if (count) scope.state.active = (scope.state.active - 1 + count) % count; scrollActiveIntoView(); }
                        else if (key === 'Enter') { event.preventDefault(); event.stopPropagation(); scope.choose(scope.entries[scope.state.active]); }
                        else if (key === 'Escape') { event.preventDefault(); event.stopPropagation(); scope.close(true); }
                        else if (key === 'Tab') { scope.close(false); }
                    };

                    ngModel.$render = function () { refreshLabel(); if (scope.open) scope.rebuild(false); };
                    scope.$watchCollection('items', function () { refreshLabel(); if (scope.open) scope.rebuild(false); });

                    function outsideClick(event) {
                        if (scope.open && !element[0].contains(event.target)) scope.$apply(function () { scope.close(false); });
                    }
                    $document.on('mousedown', outsideClick);
                    scope.$on('$destroy', function () { $document.off('mousedown', outsideClick); });
                }
            };
        }])
        .controller('CostControlController', ['$scope', '$http', function ($scope, $http) {
            var STORAGE_KEY = 'pmsCostControl.selection';
            var page = window.CostControlPage || {};

            // ---------- state ----------
            $scope.access = { IsAdmin: page.isadmin == 1, CanEnter: page.canEnter == 1, CanApprove: page.canApprove == 1, EmployeeId: page.employeeId };
            $scope.tab = 'budget';
            $scope.busy = false;
            $scope.error = '';
            $scope.notice = '';
            $scope.projects = [];
            $scope.costCodes = [];
            $scope.units = [];
            $scope.selection = { projectId: null };
            $scope.structure = { milestones: [], tasks: [] };
            $scope.budget = { versions: [], versionId: null, lines: [] };
            $scope.versionOptions = [];
            $scope.actuals = { entries: [], search: '' };
            $scope.forecast = { rows: [] };
            $scope.report = { groupBy: 'COSTCODE', rows: [], header: null };
            $scope.summary = { rows: [] };
            $scope.periods = { rows: [] };
            $scope.history = { rows: [], search: '', companyLevel: false };
            $scope.codes = { search: '' };
            $scope.accessList = { rows: [], search: '' };
            $scope.modal = { type: null, data: {}, error: '' };
            $scope.modalTasks = [];
            $scope.quickCode = { open: false };
            $scope.quickUnit = { open: false };

            // Stable option arrays for <cc-search-select> (rebuilt only when data loads).
            $scope.costTypes = ['Material', 'Labour', 'Subcontract', 'Equipment', 'Overhead', 'Other'];
            $scope.costTypeOptions = $scope.costTypes.map(function (t) { return { id: t, Name: t }; });
            $scope.groupByOptions = [{ id: 'COSTCODE', Name: 'Cost code' }, { id: 'MILESTONE', Name: 'Milestone' }, { id: 'COSTTYPE', Name: 'Cost type' }];
            $scope.levelOptions = [{ id: 'Division', Name: 'Division' }, { id: 'Section', Name: 'Section' }, { id: 'Element', Name: 'Element (posting)' }];
            $scope.sourceDocOptions = ['Bill / Invoice', 'Voucher', 'Purchase Order', 'GRN', 'Petty Cash', 'Timesheet', 'Other'].map(function (t) { return { id: t, Name: t }; });
            $scope.postingCodeOptions = [];
            $scope.sectionOptions = [];
            $scope.parentOptions = [];

            var TABS = [
                { id: 'budget', name: 'Budget', project: true },
                { id: 'actuals', name: 'Cost Entries', project: true },
                { id: 'forecast', name: 'Forecast (ETC)', project: true },
                { id: 'report', name: 'Budget vs Actual', project: true },
                { id: 'summary', name: 'Project Summary', project: false },
                { id: 'periods', name: 'Periods', project: true },
                { id: 'history', name: 'History', project: true },
                { id: 'codes', name: 'Cost Codes', project: false, admin: true },
                { id: 'access', name: 'Access', project: false, admin: true }
            ];
            var visible = TABS.filter(function (t) { return !t.admin || $scope.access.IsAdmin; });
            $scope.visibleTabs = function () { return visible; };
            $scope.needsProject = function () {
                if ($scope.tab === 'history' && $scope.history.companyLevel) return false;
                return visible.some(function (t) { return t.id === $scope.tab && t.project; });
            };

            // ---------- server call helper ----------
            function call(method, payload) {
                return $http.post('default.aspx/' + method, payload || {}).then(function (response) {
                    var result = response && response.data ? response.data.d : null;
                    if (!result) throw new Error('No response from the server.');
                    if (result.Success === false && !result.NeedsConfirm) throw new Error(result.Message || 'The request failed.');
                    return result;
                }, function (response) {
                    var message = response && response.data && response.data.Message ? response.data.Message : 'The server request failed.';
                    throw new Error(message);
                });
            }
            function showError(err) { $scope.notice = ''; $scope.error = err && err.message ? err.message : String(err); }
            function showNotice(text) { $scope.error = ''; $scope.notice = text || ''; }
            function toNumber(value) { var n = parseFloat(value); return isNaN(n) ? null : n; }
            function toIntOrNull(value) { var n = parseInt(value, 10); return isNaN(n) || n <= 0 ? null : n; }
            function pad(n) { return n < 10 ? '0' + n : '' + n; }
            // Business date only: yyyy-MM-dd from the local calendar date (no time-zone shift).
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

            // ---------- view helpers ----------
            $scope.sum = function (rows, field) {
                var total = 0;
                angular.forEach(rows || [], function (row) { total += toNumber(row[field]) || 0; });
                return Math.round(total * 100) / 100;
            };
            $scope.totalConsumed = function (rows) {
                var budget = $scope.sum(rows, 'CurrentBudget');
                if (!budget) return '-';
                return (Math.round($scope.sum(rows, 'Actual') * 10000 / budget) / 100).toFixed(2) + ' %';
            };
            $scope.eacOf = function (row) { return (toNumber(row.Actual) || 0) + (toNumber(row.EtcAmount) || 0); };
            $scope.groupLabel = function () {
                return $scope.report.groupBy === 'MILESTONE' ? 'Milestone' : ($scope.report.groupBy === 'COSTTYPE' ? 'Cost Type' : 'Cost Code');
            };
            $scope.monthLabel = function (text) {
                var d = fromDateText(text);
                return d ? d.toLocaleString('en-GB', { month: 'short', year: 'numeric' }) : text;
            };
            $scope.tasksFor = function (milestoneId) {
                return ($scope.structure.tasks || []).filter(function (t) { return milestoneId && t.MilestoneId === milestoneId; });
            };
            $scope.versionLabel = function (v) {
                var status = v.VersionStatus === 'Locked' ? 'Approved' : v.VersionStatus;
                return 'V' + v.VersionNo + ' - ' + v.VersionType + ' (' + status + ')';
            };
            $scope.currentVersion = function () {
                for (var i = 0; i < $scope.budget.versions.length; i++)
                    if ($scope.budget.versions[i].id === $scope.budget.versionId) return $scope.budget.versions[i];
                return null;
            };
            $scope.isDraft = function () { var v = $scope.currentVersion(); return !!v && v.VersionStatus === 'Draft'; };
            $scope.isSubmitted = function () { var v = $scope.currentVersion(); return !!v && v.VersionStatus === 'Submitted'; };
            $scope.isOwnSubmission = function () { var v = $scope.currentVersion(); return !!v && v.VersionStatus === 'Submitted' && v.SubmittedBy === $scope.access.EmployeeId; };
            $scope.hasOpenVersion = function () {
                return $scope.budget.versions.some(function (v) { return v.VersionStatus === 'Draft' || v.VersionStatus === 'Submitted'; });
            };
            function matches(row, text, fields) {
                if (!text) return true;
                text = text.toLowerCase();
                return fields.some(function (f) { return String(row[f] == null ? '' : row[f]).toLowerCase().indexOf(text) >= 0; });
            }
            $scope.filteredEntries = function () {
                return $scope.actuals.entries.filter(function (e) {
                    return matches(e, $scope.actuals.search, ['EntryDate', 'CostCode', 'CostCodeName', 'MilestoneName', 'TaskName', 'SiteName', 'SourceDocType', 'ReferenceNo', 'Remarks']);
                });
            };
            $scope.filteredCodes = function () {
                return $scope.costCodes.filter(function (c) { return matches(c, $scope.codes.search, ['Code', 'Name', 'CostType', 'CodeLevel', 'ParentCode', 'Description']); });
            };
            $scope.filteredHistory = function () {
                return $scope.history.rows.filter(function (h) { return matches(h, $scope.history.search, ['ActionTime', 'UserName', 'EntityType', 'Action', 'Reason', 'Changes']); });
            };
            $scope.filteredAccess = function () {
                return $scope.accessList.rows.filter(function (a) { return matches(a, $scope.accessList.search, ['EmployeeName']); });
            };
            var ENTITY_LABELS = { CostCode: 'Cost code', BudgetVersion: 'Budget version', BudgetLine: 'Budget line', CostEntry: 'Cost entry', Etc: 'Forecast (ETC)', Period: 'Month', Access: 'Access (employee)' };
            $scope.entityLabel = function (type) { return ENTITY_LABELS[type] || type; };

            // Turns the JSON before/after snapshots of an audit row into "Field: old -> new".
            function describeChanges(row) {
                var before = {}, after = {};
                try { before = row.OldData ? JSON.parse(row.OldData) : {}; } catch (e) { before = {}; }
                try { after = row.NewData ? JSON.parse(row.NewData) : {}; } catch (e) { after = {}; }
                var keys = {}, parts = [];
                angular.forEach(before, function (v, k) { keys[k] = true; });
                angular.forEach(after, function (v, k) { keys[k] = true; });
                angular.forEach(keys, function (ignored, key) {
                    var oldValue = before.hasOwnProperty(key) ? before[key] : null, newValue = after.hasOwnProperty(key) ? after[key] : null;
                    if (row.OldData && row.NewData) { if (String(oldValue) !== String(newValue)) parts.push(key + ': ' + (oldValue == null ? '-' : oldValue) + ' -> ' + (newValue == null ? '-' : newValue)); }
                    else parts.push(key + ': ' + (row.NewData ? newValue : oldValue));
                });
                return parts.join('; ');
            }

            // ---------- option builders ----------
            function buildCodeOptions() {
                var label = function (c) { return { id: c.id, Label: c.Code + ' - ' + c.Name }; };
                $scope.postingCodeOptions = $scope.costCodes.filter(function (c) { return c.CodeLevel === 'Element'; }).map(label);
                $scope.sectionOptions = $scope.costCodes.filter(function (c) { return c.CodeLevel === 'Section'; }).map(label);
                $scope.divisionOptions = $scope.costCodes.filter(function (c) { return c.CodeLevel === 'Division'; }).map(label);
            }
            function refreshParentOptions() {
                var d = $scope.modal.data || {};
                $scope.parentOptions = d.CodeLevel === 'Section' ? $scope.divisionOptions
                    : (d.CodeLevel === 'Element' ? $scope.sectionOptions : []);
                $scope.parentOptions = ($scope.parentOptions || []).filter(function (o) { return o.id !== d.Id; });
            }
            $scope.codeLevelChanged = function () { $scope.modal.data.ParentId = null; refreshParentOptions(); };
            function refreshModalTasks() {
                var milestoneId = $scope.modal.data ? $scope.modal.data.MilestoneId : null;
                $scope.modalTasks = milestoneId ? $scope.tasksFor(milestoneId) : [];
            }
            $scope.milestoneChanged = function () { $scope.modal.data.TaskId = null; refreshModalTasks(); };

            // ---------- loading ----------
            function loadLookups() {
                return call('GetLookups').then(function (result) {
                    $scope.projects = result.Projects || [];
                    $scope.costCodes = result.CostCodes || [];
                    $scope.units = result.Units || [];
                    if (result.Access) $scope.access = { IsAdmin: !!result.Access.IsAdmin, CanEnter: !!result.Access.CanEnter, CanApprove: !!result.Access.CanApprove, EmployeeId: result.Access.EmployeeId };
                    buildCodeOptions();
                });
            }
            function loadStructure() {
                $scope.structure = { milestones: [], tasks: [] };
                if (!$scope.selection.projectId) return;
                return call('GetProjectStructure', { projectId: $scope.selection.projectId }).then(function (result) {
                    $scope.structure = { milestones: result.Milestones || [], tasks: result.Tasks || [] };
                    refreshModalTasks();
                }).catch(showError);
            }
            $scope.loadBudget = function () {
                if (!$scope.selection.projectId) return;
                return call('GetBudget', { projectId: $scope.selection.projectId, versionId: $scope.budget.versionId || 0 }).then(function (result) {
                    $scope.budget.versions = result.Versions || [];
                    $scope.budget.lines = result.Lines || [];
                    $scope.versionOptions = $scope.budget.versions.map(function (v) { return { id: v.id, Label: $scope.versionLabel(v) }; });
                    if (!$scope.currentVersion()) $scope.budget.versionId = $scope.budget.versions.length ? $scope.budget.versions[0].id : null;
                }).catch(showError);
            };
            function loadEntries() {
                if (!$scope.selection.projectId) return;
                return call('GetCostEntries', { projectId: $scope.selection.projectId }).then(function (result) {
                    $scope.actuals.entries = result.Entries || [];
                }).catch(showError);
            }
            function loadForecast() {
                if (!$scope.selection.projectId) return;
                return call('GetEtc', { projectId: $scope.selection.projectId }).then(function (result) {
                    $scope.forecast.rows = result.Rows || [];
                }).catch(showError);
            }
            $scope.loadReport = function () {
                if (!$scope.selection.projectId) return;
                return call('GetBudgetVsActual', { projectId: $scope.selection.projectId, groupBy: $scope.report.groupBy }).then(function (result) {
                    $scope.report.rows = result.Rows || [];
                    $scope.report.header = result.Header || null;
                }).catch(showError);
            };
            $scope.loadSummary = function () {
                return call('GetProjectSummary').then(function (result) { $scope.summary.rows = result.Rows || []; }).catch(showError);
            };
            $scope.loadPeriods = function () {
                if (!$scope.selection.projectId) return;
                return call('GetPeriods', { projectId: $scope.selection.projectId }).then(function (result) { $scope.periods.rows = result.Rows || []; }).catch(showError);
            };
            $scope.loadHistory = function () {
                var projectId = $scope.history.companyLevel ? 0 : $scope.selection.projectId;
                if (!$scope.history.companyLevel && !projectId) return;
                return call('GetHistory', { projectId: projectId || 0 }).then(function (result) {
                    $scope.history.rows = (result.Rows || []).map(function (h) { h.Changes = describeChanges(h); return h; });
                }).catch(showError);
            };
            function loadAccess() {
                return call('GetAccessList').then(function (result) {
                    $scope.accessList.rows = (result.Rows || []).map(function (a) {
                        return { EmployeeId: a.EmployeeId, EmployeeName: a.EmployeeName, view: a.CanView == 1, enter: a.CanEnter == 1, approve: a.CanApprove == 1, saving: false, message: '' };
                    });
                }).catch(showError);
            }
            function refreshTab() {
                switch ($scope.tab) {
                    case 'budget': return $scope.loadBudget();
                    case 'actuals': return loadEntries();
                    case 'forecast': return loadForecast();
                    case 'report': return $scope.loadReport();
                    case 'summary': return $scope.loadSummary();
                    case 'periods': return $scope.loadPeriods();
                    case 'history': return $scope.loadHistory();
                    case 'codes': return loadLookups().catch(showError);
                    case 'access': return loadAccess();
                }
            }

            $scope.openTab = function (tab) {
                $scope.tab = tab;
                $scope.error = ''; $scope.notice = '';
                saveSelection();
                refreshTab();
            };
            $scope.projectChanged = function () {
                $scope.error = ''; $scope.notice = '';
                $scope.budget = { versions: [], versionId: null, lines: [] };
                $scope.versionOptions = [];
                $scope.actuals.entries = [];
                $scope.forecast.rows = [];
                $scope.report.rows = []; $scope.report.header = null;
                $scope.periods.rows = [];
                $scope.history.rows = [];
                saveSelection();
                loadStructure();
                refreshTab();
            };
            $scope.openProjectReport = function (projectId) {
                $scope.selection.projectId = projectId;
                $scope.tab = 'report';
                $scope.projectChanged();
            };
            $scope.exportFile = function (type) {
                var url = 'export.aspx?type=' + encodeURIComponent(type);
                if (type !== 'summary') url += '&projectId=' + encodeURIComponent($scope.selection.projectId);
                if (type === 'report') url += '&groupBy=' + encodeURIComponent($scope.report.groupBy);
                if (type === 'budget' && $scope.budget.versionId) url += '&versionId=' + encodeURIComponent($scope.budget.versionId);
                window.location.href = url;
            };

            // ---------- quick add: cost code (administrators) ----------
            $scope.startQuickCode = function () {
                $scope.quickUnit = { open: false };
                $scope.quickCode = { open: true, Code: '', Name: '', CostType: null, ParentId: null, error: '', saving: false };
            };
            $scope.cancelQuickCode = function () { $scope.quickCode = { open: false }; };
            $scope.saveQuickCode = function () {
                var q = $scope.quickCode;
                if (!(q.Code || '').trim() || !(q.Name || '').trim()) { q.error = 'Code and name are required.'; return; }
                if (!q.CostType) { q.error = 'Select a cost type.'; return; }
                q.saving = true; q.error = '';
                call('SaveCostCode', { request: { Id: 0, Code: q.Code, Name: q.Name, CostType: q.CostType, Description: '', CodeLevel: 'Element', ParentId: toIntOrNull(q.ParentId) } })
                    .then(function (result) {
                        return loadLookups().then(function () { $scope.modal.data.CostCodeId = result.Id; $scope.quickCode = { open: false }; });
                    })
                    .catch(function (err) { q.saving = false; q.error = err.message; });
            };

            // ---------- quick add: unit (existing tbl_unitmaster) ----------
            $scope.startQuickUnit = function () {
                $scope.quickCode = { open: false };
                $scope.quickUnit = { open: true, Name: '', error: '', saving: false };
            };
            $scope.cancelQuickUnit = function () { $scope.quickUnit = { open: false }; };
            $scope.saveQuickUnit = function () {
                var q = $scope.quickUnit;
                if (!(q.Name || '').trim()) { q.error = 'Enter a unit name.'; return; }
                q.saving = true; q.error = '';
                call('AddUnit', { unitName: q.Name.trim() })
                    .then(function (result) {
                        return loadLookups().then(function () { $scope.modal.data.UnitId = result.Id; $scope.quickUnit = { open: false }; });
                    })
                    .catch(function (err) { q.saving = false; q.error = err.message; });
            };

            // ---------- modal handling ----------
            $scope.closeModal = function () { if (!$scope.busy) $scope.modal = { type: null, data: {}, error: '' }; };
            function openModal(type, data) {
                $scope.quickCode = { open: false }; $scope.quickUnit = { open: false };
                $scope.modal = { type: type, data: data, error: '' };
                refreshModalTasks();
                refreshParentOptions();
            }
            function openReason(title, hint, button, onSave) {
                openModal('reason', { title: title, hint: hint, button: button, reason: '', onSave: onSave });
            }

            $scope.openNewVersion = function () { openModal('version', { reason: '' }); };
            $scope.openBudgetLine = function (line) {
                line = line || {};
                openModal('line', {
                    Id: line.id || 0, CostCodeId: line.CostCodeId || null,
                    MilestoneId: line.MilestoneId || null, TaskId: line.TaskId || null,
                    Quantity: toNumber(line.Quantity), UnitId: line.UnitId || null, Rate: toNumber(line.Rate),
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
                    Amount: toNumber(entry.Amount), SourceDocType: entry.SourceDocType || null, ReferenceNo: entry.ReferenceNo || '',
                    SiteName: entry.SiteName || '', Remarks: entry.Remarks || '', Reason: ''
                });
            };
            $scope.openEtc = function (row) {
                row = row || {};
                openModal('etc', {
                    fixedKey: !!row.CostCodeId, CostCodeId: row.CostCodeId || null, MilestoneId: row.MilestoneId || null,
                    CurrentBudget: toNumber(row.CurrentBudget), Actual: toNumber(row.Actual),
                    EtcAmount: toNumber(row.EtcAmount), AsOfValue: row.AsOfDate ? fromDateText(row.AsOfDate) : new Date(), Remarks: row.Remarks || ''
                });
            };
            $scope.openCostCode = function (code) {
                code = code || {};
                openModal('code', {
                    Id: code.id || 0, Code: code.Code || '', Name: code.Name || '', CostType: code.CostType || null,
                    Description: code.Description || '', CodeLevel: code.CodeLevel || 'Element', ParentId: code.ParentId || null,
                    IsUsed: code.IsUsed == 1, HasChildren: code.HasChildren == 1
                });
            };
            $scope.openReject = function () {
                openReason('Return Budget to Draft', 'The preparer will see this reason and can correct and resubmit the budget.', 'Return to Draft', function (reason) {
                    return call('RejectBudgetVersion', { versionId: $scope.budget.versionId, reason: reason }).then(function (r) { showNotice(r.Message); return $scope.loadBudget(); });
                });
            };
            $scope.openDeleteEntry = function (entry) {
                openReason('Delete Cost Entry', 'Entry of ' + entry.EntryDate + ', amount ' + entry.Amount + '. The deletion and its reason are kept in the history.', 'Delete', function (reason) {
                    return call('DeleteCostEntry', { costEntryId: entry.id, reason: reason }).then(function (r) { showNotice(r.Message); return loadEntries(); });
                });
            };
            $scope.openReopen = function (period) {
                openReason('Reopen ' + $scope.monthLabel(period.PeriodStart), 'Cost entries dated in this month can be changed again after reopening.', 'Reopen', function (reason) {
                    return call('SetPeriodStatus', { projectId: $scope.selection.projectId, periodStart: period.PeriodStart, close: false, reason: reason })
                        .then(function (r) { showNotice(r.Message); return $scope.loadPeriods(); });
                });
            };

            function validateModal() {
                var d = $scope.modal.data;
                switch ($scope.modal.type) {
                    case 'version': if ($scope.budget.versions.length && !(d.reason || '').trim()) return 'A reason is required for a revision.'; break;
                    case 'reason': if (!(d.reason || '').trim()) return 'Enter a reason.'; break;
                    case 'line':
                        if (!d.CostCodeId) return 'Select a cost code.';
                        if (d.Amount == null || d.Amount < 0) return 'Enter a budget amount of zero or more.';
                        break;
                    case 'entry':
                        if (!d.EntryDateValue) return 'Enter the cost date.';
                        if (!d.CostCodeId) return 'Select a cost code.';
                        if (!d.Amount) return 'Enter a non-zero amount.';
                        if (d.Id && !(d.Reason || '').trim()) return 'Enter the reason for changing this saved entry.';
                        break;
                    case 'etc':
                        if (!d.CostCodeId) return 'Select a cost code.';
                        if (d.EtcAmount == null || d.EtcAmount < 0) return 'Enter an estimate to complete of zero or more.';
                        if (!d.AsOfValue) return 'Enter the as-of date.';
                        break;
                    case 'code':
                        if (!(d.Code || '').trim() || !(d.Name || '').trim()) return 'Code and name are required.';
                        if (!d.CostType) return 'Select a cost type.';
                        if (d.CodeLevel === 'Section' && !d.ParentId) return 'A Section must sit under a Division.';
                        break;
                }
                return '';
            }

            function entryRequest(d, confirmDuplicate) {
                return { request: {
                    Id: d.Id, ProjectId: $scope.selection.projectId, MilestoneId: toIntOrNull(d.MilestoneId), TaskId: toIntOrNull(d.TaskId),
                    CostCodeId: d.CostCodeId, EntryDate: toDateText(d.EntryDateValue), Amount: d.Amount,
                    SourceDocType: d.SourceDocType || '', ReferenceNo: d.ReferenceNo, SiteName: d.SiteName, Remarks: d.Remarks,
                    Reason: d.Reason || '', ConfirmDuplicate: !!confirmDuplicate } };
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
                    case 'reason':
                        request = d.onSave(d.reason.trim());
                        after = function () { };
                        break;
                    case 'line':
                        request = call('SaveBudgetLine', { request: {
                            Id: d.Id, VersionId: $scope.budget.versionId, MilestoneId: toIntOrNull(d.MilestoneId), TaskId: toIntOrNull(d.TaskId),
                            CostCodeId: d.CostCodeId, Quantity: d.Quantity, UnitId: toIntOrNull(d.UnitId), Rate: d.Rate, Amount: d.Amount, Remarks: d.Remarks } });
                        after = function () { return $scope.loadBudget(); };
                        break;
                    case 'entry':
                        // A duplicate reference returns NeedsConfirm; ask once, then resend with confirmation.
                        request = call('SaveCostEntry', entryRequest(d, false)).then(function (result) {
                            if (!result.NeedsConfirm) return result;
                            if (!window.confirm(result.Message)) throw new Error('Not saved: duplicate reference.');
                            return call('SaveCostEntry', entryRequest(d, true)).then(function (second) {
                                if (second.NeedsConfirm || second.Success === false) throw new Error(second.Message || 'Not saved.');
                                return second;
                            });
                        });
                        after = loadEntries;
                        break;
                    case 'etc':
                        request = call('SaveEtc', { request: {
                            ProjectId: $scope.selection.projectId, CostCodeId: d.CostCodeId, MilestoneId: toIntOrNull(d.MilestoneId),
                            EtcAmount: d.EtcAmount, AsOfDate: toDateText(d.AsOfValue), Remarks: d.Remarks } });
                        after = loadForecast;
                        break;
                    case 'code':
                        request = call('SaveCostCode', { request: { Id: d.Id, Code: d.Code, Name: d.Name, CostType: d.CostType, Description: d.Description, CodeLevel: d.CodeLevel, ParentId: toIntOrNull(d.ParentId) } });
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
                if (question && !window.confirm(question)) return;
                $scope.busy = true;
                $scope.error = ''; $scope.notice = '';
                call(method, payload).then(function (result) {
                    $scope.busy = false;
                    if (result && result.Message) showNotice(result.Message);
                    return after();
                }).catch(function (err) { $scope.busy = false; showError(err); });
            }
            $scope.submitVersion = function () {
                confirmAndRun('Submit this budget for approval? It cannot be edited while it is waiting for approval.',
                    'SubmitBudgetVersion', { versionId: $scope.budget.versionId }, $scope.loadBudget);
            };
            $scope.approveVersion = function () {
                confirmAndRun('Approve and lock this budget? A locked budget cannot be edited; later changes need a revision.',
                    'LockBudgetVersion', { versionId: $scope.budget.versionId }, $scope.loadBudget);
            };
            $scope.discardDraft = function () {
                confirmAndRun('Discard this draft budget and all its lines?', 'DiscardDraftVersion', { versionId: $scope.budget.versionId },
                    function () { $scope.budget.versionId = null; return $scope.loadBudget(); });
            };
            $scope.deleteBudgetLine = function (line) {
                confirmAndRun('Delete this budget line?', 'DeleteBudgetLine', { budgetLineId: line.id }, $scope.loadBudget);
            };
            $scope.deleteCostCode = function (code) {
                confirmAndRun('Delete cost code ' + code.Code + '?', 'DeleteCostCode', { costCodeId: code.id }, loadLookups);
            };
            $scope.closePeriod = function (period) {
                confirmAndRun('Close ' + $scope.monthLabel(period.PeriodStart) + '? Cost entries dated in this month will be locked.',
                    'SetPeriodStatus', { projectId: $scope.selection.projectId, periodStart: period.PeriodStart, close: true, reason: '' }, $scope.loadPeriods);
            };

            // Access: entering or approving implies viewing.
            $scope.saveAccess = function (a, changed) {
                if ((changed === 'enter' && a.enter) || (changed === 'approve' && a.approve)) a.view = true;
                a.saving = true; a.message = '';
                call('SaveAccess', { request: { EmployeeId: a.EmployeeId, CanView: a.view, CanEnter: a.enter, CanApprove: a.approve } })
                    .then(function () { a.saving = false; a.message = 'Saved'; })
                    .catch(function (err) { a.saving = false; a.message = ''; showError(err); return loadAccess(); });
            };

            // ---------- start ----------
            var saved = readSelection();
            if (saved.tab && visible.some(function (t) { return t.id === saved.tab; })) $scope.tab = saved.tab;
            loadLookups().then(function () {
                var exists = $scope.projects.some(function (p) { return p.id === saved.projectId; });
                if (exists) { $scope.selection.projectId = saved.projectId; $scope.projectChanged(); }
                else if (!$scope.needsProject()) refreshTab();
            }).catch(showError);
        }]);
})();
