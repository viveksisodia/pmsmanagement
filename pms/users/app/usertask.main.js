
app.controller("usertaskCtrl", function ($scope,$q,$http,$timeout,$document,usertaskService,$compile,$filter) {
   //alert("ok");
 console.log("controller loaded");
    // Keep the page mode while date presets (Previous Week/Month/Year, etc.)
    // continue supplying their existing from/to dates.
    var taskPageQuery = new URLSearchParams(window.location.search || '');
    $scope.taskListType = String(taskPageQuery.get('type') || '').trim().toLowerCase();
    if (!$scope.taskListType) {
        $scope.taskListType = 'all';
    } else if ($scope.taskListType !== 'delayedtask') {
        $scope.taskListType = 'mytask';
    }
    // Carry the exact main date range to the task's new tab.
    var linkedFromDate = moment(taskPageQuery.get('fromdate') || '', 'YYYY-MM-DD', true);
    var linkedToDate = moment(taskPageQuery.get('todate') || '', 'YYYY-MM-DD', true);
    var hasLinkedDateRange = linkedFromDate.isValid() && linkedToDate.isValid()
        && !linkedFromDate.isAfter(linkedToDate, 'day');
    $scope.getUserTaskPageUrl = function (task) {
        if (!task || task.id == null) return '';
        var query = new URLSearchParams();
        query.set('id', String(task.id));
        var picker = $('#daterange').data('daterangepicker');
        if (picker) {
            query.set('fromdate', picker.startDate.format('YYYY-MM-DD'));
            query.set('todate', picker.endDate.format('YYYY-MM-DD'));
            if (picker.chosenLabel) query.set('range', picker.chosenLabel);
        } else if (hasLinkedDateRange) {
            query.set('fromdate', linkedFromDate.format('YYYY-MM-DD'));
            query.set('todate', linkedToDate.format('YYYY-MM-DD'));
        }
        return '/pms/users/default.aspx?' + query.toString();
    };
    $scope.openUserTaskPage = function (task, event) {
        if (event) {
            event.preventDefault();
            event.stopPropagation();
        }
        if (!task || task.id == null || String(task.id).trim() === '') return;
        window.open($scope.getUserTaskPageUrl(task), '_blank', 'noopener');
    };
    var highlightedTaskId = String(taskPageQuery.get('id') || '').trim();
    $scope.isQueryTaskHighlighted = function (task) {
        return !!highlightedTaskId && !!task && String(task.id) === highlightedTaskId;
    };
 //#region add variables
$scope.showTagDropdown = false;
$scope.mytasklist_bystatus =[];
    $scope.isload = "0";
    $scope.addnew_tag = "0";
    $scope.multiselect = "0";
    $scope.multiselectids = [];
    $scope.multiselect_status = [];
    $scope.selectedStatus =[];
    $scope.freesearch = "0";
   
    $scope.mytasklist_history = [];
    $scope.currentlistIndex = "0";
    $scope.currentlist=[];
    $scope.tagtype="project";
    $scope.currenttaskid="0";
    $scope.showlist = true;
    $scope.owners=[];
    $scope.mytasklist = [];
    $scope.mytasktaglist = [];
    $scope.selectedtagslistids = [];
    $scope.selectedtagIds = [];
    $scope.mytasklist_status = [];
    $scope.currentPage = 1;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.types = ["1", "2"];
    $scope.pageSize = 10;
    $scope.statuslists = [];
    $scope.prioritylists = [];

    // Integrated My Task views. List remains the default and keeps its existing DOM/working.
    $scope.taskView = "list";
    $scope.taskViewHours = ["09:00","10:00","11:00","12:00","13:00","14:00","15:00","16:00","17:00"];
    $scope.taskViewTimesheetCellWidth = 90;
    $scope.taskViewTimesheetStartMinutes = 9 * 60;
    $scope.taskViewTimesheetEndMinutes = 18 * 60;
    $scope.taskViewTimesheetWidth = 9 * 90;
    $scope.taskViewWeekDays = [];
    $scope.taskViewCalendarDays = [];
    $scope.taskViewCalendarTitle = "";
    $scope.taskViewSelectedCalendarKey = "";
    $scope.taskViewSelectedCalendarLabel = "";
    $scope.taskViewSelectedCalendarTasks = [];
    $scope.taskViewCalendarStatusId = "";
    $scope.taskViewStatusMap = {};
    $scope.taskViewPriorityMap = {};
    $scope.kanban = { groupBy: "status" };
    $scope.taskViewDateMap = {};
    $scope.taskViewWeekTotals = {};
    $scope.taskViewAssignedTotal = "00:00";
    $scope.taskViewActualTotal = "00:00";
    // Weekly report has its own shift-capacity / day-wise actual-time state.
    // Existing List/Kanban/Timesheet/Calendar data stays untouched.
    $scope.taskViewWeekStartDate = null;
    $scope.taskViewWeekRangeLabel = "";
    $scope.taskViewWeekWorkMap = {};
    $scope.taskViewWeekCapacityTotal = "00:00";
    $scope.taskViewWeekActualTotal = "00:00";
    $scope.taskViewWeekShiftStart = "";
    $scope.taskViewWeekShiftEnd = "";
    $scope.taskViewWeekShiftTime = "00:00";
    $scope.taskViewWeekWorkingDays = 0;
    $scope.taskViewWeekLoading = false;
    $scope.taskViewCalendarMonthDate = null;
    //#endregion
    // My Task tooltip cleanup only. Bootstrap tooltips use body as container,
    // so force-hide the current tooltip as soon as its trigger is left.
    // This does not change any timer/task behavior.
    function bindMyTaskTooltipAutoHide() {
        try {
            var root = document.querySelector('.main-wrapper');
            if (!root || root.__myTaskTooltipAutoHideBound) return;
            root.__myTaskTooltipAutoHideBound = true;

            root.addEventListener('mouseout', function (evt) {
                var target = evt.target;
                if (!target || !target.closest) return;

                var trigger = target.closest('[data-bs-toggle="tooltip"]');
                if (!trigger || !root.contains(trigger)) return;

                // Ignore movement between child elements of the same tooltip trigger.
                if (evt.relatedTarget && trigger.contains(evt.relatedTarget)) return;

                window.setTimeout(function () {
                    try {
                        if (window.bootstrap && bootstrap.Tooltip) {
                            var instance = bootstrap.Tooltip.getInstance(trigger);
                            if (instance) instance.hide();
                        }

                        // Safety cleanup for an old/orphan body tooltip left by a previous
                        // Bootstrap instance. Only runs after leaving a My Task tooltip trigger.
                        document.querySelectorAll('body > .tooltip.show').forEach(function (tip) {
                            tip.classList.remove('show');
                            window.setTimeout(function () {
                                try { if (tip && tip.parentNode) tip.parentNode.removeChild(tip); } catch (e) { }
                            }, 160);
                        });
                    } catch (e) { }
                }, 0);
            }, true);
        } catch (e) { }
    }

    $timeout(bindMyTaskTooltipAutoHide, 0);

    //#region add shorting
    $scope.sortColumn = "id";
    $scope.reverseSort = false;
   $scope.tagsnew = {
        id: "",
        tagname: "",
        tagcolor: ""
    };
    function toServerDateOnly(value) {
        var parts = getDateOnlyParts(value);
        if (!isValidDateOnlyParts(parts)) return null;

        return parts.year + '-' +
            String(parts.month).padStart(2, '0') + '-' +
            String(parts.day).padStart(2, '0');
    }
    function getDateOnlyParts(value) {
        if (value === null || value === undefined || value === '') return null;

        // A Date created by parseDate() uses local noon and therefore preserves
        // its calendar components in every browser timezone.
        if (Object.prototype.toString.call(value) === '[object Date]') {
            if (isNaN(value.getTime())) return null;
            return {
                year: value.getFullYear(),
                month: value.getMonth() + 1,
                day: value.getDate()
            };
        }

        var text = String(value).trim();
        var match;

        // Preferred API format: yyyy-MM-dd (optionally followed by a time).
        match = /^(\d{4})-(\d{2})-(\d{2})(?:[T\s].*)?$/.exec(text);
        if (match) {
            return { year: +match[1], month: +match[2], day: +match[3] };
        }

        // UI format: dd/MM/yyyy or dd-MM-yyyy.
        match = /^(\d{2})[\/-](\d{2})[\/-](\d{4})$/.exec(text);
        if (match) {
            return { year: +match[3], month: +match[2], day: +match[1] };
        }

        // Backward compatibility for old ASP.NET /Date(...)/ responses.
        //
        // SQL project dates are calendar dates created in the India business
        // timezone. ASP.NET can serialize 30/06/2026 midnight as a UTC millisecond
        // value representing 29/06/2026 18:30 UTC. Reading that value with the
        // browser's local getters makes Dubai/USA show the previous day.
        //
        // Decode the source calendar components first, then parseDate() creates a
        // local-noon Date object. Existing timeline calculations therefore keep
        // working without browser-timezone date shifting.
        match = /^\/Date\((-?\d+)(?:([+-])(\d{2})(\d{2}))?\)\/$/.exec(text);
        if (match) {
            var milliseconds = parseInt(match[1], 10);
            if (!isFinite(milliseconds)) return null;

            // Prefer an explicit ASP.NET offset when supplied.
            // Older JavaScriptSerializer responses normally omit it, so use the
            // application's India date-source offset (+05:30) as the fallback.
            var sourceOffsetMinutes = 330;

            if (match[2] && match[3] && match[4]) {
                sourceOffsetMinutes =
                    (parseInt(match[3], 10) * 60) +
                    parseInt(match[4], 10);

                if (match[2] === '-') {
                    sourceOffsetMinutes = -sourceOffsetMinutes;
                }
            }

            var sourceCalendar = new Date(
                milliseconds + (sourceOffsetMinutes * 60000)
            );

            if (isNaN(sourceCalendar.getTime())) return null;

            return {
                year: sourceCalendar.getUTCFullYear(),
                month: sourceCalendar.getUTCMonth() + 1,
                day: sourceCalendar.getUTCDate()
            };
        }

        return null;
    }

    function isValidDateOnlyParts(parts) {
        if (!parts) return false;

        var test = new Date(parts.year, parts.month - 1, parts.day, 12, 0, 0, 0);
        return test.getFullYear() === parts.year &&
            test.getMonth() === parts.month - 1 &&
            test.getDate() === parts.day;
    }
    function toServerDate(value) {
        return toServerDateOnly(value) || '';
    }
    function formatDateOnly(value) {
        var parts = getDateOnlyParts(value);
        if (!isValidDateOnlyParts(parts)) return '';

        return String(parts.day).padStart(2, '0') + '/' +
            String(parts.month).padStart(2, '0') + '/' +
            parts.year;
    }
    $scope.formatDateOnly = formatDateOnly;

    // =========================================================
    // Integrated alternate views (UI only)
    // =========================================================
    function taskViewDateFromParts(parts) {
        if (!parts) return null;
        return new Date(parts.year, parts.month - 1, parts.day, 12, 0, 0, 0);
    }

    function taskViewParseRangeDate(value) {
        if (!value) return null;
        var text = String(value).trim();
        var m = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/.exec(text);
        if (m) {
            var dt = new Date(+m[3], (+m[1]) - 1, +m[2], 12, 0, 0, 0); // MM/DD/YYYY from daterangepicker
            return isNaN(dt.getTime()) ? null : dt;
        }
        var parts = getDateOnlyParts(value);
        return isValidDateOnlyParts(parts) ? taskViewDateFromParts(parts) : null;
    }

    function taskViewDateKey(value) {
        var parts = getDateOnlyParts(value);
        if (!isValidDateOnlyParts(parts)) return '';
        return parts.year + '-' + String(parts.month).padStart(2, '0') + '-' + String(parts.day).padStart(2, '0');
    }

    function taskViewDateKeyFromDate(date) {
        if (!date || isNaN(date.getTime())) return '';
        return date.getFullYear() + '-' + String(date.getMonth() + 1).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0');
    }

    function taskViewDurationMinutes(value) {
        if (value === null || value === undefined || value === '') return 0;
        var text = String(value).trim();
        if (text.indexOf(':') > -1) {
            var p = text.split(':');
            var h = parseInt(p[0], 10) || 0;
            var m = parseInt(p[1], 10) || 0;
            return Math.max(0, h * 60 + m);
        }
        var n = parseFloat(text);
        return isFinite(n) ? Math.max(0, Math.round(n * 60)) : 0;
    }

    function taskViewTimeSeconds(value) {
        if (!value) return 0;
        var text = String(value).trim();
        var p = text.split(':');
        if (p.length < 2) return 0;
        var h = parseInt(p[0], 10) || 0;
        var m = parseInt(p[1], 10) || 0;
        var s = parseInt(p[2], 10) || 0;
        return Math.max(0, h * 3600 + m * 60 + s);
    }

    function taskViewFormatSeconds(total) {
        // Alternate report views display time only as HH:mm.
        total = Math.max(0, Math.floor(Number(total) || 0));
        var totalMinutes = Math.floor(total / 60);
        var h = Math.floor(totalMinutes / 60);
        var m = totalMinutes % 60;
        return String(h).padStart(2, '0') + ':' + String(m).padStart(2, '0');
    }

    function taskViewFormatMinutes(total) {
        return taskViewFormatSeconds((Number(total) || 0) * 60);
    }

    function taskViewClockMinutes(value) {
        if (!value) return null;
        var text = String(value).trim();
        var ampm = /\s*(AM|PM)$/i.exec(text);
        text = text.replace(/\s*(AM|PM)$/i, '');
        var p = text.split(':');
        if (!p.length) return null;
        var h = parseInt(p[0], 10);
        var m = parseInt(p[1] || '0', 10);
        if (!isFinite(h) || !isFinite(m)) return null;
        if (ampm) {
            var isPm = ampm[1].toUpperCase() === 'PM';
            h = h % 12;
            if (isPm) h += 12;
        }
        return h * 60 + m;
    }

    $scope.setTaskView = function (view) {
        if (["list","kanban","timesheet","weekly","calendar"].indexOf(view) === -1) return;
        $scope.taskView = view;
        if (view === 'list') {
            $timeout(function () { initTableUI(); }, 0);
        }
        else if (view === 'weekly') {
            // Weekly View is independent from the existing month/list filter.
            // First open defaults to the current Monday-Sunday week.
            if (!$scope.taskViewWeekStartDate) {
                taskViewBuildWeek(taskViewStartOfWeek(new Date()));
            }
            $scope.loadTaskViewWeeklyData();
        }
        else if (view === 'timesheet') {
            // Reuse the same read-only shift source so Timesheet Hours follow
            // the login/viewed employee's actual shift instead of fixed hours.
            if (!$scope.taskViewWeekStartDate) {
                taskViewBuildWeek(taskViewStartOfWeek(new Date()));
            }
            $scope.loadTaskViewWeeklyData();
        }
    };

    $scope.getTaskViewSerialNo = function (c) {
        if (!c) return '';

        var m = parseInt(c.MilestoneOrderNo || c.milestoneOrderNo || 0, 10) || 0;
        var t = parseInt(c.TaskOrderNo || c.taskOrderNo || 0, 10) || 0;
        var s = parseInt(c.SubTaskOrderNo || c.subTaskOrderNo || 0, 10) || 0;
        var type = String(c.tasktype || c.TaskType || '').toLowerCase();

        if (type === 'milestone') {
            return m > 0 ? (m + '.') : '';
        }
        if (type === 'task') {
            if (m > 0 && t > 0) return m + '.' + t;
            return t > 0 ? String(t) : '';
        }
        if (type === 'subtask') {
            if (m > 0 && t > 0 && s > 0) return m + '.' + t + '.' + s;
            if (t > 0 && s > 0) return t + '.' + s;
            return s > 0 ? String(s) : '';
        }

        // Safe fallback for any existing API row where tasktype is blank.
        if (s > 0) {
            if (m > 0 && t > 0) return m + '.' + t + '.' + s;
            if (t > 0) return t + '.' + s;
            return String(s);
        }
        if (t > 0) return m > 0 ? (m + '.' + t) : String(t);
        if (m > 0) return m + '.';
        return '';
    };

    $scope.getTaskViewName = function (c) {
        if (!c) return '';
        var name = c.Subtaskname || c.Taskname || c.Milestone || 'Task';
        var serial = $scope.getTaskViewSerialNo(c);
        return serial ? (serial + ' - ' + name) : name;
    };

    $scope.getTaskViewStatusColor = function (st) {
        var c = st && (st.colorname || st.statuscolor);
        return (!c || String(c).toUpperCase() === '#FFFFFF') ? '#804574' : c;
    };

    $scope.getTaskViewStatusTextColor = function (color) {
        return (!color || String(color).toUpperCase() === '#FFFFFF') ? '#111827' : color;
    };

    $scope.getTaskViewBorderColor = function (c) {
        if (!c) return '#3978de';
        var p = c.priority_color;
        if (p && String(p).toUpperCase() !== '#FFFFFF') return p;
        var s = c.status_color;
        return (s && String(s).toUpperCase() !== '#FFFFFF') ? s : '#3978de';
    };

    $scope.getTaskViewAssignedText = function (c) {
        var mins = c && c._taskViewDurationMinutes !== undefined ? c._taskViewDurationMinutes : taskViewDurationMinutes(c && c.TaskDuration);
        return taskViewFormatMinutes(mins);
    };

    $scope.getTaskViewActualText = function (c) {
        if (!c) return '00:00';
        var raw = c.isrun ? (c.timerText || '') : (c.timerText_spent || '');
        return taskViewFormatSeconds(taskViewTimeSeconds(raw));
    };

    $scope.hasKanbanSpentTime = function (c) {
        if (!c || !c.timerText_spent) return false;
        return taskViewTimeSeconds(c.timerText_spent) > 0;
    };

    $scope.getKanbanSpentTime = function (c) {
        if (!$scope.hasKanbanSpentTime(c)) return '';
        return String(c.timerText_spent).split('.').shift();
    };

    $scope.getTaskViewStatusTasks = function (statusId) {
        return $scope.taskViewStatusMap[String(statusId)] || [];
    };

    $scope.getTaskViewStatusCount = function (statusId) {
        return $scope.getTaskViewStatusTasks(statusId).length;
    };

    $scope.setKanbanGroupBy = function (mode) {
        mode = mode === 'priority' ? 'priority' : 'status';
        $scope.kanban.groupBy = mode;
    };

    $scope.getKanbanGroups = function () {
        // Always show every configured Status/Priority column,
        // even when a column currently has zero tasks.
        return $scope.kanban.groupBy === 'priority'
            ? ($scope.prioritylists || [])
            : ($scope.statuslists || []);
    };

    $scope.getKanbanGroupTasks = function (groupId) {
        var tasks;
        if ($scope.kanban.groupBy === 'priority') {
            tasks = $scope.taskViewPriorityMap[String(groupId)] || [];
        } else {
            tasks = $scope.taskViewStatusMap[String(groupId)] || [];
        }
        return tasks.filter($scope.taskTopStatusFilter);
    };

    $scope.getKanbanGroupCount = function (groupId) {
        return $scope.getKanbanGroupTasks(groupId).length;
    };

    $scope.getKanbanGroupColor = function (grp) {
        var c = grp && (grp.colorname || grp.statuscolor);
        return (!c || String(c).toUpperCase() === '#FFFFFF') ? '#804574' : c;
    };


    $scope.beginKanbanDrag = function (card) {
        $scope.kanban.dragCard = card || null;
    };

    $scope.endKanbanDrag = function () {
        $scope.kanban.dragCard = null;
    };

    $scope.dropKanbanCard = function (group) {
        var c = $scope.kanban.dragCard;
        if (!c || !group) return;

        if ($scope.kanban.groupBy === 'status') {
            if (String(c.P_Status) === String(group.id)) {
                $scope.endKanbanDrag();
                return;
            }

            // Reuse the existing status method so all existing server-side
            // status/completion/dependency/log behavior stays unchanged.
            $scope.selectStatus(c, group);
            $scope.endKanbanDrag();
            return;
        }

        if (String(c.Priority) === String(group.id)) {
            $scope.endKanbanDrag();
            return;
        }

        usertaskService.UpdateKanbanPriority(c.id, c.tasktype, group.id)
            .then(function (res) {
                if (!res || !res.data || res.data.d !== "1") {
                    alert((res && res.data && res.data.d) || "Priority update failed.");
                    return;
                }

                c.Priority = group.id;
                c.priority_name = group.name;
                c.priority_color = group.colorname;
                $scope.refreshTaskViewCaches();
                $scope.$applyAsync();
            })
            .catch(function () {
                // No optimistic move is made; failed saves therefore leave the
                // card in its original column automatically.
                alert("Priority update failed.");
            })
            .finally(function () {
                $scope.endKanbanDrag();
            });
    };

    $scope.getTaskViewTextColor = function (color) {
        return (!color || String(color).toUpperCase() === '#FFFFFF') ? '#111827' : color;
    };

    $scope.getTaskViewSoftColor = function (color) {
        if (!color || String(color).toUpperCase() === '#FFFFFF') return '#f3f4f6';
        var hex = String(color).trim();
        if (/^#[0-9a-fA-F]{6}$/.test(hex)) return hex + '22';
        return '#f3f4f6';
    };


    function taskViewClockText(value) {
        var mins = taskViewClockMinutes(value);
        if (mins === null) return '';
        mins = ((mins % 1440) + 1440) % 1440;
        return String(Math.floor(mins / 60)).padStart(2, '0') + ':' +
               String(mins % 60).padStart(2, '0');
    }

    function taskViewTimesheetLabel(totalMinutes) {
        var mins = ((totalMinutes % 1440) + 1440) % 1440;
        var hour24 = Math.floor(mins / 60);
        var minute = mins % 60;
        var suffix = hour24 >= 12 ? 'PM' : 'AM';
        var hour12 = hour24 % 12;
        if (hour12 === 0) hour12 = 12;
        return String(hour12).padStart(2, '0') + ':' +
               String(minute).padStart(2, '0') + ' ' + suffix;
    }

    function taskViewClockAmPmText(value) {
        var mins = taskViewClockMinutes(value);
        if (mins === null) return value || '';
        return taskViewTimesheetLabel(mins);
    }

    function taskViewBuildTimesheetScale(startText, endText) {
        var start = taskViewClockMinutes(startText);
        var end = taskViewClockMinutes(endText);

        if (start === null) start = 9 * 60;
        if (end === null) end = 18 * 60;
        if (end <= start) end += 1440;

        var span = Math.max(60, end - start);
        var slots = Math.max(1, Math.ceil(span / 60));
        var labels = [];

        for (var i = 0; i < slots; i++) {
            labels.push(taskViewTimesheetLabel(start + (i * 60)));
        }

        $scope.taskViewHours = labels;
        $scope.taskViewTimesheetStartMinutes = start;
        $scope.taskViewTimesheetEndMinutes = end;
        $scope.taskViewTimesheetWidth = slots * $scope.taskViewTimesheetCellWidth;
    }

    $scope.getTaskViewTimesheetBarStyle = function (c) {
        var cellWidth = $scope.taskViewTimesheetCellWidth || 90;
        var baseMinutes = $scope.taskViewTimesheetStartMinutes || (9 * 60);
        var endMinutes = $scope.taskViewTimesheetEndMinutes || (18 * 60);
        var totalWidth = $scope.taskViewTimesheetWidth || (9 * cellWidth);

        var start = taskViewClockMinutes(c && c.StartTime);
        if (start === null) start = baseMinutes;

        if (endMinutes > 1440 && start < baseMinutes) start += 1440;

        var duration = c && c._taskViewDurationMinutes !== undefined
            ? c._taskViewDurationMinutes
            : taskViewDurationMinutes(c && c.TaskDuration);

        if (duration <= 0) duration = 30;

        var left = Math.max(0, (start - baseMinutes) * cellWidth / 60);
        var width = Math.max(42, duration * cellWidth / 60);
        var maxWidth = totalWidth - left;

        if (maxWidth < 42) {
            left = Math.max(0, totalWidth - 42);
            maxWidth = 42;
        }

        width = Math.min(width, maxWidth);

        return {
            'left': Math.round(left) + 'px',
            'width': Math.round(width) + 'px',
            'background-color': $scope.getTaskViewBorderColor(c)
        };
    };

    function taskViewEntityKey(c) {
        if (!c) return '';
        return String(c.tasktype || 'subtask').toLowerCase() + '|' + String(c.id || '0');
    }

    function taskViewStartOfWeek(date) {
        var d = date && !isNaN(date.getTime())
            ? new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12, 0, 0, 0)
            : new Date();
        var mondayOffset = (d.getDay() + 6) % 7; // Mon=0 ... Sun=6
        return new Date(d.getFullYear(), d.getMonth(), d.getDate() - mondayOffset, 12, 0, 0, 0);
    }

    function taskViewWeekSeconds(c, dayKey) {
        var entityKey = taskViewEntityKey(c);
        if (!entityKey || !dayKey) return 0;
        return Number($scope.taskViewWeekWorkMap[entityKey + '|' + dayKey] || 0);
    }

    $scope.getTaskViewWeekCell = function (c, day) {
        if (!c || !day) return '00:00';
        return taskViewFormatSeconds(taskViewWeekSeconds(c, day.key));
    };

    $scope.getTaskViewWeekActualText = function (c) {
        if (!c) return '00:00';
        var seconds = 0;
        angular.forEach($scope.taskViewWeekDays || [], function (d) {
            seconds += taskViewWeekSeconds(c, d.key);
        });
        return taskViewFormatSeconds(seconds);
    };

    // Weekly Assigned Time is the employee's working capacity for the selected week.
    // It is intentionally separate from the existing TaskDuration used by List/Timesheet/etc.
    $scope.getTaskViewWeekAssignedText = function () {
        return $scope.taskViewWeekCapacityTotal || '00:00';
    };

    $scope.getTaskViewWeekAssignedGrandTotal = function () {
        var capacity = String($scope.taskViewWeekCapacityTotal || '00:00').split(':');
        var capacityMinutes = (parseInt(capacity[0], 10) || 0) * 60 + (parseInt(capacity[1], 10) || 0);
        var rowCount = ($scope.mytasklist || []).filter($scope.taskTopStatusFilter).length;
        return taskViewFormatMinutes(capacityMinutes * rowCount);
    };

    $scope.getTaskViewWeekFilteredTotal = function (dayKey) {
        var seconds = 0;
        angular.forEach(($scope.mytasklist || []).filter($scope.taskTopStatusFilter), function (task) {
            seconds += taskViewWeekSeconds(task, dayKey);
        });
        return taskViewFormatSeconds(seconds);
    };

    $scope.getTaskViewWeekFilteredActualTotal = function () {
        var seconds = 0;
        angular.forEach(($scope.mytasklist || []).filter($scope.taskTopStatusFilter), function (task) {
            angular.forEach($scope.taskViewWeekDays || [], function (day) {
                seconds += taskViewWeekSeconds(task, day.key);
            });
        });
        return taskViewFormatSeconds(seconds);
    };

    $scope.getTaskViewWeekTooltip = function (c, day) {
        if (!day) return '';
        var total = day.totalTime || $scope.taskViewWeekShiftTime || '00:00';
        var spent = $scope.getTaskViewWeekCell(c, day);
        var lines = [
            'Total Time : ' + total + ' hrs',
            'Spent Time : ' + spent + ' hrs'
        ];
        if (day.isHoliday) lines.push('Status : Holiday');
        else if (day.isWeekOff) lines.push('Status : Week Off');
        return lines.join('\n');
    };

    $scope.getTaskViewWeekTotal = function (key) {
        return $scope.taskViewWeekTotals[key] || '00:00';
    };

    $scope.getTaskViewWeekTotalTooltip = function (day) {
        if (!day) return '';
        var total = day.totalTime || '00:00';
        var spent = $scope.getTaskViewWeekFilteredTotal(day.key);
        var lines = [
            'Total Time : ' + total + ' hrs',
            'Spent Time : ' + spent + ' hrs'
        ];
        if (day.isHoliday) lines.push('Status : Holiday');
        else if (day.isWeekOff) lines.push('Status : Week Off');
        return lines.join('\n');
    };

    function taskViewBuildWeek(startDate) {
        startDate = taskViewStartOfWeek(startDate || new Date());
        $scope.taskViewWeekStartDate = startDate;
        $scope.taskViewWeekDays = [];

        for (var i = 0; i < 7; i++) {
            var d = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() + i, 12, 0, 0, 0);
            $scope.taskViewWeekDays.push({
                date: d,
                key: taskViewDateKeyFromDate(d),
                dayName: d.toLocaleString('en-US', { weekday: 'short' }),
                label: String(d.getDate()).padStart(2, '0') + ' ' + d.toLocaleString('en-US', { month: 'short' }),
                isWeekOff: false,
                isHoliday: false,
                totalTime: '00:00',
                actualTime: '00:00'
            });
        }

        var end = $scope.taskViewWeekDays[6].date;
        function weekDateText(d) {
            return String(d.getDate()).padStart(2, '0') + '/' +
                   String(d.getMonth() + 1).padStart(2, '0') + '/' +
                   d.getFullYear();
        }
        $scope.taskViewWeekRangeLabel = weekDateText(startDate) + ' - ' + weekDateText(end);
    }

    function taskViewApplyWeeklyData(data) {
        data = data || {};
        var dayMeta = {};
        angular.forEach(data.days || [], function (d) {
            dayMeta[d.key] = d;
        });

        angular.forEach($scope.taskViewWeekDays || [], function (d) {
            var m = dayMeta[d.key];
            if (!m) return;
            d.isWeekOff = !!m.isWeekOff;
            d.isHoliday = !!m.isHoliday;
            d.totalTime = m.totalTime || '00:00';
            d.actualTime = m.actualTime || '00:00';
        });

        var workMap = {};
        angular.forEach(data.work || [], function (w) {
            if (!w || !w.entityKey || !w.date) return;
            workMap[w.entityKey + '|' + w.date] = Number(w.seconds || 0);
        });
        $scope.taskViewWeekWorkMap = workMap;

        var rawShiftStart = data.shiftStart || '09:00 AM';
        var rawShiftEnd = data.shiftEnd || '06:00 PM';
        taskViewBuildTimesheetScale(rawShiftStart, rawShiftEnd);

        $scope.taskViewWeekShiftStart = taskViewClockAmPmText(rawShiftStart);
        $scope.taskViewWeekShiftEnd = taskViewClockAmPmText(rawShiftEnd);
        $scope.taskViewWeekShiftTime = data.shiftTime || '00:00';
        $scope.taskViewWeekCapacityTotal = data.weekCapacity || '00:00';
        $scope.taskViewWeekActualTotal = data.weekActualTime || '00:00';
        $scope.taskViewWeekWorkingDays = Number(data.workingDays || 0);

        var totals = {};
        angular.forEach($scope.taskViewWeekDays || [], function (d) {
            totals[d.key] = d.actualTime || '00:00';
        });
        $scope.taskViewWeekTotals = totals;
    }

    $scope.loadTaskViewWeeklyData = function () {
        if (!$scope.taskViewWeekStartDate) {
            taskViewBuildWeek(taskViewStartOfWeek(new Date()));
        }
        if (!usertaskService.getWeeklyReportData) return;

        var startKey = taskViewDateKeyFromDate($scope.taskViewWeekStartDate);
        var endDate = new Date(
            $scope.taskViewWeekStartDate.getFullYear(),
            $scope.taskViewWeekStartDate.getMonth(),
            $scope.taskViewWeekStartDate.getDate() + 6,
            12, 0, 0, 0
        );
        var endKey = taskViewDateKeyFromDate(endDate);

        $scope.taskViewWeekLoading = true;
        usertaskService.getWeeklyReportData(startKey, endKey)
            .then(function (response) {
                taskViewApplyWeeklyData((response.data && response.data.d) || {});
            })
            .catch(function (err) {
                console.error('Weekly report load failed:', err);
            })
            .finally(function () {
                $scope.taskViewWeekLoading = false;
            });
    };

    $scope.changeTaskViewWeek = function (delta) {
        var start = $scope.taskViewWeekStartDate || taskViewStartOfWeek(new Date());
        var next = new Date(start.getFullYear(), start.getMonth(), start.getDate() + ((Number(delta) || 0) * 7), 12, 0, 0, 0);
        taskViewBuildWeek(next);
        $scope.loadTaskViewWeeklyData();
    };

    function taskViewBuildVisibleDateMap(gridStart) {
        var gridEnd = new Date(gridStart.getFullYear(), gridStart.getMonth(),
            gridStart.getDate() + 41, 12, 0, 0, 0);
        var dateMap = {};
        angular.forEach($scope.mytasklist || [], function(c) {
            // Calendar range is inclusive. A task dated 15-17 is available on
            // 15, 16 and 17. Missing end date is treated as a one-day task.
            var startParts = getDateOnlyParts(c.Startdate);
            var endParts = getDateOnlyParts(c.Targetdate);
            if (!isValidDateOnlyParts(startParts) && isValidDateOnlyParts(endParts)) startParts = endParts;
            if (!isValidDateOnlyParts(endParts) && isValidDateOnlyParts(startParts)) endParts = startParts;

            if (isValidDateOnlyParts(startParts) && isValidDateOnlyParts(endParts)) {
                var rangeStart = taskViewDateFromParts(startParts);
                var rangeEnd = taskViewDateFromParts(endParts);
                if (rangeEnd.getTime() < rangeStart.getTime()) {
                    var rangeSwap = rangeStart;
                    rangeStart = rangeEnd;
                    rangeEnd = rangeSwap;
                }

                if (rangeStart.getTime() < gridStart.getTime()) rangeStart = new Date(gridStart.getTime());
                if (rangeEnd.getTime() > gridEnd.getTime()) rangeEnd = new Date(gridEnd.getTime());

                for (var rangeDay = new Date(rangeStart.getTime());
                     rangeDay.getTime() <= rangeEnd.getTime();
                     rangeDay.setDate(rangeDay.getDate() + 1)) {
                    var rangeKey = taskViewDateKeyFromDate(rangeDay);
                    if (!dateMap[rangeKey]) dateMap[rangeKey] = [];
                    dateMap[rangeKey].push(c);
                }
            }
        });
        $scope.taskViewDateMap = dateMap;
    }

    function taskViewBuildCalendar(monthDate, preserveSelection) {
        if (!monthDate || isNaN(monthDate.getTime())) monthDate = new Date();
        monthDate = new Date(monthDate.getFullYear(), monthDate.getMonth(), 1, 12, 0, 0, 0);
        $scope.taskViewCalendarMonthDate = monthDate;
        $scope.taskViewCalendarTitle = monthDate.toLocaleString('en-US', { month: 'long', year: 'numeric' });

        var first = new Date(monthDate.getFullYear(), monthDate.getMonth(), 1, 12, 0, 0, 0);
        var gridStart = new Date(first.getFullYear(), first.getMonth(), first.getDate() - first.getDay(), 12, 0, 0, 0);
        taskViewBuildVisibleDateMap(gridStart);
        var days = [];
        var firstTaskKey = '';
        for (var i = 0; i < 42; i++) {
            var d = new Date(gridStart.getFullYear(), gridStart.getMonth(), gridStart.getDate() + i, 12, 0, 0, 0);
            var key = taskViewDateKeyFromDate(d);
            var tasks = taskViewCalendarFilterTasks($scope.taskViewDateMap[key] || []);
            var assignedMinutes = 0;
            var actualSeconds = 0;
            angular.forEach(tasks, function (t) {
                assignedMinutes += t._taskViewDurationMinutes || 0;
                actualSeconds += taskViewTimeSeconds(t.timerText_spent || t.timerText);
            });
            if (!firstTaskKey && tasks.length && d.getMonth() === monthDate.getMonth()) firstTaskKey = key;
            days.push({
                date: d,
                key: key,
                day: d.getDate(),
                inMonth: d.getMonth() === monthDate.getMonth(),
                count: tasks.length,
                assigned: taskViewFormatMinutes(assignedMinutes),
                actual: taskViewFormatSeconds(actualSeconds)
            });
        }
        $scope.taskViewCalendarDays = days;

        var selectedKey = preserveSelection ? $scope.taskViewSelectedCalendarKey : '';
        var validSelected = false;
        if (selectedKey) {
            for (var j = 0; j < days.length; j++) {
                if (days[j].key === selectedKey && days[j].inMonth) { validSelected = true; break; }
            }
        }
        // Default Calendar selection is today when the displayed month is the
        // current month. Otherwise retain the existing first-day fallback.
        var todayKey = taskViewDateKeyFromDate(new Date());
        var todayIsInDisplayedMonth = false;
        for (var t = 0; t < days.length; t++) {
            if (days[t].key === todayKey && days[t].inMonth) {
                todayIsInDisplayedMonth = true;
                break;
            }
        }
        if (!validSelected) {
            selectedKey = todayIsInDisplayedMonth
                ? todayKey
                : (firstTaskKey || taskViewDateKeyFromDate(first));
        }
        var selectedDay = null;
        for (var k = 0; k < days.length; k++) if (days[k].key === selectedKey) { selectedDay = days[k]; break; }
        if (selectedDay) $scope.selectTaskViewCalendarDay(selectedDay);
    }

    $scope.selectTaskViewCalendarDay = function (day) {
        if (!day) return;
        $scope.taskViewSelectedCalendarKey = day.key;
        $scope.taskViewSelectedCalendarLabel = day.date.toLocaleString('en-US', { day:'2-digit', month:'short', year:'numeric' });
        $scope.taskViewSelectedCalendarTasks = taskViewCalendarFilterTasks($scope.taskViewDateMap[day.key] || []);
    };

    $scope.taskTopStatusFilter = function (task) {
        var statusId = String($scope.taskViewCalendarStatusId || '');
        return !statusId || (task && String(task.P_Status || '') === statusId);
    };

    $scope.getTaskTopStatusFilteredTasks = function () {
        return ($scope.mytasklist || []).filter($scope.taskTopStatusFilter);
    };

    function taskViewCalendarFilterTasks(tasks) {
        return (tasks || []).filter($scope.taskTopStatusFilter);
    }

    // The top counters use one in-memory status filter in every view.
    // No server request, save method or task object is changed.
    $scope.filterTaskCalendarByStatus = function (statusId) {
        var clickedStatusId = statusId ? String(statusId) : '';
        $scope.taskViewCalendarStatusId =
            String($scope.taskViewCalendarStatusId || '') === clickedStatusId
                ? ''
                : clickedStatusId;
        $scope.currentPage = 1;
        if ($scope.taskViewCalendarMonthDate) {
            taskViewBuildCalendar($scope.taskViewCalendarMonthDate, true);
        }
    };

    $scope.changeTaskViewCalendarMonth = function (delta) {
        var base = $scope.taskViewCalendarMonthDate || new Date();
        var next = new Date(base.getFullYear(), base.getMonth() + delta, 1, 12, 0, 0, 0);
        taskViewBuildCalendar(next, false);
    };

    $scope.refreshTaskViewCaches = function (skipCalendar) {
        var statusMap = {};
        var priorityMap = {};
        var dateMap = {};
        var assignedTotalMinutes = 0;
        var actualTotalSeconds = 0;

        angular.forEach($scope.mytasklist || [], function (c) {
            c._taskViewStartKey = taskViewDateKey(c.Startdate);
            c._taskViewTargetKey = taskViewDateKey(c.Targetdate);
            c._taskViewDurationMinutes = taskViewDurationMinutes(c.TaskDuration);
            assignedTotalMinutes += c._taskViewDurationMinutes;
            actualTotalSeconds += taskViewTimeSeconds(c.timerText_spent || c.timerText);

            var skey = String(c.P_Status);
            if (!statusMap[skey]) statusMap[skey] = [];
            statusMap[skey].push(c);

            var pkey = String(c.Priority);
            if (!priorityMap[pkey]) priorityMap[pkey] = [];
            priorityMap[pkey].push(c);


        });

        $scope.taskViewStatusMap = statusMap;
        $scope.taskViewPriorityMap = priorityMap;
        $scope.taskViewDateMap = dateMap;
        $scope.taskViewAssignedTotal = taskViewFormatMinutes(assignedTotalMinutes);
        $scope.taskViewActualTotal = taskViewFormatSeconds(actualTotalSeconds);

        // Weekly day totals come from the day-wise timer history loaded by
        // GetWeeklyReportData. Do not replace them with TaskDuration totals here.
        if (!$scope.taskViewWeekTotals) $scope.taskViewWeekTotals = {};

        if (!skipCalendar && $scope.taskViewCalendarMonthDate) taskViewBuildCalendar($scope.taskViewCalendarMonthDate, true);
    };

    $scope.prepareTaskViews = function (fromDate, toDate) {
        var start = taskViewParseRangeDate(fromDate) || new Date();

        // Weekly View rule for the MAIN date filter:
        // - Default/no explicit filter -> current Monday-Sunday week
        // - Today / This Week / This Month -> current Monday-Sunday week
        // - Other presets / Custom Range -> week containing the filter From Date
        // This changes only Weekly View positioning; the existing main filter
        // From/To dates and task loading remain untouched.
        var weeklyBase = start;
        try {
            var mainPicker = $('#daterange').data('daterangepicker');
            var chosen = mainPicker && mainPicker.chosenLabel
                ? String(mainPicker.chosenLabel).trim().toLowerCase()
                : '';

            if (!chosen ||
                chosen === 'today' ||
                chosen === 'this week' ||
                chosen === 'this month') {
                weeklyBase = new Date();
            }
        } catch (e) {
            // If the picker is not ready yet, default Weekly View to current week.
            weeklyBase = new Date();
        }

        taskViewBuildWeek(taskViewStartOfWeek(weeklyBase));

        $scope.refreshTaskViewCaches(true);
        taskViewBuildCalendar(new Date(start.getFullYear(), start.getMonth(), 1, 12, 0, 0, 0), false);
        if ($scope.taskView === 'weekly') {
            $scope.loadTaskViewWeeklyData();
        }
    };

    function waitForTable(callback) {

        let retries = 0;

        let interval = setInterval(function () {

            let table = document.querySelector('#tbDetails');

            if (
                table &&
                table.querySelectorAll('tbody tr').length > 0
            ) {

                clearInterval(interval);

                callback();
            }

            retries++;

            // safety stop
            if (retries > 100) {
                clearInterval(interval);
            }

        }, 100);
    }
    $scope.tableUiInitialized = false;
    $scope.isselectAll = false;
$scope.clearselectedtag=function (c) {
$scope.selectedtagIds =[];
}
$scope.checkselect=function (c) {
$scope.selectedStatus =[];
////debugger
if(c.selected)
           {
        //var index = $scope.multiselectids.indexOf(c.id&&c.type);
        var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.tasktype;}); 
        if (index ==-1) { 
if (c._oldcomplete_value == null || c._oldcomplete_value == undefined) 
{
         c._oldcomplete_value=c.complete_value;
         c._oldpending_value=c.pending_value; 
}         
        $scope.multiselectids.push({id:c.id,type:c.tasktype});
        }
     
}
else
{
        //var index = $scope.multiselectids.indexOf(c.id); 
        var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.tasktype;}); 
        if (index >-1) {
        $scope.multiselectids.splice(index, 1);
                 
        }
}



    if($scope.multiselectids.length>0)
    {
    
    $scope.multiselect = "1";
    }
    else
    {
    $scope.multiselect = "0";
    $scope.multiselectids =[];
    }
};
    $scope.selectall = function (checked) {
////debugger
        //alert('tt');
$scope.selectedStatus =[];
        $scope.multiselect="0";
       var Mselectedids="";
        $scope.isselectAll = checked;
        angular.forEach($scope.mytasklist, function (c) {
//debugger
            c.selected = checked;
            if(checked)
            {                
                       // var index = $scope.multiselectids.indexOf(c.id); 
var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.type;}); 
        
                        if (index ==-1) {           
                        //$scope.multiselectids.push(c.id);
if (c._oldcomplete_value == null || c._oldcomplete_value == undefined) 
{
                        c._oldcomplete_value=c.complete_value;
                        c._oldpending_value=c.pending_value;
}
                        $scope.multiselectids.push({id:c.id,type:c.tasktype});
                        }
            }
            else
            {
                        var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.type;}); 
                    if (index >-1) {           
                        $scope.multiselectids.splice(index,1);
                        }
            }
        });
        if($scope.isselectAll==true)
        {
         if($scope.multiselectids.length>0)
         $scope.multiselect="1";
        else
        $scope.multiselect="0";
        }
        $scope.$applyAsync(); // VERY IMPORTANT
    };
   
    $scope.tableRendered = function () {

        // prevent multiple calls
        if ($scope.tableUiInitialized) {
            return;
        }

        $scope.tableUiInitialized = true;

        setTimeout(function () {

            // init
            initTableUI();

            $scope.$applyAsync(function () {
                $scope.isLoading = false;
            });

        }, 1000);

    };

    $scope.sortData = function (column) {
        $scope.reverseSort = ($scope.sortColumn == column) ? !$scope.reverseSort : false;
        $scope.sortColumn = column;
    };
    $scope.getSortClass = function (column) {
        if ($scope.sortColumn == column) {
            return $scope.reverseSort ? "↓" : "↑";
        }
        return "";
    };
    //#endregion
    $scope.bindtasklist2 = function (fromDate, toDate) {
        //debugger
        //alert(fromDate + "/" + toDate);
        //$scope.loadsubtasklist_count();
        $scope.loadsubtasklist();

    };
    $scope.viewsearch = function ()
    {
        //debugger
        if ($scope.freesearch=="0")
            $scope.freesearch = "1";
        else
            $scope.freesearch = "0";
    };
    $scope.getRemainingOwners = function (owners) {
        if (!owners || owners.length <= 3) return "";

        return owners.slice(3).map(function (o) {
            return o.name;
        }).join(", ");
    };
    $scope.getRemainingtags = function (tags) {
        if (!tags || tags.length<=1) return "";

        return tags.slice(1).map(function (o) {
            return o.tagname;
        }).join(", ");
    };
    $scope.removetags = function (c,tagid) {
        //debugger
            $scope.removeTagToServer(c,tagid);
           
        setTimeout(function () {
            initTableUI();

        }, 500);

    };
