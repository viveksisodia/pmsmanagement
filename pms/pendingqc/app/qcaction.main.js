(function () {
    'use strict';

    if (!window.angular) {
        if (window.console && console.error) console.error('AngularJS is not loaded.');
        return;
    }

    var app;
    try {
        app = angular.module('qcActionApp');
    } catch (e) {
        app = angular.module('qcActionApp', []);
    }

    app.controller('qcActionCtrl', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
        var page = window.QCActionPage || {};

        $scope.isadmin = parseInt(page.isadmin || 0, 10) || 0;
        $scope.employeeId = parseInt(page.employeeId || 0, 10) || 0;
        $scope.regid = parseInt(page.regid || 0, 10) || 0;
        $scope.Math = window.Math;

        $scope.loading = false;
        $scope.error = '';
        $scope.success = '';
        $scope.rows = [];
        $scope.totalRows = 0;
        $scope.gridSearchText = '';
        $scope.filteredRows = [];
        $scope.pagedRows = [];
        $scope.pageSizeOptions = [10, 25, 50, 100];
        $scope.pageSize = 10;
        $scope.currentPage = 1;
        $scope.totalPages = 1;
        var loadToken = 0;
        var filterReloadPromise = null;

        $scope.historyPopup = { Show: false, Row: {}, Rows: [] };
        $scope.openHistory = function (row) {
            $scope.historyPopup = { Show: true, Row: row, Rows: row.History || [] };
        };
        $scope.selectPendingRuntime = function (row) {
            var list = row.PendingRuntimes || [];
            for (var i = 0; i < list.length; i++) {
                if (String(list[i].RuntimeId) === String(row.SelectedRuntimeId)) {
                    row.RuntimeId = list[i].RuntimeId;
                    row.Actions = list[i].Actions || [];
                    row.SelectedRuntimeId = row.RuntimeId;
                    row.SelectedActionId = '';
                    row.Remark = '';
                    break;
                }
            }
        };

        $scope.actionPopup = {
            Show: false,
            Row: {},
            Action: {},
            Remark: ''
        };

        $scope.entityTypes = [
            { Id: 'ALL', Name: 'All Types' },
            { Id: 'PROJECT', Name: 'Project' },
            { Id: 'MILESTONE', Name: 'Milestone' },
            { Id: 'TASK', Name: 'Task' },
            { Id: 'SUBTASK', Name: 'Sub Task' }
        ];

        $scope.dateFilters = [
            { Id: 'ALL', Name: 'All' },
            { Id: 'TODAY', Name: 'Today' },
            { Id: 'THISWEEK', Name: 'This Week' },
            { Id: 'THISMONTH', Name: 'This Month' },
            { Id: 'THISQUARTER', Name: 'This Quarter' },
            { Id: 'THISYEAR', Name: 'This Year' },
            { Id: 'YESTERDAY', Name: 'Yesterday' },
            { Id: 'PREVIOUSWEEK', Name: 'Previous Week' },
            { Id: 'PREVIOUSMONTH', Name: 'Previous Month' },
            { Id: 'PREVIOUSQUARTER', Name: 'Previous Quarter' },
            { Id: 'PREVIOUSYEAR', Name: 'Previous Year' },
            { Id: 'CUSTOM', Name: 'Custom Range' }
        ];

        $scope.statusFilters = [
            { Id: 'ALL', Name: 'All QC Status' },
            { Id: 'PENDING', Name: 'Pending Action' },
            { Id: 'ACTIONTAKEN', Name: 'Action Taken' },
            
        ];
//{ Id: 'CLOSED', Name: 'Closed' }
        $scope.filter = {
            EntityType: 'ALL',
            SearchText: '',
            DateFilter: 'ALL',
            FromDate: null,
            ToDate: null,
            QCStatus: 'PENDING'
        };

        function unwrapWebMethod(response) {
            return response && response.data && angular.isDefined(response.data.d)
                ? response.data.d
                : (response ? response.data : null);
        }

        $scope.load = function (preserveFeedback) {
            if (!preserveFeedback) {
                $scope.error = '';
                $scope.success = '';
            }

            if ($scope.filter.DateFilter === 'CUSTOM') {
                if (!$scope.filter.FromDate || !$scope.filter.ToDate) {
                    $scope.error = 'Please select From Date and To Date.';
                    return;
                }
                if ($scope.filter.FromDate.getTime() > $scope.filter.ToDate.getTime()) {
                    $scope.error = 'From Date cannot be greater than To Date.';
                    return;
                }
            }

            var currentLoadToken = ++loadToken;
            $scope.loading = true;

            function toDateText(value) {
                if (!value || !angular.isDate(value)) return '';
                var y = value.getFullYear();
                var m = ('0' + (value.getMonth() + 1)).slice(-2);
                var d = ('0' + value.getDate()).slice(-2);
                return y + '-' + m + '-' + d;
            }

            // Read the latest requested page from DB. Free search remains local
            // to the page already returned by the server.
            var request = {
                EntityType: $scope.filter.EntityType || 'ALL',
                SearchText: '',
                DateFilter: $scope.filter.DateFilter || 'ALL',
                FromDate: toDateText($scope.filter.FromDate),
                ToDate: toDateText($scope.filter.ToDate),
                QCStatus: $scope.filter.QCStatus || 'ALL',
                PageNumber: $scope.currentPage,
                PageSize: parseInt($scope.pageSize || 10, 10)
            };

            $http.post('default.aspx/GetQCActionGrid', {
                model: request
            }).then(function (response) {
                // Ignore an older response when the user changes filters quickly.
                if (currentLoadToken !== loadToken) return;

                var data = unwrapWebMethod(response) || {};
                if (!data.Success) {
                    $scope.error = data.Message || 'Unable to load QC list.';
                    if (window.console && console.error) {
                        console.error('QC grid load failed:', $scope.error, data);
                    }
                    $scope.rows = [];
                    $scope.totalRows = 0;
                    $scope.applyLocalSearch();
                    return;
                }

                $scope.isadmin = parseInt(data.IsAdmin || 0, 10) || 0;
                $scope.rows = data.Rows || [];
                $scope.totalRows = parseInt(data.TotalRows || $scope.rows.length || 0, 10);

                angular.forEach($scope.rows, function (row) {
                    row.SelectedRuntimeId = row.RuntimeId;
                    row.SelectedActionId = '';
                    row.Remark = '';
                    row.ActionBusy = false;
                });

                // Rebuild the visible grid immediately after every server filter response.
                // This prevents a previous empty result (for example ACTION TAKEN) from
                // leaving the ng-repeat list empty when the user switches back to PENDING.
                $scope.applyLocalSearch();
            }, function (error) {
                if (currentLoadToken !== loadToken) return;
                $scope.rows = [];
                $scope.totalRows = 0;
                $scope.applyLocalSearch();
                $scope.error = (error.data && error.data.Message) || 'Unable to load QC list.';
                if (window.console && console.error) {
                    console.error('QC grid HTTP load failed:', $scope.error, error);
                }
            }).finally(function () {
                if (currentLoadToken === loadToken)
                    $scope.loading = false;
            });
        };

        $scope.clearFilters = function () {
            $scope.filter.EntityType = 'ALL';
            $scope.filter.SearchText = '';
            $scope.gridSearchText = '';
            $scope.filter.DateFilter = 'ALL';
            $scope.filter.FromDate = null;
            $scope.filter.ToDate = null;
            $scope.filter.QCStatus = 'PENDING';
            $scope.currentPage = 1;
            $scope.load(true);
        };

        // Every dropdown change reloades fresh data from DB and starts at page 1.
        $scope.serverFilterChanged = function () {
            if (filterReloadPromise) {
                $timeout.cancel(filterReloadPromise);
            }
            filterReloadPromise = $timeout(function () {
                filterReloadPromise = null;
                $scope.currentPage = 1;
                $scope.load(true);
            }, 0);
        };

        $scope.onDateFilterChanged = function () {
            if ($scope.filter.DateFilter !== 'CUSTOM') {
                $scope.filter.FromDate = null;
                $scope.filter.ToDate = null;
            }
            $scope.serverFilterChanged();
        };

        $scope.clearGridSearch = function () {
            $scope.gridSearchText = '';
            $scope.applyLocalSearch();
        };

        function startOfDay(value) {
            if (!value || !angular.isDate(value)) return null;
            return new Date(value.getFullYear(), value.getMonth(), value.getDate());
        }

        function addDays(value, days) {
            var d = new Date(value.getFullYear(), value.getMonth(), value.getDate());
            d.setDate(d.getDate() + days);
            return d;
        }

        // Server returns QCDate as dd/MM/yyyy. Parse it explicitly so browser locale
        // cannot interpret 03/09/2026 as March 9 on some machines.
        function parseGridDate(value) {
            var text = String(value || '').trim();
            if (!text) return null;
            var m = /^(\d{1,2})\/(\d{1,2})\/(\d{4})/.exec(text);
            if (!m) return null;
            var d = parseInt(m[1], 10);
            var mo = parseInt(m[2], 10) - 1;
            var y = parseInt(m[3], 10);
            var dt = new Date(y, mo, d);
            if (dt.getFullYear() !== y || dt.getMonth() !== mo || dt.getDate() !== d) return null;
            return dt;
        }

        function mondayOfWeek(value) {
            var d = startOfDay(value);
            var day = d.getDay(); // Sunday=0
            var diff = day === 0 ? -6 : 1 - day;
            return addDays(d, diff);
        }

        function inRange(value, from, to) {
            if (!value || !from || !to) return false;
            var t = value.getTime();
            return t >= from.getTime() && t <= to.getTime();
        }

        function passesDateFilter(row) {
            var filterId = String($scope.filter.DateFilter || 'ALL').toUpperCase();
            if (filterId === 'ALL') return true;

            var rowDate = parseGridDate(row && row.QCDate);
            if (!rowDate) return false;

            var today = startOfDay(new Date());
            var from = null;
            var to = null;

            if (filterId === 'TODAY') {
                from = today; to = today;
            } else if (filterId === 'YESTERDAY') {
                from = addDays(today, -1); to = from;
            } else if (filterId === 'THISWEEK') {
                from = mondayOfWeek(today); to = addDays(from, 6);
            } else if (filterId === 'PREVIOUSWEEK') {
                to = addDays(mondayOfWeek(today), -1); from = addDays(to, -6);
            } else if (filterId === 'THISMONTH') {
                from = new Date(today.getFullYear(), today.getMonth(), 1);
                to = new Date(today.getFullYear(), today.getMonth() + 1, 0);
            } else if (filterId === 'PREVIOUSMONTH') {
                from = new Date(today.getFullYear(), today.getMonth() - 1, 1);
                to = new Date(today.getFullYear(), today.getMonth(), 0);
            } else if (filterId === 'THISQUARTER') {
                var currentQuarterMonth = Math.floor(today.getMonth() / 3) * 3;
                from = new Date(today.getFullYear(), currentQuarterMonth, 1);
                to = new Date(today.getFullYear(), currentQuarterMonth + 3, 0);
            } else if (filterId === 'PREVIOUSQUARTER') {
                var previousQuarterEnd = new Date(today.getFullYear(), Math.floor(today.getMonth() / 3) * 3, 0);
                to = previousQuarterEnd;
                from = new Date(previousQuarterEnd.getFullYear(), Math.floor(previousQuarterEnd.getMonth() / 3) * 3, 1);
            } else if (filterId === 'THISYEAR') {
                from = new Date(today.getFullYear(), 0, 1);
                to = new Date(today.getFullYear(), 11, 31);
            } else if (filterId === 'PREVIOUSYEAR') {
                from = new Date(today.getFullYear() - 1, 0, 1);
                to = new Date(today.getFullYear() - 1, 11, 31);
            } else if (filterId === 'CUSTOM') {
                from = startOfDay($scope.filter.FromDate);
                to = startOfDay($scope.filter.ToDate);
                // Until both custom dates are selected, keep the loaded grid visible.
                if (!from || !to) return true;
            } else {
                return true;
            }

            return inRange(rowDate, from, to);
        }

        function passesTypeFilter(row) {
            var selected = String($scope.filter.EntityType || 'ALL').toUpperCase().replace(/\s+/g, '');
            if (!selected || selected === 'ALL') return true;
            var rowType = String((row && row.EntityType) || '').toUpperCase().replace(/\s+/g, '');
            return rowType === selected;
        }

        function passesStatusFilter(row) {
            var selected = String($scope.filter.QCStatus || 'ALL').toUpperCase().replace(/\s+/g, '');
            if (!selected || selected === 'ALL') return true;
            var rowStatus = String((row && row.QCStatus) || '').toUpperCase().replace(/\s+/g, '');
            if (selected === 'ACTIONTAKEN') return rowStatus === 'ACTIONTAKEN';
            return rowStatus === selected;
        }

        // Local free-search: filters only the rows already loaded in the grid.
        // No WebMethod/API call is made while typing.
        $scope.localGridFilter = function (row) {
            var q = String($scope.gridSearchText || '').trim().toLowerCase();
            if (!q) return true;
            if (!row) return false;

            var values = [
                row.QCDate,
                row.EntityType,
                row.EntityName,
                row.EntityId,
                row.HierarchySrNo,
                row.ProjectName,
                row.WorkflowName,
                row.NotifiedTo,
                row.QCStatus,
                row.TakenActionName,
                row.ActionTakenByName,
                row.ActionRemark
            ];

            if (row.Actions && row.Actions.length) {
                angular.forEach(row.Actions, function (a) {
                    values.push(a.ActionName);
                });
            }

            return values.join(' ').toLowerCase().indexOf(q) !== -1;
        };

        $scope.applyGridFilters = function () {
            var source = $scope.rows || [];
            var visible = [];
            angular.forEach(source, function (row) {
                if ($scope.localGridFilter(row)) {
                    visible.push(row);
                }
            });
            $scope.filteredRows = visible;
            rebuildPagedRows();
        };

        function rebuildPagedRows() {
            var size = parseInt($scope.pageSize || 10, 10);
            if (size <= 0) size = 10;

            $scope.totalPages = Math.max(
                1,
                Math.ceil(($scope.totalRows || 0) / size)
            );

            if ($scope.currentPage > $scope.totalPages)
                $scope.currentPage = $scope.totalPages;
            if ($scope.currentPage < 1)
                $scope.currentPage = 1;

            // Rows already contain only the requested server page.
            $scope.pagedRows = $scope.filteredRows || [];
            initialiseRemarkTooltips();
        }

        // Rows are rendered by AngularJS, so Bootstrap tooltips must be
        // initialised again after loading data or changing a grid page.
        function initialiseRemarkTooltips() {
            $timeout(function () {
                var elements = document.querySelectorAll(
                    '.qc-remark-icon[data-bs-toggle="tooltip"]'
                );

                angular.forEach(elements, function (element) {
                    if (window.bootstrap && window.bootstrap.Tooltip) {
                        var existing = window.bootstrap.Tooltip.getInstance
                            ? window.bootstrap.Tooltip.getInstance(element)
                            : null;

                        if (existing) existing.dispose();

                        new window.bootstrap.Tooltip(element, {
                            container: 'body',
                            trigger: 'hover focus'
                        });
                    } else if (window.jQuery &&
                               window.jQuery.fn &&
                               window.jQuery.fn.tooltip) {
                        window.jQuery(element).tooltip({
                            container: 'body',
                            trigger: 'hover focus',
                            title: element.getAttribute('data-bs-title') || ''
                        });
                    }
                });
            }, 0, false);
        }

        $scope.changePageSize = function () {
            $scope.currentPage = 1;
            $scope.load(true);
        };

        $scope.goToPage = function (pageNo) {
            pageNo = parseInt(pageNo || 1, 10);
            if (pageNo < 1 || pageNo > $scope.totalPages ||
                pageNo === $scope.currentPage) return;

            $scope.currentPage = pageNo;
            $scope.load(true);
        };

        $scope.getPageNumbers = function () {
            var pages = [];
            var from = Math.max(1, $scope.currentPage - 2);
            var to = Math.min($scope.totalPages, from + 4);
            from = Math.max(1, to - 4);

            for (var i = from; i <= to; i++)
                pages.push(i);

            return pages;
        };

        // Backward-compatible name used by the existing load/search code.
        $scope.applyLocalSearch = function () {
            $scope.applyGridFilters();
        };

        // Free search is local-only. Rebuild only the visible array while typing.
        $scope.$watch('gridSearchText', function () {
            $scope.applyGridFilters();
        });

        $scope.getSelectedAction = function (row) {
            if (!row || !row.Actions) return null;
            var selected = parseInt(row.SelectedActionId || 0, 10);
            for (var i = 0; i < row.Actions.length; i++) {
                if (parseInt(row.Actions[i].ActionId || 0, 10) === selected)
                    return row.Actions[i];
            }
            return null;
        };

        $scope.isRemarkRequired = function (row) {
            var action = $scope.getSelectedAction(row);
            return !!(action && action.IsRemarkRequired);
        };

        // Selecting an action only opens the remark popup.
        // The QC action is executed only after the user presses Take Action in the popup.
        $scope.openActionPopup = function (row) {
            $scope.error = '';
            $scope.success = '';

            if (!row || String(row.RuntimeStatus || '').toUpperCase() !== 'OPEN')
                return;

            var action = $scope.getSelectedAction(row);
            if (!action) {
                $scope.actionPopup.Show = false;
                return;
            }

            $scope.actionPopup.Row = row;
            $scope.actionPopup.Action = action;
            $scope.actionPopup.Remark = String(row.Remark || '');
            $scope.actionPopup.Show = true;
        };

        $scope.closeActionPopup = function (resetSelection) {
            var row = $scope.actionPopup.Row;

            // Do not allow the QC popup to close while the action request is running.
            if (row && row.ActionBusy) {
                return;
            }

            if (resetSelection && row) {
                row.SelectedActionId = '';
                row.Remark = '';
            }

            $scope.actionPopup.Show = false;
            $scope.actionPopup.Row = {};
            $scope.actionPopup.Action = {};
            $scope.actionPopup.Remark = '';
        };

        $scope.submitActionPopup = function () {
            $scope.error = '';
            $scope.success = '';

            var row = $scope.actionPopup.Row;
            var action = $scope.actionPopup.Action;
            var remark = String($scope.actionPopup.Remark || '').trim();
//alert(action.ActionId);
            // Prevent double-click / duplicate action execution.
            if (row && row.ActionBusy) {
                return;
            }

            if (!row || !row.RuntimeId || String(row.RuntimeStatus || '').toUpperCase() !== 'OPEN') {
                $scope.error = 'QC runtime is not available for action.';
                if (window.console && console.error) {
                    console.error('QC Action:', $scope.error);
                }
                return;
            }

            if (!action || parseInt(action.ActionId || 0, 10) <= 0) {
                $scope.error = 'Please select QC Action.';
                return;
            }

            if (action.IsRemarkRequired && !remark) {
                $scope.error = 'Remark is required for ' + (action.ActionName || 'selected action') + '.';
                return;
            }

            row.Remark = remark;
            row.ActionBusy = true;

            // Execute the QC user action directly through the same-origin WebMethod.
            $http.post('default.aspx/ExecuteQCUserAction', {
                runtimeId: String(row.RuntimeId),
                actionId: String(action.ActionId),
                remark: remark
            }).then(function (response) {
debugger
                var data = unwrapWebMethod(response) || {};
                if (parseInt(data.Result || 0, 10) !== 1) {
                    var serviceMessage = data.Message || 'QC action could not be completed.';

                    $scope.error = serviceMessage;
                    row.ActionBusy = false;
                    return;
                }

                var successMessage = data.Message || 'QC action completed successfully.';
                $scope.success = successMessage;

                // The request completed successfully, so close the QC action popup.
                row.ActionBusy = false;
                $scope.closeActionPopup(false);

                $scope.load(true);
            }, function (error) {
debugger
                var message = '';

                if (error && error.data) {
                    message =
                        error.data.Message ||
                        error.data.ExceptionMessage ||
                        (angular.isString(error.data) ? error.data : '');
                }

                message = message || 'QC action service call failed.';
                $scope.error = message;
                row.ActionBusy = false;

                // Keep the popup open and show the error through the existing inline message.
                if (window.console && console.error) {
                    console.error('QC Action service call failed:', message, error);
                }
            });
        };

        $scope.entityClass = function (type) {
            type = String(type || '').toUpperCase();
            if (type === 'PROJECT') return 'qc-type-project';
            if (type === 'MILESTONE') return 'qc-type-milestone';
            if (type === 'TASK') return 'qc-type-task';
            return 'qc-type-subtask';
        };

        $scope.statusClass = function (status) {
            status = String(status || '').toUpperCase();
            if (status === 'PENDING') return 'qc-status-pending';
            if (status === 'ACTION TAKEN') return 'qc-status-done';
            return 'qc-status-closed';
        };

        $scope.load();
    }]);
})();
