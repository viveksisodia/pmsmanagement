(function () {
    'use strict';

    angular.module('pmsUserReportApp', ['pmsUserReport.services'])
        .directive('urHoursTooltip', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var timer, instance;
                    function hideTooltip() {
                        if (instance) instance.hide();
                    }
                    element.on('mouseleave', hideTooltip);
                    var stop = attrs.$observe('urHoursTooltip', function (title) {
                        $timeout.cancel(timer);
                        timer = $timeout(function () {
                            if (instance) { instance.dispose(); instance = null; }
                            var api = window.bootstrap;
                            if (!api || !api.Tooltip) {
                                element.attr('title', title || '');
                                return;
                            }
                            element.removeAttr('title');
                            element.removeAttr('data-bs-original-title');
                            instance = new api.Tooltip(element[0], {
                                title: title || '', trigger: 'hover', placement: 'top',
                                container: 'body', boundary: 'viewport', html: false, customClass: 'ur-hours-tooltip-popup'
                            });
                        }, 0, false);
                    });
                    scope.$on('$destroy', function () {
                        stop();
                        $timeout.cancel(timer);
                        hideTooltip();
                        element.off('mouseleave', hideTooltip);
                        if (instance) instance.dispose();
                    });
                }
            };
        }])
        .controller('UserReportController', ['$scope', '$window', 'UserReportService', '$timeout', function ($scope, $window, UserReportService, $timeout) {
            var vm = this;
            var latestLoadId = 0;
            var params = new URLSearchParams($window.location.search || '');
            vm.reportType = (params.get('type') || 'status').toLowerCase();
            vm.hoursPeriod = (params.get('hoursPeriod') || 'all').toLowerCase();
            vm.hoursSource = (params.get('hoursSource') || 'main').toLowerCase();
            vm.isIssues = vm.reportType === 'issues';
            vm.isHours = vm.reportType === 'hours';
            vm.isGrouped = !vm.isIssues && !vm.isHours;
            vm.detailLoading = false;
            vm.detailError = '';
            vm.totalRows = 0;
            var detailSequence = 0, searchTimer;
            vm.isTaskEfficiency = vm.reportType === 'taskefficiency';
            vm.isTimeEfficiency = vm.reportType === 'timeefficiency';
            vm.isEfficiencyReport = vm.isTaskEfficiency || vm.isTimeEfficiency;
            vm.title = vm.isIssues ? 'Issues Report' : (params.get('title') || 'Dashboard Report');
            vm.filterOptions = ['All', 'Today', 'This Week', 'This Month', 'This Quarter', 'This Year', 'Yesterday', 'Previous Week', 'Previous Month', 'Previous Quarter', 'Previous Year', 'Custom Range'];
            vm.filterType = params.get('filterType') || (params.get('fromDate') || params.get('toDate') ? 'Custom Range' : 'Today');
            vm.dashboardFilterType = vm.filterType;
            vm.taskPriority = 'ALL';
            vm.taskStatus = 'ALL';
            vm.taskPriorityOptions = [{ value: 'ALL', label: 'All Priorities' }];
            vm.taskStatusOptions = [{ value: 'ALL', label: 'All Statuses' }];
            vm.groupOptions = [
                { value: 'status', label: 'Status' },
                { value: 'priority', label: 'Priority' },
                { value: 'situation', label: 'Task Situation' },
                { value: 'tag', label: 'Tag' }
            ];
            vm.groupBy = vm.groupOptions.some(function (x) { return x.value === vm.reportType; }) ? vm.reportType : 'status';
            vm.groupLabel = '';
            vm.issueStatusOptions = [{ value: 'ALL', label: 'All Statuses' }];
            vm.issuePriorityOptions = [{ value: 'ALL', label: 'All Priorities' }];
            vm.issueEntityOptions = [
                { value: 'ALL', label: 'All Types' },
                { value: 'PROJECT', label: 'Project' },
                { value: 'MILESTONE', label: 'Milestone' },
                { value: 'TASK', label: 'Task' },
                { value: 'SUBTASK', label: 'Subtask' }
            ];
            vm.issueStatus = 'ALL';
            vm.issueCompletion = 'ALL';
            vm.issueEntityType = 'ALL';
            vm.issuePriority = 'ALL';
            vm.issueEmployee = '';
            vm.issueSearch = '';
            vm.employeeOptions = [];
            vm.pageSize = 5;
            vm.currentPage = 1;
            vm.rows = [];
            vm.allRows = [];
            vm.searchText = '';
            vm.days = [];
            vm.dailyTotals = [];
            vm.assignedTotalText = '00:00';
            vm.actualTotalText = '00:00';
            vm.groups = [];
            vm.selectedGroupName = null;
            vm.summary = { total: 0, completed: 0, completion: 0, timeEfficiency: 0 };
            vm.loading = false;
            vm.error = '';
            vm.groupOptions.forEach(function (item) { if (item.value === vm.groupBy) vm.groupLabel = item.label; });

            function parseDate(value, fallback) {
                if (!value) return new Date(fallback.getTime());
                var parts;
                if (/^\d{4}-\d{2}-\d{2}$/.test(value)) {
                    parts = value.split('-');
                    return new Date(Number(parts[0]), Number(parts[1]) - 1, Number(parts[2]));
                }
                if (/^\d{2}\/\d{2}\/\d{4}$/.test(value)) {
                    parts = value.split('/');
                    return new Date(Number(parts[2]), Number(parts[1]) - 1, Number(parts[0]));
                }
                // Calendar selections are date-only; timezone conversion is for timestamps only.
                var dotNet = /\/Date\((-?\d+)(?:[+-]\d{4})?\)\//.exec(String(value));
                var parsed = dotNet ? new Date(Number(dotNet[1])) : new Date(value);
                if (isNaN(parsed.getTime())) return dayStart(fallback);
                if (value instanceof Date) return dayStart(value);
                if (dotNet || /(?:Z|[+-]\d{2}:?\d{2})$/.test(String(value))) return globalDateOnly(parsed);
                return dayStart(parsed);
            }
            function asIso(date) {
                return date.getFullYear() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);
            }
            function toApiDate(date) {
                return ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();
            }
            function dayStart(date) { return new Date(date.getFullYear(), date.getMonth(), date.getDate()); }
            function shiftDays(date, delta) { var result = dayStart(date); result.setDate(result.getDate() + delta); return result; }
            function startOfWeek(date) { return shiftDays(date, -date.getDay()); }
            function endOfMonth(date) { return new Date(date.getFullYear(), date.getMonth() + 1, 0); }
            function startOfQuarter(date) { return new Date(date.getFullYear(), Math.floor(date.getMonth() / 3) * 3, 1); }
            function endOfQuarter(date) { return new Date(startOfQuarter(date).getFullYear(), startOfQuarter(date).getMonth() + 3, 0); }
            // Device-local calendar dates; do not convert date-only selections through UTC.
            function globalDateOnly(instant) { return dayStart(instant); }
            function dashboardToday() { return dayStart(new Date()); }
            function dateRangeForPreset(preset) {
                var today = dashboardToday();
                if (preset === 'All') return { from: new Date(2020, 0, 1), to: today };
                if (preset === 'Today') return { from: today, to: today };
                var weekStart = startOfWeek(today);
                if (preset === 'This Week') return { from: weekStart, to: shiftDays(weekStart, 6) };
                if (preset === 'This Month') return { from: new Date(today.getFullYear(), today.getMonth(), 1), to: endOfMonth(today) };
                if (preset === 'This Quarter') return { from: startOfQuarter(today), to: endOfQuarter(today) };
                if (preset === 'This Year') return { from: new Date(today.getFullYear(), 0, 1), to: new Date(today.getFullYear(), 11, 31) };
                if (preset === 'Yesterday') return { from: shiftDays(today, -1), to: shiftDays(today, -1) };
                if (preset === 'Previous Week') return { from: shiftDays(weekStart, -7), to: shiftDays(weekStart, -1) };
                if (preset === 'Previous Month') return { from: new Date(today.getFullYear(), today.getMonth() - 1, 1), to: new Date(today.getFullYear(), today.getMonth(), 0) };
                if (preset === 'Previous Quarter') { var quarterStart = startOfQuarter(today); return { from: new Date(quarterStart.getFullYear(), quarterStart.getMonth() - 3, 1), to: shiftDays(quarterStart, -1) }; }
                if (preset === 'Previous Year') return { from: new Date(today.getFullYear() - 1, 0, 1), to: new Date(today.getFullYear() - 1, 11, 31) };
                return { from: vm.fromDate, to: vm.toDate };
            }
            function selectDateRange() {
                if (vm.isHours && vm.hoursSource === 'user' && (vm.hoursPeriod === 'today' || vm.hoursPeriod === 'week')) {
                    var hoursToday = dashboardToday();
                    if (vm.hoursPeriod === 'today') return { from: hoursToday, to: hoursToday };
                    var hoursWeekStart = shiftDays(hoursToday, -((hoursToday.getDay() + 6) % 7));
                    return { from: hoursWeekStart, to: shiftDays(hoursWeekStart, 6) };
                }
                if (vm.isHours && (vm.filterType === 'This Week' || vm.filterType === 'Previous Week')) {
                    var monday = shiftDays(dashboardToday(), -((dashboardToday().getDay() + 6) % 7));
                    if (vm.filterType === 'Previous Week') monday = shiftDays(monday, -7);
                    return { from: monday, to: shiftDays(monday, 6) };
                }
                if (vm.filterType === 'Custom Range') return { from: vm.fromDate, to: vm.toDate };
                return dateRangeForPreset(vm.filterType);
            }

            vm.onFilterChange = function () {
                vm.taskPriority = 'ALL';
                vm.taskStatus = 'ALL';
                vm.selectedGroupName = null;
                vm.searchText = '';
                vm.issueSearch = '';
                if (vm.filterType !== 'Custom Range') vm.load();
            };

            function groupedPayload(mode, page, size) {
                var range = selectDateRange();
                return {
                    fromDate: toApiDate(range.from), toDate: toApiDate(range.to),
                    reportType: vm.reportType, groupBy: vm.groupBy,
                    groupName: mode === 'detail' ? vm.selectedGroupName : null,
                    priority: vm.taskPriority || 'ALL', status: vm.taskStatus || 'ALL',
                    search: mode === 'detail' ? (vm.searchText || '') : '', page: page || 1, pageSize: size || Number(vm.pageSize) || 5,
                    mode: mode
                };
            }
            vm.loadGroupedSummary = function () {
                $timeout.cancel(searchTimer);
                ++detailSequence;
                var requestId = ++latestLoadId;
                var initialLoading = document.getElementById('urInitialLoading');
                if (initialLoading) initialLoading.style.display = 'none';
                vm.loading = true; vm.error = ''; vm.detailError = ''; vm.detailLoading = false;
                vm.selectedGroupName = null; vm.rows = []; vm.allRows = []; vm.totalRows = 0;
                vm.currentPage = 1;
                vm.groupOptions.forEach(function (item) { if (item.value === vm.groupBy) vm.groupLabel = item.label; });
                var payload = groupedPayload('summary');
                return UserReportService.getGroupedReport(payload).then(function (data) {
                    if (requestId !== latestLoadId) return;
                    vm.groups = data.groups || [];
                    vm.summary = { total: Number(data.total) || 0, completed: Number(data.completed) || 0,
                        completion: Number(data.completion) || 0, timeEfficiency: Number(data.timeEfficiency) || 0 };
                    vm.taskPriorityOptions = [{ value: 'ALL', label: 'All Priorities' }].concat((data.priorityOptions || []).map(function (value) { return { value: value, label: value }; }));
                    vm.taskStatusOptions = [{ value: 'ALL', label: 'All Statuses' }].concat((data.statusOptions || []).map(function (value) { return { value: value, label: value }; }));
                    vm.fromDate = parseDate(data.fromDate, vm.fromDate || dashboardToday());
                    vm.toDate = parseDate(data.toDate, vm.fromDate);
                    vm.updatePaging();
                }).catch(function (error) {
                    if (requestId === latestLoadId) vm.error = error && error.status === 401 ? 'Please sign in again to view this report.' : 'Unable to load grouped report data. Please check the date range and try again.';
                }).finally(function () {
                    if (requestId === latestLoadId) { vm.loading = false; $scope.$evalAsync(); }
                });
            };
            vm.loadGroupDetails = function () {
                $timeout.cancel(searchTimer);
                var requestId = ++detailSequence;
                vm.detailLoading = true; vm.detailError = ''; vm.rows = [];
                return UserReportService.getGroupedReport(groupedPayload('detail', vm.currentPage)).then(function (data) {
                    if (requestId !== detailSequence) return;
                    vm.rows = data.rows || []; vm.totalRows = Number(data.total) || 0;
                    vm.currentPage = Number(data.page) || 1;
                    vm.updatePaging();
                }).catch(function () {
                    if (requestId === detailSequence) { vm.detailError = 'Unable to load this group. Please try again.'; vm.totalRows = 0; vm.updatePaging(); }
                }).finally(function () {
                    if (requestId === detailSequence) { vm.detailLoading = false; $scope.$evalAsync(); }
                });
            };
            $scope.$on('$destroy', function () { $timeout.cancel(searchTimer); ++latestLoadId; ++detailSequence; });

            vm.onGroupByChange = function () {
                if (vm.isGrouped) return vm.loadGroupedSummary();
                vm.selectedGroupName = null;
                vm.currentPage = 1;
                vm.rebuildGroups();
                vm.updatePaging();
            };

            vm.openGroup = function (name) {
                vm.searchText = '';
                vm.selectedGroupName = String(name || 'Unspecified');
                vm.currentPage = 1;
                if (vm.isGrouped) return vm.loadGroupDetails();
                vm.updatePaging();
            };

            vm.clearGroup = function () {
                $timeout.cancel(searchTimer); vm.searchText = '';
                if (vm.isGrouped) {
                    ++detailSequence; vm.detailLoading = false; vm.detailError = '';
                    vm.selectedGroupName = null; vm.rows = []; vm.totalRows = 0;
                    vm.currentPage = 1; vm.updatePaging(); return;
                }
                vm.selectedGroupName = null;
                vm.currentPage = 1;
                vm.updatePaging();
            };

            vm.visibleRows = function () {
                if (vm.isGrouped) return vm.rows || [];
                if (vm.isIssues || vm.isHours || vm.selectedGroupName === null) return vm.rows || [];
                var selected = String(vm.selectedGroupName || 'Unspecified');
                return (vm.rows || []).filter(function (row) {
                    if (vm.groupBy === 'tag') {
                        var tags = String(row.tags || '').split(',').map(function (tag) { return tag.trim(); }).filter(Boolean);
                        return tags.length ? tags.indexOf(selected) >= 0 : selected === 'Unspecified';
                    }
                    return String(row[vm.groupBy] || 'Unspecified') === selected;
                });
            };

            vm.rebuildGroups = function () {
                var map = Object.create(null);
                vm.groupOptions.forEach(function (item) { if (item.value === vm.groupBy) vm.groupLabel = item.label; });
                (vm.rows || []).forEach(function (row, rowIndex) {
                    var labels = vm.groupBy === 'tag' ? String(row.tags || '').split(',').map(function (x) { return x.trim(); }).filter(Boolean) : [row[vm.groupBy] || 'Unspecified'];
                    if (!labels.length) labels = ['Unspecified'];
                    labels.forEach(function (label) {
                        var key = String(label || 'Unspecified');
                        var group = map[key] || (map[key] = { name: key, count: 0, completed: 0, assigned: 0, spent: 0, efficiencies: Object.create(null) });
                        group.count++;
                        group.assigned += Number(row.assignedHours) || 0;
                        group.spent += Number(row.spentHours) || 0;
                        if (row.isComplete) group.completed++;
                        if (row.isComplete) {
                            var taskKey = row.subtaskId ? 'task-' + row.subtaskId : 'row-' + rowIndex;
                            if (!Object.prototype.hasOwnProperty.call(group.efficiencies, taskKey)) group.efficiencies[taskKey] = vm.isTaskEfficiency ? 100 : (Number(row.efficiency) || 0);
                        }
                    });
                });
                vm.groups = Object.keys(map).map(function (key) {
                    var group = map[key];
                    var efficiencyValues = Object.keys(group.efficiencies).map(function (taskKey) { return group.efficiencies[taskKey]; });
                    var average = efficiencyValues.length ? efficiencyValues.reduce(function (a, b) { return a + b; }, 0) / efficiencyValues.length : 0;
                    group.assignedText = formatHours(group.assigned);
                    group.spentText = group.spent > 0 ? formatHours(group.spent) : '-';
                    group.efficiencyText = vm.isTaskEfficiency ? formatPercent(group.count ? group.completed * 100 / group.count : 0) : (efficiencyValues.length ? formatPercent(average) : '-');
                    return group;
                }).sort(function (a, b) { return b.count - a.count || a.name.localeCompare(b.name); });
            };
            function formatPercent(value) {
                var number = Number(value);
                return isFinite(number) ? number.toFixed(1) + '%' : '-';
            }
            vm.efficiencyDisplay = function (row) {
                if (!row) return '-';
                if (vm.isTaskEfficiency) return formatPercent(row.isComplete ? 100 : 0);
                return formatPercent(row.efficiency);
            };
            vm.applyTaskFilters = function (resetGroup) {
                if (vm.isGrouped) {
                    if (resetGroup || vm.selectedGroupName === null) return vm.loadGroupedSummary();
                    vm.currentPage = 1; return vm.loadGroupDetails();
                }
                if (resetGroup) vm.selectedGroupName = null;
                var priority = String(vm.taskPriority || 'ALL').toLowerCase();
                var status = String(vm.taskStatus || 'ALL').toLowerCase();
                var search = String(vm.searchText || '').trim().toLowerCase();
                vm.rows = (vm.allRows || []).filter(function (row) {
                    var matchesText = !search || [row.projectName, row.taskName, row.description, row.priority, row.status, row.tags, row.startDate, row.endDate].join(' ').toLowerCase().indexOf(search) >= 0;
                    return matchesText && (priority === 'all' || String(row.priority || '').toLowerCase() === priority) &&
                        (status === 'all' || String(row.status || '').toLowerCase() === status);
                });
                vm.currentPage = 1;
                var completedRows = vm.rows.filter(function (row) { return !!row.isComplete; });
                var uniqueEfficiency = Object.create(null);
                completedRows.forEach(function (row, index) {
                    var key = row.subtaskId ? 'task-' + row.subtaskId : 'row-' + index;
                    if (!Object.prototype.hasOwnProperty.call(uniqueEfficiency, key)) uniqueEfficiency[key] = Number(row.efficiency) || 0;
                });
                var efficiencies = Object.keys(uniqueEfficiency).map(function (key) { return uniqueEfficiency[key]; });
                vm.summary = {
                    total: vm.rows.length,
                    completed: completedRows.length,
                    completion: vm.rows.length ? Math.round(completedRows.length * 100 / vm.rows.length) : 0,
                    timeEfficiency: efficiencies.length ? efficiencies.reduce(function (sum, value) { return sum + value; }, 0) / efficiencies.length : 0
                };
                vm.rebuildGroups();
                vm.updatePaging();
            };
            function durationMinutes(value) {
                var parts = String(value || '00:00').split(':');
                return (parseInt(parts[0], 10) || 0) * 60 + (parseInt(parts[1], 10) || 0);
            }
            vm.hoursDayTooltip = function (index) {
                var day = vm.days[index] || {};
                return 'Assigned Time : ' + (day.assignedText || '00:00') + ' hrs\nLunch Time : ' +
                    (day.lunchText || '00:00') + ' hrs\nTotal Spent Time : ' +
                    ((vm.dailyTotals || [])[index] || '00:00') + ' hrs' +
                    (day.leaveLabel ? '\nLeave : ' + day.leaveLabel : '');
            };
            vm.applyHoursFilters = function () {
                var search = String(vm.searchText || '').trim().toLowerCase();
                var priority = String(vm.taskPriority || 'ALL').trim().toLowerCase();
                var status = String(vm.taskStatus || 'ALL').trim().toLowerCase();
                vm.rows = (vm.allRows || []).filter(function (row) {
                    var matchesText = !search || [row.taskName, row.projectName].concat(row.dailyActual || []).join(' ').toLowerCase().indexOf(search) >= 0;
                    return matchesText && (priority === 'all' || String(row.priority || '').trim().toLowerCase() === priority) &&
                        (status === 'all' || String(row.status || '').trim().toLowerCase() === status);
                });
                vm.dailyTotals = vm.days.map(function (day, index) {
                    return formatHours(vm.rows.reduce(function (sum, row) { return sum + durationMinutes((row.dailyActual || [])[index]); }, 0) / 60);
                });
                vm.assignedTotalText = formatHours(vm.rows.reduce(function (sum, row) { return sum + (Number(row.assignedHours) || 0); }, 0));
                vm.actualTotalText = formatHours(vm.rows.reduce(function (sum, row) { return sum + (Number(row.actualHours) || 0); }, 0));
                vm.summary.total = vm.rows.length;
                vm.currentPage = 1;
                vm.updatePaging();
            };
            vm.applyGridSearch = function () {
                if (vm.isGrouped && vm.selectedGroupName === null) return;
                if (vm.isGrouped) {
                    $timeout.cancel(searchTimer);
                    ++latestLoadId; ++detailSequence;
                    searchTimer = $timeout(function () { vm.applyTaskFilters(false); }, 300);
                    return;
                }
                if (vm.isIssues) {
                    var issueText = String(vm.issueSearch || '').trim().toLowerCase();
                    var entityType = String(vm.issueEntityType || 'ALL').trim().toLowerCase();
                    var priority = String(vm.issuePriority || 'ALL').trim().toLowerCase();
                    var status = String(vm.issueStatus || 'ALL').trim().toLowerCase();
                    vm.rows = (vm.allRows || []).filter(function (row) {
                        var matchesText = !issueText || [row.issueName, row.serialNo, row.entityType, row.owner, row.team, row.status, row.priority, row.assignTo, row.issueType, row.projectName, row.milestoneName, row.description, row.taskName].join(' ').toLowerCase().indexOf(issueText) >= 0;
                        return matchesText && (entityType === 'all' || String(row.entityType || '').trim().toLowerCase() === entityType) &&
                            (priority === 'all' || String(row.priority || '').trim().toLowerCase() === priority) &&
                            (status === 'all' || String(row.status || '').trim().toLowerCase() === status);
                    });
                    vm.summary.total = vm.rows.length;
                    vm.summary.completed = vm.rows.filter(function (row) { return !!row.isComplete; }).length;
                    vm.summary.completion = vm.rows.length ? Math.round(vm.summary.completed * 100 / vm.rows.length) : 0;
                    vm.currentPage = 1;
                    vm.updatePaging();
                } else if (vm.isHours) vm.applyHoursFilters();
                else vm.applyTaskFilters();
            };
            function formatHours(hours) {
                var minutes = Math.round((Number(hours) || 0) * 60);
                return ('0' + Math.floor(minutes / 60)).slice(-2) + ':' + ('0' + (minutes % 60)).slice(-2);
            }
            vm.pageCount = 1;
            vm.pageStart = 0;
            vm.pageEnd = 0;
            vm.pageRows = function () {
                if (vm.isGrouped) return vm.rows || [];
                var size = Math.max(1, parseInt(vm.pageSize, 10) || 5);
                return vm.visibleRows().slice((vm.currentPage - 1) * size, vm.currentPage * size);
            };
            vm.updatePaging = function () {
                var size = Math.max(1, parseInt(vm.pageSize, 10) || 5);
                var visibleCount = vm.isGrouped ? vm.totalRows : vm.visibleRows().length;
                vm.pageCount = Math.max(1, Math.ceil(visibleCount / size));
                vm.currentPage = Math.min(Math.max(1, vm.currentPage), vm.pageCount);
                vm.pageStart = visibleCount ? ((vm.currentPage - 1) * size + 1) : 0;
                vm.pageEnd = Math.min(vm.currentPage * size, visibleCount);
            };
            vm.setPageSize = function () { vm.currentPage = 1; if (vm.isGrouped) return vm.loadGroupDetails(); vm.updatePaging(); };
            vm.previousPage = function () { if (vm.currentPage > 1) { vm.currentPage--; if (vm.isGrouped) return vm.loadGroupDetails(); } vm.updatePaging(); };
            vm.nextPage = function () { if (vm.currentPage < vm.pageCount) { vm.currentPage++; if (vm.isGrouped) return vm.loadGroupDetails(); } vm.updatePaging(); };
            vm.clearIssueFilters = function () {
                vm.issueEntityType = 'ALL';
                vm.issuePriority = 'ALL';
                vm.issueCompletion = 'ALL';
                vm.filterType = vm.dashboardFilterType;
                vm.issueStatus = 'ALL';
                vm.issueEmployee = '';
                vm.issueSearch = '';
                vm.fromDate = new Date(vm.dashboardFromDate.getTime());
                vm.toDate = new Date(vm.dashboardToDate.getTime());
                vm.load();
            };
            vm.clearTaskFilters = function () {
                $timeout.cancel(searchTimer); ++detailSequence;
                vm.filterType = initialTaskFilters.filterType;
                vm.fromDate = new Date(initialTaskFilters.fromDate.getTime());
                vm.toDate = new Date(initialTaskFilters.toDate.getTime());
                vm.taskPriority = initialTaskFilters.taskPriority;
                vm.taskStatus = initialTaskFilters.taskStatus;
                vm.groupBy = initialTaskFilters.groupBy;
                vm.searchText = ''; vm.issueSearch = '';
                vm.selectedGroupName = null; vm.currentPage = 1;
                vm.pageSize = initialTaskFilters.pageSize;
                vm.detailLoading = false; vm.detailError = ''; vm.error = '';
                return vm.load();
            };
            vm.clearReportSearch = function () {
                vm.searchText = '';
                vm.issueSearch = '';
                vm.applyGridSearch();
            };

            vm.load = function () {
                if (vm.isGrouped) return vm.loadGroupedSummary();
                var requestId = ++latestLoadId;
                var initialLoading = document.getElementById('urInitialLoading');
                if (initialLoading) initialLoading.style.display = 'none';
                vm.loading = true;
                vm.error = '';
                vm.selectedGroupName = null;
                var range = selectDateRange();
                var payload = {
                    fromDate: toApiDate(range.from),
                    toDate: toApiDate(range.to),
                    reportType: vm.reportType,
                    groupBy: vm.groupBy,
                    issueStatus: vm.issueStatus,
                    issueCompletion: 'ALL',
                    issueEntityType: vm.issueEntityType,
                    employeeSearch: vm.issueEmployee,
                    issueSearch: ''
                };
                UserReportService.getReport(payload).then(function (data) {
                    if (requestId !== latestLoadId) return;
                    vm.allRows = data.rows || [];
                    vm.rows = vm.allRows;
                    vm.days = data.days || [];
                    vm.dailyTotals = data.dailyTotals || [];
                    vm.assignedTotalText = data.assignedTotalText || '00:00';
                    vm.actualTotalText = data.actualTotalText || '00:00';
                    vm.currentPage = 1;
                    if (!vm.isIssues) {
                        vm.taskPriorityOptions = [{ value: 'ALL', label: 'All Priorities' }].concat((data.priorityOptions || []).map(function (value) { return { value: value, label: value }; }));
                        vm.taskStatusOptions = [{ value: 'ALL', label: 'All Statuses' }].concat((data.statusOptions || []).map(function (value) { return { value: value, label: value }; }));
                    } else if (vm.isIssues) {
                        vm.issuePriorityOptions = [{ value: 'ALL', label: 'All Priorities' }].concat((data.priorityOptions || []).map(function (value) { return { value: value, label: value }; }));
                        vm.issueStatusOptions = [{ value: 'ALL', label: 'All Statuses' }].concat((data.statusOptions || []).map(function (value) { return { value: value, label: value }; }));
                    }
                    var employees = Object.create(null);
                    vm.rows.forEach(function (row) {
                        [row.owner, row.assignTo].concat(String(row.team || '').split(/[,;|]/)).forEach(function (item) {
                            var value = String(item || '').trim();
                            if (value) employees[value] = true;
                        });
                    });
                    vm.employeeOptions = data.employeeOptions || Object.keys(employees).sort();
                    vm.summary = {
                        total: Number(data.total) || vm.rows.length,
                        completed: Number(data.completed) || 0,
                        completion: Number(data.completion) || 0,
                        timeEfficiency: Number(data.timeEfficiency) || 0
                    };
                    vm.fromDate = parseDate(data.fromDate, range.from);
                    vm.toDate = parseDate(data.toDate, range.to);
                    if (!vm.isIssues && !vm.isHours) vm.applyTaskFilters(true);
                    else if (vm.isHours) vm.applyHoursFilters();
                    else { vm.applyGridSearch(); }
                }).catch(function (error) {
                    if (requestId !== latestLoadId) return;
                    vm.error = error && error.status === 401 ? 'Please sign in again to view this report.' : 'Unable to load the report. Please check the date range and try again.';
                }).finally(function () {
                    if (requestId !== latestLoadId) return;
                    vm.loading = false;
                    $scope.$evalAsync();
                });
            };

            vm.exportExcel = function () {
                if (vm.loading || vm.detailLoading || vm.exportLoading) return;
                if (!vm.isIssues && !vm.isHours && vm.selectedGroupName === null) return downloadExcel(vm.groups, true);
                if (!vm.isGrouped) return downloadExcel(vm.visibleRows());
                if (vm.exportLoading) return;
                vm.exportLoading = true; vm.detailError = '';
                var payload = groupedPayload('detail', 1, 100), collected = [];
                function fetchNext() {
                    return UserReportService.getGroupedReport(payload).then(function (data) {
                        var batch = data.rows || [];
                        collected = collected.concat(batch);
                        if (batch.length && collected.length < Number(data.total)) {
                            payload.page++; return fetchNext();
                        }
                        downloadExcel(collected);
                    });
                }
                return fetchNext().catch(function () { vm.detailError = 'Unable to export the report. Please try again.'; })
                    .finally(function () { vm.exportLoading = false; $scope.$evalAsync(); });
            };
function excelWorkbook(columns,rows){
function xml(v){return String(v===null||v===undefined?'':v).replace(/[\x00-\x08\x0B\x0C\x0E-\x1F]/g,'').replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');}
function col(n){var s='';for(n++;n;n=Math.floor((n-1)/26))s=String.fromCharCode(65+(n-1)%26)+s;return s;}
function row(values,n){return '<row r="'+n+'">'+values.map(function(v,i){var ref=col(i)+n;return typeof v==='number'&&isFinite(v)?'<c r="'+ref+'"><v>'+v+'</v></c>':'<c r="'+ref+'" t="inlineStr"><is><t xml:space="preserve">'+xml(v)+'</t></is></c>';}).join('')+'</row>';}
var sheet='<?xml version="1.0" encoding="UTF-8"?><worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"><sheetData>'+row(columns,1)+rows.map(function(r,i){return row(columns.map(function(c){return r[c];}),i+2);}).join('')+'</sheetData></worksheet>';
var files=[['[Content_Types].xml','<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/><Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/></Types>'],['_rels/.rels','<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/></Relationships>'],['xl/workbook.xml','<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"><sheets><sheet name="Report" sheetId="1" r:id="rId1"/></sheets></workbook>'],['xl/_rels/workbook.xml.rels','<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/></Relationships>'],['xl/worksheets/sheet1.xml',sheet]];
var encoder=new TextEncoder(),parts=[],central=[],offset=0,centralSize=0;
function crc(data){var c=0xffffffff;for(var i=0;i<data.length;i++){c^=data[i];for(var j=0;j<8;j++)c=(c>>>1)^((c&1)?0xedb88320:0);}return (c^0xffffffff)>>>0;}
function header(size){var a=new Uint8Array(size);return {a:a,v:new DataView(a.buffer)};}
files.forEach(function(f){var name=encoder.encode(f[0]),data=encoder.encode(f[1]),sum=crc(data),h=header(30);h.v.setUint32(0,0x04034b50,true);h.v.setUint16(4,20,true);h.v.setUint16(12,33,true);h.v.setUint32(14,sum,true);h.v.setUint32(18,data.length,true);h.v.setUint32(22,data.length,true);h.v.setUint16(26,name.length,true);parts.push(h.a,name,data);var c=header(46);c.v.setUint32(0,0x02014b50,true);c.v.setUint16(4,20,true);c.v.setUint16(6,20,true);c.v.setUint16(14,33,true);c.v.setUint32(16,sum,true);c.v.setUint32(20,data.length,true);c.v.setUint32(24,data.length,true);c.v.setUint16(28,name.length,true);c.v.setUint32(42,offset,true);central.push(c.a,name);centralSize+=46+name.length;offset+=30+name.length+data.length;});
var end=header(22);end.v.setUint32(0,0x06054b50,true);end.v.setUint16(8,files.length,true);end.v.setUint16(10,files.length,true);end.v.setUint32(12,centralSize,true);end.v.setUint32(16,offset,true);return new Blob(parts.concat(central,[end.a]),{type:'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
}
            function downloadExcel(exportRows, summaryOnly) {
                var columns;
                if (vm.isHours) {
                    columns = [['Task', 'taskName'], ['Project Name', 'projectName']].concat(vm.days.map(function (day, index) { return [day.weekday + ' ' + day.dateLabel, 'dailyActual.' + index]; })).concat([['Assigned Time', 'assignedText'], ['Actual Time', 'actualText']]);
                } else if (vm.isIssues) {
                    columns = [['Issue', 'issueName'], ['Sr. No.', 'serialNo'], ['Type', 'entityType'], ['Issue Type', 'issueType'], ['Status', 'status'], ['Priority', 'priority'], ['Owner', 'owner'], ['Team', 'team'], ['Assign To', 'assignTo'], ['Project', 'projectName'], ['Milestone', 'milestoneName'], ['Create Date', 'createDate'], ['Target Date', 'targetDate']];
                } else {
                    columns = [['Project Name', 'projectName'], ['Task', 'taskName'], ['Description', 'description'], ['Priority', 'priority'], ['Status', 'status'], ['Total Completion', 'completion'], ['Assigned Time', 'assignedText'], ['Spent Time', 'spentText']];
                    if (vm.isEfficiencyReport) columns.push(['Efficiency', 'efficiencyText']);
                    columns = columns.concat([['Tags', 'tags'], ['Start Date', 'startDate'], ['End Date', 'endDate']]);
                }
                if (summaryOnly) {
                    columns = [[vm.groupLabel, 'name'], ['Count', 'count'], ['Assigned Time', 'assignedText'], ['Spent Time', 'spentText']];
                    if (vm.isEfficiencyReport) columns.push(['Efficiency', 'efficiencyText']);
                }
                function escapeHtml(value) {
                    return String(value == null ? '' : value).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/\"/g, '&quot;');
                }
                function cellValue(row, key) {
                    if (!summaryOnly && key === 'efficiencyText') return row.efficiency == null && !vm.isTaskEfficiency ? '-' : vm.efficiencyDisplay(row);
                    if (key.indexOf('dailyActual.') === 0) return (row.dailyActual || [])[Number(key.split('.')[1])] || '00:00';
                    return row[key];
                }
                var headers = columns.map(function (column) { return column[0]; });
                var workbookRows = exportRows.map(function (row) {
                    var values = {}; columns.forEach(function (column) { values[column[0]] = cellValue(row, column[1]); }); return values;
                });
                var blob = excelWorkbook(headers, workbookRows);
                var link = document.createElement('a');
                var url = URL.createObjectURL(blob);
                link.href = url;
                link.download = 'user-dashboard-' + vm.reportType + (summaryOnly ? '-Groups' : '-Details') + '.xlsx';
                document.body.appendChild(link); link.click(); document.body.removeChild(link);
                $timeout(function () { URL.revokeObjectURL(url); }, 1000);
            };

            var now = dashboardToday();
            vm.fromDate = parseDate(params.get('fromDate'), now);
            vm.toDate = parseDate(params.get('toDate'), vm.fromDate);
            vm.dashboardFromDate = new Date(vm.fromDate.getTime());
            vm.dashboardToDate = new Date(vm.toDate.getTime());
            if (vm.reportType === 'hours' && vm.hoursSource === 'user' && (vm.hoursPeriod === 'today' || vm.hoursPeriod === 'week')) {
                var hoursToday = dashboardToday();
                if (vm.hoursPeriod === 'today') {
                    vm.fromDate = new Date(hoursToday.getTime());
                    vm.toDate = new Date(hoursToday.getTime());
                } else {
                    var hoursWeekStart = shiftDays(hoursToday, -((hoursToday.getDay() + 6) % 7));
                    vm.fromDate = hoursWeekStart;
                    vm.toDate = shiftDays(hoursWeekStart, 6);
                }
            }
            var initialTaskFilters = {
                filterType: vm.filterType, fromDate: new Date(vm.fromDate.getTime()), toDate: new Date(vm.toDate.getTime()),
                taskPriority: vm.taskPriority, taskStatus: vm.taskStatus, groupBy: vm.groupBy, pageSize: vm.pageSize
            };
            var reportTop = null;
            function sizeUserReport() {
                var shell = document.getElementById('user-report-shell');
                if (!shell || vm.loading) return;
                if (reportTop === null) reportTop = shell.getBoundingClientRect().top;
                var parent = shell.parentElement.getBoundingClientRect();
                shell.style.position = 'fixed'; shell.style.top = reportTop + 'px';
                shell.style.left = Math.max(0, parent.left) + 'px'; shell.style.width = parent.width + 'px';
                shell.style.height = Math.max(120, $window.innerHeight - reportTop - 8) + 'px';
            }
            $window.addEventListener('resize', sizeUserReport);
            $scope.$watch(function () { return vm.loading; }, function (loading) { if (!loading) $timeout(sizeUserReport, 0); });
            $scope.$on('$destroy', function () { $window.removeEventListener('resize', sizeUserReport); });
            vm.load();
        }]);
}());
