(function () {
    'use strict';

    angular.module('myTeamTaskApp')
        .directive('projectSearchSelect', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, element, attrs, ngModelController) {
                    var nativeSelect = element[0];
                    var destroyed = false;
                    var options = [];
                    var filtered = [];
                    var activeIndex = 0;

                    element.addClass('project-search-native');

                    var wrapper = angular.element(
                        '<div class="angular-project-select">' +
                            '<button type="button" class="angular-project-select-toggle" aria-haspopup="listbox" aria-expanded="false">' +
                                '<span class="angular-project-select-label"></span>' +
                                '<i class="ri-arrow-down-s-line angular-project-select-arrow"></i>' +
                            '</button>' +
                            '<div class="angular-project-select-menu">' +
                                '<div class="angular-project-search-wrap">' +
                                    '<i class="ri-search-line"></i>' +
                                    '<input type="text" class="angular-project-search-input" autocomplete="off" />' +
                                '</div>' +
                                '<div class="angular-project-results" role="listbox"></div>' +
                            '</div>' +
                        '</div>'
                    );

                    element.after(wrapper);

                    var root = wrapper[0];
                    var toggle = root.querySelector('.angular-project-select-toggle');
                    var label = root.querySelector('.angular-project-select-label');
                    var menu = root.querySelector('.angular-project-select-menu');
                    var searchInput = root.querySelector('.angular-project-search-input');
                    var results = root.querySelector('.angular-project-results');

                    searchInput.placeholder = attrs.projectSearchPlaceholder || 'Search project';

                    function isOpen() {
                        return root.classList.contains('open');
                    }

                    function openMenu() {
                        if (destroyed) return;
                        root.classList.add('open');
                        toggle.setAttribute('aria-expanded', 'true');
                        searchInput.value = '';
                        filterOptions('');
                        $timeout(function () {
                            if (!destroyed) {
                                searchInput.focus();
                                searchInput.select();
                            }
                        }, 0, false);
                    }

                    function closeMenu() {
                        root.classList.remove('open');
                        toggle.setAttribute('aria-expanded', 'false');
                    }

                    function rebuildOptions() {
                        options = [];
                        for (var i = 0; i < nativeSelect.options.length; i++) {
                            var opt = nativeSelect.options[i];
                            options.push({
                                index: i,
                                text: opt.text || '',
                                disabled: !!opt.disabled
                            });
                        }
                        updateSelectedLabel();
                        if (isOpen()) filterOptions(searchInput.value || '');
                    }

                    function updateSelectedLabel() {
                        var index = nativeSelect.selectedIndex;
                        var text = index >= 0 && nativeSelect.options[index]
                            ? nativeSelect.options[index].text
                            : '';
                        label.textContent = text || attrs.projectSearchPlaceholder || 'Select project';
                    }

                    function filterOptions(term) {
                        var q = String(term || '').toLowerCase().trim();
                        filtered = options.filter(function (item) {
                            return !item.disabled && (!q || item.text.toLowerCase().indexOf(q) >= 0);
                        });

                        var selectedNativeIndex = nativeSelect.selectedIndex;
                        var selectedFilteredIndex = filtered.findIndex(function (item) {
                            return item.index === selectedNativeIndex;
                        });

                        activeIndex = selectedFilteredIndex >= 0 ? selectedFilteredIndex : 0;
                        renderResults();
                    }

                    function renderResults() {
                        while (results.firstChild) results.removeChild(results.firstChild);

                        if (!filtered.length) {
                            var empty = document.createElement('div');
                            empty.className = 'angular-project-empty';
                            empty.textContent = 'No project found';
                            results.appendChild(empty);
                            return;
                        }

                        filtered.forEach(function (item, index) {
                            var row = document.createElement('div');
                            row.className = 'angular-project-option';
                            row.setAttribute('role', 'option');
                            row.setAttribute('data-filter-index', String(index));
                            row.textContent = item.text;

                            if (item.index === nativeSelect.selectedIndex) {
                                row.classList.add('selected');
                                row.setAttribute('aria-selected', 'true');
                            }

                            if (index === activeIndex) {
                                row.classList.add('active');
                            }

                            row.addEventListener('mousedown', function (event) {
                                event.preventDefault();
                            });

                            row.addEventListener('click', function () {
                                chooseIndex(index);
                            });

                            results.appendChild(row);
                        });

                        ensureActiveVisible();
                    }

                    function ensureActiveVisible() {
                        var active = results.querySelector('.angular-project-option.active');
                        if (active && typeof active.scrollIntoView === 'function') {
                            active.scrollIntoView({ block: 'nearest' });
                        }
                    }

                    function fireNativeChange() {
                        var evt;
                        if (typeof Event === 'function') {
                            evt = new Event('change', { bubbles: true });
                        } else {
                            evt = document.createEvent('HTMLEvents');
                            evt.initEvent('change', true, false);
                        }
                        nativeSelect.dispatchEvent(evt);
                    }

                    function chooseIndex(index) {
                        if (index < 0 || index >= filtered.length) return;
                        var item = filtered[index];
                        nativeSelect.selectedIndex = item.index;
                        fireNativeChange();
                        updateSelectedLabel();
                        closeMenu();
                    }

                    function moveActive(step) {
                        if (!filtered.length) return;
                        activeIndex += step;
                        if (activeIndex < 0) activeIndex = filtered.length - 1;
                        if (activeIndex >= filtered.length) activeIndex = 0;
                        renderResults();
                    }

                    toggle.addEventListener('click', function (event) {
                        event.preventDefault();
                        event.stopPropagation();
                        if (isOpen()) closeMenu(); else openMenu();
                    });

                    searchInput.addEventListener('input', function () {
                        filterOptions(searchInput.value);
                    });

                    searchInput.addEventListener('keydown', function (event) {
                        if (event.key === 'ArrowDown') {
                            event.preventDefault();
                            moveActive(1);
                        } else if (event.key === 'ArrowUp') {
                            event.preventDefault();
                            moveActive(-1);
                        } else if (event.key === 'Enter') {
                            event.preventDefault();
                            chooseIndex(activeIndex);
                        } else if (event.key === 'Escape') {
                            event.preventDefault();
                            closeMenu();
                            toggle.focus();
                        }
                    });

                    toggle.addEventListener('keydown', function (event) {
                        if (event.key === 'ArrowDown' || event.key === 'ArrowUp' || event.key === 'Enter' || event.key === ' ') {
                            event.preventDefault();
                            openMenu();
                        }
                    });

                    function documentClick(event) {
                        if (!root.contains(event.target)) closeMenu();
                    }
                    document.addEventListener('click', documentClick);

                    var unwatchOptions = attrs.projectSearchOptions
                        ? scope.$watchCollection(attrs.projectSearchOptions, function () {
                            $timeout(rebuildOptions, 0, false);
                        })
                        : angular.noop;

                    var unwatchModel = scope.$watch(attrs.ngModel, function () {
                        $timeout(updateSelectedLabel, 0, false);
                    });

                    $timeout(rebuildOptions, 0, false);

                    scope.$on('$destroy', function () {
                        destroyed = true;
                        document.removeEventListener('click', documentClick);
                        unwatchOptions();
                        unwatchModel();
                        wrapper.remove();
                    });
                }
            };
        }])
        .controller('myTeamController', [

        '$scope',
        '$timeout',
        '$q',
        'myTeamService',
        function ($scope, $timeout, $q, api) {
            var vm = this;
            var modal;
            var chart;
            var autoSaveTimer;

            // =========================================================
            // PAGE STATE
            // =========================================================
            vm.projects = [];
            vm.assignableProjects = [];
            vm.users = [];
            vm.hours = [];
            vm.gridColumns = [];

            vm.projectId = 0;
            vm.modalProjectId = 0;
            vm.viewMode = 'list';

            vm.selectedDate = new Date();
            vm.allAvailableTasks = [];
            vm.assignmentTree = [];
            vm.taskSearch = '';
            vm.editAssignments = [];
            vm.removedIds = [];
            vm.message = {};

            // Timeline configuration
            vm.dayStart = 0;
            vm.dayEnd = 1440;
            vm.pixelsPerMinute = 1;
            vm.timelineWidth = 900;
            vm.HOUR_WIDTH = 100;
            vm.quarterWidth = vm.HOUR_WIDTH / 4;
            vm.headerGridStyle = {};

            // =========================================================
            // COMMON HELPERS
            // =========================================================
            function isoDate(d) {
                var x = new Date(d);
                var m = ('0' + (x.getMonth() + 1)).slice(-2);
                var day = ('0' + x.getDate()).slice(-2);

                return x.getFullYear() + '-' + m + '-' + day;
            }

            function minutes(v) {
                if (!v) {
                    return 0;
                }

                var m = moment(v, ['hh:mm A', 'h:mm A', 'HH:mm', 'H:mm'], true);
                return m.isValid() ? (m.hours() * 60) + m.minutes() : 0;
            }

            function hhmm(n) {
                n = Math.max(0, Math.round(n / 15) * 15);

                return (
                    ('0' + Math.floor((n % 1440) / 60)).slice(-2) +
                    ':' +
                    ('0' + (n % 60)).slice(-2)
                );
            }

            function durationValue(n) {
                return Math.floor(n / 60) + ':' + ('0' + (n % 60)).slice(-2);
            }

            function durationText(n) {
                return (
                    ('0' + Math.floor(n / 60)).slice(-2) +
                    ':' +
                    ('0' + (n % 60)).slice(-2) +
                    ' Hrs'
                );
            }

            // Hierarchy duration display only. Keeps the stored duration untouched,
            // but formats it like Assign Project: 50:30, 20:30, 1:30, etc.
            vm.hierarchyDurationText = function (value) {
                var text = String(value === null || value === undefined ? '' : value)
                    .replace(/hrs?|hours?/ig, '')
                    .trim()
                    .replace('/', ':');

                if (!text) {
                    return '0:00';
                }

                if (text.indexOf(':') >= 0) {
                    var parts = text.split(':');
                    var h = parseInt(parts[0] || '0', 10) || 0;
                    var m = parseInt(parts[1] || '0', 10) || 0;
                    return h + ':' + ('0' + m).slice(-2);
                }

                var decimalHours = parseFloat(text);
                if (!isNaN(decimalHours)) {
                    var totalMinutes = Math.round(decimalHours * 60);
                    return Math.floor(totalMinutes / 60) + ':' +
                        ('0' + (totalMinutes % 60)).slice(-2);
                }

                return text;
            };

            function copy(o) {
                return angular.fromJson(angular.toJson(o));
            }

            function notify(text, ok) {
                vm.message = {
                    text: text,
                    ok: ok
                };

                $timeout(function () {
                    vm.message = {};
                }, 3500);
            }

            function fail(e) {
                vm.loading = false;
                vm.saving = false;

                notify(
                    angular.isString(e) ? e : 'Operation failed.',
                    false
                );
            }

            // =========================================================
            // DATE MANAGEMENT
            // =========================================================
            vm.displayDate = function () {
                var d = vm.selectedDate;

                return (
                    ('0' + d.getDate()).slice(-2) +
                    '/' +
                    ('0' + (d.getMonth() + 1)).slice(-2) +
                    '/' +
                    d.getFullYear()
                );
            };

            vm.openDatePicker = function ($event) {
                var host = $event && $event.currentTarget
                    ? $event.currentTarget.parentElement
                    : null;

                var input = host
                    ? host.querySelector('input[type="date"]')
                    : null;

                if (!input) {
                    return;
                }

                try {
                    if (typeof input.showPicker === 'function') {
                        input.showPicker();
                        return;
                    }
                } catch (e) {
                }

                try {
                    input.focus();
                    input.click();
                } catch (e2) {
                }
            };

            vm.onDateSelected = function () {
                var activeId = vm.activeUser && vm.activeUser.EmployeeId;

                return vm.reload().then(function (users) {
                    if (!activeId) {
                        return users;
                    }

                    var fresh = (users || []).find(function (u) {
                        return String(u.EmployeeId) === String(activeId);
                    });

                    if (fresh) {
                        prepareAssignments(fresh);
                        vm.loadAssignableTasks();
                        $timeout(enableInteraction, 120);
                    }

                    return users;
                });
            };

            vm.changeDate = function (n) {
                var d = new Date(vm.selectedDate);

                d.setDate(d.getDate() + n);
                vm.selectedDate = d;
                vm.onDateSelected();
            };

            // =========================================================
            // TIMELINE SCALE
            // 1 hour = 100px
            // 15 minutes = 25px
            // =========================================================
            function setupScale(start, end) {
                vm.dayStart = minutes(start || '09:00');
                vm.dayEnd = minutes(end || '18:00');

                if (vm.dayEnd <= vm.dayStart) {
                    vm.dayEnd += 1440;
                }

                var span = Math.max(60, vm.dayEnd - vm.dayStart);
                var totalHours = Math.max(1, Math.ceil(span / 60));

                // Fit the full common shift range in the available overview width
                // whenever possible. Keep the original 100px/hour scale on wide
                // screens and allow horizontal scrolling only when the hour width
                // would otherwise become too small to read.
                var scroller = document.getElementById('overviewScroller');
                var availableWidth = scroller && scroller.clientWidth
                    ? Math.max(0, scroller.clientWidth - 2)
                    : totalHours * 100;
                var fittedHourWidth = Math.floor(availableWidth / totalHours);

                vm.HOUR_WIDTH = Math.max(70, Math.min(100, fittedHourWidth || 100));
                vm.pixelsPerMinute = vm.HOUR_WIDTH / 60;
                vm.timelineWidth = totalHours * vm.HOUR_WIDTH;
                vm.quarterWidth = vm.HOUR_WIDTH / 4;

                // Four 15-minute grid boxes for every hour.
                vm.gridColumns = new Array(totalHours * 4);

                vm.hours = [];

                for (var i = 0; i < totalHours; i++) {
                    var n = vm.dayStart + (i * 60);
                    var h = Math.floor((n % 1440) / 60);

                    vm.hours.push(
                        ('0' + (((h + 11) % 12) + 1)).slice(-2) +
                        ' ' +
                        (h < 12 ? 'AM' : 'PM')
                    );
                }

                vm.headerGridStyle = {
                    'grid-template-columns':
                        'repeat(' + totalHours + ',' + vm.HOUR_WIDTH + 'px)',
                    'width': vm.timelineWidth + 'px',
                    'min-width': vm.timelineWidth + 'px'
                };
            }

            // Generic slot style used by the Assign Task popup.
            vm.slotStyle = function (start, end) {
                var s = minutes(start);
                var e = minutes(end);

                if (e <= s) {
                    e += 1440;
                }

                if (s < vm.dayStart && e <= 1440 && vm.dayStart >= 720) {
                    s += 1440;
                    e += 1440;
                }

                var left = (s - vm.dayStart) * vm.pixelsPerMinute;
                var width = (e - s) * vm.pixelsPerMinute;

                if (left < 0) {
                    width += left;
                    left = 0;
                }

                if (left + width > vm.timelineWidth) {
                    width = vm.timelineWidth - left;
                }

                return {
                    left: Math.max(0, left) + 'px',
                    width: Math.max(4, width) + 'px'
                };
            };

            // =========================================================
            // MANAGE TEAM TASKS OVERVIEW STRIP
            // Mirrors Assign Project strip positioning.
            // =========================================================
            vm.shiftSlotStyle = function (user) {
                if (!user) {
                    return {};
                }

                var s = minutes(user.ShiftStart);
                var e = minutes(user.ShiftEnd);

                // Night shift: for example 09:00 PM to 06:00 AM.
                if (e <= s) {
                    e += 1440;
                }

                var left = (s - vm.dayStart) * vm.pixelsPerMinute;
                var width = (e - s) * vm.pixelsPerMinute;

                return {
                    left: Math.max(0, left) + 'px',
                    width: Math.max(0, width) + 'px'
                };
            };

            vm.taskSlotStyle = function (user, a) {
                if (!user || !a) {
                    return {};
                }

                var shiftStart = minutes(user.ShiftStart);
                var shiftEnd = minutes(user.ShiftEnd);

                if (shiftEnd <= shiftStart) {
                    shiftEnd += 1440;
                }

                var start = minutes(a.StartTime);

                // For night shift, after-midnight task belongs to same shift.
                if (shiftEnd > 1440 && start < shiftStart) {
                    start += 1440;
                }

                var dur = parseInt(a.DurationMinutes, 10) || 0;

                // Fallback: calculate duration from StartTime and EndTime.
                if (!dur) {
                    var end = minutes(a.EndTime);

                    if (end <= minutes(a.StartTime)) {
                        end += 1440;
                    }

                    dur = Math.max(0, end - minutes(a.StartTime));
                }

                var left = (start - vm.dayStart) * vm.pixelsPerMinute;
                var width = (dur * vm.pixelsPerMinute) - 4;

                return {
                    left: Math.max(0, left) + 'px',
                    width: Math.max(18, width) + 'px'
                };
            };

            // =========================================================
            // INITIAL LOAD
            // =========================================================
            vm.init = function () {
                vm.loading = true;

                api.bootstrap().then(function (r) {
                    vm.allEmployees = r.Employees || [];

                    // Both the main filter and Assign Task popup support All Project.
                    // A separate array is used so neither dropdown mutates the other.
                    var projectRows = (r.Projects || []).slice(0);
                    vm.projects = [{ Id: 0, Name: 'All Project' }].concat(projectRows);
                    vm.assignableProjects = [{ Id: 0, Name: 'All Project' }].concat(projectRows.slice(0));

                    setupScale(
                        r.DefaultShiftStart,
                        r.DefaultShiftEnd
                    );

                    vm.reload();
                }, fail);
            };

            // =========================================================
            // TOOLTIPS
            // =========================================================
            function initOverviewTooltips() {
                if (!window.bootstrap || !bootstrap.Tooltip) {
                    return;
                }

                document.querySelectorAll(
                    '#overviewScroller [data-bs-toggle="tooltip"],' +
                    '#assignTaskModal [data-bs-toggle="tooltip"]'
                ).forEach(function (el) {
                    try {
                        var old = bootstrap.Tooltip.getInstance(el);

                        if (old) {
                            old.dispose();
                        }

                        new bootstrap.Tooltip(el, {
                            container: 'body',
                            html: true,
                            trigger: 'hover',
                            boundary: 'window'
                        });
                    } catch (e) {
                    }
                });
            }

            // A Bootstrap tooltip is appended to <body>. When its assignment block is
            // removed while still hovered, mouseleave never fires and the tooltip can
            // remain visible. Dispose it before Angular removes the block from the DOM.
            function disposeAssignmentTooltipFromEvent(e) {
                var clicked = e && (e.currentTarget || e.target);
                var block = clicked && clicked.closest
                    ? clicked.closest('[data-bs-toggle="tooltip"]')
                    : null;

                if (block) {
                    try {
                        if (window.bootstrap && bootstrap.Tooltip) {
                            var instance = bootstrap.Tooltip.getInstance(block);
                            if (instance) {
                                instance.hide();
                                instance.dispose();
                            }
                        }
                    } catch (ignoreTooltipDispose) {
                    }

                    try {
                        var describedBy = block.getAttribute('aria-describedby');
                        if (describedBy) {
                            var tooltipEl = document.getElementById(describedBy);
                            if (tooltipEl && tooltipEl.parentNode) {
                                tooltipEl.parentNode.removeChild(tooltipEl);
                            }
                            block.removeAttribute('aria-describedby');
                        }
                    } catch (ignoreTooltipElement) {
                    }
                }

                // Safety cleanup only for this page's task tooltip class.
                document.querySelectorAll('.tooltip.team-task-tooltip').forEach(function (tip) {
                    if (tip && tip.parentNode) {
                        tip.parentNode.removeChild(tip);
                    }
                });
            }

            // Overview transfer uses a floating ghost so scrolling never clips the dragged bar.
            function enableOverviewMove() {
                if (!window.interact) return;
                interact('#overviewScroller .planned-block').draggable({
                    listeners: {
                        start: function (e) {
                            disposeAssignmentTooltipFromEvent(e);
                            var ghost = e.target.cloneNode(true);
                            ghost.removeAttribute('data-bs-toggle');
                            ghost.classList.add('myteam-transfer-ghost');
                            ghost.style.cssText = 'position:fixed;pointer-events:none;z-index:20000;width:' +
                                e.target.offsetWidth + 'px;left:' + e.client.x + 'px;top:' + (e.client.y - 12) + 'px;';
                            document.body.appendChild(ghost);
                            e.interaction.teamGhost = ghost;
                        },
                        move: function (e) {
                            var ghost = e.interaction.teamGhost;
                            if (ghost) { ghost.style.left = e.client.x + 'px'; ghost.style.top = (e.client.y - 12) + 'px'; }
                        },
                        end: function (e) {
                            var ghost = e.interaction.teamGhost;
                            if (ghost) ghost.remove();
                            e.interaction.teamGhost = null;
                            var hit = document.elementFromPoint(e.client.x, e.client.y);
                            var row = hit && hit.closest('#overviewScroller .time-row');
                            if (!row || vm.saving || vm.loading) return;
                            var targetId = row.getAttribute('data-employee-id');
                            var target = (vm.users || []).find(function (u) { return String(u.EmployeeId) === String(targetId); });
                            if (!target || !target.IsWorkingDay) { notify('Drop only in available area.', false); return; }
                            var sourceId = e.target.getAttribute('data-employee-id');
                            if (String(targetId) === String(sourceId)) return;
                            var source = (vm.users || []).find(function (u) { return String(u.EmployeeId) === String(sourceId); });
                            var assignment = source && source.Assignments.find(function (a) {
                                return a.ClientKey === e.target.getAttribute('data-key');
                            });
                            if (!assignment || !assignment.AssignId) return;
                            var bounds = row.getBoundingClientRect();
                            var start = vm.dayStart + Math.round((e.client.x - bounds.left) / vm.pixelsPerMinute / 15) * 15;
                            vm.saving = true;
                            $scope.$applyAsync();
                            api.moveAssignment(assignment.AssignId, targetId, isoDate(vm.selectedDate), hhmm(start))
                                .then(function () { notify('Assignment moved successfully.', true); return vm.reload(); },
                                    function (error) { notify(angular.isString(error) ? error : 'Unable to move assignment.', false); })
                                .finally(function () { vm.saving = false; });
                        }
                    }
                });
            }

            // =========================================================
            // LOAD EMPLOYEES + EXISTING ASSIGNMENTS
            // =========================================================
            vm.reload = function () {
                vm.loading = true;

                return api.day(
                    isoDate(vm.selectedDate),
                    vm.projectId,
                    vm.allEmployees
                ).then(function (r) {
                    setupScale(r.DayStart, r.DayEnd);

                    vm.users = r.Users || [];

                    vm.users.forEach(function (u) {
                        u.Assignments = (u.Assignments || []).filter(function (a) {
                            return (
                                !a.WorkDate ||
                                api.toDateOnly(a.WorkDate) === isoDate(vm.selectedDate)
                            );
                        });

                        var shiftStart = minutes(u.ShiftStart);
                        var shiftEnd = minutes(u.ShiftEnd);

                        if (shiftEnd <= shiftStart) {
                            shiftEnd += 1440;
                        }

                        var booked = u.Assignments.reduce(function (total, a) {
                            return total + (parseInt(a.DurationMinutes, 10) || 0);
                        }, 0);

                        u.TotalShiftMinutes = Math.max(0, shiftEnd - shiftStart);
                        u.TotalShiftText = durationText(u.TotalShiftMinutes);

                        u.AvailableMinutes = Math.max(
                            0,
                            u.TotalShiftMinutes - booked
                        );

                        u.AvailableText = durationText(u.AvailableMinutes);
                    });

                    vm.loading = false;

                    $timeout(function () {
                        drawChart();
                        initOverviewTooltips();
                        enableOverviewMove();
                    }, 80);

                    return vm.users;
                }, fail);
            };

            // =========================================================
            // TEAM AVAILABILITY CHART
            // =========================================================
            function drawChart() {
                var c = document.getElementById('teamAvailabilityChart');

                if (!c || !window.Chart) {
                    return;
                }

                if (chart) {
                    chart.destroy();
                }

                chart = new Chart(c, {
                    type: 'pie',
                    data: {
                        labels: vm.users.map(function (x) {
                            return x.Name;
                        }),
                        datasets: [
                            {
                                data: vm.users.map(function (x) {
                                    return x.AvailableMinutes || 0;
                                }),
                                backgroundColor: [
                                    '#9af5a5',
                                    '#adc4e7',
                                    '#c7b0f4',
                                    '#ffc3ed',
                                    '#ffdf8b',
                                    '#93d7ef',
                                    '#ff8b7e',
                                    '#b8d2ff'
                                ],
                                borderWidth: 0
                            }
                        ]
                    },
                    options: {
                        plugins: {
                            legend: {
                                display: false
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (x) {
                                        return x.label + ': ' + durationText(x.raw);
                                    }
                                }
                            }
                        }
                    }
                });
            }

            // =========================================================
            // ASSIGN TASK POPUP
            // =========================================================
            function prepareAssignments(user) {
                vm.activeUser = user;
                vm.editAssignments = copy(user.Assignments || []);

                vm.editAssignments.forEach(function (a) {
                    a.ClientKey = 'db-' + a.AssignId + '-' + a.StartTime;
                    a.EntityId =
                        a.EntityId ||
                        a.SubTaskId ||
                        a.TaskId ||
                        a.MilestoneId;

                    a.OldStartTime = a.StartTime;
                    a.OldWorkDate = a.WorkDate;
                    a.IsChanged = false;
                });

                vm.removedAssignments = [];
            }

            vm.openAssign = function (user) {
                prepareAssignments(user);

                // Assign Task popup always opens with All Project by default.
                // The user can then select one project from the popup dropdown if needed.
                vm.modalProjectId = 0;
                vm.taskSearch = '';
                vm.loadAssignableTasks();

                modal = bootstrap.Modal.getOrCreateInstance(
                    document.getElementById('assignTaskModal')
                );

                modal.show();

                $timeout(function () {
                    enableInteraction();
                    initOverviewTooltips();
                }, 250);
            };

            vm.loadAssignableTasks = function () {
                if (!vm.activeUser) {
                    vm.assignmentTree = [];
                    vm.allAvailableTasks = [];
                    return;
                }

                vm.loading = true;

                api.assignable(
                    vm.activeUser.EmployeeId,
                    isoDate(vm.selectedDate),
                    vm.modalProjectId || 0
                ).then(function (r) {
                    vm.assignmentTree = r.Projects || [];
                    vm.allAvailableTasks = r.Subtasks || [];
                    vm.loading = false;

                    $timeout(enableInteraction, 0);
                }, fail);
            };

            vm.toggleTreeNode = function (node, $event) {
                if ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();
                }
                if (!node) return;
                node.Expanded = !node.Expanded;
            };

            function treeTextMatches(text) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return false;
                return String(text || '').toLowerCase().indexOf(q) >= 0;
            }

            vm.subtaskTreeVisible = function (sub) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return true;
                return treeTextMatches(sub.Name) ||
                    treeTextMatches(sub.ProjectName) ||
                    treeTextMatches(sub.MilestoneName) ||
                    treeTextMatches(sub.TaskName);
            };

            vm.taskTreeVisible = function (task) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return true;
                if (treeTextMatches(task.Name)) return true;
                return (task.Subtasks || []).some(vm.subtaskTreeVisible);
            };

            vm.milestoneTreeVisible = function (milestone) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return true;
                if (treeTextMatches(milestone.Name)) return true;
                return (milestone.Tasks || []).some(vm.taskTreeVisible);
            };

            vm.projectTreeVisible = function (project) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return true;
                if (treeTextMatches(project.Name)) return true;
                return (project.Milestones || []).some(vm.milestoneTreeVisible);
            };

            // Project is no longer rendered as a folder in the right panel, but it remains
            // the data/filter scope. These context helpers preserve project-name search while
            // the visible hierarchy starts directly from Milestone -> Task -> Subtask.
            vm.milestoneTreeVisibleInProject = function (milestone, project) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return true;
                if (project && treeTextMatches(project.Name)) return true;
                return vm.milestoneTreeVisible(milestone);
            };

            vm.taskTreeVisibleInContext = function (task, milestone, project) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return true;
                if (project && treeTextMatches(project.Name)) return true;
                if (milestone && treeTextMatches(milestone.Name)) return true;
                return vm.taskTreeVisible(task);
            };

            vm.subtaskTreeVisibleInContext = function (subtask, task, milestone, project) {
                var q = String(vm.taskSearch || '').toLowerCase().trim();
                if (!q) return true;
                if (project && treeTextMatches(project.Name)) return true;
                if (milestone && treeTextMatches(milestone.Name)) return true;
                if (task && treeTextMatches(task.Name)) return true;
                return vm.subtaskTreeVisible(subtask);
            };

            vm.treeExpanded = function (node) {
                return !!vm.taskSearch || !!(node && node.Expanded);
            };

            function employeeExistsInSubtaskTeam(t) {
                if (!vm.activeUser || !t) return false;
                var employeeId = String(vm.activeUser.EmployeeId || '').trim();
                var team = String(t.Team || t.TeamIds || '');
                if (!employeeId || !team) return false;

                return team.split(',').some(function (id) {
                    return String(id || '').trim() === employeeId;
                });
            }

            function showSubtaskTeamError() {
                var text = 'Selected employee is not in the team of this subtask.';
                notify(text, false);
                try {
                    window.alert(text);
                } catch (e) {
                }
            }

            function showDropValidationError(text) {
                text = text || 'Unable to assign subtask.';
                notify(text, false);
                try {
                    window.alert(text);
                } catch (e) {
                }
            }

            function parseValidationDate(value) {
                if (!value) return null;

                var m = moment(value, [
                    'YYYY-MM-DD',
                    'DD/MM/YYYY',
                    'D/M/YYYY',
                    'MM/DD/YYYY',
                    'M/D/YYYY',
                    moment.ISO_8601
                ], true);

                if (!m.isValid()) {
                    m = moment(value);
                }

                return m.isValid() ? m.startOf('day') : null;
            }

            function newDropStartsInsideShift(item) {
                if (!item) return false;

                var start = minutes(item.StartTime);
                var shiftStart = vm.dayStart;
                var shiftEnd = vm.dayEnd;

                // setupScale stores night-shift end as a value greater than 1440.
                if (shiftEnd > 1440 && start < shiftStart) {
                    start += 1440;
                }

                return start >= shiftStart && start < shiftEnd;
            }

            function validateNewSubtaskDrop(t, proposed) {
                if (!t || String(t.Type || '').toLowerCase() !== 'subtask') {
                    showDropValidationError('Only subtask can be assigned from My Team.');
                    return false;
                }

                if (!employeeExistsInSubtaskTeam(t)) {
                    showSubtaskTeamError();
                    return false;
                }

                var durationMinutes = parseInt(t.DurationMinutes, 10) || 0;
                if (durationMinutes <= 0) {
                    showDropValidationError(
                        (t.Name || 'Selected subtask') +
                        ' duration is required before assignment. Please enter duration first.'
                    );
                    return false;
                }

                var selected = parseValidationDate(isoDate(vm.selectedDate));
                var startDate = parseValidationDate(t.StartDate);
                var targetDate = parseValidationDate(t.TargetDate);

                if (selected && startDate && selected.isBefore(startDate, 'day')) {
                    showDropValidationError(
                        (t.Name || 'Selected subtask') +
                        ' cannot be assigned on ' + selected.format('DD/MM/YYYY') +
                        '. Selected date should not be less than StartDate (' +
                        startDate.format('DD/MM/YYYY') + ').'
                    );
                    return false;
                }

                if (selected && targetDate && selected.isAfter(targetDate, 'day')) {
                    showDropValidationError(
                        (t.Name || 'Selected subtask') +
                        ' cannot be assigned on ' + selected.format('DD/MM/YYYY') +
                        '. Selected date should not be greater than Targetdate (' +
                        targetDate.format('DD/MM/YYYY') + ').'
                    );
                    return false;
                }

                // Same drop rule as Assign Project: the START point must be inside the
                // employee shift. Long duration may continue into the next shift/day.
                if (!newDropStartsInsideShift(proposed)) {
                    showDropValidationError('Drop only in available area.');
                    return false;
                }

                // Existing visible assignments are checked immediately in the UI.
                // Server repeats a full shift-segment overlap check before insert.
                if (overlaps(proposed)) {
                    showDropValidationError('Drop only in available area.');
                    return false;
                }

                return true;
            }

            function batchSubtaskValidationReason(t) {
                if (!t || String(t.Type || '').toLowerCase() !== 'subtask') {
                    return 'Only subtask can be assigned from My Team.';
                }

                if (!employeeExistsInSubtaskTeam(t)) {
                    return 'Selected employee is not in the team of this subtask.';
                }

                var durationMinutes = parseInt(t.DurationMinutes, 10) || 0;
                if (durationMinutes <= 0) {
                    return 'Duration is required before assignment. Please enter duration first.';
                }

                var selected = parseValidationDate(isoDate(vm.selectedDate));
                var startDate = parseValidationDate(t.StartDate);
                var targetDate = parseValidationDate(t.TargetDate);

                if (selected && startDate && selected.isBefore(startDate, 'day')) {
                    return 'Selected date should not be less than StartDate (' +
                        startDate.format('DD/MM/YYYY') + ').';
                }

                if (selected && targetDate && selected.isAfter(targetDate, 'day')) {
                    return 'Selected date should not be greater than Targetdate (' +
                        targetDate.format('DD/MM/YYYY') + ').';
                }

                return '';
            }

            function findTreeProject(projectId) {
                return (vm.assignmentTree || []).find(function (project) {
                    return String(project.Id) === String(projectId);
                }) || null;
            }

            function canonicalAvailableSubtask(subtask) {
                if (!subtask) return null;
                var key = subtask.Key || ('subtask-' + (subtask.EntityId || subtask.SubTaskId || ''));
                return (vm.allAvailableTasks || []).find(function (x) {
                    return x.Key === key;
                }) || subtask;
            }

            function collectHierarchyDropSubtasks(type, id, projectId, key) {
                type = String(type || '').toLowerCase();
                var collected = [];
                var seen = {};

                function add(sub) {
                    sub = canonicalAvailableSubtask(sub);
                    if (!sub) return;
                    var subKey = sub.Key || ('subtask-' + (sub.EntityId || sub.SubTaskId || ''));
                    if (!subKey || seen[subKey]) return;
                    seen[subKey] = true;
                    collected.push(sub);
                }

                if (type === 'subtask') {
                    var direct = (vm.allAvailableTasks || []).find(function (x) {
                        return x.Key === key || String(x.EntityId) === String(id);
                    });
                    if (direct) add(direct);
                    return collected;
                }

                var projects = vm.assignmentTree || [];
                if (projectId) {
                    var onlyProject = findTreeProject(projectId);
                    projects = onlyProject ? [onlyProject] : [];
                }

                projects.forEach(function (project) {
                    (project.Milestones || []).forEach(function (milestone) {
                        if (type === 'milestone' && String(milestone.Id) !== String(id)) {
                            return;
                        }

                        (milestone.Tasks || []).forEach(function (task) {
                            if (type === 'task' && String(task.Id) !== String(id)) {
                                return;
                            }

                            (task.Subtasks || []).forEach(add);
                        });
                    });
                });

                return collected;
            }

            function hierarchyDropNodeName(type, id, projectId) {
                type = String(type || '').toLowerCase();
                var project = findTreeProject(projectId);
                var projects = project ? [project] : (vm.assignmentTree || []);
                var name = type === 'task' ? 'Selected task' : 'Selected milestone';

                projects.some(function (p) {
                    return (p.Milestones || []).some(function (m) {
                        if (type === 'milestone' && String(m.Id) === String(id)) {
                            name = m.Name || name;
                            return true;
                        }
                        if (type === 'task') {
                            var foundTask = (m.Tasks || []).find(function (t) {
                                return String(t.Id) === String(id);
                            });
                            if (foundTask) {
                                name = foundTask.Name || name;
                                return true;
                            }
                        }
                        return false;
                    });
                });

                return name;
            }

            function hierarchyDropErrorText(error) {
                if (!error) return 'Operation failed.';
                if (angular.isString(error)) return error;
                if (error.data) {
                    if (angular.isString(error.data)) return error.data;
                    if (error.data.Message) return error.data.Message;
                    if (error.data.ExceptionMessage) return error.data.ExceptionMessage;
                }
                if (error.message) return error.message;
                return 'Operation failed.';
            }

            function parseHierarchyNextSlot(raw, fallbackDate, fallbackTime) {
                raw = raw === undefined || raw === null ? '' : String(raw);
                var parts = raw.split('~~');
                if (parts[0] !== '1') {
                    return { ok: false, reason: raw || 'Drop only in available area.' };
                }

                return {
                    ok: true,
                    startDate: api.toDateOnly(parts[1] || fallbackDate),
                    startTime: parts[2] || fallbackTime,
                    endDate: api.toDateOnly(parts[3] || parts[1] || fallbackDate),
                    endTime: parts[4] || parts[2] || fallbackTime
                };
            }

            function showHierarchyBatchFailureAlert(failures, assignedCount) {
                if (!failures || !failures.length) return;

                var heading = assignedCount > 0
                    ? 'Some subtasks could not be assigned:'
                    : 'No subtask could be assigned:';

                var lines = failures.map(function (failure, index) {
                    return (index + 1) + '. ' +
                        (failure.name || 'Subtask') + ' - ' +
                        (failure.reason || 'Operation failed.');
                });

                var message = heading + '\n\n' + lines.join('\n');
                notify(heading.replace(':', ''), false);
                try {
                    window.alert(message);
                } catch (ignoreBatchAlert) {
                }
            }

            function refreshAssignPopupAfterBatch(employeeId) {
                return vm.reload().then(function (users) {
                    var user = (users || []).find(function (u) {
                        return String(u.EmployeeId) === String(employeeId);
                    });

                    if (user) {
                        prepareAssignments(user);
                    }

                    return vm.loadAssignableTasks();
                }).then(function () {
                    $timeout(enableInteraction, 120);
                });
            }

            // Milestone/Task are drag shortcuts only. Every descendant is still saved
            // as tasktype='subtask'. Valid subtasks are assigned one-by-one, while all
            // failures are collected and shown in one alert after processing finishes.
            function addHierarchyFromRight(type, id, projectId, key, left) {
                type = String(type || '').toLowerCase();

                // Keep direct Subtask drop on the existing, already-working path.
                if (type === 'subtask') {
                    return addFromRight(key, left);
                }

                var subtasks = collectHierarchyDropSubtasks(type, id, projectId, key);
                var nodeName = hierarchyDropNodeName(type, id, projectId);

                if (!subtasks.length) {
                    showHierarchyBatchFailureAlert([{
                        name: nodeName,
                        reason: 'No available subtask found under this ' + type + '.'
                    }], 0);
                    return false;
                }

                var startMinutes = vm.dayStart +
                    Math.round(left / vm.pixelsPerMinute / 15) * 15;
                var initialStartTime = hhmm(startMinutes);
                var pointProbe = {
                    StartTime: initialStartTime,
                    EndTime: hhmm(startMinutes + 1)
                };

                // Parent drop itself must start on the employee shift and on a free point.
                // The server can then find the next complete slot for each child Subtask.
                if (!newDropStartsInsideShift(pointProbe) || overlaps(pointProbe)) {
                    showHierarchyBatchFailureAlert(subtasks.map(function (s) {
                        return {
                            name: s.Name || 'Subtask',
                            reason: 'Drop only in available area.'
                        };
                    }), 0);
                    return false;
                }

                var employeeId = vm.activeUser && vm.activeUser.EmployeeId;
                var cursorDate = isoDate(vm.selectedDate);
                var cursorTime = initialStartTime;
                var failures = [];
                var assignedCount = 0;
                var chain = $q.when();

                vm.batchAssigning = true;

                subtasks.forEach(function (subtask) {
                    chain = chain.then(function () {
                        var validationReason = batchSubtaskValidationReason(subtask);
                        if (validationReason) {
                            failures.push({
                                name: subtask.Name || 'Subtask',
                                reason: validationReason
                            });
                            return null;
                        }

                        return api.findNextAvailableSubtaskStart(
                            subtask,
                            employeeId,
                            cursorDate,
                            cursorTime
                        ).then(function (rawSlot) {
                            var slot = parseHierarchyNextSlot(rawSlot, cursorDate, cursorTime);
                            if (!slot.ok) {
                                failures.push({
                                    name: subtask.Name || 'Subtask',
                                    reason: slot.reason
                                });
                                return null;
                            }

                            return api.assignSubtaskDirect(
                                subtask,
                                employeeId,
                                slot.startDate,
                                slot.startTime
                            ).then(function (result) {
                                if (String(result) !== '1') {
                                    failures.push({
                                        name: subtask.Name || 'Subtask',
                                        reason: String(result || 'Assignment failed.')
                                    });
                                    return null;
                                }

                                assignedCount++;
                                cursorDate = slot.endDate || slot.startDate;
                                cursorTime = slot.endTime || slot.startTime;
                                return null;
                            }, function (error) {
                                failures.push({
                                    name: subtask.Name || 'Subtask',
                                    reason: hierarchyDropErrorText(error)
                                });
                                return null;
                            });
                        }, function (error) {
                            failures.push({
                                name: subtask.Name || 'Subtask',
                                reason: hierarchyDropErrorText(error)
                            });
                            return null;
                        });
                    });
                });

                chain.then(function () {
                    return refreshAssignPopupAfterBatch(employeeId);
                }).then(function () {
                    vm.batchAssigning = false;
                    if (assignedCount > 0 && failures.length === 0) {
                        notify('Assignment saved automatically.', true);
                    }
                    showHierarchyBatchFailureAlert(failures, assignedCount);
                    $scope.$applyAsync();
                }).catch(function (error) {
                    vm.batchAssigning = false;
                    failures.push({
                        name: nodeName,
                        reason: hierarchyDropErrorText(error)
                    });

                    refreshAssignPopupAfterBatch(employeeId).then(function () {
                        showHierarchyBatchFailureAlert(failures, assignedCount);
                        $scope.$applyAsync();
                    });
                });

                return true;
            }

            function removeSubtaskFromTree(key) {
                (vm.assignmentTree || []).forEach(function (project) {
                    (project.Milestones || []).forEach(function (milestone) {
                        (milestone.Tasks || []).forEach(function (task) {
                            task.Subtasks = (task.Subtasks || []).filter(function (sub) {
                                return sub.Key !== key;
                            });
                        });
                        milestone.Tasks = (milestone.Tasks || []).filter(function (task) {
                            return (task.Subtasks || []).length > 0;
                        });
                    });
                    project.Milestones = (project.Milestones || []).filter(function (milestone) {
                        return (milestone.Tasks || []).length > 0;
                    });
                });

                vm.assignmentTree = (vm.assignmentTree || []).filter(function (project) {
                    return (project.Milestones || []).length > 0;
                });
            }

            // =========================================================
            // ASSIGNMENT VALIDATION
            // =========================================================
            function overlaps(item, ignore) {
                var s = minutes(item.StartTime);
                var e = minutes(item.EndTime);

                if (e <= s) {
                    e += 1440;
                }

                return vm.editAssignments.some(function (a) {
                    if (a.ClientKey === ignore) {
                        return false;
                    }

                    var x = minutes(a.StartTime);
                    var y = minutes(a.EndTime);

                    if (y <= x) {
                        y += 1440;
                    }

                    return s < y && e > x;
                });
            }

            function withinShift(item) {
                var s = minutes(item.StartTime);
                var e = minutes(item.EndTime);

                if (e <= s) {
                    e += 1440;
                }

                return s >= vm.dayStart && e <= vm.dayEnd;
            }

            // =========================================================
            // AUTO SAVE
            // =========================================================
            function scheduleAutoSave() {
                if (autoSaveTimer) {
                    $timeout.cancel(autoSaveTimer);
                }

                autoSaveTimer = $timeout(function () {
                    vm.saveAssignments(true);
                }, 150);
            }

            // =========================================================
            // ADD TASK FROM RIGHT PANEL TO TIMELINE
            // =========================================================
            function addFromRight(key, left) {
                var t = (vm.allAvailableTasks || []).find(function (x) {
                    return x.Key === key;
                });

                if (!t) {
                    return false;
                }

                var dur = parseInt(t.DurationMinutes, 10) || 0;
                var start =
                    vm.dayStart +
                    Math.round(left / vm.pixelsPerMinute / 15) * 15;

                var a = {
                    ClientKey: 'new-' + Date.now(),
                    AssignId: 0,
                    EntityId: t.EntityId,
                    Type: 'subtask',
                    ProjectId: t.ProjectId,
                    MilestoneId: t.MilestoneId,
                    TaskId: t.TaskId || 0,
                    SubTaskId: t.SubTaskId || t.EntityId || 0,
                    Title: t.Name,
                    StartTime: hhmm(start),
                    EndTime: hhmm(start + dur),
                    Duration: durationValue(dur),
                    DurationMinutes: dur,
                    IsNew: true
                };

                // Run all applicable Assign Project-style validations BEFORE changing
                // the right hierarchy or timeline. Invalid drop therefore rolls back naturally.
                if (!validateNewSubtaskDrop(t, a)) {
                    return false;
                }

                vm.editAssignments.push(a);

                vm.allAvailableTasks = vm.allAvailableTasks.filter(function (x) {
                    return x.Key !== key;
                });

                removeSubtaskFromTree(key);

                $scope.$applyAsync();
                $timeout(enableInteraction);
                scheduleAutoSave();
                return true;
            }

            // =========================================================
            // MOVE / RESIZE EXISTING TASK
            // =========================================================
            function updatePosition(key, left, width) {
                var a = vm.editAssignments.find(function (x) {
                    return x.ClientKey === key;
                });

                if (!a) {
                    return;
                }

                var old = copy(a);

                var start =
                    vm.dayStart +
                    Math.round(left / vm.pixelsPerMinute / 15) * 15;

                var dur = Math.max(
                    15,
                    Math.round(width / vm.pixelsPerMinute / 15) * 15
                );

                a.StartTime = hhmm(start);
                a.EndTime = hhmm(start + dur);
                a.DurationMinutes = dur;
                a.Duration = durationValue(dur);

                a.EditMode =
                    Math.abs(dur - (old.DurationMinutes || dur)) > 0
                        ? 'resize-right'
                        : '';

                if (!withinShift(a) || overlaps(a, key)) {
                    angular.extend(a, old);
                    notify('Drop only in available area.', false);
                } else {
                    a.IsChanged = true;
                    scheduleAutoSave();
                }

                $scope.$applyAsync();
            }

            // =========================================================
            // DRAG / DROP / RESIZE INTERACTION
            // =========================================================
            function enableInteraction() {
                if (!window.interact) {
                    return;
                }

                // Subtask leaves keep the existing direct drag path. Milestone/Task
                // use an additional parent-drag shortcut defined below.
                // Use a body-level floating preview (same approach as Assign Project).
                // The original hierarchy card never moves, so it is not clipped/hidden
                // by the right-panel overflow while dragging across the center board.
                try {
                    interact('.external-subtask').unset();
                } catch (ignoreUnset) {
                }

                try {
                    interact('.external-assign-parent').unset();
                } catch (ignoreParentUnset) {
                }

                function getExternalSubtask(el) {
                    if (!el) {
                        return null;
                    }

                    if (el.closest) {
                        return el.closest('.external-subtask');
                    }

                    while (el && el !== document) {
                        if (el.classList && el.classList.contains('external-subtask')) {
                            return el;
                        }
                        el = el.parentNode;
                    }

                    return null;
                }

                function positionSubtaskPreview(preview, e) {
                    if (!preview || !e) {
                        return;
                    }

                    var w = preview.offsetWidth || 220;
                    var h = preview.offsetHeight || 32;
                    var left = (e.clientX || 0) + 12;
                    var top = (e.clientY || 0) - h - 6;

                    if (left + w > window.innerWidth - 6) {
                        left = Math.max(6, window.innerWidth - w - 6);
                    }

                    if (top < 6) {
                        top = Math.min(window.innerHeight - h - 6, (e.clientY || 0) + 12);
                    }

                    preview.style.left = left + 'px';
                    preview.style.top = top + 'px';
                }

                function createSubtaskPreview(original, e) {
                    if (!original) {
                        return null;
                    }

                    var preview = document.createElement('div');
                    preview.className = 'myteam-subtask-drag-preview';

                    var icon = document.createElement('i');
                    icon.className = 'bx bx-grid-vertical drag-icon';

                    var text = document.createElement('div');
                    text.className = 'preview-text';

                    var title = document.createElement('div');
                    title.className = 'preview-title';

                    var sourceTitle = original.querySelector('.title > div:first-child');
                    title.textContent = sourceTitle ? sourceTitle.textContent : 'Subtask';

                    var meta = document.createElement('div');
                    meta.className = 'preview-meta';

                    var sourceMeta = original.querySelector('.task-meta');
                    meta.textContent = sourceMeta ? sourceMeta.textContent : 'Subtask';

                    text.appendChild(title);
                    text.appendChild(meta);
                    preview.appendChild(icon);
                    preview.appendChild(text);
                    document.body.appendChild(preview);

                    positionSubtaskPreview(preview, e);
                    return preview;
                }

                function clearSubtaskPreview(interaction) {
                    if (!interaction) {
                        return;
                    }

                    var original = interaction.myTeamOriginal;
                    var preview = interaction.myTeamPreview;

                    if (original && original.classList) {
                        original.classList.remove('subtask-drag-source');
                        original.style.transform = '';
                    }

                    if (preview && preview.parentNode) {
                        preview.parentNode.removeChild(preview);
                    }

                    interaction.myTeamOriginal = null;
                    interaction.myTeamPreview = null;
                }

                interact('.external-subtask').draggable({
                    listeners: {
                        start: function (e) {
                            var original = getExternalSubtask(e.target);

                            if (!original || !e.interaction) {
                                return;
                            }

                            e.interaction.myTeamOriginal = original;
                            e.interaction.myTeamPreview = createSubtaskPreview(original, e);
                            original.classList.add('subtask-drag-source');
                        },
                        move: function (e) {
                            if (!e.interaction) {
                                return;
                            }

                            positionSubtaskPreview(e.interaction.myTeamPreview, e);
                        },
                        end: function (e) {
                            var interaction = e.interaction;
                            var original = interaction && interaction.myTeamOriginal
                                ? interaction.myTeamOriginal
                                : getExternalSubtask(e.target);

                            var key = original ? original.getAttribute('data-key') : '';
                            var row = document.getElementById('assignRow');
                            var r = row && row.getBoundingClientRect();

                            if (
                                key &&
                                r &&
                                e.client.x >= r.left &&
                                e.client.x <= r.right &&
                                e.client.y >= r.top &&
                                e.client.y <= r.bottom
                            ) {
                                addFromRight(
                                    key,
                                    e.client.x - r.left
                                );
                            }

                            clearSubtaskPreview(interaction);
                        }
                    }
                });

                function getExternalAssignParent(el) {
                    if (!el) return null;
                    if (el.closest) return el.closest('.external-assign-parent');
                    while (el && el !== document) {
                        if (el.classList && el.classList.contains('external-assign-parent')) return el;
                        el = el.parentNode;
                    }
                    return null;
                }

                function createParentPreview(original, e) {
                    if (!original) return null;

                    var preview = document.createElement('div');
                    preview.className = 'myteam-subtask-drag-preview';

                    var icon = document.createElement('i');
                    icon.className = 'bx bx-grid-vertical drag-icon';

                    var text = document.createElement('div');
                    text.className = 'preview-text';

                    var title = document.createElement('div');
                    title.className = 'preview-title';
                    var sourceTitle = original.querySelector('.tree-name');
                    title.textContent = sourceTitle ? sourceTitle.textContent : 'Hierarchy';

                    var meta = document.createElement('div');
                    meta.className = 'preview-meta';
                    var type = String(original.getAttribute('data-type') || '').toLowerCase();
                    var sourceDuration = original.querySelector('.tree-duration');
                    meta.textContent = (type === 'milestone' ? 'Milestone' : 'Task') +
                        ' - assigns child subtasks' +
                        (sourceDuration ? ' ' + sourceDuration.textContent : '');

                    text.appendChild(title);
                    text.appendChild(meta);
                    preview.appendChild(icon);
                    preview.appendChild(text);
                    document.body.appendChild(preview);
                    positionSubtaskPreview(preview, e);
                    return preview;
                }

                function clearParentPreview(interaction) {
                    if (!interaction) return;
                    var original = interaction.myTeamParentOriginal;
                    var preview = interaction.myTeamParentPreview;

                    if (original && original.classList) {
                        original.classList.remove('parent-drag-source');
                        original.style.transform = '';
                    }
                    if (preview && preview.parentNode) preview.parentNode.removeChild(preview);

                    interaction.myTeamParentOriginal = null;
                    interaction.myTeamParentPreview = null;
                }

                // Milestone/Task can be dragged as shortcuts. The drop handler expands them
                // to their descendant Subtasks and saves only those Subtask rows.
                interact('.external-assign-parent').draggable({
                    ignoreFrom: 'i',
                    listeners: {
                        start: function (e) {
                            var original = getExternalAssignParent(e.target);
                            if (!original || !e.interaction) return;

                            e.interaction.myTeamParentOriginal = original;
                            e.interaction.myTeamParentPreview = createParentPreview(original, e);
                            original.classList.add('parent-drag-source');
                        },
                        move: function (e) {
                            if (!e.interaction) return;
                            positionSubtaskPreview(e.interaction.myTeamParentPreview, e);
                        },
                        end: function (e) {
                            var interaction = e.interaction;
                            var original = interaction && interaction.myTeamParentOriginal
                                ? interaction.myTeamParentOriginal
                                : getExternalAssignParent(e.target);

                            var type = original ? original.getAttribute('data-type') : '';
                            var id = original ? original.getAttribute('data-id') : '';
                            var projectId = original ? original.getAttribute('data-project-id') : '';
                            var row = document.getElementById('assignRow');
                            var r = row && row.getBoundingClientRect();

                            if (
                                type && id && r &&
                                e.client.x >= r.left && e.client.x <= r.right &&
                                e.client.y >= r.top && e.client.y <= r.bottom
                            ) {
                                addHierarchyFromRight(
                                    type,
                                    id,
                                    projectId,
                                    '',
                                    e.client.x - r.left
                                );
                            }

                            clearParentPreview(interaction);
                        }
                    }
                });

                // Move / resize task already present in Assign Task popup.
                interact('#assignRow .planned-block')
                    .draggable({
                        ignoreFrom: '.resize-handle,.ri-close-line',
                        listeners: {
                            move: function (e) {
                                var x = (+e.target.dataset.x || 0) + e.dx;

                                e.target.style.transform =
                                    'translateX(' + x + 'px)';

                                e.target.dataset.x = x;
                            },
                            end: function (e) {
                                var left =
                                    parseFloat(e.target.style.left) +
                                    (+e.target.dataset.x || 0);

                                e.target.style.transform = '';
                                e.target.dataset.x = 0;

                                updatePosition(
                                    e.target.dataset.key,
                                    left,
                                    e.target.offsetWidth
                                );
                            }
                        }
                    })
                    .resizable({
                        edges: {
                            right: '.resize-handle'
                        },
                        listeners: {
                            move: function (e) {
                                e.target.style.width = e.rect.width + 'px';
                            },
                            end: function (e) {
                                updatePosition(
                                    e.target.dataset.key,
                                    parseFloat(e.target.style.left),
                                    e.rect.width
                                );
                            }
                        }
                    });
            }

            // =========================================================
            // REMOVE ASSIGNMENT
            // =========================================================
            vm.removeAssignment = function (a, e) {
                if (e) {
                    e.preventDefault && e.preventDefault();
                    e.stopPropagation && e.stopPropagation();
                }

                // Hide/dispose the currently visible assignment tooltip before the
                // hovered assignment DOM node is removed.
                disposeAssignmentTooltipFromEvent(e);

                if (a.AssignId) {
                    vm.removedAssignments.push({
                        AssignId: a.AssignId,
                        Type: a.Type
                    });
                }

                vm.editAssignments = vm.editAssignments.filter(function (x) {
                    return x.ClientKey !== a.ClientKey;
                });

                if (a.IsNew) {
                    vm.loadAssignableTasks();
                }

                scheduleAutoSave();
            };

            // =========================================================
            // SAVE ASSIGNMENTS
            // =========================================================
            vm.saveAssignments = function (auto) {
                if (!vm.activeUser) {
                    return;
                }

                if (vm.saving) {
                    vm.saveAgain = true;
                    return;
                }

                var changed = vm.editAssignments.filter(function (a) {
                    return a.IsNew || a.IsChanged;
                });

                var removed = vm.removedAssignments.slice(0);

                if (!changed.length && !removed.length) {
                    return;
                }

                vm.saving = true;

                var employeeId = vm.activeUser.EmployeeId;

                var request = {
                    EmployeeId: employeeId,
                    WorkDate: isoDate(vm.selectedDate),
                    RemovedAssignments: removed,
                    Assignments: changed.map(function (a) {
                        return {
                            AssignId: a.AssignId || 0,
                            EntityId: a.EntityId,
                            TaskType: a.Type,
                            StartTime: a.StartTime,
                            Duration:
                                a.Duration ||
                                durationValue(a.DurationMinutes),
                            OldStartTime: a.OldStartTime || '',
                            OldWorkDate: a.OldWorkDate || '',
                            EditMode: a.EditMode || ''
                        };
                    })
                };

                api.save(request).then(function (r) {
                    notify(
                        auto
                            ? 'Assignment saved automatically.'
                            : (r.Message || 'Assignments saved successfully.'),
                        true
                    );

                    return vm.reload();
                }).then(function (users) {
                    vm.saving = false;

                    var user = (users || []).find(function (u) {
                        return u.EmployeeId === employeeId;
                    });

                    if (user) {
                        prepareAssignments(user);
                        vm.loadAssignableTasks();
                        $timeout(enableInteraction, 200);
                    }

                    if (vm.saveAgain) {
                        vm.saveAgain = false;
                        scheduleAutoSave();
                    }
                }, function (error) {
                    // Server repeats all critical validations. If it rejects stale data
                    // (team/date/duration/already-assigned/overlap/etc.), restore the popup
                    // from DB so the temporary card and right-tree removal are rolled back.
                    vm.saving = false;
                    vm.saveAgain = false;

                    var message = angular.isString(error) && error
                        ? error
                        : 'Operation failed.';

                    notify(message, false);
                    try {
                        window.alert(message);
                    } catch (e) {
                    }

                    vm.reload().then(function (users) {
                        var user = (users || []).find(function (u) {
                            return u.EmployeeId === employeeId;
                        });

                        if (user) {
                            prepareAssignments(user);
                        }

                        vm.loadAssignableTasks();
                        $timeout(enableInteraction, 200);
                    });
                });
            };

            // =========================================================
            // START PAGE
            // =========================================================
            vm.init();
        }
    ]);
})();