$scope.removetagsnew = function (tagid) {
//debugger
        //var index = $scope.selectedtagIds.indexOf(tagid);
       var index = $scope.selectedtagIds.findIndex(x => x.id == tagid);
        if (index > -1) {
            $scope.selectedtagIds.splice(index, 1);
            
                $scope.removetagToServernew($scope.currentlist, $scope.selectedtagIds, tagid);
           

        }
       $scope.filteredTagList(); 

    };
$scope.removetagsnew_multi = function (tagid) {
//debugger
        //var index = $scope.selectedtagIds.indexOf(tagid);
       var index = $scope.selectedtagIds.findIndex(x => x.id == tagid);
        if (index > -1) {
            $scope.selectedtagIds.splice(index, 1);           
        }
       $scope.filteredTagList(); 

    };
$scope.removetagToServernew = function (obj, selected_obj, removetagid) {
        debugger
        
        //var ids = selected_obj.join(",");
        var ids = selected_obj.map(function(x) {return x.id;}).join(",");
        let resultnew;

        
        resultnew = usertaskService.UpdateP_Tag(obj.id,obj.tasktype, ids);
        
        if (resultnew) {
            resultnew.then(function (res) {
                //debugger;
                if (res.data.d == "1") {
                    var tindex=$scope.mytasklist.findIndex(m => m.id == obj.id);
                        if (tindex !== -1) {
                           var tasks=$scope.mytasklist[tindex];
                            var indx = tasks.tags.findIndex(m => m.id == removetagid);
                            if (indx !== -1) {                        
                                tasks.tags.splice(indx, 1);
                            }

                        }
                    
                }
                //console.log(res);
            })
                .catch(function (err) {
//debugger
                    //console.error(err);
                    alert("Update failed");
                });
        }
    };

    $scope.removeTagToServer = function (c,tagid) {
        //debugger
       
        let resultnew;
var updateids="";
    angular.forEach(c.tags, function (t) 
    {
        if(t.id!=tagid)
        {
            if(updateids=="")
            {updateids=t.id;}
            else
            {updateids=","+t.id;}
        }
    });
        
        resultnew = usertaskService.removetags(c.id,c.tasktype,updateids);
       
        if (resultnew) {
            resultnew.then(function (res) {
                //debugger;
                if (res.data.d == "1") {                    
                    var tagindx = c.tags.findIndex(f => f.id === tagid);
                    if (tagindx > -1) {
                        c.tags.splice(tagindx, 1);

                    }
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    alert("Update failed");
                });
        }
    };
$scope.closenewtabgdiv = function () {
document.getElementById('dvnewtag').style.display = 'none';
var notes = document.getElementById('ContentPlaceHolder1_tags');
        if (!notes) { return; }
notes.value="";
}
$scope.stopTagEvent = function ($event) {
        if ($event) $event.stopPropagation();
    };

    $scope.toggleTag = function (tag) {
        //debugger
if(tag.id=="-1")
{
document.getElementById('dvnewtag').style.display = '';
$scope.closelist();
}
else
{
         document.getElementById('dvnewtag').style.display = 'none';
        //var index = $scope.selectedtagIds.indexOf(tag.id); 
        var index = $scope.selectedtagIds.findIndex(function (x) {return x.id == tag.id;});       
        if (index > -1) {           
        } else {

           $scope.selectedtagIds.push({
            id:tag.id,
            name:tag.tagname
            }); // select
           $scope.updateTagsToServer($scope.currentlist,$scope.selectedtagIds, tag.id);            
        }
        $scope.tagSearch = "";
        $scope.filterTagList();
}
        
    };
$scope.toggleTag_multi = function (tag) {
        //debugger
$scope.addnew_tag="0";
 if(tag.id=="-1")
{
document.getElementById('dvnewtag2').style.display = '';
$scope.closelist();
$scope.addnew_tag="1";
}
else
{
         document.getElementById('dvnewtag2').style.display = 'none';
        var index = $scope.selectedtagIds.findIndex(function (x) {return x.id == tag.id;});       
        if (index > -1) {           
        } else {
           $scope.selectedtagIds.push({
            id:tag.id,
            name:tag.tagname
            });                     
        }
        $scope.tagSearch = "";
        $scope.filterTagList();
}
        
    };
    $scope.updateTagsToServer = function (obj, selected_obj, newid) {
        //debugger
        if (!selected_obj || selected_obj.length === 0) {
            alert("No tag selected");
            return;
        }
        //var ids = selected_obj.join(",");
       var ids = selected_obj.map(function(x) {return x.id;}).join(",");
        let resultnew;

       
        resultnew = usertaskService.UpdateP_Tag(obj.id,obj.tasktype, ids);
        
        if (resultnew) {
            resultnew.then(function (res) {
                //debugger;
                if (res.data.d == "1") {
                    var addlist = $scope.mytasktaglist.find(m => m.id == newid);
                    if (addlist) {
                        let newrow = angular.copy($scope.tagsnew);
                        newrow.id = addlist.id;
                        newrow.tagname = addlist.tagname;                       
                        newrow.tagcolor = addlist.tagcolor;                       
                        obj.tags.push(newrow);
                    }
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }
    };


  $scope.filterTagList = function () {
//debugger
        var search = ($scope.tagSearch || "").toLowerCase().trim();
        $scope.filteredTagList = $scope.mytasktaglist.filter(function (item) {
            var alreadySelected = $scope.selectedtagIds.some(function (s) {
                return s.id == item.id;
            });
            var match = item.tagname.toLowerCase().indexOf(search) !== -1;
            return !alreadySelected && match;
        });

        //$scope.supplierHighlightIndex = $scope.filterTagList.length > 0 ? 0 : -1;
    };
$scope.showhistory = function (taskid,htype) {
//debugger
 usertaskService.gettaskhistory(taskid,htype).then(function (response) {

        //debugger;

        $scope.mytasklist_history = response.data.d;
if ($scope.mytasklist_history) {
//alert($scope.mytasklist_history.taskname);
                        angular.forEach($scope.mytasklist_history.history, function (s) {
                            

                            if (s.startdate) {
                                if (typeof s.startdate === 'string' && s.startdate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.startdate.substr(6));
                                    s.startdate = new Date(timestamp);
                                } else {
                                    s.startdate = new Date(s.startdate);
                                }
                            }
if (s.enddate) {
                                if (typeof s.enddate === 'string' && s.enddate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.enddate.substr(6));
                                    s.enddate = new Date(timestamp);
                                } else {
                                    s.enddate = new Date(s.enddate);
                                }
                            }
});
}
        

    }).catch(function (error) {

        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        console.error("HTTP ERROR:", error);

    }).finally(function () {

     
    

    });
}
    $scope.getProjectHeadAvatarImage = function (ownerid) {
        var owner = ($scope.owners || []).find(function (o) { return o.id == ownerid; });
        return owner && owner.profileimage ? owner.profileimage : '';
    };
    $scope.getProjectHeadInitials = function (ownerid) {
        var name = $scope.getOwnerName(ownerid);
        var parts = String(name || '').trim().split(/\s+/);
        if (!name) return '?';
        return (parts[0].charAt(0) + (parts.length > 1 ? parts[parts.length - 1].charAt(0) : '')).toUpperCase();
    };
    $scope.getOwnerName = function (ownerid) {
    //debugger
        var owner = $scope.owners.find(function (o) {
            return o.id == ownerid;
        });

        return owner ? owner.name : '';
    };
$scope.getOwnerProfile = function (ownerid) {

    var defaultImage = '/pms/Image/user.jpg';
    try {
        if (!ownerid || !$scope.owners || !$scope.owners.length) {
            return defaultImage;
        }

        var owner = $scope.owners.find(function (o) {
            return o.id == ownerid;
        });

        return (owner && owner.profileimage && owner.profileimage != '')
            ? owner.profileimage
            : defaultImage;
    }
    catch{
        return defaultImage;
    }
};
$scope.openTagDropdown = function ($event) {
//debugger
    $event.stopPropagation();
    $scope.showTagDropdown = true;
$scope.tagSearch = "";
        $scope.filterTagList();
};
$scope.closetagwindow = function() {
$scope.selectedtagIds=[];
 document.getElementById('dvnewtag').style.display = 'none';
 document.getElementById('ContentPlaceHolder1_tags').value = '';
 document.getElementById('tagscolor').value = '';
 document.getElementById('dvnewtag2').style.display = 'none';
 document.getElementById('ContentPlaceHolder1_tags2').value = '';
 document.getElementById('tagscolor2').value = '';
}
 $scope.closelist = function() {
//debugger

if($scope.showTagDropdown==true)
    $scope.showTagDropdown = false;
};

//$scope.getSelectedTags = function () {
       // return $scope.mytasktaglist.filter(o => $scope.selectedtagIds.includes(o.id));
   // };
$scope.getSelectedTags = function () {

    return $scope.mytasktaglist.filter(t =>
        $scope.selectedtagIds.some(s => s.id == t.id)
    );

};

$scope.loadtaglist = function () {

    //debugger;
    //$scope.isLoading = true;

    usertaskService.gettaglist()
        .then(function (response) {
            //debugger;
            // assign data
            $scope.mytasktaglist = response.data.d;

        })
        .catch(function (error) {

            //debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);

        })
        .finally(function () {

            //$scope.isLoading = false;

        });
};
$scope.loaduserlist = function () {

        //debugger;
        //$scope.isLoading = true;

        usertaskService.getuserlist()
            .then(function (response) {

                //debugger;
                // assign data
                $scope.owners = response.data.d;

            })
            .catch(function (error) {

                //debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
$scope.loadstatuslist = function () {

    //debugger;
    //$scope.isLoading = true;

    usertaskService.getstatuslist()
        .then(function (response) {

            //debugger;
            // assign data
            $scope.statuslists = response.data.d;

        })
        .catch(function (error) {

            //debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);

        })
        .finally(function () {

            //$scope.isLoading = false;

        });
};
$scope.selectStatus_multi = function (item) {

        //debugger;
        
        var pid = 0;
        let resultnew;
       if($scope.multiselectids.length==0)
       {return}
       if(item.id=="")
       {return}
            //var ids = $scope.multiselectids.join(",");
            var ids = $scope.multiselectids.map(function(x) {return x.id + ":" + x.type;}).join(",");
            resultnew = usertaskService.UpdateSubTask_StatusMulti(ids,item.id,item.iscomplete);        

        if (resultnew) {
            resultnew.then(function (res) {
                //debugger;
                if (res.data.d != "1") {
                    alert(res.data.d);
                }
                else {
$scope.selectedStatus.push({name:item.name,colorname:item.colorname});
                 angular.forEach($scope.multiselectids, function (s) {
                    let sindex = $scope.mytasklist.findIndex(function (m) {return m.id == s.id&&m.tasktype==s.type;});
                     var task = $scope.mytasklist[sindex];                   
                    task.P_Status=item.id;
                    task.status_color = item.colorname;
                    task.status_name = item.name; 
                    if(item.iscomplete=="1")
                    {
                     task.complete_value ="100";
                     task.pending_value ="0";
                    }
                    else
                    {
                    task.complete_value =task._oldcomplete_value;
                    task.pending_value =task._oldpending_value;
                    }

});

        angular.forEach($scope.mytasklist, function (c) {
//debugger
            c.selected = false;
});

// Status is already saved successfully. Rebuild the existing status/count/Kanban
// caches from the same in-memory mytasklist so the card moves immediately to
// its new status column without reloading the page or changing the save flow.
$scope.filterstatuslist_count();
$scope.$applyAsync();

try
{
$scope.multiselect = "0";
// Clear the actual multi-selection memory after a successful status update.
// The checkboxes were already being visually cleared above, but without this
// reset the previous task ids remained in multiselectids and were sent again
// on the next status change.
$scope.multiselectids = [];
$scope.isselectAll = false;
var checkall=document.getElementById('chkAll');
if(checkall)
checkall.checked=false;
}
catch{}
                    
                   
                }
                //alert(res.d);
                //console.log(res);
            })
                .catch(function (err) {
//debugger
                    console.error(err);
                    //alert("Update failed");
                });
        }
    };
$scope.selectStatus = function (c,item) {

        //debugger;
        
        var pid = 0;
        let resultnew;
        
            pid = c.id;
 if (c.tasktype == 'milestone') {        
        resultnew = usertaskService.UpdateMilestone_Status(c.id, item.id, item.iscomplete);
    }
    else if (c.tasktype == 'task') {       
        resultnew = usertaskService.UpdateTask_Status(c.id, item.id, item.iscomplete);
    }
    else if (c.tasktype == 'subtask') {       
        resultnew = usertaskService.UpdateSubTask_Status(c.id, item.id, item.iscomplete);
    }
           // resultnew = usertaskService.UpdateSubTask_Status(c.id,c.tasktype, item.id,item.iscomplete);        

        if (resultnew) {
            resultnew.then(function (res) {
                //debugger;
                if (res.data.d != "1") {
                    alert(res.data.d);
           
                }
                else {

                    c.P_Status = item.id;
                    c.status_color = item.colorname;
                    c.status_name = item.name;                    
                    if(item.iscomplete=="1")
                    {
                      c.complete_value ="100";
                      c.pending_value ="0";
                    }
                    else
                    {
                    c.complete_value =c._oldcomplete_value;
                    c.pending_value =c._oldpending_value;
                    }
                   $scope.filterstatuslist_count();
                   $scope.$applyAsync();
                }
                //alert(res.d);
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }
    };

$scope.loadprioritylist = function () {

    //debugger;
    //$scope.isLoading = true;

    usertaskService.getprioritylist()
        .then(function (response) {
            //debugger;
            // assign data
            $scope.prioritylists = response.data.d;

        })
        .catch(function (error) {
            //debugger;
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);

        })
        .finally(function () {

            //$scope.isLoading = false;

        });
    };
$scope.getContrastColor = function (hex) {

    if (!hex) return '#000';

    hex = hex.replace('#', '');

    if (hex.length === 3) {
        hex = hex[0]+hex[0]+hex[1]+hex[1]+hex[2]+hex[2];
    }

    var r = parseInt(hex.substr(0,2),16);
    var g = parseInt(hex.substr(2,2),16);
    var b = parseInt(hex.substr(4,2),16);

    var brightness = ((r * 299) + (g * 587) + (b * 114)) / 1000;

    return brightness > 155 ? '#000' : '#fff';
};
$scope.loadsubtasklist_count = function () {
////debugger
   // alert("ok");
    $scope.isLoading = true;
    $scope.isLoading = true;
    var fdate = moment()
        .startOf('month')
        .format('MM/DD/YYYY');

    var tdate = moment()
        .endOf('month')
        .format('MM/DD/YYYY');

    // GET PICKER IF AVAILABLE
    var picker = $('#daterange').data('daterangepicker');

    if (picker) {

        fdate = picker.startDate.format('MM/DD/YYYY');

        tdate = picker.endDate.format('MM/DD/YYYY');

    }
    if (!picker && hasLinkedDateRange) {
        fdate = linkedFromDate.format('MM/DD/YYYY');
        tdate = linkedToDate.format('MM/DD/YYYY');
    }
    usertaskService.getsubtaskslist_count(fdate, tdate).then(function (response) {

        //debugger;

        $scope.mytasklist_status = response.data.d;
        

    }).catch(function (error) {

        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        console.error("HTTP ERROR:", error);

    }).finally(function () {

       
    $scope.isLoading = false;
        

    });
      
};

    $scope.loadsubtasklist = function () {

    //alert("ok");
        $scope.isLoading = true;

        var fdate = moment()
            .startOf('month')
            .format('MM/DD/YYYY');

        var tdate = moment()
            .endOf('month')
            .format('MM/DD/YYYY');

        // GET PICKER IF AVAILABLE
        var picker = $('#daterange').data('daterangepicker');

        if (picker) {

            fdate = picker.startDate.format('MM/DD/YYYY');

            tdate = picker.endDate.format('MM/DD/YYYY');

        }
       
   
        if (!picker && hasLinkedDateRange) {
            fdate = linkedFromDate.format('MM/DD/YYYY');
            tdate = linkedToDate.format('MM/DD/YYYY');
        }
        usertaskService.getsubtaskslist(fdate, tdate, $scope.taskListType).then(function (response) {

        $scope.mytasklist = response.data.d || [];
        if (highlightedTaskId) {
            var highlightedRows = $filter('orderBy')(
                $filter('filter')($filter('filter')($scope.mytasklist, $scope.taskTopStatusFilter), $scope.searchText),
                $scope.sortColumn, $scope.reverseSort);
            var highlightedIndex = highlightedRows.findIndex($scope.isQueryTaskHighlighted);
            if (highlightedIndex >= 0) {
                $scope.currentPage = Math.floor(highlightedIndex / $scope.pageSize) + 1;
            }
        }
        $scope.prepareTaskViews(fdate, tdate);
        $scope.isload = "1";
if ($scope.mytasklist && $scope.mytasklist.length > 0) {
$scope.filterstatuslist_count();
                        angular.forEach($scope.mytasklist, function (s) {
                            

                            if (s.Startdate) {
                                if (typeof s.Startdate === 'string' && s.Startdate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.Startdate.substr(6));
                                    s.Startdate = new Date(timestamp);
                                } else {
                                    s.Startdate = new Date(s.Startdate);
                                }
                            }
if (s.Targetdate) {
                                if (typeof s.Targetdate === 'string' && s.Targetdate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.Targetdate.substr(6));
                                    s.Targetdate = new Date(timestamp);
                                } else {
                                    s.Targetdate = new Date(s.Targetdate);
                                }
                            }
                            
                        });
    
    
    setTimeout(function () {

            // init

            initTableUI();


            $scope.$applyAsync(function () {               
            });

        }, 0);
    
                    }
        $scope.isLoading = false;
           $scope.autostartTimers(); 
    }).catch(function (error) {

        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        console.error("HTTP ERROR:", error);

            })
    .finally(function () {
        $scope.isLoading = false;
    });
};
$scope.activeTimerId = 0;
$scope.currentTimer = null;
function syncClientTimer(c) {
    if (!c) return;

    c.elapsedMilliseconds = Number(c.elapsedMilliseconds || 0);

    if (c.timerStartedAt) {
        c.elapsedMilliseconds = Number(c.timerBaseMilliseconds || 0) +
            (Date.now() - c.timerStartedAt);
    }

    c.timerText = formatTimer(c.elapsedMilliseconds);
}

function startClientTimer(c) {
    if (!c) return;

    if (c.currentTimer) {
        clearInterval(c.currentTimer);
        c.currentTimer = null;
    }

    c.elapsedMilliseconds = Number(c.elapsedMilliseconds || 0);
    c.timerBaseMilliseconds = c.elapsedMilliseconds;
    c.timerStartedAt = Date.now();
    c.timerText = formatTimer(c.elapsedMilliseconds);

    c.currentTimer = setInterval(function () {
        syncClientTimer(c);
        $scope.$applyAsync();
    }, 250);
}

function freezeClientTimer(c) {
    if (!c) return;

    syncClientTimer(c);

    if (c.currentTimer) {
        clearInterval(c.currentTimer);
        c.currentTimer = null;
    }

    c.timerBaseMilliseconds = Number(c.elapsedMilliseconds || 0);
    c.timerStartedAt = null;
}

function refreshRunningTimersAfterBrowserReturn() {
    angular.forEach($scope.mytasklist || [], function (c) {
        if (Number(c.isend) === 0 &&
            Number(c.isrun) === 1 &&
            Number(c.ispause) === 0 &&
            c.timerStartedAt) {
            syncClientTimer(c);
        }
    });

    $scope.$applyAsync();
}

function onTimerVisibilityChange() {
    if (!document.hidden) {
        refreshRunningTimersAfterBrowserReturn();
    }
}

function onTimerWindowFocus() {
    refreshRunningTimersAfterBrowserReturn();
}

document.addEventListener('visibilitychange', onTimerVisibilityChange);
window.addEventListener('focus', onTimerWindowFocus);
window.addEventListener('pageshow', onTimerWindowFocus);

$scope.$on('$destroy', function () {
    document.removeEventListener('visibilitychange', onTimerVisibilityChange);
    window.removeEventListener('focus', onTimerWindowFocus);
    window.removeEventListener('pageshow', onTimerWindowFocus);
});

$scope.autostartTimers = function () {

    angular.forEach($scope.mytasklist, function (c) {

        // server side value
        c.elapsedMilliseconds =
            c.elapsedMilliseconds || 0;

        // format existing timer
        c.timerText =
            formatTimer(c.elapsedMilliseconds);

        // auto start only running timers
        if (c.isend == 0&&c.isrun == 1 && c.ispause == 0) {

            startClientTimer(c);
        }
    });
};


// ---------------------------------------------------------
// Timer actions: keep existing server save methods.
// Only UI flow changed: pause/stop save directly with blank remark,
// and Start checks for another currently-running timer first.
// ---------------------------------------------------------
function timerFindListItem(taskType, entityId) {
    return ($scope.mytasklist || []).find(function (x) {
        return String(x.tasktype || '').toLowerCase() === String(taskType || '').toLowerCase() &&
               String(x.id) === String(entityId);
    });
}

function timerApplyStoppedUi(taskType, entityId) {
    var s = timerFindListItem(taskType, entityId);
    if (!s) return;
    if (s.currentTimer) {
        clearInterval(s.currentTimer);
        s.currentTimer = null;
    }
    s.isend = 1;
    s.isrun = 0;
    s.ispause = 0;
    s.elapsedMilliseconds = 0;
    s.timerText = '00:00:00';
    usertaskService.Getspenttime_subtask(s.id, s.tasktype).then(function (res) {
        if (res && res.data) s.timerText_spent = res.data.d;
    });
}

function timerStopDirect(c, done) {
    if (!c) { if (done) done(false); return; }
    freezeClientTimer(c);
    usertaskService.SaveTimer(
        c.tasktype,
        c.ProjectId,
        c.Milestoneid,
        c.Task,
        c.id,
        '',
        c.timerText || ''
    ).then(function (res) {
        var ok = !!(res && res.data && String(res.data.d) === '1');
        if (ok) {
            timerApplyStoppedUi(c.tasktype, c.id);
            $scope.$applyAsync();
        } else if (c.isrun && !c.ispause) {
            startClientTimer(c);
        }
        if (done) done(ok);
    }).catch(function (err) {
        console.error('Timer stop failed', err);
        if (c.isrun && !c.ispause) startClientTimer(c);
        if (done) done(false);
    });
}

function timerStartExisting(c) {
    return usertaskService.SaveTimerstartdetails(c.tasktype, c.ProjectId, c.Milestoneid, c.Task, c.id)
    .then(function (res) {
        if (res && res.data && res.data.d != '0') {
            c.isrun = 1;
            c.ispause = 0;
            c.isend = 0;
            c.elapsedMilliseconds = c.elapsedMilliseconds || 0;
            startClientTimer(c);
        } else { throw new Error('Timer start failed'); }
    })
    .catch(function (err) {
        console.error('Timer start failed', err);
        throw err;
    });
}

function timerShowSwitchConfirm(running, selected, proceed, finish) {
    var serial = running.serialNo ? String(running.serialNo).trim() : '';
    var name = running.taskName || '';
    var displayName = (serial ? serial + '- ' : '') + name;
    var typeName = running.typeName || 'Task';
    var html = '<div style="text-align:left;line-height:1.8;font-size:14px;">' +
               '<div><b>Task Name:</b> ' + timerEscapeHtml(displayName) + '</div>' +
               '<div><b>Type:</b> ' + timerEscapeHtml(typeName) + '</div>' +
               '<div style="margin-top:8px;">Continue will stop all your running timers before starting the selected task.</div>' +
               '</div>';

    if (window.Swal && Swal.fire) {
        Swal.fire({
            title: 'Timer Already Running',
            html: html,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Continue',
            cancelButtonText: 'Cancel',
            reverseButtons: true,
            allowOutsideClick: false,
            allowEscapeKey: true,
            heightAuto: false,
            scrollbarPadding: false
        }).then(function (x) {
            if (!x.isConfirmed) { finish(); return; }
            var oldTimer = {
                tasktype: running.taskType,
                ProjectId: running.projectId,
                Milestoneid: running.milestoneId,
                Task: running.taskId,
                id: running.entityId,
                timerText: (timerFindListItem(running.taskType, running.entityId) || {}).timerText || ''
            };
            timerStopDirect(oldTimer, function (ok) {
                if (ok) proceed();
                else { timerGuardError('Could not stop the running timer. Please try again.'); finish(); }
            });
        });
    } else {
        var msg = 'Task Name: ' + displayName + '\nType: ' + typeName +
                  '\n\nContinue will stop all your running timers before starting the selected task.';
        if (window.confirm(msg)) {
            var oldTimer2 = {
                tasktype: running.taskType,
                ProjectId: running.projectId,
                Milestoneid: running.milestoneId,
                Task: running.taskId,
                id: running.entityId,
                timerText: (timerFindListItem(running.taskType, running.entityId) || {}).timerText || ''
            };
            timerStopDirect(oldTimer2, function (ok) {
                if (ok) proceed();
                else { timerGuardError('Could not stop the running timer. Please try again.'); finish(); }
            });
        } else { finish(); }
    }
}

function timerEscapeHtml(value) {
    return String(value == null ? '' : value)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

// Match the current status ID against the status master completion flag.
$scope.isTaskTimerComplete = function (c) {
    if (!c) return false;
    return ($scope.statuslists || []).some(function (status) {
        return String(status.id) === String(c.P_Status) &&
               String(status.iscomplete) === '1';
    });
};

var timerSwitchBusy = false;
function timerGuardError(message) {
    if (window.Swal && Swal.fire) Swal.fire({icon:'error', title:'Timer', text:message});
    else window.alert(message);
}
function timerGuardedAction(c, action) {
    if (!c || timerSwitchBusy) return;
    if ($scope.isTaskTimerComplete(c)) {
        timerGuardError('Cannot start or resume a timer for a completed task.');
        return;
    }
    timerSwitchBusy = true;
    var confirmed = false;
    var stopped = {};
    function finish() { timerSwitchBusy = false; }
    function check() {
        if ($scope.isTaskTimerComplete(c)) {
            timerGuardError('Cannot start or resume a timer for a completed task.');
            finish();
            return;
        }
        usertaskService.GetRunningTimerBeforeStart(c.tasktype, c.id).then(function(res) {
            var running = res && res.data ? res.data.d : null;
            if (!running || typeof running.hasRunning !== 'boolean') {
                throw new Error('Invalid running timer response');
            }
            if ($scope.isTaskTimerComplete(c)) {
                timerGuardError('Cannot start or resume a timer for a completed task.');
                finish();
                return;
            }
            if (!running.hasRunning) {
                return action().then(finish, function(err) {
                    console.error('Timer action failed', err);
                    timerGuardError('Could not start the timer. Please try again.');
                    finish();
                });
            }
            function stopNext() {
                var key = String(running.transactionId);
                if (stopped[key]) {
                    timerGuardError('The previous timer is still running. Please refresh and try again.');
                    finish();
                    return;
                }
                stopped[key] = true;
                timerStopDirect({
                    tasktype:running.taskType, ProjectId:running.projectId,
                    Milestoneid:running.milestoneId, Task:running.taskId,
                    id:running.entityId
                }, function(ok) {
                    if (ok) check();
                    else { timerGuardError('Could not stop the running timer. Please try again.'); finish(); }
                });
            }
            if (confirmed) stopNext();
            else timerShowSwitchConfirm(running, c, function() {
                confirmed = true;
                stopped[String(running.transactionId)] = true;
                check();
            }, finish);
        }).catch(function(err) {
            console.error('Running timer check failed', err);
            timerGuardError('Unable to check your running timers. Please try again.');
            finish();
        });
    }
    check();
}
$scope.timerstart = function(c) {
    if (!c || (c.isrun && !c.ispause)) return;
    timerGuardedAction(c, function() { return timerStartExisting(c); });
};

$scope.timerpause = function(c) {
    if (!c || !c.isrun || c.ispause) return;
    freezeClientTimer(c);
    usertaskService.SaveTimerpausedetails(c.tasktype, c.ProjectId, c.Milestoneid, c.Task, c.id, '')
    .then(function (res) {
        if (res && res.data && String(res.data.d) === '1') {
            c.isrun = 1;
            c.ispause = 1;
            c.isend = 0;
            $scope.$applyAsync();
        } else {
            c.ispause = 0;
            startClientTimer(c);
        }
    })
    .catch(function (err) {
        console.error('Timer pause failed', err);
        c.ispause = 0;
        startClientTimer(c);
    });
};

$scope.closepause = function() {
    if (!$scope.currentlist) return;
    var sid = $scope.currentlist.id;
    var s = $scope.mytasklist.find(function (f) { return f.id == sid; });
    if (!s) return;
    s.isrun = 1;
    s.ispause = 0;
    startClientTimer(s);
};

$scope.timerresume = function(c) {
    if (!c || !c.ispause) return;
    timerGuardedAction(c, function() {
        return usertaskService.SaveTimerresumedetails(c.tasktype,c.ProjectId,c.Milestoneid,c.Task,c.id)
        .then(function(res) {
            if (res && res.data && res.data.d != '0') {
                c.ispause = 0;
                startClientTimer(c);
            } else { throw new Error('Timer resume failed'); }
        });
    });
};

$scope.timersstop = function(c) {
    // Direct stop, blank remark; existing SaveTimer keeps pause/stop history and spent time.
    timerStopDirect(c);
};

$scope.closestop = function () {
//debugger
    if (!$scope.currentlist)
        return;
    var sid = $scope.currentlist.id;
    var s = $scope.mytasklist.find(function (f) {
        return f.id == sid;

    });

    if (!s)
        return;

    s.isrun = 1;
  if(s.ispause==0)
    {
    startClientTimer(s);
  }

};


function formatTimer(totalMilliseconds){

    var totalSeconds = Math.floor(totalMilliseconds / 1000);

    var hrs = Math.floor(totalSeconds / 3600);

    var mins = Math.floor((totalSeconds % 3600) / 60);

    var secs = totalSeconds % 60;

    var ms = totalMilliseconds % 1000;

    return pad(hrs) + ':' +
           pad(mins) + ':' +
           pad(secs) + ':' +
           padMs(ms);
}

function pad(n){

    return n < 10 ? '0' + n : n;
}

function padMs(n){

    if(n < 10) return '00' + n;

    if(n < 100) return '0' + n;

    return n;
}

$scope.SavePauseDetials=function()
{
//alert("ok");
//debugger
    if(!$scope.currentlist)
    {
    return;
    }
var notes=document.getElementById('ContentPlaceHolder1_Notes');
if(!notes){return;}
if(notes.value==""){
alert("Please enter note");
        return;
    }
    document.getElementById('divpause').style.display = 'none';
    document.getElementById('pnlimgpause').style.display = 'block';
                var ProjectId = $scope.currentlist.ProjectId;
                var Milestoneid = $scope.currentlist.Milestoneid;
                var taskid = $scope.currentlist.Task;
                var subtaskid =$scope.currentlist.id;
                var utype =$scope.currentlist.tasktype;

usertaskService.SaveTimerpausedetails(utype,ProjectId,Milestoneid,taskid,subtaskid,notes.value)
.then(function(res) {
    if (!res) {
        alert("Failed to calculate end date");
        return;
    } 
    if(res.data.d=="1")
    {
//debugger
      notes.value="";
     //alert("save successfully");
     document.getElementById('btnclsNotes2').click();
    } 
    document.getElementById('pnlimgpause').style.display = 'none';
    document.getElementById('divpause').style.display = 'block';
    //let end = normalize(parseDotNetDate(newEndDate.data.d));
     //newRow.EndDate=end;

})
.catch(function(err) {

    console.error("Update failed", err);
    alert("Update failed");
    document.getElementById('pnlimgpause').style.display = 'none';
    document.getElementById('divpause').style.display = 'block';
});

}
    $scope.SaveStopDetials=function()
{
//alert("ok");
//debugger
    if(!$scope.currentlist)
    {
    return;
    }
    var notes=document.getElementById('ContentPlaceHolder1_TimerNotes');
    if(!notes){return;}
    if(notes.value==""){
    alert("Please enter note");
        return;
    }
    document.getElementById('divstop').style.display = 'none';
    document.getElementById('pnlimgstop').style.display = 'block';
                var ProjectId = $scope.currentlist.ProjectId;
                var Milestoneid = $scope.currentlist.Milestoneid;
                var taskid = $scope.currentlist.Task;
                var subtaskid =$scope.currentlist.id;
                var timingdetails =$scope.currentlist.timerText;
                var utype =$scope.currentlist.tasktype;

usertaskService.SaveTimer(utype,ProjectId,Milestoneid,taskid,subtaskid,notes.value,timingdetails)
.then(function(res) {
    if (!res) {
        alert("Failed to calculate end date");
        return;
    } 
    if(res.data.d=="1")
    {
//debugger
      //alert("save successfully");
      notes.value=""
      document.getElementById('btnclsTimer2').click();
     let sindex = $scope.mytasklist.findIndex(m => m.id == subtaskid&&m.tasktype ==utype);
     var s=$scope.mytasklist[sindex];
         if (s.currentTimer) {

                clearInterval(s.currentTimer);

                s.currentTimer = null;
            }
            // reset flags
            s.isend = 1;
            s.isrun = 0;
            s.ispause = 0;
            s.elapsedMilliseconds = 0;        
            
            s.timerText = "00:00:00";
            usertaskService.Getspenttime_subtask(s.id,s.tasktype)
            .then(function (res) {
            //debugger
                s.timerText_spent = res.data.d;
            })
            .catch(function (err) {
//debugger
            });

            $scope.$applyAsync();
    }  
    document.getElementById('pnlimgstop').style.display = 'none';
    document.getElementById('divstop').style.display = 'block';
    

})
.catch(function(err) {
//debugger
    console.error("Update failed", err);
    alert("Update failed");
    document.getElementById('pnlimgstop').style.display = 'none';
    document.getElementById('divstop').style.display = 'block';
});

    }
    $scope.Addtags = function (c,addtype)
    {
        //debugger;
        $scope.currentlist = c;
        $scope.tagtype = addtype;
        $scope.selectedtagIds = [];
         if(c.tags)
        {
               // $scope.selectedtagIds = c.tags.map(t => t.id);
         $scope.selectedtagIds = c.tags.map(function (x) {
                            return {
                                id: x.id,
                                name: x.tagname
                            };
                        });
        }
$scope.filteredTagList();
        //$scope.SvaeadTags(addtype);
    }
    $scope.SvaeadTags = function ()
    {
        //debugger;
        //alert($scope.tagtype);
        if (!$scope.currentlist){
            return;
        }
        var c = $scope.currentlist;
        var tagtype = $scope.tagtype;
        var notes = document.getElementById('ContentPlaceHolder1_tags');
        if (!notes) { return; }
        if (notes.value == "") {
            alert("Please enter tagname");
            notes.focus();
            return;
        }
        var notescolor = document.getElementById('tagscolor');
        if (!notescolor) { return; }
        if (notescolor.value == "") {
            alert("Please enter tag color");
            notescolor.focus();
            return;
        }
        usertaskService.SaveTags(c, notes.value, notescolor.value, tagtype)
            .then(function (res) {
                if (!res) {
                    alert("Failed");
                    return;
                }
                if (res.data.d!= "0"&& res.data.d!= "") {
                    //debugger
                    //alert("save successfully");
                    document.getElementById('dvnewtag').style.display = 'none';
                    
                    let sindex = $scope.mytasklist.findIndex(m => m.id == c.id);
                    var s = $scope.mytasklist[sindex];
                    if (!s.tags) {
                        s.tags = [];
                    }
                var addNewIndex = $scope.mytasktaglist.findIndex(function (x) {
                    return x.id == -1;
                });
                var newTag = {
                    id: res.data.d,
                    tagname: notes.value,
                    tagcolor: notescolor.value
                };

                if (addNewIndex > -1) {
                    $scope.mytasktaglist.splice(addNewIndex, 0, newTag);
                } else {
                    $scope.mytasktaglist.push(newTag);
                }
 
                    s.tags.push({
                        id: res.data.d,
                        tagname: notes.value,
                        tagcolor: notescolor.value
                    }); 
                     $scope.selectedtagIds.push({
                    id:res.data.d,
                    name:notes.value
                    });
                    notes.value = ""
                    notescolor.value = ""
                    $scope.getSelectedTags();
                    //document.getElementById('btnclstags').click();
                    $scope.$applyAsync();
                }
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';


            })
            .catch(function (err) {

                //console.error("Update failed", err);
                alert("Update failed");
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';
            });
$scope.filteredTagList();
    }
$scope.SvaeadTags_Multi = function ()
    {
//alert('ok');
        //debugger;
        //alert($scope.tagtype);
        if ($scope.multiselectids.length==0){
            return;
        }
       
var new_tagname="";     
var new_tagcolor="";     
if($scope.addnew_tag=="1")
{  
        var notes = document.getElementById('ContentPlaceHolder1_tags2');
        if (!notes) { return; }
        if (notes.value == "") {
            alert("Please enter tagname");
            notes.focus();
            return;
        }

        var notescolor = document.getElementById('tagscolor2');
        if (!notescolor) { return; }
        if (notescolor.value == "") {
            alert("Please enter tag color");
            notescolor.focus();
            return;
        }
          usertaskService.SaveTagsNew(notes.value, notescolor.value)
            .then(function (res) {
                //if (!res) {
                    //alert("Failed");
                    //return;
                //}
                if (res.data.d!= "0"&& res.data.d!= "") {
                    //debugger                    
                var addNewIndex = $scope.mytasktaglist.findIndex(function (x) {
                    return x.id == -1;
                });
                var newTag = {
                    id: res.data.d,
                    tagname: notes.value,
                    tagcolor: notescolor.value
                };

                if (addNewIndex > -1) {
                    $scope.mytasktaglist.splice(addNewIndex, 0, newTag);
                } else {
                    $scope.mytasktaglist.push(newTag);
                } 
                     $scope.selectedtagIds.push({
                    id:res.data.d,
                    name:notes.value
                    });
                    notes.value = ""
                    notescolor.value = ""
                    $scope.$applyAsync();
                }
               
            })
            .catch(function (err) {

            });

  }
 if ($scope.selectedtagIds.length==0){
            return;
        } 
        var ids = $scope.multiselectids.map(function(x) {return x.id + ":" + x.type;}).join(",");
        var tagids = $scope.selectedtagIds.map(function(x) {return x.id;}).join(",");
        usertaskService.SaveTags_update_Multi(ids,tagids)
            .then(function (res) {
//debugger
                if (!res) {
                    alert("Failed");
                    return;
                }
                if (res.data.d!= "0"&& res.data.d!= "") {
                    //debugger
                    //alert("save successfully");
angular.forEach($scope.multiselectids, function (t) {

    if ($scope.selectedtagIds.length > 0) {

        var tindex = $scope.mytasklist.findIndex(function (x) {
            return x.id == t.id && x.tasktype == t.type;
        });

        if (tindex > -1) {

            var task = $scope.mytasklist[tindex];

            var selectedTagListnew = $scope.mytasktaglist.filter(function (tag) {
                return $scope.selectedtagIds.some(function (sel) {
                    return sel.id == tag.id;
                });
            });

            // clear existing tags
            task.tags.length = 0;

            // push one by one
            angular.forEach(selectedTagListnew, function (tag) {
                task.tags.push(angular.copy(tag));
            });

            console.log(task.tags);
        }
    }
});

try
{
angular.forEach($scope.mytasklist, function (c) {
//debugger
            c.selected = false;
});
$scope.multiselect = "0";
var checkall=document.getElementById('chkAll');
if(checkall)
checkall.checked=false;
}
catch{}
                    document.getElementById('dvnewtag2').style.display = 'none';
                    document.getElementById('btnclstags2').click();
                    $scope.$applyAsync();
                }
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';


            })
            .catch(function (err) {
                //console.error("Update failed", err);
                alert("Update failed");
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';
            });

    }

$scope.tagSearch = "";

//$scope.loadsubtasklist_count();
//$scope.loadsubtasklist();
$q.all([
$scope.loadstatuslist(),
$scope.loadprioritylist(),
$scope.loaduserlist(),
$scope.loadtaglist()

]).then(function () {
$scope.filteredTagList = angular.copy($scope.mytasktaglist);
    $scope.loadsubtasklist();
});

$scope.filterstatuslist_count = function () {
//debugger
    if ($scope.statuslists && $scope.statuslists.length > 0) {

        $scope.mytasklist_bystatus = [];

        angular.forEach($scope.statuslists, function (status) {

            var total = $scope.mytasklist.filter(function (t) {
                return t.P_Status == status.id;
            }).length;

            if (total > 0) {
                $scope.mytasklist_bystatus.push({
                    statusid: status.id,
                    statusname: status.name,
                    statuscolor: status.colorname,
                    statusicon: status.statusicon,
                    total: total
                });
            }

        });
//debugger
        console.log($scope.mytasklist_bystatus);
        $scope.refreshTaskViewCaches();
    }
};
//// end scopes
});

//function inittooltip() {

//    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {

//        let instance = bootstrap.Tooltip.getInstance(el);
//        let newTitle = el.getAttribute('data-bs-title');

//        if (instance) {
//            instance.setContent({ '.tooltip-inner': newTitle });
//        } else {
//            new bootstrap.Tooltip(el);
//        }

//    });


//}
function initTableUI() {
    //debugger
    //inittooltip();
    initProjectTableColumnDragDrop();
}
function getProjectTableElement() {
    var ctrltbl= document.querySelector('.project-table');
    return document.querySelector('.project-table');
}

function getProjectTableStorageKey() {
    var projectId = 'default';
    try {
        var hash = window.location.hash || '';
        var match = hash.match(/id=([^&]+)/i);
        if (match && match[1]) {
            projectId = decodeURIComponent(match[1]);
        }
    } catch (ex) { }
    return 'pms_project_table_column_order_' + projectId;
}

function getHeaderIndex(cell) {
    if (!cell || !cell.parentNode) return -1;
    return Array.prototype.indexOf.call(cell.parentNode.children, cell);
}

function ensureProjectTableOriginalIndexes(table) {
    if (!table || !table.rows) return;

    Array.prototype.forEach.call(table.rows, function (row) {
        Array.prototype.forEach.call(row.children, function (cell, index) {
            if (!cell.hasAttribute('data-original-index')) {
                cell.setAttribute('data-original-index', index);
            }
        });
    });
}

function getProjectTableHeaderOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return [];
    return Array.prototype.map.call(table.tHead.rows[0].cells, function (th) {
        return parseInt(th.getAttribute('data-original-index'), 10);
    });
}

function syncProjectTableRowsWithHeaderOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length || !table.tBodies.length) return;

    ensureProjectTableOriginalIndexes(table);

    var order = getProjectTableHeaderOrder(table);
    if (!order.length) return;

    Array.prototype.forEach.call(table.tBodies[0].rows, function (row) {
        if (!row || !row.children || row.children.length !== order.length) return;

        order.forEach(function (originalIndex) {
            var foundCell = null;

            Array.prototype.forEach.call(row.children, function (cell) {
                if (parseInt(cell.getAttribute('data-original-index'), 10) === originalIndex) {
                    foundCell = cell;
                }
            });

            if (foundCell) {
                row.appendChild(foundCell);
            }
        });
    });
}

