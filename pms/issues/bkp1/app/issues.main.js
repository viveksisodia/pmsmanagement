(function () {
    'use strict';

    angular.module('pmsIssuesApp', [])
        .controller('IssuesController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
            var page = window.IssuesPage || {};
            var requestToken = 0;
            var reloadTimer = null;

            $scope.isadmin = parseInt(page.isadmin || 0, 10) || 0;
            $scope.loading = false;
            $scope.error = '';
            $scope.rows = [];
            $scope.filteredRows = [];
            $scope.totalRows = 0;
            $scope.currentPage = 1;
            $scope.pageSize = 25;
            $scope.pageSizeOptions = [10, 25, 50, 100];
            $scope.totalPages = 1;
            $scope.pageNumbers = [];
            $scope.startRow = 0;
            $scope.endRow = 0;
            $scope.searchText = '';
            $scope.employeeOptions = [];
            $scope.selectedEmployeeIds = [];
            $scope.employeeSearch = '';
            $scope.employeeActiveIndex = -1;
            $scope.employeeMenuOpen = false;
            $scope.employeeError = '';

            $scope.entityTypes = [
                { Id: 'ALL', Name: 'All Types' },
                { Id: 'PROJECT', Name: 'Project' },
                { Id: 'MILESTONE', Name: 'Milestone' },
                { Id: 'TASK', Name: 'Task' },
                { Id: 'SUBTASK', Name: 'Subtask' }
            ];
            $scope.dateFilters = [
                { Id: 'ALL', Name: 'All Dates' }, { Id: 'TODAY', Name: 'Today' },
                { Id: 'THISWEEK', Name: 'This Week' }, { Id: 'THISMONTH', Name: 'This Month' },
                { Id: 'THISQUARTER', Name: 'This Quarter' }, { Id: 'THISYEAR', Name: 'This Year' },
                { Id: 'YESTERDAY', Name: 'Yesterday' }, { Id: 'PREVIOUSWEEK', Name: 'Previous Week' },
                { Id: 'PREVIOUSMONTH', Name: 'Previous Month' }, { Id: 'PREVIOUSQUARTER', Name: 'Previous Quarter' },
                { Id: 'PREVIOUSYEAR', Name: 'Previous Year' }, { Id: 'CUSTOM', Name: 'Custom Range' }
            ];
            $scope.issueStatusFilters = [
                { Id: 'PENDING', Name: 'Pending Issue' },
                { Id: 'ALL', Name: 'All Issue' },
                { Id: 'COMPLETE', Name: 'Complete Issue' }
            ];
            $scope.filter = { EntityType: 'ALL', DateFilter: 'ALL', FromDate: null, ToDate: null, IssueStatus: 'PENDING' };

            function unwrap(response) {
                var value = response && response.data ? response.data.d : null;
                if (angular.isString(value)) {
                    try { value = JSON.parse(value); } catch (ignore) { }
                }
                return value || {};
            }
            function toApiDate(date) {
                if (!(date instanceof Date) || isNaN(date.getTime())) return '';
                return date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);
            }
            function day(date) { return new Date(date.getFullYear(), date.getMonth(), date.getDate()); }
            function addDays(date, count) { var result = day(date); result.setDate(result.getDate() + count); return result; }
            function monday(date) { var result = day(date); var offset = result.getDay() === 0 ? -6 : 1 - result.getDay(); return addDays(result, offset); }
            function getDateRange() {
                var id = $scope.filter.DateFilter;
                var today = day(new Date());
                var start = null, end = null, weekStart;
                if (id === 'ALL') return { from: '', to: '' };
                if (id === 'CUSTOM') return { from: toApiDate($scope.filter.FromDate), to: toApiDate($scope.filter.ToDate) };
                if (id === 'TODAY') start = end = today;
                else if (id === 'YESTERDAY') start = end = addDays(today, -1);
                else if (id === 'THISWEEK') { start = monday(today); end = addDays(start, 6); }
                else if (id === 'PREVIOUSWEEK') { end = addDays(monday(today), -1); start = addDays(end, -6); }
                else if (id === 'THISMONTH') { start = new Date(today.getFullYear(), today.getMonth(), 1); end = new Date(today.getFullYear(), today.getMonth() + 1, 0); }
                else if (id === 'PREVIOUSMONTH') { start = new Date(today.getFullYear(), today.getMonth() - 1, 1); end = new Date(today.getFullYear(), today.getMonth(), 0); }
                else if (id === 'THISQUARTER') { weekStart = Math.floor(today.getMonth() / 3) * 3; start = new Date(today.getFullYear(), weekStart, 1); end = new Date(today.getFullYear(), weekStart + 3, 0); }
                else if (id === 'PREVIOUSQUARTER') { weekStart = Math.floor(today.getMonth() / 3) * 3; end = new Date(today.getFullYear(), weekStart, 0); start = new Date(end.getFullYear(), Math.floor(end.getMonth() / 3) * 3, 1); }
                else if (id === 'THISYEAR') { start = new Date(today.getFullYear(), 0, 1); end = new Date(today.getFullYear(), 11, 31); }
                else if (id === 'PREVIOUSYEAR') { start = new Date(today.getFullYear() - 1, 0, 1); end = new Date(today.getFullYear() - 1, 11, 31); }
                return { from: toApiDate(start), to: toApiDate(end) };
            }
            function rebuildSearch() {
                var query = String($scope.searchText || '').trim().toLowerCase();
                $scope.filteredRows = ($scope.rows || []).filter(function (row) {
                    if (!query) return true;
                    return [row.Id, row.HierarchySrNo, row.EntityType, row.IssueName, row.OwnerName, row.TeamNames,
                        row.StatusName, row.PriorityName, row.AssigneeName, row.CreateDate, row.TargetDate,
                        row.IssueType, row.ProjectName, row.MilestoneName, row.Description].join(' ').toLowerCase().indexOf(query) >= 0;
                });
            }
            function setPager(data) {
                $scope.totalRows = parseInt(data.TotalRows || 0, 10);
                $scope.totalPages = Math.max(1, parseInt(data.TotalPages || 1, 10));
                $scope.currentPage = parseInt(data.PageNumber || $scope.currentPage || 1, 10);
                $scope.rows = data.Rows || [];
                $scope.pageNumbers = [];
                var from = Math.max(1, $scope.currentPage - 2);
                var to = Math.min($scope.totalPages, from + 4);
                from = Math.max(1, to - 4);
                for (var p = from; p <= to; p++) $scope.pageNumbers.push(p);
                $scope.startRow = $scope.totalRows ? ((($scope.currentPage - 1) * $scope.pageSize) + 1) : 0;
                $scope.endRow = Math.min($scope.totalRows, $scope.currentPage * $scope.pageSize);
                rebuildSearch();
            }

            $scope.loadEmployeeOptions = function () {
                $scope.employeeError = '';
                $http.post('default.aspx/GetIssueEmployees', {}).then(function (response) {
                    var data = unwrap(response);
                    if (!data.Success) {
                        $scope.employeeOptions = [];
                        $scope.employeeError = data.Message || 'Unable to load employees.';
                        return;
                    }
                    $scope.employeeOptions = data.Employees || [];
                }, function (error) {
                    $scope.employeeOptions = [];
                    $scope.employeeError = (error.data && (error.data.Message || error.data.ExceptionMessage)) || 'Unable to load employees.';
                });
            };
            $scope.toggleEmployeeMenu = function (event) {
                if (event) event.stopPropagation();
                $scope.employeeMenuOpen = !$scope.employeeMenuOpen;
            };
            $scope.openEmployeeMenu = function (event) {
                if (event) event.stopPropagation();
                $scope.employeeMenuOpen = true;
            };
            $scope.closeEmployeeMenu = function (event) {
                if (event && event.stopPropagation) event.stopPropagation();
                $scope.employeeMenuOpen = false;
            };
            $scope.isEmployeeSelected = function (employeeId) {
                return $scope.selectedEmployeeIds.indexOf(String(employeeId)) >= 0;
            };
            $scope.getSelectedEmployees = function () {
                return ($scope.employeeOptions || []).filter(function (employee) {
                    return $scope.isEmployeeSelected(employee.Id);
                });
            };
            $scope.getAvailableEmployeeOptions = function () {
                return ($scope.employeeOptions || []).filter(function (employee) {
                    return !$scope.isEmployeeSelected(employee.Id);
                });
            };
            $scope.getFilteredEmployeeOptions = function () {
                var query = String($scope.employeeSearch || '').trim().toLowerCase();
                return $scope.getAvailableEmployeeOptions().filter(function (employee) {
                    return !query || String(employee.Name || '').toLowerCase().indexOf(query) >= 0;
                });
            };
            $scope.resetEmployeeActiveIndex = function () {
                $scope.employeeActiveIndex = -1;
            };
            $scope.employeeSearchKeydown = function (event) {
                var options = $scope.getFilteredEmployeeOptions();
                var key = event.which || event.keyCode;
                if (key === 40 || key === 38) {
                    event.preventDefault();
                    event.stopPropagation();
                    $scope.employeeMenuOpen = true;
                    if (!options.length) { $scope.employeeActiveIndex = -1; return; }
                    if ($scope.employeeActiveIndex < 0) {
                        $scope.employeeActiveIndex = key === 40 ? 0 : options.length - 1;
                    } else if (key === 40) {
                        $scope.employeeActiveIndex = Math.min(options.length - 1, $scope.employeeActiveIndex + 1);
                    } else {
                        $scope.employeeActiveIndex = Math.max(0, $scope.employeeActiveIndex - 1);
                    }
                    return;
                }
                if (key === 13) {
                    event.preventDefault();
                    event.stopPropagation();
                    if (options.length && $scope.employeeMenuOpen) {
                        var index = $scope.employeeActiveIndex;
                        if (index < 0 && options.length === 1) index = 0;
                        if (index >= 0 && index < options.length) $scope.toggleEmployee(options[index].Id, event);
                    }
                    return;
                }
                if (key === 27) {
                    event.preventDefault();
                    event.stopPropagation();
                    $scope.employeeMenuOpen = false;
                    $scope.employeeActiveIndex = -1;
                }
            };
            $scope.toggleEmployee = function (employeeId, event) {
                if (event) event.stopPropagation();
                var id = String(employeeId);
                var index = $scope.selectedEmployeeIds.indexOf(id);
                if (index >= 0) $scope.selectedEmployeeIds.splice(index, 1);
                else $scope.selectedEmployeeIds.push(id);
                $scope.employeeSearch = '';
                $scope.employeeActiveIndex = -1;
                $scope.employeeMenuOpen = true;
                $timeout(function () {
                    var input = document.getElementById('employeeSearchInput');
                    if (input) input.focus();
                }, 0);
            };
            $scope.unselectEmployee = function (employeeId, event) {
                if (event) event.stopPropagation();
                var index = $scope.selectedEmployeeIds.indexOf(String(employeeId));
                if (index >= 0) $scope.selectedEmployeeIds.splice(index, 1);
                $scope.employeeSearch = '';
                $scope.employeeActiveIndex = -1;
                $timeout(function () {
                    var input = document.getElementById('employeeSearchInput');
                    if (input) input.focus();
                }, 0);
                if ($scope.selectedEmployeeIds.length === 0) {
                    $scope.currentPage = 1;
                    $scope.load();
                }
            };
            $scope.applyEmployeeFilter = function () {
                $scope.employeeMenuOpen = false;
                $scope.employeeSearch = '';
                $scope.currentPage = 1;
                $scope.load();
            };
            $scope.clearEmployeeFilter = function () {
                $scope.selectedEmployeeIds = [];
                $scope.employeeSearch = '';
                $scope.employeeMenuOpen = false;
                $scope.currentPage = 1;
                $scope.load();
            };

            $scope.load = function () {
                if ($scope.filter.DateFilter === 'CUSTOM') {
                    if (!$scope.filter.FromDate || !$scope.filter.ToDate) { $scope.error = 'Choose both dates for the custom range.'; return; }
                    if (day($scope.filter.FromDate) > day($scope.filter.ToDate)) { $scope.error = 'From date must be on or before To date.'; return; }
                }
                var range = getDateRange();
                var token = ++requestToken;
                $scope.loading = true;
                $scope.error = '';
                $http.post('default.aspx/GetIssuesGrid', { request: {
                    EntityType: $scope.filter.EntityType || 'ALL',
                    IssueStatus: $scope.filter.IssueStatus || 'PENDING',
                    FromDate: range.from,
                    ToDate: range.to,
                    EmployeeIds: $scope.selectedEmployeeIds.map(function (id) { return parseInt(id, 10); }),
                    PageNumber: $scope.currentPage,
                    PageSize: $scope.pageSize
                }}).then(function (response) {
                    if (token !== requestToken) return;
                    var data = unwrap(response);
                    if (!data.Success) { $scope.rows = []; $scope.filteredRows = []; $scope.totalRows = 0; $scope.error = data.Message || 'Unable to load issues.'; return; }
                    $scope.isadmin = parseInt(data.IsAdmin || 0, 10) || 0;
                    setPager(data);
                }, function (error) {
                    if (token !== requestToken) return;
                    $scope.rows = []; $scope.filteredRows = []; $scope.totalRows = 0;
                    $scope.error = (error.data && (error.data.Message || error.data.ExceptionMessage)) || 'Unable to load issues.';
                }).finally(function () {
                    if (token === requestToken) $scope.loading = false;
                });
            };
            $scope.filterChanged = function () {
                if (reloadTimer) $timeout.cancel(reloadTimer);
                reloadTimer = $timeout(function () { reloadTimer = null; $scope.currentPage = 1; $scope.load(); }, 0);
            };
            $scope.dateFilterChanged = function () {
                if ($scope.filter.DateFilter !== 'CUSTOM') { $scope.filter.FromDate = null; $scope.filter.ToDate = null; }
                $scope.filterChanged();
            };
            $scope.clearFilters = function () {
                $scope.filter = { EntityType: 'ALL', DateFilter: 'ALL', FromDate: null, ToDate: null, IssueStatus: 'PENDING' };
                $scope.searchText = '';
                $scope.selectedEmployeeIds = [];
                $scope.employeeSearch = '';
                $scope.employeeMenuOpen = false;
                $scope.currentPage = 1;
                $scope.load();
            };
            $scope.changePageSize = function () { $scope.currentPage = 1; $scope.load(); };
            $scope.goToPage = function (pageNo) { pageNo = parseInt(pageNo || 1, 10); if (pageNo < 1 || pageNo > $scope.totalPages || pageNo === $scope.currentPage) return; $scope.currentPage = pageNo; $scope.load(); };
            $scope.entityClass = function (type) { type = String(type || '').toUpperCase(); return 'issues-type-' + (type === 'SUBTASK' ? 'subtask' : type.toLowerCase()); };
            $scope.statusClass = function (complete) { return parseInt(complete || 0, 10) === 1 ? 'issues-status-complete' : 'issues-status-pending'; };
            $scope.$watch('searchText', rebuildSearch);
            $scope.loadEmployeeOptions();
            $scope.load();
        }]);
}());
