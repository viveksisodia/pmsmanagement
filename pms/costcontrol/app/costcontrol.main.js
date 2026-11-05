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
            $scope.report = { groupBy: 'COSTCODE', rows: [], header: null, snapshotId: 0, showBreakdown: false };
            $scope.movements = { rows: [], reserves: [] };
            $scope.changes = { rows: [] };
            $scope.approvals = { rows: [] };
            $scope.snapshotOptions = [{ id: 0, Label: 'Live (today)' }];
            $scope.reserveOptions = [];
            $scope.changeOptions = [];
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
                { id: 'movements', name: 'Movements & Reserves', project: true },
                { id: 'changes', name: 'Changes', project: true },
                { id: 'summary', name: 'Project Summary', project: false },
                { id: 'approvals', name: 'Pending Approvals', project: false },
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
                    var splits = result.Splits || [];
                    $scope.actuals.entries = (result.Entries || []).map(function (e) { e.splits = splits.filter(function (s) { return s.EntryId === e.id; }); return e; });
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
                return call('GetBudgetVsActual', { projectId: $scope.selection.projectId, groupBy: $scope.report.groupBy, snapshotId: $scope.report.snapshotId || 0 }).then(function (result) {
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
                    case 'report': return loadSnapshots().then($scope.loadReport);
                    case 'movements': return loadMovements();
                    case 'changes': return loadChanges();
                    case 'approvals': return $scope.loadApprovals();
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
                $scope.report.rows = []; $scope.report.header = null; $scope.report.snapshotId = 0;
                $scope.snapshotOptions = [{ id: 0, Label: 'Live (today)' }];
                $scope.movements = { rows: [], reserves: [] }; $scope.reserveOptions = [];
                $scope.changes = { rows: [] }; $scope.changeOptions = [];
                $scope.periods.rows = [];
                $scope.history.rows = [];
                saveSelection();
                loadStructure();
                // Changes and reserves feed the cost-entry and movement forms.
                if ($scope.tab !== 'changes') loadChanges();
                if ($scope.tab !== 'movements') loadMovements();
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
                if (type === 'report') url += '&groupBy=' + encodeURIComponent($scope.report.groupBy) + '&snapshotId=' + encodeURIComponent($scope.report.snapshotId || 0);
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
                var wanted = q.Name.replace(/\s+/g, ' ').trim().toUpperCase();
                var existing = $scope.units.filter(function (u) { return String(u.UnitName || '').replace(/\s+/g, ' ').trim().toUpperCase() === wanted; })[0];
                if (existing) { $scope.modal.data.UnitId = existing.id; $scope.quickUnit = { open: false }; showNotice('Unit "' + existing.UnitName + '" already exists, so it has been selected instead of adding a duplicate.'); return; }
                q.saving = true; q.error = '';
                call('AddUnit', { unitName: q.Name.trim() })
                    .then(function (result) {
                        return loadLookups().then(function () { $scope.modal.data.UnitId = result.Id; $scope.quickUnit = { open: false }; if (result.AlreadyExists) showNotice(result.Message); });
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
                    SiteName: entry.SiteName || '', Remarks: entry.Remarks || '', Reason: '', ChangeId: entry.ChangeId || null,
                    IsSplit: !!(entry.splits && entry.splits.length),
                    Splits: (entry.splits || []).map(function (s) { return { CostCodeId: s.CostCodeId, MilestoneId: s.MilestoneId || null, TaskId: s.TaskId || null, Amount: toNumber(s.Amount), Percent: toNumber(s.SplitPercent) }; })
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
                var phase3Message = validatePhase3Modal($scope.modal.type, d);
                if (phase3Message) return phase3Message;
                switch ($scope.modal.type) {
                    case 'version': if ($scope.budget.versions.length && !(d.reason || '').trim()) return 'A reason is required for a revision.'; break;
                    case 'reason': if (!(d.reason || '').trim()) return 'Enter a reason.'; break;
                    case 'line':
                        if (!d.CostCodeId) return 'Select a cost code.';
                        if (d.Amount == null || d.Amount < 0) return 'Enter a budget amount of zero or more.';
                        break;
                    case 'entry':
                        if (!d.EntryDateValue) return 'Enter the cost date.';
                        if (!d.Amount) return 'Enter a non-zero amount.';
                        if (d.IsSplit) {
                            if (d.Splits.length < 2) return 'A split needs at least two rows.';
                            if (d.Splits.some(function (s) { return !s.CostCodeId || !s.Amount; })) return 'Each split row needs a cost code and an amount.';
                            if (!$scope.splitBalanced()) return 'The split rows must total exactly the entry amount (100 %).';
                            d.CostCodeId = d.Splits[0].CostCodeId;
                        } else if (!d.CostCodeId) return 'Select a cost code.';
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
                    Reason: d.Reason || '', ConfirmDuplicate: !!confirmDuplicate, ChangeId: toIntOrNull(d.ChangeId),
                    Splits: d.IsSplit ? d.Splits.map(function (s) { return { CostCodeId: s.CostCodeId, MilestoneId: toIntOrNull(s.MilestoneId), TaskId: toIntOrNull(s.TaskId), Amount: s.Amount }; }) : [] } };
            }

            $scope.saveModal = function () {
                var message = validateModal();
                if (message) { $scope.modal.error = message; return; }
                var d = $scope.modal.data, request, after;
                var phase3 = savePhase3Modal($scope.modal.type, d);
                if (phase3) { request = phase3.request; after = phase3.after; }
                else switch ($scope.modal.type) {
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

            // =====================================================================
            // Phase 3: snapshots, movements & reserves, changes & at-risk, splits,
            // pending approvals, per-project access
            // =====================================================================
            $scope.reserveTypeOptions = [
                { id: 'ContractorContingency', Name: 'Contractor contingency (internal)' },
                { id: 'ClientAllowance', Name: 'Client allowance' },
                { id: 'ProvisionalSum', Name: 'Provisional sum' }];
            $scope.movementTypeOptions = [
                { id: 'Transfer', Name: 'Transfer between budget lines' },
                { id: 'ReserveAddition', Name: 'Reserve addition' },
                { id: 'ReserveRelease', Name: 'Reserve release (reserve to budget)' },
                { id: 'ReserveReturn', Name: 'Reserve return (budget to reserve)' },
                { id: 'Correction', Name: 'Baseline correction' }];
            $scope.changeClassOptions = [
                { id: 'ClientVariation', Name: 'Client variation' }, { id: 'InternalBudgetChange', Name: 'Internal budget change' },
                { id: 'SubcontractVariation', Name: 'Subcontract variation' }, { id: 'BaselineCorrection', Name: 'Baseline correction' },
                { id: 'BudgetTransfer', Name: 'Budget transfer' }, { id: 'ContingencyRelease', Name: 'Contingency release' }];
            $scope.statusFieldOptions = [{ id: 'Client', Name: 'Client approval' }, { id: 'Subcontract', Name: 'Subcontract approval' }];
            var STATUS_VALUES = {
                Client: ['NotApplicable', 'Pending', 'Submitted', 'Approved', 'Rejected'],
                Subcontract: ['NotApplicable', 'Pending', 'Approved', 'Rejected']
            };
            $scope.statusValueOptions = [];
            function labelOf(options, id) { var o = options.filter(function (x) { return x.id === id; })[0]; return o ? o.Name : id; }
            $scope.reserveTypeLabel = function (id) { return labelOf($scope.reserveTypeOptions, id); };
            $scope.movementTypeLabel = function (id) { return id === 'ApprovedChange' ? 'Approved change' : labelOf($scope.movementTypeOptions, id); };
            $scope.changeClassLabel = function (id) { return labelOf($scope.changeClassOptions, id); };
            $scope.statusBadge = function (s) {
                if (s === 'Approved' || s === 'Posted' || s === 'Locked') return 'cc-badge-approved';
                if (s === 'Rejected') return 'cc-badge-rejected';
                if (s === 'Submitted') return 'cc-badge-submitted';
                if (s === 'Draft' || s === 'Pending') return 'cc-badge-draft';
                return 'cc-badge-muted';
            };
            $scope.vacP = function () {
                var h = $scope.report.header || {};
                return $scope.sum($scope.report.rows, 'CurrentBudget') + (toNumber(h.RSV) || 0) - $scope.sum($scope.report.rows, 'Eac') - (toNumber(h.PPC) || 0);
            };

            // ---------- snapshots ----------
            function loadSnapshots() {
                if (!$scope.selection.projectId) return Promise.resolve();
                return call('GetSnapshots', { projectId: $scope.selection.projectId }).then(function (result) {
                    var options = [{ id: 0, Label: 'Live (today)' }];
                    (result.Rows || []).forEach(function (s) { options.push({ id: s.id, Label: $scope.monthLabel(s.PeriodStart) + ' - snapshot v' + s.SnapshotVersion }); });
                    $scope.snapshotOptions = options;
                    if (!options.some(function (o) { return o.id === $scope.report.snapshotId; })) $scope.report.snapshotId = 0;
                }).catch(showError);
            }
            $scope.viewSnapshot = function (snapshotId) {
                $scope.report.snapshotId = snapshotId;
                $scope.tab = 'report';
                saveSelection();
                loadSnapshots().then(function () { $scope.report.snapshotId = snapshotId; return $scope.loadReport(); });
            };

            // ---------- movements & reserves ----------
            function loadMovements() {
                if (!$scope.selection.projectId) return;
                return call('GetMovements', { projectId: $scope.selection.projectId }).then(function (result) {
                    var lines = result.Lines || [];
                    $scope.movements.rows = (result.Movements || []).map(function (m) { m.lines = lines.filter(function (l) { return l.MovementId === m.id; }); return m; });
                    $scope.movements.reserves = result.Reserves || [];
                    $scope.reserveOptions = $scope.movements.reserves.map(function (r) { return { id: r.id, Label: r.Name + ' (' + $scope.reserveTypeLabel(r.ReserveType) + ', ' + (toNumber(r.Remaining) || 0).toFixed(2) + ')' }; });
                }).catch(showError);
            }
            $scope.openReserve = function (r) {
                r = r || {};
                openModal('reserve', { Id: r.id || 0, ReserveType: r.ReserveType || null, Name: r.Name || '', Remarks: r.Remarks || '' });
            };
            var MOVEMENT_KINDS = {
                Transfer: ['Budget'], Correction: ['Budget'], ApprovedChange: ['Budget'],
                ReserveAddition: ['Reserve'], ReserveRelease: ['Reserve', 'Budget'], ReserveReturn: ['Reserve', 'Budget']
            };
            $scope.movementAllows = function (kind) { return (MOVEMENT_KINDS[$scope.modal.data.MovementType] || []).indexOf(kind) >= 0; };
            $scope.movementMustNet = function () { var t = $scope.modal.data.MovementType; return t === 'Transfer' || t === 'ReserveRelease' || t === 'ReserveReturn'; };
            $scope.movementTypeHint = function (t) {
                switch (t) {
                    case 'Transfer': return 'Move budget from one line (negative amount) to another (positive amount). The total must be zero.';
                    case 'ReserveAddition': return 'Sets or increases a reserve. Reserve lines only, positive amounts. This changes the total available budget, so it needs approval.';
                    case 'ReserveRelease': return 'Reserve line negative, budget lines positive; the total must be zero, so RB + RSV stays the same.';
                    case 'ReserveReturn': return 'Budget lines negative, reserve line positive; the total must be zero.';
                    case 'Correction': return 'Corrects an error in the approved baseline (ADJ). Budget lines only; the reason is mandatory.';
                    case 'ApprovedChange': return 'Created from an internally authorised change. When approved, the change becomes budget-posted.';
                    default: return 'Choose the movement type.';
                }
            };
            $scope.movementTypeChanged = function () {
                var allowed = MOVEMENT_KINDS[$scope.modal.data.MovementType] || [];
                $scope.modal.data.Lines = ($scope.modal.data.Lines || []).filter(function (l) { return allowed.indexOf(l.LineKind) >= 0; });
            };
            $scope.addMovementLine = function (kind) {
                $scope.modal.data.Lines.push({ LineKind: kind, CostCodeId: null, MilestoneId: null, ReserveId: null, Amount: null });
            };
            $scope.movementTotals = function () {
                var budget = 0, reserve = 0;
                (($scope.modal.data && $scope.modal.data.Lines) || []).forEach(function (l) { var a = toNumber(l.Amount) || 0; if (l.LineKind === 'Reserve') reserve += a; else budget += a; });
                budget = Math.round(budget * 100) / 100; reserve = Math.round(reserve * 100) / 100;
                return { budget: budget, reserve: reserve, net: Math.round((budget + reserve) * 100) / 100 };
            };
            $scope.openMovement = function (m) {
                if (!m && !$scope.reserveOptions.length) loadMovements();
                m = m || {};
                openModal('movement', {
                    Id: m.id || 0, MovementType: m.MovementType || null, Reason: m.Reason || '', RiskReference: m.RiskReference || '', ChangeId: m.ChangeId || null,
                    Lines: (m.lines || []).map(function (l) { return { LineKind: l.LineKind, CostCodeId: l.CostCodeId, MilestoneId: l.MilestoneId || null, ReserveId: l.ReserveId, Amount: toNumber(l.Amount) }; })
                });
            };
            $scope.movementAction = function (m, action) {
                if (action === 'Reject') {
                    openReason('Return Movement #' + m.MovementNo, 'The preparer will see this reason and can correct and resubmit.', 'Return to Draft', function (reason) {
                        return call('SetMovementStatus', { movementId: m.id, action: 'Reject', reason: reason }).then(function (r) { showNotice(r.Message); return loadMovements(); });
                    });
                    return;
                }
                var question = { Submit: 'Submit this movement for approval?', Discard: 'Discard this draft movement?', Approve: 'Approve this movement? The revised budget changes immediately.' }[action];
                confirmAndRun(question, 'SetMovementStatus', { movementId: m.id, action: action, reason: '' }, function () { loadChanges(); return loadMovements(); });
            };

            // ---------- change register & at-risk ----------
            function loadChanges() {
                if (!$scope.selection.projectId) return;
                return call('GetChanges', { projectId: $scope.selection.projectId }).then(function (result) {
                    var allocations = result.Allocations || [];
                    $scope.changes.rows = (result.Changes || []).map(function (c) { c.allocations = allocations.filter(function (a) { return a.ChangeId === c.id; }); return c; });
                    // Cost entries may be linked to open changes; at-risk ones only once authorised (AR-03).
                    $scope.changeOptions = $scope.changes.rows
                        .filter(function (c) { return c.RecordStatus === 'Open' && (c.IsAtRisk != 1 || c.AtRiskAuthorisedOn); })
                        .map(function (c) { return { id: c.id, Label: '#' + c.ChangeNo + ' ' + c.Title + (c.IsAtRisk == 1 ? ' (at risk - authorised)' : '') }; });
                }).catch(showError);
            }
            $scope.changeTotals = function () {
                var t = { pce: 0, ppc: 0, atRisk: 0, clientApproved: 0 };
                $scope.changes.rows.forEach(function (c) {
                    var pending = c.RecordStatus === 'Open' && c.BudgetPostingStatus === 'NotPosted' && c.InternalStatus !== 'Rejected';
                    if (pending) { t.pce += toNumber(c.EstimatedCost) || 0; if (c.IsProbable == 1) t.ppc += Math.max((toNumber(c.EstimatedCost) || 0) - (toNumber(c.AmountInEtc) || 0), 0); }
                    if (c.IsAtRisk == 1 && c.ClientStatus !== 'Approved') t.atRisk += toNumber(c.CostIncurred) || 0;
                    if (c.ClientValueApproved !== null) t.clientApproved += toNumber(c.ClientValueApproved) || 0;
                });
                return t;
            };
            $scope.openChange = function (c) {
                c = c || {};
                openModal('change', {
                    Id: c.id || 0, Title: c.Title || '', Description: c.Description || '', ChangeClass: c.ChangeClass || null,
                    EstimatedCost: toNumber(c.EstimatedCost), AmountInEtc: toNumber(c.AmountInEtc) || 0, IsProbable: c.IsProbable == 1,
                    ClientValueProposed: toNumber(c.ClientValueProposed), SubcontractValue: toNumber(c.SubcontractValue), IsAtRisk: c.IsAtRisk == 1,
                    Allocations: (c.allocations || []).map(function (a) { return { CostCodeId: a.CostCodeId, MilestoneId: a.MilestoneId || null, Amount: toNumber(a.Amount) }; })
                });
            };
            $scope.statusFieldChanged = function () {
                $scope.statusValueOptions = (STATUS_VALUES[$scope.modal.data.Field] || []).map(function (v) { return { id: v, Name: v }; });
                $scope.modal.data.Value = null;
            };
            $scope.openChangeStatus = function (c) {
                openModal('changeStatus', { ChangeId: c.id, ChangeNo: c.ChangeNo, Field: 'Client', Value: null, Reason: '', DecisionDateValue: new Date(), Amount: toNumber(c.ClientValueProposed) });
                $scope.statusValueOptions = STATUS_VALUES.Client.map(function (v) { return { id: v, Name: v }; });
            };
            function changeStatusCall(c, field, value, reason) {
                return call('SetChangeStatus', { changeId: c.id, field: field, value: value, reason: reason || '', decisionDate: '', amount: null })
                    .then(function (r) { showNotice(r.Message); return loadChanges(); });
            }
            $scope.changeAction = function (c, field, value) {
                if (value === 'Rejected' || value === 'Withdrawn') {
                    openReason((value === 'Rejected' ? 'Reject cost authorisation of' : 'Withdraw') + ' change #' + c.ChangeNo, 'The change record and this reason are kept (CM-3).',
                        value === 'Rejected' ? 'Reject' : 'Withdraw', function (reason) { return changeStatusCall(c, field, value, reason); });
                    return;
                }
                var question = field === 'Internal' ? 'Authorise the internal cost of change #' + c.ChangeNo + '? This does not change client approval or the budget.'
                    : field === 'AtRisk' ? 'Authorise at-risk work for change #' + c.ChangeNo + '? Cost may then be booked before client approval and will show as at-risk exposure.'
                    : 'Close change #' + c.ChangeNo + '?';
                if (!window.confirm(question)) return;
                $scope.busy = true;
                changeStatusCall(c, field, value, '').then(function () { $scope.busy = false; }).catch(function (err) { $scope.busy = false; showError(err); });
            };
            $scope.postChange = function (c) {
                confirmAndRun('Create the budget posting for change #' + c.ChangeNo + '? A draft movement is created in Movements & Reserves; it changes the budget only after approval.',
                    'CreateChangePosting', { changeId: c.id }, function () { loadMovements(); return loadChanges(); });
            };

            // ---------- split allocation of a cost entry ----------
            $scope.splitToggled = function () {
                var d = $scope.modal.data;
                if (d.IsSplit && (!d.Splits || d.Splits.length < 2)) {
                    d.Splits = [{ CostCodeId: d.CostCodeId || null, MilestoneId: d.MilestoneId || null, Amount: null, Percent: 50 },
                                { CostCodeId: null, MilestoneId: null, Amount: null, Percent: 50 }];
                    d.Splits.forEach(function (s) { $scope.splitPercentChanged(s); });
                }
            };
            $scope.addSplit = function () { $scope.modal.data.Splits.push({ CostCodeId: null, MilestoneId: null, Amount: null, Percent: null }); };
            $scope.removeSplit = function (i) { $scope.modal.data.Splits.splice(i, 1); };
            $scope.splitPercentChanged = function (s) {
                var total = toNumber($scope.modal.data.Amount);
                if (total !== null && s.Percent !== null && s.Percent !== undefined) s.Amount = Math.round(total * s.Percent) / 100;
            };
            $scope.splitAmountChanged = function (s) {
                var total = toNumber($scope.modal.data.Amount);
                if (total) s.Percent = Math.round((toNumber(s.Amount) || 0) * 10000 / total) / 100;
            };
            $scope.balanceSplits = function () {
                var d = $scope.modal.data, rows = d.Splits;
                if (!rows.length) return;
                var others = 0;
                for (var i = 0; i < rows.length - 1; i++) others += toNumber(rows[i].Amount) || 0;
                var last = rows[rows.length - 1];
                last.Amount = Math.round(((toNumber(d.Amount) || 0) - others) * 100) / 100;
                $scope.splitAmountChanged(last);
            };
            $scope.splitTotals = function () {
                var rows = ($scope.modal.data && $scope.modal.data.Splits) || [];
                return { amount: $scope.sum(rows, 'Amount'), percent: $scope.sum(rows, 'Percent') };
            };
            $scope.splitBalanced = function () {
                var d = $scope.modal.data || {};
                return Math.round(($scope.splitTotals().amount - (toNumber(d.Amount) || 0)) * 100) === 0;
            };

            // ---------- pending approvals ----------
            $scope.loadApprovals = function () {
                return call('GetPendingApprovals').then(function (result) { $scope.approvals.rows = result.Rows || []; }).catch(showError);
            };
            $scope.openApproval = function (a) {
                $scope.selection.projectId = a.ProjectId;
                $scope.tab = a.ItemType === 'Budget' ? 'budget' : (a.ItemType === 'Movement' ? 'movements' : 'changes');
                if (a.ItemType === 'Budget') $scope.budget.versionId = a.ItemId;
                $scope.projectChanged();
                if (a.ItemType === 'Budget') { $scope.budget.versionId = a.ItemId; $scope.loadBudget(); }
            };

            // ---------- per-project access ----------
            $scope.openAccessProjects = function (a) {
                call('GetAccessProjects', { employeeId: a.EmployeeId }).then(function (result) {
                    var selected = {};
                    (result.ProjectIds || []).forEach(function (id) { selected[id] = true; });
                    openModal('accessProjects', { EmployeeId: a.EmployeeId, EmployeeName: a.EmployeeName, selected: selected, search: '' });
                }).catch(showError);
            };

            function savePhase3Modal(type, d) {
                switch (type) {
                    case 'reserve':
                        return { request: call('SaveReserve', { request: { Id: d.Id, ProjectId: $scope.selection.projectId, ReserveType: d.ReserveType, Name: d.Name, Remarks: d.Remarks } }), after: loadMovements };
                    case 'movement':
                        return { request: call('SaveMovement', { request: {
                            Id: d.Id, ProjectId: $scope.selection.projectId, MovementType: d.MovementType, Reason: d.Reason, RiskReference: d.RiskReference,
                            Lines: d.Lines.map(function (l) { return { LineKind: l.LineKind, CostCodeId: l.LineKind === 'Budget' ? toIntOrNull(l.CostCodeId) : null,
                                MilestoneId: l.LineKind === 'Budget' ? toIntOrNull(l.MilestoneId) : null, ReserveId: l.LineKind === 'Reserve' ? toIntOrNull(l.ReserveId) : null,
                                Amount: toNumber(l.Amount) || 0, Remarks: '' }; }) } }), after: loadMovements };
                    case 'change':
                        return { request: call('SaveChange', { request: {
                            Id: d.Id, ProjectId: $scope.selection.projectId, Title: d.Title, Description: d.Description, ChangeClass: d.ChangeClass,
                            EstimatedCost: d.EstimatedCost, AmountInEtc: d.AmountInEtc || 0, IsProbable: !!d.IsProbable,
                            ClientValueProposed: d.ClientValueProposed, SubcontractValue: d.SubcontractValue, IsAtRisk: !!d.IsAtRisk,
                            Allocations: d.Allocations.map(function (a) { return { CostCodeId: a.CostCodeId, MilestoneId: toIntOrNull(a.MilestoneId), Amount: toNumber(a.Amount) || 0 }; }) } }),
                            after: loadChanges };
                    case 'changeStatus':
                        var needsDate = d.Value === 'Approved' || d.Value === 'Rejected';
                        return { request: call('SetChangeStatus', { changeId: d.ChangeId, field: d.Field, value: d.Value, reason: d.Reason || '',
                            decisionDate: needsDate ? toDateText(d.DecisionDateValue) : '', amount: d.Field === 'Client' && d.Value === 'Approved' ? d.Amount : null }), after: loadChanges };
                    case 'accessProjects':
                        var ids = Object.keys(d.selected).filter(function (k) { return d.selected[k]; }).map(function (k) { return parseInt(k, 10); });
                        return { request: call('SaveAccessProjects', { employeeId: d.EmployeeId, projectIds: ids }).then(function (r) { showNotice(r.Message); return r; }), after: function () { } };
                }
                return null;
            }
            function validatePhase3Modal(type, d) {
                switch (type) {
                    case 'reserve':
                        if (!d.ReserveType) return 'Select the reserve type.';
                        if (!(d.Name || '').trim()) return 'Enter the reserve name.';
                        break;
                    case 'movement':
                        if (!d.MovementType) return 'Select the movement type.';
                        if (!(d.Reason || '').trim()) return 'A reason is required.';
                        if (!d.Lines.length) return 'Add at least one line.';
                        if (d.Lines.some(function (l) { return !toNumber(l.Amount); })) return 'Every line needs a non-zero amount.';
                        if (d.Lines.some(function (l) { return l.LineKind === 'Budget' ? !l.CostCodeId : !l.ReserveId; })) return 'Every line needs a cost code or a reserve.';
                        if ($scope.movementMustNet() && $scope.movementTotals().net !== 0) return 'This movement must net to zero (currently ' + $scope.movementTotals().net.toFixed(2) + ').';
                        break;
                    case 'change':
                        if (!(d.Title || '').trim()) return 'Enter a title.';
                        if (!d.ChangeClass) return 'Select the change class.';
                        if (d.EstimatedCost === null || d.EstimatedCost === undefined) return 'Enter the estimated cost (0 if none).';
                        if ((d.AmountInEtc || 0) < 0 || (d.AmountInEtc || 0) > Math.abs(d.EstimatedCost)) return 'The part already in ETC must be between 0 and the estimated cost.';
                        if (d.Allocations.some(function (a) { return !a.CostCodeId || !toNumber(a.Amount); })) return 'Each allocation needs a cost code and an amount.';
                        break;
                    case 'changeStatus':
                        if (!d.Field || !d.Value) return 'Select the status and its new value.';
                        if (d.Value === 'Rejected' && !(d.Reason || '').trim()) return 'A reason is required.';
                        if ((d.Value === 'Approved' || d.Value === 'Rejected') && !d.DecisionDateValue) return 'Enter the decision date.';
                        if (d.Field === 'Client' && d.Value === 'Approved' && (d.Amount === null || d.Amount === undefined)) return 'Enter the approved client value.';
                        break;
                }
                return '';
            }

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