function startProjectTableRowObserver(table) {
    if (!table || !table.tBodies.length || table.dataset.rowObserverStarted === '1') return;

    table.dataset.rowObserverStarted = '1';

    var observer = new MutationObserver(function (mutations) {
        // IMPORTANT:
        // Do NOT use setTimeout here. setTimeout allows the new task/subtask
        // row to paint first in the old column position, then it moves again,
        // so the user sees blinking/flicker. MutationObserver runs before the
        // browser paints, so sync immediately.
        var hasNewRows = false;
        for (var i = 0; i < mutations.length; i++) {
            if (mutations[i].addedNodes && mutations[i].addedNodes.length) {
                hasNewRows = true;
                break;
            }
        }

        if (hasNewRows) {
            syncProjectTableRowsWithHeaderOrder(table);
        }
    });

    observer.observe(table.tBodies[0], { childList: true });
}

function clearProjectTableDragVisualState(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return;
    Array.prototype.forEach.call(table.tHead.rows[0].cells, function (th) {
        th.classList.remove('dragging-col');
        th.classList.remove('drag-over-col');
    });
}

function clearProjectTableDragState(table) {
    clearProjectTableDragVisualState(table);
    if (table && table.dataset) {
        delete table.dataset.dragFromIndex;
    }
}

function moveProjectTableColumn(table, fromIndex, toIndex) {
    if (!table || fromIndex === toIndex || fromIndex < 0 || toIndex < 0) return;

    ensureProjectTableOriginalIndexes(table);

    Array.prototype.forEach.call(table.rows, function (row) {
        if (!row || !row.children || row.children.length <= Math.max(fromIndex, toIndex)) return;

        var movingCell = row.children[fromIndex];
        if (!movingCell) return;

        var referenceCell = (fromIndex < toIndex)
            ? row.children[toIndex].nextSibling
            : row.children[toIndex];

        row.insertBefore(movingCell, referenceCell);
    });

    syncProjectTableRowsWithHeaderOrder(table);
}

function saveProjectTableColumnOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return;

    var order = Array.prototype.map.call(table.tHead.rows[0].cells, function (th) {
        return parseInt(th.getAttribute('data-original-index'), 10);
    });

    localStorage.setItem(getProjectTableStorageKey(), JSON.stringify(order));
}

function applySavedProjectTableColumnOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return;

    var savedOrder = [];
    try {
        savedOrder = JSON.parse(localStorage.getItem(getProjectTableStorageKey()) || '[]');
    } catch (ex) {
        savedOrder = [];
    }

    var headerCells = table.tHead.rows[0].cells;
    if (!savedOrder.length || savedOrder.length !== headerCells.length) return;

    savedOrder.forEach(function (originalIndex, targetIndex) {
        var currentCells = table.tHead.rows[0].cells;
        var currentIndex = -1;

        Array.prototype.forEach.call(currentCells, function (th, idx) {
            if (parseInt(th.getAttribute('data-original-index'), 10) === originalIndex) {
                currentIndex = idx;
            }
        });

        if (currentIndex > -1 && currentIndex !== targetIndex) {
            moveProjectTableColumn(table, currentIndex, targetIndex);
        }
    });
}

function initProjectTableColumnDragDrop() {
    //debugger
    var table = getProjectTableElement();
    if (!table || !table.tHead || !table.tHead.rows.length) return;

    var headerRow = table.tHead.rows[0];
    var fixedColumns =0; // Action + Milestone stay fixed

    Array.prototype.forEach.call(headerRow.cells, function (th, index) {
        if (!th.hasAttribute('data-original-index')) {
            th.setAttribute('data-original-index', index);
        }

        if (index < fixedColumns) {
            th.classList.add('column-fixed');
            th.removeAttribute('draggable');
            return;
        }

        th.classList.add('column-draggable');

        if (th.dataset.columnDragBound === '1') return;

        th.setAttribute('draggable', 'true');
        th.dataset.columnDragBound = '1';

        th.addEventListener('dragstart', function (e) {
            var fromIndex = getHeaderIndex(this);
            if (fromIndex < fixedColumns) {
                e.preventDefault();
                return;
            }

            table.dataset.dragFromIndex = fromIndex;
            this.classList.add('dragging-col');

            if (e.dataTransfer) {
                e.dataTransfer.effectAllowed = 'move';
                try {
                    e.dataTransfer.setData('text/plain', fromIndex.toString());
                } catch (ex) { }
            }
        });

        th.addEventListener('dragover', function (e) {
            var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
            var toIndex = getHeaderIndex(this);
            if (isNaN(fromIndex) || fromIndex < fixedColumns || toIndex < fixedColumns) return;

            e.preventDefault();
            clearProjectTableDragVisualState(table);
            this.classList.add('drag-over-col');

            var draggingCell = headerRow.cells[fromIndex];
            if (draggingCell) {
                draggingCell.classList.add('dragging-col');
            }
        });

        th.addEventListener('drop', function (e) {
            e.preventDefault();

            var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
            var toIndex = getHeaderIndex(this);

            if (isNaN(fromIndex) || fromIndex < fixedColumns || toIndex < fixedColumns || fromIndex === toIndex) {
                clearProjectTableDragState(table);
                return;
            }

            moveProjectTableColumn(table, fromIndex, toIndex);
            saveProjectTableColumnOrder(table);
            clearProjectTableDragState(table);
        });

        th.addEventListener('dragend', function () {
            clearProjectTableDragState(table);
        });
    });

    ensureProjectTableOriginalIndexes(table);

    if (table.dataset.columnOrderApplied !== '1') {
        applySavedProjectTableColumnOrder(table);
        table.dataset.columnOrderApplied = '1';
    }

    syncProjectTableRowsWithHeaderOrder(table);
    startProjectTableRowObserver(table);
}
/* =========================================================
   PROJECT TABLE COLUMN MENU - V7
   Features:
   1) Drag/drop column reorder
   2) Hide current column
   3) Show hidden column before/after current header
   4) Persist order + hidden columns in localStorage
   ========================================================= */
(function () {
    var FIXED_COLUMNS =0; // Action + Milestone cannot hide/move
    var MENU_ID = 'pmsProjectColumnHeaderMenu';

    function tableEl() {
        //debugger
        //var ctrltbl1 = document.querySelector('#tbDetails');
        setTimeout(function () {
            //debugger
            var ctrltbl1 = document.querySelector('#tbDetails');
            var ctrltbl2 = document.querySelector('.project-table');
           // console.log(ctrltbl1);

        }, 500);
        
        return document.querySelector('.project-table');
    }

    function storageKey() {
        var projectId = 'default';
        try {
            var hash = window.location.hash || '';
            var match = hash.match(/id=([^&]+)/i);
            if (match && match[1]) projectId = decodeURIComponent(match[1]);
        } catch (ex) { }
        return 'pms_project_table_column_config_' + projectId;
    }

    function oldOrderKey() {
        var projectId = 'default';
        try {
            var hash = window.location.hash || '';
            var match = hash.match(/id=([^&]+)/i);
            if (match && match[1]) projectId = decodeURIComponent(match[1]);
        } catch (ex) { }
        return 'pms_project_table_column_order_' + projectId;
    }

    function injectColumnMenuStyle() {
        if (document.getElementById('pmsColumnMenuStyle')) return;
        var css = document.createElement('style');
        css.id = 'pmsColumnMenuStyle';
        css.innerHTML = `
            .project-table thead th{ position:sticky; }
            .pms-th-inner{ display:flex; align-items:center; justify-content:center; gap:4px; width:100%; }
            .pms-th-text{ flex:1 1 auto; text-align:center; }
            .pms-th-menu-btn{
                flex:0 0 auto; border:0; background:transparent; color:#6b7280;
                width:20px; height:20px; line-height:18px; border-radius:50%;
                display:inline-flex; align-items:center; justify-content:center;
                font-size:15px; cursor:pointer; padding:0; margin-left:2px;
            }
            .pms-th-menu-btn:hover{ background:#f3e8ff; color:#7c006d; }
            .pms-column-menu{
                position:fixed; z-index:999999; min-width:150px; background:#fff;
                border:1px solid #ddd; border-radius:8px; box-shadow:0 8px 24px rgba(0,0,0,.18);
                padding:6px; font-size:13px; color:#111827;
            }
            .pms-column-menu .menu-title{font-weight:600; padding:7px 9px; border-bottom:1px solid #eee; margin-bottom:4px;}
            .pms-column-menu button{
                display:block; text-align:left; border:0; background:#fff;
                padding:7px 9px; border-radius:6px; cursor:pointer; color:#111827;
            }
            .pms-column-menu button:hover{background:#f3f4f6;}
            .pms-column-menu button:disabled{color:#aaa; cursor:not-allowed; background:#fff;}
            .pms-column-menu .menu-sep{height:1px;background:#eee;margin:5px 0;}
            .project-table thead th.column-draggable{ cursor:move; }
            .project-table thead th.column-draggable::after{ content:"" !important; }
        `;
        document.head.appendChild(css);
    }

    function ensureIndexes(table) {
        if (!table || !table.rows) return;
        Array.prototype.forEach.call(table.rows, function (row) {
            Array.prototype.forEach.call(row.children, function (cell, index) {
                if (!cell.hasAttribute('data-original-index')) {
                    cell.setAttribute('data-original-index', index);
                }
            });
        });
    }

    function getOriginalIndex(cell) {
        return parseInt(cell.getAttribute('data-original-index'), 10);
    }

    function getHeaderRow(table) {
        return table && table.tHead && table.tHead.rows.length ? table.tHead.rows[0] : null;
    }

    function getAllOriginalIndexes(table) {
        var row = getHeaderRow(table);
        if (!row) return [];
        return Array.prototype.map.call(row.cells, function (th) { return getOriginalIndex(th); });
    }

    function headerTitle(th) {
        if (!th) return '';
        var clone = th.cloneNode(true);
        Array.prototype.forEach.call(clone.querySelectorAll('.pms-th-menu-btn'), function (x) { x.remove(); });
        return (clone.innerText || clone.textContent || '').replace(/[↕]/g, '').replace(/\s+/g, ' ').trim() || 'Column';
    }

    function loadConfig(table) {
        var cfg = { order: [], hidden: [] };
        try {
            var raw = localStorage.getItem(storageKey());
            if (raw) cfg = JSON.parse(raw) || cfg;
        } catch (ex) { }

        // Backward support from old v5/v6 order-only storage
        if (!cfg.order || !cfg.order.length) {
            try {
                var old = JSON.parse(localStorage.getItem(oldOrderKey()) || '[]');
                if (old && old.length) cfg.order = old;
            } catch (ex2) { }
        }

        var all = getAllOriginalIndexes(table);
        if (!cfg.order || cfg.order.length !== all.length) cfg.order = all;
        if (!cfg.hidden) cfg.hidden = [];
        cfg.hidden = cfg.hidden.map(function (x) { return parseInt(x, 10); }).filter(function (x) { return !isNaN(x); });
        return cfg;
    }

    function saveConfig(table) {
        if (!table) return;
        var cfg = {
            order: getAllOriginalIndexes(table),
            hidden: []
        };
        Array.prototype.forEach.call(table.querySelectorAll('[data-original-index]'), function (cell) {
            var oi = getOriginalIndex(cell);
            if (cell.style.display === 'none' && cfg.hidden.indexOf(oi) === -1) cfg.hidden.push(oi);
        });
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        localStorage.setItem(oldOrderKey(), JSON.stringify(cfg.order));
    }

    function applyOrder(table, order) {
        if (!table || !order || !order.length) return;
        ensureIndexes(table);
        Array.prototype.forEach.call(table.rows, function (row) {
            order.forEach(function (originalIndex) {
                var found = null;
                Array.prototype.forEach.call(row.children, function (cell) {
                    if (getOriginalIndex(cell) === originalIndex) found = cell;
                });
                if (found) row.appendChild(found);
            });
        });
    }

    function applyHidden(table, hidden) {
        if (!table) return;
        hidden = hidden || [];
        Array.prototype.forEach.call(table.querySelectorAll('[data-original-index]'), function (cell) {
            var oi = getOriginalIndex(cell);
            cell.style.display = hidden.indexOf(oi) > -1 ? 'none' : '';
        });
    }

    function applyConfig(table) {
        if (!table) return;
        ensureIndexes(table);
        var cfg = loadConfig(table);
        applyOrder(table, cfg.order);
        applyHidden(table, cfg.hidden);
        decorateHeaders(table);
    }

    function closeMenu() {
        var old = document.getElementById(MENU_ID);
        if (old) old.remove();
    }

    function findHeaderByOriginal(table, originalIndex) {
        var row = getHeaderRow(table);
        if (!row) return null;
        var found = null;
        Array.prototype.forEach.call(row.cells, function (th) {
            if (getOriginalIndex(th) === originalIndex) found = th;
        });
        return found;
    }

    function moveOriginalBeforeAfter(table, hiddenOriginal, targetOriginal, pos) {
        var cfg = loadConfig(table);
        cfg.hidden = cfg.hidden.filter(function (x) { return x !== hiddenOriginal; });
        cfg.order = cfg.order.filter(function (x) { return x !== hiddenOriginal; });
        var targetIndex = cfg.order.indexOf(targetOriginal);
        if (targetIndex < 0) targetIndex = cfg.order.length - 1;
        if (pos === 'after') targetIndex++;
        cfg.order.splice(targetIndex, 0, hiddenOriginal);
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        localStorage.setItem(oldOrderKey(), JSON.stringify(cfg.order));
        applyConfig(table);
    }

    function hideColumn(table, originalIndex) {
        if (originalIndex < FIXED_COLUMNS) return;
        var cfg = loadConfig(table);
        if (cfg.hidden.indexOf(originalIndex) === -1) cfg.hidden.push(originalIndex);
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        applyConfig(table);
    }

    function showAllColumns(table) {
        var cfg = loadConfig(table);
        cfg.hidden = [];
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        applyConfig(table);
    }

    function hiddenColumns(table) {
        var cfg = loadConfig(table);
        return cfg.hidden.map(function (oi) {
            var th = findHeaderByOriginal(table, oi);
            return { originalIndex: oi, title: headerTitle(th) };
        });
    }

    function openMenu(e, th) {
        //debugger
        e.preventDefault();
        e.stopPropagation();
        closeMenu();

        var table = tableEl();
        if (!table) return;
        var oi = getOriginalIndex(th);
        var hidden = hiddenColumns(table);
        var title = headerTitle(th);

        var menu = document.createElement('div');
        menu.id = MENU_ID;
        menu.className = 'pms-column-menu';
        //  menu.innerHTML = '<div class="menu-title">' + title + '</div>';

        var hideBtn = document.createElement('button');
        hideBtn.type = 'button';
        hideBtn.innerHTML = 'Hide';
        hideBtn.className = 'p-0';
        hideBtn.disabled = oi < FIXED_COLUMNS;
        hideBtn.onclick = function (ev) { ev.stopPropagation(); hideColumn(table, oi); closeMenu(); };
        menu.appendChild(hideBtn);

        var showAll = document.createElement('button');
        showAll.type = 'button';
        showAll.innerHTML = 'Show all';
        showAll.className = 'p-0';
        showAll.disabled = hidden.length === 0;
        showAll.onclick = function (ev) { ev.stopPropagation(); showAllColumns(table); closeMenu(); };
        menu.appendChild(showAll);

        var sep = document.createElement('div');
        sep.className = 'menu-sep';
        menu.appendChild(sep);

        if (!hidden.length) {
            var none = document.createElement('button');
            none.type = 'button';
            none.disabled = true;
            none.innerHTML = 'No hidden columns';
            menu.appendChild(none);
        } else {
            hidden.forEach(function (h) {
                //var b1 = document.createElement('button');
                //b1.type = 'button';
                //b1.innerHTML = '<i class="bi bi-arrow-left-square" title="Add Before"></i> ' + h.title;
                //b1.onclick = function (ev) { ev.stopPropagation(); moveOriginalBeforeAfter(table, h.originalIndex, oi, 'before'); closeMenu(); };
                //menu.appendChild(b1);

                //var b2 = document.createElement('button');
                //b2.type = 'button';
                //b2.innerHTML = '<i class="bi bi-arrow-right-square" title="Add After"></i> ' + h.title;
                //b2.onclick = function (ev) { ev.stopPropagation(); moveOriginalBeforeAfter(table, h.originalIndex, oi, 'after'); closeMenu(); };
                //menu.appendChild(b2);

                var rowWrap = document.createElement('div');
                rowWrap.style.display = 'flex';
                rowWrap.style.gap = '6px';
                rowWrap.style.alignItems = 'center';

                // BEFORE BUTTON
                var b1 = document.createElement('button');
                b1.type = 'button';
                b1.className = 'd-flex align-items-center p-0';
                b1.innerHTML = '<i class="bi bi-arrow-left-square me-1" title="Add Before"></i> ';

                b1.onclick = function (ev) {
                    ev.stopPropagation();
                    moveOriginalBeforeAfter(table, h.originalIndex, oi, 'before');
                    closeMenu();
                };


                var bspan = document.createElement('span');
                bspan.type = 'span';
                bspan.className = 'd-flex align-items-center';
                bspan.innerHTML = ' ' + h.title + ' ';

                // AFTER BUTTON
                var b2 = document.createElement('button');
                b2.type = 'button';
                b2.className = 'd-flex align-items-center  p-0';
                b2.innerHTML = ' <i class="bi bi-arrow-right-square ms-1" title="Add After"></i>';

                b2.onclick = function (ev) {
                    ev.stopPropagation();
                    moveOriginalBeforeAfter(table, h.originalIndex, oi, 'after');
                    closeMenu();
                };

                // APPEND BOTH IN ONE LINE
                rowWrap.appendChild(b1);
                rowWrap.appendChild(bspan);
                rowWrap.appendChild(b2);

                menu.appendChild(rowWrap);

            });
        }

        document.body.appendChild(menu);
        var rect = th.getBoundingClientRect();
        var left = Math.min(rect.right - 150, window.innerWidth - 150);
        if (left < 8) left = 8;
        menu.style.left = left + 'px';
        menu.style.top = (rect.bottom + 4) + 'px';
    }

    function decorateHeaders(table) {
        injectColumnMenuStyle();
        var row = getHeaderRow(table);
        if (!row) return;

        Array.prototype.forEach.call(row.cells, function (th) {
            if (th.dataset.menuDecorated === '1') return;
            th.dataset.menuDecorated = '1';

            var originalHtml = th.innerHTML;
            th.innerHTML = '<div class="pms-th-inner"><span class="pms-th-text">' + originalHtml + '</span><button type="button" class="pms-th-menu-btn" title="Column options">⋮</button></div>';

            var btn = th.querySelector('.pms-th-menu-btn');
            btn.addEventListener('click', function (ev) { openMenu(ev, th); });
            btn.addEventListener('mousedown', function (ev) { ev.stopPropagation(); });
            btn.addEventListener('dragstart', function (ev) { ev.preventDefault(); ev.stopPropagation(); });
        });
    }

    function getHeaderIndex(cell) {
        if (!cell || !cell.parentNode) return -1;
        return Array.prototype.indexOf.call(cell.parentNode.children, cell);
    }

    function clearVisual(table) {
        var row = getHeaderRow(table);
        if (!row) return;
        Array.prototype.forEach.call(row.cells, function (th) {
            th.classList.remove('dragging-col');
            th.classList.remove('drag-over-col');
        });
    }

    function clearDrag(table) {
        clearVisual(table);
        if (table && table.dataset) delete table.dataset.dragFromIndex;
    }

    function moveColumn(table, fromIndex, toIndex) {
        if (!table || fromIndex === toIndex || fromIndex < 0 || toIndex < 0) return;
        ensureIndexes(table);
        Array.prototype.forEach.call(table.rows, function (row) {
            if (!row || !row.children || row.children.length <= Math.max(fromIndex, toIndex)) return;
            var movingCell = row.children[fromIndex];
            var ref = fromIndex < toIndex ? row.children[toIndex].nextSibling : row.children[toIndex];
            row.insertBefore(movingCell, ref);
        });
        applyHidden(table, loadConfig(table).hidden);
    }

    function rowObserver(table) {
        if (!table || !table.tBodies.length || table.dataset.rowObserverStartedV7 === '1') return;
        table.dataset.rowObserverStartedV7 = '1';
        var obs = new MutationObserver(function (mutations) {
            var hasNew = false;
            for (var i = 0; i < mutations.length; i++) {
                if (mutations[i].addedNodes && mutations[i].addedNodes.length) { hasNew = true; break; }
            }
            if (hasNew) applyConfig(table);
        });
        obs.observe(table.tBodies[0], { childList: true });
    }

    // Override previous function from v5/v6
    window.initProjectTableColumnDragDrop = function () {
        //alert("ok");
        var table = tableEl();
        if (!table || !getHeaderRow(table)) return;

        injectColumnMenuStyle();
        ensureIndexes(table);
        decorateHeaders(table);
        applyConfig(table);

        var row = getHeaderRow(table);
        Array.prototype.forEach.call(row.cells, function (th) {
            var idx = getHeaderIndex(th);
            var oi = getOriginalIndex(th);

            if (oi < FIXED_COLUMNS) {
                th.classList.add('column-fixed');
                th.removeAttribute('draggable');
                return;
            }

            th.classList.add('column-draggable');
            th.setAttribute('draggable', 'true');

            if (th.dataset.dragBoundV7 === '1') return;
            th.dataset.dragBoundV7 = '1';

            th.addEventListener('dragstart', function (e) {
                closeMenu();
                var fromIndex = getHeaderIndex(this);
                var originalIndex = getOriginalIndex(this);
                if (originalIndex < FIXED_COLUMNS) { e.preventDefault(); return; }
                table.dataset.dragFromIndex = fromIndex;
                this.classList.add('dragging-col');
                if (e.dataTransfer) {
                    e.dataTransfer.effectAllowed = 'move';
                    try { e.dataTransfer.setData('text/plain', fromIndex.toString()); } catch (ex) { }
                }
            });

            th.addEventListener('dragover', function (e) {
                var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
                var toIndex = getHeaderIndex(this);
                if (isNaN(fromIndex) || getOriginalIndex(this) < FIXED_COLUMNS) return;
                e.preventDefault();
                clearVisual(table);
                this.classList.add('drag-over-col');
                if (row.cells[fromIndex]) row.cells[fromIndex].classList.add('dragging-col');
            });

            th.addEventListener('drop', function (e) {
                e.preventDefault();
                var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
                var toIndex = getHeaderIndex(this);
                if (isNaN(fromIndex) || getOriginalIndex(this) < FIXED_COLUMNS || fromIndex === toIndex) {
                    clearDrag(table);
                    return;
                }
                moveColumn(table, fromIndex, toIndex);
                saveConfig(table);
                clearDrag(table);
            });

            th.addEventListener('dragend', function () { clearDrag(table); });
        });

        rowObserver(table);
        applyConfig(table);
    };

    document.addEventListener('click', function () { closeMenu(); });
    window.addEventListener('resize', function () { closeMenu(); });
})();


// =========================================================
// KANBAN HTML5 DRAG / DROP
// Kept isolated from existing dropdown/status/timer working.
// =========================================================
app.directive('kanbanCardDrag', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.attr('draggable', 'true');

            element.on('dragstart', function (evt) {
                var e = evt.originalEvent || evt;
                var card = scope.$eval(attrs.kanbanCardDrag);

                element.addClass('kanban-card-dragging');

                scope.beginKanbanDrag(card);

                if (e.dataTransfer) {
                    e.dataTransfer.effectAllowed = 'move';
                    try { e.dataTransfer.setData('text/plain', String((card && card.id) || '')); } catch (ignore) { }
                }
            });

            element.on('dragend', function () {
                element.removeClass('kanban-card-dragging');
                scope.$applyAsync(function () {
                    scope.endKanbanDrag();
                });
            });

            scope.$on('$destroy', function () {
                element.off('dragstart');
                element.off('dragend');
            });
        }
    };
});

app.directive('kanbanDropGroup', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            function removeHover() {
                element.removeClass('kanban-drag-over');
            }

            element.on('dragover', function (evt) {
                var e = evt.originalEvent || evt;
                if (e.preventDefault) e.preventDefault();
                if (e.dataTransfer) e.dataTransfer.dropEffect = 'move';
                element.addClass('kanban-drag-over');
            });

            element.on('dragleave', function (evt) {
                var e = evt.originalEvent || evt;
                var related = e.relatedTarget;
                if (!related || !element[0].contains(related)) {
                    removeHover();
                }
            });

            element.on('drop', function (evt) {
                var e = evt.originalEvent || evt;
                if (e.preventDefault) e.preventDefault();
                if (e.stopPropagation) e.stopPropagation();
                removeHover();

                var group = scope.$eval(attrs.kanbanDropGroup);
                if (scope.$root.$$phase) {
                    scope.dropKanbanCard(group);
                } else {
                    scope.$apply(function () {
                        scope.dropKanbanCard(group);
                    });
                }
            });

            scope.$on('$destroy', function () {
                element.off('dragover');
                element.off('dragleave');
                element.off('drop');
            });
        }
    };
});
