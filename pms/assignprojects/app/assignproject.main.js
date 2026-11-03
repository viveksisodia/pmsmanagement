
app.directive('pmsTeamPickerKeys', function () {
    return {restrict:'A',link:function(scope,element) {
        var input=element[0], wrap=input.closest('.pms-team-search-wrap'), index=-1;
        function options() { return Array.prototype.slice.call(wrap.querySelectorAll('.pms-team-person-option')); }
        function reset() { index=-1; options().forEach(function(a){a.classList.remove('pms-team-key-active');}); }
        function keydown(e) {
            var key=e.key || ({9:'Tab',13:'Enter',27:'Escape',38:'ArrowUp',40:'ArrowDown'})[e.keyCode];
            if (['Tab','Enter','Escape','ArrowUp','ArrowDown'].indexOf(key)<0) return;
            if(key==='Tab' || key==='Escape') {
                reset(); scope.$evalAsync(function(){scope.teamPicker.open=false; if(key==='Tab') scope.teamPicker.search='';});
                if(key==='Escape'){e.preventDefault();e.stopPropagation();} return;
            }
            e.preventDefault();e.stopPropagation();
            var list=options();
            if(key==='Enter') {
                if(scope.teamPicker.open && index>=0 && list[index]) {
                    var chosen=list[index];reset();chosen.click();input.focus();
                } return;
            }
            scope.$evalAsync(function(){scope.teamPicker.open=true;});
            if(!list.length)return;
            index=index<0 ? (key==='ArrowDown'?0:list.length-1) : (index+(key==='ArrowDown'?1:-1)+list.length)%list.length;
            list.forEach(function(a,i){a.classList.toggle('pms-team-key-active',i===index);});
            list[index].scrollIntoView({block:'nearest'});
        }
        scope.$watchCollection('selectedOwnerIds', reset);
        element.on('keydown',keydown);element.on('input',reset);element.on('blur',reset);
        scope.$on('$destroy',function(){element.off('keydown',keydown);element.off('input',reset);element.off('blur',reset);});
    }};
});

app.directive('pmsOwnerKeys', function () {
    return { restrict: 'A', link: function (scope, element) {
        var input = element[0], index = -1;
        function items() { return Array.prototype.slice.call(input.parentNode.querySelectorAll('a.dropdown-item')); }
        function reset() { index = -1; items().forEach(function (a) { a.classList.remove('pms-owner-key-active'); }); }
        function keydown(e) {
            var key = e.key || ({38:'ArrowUp',40:'ArrowDown',13:'Enter',27:'Escape'})[e.keyCode];
            if (['ArrowUp','ArrowDown','Enter','Escape'].indexOf(key) < 0) return;
            e.preventDefault(); e.stopPropagation();
            var list = items();
            if (key === 'Escape') {
                reset();
                var wrap = input.closest('.dropdown.w-100');
                var toggle = wrap && wrap.querySelector('[data-bs-toggle="dropdown"]');
                if (toggle && window.bootstrap && bootstrap.Dropdown) bootstrap.Dropdown.getOrCreateInstance(toggle).hide();
                if (toggle) { toggle.setAttribute('tabindex','0'); toggle.focus(); }
                return;
            }
            if (!list.length) return;
            if (key === 'Enter') { if (index >= 0 && list[index]) { var chosen = list[index]; reset(); chosen.click(); } return; }
            index = index < 0 ? (key === 'ArrowDown' ? 0 : list.length - 1) : (index + (key === 'ArrowDown' ? 1 : -1) + list.length) % list.length;
            list.forEach(function (a, i) { a.classList.toggle('pms-owner-key-active', i === index); });
            list[index].scrollIntoView({block:'nearest'});
        }
        element.on('keydown', keydown); element.on('input', reset); element.on('focus', reset);
        scope.$on('$destroy', function () { element.off('keydown', keydown); element.off('input', reset); element.off('focus', reset); });
    }};
});
app.directive('pmsPersonAvatar', function () {
    return { restrict:'A', scope:{ personName:'@', personPhoto:'@' },
        template:'<img ng-if="photo" ng-src="{{photo}}" alt="{{personName}}"><span ng-if="!photo" class="pms-person-initials">{{initials}}</span>',
        link:function(scope,element) {
            function refresh() {
                var name = (scope.personName || '').trim(), parts = name.split(/\s+/);
                scope.initials = name ? (parts[0].charAt(0) + (parts.length > 1 ? parts[parts.length-1].charAt(0) : parts[0].charAt(1))).toUpperCase() : '?';
                var photo = scope.personPhoto || '';
                scope.photo = /(?:^|\/)user\.jpg(?:\?.*)?$/i.test(photo) ? '' : photo;
            }
            scope.$watchGroup(['personName','personPhoto'],refresh);
            function failed(e) { if (e.target.tagName === 'IMG') scope.$applyAsync(function () { scope.photo = ''; }); }
            element[0].addEventListener('error',failed,true);
            scope.$on('$destroy',function () { element[0].removeEventListener('error',failed,true); });
        }
    };
});





app.directive('ngDragstart', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragstart);
        element.on('dragstart', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDragover', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragover);
        element.on('dragover', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDragleave', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragleave);
        element.on('dragleave', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDrop', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDrop);
        element.on('drop', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDragend', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragend);
        element.on('dragend', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});

// Fire hierarchy start-date logic only after the browser commits a complete date.
// Native date pickers can emit intermediate input/model changes while the user
// navigates month or year; the DOM change event represents the selected date.
app.directive('hierarchyDateSelect', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.hierarchyDateSelect);

        function onCommittedDateChange(event) {
            var selectedValue = element[0] && element[0].value
                ? element[0].value
                : '';

            scope.$applyAsync(function () {
                fn(scope, {
                    $selectedDateValue: selectedValue,
                    $event: event.originalEvent || event
                });
            });
        }

        element.on('change', onCommittedDateChange);
        scope.$on('$destroy', function () {
            element.off('change', onCommittedDateChange);
        });
    };
});

app.controller("assignprojectCtrl", function ($scope, $http, $timeout,$document,assignprojectService,assignprojectState, $compile) {
    
    //alert("ok");
    //#region add variables
    $scope.zoom = {level: 100};

    // One configurable source for reset scheduling.
    // Working hours may be greater than this value. It is used only to split
    // total hours across reset dates/times and for informational display.
    $scope.perdayhrs = 8;
    if (assignprojectService.setPerDayHours) {
        assignprojectService.setPerDayHours($scope.perdayhrs);
    }
    $scope.$watch('perdayhrs', function (value) {
        var parsed = Number(value);
        if (!isFinite(parsed) || parsed <= 0) {
            parsed = 8;
        }
        if (assignprojectService.setPerDayHours) {
            assignprojectService.setPerDayHours(parsed);
        }
        if ($scope.view === 'timesheet' && angular.isFunction($scope.buildTimesheet)) {
            $scope.buildTimesheet();
        }
    });
    $scope.isLoading = false;
    $scope.isLoading2 = false;   
    $scope.activetype = '0';
    $scope.projectState = assignprojectState;
    $scope.isload = "0";
    $scope.projectmasterlist = [];
    $scope.view = 'table';
    // The user-facing Days selection uses the existing daily-cell ('week') scale.
    $scope.ganttScale = "week";
    $scope.allGanttRows = [];
    $scope.ganttRows = [];
    $scope.ganttCells = [];
    $scope.ganttHeaderGroups = [];
    $scope.ganttCellWidth = 40;
    $scope.ganttStart = null;
    $scope.ganttEnd = null;
    $scope.ganttIsLoading = false;

    // Timesheet view uses the same already-loaded project hierarchy.
    $scope.timesheetRows = [];
    $scope.timesheetDays = [];
    $scope.timesheetMonths = [];
    $scope.timesheetDayWidth = 24;
    $scope.timesheetTimelineWidth = 0;
    $scope.calendarDate = null;
    $scope.calendarTitle = '';
    $scope.calendarWeeks = [];
    $scope.calendarWeekDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    $scope.selectedCalendarDay = null;
    $scope.timesheetRangeLabel = '';
    $scope.timesheetStart = null;
    $scope.timesheetEnd = null;
    $scope.currentIndex = 0;
    $scope.currentlist = [];
    $scope.currentUserType = "";
    $scope.currentFileType = "";
    $scope.currentFProjectId = "";
    $scope.currentFMilestoneId = "";
    $scope.currentFTaskId = "";
    $scope.currentFId = "";


    $scope.currentownerslist = [];

    $scope.owners = [];
    $scope.ownersnew = {
        id: "",
        name: "",
        profileimage: "",
        shortname: ""
    };

    $scope.selectedOwnerIds = [];
    $scope.searchOwner = "";

    // Kanban popup editors. Owner is intentionally single-select.
    $scope.kanbanOwnerEdit = {
        wrapper: null,
        context: null,
        selectedOwnerId: "",
        originalOwnerId: "",
        searchText: "",
        saving: false
    };
    $scope.kanbanDurationEdit = {
        wrapper: null,
        context: null,
        hours: "",
        minutes: "",
        originalHours: "",
        originalMinutes: "",
        saving: false
    };

    $scope.currentPage = 1;
    $scope.searchText = "";
    $scope.sortColumn = "id";
    $scope.reverseSort = false;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.types = ["1", "2"];
    $scope.pageSize = 10;
    $scope.ProjectName = "";
    //End comman function
    $scope.statuslists = [];
    $scope.taskSituationLists = [];
    $scope.prioritylists = [];
    $scope.projectlist = [];
    $scope.projectDurationDays = 0;
    // Top summary filter is independent from activetype (All/Unassigned/Assigned).
    // Keeping this separate preserves the existing assignment-filter behaviour.
    $scope.topStatusFilter = 'all';
    $scope.taskSituationFilter = 'all';
    $scope.topFilterCounts = {
        total: 0,
        byStatus: {}
    };

    $scope.refreshProjectDurationDays = function () {
        var project = $scope.projectlist || {};
        var startDate = project.StartDate;
        var endDate =
            project.TargetDate !== null &&
                project.TargetDate !== undefined &&
                String(project.TargetDate).trim() !== ''
                ? project.TargetDate
                : project.StartDate;

        if (!startDate || !endDate) {
            $scope.projectDurationDays = 0;
            return Promise.resolve(0);
        }

        return assignprojectService.getProjectDurationDays(startDate, endDate)
            .then(function (response) {
                var value = response && response.data
                    ? response.data.d
                    : 0;
                var durationDays = Number(value);

                $scope.projectDurationDays =
                    isFinite(durationDays) && durationDays > 0
                        ? durationDays
                        : 0;

                return $scope.projectDurationDays;
            })
            .catch(function (error) {
                console.error(
                    'Project duration days calculation failed',
                    error
                );
                $scope.projectDurationDays = 0;
                return 0;
            });
    };

    $scope.milestonetasklist = [];
    $scope.showfileslist = [];
    $scope.currentprojectid = "0";
    $scope.selectedprojectid = "";
    $scope.selectedprojectname = "";
    $scope.assignprojecttask = function (viewtype) {
        debugger
        if (viewtype != "") {
            if (viewtype == "0"&&$scope.activetype == "0") {
                document.getElementById('goproject').click();
            }
            else {
                $scope.activetype = viewtype;
            }

        }
        else {
            $scope.activetype = "0";
        }

        // Refresh the same Kanban board when All / Unassigned / Assigned changes.
        if ($scope.view === "kanban") {
            $scope.openKanbanView();
        }
        else if ($scope.view === "timesheet") {
            $scope.openTimesheetView();
        }
        else if ($scope.view === "calendar") {
            $scope.openCalendarView();
        }

        $scope.refreshTopFilterCounts();
    }

    // Assignment view hierarchy: 0 = All, 1 = Unassigned, 2 = Assigned
    // Parent remains visible when any matching lowest-level child exists.
    $scope.isDirectlyAssignedForView = function (item) {
        if (!item) return false;

        var assignTo = item.assignto;
        if (assignTo === undefined) assignTo = item.AssignTo;
        if (assignTo === undefined) assignTo = item.assignid;
        if (assignTo === undefined) assignTo = item.AssignId;

        var assignedFlag = item.isassigned;
        if (assignedFlag === undefined) assignedFlag = item.IsAssigned;
        if (assignedFlag === undefined) assignedFlag = item.isreassign;
        if (assignedFlag === undefined) assignedFlag = item.isAssignedTemp;

        if (assignedFlag === true || Number(assignedFlag) === 1) {
            return true;
        }

        return assignTo !== null &&
            assignTo !== undefined &&
            String(assignTo).trim() !== '' &&
            String(assignTo) !== '0';
    };

    $scope.hasAssignedTaskBranch = function (task) {
        if (!task) return false;

        var subtasks = task.subtasks || task.Subtasks || [];
        if (subtasks.length > 0) {
            for (var i = 0; i < subtasks.length; i++) {
                if ($scope.isDirectlyAssignedForView(subtasks[i])) return true;
            }
            return false;
        }

        return $scope.isDirectlyAssignedForView(task);
    };

    $scope.hasUnassignedTaskBranch = function (task) {
        if (!task) return false;

        var subtasks = task.subtasks || task.Subtasks || [];
        if (subtasks.length > 0) {
            for (var i = 0; i < subtasks.length; i++) {
                if (!$scope.isDirectlyAssignedForView(subtasks[i])) return true;
            }
            return false;
        }

        return !$scope.isDirectlyAssignedForView(task);
    };

    $scope.hasAssignedMilestoneBranch = function (milestone) {
        if (!milestone) return false;

        var tasks = milestone.tasks || milestone.Tasks || [];
        if (tasks.length > 0) {
            for (var i = 0; i < tasks.length; i++) {
                if ($scope.hasAssignedTaskBranch(tasks[i])) return true;
            }
            return false;
        }

        return $scope.isDirectlyAssignedForView(milestone);
    };

    $scope.hasUnassignedMilestoneBranch = function (milestone) {
        if (!milestone) return false;

        var tasks = milestone.tasks || milestone.Tasks || [];
        if (tasks.length > 0) {
            for (var i = 0; i < tasks.length; i++) {
                if ($scope.hasUnassignedTaskBranch(tasks[i])) return true;
            }
            return false;
        }

        return !$scope.isDirectlyAssignedForView(milestone);
    };

    $scope.getTopStatusId = function (item) {
        if (!item) return '';
        var statusId = item.P_Status;
        if (statusId === undefined) statusId = item.statusid;
        if (statusId === undefined) statusId = item.StatusId;
        return statusId === undefined || statusId === null ? '' : String(statusId);
    };

    $scope.matchesTopStatusFilter = function (item, statusId) {
        var filter = statusId === undefined || statusId === null || statusId === ''
            ? $scope.topStatusFilter
            : statusId;
        return String(filter || 'all') === 'all' ||
            $scope.getTopStatusId(item) === String(filter);
    };

    $scope.getTaskSituationId = function (item) {
        if (!item) return '';
        var situationId = item.CriticalLabelId;
        if (situationId === undefined) situationId = item.criticalLabelId;
        if (situationId === undefined) situationId = item.criticallabelid;
        return situationId === undefined || situationId === null ? '' : String(situationId);
    };

    $scope.matchesTaskSituationFilter = function (item) {
        var filter = String($scope.taskSituationFilter || 'all');
        if (filter === 'all') return true;

        var situationId = $scope.getTaskSituationId(item);
        if (filter === 'none') return situationId === '' || situationId === '0';
        return situationId === filter;
    };

    $scope.matchesAssignmentView = function (item) {
        var view = String($scope.activetype || '0');
        if (view === '0') return true;

        var assigned = $scope.isDirectlyAssignedForView(item);
        if (view === '1') return !assigned;
        if (view === '2') return assigned;
        return true;
    };

    $scope.matchesTopLeafFilters = function (item, filterName) {
        return $scope.matchesAssignmentView(item) &&
            $scope.matchesTopStatusFilter(item, filterName) &&
            $scope.matchesTaskSituationFilter(item);
    };

    $scope.hasMatchingTaskBranch = function (task, filterName) {
        if (!task) return false;

        var subtasks = task.subtasks || task.Subtasks || [];
        if (subtasks.length > 0) {
            for (var i = 0; i < subtasks.length; i++) {
                if ($scope.matchesTopLeafFilters(subtasks[i], filterName)) return true;
            }
            return false;
        }

        return $scope.matchesTopLeafFilters(task, filterName);
    };

    $scope.hasMatchingMilestoneBranch = function (milestone, filterName) {
        if (!milestone) return false;

        var tasks = milestone.tasks || milestone.Tasks || [];
        if (tasks.length > 0) {
            for (var i = 0; i < tasks.length; i++) {
                if ($scope.hasMatchingTaskBranch(tasks[i], filterName)) return true;
            }
            return false;
        }

        return $scope.matchesTopLeafFilters(milestone, filterName);
    };

    $scope.refreshTopFilterCounts = function () {
        var counts = { total: 0, byStatus: {} };

        angular.forEach($scope.statuslists || [], function (status) {
            counts.byStatus[String(status.id)] = 0;
        });

        function addLeaf(item) {
            if (!$scope.matchesAssignmentView(item) || !$scope.matchesTaskSituationFilter(item)) return;

            counts.total++;
            var statusId = $scope.getTopStatusId(item);
            if (statusId !== '') {
                if (counts.byStatus[statusId] === undefined) counts.byStatus[statusId] = 0;
                counts.byStatus[statusId]++;
            }
        }

        angular.forEach((($scope.projectlist || {}).miles || []), function (milestone) {
            var tasks = milestone.tasks || milestone.Tasks || [];
            if (!tasks.length) {
                addLeaf(milestone);
                return;
            }

            angular.forEach(tasks, function (task) {
                var subtasks = task.subtasks || task.Subtasks || [];
                if (!subtasks.length) {
                    addLeaf(task);
                    return;
                }
                angular.forEach(subtasks, addLeaf);
            });
        });

        $scope.topFilterCounts = counts;
        return counts;
    };

    $scope.applyTopStatusFilter = function (statusId) {
        $scope.topStatusFilter = statusId === undefined || statusId === null || statusId === ''
            ? 'all'
            : String(statusId);

        // Open only matching branches. Existing expansion state is preserved for Total.
        if ($scope.topStatusFilter !== 'all') {
            angular.forEach((($scope.projectlist || {}).miles || []), function (milestone) {
                if (!$scope.hasMatchingMilestoneBranch(milestone)) return;
                milestone.isExpanded = true;
                angular.forEach(milestone.tasks || milestone.Tasks || [], function (task) {
                    if ($scope.hasMatchingTaskBranch(task)) task.isExpanded = true;
                });
            });
        }

        if ($scope.view === 'kanban') $scope.openKanbanView();
        else if ($scope.view === 'timesheet') $scope.openTimesheetView();
        else if ($scope.view === 'calendar') $scope.openCalendarView();
        else if ($scope.view === 'gantt') $scope.buildGantt();
    };

    $scope.applyTaskSituationFilter = function (selectedSituationId) {
        // The dropdown is rendered inside ng-if, which creates a child scope.
        // Always copy its selected value back to the controller scope used by
        // the common List/Kanban/Gantt/Timesheet/Calendar filter predicates.
        $scope.taskSituationFilter = String(selectedSituationId || 'all');
        $scope.refreshTopFilterCounts();

        if ($scope.taskSituationFilter !== 'all' || $scope.topStatusFilter !== 'all') {
            angular.forEach((($scope.projectlist || {}).miles || []), function (milestone) {
                if (!$scope.hasMatchingMilestoneBranch(milestone)) return;
                milestone.isExpanded = true;
                angular.forEach(milestone.tasks || milestone.Tasks || [], function (task) {
                    if ($scope.hasMatchingTaskBranch(task)) task.isExpanded = true;
                });
            });
        }

        if ($scope.view === 'kanban') $scope.openKanbanView();
        else if ($scope.view === 'timesheet') $scope.openTimesheetView();
        else if ($scope.view === 'calendar') $scope.openCalendarView();
        else if ($scope.view === 'gantt') $scope.buildGantt();
    };

    $scope.filterMilestoneAssignView = function (milestone) {
        return $scope.hasMatchingMilestoneBranch(milestone);
    };

    $scope.filterTaskAssignView = function (task) {
        return $scope.hasMatchingTaskBranch(task);
    };

    $scope.filterSubtaskAssignView = function (subtask) {
        return $scope.matchesTopLeafFilters(subtask);
    };


    // Kanban uses the existing project hierarchy and activetype:
    // 0 = All, 1 = Unassigned, 2 = Assigned.
    // Every hierarchy level becomes a card: milestone, task and subtask.
    $scope.kanbanItems = [];
    $scope._kanbanProjectRef = null;

    $scope.getKanbanItemTitle = function (item) {
        if (!item) return '';
        var src = item.source || item;
        if (item.tasktype === 'milestone') return src.Milestone || src.milestone || '';
        if (item.tasktype === 'task') return src.taskname || src.Taskname || src.TaskName || '';
        return src.Subtaskname || src.subtaskname || src.SubTaskName || '';
    };

    $scope.getKanbanDescription = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.SubTaskDescription || src.taskdescription || src.TaskDescription ||
            src.miledescription || src.MilestoneDescription || '';
    };

    $scope.getKanbanStatusId = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.P_Status !== undefined ? src.P_Status :
            (src.statusid !== undefined ? src.statusid : src.StatusId);
    };

    $scope.getKanbanPriorityId = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.Priority !== undefined ? src.Priority :
            (src.priorityid !== undefined ? src.priorityid : src.PriorityId);
    };

    $scope.getKanbanPriorityName = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.priority_name || src.Priorityname || src.priorityName || '';
    };

    $scope.getKanbanPriorityColor = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.priority_color || src.Prioritycolor || src.priorityColor || '#eef2f7';
    };
    $scope.getKanbanStatusName = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.status_name || src.Statusname || src.statusName || '';
    };
    $scope.getKanbanStatusColor = function (item, status) {
        var src = (item && (item.source || item)) || {};
        return src.status_color || src.StatusColor || (status && status.colorname) || '#3978de';
    };

    $scope.getKanbanStartDate = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.StartDate || src.Startdate || src.startDate || src.startdate || null;
    };

    $scope.getKanbanEndDate = function (item) {
        var src = (item && (item.source || item)) || {};
        return src.EndDate || src.Targetdate || src.TargetDate || src.endDate || src.targetdate || null;
    };

    function clampKanbanPercent(value) {
        value = Number(value) || 0;
        return Math.max(0, Math.min(100, Math.round(value)));
    }

    function getKanbanCalendarDayNumber(value) {
        var date = normalizeHierarchyDateOnly(value);
        if (!date) return null;
        return Math.floor(Date.UTC(
            date.getFullYear(),
            date.getMonth(),
            date.getDate()
        ) / 86400000);
    }

    $scope.getKanbanTimelineMetrics = function (item) {
        var startValue = $scope.getKanbanStartDate(item);
        var endValue = $scope.getKanbanEndDate(item) || startValue;
        var startDay = getKanbanCalendarDayNumber(startValue);
        var endDay = getKanbanCalendarDayNumber(endValue);

        if (startDay === null && endDay !== null) startDay = endDay;
        if (endDay === null && startDay !== null) endDay = startDay;

        if (startDay === null || endDay === null || endDay < startDay) {
            return {
                valid: false,
                percent: 0,
                totalDays: 0,
                elapsedDays: 0,
                leftDays: 0,
                startsInDays: 0
            };
        }

        var now = new Date();
        var todayDay = Math.floor(Date.UTC(
            now.getFullYear(),
            now.getMonth(),
            now.getDate()
        ) / 86400000);

        var totalDays = (endDay - startDay) + 1;
        var elapsedDays = 0;
        var startsInDays = 0;

        if (todayDay < startDay) {
            startsInDays = startDay - todayDay;
        } else if (todayDay > endDay) {
            elapsedDays = totalDays;
        } else {
            // Count only fully completed days. The currently running day is
            // intentionally excluded from elapsed days and remains in left days.
            elapsedDays = Math.max(todayDay - startDay, 0);
        }

        var leftDays = Math.max(totalDays - elapsedDays, 0);
        var percent = totalDays > 0
            ? clampKanbanPercent((elapsedDays / totalDays) * 100)
            : 0;

        return {
            valid: true,
            percent: percent,
            totalDays: totalDays,
            elapsedDays: elapsedDays,
            leftDays: leftDays,
            startsInDays: startsInDays
        };
    };
    $scope.getKanbanTimelinePercent = function (item) {
        return $scope.getKanbanTimelineMetrics(item).percent;
    };
    $scope.getKanbanTimelineTooltip = function (item) {
        var metrics = $scope.getKanbanTimelineMetrics(item);
        if (!metrics.valid) return 'Timeline dates are not available';

        var source = (item && (item.source || item)) || {};
        var startValue = $scope.getKanbanStartDate(item);
        var endValue = $scope.getKanbanEndDate(item) || startValue;
        var startText = formatDateOnly(startValue);
        var endText = formatDateOnly(endValue);
        var startTime = source.StartTime || source.starttime || '';
        var workingMinutes = $scope.getDurationMinutesForValidation
            ? $scope.getDurationMinutesForValidation(source)
            : 0;
        var capacity = getPerDayDateCapacity(source, startValue, endValue);
        var workingText = Math.floor(workingMinutes / 60) + ':' +
            String(workingMinutes % 60).padStart(2, '0');

        var text = 'Start Date: ' + startText +
            (startTime ? ' ' + startTime : '') +
            ' | End Date: ' + endText +
            ' | Working Hrs: ' + workingText +
            ' | Reset Basis: ' + capacity.perDayHours + ' hrs/day' +
            ' | Duration: ' + metrics.totalDays +
            ' | Spent Days: ' + metrics.elapsedDays +
            ' | Left Days: ' + metrics.leftDays;

        if (metrics.startsInDays > 0) {
            text += ' | Starts In: ' + metrics.startsInDays + ' days';
        }
        return text;
    };


    /*
     * Keep the table/Kanban Timeline bar connected to the hierarchy row that
     * Angular is rendering. This changes display state only; save/validation
     * services remain untouched.
     */
    $scope.refreshHierarchyTimelineState = function (item, type, startValue, endValue) {
        if (!item) return;

        var normalizedType = String(type || '').toLowerCase();
        var startDate = cloneHierarchyDateOnly(startValue);
        var endDate = cloneHierarchyDateOnly(endValue || startValue);
        var targets = [];

        function addTarget(target) {
            if (!target) return;
            if (targets.indexOf(target) === -1) targets.push(target);
        }

        function findHierarchyRow() {
            var miles = ($scope.projectlist && $scope.projectlist.miles) || [];
            var itemId = String(item.id !== undefined ? item.id : '');

            for (var mi = 0; mi < miles.length; mi++) {
                var mile = miles[mi];

                if (normalizedType === 'milestone' && String(mile.id) === itemId) {
                    return mile;
                }

                var tasks = mile.tasks || [];
                for (var ti = 0; ti < tasks.length; ti++) {
                    var task = tasks[ti];

                    if (normalizedType === 'task' && String(task.id) === itemId) {
                        return task;
                    }

                    var subtasks = task.subtasks || [];
                    for (var si = 0; si < subtasks.length; si++) {
                        if (
                            normalizedType === 'subtask' &&
                            String(subtasks[si].id) === itemId
                        ) {
                            return subtasks[si];
                        }
                    }
                }
            }

            return null;
        }

        addTarget(item.source || item);
        addTarget(item);
        addTarget(findHierarchyRow());

        targets.forEach(function (target) {
            if (normalizedType === 'milestone') {
                target.StartDate = cloneHierarchyDateOnly(startDate);
                target.EndDate = cloneHierarchyDateOnly(endDate);
            } else {
                target.Startdate = cloneHierarchyDateOnly(startDate);
                target.Targetdate = cloneHierarchyDateOnly(endDate);
            }

            // A lightweight revision makes the table DOM refresh even when the
            // date value returned by the server is equal to a previous Date object.
            target._timelineRevision = (Number(target._timelineRevision) || 0) + 1;
        });

        $scope.$evalAsync();
    };

    /*
     * Bootstrap stores the initial tooltip title. Recreate only the hovered
     * Timeline tooltip so it always shows the latest dates/day totals.
     */
    $scope.refreshHierarchyTimelineTooltip = function ($event, item) {
        var el = $event && ($event.currentTarget || $event.target);
        if (!el || !window.bootstrap || !bootstrap.Tooltip) return;

        var title = $scope.getKanbanTimelineTooltip(item);
        el.setAttribute('data-bs-title', title);
        el.setAttribute('data-bs-original-title', title);

        var instance = bootstrap.Tooltip.getInstance(el);
        if (instance) instance.dispose();

        new bootstrap.Tooltip(el, {
            container: 'body',
            trigger: 'hover',
            boundary: 'window'
        });
    };


    // ---------------------------------------------------------------------
    // Hierarchy timeline hover popup + end-date editor.
    // This is intentionally separate from the existing duration/start-date
    // handlers so the current hierarchy and assignment behaviour is unchanged.
    // ---------------------------------------------------------------------
    var hierarchyTimelineHideTimer = null;

    $scope.hierarchyTimelineHover = {
        visible: false,
        top: 0,
        left: 0,
        item: null,
        type: '',
        parent: null,
        grandparent: null,
        label: '',
        startText: '',
        endText: '',
        metrics: null,
        canEdit: false
    };

    $scope.hierarchyEndDateEdit = {
        item: null,
        type: '',
        parent: null,
        grandparent: null,
        label: '',
        startDate: null,
        oldEndDate: null,
        newEndDate: null,
        minDate: '',
        saving: false
    };

    function cancelHierarchyTimelineHideTimer() {
        if (hierarchyTimelineHideTimer) {
            $timeout.cancel(hierarchyTimelineHideTimer);
            hierarchyTimelineHideTimer = null;
        }
    }

    $scope.cancelHierarchyTimelineHide = function () {
        cancelHierarchyTimelineHideTimer();
    };

    $scope.scheduleHierarchyTimelineHide = function () {
        cancelHierarchyTimelineHideTimer();
        hierarchyTimelineHideTimer = $timeout(function () {
            $scope.hierarchyTimelineHover.visible = false;
        }, 180);
    };

    $scope.closeHierarchyTimelineHover = function () {
        cancelHierarchyTimelineHideTimer();
        $scope.hierarchyTimelineHover.visible = false;
    };

    function findHierarchyTimelineContext(item, type, parent, grandparent) {
        var normalizedType = String(type || '').toLowerCase();
        var context = {
            item: item,
            type: normalizedType,
            parent: parent || null,
            grandparent: grandparent || null
        };

        if (context.parent && (normalizedType !== 'subtask' || context.grandparent)) {
            return context;
        }

        var miles = ($scope.projectlist && $scope.projectlist.miles) || [];
        var itemId = String(item && item.id !== undefined ? item.id : '');

        for (var mi = 0; mi < miles.length; mi++) {
            var mile = miles[mi];
            if (normalizedType === 'milestone' && String(mile.id) === itemId) {
                context.item = mile;
                return context;
            }

            var tasks = mile.tasks || [];
            for (var ti = 0; ti < tasks.length; ti++) {
                var task = tasks[ti];
                if (normalizedType === 'task' && String(task.id) === itemId) {
                    context.item = task;
                    context.parent = mile;
                    return context;
                }

                var subtasks = task.subtasks || [];
                for (var si = 0; si < subtasks.length; si++) {
                    if (normalizedType === 'subtask' && String(subtasks[si].id) === itemId) {
                        context.item = subtasks[si];
                        context.parent = task;
                        context.grandparent = mile;
                        return context;
                    }
                }
            }
        }

        return context;
    }

    $scope.openHierarchyTimelineHover = function ($event, item, type, parent, grandparent) {
        if (!item) return;

        cancelHierarchyTimelineHideTimer();

        var context = findHierarchyTimelineContext(item, type, parent, grandparent);
        var metrics = $scope.getKanbanTimelineMetrics(context.item);
        var target = $event && ($event.currentTarget || $event.target);
        var rect = target && target.getBoundingClientRect
            ? target.getBoundingClientRect()
            : { left: 20, right: 220, top: 20, bottom: 45 };

        var popupWidth = 330;
        var popupHeight = 238;
        var viewportWidth = window.innerWidth || document.documentElement.clientWidth || 1024;
        var viewportHeight = window.innerHeight || document.documentElement.clientHeight || 768;
        var left = Math.min(
            Math.max(8, rect.left + ((rect.width || (rect.right - rect.left)) / 2) - (popupWidth / 2)),
            Math.max(8, viewportWidth - popupWidth - 8)
        );
        var top = rect.bottom + 8;

        if (top + popupHeight > viewportHeight) {
            top = Math.max(8, rect.top - popupHeight - 8);
        }

        $scope.hierarchyTimelineHover = {
            visible: true,
            top: Math.round(top),
            left: Math.round(left),
            item: context.item,
            type: context.type,
            parent: context.parent,
            grandparent: context.grandparent,
            label: $scope.getHierarchyItemLabel(context.type, context.item),
            startText: formatDateOnly($scope.getKanbanStartDate(context.item)),
            endText: formatDateOnly(
                $scope.getKanbanEndDate(context.item) ||
                $scope.getKanbanStartDate(context.item)
            ),
            metrics: metrics,
            canEdit: typeof $scope.canEdit === 'function'
                ? $scope.canEdit(context.type)
                : true
        };
    };

    $scope.openHierarchyEndDatePopup = function ($event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }

        cancelHierarchyTimelineHideTimer();

        var hover = $scope.hierarchyTimelineHover || {};
        if (!hover.item || !hover.type) return;
        if (!$scope.requireEdit(hover.type)) return;

        var startDate = normalizeHierarchyDateOnly($scope.getKanbanStartDate(hover.item));
        var endDate = normalizeHierarchyDateOnly(
            $scope.getKanbanEndDate(hover.item) ||
            $scope.getKanbanStartDate(hover.item)
        );

        if (!startDate || !endDate) {
            alert('Timeline dates are not available.');
            return;
        }

        $scope.hierarchyEndDateEdit = {
            item: hover.item,
            type: hover.type,
            parent: hover.parent,
            grandparent: hover.grandparent,
            label: hover.label,
            startDate: cloneHierarchyDateOnly(startDate),
            oldEndDate: cloneHierarchyDateOnly(endDate),
            newEndDate: cloneHierarchyDateOnly(endDate),
            minDate: toServerDateOnly(startDate),
            saving: false
        };

        $scope.closeHierarchyTimelineHover();
        $timeout(function () {
            showKanbanPopup('hierarchyEndDateModal');
        }, 0);
    };

    function escapeHierarchyHtml(value) {
        return String(value === null || value === undefined ? '' : value)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#039;');
    }

    function getHierarchyEndDateValue(item, type) {
        if (!item) return null;
        return normalizeHierarchyDateOnly(
            String(type || '').toLowerCase() === 'milestone'
                ? (item.EndDate || item.TargetDate)
                : (item.Targetdate || item.TargetDate || item.EndDate)
        );
    }

    function getLatestHierarchyChildEnd(item, type) {
        var normalizedType = String(type || '').toLowerCase();
        var latest = null;

        function consider(child, childType) {
            if (!child) return;
            var childEnd = getHierarchyEndDateValue(child, childType);
            if (childEnd && (!latest || childEnd > latest.date)) {
                latest = {
                    date: childEnd,
                    label: $scope.getHierarchyItemLabel(childType, child)
                };
            }
        }

        if (normalizedType === 'task') {
            angular.forEach(item.subtasks || [], function (subtask) {
                consider(subtask, 'subtask');
            });
        } else if (normalizedType === 'milestone') {
            angular.forEach(item.tasks || [], function (task) {
                consider(task, 'task');
                angular.forEach(task.subtasks || [], function (subtask) {
                    consider(subtask, 'subtask');
                });
            });
        }

        return latest;
    }

    function addHierarchyExtensionEntry(plan, type, item, selectedEnd) {
        if (!item) return;
        var oldEnd = getHierarchyEndDateValue(item, type);
        if (!oldEnd) {
            throw new Error($scope.getHierarchyItemLabel(type, item) + ' end date is invalid.');
        }

        var isExtension = selectedEnd > oldEnd;
        plan.push({
            type: type,
            item: item,
            label: $scope.getHierarchyItemLabel(type, item),
            oldEnd: oldEnd,
            newEnd: cloneHierarchyDateOnly(isExtension ? selectedEnd : oldEnd),
            isExtension: isExtension,
            needsSave: isExtension
        });
    }

    function buildHierarchyEndDatePlan(edit, selectedEnd) {
        var type = String(edit.type || '').toLowerCase();

        if (type === 'subtask' && (!edit.parent || !edit.grandparent)) {
            throw new Error('Task or milestone hierarchy was not found for this subtask.');
        }
        if (type === 'task' && !edit.parent) {
            throw new Error('Milestone hierarchy was not found for this task.');
        }

        var plan = [{
            type: type,
            item: edit.item,
            label: $scope.getHierarchyItemLabel(type, edit.item),
            oldEnd: getHierarchyEndDateValue(edit.item, type),
            newEnd: cloneHierarchyDateOnly(selectedEnd),
            isCurrent: true,
            isExtension: selectedEnd > getHierarchyEndDateValue(edit.item, type),
            needsSave: true
        }];

        if (type === 'subtask') {
            addHierarchyExtensionEntry(plan, 'task', edit.parent, selectedEnd);
            addHierarchyExtensionEntry(plan, 'milestone', edit.grandparent, selectedEnd);
        } else if (type === 'task') {
            addHierarchyExtensionEntry(plan, 'milestone', edit.parent, selectedEnd);
        }

        var project = $scope.projectlist || {};
        var projectEnd = normalizeHierarchyDateOnly(project.TargetDate);
        if (!projectEnd) {
            throw new Error('Project end date is invalid.');
        }

        var projectIsExtended = selectedEnd > projectEnd;
        plan.push({
            type: 'project',
            item: project,
            label: project.ProjectName
                ? 'Project "' + project.ProjectName + '"'
                : 'Project',
            oldEnd: projectEnd,
            newEnd: cloneHierarchyDateOnly(projectIsExtended ? selectedEnd : projectEnd),
            isExtension: projectIsExtended,
            needsSave: projectIsExtended
        });

        return plan;
    }

    function hierarchyExtensionConfirmationHtml(plan) {
        var rows = '';
        angular.forEach(plan, function (entry) {
            rows += '<tr>' +
                '<td style="padding:6px 8px;text-align:left;border-bottom:1px solid #edf0f2;">' +
                escapeHierarchyHtml(entry.label) +
                (entry.isCurrent ? ' <span style="color:#6c757d;">(Selected)</span>' : '') +
                '</td>' +
                '<td style="padding:6px 8px;white-space:nowrap;border-bottom:1px solid #edf0f2;">' +
                escapeHierarchyHtml(formatDateOnly(entry.oldEnd)) +
                '</td>' +
                '<td style="padding:6px 8px;white-space:nowrap;font-weight:600;color:' +
                (entry.isCurrent || entry.isExtension ? '#c0392b' : '#198754') +
                ';border-bottom:1px solid #edf0f2;">' +
                escapeHierarchyHtml(formatDateOnly(entry.newEnd)) +
                (!entry.isCurrent && !entry.isExtension
                    ? ' <span style="font-weight:400;font-size:10px;">(No change)</span>'
                    : '') +
                '</td>' +
                '</tr>';
        });

        return '<div style="font-size:13px;margin-bottom:8px;">' +
            'The selected date exceeds the parent timeline. The following dates will be extended:' +
            '</div>' +
            '<div style="font-size:12px;color:#6c757d;margin-bottom:10px;">' +
            'Later subtasks, tasks and milestones will be re-sequenced from the previous completion date/time while keeping their existing duration days and working hours.' +
            '</div>' +
            '<div style="max-height:280px;overflow:auto;border:1px solid #e6e9ec;border-radius:6px;">' +
            '<table style="width:100%;border-collapse:collapse;font-size:12px;">' +
            '<thead><tr style="background:#f6f8fa;">' +
            '<th style="padding:7px 8px;text-align:left;">Hierarchy</th>' +
            '<th style="padding:7px 8px;">Existing</th>' +
            '<th style="padding:7px 8px;">New</th>' +
            '</tr></thead><tbody>' + rows + '</tbody></table></div>';
    }

    function confirmHierarchyEndDatePlan(plan) {
        var hasParentExtension = plan.some(function (entry) {
            return !entry.isCurrent && entry.isExtension;
        });

        if (!hasParentExtension) {
            return Promise.resolve(true);
        }

        if (window.Swal && Swal.fire) {
            return Swal.fire({
                title: 'Extend Timeline!!',
                html: hierarchyExtensionConfirmationHtml(plan),
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, extend',
                cancelButtonText: 'No',
                allowOutsideClick: false,
                allowEscapeKey: false,
                width: 650
            }).then(function (result) {
                return !!(result && result.isConfirmed);
            });
        }

        return Promise.resolve(window.confirm('The end date exceeds its parent timeline. Extend the parent dates also?'));
    }

    function parseHierarchyEndDateSaveResponse(response) {
        var raw = response && response.data ? response.data.d : null;
        if (raw === null || raw === undefined || raw === '' || raw === '0' || raw === 0) {
            throw new Error('Timeline date was not updated.');
        }

        var parts = String(raw).split('|');
        return {
            endDate: normalizeHierarchyDateOnly(parts[0]),
            days: parts.length > 1 && isFinite(Number(parts[1]))
                ? Number(parts[1])
                : null
        };
    }

    function applyHierarchyEndDateLocal(entry, saveResult) {
        var type = String(entry.type || '').toLowerCase();
        var item = entry.item;
        var startDate = cloneHierarchyDateOnly(
            (saveResult && saveResult.startDate) ||
            (type === 'project'
                ? (($scope.projectlist || {}).StartDate)
                : $scope.getKanbanStartDate(item))
        );
        var endDate = cloneHierarchyDateOnly(
            (saveResult && saveResult.endDate) || entry.newEnd
        );

        if (type === 'project') {
            if ($scope.projectlist) {
                if (startDate) {
                    $scope.projectlist.StartDate = toServerDateOnly(startDate);
                }
                $scope.projectlist.TargetDate = toServerDateOnly(endDate);
            }
            return;
        }

        if (type === 'milestone') {
            if (startDate) item.StartDate = cloneHierarchyDateOnly(startDate);
            item.EndDate = cloneHierarchyDateOnly(endDate);
            item._oldStartDate = angular.copy(item.StartDate);
            item._oldEndDate = angular.copy(item.EndDate);
        } else {
            if (startDate) item.Startdate = cloneHierarchyDateOnly(startDate);
            item.Targetdate = cloneHierarchyDateOnly(endDate);
            item._oldStartdate = angular.copy(item.Startdate);
            item._oldTargetdate = angular.copy(item.Targetdate);
        }

        if (saveResult && saveResult.days !== null && saveResult.days !== undefined) {
            item.txtdays = String(saveResult.days);
            item._oldtxtdays = angular.copy(item.txtdays);
        }

        if (saveResult && saveResult.startTime) {
            item.StartTime = saveResult.startTime;
            item.starttime = saveResult.startTime;
        }

        $scope.refreshHierarchyTimelineState(
            item,
            type,
            startDate || $scope.getKanbanStartDate(item),
            endDate
        );
    }

    function saveHierarchyEndDateEntry(entry) {
        debugger
        if (entry.type === 'project') {
            var project = $scope.projectlist || {};
            var projectId = project.id || project.ProjectId || project.projectid || 0;
            if (!projectId) {
                return Promise.reject(new Error('Project ID was not found.'));
            }

            return assignprojectService.extendProjectTimeline(
                projectId,
                toServerDateOnly(project.StartDate),
                toServerDateOnly(entry.newEnd),
                toServerDateOnly(entry.oldEnd)
            ).then(function (response) {
                var result = response && response.data ? response.data.d : null;
                if (result === null || result === undefined || result === '' || result === '0' || result === 0 || result === false) {
                    throw new Error('Project timeline was not extended.');
                }
                applyHierarchyEndDateLocal(entry, { endDate: entry.newEnd, days: null });
                return entry;
            });
        }

        return assignprojectService.updateHierarchyEndDate(
            entry.item.id,
            entry.type,
            toServerDateOnly(entry.newEnd)
        ).then(function (response) {
            var saveResult = parseHierarchyEndDateSaveResponse(response);
            applyHierarchyEndDateLocal(entry, saveResult);
            return refreshHierarchyLeftDays(entry.item, entry.type)
                .catch(function () { return null; });
        });
    }

    function getHierarchyPlanEntryId(entry) {
        if (!entry) return 0;
        var type = String(entry.type || '').toLowerCase();
        var item = entry.item || {};

        if (type === 'project') {
            var project = $scope.projectlist || item;
            return project.id || project.ProjectId || project.projectid || 0;
        }

        return item.id || 0;
    }

    function findHierarchyCascadeLocalItem(type, id) {
        var normalizedType = String(type || '').toLowerCase();
        var itemId = String(id || '0');

        if (normalizedType === 'project') {
            return $scope.projectlist || null;
        }

        var miles = ($scope.projectlist && $scope.projectlist.miles) || [];
        for (var mi = 0; mi < miles.length; mi++) {
            var milestone = miles[mi];
            if (normalizedType === 'milestone' && String(milestone.id) === itemId) {
                return milestone;
            }

            var tasks = milestone.tasks || [];
            for (var ti = 0; ti < tasks.length; ti++) {
                var task = tasks[ti];
                if (normalizedType === 'task' && String(task.id) === itemId) {
                    return task;
                }

                var subtasks = task.subtasks || [];
                for (var si = 0; si < subtasks.length; si++) {
                    if (normalizedType === 'subtask' && String(subtasks[si].id) === itemId) {
                        return subtasks[si];
                    }
                }
            }
        }

        return null;
    }

    function parseHierarchyCascadeSaveResponse(response) {
        var raw = response && response.data ? response.data.d : null;
        if (raw === null || raw === undefined || raw === '' || raw === '0' || raw === 0) {
            throw new Error('Hierarchy timeline was not extended.');
        }

        var text = String(raw);
        var firstPipe = text.indexOf('|');
        var status = firstPipe >= 0 ? text.substring(0, firstPipe) : text;
        if (status !== '1') {
            var serverMessage = firstPipe >= 0 ? text.substring(firstPipe + 1) : text;
            throw new Error(serverMessage || 'Hierarchy timeline was not extended.');
        }

        var result = {};
        var detailText = firstPipe >= 0 ? text.substring(firstPipe + 1) : '';
        angular.forEach(detailText ? detailText.split(';') : [], function (row) {
            if (!row) return;
            var values = row.split(',');
            if (values.length < 3) return;

            var type = String(values[0] || '').toLowerCase();
            var id = String(values[1] || '0');
            var endDate = normalizeHierarchyDateOnly(values[2]);
            var days = values.length > 3 && values[3] !== '' && isFinite(Number(values[3]))
                ? Number(values[3])
                : null;
            var startDate = values.length > 4
                ? normalizeHierarchyDateOnly(values[4])
                : null;
            var startTime = values.length > 5
                ? String(values[5] || '').trim()
                : '';

            result[type + ':' + id] = {
                type: type,
                id: id,
                startDate: startDate,
                endDate: endDate,
                days: days,
                startTime: startTime
            };
        });

        return result;
    }

    function saveHierarchyExtensionCascade(plan, currentEntry, saveOptions) {
        saveOptions = saveOptions || {};

        var currentType = String(currentEntry.type || '').toLowerCase();
        var currentId = getHierarchyPlanEntryId(currentEntry);
        var startDate = saveOptions.startDate || (currentType === 'project'
            ? (($scope.projectlist || {}).StartDate)
            : $scope.getKanbanStartDate(currentEntry.item));

        if (!currentId) {
            return Promise.reject(new Error(currentEntry.label + ' ID was not found.'));
        }

        return assignprojectService.extendHierarchyTimeline(
            currentId,
            toServerDateOnly(startDate),
            toServerDateOnly(currentEntry.newEnd),
            toServerDateOnly(currentEntry.oldEnd),
            currentType,
            saveOptions.durationDays,
            saveOptions.startTime,
            saveOptions.perdayhrs,
            saveOptions.changeSource || 'enddate'
        ).then(function (response) {
            var savedMap = parseHierarchyCascadeSaveResponse(response);
            var appliedKeys = {};
            var refreshChain = Promise.resolve();

            angular.forEach(savedMap, function (savedResult, key) {
                var entryType = String(savedResult.type || key.split(':')[0] || '').toLowerCase();
                var entryId = String(savedResult.id || key.split(':')[1] || '0');
                var localItem = findHierarchyCascadeLocalItem(entryType, entryId);

                if (!localItem && entryType !== 'project') return;

                applyHierarchyEndDateLocal({
                    type: entryType,
                    item: localItem || ($scope.projectlist || {}),
                    newEnd: savedResult.endDate
                }, savedResult);

                appliedKeys[entryType + ':' + entryId] = true;

                if (entryType !== 'project' && localItem) {
                    refreshChain = refreshChain.then(function () {
                        return refreshHierarchyLeftDays(localItem, entryType)
                            .catch(function () { return null; });
                    });
                }
            });

            // Backward compatibility for a server that returns only the original
            // selected/parent rows and no forward-cascade rows.
            angular.forEach(plan, function (entry) {
                if (!entry.isCurrent && !entry.needsSave) return;

                var entryType = String(entry.type || '').toLowerCase();
                var entryId = String(getHierarchyPlanEntryId(entry));
                var key = entryType + ':' + entryId;
                if (appliedKeys[key]) return;

                var savedResult = {
                    startDate: entryType === 'project'
                        ? normalizeHierarchyDateOnly(($scope.projectlist || {}).StartDate)
                        : normalizeHierarchyDateOnly($scope.getKanbanStartDate(entry.item)),
                    endDate: entry.newEnd,
                    days: null
                };

                applyHierarchyEndDateLocal(entry, savedResult);

                if (entryType !== 'project') {
                    refreshChain = refreshChain.then(function () {
                        return refreshHierarchyLeftDays(entry.item, entry.type)
                            .catch(function () { return null; });
                    });
                }
            });

            return refreshChain;
        });
    }

    function saveHierarchySelectedStartDateOnly(item, type, startDate) {
        var normalizedType = String(type || '').toLowerCase();
        var serverStartDate = toServerDateOnly(startDate);
        var startTime = item.StartTime || item.starttime || '';
        var request;

        if (!serverStartDate) {
            return Promise.reject(new Error('Selected start date is invalid.'));
        }

        if (normalizedType === 'milestone') {
            request = assignprojectService.updatetimeline_mile(
                item.id,
                item.txtdays,
                serverStartDate,
                startTime,
                $scope.perdayhrs,
                ''
            );
        } else if (normalizedType === 'task') {
            request = assignprojectService.updatetimeline_task(
                item.id,
                item.txtdays,
                serverStartDate,
                startTime,
                $scope.perdayhrs,
                ''
            );
        } else if (normalizedType === 'subtask') {
            request = assignprojectService.updatetimeline_subtask(
                item.id,
                item.txtdays,
                serverStartDate,
                startTime,
                $scope.perdayhrs,
                ''
            );
        } else {
            return Promise.reject(new Error('Invalid hierarchy type.'));
        }

        return request.then(function (response) {
            var raw = response && response.data ? response.data.d : null;
            if (raw === null || raw === undefined || raw === '' || raw === '0' || raw === 0) {
                throw new Error('Timeline date was not updated.');
            }

            if (String(raw).indexOf('ERROR|') === 0) {
                throw new Error(String(raw).substring(6) || 'Timeline date was not updated.');
            }

            var savedEndDate = hierarchyResponseDate(response);
            if (!savedEndDate) {
                throw new Error('Timeline date was not updated.');
            }

            if (normalizedType === 'milestone') {
                item.StartDate = cloneHierarchyDateOnly(startDate);
                item.EndDate = cloneHierarchyDateOnly(savedEndDate);
            } else {
                item.Startdate = cloneHierarchyDateOnly(startDate);
                item.Targetdate = cloneHierarchyDateOnly(savedEndDate);
            }

            $scope.refreshHierarchyTimelineState(
                item,
                normalizedType,
                startDate,
                savedEndDate
            );

            return {
                saved: true,
                cancelled: false,
                response: response
            };
        });
    }

    function saveHierarchyStartDateExtension(
        item,
        type,
        parent,
        grandparent,
        startDate,
        calculatedEndDate,
        changeSource
    ) {
        changeSource = String(changeSource || 'startdate').toLowerCase();
        var context = findHierarchyTimelineContext(
            item,
            type,
            parent,
            grandparent
        );

        var plan = buildHierarchyEndDatePlan({
            item: context.item,
            type: context.type,
            parent: context.parent,
            grandparent: context.grandparent
        }, calculatedEndDate);

        var hasParentExtension = plan.some(function (entry) {
            return !entry.isCurrent && entry.isExtension;
        });

        var currentEntry = null;
        angular.forEach(plan, function (entry) {
            if (entry.isCurrent) currentEntry = entry;
        });

        if (!currentEntry) {
            return Promise.reject(new Error('Selected hierarchy item was not found.'));
        }

        /*
         * Normal start-date or duration-days changes update only the selected
         * row. Forward hierarchy re-sequencing is allowed only when the new end
         * date exceeds a parent, the Extend Timeline popup is shown, and the
         * user confirms "Yes, extend".
         */
        if (!hasParentExtension) {
            return saveHierarchySelectedStartDateOnly(item, type, startDate);
        }

        return confirmHierarchyEndDatePlan(plan)
            .then(function (confirmed) {
                if (!confirmed) {
                    return {
                        saved: false,
                        cancelled: true,
                        response: null
                    };
                }

                /*
                 * Use the exact same confirmed hierarchy save path as
                 * saveHierarchyEndDate(). Only the change payload differs:
                 * Start Date and Duration Days must also persist the selected
                 * start date, duration days and start time before cascading.
                 */
                return saveHierarchyEndDatePlan(plan, {
                    startDate: startDate,
                    durationDays: item.txtdays,
                    startTime: item.StartTime || item.starttime || '',
                    perdayhrs: $scope.perdayhrs,
                    changeSource: changeSource
                }).then(function () {
                    var savedStartDate = normalizeHierarchyDateOnly(
                        $scope.getKanbanStartDate(item)
                    ) || startDate;
                    var savedEndDate = getHierarchyEndDateValue(item, type) ||
                        calculatedEndDate;

                    $scope.refreshHierarchyTimelineState(
                        item,
                        type,
                        savedStartDate,
                        savedEndDate
                    );

                    var refreshProjectDays = $scope.refreshProjectDurationDays
                        ? $scope.refreshProjectDurationDays()
                        : Promise.resolve();

                    return Promise.resolve(refreshProjectDays)
                        .catch(function () { return null; })
                        .then(function () {
                            return {
                                saved: true,
                                cancelled: false,
                                response: {
                                    data: {
                                        d: toServerDateOnly(savedEndDate)
                                    }
                                }
                            };
                        });
                });
            });
    }

    function saveHierarchyEndDatePlan(plan, saveOptions) {
        debugger
        saveOptions = saveOptions || {};
        var currentEntry = null;
        angular.forEach(plan, function (entry) {
            if (entry.isCurrent) currentEntry = entry;
        });

        var hasParentExtension = plan.some(function (entry) {
            return !entry.isCurrent && entry.isExtension;
        });

        // Forward cascade is permitted only after an exceeded parent produced
        // the Extend Timeline confirmation and the user clicked "Yes, extend".
        // Extending only the selected row inside its parent updates that row
        // normally and does not recalculate later siblings.
        if (currentEntry && hasParentExtension) {
            return saveHierarchyExtensionCascade(
                plan,
                currentEntry,
                saveOptions
            );
        }

        // Keep the existing reduction behaviour unchanged.
        var rank = { project: 0, milestone: 1, task: 2, subtask: 3 };
        var ordered = plan.slice().sort(function (a, b) {
            return rank[a.type] - rank[b.type];
        });
        var chain = Promise.resolve();

        angular.forEach(ordered, function (entry) {
            if (!entry.isCurrent && !entry.needsSave) return;
            chain = chain.then(function () {
                return saveHierarchyEndDateEntry(entry);
            });
        });

        return chain;
    }

    $scope.saveHierarchyEndDate = function () {
        debugger
        var edit = $scope.hierarchyEndDateEdit || {};
        if (edit.saving || !edit.item || !edit.type) return;
        if (!$scope.requireEdit(edit.type)) return;

        var startDate = normalizeHierarchyDateOnly(edit.startDate);
        var selectedEnd = normalizeHierarchyDateOnly(edit.newEndDate);

        if (!startDate || !selectedEnd) {
            alert('Please select a valid end date.');
            return;
        }

        if (selectedEnd < startDate) {
            alert(edit.label + ' end date cannot be before its start date.');
            return;
        }

        var latestChild = getLatestHierarchyChildEnd(edit.item, edit.type);
        if (latestChild && selectedEnd < latestChild.date) {
            var childMessage = latestChild.label +
                ' already ends on ' + formatDateOnly(latestChild.date) +
                '. ' + edit.label + ' cannot end before this date.';

            if (window.Swal && Swal.fire) {
                Swal.fire({
                    title: 'End Date Not Allowed',
                    text: childMessage,
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            } else {
                alert(childMessage);
            }
            return;
        }

        var currentEnd = getHierarchyEndDateValue(edit.item, edit.type);
        if (currentEnd && selectedEnd.getTime() === currentEnd.getTime()) {
            hideKanbanPopup('hierarchyEndDateModal');
            return;
        }

        var plan;
        try {
            plan = buildHierarchyEndDatePlan(edit, selectedEnd);
        } catch (planError) {
            alert(planError.message || 'Unable to validate hierarchy timeline.');
            return;
        }

        edit.saving = true;

        confirmHierarchyEndDatePlan(plan)
            .then(function (confirmed) {
                if (!confirmed) return false;
                return saveHierarchyEndDatePlan(plan).then(function () {
                    return true;
                });
            })
            .then(function (saved) {
                if (!saved) return;

                hideKanbanPopup('hierarchyEndDateModal');
                return $scope.refreshProjectDurationDays().then(function () {
                    if (window.Swal && Swal.fire) {
                        return Swal.fire({
                            title: 'Timeline Updated',
                            text: edit.label + ' end date has been updated successfully.',
                            icon: 'success',
                            timer: 1700,
                            showConfirmButton: false
                        });
                    }
                    return null;
                });
            })
            .catch(function (error) {
                debugger
                console.error('Hierarchy end-date update failed', error);
                if (window.Swal && Swal.fire) {
                    Swal.fire({
                        title: 'Timeline Not Updated',
                        text: error && error.message
                            ? error.message
                            : 'The hierarchy timeline could not be updated.',
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                } else {
                    alert(error && error.message
                        ? error.message
                        : 'The hierarchy timeline could not be updated.');
                }
            })
            .finally(function () {
                edit.saving = false;
                $scope.$applyAsync();
            });
    };

    function getKanbanMinutesFromDisplay(value) {
        if (value === null || value === undefined || value === '') return 0;

        var text = String(value)
            .toLowerCase()
            .replace(/hrs?/g, '')
            .trim();

        var separator = text.indexOf(':') >= 0 ? ':' : (text.indexOf('/') >= 0 ? '/' : null);
        if (separator) {
            var parts = text.split(separator);
            var hours = parseInt(parts[0] || '0', 10) || 0;
            var minutes = parseInt(parts[1] || '0', 10) || 0;
            return Math.max(0, (hours * 60) + minutes);
        }

        var decimalHours = parseFloat(text);
        return isNaN(decimalHours) ? 0 : Math.max(0, Math.round(decimalHours * 60));
    }

    $scope.getKanbanAllottedMinutes = function (item) {
        var duration = $scope.getKanbanDurationParts(item);
        return Math.max(0, (duration.hours * 60) + duration.minutes);
    };

    $scope.getKanbanAllottedPercent = function (item) {
        return $scope.getKanbanAllottedMinutes(item) > 0 ? 100 : 0;
    };

    $scope.getKanbanAllottedTooltip = function (item) {
        return 'Allotted Time: ' + ($scope.getKanbanDuration(item) || '0:00 hrs');
    };

    $scope.getKanbanSpentPercent = function (item) {
        // This existing call loads spent time only once and then returns the cached value.
        $scope.getKanbanDuration_spent(item);

        var allottedMinutes = $scope.getKanbanAllottedMinutes(item);
        var spentMinutes = getKanbanMinutesFromDisplay(item && item.itemduration);
        if (allottedMinutes <= 0 || spentMinutes <= 0) return 0;

        return clampKanbanPercent((spentMinutes / allottedMinutes) * 100);
    };

    $scope.getKanbanSpentTooltip = function (item) {
        var spentTime = $scope.getKanbanDuration_spent(item) || '0:00 hrs';
        return 'Spent Time: ' + spentTime +
            ' | Allotted Time: ' + ($scope.getKanbanDuration(item) || '0:00 hrs') +
            ' | Progress: ' + $scope.getKanbanSpentPercent(item) + '%';
    };

    $scope.getKanbanDurationParts = function (item) {
        var src = (item && (item.source || item)) || {};
        var rawHours = src.TaskDuration !== undefined && src.TaskDuration !== null
            ? src.TaskDuration
            : (src.taskduration !== undefined && src.taskduration !== null ? src.taskduration : '');
        var rawMinutes = src.TaskDuration_min !== undefined && src.TaskDuration_min !== null
            ? src.TaskDuration_min
            : (src.taskduration_min !== undefined && src.taskduration_min !== null ? src.taskduration_min : '');

        var hours = 0;
        var minutes = 0;
        var text = String(rawHours === undefined || rawHours === null ? '' : rawHours).trim();

        if (text.indexOf(':') >= 0 || text.indexOf('/') >= 0) {
            var splitChar = text.indexOf(':') >= 0 ? ':' : '/';
            var parts = text.split(splitChar);
            hours = parseInt(parts[0] || '0', 10) || 0;
            minutes = parseInt(parts[1] || '0', 10) || 0;
        } else {
            hours = parseInt(text || '0', 10) || 0;
            minutes = parseInt(rawMinutes || '0', 10) || 0;
        }

        return { hours: hours, minutes: minutes };
    };

    $scope.getKanbanDuration = function (item) {
        var duration = $scope.getKanbanDurationParts(item);
        if (!duration.hours && !duration.minutes) return '';
        return duration.hours + ':' + (duration.minutes < 10 ? '0' : '') + duration.minutes + ' hrs';
    };
    $scope.getKanbanDuration_spent = function (item) {
        if (!item) {
            return '0:00 hrs';
        }
        var ownerid = $scope.getKanbanassignto(item);
        
        if (item.id == "2364") {
            //debugger
        }
        if (!item.itemduration) {
            item.itemduration = '0:00 hrs';
        }

        if (!item.spentTimeLoaded && !item.spentTimeLoading) {
            item.spentTimeLoading = true;
           
            $scope.getspenttimeduration(
                item.id,
                item.tasktype,
                ownerid
            )
                .then(function (spentTime) {
                   
                    item.itemduration = spentTime || '0:00 hrs';
                    item.spentTimeLoaded = true;
                })
                .catch(function (error) {
                    alert(error);
                    item.itemduration = '0:00 hrs';
                    item.spentTimeLoaded = false;
                })
                .finally(function () {
                    item.spentTimeLoading = false;
                });
        }

        return item.itemduration;
    };
    $scope.getspenttimeduration = function (taskid, utype,owner) {
        
        return assignprojectService.gettimespent(taskid, utype, owner)
            .then(function (response) {
                if (taskid == "2364") {
                    debugger
                }
                var timeValue =
                    response &&
                        response.data &&
                        response.data.d
                        ? response.data.d
                        : '00:00:00:00';

                var parts = String(timeValue).split(':');

                var hrs = parseInt(parts[0], 10) || 0;
                var mins = parseInt(parts[1], 10) || 0;

                // Also manage when minutes exceed 59.
                hrs += Math.floor(mins / 60);
                mins = mins % 60;

                var formattedMinutes =
                    mins < 10 ? '0' + mins : String(mins);

                return hrs + ':' + formattedMinutes + ' hrs';
            })
            .catch(function (error) {
                debugger
                alert(error);
                console.error(
                    'Error getting spent-time duration:',
                    {
                        taskid: taskid,
                        tasktype: utype,
                        error: error
                    }
                );

                return '0:00 hrs';
            });
    };
    $scope.getKanbanDuration_spent_old = function (item) {
        var src = (item && (item.source || item)) || {};
        $scope.getspenttimeduration(src.id,src.tasktype);
    }
    $scope.getspenttimeduration_old = function (taskid, utype) {
        debugger
        return assignprojectService.gettimespent(taskid, utype)
            .then(function (response) {

                var timeValue = response &&
                    response.data &&
                    response.data.d
                    ? response.data.d
                    : "00:00:00:00";

                var parts = String(timeValue).split(':');

                var hrs = parseInt(parts[0], 10) || 0;
                var mins = parseInt(parts[1], 10) || 0;

                mins = mins < 10 ? '0' + mins : mins;

                return hrs + ':' + mins + ' hrs';
            })
            .catch(function (error) {

                console.error(
                    'Error getting spent-time duration:',
                    {
                        taskid: taskid,
                        tasktype: utype,
                        error: error
                    }
                );

                return '0:00 hrs';
            });
    };
   
    //$scope.getKanbanDuration = function (item) {
    //    debugger
    //    var src = (item && (item.source || item)) || {};
    //    return src.TaskDuration || src.taskduration || '';
    //};

    $scope.getKanbanOwnerId = function (item) {
        var src = (item && (item.source || item)) || {};
        // || src.assignto || src.AssignTo
        return src.owner || src.Owner || '';
    };
    $scope.getKanbanassignto = function (item) {
        var src = (item && (item.source || item)) || {};
        // || src.assignto || src.AssignTo
        return src.assignto || src.AssignTo || '0';
    };

    $scope.getKanbanOwnerName = function (item) {
        var src = (item && (item.source || item)) || {};
        var owners = src.owners || src.Owners || [];
        if (owners.length) return owners[0].name || owners[0].DisplayName || '';

        var ownerId = $scope.getKanbanOwnerId(item);
        if (ownerId !== undefined && ownerId !== null && String(ownerId).trim() !== '' && String(ownerId) !== '0') {
            var owner = ($scope.owners || []).find(function (x) {
                return String(x.id) === String(ownerId);
            });
            if (owner) return owner.name || owner.DisplayName || '';
        }
        return '';
    };
    $scope.getKanbanAssignName = function (item) {
        //debugger
        var ownerId = $scope.getKanbanassignto(item);
        if (ownerId !== undefined && ownerId !== null && String(ownerId).trim() !== '' && String(ownerId) !== '0') {
            var owner = ($scope.owners || []).find(function (x) {
                return String(x.id) === String(ownerId);
            });
            if (owner) return owner.name || owner.DisplayName || '';
        }
        return '';
    };
    $scope.getKanbanOwnerProfilenew = function (item) {
        var defaultImage = '/pms/Image/user.jpg';
        var src = (item && (item.source || item)) || {};
        var owners = src.owners || src.Owners || [];
        if (owners.length) return owners[0].name || owners[0].DisplayName || '';

        var ownerid = $scope.getKanbanOwnerId(item);
        try {
            if (angular.isArray(ownerid)) {
                ownerid = ownerid.length ? ownerid[0].id : '';
            }
            else if (angular.isObject(ownerid)) {
                return (ownerid.profileimage && ownerid.profileimage != '') ? ownerid.profileimage : defaultImage;
            }

            ownerid = String(ownerid || '').split(',')[0];
            if (!ownerid || !$scope.owners || !$scope.owners.length) {
                return defaultImage;
            }

            var owner = $scope.owners.find(function (o) {
                return String(o.id) == ownerid;
            });

            return (owner && owner.profileimage && owner.profileimage != '')
                ? owner.profileimage
                : defaultImage;
        }
        catch (e) {
            return defaultImage;
        }
    };
    $scope.prepareKanbanItems = function () {
        var items = [];

        angular.forEach(($scope.projectlist && $scope.projectlist.miles) || [], function (mile, mileIndex) {
            var milestoneName = mile.Milestone || mile.milestone || '';
            var tasks = mile.tasks || mile.Tasks || [];
            var milestoneSerial = String(mileIndex + 1);

            // serialNo is display-only. The original database id remains unchanged
            // and continues to be used by all existing Kanban actions.
            items.push({
                source: mile,
                id: mile.id,
                serialNo: milestoneSerial,
                tasktype: 'milestone',
                milestoneName: milestoneName,
                taskName: ''
            });

            angular.forEach(tasks, function (task, taskIndex) {
                var taskName = task.taskname || task.Taskname || task.TaskName || '';
                var subtasks = task.subtasks || task.Subtasks || task.subtasklist || [];
                var taskSerial = String(mileIndex + 1) + '.' + String(taskIndex + 1);

                items.push({
                    source: task,
                    id: task.id,
                    serialNo: taskSerial,
                    tasktype: 'task',
                    milestoneName: milestoneName,
                    taskName: taskName
                });

                angular.forEach(subtasks, function (subtask, subtaskIndex) {
                    items.push({
                        source: subtask,
                        id: subtask.id,
                        serialNo: taskSerial + '.' + String(subtaskIndex + 1),
                        tasktype: 'subtask',
                        milestoneName: milestoneName,
                        taskName: taskName
                    });
                });
            });
        });

        $scope.kanbanItems = items;
        $scope._kanbanProjectRef = $scope.projectlist;
    };

    $scope.ensureKanbanItems = function () {
        if ($scope._kanbanProjectRef !== $scope.projectlist) {
            $scope.prepareKanbanItems();
        }
    };

    $scope.filterKanbanByAssignView = function (item) {
        var view = String($scope.activetype || '0');
        var assigned = $scope.isDirectlyAssignedForView(item && (item.source || item));
        if (view === '0') return true;
        if (view === '1') return !assigned;
        if (view === '2') return assigned;
        return true;
    };

    $scope.getKanbanItemsByStatus = function (status) {
        $scope.ensureKanbanItems();
        return ($scope.kanbanItems || []).filter(function (item) {
            return String($scope.getKanbanStatusId(item)) === String(status.id) &&
                $scope.matchesTopLeafFilters(item && (item.source || item));
        });
    };

    $scope.getKanbanCount = function (status) {
        return $scope.getKanbanItemsByStatus(status).length;
    };

    // Keep List, Gantt and Kanban bound to the same project hierarchy.
    // Call this after any successful status, priority, assignment, removal or edit change.
    $scope.syncListAndKanbanViews = function () {
        // Force card wrappers to be rebuilt when hierarchy rows were added/removed/replaced.
        $scope._kanbanProjectRef = null;
        $scope.refreshTopFilterCounts();

        if ($scope.view === 'timesheet') {
            $scope.buildTimesheet();
        }

        if ($scope.view === 'calendar') {
            $scope.buildCalendar();
        }

        if ($scope.view === 'kanban') {
            $scope.prepareKanbanItems();

            $timeout(function () {
                if (window.bootstrap && bootstrap.Tooltip) {
                    var elements = document.querySelectorAll('#kanbanboard [data-bs-toggle="tooltip"]');
                    angular.forEach(elements, function (element) {
                        var oldTooltip = bootstrap.Tooltip.getInstance(element);
                        if (oldTooltip) oldTooltip.dispose();
                        new bootstrap.Tooltip(element);
                    });
                }

                // ng-repeat has rebuilt the cards; restore native Kanban drag/drop.
                initializeKanbanDragDrop();
            }, 0);
        }

        $scope.$applyAsync();
    };

    $scope.openKanbanView = function () {
        $scope.isLoading2 = true;

        // Build Kanban cards from the already loaded project hierarchy.
        $scope.prepareKanbanItems();

        $timeout(function () {
            // Re-initialize Bootstrap tooltips created by ng-repeat.
            if (window.bootstrap && bootstrap.Tooltip) {
                var elements = document.querySelectorAll('#kanbanboard [data-bs-toggle="tooltip"]');
                angular.forEach(elements, function (element) {
                    if (!bootstrap.Tooltip.getInstance(element)) {
                        new bootstrap.Tooltip(element);
                    }
                });
            }

            // Bind drag/drop only after Angular has rendered the status columns/cards.
            initializeKanbanDragDrop();

            $scope.isLoading2 = false;
        }, 0);
    };

    // Open the existing Assign Task modal from a Kanban card.
    // This reuses openAssignTask(), permissions, employee loading and timeline logic.
    $scope.openKanbanAssignTask = function (item, index, $event) {
        
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }

        if (!item || !item.source) return;

        $scope.openAssignTask(item.source, item.tasktype, index);
    };


    // Resolve the original hierarchy object and parent indexes for Kanban actions.
    $scope.getKanbanContext = function (item) {
        var src = (item && (item.source || item)) || {};
        var type = String((item && item.tasktype) || src.tasktype || '').toLowerCase();
        var result = { item: src, type: type, mile: null, task: null, mileIndex: -1, taskIndex: -1, subtaskIndex: -1 };
        var miles = (($scope.projectlist || {}).miles || []);

        angular.forEach(miles, function (mile, mi) {
            if (result.mileIndex !== -1 && type === 'milestone') return;

            if (type === 'milestone' && String(mile.id) === String(src.id)) {
                result.mile = mile;
                result.item = mile;
                result.mileIndex = mi;
                return;
            }

            var tasks = mile.tasks || [];
            angular.forEach(tasks, function (task, ti) {
                if (type === 'task' && String(task.id) === String(src.id)) {
                    result.mile = mile;
                    result.task = task;
                    result.item = task;
                    result.mileIndex = mi;
                    result.taskIndex = ti;
                    return;
                }

                if (type === 'subtask') {
                    var subs = task.subtasks || task.subtasklist || task.subtask || [];
                    angular.forEach(subs, function (sub, si) {
                        if (String(sub.id) === String(src.id)) {
                            result.mile = mile;
                            result.task = task;
                            result.item = sub;
                            result.mileIndex = mi;
                            result.taskIndex = ti;
                            result.subtaskIndex = si;
                        }
                    });
                }
            });
        });

        return result;
    };

    $scope.stopKanbanMenuClick = function ($event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
    };

    // Inline Kanban editors reuse the existing status/priority methods so all
    // current database updates, completion calculations and dependency checks stay unchanged.
    $scope.selectKanbanStatus = function (item, newStatus, $event) {
        $scope.stopKanbanMenuClick($event);

        var ctx = $scope.getKanbanContext(item);
        if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
            alert('Unable to identify this Kanban item.');
            return;
        }
        if (!newStatus || newStatus.id === undefined || newStatus.id === null || newStatus.id === '') {
            alert('Please select a valid status.');
            return;
        }
        if (String($scope.getKanbanStatusId(item)) === String(newStatus.id)) return;

        return $scope.selectStatus(ctx.item, newStatus, ctx.type);
    };


    // Native delegated drag/drop for the AngularJS Kanban board.
    // The DOM card is not moved manually. On drop we call the existing selectStatus(),
    // so milestone/task/subtask permissions, dependency checks, DB updates and completion
    // calculations continue to use the existing application flow.
    var kanbanDragState = {
        item: null,
        card: null,
        sourceStatusId: null
    };
    var kanbanSuppressClickUntil = 0;
    var kanbanPointerDownTarget = null;

    function kanbanMatches(element, selector) {
        if (!element || element.nodeType !== 1) return false;
        var fn = element.matches || element.msMatchesSelector || element.webkitMatchesSelector;
        return fn ? fn.call(element, selector) : false;
    }

    function closestKanbanElement(element, selector, stopElement) {
        var current = element;
        while (current && current !== stopElement && current.nodeType === 1) {
            if (kanbanMatches(current, selector)) return current;
            current = current.parentElement;
        }
        if (current && current === stopElement && kanbanMatches(current, selector)) return current;
        return null;
    }

    function getKanbanRepeatValue(element, propertyName, board) {
        var current = element;
        while (current && current !== board) {
            try {
                var elementScope = angular.element(current).scope();
                if (elementScope && elementScope[propertyName] !== undefined && elementScope[propertyName] !== null) {
                    return elementScope[propertyName];
                }
            } catch (ignoreScopeError) { }
            current = current.parentElement;
        }
        return null;
    }

    function ensureKanbanDragStyles() {
        if (document.getElementById('pms-kanban-native-drag-style')) return;

        var style = document.createElement('style');
        style.id = 'pms-kanban-native-drag-style';
        style.type = 'text/css';
        style.textContent =
            '#kanbanboard .pms-kanban-card[draggable="true"]{cursor:grab;}' +
            '#kanbanboard .pms-kanban-card.pms-kanban-dragging{opacity:.45;cursor:grabbing;}' +
            '#kanbanboard .pms-kanban-card.pms-kanban-saving{opacity:.65;pointer-events:none;}' +
            '#kanbanboard .pms-kanban-column.pms-kanban-drop-target .pms-kanban-body{' +
                'outline:2px dashed rgba(57,120,222,.55);outline-offset:-3px;background:rgba(57,120,222,.04);' +
            '}';
        document.head.appendChild(style);
    }

    function clearKanbanDropIndicators(board) {
        if (!board) return;
        angular.forEach(board.querySelectorAll('.pms-kanban-drop-target'), function (column) {
            column.classList.remove('pms-kanban-drop-target');
        });
    }

    function refreshKanbanDragElements(board) {
        if (!board) return;

        angular.forEach(board.querySelectorAll('.pms-kanban-column'), function (column) {
            column._pmsKanbanStatus = getKanbanRepeatValue(column, 'status', board);
        });

        angular.forEach(board.querySelectorAll('.pms-kanban-card'), function (card) {
            var cardItem = getKanbanRepeatValue(card, 'item', board);
            card._pmsKanbanItem = cardItem;

            var source = cardItem && (cardItem.source || cardItem);
            var type = cardItem && (cardItem.tasktype || (source && source.tasktype));
            var canDrag = !!(cardItem && source && source.id && type && $scope.canEdit(type, source));

            card.setAttribute('draggable', canDrag ? 'true' : 'false');
            if (canDrag) {
                card.setAttribute('aria-grabbed', 'false');
            } else {
                card.removeAttribute('aria-grabbed');
            }
        });
    }

    function resetKanbanDragState(board) {
        clearKanbanDropIndicators(board);
        if (kanbanDragState.card) {
            kanbanDragState.card.classList.remove('pms-kanban-dragging');
            kanbanDragState.card.setAttribute('aria-grabbed', 'false');
        }
        kanbanDragState.item = null;
        kanbanDragState.card = null;
        kanbanDragState.sourceStatusId = null;
    }

    function initializeKanbanDragDrop() {
        var board = document.getElementById('kanbanboard');
        if (!board) return;

        ensureKanbanDragStyles();
        refreshKanbanDragElements(board);

        // Event delegation means the handlers survive ng-repeat card movement/rebuilds.
        if (board._pmsKanbanDragDropBound) return;
        board._pmsKanbanDragDropBound = true;

        // Remember where the pointer started so menus/buttons inside a draggable card
        // keep their existing click behavior. dragstart.target is normally the card itself.
        board.addEventListener('mousedown', function (event) {
            kanbanPointerDownTarget = event.target;
        }, true);

        board.addEventListener('dragstart', function (event) {
            var card = closestKanbanElement(event.target, '.pms-kanban-card', board);
            if (!card || card.getAttribute('draggable') !== 'true') {
                event.preventDefault();
                return;
            }

            // Do not start card dragging from dropdowns, links, inputs or action buttons.
            var dragOrigin = kanbanPointerDownTarget && card.contains(kanbanPointerDownTarget)
                ? kanbanPointerDownTarget
                : event.target;
            var interactive = closestKanbanElement(
                dragOrigin,
                'a,button,input,select,textarea,.dropdown-menu,.pms-kanban-inline-editor',
                card
            );
            if (interactive && interactive !== card) {
                event.preventDefault();
                return;
            }

            var item = card._pmsKanbanItem || getKanbanRepeatValue(card, 'item', board);
            var source = item && (item.source || item);
            var type = item && (item.tasktype || (source && source.tasktype));
            if (!item || !source || !source.id || !type || !$scope.canEdit(type, source)) {
                event.preventDefault();
                return;
            }

            kanbanDragState.item = item;
            kanbanDragState.card = card;
            kanbanDragState.sourceStatusId = $scope.getKanbanStatusId(item);

            card.classList.add('pms-kanban-dragging');
            card.setAttribute('aria-grabbed', 'true');

            if (event.dataTransfer) {
                event.dataTransfer.effectAllowed = 'move';
                // Firefox requires data to be set before drag events continue.
                event.dataTransfer.setData(
                    'text/plain',
                    String(type) + ':' + String(source.id)
                );
            }
        });

        board.addEventListener('dragover', function (event) {
            if (!kanbanDragState.item) return;

            var column = closestKanbanElement(event.target, '.pms-kanban-column', board);
            if (!column) return;

            event.preventDefault();
            if (event.dataTransfer) event.dataTransfer.dropEffect = 'move';

            clearKanbanDropIndicators(board);
            column.classList.add('pms-kanban-drop-target');
        });

        board.addEventListener('dragleave', function (event) {
            var column = closestKanbanElement(event.target, '.pms-kanban-column', board);
            if (!column) return;
            if (event.relatedTarget && column.contains(event.relatedTarget)) return;
            column.classList.remove('pms-kanban-drop-target');
        });

        board.addEventListener('drop', function (event) {
            if (!kanbanDragState.item) return;

            var column = closestKanbanElement(event.target, '.pms-kanban-column', board);
            if (!column) {
                resetKanbanDragState(board);
                return;
            }

            event.preventDefault();
            event.stopPropagation();

            var targetStatus = column._pmsKanbanStatus || getKanbanRepeatValue(column, 'status', board);
            var droppedItem = kanbanDragState.item;
            var droppedCard = kanbanDragState.card;
            var sourceStatusId = kanbanDragState.sourceStatusId;

            resetKanbanDragState(board);
            kanbanSuppressClickUntil = Date.now() + 300;

            if (!targetStatus || targetStatus.id === undefined || targetStatus.id === null || targetStatus.id === '') {
                alert('Unable to identify the target Kanban status.');
                return;
            }

            // Dropping inside the same status column performs no DB call.
            if (String(sourceStatusId) === String(targetStatus.id)) return;

            var updateStatus = function () {
                var updatePromise = $scope.selectKanbanStatus(droppedItem, targetStatus);
                if (updatePromise && droppedCard) {
                    droppedCard.classList.add('pms-kanban-saving');
                    if (angular.isFunction(updatePromise.finally)) {
                        updatePromise.finally(function () {
                            droppedCard.classList.remove('pms-kanban-saving');
                        });
                    }
                }
            };

            if ($scope.$root && $scope.$root.$$phase) {
                updateStatus();
            } else {
                $scope.$applyAsync(updateStatus);
            }
        });

        board.addEventListener('dragend', function () {
            kanbanPointerDownTarget = null;
            resetKanbanDragState(board);
        });

        // Avoid opening the edit form from the synthetic click some browsers emit after a drop.
        board.addEventListener('click', function (event) {
            if (Date.now() >= kanbanSuppressClickUntil) return;
            var card = closestKanbanElement(event.target, '.pms-kanban-card', board);
            if (!card) return;
            event.preventDefault();
            event.stopPropagation();
        }, true);
    }

    $scope.selectKanbanPriority = function (item, newPriority, $event) {
        $scope.stopKanbanMenuClick($event);

        var ctx = $scope.getKanbanContext(item);
        if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
            alert('Unable to identify this Kanban item.');
            return;
        }
        if (!newPriority || newPriority.id === undefined || newPriority.id === null || newPriority.id === '') {
            alert('Please select a valid priority.');
            return;
        }
        if (String($scope.getKanbanPriorityId(item)) === String(newPriority.id)) return;

        return $scope.selectPriority(ctx.item, newPriority, ctx.type);
    };


    function showKanbanPopup(modalId) {
        var modalElement = document.getElementById(modalId);
        if (!modalElement) return;

        try {
            if (window.bootstrap && bootstrap.Modal) {
                bootstrap.Modal.getOrCreateInstance(modalElement).show();
            } else if (window.jQuery) {
                window.jQuery(modalElement).modal('show');
            }
        } catch (ex) {
            console.error('Unable to open Kanban popup', ex);
        }
    }

    function hideKanbanPopup(modalId) {
        var modalElement = document.getElementById(modalId);
        if (!modalElement) return;

        try {
            if (window.bootstrap && bootstrap.Modal) {
                bootstrap.Modal.getOrCreateInstance(modalElement).hide();
            } else if (window.jQuery) {
                window.jQuery(modalElement).modal('hide');
            }
        } catch (ex) {
            console.error('Unable to close Kanban popup', ex);
        }
    }

    $scope.openKanbanOwnerPopup = function (item, $event) {
        $scope.stopKanbanMenuClick($event);

        var ctx = $scope.getKanbanContext(item);
        if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
            alert('Unable to identify this Kanban item.');
            return;
        }
        if (!$scope.requireEdit(ctx.type)) return;

        var currentOwnerId = $scope.getKanbanOwnerId(item);
        $scope.kanbanOwnerEdit.wrapper = item;
        $scope.kanbanOwnerEdit.context = ctx;
        $scope.kanbanOwnerEdit.selectedOwnerId = currentOwnerId ? String(currentOwnerId) : '';
        $scope.kanbanOwnerEdit.originalOwnerId = currentOwnerId ? String(currentOwnerId) : '';
        $scope.kanbanOwnerEdit.searchText = '';
        $scope.kanbanOwnerEdit.saving = false;

        $timeout(function () {
            showKanbanPopup('kanbanOwnerModal');
        }, 0);
    };

    $scope.selectKanbanPopupOwner = function (owner, $event) {
        $scope.stopKanbanMenuClick($event);
        if (!owner || owner.id === undefined || owner.id === null) return;
        // Single owner only: selecting a new row replaces the previous selection.
        $scope.kanbanOwnerEdit.selectedOwnerId = String(owner.id);
    };

    $scope.getKanbanPopupSelectedOwner = function () {
        var selectedId = ($scope.kanbanOwnerEdit || {}).selectedOwnerId;
        return ($scope.owners || []).find(function (owner) {
            return String(owner.id) === String(selectedId);
        }) || null;
    };

    $scope.saveKanbanOwner = function () {
        var edit = $scope.kanbanOwnerEdit || {};
        var ctx = edit.context;
        var selectedOwnerId = String(edit.selectedOwnerId || '').trim();

        if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
            alert('Unable to identify this Kanban item.');
            return;
        }
        if (!$scope.requireEdit(ctx.type)) return;
        if (!selectedOwnerId || selectedOwnerId === '0') {
            alert('Please select one owner.');
            return;
        }

        var selectedOwner = $scope.getKanbanPopupSelectedOwner();
        if (!selectedOwner) {
            alert('Please select a valid owner.');
            return;
        }

        if (String(edit.originalOwnerId || '') === selectedOwnerId) {
            hideKanbanPopup('kanbanOwnerModal');
            return;
        }

        edit.saving = true;
        assignprojectService.updateKanbanOwner(ctx.item.id, selectedOwnerId, ctx.type)
            .then(function (response) {
                var result = response && response.data ? response.data.d : '0';
                if (String(result) !== '1') {
                    throw new Error(result || 'Owner update failed.');
                }

                if (ctx.type === 'milestone') {
                    ctx.item.Owner = selectedOwnerId;
                } else {
                    ctx.item.owner = selectedOwnerId;
                    ctx.item.Owner = selectedOwnerId;
                }

                // Keep exactly one owner in the hierarchy/card display.
                ctx.item.owners = [angular.copy(selectedOwner)];
                ctx.item.Owners = ctx.item.owners;
                edit.originalOwnerId = selectedOwnerId;

                hideKanbanPopup('kanbanOwnerModal');
                $scope.syncListAndKanbanViews();
            })
            .catch(function (error) {
                var message = error && error.message ? error.message : 'Owner update failed.';
                alert(message);
            })
            .finally(function () {
                edit.saving = false;
            });
    };
    $scope.openKanbanDurationPopup = function (item, $event) {
        $scope.stopKanbanMenuClick($event);

        var ctx = $scope.getKanbanContext(item);

        if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
            alert('Unable to identify this Kanban item.');
            return;
        }

        if (!$scope.requireEdit(ctx.type)) return;

        var duration = $scope.getKanbanDurationParts(ctx.item);

        $scope.kanbanDurationEdit.wrapper = item;
        $scope.kanbanDurationEdit.context = ctx;

        // input[type="number"] requires numbers, not strings.
        $scope.kanbanDurationEdit.hours =
            parseInt(duration.hours, 10) || 0;

        $scope.kanbanDurationEdit.minutes =
            parseInt(duration.minutes, 10) || 0;

        $scope.kanbanDurationEdit.originalHours =
            $scope.kanbanDurationEdit.hours;

        $scope.kanbanDurationEdit.originalMinutes =
            $scope.kanbanDurationEdit.minutes;

        $scope.kanbanDurationEdit.saving = false;

        $timeout(function () {
            showKanbanPopup('kanbanDurationModal');
        }, 0);
    };
    //$scope.openKanbanDurationPopup = function (item, $event) {
    //    $scope.stopKanbanMenuClick($event);

    //    var ctx = $scope.getKanbanContext(item);
    //    if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
    //        alert('Unable to identify this Kanban item.');
    //        return;
    //    }
    //    if (!$scope.requireEdit(ctx.type)) return;

    //    var duration = $scope.getKanbanDurationParts(ctx.item);
    //    $scope.kanbanDurationEdit.wrapper = item;
    //    $scope.kanbanDurationEdit.context = ctx;
    //    $scope.kanbanDurationEdit.hours = String(duration.hours || 0);
    //    $scope.kanbanDurationEdit.minutes = String(duration.minutes || 0);
    //    $scope.kanbanDurationEdit.originalHours = String(duration.hours || 0);
    //    $scope.kanbanDurationEdit.originalMinutes = String(duration.minutes || 0);
    //    $scope.kanbanDurationEdit.saving = false;

    //    $timeout(function () {
    //        showKanbanPopup('kanbanDurationModal');
    //    }, 0);
    //};

    $scope.saveKanbanDuration = function () {
        debugger
        var edit = $scope.kanbanDurationEdit || {};
        
        var ctx = edit.context;
        if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
            alert('Unable to identify this Kanban item.');
            return;
        }
        if (!$scope.requireEdit(ctx.type)) return;

        var hoursText = String(edit.hours === undefined || edit.hours === null ? '' : edit.hours).trim();
        var minutesText = String(edit.minutes === undefined || edit.minutes === null ? '' : edit.minutes).trim();
        if ((hoursText && !/^\d+$/.test(hoursText)) || (minutesText && !/^\d+$/.test(minutesText))) {
            alert('Duration accepts numbers only.');
            return;
        }

        var hours = parseInt(hoursText || '0', 10) || 0;
        var minutes = parseInt(minutesText || '0', 10) || 0;
        if (minutes < 0 || minutes >= 60) {
            alert('Minutes must be less than 60.');
            return;
        }

        var totalMinutes = (hours * 60) + minutes;
        if (totalMinutes <= 0) {
            alert('Duration must be greater than zero.');
            return;
        }

        var capacityMessage = getHierarchyDurationCapacityError(
            ctx.type,
            ctx.item,
            totalMinutes
        );
        if (capacityMessage) {
            alert(capacityMessage);
            return;
        }

        if ($scope.validateResizeDurationByParent &&
            !$scope.validateResizeDurationByParent(ctx.item.id, ctx.type, totalMinutes, true)) {
            alert($scope._lastResizeDurationValidationMessage || 'Duration exceeds the parent duration.');
            return;
        }

        if (String(edit.originalHours) === String(hours) && String(edit.originalMinutes) === String(minutes)) {
            hideKanbanPopup('kanbanDurationModal');
            return;
        }

        edit.saving = true;
        assignprojectService.updateKanbanDuration(ctx.item.id, hours, minutes, ctx.type, $scope.perdayhrs)
            .then(function (response) {
                var result = response && response.data ? response.data.d : '0';
                if (String(result) !== '1') {
                    throw new Error(result || 'Duration update failed.');
                }

                ctx.item.TaskDuration = String(hours);
                ctx.item.TaskDuration_min = String(minutes);
                ctx.item.taskduration = String(hours);
                ctx.item.taskduration_min = String(minutes);
                ctx.item.DurationMinutes = totalMinutes;
                ctx.item._oldTaskDuration = String(hours);
                ctx.item._oldTaskDuration_min = String(minutes);
                edit.originalHours = String(hours);
                edit.originalMinutes = String(minutes);

                hideKanbanPopup('kanbanDurationModal');
                $scope.syncListAndKanbanViews();
            })
            .catch(function (error) {
                var message = error && error.message ? error.message : 'Duration update failed.';
                alert(message);
            })
            .finally(function () {
                edit.saving = false;
            });
    };

    // Reuse the legacy dependency WebMethods and stored procedure, while
    // rendering the selection UI within this page for both List and Gantt.
    $scope.completionDependency = { open: false };
    var completionDependencyRequest = 0;
    function completionDependencyPost(method, body) {
        return $http.post('/pms/AddSTDependency.aspx/' + method, body)
            .then(function (response) { return response.data.d; });
    }
    function completionOptions(rows) {
        return (rows || []).filter(function (option) { return option && Number(option.Value) > 0; });
    }
    function completionHierarchy(id, type, item) {
        var path = { milestone: type === 'milestone' ? id : (item.Milestoneid || item.MilestonId || item.MilestoneId || 0),
            task: type === 'task' ? id : (type === 'subtask' ? (item.Task || item.task || item.TaskId || 0) : 0),
            subtask: type === 'subtask' ? id : 0 };
        angular.forEach((($scope.projectlist || {}).miles || []), function (milestone) {
            if (type === 'milestone' && String(milestone.id) === String(id)) path.milestone = milestone.id;
            angular.forEach(milestone.tasks || [], function (task) {
                if (type === 'task' && String(task.id) === String(id)) path.milestone = milestone.id;
                angular.forEach(task.subtasks || [], function (subtask) {
                    if (type === 'subtask' && String(subtask.id) === String(id)) {
                        path.milestone = milestone.id; path.task = task.id;
                    }
                });
            });
        });
        return path;
    }
    function completionLoadSubtasks(state, seedId) {
        var ids = state.selected.task.map(function (row) { return row.Value; }).join(',');
        var request = state.subtaskLoadRequest = (state.subtaskLoadRequest || 0) + 1;
        state.loadingSubtasks = !!ids;
        // Keep existing choices while another task is added. Once the new list
        // arrives, retain only selections that still belong to selected tasks.
        if (!ids) { state.selected.subtask = []; state.subtasks = []; return; }
        completionDependencyPost('bindsubtasks', { mprojectid: String(state.projectId), tid: ids, stid: '0' })
            .then(function (rows) {
                if (!state.open || request !== state.subtaskLoadRequest ||
                    ids !== state.selected.task.map(function (row) { return row.Value; }).join(',')) return;
                var choices = completionOptions(rows).filter(function (row) {
                    return state.type !== 'subtask' || String(row.Value) !== String(state.id);
                });
                var retained = state.selected.subtask.map(function (row) { return String(row.Value); });
                var seedIds = state.editSeed ? state.editSeed.subtask :
                    (state.type === 'subtask' ? [] : [String(seedId || 0)]);
                state.loadingSubtasks = false;
                state.subtasks = choices;
                state.selected.subtask = choices.filter(function (row) {
                    return retained.indexOf(String(row.Value)) >= 0 || seedIds.indexOf(String(row.Value)) >= 0;
                });
            }, function () { if (state.open && request === state.subtaskLoadRequest) { state.loadingSubtasks = false; state.error = 'Unable to load subtasks.'; } });
    }
    function completionLoadTasks(state, seedId) {
        var ids = state.selected.milestone.map(function (row) { return row.Value; }).join(',');
        var request = state.taskLoadRequest = (state.taskLoadRequest || 0) + 1;
        state.loadingTasks = !!ids;
        if (!ids) {
            ++state.subtaskLoadRequest;
            state.loadingSubtasks = false;
            state.selected.task = []; state.selected.subtask = [];
            state.tasks = []; state.subtasks = [];
            return;
        }
        completionDependencyPost('bindtasks', { mprojectid: String(state.projectId), mid: ids, tid: '0' })
            .then(function (rows) {
                if (!state.open || request !== state.taskLoadRequest ||
                    ids !== state.selected.milestone.map(function (row) { return row.Value; }).join(',')) return;
                var choices = completionOptions(rows);
                var retained = state.selected.task.map(function (row) { return String(row.Value); });
                var seedIds = state.editSeed ? state.editSeed.task : [String(seedId || 0)];
                state.loadingTasks = false;
                state.tasks = choices;
                state.selected.task = choices.filter(function (row) {
                    return retained.indexOf(String(row.Value)) >= 0 || seedIds.indexOf(String(row.Value)) >= 0;
                });
                completionLoadSubtasks(state, state.defaults.subtask);
            }, function () { if (state.open && request === state.taskLoadRequest) { state.loadingTasks = false; state.error = 'Unable to load tasks.'; } });
    }
    function completionLoadExisting(state) {
        state.loadingExisting = true;
        completionDependencyPost('bindtaskdata', { RecordId: String(state.id), RecordType: state.type })
            .then(function (html) {
                if (!state.open) return;
                var detached = document.createElement('div');
                detached.innerHTML = html || '';
                state.existing = Array.prototype.map.call(detached.querySelectorAll('tbody tr'), function (row) {
                    var cells = row.querySelectorAll('td');
                    var editLink = row.querySelector('a[onclick*="Binddetails"]');
                    var match = editLink && (editLink.getAttribute('onclick') || '').match(/Binddetails\((\d+)\)/);
                    var value = function (index) { return cells[index] ? (cells[index].textContent || '').replace(/\s+/g, ' ').trim() : ''; };
                    var dependencyNames = cells[6] ? (cells[6].innerText || cells[6].textContent || '').split(/\n|\s*[,;]\s*/).map(function (part) { return part.trim(); }).filter(Boolean) : [];
                    return cells.length >= 7 && match ? {
                        id: Number(match[1]), milestone: value(3), task: value(4),
                        subtask: value(5), name: value(6), names: dependencyNames.length ? dependencyNames : [value(6)]
                    } : null;
                }).filter(function (row) { return row && row.name; });
                // The legacy HTML concatenates multiple linked names. Read the
                // stored ID lists for each row and display each level separately.
                state.existing.forEach(function (existing) {
                    completionDependencyPost('Binddetail', { RecordId: existing.id }).then(function (xml) {
                        if (!state.open || state.existing.indexOf(existing) < 0) return;
                        var entry = new DOMParser().parseFromString(xml, 'text/xml').getElementsByTagName('Table')[0];
                        if (!entry) return;
                        var miles = (($scope.projectlist || {}).miles || []), tasks = [], subtasks = [];
                        miles.forEach(function (mile) { (mile.tasks || []).forEach(function (task) {
                            tasks.push(task); (task.subtasks || []).forEach(function (subtask) { subtasks.push(subtask); });
                        }); });
                        var levels = [
                            { key: 'dmilestoneid', label: 'Milestones', rows: miles, name: function (row) { return row.Milestone; } },
                            { key: 'dtaskid', label: 'Tasks', rows: tasks, name: function (row) { return row.TaskName || row.taskname; } },
                            { key: 'dsubtaskid', label: 'Subtasks', rows: subtasks, name: function (row) { return row.SubtaskName || row.subtaskname; } }
                        ];
                        function renderLinkedNames() {
                            existing.names = levels.map(function (level) {
                            var ids = completionCsv(entry, level.key).filter(function (id) { return id !== '0'; });
                            if (!ids.length) return '';
                            return level.label + ': ' + ids.map(function (id) {
                                var option = (level.options || []).find(function (row) { return String(row.Value) === String(id); });
                                var match = level.rows.find(function (row) { return String(row.id) === String(id); });
                                return option ? option.Text : (match ? (level.name(match) || '#' + id) : '#' + id);
                            }).join(', ');
                            }).filter(Boolean);
                        }
                        renderLinkedNames();
                        // Some subtask arrays are not populated in the current
                        // view. Resolve their saved IDs from the same project
                        // dropdown endpoint used by the editor.
                        var taskIds = completionCsv(entry, 'dtaskid').filter(function (id) { return id !== '0'; });
                        if (taskIds.length && completionCsv(entry, 'dsubtaskid').length) {
                            completionDependencyPost('bindsubtasks', {
                                mprojectid: String(state.projectId), tid: taskIds.join(','), stid: '0'
                            }).then(function (options) {
                                if (!state.open || state.existing.indexOf(existing) < 0) return;
                                levels[2].options = completionOptions(options);
                                renderLinkedNames();
                            });
                        }
                    });
                });
            }, function () { if (state.open) state.error = 'Unable to load existing dependencies.'; })
            .finally(function () { state.loadingExisting = false; });
    }
    $scope.showCompletionDependencyPopup = function (id, type, projectId, name, item) {
        var request = ++completionDependencyRequest;
        $scope.completionDependency.open = false;
        var defaults = completionHierarchy(id, type, item || {});
        var state = $scope.completionDependency = {
            open: true, mode: 'list', id: id, type: type, projectId: projectId,
            name: name, menu: '', search: {}, selected: { milestone: [], task: [], subtask: [] },
            defaults: defaults, milestones: [], tasks: [], subtasks: [], existing: [],
            loadingExisting: false, error: '', notice: '', saving: false
        };
        completionLoadExisting(state);
        completionDependencyPost('bindmilestonedata', { mprojectid: String(projectId), mid: '0' })
            .then(function (rows) {
                if (request !== completionDependencyRequest) return;
                state.milestones = completionOptions(rows);
                var own = state.milestones.find(function (row) { return String(row.Value) === String(defaults.milestone); });
                if (own) { state.selected.milestone = [own]; completionLoadTasks(state, defaults.task); }
            }, function () {
                if (request === completionDependencyRequest) state.error = 'Unable to load milestones.';
            });
    };
    $scope.closeCompletionDependencyPopup = function () {
        ++completionDependencyRequest;
        $scope.completionDependency.open = false;
    };
    function completionCsv(xmlRow, name) {
        var node = xmlRow.getElementsByTagName(name)[0];
        return node && node.textContent ? node.textContent.split(',').map(function (part) { return part.trim(); }).filter(Boolean) : [];
    }
    $scope.addCompletionDependency = function () {
        $scope.resetCompletionDependencyEdit();
        $scope.completionDependency.mode = 'add';
    };
    $scope.backToCompletionDependencyList = function () {
        var state = $scope.completionDependency;
        state.mode = 'list'; state.menu = ''; state.error = '';
        state.editingId = 0; state.editSeed = null;
    };
    $scope.editCompletionDependency = function (existing) {
        var state = $scope.completionDependency;
        state.error = ''; state.notice = '';
        completionDependencyPost('Binddetail', { RecordId: Number(existing.id) }).then(function (xml) {
            if (!state.open) return;
            var parsed = new DOMParser().parseFromString(xml, 'text/xml');
            var entry = parsed.getElementsByTagName('Table')[0];
            if (!entry || completionCsv(entry, 'id')[0] !== String(existing.id)) {
                state.error = 'Unable to load dependency details.'; return;
            }
            state.editingId = Number(existing.id);
            state.editSeed = { milestone: completionCsv(entry, 'dmilestoneid'),
                task: completionCsv(entry, 'dtaskid'), subtask: completionCsv(entry, 'dsubtaskid') };
            state.selected = { milestone: [], task: [], subtask: [] };
            state.selected.milestone = state.milestones.filter(function (row) {
                return state.editSeed.milestone.indexOf(String(row.Value)) >= 0;
            });
            state.menu = '';
            completionLoadTasks(state, 0);
            state.mode = 'edit';
        }, function () { state.error = 'Unable to load dependency details.'; });
    };
    $scope.resetCompletionDependencyEdit = function () {
        var state = $scope.completionDependency;
        state.editingId = 0; state.editSeed = null;
        state.error = ''; state.notice = '';
        state.selected = { milestone: [], task: [], subtask: [] };
        var own = state.milestones.find(function (row) { return String(row.Value) === String(state.defaults.milestone); });
        if (own) { state.selected.milestone = [own]; completionLoadTasks(state, state.defaults.task); }
    };
    $scope.deleteCompletionDependency = function (existing) {
        var state = $scope.completionDependency;
        if (!window.confirm('Delete this dependency?')) return;
        state.error = ''; state.notice = '';
        completionDependencyPost('DeleteDependency', { RecordId: Number(existing.id) }).then(function (result) {
            if (!state.open) return;
            if (String(result) !== '1') { state.error = 'Dependency could not be deleted.'; return; }
            state.notice = 'Dependency deleted.';
            if (state.editingId === existing.id) $scope.resetCompletionDependencyEdit();
            completionLoadExisting(state);
        }, function () { state.error = 'Dependency could not be deleted.'; });
    };
    $scope.completionDependencySelected = function (level, id) {
        return ($scope.completionDependency.selected[level] || []).some(function (row) { return String(row.Value) === String(id); });
    };
    $scope.completionDependencyOutsideClick = function (event) {
        if (!event.target.closest('.pms-completion-picker')) {
            $scope.completionDependency.menu = '';
        }
    };
    $scope.completionDependencyLabel = function (level) {
        var rows = $scope.completionDependency.selected[level] || [];
        return rows.length ? rows.map(function (row) { return row.Text; }).join(', ') :
            'Select ' + (level === 'subtask' ? 'subtasks' : level + 's') + ' from this project';
    };
    $scope.toggleCompletionDependency = function (level, option) {
        var state = $scope.completionDependency;
        var selected = state.selected[level];
        var index = selected.findIndex(function (row) { return String(row.Value) === String(option.Value); });
        var removing = index >= 0;
        if (!removing) selected.push(option); else selected.splice(index, 1);
        state.search[level] = '';
        state.error = '';
        if (level === 'milestone') {
            state.editSeed = null;
            completionLoadTasks(state, removing ? 0 : state.defaults.task);
        } else if (level === 'task') {
            state.editSeed = null;
            completionLoadSubtasks(state, removing ? 0 : state.defaults.subtask);
        }
    };
    $scope.saveCompletionDependency = function () {
        var state = $scope.completionDependency;
        if (!state.selected.milestone.length) { state.error = 'Select at least one milestone.'; return; }
        var selectedIds = function (level) { return state.selected[level].map(function (row) { return String(row.Value); }); };
        var explicit = ['milestone', 'task', 'subtask'].some(function (level) {
            return selectedIds(level).some(function (value) { return value !== String(state.defaults[level] || 0); });
        });
        if (!explicit && !state.editingId) { state.error = 'Select another milestone, task, or subtask as the dependency.'; return; }
        state.saving = true; state.error = ''; state.notice = '';
        var milestones = selectedIds('milestone');
        var tasks = selectedIds('task');
        var subtasks = selectedIds('subtask');
        // Preselected ancestors navigate the picker; include one only when a
        // chosen child belongs to it, so it cannot become a self dependency.
        var explicitTasks = tasks.filter(function (value) { return value !== String(state.defaults.task || 0); });
        var explicitSubtasks = subtasks.filter(function (value) { return value !== String(state.defaults.subtask || 0); });
        var childInOwnTask = false, childInOwnMilestone = false;
        angular.forEach((($scope.projectlist || {}).miles || []), function (milestone) {
            if (String(milestone.id) !== String(state.defaults.milestone)) return;
            angular.forEach(milestone.tasks || [], function (task) {
                if (explicitTasks.indexOf(String(task.id)) >= 0) childInOwnMilestone = true;
                angular.forEach(task.subtasks || [], function (subtask) {
                    if (explicitSubtasks.indexOf(String(subtask.id)) < 0) return;
                    childInOwnMilestone = true;
                    if (String(task.id) === String(state.defaults.task)) childInOwnTask = true;
                });
            });
        });
        if (!childInOwnMilestone) milestones = milestones.filter(function (value) { return value !== String(state.defaults.milestone || 0); });
        if (!childInOwnTask) tasks = tasks.filter(function (value) { return value !== String(state.defaults.task || 0); });
        subtasks = explicitSubtasks;
        if (!milestones.length && (tasks.length || subtasks.length)) {
            // Legacy sp_dependencydetails joins tasks through their milestones.
            milestones = [String(state.defaults.milestone)];
        }
        var details = { id: String(state.editingId || 0), mprojectid: String(state.projectId), did: String(state.id), dtype: state.type,
            pid: '0', mid: state.type === 'milestone' ? String(state.id) : '0',
            tid: state.type === 'task' ? String(state.id) : '0',
            stid: state.type === 'subtask' ? String(state.id) : '0',
            dmid: milestones.join(','), dtaskid: tasks.join(','), dsubtaskid: subtasks.join(',') };
        completionDependencyPost('SaveeDependency', { mdetails: details }).then(function (result) {
            if (String(result) === '0') { state.error = 'Dependency could not be saved.'; return; }
            state.notice = 'Dependency saved.';
            state.mode = 'list'; state.menu = '';
            state.editingId = 0; state.editSeed = null;
            state.selected = { milestone: [], task: [], subtask: [] };
            state.tasks = []; state.subtasks = [];
            var own = state.milestones.find(function (row) { return String(row.Value) === String(state.defaults.milestone); });
            if (own) { state.selected.milestone = [own]; completionLoadTasks(state, state.defaults.task); }
            completionLoadExisting(state);
        }, function (error) {
            state.error = (error.data && error.data.Message) || 'Dependency could not be saved.';
        }).finally(function () { state.saving = false; });
    };

    // List, Kanban and Gantt share the same predecessor/successor popup and
    // Gantt dependency records. Keep each view's own action menu intact.
    $scope.closeListActionDropdown = function ($event) {
        if (!$event) return;
        var menu = $event.target.closest('.dropdown-menu');
        var trigger = menu && menu.previousElementSibling;
        if (trigger && window.bootstrap && bootstrap.Dropdown) {
            var dropdown = bootstrap.Dropdown.getInstance(trigger);
            if (dropdown) dropdown.hide();
        }
        $event.stopPropagation();
    };
    $scope.openListCompletionDependency = function (item, type, $event) {
        if ($event) { $event.preventDefault(); $scope.closeListActionDropdown($event); }
        if (!item || !item.id) return;
        var projectId = item.ProjectId || (($scope.projectlist || {}).id) || 0;
        var rows = ($scope.buildGanttRows && $scope.projectlist) ? $scope.buildGanttRows() : [];
        $scope.originalRows = rows;
        var row = rows.find(function (candidate) {
            return candidate.type === type && String(candidate.id) === String(item.id);
        });
        if (!row && $scope.makeGanttRow) {
            row = $scope.makeGanttRow(item, type, 0, null);
            row.serialNo = item.orderno || item.serialNo || '';
        }
        if (!row) return;
        row.ProjectId = row.ProjectId || projectId;
        row.name = row.name || item.Milestone || item.taskname || item.TaskName ||
            item.Subtaskname || item.SubtaskName || item.subtaskname || item.name || 'item';
        $scope.ganttSuppressPopupUntil = 0;
        $scope.openGanttSubtaskPopup(row, $event);
    };

    $scope.openKanbanDependency = function (item, $event) {
        var ctx = $scope.getKanbanContext(item);
        var src = ctx.item;
        var type = ctx.type;
        if (!src || !src.id || (type !== 'milestone' && type !== 'task' && type !== 'subtask')) return;
        $scope.openListCompletionDependency(src, type, $event);
    };

    $scope.openKanbanIssue = function (item, $event) {
        if ($event) { $event.preventDefault(); $scope.closeListActionDropdown($event); }
        var ctx = $scope.getKanbanContext(item);
        $scope.openIssueList(ctx.item, ctx.type, $scope.getKanbanItemTitle(item));
    };

    // All three views share the existing issue database endpoint and the
    // existing issue editor. The list is the first screen in this page.
    $scope.issuePanel = { open: false, mode: 'list', rows: [] };
    var issuePanelRequest = 0;
    function issueContext(item, type, title) {
        var path = completionHierarchy(item.id, type, item);
        var projectId = item.ProjectId || item.projectid || (($scope.projectlist || {}).id) || 0;
        var mile = (($scope.projectlist || {}).miles || []).find(function (row) { return String(row.id) === String(path.milestone); });
        var task = mile && (mile.tasks || []).find(function (row) { return String(row.id) === String(path.task); });
        var name = title || item.Milestone || item.TaskName || item.Subtaskname || item.SubtaskName || item.taskname || item.subtaskname || 'Item';
        var breadcrumb = [mile && mile.Milestone, task && (task.taskname || task.TaskName), type === 'subtask' && name].filter(Boolean).join(' / ') || name;
        return { item: item, type: type, title: name, breadcrumb: breadcrumb,
            projectId: projectId, milestoneId: path.milestone || 0, taskId: path.task || 0, subtaskId: path.subtask || 0 };
    }
    $scope.openIssueList = function (item, type, title, $event) {
        if ($event) { $event.preventDefault(); $scope.closeListActionDropdown($event); }
        if (!item || !item.id) return;
        ++issuePanelRequest;
        $scope.issuePanel = { open: true, mode: 'list', rows: [], loading: true, error: '', search: '', page: 1, pageSize: 10,
            context: issueContext(item, type, title) };
        $scope.refreshIssueList();
    };
    $scope.refreshIssueList = function () {
        var state = $scope.issuePanel;
        if (!state.open) return;
        var request = ++issuePanelRequest, context = state.context;
        state.loading = true; state.error = '';
        return $http.post('/pms/Addissues.aspx/bindIssuesdata', {
            RecordId: String(context.projectId), milstoneid: String(context.milestoneId),
            taskid: String(context.taskId), subtaskid: String(context.subtaskId),
            issuetype: context.type === 'milestone' ? 'milstone' : context.type
        }).then(function (response) {
            if (request !== issuePanelRequest || !state.open) return;
            var detached = document.createElement('div');
            detached.innerHTML = response.data.d || '';
            state.rows = Array.prototype.map.call(detached.querySelectorAll('tbody tr'), function (tr) {
                var cells = tr.querySelectorAll('td');
                if (cells.length < 10) return null;
                var text = function (n) { return cells[n] ? (cells[n].textContent || '').replace(/\s+/g, ' ').trim() : ''; };
                // The legacy cells contain a hidden Bootstrap dropdown with
                // every possible choice. Read only the visible selected badge.
                var selected = function (n, prefix) {
                    var badge = cells[n] && cells[n].querySelector('a[id^="' + prefix + '"]');
                    return badge ? (badge.textContent || '').replace(/\s+/g, ' ').trim() : text(n);
                };
                var people = function (n) {
                    if (!cells[n]) return [];
                    var names = [];
                    Array.prototype.forEach.call(cells[n].querySelectorAll('.avatar-group .avatar-group-item'), function (anchor) {
                        var overflow = anchor.getAttribute('data-bs-original-title') === 'more';
                        var value = overflow ? anchor.getAttribute('title') :
                            (anchor.getAttribute('data-bs-original-title') || anchor.getAttribute('title'));
                        if (!value) return;
                        (overflow ? value.split(',') : [value]).forEach(function (name) {
                            name = name.replace(/\s+/g, ' ').trim();
                            if (name && name.toLowerCase() !== 'more' && names.indexOf(name) === -1) names.push(name);
                        });
                    });
                    return names;
                };
                var link = Array.prototype.find.call(tr.querySelectorAll('a[onclick]'), function (anchor) {
                    return /\bBinddetails\(\d+\)/.test(anchor.getAttribute('onclick') || '');
                });
                var deleteLink = tr.querySelector('a[onclick*="deletedata("]');
                var match = link && (link.getAttribute('onclick') || '').match(/Binddetails\((\d+)\)/);
                var owners = people(2), teams = people(3);
                return { id: match ? Number(match[1]) : null, canEdit: !!link && link.style.display !== 'none',
                    canDelete: !!deleteLink && deleteLink.style.display !== 'none', number: text(0), name: text(1), owner: owners.join(', '), owners: owners,
                    team: teams.join(', '), teams: teams, status: selected(4, 'btnsts_'), priority: selected(5, 'btnpri_'), assignee: text(7),
                    created: text(8), target: text(9), issueType: selected(10, 'btnissutype_'), milestone: text(12) };
            }).filter(Boolean);
            state.page = 1;
        }, function () { if (request === issuePanelRequest) state.error = 'Unable to load issues. Please try again.'; })
        .finally(function () { if (request === issuePanelRequest) state.loading = false; });
    };
    $scope.issueFilteredRows = function () {
        var state = $scope.issuePanel, needle = (state.search || '').toLowerCase();
        return (state.rows || []).filter(function (row) {
            return !needle || [row.name, row.owner, row.team, row.status, row.priority, row.assignee, row.issueType].join(' ').toLowerCase().indexOf(needle) !== -1;
        });
    };
    $scope.issuePageRows = function () {
        var state = $scope.issuePanel, rows = $scope.issueFilteredRows();
        return rows.slice((state.page - 1) * state.pageSize, state.page * state.pageSize);
    };
    $scope.issueFormOptions = { employees: [], statuses: [], priorities: [], types: [], replicabilities: [] };
    // Owner, Team and Assign To use only people attached to the selected
    // milestone/task/subtask. Keep the complete employee master list for the
    // unrelated reminder notification field.
    $scope.issueEmployeeOptions = [];
    $scope.issueMulti = { open: '', active: 0, search: { Owner: '', team: '', AssignTo: '' } };
    function newIssueForm() {
        return { Issues: '', Description: '', Owner: [], team: [], AssignTo: '', StartDate: null, EndDate: null,
            Reminder: '0', ReminderType: '', Days: null, Time: null, Notifyusers: '', ChooseDate: null,
            IssueStatus: '', Priority: '', IssueType: '', Replicability: '', UploadImage: '' };
    }
    $scope.issueForm = newIssueForm();
    function findIssueHierarchyItem(context) {
        if (!context) return null;
        var milestones = ($scope.projectlist && $scope.projectlist.miles) || [];
        var milestone = milestones.find(function (row) {
            return String(row.id) === String(context.milestoneId);
        });
        if (context.type === 'milestone') return milestone || context.item || null;
        var task = milestone && (milestone.tasks || []).find(function (row) {
            return String(row.id) === String(context.taskId);
        });
        if (context.type === 'task') return task || context.item || null;
        var subtask = task && (task.subtasks || []).find(function (row) {
            return String(row.id) === String(context.subtaskId);
        });
        return subtask || context.item || null;
    }
    function getIssueTeamEmployeeIds(context) {
        var selected = findIssueHierarchyItem(context);
        var sourceItems = [selected, context && context.item];
        var ids = [];
        function add(value) {
            if (value === null || value === undefined) return;
            if (angular.isArray(value)) {
                value.forEach(add);
                return;
            }
            if (angular.isObject(value)) {
                add(value.id || value.empid || value.employeeid || value.ownerid || value.EmployeeId);
                return;
            }
            String(value).split(/[,;|]/).forEach(function (part) {
                var id = part.trim();
                if (id && id !== '0' && ids.indexOf(id) === -1) ids.push(id);
            });
        }
        sourceItems.forEach(function (item) {
            if (!item) return;
            add(item.team);
            add(item.Team);
            add(item.teams);
            add(item.Teams);
        });
        return ids;
    }
    function bindIssueEmployeeOptions() {
        var allowedIds = getIssueTeamEmployeeIds($scope.issuePanel.context);
        $scope.issuePanel.teamEmployeeIds = allowedIds;
        $scope.issueEmployeeOptions = ($scope.issueFormOptions.employees || []).filter(function (person) {
            return allowedIds.indexOf(String(person.id)) !== -1;
        });
    }
    function loadIssueOptions() {
        if ($scope.issueFormOptions.employees.length) {
            bindIssueEmployeeOptions();
            return Promise.resolve();
        }
        $scope.issuePanel.loadingOptions = true;
        return Promise.all([
            $http.post('/pms/Addissues.aspx/GetIssueFormOptions', {}),
            $http.post('/pms/assignprojects/default.aspx/bindIssueStatusOptions', {})
        ]).then(function (responses) {
            $scope.issueFormOptions = responses[0].data.d || $scope.issueFormOptions;
            $scope.issueFormOptions.statuses = responses[1].data.d || [];
            bindIssueEmployeeOptions();
        }, function () {
            throw new Error('Unable to load issue options. Please try again.');
        }).finally(function () { $scope.issuePanel.loadingOptions = false; });
    }
    $scope.openIssueEditor = function () {
        $scope.issuePanel.mode = 'add';
        $scope.issuePanel.formError = '';
        $scope.issueForm = newIssueForm();
        $scope.issueMulti.open = '';
        $scope.issueMulti.search = { Owner: '', team: '', AssignTo: '' };
        loadIssueOptions().catch(function (error) { $scope.issuePanel.formError = error.message; });
    };
    function issueXmlField(record, name) {
        var node = Array.prototype.find.call(record.children, function (child) {
            return child.localName.toLowerCase() === name.toLowerCase();
        });
        return node ? node.textContent.trim() : '';
    }
    function issueFormDate(value) {
        var match = (value || '').match(/^(\d{4})-(\d\d)-(\d\d)/);
        return match ? new Date(Date.UTC(+match[1], +match[2] - 1, +match[3])) : null;
    }
    function issueFormTime(value) {
        var match = (value || '').match(/(?:T|^)(\d\d):(\d\d)/);
        return match ? new Date(Date.UTC(1970, 0, 1, +match[1], +match[2])) : null;
    }
    $scope.editIssue = function (issue) {
        if (!issue.id || !issue.canEdit) return;
        var state = $scope.issuePanel;
        state.mode = 'add'; state.formError = ''; state.loadingOptions = true;
        $scope.issueForm = newIssueForm();
        $scope.issueMulti.open = '';
        $scope.issueMulti.search = { Owner: '', team: '', AssignTo: '' };
        Promise.all([loadIssueOptions(), $http.post('/pms/Addissues.aspx/Bindissuesdetail', { RecordId: issue.id })])
            .then(function (results) {
                if (!state.open) return;
                var doc = new DOMParser().parseFromString(results[1].data.d || '', 'application/xml');
                var record = doc.documentElement && doc.documentElement.firstElementChild;
                if (!record || doc.querySelector('parsererror')) throw new Error('Unable to read issue details.');
                var get = function (name) { return issueXmlField(record, name); };
                var multi = function (name) { return get(name).split(',').map(function (id) { return id.trim(); }).filter(Boolean); };
                $scope.$evalAsync(function () {
                    $scope.issueForm = {
                        id: String(issue.id), Issues: get('Issues'), Description: get('Description'), Owner: multi('Owner').slice(0, 1), team: multi('team'),
                        AssignTo: get('AssignTo'), StartDate: issueFormDate(get('StartDate')), EndDate: issueFormDate(get('EndDate')),
                        Reminder: get('Reminder') || '0', ReminderType: get('ReminderType'), Days: get('Days'),
                        Time: issueFormTime(get('Time')), Notifyusers: get('Notifyusers'), ChooseDate: issueFormDate(get('ChooseDate')),
                        IssueStatus: get('IssueStatus'), Priority: get('Priority'), IssueType: get('IssueType'),
                        Replicability: get('Replicability'), UploadImage: get('UploadImage'),
                        ProjectId: get('ProjectId'), Milestoneid: get('Milestoneid'), tasklistid: get('tasklistid'),
                        taskid: get('taskid'), subtaskid: get('subtaskid')
                    };
                    $scope.issueMulti.search.AssignTo = $scope.issueForm.AssignTo ? $scope.issuePersonName($scope.issueForm.AssignTo) : '';
                    $scope.issueMulti.search.Owner = $scope.issueForm.Owner.length ? $scope.issuePersonName($scope.issueForm.Owner[0]) : '';
                });
            }).catch(function (error) { state.formError = error.message || 'Unable to load issue details.'; })
            .finally(function () { state.loadingOptions = false; $scope.$evalAsync(); });
    };
    $scope.deleteIssue = function (issue) {
        if (!issue.id || !issue.canDelete || !window.confirm('Delete issue "' + issue.name + '"?')) return;
        var state = $scope.issuePanel;
        state.deletingId = issue.id; state.error = '';
        $http.post('/pms/Addissues.aspx/Deleteissuesdata', { RecordId: issue.id }).then(function () {
            $scope.refreshIssueList();
        }, function () { state.error = 'Unable to delete this issue. Please try again.'; })
        .finally(function () { state.deletingId = null; });
    };
    $scope.toggleIssuePerson = function (field, option) {
        var items = $scope.issueForm[field], index = items.indexOf(String(option.id));
        if (index >= 0) items.splice(index, 1);
        else items.push(String(option.id));
        $scope.issueMulti.search[field] = '';
        $scope.issueMulti.active = 0;
    };
    $scope.issuePersonMatches = function (field) {
        var needle = ($scope.issueMulti.search[field] || '').trim().toLowerCase();
        return ($scope.issueEmployeeOptions || []).filter(function (person) {
            return !needle || String(person.name || '').toLowerCase().indexOf(needle) !== -1;
        });
    };
    $scope.openIssuePeople = function (field) {
        if ($scope.issueMulti.open !== field) $scope.issueMulti.active = -1;
        if ((field === 'AssignTo' || field === 'Owner') && $scope.issueMulti.open !== field) $scope.issueMulti.search[field] = '';
        $scope.issueMulti.open = field;
    };
    $scope.issuePersonKeydown = function (field, event) {
        if (event.key === 'Escape') { $scope.issueMulti.open = ''; event.preventDefault(); return; }
        if (event.key === 'ArrowDown' || event.key === 'ArrowUp') {
            event.preventDefault();
            if ($scope.issueMulti.open !== field) $scope.openIssuePeople(field);
            var list = $scope.issuePersonMatches(field);
            if (list.length) {
                var current = Math.min($scope.issueMulti.active, list.length - 1);
                $scope.issueMulti.active = event.key === 'ArrowDown' ? (current + 1) % list.length :
                    (current <= 0 ? list.length - 1 : current - 1);
                setTimeout(function () {
                    var menu = event.target.closest('.pms-issue-field').querySelector('.pms-issue-person-list');
                    var option = menu && menu.children[$scope.issueMulti.active];
                    if (option) option.scrollIntoView({ block: 'nearest' });
                }, 0);
            }
        } else if (event.key === 'Enter' && $scope.issueMulti.open === field) {
            var matches = $scope.issuePersonMatches(field);
            if (matches.length) {
                event.preventDefault();
                $scope.chooseIssuePerson(field, matches[Math.max(0, Math.min($scope.issueMulti.active, matches.length - 1))]);
            }
        }
    };
    $scope.chooseIssuePerson = function (field, person) {
        if (field === 'AssignTo') {
            $scope.issueForm.AssignTo = String(person.id);
            $scope.issueMulti.search.AssignTo = person.name;
            $scope.issueMulti.open = '';
            if (document.activeElement && document.activeElement.id === 'pmsIssueAssigneeSearch') document.activeElement.blur();
        } else if (field === 'Owner') {
            $scope.issueForm.Owner = [String(person.id)];
            $scope.issueMulti.search.Owner = person.name;
            $scope.issueMulti.open = '';
            if (document.activeElement && document.activeElement.id === 'pmsIssueOwnerSearch') document.activeElement.blur();
        } else $scope.toggleIssuePerson(field, person);
        $scope.issueMulti.active = 0;
    };
    $scope.issuePersonName = function (id) {
        var option = $scope.issueFormOptions.employees.find(function (row) { return String(row.id) === String(id); });
        return option ? option.name : id;
    };
    $scope.closeIssuePersonList = function ($event) {
        if (!$event.target.closest('.pms-issue-multi') && !$event.target.closest('.pms-issue-single') && !$event.target.closest('.pms-issue-person-list')) {
            $scope.issueMulti.open = '';
            $scope.issueMulti.search.AssignTo = $scope.issueForm.AssignTo ? $scope.issuePersonName($scope.issueForm.AssignTo) : '';
            $scope.issueMulti.search.Owner = $scope.issueForm.Owner.length ? $scope.issuePersonName($scope.issueForm.Owner[0]) : '';
        }
    };
    function issueDateValue(value) {
        if (!value) return '';
        if (typeof value === 'string') return value;
        return value.getUTCFullYear() + '-' + String(value.getUTCMonth() + 1).padStart(2, '0') + '-' + String(value.getUTCDate()).padStart(2, '0');
    }
    function issueTimeValue(value) {
        if (!value) return '';
        if (typeof value === 'string') return value;
        return String(value.getUTCHours()).padStart(2, '0') + ':' + String(value.getUTCMinutes()).padStart(2, '0');
    }
    $scope.saveIssueForm = function (addMore) {
        var state = $scope.issuePanel, form = $scope.issueForm, context = state.context;
        state.formError = '';
        if (state.loadingOptions || state.saving) return;
        if (!form.Issues.trim()) { state.formError = 'Enter the issue title.'; return; }
        if (!form.AssignTo || !form.IssueStatus || !form.Priority || !form.IssueType || !form.Replicability) {
            state.formError = 'Select assignee, status, priority, issue type and replicability.'; return;
        }
        if (form.StartDate && form.EndDate && form.EndDate < form.StartDate) {
            state.formError = 'End date must be on or after start date.'; return;
        }
        var payload = {
            id: form.id || '0', ProjectId: form.ProjectId || String(context.projectId), Milestoneid: form.Milestoneid || String(context.milestoneId),
            tasklistid: form.tasklistid || '0', taskid: form.taskid || String(context.taskId), subtaskid: form.subtaskid || String(context.subtaskId),
            Issues: form.Issues.trim(), StartDate: issueDateValue(form.StartDate), EndDate: issueDateValue(form.EndDate),
            Owner: form.Owner.join(','), team: form.team.join(','), Issueon: context.type,
            project_id: String(context.projectId), Description: form.Description || '',
            Reminder: form.Reminder || '0', ReminderType: form.Reminder === '1' ? (form.ReminderType || '') : '',
            Days: form.Reminder === '1' && form.ReminderType === '3' ? String(form.Days || '') : '',
            Time: form.Reminder !== '0' ? issueTimeValue(form.Time) : '',
            Notifyusers: form.Reminder !== '0' ? (form.Notifyusers || '') : '',
            ChooseDate: form.Reminder === '2' ? issueDateValue(form.ChooseDate) : '', AssignTo: form.AssignTo,
            IssueStatus: form.IssueStatus, Priority: form.Priority, IssueType: form.IssueType,
            Replicability: form.Replicability, UploadImage: form.UploadImage || ''
        };
        state.saving = true;
        $http.post('/pms/Addissues.aspx/SaveIsues', { issuesdetails: payload }).then(function (response) {
            var savedId = Number(response.data.d);
            if (!savedId) throw new Error('Issue could not be saved.');
            var input = document.getElementById('pmsIssueFiles');
            if (!input || !input.files || !input.files.length) return;
            var data = new FormData();
            Array.prototype.forEach.call(input.files, function (file) { data.append(file.name, file); });
            return $http.post('/pms/FileUploadHandlerIssues.ashx?id=' + encodeURIComponent(savedId), data, {
                transformRequest: angular.identity, headers: { 'Content-Type': undefined }
            }).catch(function () { throw new Error('Issue saved, but the file upload failed. Return to issues to see the saved record.'); });
        }).then(function () {
            state.saving = false;
            if (addMore) {
                $scope.issueForm = newIssueForm();
                var fileInput = document.getElementById('pmsIssueFiles');
                if (fileInput) fileInput.value = '';
            } else $scope.backToIssueList();
        }).catch(function (error) {
            state.formError = error.message || (error.data && error.data.Message) || 'Issue could not be saved.';
            state.saving = false;
        });
    };
    $scope.backToIssueList = function () { $scope.issuePanel.mode = 'list'; $scope.refreshIssueList(); };
    $scope.closeIssueList = function () { ++issuePanelRequest; $scope.issuePanel.open = false; };
    $scope.openGanttIssue = function (row, $event) {
        if ($event) $event.stopPropagation();
        $scope.closeGanttRowMenu();
        var context = getGanttHierarchyContext(row);
        if (context) $scope.openIssueList(context.item, row.type, row.name);
    };

    $scope.openKanbanFiles = function (item, $event) {
        $scope.stopKanbanMenuClick($event);
        var ctx = $scope.getKanbanContext(item);
        var index = ctx.type === 'milestone' ? ctx.mileIndex : (ctx.type === 'task' ? ctx.taskIndex : ctx.subtaskIndex);
        $scope.loadSrcFiles(ctx.item, ctx.type, index);
    };

    $scope.editKanbanItem = function (item, $event) {
        $scope.stopKanbanMenuClick($event);
        var ctx = $scope.getKanbanContext(item);
        var src = ctx.item;
        var url = '';
        var title = '';

        if (ctx.type === 'milestone') {
            url = '/pms/editmile.aspx?id=' + src.id + '&pid=' + (src.ProjectId || (($scope.projectlist || {}).id));
            title = 'Milestone';
        } else if (ctx.type === 'task') {
            url = '/pms/edittask.aspx?id=' + src.id + '&mid=' + (src.Milestoneid || src.MilestonId || (ctx.mile && ctx.mile.id));
            title = 'Task';
        } else {
            url = '/pms/addnewsubtask.aspx?id=' + src.id + '&tid=' + (src.Task || src.task || (ctx.task && ctx.task.id));
            title = 'Sub Task';
        }

        $scope.loadformedit(src, ctx.type, 'editdetails', url, title);
    };

    $scope.cloneKanbanItem = function (item, $event) {
        $scope.stopKanbanMenuClick($event);
        var ctx = $scope.getKanbanContext(item);
        if (ctx.type === 'milestone') {
            $scope.addRowUpDown_mile(ctx.mileIndex, ctx.item, 'down');
        } else if (ctx.type === 'task') {
            $scope.addRowUpDown_task(ctx.taskIndex, ctx.mile, ctx.item, 'down');
        } else {
            $scope.addRowUpDown_subtask(ctx.subtaskIndex, ctx.task, ctx.item, 'down');
        }
    };
    $scope.deleteKanbanItem = function (item, $event) {
        $scope.stopKanbanMenuClick($event);

        var ctx = $scope.getKanbanContext(item);
        if (!ctx || !ctx.item || !ctx.item.id || !ctx.type) {
            alert('Unable to identify this Kanban item.');
            return;
        }

        if (ctx.type === 'milestone') {
            // Same method and argument order used by the table view.
            $scope.deletedatamile(ctx.mileIndex, ctx.item);
        } else if (ctx.type === 'task') {
            if (!ctx.mile || ctx.taskIndex < 0) {
                alert('Unable to identify the parent milestone.');
                return;
            }
            $scope.deletedatatask(ctx.taskIndex, ctx.item, ctx.mile);
        } else if (ctx.type === 'subtask') {
            if (!ctx.task || ctx.subtaskIndex < 0) {
                alert('Unable to identify the parent task.');
                return;
            }
            $scope.deletedatasubtask(ctx.subtaskIndex, ctx.item, ctx.task);
        }
    };
    $scope.deleteKanbanItemold = function (item, $event) {
        $scope.stopKanbanMenuClick($event);
        var ctx = $scope.getKanbanContext(item);
        if (ctx.type === 'milestone') {
            $scope.deletedatamilestone(ctx.mileIndex, ctx.item);
        } else if (ctx.type === 'task') {
            $scope.deletedatatask(ctx.taskIndex, ctx.item, ctx.mile);
        } else {
            $scope.deletedatasubtask(ctx.subtaskIndex, ctx.item, ctx.task);
        }
    };

    $scope.openKanbanItem = function (item) {
        if (!item || !item.source) return;
        var src = item.source;
        if (item.tasktype === 'milestone' && angular.isFunction($scope.edit_milestone)) {
            $scope.edit_milestone(src);
        } else if (item.tasktype === 'task' && angular.isFunction($scope.edit_task)) {
            $scope.edit_task(src);
        } else if (item.tasktype === 'subtask' && angular.isFunction($scope.edit_subtask)) {
            $scope.edit_subtask(src);
        }
    };

    $scope.getAssignNodeCompleteValue = function (node) {
        //debugger
        if (!node) return 0;

        var src = node.source || {};
        var item = node.item || node;

        return node.iscomplete !== undefined ? node.iscomplete :
            node.IsComplete !== undefined ? node.IsComplete :
                item.iscomplete !== undefined ? item.iscomplete :
                    item.IsComplete !== undefined ? item.IsComplete :
                        0;
    };

    $scope.isCompletedAssignNode = function (node, utype) {
        //alert(utype);
        var v = $scope.getAssignNodeCompleteValue(node);
        return v === 1 || v === "1" || v === true || v === "true" || v === "True";
    };
    // Page permissions are rendered from default.aspx.cs.
    // Edit and Assign are separate permissions for Milestone, Task and Subtask.
    var pmsPerm = window.PmsAssignPermissions || {};
    function toPermInt(value) {
        return parseInt(value || 0, 10) === 1 ? 1 : 0;
    }
    $scope.pmsPermissions = {
        milestone: {
            canEdit: toPermInt(pmsPerm.ischeckEdit),
            canAssign: toPermInt(pmsPerm.ischeckAssign)
        },
        task: {
            canEdit: toPermInt(pmsPerm.ischeckEdit_task),
            canAssign: toPermInt(pmsPerm.ischeckAssign_task)
        },
        subtask: {
            canEdit: toPermInt(pmsPerm.ischeckEdit_stask),
            canAssign: toPermInt(pmsPerm.ischeckAssign_stask)
        }
    };
    $scope.getPermissionType = function (utype) {
        utype = String(utype || '').toLowerCase().trim();
        if (utype === 'milestone' || utype === 'mile') return 'milestone';
        if (utype === 'task') return 'task';
        if (utype === 'subtask' || utype === 'stask' || utype === 'sub task') return 'subtask';
        return '';
    };

    $scope.resolvePermissionType = function (utype, obj) {
        var key = $scope.getPermissionType(utype);
        if (key) return key;

        obj = obj || null;

        if (obj) {
            key = $scope.getPermissionType(obj.type || obj.tasktype || obj.TaskType || obj.utype || obj.currentFileType);
            if (key) return key;

            if (obj.source) {
                key = $scope.getPermissionType(obj.source.type || obj.source.tasktype || obj.source.TaskType || obj.source.utype);
                if (key) return key;
            }

            // Fallback by object fields when type is blank after refresh/update.
            if (obj.Subtaskname || obj.SubTaskDescription || obj.Task) return 'subtask';
            if (obj.Taskname || obj.taskname) return 'task';
            if (obj.Milestone || obj.milestone || obj.EndDate) return 'milestone';
        }

        key = $scope.getPermissionType($scope.currentFileType);
        if (key) return key;

        if ($scope.currentlist) {
            key = $scope.getPermissionType($scope.currentlist.type || $scope.currentlist.tasktype || $scope.currentlist.TaskType || $scope.currentlist.utype);
            if (key) return key;

            if ($scope.currentlist.Subtaskname || $scope.currentlist.SubTaskDescription || $scope.currentlist.Task) return 'subtask';
            if ($scope.currentlist.Taskname || $scope.currentlist.taskname) return 'task';
            if ($scope.currentlist.Milestone || $scope.currentlist.milestone || $scope.currentlist.EndDate) return 'milestone';
        }

        key = $scope.getPermissionType($scope.lastPermissionType);
        if (key) return key;

        if ($scope.lastEditObj) {
            if ($scope.lastEditObj.Subtaskname || $scope.lastEditObj.SubTaskDescription || $scope.lastEditObj.Task) return 'subtask';
            if ($scope.lastEditObj.Taskname || $scope.lastEditObj.taskname) return 'task';
            if ($scope.lastEditObj.Milestone || $scope.lastEditObj.milestone || $scope.lastEditObj.EndDate) return 'milestone';
        }

        return '';
    };
    $scope.canEdit = function (utype, obj) {
        var key = $scope.resolvePermissionType(utype, obj);
        if (!key) return false;
        return !!($scope.pmsPermissions[key] && $scope.pmsPermissions[key].canEdit === 1);
    };

    $scope.canAssign = function (utype) {
        var key = $scope.getPermissionType(utype);
        return !!(key && $scope.pmsPermissions[key] && $scope.pmsPermissions[key].canAssign === 1);
    };
    $scope.requireEdit = function (utype, obj, suppressAlert) {
        var key = $scope.resolvePermissionType(utype, obj);

        // Blank/internal duplicate calls after update/refresh should not show a false alert.
        if (!key) {
            return false;
        }

        if ($scope.canEdit(key, obj)) {
            $scope.lastPermissionType = key;
            if (obj) $scope.lastEditObj = obj;
            return true;
        }

        if (!suppressAlert) {
            alert('You do not have edit permission.');
        }
        return false;
    };

    $scope.requireAssign = function (utype, suppressAlert) {
        if ($scope.canAssign(utype)) return true;
        if (!suppressAlert) {
            alert('You do not have assign permission.');
        }
        return false;
    };
    //#endregion

    //#region assign users

    $scope.isreassign = 0;
    $scope.currenttaskIndex = 0;
    $scope.currenttaskType = "";
    $scope.currenttasklist = [];
    $scope.currenttaskteam = [];
    $scope.selectedAssignTasks = [];
    $scope.assignTree = [];
    $scope.assignTreeNodeMap = {};

    $scope.normalizeAssignTaskType = function (tasktype) {
        var type = String(tasktype || '').trim().toLowerCase();
        if (type === 'milestones') return 'milestone';
        if (type === 'tasks') return 'task';
        if (type === 'subtasks') return 'subtask';
        return type;
    };

    $scope.getAssignTaskKey = function (taskid, tasktype) {
        return String(taskid || '') + '_' + $scope.normalizeAssignTaskType(tasktype);
    };

    $scope.syncSelectedAssignTasks = function () {
        var selected = ($scope.currenttasklist || []).filter(function (x) {
            return x.isSelected === true && x.isAssignedTemp !== true && x.isRemovedTemp !== true && $scope.canAssign(x.tasktype);
        });
        var treeSelected = $scope.collectSelectedAssignTreeTasks ? $scope.collectSelectedAssignTreeTasks() : [];
        var map = {}, list = [];
        angular.forEach(selected.concat(treeSelected), function (x) {
            var k = $scope.getAssignTaskKey(x.id, x.tasktype);
            if (!map[k]) { map[k] = true; list.push(x); }
        });
        $scope.selectedAssignTasks = list;
    };

    $scope.toggleAssignTaskSelection = function (task, $event) {
        if ($event) { $event.stopPropagation(); }
        if (!task) return;
        $timeout(function () { $scope.syncSelectedAssignTasks(); }, 0);
    };

$scope.getTaskDisplayName = function (item, type) {
    if (!item) return '';
    if (type === 'milestone') return item.Milestone || item.Milestonename || item.Subtaskname || item.Taskname || item.taskname || item.name || '';
    if (type === 'task') return item.taskname || item.Taskname || item.Subtaskname || item.name || '';
    return item.Subtaskname || item.subtaskname || item.Taskname || item.taskname || item.name || '';
};

$scope.getTaskDurationText = function (item) {
    if (!item) return '';

    var rawHours = item.TaskDuration !== undefined && item.TaskDuration !== null
        ? item.TaskDuration
        : (item.taskduration !== undefined && item.taskduration !== null
            ? item.taskduration
            : (item.TaskDuration_hr || item.TaskDuration_hrs || item.TaskDurationhour || item.TaskDuration_hour || ''));

    var rawMinutes = item.TaskDuration_min !== undefined && item.TaskDuration_min !== null
        ? item.TaskDuration_min
        : (item.taskduration_min !== undefined && item.taskduration_min !== null
            ? item.taskduration_min
            : (item.DurationMin !== undefined && item.DurationMin !== null ? item.DurationMin : ''));

    var text = String(rawHours === undefined || rawHours === null ? '' : rawHours).trim();
    var minuteText = String(rawMinutes === undefined || rawMinutes === null ? '' : rawMinutes).trim();

    if (!text && !minuteText) return '';

    text = text.replace(/hrs?|hours?/ig, '').trim();
    var separator = text.indexOf(':') >= 0 ? ':' : (text.indexOf('/') >= 0 ? '/' : null);
    var hours = 0;
    var minutes = 0;

    if (separator) {
        var parts = text.split(separator);
        hours = parseInt(parts[0] || '0', 10);
        minutes = parseInt((parts[1] !== undefined && String(parts[1]).trim() !== '') ? parts[1] : (minuteText || '0'), 10);
    } else if (text.indexOf('.') >= 0 && minuteText === '') {
        var decimalHours = parseFloat(text);
        if (!isNaN(decimalHours)) {
            var totalFromDecimal = Math.round(decimalHours * 60);
            hours = Math.floor(totalFromDecimal / 60);
            minutes = totalFromDecimal % 60;
        }
    } else {
        hours = parseInt(text || '0', 10);
        minutes = parseInt(minuteText || '0', 10);
    }

    if (isNaN(hours) || hours < 0) hours = 0;
    if (isNaN(minutes) || minutes < 0) minutes = 0;

    // Carry values such as 1:75 safely as 2:15 instead of losing the minutes.
    var totalMinutes = (hours * 60) + minutes;
    hours = Math.floor(totalMinutes / 60);
    minutes = totalMinutes % 60;

    return String(hours) + ':' + String(minutes).padStart(2, '0');
};

// Assign date validation:
// - milestone validates against EndDate
// - task/subtask validates against Targetdate
// This is UI-only validation and does not change existing DB/owner save logic.
$scope.parseAssignValidationDate = function (value) {
    if (!value) return null;

    if (Object.prototype.toString.call(value) === '[object Date]') {
        if (isNaN(value.getTime())) return null;
        return moment(value).startOf('day');
    }

    if (typeof value === 'number') {
        var mn = moment(value).startOf('day');
        return mn.isValid() ? mn : null;
    }

    var s = String(value || '').trim();
    if (!s) return null;

    var dotNet = /\/Date\((\d+)\)\//.exec(s);
    if (dotNet && dotNet[1]) {
        var md = moment(parseInt(dotNet[1], 10)).startOf('day');
        return md.isValid() ? md : null;
    }

    var formats = [
        moment.ISO_8601,
        'MM/DD/YYYY', 'M/D/YYYY',
        'DD/MM/YYYY', 'D/M/YYYY',
        'YYYY-MM-DD',
        'DD-MMM-YYYY', 'D-MMM-YYYY',
        'MMM DD, YYYY'
    ];

    var m = moment(s, formats, true);
    if (!m.isValid()) m = moment(s);
    return m.isValid() ? m.startOf('day') : null;
};

$scope.formatAssignValidationDate = function (m) {
    if (!m || !m.isValid || !m.isValid()) return '';
    return m.format('DD/MM/YYYY');
};

$scope.copyAssignRootInfo = function (items, rootId, rootType) {
    angular.forEach(items || [], function (x) {
        if (!x) return;
        x._assignDropRootId = rootId;
        x._assignDropRootType = rootType;
    });
    return items || [];
};

$scope.getAssignValidationEndDate = function (item, tasktype) {
    if (!item) return null;
    tasktype = String(tasktype || item.tasktype || item.type || '').toLowerCase();

    if (tasktype === 'milestone') {
        return item.EndDate || item.endDate || item.enddate || item.Targetdate || item.TargetDate || item.targetdate || null;
    }

    // task/subtask use Targetdate. EndDate fallback is only for safety when API returns a different casing/name.
    return item.Targetdate || item.TargetDate || item.targetdate || item.targetDate || item.EndDate || item.endDate || item.enddate || null;
};

$scope.findAssignValidationItem = function (taskid, tasktype, preferred) {
    debugger
    tasktype = String(tasktype || '').toLowerCase();
    var found = null;

    function hasValidationDate(obj) {
        return !!$scope.getAssignValidationEndDate(obj, tasktype);
    }

    function tryObj(obj, objType) {
        if (found || !obj) return;
        var t = String(objType || obj.tasktype || obj.taskType || obj.type || tasktype || '').toLowerCase();
        var id = obj.id || obj.taskid || obj.taskId || obj.Task || obj.subtaskid || obj.Milestoneid;
        if (String(id) === String(taskid) && t === tasktype) found = obj;
    }

    if (preferred && hasValidationDate(preferred)) return preferred;

    angular.forEach($scope.currenttasklist || [], function (x) { tryObj(x); });
    angular.forEach($scope.selectedAssignTasks || [], function (x) { tryObj(x); });

    if ($scope.assignTreeNodeMap) {
        var node = $scope.assignTreeNodeMap[$scope.getAssignTaskKey(taskid, tasktype)];
        if (node) {
            tryObj(node.source, node.type);
            tryObj(node.item, node.type);
            tryObj(node, node.type);
        }
    }

    angular.forEach(($scope.projectlist && $scope.projectlist.miles) || [], function (mile) {
        if (found) return;
        tryObj(mile, 'milestone');
        angular.forEach(mile.tasks || [], function (task) {
            if (found) return;
            tryObj(task, 'task');
            angular.forEach(task.subtasks || [], function (sub) {
                if (found) return;
                tryObj(sub, 'subtask');
            });
        });
    });

    angular.forEach($scope.currenttaskteam || [], function (emp) {
        if (found) return;
        angular.forEach(emp.usertasks || [], function (x) { tryObj(x); });
        angular.forEach(emp.taskSegments || [], function (x) { tryObj(x); });
    });

    return found || preferred || null;
};

// Resolve planning dates from the original hierarchy, not assignment work dates.
$scope.findAssignDateValidationItem = function (taskid, tasktype, fallback) {
    tasktype = String(tasktype || '').toLowerCase();
    var found = null;
    angular.forEach(($scope.projectlist && $scope.projectlist.miles) || [], function (mile) {
        if (tasktype === 'milestone' && String(mile.id) === String(taskid)) found = mile;
        angular.forEach(mile.tasks || [], function (task) {
            if (tasktype === 'task' && String(task.id) === String(taskid)) found = task;
            angular.forEach(task.subtasks || [], function (sub) {
                if (tasktype === 'subtask' && String(sub.id) === String(taskid)) found = sub;
            });
        });
    });
    if (found) return found;
    var node = $scope.assignTreeNodeMap && $scope.assignTreeNodeMap[$scope.getAssignTaskKey(taskid, tasktype)];
    if (node) {
        var source = node.source || node.item;
        if (source && $scope.getAssignValidationEndDate(source, tasktype) &&
            (source.Startdate || source.StartDate || source.startdate || source.startDate)) return source;
    }
    return $scope.findAssignValidationItem(taskid, tasktype, fallback);
};

$scope.validateAssignSelectedDateWithTaskEndDate = function (tasks, assignDate, suppressAlert) {
    debugger
    tasks = angular.isArray(tasks) ? tasks : (tasks ? [tasks] : []);
    if (!tasks.length) return true;

    var selectedM = $scope.parseAssignValidationDate(assignDate || $scope.selectedDate);
    if (!selectedM) selectedM = moment($scope.selectedDate).startOf('day');
    if (!selectedM || !selectedM.isValid()) {
        if (!suppressAlert) alert('A valid assignment date is required.');
        return false;
    }

    var checked = {};
    for (var i = 0; i < tasks.length; i++) {
        var task = tasks[i] || {};
        var checkId = task._assignDropRootId || task.id || task.taskid || task.Task || task.subtaskid;
        var checkType = String(task._assignDropRootType || task.tasktype || task.taskType || task.type || '').toLowerCase();
        if (!checkId || !checkType) continue;

        var key = checkType + '_' + checkId;
        if (checked[key]) continue;
        checked[key] = true;
        debugger
        var item = $scope.findAssignDateValidationItem(checkId, checkType, task);
        var rawEnd = $scope.getAssignValidationEndDate(item, checkType);
        var endM = $scope.parseAssignValidationDate(rawEnd);
        var rawStart = item && (item.Startdate || item.StartDate || item.startdate || item.startDate);
        var startM = $scope.parseAssignValidationDate(rawStart);
        var name = $scope.getTaskDisplayName(item || task, checkType) || $scope.getTaskDisplayName(task, checkType) || 'Selected task';
        if (!startM || !startM.isValid() || !endM || !endM.isValid() || endM.isBefore(startM, 'day')) {
            if (!suppressAlert) alert(name + ' cannot be assigned because its StartDate/EndDate range is missing or invalid.');
            return false;
        }
        if (selectedM.isBefore(startM, 'day')) {
            if (!suppressAlert) alert(name + ' cannot be assigned on ' + $scope.formatAssignValidationDate(selectedM) +
                '. Selected date should not be less than StartDate (' + $scope.formatAssignValidationDate(startM) + ').');
            return false;
        }

        if (selectedM.isAfter(endM, 'day')) {
            var name = $scope.getTaskDisplayName(item || task, checkType) || $scope.getTaskDisplayName(task, checkType) || 'Selected task';
            var label = (checkType === 'milestone') ? 'EndDate' : 'Targetdate';
            var msg = name + ' cannot be assigned on ' + $scope.formatAssignValidationDate(selectedM) +
                '. Selected date should not be greater than ' + label + ' (' + $scope.formatAssignValidationDate(endM) + ').';
            if (!suppressAlert) alert(msg);
            return false;
        }
    }

    return true;
};

    $scope.makeAssignSource = function (item, type, fallback) {
        fallback = fallback || {};
        if (!item) return null;
        return {
            id: item.id,
            tasktype: $scope.normalizeAssignTaskType(type),
            owner: item.owner || item.Owner || fallback.owner || '',
            ProjectId: item.ProjectId || fallback.ProjectId,
            Milestoneid: item.Milestoneid || item.MilestoneId || fallback.Milestoneid,
            Task: item.Task || item.task || fallback.Task,
            // Project hierarchy returns duration as separate hour/minute fields, while
            // assignment APIs may return one HH:mm value. Always compose both first.
            TaskDuration: $scope.getTaskDurationText(item) || $scope.getTaskDurationText(fallback) || '',
            Startdate: item.Startdate || item.StartDate || item.startdate || item.startDate || fallback.Startdate || fallback.StartDate || fallback.startdate || fallback.startDate || null,
            EndDate: item.EndDate || item.endDate || item.enddate || fallback.EndDate || fallback.endDate || fallback.enddate || null,
            Targetdate: item.Targetdate || item.TargetDate || item.targetdate || item.targetDate || fallback.Targetdate || fallback.TargetDate || fallback.targetdate || fallback.targetDate || null,
            TargetDate: item.TargetDate || item.Targetdate || item.targetDate || item.targetdate || fallback.TargetDate || fallback.Targetdate || fallback.targetDate || fallback.targetdate || null,
            iscomplete: item.iscomplete !== undefined ? item.iscomplete :
                item.IsComplete !== undefined ? item.IsComplete :
                    item.isComplete !== undefined ? item.isComplete :
                        fallback.iscomplete !== undefined ? fallback.iscomplete : 0,
            Subtaskname: $scope.getTaskDisplayName(item, type),
            isassigned: item.isassigned === true || item.IsAssigned === true ||
                String(item.isassigned || item.IsAssigned || fallback.isassigned || fallback.IsAssigned || '0') === '1' ||
                (String(item.assignto || item.AssignTo || fallback.assignto || fallback.AssignTo || '').trim() !== '' &&
                 String(item.assignto || item.AssignTo || fallback.assignto || fallback.AssignTo || '0') !== '0'),
            _treeSource: true,
            isAssignedTemp: false,
            isSelected: false
        };
    };



$scope.getAssignmentLevel = function () {
    return $scope.assignmentProjectSettings && $scope.assignmentProjectSettings.Level || 'subtask';
};

$scope.buildAssignTreeFromProjectList = function (selectedItem, selectedType) {
    $scope.assignTree = [];
    $scope.assignTreeNodeMap = {};

    var sourceMap = {};
    angular.forEach($scope.currenttasklist || [], function (x) {
        sourceMap[$scope.getAssignTaskKey(x.id, x.tasktype)] = x;
    });

    function getSource(item, type, fallback) {
        if (!item || item.id === undefined || item.id === null) return null;
        var k = $scope.getAssignTaskKey(item.id, type);
        return sourceMap[k] || $scope.makeAssignSource(item, type, fallback);
    }

    function register(node) {
        if (!node) return node;
        $scope.assignTreeNodeMap[$scope.getAssignTaskKey(node.id, node.type)] = node;
        return node;
    }

    function buildSubtaskNode(sub, task, mile, parentTaskNode) {
        var src = getSource(sub, 'subtask', {
            ProjectId: sub.ProjectId || task.ProjectId || mile.ProjectId,
            Milestoneid: sub.Milestoneid || task.Milestoneid || mile.id,
            Task: sub.Task || sub.task || task.id,
            owner: sub.owner || task.owner || mile.Owner
        });

        // Completed subtasks must never be shown in the assign tree,
        // whether Assign is opened from milestone, task, or subtask.
        if ($scope.isCompletedAssignNode({ item: sub, source: src })) {
            return null;
        }

        return register({
            type: 'subtask',
            id: sub.id,
            parent: parentTaskNode || null,
            name: $scope.getTaskDisplayName(sub, 'subtask'),
            item: sub,
            source: src,
            children: [],
            expanded: true,
            isSelected: false
        });
    }

    function buildTaskNode(task, mile, parentMileNode) {
        var src = getSource(task, 'task', {
            ProjectId: task.ProjectId || mile.ProjectId,
            Milestoneid: task.Milestoneid || mile.id,
            owner: task.owner || task.Owner || mile.Owner
        });

        var taskNode = register({
            type: 'task',
            id: task.id,
            parent: parentMileNode || null,
            name: $scope.getTaskDisplayName(task, 'task'),
            item: task,
            source: src,
            children: [],
            expanded: true,
            isSelected: false
        });

        if ($scope.getAssignmentLevel() !== 'subtask') return taskNode;

        // Remember that this task owns subtasks even when every subtask is completed.
        // This prevents the parent task from becoming assignable as a lowest-level item.
        taskNode.hasSubtaskHierarchy = !!(task.subtasks && task.subtasks.length > 0);

        // First preference: use hierarchy already available in projectlist.miles.
        angular.forEach(task.subtasks || [], function (sub) {
            var subNode = buildSubtaskNode(sub, task, mile, taskNode);
            if (subNode) taskNode.children.push(subNode);
        });

        // Fallback: when user clicks a task and service returns a flat list,
        // create only non-completed subtasks under this clicked task.
        if (taskNode.children.length === 0 && !taskNode.hasSubtaskHierarchy) {
            angular.forEach($scope.currenttasklist || [], function (x) {
                var xtype = String(x.tasktype || '').toLowerCase();
                var xtaskid = x.Task || x.task || x.taskid || x.TaskId;
                if (xtype === 'subtask' && String(xtaskid) === String(task.id)) {
                    taskNode.hasSubtaskHierarchy = true;
                    var subNode = buildSubtaskNode(x, task, mile, taskNode);
                    if (subNode) taskNode.children.push(subNode);
                }
            });
        }

        return taskNode;
    }

    function buildMilestoneNode(mile) {
        var src = getSource(mile, 'milestone', {
            ProjectId: mile.ProjectId || $scope.projectlist.id,
            Milestoneid: mile.id,
            owner: mile.Owner
        });

        var mileNode = register({
            type: 'milestone',
            id: mile.id,
            parent: null,
            name: $scope.getTaskDisplayName(mile, 'milestone'),
            item: mile,
            source: src,
            children: [],
            expanded: true,
            isSelected: false
        });

        if ($scope.getAssignmentLevel() !== 'milestone') {
        angular.forEach(mile.tasks || [], function (t) {
            mileNode.children.push(buildTaskNode(t, mile, mileNode));
        });
        }

        return mileNode;
    }

    if (selectedType === 'milestone') {
        $scope.assignTree.push(buildMilestoneNode(selectedItem));
    } else if (selectedType === 'task') {
        var fakeMile = { id: selectedItem.Milestoneid, ProjectId: selectedItem.ProjectId, Owner: selectedItem.owner || selectedItem.Owner };
        $scope.assignTree.push(buildTaskNode(selectedItem, fakeMile, null));
    } else if (selectedType === 'subtask') {
        var fakeTask = { id: selectedItem.Task || selectedItem.task, Milestoneid: selectedItem.Milestoneid, ProjectId: selectedItem.ProjectId, owner: selectedItem.owner || selectedItem.Owner };
        var fakeMile2 = { id: selectedItem.Milestoneid, ProjectId: selectedItem.ProjectId, Owner: selectedItem.owner || selectedItem.Owner };
        $scope.assignTree.push(buildSubtaskNode(selectedItem, fakeTask, fakeMile2, null));
    }
};

$scope.getLowestAssignItemsFromNode = function (node) {
    var result = [];
    if (!node) return result;

    function canUseLowestSource(src) {
        return src && src.isAssignedTemp !== true && !$scope.isAssignSourceAssigned(src);
    }

    if (node.type === 'subtask') {
        if (canUseLowestSource(node.source)) result.push(node.source);
        return result;
    }

    if (node.type === 'task') {
        if (node.children && node.children.length > 0) {
            angular.forEach(node.children, function (s) {
                result = result.concat($scope.getLowestAssignItemsFromNode(s));
            });
        } else if (!node.hasSubtaskHierarchy && canUseLowestSource(node.source)) {
            result.push(node.source);
        }
        return result;
    }

    if (node.type === 'milestone') {
        if (node.children && node.children.length > 0) {
            angular.forEach(node.children, function (t) {
                result = result.concat($scope.getLowestAssignItemsFromNode(t));
            });
        } else if (canUseLowestSource(node.source)) {
            result.push(node.source);
        }
    }
    return result;
};

$scope.isAssignSourceAssigned = function (src) {
    if (!src) return false;
    // isAssignedTemp is used only for temporary hide/remove in right tree.
    // Do NOT treat it as assigned; green tick should come only from DB/current scheduler data.
    if (src.isassigned === true || src.IsAssigned === true ||
        String(src.isassigned || src.IsAssigned || '0') === '1' ||
        (String(src.assignto || src.AssignTo || '').trim() !== '' &&
         String(src.assignto || src.AssignTo || '0') !== '0')) return true;

    var sid = String(src.id || '');
    var stype = $scope.normalizeAssignTaskType(src.tasktype || src.type);
    var found = false;

    angular.forEach($scope.currenttaskteam || [], function (emp) {
        angular.forEach(emp.taskSegments || [], function (seg) {
            if (found) return;
            if (String(seg.taskid || seg.id || '') === sid &&
                $scope.normalizeAssignTaskType(seg.tasktype || seg.taskType) === stype) {
                found = true;
            }
        });
        angular.forEach(emp.usertasks || [], function (task) {
            if (found) return;
            if (String(task.id || task.taskid || '') === sid &&
                $scope.normalizeAssignTaskType(task.tasktype || task.taskType) === stype) {
                found = true;
            }
        });
    });

    return found;
};

$scope.syncRightPanelAssignmentState = function () {
    var assignedMap = {};

    function addAssigned(item) {
        if (!item) return;
        var id = item.id || item.taskid || item.TaskId || item.subtaskid || item.SubtaskId;
        var type = item.tasktype || item.taskType || item.type;
        if (id === undefined || id === null || String(id).trim() === '') return;
        type = $scope.normalizeAssignTaskType(type);
        if (!type) return;
        assignedMap[$scope.getAssignTaskKey(id, type)] = true;
    }

    angular.forEach($scope.currenttaskteam || [], function (emp) {
        angular.forEach(emp.usertasks || [], addAssigned);
        angular.forEach(emp.taskSegments || [], addAssigned);
    });

    angular.forEach($scope.currenttasklist || [], function (item) {
        if (!item) return;
        var key = $scope.getAssignTaskKey(item.id, item.tasktype);
        if (assignedMap[key]) {
            item.isassigned = true;
            item.IsAssigned = true;
        }
    });

    angular.forEach($scope.assignTree || [], function walk(node) {
        if (!node) return;
        var key = $scope.getAssignTaskKey(node.id, node.type);
        if (assignedMap[key]) {
            node.isassigned = true;
            node.IsAssigned = true;
            if (node.source) {
                node.source.isassigned = true;
                node.source.IsAssigned = true;
            }
        }
        angular.forEach(node.children || [], walk);
    });
};

$scope.isAssignTreeLowestNode = function (node) {
    return !!node && (!node.children || node.children.length === 0);
};

$scope.isAssignTreeNodeAssigned = function (node) {
    // Right side panel now shows only unassigned lowest tasks.
    // Assigned tasks are hidden instead of showing a green tick.
    if (!node || !$scope.isAssignTreeLowestNode(node)) return false;
    return $scope.isAssignSourceAssigned(node.source);
};

$scope.visibleAssignNode = function (node) {
    if (!node) return false;

    // Right side panel should show only unassigned tasks.
    // If a lowest task is already assigned, hide it. When it is removed from center,
    // current scheduler data is updated and the task becomes visible again.
    if (node.isRemovedTemp === true || node.isAssignedTemp === true) return false;
    if (node.source && (node.source.isRemovedTemp === true || node.source.isAssignedTemp === true)) return false;

    if (node.children && node.children.length > 0) {
        var hasVisibleChild = false;
        angular.forEach(node.children, function (child) {
            if (!hasVisibleChild && $scope.visibleAssignNode(child)) {
                hasVisibleChild = true;
            }
        });
        return hasVisibleChild;
    }

    return !$scope.isAssignSourceAssigned(node.source);
};


$scope.getAssignTreeRowClass = function (node) {
    if (!node) return '';
    var cls = 'assign-tree-row ';
    if (node.type === 'milestone') cls += 'assign-tree-milestone';
    else if (node.type === 'task') cls += 'assign-tree-task';
    else cls += 'assign-tree-subtask';

    if (!$scope.canAssign(node.type)) {
        cls += ' assign-no-permission';
    }

    return cls;
};

$scope.toggleAssignTreeExpand = function (node, $event) {
    if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
    }
    if (!node || !node.children || node.children.length === 0) return;
    node.expanded = !node.expanded;
};

$scope.removeAssignTreeNode = function (node, $event) {
    if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
    }
    if (!node) return;
    if (!$scope.requireAssign(node.type)) return;

    // Right panel remove only hides the item from Assign window.
    // It does not change database and does not affect center panel.
    var items = $scope.getLowestAssignItemsFromNode(node);
    if (!items.length && node.source) {
        items = [node.source];
    }

    angular.forEach(items, function (x) {
        if (!x) return;
        x.isAssignedTemp = true;
        x.isRemovedTemp = true;
        x.isSelected = false;

        // keep currenttasklist object in sync when source object is different
        angular.forEach($scope.currenttasklist || [], function (t) {
            if (String(t.id) === String(x.id) && String(t.tasktype) === String(x.tasktype)) {
                t.isAssignedTemp = true;
                t.isRemovedTemp = true;
                t.isSelected = false;
            }
        });
    });

    node.isRemovedTemp = true;
    node.isSelected = false;
    $scope.syncSelectedAssignTasks();
};

$scope.syncAssignTreeNodeSource = function (node) {
    if (!node) return;
    if (node.source) {
        node.source.isSelected = node.isSelected === true;

        // keep currenttasklist object in sync when source object is different
        angular.forEach($scope.currenttasklist || [], function (t) {
            if (String(t.id) === String(node.source.id) &&
                String(t.tasktype || '').toLowerCase() === String(node.source.tasktype || '').toLowerCase()) {
                t.isSelected = node.isSelected === true;
            }
        });
    }
};

$scope.setAssignTreeNodeCheckedDown = function (node, checked) {
    if (!node) return;
    if (!$scope.canAssign(node.type)) {
        node.isSelected = false;
        $scope.syncAssignTreeNodeSource(node);
        return;
    }

    // DB assigned/temporary removed lowest items should not be selectable again.
    if ($scope.isAssignTreeNodeAssigned && $scope.isAssignTreeNodeAssigned(node)) {
        node.isSelected = false;
        $scope.syncAssignTreeNodeSource(node);
        return;
    }
    if (node.isRemovedTemp === true || (node.source && node.source.isRemovedTemp === true)) {
        node.isSelected = false;
        $scope.syncAssignTreeNodeSource(node);
        return;
    }

    node.isSelected = checked === true;
    $scope.syncAssignTreeNodeSource(node);

    angular.forEach(node.children || [], function (child) {
        $scope.setAssignTreeNodeCheckedDown(child, checked === true);
    });
};

$scope.refreshAssignTreeParentFrom = function (node) {
    function isActiveChild(child) {
        return child && $scope.visibleAssignNode(child) &&
            !(child.isRemovedTemp === true) && !(child.source && child.source.isRemovedTemp === true) &&
            !($scope.isAssignTreeNodeAssigned && $scope.isAssignTreeNodeAssigned(child));
    }

    while (node) {
        var children = (node.children || []).filter(isActiveChild);
        if (children.length > 0) {
            var allChecked = true;
            angular.forEach(children, function (child) {
                if (child.isSelected !== true) allChecked = false;
            });
            node.isSelected = allChecked;
            $scope.syncAssignTreeNodeSource(node);
        }
        node = node.parent || null;
    }
};

$scope.refreshAssignTreeParents = function () {
    function walk(node) {
        if (!node) return;
        angular.forEach(node.children || [], walk);
        $scope.refreshAssignTreeParentFrom(node.parent || null);
    }
    angular.forEach($scope.assignTree || [], walk);
};

$scope.onAssignTreeCheckboxChanged = function (node, checkedValue) {
    if (!node) return;

    var checked = (typeof checkedValue === 'boolean') ? checkedValue : (node.isSelected === true);

    // Downward cascade:
    // milestone => all tasks/subtasks, task => all subtasks, subtask => itself.
    $scope.setAssignTreeNodeCheckedDown(node, checked);

    // Upward reverse cascade:
    // if any child unchecked => parent unchecked, if all children checked => parent checked.
    $scope.refreshAssignTreeParentFrom(node.parent || null);

    // Keep selectedAssignTasks updated for drag/drop assignment.
    $scope.syncSelectedAssignTasks();
};

$scope.toggleAssignTreeSelection = function (node, $event, checkedValue) {
    if ($event) {
        $event.stopPropagation && $event.stopPropagation();
        // IMPORTANT: do not call preventDefault here, otherwise checkbox visual state
        // can revert and ng-model/ng-checked will look wrong.
    }
    if (!node) return;
    if (!$scope.requireAssign(node.type)) {
        node.isSelected = false;
        $scope.syncAssignTreeNodeSource(node);
        return;
    }

    var checked;
    if (typeof checkedValue === 'boolean') {
        checked = checkedValue;
    } else if ($event && $event.target && typeof $event.target.checked === 'boolean') {
        checked = $event.target.checked;
    } else {
        checked = node.isSelected === true;
    }

    node.isSelected = checked === true;
    $scope.onAssignTreeCheckboxChanged(node, node.isSelected);
};

$scope.collectSelectedAssignTreeTasks = function () {
    var selected = [];
    angular.forEach($scope.assignTree || [], function walk(node) {
        if (!node) return;
        if (node.isSelected === true) {
            if (!$scope.canAssign(node.type)) { node.isSelected = false; return; }
            if ($scope.isAssignTreeNodeAssigned && $scope.isAssignTreeNodeAssigned(node)) { node.isSelected = false; return; }
            selected = selected.concat($scope.copyAssignRootInfo($scope.getLowestAssignItemsFromNode(node), node.id, node.type));
            return;
        }
        angular.forEach(node.children || [], walk);
    });

    var unique = {}, list = [];
    angular.forEach(selected, function (x) {
        var k = $scope.getAssignTaskKey(x.id, x.tasktype);
        if (!unique[k]) { unique[k] = true; list.push(x); }
    });
    return list;
};

$scope.gridColumns = [];
$scope.selectedDate = new Date();

// Assignment dates are calendar dates, not UTC instants.
// ASP.NET serializes DateTime as /Date(milliseconds)/. When the server date is
// midnight in India, reading those milliseconds directly in another timezone
// can display the previous day (for example, 03-Jul becomes 02-Jul).
// Rebuild the date from its source calendar components before comparing or rendering.
function getAssignWorkDateMoment(value) {
    if (value === null || value === undefined || value === '') {
        return moment(NaN);
    }

    var parts = null;
    try {
        parts = getDateOnlyParts(value);
    } catch (e) { }

    if (parts && isValidDateOnlyParts(parts)) {
        return moment({
            year: parts.year,
            month: parts.month - 1,
            day: parts.day
        }).startOf('day');
    }

    // Local Date values (selectedDate etc.) must keep their visible calendar day.
    if (Object.prototype.toString.call(value) === '[object Date]') {
        if (isNaN(value.getTime())) return moment(NaN);
        return moment({
            year: value.getFullYear(),
            month: value.getMonth(),
            day: value.getDate()
        }).startOf('day');
    }

    var parsed = moment(value, [moment.ISO_8601, 'YYYY-MM-DD', 'MM/DD/YYYY', 'M/D/YYYY'], true);
    return parsed.isValid() ? parsed.startOf('day') : moment(value).startOf('day');
}

$scope.getAssignWorkDateMoment = getAssignWorkDateMoment;

$scope.openCalendar = function () {

    var picker = $('#hiddenDatePicker').data('daterangepicker');

    if (picker) {
        picker.show();
    }
};
$scope.removeassigntask = function (taskid, tasktype, index) {
    // Temporary remove from right side task list only
    if (angular.isNumber(index) && index > -1) {
        $scope.currenttasklist.splice(index, 1);
        return;
    }

    var taskIndex = $scope.currenttasklist.findIndex(function (x) {
        return String(x.id) === String(taskid) &&
               $scope.normalizeAssignTaskType(x.tasktype) === $scope.normalizeAssignTaskType(tasktype);
    });

    if (taskIndex > -1) {
        $scope.currenttasklist.splice(taskIndex, 1);
    }
};

$scope.setAssignTreeHiddenByKey = function (taskid, tasktype, hidden) {
    var key = $scope.getAssignTaskKey(taskid, tasktype);
    var node = $scope.assignTreeNodeMap ? $scope.assignTreeNodeMap[key] : null;

    function applyFlag(obj) {
        if (!obj) return;
        obj.isAssignedTemp = hidden === true;
        obj.isRemovedTemp = hidden === true;
        obj.isSelected = false;
    }

    applyFlag(node);
    if (node && node.source) applyFlag(node.source);

    angular.forEach($scope.currenttasklist || [], function (t) {
        if (String(t.id) === String(taskid) &&
            $scope.normalizeAssignTaskType(t.tasktype) === $scope.normalizeAssignTaskType(tasktype)) {
            applyFlag(t);
        }
    });

    if (node) {
        $scope.refreshAssignTreeParentFrom(node.parent || null);
    }
};

$scope.setAssignTreePersistentStateByKey = function (taskid, tasktype, assigned) {
    var key = $scope.getAssignTaskKey(taskid, tasktype);
    var node = $scope.assignTreeNodeMap ? $scope.assignTreeNodeMap[key] : null;

    function applyState(obj) {
        if (!obj) return;
        obj.isassigned = assigned === true;
        obj.IsAssigned = assigned === true;
    }

    applyState(node);
    if (node && node.source) applyState(node.source);

    angular.forEach($scope.currenttasklist || [], function (t) {
        if (String(t.id) === String(taskid) &&
            $scope.normalizeAssignTaskType(t.tasktype) === $scope.normalizeAssignTaskType(tasktype)) {
            applyState(t);
        }
    });
};

$scope.removeCurrentTaskFromRight = function (taskid, tasktype) {
    var matchedTask = null;

    angular.forEach($scope.currenttasklist || [], function (x) {
        if (!matchedTask &&
            String(x.id) === String(taskid) &&
            $scope.normalizeAssignTaskType(x.tasktype) === $scope.normalizeAssignTaskType(tasktype)) {
            matchedTask = x;
        }
    });

    if (!matchedTask && $scope.assignTreeNodeMap) {
        var node = $scope.assignTreeNodeMap[$scope.getAssignTaskKey(taskid, tasktype)];
        if (node && node.source) matchedTask = node.source;
    }

    if (matchedTask) {
        matchedTask._oldAssignedTemp = matchedTask.isAssignedTemp === true;
        matchedTask._oldRemovedTemp = matchedTask.isRemovedTemp === true;
    }

    // Hide from right immediately after successful/attempted drag assignment.
    // If assignment fails, restoreCurrentTaskToRight() will undo this.
    $scope.setAssignTreeHiddenByKey(taskid, tasktype, true);
    $scope.syncSelectedAssignTasks();

    return { item: matchedTask, index: -1, id: taskid, tasktype: tasktype };
};

// Hide a complete dragged selection from right panel immediately.
// Used for multi-select drag so selected rows disappear together, not one-by-one after AJAX completes.
$scope.hideTasksFromRight = function (tasks) {
    var removed = [];
    angular.forEach(tasks || [], function (t) {
        if (!t) return;
        removed.push($scope.removeCurrentTaskFromRight(t.id, t.tasktype));
        t.isSelected = false;
    });
    $scope.syncSelectedAssignTasks();
    if ($scope.$applyAsync) $scope.$applyAsync();
    return removed;
};

$scope.restoreTasksToRight = function (removedList) {
    angular.forEach(removedList || [], function (r) {
        $scope.restoreCurrentTaskToRight(r);
    });
    $scope.syncSelectedAssignTasks();
    if ($scope.$applyAsync) $scope.$applyAsync();
};



$scope.durationHHMMToMinutes = function (duration) {
    duration = String(duration || '0:00');
    var arr = duration.split(':');
    var h = parseInt(arr[0] || '0', 10);
    var m = parseInt(arr[1] || '0', 10);
    if (isNaN(h)) h = 0;
    if (isNaN(m)) m = 0;
    return (h * 60) + m;
};

$scope.getLatestAssignTimePayload = function (seg) {
    seg = seg || {};
    var durationMinutes = parseInt(seg.DurationMinutes || 0, 10);
    var duration = seg.TaskDuration || seg.taskduration || seg.Duration || seg.duration || '';
    if (!duration && durationMinutes > 0) {
        duration = $scope.formatDurationHHMM(durationMinutes);
    }

    var latestStartDate = seg.workDateKey || seg.WorkDateKey || seg.workDate || seg.StartDateKey || seg.Startdate || seg.StartDate || null;
    var latestStartMoment = getAssignWorkDateMoment(latestStartDate);
    if (latestStartMoment.isValid()) {
        latestStartDate = latestStartMoment.format('YYYY-MM-DD');
    }

    return {
        StartTime: seg.StartTime || seg.starttime || '',
        EndTime: seg.EndTime || seg.endtime || '',
        TaskDuration: duration || '',
        taskduration: duration || '',
        DurationMinutes: durationMinutes || $scope.durationHHMMToMinutes(duration || '0:00'),
        Startdate: latestStartDate,
        Targetdate: seg.Targetdate || seg.TargetDate || null
    };
};

    $scope.gettaskparent_details = function (taskid,staktypes,mid,tid) {
        debugger
        if (!taskid) return '';
        var taskType = (staktypes || '').toLowerCase();

        var parentdetails = "";
        var milename = "";
        var taskname = "";
        if (staktypes == "subtask") {
            
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
            if (mindex > -1) {
                milename = $scope.projectlist.miles[mindex].Milestone;
            }
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == tid);
            if (tindex > -1) {
                taskname = $scope.projectlist.miles[mindex].tasks[tindex].taskname;
            }
            

        }
       else if (staktypes == "task") {
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
            if (mindex > -1) {
                milename = $scope.projectlist.miles[mindex].Milestone;
            }
            
        }
        if (milename != '') {
            parentdetails = "Milestone :" + milename;
        }
        if (taskname != '') {
            parentdetails += "</br> Task Name :" + taskname +"</br>";
        }

        return parentdetails;
    };

$scope.getCenterTaskTooltip = function (seg) {
    if (!seg) return '';
    var taskType = String(seg.tasktype || seg.taskType || '').toLowerCase();
    var label = taskType === 'milestone' ? 'Milestone Name' :
        taskType === 'subtask' ? 'Subtask Name' : 'Task Name';
    var itemId = seg.taskid || seg.id;
    var source = $scope.findAssignValidationItem(itemId, taskType, null) || seg;
    var name = seg.taskname || seg.Taskname || seg.Subtaskname || source.Subtaskname ||
        source.taskname || source.Milestone || 'Task';
    var description = source.SubTaskDescription || source.taskdescription || source.TaskDescription ||
        source.miledescription || source.MilestoneDescription || source.Description || source.description ||
        seg.SubTaskDescription || seg.Description || seg.description || '';
    var duration = seg.TaskDuration || seg.taskduration || $scope.formatDurationHHMM(seg.DurationMinutes || 0);
    function enc(v) {
        return String(v == null ? '' : v)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    return '<div class="text-start assign-center-tooltip">' +
        '<div>Duration: ' + enc(duration) + ' hrs</div>' +
        '<div>' + label + ': <strong>' + enc(name) + '</strong></div>' +
        '<div>Description: ' + enc(description || '-').replace(/\r?\n/g, '<br>') + '</div>' +
        '</div>';
};

    $scope.refreshCenterTaskTooltip = function ($event, seg) {
    //debugger
    var el = $event && ($event.currentTarget || $event.target);
    if (!el || !window.bootstrap || !bootstrap.Tooltip) return;
    var title = $scope.getCenterTaskTooltip(seg);
    el.setAttribute('data-bs-title', title);
    el.setAttribute('data-bs-original-title', title);
    var inst = bootstrap.Tooltip.getInstance(el);
    if (inst) inst.dispose();
    new bootstrap.Tooltip(el, {
        html: true,
        customClass: 'assign-center-tooltip-popup',
        container: 'body',
        trigger: 'hover',
        boundary: 'window'
    });
};

$scope.disposeCenterTaskTooltips = function () {
    if (!window.bootstrap || !bootstrap.Tooltip) return;
    document.querySelectorAll('.tooltip').forEach(function (x) {
        if (x && x.parentNode) x.parentNode.removeChild(x);
    });
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
        var inst = bootstrap.Tooltip.getInstance(el);
        if (inst) inst.dispose();
    });
};

// Reset all active tooltip/popover DOM after assign/remove/employee-to-employee move.
// This prevents old tooltip text/position from staying visible after owner/profile changes.
$scope.resetAssignTooltips = function () {
    try {
        if (window.bootstrap) {
            document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                var inst = bootstrap.Tooltip && bootstrap.Tooltip.getInstance(el);
                if (inst) inst.dispose();
            });
            document.querySelectorAll('[data-bs-toggle="popover"]').forEach(function (el) {
                var pop = bootstrap.Popover && bootstrap.Popover.getInstance(el);
                if (pop) pop.dispose();
            });
        }
        if (window.jQuery) {
            try { $('[data-toggle="tooltip"],[data-bs-toggle="tooltip"]').tooltip('dispose'); } catch (e1) { }
            try { $('[data-toggle="popover"],[data-bs-toggle="popover"]').popover('dispose'); } catch (e2) { }
        }
        document.querySelectorAll('.tooltip,.popover').forEach(function (x) {
            if (x && x.parentNode) x.parentNode.removeChild(x);
        });
    } catch (e) { }
};

// Re-create tooltips after Angular adds/updates assignto avatar DOM.
// This fixes tooltip not showing after assign/move updates assignto in scope.
$scope.reinitAssignTooltips = function () {
    try {
        if (!window.bootstrap || !bootstrap.Tooltip) return;
        document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
            try {
                var old = bootstrap.Tooltip.getInstance(el);
                if (old) old.dispose();
                new bootstrap.Tooltip(el, {
                    container: 'body',
                    trigger: 'hover',
                    boundary: 'window'
                });
            } catch (e1) { }
        });
    } catch (e) { }
};

$scope.scheduleAssignToTooltipRefresh = function (delayMs) {
    if ($scope._assignToTooltipTimer) {
        try { $timeout.cancel($scope._assignToTooltipTimer); } catch (e) { }
    }
    $scope._assignToTooltipTimer = $timeout(function () {
        if ($scope.resetAssignTooltips) { $scope.resetAssignTooltips(); }
        if ($scope.reinitAssignTooltips) { $scope.reinitAssignTooltips(); }
    }, angular.isNumber(delayMs) ? delayMs : 150);
};

$scope.removeAssignedTaskFromCenter = function (emp, seg, $event) {
    if ($event) {
        $event.preventDefault();
        $event.stopPropagation();
    }
    if ($scope.resetAssignTooltips) {
        $scope.resetAssignTooltips();
    } else if ($scope.disposeCenterTaskTooltips) {
        $scope.disposeCenterTaskTooltips();
    }

    if (!emp || !seg) return;

    var tasktype = seg.tasktype || seg.taskType || $scope.currenttaskType;
    if (!$scope.requireAssign(tasktype)) return;

    var taskid = seg.taskid || seg.id;
    var assignRowId = seg.assignid || seg.AssignId || seg.assignmentid || taskid;

    var oldSegments = angular.copy(emp.taskSegments || []);
    var oldUserTasks = angular.copy(emp.usertasks || []);

    emp.taskSegments = (emp.taskSegments || []).filter(function (x) {
        return !(String(x.taskid || x.id) === String(taskid) &&
                 String(x.tasktype || x.taskType) === String(tasktype));
    });

    emp.usertasks = (emp.usertasks || []).filter(function (x) {
        return !(String(x.id) === String(taskid) &&
                 String(x.tasktype) === String(tasktype));
    });

    // Update available time immediately on remove from center.
    $scope.recalculateEmployeeAvailableTime(emp);

    if (assignprojectService.Unassigntaskfromuser) {
        assignprojectService.Unassigntaskfromuser(assignRowId, tasktype)
            .then(function (response) {
                if (response.data.d != 1 && response.data.d !== "1") {
                    throw new Error("Remove failed");
                }

                // Center remove means undo temporary assignment in assign window.
                // Do not delete from right panel; show it again.
                var rightTask = null;
                angular.forEach($scope.currenttasklist || [], function (x) {
                    if (!rightTask &&
                        String(x.id) === String(taskid) &&
                        String(x.tasktype) === String(tasktype)) {
                        rightTask = x;
                    }
                });

                var latestTime = $scope.getLatestAssignTimePayload(seg);
                var planningItem = $scope.findAssignDateValidationItem(taskid, tasktype, rightTask);
                delete latestTime.Startdate;
                delete latestTime.Targetdate;
                if (planningItem) {
                    latestTime.Startdate = planningItem.Startdate || planningItem.StartDate || planningItem.startdate || planningItem.startDate || null;
                    latestTime.Targetdate = $scope.getAssignValidationEndDate(planningItem, tasktype);
                }

                if (rightTask) {
                    // When task is removed from center, show it in right list with latest resized/moved time.
                    // Important: use latest center segment values, not old master duration.
                    angular.extend(rightTask, latestTime);
                    rightTask.isAssignedTemp = false;
                    rightTask.isRemovedTemp = false;
                    rightTask.isSelected = false;
                } else {
                    var restoredTask = angular.extend({
                        id: taskid,
                        tasktype: tasktype,
                        owner: '',
                        Subtaskname: seg.taskname || seg.Subtaskname || seg.Taskname || '',
                        Taskname: seg.taskname || seg.Taskname || '',
                        isAssignedTemp: false,
                        isRemovedTemp: false,
                        isSelected: false
                    }, latestTime);
                    $scope.currenttasklist.push(restoredTask);
                }

                // Clear owner from main milestone/task/subtask also after removing from center.
                $scope.syncRemovedOwnerEverywhere(taskid, tasktype);
                $scope.updateMainOwnerByTaskType(taskid, tasktype, '');

                // Undo/hide reversal in right hierarchy after removing from center.
                $scope.setAssignTreePersistentStateByKey(taskid, tasktype, false);
                $scope.setAssignTreeHiddenByKey(taskid, tasktype, false);
                $scope.syncSelectedAssignTasks();
                $scope.reloadAssignEmployeeForDate(emp.id, $scope.toAssignServerDateOnly($scope.selectedDate), 30);

                // Ensure the Kanban card/count moves to Unassigned immediately.
                if ($scope.syncKanbanAssignmentState) {
                    $scope.syncKanbanAssignmentState(taskid, tasktype, null, false);
                }
                if ($scope.syncListAndKanbanViews) {
                    $scope.syncListAndKanbanViews();
                }
                $scope.$applyAsync();
            })
            .catch(function () {
                emp.taskSegments = oldSegments;
                emp.usertasks = oldUserTasks;
                $scope.recalculateEmployeeAvailableTime(emp);
                alert("Unable to remove assigned task.");
            });
    }
};

$scope.restoreCurrentTaskToRight = function (removed) {
    if (!removed) return;

    if (removed.item) {
        removed.item.isAssignedTemp = removed.item._oldAssignedTemp === true ? true : false;
        removed.item.isRemovedTemp = removed.item._oldRemovedTemp === true ? true : false;
        removed.item.isSelected = false;
    }

    if (removed.id && removed.tasktype && !removed.item) {
        $scope.setAssignTreeHiddenByKey(removed.id, removed.tasktype, false);
    } else if (removed.id && removed.tasktype && removed.item && removed.item._oldAssignedTemp !== true && removed.item._oldRemovedTemp !== true) {
        $scope.setAssignTreeHiddenByKey(removed.id, removed.tasktype, false);
    }

    $scope.syncSelectedAssignTasks();
};
    $scope._openAssignTaskRequestId =
        $scope._openAssignTaskRequestId || 0;

    $scope.openAssignTask = function (c, strtype, index) {
        debugger;

        strtype = String(strtype || '').toLowerCase();

        if (!$scope.requireAssign(strtype)) return;
        if (!c) return;

        /*
         * Every popup opening gets a unique request ID.
         * Previous pending response cannot overwrite new popup data.
         */
        var currentRequestId =
            ++$scope._openAssignTaskRequestId;

        /*
         * Dispose old tooltips before clearing timeline.
         */
        if (
            typeof $scope.resetAssignTooltips ===
            'function'
        ) {
            try {
                $scope.resetAssignTooltips();
            } catch (tooltipError) {
                console.log(
                    'Tooltip reset error',
                    tooltipError
                );
            }
        }

        /*
         * Always reset previous Assign Task popup details.
         */
        $scope.isLoading2 = true;
        $scope.isAssignRefreshing = false;

        $scope.currenttaskIndex = index;
        $scope.currenttaskType = strtype;

        $scope.currentFMilestoneId = '';
        $scope.currentFTaskId = '';
        $scope.currentFId = '';

        $scope.assignmentProjectSettings = null;
        $scope.currenttasklist = [];
        $scope.currenttaskteam = [];

        $scope.selectedAssignTasks = [];
        $scope.assignTree = [];
        $scope.assignTreeNodeMap = {};

        $scope.hours = [];
        $scope.gridColumns = [];

        $scope.timelineWidth = 0;
        $scope.timelineStartMoment = null;
        $scope.timelineEndMoment = null;

        /*
         * Reset any temporary assignment state.
         */
        $scope.isreassign = 0;
        $scope.assignSelectedNode = null;
        $scope.assignDragItem = null;

        /*
         * Keep the currently selected calendar date.
         * Uncomment this line when every open should reset to today.
         */
        // $scope.selectedDate = new Date();

        /*
         * Set fresh hierarchy IDs.
         */
        if (strtype === 'milestone') {
            $scope.currentFMilestoneId =
                c.id || '';

            $scope.currentFId =
                c.id || '';
        }
        else if (strtype === 'task') {
            $scope.currentFMilestoneId =
                c.Milestoneid ||
                c.MilestoneId ||
                c.milestoneid ||
                '';

            $scope.currentFTaskId =
                c.id || '';

            $scope.currentFId =
                c.id || '';
        }
        else if (strtype === 'subtask') {
            $scope.currentFMilestoneId =
                c.Milestoneid ||
                c.MilestoneId ||
                c.milestoneid ||
                '';

            $scope.currentFTaskId =
                c.Task ||
                c.TaskId ||
                c.Taskid ||
                c.taskid ||
                '';

            $scope.currentFId =
                c.id || '';
        }

        $http.post('/pms/assignprojects/default.aspx/GetAssignmentProjectSettings', {
            id: String(c.id), utype: strtype
        }).then(function (settingsResponse) {
            if (currentRequestId !== $scope._openAssignTaskRequestId) return null;
            var settings = settingsResponse.data.d;
            if (!settings || !settings.ProjectId || ['milestone', 'task', 'subtask'].indexOf(settings.Level) < 0) {
                throw new Error('Unable to load project assignment settings.');
            }
            $scope.assignmentProjectSettings = settings;
            return assignprojectService.getunassigntasklist(c, strtype);
        }).then(function (response) {

                /*
                 * Ignore response belonging to an older popup click.
                 */
                if (
                    currentRequestId !==
                    $scope._openAssignTaskRequestId
                ) {
                    return null;
                }

                $scope.currenttasklist =
                    response &&
                        response.data
                        ? response.data.d || []
                        : [];

                /*
                 * Clear old temporary states from freshly loaded rows.
                 */
                angular.forEach(
                    $scope.currenttasklist,
                    function (task) {
                        if (!task) return;

                        task.isAssignedTemp = false;
                        task.isRemovedTemp = false;
                        task.isSelected = false;
                        task.checked = false;

                        // Keep one canonical duration contract in the assignment popup.
                        // Example: TaskDuration=2 and TaskDuration_min=30 becomes 2:30.
                        var canonicalDuration = $scope.getTaskDurationText(task);
                        if (canonicalDuration) task.TaskDuration = canonicalDuration;
                    }
                );

                /*
                 * Always rebuild assignment hierarchy from zero.
                 */
                $scope.assignTree = [];
                $scope.assignTreeNodeMap = {};
                $scope.selectedAssignTasks = [];

                $scope.buildAssignTreeFromProjectList(
                    c,
                    strtype
                );

                /*
                 * Safely collect team employee IDs.
                 */
                var teams =
                    angular.isArray(c.teams)
                        ? c.teams
                        : [];

                var userIds = teams
                    .filter(function (employee) {
                        return (
                            employee &&
                            employee.id !== undefined &&
                            employee.id !== null &&
                            String(employee.id).trim() !== ''
                        );
                    })
                    .map(function (employee) {
                        return employee.id;
                    })
                    .join(',');

                /*
                 * No employees mapped: keep center timeline empty.
                 */
                if (!userIds) {
                    $scope.currenttaskteam = [];
                    $scope.hours = [];
                    $scope.gridColumns = [];
                    $scope.timelineWidth = 0;

                    return null;
                }

                var selectedDate =
                    $scope.selectedDate || new Date();

                var checkDate =
                    $scope.toAssignServerDateOnly(
                        selectedDate
                    );

                /*
                 * Return promise so loader closes only after
                 * employee assignments and task segments load.
                 */
                return $scope.getuserwithtasklist(
                    userIds,
                    checkDate
                );
            })
            .then(function () {

                if (
                    currentRequestId !==
                    $scope._openAssignTaskRequestId
                ) {
                    return;
                }

                /*
                 * Sync fresh checkbox state after assignments load.
                 */
                if (
                    typeof $scope.syncSelectedAssignTasks ===
                    'function'
                ) {
                    $scope.syncSelectedAssignTasks();
                }

                /*
                 * Bind drag/drop only for newly loaded popup data.
                 */
                $scope.rebindAssignDragFast(30);
                $scope.$applyAsync();
            })
            .catch(function (error) {

                if (
                    currentRequestId !==
                    $scope._openAssignTaskRequestId
                ) {
                    return;
                }

                console.error(
                    'Open Assign Task failed',
                    error
                );

                /*
                 * Keep popup completely empty after failed request.
                 */
                $scope.currenttasklist = [];
                $scope.currenttaskteam = [];

                $scope.selectedAssignTasks = [];
                $scope.assignTree = [];
                $scope.assignTreeNodeMap = {};

                $scope.hours = [];
                $scope.gridColumns = [];
                $scope.timelineWidth = 0;

                alert(
                    'HTTP ERROR: ' +
                    (
                        error.statusText ||
                        error.message ||
                        'Unable to load assignment details.'
                    )
                );
            })
            .finally(function () {

                /*
                 * Old request must not close loader
                 * for a newly opened Assign Task popup.
                 */
                if (
                    currentRequestId ===
                    $scope._openAssignTaskRequestId
                ) {
                    $scope.isLoading2 = false;
                    $scope.isAssignRefreshing = false;
                    $scope.$applyAsync();
                }
            });
    };
    $scope.openAssignTask_old = function (c, strtype, index) {
        debugger
        //$scope.selectedDate = new Date();
        if (!$scope.requireAssign(strtype)) return;
        $scope.isLoading2 = true;
        $scope.currenttaskIndex = index;

        if (strtype == "milestone") {
            $scope.currentFMilestoneId = c.id;
        }
        else if (strtype == "task" || strtype == "subtask") {
            $scope.currentFMilestoneId = c.Milestoneid;
        }

        $scope.currenttaskType = strtype;
        $scope.currenttasklist = [];

        assignprojectService.getunassigntasklist(c, strtype)
            .then(function (response) {
                $scope.currenttasklist = response.data.d || [];
                angular.forEach($scope.currenttasklist, function(t){
                    t.isAssignedTemp = false;
                    t.isRemovedTemp = false;
                    t.isSelected = false;
                    var canonicalDuration = $scope.getTaskDurationText(t);
                    if (canonicalDuration) t.TaskDuration = canonicalDuration;
                });
                $scope.buildAssignTreeFromProjectList(c, strtype);
                var userIds = c.teams
                    .map(function (x) {
                        return x.id;
                    })
                    .join(',');
// timeline header will be generated from employee shifts after users load
                if (userIds != "") {
                    $scope.getuserwithtasklist(userIds, $scope.toAssignServerDateOnly($scope.selectedDate));
                }
               $scope.isLoading2 = false; 

                

             $scope.rebindAssignDragFast(30);

            })
            .catch(function (error) {

                alert("HTTP ERROR: " +
                    (error.statusText || error.message || "Unknown error"));

            });

    };
// add time slot in header
  $scope.generateTimeSlots = function (startHour, totalHours) {

    $scope.hours = [];

    for (var i = 1; i <= totalHours; i++) {

        var h = startHour + i;

        var hour24 = h % 24;

        var period = hour24 >= 12 ? "PM" : "AM";

        var hour12 = hour24 % 12;
        if (hour12 === 0) hour12 = 12;

        $scope.hours.push({
            hour: ("0" + hour12).slice(-2),
            period: period
        });
    }
    $scope.gridColumns = new Array($scope.hours.length * 4);
};  
    $scope.dateChanged = function () {
    debugger
    var checkdate = $scope.toAssignServerDateOnly($scope.selectedDate);

    if ($scope.currenttaskteam && $scope.currenttaskteam.length) {

        var userIds = $scope.currenttaskteam
            .map(function (x) {
                return x.id;
            })
            .join(',');

        $scope.getuserwithtasklist(userIds, checkdate);
    }
};
$scope.isSegmentVisible = function (segDate) {
    if (!segDate) return false;

    var segmentDay = getAssignWorkDateMoment(segDate);
    var selectedDay = getAssignWorkDateMoment($scope.selectedDate);

    return segmentDay.isValid() &&
        selectedDay.isValid() &&
        segmentDay.isSame(selectedDay, 'day');
};
    $scope.hours = [];
    $scope.shiftStart = 9;   // 9 AM
    $scope.shiftHours = 8;   // 8 hour shift
    
$scope.generateTimeSlots_employee = function (shiftStart, shiftEnd) {
    var start = moment(shiftStart, "hh:mm A");
    var end = moment(shiftEnd, "hh:mm A");
var hours = [];

   

    // Night Shift Handle
    if (end.isSameOrBefore(start)) {
        end.add(1, "day");
    }

    while (start.isBefore(end)) {

        hours.push({
            hour: start.format("hh"),
            minute: start.format("mm"),
            period: start.format("A"),
            label: start.format("hh:mm A")
        });

        start.add(1, "hour");
    }

    return hours;
};

$scope.prepareTimelineEmployee = function(emp) {
    emp.hours = $scope.generateTimeSlots_employee(emp.ShiftStart, emp.ShiftEnd);
    var hourWidth = document.querySelector('.time-col') ? document.querySelector('.time-col').offsetWidth : 100;
    if (!hourWidth || hourWidth === 0) hourWidth = 100;
    // keep row width same as header width, so bars/grid align with header
    emp.timelineWidth = Math.max(($scope.hours || []).length * hourWidth, emp.hours.length * hourWidth);
    emp.gridColumns = new Array(Math.max(1, Math.round(emp.timelineWidth / 25)));
    return emp;
};


// === Employee shift based timeline (fixed) ===
$scope.HOUR_WIDTH = 100;
$scope.timelineStartMoment = null;
$scope.timelineEndMoment = null;
$scope.timelineWidth = 0;

$scope.parseTimeOnDate = function (timeText, baseDate) {
    var m = moment(baseDate || $scope.selectedDate).startOf('day');
    var t = moment((timeText || '09:00 AM').toString().trim(), ['hh:mm A', 'h:mm A', 'HH:mm', 'HH:mm:ss'], true);
    if (!t.isValid()) t = moment((timeText || '09:00 AM').toString().trim(), ['hh:mm A', 'h:mm A', 'HH:mm', 'HH:mm:ss']);
    return m.hour(t.hour()).minute(t.minute()).second(0).millisecond(0);
};

$scope.getSegmentDateMoment = function (seg) {
    var d = seg
        ? (seg.workDateKey || seg.WorkDateKey || seg.workDate || seg.StartDateKey || seg.Startdate || seg.StartDate)
        : null;
    return getAssignWorkDateMoment(d || $scope.selectedDate);
};


$scope.getShiftKey = function (emp) {
    return ((emp.ShiftStart || '') + '-' + (emp.ShiftEnd || '')).toLowerCase().replace(/\s+/g, '');
};

$scope.buildTimelineFromEmployeeShifts = function () {
    var base = moment($scope.selectedDate).startOf('day');
    var minStart = null;
    var maxEnd = null;
    var shiftKeys = [];

    angular.forEach($scope.currenttaskteam || [], function (emp) {
        emp._shiftStartM = $scope.parseTimeOnDate(emp.ShiftStart, base);
        emp._shiftEndM = $scope.parseTimeOnDate(emp.ShiftEnd, base);

        // Night shift support: 09 PM - 06 AM means end is next day.
        if (emp._shiftEndM.isSameOrBefore(emp._shiftStartM)) {
            emp._shiftEndM.add(1, 'day');
        }

        var key = $scope.getShiftKey(emp);
        if (key && shiftKeys.indexOf(key) < 0) shiftKeys.push(key);

        if (!minStart || emp._shiftStartM.isBefore(minStart)) minStart = emp._shiftStartM.clone();
        if (!maxEnd || emp._shiftEndM.isAfter(maxEnd)) maxEnd = emp._shiftEndM.clone();
    });

    if (!minStart || !maxEnd) {
        minStart = base.clone().hour(9);
        maxEnd = base.clone().hour(18);
    }

    $scope.isMultiShift = shiftKeys.length > 1;

    // Single shift: show only that shift hours.
    // Multiple shifts: show fixed 24-hour header from earliest shift start.
    minStart.minute(0).second(0).millisecond(0);
    if ($scope.isMultiShift) {
        maxEnd = minStart.clone().add(24, 'hours');
    } else {
        if (maxEnd.minute() > 0 || maxEnd.second() > 0) {
            maxEnd.add(1, 'hour').minute(0).second(0).millisecond(0);
        }
    }

    $scope.timelineStartMoment = minStart;
    $scope.timelineEndMoment = maxEnd;
    $scope.timelineStartText = minStart.format('hh:mm A');
    $scope.timelineEndText = maxEnd.format('hh:mm A');

    var totalHours = Math.max(1, Math.ceil(maxEnd.diff(minStart, 'minutes') / 60));
    $scope.hours = [];
    for (var i = 0; i < totalHours; i++) {
        var h = minStart.clone().add(i, 'hour');
        $scope.hours.push({
            hour: h.format('hh'),
            period: h.format('A'),
            label: h.format('hh:mm A'),
            dayplus: h.diff(base, 'days') > 0 ? '+1' : ''
        });
    }

    $scope.timelineWidth = totalHours * $scope.HOUR_WIDTH;
    $scope.gridColumns = new Array(totalHours * 4);
};

$scope.applyTimelineToEmployees = function () {
    $scope.buildTimelineFromEmployeeShifts();

    angular.forEach($scope.currenttaskteam || [], function (emp) {
        emp.timelineWidth = $scope.timelineWidth;
        emp.gridColumns = $scope.gridColumns;
        emp.availLeft = Math.max(0, emp._shiftStartM.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);
        emp.availWidth = Math.max(0, emp._shiftEndM.diff(emp._shiftStartM, 'minutes') / 60 * $scope.HOUR_WIDTH);

        angular.forEach(emp.taskSegments || [], function (seg) {
            var segDate = $scope.getSegmentDateMoment(seg).startOf('day');
            var start = $scope.parseTimeOnDate(seg.StartTime || emp.ShiftStart, segDate);
            var empStart = $scope.parseTimeOnDate(emp.ShiftStart, segDate);
            var empEnd = $scope.parseTimeOnDate(emp.ShiftEnd, segDate);
            if (empEnd.isSameOrBefore(empStart)) empEnd.add(1, 'day');
            if (start.isBefore(empStart) && empEnd.date() !== empStart.date()) start.add(1, 'day');

            var minutes = parseInt(seg.DurationMinutes || 0, 10);
            if (!minutes && seg.EndTime) {
                var end = $scope.parseTimeOnDate(seg.EndTime, segDate);
                if (end.isSameOrBefore(start)) end.add(1, 'day');
                minutes = Math.max(0, end.diff(start, 'minutes'));
            }
            seg.left = Math.max(0, start.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);
            seg.width = Math.max(18, (minutes / 60 * $scope.HOUR_WIDTH) - 4); // 4px gap between tasks
        });
        $scope.recalculateEmployeeAvailableTime(emp);
    });

};

// === Assign window center panel: move + resize task time in same employee row only ===
$scope.centerMinMinutes = 15;
$scope.centerMinWidthMinutes = 15;

$scope.roundCenterMinutes = function (minutes) {
    var step = $scope.centerMinMinutes || 15;
    return Math.round(minutes / step) * step;
};

$scope.formatDurationHHMM = function (minutes) {
    minutes = Math.max(0, parseInt(minutes || 0, 10));
    var h = Math.floor(minutes / 60);
    var m = minutes % 60;
    return ("0" + h).slice(-2) + ":" + ("0" + m).slice(-2);
};


// Return all visible segments belonging to the same assignment row.
$scope.getSegmentsForSameAssign = function (emp, seg) {
    if (!emp || !seg) return [];
    var assignId = seg.assignid || seg.AssignId || seg.assignmentid || seg.taskid || seg.id;
    var taskId = seg.taskid || seg.id || seg.Task || seg.subtaskid || seg.taskId;
    var taskType = String(seg.tasktype || seg.taskType || seg.type || '').toLowerCase();

    var list = [];
    angular.forEach(emp.taskSegments || [], function (x) {
        var xid = x.assignid || x.AssignId || x.assignmentid || x.taskid || x.id;
        var xtaskId = x.taskid || x.id || x.Task || x.subtaskid || x.taskId;
        var xtype = String(x.tasktype || x.taskType || x.type || '').toLowerCase();
        if ((assignId && String(xid) === String(assignId)) || (!assignId && String(xtaskId) === String(taskId) && xtype === taskType)) {
            list.push(x);
        }
    });

    list.sort(function (a, b) {
        var da = $scope.getSegmentDateMoment(a);
        var db = $scope.getSegmentDateMoment(b);
        if (da.isBefore(db)) return -1;
        if (da.isAfter(db)) return 1;
        var sa = moment(a.StartTime || '12:00 AM', ['hh:mm A', 'h:mm A', 'HH:mm']);
        var sb = moment(b.StartTime || '12:00 AM', ['hh:mm A', 'h:mm A', 'HH:mm']);
        return sa.diff(sb, 'minutes');
    });
    return list;
};

$scope.isLastVisibleSegmentOfAssign = function (emp, seg) {
    var list = $scope.getSegmentsForSameAssign(emp, seg);
    if (list.length <= 1) return true;
    return list[list.length - 1] === seg;
};

$scope.getCumulativeDurationForAssign = function (emp, seg) {
    var total = 0;
    angular.forEach($scope.getSegmentsForSameAssign(emp, seg), function (x) {
        total += parseInt(x.DurationMinutes || 0, 10) || 0;
    });
    return total;
};

// Fixed left column width for employee available time in center scheduler.
$scope.availableTimeColWidth = 85;

$scope.getSelectedWorkDateKey = function () {
    var selected = getAssignWorkDateMoment($scope.selectedDate);
    return selected.isValid() ? selected.format('YYYY-MM-DD') : moment().format('YYYY-MM-DD');
};

$scope.minutesToAvailableLabel = function (minutes) {
    minutes = Math.max(0, parseInt(minutes || 0, 10));
    var h = Math.floor(minutes / 60);
    var m = minutes % 60;
    if (h <= 0 && m <= 0) return '00:00';
    return ("0" + h).slice(-2) + ':' + ("0" + m).slice(-2);
};

$scope.recalculateEmployeeAvailableTime = function (emp) {
    if (!emp) return;

    var shiftMinutes = 0;
    try {
        var ss = emp._shiftStartM ? emp._shiftStartM.clone() : moment(emp.ShiftStart || '09:00 AM', ['hh:mm A', 'h:mm A', 'HH:mm']);
        var se = emp._shiftEndM ? emp._shiftEndM.clone() : moment(emp.ShiftEnd || '06:00 PM', ['hh:mm A', 'h:mm A', 'HH:mm']);
        if (se.isSameOrBefore(ss)) se.add(1, 'day');
        shiftMinutes = Math.max(0, se.diff(ss, 'minutes'));
    } catch (ex) {
        shiftMinutes = parseInt(emp.ShiftHours || 0, 10) * 60;
    }

    var selectedKey = $scope.getSelectedWorkDateKey();
    var bookedMinutes = 0;

    angular.forEach(emp.taskSegments || [], function (seg) {
        try {
            var segKey = $scope.getSegmentDateMoment(seg).format('YYYY-MM-DD');
            if (segKey === selectedKey) {
                bookedMinutes += parseInt(seg.DurationMinutes || 0, 10) || 0;
            }
        } catch (e) {}
    });

    emp.BookedMinutes = bookedMinutes;
    emp.AvailableMinutes = Math.max(0, shiftMinutes - bookedMinutes);
    emp.AvailableTime = $scope.minutesToAvailableLabel(emp.AvailableMinutes);
    emp.AvailableTitle = 'Shift: ' + $scope.minutesToAvailableLabel(shiftMinutes) +
        ' | Booked: ' + $scope.minutesToAvailableLabel(bookedMinutes) +
        ' | Available: ' + emp.AvailableTime;
};

$scope.recalculateAllAvailableTimes = function () {
    angular.forEach($scope.currenttaskteam || [], function (emp) {
        $scope.recalculateEmployeeAvailableTime(emp);
    });
};

$scope.updateCenterSegmentTime = function (emp, seg, startM, durationMinutes) {
    if (!emp || !seg || !startM) return;

    durationMinutes = Math.max($scope.centerMinWidthMinutes || 15, parseInt(durationMinutes || 0, 10));
    var endM = startM.clone().add(durationMinutes, 'minutes');

    seg.StartTime = startM.format('hh:mm A');
    seg.EndTime = endM.format('hh:mm A');
    seg.DurationMinutes = durationMinutes;
    seg.TaskDuration = $scope.formatDurationHHMM(durationMinutes);
    seg.taskduration = seg.TaskDuration;

    // In multi-shift view header can start before employee shift.
    // Always calculate left from common timeline header start.
    seg.left = Math.max(0, startM.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);
    seg.width = Math.max(18, (durationMinutes / 60 * $scope.HOUR_WIDTH) - 4);

    // keep source task duration in sync if available
    angular.forEach(emp.usertasks || [], function (x) {
        if (String(x.id) === String(seg.taskid || seg.id) && String(x.tasktype) === String(seg.tasktype || seg.taskType)) {
            x.StartTime = seg.StartTime;
            x.EndTime = seg.EndTime;
            x.TaskDuration = seg.TaskDuration;
            x.taskduration = seg.TaskDuration;
        }
    });

    // Update Available Time immediately while moving/resizing before server reload.
    $scope.recalculateEmployeeAvailableTime(emp);
};


// Sync move/resize time changes everywhere immediately:
// 1) center employee task list
// 2) main project grid hierarchy
// 3) right assign tree/currenttasklist panel
$scope.splitDurationHHMM = function (duration) {
    duration = String(duration || '00:00');
    var arr = duration.split(':');
    var h = parseInt(arr[0] || '0', 10);
    var m = parseInt(arr[1] || '0', 10);
    if (isNaN(h)) h = 0;
    if (isNaN(m)) m = 0;
    return {
        hh: ("0" + h).slice(-2),
        mm: ("0" + m).slice(-2),
        hours: String(h),
        minutes: ("0" + m).slice(-2),
        full: ("0" + h).slice(-2) + ":" + ("0" + m).slice(-2)
    };
};

$scope.applyTimeDurationToItem = function (item, startTime, taskDuration, mode) {
    if (!item) return;

    var d = $scope.splitDurationHHMM(taskDuration);

    item.StartTime = startTime;
    item.starttime = startTime;

    // Main editable grid uses separate hour/minute inputs.
    // Right panel and assign source objects use HH:MM text.
    if (mode === 'main') {
        item.TaskDuration = d.hours;
        item.TaskDuration_min = d.minutes;
        item.taskduration = d.full;
        item.DurationMinutes = (parseInt(d.hours || '0', 10) * 60) + parseInt(d.minutes || '0', 10);
    } else {
        item.TaskDuration = d.full;
        item.TaskDuration_min = d.minutes;
        item.taskduration = d.full;
        item.DurationMinutes = (parseInt(d.hours || '0', 10) * 60) + parseInt(d.minutes || '0', 10);
    }
};

$scope.syncMovedResizedTaskEverywhere = function (taskid, tasktype, startTime, taskDuration) {
    tasktype = String(tasktype || '').toLowerCase();

    // Right flat source list
    angular.forEach($scope.currenttasklist || [], function (x) {
        if (String(x.id) === String(taskid) && String(x.tasktype || '').toLowerCase() === tasktype) {
            $scope.applyTimeDurationToItem(x, startTime, taskDuration, 'right');
        }
    });

    // Selected items cache
    angular.forEach($scope.selectedAssignTasks || [], function (x) {
        if (String(x.id) === String(taskid) && String(x.tasktype || '').toLowerCase() === tasktype) {
            $scope.applyTimeDurationToItem(x, startTime, taskDuration, 'right');
        }
    });

    // Main project hierarchy
    angular.forEach(($scope.projectlist && $scope.projectlist.miles) || [], function (mile) {
        if (tasktype === 'milestone' && String(mile.id) === String(taskid)) {
            $scope.applyTimeDurationToItem(mile, startTime, taskDuration, 'main');
        }

        angular.forEach(mile.tasks || [], function (task) {
            if (tasktype === 'task' && String(task.id) === String(taskid)) {
                $scope.applyTimeDurationToItem(task, startTime, taskDuration, 'main');
            }

            angular.forEach(task.subtasks || [], function (sub) {
                if (tasktype === 'subtask' && String(sub.id) === String(taskid)) {
                    $scope.applyTimeDurationToItem(sub, startTime, taskDuration, 'main');
                }
            });
        });
    });

    // Right hierarchy tree
    function syncNode(node) {
        if (!node) return;
        if (String(node.id) === String(taskid) && String(node.type || '').toLowerCase() === tasktype) {
            if (node.source) $scope.applyTimeDurationToItem(node.source, startTime, taskDuration, 'right');
            if (node.item) $scope.applyTimeDurationToItem(node.item, startTime, taskDuration, 'main');
        }
        angular.forEach(node.children || [], syncNode);
    }

    angular.forEach($scope.assignTree || [], syncNode);

    if ($scope.syncSelectedAssignTasks) {
        $scope.syncSelectedAssignTasks();
    }
};



// Find actual item from main project hierarchy by type/id.
$scope.findMainHierarchyItemByAssign = function (taskid, tasktype) {
    tasktype = String(tasktype || '').toLowerCase();

    var found = null;
    angular.forEach(($scope.projectlist && $scope.projectlist.miles) || [], function (mile) {
        if (found) return;

        if (tasktype === 'milestone' && String(mile.id) === String(taskid)) {
            found = mile;
            return;
        }

        angular.forEach(mile.tasks || [], function (task) {
            if (found) return;

            if (tasktype === 'task' && String(task.id) === String(taskid)) {
                found = task;
                return;
            }

            angular.forEach(task.subtasks || [], function (sub) {
                if (found) return;
                if (tasktype === 'subtask' && String(sub.id) === String(taskid)) {
                    found = sub;
                }
            });
        });
    });

    return found;
};

// Use existing main-view duration update method for resize changes.
// This keeps main grid, DB TaskDuration column, calculations and validation same as manual edit.
$scope.updateAssignResizeDurationUsingMainUpdateHrs = function (taskid, tasktype, startTime, taskDuration) {
    var item = $scope.findMainHierarchyItemByAssign(taskid, tasktype);
    if (!item || !$scope.update_hrs) return;

    $scope.applyTimeDurationToItem(item, startTime, taskDuration, 'main');

    try {
        item._oldTaskDuration = item.TaskDuration;
        item._oldTaskDuration_min = item.TaskDuration_min;
        $scope.update_hrs(item, String(tasktype || '').toLowerCase());
    }
    catch (ex) {
        console.error('update_hrs failed from assign resize', ex);
    }
};


// Validate resize duration before save.
// Rule:
//   Subtask resize  => Sum of all subtasks duration must be <= parent task duration
//   Task resize     => Sum of all tasks duration must be <= parent milestone duration
//   Milestone resize=> Sum of all milestones duration must be <= project duration (if project duration exists)
$scope.getDurationMinutesForValidation = function (item) {
    if (!item) return 0;

    function toInt(v) {
        var n = parseInt(v || '0', 10);
        return isNaN(n) ? 0 : n;
    }

    // Main grid normally stores hour and minute separately.
    if (item.TaskDuration_min !== undefined && item.TaskDuration_min !== null && String(item.TaskDuration || '').indexOf(':') < 0) {
        return (toInt(item.TaskDuration) * 60) + toInt(item.TaskDuration_min);
    }

    // Right panel / assigned segments normally use HH:MM.
    var d = item.TaskDuration || item.taskduration || item.Duration || item.duration || '';
    if (String(d).indexOf(':') >= 0) {
        var arr = String(d).split(':');
        return (toInt(arr[0]) * 60) + toInt(arr[1]);
    }

    if (item.DurationMinutes !== undefined && item.DurationMinutes !== null) {
        return toInt(item.DurationMinutes);
    }

    // Some project/milestone rows may contain calculated hours.
    if (item.Durationhourcal !== undefined && item.Durationhourcal !== null && item.Durationhourcal !== '') {
        var h = parseFloat(item.Durationhourcal || 0);
        return isNaN(h) ? 0 : Math.round(h * 60);
    }

    return toInt(d) * 60;
};

$scope.findHierarchyContextByAssign = function (taskid, tasktype) {
    tasktype = String(tasktype || '').toLowerCase();
    var ctx = { project: $scope.projectlist || null, milestone: null, task: null, subtask: null, item: null };

    angular.forEach((($scope.projectlist && $scope.projectlist.miles) || []), function (mile) {
        if (ctx.item) return;

        if (tasktype === 'milestone' && String(mile.id) === String(taskid)) {
            ctx.milestone = mile;
            ctx.item = mile;
            return;
        }

        angular.forEach(mile.tasks || [], function (task) {
            if (ctx.item) return;

            if (tasktype === 'task' && String(task.id) === String(taskid)) {
                ctx.milestone = mile;
                ctx.task = task;
                ctx.item = task;
                return;
            }

            angular.forEach(task.subtasks || [], function (sub) {
                if (ctx.item) return;
                if (tasktype === 'subtask' && String(sub.id) === String(taskid)) {
                    ctx.milestone = mile;
                    ctx.task = task;
                    ctx.subtask = sub;
                    ctx.item = sub;
                }
            });
        });
    });

    return ctx;
};

$scope.validateResizeDurationByParent = function (taskid, tasktype, newDurationMinutes, suppressAlert) {
    tasktype = String(tasktype || '').toLowerCase();
    newDurationMinutes = parseInt(newDurationMinutes || 0, 10);
    if (isNaN(newDurationMinutes)) newDurationMinutes = 0;
    $scope._lastResizeDurationValidationMessage = '';

    var ctx = $scope.findHierarchyContextByAssign(taskid, tasktype);
    if (!ctx || !ctx.item) {
        return true;
    }

    function fail(message) {
        $scope._lastResizeDurationValidationMessage = message;
        if (!suppressAlert) alert(message);
        return false;
    }

    if (tasktype === 'subtask') {
        var parentTask = ctx.task;
        if (!parentTask) return true;

        var totalSubMinutes = 0;
        angular.forEach(parentTask.subtasks || [], function (subtask) {
            totalSubMinutes += String(subtask.id) === String(taskid)
                ? newDurationMinutes
                : $scope.getDurationMinutesForValidation(subtask);
        });

        var taskMinutes = $scope.getDurationMinutesForValidation(parentTask);
        if (taskMinutes > 0 && totalSubMinutes > taskMinutes) {
            return fail('Total subtask duration cannot be greater than task duration.');
        }
    }

    if (tasktype === 'task') {
        var ownSubtaskMinutes = 0;
        angular.forEach((ctx.item && ctx.item.subtasks) || [], function (subtask) {
            ownSubtaskMinutes += $scope.getDurationMinutesForValidation(subtask);
        });
        if (ownSubtaskMinutes > newDurationMinutes) {
            return fail('Total subtask duration cannot be greater than task duration.');
        }

        var parentMile = ctx.milestone;
        if (!parentMile) return true;

        var totalTaskMinutes = 0;
        angular.forEach(parentMile.tasks || [], function (task) {
            totalTaskMinutes += String(task.id) === String(taskid)
                ? newDurationMinutes
                : $scope.getDurationMinutesForValidation(task);
        });

        var mileMinutes = $scope.getDurationMinutesForValidation(parentMile);
        if (mileMinutes > 0 && totalTaskMinutes > mileMinutes) {
            return fail('Total task duration cannot be greater than milestone duration.');
        }
    }

    if (tasktype === 'milestone') {
        var ownTaskMinutes = 0;
        angular.forEach((ctx.item && ctx.item.tasks) || [], function (task) {
            ownTaskMinutes += $scope.getDurationMinutesForValidation(task);
        });
        if (ownTaskMinutes > newDurationMinutes) {
            return fail('Total task duration cannot be greater than milestone duration.');
        }

        var project = ctx.project || $scope.projectlist || {};
        var projectMinutes = $scope.getDurationMinutesForValidation(project);
        if (projectMinutes > 0) {
            var totalMileMinutes = 0;
            angular.forEach(project.miles || [], function (milestone) {
                totalMileMinutes += String(milestone.id) === String(taskid)
                    ? newDurationMinutes
                    : $scope.getDurationMinutesForValidation(milestone);
            });

            if (totalMileMinutes > projectMinutes) {
                return fail('Total milestone duration cannot be greater than project duration.');
            }
        }
    }

    return true;
};

$scope.restoreCenterSegmentTime = function (seg, backup) {
    if (!seg || !backup) return;
    seg.StartTime = backup.StartTime;
    seg.EndTime = backup.EndTime;
    seg.DurationMinutes = backup.DurationMinutes;
    seg.TaskDuration = backup.TaskDuration;
    seg.taskduration = backup.taskduration;
    seg.left = backup.left;
    seg.width = backup.width;
};


// Assignment drop/move must NOT change the original owner in main milestone/task/subtask tables.
// Assigned employee is saved only in Pms_tbl_assigntasks.ownerid.
$scope.updateMainOwnerByTaskType = function (taskid, tasktype, ownerid) {
    return Promise.resolve('1');
};

// Return owner object from master owner list by id.
$scope.getOwnerObjectByIdForAssign = function (ownerid) {
    ownerid = String(ownerid || '').split(',')[0];
    var found = null;
    angular.forEach($scope.owners || [], function (o) {
        if (!found && String(o.id) === ownerid) {
            found = o;
        }
    });
    return found;
};

// Scope-only assignto sync.
// This does NOT update original Owner and does NOT call DB.
// It updates only obj.assignto in Angular scope for milestone/task/subtask.
$scope.setScopeAssignToOnly = function (obj, emp) {
    if (!obj) return;

    var empId = '';
    if (emp && angular.isObject(emp)) {
        empId = emp.id || emp.empid || emp.ownerid || '';
    } else {
        empId = emp || '';
    }
    
        obj.assignto = String(empId || '');
    
};

$scope.clearScopeAssignToOnly = function (obj) {
    if (!obj) return;
    obj.assignto = '';
};

$scope.syncScopeAssignToOnlyEverywhere = function (taskid, tasktype, emp, clearOnly) {
    tasktype = String(tasktype || '').toLowerCase();

    function getObjId(obj) {
        if (!obj) return '';
        return obj.id || obj.taskid || obj.taskId || obj.Task || obj.subtaskid || '';
    }

    function getObjType(obj) {
        if (!obj) return '';
        return String(obj.tasktype || obj.taskType || obj.type || '').toLowerCase();
    }

    function same(obj) {
        if (!obj) return false;
        var objType = getObjType(obj);
        if (!objType) objType = tasktype;
        return String(getObjId(obj)) === String(taskid) && objType === tasktype;
    }

    function apply(obj) {
        if (!obj) return;
        if (clearOnly === true) $scope.clearScopeAssignToOnly(obj);
        else $scope.setScopeAssignToOnly(obj, emp);
    }

    // Right list cache
    angular.forEach($scope.currenttasklist || [], function (x) {
        if (same(x)) apply(x);
    });

    // Selected task cache
    angular.forEach($scope.selectedAssignTasks || [], function (x) {
        if (same(x)) apply(x);
    });

    // Main project hierarchy
    angular.forEach(($scope.projectlist && $scope.projectlist.miles) || [], function (mile) {
        if (tasktype === 'milestone' && String(mile.id) === String(taskid)) apply(mile);

        angular.forEach(mile.tasks || [], function (task) {
            if (tasktype === 'task' && String(task.id) === String(taskid)) apply(task);

            angular.forEach(task.subtasks || [], function (sub) {
                if (tasktype === 'subtask' && String(sub.id) === String(taskid)) apply(sub);
            });
        });
    });

    // Right hierarchy tree
    function syncNode(node) {
        if (!node) return;
        if (String(node.id) === String(taskid) && String(node.type || '').toLowerCase() === tasktype) {
            apply(node);
            apply(node.source);
            apply(node.item);
        }
        angular.forEach(node.children || [], syncNode);
    }
    angular.forEach($scope.assignTree || [], syncNode);

    // Center employee rows
    angular.forEach($scope.currenttaskteam || [], function (employeeRow) {
        angular.forEach(employeeRow.usertasks || [], function (x) {
            if (same(x)) apply(x);
        });

        angular.forEach(employeeRow.taskSegments || [], function (x) {
            if (same(x)) apply(x);
        });
    });

    if ($scope.syncSelectedAssignTasks) {
        $scope.syncSelectedAssignTasks();
    }
    if ($scope.$applyAsync) {
        $scope.$applyAsync();
    }

    // Rebind tooltip after assignto avatar appears/changes in Angular DOM.
    if ($scope.scheduleAssignToTooltipRefresh) {
        $scope.scheduleAssignToTooltipRefresh(180);
    }

    // Assignment/remove done from scheduler, list or Kanban must reflect everywhere.
    if ($scope.syncListAndKanbanViews) {
        $scope.syncListAndKanbanViews();
    }
};

// Center remove must only remove/undo scheduler assignment.
// Do not clear original Owner. Clear only scope assignto.
// Keep the already-rendered Kanban wrapper and its source object in sync.
// This makes a card move immediately between All / Unassigned / Assigned
// after assign or remove, without reopening Kanban or reloading the project.
$scope.syncKanbanAssignmentState = function (taskid, tasktype, emp, isAssigned) {
    tasktype = String(tasktype || '').toLowerCase();

    var empId = '';
    if (isAssigned) {
        if (emp && angular.isObject(emp)) {
            empId = emp.id || emp.empid || emp.ownerid || '';
        } else {
            empId = emp || '';
        }
    }

    angular.forEach($scope.kanbanItems || [], function (item) {
        if (!item) return;

        var itemType = String(item.tasktype || item.type || '').toLowerCase();
        var itemId = item.id;

        if (String(itemId) !== String(taskid) || itemType !== tasktype) return;

        item.assignto = isAssigned ? String(empId || '') : '';
        item.isassigned = isAssigned ? 1 : 0;
        item.isreassign = isAssigned ? 1 : 0;

        if (item.source) {
            item.source.assignto = item.assignto;
            item.source.isassigned = item.isassigned;
            item.source.isreassign = item.isreassign;
        }
    });

    // Rebuild wrappers from the shared hierarchy so counts and current activetype
    // are recalculated immediately in the open Kanban view.
    if ($scope.view === 'kanban') {
        $scope._kanbanProjectRef = null;
        $scope.prepareKanbanItems();
    }

    $scope.$applyAsync();
};

$scope.syncRemovedOwnerEverywhere = function (taskid, tasktype) {
    $scope.syncScopeAssignToOnlyEverywhere(taskid, tasktype, null, true);
    $scope.syncKanbanAssignmentState(taskid, tasktype, null, false);
};

// Assignment drop/move must not overwrite original Owner.
// Update only Angular scope field: assignto.
$scope.syncAssignedOwnerEverywhere = function (taskid, tasktype, emp) {
    $scope.syncScopeAssignToOnlyEverywhere(taskid, tasktype, emp, false);
    $scope.syncKanbanAssignmentState(taskid, tasktype, emp, true);
};



$scope.findAssignTimelineRowFromPoint = function (clientX, clientY) {
    var rows = document.querySelectorAll('.timeline-row');
    var found = null;
    angular.forEach(rows, function (row) {
        var r = row.getBoundingClientRect();
        if (clientX >= r.left && clientX <= r.right && clientY >= r.top && clientY <= r.bottom) {
            found = row;
        }
    });
    return found;
};

$scope.getAssignTimelineLeftFromMouse = function (row, clientX) {
    if (!row) return 0;
    var r = row.getBoundingClientRect();
    var availableOffset = parseFloat($scope.availableTimeColWidth || 0) || 0;
    var rawLeft = Math.max(0, clientX - r.left - availableOffset);
    var empid = row.getAttribute('data-empid');
    var emp = ($scope.currenttaskteam || []).find(function (e) { return String(e.id) === String(empid); });
    var empStart = emp && emp._shiftStartM ? emp._shiftStartM.clone() : $scope.parseTimeOnDate(row.getAttribute('data-shiftstart') || '09:00 AM', $scope.selectedDate);
    var empEnd = emp && emp._shiftEndM ? emp._shiftEndM.clone() : $scope.parseTimeOnDate(row.getAttribute('data-shiftend') || '06:00 PM', $scope.selectedDate);
    if (empEnd.isSameOrBefore(empStart)) empEnd.add(1, 'day');
    var minLeft = Math.max(0, empStart.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);
    var maxLeft = Math.max(minLeft, empEnd.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);
    return Math.max(minLeft, Math.min(rawLeft, maxLeft - Math.max(10, ($scope.HOUR_WIDTH / 4))));
};

$scope.timelineLeftToAssignTime = function (left) {
    var m = ($scope.timelineStartMoment ? $scope.timelineStartMoment.clone() : moment($scope.selectedDate).hour(9).minute(0));
    return m.add($scope.roundCenterMinutes((left / $scope.HOUR_WIDTH) * 60), 'minutes').format('hh:mm A');
};

$scope.refreshAssignTeamRows = function () {
    var ids = ($scope.currenttaskteam || []).map(function (x) { return x.id; }).join(',');
    if (!ids) return;
    var checkdate = $scope.toAssignServerDateOnly($scope.selectedDate);
    $scope.getuserwithtasklist(ids, checkdate);
};

// Fast assign-window refresh helpers.
// This keeps render fast after date change, drop, remove, move, and resize.
$scope._assignReloadSeq = 0;
$scope.rebindAssignDragFast = function (delayMs) {
    if ($scope._assignRebindTimer) {
        try { $timeout.cancel($scope._assignRebindTimer); } catch (e) { }
    }
    $scope._assignRebindTimer = $timeout(function () {
        try {
            if (window.interact) {
                interact('.external-task').unset();
                interact('.drag-item').unset();
            }
        } catch (e1) { }
        try { enableSheetTask(); } catch (e2) { }
        try { enableRightDrag($scope); } catch (e3) { }
    }, angular.isNumber(delayMs) ? delayMs : 30);
};
$scope.replaceAssignEmployee = function (empid, nelist) {
    if (!nelist) return;
    $scope.prepareTimelineEmployee(nelist);
    var idx = ($scope.currenttaskteam || []).findIndex(function (m) {
        return String(m.id) === String(empid);
    });
    if (idx > -1) {
        $scope.currenttaskteam.splice(idx, 1, angular.copy(nelist));
    } else {
        $scope.currenttaskteam.push(angular.copy(nelist));
    }
};
$scope.reloadAssignEmployeeForDate = function (empid, checkdate, delayMs) {
    if (!empid) return Promise.resolve();
    checkdate = checkdate || $scope.toAssignServerDateOnly($scope.selectedDate);
    return assignprojectService.getuserwithtasklist_bysingleid(empid, checkdate).then(function (response) {
        var nelist = response && response.data ? response.data.d : null;
        if (nelist) {
            $scope.replaceAssignEmployee(empid, nelist);
            $scope.applyTimelineToEmployees();
            if ($scope.resetAssignTooltips) { $scope.resetAssignTooltips(); }
            if ($scope.scheduleAssignToTooltipRefresh) { $scope.scheduleAssignToTooltipRefresh(180); }
            $scope.rebindAssignDragFast(delayMs || 30);
            $scope.$applyAsync();
        }
        return nelist;
    });
};

$scope.moveAssignedSegmentToEmployee = function (oldEmp, newEmpId, seg, newLeft) {
    if (!seg || !newEmpId) return;
    var assignId = seg.assignid || seg.AssignId || seg.assignmentid;
    if (!assignId) {
        alert('Assignment id not found. Please reload and try again.');
        return;
    }
    var taskItemId = seg.taskid || seg.id || seg.Task || seg.subtaskid || seg.taskId;
    var taskType = seg.tasktype || seg.taskType || seg.type;
    if (!$scope.requireAssign(taskType)) return;
    var newEmp = ($scope.currenttaskteam || []).find(function (e) { return String(e.id) === String(newEmpId); }) || { id: newEmpId };

    var startTime = $scope.timelineLeftToAssignTime(newLeft || 0);
    var changedate = '';
    try { changedate = $scope.getSegmentDateMoment(seg).format('YYYY-MM-DD'); }
    catch (e) { changedate = $scope.toAssignServerDateOnly($scope.selectedDate); }

    var moveValidationItem = $scope.findAssignValidationItem(taskItemId, taskType, seg);
    if (!$scope.validateAssignSelectedDateWithTaskEndDate([moveValidationItem || seg], changedate)) {
        $scope.refreshAssignTeamRows();
        return;
    }

    assignprojectService.UpdateAssignOwnerTime(assignId, newEmpId, changedate, startTime)
        .then(function (response) {
            var result = response && response.data ? response.data.d : '0';
            if (result !== '1' && result !== 1) {
                alert(result || 'Unable to move task to another employee.');
                $scope.refreshAssignTeamRows();
                return;
            }

            // Move from one user to another must update only assignto in scope.
            // It must not update original Owner in DB/main records.
            if (seg) {
                seg.assignto = String(newEmpId || '');
            }
            if (taskItemId && taskType) {
                $scope.syncAssignedOwnerEverywhere(taskItemId, taskType, newEmpId);
                return $scope.updateMainOwnerByTaskType(taskItemId, taskType, newEmpId).then(function () {
                    $scope.refreshAssignTeamRows();
                    if ($scope.scheduleAssignToTooltipRefresh) { $scope.scheduleAssignToTooltipRefresh(220); }
                });
            }

            $scope.refreshAssignTeamRows();
            if ($scope.scheduleAssignToTooltipRefresh) { $scope.scheduleAssignToTooltipRefresh(220); }
        }, function () {
            alert('Unable to move task to another employee.');
            $scope.refreshAssignTeamRows();
        });
};



// Center move overlap helpers: used only for center-to-center move.
// It ignores the assignment currently being moved, so dropping in a free area does not show a false overlap popup.
$scope.getCenterDropConflictInfo = function (row, timelineLeft, width, ignoreAssignId) {
    var info = { ok: false, overlap: false, outsideShift: false, reason: '' };
    if (!row) { info.reason = 'No employee row found.'; return info; }

    var availableOffset = parseFloat($scope.availableTimeColWidth || 0) || 0;
    var taskLeft = availableOffset + Math.max(0, parseFloat(timelineLeft || 0));

    var strip = row.querySelector('.available-strip');
    if (!strip) { info.outsideShift = true; info.reason = 'Employee shift/free strip not found.'; return info; }

    var stripLeft = parseFloat(strip.style.left || strip.offsetLeft || 0);
    var stripWidth = parseFloat(strip.style.width || strip.offsetWidth || 0);
    var stripRight = stripLeft + stripWidth;

    // For multi-day scheduling, the full task width may continue to next day.
    // Only the START point must be inside the current shift.
    if (taskLeft < stripLeft || taskLeft >= stripRight) {
        info.outsideShift = true;
        info.reason = 'Drop is outside employee shift time.';
        return info;
    }

    var planned = row.querySelectorAll('.center-planned-time,.planned-time,.task-bar');
    for (var i = 0; i < planned.length; i++) {
        var el = planned[i];
        if (!el || el.classList.contains('assign-center-drag-preview') || el.classList.contains('assign-drag-preview')) continue;

        var elAssignId = el.getAttribute('data-assignid') || '';
        if (ignoreAssignId && elAssignId && String(elAssignId) === String(ignoreAssignId)) continue;

        var elLeft = parseFloat(el.style.left || el.offsetLeft || 0);
        var elWidth = parseFloat(el.style.width || el.offsetWidth || 0);
        var elRight = elLeft + elWidth;
        var probeWidth = Math.max(6, (($scope.HOUR_WIDTH || 100) / 16));
        var probeRight = Math.min(taskLeft + probeWidth, stripRight);

        // Check only the drop START point. Long duration can continue to next day.
        if (taskLeft < elRight && probeRight > elLeft) {
            info.overlap = true;
            info.reason = 'Drop overlaps existing task.';
            return info;
        }
    }

    info.ok = true;
    return info;
};

$scope.findCenterFreeTimelineLeft = function (row, preferredTimelineLeft, width, ignoreAssignId) {
    if (!row) return null;
    var availableOffset = parseFloat($scope.availableTimeColWidth || 0) || 0;
    var strip = row.querySelector('.available-strip');
    if (!strip) return null;

    var stripLeft = parseFloat(strip.style.left || strip.offsetLeft || 0);
    var stripWidth = parseFloat(strip.style.width || strip.offsetWidth || 0);
    var stripRight = stripLeft + stripWidth;
    var stepPx = Math.max(10, (($scope.HOUR_WIDTH || 100) / 4)); // 15 min

    var startVisualLeft = availableOffset + Math.max(0, parseFloat(preferredTimelineLeft || 0));
    if (startVisualLeft < stripLeft) startVisualLeft = stripLeft;
    if (startVisualLeft >= stripRight) startVisualLeft = stripRight - stepPx;

    for (var visualLeft = startVisualLeft; visualLeft < stripRight; visualLeft += stepPx) {
        var timelineLeft = visualLeft - availableOffset;
        var c = $scope.getCenterDropConflictInfo(row, timelineLeft, width, ignoreAssignId);
        if (c.ok) return timelineLeft;
    }
    return null;
};

$scope.showCenterMoveOverlapPopup = function (row, taskItemId, taskType, timelineLeft, width, callback) {
    alert('Drop only in available area');
    if (callback) { callback('cancel'); }
};

$scope.applyCenterMoveScheduleChoice = function (oldEmp, targetEmp, row, seg, timelineLeft, width, choice) {
    if (!seg || !targetEmp || !row) return;

    var assignId = seg.assignid || seg.AssignId || seg.assignmentid;
    var taskItemId = seg.taskid || seg.id || seg.Task || seg.subtaskid || seg.taskId;
    var taskType = seg.tasktype || seg.taskType || seg.type;
    if (!assignId || !taskItemId || !taskType) return;

    var setDate = '';
    try { setDate = $scope.getSegmentDateMoment(seg).format('YYYY-MM-DD'); }
    catch (e) { setDate = $scope.toAssignServerDateOnly($scope.selectedDate); }

    if (choice === 'findfree') {
        var freeLeft = $scope.findCenterFreeTimelineLeft(row, timelineLeft, width, assignId);
        var freeDate = setDate;

        if (freeLeft === null || freeLeft === undefined) {
            // No slot left today. Move to next day at employee shift start.
            freeDate = $scope.getNextAssignDate ? $scope.getNextAssignDate(1) : $scope.toAssignServerDateOnly(moment($scope.selectedDate).add(1, 'days').toDate());
            var empStart = targetEmp._shiftStartM ? targetEmp._shiftStartM.clone() : $scope.parseTimeOnDate(targetEmp.ShiftStart || '09:00 AM', $scope.selectedDate);
            freeLeft = Math.max(0, empStart.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);
        }

        var freeStartTime = $scope.timelineLeftToAssignTime(freeLeft || 0);
        var freeValidationItem = $scope.findAssignValidationItem(taskItemId, taskType, seg);
        if (!$scope.validateAssignSelectedDateWithTaskEndDate([freeValidationItem || seg], freeDate)) {
            $scope.refreshAssignTeamRows();
            return;
        }
        assignprojectService.UpdateAssignOwnerTime(assignId, targetEmp.id, freeDate, freeStartTime).then(function (response) {
            var result = response && response.data ? response.data.d : '0';
            if (result !== '1' && result !== 1) {
                alert(result || 'Unable to move task.');
                $scope.refreshAssignTeamRows();
                return;
            }
            $scope.syncAssignedOwnerEverywhere(taskItemId, taskType, targetEmp);
            $scope.updateMainOwnerByTaskType(taskItemId, taskType, targetEmp.id).finally(function () {
                $scope.refreshAssignTeamRows();
            });
        }, function () {
            alert('Unable to move task.');
            $scope.refreshAssignTeamRows();
        });
        return;
    }

    if (choice === 'shift') {
        var startTime = $scope.timelineLeftToAssignTime(timelineLeft || 0);
        var shiftValidationItem = $scope.findAssignValidationItem(taskItemId, taskType, seg);
        if (!$scope.validateAssignSelectedDateWithTaskEndDate([shiftValidationItem || seg], setDate)) {
            $scope.refreshAssignTeamRows();
            return;
        }
        assignprojectService.ShiftAssignTaskWithExisting(taskItemId, targetEmp.id, taskType, setDate, startTime).then(function (response) {
            var result = response && response.data ? response.data.d : '0';
            if (result !== '1' && result !== 1) {
                alert(result || 'Unable to shift existing task.');
                $scope.refreshAssignTeamRows();
                return;
            }
            $scope.syncAssignedOwnerEverywhere(taskItemId, taskType, targetEmp);
            $scope.updateMainOwnerByTaskType(taskItemId, taskType, targetEmp.id).finally(function () {
                $scope.refreshAssignTeamRows();
            });
        }, function () {
            alert('Unable to shift existing task.');
            $scope.refreshAssignTeamRows();
        });
    }
};

$scope.startCenterSegmentDrag = function ($event, emp, seg, mode) {
    if (!$event || !emp || !seg) return;
    if (mode === 'move' && $event.target && $event.target.closest && $event.target.closest('.planned-close,.center-resize-handle')) return;

    var dragTaskType = seg.tasktype || seg.taskType || seg.type;
    if (!$scope.requireAssign(dragTaskType)) return;

    // Move can now drop on another employee row. Resize stays in same row.
    $event.preventDefault();
    $event.stopPropagation();

    if (!$scope.timelineStartMoment) {
        $scope.applyTimelineToEmployees();
    }

    var startClientX = $event.clientX;
    var startLeft = parseFloat(seg.left || 0);
    var startWidth = parseFloat(seg.width || 18);
    var gapPx = 4;
    var startDuration = Math.max($scope.centerMinWidthMinutes || 15, Math.round(((startWidth + gapPx) / $scope.HOUR_WIDTH) * 60));

    var originalSegmentState = {
        StartTime: seg.StartTime,
        EndTime: seg.EndTime,
        DurationMinutes: seg.DurationMinutes,
        TaskDuration: seg.TaskDuration,
        taskduration: seg.taskduration,
        left: seg.left,
        width: seg.width
    };

   function positionCenterDragGhost(ev) {
//alert("ok");
    if (!dragGhost || !ev) return;

    var w = dragGhost.offsetWidth || Math.max(80, startWidth);
    var h = dragGhost.offsetHeight || 24;
    var left = ev.clientX;
    //var top = ev.clientY; // keep helper at same cursor Y position
    var top = ev.clientY - h - 3;; // keep helper at same cursor Y position

    if (left + w > window.innerWidth - 6)
        left = Math.max(6, window.innerWidth - w - 6);

    if (top < 6)
        top = 6;

    
document.body.style.setProperty('cursor', 'move', 'important');
document.documentElement.style.setProperty('cursor', 'move', 'important');
dragGhost.style.setProperty('left', left + 'px', 'important');
dragGhost.style.setProperty('top', top + 'px', 'important');

}


    var dragGhost = null;
    if (mode === 'move') {

        dragGhost = document.createElement('div');
        dragGhost.className = 'task-bar dragging assign-center-drag-preview';
        dragGhost.style.position = 'fixed';
        dragGhost.style.zIndex = '2147483647';
        dragGhost.style.pointerEvents = 'none';
        dragGhost.style.width = Math.max(80, startWidth) + 'px';
        document.body.style.setProperty('cursor', 'move', 'important');
        dragGhost.style.setProperty('cursor', 'move', 'important');
        dragGhost.innerHTML = '<div class="task-name">' + (seg.taskname || '') + '</div>';
        document.body.appendChild(dragGhost);
        
        positionCenterDragGhost($event);
    }

    var shiftStartM = emp._shiftStartM ? emp._shiftStartM.clone() : $scope.parseTimeOnDate(emp.ShiftStart, $scope.selectedDate);
    var shiftEndM = emp._shiftEndM ? emp._shiftEndM.clone() : $scope.parseTimeOnDate(emp.ShiftEnd, $scope.selectedDate);
    if (shiftEndM.isSameOrBefore(shiftStartM)) shiftEndM.add(1, 'day');

    var minLeft = Math.max(0, shiftStartM.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);
    var maxLeft = Math.max(minLeft, shiftEndM.diff($scope.timelineStartMoment, 'minutes') / 60 * $scope.HOUR_WIDTH);

    var body = angular.element(document.body);
    body.addClass('center-segment-time-editing');

    function pxToMinutes(px) {
        return $scope.roundCenterMinutes((px / $scope.HOUR_WIDTH) * 60);
    }

    function onMove(ev) {
        ev.preventDefault();
        var dx = ev.clientX - startClientX;
        if (dragGhost) {
            positionCenterDragGhost(ev);
        }

        $scope.$applyAsync(function () {
            var newLeft = startLeft;
            var newDuration = startDuration;

            if (mode === 'resize-left') {
                var rightEdge = startLeft + startWidth + gapPx;
                newLeft = startLeft + dx;
                newLeft = Math.max(minLeft, Math.min(newLeft, rightEdge - (($scope.centerMinWidthMinutes || 15) / 60 * $scope.HOUR_WIDTH)));
                newLeft = (pxToMinutes(newLeft) / 60) * $scope.HOUR_WIDTH;
                newDuration = pxToMinutes(rightEdge - newLeft);
            }
            else if (mode === 'resize-right') {
                var newRight = startLeft + startWidth + gapPx + dx;
                newRight = Math.max(startLeft + (($scope.centerMinWidthMinutes || 15) / 60 * $scope.HOUR_WIDTH), Math.min(newRight, maxLeft));
                newDuration = pxToMinutes(newRight - startLeft);
                newLeft = startLeft;
            }
            else { // move
                newLeft = startLeft + dx;
                var widthWithGap = Math.max(18, startWidth + gapPx);
                newLeft = Math.max(minLeft, Math.min(newLeft, maxLeft - widthWithGap));
                newLeft = (pxToMinutes(newLeft) / 60) * $scope.HOUR_WIDTH;
                newDuration = startDuration;
            }

            var newStart = $scope.timelineStartMoment.clone().add(pxToMinutes(newLeft), 'minutes');
            $scope.updateCenterSegmentTime(emp, seg, newStart, newDuration);
        });
    }

    function onUp(ev) {
        if (ev) ev.preventDefault();
        angular.element(document).off('mousemove', onMove);
        angular.element(document).off('mouseup', onUp);
        body.removeClass('center-segment-time-editing');

        var dropRow = (mode === 'move' && ev) ? $scope.findAssignTimelineRowFromPoint(ev.clientX, ev.clientY) : null;
        var dropEmpId = dropRow ? dropRow.getAttribute('data-empid') : null;
        if (dragGhost && dragGhost.parentNode) dragGhost.parentNode.removeChild(dragGhost);

        if (mode === 'move') {
            var targetRow = dropRow || document.querySelector('.timeline-row[data-empid="' + emp.id + '"]');
            var targetEmpId = dropEmpId || emp.id;
            var targetEmp = ($scope.currenttaskteam || []).find(function (e) { return String(e.id) === String(targetEmpId); }) || { id: targetEmpId };
            var targetLeft = (dropRow && String(targetEmpId) !== String(emp.id))
                ? $scope.getAssignTimelineLeftFromMouse(dropRow, ev.clientX)
                : parseFloat(seg.left || 0);
            var assignIdForConflict = seg.assignid || seg.AssignId || seg.assignmentid;
            var moveWidth = Math.max(18, startWidth + gapPx);
            var centerConflict = $scope.getCenterDropConflictInfo(targetRow, targetLeft, moveWidth, assignIdForConflict);

            if (!centerConflict.ok) {
                $scope.$applyAsync(function () {
                    $scope.restoreCenterSegmentTime(seg, originalSegmentState);
                    alert('Drop only in available area');
                    $scope.refreshAssignTeamRows();
                    if ($scope.resetAssignTooltips) { $scope.resetAssignTooltips(); }
                });
                return;
            }

            if (dropEmpId && String(dropEmpId) !== String(emp.id)) {
                $scope.$applyAsync(function () {
                    $scope.restoreCenterSegmentTime(seg, originalSegmentState);
                    $scope.moveAssignedSegmentToEmployee(emp, dropEmpId, seg, targetLeft);
                });
                return;
            }
        }

        if (mode === 'resize-left' || mode === 'resize-right') {
            // Multi-day split task: resize allowed only on the LAST visible day.
            // Duration saved in DB is cumulative of all visible day segments.
            if (!$scope.isLastVisibleSegmentOfAssign(emp, seg)) {
                alert('Resize only last day segment. Previous day time is fixed.');
                $scope.$applyAsync(function () {
                    $scope.restoreCenterSegmentTime(seg, originalSegmentState);
                });
                return;
            }

            // Validation must use real hierarchy item id (taskid/subtaskid/milestoneid),
            // not Pms_tbl_assigntasks.assignid. assignid is only for DB update.
            var resizeTaskId = seg.taskid || seg.id || seg.Task || seg.subtaskid || seg.taskId;
            var resizeTaskType = seg.tasktype || seg.taskType || seg.type;
            var resizeMinutes = $scope.getCumulativeDurationForAssign(emp, seg);

            var isValidResize = $scope.validateResizeDurationByParent(
                resizeTaskId,
                resizeTaskType,
                resizeMinutes
            );

            if (!isValidResize) {
                $scope.$applyAsync(function () {
                    $scope.restoreCenterSegmentTime(seg, originalSegmentState);
                });
                return;
            }
        }

        // Keep original visible segment info so server can update only this day segment
        // when one assignment is split visually across multiple days.
        seg._resizeOriginalStartTime = originalSegmentState.StartTime;
        seg._resizeOriginalDurationMinutes = originalSegmentState.DurationMinutes;
        try {
            seg._resizeOriginalWorkDate = $scope.getSegmentDateMoment(seg).format('YYYY-MM-DD');
        } catch (exOldDate) {
            seg._resizeOriginalWorkDate = ($scope.Assigncheckdate ? $scope.toAssignServerDateOnly($scope.Assigncheckdate) : $scope.toAssignServerDateOnly(new Date()));
        }

        // Hook for DB save later. Currently it updates UI/state only.
        // Example payload: taskid, tasktype, ownerid, StartTime, EndTime, TaskDuration.
        if ($scope.onCenterSegmentTimeChanged) {
            $scope.$applyAsync(function () {
                $scope.onCenterSegmentTimeChanged(emp, seg, mode);
            });
        }
    }

    angular.element(document).on('mousemove', onMove);
    angular.element(document).on('mouseup', onUp);
};



// Save center panel move/resize changes to database.
// Move updates StartTime. Resize updates TaskDuration. EndTime/DurationMinutes stay synced in UI.
$scope.onCenterSegmentTimeChanged = function (emp, seg, editMode) {
    if (!emp || !seg) return;

    var assignId = seg.assignid || seg.AssignId || seg.assignmentid || seg.taskid || seg.id;
    var taskItemId = seg.taskid || seg.id || seg.Task || seg.subtaskid || seg.taskId;
    var taskType = seg.tasktype || seg.taskType || seg.type;

    if (!assignId || !taskType) return;
    if (!$scope.requireAssign(taskType, true)) {
        $scope.refreshAssignTeamRows();
        return;
    }
    if (!taskItemId) taskItemId = assignId;

    editMode = editMode || seg._lastCenterEditMode || '';

    var startTime = seg.StartTime || '';
    var taskDuration = seg.TaskDuration || seg.taskduration || $scope.formatDurationHHMM(seg.DurationMinutes || 0);

    // For resize on last day, visible segment duration may be 05:00,
    // but DB TaskDuration must become cumulative: previous days + resized last day.
    if (editMode === 'resize-left' || editMode === 'resize-right') {
        var totalResizeMinutes = $scope.getCumulativeDurationForAssign(emp, seg);
        taskDuration = $scope.formatDurationHHMM(totalResizeMinutes);
    }

    var changedate = '';
    try {
        changedate = $scope.getSegmentDateMoment(seg).format('YYYY-MM-DD');
    } catch (exDate) {
        changedate = ($scope.Assigncheckdate ? $scope.toAssignServerDateOnly($scope.Assigncheckdate) : $scope.toAssignServerDateOnly(new Date()));
    }
    if (!startTime || !taskDuration) return;

    if (editMode === 'move') {
        var timeMoveValidationItem = $scope.findAssignValidationItem(taskItemId, taskType, seg);
        if (!$scope.validateAssignSelectedDateWithTaskEndDate([timeMoveValidationItem || seg], changedate)) {
            $scope.refreshAssignTeamRows();
            return;
        }
    }

    // Update main grid and right assign panel immediately after move/resize.
    $scope.syncMovedResizedTaskEverywhere(taskItemId, taskType, startTime, taskDuration);

    // On resize, update duration through existing main view method also.
    // Move only changes StartTime; resize changes TaskDuration.
    if (editMode === 'resize-left' || editMode === 'resize-right') {
        $scope.updateAssignResizeDurationUsingMainUpdateHrs(taskItemId, taskType, startTime, taskDuration);
    }

    var oldStartTimeForSegment = seg._resizeOriginalStartTime || '';
    var oldDateForSegment = seg._resizeOriginalWorkDate || changedate;

    assignprojectService.UpdateAssignTaskTime(assignId, taskType, startTime, taskDuration, changedate, oldStartTimeForSegment, oldDateForSegment, editMode)
        .then(function (response) {
            var result = response && response.data ? response.data.d : '0';
            if (result !== '1') {
                alert('Time update failed.');
                $scope.refreshAssignTeamRows();
                return;
            }
            $scope.reloadAssignEmployeeForDate(emp.id, $scope.toAssignServerDateOnly($scope.selectedDate), 30);
        }, function () {
            alert('Time update failed.');
            $scope.refreshAssignTeamRows();
        });
};
    $scope.getuserwithtasklist = function (teams, checkdate) {
        debugger
        var loadSeq = ++$scope._assignReloadSeq;
        $scope.isAssignRefreshing = true;
        return assignprojectService.getuserwithtasklist(teams, checkdate)
            .then(function (response) {
                // Ignore old response if user changed date quickly.
                if (loadSeq !== $scope._assignReloadSeq) return;
                $scope.currenttaskteam = response.data.d || [];
                // Hide active DB assignments from the unassigned right panel even
                // when a malformed old duration prevents a visual segment.
                if ($scope.syncRightPanelAssignmentState) {
                    $scope.syncRightPanelAssignmentState();
                }
                $scope.applyTimelineToEmployees();
                if ($scope.resetAssignTooltips) { $scope.resetAssignTooltips(); }
                if ($scope.scheduleAssignToTooltipRefresh) { $scope.scheduleAssignToTooltipRefresh(180); }
                $scope.rebindAssignDragFast(30);
                $scope.$applyAsync();
            })
            .catch(function (error) {
                console.log(error);
            })
            .finally(function () {
                if (loadSeq === $scope._assignReloadSeq) {
                    $scope.isAssignRefreshing = false;
                }
            });
    };

 $scope.changeDay = function (days) {
        var dt = normalizeHierarchyDateOnly($scope.selectedDate) || new Date();
        dt.setDate(dt.getDate() + days);
        $scope.selectedDate = dt;
        document.getElementById("datePicker").value = $filter('date')(dt, 'yyyy-MM-dd');

        var checkdate = $scope.toAssignServerDateOnly($scope.selectedDate);
        var userIds = '';
        if ($scope.currenttaskteam && $scope.currenttaskteam.length) {
            userIds = $scope.currenttaskteam.map(function (x) { return x.id; }).join(',');
        }
        if (userIds) {
            $scope.getuserwithtasklist(userIds, checkdate);
        }
    };

    $scope.assignTaskToUser = function (row, task) {
        if (!$scope.requireAssign(task && (task.tasktype || task.type))) return;

        row.tasks.push({

            id: new Date().getTime(),
            title: task.title,
            left: 40,
            width: 240
        });

        var index = $scope.sideTasks.indexOf(task);

        if (index > -1) {
            $scope.sideTasks.splice(index, 1);
        }
    };

$scope.getSelectedTasksForDrop = function (dragTaskId, dragTaskType) {
    var selected = $scope.collectSelectedAssignTreeTasks ? $scope.collectSelectedAssignTreeTasks() : [];

    if (!selected.length) {
        selected = ($scope.currenttasklist || []).filter(function (x) {
            return x.isSelected === true && x.isAssignedTemp !== true && x.isRemovedTemp !== true;
        });
    }

    if (selected.length > 0) {
        return selected;
    }

    var node = $scope.assignTreeNodeMap ? $scope.assignTreeNodeMap[$scope.getAssignTaskKey(dragTaskId, dragTaskType)] : null;
    if (node) {
        if (!$scope.canAssign(node.type)) return [];
        return $scope.copyAssignRootInfo($scope.getLowestAssignItemsFromNode(node), dragTaskId, dragTaskType);
    }

    var single = null;
    angular.forEach($scope.currenttasklist || [], function (x) {
        if (!single && String(x.id) === String(dragTaskId) && String(x.tasktype) === String(dragTaskType) && $scope.canAssign(x.tasktype)) {
            single = x;
        }
    });

    return single ? [single] : [];
};

// Assignment hierarchy validation:
// A milestone/task parent cannot be assigned until every task has at least one subtask.
// This guard runs before any right-panel item is hidden or any assignment is saved.
$scope.findAssignHierarchyRoot = function (rootId, rootType) {
    rootType = String(rootType || '').toLowerCase();
    var result = { node: null, item: null, type: rootType, id: rootId };

    if ($scope.assignTreeNodeMap && rootId && rootType) {
        result.node = $scope.assignTreeNodeMap[$scope.getAssignTaskKey(rootId, rootType)] || null;
        if (result.node) {
            result.item = result.node.item || result.node.source || null;
            return result;
        }
    }

    angular.forEach(($scope.projectlist && $scope.projectlist.miles) || [], function (mile) {
        if (result.item) return;

        if (rootType === 'milestone' && String(mile.id) === String(rootId)) {
            result.item = mile;
            return;
        }

        angular.forEach(mile.tasks || [], function (task) {
            if (result.item) return;
            if (rootType === 'task' && String(task.id) === String(rootId)) {
                result.item = task;
            }
        });
    });

    return result;
};

$scope.assignTaskHasSubtaskHierarchy = function (taskItem, taskNode) {
    var subtasks = taskItem && (taskItem.subtasks || taskItem.Subtasks || taskItem.subtasklist || taskItem.subtask);
    if (angular.isArray(subtasks)) return subtasks.length > 0;

    if (taskNode) {
        // hasSubtaskHierarchy remains true even when all children are completed/filtered.
        if (taskNode.hasSubtaskHierarchy === true) return true;
        if (taskNode.children && taskNode.children.length > 0) return true;
    }

    return false;
};

$scope.getFirstAssignHierarchyWithoutSubtask = function (tasks) {
    tasks = angular.isArray(tasks) ? tasks : (tasks ? [tasks] : []);
    var checkedRoots = {};

    for (var i = 0; i < tasks.length; i++) {
        var draggedItem = tasks[i] || {};
        var rootId = draggedItem._assignDropRootId || draggedItem.id || draggedItem.taskid || draggedItem.Task || draggedItem.Milestoneid;
        var rootType = String(draggedItem._assignDropRootType || draggedItem.tasktype || draggedItem.taskType || draggedItem.type || '').toLowerCase();

        if (!rootId || !rootType || rootType === 'subtask') continue;

        var rootKey = rootType + '_' + rootId;
        if (checkedRoots[rootKey]) continue;
        checkedRoots[rootKey] = true;

        var root = $scope.findAssignHierarchyRoot(rootId, rootType);
        var rootNode = root.node;
        var rootItem = root.item || draggedItem;

        var level = $scope.getAssignmentLevel();
        if (level === 'milestone') {
            if (rootType !== 'milestone') return { message: 'This project allows milestone assignment only.' };
            continue;
        }
        if (level === 'task') {
            if (rootType === 'task') continue;
            var enabledTasks = rootNode && rootNode.children || rootItem.tasks || [];
            if (!enabledTasks.length) return { message: 'Milestone "' +
                $scope.getTaskDisplayName(rootItem, 'milestone') +
                '" cannot be assigned without tasks. Kindly create a task first.' };
            continue;
        }
        if (rootType === 'task') {
            if (!$scope.assignTaskHasSubtaskHierarchy(rootItem, rootNode)) {
                return {
                    type: 'task',
                    taskName: $scope.getTaskDisplayName(rootItem, 'task') || rootNode && rootNode.name || 'Selected task'
                };
            }
            continue;
        }

        if (rootType === 'milestone') {
            var milestoneName = $scope.getTaskDisplayName(rootItem, 'milestone') || rootNode && rootNode.name || 'Selected milestone';
            var taskEntries = [];
            var milestoneTasks = rootItem && (rootItem.tasks || rootItem.Tasks || rootItem.tasklist);

            if (angular.isArray(milestoneTasks) && milestoneTasks.length > 0) {
                angular.forEach(milestoneTasks, function (taskItem) {
                    var taskNode = null;
                    if ($scope.assignTreeNodeMap && taskItem && taskItem.id !== undefined) {
                        taskNode = $scope.assignTreeNodeMap[$scope.getAssignTaskKey(taskItem.id, 'task')] || null;
                    }
                    taskEntries.push({ item: taskItem, node: taskNode });
                });
            } else if (rootNode && rootNode.children && rootNode.children.length > 0) {
                angular.forEach(rootNode.children, function (taskNode) {
                    if (taskNode && taskNode.type === 'task') {
                        taskEntries.push({ item: taskNode.item || taskNode.source, node: taskNode });
                    }
                });
            }

            if (taskEntries.length === 0) {
                return {
                    type: 'milestone',
                    milestoneName: milestoneName,
                    noTask: true
                };
            }

            for (var t = 0; t < taskEntries.length; t++) {
                var entry = taskEntries[t];
                if (!$scope.assignTaskHasSubtaskHierarchy(entry.item, entry.node)) {
                    return {
                        type: 'milestone',
                        milestoneName: milestoneName,
                        taskName: $scope.getTaskDisplayName(entry.item, 'task') || entry.node && entry.node.name || 'Selected task'
                    };
                }
            }
        }
    }

    return null;
};

$scope.validateAssignHierarchyBeforeDrop = function (tasks, alreadyHidden, suppressAlert) {
    var invalid = $scope.getFirstAssignHierarchyWithoutSubtask(tasks);
    if (!invalid) return true;

    if (alreadyHidden) {
        $scope.restoreAlreadyHiddenAssignTasks(tasks);
    }

    if (!suppressAlert) {
        var message;
        if (invalid.message) {
            message = invalid.message;
        } else if (invalid.type === 'milestone' && invalid.noTask) {
            message = 'Milestone "' + invalid.milestoneName + '" cannot be assigned without task/subtask. Kindly create a task and subtask first.';
        } else if (invalid.type === 'milestone') {
            message = 'Milestone "' + invalid.milestoneName + '" cannot be assigned because task "' + invalid.taskName + '" has no subtask. Kindly create a subtask first.';
        } else {
            message = 'Task "' + invalid.taskName + '" cannot be assigned without subtask. Kindly create a subtask first.';
        }
        alert(message);
    }

    return false;
};

$scope.getDurationMinutesFromTask = function (task) {
    if (!task) return 0;
    var d = task.TaskDuration || task.taskduration || task.Duration || task.duration || "";
    d = String(d);
    if (d.indexOf(':') > -1) {
        var a = d.split(':');
        return (parseInt(a[0], 10) || 0) * 60 + (parseInt(a[1], 10) || 0);
    }
    if (d.indexOf('/') > -1) {
        var b = d.split('/');
        return (parseInt(b[0], 10) || 0) * 60 + (parseInt(b[1], 10) || 0);
    }
    var n = parseInt(d, 10);
    return isNaN(n) ? 0 : n * 60;
};

// Right-to-center assignment duration validation.
// This is UI-only guard: if duration is missing/zero, do not save and restore right panel.
$scope.getAssignDurationMinutes = function (task) {
    if (!task) return 0;

    var directMinutes = parseInt(task.DurationMinutes || task.durationMinutes || task.durationminutes || task.TotalMinutes || 0, 10);
    if (!isNaN(directMinutes) && directMinutes > 0) return directMinutes;

    var hasHourMinuteFields =
        task.TaskDuration_hr !== undefined || task.TaskDuration_hrs !== undefined ||
        task.TaskDurationhour !== undefined || task.TaskDuration_hour !== undefined ||
        task.DurationHour !== undefined || task.DurationHours !== undefined ||
        task.TaskDuration_min !== undefined || task.DurationMin !== undefined || task.DurationMinutesOnly !== undefined;

    if (hasHourMinuteFields) {
        var hrValue = task.TaskDuration_hr || task.TaskDuration_hrs || task.TaskDurationhour ||
            task.TaskDuration_hour || task.DurationHour || task.DurationHours || 0;
        var minValue = task.TaskDuration_min || task.DurationMin || task.DurationMinutesOnly || 0;
        var fieldHours = parseInt(hrValue || 0, 10);
        var fieldMinutes = parseInt(minValue || 0, 10);
        if (isNaN(fieldHours)) fieldHours = 0;
        if (isNaN(fieldMinutes)) fieldMinutes = 0;
        if ((fieldHours * 60) + fieldMinutes > 0) return (fieldHours * 60) + fieldMinutes;
    }

    var d = task.TaskDuration || task.taskduration || task.Duration || task.duration || '';
    d = String(d || '').trim();
    if (!d) return 0;

    d = d.replace(/hrs?|hours?/ig, '').trim();

    if (d.indexOf(':') > -1 || d.indexOf('/') > -1) {
        var sep = d.indexOf(':') > -1 ? ':' : '/';
        var parts = d.split(sep);
        var h = parseInt(parts[0] || '0', 10);
        var m = parseInt(parts[1] || '0', 10);
        if (isNaN(h)) h = 0;
        if (isNaN(m)) m = 0;
        return (h * 60) + m;
    }

    var decimalHours = parseFloat(d);
    return isNaN(decimalHours) ? 0 : Math.round(decimalHours * 60);
};

$scope.getAssignTaskIdentityForDuration = function (task) {
    task = task || {};
    return {
        id: task.id || task.taskid || task.taskId || task.Task || task.subtaskid || task.Milestoneid,
        tasktype: String(task.tasktype || task.taskType || task.type || '').toLowerCase()
    };
};

$scope.findAssignDurationSource = function (task) {
    if (!task) return null;
    var ident = $scope.getAssignTaskIdentityForDuration(task);
    if (ident.id && ident.tasktype && $scope.findAssignValidationItem) {
        return $scope.findAssignValidationItem(ident.id, ident.tasktype, task) || task;
    }
    return task;
};

$scope.getFirstAssignTaskWithoutDuration = function (tasks) {
    tasks = angular.isArray(tasks) ? tasks : (tasks ? [tasks] : []);
    var checked = {};

    for (var i = 0; i < tasks.length; i++) {
        var task = tasks[i] || {};
        var ident = $scope.getAssignTaskIdentityForDuration(task);
        var key = String(ident.tasktype || '') + '_' + String(ident.id || i);
        if (checked[key]) continue;
        checked[key] = true;

        var minutes = $scope.getAssignDurationMinutes(task);
        var source = $scope.findAssignDurationSource(task);
        if (minutes <= 0 && source && source !== task) {
            minutes = $scope.getAssignDurationMinutes(source);
        }

        if (minutes <= 0) {
            return source || task;
        }
    }

    return null;
};

$scope.restoreAlreadyHiddenAssignTasks = function (tasks) {
    angular.forEach(tasks || [], function (task) {
        if (!task) return;

        var ident = $scope.getAssignTaskIdentityForDuration(task);
        if (ident.id && ident.tasktype) {
            $scope.setAssignTreeHiddenByKey(ident.id, ident.tasktype, false);
        }

        if (task._assignDropRootId && task._assignDropRootType) {
            $scope.setAssignTreeHiddenByKey(task._assignDropRootId, task._assignDropRootType, false);
        }

        task.isAssignedTemp = false;
        task.isRemovedTemp = false;
        task.isSelected = false;
    });

    $scope.syncSelectedAssignTasks();
    if ($scope.$applyAsync) $scope.$applyAsync();
};

$scope.validateAssignDurationBeforeDrop = function (tasks, alreadyHidden, suppressAlert) {
    tasks = angular.isArray(tasks) ? tasks : (tasks ? [tasks] : []);
    if (!tasks.length) return true;

    var invalid = $scope.getFirstAssignTaskWithoutDuration(tasks);
    if (!invalid) return true;

    if (alreadyHidden) {
        $scope.restoreAlreadyHiddenAssignTasks(tasks);
    }

    if (!suppressAlert) {
        var invalidType = String(invalid.tasktype || invalid.taskType || invalid.type || '').toLowerCase();
        var invalidName = $scope.getTaskDisplayName(invalid, invalidType) ||
            invalid.Subtaskname || invalid.Taskname || invalid.Milestone || invalid.Milestonename || invalid.name || 'Selected task';
        //invalidName + 
        alert(invalidName + ' duration is required before assignment. Please enter duration first.');
    }

    return false;
};

$scope.addMinutesToAssignTime = function (timeText, minutes) {
    var m = moment(timeText, ['hh:mm A', 'h:mm A', 'HH:mm']);
    if (!m.isValid()) m = moment('09:00 AM', 'hh:mm A');
    return m.add(minutes || 0, 'minutes').format('hh:mm A');
};



// Timezone-safe date-only helper for right-panel -> center assignment.
// Never use toISOString() here because UTC conversion can move the selected calendar date.
$scope.toAssignServerDateOnly = function (value) {
    if (value === null || value === undefined || value === '') return '';

    if (Object.prototype.toString.call(value) === '[object Date]') {
        if (isNaN(value.getTime())) return '';
        return value.getFullYear() + '-' +
            String(value.getMonth() + 1).padStart(2, '0') + '-' +
            String(value.getDate()).padStart(2, '0');
    }

    var text = String(value).trim();
    var iso = /^(\d{4})-(\d{2})-(\d{2})/.exec(text);
    if (iso) return iso[1] + '-' + iso[2] + '-' + iso[3];

    // Assignment screen dates are MM/DD/YYYY.
    var us = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/.exec(text);
    if (us) {
        return us[3] + '-' + String(us[1]).padStart(2, '0') + '-' + String(us[2]).padStart(2, '0');
    }

    var parsed = moment(text, [moment.ISO_8601, 'MM/DD/YYYY', 'M/D/YYYY', 'YYYY-MM-DD'], true);
    return parsed.isValid() ? parsed.format('YYYY-MM-DD') : '';
};

$scope.getNextAssignDate = function (days) {
    var d = moment($scope.selectedDate).add(days || 1, 'days').toDate();
    return $scope.toAssignServerDateOnly(d);
};

$scope.setSelectedAssignDateAndRefresh = function (assignDate, empid) {
    if (!assignDate) return;

    var m = moment(assignDate, ['MM/DD/YYYY', 'M/D/YYYY', 'YYYY-MM-DD']);
    if (!m.isValid()) return;

    $scope.selectedDate = m.toDate();

    var checkdate = $scope.toAssignServerDateOnly($scope.selectedDate);
    var userIds = '';

    if ($scope.currenttaskteam && $scope.currenttaskteam.length) {
        userIds = $scope.currenttaskteam.map(function (x) { return x.id; }).join(',');
    }
    else if (empid) {
        userIds = empid;
    }

    if (userIds) {
        $scope.getuserwithtasklist(userIds, checkdate);
    }
};

$scope.changeassignWithShift = function (empid, tasks, starttime, assignDate, alreadyHidden) {
    tasks = tasks || [];
    if (!tasks.length) return;

    var notAllowedShift = (tasks || []).filter(function (t) { return !$scope.canAssign(t.tasktype || t.taskType || t.type); });
    if (notAllowedShift.length) {
        if (alreadyHidden) $scope.restoreAlreadyHiddenAssignTasks(tasks);
        $scope.requireAssign(notAllowedShift[0].tasktype || notAllowedShift[0].taskType || notAllowedShift[0].type);
        return;
    }

    if (!$scope.validateAssignHierarchyBeforeDrop(tasks, alreadyHidden, false)) {
        return;
    }

    if (!$scope.validateAssignDurationBeforeDrop(tasks, alreadyHidden, false)) {
        return;
    }

    var validationDate = $scope.toAssignServerDateOnly(assignDate || $scope.selectedDate);
    if (!$scope.validateAssignSelectedDateWithTaskEndDate(tasks, validationDate)) {
        return;
    }

    var removedRightTasks = alreadyHidden ? [] : $scope.hideTasksFromRight(tasks);

    var setdate = $scope.toAssignServerDateOnly(assignDate || $scope.selectedDate);
    var nextStart = starttime;

    // Shift Existing option must work same as Find Free Slot for task/milestone parent:
    // if dragged task has multiple subtasks, save every lowest child one by one.
    // Earlier only tasks[0] was sent to server, so only one subtask was dropped.
    var chain = Promise.resolve();

    angular.forEach(tasks, function (task) {
        chain = chain.then(function () {
            return assignprojectService
                .ShiftAssignTaskWithExisting(task.id, empid, task.tasktype, setdate, nextStart)
                .then(function (response) {
                    if (response.data.d !== '1' && response.data.d !== 1) {
                        throw new Error(response.data.d || 'Unable to shift existing task.');
                    }

                    var droppedEmp = ($scope.currenttaskteam || []).find(function (e) {
                        return String(e.id) === String(empid);
                    }) || { id: empid };

                    $scope.syncAssignedOwnerEverywhere(task.id, task.tasktype, droppedEmp);

                    return $scope.updateMainOwnerByTaskType(task.id, task.tasktype, empid).then(function () {
                        // Next child starts after current child duration. Server-side shift will push old existing rows.
                        nextStart = $scope.addMinutesToAssignTime(nextStart, $scope.getDurationMinutesFromTask(task));
                    });
                });
        });
    });

    return chain
        .then(function () {
            return assignprojectService.getuserwithtasklist_bysingleid(empid, setdate);
        })
        .then(function (response) {
            var nelist = response.data.d;
            if (nelist) { $scope.prepareTimelineEmployee(nelist); }

            var idx = ($scope.currenttaskteam || []).findIndex(function (m) {
                return String(m.id) === String(empid);
            });
            if (idx > -1) $scope.currenttaskteam.splice(idx, 1, angular.copy(nelist));
            else $scope.currenttaskteam.push(angular.copy(nelist));

            $scope.applyTimelineToEmployees();

            angular.forEach(tasks, function (task) {
                task.isSelected = false;
            });
            $scope.syncSelectedAssignTasks();

            if (assignDate && assignDate !== $scope.toAssignServerDateOnly($scope.selectedDate)) {
                $scope.setSelectedAssignDateAndRefresh(assignDate, empid);
            }

            $scope.rebindAssignDragFast(30);
        })
        .catch(function (error) {
            console.error(error);
            if (!alreadyHidden) {
                $scope.restoreTasksToRight(removedRightTasks);
            }
            alert(error.message || 'Unable to shift existing task.');
        });
};

$scope.changeassignMultiple = function (empid, tasks, starttime, assignDate, alreadyHidden) {
    tasks = tasks || [];
    if (!tasks.length) return;

    var notAllowed = (tasks || []).filter(function (t) { return !$scope.canAssign(t.tasktype || t.taskType || t.type); });
    if (notAllowed.length) {
        if (alreadyHidden) $scope.restoreAlreadyHiddenAssignTasks(tasks);
        $scope.requireAssign(notAllowed[0].tasktype || notAllowed[0].taskType || notAllowed[0].type);
        return;
    }

    if (!$scope.validateAssignHierarchyBeforeDrop(tasks, alreadyHidden, false)) {
        return;
    }

    if (!$scope.validateAssignDurationBeforeDrop(tasks, alreadyHidden, false)) {
        return;
    }

    var validationDate = $scope.toAssignServerDateOnly(assignDate || $scope.selectedDate);
    if (!$scope.validateAssignSelectedDateWithTaskEndDate(tasks, validationDate)) {
        return;
    }

    // Complete milestone/task drop:
    // assign every lowest child one-by-one. Each next child asks server for the
    // first available start from the previous child's end time, so it can continue
    // to next day without overlapping any existing assignment.
    // Keep center window on the date selected before drop. Do not auto-switch to next day
    // just because remaining milestone children are saved on next dates.
    var removedRightTasks = alreadyHidden ? [] : $scope.hideTasksFromRight(tasks);

    var chain = Promise.resolve();
    var originalSelectedDate = $scope.toAssignServerDateOnly($scope.selectedDate);
    var cursorDate = $scope.toAssignServerDateOnly(assignDate || originalSelectedDate);
    var cursorTime = starttime;
    var lastAssignedDate = cursorDate;

    function parseNextSlotResult(raw) {
        raw = (raw === undefined || raw === null) ? '' : String(raw);
        var parts = raw.split('~~');
        if (parts[0] !== '1') {
            throw new Error(raw || 'Drop only in available area');
        }
        return {
            startDate: $scope.toAssignServerDateOnly(parts[1] || cursorDate),
            startTime: parts[2] || cursorTime,
            endDate: $scope.toAssignServerDateOnly(parts[3] || parts[1] || cursorDate),
            endTime: parts[4] || parts[2] || cursorTime
        };
    }

    angular.forEach(tasks, function (task) {
        chain = chain.then(function () {
            if (!assignprojectService.FindNextAvailableAssignStart) {
                return {
                    startDate: cursorDate,
                    startTime: cursorTime,
                    endDate: cursorDate,
                    endTime: $scope.addMinutesToAssignTime(cursorTime, $scope.getDurationMinutesFromTask(task))
                };
            }

            return assignprojectService
                .FindNextAvailableAssignStart(task.id, empid, task.tasktype, cursorDate, cursorTime)
                .then(function (res) {
                    return parseNextSlotResult(res && res.data ? res.data.d : '');
                });
        }).then(function (slot) {
            lastAssignedDate = slot.startDate;
            // keepSelectedDate=true prevents changeassign() from moving popup date to next day
            return $scope.changeassign(empid, task.id, task.tasktype, task.owner, slot.startTime, true, slot.startDate, true, true, true)
                .then(function () {
                    cursorDate = slot.endDate || slot.startDate;
                    cursorTime = slot.endTime || slot.startTime;
                });
        });
    });

    chain.then(function () {
        angular.forEach(tasks, function (task) { task.isSelected = false; });
        $scope.syncSelectedAssignTasks();
        if ($scope.resetAssignTooltips) { $scope.resetAssignTooltips(); }

        // After one-by-one milestone scheduling, reload only the originally selected date
        // so UI stays on the date where user dropped the milestone (example: 01/07/2026),
        // while next-day records remain saved and visible after user clicks next date.
        return $scope.reloadAssignEmployeeForDate(empid, originalSelectedDate, 30);
    }).catch(function (err) {
        console.error(err);
        if (!alreadyHidden) {
            $scope.restoreTasksToRight(removedRightTasks);
        }
        var msg = (err && err.message) ? err.message : 'Drop only in available area';
        alert(msg);
    });
};

$scope.changeassign = function (empid, taskid, tasktypes, taskowner, starttime, silent, assignDate, alreadyHidden, keepSelectedDate, skipEmployeeReload) {
    debugger
    if (!$scope.requireAssign(tasktypes, silent)) {
        return Promise.resolve({ _assignPermissionCancelled: true });
    }

    var hierarchyValidationItem = $scope.findAssignValidationItem(taskid, tasktypes, { id: taskid, tasktype: tasktypes });
    if (!$scope.validateAssignHierarchyBeforeDrop([hierarchyValidationItem || { id: taskid, tasktype: tasktypes }], alreadyHidden, silent)) {
        return Promise.resolve({ _assignHierarchyValidationCancelled: true });
    }

    var setdate = $scope.toAssignServerDateOnly(assignDate || $scope.selectedDate);

    var validationItem = hierarchyValidationItem || $scope.findAssignValidationItem(taskid, tasktypes, { id: taskid, tasktype: tasktypes });
    var durationValidationItem = validationItem || { id: taskid, tasktype: tasktypes };
    if (!$scope.validateAssignDurationBeforeDrop([durationValidationItem], alreadyHidden, silent)) {
        return Promise.resolve({ _assignDurationValidationCancelled: true });
    }

    var assignmentValidationDate = keepSelectedDate ? $scope.toAssignServerDateOnly($scope.selectedDate) : setdate;
    if (!$scope.validateAssignSelectedDateWithTaskEndDate([validationItem || { id: taskid, tasktype: tasktypes }], assignmentValidationDate, silent)) {
        return Promise.resolve({ _assignDateValidationCancelled: true });
    }

    // Remove immediately from right panel for temporary UI feedback.
    // If save fails, it will be restored in catch.
    var removedRightTask = alreadyHidden ? null : $scope.removeCurrentTaskFromRight(taskid, tasktypes);

    return assignprojectService
        .Updatetaskassignto(
            taskid,
            empid,
            tasktypes,
            setdate,
            starttime
        )
        .then(function (response) {

            if (response.data.d == 1) {

                // Update owner immediately in main view and right hierarchy on successful drop.
                $scope.setAssignTreePersistentStateByKey(taskid, tasktypes, true);
                var droppedEmp = ($scope.currenttaskteam || []).find(function (e) { return String(e.id) === String(empid); }) || { id: empid };
                $scope.syncAssignedOwnerEverywhere(taskid, tasktypes, droppedEmp);

                // Ensure the Kanban card/count moves to Assigned immediately.
                if ($scope.syncKanbanAssignmentState) {
                    $scope.syncKanbanAssignmentState(taskid, tasktypes, droppedEmp, true);
                }
                if ($scope.syncListAndKanbanViews) {
                    $scope.syncListAndKanbanViews();
                }

                return $scope.updateMainOwnerByTaskType(taskid, tasktypes, empid).then(function () {
                    if (skipEmployeeReload) {
                        return { _skipEmployeeReload: true, data: { d: null } };
                    }
                    return assignprojectService
                    .getuserwithtasklist_bysingleid(
                        empid,
                        setdate
                    );
                });
            }

            throw new Error(response.data.d || "Task assignment failed.");

        })
        .then(function (response) {

            if (response && response._skipEmployeeReload) {
                return;
            }

            var nelist = response.data.d;

            if (nelist) { $scope.prepareTimelineEmployee(nelist); }

            if (!nelist) {
                return;
            }

            var Nempindex = $scope.currenttaskteam.findIndex(function (m) {
                return String(m.id) === String(empid);
            });

            if (Nempindex > -1) {

                // Replace complete employee object
                $scope.currenttaskteam.splice(
                    Nempindex,
                    1,
                    angular.copy(nelist)
                );

            } else {

                $scope.currenttaskteam.push(
                    angular.copy(nelist)
                );
            }

            // Recalculate center timeline positions and Available Time after drop.
            $scope.applyTimelineToEmployees();
            if ($scope.resetAssignTooltips) { $scope.resetAssignTooltips(); }

            // Already removed from right panel before server call.

            if (!keepSelectedDate && assignDate && assignDate !== $scope.toAssignServerDateOnly($scope.selectedDate)) {
                $scope.setSelectedAssignDateAndRefresh(assignDate, empid);
            }

            $scope.rebindAssignDragFast(30);

        })
        .catch(function (error) {

            console.error("Assign Task Error:", error);

            // Restore right panel task because assignment failed.
            if (!alreadyHidden) {
                $scope.restoreCurrentTaskToRight(removedRightTask);
            }

            if (!silent) {
                alert(
                    error.message ||
                    "Unable to assign task."
                );
            }
            throw error;

        });
};
$scope.changeassignold = function (empid, taskid, tasktypes, taskowner,starttime) {

 if (!$scope.requireAssign(tasktypes)) return;
 alert(empid+','+taskid+','+tasktypes+','+taskowner+','+starttime);
var setdate=$scope.toAssignServerDateOnly($scope.selectedDate);
    assignprojectService
        .Updatetaskassignto(taskid, empid, tasktypes,setdate,starttime)
        .then(function (response) {

            if (response.data.d == 1) {
                return assignprojectService
                    .getuserwithtasklist_bysingleid(
                        empid,
                        setdate
                    );
            }
            else {

                throw new Error("Task assignment failed.");
            }

        })
        .then(function (response) {

            var nelist = response.data.d;

            if (nelist) { $scope.prepareTimelineEmployee(nelist); }

            if (!nelist) {
                return;
            }

            var Nempindex =
                $scope.currenttaskteam.findIndex(function (m) {
                    return m.id == empid;
                });

            if (Nempindex > -1) {

                $scope.currenttaskteam[Nempindex].usertasks =
                    nelist.usertasks || [];

                $scope.currenttaskteam[Nempindex].taskSegments =
                    nelist.taskSegments || [];

                $scope.currenttaskteam[Nempindex].ShiftStart =
                    nelist.ShiftStart;

                $scope.currenttaskteam[Nempindex].ShiftHours =
                    nelist.ShiftHours;

                $scope.currenttaskteam[Nempindex].ShiftEnd =
                    nelist.ShiftEnd;
            }
            else {

                $scope.currenttaskteam.push(nelist);
            }

$scope.applyTimelineToEmployees();
$scope.rebindAssignDragFast(30);
            $scope.$applyAsync();

        })
        .catch(function (error) {
            console.error("Assign Task Error:", error);

            alert(
                error.message ||
                "Unable to assign task."
            );

        });
};

   

   
    //#endregion 
    

    //#region Add utility
    function parseDate(value) {
        var parts = getDateOnlyParts(value);
        if (!isValidDateOnlyParts(parts)) return null;

        // Keep the existing Date-object contract required by the timeline.
        // Local noon is created from already-normalized calendar components, so
        // the same project date is displayed in every browser country.
        // Never convert this date-only value with toISOString().
        return new Date(parts.year, parts.month - 1, parts.day, 12, 0, 0, 0);
    }
    function parseDateold(val) {
        return parseDate(val);
    }
    $scope.formatDateToDDMMYYYY = function (date) {
        var parts = getDateOnlyParts(date);
        if (!isValidDateOnlyParts(parts)) return '';
        return String(parts.day).padStart(2, '0') + '/' +
            String(parts.month).padStart(2, '0') + '/' +
            parts.year;
    };
    $scope.formatDateToMMDDYYYY = function (date) {
        var parts = getDateOnlyParts(date);
        if (!isValidDateOnlyParts(parts)) return '';
        return String(parts.month).padStart(2, '0') + '/' +
            String(parts.day).padStart(2, '0') + '/' +
            parts.year;
    };
    $scope.confirmCopy = function () {
        Swal.fire({
            title: "Copy?",
            text: "Are you sure you want to copy this project data?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Copy",
            cancelButtonText: "Cancel"
        }).then((result) => {
            if (result.isConfirmed) {
                $scope.savecopy();
            }
        });
    };
    $scope.formatDuration = function (c2) {

        let v = (c2.TaskDuration || '').replace(/\D/g, ''); // only digits

        // before = all digits except last 2
        let before = v.slice(0, -2);
        let after = v.slice(-2);

        if (v.length === 0) {
            c2.TaskDuration = '/';
        }
        else if (v.length <= 2) {
            c2.TaskDuration = v + '/';
        }
        else {
            c2.TaskDuration = before + '/' + after;
        }
    };
    $scope.normalizeTime = function (val) {

        if (!val) return "";
        val = val.toString().replace(/[^0-9]/g, ""); // remove : if exists
        //if (val.length === 3) {
        //val = "0" + val;
        //}
        //if (val.length !== 4) return val;
        //return val.substring(0, 2) + ":" + val.substring(2, 4);
        val = val.substring(val.length - 2, val.length);
        return val;
    };

    $scope.normalizeTime_calhr_new = function (val1, val2) {



        let val_hr = (val1 == "" ? "0" : val1);
        let val_min = (val2 == "" ? "0" : val2);

        var ret = (parseInt(val_hr) * 60) + parseInt(val_min);
        return ret;
    };


    function getPerDayHours() {
        var value = Number($scope.perdayhrs);
        if (!isFinite(value) || value <= 0) value = 8;
        return value;
    }

    // PMS rule: one working date can contain at most $scope.perdayhrs hours.
    // txtdays is preferred because it already represents working days.
    // A 0-day row still represents its selected date and therefore gets one day slot.
    function getPerDayDateCapacity(item, startDate, endDate) {
        var validationDays = 0;
        var rawDays = item && item.txtdays !== null && item.txtdays !== undefined
            ? String(item.txtdays).trim()
            : '';

        if (rawDays !== '') {
            var durationDays = Number(rawDays);
            if (isFinite(durationDays) && durationDays >= 0) {
                validationDays = durationDays === 0 ? 1 : durationDays;
            }
        }

        if (validationDays <= 0) {
            var start = normalizeHierarchyDateOnly(startDate);
            var end = normalizeHierarchyDateOnly(endDate);

            if (start && end) {
                start.setHours(0, 0, 0, 0);
                end.setHours(0, 0, 0, 0);
                validationDays = Math.max(
                    1,
                    Math.floor((end - start) / 86400000) + 1
                );
            }
        }

        if (!isFinite(validationDays) || validationDays <= 0) {
            validationDays = 1;
        }
        var perDayHours = getPerDayHours();
        return {
            days: validationDays,
            perDayHours: perDayHours,
            hours: validationDays * perDayHours,
            minutes: Math.round(validationDays * perDayHours * 60)
        };
    }

    function formatWorkingMinutesForCapacity(totalMinutes) {
        var minutes = Math.max(0, Math.round(Number(totalMinutes) || 0));
        return Math.floor(minutes / 60) + ':' +
            String(minutes % 60).padStart(2, '0');
    }

    // Capacity validation only:
    // saved txtdays=0 remains 0, but is treated as one working date while
    // checking hours. End-date calculation and database txtdays are unchanged.
    function getHierarchyDurationCapacityError(type, item, candidateMinutes) {
        if (!item) return '';

        var rawDays = item.txtdays === null || item.txtdays === undefined
            ? ''
            : String(item.txtdays).trim();

        if (rawDays === '') {
            return $scope.getHierarchyItemLabel(type, item) +
                ' duration days cannot be blank.';
        }

        var durationDays = Number(rawDays);
        if (!isFinite(durationDays) || durationDays < 0) {
            return $scope.getHierarchyItemLabel(type, item) +
                ' duration days is invalid.';
        }

        var validationDays = durationDays === 0 ? 1 : durationDays;
        var allowedMinutes = Math.round(
            validationDays * getPerDayHours() * 60
        );
        var workingMinutes = candidateMinutes;

        if (workingMinutes === undefined || workingMinutes === null) {
            workingMinutes = $scope.getDurationMinutesForValidation
                ? $scope.getDurationMinutesForValidation(item)
                : 0;
        }

        workingMinutes = Math.max(0, Math.round(Number(workingMinutes) || 0));

        if (workingMinutes > allowedMinutes) {
            return $scope.getHierarchyItemLabel(type, item) +
                ' Working Hrs ' + formatWorkingMinutesForCapacity(workingMinutes) +
                ' cannot be greater than ' +
                formatWorkingMinutesForCapacity(allowedMinutes) +
                ' for ' + rawDays + ' duration days.';
        }

        return '';
    }

    $scope.validateHierarchyDurationCapacity = function (type, item, candidateMinutes, showAlert) {
        var message = getHierarchyDurationCapacityError(type, item, candidateMinutes);
        $scope._lastDurationCapacityValidationMessage = message;
        if (message && showAlert !== false) alert(message);
        return !message;
    };

    function getHierarchyHourTexts(item) {
        item = item || {};
        var rawHours = item.TaskDuration;
        if (rawHours === undefined || rawHours === null) {
            rawHours = item.taskduration;
        }

        var rawMinutes = item.TaskDuration_min;
        if (rawMinutes === undefined || rawMinutes === null) {
            rawMinutes = item.taskduration_min;
        }

        return {
            hours: rawHours === undefined || rawHours === null
                ? ''
                : String(rawHours).trim(),
            minutes: rawMinutes === undefined || rawMinutes === null
                ? ''
                : String(rawMinutes).trim()
        };
    }

    function validateHierarchyResetWorkingHours(type, item) {
        var values = getHierarchyHourTexts(item);
        if (values.hours === '' && values.minutes === '') {
            alert(
                $scope.getHierarchyItemLabel(type, item) +
                ' Working Hrs cannot be blank.'
            );
            return false;
        }

        var capacityMessage = getHierarchyDurationCapacityError(type, item, null);
        if (capacityMessage) {
            alert(capacityMessage);
            return false;
        }
        return true;
    }

    function validateHierarchyResetBranchHours(type, list, startIndex) {
        list = list || [];
        startIndex = Math.max(0, parseInt(startIndex, 10) || 0);

        for (var i = startIndex; i < list.length; i++) {
            var item = list[i];
            if (!validateHierarchyResetWorkingHours(type, item)) {
                return false;
            }

            if (type === 'milestone') {
                var tasks = item.tasks || [];
                for (var t = 0; t < tasks.length; t++) {
                    if (!validateHierarchyResetWorkingHours('task', tasks[t])) {
                        return false;
                    }
                    var taskSubtasks = tasks[t].subtasks || [];
                    for (var s = 0; s < taskSubtasks.length; s++) {
                        if (!validateHierarchyResetWorkingHours('subtask', taskSubtasks[s])) {
                            return false;
                        }
                    }
                }
            } else if (type === 'task') {
                var subtasks = item.subtasks || [];
                for (var z = 0; z < subtasks.length; z++) {
                    if (!validateHierarchyResetWorkingHours('subtask', subtasks[z])) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    function parseHierarchyTimeMinutes(value) {
        var text = String(value || '').trim().toUpperCase();
        if (!text) return null;

        var match = /^(\d{1,2}):(\d{2})(?:\s*(AM|PM))?$/.exec(text);
        if (!match) return null;

        var hours = parseInt(match[1], 10);
        var minutes = parseInt(match[2], 10);
        var period = match[3] || '';

        if (minutes < 0 || minutes > 59) return null;
        if (period) {
            if (hours < 1 || hours > 12) return null;
            if (period === 'AM' && hours === 12) hours = 0;
            if (period === 'PM' && hours !== 12) hours += 12;
        } else if (hours < 0 || hours > 23) {
            return null;
        }

        return (hours * 60) + minutes;
    }

    function formatHierarchyTimeMinutes(totalMinutes) {
        totalMinutes = Math.round(Number(totalMinutes) || 0);
        totalMinutes = ((totalMinutes % 1440) + 1440) % 1440;

        var hours24 = Math.floor(totalMinutes / 60);
        var minutes = totalMinutes % 60;
        var period = hours24 >= 12 ? 'PM' : 'AM';
        var hours12 = hours24 % 12;
        if (hours12 === 0) hours12 = 12;

        return String(hours12).padStart(2, '0') + ':' +
            String(minutes).padStart(2, '0') + ' ' + period;
    }

    function getHierarchyResetStartTime(item) {
        var value = item && (item.StartTime || item.starttime);
        var parsed = parseHierarchyTimeMinutes(value);
        return parsed === null ? 9 * 60 : parsed;
    }

    function createHierarchyResetCursor(dateValue, item) {
        var date = cloneHierarchyDateOnly(dateValue);
        if (!date) return null;

        return {
            date: date,
            dayStartMinutes: getHierarchyResetStartTime(item),
            offsetMinutes: 0
        };
    }

    function cloneHierarchyResetCursor(cursor) {
        if (!cursor || !cursor.date) return null;
        return {
            date: cloneHierarchyDateOnly(cursor.date),
            dayStartMinutes: cursor.dayStartMinutes,
            offsetMinutes: cursor.offsetMinutes
        };
    }

    function getHierarchyResetCursorStart(cursor) {
        var copy = cloneHierarchyResetCursor(cursor);
        if (!copy) return null;

        return {
            date: copy.date,
            time: formatHierarchyTimeMinutes(
                copy.dayStartMinutes + copy.offsetMinutes
            )
        };
    }

    function applyHierarchyResetCursor(item, type, cursor) {
        var start = getHierarchyResetCursorStart(cursor);
        if (!start || !item) return null;

        if (item._oldStartTime === undefined) {
            item._oldStartTime = angular.copy(
                item.StartTime || item.starttime || ''
            );
        }

        if (type === 'milestone') {
            item.StartDate = cloneHierarchyDateOnly(start.date);
        } else {
            item.Startdate = cloneHierarchyDateOnly(start.date);
        }

        item.StartTime = start.time;
        item.starttime = start.time;
        return start;
    }

    function getHierarchyResetSpan(cursor, item) {
        var durationMinutes = $scope.getDurationMinutesForValidation
            ? $scope.getDurationMinutesForValidation(item)
            : 0;
        durationMinutes = Math.max(0, Number(durationMinutes) || 0);

        var perDayMinutes = Math.max(1, Math.round(getPerDayHours() * 60));
        var startOffsetMinutes = cursor
            ? Math.max(0, Number(cursor.offsetMinutes) || 0)
            : 0;
        var totalMinutes = startOffsetMinutes + durationMinutes;

        return {
            durationMinutes: durationMinutes,
            perDayMinutes: perDayMinutes,
            totalMinutes: totalMinutes,
            workDays: Math.max(1, Math.ceil(totalMinutes / perDayMinutes)),
            remainingMinutes: totalMinutes % perDayMinutes
        };
    }

    // Subtask, task and milestone sequencing is hour based. The next item
    // starts from cumulative working minutes and $scope.perdayhrs.
    // This calculation is intentionally independent from the displayed EndDate,
    // because EndDate is controlled only by the item's txtdays value.
    async function advanceHierarchyResetCursorByHours(cursor, item) {
        var next = cloneHierarchyResetCursor(cursor);
        if (!next) return null;

        var span = getHierarchyResetSpan(next, item);
        if (span.durationMinutes <= 0) return next;

        var dateDays = span.remainingMinutes > 0
            ? span.workDays
            : span.workDays + 1;

        var resolvedDate = null;
        try {
            var response = await assignprojectService.getnew_enddate(
                toServerDateOnly(next.date),
                dateDays,
                resolveHierarchyProjectId(item)
            );
            resolvedDate = hierarchyResponseDate(response);
        } catch (error) {
            console.error('Unable to resolve next reset start date from working hours', error);
        }

        if (!resolvedDate) {
            resolvedDate = addHierarchyCalendarDays(
                next.date,
                span.remainingMinutes > 0
                    ? span.workDays - 1
                    : span.workDays
            );
        }

        next.date = cloneHierarchyDateOnly(resolvedDate);
        next.offsetMinutes = span.remainingMinutes > 0
            ? span.remainingMinutes
            : 0;

        return next;
    }

    // All hierarchy levels use the same cumulative working-hours cursor.
    // The next subtask, task or milestone starts from the previous item's
    // actual working finish date/time. The displayed EndDate is still
    // calculated separately from that item's txtdays value.

    $scope.validateTime = function (c, index, ctype) {
        if (!c || !c.TaskDuration_min) return;
        //val = $scope.normalizeTime(c.TaskDuration);
        var min = c.TaskDuration_min;

        if (min >= 60) {
            alert("Minutes must be less than 60");
            c.TaskDuration_min = "";
            setTimeout(function () {
                var el = document.querySelectorAll("input[ng-model='c.TaskDuration_min']")[index];
                if (ctype == "task")
                    el = document.querySelectorAll("input[ng-model='c1.TaskDuration_min']")[index];
                if (ctype == "subtask")
                    el = document.querySelectorAll("input[ng-model='c2.TaskDuration_min']")[index];
                if (el) el.focus();
            }, 0);
            return false;
        }
        return true;
    };

    $scope.validateTime_actual = function (c) {
        if (!c || !c.TaskDuration) return;
        val = $scope.normalizeTime(c.TaskDuration);

        var match = val.match(/^([01]?\d|2[0-3]):([0-5]\d)$/);

        if (!match) {
            alert("Invalid time. Use HH:MM format");
            c.TaskDuration = "";
            return false;
        }

        var min = parseInt(match[2], 10);

        if (min >= 60) {
            alert("Minutes must be less than 60");
            c.TaskDuration = "";
            return false;
        }

        // normalize (keep HH:MM only)
        c.TaskDuration =
            match[1].padStart(2, "0") + ":" +
            match[2].padStart(2, "0");

        return true;
    };
    $scope.formatDuration = function (val) {

        if (val == null || val === "") return "";

        // if number (hours)
        if (!isNaN(val)) {
            var h = parseInt(val, 10);
            return (h < 10 ? "0" + h : h) + ":00";
        }

        return val; // already formatted
    };
    // Universal hierarchy date parser.
    // It accepts yyyy-MM-dd, UI dates, Date objects and legacy /Date(...)/ values,
    // but always rebuilds a local calendar date from year/month/day parts.
    // No UTC conversion or toISOString() is used.
    function parseDotNetDate(value) {
        return normalizeHierarchyDateOnly(value);
    }

    function normalizeHierarchyDateOnly(value) {
        var parts = getDateOnlyParts(value);
        if (!isValidDateOnlyParts(parts)) return null;

        // Local noon avoids midnight/DST boundary shifts while preserving
        // the same calendar date in every browser timezone.
        return new Date(
            parts.year,
            parts.month - 1,
            parts.day,
            12,
            0,
            0,
            0
        );
    }

    function cloneHierarchyDateOnly(value) {
        var date = normalizeHierarchyDateOnly(value);
        if (!date) return null;

        return new Date(
            date.getFullYear(),
            date.getMonth(),
            date.getDate(),
            12,
            0,
            0,
            0
        );
    }

    function addHierarchyCalendarDays(value, days) {
        var date = cloneHierarchyDateOnly(value);
        if (!date) return null;

        date.setDate(date.getDate() + Number(days || 0));
        return cloneHierarchyDateOnly(date);
    }

    function hierarchyResponseDate(response) {
        if (!response || !response.data) return null;
        return normalizeHierarchyDateOnly(response.data.d);
    }

    async function refreshHierarchyLeftDays(item, itemType) {
        var response = await assignprojectService.getleftdays(item.id, itemType);
        if (
            response &&
            response.data &&
            response.data.d !== undefined &&
            response.data.d !== null
        ) {
            item.leftdays = response.data.d;
        }
    }

    $scope.isnumeric = function (event) {
        let charCode = event.which || event.keyCode;

        // Allow: backspace, tab, enter, arrows, delete
        if (
            charCode === 8 ||   // backspace
            charCode === 9 ||   // tab
            charCode === 13 ||  // enter
            charCode === 46 ||  // delete / dot
            (charCode >= 35 && charCode <= 40) // arrows
        ) {
            return true;
        }

        // Allow digits (0-9)
        if (charCode >= 48 && charCode <= 57) {
            return true;
        }

        // Block everything else
        event.preventDefault();
        return false;
    };
    $scope.highlightMatch = function (text, search) {
        if (!search || search.trim() === "") return text;
        let pattern = new RegExp("(" + search + ")", "gi");
        return text.replace(pattern, "<b>$1</b>");
    };
    $scope.initTableDrag11 = function () {
        alert("ok");
        if ($scope.view == "table") {

            initProjectTableColumnDragDrop();
        }
    };
    //#endregion
    document.addEventListener('click', function () {

        var scope = angular.element(document.body).scope();

        if (!scope) return;

        scope.$applyAsync(function () {

            angular.forEach(scope.ganttRows || [], function (r) {

                r.showActionMenu = false;

            });

        });

    });
    $scope.openDescriptionPopup = function ($event, c, colname, utype) {

        let rect = $event.target.getBoundingClientRect();

        Swal.fire({

            input: 'textarea',

            inputValue:
                c.miledescription ||
                c.taskdescripton ||
                c.SubTaskDescription ||
                '',

            showConfirmButton: false,
            showCancelButton: false,

            backdrop: false,

            width: '350px',

            customClass: {
                popup: 'small-desc-popup'
            },

            didOpen: function () {

                let popup = Swal.getPopup();

                // POSITION NEAR INPUT
                popup.style.position = 'fixed';

                popup.style.margin = '0';

                popup.style.left = rect.left + 'px';

                popup.style.top =
                    (rect.bottom + 5) + 'px';

                const textarea = Swal.getInput();

                textarea.focus();

                function saveData() {

                    var val = textarea.value || '';

                    if (utype == "milestone") {

                        c.miledescription = val;

                        $scope.updatedetails_mile(c, colname);
                    }

                    else if (utype == "task") {

                        c.taskdescripton = val;

                        $scope.updatedetails_task(c, colname);
                    }

                    else if (utype == "subtask") {

                        c.SubTaskDescription = val;

                        $scope.updatedetails_subtask(c, colname);
                    }

                    Swal.close();
                }

                textarea.addEventListener('keydown', function (e) {

                    if (e.key === 'Tab') {

                        e.preventDefault();

                        saveData();
                    }
                });

                textarea.addEventListener('blur', function () {

                    saveData();
                });
            }
        });
    };
    $scope.openDescriptionPopup2 = function (c, colname, utype) {

        Swal.fire({

            input: 'textarea',

            inputValue:
                c.miledescription ||
                c.taskdescription ||
                c.SubTaskDescription ||
                '',

            inputPlaceholder: 'Enter description...',

            showConfirmButton: false,

            showCancelButton: false,

            width: '400px',

            padding: '10px',

            allowOutsideClick: true,

            customClass: {
                popup: 'small-desc-popup',
                title: 'small-desc-title',
                input: 'small-desc-input'
            },

            // ===== POPUP OPEN =====
            didOpen: function () {

                const textarea = Swal.getInput();

                textarea.focus();

                // ===== SAVE =====
                function saveData() {

                    var val = textarea.value || '';

                    // MILESTONE
                    if (utype == "milestone") {

                        c.miledescription = val;

                        $scope.updatedetails_mile(c, colname);
                    }

                    // TASK
                    else if (utype == "task") {

                        c.taskdescripton = val;

                        $scope.updatedetails_task(c, colname);
                    }

                    // SUBTASK
                    else if (utype == "subtask") {

                        c.SubTaskDescription = val;

                        $scope.updatedetails_subtask(c, colname);
                    }

                    c.isDescPopupOpen = false;

                    Swal.close();
                }

                // TAB / CTRL+ENTER SAVE
                textarea.addEventListener('keydown', function (e) {

                    if (e.key === 'Tab') {

                        e.preventDefault();

                        saveData();
                    }

                    if (e.ctrlKey && e.key === 'Enter') {

                        e.preventDefault();

                        saveData();
                    }
                });

                // OUTSIDE CLICK SAVE
                document.addEventListener(
                    'click',
                    outsideClickHandler,
                    true
                );

                function outsideClickHandler(e) {

                    if (!Swal.getPopup()) return;

                    if (!Swal.getPopup().contains(e.target)) {

                        document.removeEventListener(
                            'click',
                            outsideClickHandler,
                            true
                        );

                        saveData();
                    }
                }
            },

            // ===== POPUP CLOSE =====
            willClose: function () {

                c.isDescPopupOpen = false;
            }
        });
    };
    $scope.openDescriptionPopupold = function (c, colname, utype) {
        Swal.fire({
            title: 'Description',
            input: 'textarea',
            inputValue: c.Description || '',
            inputPlaceholder: 'Enter description...',
            showCancelButton: true,
            confirmButtonText: 'Save',
            cancelButtonText: 'Close',
            width: '400px',
            padding: '10px',

            customClass: {
                popup: 'small-desc-popup',
                title: 'small-desc-title',
                input: 'small-desc-input',
                confirmButton: 'small-desc-btn',
                cancelButton: 'small-desc-btn'
            },

            inputValidator: (value) => {

                if (!value) {
                    return 'Please enter description';
                }
            }

        }).then((result) => {

            if (result.isConfirmed) {

                // UPDATE UI
                if (utype == "milestone") {
                    c.miledescription = result.value;
                    $scope.updatedetails_mile(c, colname);
                }

                Swal.fire({
                    icon: 'success',
                    title: 'Saved',
                    timer: 1000,
                    showConfirmButton: false
                });
            }
        });
    };


    //#region manage projectmaster list
    $scope.selectedprojectid = "0";
    $scope.selectedprojectname = "";
    $scope.selectProject = function (p) {
        $scope.selectedprojectid = "0";
        $scope.selectedprojectname = "";
        $scope.selectedprojectid = p.id;
        $scope.selectedprojectname = p.ProjectName;
    };
    $scope.getProjectName = function () {
        if (!$scope.projectmasterlist) return '';

        var proj = $scope.projectmasterlist.find(function (p) {
            return String(p.id) === String($scope.selectedprojectid);
        });

        return proj ? proj.ProjectName : '';
    };
    $scope.focusProjectSearch = function ($event) {
        setTimeout(function () {
            var dropdown = $event.target.closest(".dropdown");
            if (!dropdown) return;

            var input = dropdown.querySelector(".project-search");
            if (input) input.focus();
        }, 200);
    };
    $scope.loadprojectlist = function (cpid) {

        assignprojectService.getprojectmasterlist(cpid).then(function (response) {
            var data = response.data.d;
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            $scope.projectmasterlist = data || [];

        }, function (error) {
            console.error(error);
            alert("Error: " + error.status);
        });

    };
    $scope.loadprojectlist_reset = function (cpid) {
        //alert(cpid);
        //return;
        assignprojectService.getprevious_projectdetails(cpid).then(function (response) {
            var data = response.data.d;
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            $scope.projectlist = [];
            $scope.projectlist = data || [];
            $scope.refreshProjectDurationDays();

            $scope.isload = "1";
            angular.forEach($scope.projectlist.miles, function (c) {
                c.isExpanded = false;
                if (c.StartDate) {
                    c.StartDate = normalizeHierarchyDateOnly(c.StartDate);
                }
                if (c.EndDate) {
                    c.EndDate = normalizeHierarchyDateOnly(c.EndDate);
                }
                try {
                    //c.TaskDuration = $scope.formatDuration(c.TaskDuration);
                    c.TaskDuration = c.TaskDuration;
                }
                catch{ }

                // TASKS
                if (c.tasks && c.tasks.length > 0) {

                    angular.forEach(c.tasks, function (t) {

                        t.isExpanded = false;

                        if (t.Startdate) {
                            t.Startdate = normalizeHierarchyDateOnly(t.Startdate);
                        }
                        if (t.Targetdate) {
                            t.Targetdate = normalizeHierarchyDateOnly(t.Targetdate);
                        }
                        try {
                            //t.TaskDuration = $scope.formatDuration(t.TaskDuration);
                            t.TaskDuration = t.TaskDuration;
                        }
                        catch{ }
                        // SUBTASKS (c2 FIX)
                        if (t.subtasks && t.subtasks.length > 0) {

                            angular.forEach(t.subtasks, function (s) {


                                if (s.Startdate) {
                                    s.Startdate = normalizeHierarchyDateOnly(s.Startdate);
                                }
                                if (s.Targetdate) {
                                    s.Targetdate = normalizeHierarchyDateOnly(s.Targetdate);
                                }
                                try {
                                    //s.TaskDuration = $scope.formatDuration(s.TaskDuration);
                                    s.TaskDuration = s.TaskDuration;
                                }
                                catch{ }
                            });

                        }

                    });

                }

            });
            setTimeout(initTableUI, 300);

        }, function (error) {
            console.error(error);
            alert("Error: " + error.status);
        });

    };


    $scope.selectProject = function (p) {
        $scope.selectedprojectid = p.id;
        $scope.selectedprojectname = p.ProjectName;
    };
    $scope.savecopy = function () {
        if (!$scope.selectedprojectid || $scope.selectedprojectid == 0) {
            alert("Please select a project");
            return;
        }
        $scope.currentprojectid = $scope.projectlist.id;
        if (!$scope.currentprojectid || $scope.currentprojectid == 0) {
            alert("Invalid current project");
            return;
        }
        //if (!confirm("Are you sure you want to copy this project data?")) {
        //   return; // stop if user clicks Cancel
        //}
        debugger
        assignprojectService.copyProject($scope.currentprojectid, $scope.selectedprojectid)
            .then(function (res) {

                var data = res.data.d;
                if ($scope.currentprojectid) {
                    $timeout(function () {
                        $scope.loadprojectbyid($scope.currentprojectid);
                        $scope.showlist = false;
                        $scope.showAddForm = true;
                    }, 0);
                }
                var modalEl = document.getElementById('addcopy');
                var modal = bootstrap.Modal.getInstance(modalEl);
                if (modal) modal.hide();
                //alert("Copied successfully");


            })
            .catch(function (err) {
                debugger
                console.error(err);
                alert("Error occurred while copying");
            });
    };

    //#endregion


    //#region mange popup

    $scope.closemile = function () {

        document.getElementById('btnclsmile').click();
        if (document.getElementById("hdneditid").value == "1") {
            if (!$scope.currentlist) return;
            let obj = $scope.currentlist;
            let utype = $scope.currentFileType;
            let searchtype = $scope.currentUserType;
            let resultnew = null;
            // API call based on type
            var mid = "0";
            resultnew = assignprojectService.getitemdetails(obj.id, utype);

            if (!resultnew) return;

            resultnew.then(function (res) {
                let getlist = res.data.d;
                if (getlist) {
                    debugger
                    //alert(utype);
                    if (utype === 'milestone') {
                        mid = obj.id;
                        if (getlist.StartDate) {
                            getlist.StartDate = normalizeHierarchyDateOnly(getlist.StartDate);
                        }
                        if (getlist.EndDate) {
                            getlist.EndDate = normalizeHierarchyDateOnly(getlist.EndDate);
                        }
                        let mindex = $scope.projectlist.miles.findIndex(m => m.id == obj.id);
                        let ulist = $scope.projectlist.miles[mindex];
                        ulist.Milestone = getlist.Milestone;
                        ulist.Weightage = getlist.Weightage;
                        ulist.StartDate = getlist.StartDate;
                        ulist.EndDate = getlist.EndDate;
                        ulist.Owner = getlist.Owner;
                        ulist.Access = getlist.Access;
                        ulist.team = getlist.team;
                        ulist.P_Status = getlist.P_Status;
                        ulist.status_name = getlist.status_name;
                        ulist.status_color = getlist.status_color;
                        ulist.Priority = getlist.Priority;
                        ulist.priority_name = getlist.priority_name;
                        ulist.priority_color = getlist.priority_color;
                        ulist.Budget = getlist.Budget;
                        ulist.Actual = getlist.Actual;
                        ulist.uid = getlist.uid;
                        ulist.cdate = getlist.cdate;
                        ulist.statuscomplete = getlist.statuscomplete;
                        ulist.istask = getlist.istask;
                        ulist.checkmileWeightage = getlist.checkmileWeightage;
                        ulist.iscomplete = getlist.iscomplete;
                        ulist.iscompletedate = getlist.iscompletedate;
                        ulist.iscompletepercentage = getlist.iscompletepercentage;
                        ulist.Durationtype = getlist.Durationtype;
                        ulist.TaskDuration = getlist.TaskDuration;
                        ulist.Durationhourcal = getlist.Durationhourcal;
                        ulist.orderno = getlist.orderno;
                        ulist.miledescription = getlist.miledescription;
                        ulist.Uploaddrawing = getlist.Uploaddrawing;
                        ulist.txtdays = getlist.txtdays;
                        ulist.complete_value = getlist.complete_value;
                        ulist.pending_value = getlist.pending_value;
                        ulist.taskcount = getlist.taskcount;
                        ulist.leftdays = getlist.leftdays;
                        ulist.owners = getlist.owners;
                        ulist.teams = getlist.teams;
                        ulist.viewfiles = getlist.viewfiles;
                        ulist.tasks = getlist.tasks;
                        // TASKS
                        if (ulist.tasks && ulist.tasks.length > 0) {

                            angular.forEach(ulist.tasks, function (t) {                                

                                if (t.Startdate) {
                                    t.Startdate = normalizeHierarchyDateOnly(t.Startdate);
                                }
                                if (t.Targetdate) {
                                    t.Targetdate = normalizeHierarchyDateOnly(t.Targetdate);
                                }
                                try {
                                    //t.TaskDuration = $scope.formatDuration(t.TaskDuration);
                                    t.TaskDuration = t.TaskDuration;
                                }
                                catch{ }
                                // SUBTASKS (c2 FIX)
                                if (t.subtasks && t.subtasks.length > 0) {

                                    angular.forEach(t.subtasks, function (s) {


                                        if (s.Startdate) {
                                            s.Startdate = normalizeHierarchyDateOnly(s.Startdate);
                                        }
                                        if (s.Targetdate) {
                                            s.Targetdate = normalizeHierarchyDateOnly(s.Targetdate);
                                        }
                                        try {
                                            //s.TaskDuration = $scope.formatDuration(s.TaskDuration);
                                            s.TaskDuration = s.TaskDuration;
                                        }
                                        catch{ }
                                    });

                                }

                            });

                        }
                    }
                    else if (utype === 'task') {
                        mid = obj.Milestoneid;
                        if (getlist.Startdate) {
                            getlist.Startdate = normalizeHierarchyDateOnly(getlist.Startdate);
                        }
                        if (getlist.Targetdate) {
                            getlist.Targetdate = normalizeHierarchyDateOnly(getlist.Targetdate);
                        }

                        let mindex = $scope.projectlist.miles.findIndex(m => m.id == obj.Milestoneid);
                        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == obj.id);
                        let ulist = $scope.projectlist.miles[mindex].tasks[tindex];
                        ulist.taskname = getlist.taskname;
                        ulist.taskdescripton = getlist.taskdescripton;
                        ulist.owner = getlist.owner;
                        ulist.team = getlist.team;
                        ulist.Startdate = getlist.Startdate;
                        ulist.Targetdate = getlist.Targetdate;
                        ulist.Weightage = getlist.Weightage;
                        ulist.StartTime = getlist.StartTime;
                        ulist.Timeline = getlist.Timeline;
                        ulist.TaskDuration = getlist.TaskDuration;
                        ulist.P_Status = getlist.P_Status;
                        ulist.status_name = getlist.status_name;
                        ulist.status_color = getlist.status_color;
                        ulist.Priority = getlist.Priority;
                        ulist.priority_name = getlist.priority_name;
                        ulist.priority_color = getlist.priority_color;
                        ulist.Budget = getlist.Budget;
                        ulist.Actual = getlist.Actual;
                        ulist.Reminder = getlist.Reminder;
                        ulist.ReminderType = getlist.ReminderType;
                        ulist.Days = getlist.Days;
                        ulist.Time = getlist.Time;
                        ulist.uid = getlist.uid;
                        ulist.cdate = getlist.cdate;
                        ulist.tagto = getlist.tagto;
                        ulist.dependancy = getlist.dependancy;
                        ulist.issubtask = getlist.issubtask;
                        ulist.checkTaskWeightage = getlist.checkTaskWeightage;
                        ulist.iscomplete = getlist.iscomplete;
                        ulist.iscompletepercentage = getlist.iscompletepercentage;
                        ulist.orderno = getlist.orderno;
                        ulist.txtdays = getlist.txtdays;
                        ulist.complete_value = getlist.complete_value;
                        ulist.pending_value = getlist.pending_value;
                        ulist.leftdays = getlist.leftdays;
                        ulist.owners = getlist.owners;
                        ulist.teams = getlist.teams;
                        ulist.viewfiles = getlist.viewfiles;
                        ulist.subtasks = getlist.subtasks;
                        //Subtask
                       
                               
                        if (ulist.subtasks && ulist.subtasks.length > 0) {

                            angular.forEach(ulist.subtasks, function (s) {


                                        if (s.Startdate) {
                                            s.Startdate = normalizeHierarchyDateOnly(s.Startdate);
                                        }
                                        if (s.Targetdate) {
                                            s.Targetdate = normalizeHierarchyDateOnly(s.Targetdate);
                                        }
                                        try {
                                            //s.TaskDuration = $scope.formatDuration(s.TaskDuration);
                                            s.TaskDuration = s.TaskDuration;
                                        }
                                        catch{ }
                                    });

                                }

                            

                    }
                    else if (utype === 'subtask') {
                        mid = obj.Milestoneid;
                        if (getlist.Startdate) {
                            getlist.Startdate = normalizeHierarchyDateOnly(getlist.Startdate);
                        }
                        if (getlist.Targetdate) {
                            getlist.Targetdate = normalizeHierarchyDateOnly(getlist.Targetdate);
                        }
                        mid = obj.Milestoneid;
                        let mindex = $scope.projectlist.miles.findIndex(m => m.id == obj.Milestoneid);
                        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == obj.Task);
                        let stindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == obj.id);
                        let ulist = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[stindex];
                        ulist.Subtaskname = getlist.Subtaskname;
                        ulist.SubTaskDescription = getlist.SubTaskDescription;
                        ulist.owner = getlist.owner;
                        ulist.team = getlist.team;
                        ulist.Startdate = getlist.Startdate;
                        ulist.Targetdate = getlist.Targetdate;
                        ulist.Weightage = getlist.Weightage;
                        ulist.StartTime = getlist.StartTime;
                        ulist.Timeline = getlist.Timeline;
                        ulist.TaskDuration = getlist.TaskDuration;
                        ulist.P_Status = getlist.P_Status;
                        ulist.status_name = getlist.status_name;
                        ulist.status_color = getlist.status_color;
                        ulist.Priority = getlist.Priority;
                        ulist.priority_name = getlist.priority_name;
                        ulist.priority_color = getlist.priority_color;
                        ulist.Budget = getlist.Budget;
                        ulist.Actual = getlist.Actual;
                        ulist.Reminder = getlist.Reminder;
                        ulist.ReminderType = getlist.ReminderType;
                        ulist.Days = getlist.Days;
                        ulist.Time = getlist.Time;
                        ulist.uid = getlist.uid;
                        ulist.cdate = getlist.cdate;
                        ulist.tagto = getlist.tagto;
                        ulist.dependancy = getlist.dependancy;
                        ulist.iscomplete = getlist.iscomplete;
                        ulist.iscompletepercentage = getlist.iscompletepercentage;
                        ulist.orderno = getlist.orderno;
                        ulist.txtdays = getlist.txtdays;
                        ulist.complete_value = getlist.complete_value;
                        ulist.pending_value = getlist.pending_value;
                        ulist.leftdays = getlist.leftdays;
                        ulist.owners = getlist.owners;
                        ulist.teams = getlist.teams;
                        ulist.viewfiles = getlist.viewfiles;


                    }
                }
                if ($scope.view == "gantt") {
                    $scope.updateGanttCompletion(mid);
                }

            })
                .catch(function (err) {
                    console.error(err);
                    alert("Update failed");
                })
                .finally(function () {

                });
        }
        $scope.$applyAsync();
    };
    $scope.closepopup_canvas_issue = function () {

        try {
            var btn = (window.parent && window.parent.document)
                ? window.parent.document.getElementById("btnclosemainpopup_issue")
                : document.getElementById("btnclosemainpopup_issue");
            if (btn == null) {
                btn = document.getElementById("btnclosemainpopup_issue");
            }
            if (btn) {
                btn.click();
            }
        }
        catch
        {

        }

        if (!$scope.currentlist) return;
        let obj = $scope.currentlist;
        let utype = $scope.currentFileType;
        let searchtype = $scope.currentUserType;
        let resultnew = null;
        // API call based on type
        if (utype === 'milestone') {
            resultnew = assignprojectService.getissuesdetails(obj.id, utype);
        }
        else if (utype === 'task') {
            resultnew = assignprojectService.getissuesdetails(obj.id, utype);
        }
        else if (utype === 'subtask') {
            resultnew = assignprojectService.getissuesdetails(obj.id, utype);
        }

        if (!resultnew) return;

        resultnew.then(function (res) {
            let getlist = res.data.d;
            if (getlist) {
                obj.totalissue = getlist.totalissue || 0;
                obj.completeissue = getlist.completeissue || 0;
                obj.pendingissue = getlist.pendingissue || 0;

                // Calculate percentages safely
                obj.completeissue_rate = getlist.completeissues_rate || 0;
                obj.pendingissue_rate = getlist.pendingissues_rate || 0;
            }

        })
            .catch(function (err) {
                console.error(err);
                alert("Update failed");
            })
            .finally(function () {

            });
    };
    $scope.closepopup_canvas_dependency = function () {

        try {
            var btn = (window.parent && window.parent.document)
                ? window.parent.document.getElementById("btnclosemainpopupD")
                : document.getElementById("btnclosemainpopupD");
            if (btn == null) {
                btn = document.getElementById("btnclosemainpopupD");
            }
            if (btn) {
                btn.click();
            }
        }
        catch
        {

        }
        if (!$scope.currentlist) return;
        let obj = $scope.currentlist;
        let utype = $scope.currentFileType;
        let searchtype = $scope.currentUserType;
        let resultnew = null;
        if (utype === 'milestone') {
            resultnew = assignprojectService.getdependencydetails(obj.id, utype);
        }
        else if (utype === 'task') {
            resultnew = assignprojectService.getdependencydetails(obj.id, utype);
        }
        else if (utype === 'subtask') {
            resultnew = assignprojectService.getdependencydetails(obj.id, utype);
        }

        if (!resultnew) return;

        resultnew.then(function (res) {
            let getD = res.data.d;
            //alert(getD);
            obj.totaldependency = getD;

        })
            .catch(function (err) {
                console.error(err);
                alert("Update failed");
            })
            .finally(function () {

            });


    };
    $scope.loadform = function loadsrc(c, utype, seearchtype, val1, val2, val3) {
        $scope.currentlist = c;
        $scope.currentUserType = seearchtype;
        $scope.currentFileType = utype;
        count11++;

        $scope.modalTitle = "Manage " + val3;
        document.getElementById(val1).src = "";
        if (val2.includes("aspx?")) {
            document.getElementById(val1).src = val2 + "&ctype=ang&count=" + count11;
        }
        else {

            document.getElementById(val1).src = val2 + "?ctype=ang&count=" + count11;
        }
        try {

            //document.getElementById("").innerHTML = val3;
        }
        catch (ex) {
        }
    };
    $scope.loadformedit = function (c, utype, frameId, url, title) {

        document.getElementById("hdneditid").value = "0";
        $scope.currentlist = c;
        $scope.currentFileType = utype;
        // safe counter
        $scope.popupCounter = ($scope.popupCounter || 0) + 1;

        // modal title
        $scope.modalTitle = "Edit " + title;

        // build URL
        let finalUrl = "";

        if (url.indexOf("aspx?") !== -1) {
            finalUrl = url + "&ctype=ang&count=" + $scope.popupCounter;
        } else {
            finalUrl = url + "?ctype=ang&count=" + $scope.popupCounter;
        }

        // set iframe src
        let frame = document.getElementById(frameId);
        if (frame) {
            frame.src = "";
            frame.src = finalUrl;
        }

    };
    $scope.loadformedit_gantt = function (c, utype, frameId, url, title) {

        if (utype == "milestone") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
            if (mindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex];
                $scope.loadformedit(c1, utype, frameId, url, title);
            }
        }
        else if (utype == "task") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);

            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
            if (tindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
                $scope.loadformedit(c1, utype, frameId, url, title);
            }
        }
        else if (utype == "subtask") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
            if (tindex !== -1) {
                let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);
                if (sindex !== -1) {
                    let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                    $scope.loadformedit(c1, utype, frameId, url, title);
                }
            }
        }

    };
    //#endregion
    //#region mange owners and teams  


    // Toggle selection on click
    $scope.toggleOwner = function (owner) {
        if (!$scope.requireEdit($scope.currentFileType)) return;
        var index = $scope.selectedOwnerIds.indexOf(owner.id);

        if (index > -1) {
            // $scope.selectedOwnerIds.splice(index, 1); // deselect
            //if($scope.currentUserType=="team")
            //{
            //$scope.removeTeamToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,owner.id);
            //   }
            //else
            //{
            //$scope.removeOwnerToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,owner.id);
            //}
        } else {

            $scope.selectedOwnerIds.push(owner.id); // select
            if ($scope.currentUserType == "team") {
                $scope.updateTeamToServer($scope.currentFileType, $scope.currentlist, $scope.selectedOwnerIds, owner.id);
            }
            else {
                $scope.updateOwnerToServer($scope.currentFileType, $scope.currentlist, $scope.selectedOwnerIds, owner.id);
            }
        }

        initTableUI();
    };

    // Remove selected owner
    $scope.removeOwner = function (ownerId) {
        if (!$scope.requireEdit($scope.currentFileType)) return;
        var index = $scope.selectedOwnerIds.indexOf(ownerId);
        if (index > -1) {
            if ($scope.currentUserType == "team") {
                var remainingIds = $scope.selectedOwnerIds.filter(function (id) { return id !== ownerId; });
                $scope.removeTeamToServer($scope.currentFileType, $scope.currentlist, remainingIds, ownerId);
            }
            else {
                $scope.selectedOwnerIds.splice(index, 1);
                $scope.removeOwnerToServer($scope.currentFileType, $scope.currentlist, $scope.selectedOwnerIds, ownerId);
            }

        }
        initTableUI();

    };

    // Display only employees who are not already selected in Add Team.
    $scope.isAvailableTeamPerson = function (person) {
        if (!person) return false;
        return !($scope.selectedOwnerIds || []).some(function (id) {
            return String(id) === String(person.id);
        });
    };

    // Get selected owner objects for displaying tags
    $scope.getSelectedOwners = function () {
        return $scope.owners.filter(o => $scope.selectedOwnerIds.includes(o.id));
    };

    $scope.closelist = function () {
        if ($scope.showOwnerDropdown == true)
            $scope.showOwnerDropdown = false;
    };
    $scope.openlist = function () {
        if ($scope.showOwnerDropdown == false)
            $scope.showOwnerDropdown = true;
    };

    $scope.loadSrcOwners = function (c, addtype, strtype, index) {
        if (!$scope.requireEdit(strtype)) return;
        if (strtype == "milestone") {
            $scope.currentFMilestoneId = c.id;
        }
        else if (strtype == "task" || strtype == "subtask") {
            $scope.currentFMilestoneId = c.Milestoneid;
            if (strtype == "subtask") {
                $scope.currentFTaskId = c.Task;
            }
        }



        $scope.currentFileType = "";
        $scope.currentFProjectId = "";
        $scope.currentFId = "";
        $scope.currentUserType = "";


        $scope.currentUserType = addtype;
        $scope.currentlist = c;
        $scope.currentIndex = index;
        $scope.currentFileType = strtype;
        $scope.currentFProjectId = c.ProjectId;
        $scope.currentFId = c.id;

        switch (addtype) {
            case "owner":
                $scope.modalTitle = "Add owners";
                break;
            case "team":
                $scope.modalTitle = "Add teams";
                break;
            default:
                $scope.modalTitle = "Add owners";
        }
        $scope.selectedItem = c;
        $scope.selectedIndex = index;
        // Example: open modal flag
        $scope.showOwnerDropdown = false;
        $scope.selectedOwnerIds = [];
        if (addtype == "owner") {
            if (c.owners && c.owners.length > 0) {
                angular.forEach(c.owners, function (owner) {
                    $scope.selectedOwnerIds.push(owner.id);;
                });
            }
        }
        else if (addtype == "team") {
            if (c.teams && c.teams.length > 0) {
                angular.forEach(c.teams, function (owner) {
                    $scope.selectedOwnerIds.push(owner.id);;
                });
            }
        }
    };
    $scope.loadSrcOwnersnew = function (c, addtype, strtype, index) {
        if (!$scope.requireEdit(strtype)) return;
        let refId;
        let reftype = "";
        if (strtype === "milestone") {
            $scope.currentFMilestoneId = c.id;
            refId = c.ProjectId;
            reftype = "project";
            //$scope.loaduserlist_byrefid(refId, 'project', addtype);
        } else if (strtype === "task" || strtype === "subtask") {
            $scope.currentFMilestoneId = c.Milestoneid;
            if (strtype === "task") {
                refId = c.Milestoneid;
                reftype = "milestone";
                //$scope.loaduserlist_byrefid(refId, 'milestone', addtype);
            } else {
                $scope.currentFTaskId = c.Task;
                refId = c.Task;
                reftype = "task";
                //$scope.loaduserlist_byrefid(refId, 'task', addtype);
            }
        }
        refId = "0";
        $scope.loaduserlist_byrefid(refId, 'project', addtype);
        // Set scope variables
        $scope.currentUserType = addtype;
        $scope.currentFileType = strtype;
        $scope.currentFProjectId = c.ProjectId;
        $scope.currentFId = c.id;
        $scope.currentlist = c;
        $scope.currentIndex = index;

        $scope.modalTitle = addtype === "team" ? "Add teams" : "Add owners";
        $scope.selectedItem = c;
        $scope.selectedIndex = index;

        // Populate selected IDs
        $scope.selectedOwnerIds = [];
        if (addtype === "owner" && c.owners) {
            $scope.selectedOwnerIds = c.owners.map(o => o.id);
        } else if (addtype === "team" && c.teams) {
            $scope.selectedOwnerIds = c.teams.map(t => t.id);
        }
    };

    //#endregion
    //#region update owner
    $scope.updateOwnerToServer = function (utype, obj, selected_obj, newid) {
        if (!$scope.requireEdit(utype)) return;
        if (!selected_obj || selected_obj.length === 0) {
            alert("No owners selected");
            return;
        }
        var ids = selected_obj.join(",");
        let resultnew;

        if (utype == 'milestone') {
            resultnew = assignprojectService.UpdateMilestone_Owner(obj.id, ids);
        }
        else if (utype == 'task') {
            resultnew = assignprojectService.UpdateTask_Owner(obj.id, ids);
        }
        else if (utype == 'subtask') {
            resultnew = assignprojectService.UpdateSubtask_Owner(obj.id, ids);
        }
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    var addlist = $scope.owners.find(m => m.id == newid);
                    if (addlist) {
                        let newrow = angular.copy($scope.ownersnew);
                        newrow.id = addlist.id;
                        newrow.name = addlist.name;
                        newrow.profileimage = addlist.profileimage;
                        newrow.shortname = addlist.shortname;
                        obj.owners.push(newrow);
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
    //#endregion
    //#region remove owner
    $scope.removeOwnerToServer = function (utype, obj, selected_obj, newid) {
        if (!$scope.requireEdit(utype)) return;
        // alert("No owners selected");
        //return;
        //}
        var ids = selected_obj.join(",");
        let resultnew;

        if (utype == 'milestone') {
            resultnew = assignprojectService.UpdateMilestone_Owner(obj.id, ids);
        }
        else if (utype == 'task') {
            resultnew = assignprojectService.UpdateTask_Owner(obj.id, ids);
        }
        else if (utype == 'subtask') {
            resultnew = assignprojectService.UpdateSubtask_Owner(obj.id, ids);
        }
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    var indx = obj.owners.findIndex(m => m.id == newid);
                    if (indx !== -1) {
                        obj.owners.splice(indx, 1);
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
    //#endregion
    //#region update team

    // Keep the already-loaded Angular hierarchy in sync with the server-side
    // Team cascade. Without this, the database is updated but parent/child team
    // popups continue showing the old arrays until the complete page is reloaded.
    function addTeamMemberIfMissing(entity, teamMember) {
        if (!entity || !teamMember) return;

        entity.teams = entity.teams || [];

        var exists = entity.teams.some(function (member) {
            return String(member.id) === String(teamMember.id);
        });

        if (!exists) {
            entity.teams.push(angular.copy(teamMember));
        }

        var teamIds = String(entity.team || '')
            .split(',')
            .map(function (value) { return value.trim(); })
            .filter(function (value, index, list) {
                return value !== '' && list.indexOf(value) === index;
            });

        if (teamIds.indexOf(String(teamMember.id)) === -1) {
            teamIds.push(String(teamMember.id));
        }

        entity.team = teamIds.join(',');
    }

    function syncAddedTeamInHierarchy(utype, selectedEntity, teamMember) {
        if (!$scope.projectlist || !selectedEntity || !teamMember) return;

        var project = $scope.projectlist;
        var milestones = project.miles || [];
        var milestone = null;
        var task = null;

        if (utype === 'milestone') {
            milestone = selectedEntity;
        } else {
            milestone = milestones.find(function (item) {
                return String(item.id) === String(selectedEntity.Milestoneid);
            });
        }

        if (utype === 'task') {
            task = selectedEntity;
        } else if (utype === 'subtask' && milestone) {
            task = (milestone.tasks || []).find(function (item) {
                return String(item.id) === String(selectedEntity.Task);
            });
        }

        // Every add moves upward to the Project.
        addTeamMemberIfMissing(project, teamMember);

        if (utype === 'milestone') {
            // Milestone + all Tasks + all Subtasks.
            addTeamMemberIfMissing(milestone, teamMember);
            angular.forEach((milestone && milestone.tasks) || [], function (taskItem) {
                addTeamMemberIfMissing(taskItem, teamMember);
                angular.forEach(taskItem.subtasks || [], function (subtaskItem) {
                    addTeamMemberIfMissing(subtaskItem, teamMember);
                });
            });
        } else if (utype === 'task') {
            // Parent Milestone + selected Task + its Subtasks.
            addTeamMemberIfMissing(milestone, teamMember);
            addTeamMemberIfMissing(task, teamMember);
            angular.forEach((task && task.subtasks) || [], function (subtaskItem) {
                addTeamMemberIfMissing(subtaskItem, teamMember);
            });
        } else if (utype === 'subtask') {
            // Parent Milestone + parent Task + selected Subtask only.
            // Sibling Subtasks are intentionally not changed.
            addTeamMemberIfMissing(milestone, teamMember);
            addTeamMemberIfMissing(task, teamMember);
            addTeamMemberIfMissing(selectedEntity, teamMember);
        }
    }

    function removeTeamMemberIfExists(entity, employeeId) {
        if (!entity) return;

        entity.teams = entity.teams || [];
        entity.teams = entity.teams.filter(function (member) {
            return String(member.id) !== String(employeeId);
        });

        entity.team = String(entity.team || '')
            .split(',')
            .map(function (value) { return value.trim(); })
            .filter(function (value, index, list) {
                return value !== ''
                    && String(value) !== String(employeeId)
                    && list.indexOf(value) === index;
            })
            .join(',');
    }

    function syncRemovedTeamDownward(utype, selectedEntity, employeeId) {
        if (!$scope.projectlist || !selectedEntity) return;

        var project = $scope.projectlist;
        var milestones = project.miles || [];
        var milestone = null;
        var task = null;

        if (utype === 'project') {
            removeTeamMemberIfExists(project, employeeId);
            angular.forEach(milestones, function (milestoneItem) {
                removeTeamMemberIfExists(milestoneItem, employeeId);
                angular.forEach(milestoneItem.tasks || [], function (taskItem) {
                    removeTeamMemberIfExists(taskItem, employeeId);
                    angular.forEach(taskItem.subtasks || [], function (subtaskItem) {
                        removeTeamMemberIfExists(subtaskItem, employeeId);
                    });
                });
            });
            return;
        }

        if (utype === 'milestone') {
            milestone = selectedEntity;
        } else {
            milestone = milestones.find(function (item) {
                return String(item.id) === String(selectedEntity.Milestoneid);
            });
        }

        if (utype === 'task') {
            task = selectedEntity;
        } else if (utype === 'subtask' && milestone) {
            task = (milestone.tasks || []).find(function (item) {
                return String(item.id) === String(selectedEntity.Task);
            });
        }

        if (utype === 'milestone') {
            removeTeamMemberIfExists(milestone, employeeId);
            angular.forEach((milestone && milestone.tasks) || [], function (taskItem) {
                removeTeamMemberIfExists(taskItem, employeeId);
                angular.forEach(taskItem.subtasks || [], function (subtaskItem) {
                    removeTeamMemberIfExists(subtaskItem, employeeId);
                });
            });
        } else if (utype === 'task') {
            removeTeamMemberIfExists(task, employeeId);
            angular.forEach((task && task.subtasks) || [], function (subtaskItem) {
                removeTeamMemberIfExists(subtaskItem, employeeId);
            });
        } else if (utype === 'subtask') {
            removeTeamMemberIfExists(selectedEntity, employeeId);
        }
    }

    $scope.updateTeamToServer = function (utype, obj, selected_obj, newid) {
        if (!$scope.requireEdit(utype)) return;
        if (!selected_obj || selected_obj.length === 0) {
            alert("No owners selected");
            return;
        }
        var ids = selected_obj.join(",");
        let resultnew;

        if (utype == 'milestone') {
            resultnew = assignprojectService.UpdateMilestone_Team(obj.id, ids);
        }
        else if (utype == 'task') {
            resultnew = assignprojectService.UpdateTask_Team(obj.id, ids);
        }
        else if (utype == 'subtask') {
            resultnew = assignprojectService.UpdateSubtask_Team(obj.id, ids);
        }
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    var addlist = $scope.owners.find(m => m.id == newid);
                    if (addlist) {
                        let newrow = angular.copy($scope.ownersnew);
                        newrow.id = addlist.id;
                        newrow.name = addlist.name;
                        newrow.profileimage = addlist.profileimage;
                        newrow.shortname = addlist.shortname;
                        syncAddedTeamInHierarchy(utype, obj, newrow);
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
    //#endregion
    //#region remove team
    $scope.removeTeamToServer = function (utype, obj, selected_obj, newid) {
        if (!$scope.requireEdit(utype)) return;
        // alert("No owners selected");
        // return;
        //}
        var ids = selected_obj.join(",");
        let resultnew;

        if (utype == 'project') {
            resultnew = $http.post("/pms/assignprojects/default.aspx/UpdateProject_Team", { id: String(obj.id), empids: ids });
        }
        else if (utype == 'milestone') {
            resultnew = assignprojectService.UpdateMilestone_Team(obj.id, ids);
        }
        else if (utype == 'task') {
            resultnew = assignprojectService.UpdateTask_Team(obj.id, ids);
        }
        else if (utype == 'subtask') {
            resultnew = assignprojectService.UpdateSubtask_Team(obj.id, ids);
        }
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    syncRemovedTeamDownward(utype, obj, newid);
                    if ($scope.currentlist === obj && $scope.currentUserType === "team") {
                        $scope.selectedOwnerIds = $scope.selectedOwnerIds.filter(function (id) {
                            return String(id) !== String(newid);
                        });
                    }
                } else {
                    showTeamRemovalFailure(res.data.d);
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    alert("Update failed");
                });
        }
    };
    //#endregion
    //#region manage date format
    function convertDotNetDate(dateString) {
        var parts = getDateOnlyParts(dateString);
        if (!isValidDateOnlyParts(parts)) return null;
        return String(parts.day).padStart(2, '0') + '-' +
            String(parts.month).padStart(2, '0') + '-' +
            parts.year;
    }
    //#endregion

    $scope.closepopupmainmulti = function closeproject() {
        window.parent.document.getElementById('btnclosemainpopupmulti').click();
    }
    $scope.getRemainingOwners = function (owners) {
        if (!owners || owners.length <= 3) return "";

        return owners.slice(3).map(function (o) {
            return o.name;
        }).join(", ");
    };


    //comman function
    function getHashParams() {

        //if (hash.startsWith("#")) {
        // hash = hash.substring(1);
        //}
        //if (hash.startsWith("#")) {
        //  hash = hash.substring(1);
        // }

        //return new URLSearchParams(hash);
        let params = new URLSearchParams(window.location.search);
        return new URLSearchParams(params);
    }


    // Resolve the project ID for every end-date calculation.
    // Existing rows normally carry ProjectId, while copied/new/reset rows may
    // temporarily rely on the loaded project or the page query-string ID.
    function resolveHierarchyProjectId(item) {
        var candidates = [
            item && item.ProjectId,
            item && item.projectId,
            item && item.projectid,
            $scope.projectlist && $scope.projectlist.id,
            $scope.projectlist && $scope.projectlist.ProjectId,
            $scope.projectlist && $scope.projectlist.projectId,
            $scope.projectlist && $scope.projectlist.projectid
        ];

        for (var i = 0; i < candidates.length; i++) {
            var value = candidates[i];
            if (value !== undefined && value !== null &&
                String(value).trim() !== '' && String(value).trim() !== '0') {
                return String(value).trim();
            }
        }

        try {
            var queryId = new URLSearchParams(window.location.search).get('id');
            if (queryId && String(queryId).trim() !== '0') {
                return String(queryId).trim();
            }
        } catch (ignore) { }

        return '0';
    }

    function setCustomHash(value) {
        window.location.hash = "##" + value;
    }


    $scope.showlist = true;

    let params = getHashParams();
    if (params.get("id")) {
        let id = params.get("id");

        if (id) {
            $timeout(function () {
                $scope.loadprojectbyid(id);
                $scope.showlist = false;
                $scope.showAddForm = true;
            }, 0);
        }
    }
    else {
        $scope.view = params.get("viewtype") || "table";
        $scope.setView = function (mode) {
            $scope.view = mode;
            setCustomHash("viewtype=" + mode);
        };
    }



    $timeout(function () {
        $scope.loadstatuslist();

    }, 0);
    $timeout(function () {
        $scope.loadTaskSituationList();

    }, 0);
    $timeout(function () {
        $scope.loadprioritylist();

    }, 0);
    $timeout(function () {
        $scope.loaduserlist();

    }, 0);




    $scope.loadprojectbyid = function (id) {

        $scope.isLoading = true;

        assignprojectService.getprojectlistbyid(id).then(function (response) {
            debugger
            $scope.projectlist = response.data.d;
            $scope.refreshProjectDurationDays();
            $scope.refreshTopFilterCounts();
            $scope.isload = "1";
            angular.forEach($scope.projectlist.miles, function (c) {
                c.isExpanded = false;
                c.StartDate = parseDate(c.StartDate);
                c.EndDate = parseDate(c.EndDate);
                //if (c.StartDate) {
                //    if (typeof c.StartDate === 'string' && c.StartDate.indexOf('/Date(') === 0) {
                //        var timestamp = parseInt(c.StartDate.substr(6));
                //        c.StartDate = new Date(timestamp);
                //    } else {
                //        c.StartDate = normalizeHierarchyDateOnly(c.StartDate);
                //    }
                //}
                //if (c.EndDate) {
                //    if (typeof c.EndDate === 'string' && c.EndDate.indexOf('/Date(') === 0) {
                //        var timestamp = parseInt(c.EndDate.substr(6));
                //        c.EndDate = new Date(timestamp);
                //    } else {
                //        c.EndDate = normalizeHierarchyDateOnly(c.EndDate);
                //    }
                //}
                try {
                    //c.TaskDuration = $scope.formatDuration(c.TaskDuration);
                    c.TaskDuration = c.TaskDuration;
                }
                catch{ }

                // TASKS
                if (c.tasks && c.tasks.length > 0) {

                    angular.forEach(c.tasks, function (t) {

                        t.isExpanded = false;
                        t.Startdate = parseDate(t.Startdate);
                        t.Targetdate = parseDate(t.Targetdate);
                        //if (t.Startdate) {
                        //    if (typeof t.Startdate === 'string' && t.Startdate.indexOf('/Date(') === 0) {
                        //        var timestamp = parseInt(t.Startdate.substr(6));
                        //        t.Startdate = new Date(timestamp);
                        //    } else {
                        //        t.Startdate = normalizeHierarchyDateOnly(t.Startdate);
                        //    }
                        //}
                        //if (t.Targetdate) {
                        //    if (typeof t.Targetdate === 'string' && t.Targetdate.indexOf('/Date(') === 0) {
                        //        var timestamp = parseInt(t.Targetdate.substr(6));
                        //        t.Targetdate = new Date(timestamp);
                        //    } else {
                        //        t.Targetdate = normalizeHierarchyDateOnly(t.Targetdate);
                        //    }
                        //}
                        try {
                            //t.TaskDuration = $scope.formatDuration(t.TaskDuration);
                            t.TaskDuration = t.TaskDuration;
                        }
                        catch{ }
                        // SUBTASKS (c2 FIX)
                        if (t.subtasks && t.subtasks.length > 0) {

                            angular.forEach(t.subtasks, function (s) {
                                s.Startdate = parseDate(s.Startdate);
                                s.Targetdate = parseDate(s.Targetdate);

                                //if (s.Startdate) {
                                //    if (typeof s.Startdate === 'string' && s.Startdate.indexOf('/Date(') === 0) {
                                //        var timestamp = parseInt(s.Startdate.substr(6));
                                //        s.Startdate = new Date(timestamp);
                                //    } else {
                                //        s.Startdate = normalizeHierarchyDateOnly(s.Startdate);
                                //    }
                                //}
                                //if (s.Targetdate) {
                                //    if (typeof s.Targetdate === 'string' && s.Targetdate.indexOf('/Date(') === 0) {
                                //        var timestamp = parseInt(s.Targetdate.substr(6));
                                //        s.Targetdate = new Date(timestamp);
                                //    } else {
                                //        s.Targetdate = normalizeHierarchyDateOnly(s.Targetdate);
                                //    }
                                //}
                                try {
                                    //s.TaskDuration = $scope.formatDuration(s.TaskDuration);
                                    s.TaskDuration = s.TaskDuration;
                                }
                                catch{ }
                            });

                        }

                    });

                }

            });
            //alert($scope.projectlist.ProjectName);
            $timeout(function () {

                initTableUI();
                //getChildParents();

            }, 500);
$scope.isLoading = false;
        }).catch(function (error) {
            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);

        }).finally(function () {


           // $scope.isLoading = false;


        });
        $timeout(function () {
            //$scope.buildGantt();
            //alert($scope.ganttScale);
        }, 0);
    };

    $scope.loadstatuslist = function () {


        assignprojectService.getstatuslist()
            .then(function (response) {

                $scope.statuslists = response.data.d;
                $scope.refreshTopFilterCounts();

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadTaskSituationList = function () {
        assignprojectService.gettasksituationlist()
            .then(function (response) {
                $scope.taskSituationLists = response.data.d || [];
            })
            .catch(function (error) {
                console.error("Unable to load Task Situation list:", error);
            });
    };
    $scope.selectStatus_gantt = function (c, item, utype) {
        if (utype == "milestone") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
            if (mindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex];
                $scope.selectStatus(c1, item, utype);
            }
        }
        else if (utype == "task") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            if (mindex !== -1) {
                let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
                $scope.selectStatus(c1, item, utype);
            }
        }
        else if (utype == "subtask") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            if (mindex !== -1) {
                let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
                if (tindex !== -1) {
                    let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);
                    let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                    $scope.selectStatus(c1, item, utype);
                }
            }
        }
    }
    $scope.selectStatus = function (c, item, utype) {
        if (!c || !c.id || !item || item.id === undefined || item.id === null || item.id === '') {
            alert('Please select a valid status.');
            return null;
        }
        if (!$scope.requireEdit(utype, c)) return null;
        if (String(c.P_Status !== undefined ? c.P_Status : (c.statusid !== undefined ? c.statusid : c.StatusId)) === String(item.id)) {
            return null;
        }
        if (c._statusUpdating) return null;

        var mid = "0";
        var resultnew;
        if (utype == 'milestone') {
            mid = c.id;
            resultnew = assignprojectService.UpdateMilestone_Status(c.id, item.id, item.iscomplete);
        }
        else if (utype == 'task') {
            mid = c.Milestoneid;
            resultnew = assignprojectService.UpdateTask_Status(c.id, item.id, item.iscomplete);
        }
        else if (utype == 'subtask') {
            mid = c.Milestoneid;
            resultnew = assignprojectService.UpdateSubTask_Status(c.id, item.id, item.iscomplete);
        }

        if (!resultnew) {
            alert('Unable to update status for this item.');
            return null;
        }

        c._statusUpdating = true;
        return resultnew.then(function (res) {
            if (!res || !res.data || res.data.d != "1") {
                var errorMessage = (res && res.data && res.data.d) || 'Status update failed.';
                var dependencyMatch = /^GANTT_DEPENDENCY_BLOCKED\|([^|]+)\|(\d+)\|([^|]+)(?:\|([^|]*)\|([^|]*))?$/.exec(String(errorMessage));
                if (dependencyMatch) {
                var predecessorType = dependencyMatch[1].toLowerCase();
                var predecessorId = dependencyMatch[2];
                var predecessor = ($scope.originalRows || []).find(function (row) {
                    return String(row.type || '').toLowerCase() === predecessorType && String(row.id) === predecessorId;
                });
                var hierarchyPredecessor = null;
                var miles = (($scope.projectlist || {}).miles || []);
                angular.forEach(miles, function (mile, mileIndex) {
                    var mileNo = String(mile.orderno || (mileIndex + 1));
                    var mileName = mile.Milestone || mile.milestone || '';
                    if (predecessorType === 'milestone' && String(mile.id) === predecessorId) {
                        hierarchyPredecessor = { serialNo: mileNo, name: mileName };
                    }
                    angular.forEach(mile.tasks || mile.Tasks || [], function (task, taskIndex) {
                        var taskNo = String(task.orderno || (taskIndex + 1));
                        var taskName = task.taskname || task.Taskname || task.TaskName || '';
                        if (predecessorType === 'task' && String(task.id) === predecessorId) {
                            hierarchyPredecessor = { serialNo: mileNo + '.' + taskNo, name: taskName };
                        }
                        angular.forEach(task.subtasks || task.Subtasks || task.subtasklist || [], function (subtask, subtaskIndex) {
                            if (predecessorType === 'subtask' && String(subtask.id) === predecessorId) {
                                hierarchyPredecessor = {
                                    serialNo: mileNo + '.' + taskNo + '.' + String(subtask.orderno || (subtaskIndex + 1)),
                                    name: subtask.Subtaskname || subtask.subtaskname || subtask.SubTaskName || ''
                                };
                            }
                        });
                    });
                });
                function decodeDependencyLabel(value) {
                    if (!value) return '';
                    try { return decodeURIComponent(value.replace(/\+/g, '%20')); }
                    catch (ignore) { return value; }
                }
                var serverSerial = decodeDependencyLabel(dependencyMatch[4]);
                var serverName = decodeDependencyLabel(dependencyMatch[5]);
                var predecessorLabel = [
                    (predecessor && predecessor.serialNo) || (hierarchyPredecessor && hierarchyPredecessor.serialNo) || serverSerial,
                    (predecessor && predecessor.name) || (hierarchyPredecessor && hierarchyPredecessor.name) || serverName
                ].filter(Boolean).join(' ');
                    if (!predecessorLabel) predecessorLabel = predecessorType + ' #' + predecessorId;
                    errorMessage = 'Cannot complete this item yet. Complete predecessor ' + predecessorLabel +
                        ' first (' + dependencyMatch[3] + ').';
                    if (window.Swal && Swal.fire) {
                        Swal.fire({
                            title: 'Dependency required',
                            text: errorMessage,
                            icon: 'warning',
                            position: 'top',
                            confirmButtonText: 'OK',
                            allowOutsideClick: false,
                            allowEscapeKey: true
                        });
                    } else {
                        alert(errorMessage);
                    }
                    return;
                }
                alert(errorMessage);
                return;
            }

            c.status_color = item.colorname;
            c.status_name = item.name;
            c.P_Status = item.id;
            c.statusid = item.id;
            c.StatusId = item.id;
            c.iscomplete = item.iscomplete;
            c.IsComplete = item.iscomplete;

            if ($scope.view == "gantt") {
                if (utype == 'milestone') {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.id);
                    if (mindex !== -1) {
                        $scope.projectlist.miles[mindex].status_color = item.colorname;
                        $scope.projectlist.miles[mindex].status_name = item.name;
                    }
                }
                else if (utype == 'task') {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    if (mindex !== -1) {
                        var tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.id);
                        if (tindex !== -1) {
                            $scope.projectlist.miles[mindex].tasks[tindex].status_color = item.colorname;
                            $scope.projectlist.miles[mindex].tasks[tindex].status_name = item.name;
                        }
                    }
                }
                else if (utype == 'subtask') {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    if (mindex !== -1) {
                        var tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.Task);
                        if (tindex !== -1) {
                            let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == c.id);
                            if (sindex !== -1) {
                                $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].status_color = item.colorname;
                                $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].status_name = item.name;
                            }
                        }
                    }
                }
            }

            if (mid != "0") {
                $scope.loadmilecompletiondetails(mid);
            }

            if ($scope.syncListAndKanbanViews) {
                $scope.syncListAndKanbanViews();
            } else {
                $scope.$applyAsync();
            }
        }).catch(function () {
            alert("Update failed");
        }).finally(function () {
            c._statusUpdating = false;
        });
    };


    $scope.selectPriority = function (c, item, utype) {
        if (!c || !c.id || !item || item.id === undefined || item.id === null || item.id === '') {
            alert('Please select a valid priority.');
            return null;
        }
        if (!$scope.requireEdit(utype, c)) return null;
        if (String(c.Priority !== undefined ? c.Priority : (c.priorityid !== undefined ? c.priorityid : c.PriorityId)) === String(item.id)) {
            return null;
        }
        if (c._priorityUpdating) return null;

        var oldPriority = {
            id: c.Priority,
            priorityid: c.priorityid,
            priorityId: c.PriorityId,
            name: c.priority_name,
            color: c.priority_color
        };
        var resultnew;

        // Keep the existing instant UI update, but roll it back when the server rejects/fails.
        c.priority_color = item.colorname;
        c.priority_name = item.name;
        c.Priority = item.id;
        c.priorityid = item.id;
        c.PriorityId = item.id;
        c._priorityUpdating = true;

        if (utype == 'milestone')
            resultnew = assignprojectService.MileUpdatePriority_Status(c.id, item.id);
        else if (utype == 'task')
            resultnew = assignprojectService.TaskUpdatePriority_Status(c.id, item.id);
        else if (utype == 'subtask')
            resultnew = assignprojectService.SubtaskUpdatePriority_Status(c.id, item.id);

        function rollbackPriority() {
            c.Priority = oldPriority.id;
            c.priorityid = oldPriority.priorityid;
            c.PriorityId = oldPriority.priorityId;
            c.priority_name = oldPriority.name;
            c.priority_color = oldPriority.color;
        }

        if (!resultnew) {
            rollbackPriority();
            c._priorityUpdating = false;
            alert('Unable to update priority for this item.');
            return null;
        }

        return resultnew.then(function (res) {
            var serverValue = res && res.data ? res.data.d : '';
            if (!serverValue || String(serverValue) === 'You do not have edit permission.') {
                rollbackPriority();
                alert(serverValue || 'Priority update failed.');
                return;
            }

            // Use the authoritative name/color returned by the existing WebMethod when available.
            var parts = String(serverValue).split('~~');
            if (parts.length > 0 && parts[0]) c.priority_name = parts[0];
            if (parts.length > 1 && parts[1]) c.priority_color = parts[1];

            if ($scope.syncListAndKanbanViews) {
                $scope.syncListAndKanbanViews();
            }
        }).catch(function (err) {
            rollbackPriority();
            console.error("Update failed", err);
            alert('Priority update failed.');
        }).finally(function () {
            c._priorityUpdating = false;
        });
    };

    $scope.loadprioritylist = function () {


        assignprojectService.getprioritylist()
            .then(function (response) {

                $scope.prioritylists = response.data.d;

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.getSelectedOwnerId = function (row) {
        // debugger
        //alert(row ? (row.Owner || row.owner || null) : null);
        return row ? (row.Owner || row.owner || null) : null;


    };
    $scope.focusOwnerSearch = function ($event, c) {

        setTimeout(function () {
            // find current dropdown container
            var dropdown = $event.target.closest(".dropdown");
            if (!dropdown) return;
            // find input inside it
            var input = dropdown.parentNode.querySelector(".owner-search");

            if (input) {
                input.focus();
                input.value = input.value; // helps cursor position fix in some browsers
            }

        }, 200); // important delay for bootstrap render
    };
    $scope.getOwnerName = function (ownerid) {
        if (angular.isArray(ownerid)) {
            ownerid = ownerid.length ? ownerid[0].id : '';
        }
        else if (angular.isObject(ownerid)) {
            return ownerid.name || '';
        }
        ownerid = String(ownerid || '').split(',')[0];
        var owner = ($scope.owners || []).find(function (o) {
            return String(o.id) == ownerid;
        });
        return owner ? owner.name : '';
    };

    $scope.getOwnerProfilenew = function (ownerid) {
        var defaultImage = '/pms/Image/user.jpg';
        try {
            if (angular.isArray(ownerid)) {
                ownerid = ownerid.length ? ownerid[0].id : '';
            }
            else if (angular.isObject(ownerid)) {
                return (ownerid.profileimage && ownerid.profileimage != '') ? ownerid.profileimage : defaultImage;
            }

            ownerid = String(ownerid || '').split(',')[0];
            if (!ownerid || !$scope.owners || !$scope.owners.length) {
                return defaultImage;
            }

            var owner = $scope.owners.find(function (o) {
                return String(o.id) == ownerid;
            });

            return (owner && owner.profileimage && owner.profileimage != '')
                ? owner.profileimage
                : defaultImage;
        }
        catch (e) {
            return defaultImage;
        }
    };
    $scope.selectOwner_gantt = function (c, o, type) {
        if (!$scope.requireEdit(type)) return;
        c.Owner = o.id;
        var data = {
            id: c.id,
            empid: o.id,
            utype: type
        };

        $http({
            method: "POST",
            url: "/pms/assignprojects/default.aspx/updateOwner",
            data: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            }
        }).then(function (res) {
            if (res.data.d == "1") {
                if (type == "milestone") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].Owner = o.id;
                }
                else if (type == "task") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].owner = o.id;
                }
                else if (type == "subtask") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.Task);
                    let stindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[stindex].owner = o.id;
                }
                //console.log("Owner updated successfully");
            } else {
                alert("Update failed");
            }

        }).catch(function (err) {
            console.error(err);
            alert("Server error");
        });
    };
    $scope.selectOwner = function (c, o, type) {
        debugger
        if (!$scope.requireEdit(type)) return;
        if (type == "milestone")
            c.Owner = o.id;
        if (type == "task" || type == "subtask")
            c.owner = o.id;

        var data = {
            id: c.id,
            empid: o.id,
            utype: type
        };

        $http({
            method: "POST",
            url: "/pms/assignprojects/default.aspx/updateOwner",
            data: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            }
        }).then(function (res) {
            if (res.data.d == "1") {
                if (type == "milestone") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].Owner = o.id;
                }
                else if (type == "task") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].owner = o.id;
                }
                else if (type == "subtask") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.Task);
                    let stindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[stindex].owner = o.id;
                }
                //console.log("Owner updated successfully");
            } else {
                alert("Update failed");
            }

        }).catch(function (err) {
            console.error(err);
            alert("Server error");
        });
    };
    $scope.selectedfilterTeams = [];
    $scope.loaduserlist = function () {


        assignprojectService.getuserlist()
            .then(function (response) {

                $scope.owners = response.data.d;
                $scope.selectedfilterTeams = angular.copy($scope.owners);

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loaduserlist_byrefid = function (refid, usertype, searchtype) {
        assignprojectService.getuserlist_byrefid(refid, usertype, searchtype)
            .then(function (response) {
                $scope.owners = response.data.d;
            })
            .catch(function (error) {
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);
            })
            .finally(function () {

            });
    };

    // Commit list names only after editing finishes. Reading the input value
    // also covers the order in which ngModel's blur listener runs.
    $scope.saveListNameOnBlur = function ($event, item, field, type) {
        if (!item || !$event || !$event.target) return;
        var value = $event.target.value;
        item[field] = value;
        if (type === 'milestone') $scope.updatedetails_mile(item, field);
        else if (type === 'task') $scope.updatedetails_task(item, field);
        else if (type === 'subtask') $scope.updatedetails_subtask(item, field);
    };

    // update milestone details


    $scope.updatedetails_mile = function (c, colname) {
        if (!$scope.requireEdit('milestone')) return;

        assignprojectService.updatedetails_mile_bycol(c.id, colname, c[colname])

            .then(function (res) {
                console.log("Status updated");
            })
            .catch(function (err) {
                console.error("Update failed", err);
            });
    };
    $scope.updatedetails_task = function (c, colname) {
        if (!$scope.requireEdit('task')) return;

        assignprojectService.updatedetails_task_bycol(c.id, colname, c[colname])

            .then(function (res) {
                console.log("Status updated");
            })
            .catch(function (err) {
                console.error("Update failed", err);
            });
    };
    $scope.updatedetails_subtask = function (c, colname) {
        if (!$scope.requireEdit('subtask')) return;

        assignprojectService.updatedetails_subtask_bycol(c.id, colname, c[colname])

            .then(function (res) {
                console.log("Status updated");
            })
            .catch(function (err) {
                console.error("Update failed", err);
            });
    };
    // Normal edits may save Duration Days directly when either Duration Days
    // or Start Date is blank. Keep the current dates unchanged; Reset performs
    // the complete blank/capacity/hierarchy validation later.
    function saveHierarchyDurationDaysWithoutValidation(item, type) {
        type = String(type || '').toLowerCase();

        var durationDaysValue = item && item.txtdays !== null && item.txtdays !== undefined
            ? String(item.txtdays).trim()
            : '';
        var request = null;

        if (type === 'milestone') {
            request = assignprojectService.updatedetails_mile_bycol(
                item.id, 'txtdays', durationDaysValue, $scope.perdayhrs
            );
        } else if (type === 'task') {
            request = assignprojectService.updatedetails_task_bycol(
                item.id, 'txtdays', durationDaysValue, $scope.perdayhrs
            );
        } else if (type === 'subtask') {
            request = assignprojectService.updatedetails_subtask_bycol(
                item.id, 'txtdays', durationDaysValue, $scope.perdayhrs
            );
        }

        if (!request || !request.then) {
            return Promise.reject(new Error('Duration days update is not available.'));
        }

        // Keep a reference to the blur-save so Reset Start Date can wait for
        // the latest Duration Days value before validating the hierarchy.
        // This prevents the first Reset click from reading stale data while the
        // input's ng-blur request is still running.
        var trackedPromise = Promise.resolve(request).then(function (response) {
            var result = response && response.data ? response.data.d : null;
            if (String(result) !== '1') {
                throw new Error(
                    result && String(result) !== '0'
                        ? String(result)
                        : 'Duration days update failed.'
                );
            }

            // Preserve exactly what the user entered in both the live value and
            // rollback value. Do not clear it merely because Start Date is blank.
            item.txtdays = durationDaysValue;
            item._oldtxtdays = durationDaysValue;
            return response;
        });

        item._durationDaysSavePromise = trackedPromise;
        trackedPromise.then(function () {
            if (item._durationDaysSavePromise === trackedPromise) {
                item._durationDaysSavePromise = null;
            }
        }, function () {
            if (item._durationDaysSavePromise === trackedPromise) {
                item._durationDaysSavePromise = null;
            }
        });

        return trackedPromise;
    }

    function saveBlankHierarchyDurationDays(item, type) {
        if (item) item.txtdays = '';
        return saveHierarchyDurationDaysWithoutValidation(item, type);
    }

    // A normal Start Date selection must not require Duration Days or
    // Working Hrs. When either value is blank, save only the selected Start Date
    // and preserve the current End Date. Reset Start Date keeps its existing
    // complete blank/capacity/hierarchy validation.
    function hasBlankHierarchyDurationInput(item) {
        if (!item) return true;

        var rawDays = item.txtdays === null || item.txtdays === undefined
            ? ''
            : String(item.txtdays).trim();
        var hourValues = getHierarchyHourTexts(item);
        var workingHoursBlank =
            hourValues.hours === '' && hourValues.minutes === '';

        return rawDays === '' || workingHoursBlank;
    }

    function hasBlankHierarchyStartDate(item, type) {
        if (!item) return true;

        var normalizedType = String(type || '').toLowerCase();
        var startValue = normalizedType === 'milestone'
            ? item.StartDate
            : item.Startdate;

        return !normalizeHierarchyDateOnly(startValue);
    }

    function saveHierarchyStartDateWithoutDuration(item, type, startDate) {
        var normalizedType = String(type || '').toLowerCase();
        var serverStartDate = toServerDateOnly(startDate);
        var request;

        if (!serverStartDate) {
            return Promise.reject(new Error('Selected start date is invalid.'));
        }

        if (normalizedType === 'milestone') {
            request = assignprojectService.updatedetails_mile_bycol(
                item.id,
                'StartDate',
                serverStartDate,
                $scope.perdayhrs
            );
        } else if (normalizedType === 'task') {
            request = assignprojectService.updatedetails_task_bycol(
                item.id,
                'Startdate',
                serverStartDate,
                $scope.perdayhrs
            );
        } else if (normalizedType === 'subtask') {
            request = assignprojectService.updatedetails_subtask_bycol(
                item.id,
                'Startdate',
                serverStartDate,
                $scope.perdayhrs
            );
        } else {
            return Promise.reject(new Error('Invalid hierarchy type.'));
        }

        return request.then(function (response) {
            var result = response && response.data ? response.data.d : null;
            if (String(result) !== '1') {
                throw new Error(
                    result && String(result) !== '0'
                        ? String(result)
                        : 'Start date update failed.'
                );
            }

            if (normalizedType === 'milestone') {
                item.StartDate = cloneHierarchyDateOnly(startDate);
                item._oldStartDate = cloneHierarchyDateOnly(startDate);
            } else {
                item.Startdate = cloneHierarchyDateOnly(startDate);
                item._oldStartdate = cloneHierarchyDateOnly(startDate);
            }

            item._pendingHierarchyStartDate = cloneHierarchyDateOnly(startDate);

            $scope.refreshHierarchyTimelineState(
                item,
                normalizedType,
                startDate,
                getHierarchyEndDateValue(item, normalizedType)
            );
            $scope.$applyAsync();

            return response;
        });
    }

    $scope.update_duration = function (c, utype, updatefrom) {
        utype = utype || 'milestone';

        if (!$scope.requireEdit(utype)) return;
        if (!c) return;

        var itemLabel =
            $scope.getHierarchyItemLabel('milestone', c);

        var projectStart =
            normalizeHierarchyDateOnly(
                $scope.projectlist &&
                $scope.projectlist.StartDate
            );

        var projectEnd =
            normalizeHierarchyDateOnly(
                $scope.projectlist &&
                $scope.projectlist.TargetDate
            );

        var start =
            normalizeHierarchyDateOnly(c.StartDate);

        function restoreOldValue() {
            debugger
            c._isUpdating = true;
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.id);
            if (updatefrom === 'duration') {
                c.txtdays = angular.copy(c._oldtxtdays);
                $scope.projectlist.miles[mindex].txtdays = angular.copy(c._oldtxtdays);
            } else {
                c.StartDate = angular.copy(c._oldStartDate);
                $scope.projectlist.miles[mindex].StartDate = angular.copy(c._oldStartDate);
                c.EndDate = angular.copy(c._oldEndDate);
                $scope.projectlist.miles[mindex].TargetDate = angular.copy(c._oldEndDate);
            }

            c._isUpdating = false;
            $scope.refreshHierarchyTimelineState(
                c,
                'milestone',
                c.StartDate,
                c.EndDate
            );
            $scope.$applyAsync();
        }

        if (updatefrom !== 'duration' && hasBlankHierarchyDurationInput(c)) {
            if (!projectStart || !projectEnd) {
                alert('Project start date or end date is invalid.');
                restoreOldValue();
                return;
            }

            if (!start) {
                alert(itemLabel + ' start date is invalid.');
                restoreOldValue();
                return;
            }

            if (start < projectStart) {
                alert(itemLabel + ' start date cannot be before Project start date.');
                restoreOldValue();
                return;
            }

            if (start > projectEnd) {
                alert(itemLabel + ' start date cannot be after Project end date.');
                restoreOldValue();
                return;
            }

            saveHierarchyStartDateWithoutDuration(c, 'milestone', start)
                .catch(function (error) {
                    console.error('Milestone start-date update failed', error);
                    alert(error && error.message ? error.message : 'Start date update failed.');
                    restoreOldValue();
                });
            return;
        }

        var milestoneDurationDaysBlank =
            c.txtdays === null || c.txtdays === undefined || String(c.txtdays).trim() === '';

        if (milestoneDurationDaysBlank || !start) {
            saveHierarchyDurationDaysWithoutValidation(c, 'milestone')
                .then(function () {
                    // The value has already been saved and retained by
                    // saveHierarchyDurationDaysWithoutValidation(). Keep it
                    // visible when Start Date is blank.
                    $scope.$applyAsync();
                })
                .catch(function (error) {
                    console.error('Milestone duration-days direct update failed', error);
                    alert(error && error.message ? error.message : 'Duration days update failed.');
                    restoreOldValue();
                });
            return;
        }

        function formatPopupDate(value) {
            var dateValue =
                normalizeHierarchyDateOnly(value);

            if (!dateValue) return '';

            var day =
                String(dateValue.getDate())
                    .padStart(2, '0');

            var month =
                String(dateValue.getMonth() + 1)
                    .padStart(2, '0');

            var year =
                dateValue.getFullYear();

            return day + '/' + month + '/' + year;
        }

        function saveMilestone(endDate, serverStartDate) {
            $scope.refreshHierarchyTimelineState(
                c,
                'milestone',
                start,
                endDate
            );

            return assignprojectService
                .updatetimeline_mile(
                    c.id,
                    c.txtdays,
                    serverStartDate
                );
        }

        if (!hasValidHierarchyDurationDays(c)) {
            alertInvalidHierarchyDuration('milestone',c);

            restoreOldValue();
            return;
        }

        var capacityMessage = getHierarchyDurationCapacityError('milestone', c, null);
        if (capacityMessage) {
            alert(capacityMessage);
            restoreOldValue();
            return;
        }

        if (
            !validateHierarchyDurationDayTotals(
                c,
                'milestone',
                null,
                updatefrom,
                restoreOldValue
            )
        ) {
            return;
        }

        if (!projectStart || !projectEnd) {
            alert(
                'Project start date or end date is invalid.'
            );

            restoreOldValue();
            return;
        }

        if (!start) {
            alert(
                itemLabel +
                ' start date is invalid.'
            );

            restoreOldValue();
            return;
        }

        if (start < projectStart) {
            alert(
                itemLabel +
                ' start date cannot be before Project start date.'
            );

            restoreOldValue();
            return;
        }

        if (start > projectEnd && updatefrom === 'duration') {
            alert(
                itemLabel +
                ' start date cannot be after Project end date.'
            );

            restoreOldValue();
            return;
        }

        var serverStartDate =
            toServerDateOnly(start);

        if (!serverStartDate) {
            alert(
                itemLabel +
                ' start date is invalid.'
            );

            restoreOldValue();
            return;
        }

        assignprojectService
            .getnew_enddate(
                serverStartDate,
            c.txtdays,
            resolveHierarchyProjectId(c)
            )
            .then(function (response) {
                var end =
                    hierarchyResponseDate(response);

                if (!end) {
                    throw new Error(
                        'Unable to calculate end date.'
                    );
                }

                if (end < start) {
                    alert(
                        itemLabel +
                        ' end date cannot be before Start date.'
                    );

                    restoreOldValue();
                    return null;
                }

                /*
                 * Cascade later hierarchy only when this calculated end date
                 * exceeds a parent and the user confirms the Extend Timeline
                 * popup. Otherwise update only this Milestone.
                 */
                return saveHierarchyStartDateExtension(
                    c,
                    'milestone',
                    null,
                    null,
                    start,
                    end,
                    updatefrom === 'duration' ? 'duration' : 'startdate'
                ).then(function (extensionResult) {
                    if (!extensionResult || extensionResult.cancelled) {
                        restoreOldValue();
                        return null;
                    }

                    return extensionResult.response;
                });
            })
            .then(function (response) {
                if (!response) return null;

                var savedEnd =hierarchyResponseDate(response);
                if (savedEnd) {
                    $scope.refreshHierarchyTimelineState(
                        c,
                        'milestone',
                        c.StartDate,
                        savedEnd
                    );
                } else {
                    $scope.refreshHierarchyTimelineState(
                        c,
                        'milestone',
                        c.StartDate,
                        c.EndDate
                    );
                }

                c._oldStartDate =
                    angular.copy(c.StartDate);

                c._oldEndDate =
                    angular.copy(c.EndDate);

                c._oldtxtdays =
                    angular.copy(c.txtdays);

                return assignprojectService
                    .getleftdays(
                        c.id,
                        'milestone'
                    );
            })
            .then(function (response) {
                if (
                    response &&
                    response.data &&
                    response.data.d !== undefined &&
                    response.data.d !== null
                ) {
                    c.leftdays =
                        response.data.d;
                }
            })
            .catch(function (error) {
                console.error(
                    'Milestone duration update failed',
                    error
                );

                alert(
                    'Failed to update ' +
                    itemLabel +
                    '.'
                );

                restoreOldValue();
            });
    };
    $scope.update_duration_old = function (c, utype, updatefrom) {
        utype = utype || 'milestone';

        if (!$scope.requireEdit(utype)) return;
        if (!c) return;

        var itemLabel = $scope.getHierarchyItemLabel('milestone', c);
        var projectStart = normalizeHierarchyDateOnly($scope.projectlist && $scope.projectlist.StartDate);
        var projectEnd = normalizeHierarchyDateOnly($scope.projectlist && $scope.projectlist.TargetDate);
        var start = normalizeHierarchyDateOnly(c.StartDate);

        function restoreOldValue() {
            c._isUpdating = true;
            if (updatefrom === 'duration') {
                c.txtdays = angular.copy(c._oldtxtdays);
            } else {
                c.StartDate = angular.copy(c._oldStartDate);
                c.EndDate = angular.copy(c._oldEndDate);
            }
            c._isUpdating = false;
        }

        if (!hasValidHierarchyDurationDays(c)) {
            alertInvalidHierarchyDuration('milestone', c);
            restoreOldValue();
            return;
        }

        var capacityMessage = getHierarchyDurationCapacityError('milestone', c, null);
        if (capacityMessage) {
            alert(capacityMessage);
            restoreOldValue();
            return;
        }

        if (!validateHierarchyDurationDayTotals(
            c,
            'milestone',
            null,
            updatefrom,
            restoreOldValue
        )) {
            return;
        }

        if (!projectStart || !projectEnd) {
            alert('Project start date or end date is invalid.');
            restoreOldValue();
            return;
        }

        if (!start) {
            alert(itemLabel + ' start date is invalid.');
            restoreOldValue();
            return;
        }

        if (start < projectStart) {
            alert(itemLabel + ' start date cannot be before Project start date.');
            restoreOldValue();
            return;
        }

        if (start > projectEnd) {
            alert(itemLabel + ' start date cannot be after Project end date.');
            restoreOldValue();
            return;
        }

        var serverStartDate = toServerDateOnly(start);
        if (!serverStartDate) {
            alert(itemLabel + ' start date is invalid.');
            restoreOldValue();
            return;
        }

        assignprojectService.getnew_enddate(serverStartDate, c.txtdays,resolveHierarchyProjectId(c))
            .then(function (response) {
                var end = hierarchyResponseDate(response);
                if (!end) {
                    throw new Error('Unable to calculate end date.');
                }

                if (end > projectEnd) {
                    alert(itemLabel + ' end date cannot be after Project end date.');
                    restoreOldValue();
                    return null;
                }

                if (end < start) {
                    alert(itemLabel + ' end date cannot be before Start date.');
                    restoreOldValue();
                    return null;
                }

                c.StartDate = cloneHierarchyDateOnly(start);
                c.EndDate = cloneHierarchyDateOnly(end);

                return assignprojectService.updatetimeline_mile(
                    c.id,
                    c.txtdays,
                    serverStartDate
                );
            })
            .then(function (response) {
                if (!response) return null;

                var savedEnd = hierarchyResponseDate(response);
                if (savedEnd) c.EndDate = cloneHierarchyDateOnly(savedEnd);

                c._oldStartDate = angular.copy(c.StartDate);
                c._oldEndDate = angular.copy(c.EndDate);
                c._oldtxtdays = angular.copy(c.txtdays);

                return assignprojectService.getleftdays(c.id, 'milestone');
            })
            .then(function (response) {
                if (
                    response &&
                    response.data &&
                    response.data.d !== undefined &&
                    response.data.d !== null
                ) {
                    c.leftdays = response.data.d;
                }
            })
            .catch(function (error) {
                console.error('Milestone duration update failed', error);
                alert('Failed to update ' + itemLabel + '.');
                restoreOldValue();
            });
    };
    $scope.update_durationold17072026 = function (c, utype, updatefrom) {
        debugger
        if (!$scope.requireEdit(utype || 'milestone')) return;
        let projectStart = normalizeHierarchyDateOnly($scope.projectlist.StartDate);
        let projectEnd = normalizeHierarchyDateOnly($scope.projectlist.TargetDate);

        // Current start date

        let start = normalizeHierarchyDateOnly(c.StartDate);
        // Normalize dates (fix 1-day issue)
        //function normalize(d) {
        //    if (!d) return null;
        //    d = new Date(d);
        //    d.setHours(0, 0, 0, 0);
        //    return d;
        //}
        function normalize(value) {
            var date = parseDotNetDate(value);

            if (!date) return null;

            return new Date(
                date.getFullYear(),
                date.getMonth(),
                date.getDate(),
                0,
                0,
                0,
                0
            );
        }
        projectStart = normalize(projectStart);
        projectEnd = normalize(projectEnd);
        start = normalize(start);
        // Check start date before project start
        if (start && projectStart && start < projectStart) {
            alert($scope.getHierarchyItemLabel("milestone", c) + " start date cannot be before Project start date");
            c._isUpdating = true;

            if (updatefrom == "duration") {
                c.txtdays = c._oldtxtdays || null;
            }
            else {
                c.StartDate = c._oldStartDate || null;
            }
            c._isUpdating = false;
            return;
        }
        if (start && projectStart && start > projectEnd) {
            alert($scope.getHierarchyItemLabel("milestone", c) + " start date cannot be after Project end date");
            c._isUpdating = true;
            if (updatefrom == "duration") {
                c.txtdays = c._oldtxtdays || null;
            }
            else {
                c.StartDate = c._oldStartDate || null;
            }
            c._isUpdating = false;
            return;
        }

        // Get new end date asynchronously
        assignprojectService.getnew_enddate(start, c.txtdays, resolveHierarchyProjectId(c)).then(function (newEndDate) {
            if (!newEndDate) {
                alert("Failed to calculate end date");
                return;
            }

            // Normalize new end date
            let end = normalize(parseDotNetDate(newEndDate.data.d));
            c.EndDate = end;

            // Validation against project end
            if (end && projectEnd && end > projectEnd) {
                alert($scope.getHierarchyItemLabel("milestone", c) + " end date cannot be after Project end date");

                c._isUpdating = true;

                if (updatefrom == "duration") {
                    c.txtdays = c._oldtxtdays || null;
                }
                else {
                    c.EndDate = c._oldEndDate || null;
                }
                c._isUpdating = false;
                return;
            }

            // Validation start < end
            if (start && end && end < start) {
                alert("End date cannot be before Start date");
                //c.EndDate = c._oldEndDate || null;
                //if (updatefrom=="duration")
                //{
                //c.txtdays = c._oldtxtdays || null;
                // }
                c._isUpdating = true;

                if (updatefrom == "duration") {
                    c.txtdays = c._oldtxtdays || null;
                }
                else {
                    c.EndDate = c._oldEndDate || null;
                }
                c._isUpdating = false;
                return;
            }

            c._oldStartDate = angular.copy(c.StartDate);
            c._oldEndDate = angular.copy(c.EndDate);

            // Update timeline
            let resultnew;
            if (utype === 'milestone') {
                resultnew = assignprojectService.updatetimeline_mile(c.id, c.txtdays, toServerDateOnly(start));
            }

            if (!resultnew) return;

            resultnew.then(function (res) {
                let newDate = parseDotNetDate(res.data.d);
                c.EndDate = newDate;
                // get left days
                return assignprojectService.getleftdays(c.id, utype);
            }).then(function (resdays) {
                if (resdays && resdays.data && resdays.data.d) {
                    c.leftdays = resdays.data.d;
                }
            }).catch(function (err) {
                console.error("Update failed", err);
            });

        }).catch(function (err) {
            console.error("Failed to calculate new end date", err);
        });
    };

    $scope.update_duration_task = function (c, m, updatefrom) {
        debugger
        if (!$scope.requireEdit('task')) return;
        if (!c || !m) return;

        var itemLabel = $scope.getHierarchyItemLabel('task', c);
        var parentLabel = $scope.getHierarchyItemLabel('milestone', m);
        var milestoneStart = normalizeHierarchyDateOnly(m.StartDate);
        var milestoneEnd = normalizeHierarchyDateOnly(m.EndDate);
        var start = normalizeHierarchyDateOnly(c.Startdate);

        function restoreOldValue() {
            c._isUpdating = true;
            if (updatefrom === 'duration') {
                c.txtdays = angular.copy(c._oldtxtdays);
            } else {
                c.Startdate = angular.copy(c._oldStartdate);
                c.Targetdate = angular.copy(c._oldTargetdate);
            }
            c._isUpdating = false;
            $scope.refreshHierarchyTimelineState(
                c,
                'task',
                c.Startdate,
                c.Targetdate
            );
        }

        if (updatefrom !== 'duration' && hasBlankHierarchyDurationInput(c)) {
            if (!milestoneStart || !milestoneEnd) {
                alert(parentLabel + ' start date or end date is invalid.');
                restoreOldValue();
                return;
            }

            if (!start) {
                alert(itemLabel + ' start date is invalid.');
                restoreOldValue();
                return;
            }

            if (start < milestoneStart) {
                alert(itemLabel + ' start date cannot be before ' + parentLabel + ' start date.');
                restoreOldValue();
                return;
            }

            if (start > milestoneEnd) {
                alert(itemLabel + ' start date cannot be after ' + parentLabel + ' end date.');
                restoreOldValue();
                return;
            }

            saveHierarchyStartDateWithoutDuration(c, 'task', start)
                .catch(function (error) {
                    console.error('Task start-date update failed', error);
                    alert(error && error.message ? error.message : 'Start date update failed.');
                    restoreOldValue();
                });
            return;
        }

        var taskDurationDaysBlank =
            c.txtdays === null || c.txtdays === undefined || String(c.txtdays).trim() === '';

        if (taskDurationDaysBlank || !start) {
            saveHierarchyDurationDaysWithoutValidation(c, 'task')
                .then(function () {
                    $scope.$applyAsync();
                })
                .catch(function (error) {
                    console.error('Task duration-days direct update failed', error);
                    alert(error && error.message ? error.message : 'Duration days update failed.');
                    restoreOldValue();
                });
            return;
        }

        if (!hasValidHierarchyDurationDays(c)) {
            alertInvalidHierarchyDuration('task', c);
            restoreOldValue();
            return;
        }

        var capacityMessage = getHierarchyDurationCapacityError('task', c, null);
        if (capacityMessage) {
            alert(capacityMessage);
            restoreOldValue();
            return;
        }

        if (!validateHierarchyDurationDayTotals(
            c,
            'task',
            m,
            updatefrom,
            restoreOldValue
        )) {
            return;
        }

        if (!milestoneStart || !milestoneEnd) {
            alert(parentLabel + ' start date or end date is invalid.');
            restoreOldValue();
            return;
        }

        if (!start) {
            alert(itemLabel + ' start date is invalid.');
            restoreOldValue();
            return;
        }

        if (start < milestoneStart) {
            alert(itemLabel + ' start date cannot be before ' + parentLabel + ' start date.');
            restoreOldValue();
            return;
        }

        if (start > milestoneEnd && updatefrom === 'duration') {
            alert(itemLabel + ' start date cannot be after ' + parentLabel + ' end date.');
            restoreOldValue();
            return;
        }

        var serverStartDate = toServerDateOnly(start);
        if (!serverStartDate) {
            alert(itemLabel + ' start date is invalid.');
            restoreOldValue();
            return;
        }

        assignprojectService.getnew_enddate(serverStartDate, c.txtdays,resolveHierarchyProjectId(c))
            .then(function (response) {
                var end = hierarchyResponseDate(response);
                if (!end) throw new Error('Unable to calculate end date.');

                if (end < start) {
                    alert(itemLabel + ' end date cannot be before Start date.');
                    restoreOldValue();
                    return null;
                }

                /*
                 * Cascade later hierarchy only when this calculated end date
                 * exceeds a parent and the user confirms the Extend Timeline
                 * popup. Otherwise update only this Task.
                 */
                return saveHierarchyStartDateExtension(
                    c,
                    'task',
                    m,
                    null,
                    start,
                    end,
                    updatefrom === 'duration' ? 'duration' : 'startdate'
                ).then(function (extensionResult) {
                    if (!extensionResult || extensionResult.cancelled) {
                        restoreOldValue();
                        return null;
                    }

                    return extensionResult.response;
                });
            })
            .then(function (response) {
                if (!response) return null;

                var savedEnd = hierarchyResponseDate(response);
                $scope.refreshHierarchyTimelineState(
                    c,
                    'task',
                    c.Startdate,
                    savedEnd || c.Targetdate
                );

                c._oldStartdate = angular.copy(c.Startdate);
                c._oldTargetdate = angular.copy(c.Targetdate);
                c._oldtxtdays = angular.copy(c.txtdays);

                return assignprojectService.getleftdays(c.id, 'task');
            })
            .then(function (response) {
                if (
                    response &&
                    response.data &&
                    response.data.d !== undefined &&
                    response.data.d !== null
                ) {
                    c.leftdays = response.data.d;
                }
            })
            .catch(function (error) {
                console.error('Task duration update failed', error);
                alert('Failed to update ' + itemLabel + '.');
                restoreOldValue();
            });
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

    // ASPX Angular expressions can access only scope members.
    // Without this assignment {{formatDateOnly(...)}} renders blank.
    $scope.formatDateOnly = formatDateOnly;

    function formatDateDDMMYYYY(value) {
        return formatDateOnly(value).replace(/\//g, '-');
    }
    $scope.update_duration_subtask = function (c, t, updatefrom) {
        if (!$scope.requireEdit('subtask')) return;
        if (!c || !t) return;

        var itemLabel = $scope.getHierarchyItemLabel('subtask', c);
        var parentLabel = $scope.getHierarchyItemLabel('task', t);
        var taskStart = normalizeHierarchyDateOnly(t.Startdate);
        var taskEnd = normalizeHierarchyDateOnly(t.Targetdate);
        var start = normalizeHierarchyDateOnly(c.Startdate);

        function restoreOldValue() {
            c._isUpdating = true;
            if (updatefrom === 'duration') {
                c.txtdays = angular.copy(c._oldtxtdays);
            } else {
                c.Startdate = angular.copy(c._oldStartdate);
                c.Targetdate = angular.copy(c._oldTargetdate);
            }
            c._isUpdating = false;
            $scope.refreshHierarchyTimelineState(
                c,
                'subtask',
                c.Startdate,
                c.Targetdate
            );
        }

        if (updatefrom !== 'duration' && hasBlankHierarchyDurationInput(c)) {
            if (!taskStart || !taskEnd) {
                alert(parentLabel + ' start date or end date is invalid.');
                restoreOldValue();
                return;
            }

            if (!start) {
                alert(itemLabel + ' start date is invalid.');
                restoreOldValue();
                return;
            }

            if (start < taskStart) {
                alert(itemLabel + ' start date cannot be before ' + parentLabel + ' start date.');
                restoreOldValue();
                return;
            }

            if (start > taskEnd) {
                alert(itemLabel + ' start date cannot be after ' + parentLabel + ' end date.');
                restoreOldValue();
                return;
            }

            saveHierarchyStartDateWithoutDuration(c, 'subtask', start)
                .catch(function (error) {
                    console.error('Subtask start-date update failed', error);
                    alert(error && error.message ? error.message : 'Start date update failed.');
                    restoreOldValue();
                });
            return;
        }

        var subtaskDurationDaysBlank =
            c.txtdays === null || c.txtdays === undefined || String(c.txtdays).trim() === '';

        if (subtaskDurationDaysBlank || !start) {
            saveHierarchyDurationDaysWithoutValidation(c, 'subtask')
                .then(function () {
                    $scope.$applyAsync();
                })
                .catch(function (error) {
                    console.error('Subtask duration-days direct update failed', error);
                    alert(error && error.message ? error.message : 'Duration days update failed.');
                    restoreOldValue();
                });
            return;
        }

        if (!hasValidHierarchyDurationDays(c)) {
            alertInvalidHierarchyDuration('subtask', c);
            restoreOldValue();
            return;
        }

        var capacityMessage = getHierarchyDurationCapacityError('subtask', c, null);
        if (capacityMessage) {
            alert(capacityMessage);
            restoreOldValue();
            return;
        }

        if (!validateHierarchyDurationDayTotals(
            c,
            'subtask',
            t,
            updatefrom,
            restoreOldValue
        )) {
            return;
        }

        if (!taskStart || !taskEnd) {
            alert(parentLabel + ' start date or end date is invalid.');
            restoreOldValue();
            return;
        }

        if (!start) {
            alert(itemLabel + ' start date is invalid.');
            restoreOldValue();
            return;
        }

        if (start < taskStart) {
            alert(itemLabel + ' start date cannot be before ' + parentLabel + ' start date.');
            restoreOldValue();
            return;
        }

        if (start > taskEnd && updatefrom === 'duration') {
            alert(itemLabel + ' start date cannot be after ' + parentLabel + ' end date.');
            restoreOldValue();
            return;
        }

        var serverStartDate = toServerDateOnly(start);
        if (!serverStartDate) {
            alert(itemLabel + ' start date is invalid.');
            restoreOldValue();
            return;
        }

        assignprojectService.getnew_enddate(serverStartDate, c.txtdays, resolveHierarchyProjectId(c))
            .then(function (response) {
                var end = hierarchyResponseDate(response);
                if (!end) throw new Error('Unable to calculate end date.');

                if (end < start) {
                    alert(itemLabel + ' end date cannot be before Start date.');
                    restoreOldValue();
                    return null;
                }

                /*
                 * Cascade later hierarchy only when this calculated end date
                 * exceeds a parent and the user confirms the Extend Timeline
                 * popup. Otherwise update only this Subtask.
                 */
                return saveHierarchyStartDateExtension(
                    c,
                    'subtask',
                    t,
                    null,
                    start,
                    end,
                    updatefrom === 'duration' ? 'duration' : 'startdate'
                ).then(function (extensionResult) {
                    if (!extensionResult || extensionResult.cancelled) {
                        restoreOldValue();
                        return null;
                    }

                    return extensionResult.response;
                });
            })
            .then(function (response) {
                if (!response) return null;

                var savedEnd = hierarchyResponseDate(response);
                $scope.refreshHierarchyTimelineState(
                    c,
                    'subtask',
                    c.Startdate,
                    savedEnd || c.Targetdate
                );

                c._oldStartdate = angular.copy(c.Startdate);
                c._oldTargetdate = angular.copy(c.Targetdate);
                c._oldtxtdays = angular.copy(c.txtdays);

                return assignprojectService.getleftdays(c.id, 'subtask');
            })
            .then(function (response) {
                if (
                    response &&
                    response.data &&
                    response.data.d !== undefined &&
                    response.data.d !== null
                ) {
                    c.leftdays = response.data.d;
                }
            })
            .catch(function (error) {
                console.error('Subtask duration update failed', error);
                alert('Failed to update ' + itemLabel + '.');
                restoreOldValue();
            });
    };
    $scope.update_durationold = function (c, utype) {
        if (!$scope.requireEdit(utype)) return;
        let resultnew;

        if (utype == 'milestone') {
            resultnew = assignprojectService.updatetimeline_mile(c.id, c.txtdays, normalizeHierarchyDateOnly(c.StartDate));
        }
        else if (utype == 'task') {
            resultnew = assignprojectService.updatetimeline_task(c.id, c.txtdays, normalizeHierarchyDateOnly(c.Startdate));
        }
        else if (utype == 'subtask') {
            resultnew = assignprojectService.updatetimeline_subtask(c.id, c.txtdays, normalizeHierarchyDateOnly(c.Startdate));
        }

        if (!resultnew) return;

        resultnew.then(function (res) {

            let newDate = parseDotNetDate(res.data.d);

            if (utype == 'milestone') {
                c.EndDate = newDate;
            } else {
                c.Targetdate = newDate;
            }

            return assignprojectService.getleftdays(c.id, utype);

        }).then(function (resdays) {

            if (resdays && resdays.data && resdays.data.d) {
                c.leftdays = resdays.data.d;
            }

        }).catch(function (err) {
            console.error("Update failed", err);
        });
    };

    $scope.parseDuration = function (val1, val2) {

        let hrs = parseInt(val1) || 0;
        let min = parseInt(val2) || 0;

        return hrs + (min / 60);
    };
    $scope.getTotal = function (list) {
        let total = 0;

        (list || []).forEach(item => {
            total += $scope.parseDuration(item.TaskDuration, item.TaskDuration_min);
        });

        return total;
    };
    $scope.validateMilestone = function (milestone) {

        let milestoneHrs = $scope.parseDuration(milestone.TaskDuration, milestone.TaskDuration_min);
        let totalTaskHrs = $scope.getTotal(milestone.tasks);

        return totalTaskHrs <= milestoneHrs;
    };
    $scope.validateTask = function (task) {

        let taskHrs = $scope.parseDuration(task.TaskDuration, task.TaskDuration_min);
        let totalSubtaskHrs = $scope.getTotal(task.subtasks);

        return totalSubtaskHrs <= taskHrs;
    };
    $scope.hrs_validate = function (c, utype) {
        let ret_hrs = 0;
        if (utype == "task") {
            ret_hrs += parseFloat($scope.normalizeTime_calhr_new(c.TaskDuration, c.TaskDuration_min));

        }
        if (utype == "subtask") {
        }
        return ret_hrs;
    }
    $scope.checkLimit = function (item, parent, type) {
        let isValid = true;

        if (type === 'task') {
            isValid = $scope.validateMilestone(parent);
        }

        if (type === 'subtask') {
            isValid = $scope.validateTask(parent);
        }

        if (!isValid) {
            alert("Hours exceeded!");

            // revert value
            item.TaskDuration = item._oldTaskDuration || '';
            item.TaskDuration_min = item._oldTaskDuration_min || '';

        } else {
            item._oldTaskDuration = item.TaskDuration;
            item._oldTaskDuration_min = item.TaskDuration_min;
            $scope.update_hrs(item, type);
        }

    };
    $scope.storeOldDuration = function (c) {
        if (!c) return;
        c._oldTaskDuration = (c.TaskDuration !== undefined && c.TaskDuration !== null) ? c.TaskDuration : '';
        c._oldTaskDuration_min = (c.TaskDuration_min !== undefined && c.TaskDuration_min !== null) ? c.TaskDuration_min : '';
    };

    $scope.update_hrs = function (c, utype) {
        if (!c) return;
        if (!$scope.requireEdit(utype)) return;
        if (c._isRestoringDuration) return;
        utype = String(utype || '').toLowerCase();

        var durationHoursText = c.TaskDuration === null || c.TaskDuration === undefined
            ? ''
            : String(c.TaskDuration).trim();
        var durationMinutesText = c.TaskDuration_min === null || c.TaskDuration_min === undefined
            ? ''
            : String(c.TaskDuration_min).trim();
        var isBlankWorkingHours = durationHoursText === '' && durationMinutesText === '';
        var isBlankStartDate = hasBlankHierarchyStartDate(c, utype);

        //let newval = parseFloat(c.TaskDuration || "0");
        let newval = parseFloat($scope.normalizeTime_calhr_new(c.TaskDuration, c.TaskDuration_min));
        if (isNaN(newval)) newval = 0;

        function restoreOldDuration() {
            c._isRestoringDuration = true;
            c.TaskDuration = (c._oldTaskDuration !== undefined && c._oldTaskDuration !== null) ? c._oldTaskDuration : '';
            c.TaskDuration_min = (c._oldTaskDuration_min !== undefined && c._oldTaskDuration_min !== null) ? c._oldTaskDuration_min : '';

            // keep the restored value visible and stop another blur cycle from showing same alert again
            try {
                if (!$scope.$$phase) {
                    $scope.$applyAsync();
                }
            } catch (exApply) { }

            setTimeout(function () {
                c._isRestoringDuration = false;
            }, 250);
        }

        function showDurationValidationAlert(message) {
            restoreOldDuration();

            if (c._durationAlertShowing) return;
            c._durationAlertShowing = true;

            setTimeout(function () {
                alert(message || 'Invalid duration.');
                try {
                    if (document.activeElement && document.activeElement.blur) {
                        document.activeElement.blur();
                    }
                } catch (exBlur) { }

                setTimeout(function () {
                    c._durationAlertShowing = false;
                }, 300);
            }, 0);
        }

        // Normal edits bypass validation when Working Hrs or Start Date is blank.
        // Reset performs the complete validation later.
        // txtdays=0 is treated as one day when validation is applicable.
        if (!isBlankWorkingHours && !isBlankStartDate) {
            var capacityMessage = getHierarchyDurationCapacityError(utype, c, newval);
            if (capacityMessage) {
                showDurationValidationAlert(capacityMessage);
                return;
            }
        }

        // Same validation as assign resize:
        // subtask total <= parent task duration
        // task total <= parent milestone duration
        // milestone total <= project duration when project duration is available
        if (!isBlankWorkingHours &&
            !isBlankStartDate &&
            $scope.validateResizeDurationByParent &&
            !$scope.validateResizeDurationByParent(c.id, utype, newval, true)) {
            showDurationValidationAlert($scope._lastResizeDurationValidationMessage || 'Invalid duration.');
            return;
        }

        // Capacity and hierarchy totals are both valid; save the duration.
        var duration_val = "";

        if (c.TaskDuration && c.TaskDuration_min) {
            duration_val = c.TaskDuration + ":" + c.TaskDuration_min;
        } else if (c.TaskDuration) {
            duration_val = c.TaskDuration + ":";
        } else if (c.TaskDuration_min) {
            duration_val = ":" + c.TaskDuration_min;
        }

        var req = null;
        if (utype == "milestone") {
            req = assignprojectService.updatedetails_mile_bycol(c.id, 'TaskDuration', duration_val, $scope.perdayhrs);
        }
        else if (utype == "task") {
            req = assignprojectService.updatedetails_task_bycol(c.id, 'TaskDuration', duration_val, $scope.perdayhrs);
        }
        else if (utype == "subtask") {
            req = assignprojectService.updatedetails_subtask_bycol(c.id, 'TaskDuration', duration_val, $scope.perdayhrs);
        }

        if (req && req.then) {
            // Track the Working Hours blur-save for the same reason as Duration
            // Days: Reset must not validate while this request is still pending.
            var workingHoursPromise = Promise.resolve(req).then(function (res) {
                var saveResult = res && res.data ? res.data.d : null;
                if (String(saveResult) !== '1') {
                    var saveMessage = saveResult && String(saveResult) !== '0'
                        ? String(saveResult)
                        : 'Duration update failed.';
                    showDurationValidationAlert(saveMessage);
                    throw new Error(saveMessage);
                }

                c._oldTaskDuration = c.TaskDuration;
                c._oldTaskDuration_min = c.TaskDuration_min;
                console.log("Status updated");
                return res;
            }).catch(function (err) {
                console.error("Update failed", err);
                restoreOldDuration();
                throw err;
            });

            c._workingHoursSavePromise = workingHoursPromise;
            workingHoursPromise.then(function () {
                if (c._workingHoursSavePromise === workingHoursPromise) {
                    c._workingHoursSavePromise = null;
                }
            }, function () {
                if (c._workingHoursSavePromise === workingHoursPromise) {
                    c._workingHoursSavePromise = null;
                }
            });

            return workingHoursPromise;
        }

        return null;
    };

    $scope.update_hrs_old = function (c, utype) {
        if (!$scope.requireEdit(utype)) return;
        let newval = parseFloat($scope.normalizeTime_calhr(c.TaskDuration));

        // No per-day maximum is applied while entering working hours.
        // This legacy path follows the same hierarchy-only validation rule.
        if ($scope.validateResizeDurationByParent &&
            !$scope.validateResizeDurationByParent(c.id, utype, newval, true)) {
            alert($scope._lastResizeDurationValidationMessage || 'Invalid duration.');
            c.TaskDuration = c._oldTaskDuration || '';
            return;
        }

        if (utype == "milestone")
            assignprojectService.updatedetails_mile_bycol(c.id, 'TaskDuration', c.TaskDuration, $scope.perdayhrs)
        else if (utype == "task")
            assignprojectService.updatedetails_task_bycol(c.id, 'TaskDuration', c.TaskDuration, $scope.perdayhrs)
        else if (utype == "subtask")
            assignprojectService.updatedetails_subtask_bycol(c.id, 'TaskDuration', c.TaskDuration, $scope.perdayhrs)

                .then(function (res) {
                    console.log("Status updated");
                })
                .catch(function (err) {
                    console.error("Update failed", err);
                });

    };
    // end update details milestone



    //#region manage files upload

    $scope.modalTitle = ""; // Default

    $scope.loadSrcFiles = function (c, strtype, index) {
        // File list can be opened for viewing, but upload/delete is controlled by canEdit(currentFileType).
        if (strtype == "milestone") {
            $scope.currentFMilestoneId = c.id;
        }
        else if (strtype == "task" || strtype == "subtask") {
            $scope.currentFMilestoneId = c.Milestoneid;
            if (strtype == "subtask") {
                $scope.currentFTaskId = c.Task;
            }
        }

        $scope.showfileslist = null;
        $scope.currentFileType = "";
        $scope.currentFProjectId = "";
        $scope.currentFId = "";

        $scope.currentlist = c;
        $scope.currentIndex = index;
        $scope.currentFileType = strtype;
        $scope.currentFProjectId = c.ProjectId;
        $scope.currentFId = c.id;

        //$scope.showfileslist=angular.copy($scope.projectlist.miles[index].viewfiles);
        $scope.showfileslist = angular.copy(c.viewfiles);

        // Set modal title based on type
        switch (strtype) {
            case "milestone":
                $scope.modalTitle = "Upload milestone documents";
                break;
            case "task":
                $scope.modalTitle = "Upload task documents";
                break;
            case "subtask":
                $scope.modalTitle = "Upload sub task documents";
                break;
            default:
                $scope.modalTitle = "Upload Files";
        }

        // Update label in the modal
        var lbl = document.getElementById('ContentPlaceHolder1_lblfiles');
        if (lbl) lbl.innerHTML = $scope.modalTitle;


        // $scope.bindTaskData(strid, strtype);
    };


    $scope.isDragging = false;
    $scope.uploadedFiles = [];
    //$scope.showfileslist = [];

    const dropZone = document.getElementById("dropZone");
    const fileInput = document.getElementById("fileupload");

    // Click drop zone → open file picker
    $scope.openFilePicker = function () {
        if (!$scope.requireEdit($scope.currentFileType)) return;

        fileInput.click();
    };

    // Files selected via input
    $scope.filesSelected = function (files) {
        if (!$scope.requireEdit($scope.currentFileType)) return;
        $scope.processFiles(files, $scope.currentFProjectId, $scope.currentFId, $scope.currentFileType, $scope.currentlist);
    };

    // Drag over
    dropZone.addEventListener("dragover", function (e) {
        e.preventDefault();
        dropZone.style.borderColor = "#007bff";
        dropZone.style.backgroundColor = "#f0f8ff";
        $scope.$apply(() => { $scope.isDragging = true; });
    });

    // Drag leave
    dropZone.addEventListener("dragleave", function (e) {
        e.preventDefault();
        dropZone.style.borderColor = "#ccc";
        dropZone.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
    });

    // Drop files
    dropZone.addEventListener("drop", function (e) {
        e.preventDefault();
        dropZone.style.borderColor = "#ccc";
        dropZone.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
        $scope.processFiles(
            e.dataTransfer.files,
            $scope.currentFProjectId,
            $scope.currentFId,
            $scope.currentFileType,
            $scope.currentlist
        );
    });

    $scope.processFiles = function (files, fpid, fid, ftype, obj) {
        if (!$scope.requireEdit(ftype)) return;
        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            let reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(() => {
                    $scope.uploadedFiles.push({
                        name: file.name,
                        type: file.type,
                        data: evt.target.result
                    });
                });
            };
            reader.readAsDataURL(file);
        }

        // Optional: auto-upload
        $scope.uploadFilesToServer(files, fpid, fid, ftype, obj);
    };

    $scope.uploadFilesToServer = function (files, pid, id, type, obj) {
        if (!$scope.requireEdit(type)) return;
        let formData = new FormData();
        for (let i = 0; i < files.length; i++) {
            formData.append("files", files[i]);
        }
        formData.append("pid", pid);
        formData.append("ftype", type);
        formData.append("id", id);
        $http.post("/pms/SingleFileUploadHandlerANG.ashx", formData, {
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).then(function (res) {
            try {
                res.data.forEach(file => {
                    //$scope.projectlist.miles[$scope.currentIndex].viewfiles.push(file);
                    obj.viewfiles.push(file);
                });
            }
            catch
            { }
            //$scope.showfileslist = angular.copy($scope.projectlist.miles[$scope.currentIndex].viewfiles);
            $scope.showfileslist = angular.copy($scope.currentlist.viewfiles);
            console.log("Upload success", res.data);
        }, function (err) {

            console.error("Upload failed", err);
        });
    };
$scope.downloadFile = function (file) {

    var link = document.createElement('a');

    link.href = file.fileurl;
    link.download = file.filedes || '';

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
    $scope.removedocumentnew = function (fileId) {
        if (!$scope.requireEdit($scope.currentFileType)) return;
        //if (!confirm("Are you sure you want to delete this file?")) return;

        $http({
            method: 'POST',
            url: '/pms/DeleteimagehandlerANG.ashx',
            data: { id: fileId },
            headers: { 'Content-Type': 'application/json' }
        }).then(function (res) {
            if (res.data.success) {
                if ($scope.currentFileType == "milestone") {
                    let index = $scope.projectlist.miles[$scope.currentIndex].viewfiles.findIndex(f => f.id == fileId);
                    if (index !== -1) {
                        //$scope.projectlist.miles[$scope.currentIndex].viewfiles.splice(index, 1);
                        $scope.projectlist.miles[$scope.currentIndex].viewfiles.splice(index, 1);
                        $scope.showfileslist = angular.copy($scope.projectlist.miles[$scope.currentIndex].viewfiles);
                    }
                }
                else if ($scope.currentFileType == "task") {

                    var milesid = $scope.currentFMilestoneId;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == milesid);
                    let index = $scope.projectlist.miles[mindex].tasks[$scope.currentIndex].viewfiles.findIndex(f => f.id == fileId);
                    if (index !== -1) {
                        $scope.projectlist.miles[mindex].tasks[$scope.currentIndex].viewfiles.splice(index, 1);
                        $scope.showfileslist = angular.copy($scope.projectlist.miles[mindex].tasks[$scope.currentIndex].viewfiles);
                    }
                }
                else if ($scope.currentFileType == "subtask") {
                    var milesid = $scope.currentFMilestoneId;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == milesid);
                    var taskid = $scope.currentFTaskId;
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == taskid);
                    let index = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[$scope.currentIndex].viewfiles.findIndex(f => f.id == fileId);
                    if (index !== -1) {
                        $scope.projectlist.miles[mindex].tasks[tindex].subtasks[$scope.currentIndex].viewfiles.splice(index, 1);
                        $scope.showfileslist = angular.copy($scope.projectlist.miles[mindex].tasks[tindex].subtasks[$scope.currentIndex].viewfiles);
                    }
                }
            } else {
                alert("Failed to delete file: " + res.data.message);
            }
        }, function (err) {
            alert("Error deleting file");
        });
    };
    //#endregion
    //#region manage milestone new
    $scope.newmilestonelist = {
        id: 0,
        ProjectId: 0,
        Milestone: "",
        Weightage: null,
        StartDate: null,
        EndDate: null,
        Owner: "",
        assignto:"0",
        Access: null,
        team: "",
        P_Status: null,
        status_name: "-",
        status_color: "",
        Priority: null,
        priority_name: "-",
        priority_color: "",
        Budget: null,
        Actual: null,
        uid: 0,
        cdate: null,
        statuscomplete: 0,
        istask: 0,
        checkmileWeightage: 0,
        iscomplete: 0,
        iscompletedate: null,
        iscompletepercentage: 0,
        Durationtype: 0,
        TaskDuration: "",
        TaskDuration_min: "",
        Durationhourcal: 0,
        orderno: null,
        miledescription: "",
        Uploaddrawing: "",
        txtdays: "",
        complete_value: "0",
        pending_value: "100",
        taskcount: 0,
        isExpanded: false,
        leftdays: "",
        owners: [],
        teams: [],
        viewfiles: [],
        tasks: [],
        CriticalLabelId:"0",
        CriticalLabel:"",
        CriticalLabelcolor:""

    };
    $scope.newMid = "0";
    $scope.addRowUpDown_default = async function () {
        debugger;

        if (!$scope.projectlist) {
            return;
        }

        if (!$scope.projectlist.miles) {
            $scope.projectlist.miles = [];
        }

        // Copy default milestone template
        let newRow = angular.copy($scope.newmilestonelist);

        /*
         * Date rule for a newly added default milestone:
         * - First milestone: use only Project StartDate.
         * - When any milestone already exists: keep StartDate blank.
         * - EndDate and duration days always remain blank initially.
         */
        if (
            $scope.projectlist.miles.length === 0 &&
            $scope.projectlist.StartDate
        ) {
            newRow.StartDate = parseDotNetDate(
                $scope.projectlist.StartDate
            );
        }
        else {
            newRow.StartDate = null;
        }

        newRow.EndDate = null;
        newRow.txtdays = "";

        /*
         * Default status
         */
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";

        try {
            var ststuddefault = ($scope.statuslists || []).find(
                function (m) {
                    return Number(m.isdefault) === 1;
                }
            );

            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }
        }
        catch (statusError) {
            console.error("Default status error", statusError);
        }

        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        /*
         * Default priority
         */
        var npriority = 0;
        var npriority_name = "-";
        var npriority_color = "";

        try {
            var prioritydefault = ($scope.prioritylists || []).find(
                function (m) {
                    return Number(m.isdefault) === 1;
                }
            );

            if (prioritydefault) {
                npriority = prioritydefault.id;
                npriority_name = prioritydefault.name;
                npriority_color = prioritydefault.colorname;
            }
        }
        catch (priorityError) {
            console.error("Default priority error", priorityError);
        }

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;

        /*
         * Copy project properties
         */
        newRow.Owner = $scope.projectlist.Owner;
        newRow.owners = $scope.projectlist.owners;
        newRow.team = $scope.projectlist.team;
        newRow.teams = $scope.projectlist.teams;
        newRow.ProjectId = $scope.projectlist.id;

        if ($scope.projectlist.hastask == 1) {
            newRow.istask = 1;
        }

        if ($scope.projectlist.checkprojectWeightage == 1) {
            newRow.checkmileWeightage = 1;
        }

        newRow.orderno = $scope.projectlist.miles.length + 1;
        newRow.cdate = null;
        newRow.iscompletedate = null;

        /*
         * Add the new blank/default row to the UI.
         */
        $scope.projectlist.miles.push(newRow);

        /*
         * Save the milestone with the initial date values above.
         */
        try {
            var response = await $scope.addnewmilestone(
                newRow,
                newRow.orderno
            );

            newRow.id = response;

            var new_weightage =
                await assignprojectService.getweightage_default(
                    newRow.ProjectId,
                    "milestone"
                );

            newRow.Weightage = new_weightage.data.d;

            angular.forEach(
                $scope.projectlist.miles,
                function (m) {
                    m.Weightage = new_weightage.data.d;
                }
            );

            $scope.refreshOrderNo();

            // The Kanban board uses a flattened card list. Rebuild it after
            // the hierarchy array changes so the new default milestone appears
            // immediately without switching view or reloading the page.
            if (angular.isFunction($scope.syncListAndKanbanViews)) {
                $scope.syncListAndKanbanViews();
            }

            if ($scope.view === "gantt") {
                $scope.buildGantt();
                setTimeout(initTableUI, 300);
            }

            if (!$scope.$$phase) {
                $scope.$apply();
            }
        }
        catch (err) {
            // Remove temporary row when database save fails
            var index = $scope.projectlist.miles.indexOf(newRow);

            if (index > -1) {
                $scope.projectlist.miles.splice(index, 1);
            }

            console.error("Failed to save default milestone", err);
        }
    };
    $scope.addRowUpDown_default_old = function () {
        debugger
        if (!$scope.projectlist || !$scope.projectlist.miles) {
            $scope.projectlist.miles = [];
        }

        // Copy the default template
        let newRow = angular.copy($scope.newmilestonelist);

        // Set dates
        if ($scope.projectlist.miles.length > 0) {
            var milsindex = $scope.projectlist.miles.length - 1;
            var mils = $scope.projectlist.miles[milsindex];
            try {
                newRow.StartDate = mils.EndDate;
            }
            catch{
                newRow.StartDate =normalizeHierarchyDateOnly($scope.projectlist.StartDate)
            }
        }
        else {
            if ($scope.projectlist.StartDate) {
                var newstartdate= normalizeHierarchyDateOnly($scope.projectlist.StartDate);
                if (newstartdate) {
                    newRow.StartDate = parseDotNetDate(newstartdate);
                }
            }
        }
        //if ($scope.projectlist.StartDate) {
        //    newRow.StartDate = $scope.projectlist.StartDate.indexOf('/Date(') === 0
        //        ? new Date(parseInt($scope.projectlist.StartDate.substr(6)))
        //        : normalizeHierarchyDateOnly($scope.projectlist.StartDate);
        //}
        //if ($scope.projectlist.TargetDate) {
        //    newRow.EndDate = $scope.projectlist.TargetDate.indexOf('/Date(') === 0
        //        ? new Date(parseInt($scope.projectlist.TargetDate.substr(6)))
        //        : normalizeHierarchyDateOnly($scope.projectlist.TargetDate);
        //}


        try {
            //newRow.txtdays = getDaysBetween(newRow.StartDate, newRow.EndDate);
            newRow.txtdays = "1";
            /**/
            assignprojectService.getnew_enddate(newRow.StartDate, newRow.txtdays, resolveHierarchyProjectId(newRow))
                .then(function (newEndDate) {
                    if (!newEndDate) {
                        alert("Failed to calculate end date");
                        return;
                    }
                    let end = parseDotNetDate(newEndDate.data.d);
                    newRow.EndDate = end;

                })
                .catch(function (err) {
                    debugger
                    console.error("Update failed", err);

                });

        }
        catch
        {
        }
        // Copy other properties
        // newRow.P_Status = $scope.projectlist.ProjectStatus;
        debugger
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";
        try {
            var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }

        }
        catch
        { }
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        //newRow.Priority = $scope.projectlist.Priority;
        var npriority = 0;
        var npriority_name = "-";
        var npriority_color = "";
        try {
            var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
            if (prioritydefault) {
                npriority = prioritydefault.id;
                npriority_name = prioritydefault.name;
                npriority_color = prioritydefault.colorname;
            }

        }
        catch
        { }
        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;


        newRow.Owner = $scope.projectlist.Owner;
        newRow.owners = $scope.projectlist.owners;
        newRow.team = $scope.projectlist.team;
        newRow.teams = $scope.projectlist.teams;
        newRow.ProjectId = $scope.projectlist.id;
        if ($scope.projectlist.hastask == 1) { newRow.istask = 1; }
        if ($scope.projectlist.checkprojectWeightage == 1) { newRow.checkmileWeightage = 1; }

        // Set order number
        newRow.orderno = $scope.projectlist.miles.length + 1;
        // **Push immediately so UI updates**
        //$scope.projectlist.miles.splice(insertIndex, 0, newRow);
        debugger
        $scope.projectlist.miles.push(newRow);

        // Clear temporary fields
        newRow.cdate = null;
        newRow.iscompletedate = null;

        // Save to DB asynchronously
        $scope.addnewmilestone(newRow, newRow.orderno)
            .then(async function (response) {
                let newid = response;

                // Update the row ID without removing/re-adding
                newRow.id = newid;
                // let new_weightage = await assignprojectService.getweightage(newid,'milestone');
                let new_weightage = await assignprojectService.getweightage_default(newRow.ProjectId, 'milestone')
                newRow.Weightage = new_weightage.data.d;
                angular.forEach($scope.projectlist.miles, function (m) {
                    m.Weightage = new_weightage.data.d;
                });
                // Refresh order numbers if needed
                $scope.refreshOrderNo();
                if ($scope.view == "gantt") {
                    $scope.buildGantt();
                    setTimeout(initTableUI, 300);
                }



                // Make sure Angular notices changes
                if (!$scope.$$phase) $scope.$apply();
            })
            .catch(function (err) {
                // On failure, remove the row immediately
                let index = $scope.projectlist.miles.indexOf(newRow);
                if (index > -1) $scope.projectlist.miles.splice(index, 1);
                console.error("Failed to save default milestone", err);
            });
    };
    $scope.addRowUpDown_mile_gantt = function ($index, c, position) {
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
        if (mindex !== -1) {
            let c1 = $scope.projectlist.miles[mindex];
            $scope.addRowUpDown_mile(mindex, c1, position);
        }

    }
    $scope.addRowUpDown_mile = async function ($index, c, position) {

        /*
         * Parse dates without directly using new Date("yyyy-MM-dd"),
         * because that can shift the date according to browser timezone.
         */
        if (c.StartDate) {
            c.StartDate = parseDotNetDate(c.StartDate);
        }

        if (c.EndDate) {
            c.EndDate = parseDotNetDate(c.EndDate);
        }

        // Create a separate template copy.
        let milestoneTemplate = angular.copy($scope.newmilestonelist);

        /*
         * addRowUpDown_mile is used only when the project already has
         * a milestone. Do not copy any previous/current milestone date.
         */
        milestoneTemplate.StartDate = null;
        milestoneTemplate.EndDate = null;
        milestoneTemplate.txtdays = "";

        let newRow = angular.copy(milestoneTemplate);

        let insertIndex;
        let newOrderNo;

        if (position === "up") {
            insertIndex = $index;
            newOrderNo = Number(c.orderno) || 1;
        }
        else {
            insertIndex = $index + 1;
            newOrderNo = (Number(c.orderno) || 0) + 1;
        }

        /*
         * Default status
         */
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";

        try {
            var statusDefault = ($scope.statuslists || []).find(function (m) {return Number(m.isdefault) === 1;}
            );

            if (statusDefault) {
                nstatus = statusDefault.id;
                nstatus_name = statusDefault.name;
                nstatus_color = statusDefault.colorname;
            }
        }
        catch (statusError) {
            console.error("Default status error", statusError);
        }

        /*
         * Default priority
         */
        var npriority = 0;
        var npriority_name = "-";
        var npriority_color = "";

        try {
            var priorityDefault = ($scope.prioritylists || []).find(
                function (m) {
                    return Number(m.isdefault) === 1;
                }
            );

            if (priorityDefault) {
                npriority = priorityDefault.id;
                npriority_name = priorityDefault.name;
                npriority_color = priorityDefault.colorname;
            }
        }
        catch (priorityError) {
            console.error("Default priority error", priorityError);
        }

        newRow.orderno = newOrderNo;
        newRow.ProjectId = c.ProjectId;

        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;

        newRow.Owner = c.Owner;
        newRow.owners = angular.copy(c.owners || []);
        newRow.team = c.team;
        newRow.teams = angular.copy(c.teams || []);

        if ($scope.projectlist.hastask == 1) {
            newRow.istask = 1;
        }

        if ($scope.projectlist.checkprojectWeightage == 1) {
            newRow.checkmileWeightage = 1;
        }

        newRow.cdate = null;
        newRow.iscompletedate = null;
        newRow.tasks = [];
        newRow.viewfiles = [];

        /*
         * Insert in UI only after EndDate calculation
         */
        $scope.projectlist.miles.splice(
            insertIndex,
            0,
            newRow
        );

        try {
            /*
             * Important:
             * Save newRow, not angular.copy(c).
             *
             * c is the existing milestone.
             * newRow is the newly generated milestone.
             */
            var newid = await $scope.addnewmilestone(
                newRow,
                newOrderNo
            );

            newRow.id = newid;

            var newWeightageResponse =
                await assignprojectService.getweightage_default(
                    newRow.ProjectId,
                    "milestone"
                );

            if (
                newWeightageResponse &&
                newWeightageResponse.data
            ) {
                newRow.Weightage =
                    newWeightageResponse.data.d;

                angular.forEach(
                    $scope.projectlist.miles,
                    function (m) {
                        m.Weightage =
                            newWeightageResponse.data.d;
                    }
                );
            }

            $scope.refreshOrderNo();

            if ($scope.view === "gantt") {

                let mindex =
                    $scope.projectlist.miles.findIndex(
                        function (f) {
                            return String(f.id) === String(newid);
                        }
                    );

                if (mindex >= 0) {
                    var m =
                        $scope.projectlist.miles[mindex];

                    var mr =
                        $scope.makeGanttRow(
                            m,
                            "milestone",
                            0,
                            null
                        );

                    var ganttInsertIndex = 0;
                    var milestoneCount = 0;

                    for (
                        var i = 0;
                        i < $scope.ganttRows.length;
                        i++
                    ) {
                        if (
                            $scope.ganttRows[i].type ===
                            "milestone"
                        ) {
                            if (
                                milestoneCount === mindex
                            ) {
                                ganttInsertIndex = i;
                                break;
                            }

                            milestoneCount++;
                        }

                        ganttInsertIndex = i + 1;
                    }

                    $scope.ganttRows.splice(
                        ganttInsertIndex,
                        0,
                        mr
                    );
                }
            }

            initTableUI();

            if (!$scope.$$phase) {
                $scope.$apply();
            }
        }
        catch (err) {

            // Remove temporary row if save fails
            var insertedRowIndex =
                $scope.projectlist.miles.indexOf(newRow);

            if (insertedRowIndex > -1) {
                $scope.projectlist.miles.splice(
                    insertedRowIndex,
                    1
                );
            }

            console.error(
                "Failed to save milestone",
                err
            );

            alert("Failed to save milestone");
        }
    };
    $scope.addRowUpDown_mile_old = function ($index, c, position) {
        if (c.StartDate) {
            c.StartDate = normalizeHierarchyDateOnly(c.StartDate);
        }
        if (c.EndDate) {
            c.EndDate = normalizeHierarchyDateOnly(c.EndDate);
        }
        $scope.newmilestonelist.StartDate = c.EndDate
        //$scope.newmilestonelist.StartDate = c.StartDate
        //$scope.newmilestonelist.EndDate = c.EndDate



        let newRow = angular.copy($scope.newmilestonelist);
        let insertIndex, newOrderNo;
        if (position === 'up') {
            insertIndex = $index;        // UI
            newOrderNo = c.orderno;        // DB
        } else {
            insertIndex = $index + 1;
            newOrderNo = c.orderno + 1;
        }
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";
        //nstatus=c.P_Status;    
        //nstatus_name=c.status_name;    
        //nstatus_color=c.status_color;
        try {
            newRow.txtdays = getDaysBetween(newRow.StartDate, newRow.EndDate);
        }
        catch
        {
        }
        try {
            var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }
            var npriority = 0;
            var npriority_name = "-";
            var npriority_color = "";
            try {
                var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
                if (prioritydefault) {
                    npriority = prioritydefault.id;
                    npriority_name = prioritydefault.name;
                    npriority_color = prioritydefault.colorname;
                }

            }
            catch
            { }

        }
        catch
        { }
        newRow.orderno = newOrderNo;
        newRow.ProjectId = c.ProjectId;

        //newRow.P_Status=c.P_Status;    
        //newRow.status_name=c.status_name;    
        //newRow.status_color=c.status_color; 
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;



        newRow.Owner = c.Owner;
        newRow.owners = c.owners;
        newRow.team = c.team;
        newRow.teams = c.teams;
        try {
            if ($scope.projectlist.hastask == 1) { newRow.istask = 1; }
            if ($scope.projectlist.checkprojectWeightage == 1) { newRow.checkmileWeightage = 1; }
        }
        catch{ }

        // Insert in UI
        $scope.projectlist.miles.splice(insertIndex, 0, newRow);
        // Insert in DB    
        let c1 = angular.copy(c);
        c1.cdate = null;
        c1.iscompletedate = null;
        c1.tasks = [];
        c1.viewfiles = [];
        //$scope.addnewmilestone(c,newOrderNo)
        $scope.addnewmilestone(c1, newOrderNo)
            .then(async function (response) {
                var newid = response;
                //let new_weightage = await assignprojectService.getweightage(newid,'milestone');
                let new_weightage = await assignprojectService.getweightage_default(c1.ProjectId, 'milestone')
                newRow.Weightage = new_weightage.data.d;
                angular.forEach($scope.projectlist.miles, function (m) {
                    m.Weightage = new_weightage.data.d;
                });

                $scope.projectlist.miles[insertIndex].id = newid;
                $scope.refreshOrderNo();
                if ($scope.view == "gantt") {
                    let mindex = $scope.projectlist.miles.findIndex(f => f.id === newid);
                    var m = $scope.projectlist.miles[mindex];
                    var mr = $scope.makeGanttRow(m, "milestone", 0, null);
                    var ganttInsertIndex = 0;
                    var milestoneCount = 0;
                    for (var i = 0; i < $scope.ganttRows.length; i++) {

                        if ($scope.ganttRows[i].type === "milestone") {

                            if (milestoneCount === mindex) {

                                ganttInsertIndex = i;

                                break;
                            }

                            milestoneCount++;
                        }

                        ganttInsertIndex = i + 1;
                    }
                    // ================= INSERT AT SAME POSITION =================
                    $scope.ganttRows.splice(ganttInsertIndex, 0, mr);

                }

            })
            .catch(function (err) {
                $scope.projectlist.miles.splice(insertIndex, 1);
                //alert("Failed to save row");
            });
        initTableUI();
    };

    $scope.refreshOrderNo = function () {
        if (!$scope.projectlist || !$scope.projectlist.miles) return;

        let updatePromises = [];

        angular.forEach($scope.projectlist.miles, function (item, index) {
            // Update UI order number

            item.orderno = index + 1;
            let promise = assignprojectService.updateorderno_milestone(item.id, item.orderno)
                .then(function (response) {
                    if (response.data && response.data.d) {
                        console.log('Updated orderno for id:', item.id, 'to', item.orderno);
                    } else {
                        console.warn('Update returned false for id:', item.id);
                    }
                })
                .catch(function (err) {
                    console.error('Failed to update orderno for id:', item.id, err);
                });

            updatePromises.push(promise);
        });

        // Wait for all updates to finish
        Promise.all(updatePromises).then(function () {
            console.log('All order numbers updated successfully.');
        });

    };
    $scope.addnewmilestone = function (object, orderno) {
        return assignprojectService.addnewmilestonerow(object, orderno)
            .then(function (response) {
                if (response.data.d) {
                    return response.data.d;
                } else {
                    return $q.reject({ message: "Server did not return data" });
                }
            });
    };
    $scope.deletedatamile = function (index, c) {
        var isupdate = "0";
        assignprojectService.removemilestone(c.id)
            .then(async function (response) {
                if (response.data.d) {
                    isupdate = response.data.d;
                    if (isupdate == "1") {
                        var removeid = c.id;
                        let realIndex = $scope.projectlist.miles.findIndex(function (f) {
                            return String(f.id) === String(removeid);
                        });
                        if (realIndex !== -1) {
                            $scope.projectlist.miles.splice(realIndex, 1);

                            // Remove the stale wrapper card from Kanban immediately.
                            if (angular.isFunction($scope.syncListAndKanbanViews)) {
                                $scope.syncListAndKanbanViews();
                            }

                            if ($scope.view == "gantt") {
                                $scope.deleteGanttRowById(removeid, 'milestone');

                            }
                        }
                    }
                    let new_weightage = await assignprojectService.getweightage_default(c.ProjectId, 'milestone');
                    var WeightageN = new_weightage.data.d;
                    angular.forEach($scope.projectlist.miles, function (m) {
                        m.Weightage = WeightageN;
                    });
                    $scope.$applyAsync();
                    //if ($scope.view == "kanban") {
                    //    $scope.changeView('kanban', $event);
                    //}
                }
            })



    };
    $scope.deleteGanttRowById = function (id, type) {

        var prefix = "";

        if (type === "milestone") {
            prefix = "m_";
        }
        else if (type === "task") {
            prefix = "t_";
        }
        else if (type === "subtask") {
            prefix = "s_";
        }

        var uid = prefix + id;

        // remove selected row
        var index = $scope.ganttRows.findIndex(function (r) {
            return r.uid === uid;
        });

        if (index > -1) {
            $scope.ganttRows.splice(index, 1);
        }

        // also remove child rows
        //$scope.ganttRows = $scope.ganttRows.filter(function (r) {

        //    if (type === "milestone") {
        //        return r.parentUid !== uid;
        //    }

        //    if (type === "task") {
        //        return r.parentUid !== uid;
        //    }

        //    return true;
        //});

        // update original rows also
        //$scope.originalRows = angular.copy($scope.ganttRows);

        //$scope.buildGanttTimeline();
    };
    $scope.deleteGanttRowById11 = function (id, type) {

        var uid = "";

        if (type === "milestone") {
            uid = "m_" + id;
        }
        else if (type === "task") {
            uid = "t_" + id;
        }
        else {
            uid = "s_" + id;
        }

        // ================= GET ALL CHILD UIDS =================

        var removeMap = {};

        function collectChildren(parentUid) {

            removeMap[parentUid] = true;

            angular.forEach($scope.ganttRows, function (r) {

                if (r.parentUid === parentUid) {

                    collectChildren(r.uid);
                }
            });
        }

        collectChildren(uid);

        // ================= REMOVE ONLY RELATED ROWS =================

        $scope.ganttRows = $scope.ganttRows.filter(function (r) {

            return !removeMap[r.uid];
        });

        // update original rows
        $scope.originalRows = angular.copy($scope.ganttRows);

        $scope.buildGanttTimeline();
    };
    $scope.deletedata_gantt = function (c, utype) {
        if (utype == "milestone") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
            if (mindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex];
                $scope.deletedatamile(mindex, c1);
            }
        }
        else if (utype == "task") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);

            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
            if (tindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex];

                $scope.deletedatatask(tindex, c1, $scope.projectlist.miles[mindex]);
            }
        }
        else if (utype == "subtask") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
            if (tindex !== -1) {
                let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);
                if (sindex !== -1) {
                    let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                    $scope.deletedatasubtask(tindex, c1, $scope.projectlist.miles[mindex].tasks[tindex]);
                }
            }
        }


    };
    //#endregion
    //#region manage milestone task
    $scope.newtasklist = {

        id: 0,
        ProjectId: 0,
        Milestoneid: 0,
        taskname: "",
        taskdescripton: "",
        owner: "",
assignto:"0",
        team: "",
        Startdate: null,
        Targetdate: null,
        Weightage: null,
        StartTime: "",
        Timeline: "",
        TaskDuration: "",
        TaskDuration_min: "",
        P_Status: null,
        status_name: "-",
        status_color: "",
        Priority: null,
        priority_name: "-",
        priority_color: "",
        Budget: null,
        Actual: null,
        Reminder: "",
        ReminderType: null,
        Days: "",
        Time: "",
        uid: null,
        cdate: null,
        tagto: "",
        dependancy: "",
        issubtask: null,
        checkTaskWeightage: "",
        iscomplete: 0,
        iscompletepercentage: 0,
        orderno: null,
        txtdays: "",
        complete_value: "0",
        pending_value: "100",
        isExpanded: false,
        leftdays: "",
        owners: [],
        teams: [],
        viewfiles: [],
        subtasks: [],
        CriticalLabelId:"0",
        CriticalLabel:"",
        CriticalLabelcolor:""

    };
    $scope.addRowUpDown_taskdefault = async function (m) {
        if (!m) {
            return;
        }

        if (!m.tasks) {
            m.tasks = [];
        }

        // Copy default task template.
        let newRow = angular.copy($scope.newtasklist);

        /*
         * New tasks must start with blank dates and blank duration days.
         */
        newRow.Startdate = null;
        newRow.Targetdate = null;
        newRow.txtdays = "";

        /*
         * Default status
         */
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";

        try {
            var statusDefault =
                ($scope.statuslists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (statusDefault) {
                nstatus = statusDefault.id;
                nstatus_name = statusDefault.name;
                nstatus_color = statusDefault.colorname;
            }
        }
        catch (statusError) {
            console.error(
                "Default task status error",
                statusError
            );
        }

        /*
         * Default priority
         */
        var npriority = 0;
        var npriority_name = "-";
        var npriority_color = "";

        try {
            var priorityDefault =
                ($scope.prioritylists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (priorityDefault) {
                npriority = priorityDefault.id;
                npriority_name = priorityDefault.name;
                npriority_color = priorityDefault.colorname;
            }
        }
        catch (priorityError) {
            console.error(
                "Default task priority error",
                priorityError
            );
        }

        /*
         * Copy milestone details into new task
         */
        newRow.ProjectId = m.ProjectId;
        newRow.Milestoneid = m.id;

        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;

        newRow.Owner = m.Owner;
        newRow.owners = angular.copy(m.owners || []);
        newRow.team = m.team;
        newRow.teams = angular.copy(m.teams || []);

        if ($scope.projectlist.hassubtask == 1) {
            newRow.issubtask = 1;
        }

        if ($scope.projectlist.checktaskWeightage == 1) {
            newRow.checkmileWeightage = 1;
        }

        newRow.orderno = m.tasks.length + 1;

        // Clear temporary fields before saving
        newRow.cdate = null;
        newRow.iscompletedate = null;

        /*
         * Targetdate is available now.
         * Add task into UI.
         */
        m.tasks.push(newRow);

        try {
            /*
             * Save task after Startdate and Targetdate calculation.
             */
            var newid = await $scope.addnewtask(
                newRow,
                newRow.orderno
            );

            newRow.id = newid;

            /*
             * Update task weightage
             */
            var newWeightageResponse =
                await assignprojectService.getweightage_default(
                    newRow.Milestoneid,
                    "task"
                );

            if (
                newWeightageResponse &&
                newWeightageResponse.data
            ) {
                newRow.Weightage =
                    newWeightageResponse.data.d;

                var milestoneIndex =
                    $scope.projectlist.miles.findIndex(
                        function (mile) {
                            return String(mile.id) ===
                                String(newRow.Milestoneid);
                        }
                    );

                if (
                    milestoneIndex !== -1 &&
                    $scope.projectlist.miles[milestoneIndex].tasks
                ) {
                    angular.forEach(
                        $scope.projectlist
                            .miles[milestoneIndex]
                            .tasks,
                        function (taskItem) {
                            taskItem.Weightage =
                                newWeightageResponse.data.d;
                        }
                    );
                }
            }

            $scope.refreshOrderNo_tasks(m);

            m.taskcount = 1;

            $scope.toggleMile_default(m);

            /*
             * Add task into Gantt view
             */
            if ($scope.view === "gantt") {
                var taskIndex = m.tasks.findIndex(
                    function (taskItem) {
                        return String(taskItem.id) ===
                            String(newid);
                    }
                );

                if (taskIndex !== -1) {
                    var savedTask = m.tasks[taskIndex];

                    var ganttTaskRow =
                        $scope.makeGanttRow(
                            savedTask,
                            "task",
                            1,
                            "m_" + m.id
                        );

                    try {
                        $scope.ganttRows.push(ganttTaskRow);

                        var ganttMilestoneIndex =
                            $scope.ganttRows.findIndex(
                                function (row) {
                                    return (
                                        row.type === "milestone" &&
                                        String(row.id) === String(m.id)
                                    );
                                }
                            );

                        if (ganttMilestoneIndex !== -1) {
                            $scope.ganttRows[
                                ganttMilestoneIndex
                            ].hasChildren = true;

                            if (
                                $scope.ganttRows[
                                    ganttMilestoneIndex
                                ].raw
                            ) {
                                $scope.ganttRows[
                                    ganttMilestoneIndex
                                ].raw.hasChildren = true;
                            }
                        }
                    }
                    catch (ganttError) {
                        console.error(
                            "Failed to add task into Gantt",
                            ganttError
                        );
                    }
                }
            }

            if (!$scope.$$phase) {
                $scope.$apply();
            }
        }
        catch (err) {
            // Remove temporary task when save fails
            var insertedIndex = m.tasks.indexOf(newRow);

            if (insertedIndex > -1) {
                m.tasks.splice(insertedIndex, 1);
            }

            console.error(
                "Failed to save default task",
                err
            );

            alert("Failed to save default task");
        }
    };
    $scope.addRowUpDown_taskdefault_old = function (m) {
        if (!m.tasks) {
            m.tasks = [];
        }

        // Copy the default template
        let newRow = angular.copy($scope.newtasklist);

        // Set dates
        if (m.StartDate) {

            try {

                newRow.Startdate = normalizeHierarchyDateOnly(m.StartDate);
            } catch{
                newRow.Startdate = normalizeHierarchyDateOnly(m.StartDate)
            }
        }

        if (m.EndDate) {
            try {
                newRow.Targetdate = normalizeHierarchyDateOnly(m.EndDate);

            } catch{
                newRow.Targetdate = normalizeHierarchyDateOnly(m.EndDate)
            }
        }
        /*
        
        try
        {
        newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
        }
        catch
        {
        }*/
        // Copy other properties
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";
        try {
            var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }
            var npriority = 0;
            var npriority_name = "-";
            var npriority_color = "";
            try {
                var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
                if (prioritydefault) {
                    npriority = prioritydefault.id;
                    npriority_name = prioritydefault.name;
                    npriority_color = prioritydefault.colorname;
                }

            }
            catch
            { }
        }
        catch
        { }
        newRow.ProjectId = m.ProjectId;
        newRow.Milestoneid = m.id;

        //newRow.P_Status = m.P_Status;
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;


        //newRow.Priority = m.Priority;
        newRow.Owner = m.Owner;
        newRow.owners = m.owners;
        newRow.team = m.team;
        newRow.teams = m.teams;
        if ($scope.projectlist.hassubtask == 1) { newRow.issubtask = 1; }
        if ($scope.projectlist.checktaskWeightage == 1) { newRow.checkmileWeightage = 1; }
        // Set order number
        newRow.orderno = m.tasks.length + 1;

        // **Push immediately so UI updates**
        //$scope.projectlist.miles.splice(insertIndex, 0, newRow);
        m.tasks.push(newRow);

        // Clear temporary fields
        newRow.cdate = null;
        newRow.iscompletedate = null;

        // Save to DB asynchronously
        $scope.addnewtask(newRow, newRow.orderno)
            .then(async function (response) {
                let newid = response;
                newRow.id = newid;
                //let new_weightage = await assignprojectService.getweightage(newid,'task');
                let new_weightage = await assignprojectService.getweightage_default(newRow.Milestoneid, 'task')
                newRow.Weightage = new_weightage.data.d;
                var gmindex = $scope.projectlist.miles.findIndex(f => f.id === newRow.Milestoneid);
                angular.forEach($scope.projectlist.miles[gmindex].tasks, function (t) {
                    t.Weightage = new_weightage.data.d;
                });
                // Refresh order numbers if needed
                $scope.refreshOrderNo_tasks(m);
                m.taskcount = 1;
                $scope.toggleMile_default(m);
                //toggleMilestone(m);
                if ($scope.view == "gantt") {

                    let mindex = $scope.projectlist.miles.findIndex(f => f.id === m.id);
                    let tindex = m.tasks.findIndex(f => f.id === newid);
                    var t = m.tasks[tindex];
                    var mr = $scope.makeGanttRow(t, "task", 1, "m_" + m.id);
                    try {
                        $scope.ganttRows.push(mr);
                        let gmindex = $scope.ganttRows.findIndex(r => r.type === "milestone" && r.id == m.id);
                        if (gmindex !== -1) {
                            $scope.ganttRows[gmindex].hasChildren = true;
                            $scope.ganttRows[gmindex].raw.hasChildren = true;
                        }
                    }
                    catch (e) {
                        alert(e.message);
                    }
                    //setTimeout(initTableUI, 300);

                }
                // Make sure Angular notices changes
                if (!$scope.$$phase) $scope.$apply();
            })
            .catch(function (err) {
                // On failure, remove the row immediately
                let index = m.tasks.indexOf(newRow);
                if (index > -1) m.tasks.splice(index, 1);
                console.error("Failed to save default milestone", err);
            });
    };
    $scope.addRowUpDown_task_gantt = function ($index, c, position) {
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);

        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
        if (tindex !== -1) {
            let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
            $scope.addRowUpDown_task(tindex, $scope.projectlist.miles[mindex], c1, position);
        }

    }
    $scope.addRowUpDown_task = async function ($index, m, c, position) {
        if (!m || !c) {
            return;
        }

        if (!m.tasks) {
            m.tasks = [];
        }

        /*
         * Create a new task without copying dates from the current task
         * or its milestone.
         */
        let taskTemplate = angular.copy($scope.newtasklist);
        taskTemplate.Startdate = null;
        taskTemplate.Targetdate = null;
        taskTemplate.txtdays = "";

        let newRow = angular.copy(taskTemplate);

        let insertIndex;
        let newOrderNo;

        if (position === "up") {
            insertIndex = $index;
            newOrderNo = Number(c.orderno) || ($index + 1);
        }
        else {
            insertIndex = $index + 1;
            newOrderNo =
                (Number(c.orderno) || ($index + 1)) + 1;
        }

        /*
         * Default status.
         */
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";

        try {
            var statusDefault =
                ($scope.statuslists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (statusDefault) {
                nstatus = statusDefault.id;
                nstatus_name = statusDefault.name;
                nstatus_color = statusDefault.colorname;
            }
        }
        catch (statusError) {
            console.error(
                "Default task status error",
                statusError
            );
        }

        /*
         * Default priority.
         */
        var npriority = 0;
        var npriority_name = "-";
        var npriority_color = "";

        try {
            var priorityDefault =
                ($scope.prioritylists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (priorityDefault) {
                npriority = priorityDefault.id;
                npriority_name = priorityDefault.name;
                npriority_color = priorityDefault.colorname;
            }
        }
        catch (priorityError) {
            console.error(
                "Default task priority error",
                priorityError
            );
        }

        /*
         * Set new task properties.
         */
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;

        newRow.orderno = newOrderNo;
        newRow.ProjectId = c.ProjectId || m.ProjectId;
        newRow.Milestoneid = c.Milestoneid || m.id;

        newRow.Owner = c.Owner;
        newRow.owners = angular.copy(c.owners || []);
        newRow.team = c.team;
        newRow.teams = angular.copy(c.teams || []);

        if ($scope.projectlist.hassubtask == 1) {
            newRow.issubtask = 1;
        }

        if ($scope.projectlist.checktaskWeightage == 1) {
            newRow.checkmileWeightage = 1;
        }

        /*
         * Clear server-generated and child properties.
         */
        newRow.cdate = null;
        newRow.iscompletedate = null;
        newRow.subtasks = [];
        newRow.viewfiles = [];

        /*
         * Insert into UI after Targetdate is available.
         */
        m.tasks.splice(insertIndex, 0, newRow);

        initTableUI();

        try {
            /*
             * Save newRow, not angular.copy(c).
             */
            var newid = await $scope.addnewtask(
                newRow,
                newOrderNo
            );

            newRow.id = newid;

            /*
             * Update task weightage.
             */
            var newWeightageResponse =
                await assignprojectService.getweightage_default(
                    newRow.Milestoneid,
                    "task"
                );

            if (
                newWeightageResponse &&
                newWeightageResponse.data
            ) {
                newRow.Weightage =
                    newWeightageResponse.data.d;

                var milestoneIndex =
                    $scope.projectlist.miles.findIndex(
                        function (mile) {
                            return String(mile.id) ===
                                String(newRow.Milestoneid);
                        }
                    );

                if (
                    milestoneIndex !== -1 &&
                    $scope.projectlist.miles[milestoneIndex].tasks
                ) {
                    angular.forEach(
                        $scope.projectlist
                            .miles[milestoneIndex]
                            .tasks,
                        function (taskItem) {
                            taskItem.Weightage =
                                newWeightageResponse.data.d;
                        }
                    );
                }
            }

            $scope.refreshOrderNo_tasks(m);

            /*
             * Add new task to Gantt.
             */
            if ($scope.view === "gantt") {
                var taskIndex = m.tasks.findIndex(
                    function (taskItem) {
                        return String(taskItem.id) ===
                            String(newid);
                    }
                );

                if (taskIndex !== -1) {
                    var savedTask = m.tasks[taskIndex];

                    var ganttTaskRow =
                        $scope.makeGanttRow(
                            savedTask,
                            "task",
                            1,
                            "m_" + m.id
                        );

                    var ganttInsertIndex =
                        $scope.ganttRows.length;

                    var milestoneGanttIndex =
                        $scope.ganttRows.findIndex(
                            function (row) {
                                return (
                                    row.type === "milestone" &&
                                    String(row.id) === String(m.id)
                                );
                            }
                        );

                    if (milestoneGanttIndex !== -1) {
                        ganttInsertIndex =
                            milestoneGanttIndex + 1;

                        var foundTaskCount = 0;

                        for (
                            var i = milestoneGanttIndex + 1;
                            i < $scope.ganttRows.length;
                            i++
                        ) {
                            var ganttRow =
                                $scope.ganttRows[i];

                            if (
                                ganttRow.type === "milestone"
                            ) {
                                break;
                            }

                            if (
                                ganttRow.type === "task"
                            ) {
                                if (
                                    foundTaskCount === taskIndex
                                ) {
                                    ganttInsertIndex = i;
                                    break;
                                }

                                foundTaskCount++;
                                ganttInsertIndex = i + 1;
                            }
                        }
                    }

                    $scope.ganttRows.splice(
                        ganttInsertIndex,
                        0,
                        ganttTaskRow
                    );

                    if (milestoneGanttIndex !== -1) {
                        $scope.ganttRows[
                            milestoneGanttIndex
                        ].hasChildren = true;

                        if (
                            $scope.ganttRows[
                                milestoneGanttIndex
                            ].raw
                        ) {
                            $scope.ganttRows[
                                milestoneGanttIndex
                            ].raw.hasChildren = true;
                        }
                    }
                }
            }

            if (!$scope.$$phase) {
                $scope.$apply();
            }
        }
        catch (saveError) {
            /*
             * Remove temporary task when DB save fails.
             */
            var insertedIndex =
                m.tasks.indexOf(newRow);

            if (insertedIndex > -1) {
                m.tasks.splice(insertedIndex, 1);
            }

            console.error(
                "Failed to save new task",
                saveError
            );

            alert("Failed to save new task");
        }
    };
    $scope.addRowUpDown_task_old = function ($index, m, c, position) {
        if (c.Startdate) {
            c.Startdate = normalizeHierarchyDateOnly(c.Startdate);
        }
        if (c.Targetdate) {
            c.Targetdate = normalizeHierarchyDateOnly(c.Targetdate);
        }
        $scope.newtasklist.Startdate = c.Startdate
        $scope.newtasklist.Targetdate = c.Targetdate

        let newRow = angular.copy($scope.newtasklist);

        let insertIndex, newOrderNo;

        if (position === 'up') {
            insertIndex = $index;        // UI
            newOrderNo = c.orderno;        // DB
        } else {
            insertIndex = $index + 1;
            newOrderNo = c.orderno + 1;
        }
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";
        try {
            var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }


            var npriority = 0;
            var npriority_name = "-";
            var npriority_color = "";
            try {
                var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
                if (prioritydefault) {
                    npriority = prioritydefault.id;
                    npriority_name = prioritydefault.name;
                    npriority_color = prioritydefault.colorname;
                }

            }
            catch
            { }

        }
        catch
        { }
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;


        newRow.orderno = newOrderNo;
        newRow.ProjectId = c.ProjectId;
        newRow.Milestoneid = c.Milestoneid;


        //newRow.P_Status=c.P_Status;    
        //newRow.Priority=c.Priority;  
        /*
        
        try
        {
        newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
        }
        catch
        {
        }*/


        newRow.Owner = c.Owner;
        newRow.owners = c.owners;
        newRow.team = c.team;
        newRow.teams = c.teams;
        if ($scope.projectlist.hassubtask == 1) { newRow.issubtask = 1; }
        if ($scope.projectlist.checktaskWeightage == 1) { newRow.checkmileWeightage = 1; }
        // Insert in UI
        //$scope.projectlist.miles.tasks.splice(insertIndex, 0, newRow); 
        m.tasks.splice(insertIndex, 0, newRow);
        // Insert in DB    
        let c1 = angular.copy(c);
        c1.cdate = null;
        c1.iscompletedate = null;
        c1.subtasks = [];
        c1.viewfiles = [];
        $scope.addnewtask(c1, newOrderNo)
            .then(async function (response) {
                var newid = response;
                //let new_weightage = await assignprojectService.getweightage(newid,'task');
                let new_weightage = await assignprojectService.getweightage_default(c1.Milestoneid, 'task')
                newRow.Weightage = new_weightage.data.d;
                var gmindex = $scope.projectlist.miles.findIndex(f => f.id === newRow.Milestoneid);
                angular.forEach($scope.projectlist.miles[gmindex].tasks, function (t) {
                    t.Weightage = new_weightage.data.d;
                });
                //$scope.projectlist.miles.tasks[insertIndex].id=newid;       
                m.tasks[insertIndex].id = newid;
                $scope.refreshOrderNo_tasks(m);
                if ($scope.view == "gantt") {

                    let mindex = $scope.projectlist.miles.findIndex(f => f.id === m.id);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === newid);
                    var t = $scope.projectlist.miles[mindex].tasks[tindex];
                    var mr = $scope.makeGanttRow(t, "task", 1, "m_" + m.id);
                    var ganttInsertIndex = 0;
                    var milestoneCount = 0;
                    for (var i = 0; i < $scope.ganttRows.length; i++) {

                        if ($scope.ganttRows[i].type === "task") {
                            if (milestoneCount === tindex) {
                                ganttInsertIndex = i;
                                break;
                            }
                            milestoneCount++;
                        }
                        ganttInsertIndex = i + 1;
                    }
                    // ================= INSERT AT SAME POSITION =================
                    $scope.ganttRows.splice(ganttInsertIndex, 0, mr);
                    let gmindex = $scope.ganttRows.findIndex(r => r.type === "milestone" && r.id == m.id);
                    if (gmindex !== -1) {
                        $scope.ganttRows[gmindex].hasChildren = true;
                        $scope.ganttRows[gmindex].raw.hasChildren = true;
                    }
                    //$scope.buildGantt();
                    //setTimeout(initTableUI, 300);
                }

            })
            .catch(function (err) {
                m.tasks.splice(insertIndex, 1);
                //alert("Failed to save row");
            });
        initTableUI();
    };
    $scope.refreshOrderNo_tasks = function (m) {
        if (!m.tasks) return;

        let updatePromises = [];
        angular.forEach(m.tasks, function (item, index) {
            // Update UI order number

            item.orderno = index + 1;
            let promise = assignprojectService.updateorderno_task(item.id, item.orderno)
                .then(function (response) {
                    if (response.data && response.data.d) {
                        console.log('Updated orderno for id:', item.id, 'to', item.orderno);
                    } else {
                        console.warn('Update returned false for id:', item.id);
                    }
                })
                .catch(function (err) {
                    console.error('Failed to update orderno for id:', item.id, err);
                });

            updatePromises.push(promise);
        });

        // Wait for all updates to finish
        Promise.all(updatePromises).then(function () {
            console.log('All order numbers updated successfully.');
        });
    };
    $scope.addnewtask = function (object, orderno) {
        return assignprojectService.addnewtaskrow(object, orderno)
            .then(function (response) {
                if (response.data.d) {
                    return response.data.d;
                } else {
                    return $q.reject({ message: "Server did not return data" });
                }
            });
    };
    $scope.deletedatatask = function (index, c, m) {
        var isupdate = "0";
        assignprojectService.removetask(c.id)
            .then(async function (response) {
                if (response.data.d) {
                    isupdate = response.data.d;
                    if (isupdate == "1") {
                        var removeid = c.id;
                        let realIndex = (m.tasks || []).findIndex(function (f) {
                            return String(f.id) === String(removeid);
                        });
                        if (realIndex !== -1) {
                            m.tasks.splice(realIndex, 1);

                            // Rebuild Kanban because its flattened array still contains
                            // the deleted task wrapper until it is explicitly refreshed.
                            if (angular.isFunction($scope.syncListAndKanbanViews)) {
                                $scope.syncListAndKanbanViews();
                            }

                            if ($scope.view == "gantt") {
                                $scope.deleteGanttRowById(removeid, 'task');

                            }
                        }
                    }
                    let new_weightage = await assignprojectService.getweightage_default(c.Milestoneid, 'task');
                    var WeightageN = new_weightage.data.d;
                    var gmindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
                    angular.forEach($scope.projectlist.miles[gmindex].tasks, function (t) {
                        t.Weightage = WeightageN;
                    });
                    $scope.$applyAsync();
                }
            })



    };
    //#endregion
    //#region manage milestone subtask
    $scope.newsubtasklist = {

        id: 0,
        ProjectId: 0,
        Milestoneid: 0,
        Task: 0,
        Subtaskname: "",
        SubTaskDescription: "",
        owner: "",
assignto:"0",
        team: "",
        Startdate: null,
        Targetdate: null,
        Weightage: null,
        StartTime: "",
        Timeline: "",
        TaskDuration: "",
        TaskDuration_min: "",
        P_Status: null,
        status_name: "-",
        status_color: "",
        Priority: null,
        priority_name: "-",
        priority_color: "",
        Budget: null,
        Actual: null,
        Reminder: "",
        ReminderType: null,
        Days: "",
        Time: "",
        uid: null,
        cdate: null,
        tagto: "",
        dependancy: "",
        iscomplete: 0,
        iscompletepercentage: 0,
        orderno: null,
        txtdays: "",
        complete_value: "0",
        pending_value: "100",
        leftdays: "",
        owners: [],
        teams: [],
        viewfiles: [],
        CriticalLabelId:"0",
        CriticalLabel:"",
        CriticalLabelcolor:""
    };
    $scope.addRowUpDown_subtaskdefault = async function (t) {
        if (!t) {
            return;
        }

        if (!t.subtasks) {
            t.subtasks = [];
        }

        // Use subtask template; fall back to existing task template.
        let newRow = angular.copy(
            $scope.newsubtasklist || $scope.newtasklist
        );

        /*
         * New subtasks must start with blank dates and blank duration days.
         */
        newRow.Startdate = null;
        newRow.Targetdate = null;
        newRow.txtdays = "";

        /*
         * Default status.
         */
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";

        try {
            var statusDefault =
                ($scope.statuslists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (statusDefault) {
                nstatus = statusDefault.id;
                nstatus_name = statusDefault.name;
                nstatus_color = statusDefault.colorname;
            }
        }
        catch (statusError) {
            console.error(
                "Default subtask status error",
                statusError
            );
        }

        /*
         * Default priority.
         */
        var npriority = 0;
        var npriority_name = "-";
        var npriority_color = "";

        try {
            var priorityDefault =
                ($scope.prioritylists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (priorityDefault) {
                npriority = priorityDefault.id;
                npriority_name = priorityDefault.name;
                npriority_color = priorityDefault.colorname;
            }
        }
        catch (priorityError) {
            console.error(
                "Default subtask priority error",
                priorityError
            );
        }

        /*
         * Set new subtask properties.
         */
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;

        newRow.ProjectId = t.ProjectId;
        newRow.Milestoneid = t.Milestoneid;
        newRow.Task = t.id;

        newRow.Owner = t.Owner;
        newRow.owners = angular.copy(t.owners || []);
        newRow.team = t.team;
        newRow.teams = angular.copy(t.teams || []);

        newRow.orderno = t.subtasks.length + 1;

        // Clear temporary/server-generated fields.
        newRow.cdate = null;
        newRow.iscompletedate = null;
        newRow.viewfiles = [];

        /*
         * Targetdate is ready, so insert into UI.
         */
        t.subtasks.push(newRow);

        try {
            /*
             * Save after Startdate and Targetdate are calculated.
             */
            var newid = await $scope.addnewsubtask(
                newRow,
                newRow.orderno
            );

            newRow.id = newid;

            /*
             * Update subtask weightage.
             */
            var weightageResponse =
                await assignprojectService.getweightage_default(
                    newRow.Task,
                    "subtask"
                );

            if (
                weightageResponse &&
                weightageResponse.data
            ) {
                newRow.Weightage = weightageResponse.data.d;

                var milestoneIndex =
                    $scope.projectlist.miles.findIndex(
                        function (mile) {
                            return String(mile.id) ===
                                String(newRow.Milestoneid);
                        }
                    );

                if (milestoneIndex !== -1) {
                    var milestone =
                        $scope.projectlist.miles[milestoneIndex];

                    var taskIndex =
                        (milestone.tasks || []).findIndex(
                            function (taskItem) {
                                return String(taskItem.id) ===
                                    String(newRow.Task);
                            }
                        );

                    if (taskIndex !== -1) {
                        angular.forEach(
                            milestone.tasks[taskIndex].subtasks || [],
                            function (subtaskItem) {
                                subtaskItem.Weightage =
                                    weightageResponse.data.d;
                            }
                        );
                    }
                }
            }

            $scope.refreshOrderNo_subtasks(t);
            $scope.toggleTask_default(t);

            /*
             * Add new subtask into Gantt view.
             */
            if ($scope.view === "gantt") {
                var subtaskIndex =
                    t.subtasks.findIndex(
                        function (subtaskItem) {
                            return String(subtaskItem.id) ===
                                String(newid);
                        }
                    );

                if (subtaskIndex !== -1) {
                    var savedSubtask =
                        t.subtasks[subtaskIndex];

                    var ganttSubtaskRow =
                        $scope.makeGanttRow(
                            savedSubtask,
                            "subtask",
                            2,
                            "t_" + t.id
                        );

                    $scope.ganttRows.push(
                        ganttSubtaskRow
                    );

                    var ganttTaskIndex =
                        $scope.ganttRows.findIndex(
                            function (row) {
                                return (
                                    row.type === "task" &&
                                    String(row.id) === String(t.id)
                                );
                            }
                        );

                    if (ganttTaskIndex !== -1) {
                        $scope.ganttRows[
                            ganttTaskIndex
                        ].hasChildren = true;

                        if (
                            $scope.ganttRows[
                                ganttTaskIndex
                            ].raw
                        ) {
                            $scope.ganttRows[
                                ganttTaskIndex
                            ].raw.hasChildren = true;
                        }
                    }

                    setTimeout(initTableUI, 300);
                }
            }

            if (!$scope.$$phase) {
                $scope.$apply();
            }
        }
        catch (saveError) {
            // Remove temporary UI row if save fails.
            var insertedIndex =
                t.subtasks.indexOf(newRow);

            if (insertedIndex > -1) {
                t.subtasks.splice(insertedIndex, 1);
            }

            console.error(
                "Failed to save default subtask",
                saveError
            );

            alert("Failed to save default subtask");
        }
    };
    $scope.addRowUpDown_subtaskdefault_old = function (t) {
        if (!t.subtasks) {
            t.subtasks = [];
        }

        // Copy the default template
        let newRow = angular.copy($scope.newsubtasklist);

        // Set dates
        if (t.Startdate) {

            try {
                newRow.Startdate = normalizeHierarchyDateOnly(t.Startdate);

            } catch{
                newRow.Startdate = normalizeHierarchyDateOnly(t.Startdate)
            }
        }

        if (t.Targetdate) {
            try {
                newRow.Targetdate = normalizeHierarchyDateOnly(t.Targetdate);

            } catch{
                newRow.Targetdate = normalizeHierarchyDateOnly(t.Targetdate)
            }
        }
        /*
        
        try
        {
        newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
        }
        catch
        {
        }*/

        // Copy other properties
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";
        try {
            var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }

            var npriority = 0;
            var npriority_name = "-";
            var npriority_color = "";
            try {
                var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
                if (prioritydefault) {
                    npriority = prioritydefault.id;
                    npriority_name = prioritydefault.name;
                    npriority_color = prioritydefault.colorname;
                }

            }
            catch
            { }

        }
        catch
        { }
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;


        newRow.ProjectId = t.ProjectId;
        newRow.Milestoneid = t.Milestoneid;
        newRow.Task = t.id;

        //newRow.P_Status = t.P_Status;
        //newRow.Priority = t.Priority;
        newRow.Owner = t.Owner;
        newRow.owners = t.owners;
        newRow.team = t.team;
        newRow.teams = t.teams;



        // Set order number
        newRow.orderno = t.subtasks.length + 1;

        // **Push immediately so UI updates**
        //$scope.projectlist.miles.splice(insertIndex, 0, newRow);
        t.subtasks.push(newRow);

        // Clear temporary fields
        newRow.cdate = null;
        newRow.iscompletedate = null;

        // Save to DB asynchronously
        $scope.addnewsubtask(newRow, newRow.orderno)
            .then(async function (response) {
                let newid = response;

                // Update the row ID without removing/re-adding
                newRow.id = newid;
                //let new_weightage = await assignprojectService.getweightage(newid,'subtask');
                let new_weightage = await assignprojectService.getweightage_default(newRow.Task, 'subtask');
                newRow.Weightage = new_weightage.data.d;
                var gmindex = $scope.projectlist.miles.findIndex(f => f.id === newRow.Milestoneid);
                var gtindex = $scope.projectlist.miles[gmindex].tasks.findIndex(f => f.id === newRow.Task);
                angular.forEach($scope.projectlist.miles[gmindex].tasks[gtindex].subtasks, function (s) {
                    s.Weightage = new_weightage.data.d;
                });
                // Refresh order numbers if needed
                $scope.refreshOrderNo_subtasks(t);
                $scope.toggleTask_default(t);
                if ($scope.view == "gantt") {

                    let sindex = t.subtasks.findIndex(f => f.id === newid);
                    var s = t.subtasks[sindex];
                    var mr = $scope.makeGanttRow(s, "subtask", 2, "t_" + t.id);
                    $scope.ganttRows.push(mr);
                    try {
                        let gtindex = $scope.ganttRows.findIndex(r => r.type === "task" && r.id == t.id);
                        if (gtindex !== -1) {
                            $scope.ganttRows[gtindex].hasChildren = true;
                            $scope.ganttRows[gtindex].raw.hasChildren = true;
                        }
                    }
                    catch{ }

                    setTimeout(initTableUI, 300);
                }

                // Make sure Angular notices changes
                if (!$scope.$$phase) $scope.$apply();
            })
            .catch(function (err) {
                // On failure, remove the row immediately
                let index = t.subtasks.indexOf(newRow);
                if (index > -1) t.subtasks.splice(index, 1);
                console.error("Failed to save default milestone", err);
            });
    };
    $scope.addRowUpDown_subtask_gantt = function ($index, c, position) {
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
        if (tindex !== -1) {
            let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);
            if (sindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                $scope.addRowUpDown_subtask(sindex, $scope.projectlist.miles[mindex].tasks[tindex], c1, position);
            }
        }

    }
    $scope.addRowUpDown_subtask = async function ($index, t, c, position) {
        if (!t || !c) {
            return;
        }

        if (!t.subtasks) {
            t.subtasks = [];
        }

        /*
         * Create a new subtask without copying dates from the current
         * subtask or its parent task.
         */
        var subtaskTemplate = angular.copy(
            $scope.newsubtasklist || {}
        );
        subtaskTemplate.Startdate = null;
        subtaskTemplate.Targetdate = null;
        subtaskTemplate.txtdays = "";

        var newRow = angular.copy(
            subtaskTemplate
        );

        var insertIndex;
        var newOrderNo;

        if (position === "up") {
            insertIndex = $index;
            newOrderNo =
                Number(c.orderno) || ($index + 1);
        }
        else {
            insertIndex = $index + 1;
            newOrderNo =
                (Number(c.orderno) || ($index + 1)) + 1;
        }

        /*
         * Default status.
         */
        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";

        try {
            var statusDefault =
                ($scope.statuslists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (statusDefault) {
                nstatus = statusDefault.id;
                nstatus_name = statusDefault.name;
                nstatus_color = statusDefault.colorname;
            }
        }
        catch (statusError) {
            console.error(
                "Default subtask status error",
                statusError
            );
        }

        /*
         * Default priority.
         */
        var npriority = 0;
        var npriority_name = "-";
        var npriority_color = "";

        try {
            var priorityDefault =
                ($scope.prioritylists || []).find(
                    function (item) {
                        return Number(item.isdefault) === 1;
                    }
                );

            if (priorityDefault) {
                npriority = priorityDefault.id;
                npriority_name = priorityDefault.name;
                npriority_color = priorityDefault.colorname;
            }
        }
        catch (priorityError) {
            console.error(
                "Default subtask priority error",
                priorityError
            );
        }

        /*
         * Set new subtask properties.
         */
        newRow.orderno = newOrderNo;

        newRow.ProjectId =
            c.ProjectId || t.ProjectId;

        newRow.Milestoneid =
            c.Milestoneid || t.Milestoneid;

        newRow.Task =
            c.Task || t.id;

        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;

        newRow.Owner = c.Owner || t.Owner;
        newRow.owners = angular.copy(
            c.owners || t.owners || []
        );

        newRow.team = c.team || t.team;
        newRow.teams = angular.copy(
            c.teams || t.teams || []
        );

        if (
            $scope.projectlist.checksubtaskWeightage == 1
        ) {
            newRow.Weightage = c.Weightage;
        }

        /*
         * Clear server-generated fields.
         */
        newRow.cdate = null;
        newRow.iscompletedate = null;
        newRow.viewfiles = [];

        /*
         * Insert into UI after Targetdate calculation.
         */
        t.subtasks.splice(
            insertIndex,
            0,
            newRow
        );

        try {
            /*
             * Save newRow.
             * Do not modify or save existing subtask c.
             */
            var newid =
                await $scope.addnewsubtask(
                    newRow,
                    newOrderNo
                );

            newRow.id = newid;

            /*
             * Update subtask weightage.
             */
            var newWeightageResponse =
                await assignprojectService.getweightage_default(
                    newRow.Task,
                    "subtask"
                );

            if (
                newWeightageResponse &&
                newWeightageResponse.data
            ) {
                newRow.Weightage =
                    newWeightageResponse.data.d;

                var milestoneIndex =
                    $scope.projectlist.miles.findIndex(
                        function (mile) {
                            return String(mile.id) ===
                                String(newRow.Milestoneid);
                        }
                    );

                if (milestoneIndex !== -1) {
                    var milestone =
                        $scope.projectlist.miles[
                        milestoneIndex
                        ];

                    var taskIndex =
                        (milestone.tasks || []).findIndex(
                            function (taskItem) {
                                return String(taskItem.id) ===
                                    String(newRow.Task);
                            }
                        );

                    if (taskIndex !== -1) {
                        angular.forEach(
                            milestone.tasks[taskIndex]
                                .subtasks || [],
                            function (subtaskItem) {
                                subtaskItem.Weightage =
                                    newWeightageResponse.data.d;
                            }
                        );
                    }
                }
            }

            $scope.refreshOrderNo_subtasks(t);

            /*
             * Add new subtask into Gantt view.
             */
            if ($scope.view === "gantt") {
                var savedSubtaskIndex =
                    t.subtasks.findIndex(
                        function (subtaskItem) {
                            return String(subtaskItem.id) ===
                                String(newid);
                        }
                    );

                if (savedSubtaskIndex !== -1) {
                    var savedSubtask =
                        t.subtasks[
                        savedSubtaskIndex
                        ];

                    var ganttSubtaskRow =
                        $scope.makeGanttRow(
                            savedSubtask,
                            "subtask",
                            2,
                            "t_" + t.id
                        );

                    var ganttTaskIndex =
                        $scope.ganttRows.findIndex(
                            function (row) {
                                return (
                                    row.type === "task" &&
                                    String(row.id) ===
                                    String(t.id)
                                );
                            }
                        );

                    var ganttInsertIndex =
                        ganttTaskIndex !== -1
                            ? ganttTaskIndex + 1
                            : $scope.ganttRows.length;

                    /*
                     * Search only inside the current task's
                     * Gantt children.
                     */
                    if (ganttTaskIndex !== -1) {
                        var foundSubtaskCount = 0;

                        for (
                            var i = ganttTaskIndex + 1;
                            i < $scope.ganttRows.length;
                            i++
                        ) {
                            var ganttRow =
                                $scope.ganttRows[i];

                            if (
                                ganttRow.type === "task" ||
                                ganttRow.type === "milestone"
                            ) {
                                break;
                            }

                            if (
                                ganttRow.type === "subtask"
                            ) {
                                if (
                                    foundSubtaskCount ===
                                    savedSubtaskIndex
                                ) {
                                    ganttInsertIndex = i;
                                    break;
                                }

                                foundSubtaskCount++;
                                ganttInsertIndex = i + 1;
                            }
                        }
                    }

                    $scope.ganttRows.splice(
                        ganttInsertIndex,
                        0,
                        ganttSubtaskRow
                    );

                    if (ganttTaskIndex !== -1) {
                        $scope.ganttRows[
                            ganttTaskIndex
                        ].hasChildren = true;

                        if (
                            $scope.ganttRows[
                                ganttTaskIndex
                            ].raw
                        ) {
                            $scope.ganttRows[
                                ganttTaskIndex
                            ].raw.hasChildren = true;
                        }
                    }
                }
            }

            initTableUI();

            if (!$scope.$$phase) {
                $scope.$apply();
            }
        }
        catch (saveError) {
            /*
             * Remove temporary row when saving fails.
             */
            var insertedRowIndex =
                t.subtasks.indexOf(newRow);

            if (insertedRowIndex > -1) {
                t.subtasks.splice(
                    insertedRowIndex,
                    1
                );
            }

            console.error(
                "Failed to save new subtask",
                saveError
            );

            alert("Failed to save new subtask");
        }
    };
    $scope.addRowUpDown_subtask_old = function ($index, t, c, position) {
        if (c.Startdate) {
            c.Startdate = normalizeHierarchyDateOnly(c.Startdate);
        }
        if (c.Targetdate) {
            c.Targetdate = normalizeHierarchyDateOnly(c.Targetdate);
        }
        $scope.newsubtasklist.Startdate = c.Startdate
        $scope.newsubtasklist.Targetdate = c.Targetdate

        let newRow = angular.copy($scope.newsubtasklist);
        let insertIndex, newOrderNo;

        if (position === 'up') {
            insertIndex = $index;        // UI
            newOrderNo = c.orderno;        // DB
        } else {
            insertIndex = $index + 1;
            newOrderNo = c.orderno + 1;
        }
        newRow.orderno = newOrderNo;
        newRow.ProjectId = c.ProjectId;
        newRow.Milestoneid = c.Milestoneid;
        newRow.Task = c.Task;


        var nstatus = 0;
        var nstatus_name = "";
        var nstatus_color = "";
        try {
            var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }

            var npriority = 0;
            var npriority_name = "-";
            var npriority_color = "";
            try {
                var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
                if (prioritydefault) {
                    npriority = prioritydefault.id;
                    npriority_name = prioritydefault.name;
                    npriority_color = prioritydefault.colorname;
                }

            }
            catch
            { }

        }
        catch
        { }
        /*
        try
        {
        newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
        }
        catch
        {
        }*/
        newRow.P_Status = nstatus;
        newRow.status_name = nstatus_name;
        newRow.status_color = nstatus_color;

        newRow.Priority = npriority;
        newRow.priority_name = npriority_name;
        newRow.priority_color = npriority_color;



        newRow.Owner = c.Owner;
        newRow.team = c.team;
        if ($scope.projectlist.checksubtaskWeightage == 1) { newRow.Weightage = c.Weightage; }
        // Insert in UI
        //$scope.projectlist.miles.tasks.splice(insertIndex, 0, newRow); 
        t.subtasks.splice(insertIndex, 0, newRow);
        // Insert in DB    

        c.cdate = null;
        c.iscompletedate = null;
        c.viewfiles = [];
        $scope.addnewsubtask(c, newOrderNo)
            .then(async function (response) {
                var newid = response;
                //let new_weightage = await assignprojectService.getweightage(newid,'subtask');
                let new_weightage = await assignprojectService.getweightage_default(c.Task, 'subtask');
                newRow.Weightage = new_weightage.data.d;
                var gmindex = $scope.projectlist.miles.findIndex(f => f.id === newRow.Milestoneid);
                var gtindex = $scope.projectlist.miles[gmindex].tasks.findIndex(f => f.id === newRow.Task);
                angular.forEach($scope.projectlist.miles[gmindex].tasks[gtindex].subtasks, function (s) {
                    s.Weightage = new_weightage.data.d;
                });
                //$scope.projectlist.miles.tasks[insertIndex].id=newid;       
                t.subtasks[insertIndex].id = newid;
                $scope.refreshOrderNo_subtasks(t);
                if ($scope.view == "gantt") {
                    let mindex = $scope.projectlist.miles.findIndex(f => f.id === t.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === t.id);
                    let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === newid);
                    var s = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                    var mr = $scope.makeGanttRow(s, "subtask", 2, "t_" + t.id);
                    var ganttInsertIndex = 0;
                    var milestoneCount = 0;
                    for (var i = 0; i < $scope.ganttRows.length; i++) {

                        if ($scope.ganttRows[i].type === "subtask") {
                            if (milestoneCount === sindex) {
                                ganttInsertIndex = i;
                                break;
                            }
                            milestoneCount++;
                        }
                        ganttInsertIndex = i + 1;
                    }
                    // ================= INSERT AT SAME POSITION =================
                    $scope.ganttRows.splice(ganttInsertIndex, 0, mr);
                    try {
                        let gtindex = $scope.ganttRows.findIndex(r => r.type === "task" && r.id == t.id);
                        if (gtindex !== -1) {
                            $scope.ganttRows[gtindex].hasChildren = true;
                            $scope.ganttRows[gtindex].raw.hasChildren = true;
                        }
                    }
                    catch{ }
                }

            })
            .catch(function (err) {
                t.subtasks.splice(insertIndex, 1);
                //alert("Failed to save row");
            });

    };
    $scope.refreshOrderNo_subtasks = function (t) {
        if (!t.subtasks) return;

        let updatePromises = [];
        angular.forEach(t.subtasks, function (item, index) {
            // Update UI order number

            item.orderno = index + 1;
            let promise = assignprojectService.updateorderno_subtask(item.id, item.orderno)
                .then(function (response) {
                    if (response.data && response.data.d) {
                        console.log('Updated orderno for id:', item.id, 'to', item.orderno);
                    } else {
                        console.warn('Update returned false for id:', item.id);
                    }
                })
                .catch(function (err) {
                    console.error('Failed to update orderno for id:', item.id, err);
                });

            updatePromises.push(promise);
        });

        // Wait for all updates to finish
        Promise.all(updatePromises).then(function () {
            console.log('All order numbers updated successfully.');
        });
    };
    $scope.addnewsubtask = function (object, orderno) {
        return assignprojectService.addnewsubtaskrow(object, orderno)
            .then(function (response) {
                if (response.data.d) {
                    return response.data.d;
                } else {
                    return $q.reject({ message: "Server did not return data" });
                }
            });
    };
    $scope.deletedatasubtask = function (index, c, t) {
        var isupdate = "0";
        assignprojectService.removesubtask(c.id)
            .then(async function (response) {
                if (response.data.d) {
                    isupdate = response.data.d;
                    if (isupdate == "1") {
                        var removeid = c.id;
                        let realIndex = (t.subtasks || []).findIndex(function (f) {
                            return String(f.id) === String(removeid);
                        });
                        if (realIndex !== -1) {
                            t.subtasks.splice(realIndex, 1);

                            // Rebuild Kanban so the deleted subtask card and column
                            // count are updated in the same click.
                            if (angular.isFunction($scope.syncListAndKanbanViews)) {
                                $scope.syncListAndKanbanViews();
                            }

                            if ($scope.view == "gantt") {
                                $scope.deleteGanttRowById(removeid, 'subtask');

                            }
                        }
                    }
                    let new_weightage = await assignprojectService.getweightage_default(c.Task, 'subtask');
                    var WeightageN = new_weightage.data.d;
                    var gmindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
                    var gtindex = $scope.projectlist.miles[gmindex].tasks.findIndex(f => f.id === c.Task);
                    angular.forEach($scope.projectlist.miles[gmindex].tasks[gtindex].subtasks, function (s) {
                        s.Weightage = new_weightage.data.d;
                    });
                    $timeout(function () {

                    }, 0);
                }
            })

        initTableUI();

    };
    //#endregion
    //#region add default gantt
    $scope.adddefault_gantt = function (c, utype) {
        if (utype == "milestone") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
            if (mindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex];
                $scope.addRowUpDown_taskdefault(c1);

            }
        }
        else if (utype == "task") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
            if (tindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
                $scope.addRowUpDown_subtaskdefault(c1);
            }
        }
    }


    //#endregion
    //#region bind lists
    $scope.removeowners = function (obj1, obj2, utype) {
        var projectowneres = {
            id: obj1.id,
            Owner: obj2.id,
            Ownertype: utype
        };

        $http({
            method: "POST",
            url: '/pms/assignprojects/default.aspx/removeownerbyid',
            headers: { "Content-Type": "application/json" },
            data: { projectowneres: projectowneres }
        }).then(function (response) {
            if (response.data.d == "1") {
                let indx = obj1.owners.findIndex(m => m.id == obj2.id);
                if (indx !== -1) {
                    obj1.owners.splice(indx, 1);
                    //start
                    if ($scope.view === "gantt") {

                        let rid = obj1.id;
                        let ownid = obj2.id;
                        //MILESTONE
                        if (utype === 'milestone') {
                            let m = $scope.projectlist.miles.find(x => x.id == rid);
                            if (m && m.owners) {
                                let idx = m.owners.findIndex(o => o.id == ownid);
                                if (idx !== -1) {
                                    m.owners.splice(idx, 1);
                                }
                            }
                        }
                        //TASK
                        else if (utype === 'task') {
                            let m = $scope.projectlist.miles.find(x => x.id == obj1.Milestoneid);
                            if (m && m.tasks) {
                                let t = m.tasks.find(x => x.id == rid);
                                if (t && t.owners) {
                                    let idx = t.owners.findIndex(o => o.id == ownid);
                                    if (idx !== -1) {
                                        t.owners.splice(idx, 1);
                                    }
                                }
                            }
                        }
                        // SUBTASK
                        else if (utype === 'subtask') {
                            let m = $scope.projectlist.miles.find(x => x.id == obj1.Milestoneid);
                            if (m && m.tasks) {
                                let t = m.tasks.find(x => x.id == obj1.Taskid); // ✅ use Taskid
                                if (t && t.subtasks) {
                                    let s = t.subtasks.find(x => x.id == rid);
                                    if (s && s.owners) {
                                        let idx = s.owners.findIndex(o => o.id == ownid);
                                        if (idx !== -1) {
                                            s.owners.splice(idx, 1);
                                        }
                                    }
                                }
                            }
                        }
                    }// end
                }


            }

        }, function (error) {
            alert("Error: " + (error.data || error.statusText));
        });
        initTableUI();
        return false;
    };
    // Avatar tooltips must close even after clicking a member or remove icon.
    $scope.hideTeamAvatarTooltip = function ($event) {
        var el = $event && $event.currentTarget;
        if (!el) return;
        var popupId = el.getAttribute("aria-describedby");
        if (el.blur) el.blur();
        if (window.bootstrap && window.bootstrap.Tooltip) {
            var instance = window.bootstrap.Tooltip.getInstance(el);
            if (instance) instance.hide();
        }
        // Clean up a popup left by an older tooltip instance after re-rendering.
        if (popupId) {
            popupId.split(/\s+/).forEach(function (id) {
                var popup = document.getElementById(id);
                if (popup && popup.classList.contains("tooltip")) popup.remove();
            });
        }
    };

    function showTeamRemovalFailure(result) {
        if ($scope.resetAssignTooltips) $scope.resetAssignTooltips();
        var prefix = "TEAM_ASSIGNMENT_BLOCKED:";
        var message = typeof result === "string" && result.indexOf(prefix) === 0
            ? result.substring(prefix.length)
            : "Unable to remove the team member. Please try again.";
        if (window.Swal && window.Swal.fire) {
            window.Swal.fire({
                title: "Cannot remove team member",
                text: message,
                icon: "warning",
                showCloseButton: true,
                confirmButtonText: "Close",
                allowOutsideClick: false,
                allowEscapeKey: false
            });
        } else {
            alert(message);
        }
    }
    $scope.removeteams = function (obj1, obj2, utype) {
        debugger
        if (!$scope.requireEdit(utype)) return false;
        var projectteams = {
            id: obj1.id,
            Team: obj2.id,
            Teamtype: utype
        };

        $http({
            method: "POST",
            url: '/pms/assignprojects/default.aspx/removeteambyid',
            headers: { "Content-Type": "application/json" },
            data: { projectteams: projectteams }
        }).then(function (response) {
            if (response.data.d == "1") {
                syncRemovedTeamDownward(utype, obj1, obj2.id);
                $scope.$applyAsync();
                setTimeout(function () {
                    initTableUI11();
                }, 300);
            } else {
                showTeamRemovalFailure(response.data.d);
            }

        }, function (error) {
            alert("Error: " + (error.data || error.statusText));
        });


        return false;
    };
    $scope.mytasklist_bystatus = [];
    $scope.filterstatuslist_count = function () {
        if ($scope.statuslists && projectlist.miles.length > 0) {

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
            console.log($scope.mytasklist_bystatus);
        }
    };
    $scope.toggleMile_default = function (item) {
        if (!item.isExpanded) {
            item.isExpanded = true;
        }
        setTimeout(initTableUI, 300);
    };
    $scope.toggleTask_default = function (item) {
        if (!item.isExpanded) {
            item.isExpanded = true;
        }
        setTimeout(initTableUI, 300);
    };

    $scope.toggleMilestone = function (item) {
        debugger
        item.isExpanded = !item.isExpanded;
        angular.forEach(item.tasks || [], function (task) {
            if (!task) return;

            task.isExpanded = false;

            // Reset any nested expansion state, if present.
            angular.forEach(task.subtasks || [], function (subtask) {
                if (subtask) {
                    subtask.isExpanded = false;
                }
            });
        });

        setTimeout(initTableUI, 300);
    };
    $scope.toggleTask = function (item) {
        item.isExpanded = !item.isExpanded;
        setTimeout(initTableUI, 300);
    };

    //$scope.updatesrc = function($event,mid) {
    //debugger
    //   var img = $event.currentTarget.querySelector('img'); // get clicked image
    // if (!img) return;

    //var currentSrc = img.src;

    //if (currentSrc.includes('open.png')) {

    // img.src = '/pms/assets/images/close.png';
    //img.alt = 'close';
    //} else {
    //img.src = '/pms/assets/images/open.png';
    //img.alt = 'open';
    // }
    //};
    $scope.loadtasksbyid = function (id) {
        assignprojectService.gettasklistbyid(id
        ).then(function (response) {
            $scope.milestonetasklist = response.data.d || {};
            $scope.milestonetasklist.StartDate = normalizeHierarchyDateOnly($scope.milestonetasklist.StartDate);
            $scope.milestonetasklist.EndDate = normalizeHierarchyDateOnly($scope.milestonetasklist.EndDate);
            //setTimeout(initTableUI, 300);
            angular.forEach($scope.milestonetasklist.tasks || [], function (c) {
                c.Startdate = normalizeHierarchyDateOnly(c.Startdate);
                c.Targetdate = normalizeHierarchyDateOnly(c.Targetdate);
            });

        }).catch(function (error) {

            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        }).finally(function () {
            //$scope.isLoading = false;
        });
    };
    //#endregion



    $scope.startOfDay = function (dt) {
        var d = normalizeHierarchyDateOnly(dt);
        if (!d) return null;
        d.setHours(0, 0, 0, 0);
        return d;
    };

    $scope.addDays = function (dt, days) {
        var d = normalizeHierarchyDateOnly(dt);
        if (!d) return null;
        d.setDate(d.getDate() + days);
        return normalizeHierarchyDateOnly(d);
    };

    $scope.diffDays = function (d1, d2) {
        var ms = 1000 * 60 * 60 * 24;
        return Math.round(($scope.startOfDay(d1) - $scope.startOfDay(d2)) / ms);
    };

    $scope.getTaskEnd = function (obj, type) {
        if (!obj) return null;

        if (type === "milestone") {
            var milestoneStart = obj.StartDate || obj.Startdate || obj.startDate || obj.startdate;
            var milestoneEnd = obj.EndDate || obj.TargetDate || obj.Targetdate || obj.endDate || obj.targetdate;
            if (milestoneEnd) return normalizeHierarchyDateOnly(milestoneEnd);
            if (milestoneStart && obj.txtdays) return $scope.addDays(milestoneStart, Math.max(0, (parseInt(obj.txtdays, 10) || 1) - 1));
            return milestoneStart ? normalizeHierarchyDateOnly(milestoneStart) : null;
        }

        if (type === "task" || type === "subtask") {
            var childStart = obj.Startdate || obj.StartDate || obj.startdate || obj.startDate;
            var childEnd = obj.Targetdate || obj.TargetDate || obj.EndDate || obj.targetdate || obj.endDate;
            if (childEnd) return normalizeHierarchyDateOnly(childEnd);
            if (childStart && obj.txtdays) return $scope.addDays(childStart, Math.max(0, (parseInt(obj.txtdays, 10) || 1) - 1));
            return childStart ? normalizeHierarchyDateOnly(childStart) : null;
        }

        return null;
    };

    $scope.toProgress = function (val) {
        //debugger
        var n = parseFloat(val || 0);
        if (isNaN(n)) n = 0;
        if (n < 0) n = 0;
        if (n > 100) n = 100;
        return n;
    };
    $scope.updatedetails_gant = function (c) {

        let resultnew = null;
        if (c.type === 'milestone') {
            resultnew = assignprojectService.updatedetails_mile_bycol(c.id, 'Milestone', c.name);
        }
        else if (c.type === 'task') {
            resultnew = assignprojectService.updatedetails_task_bycol(c.id, 'taskname', c.name);
        }
        else if (c.type === 'subtask') {
            resultnew = assignprojectService.updatedetails_subtask_bycol(c.id, 'Subtaskname', c.name);
        }

        if (!resultnew) return;

        resultnew.then(function (res) {
            if (res.data.d === "1") {
                if ($scope.view === "gantt") {
                    let rid = c.id;
                    if (c.type === 'milestone') {
                        let mindex = $scope.projectlist.miles.findIndex(m => m.id == rid);
                        if (mindex !== -1) {
                            $scope.projectlist.miles[mindex].Milestone = c.name;
                        }
                    }
                    else if (c.type === 'task') {
                        let mid = c.Milestoneid;
                        let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                        if (mindex !== -1) {
                            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(t => t.id == rid);
                            if (tindex !== -1) {
                                $scope.projectlist.miles[mindex].tasks[tindex].taskname = c.name;
                            }
                        }
                    }
                    else if (c.type === 'subtask') {
                        let mid = c.Milestoneid;
                        let tid = c.Task;
                        let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                        if (mindex !== -1) {
                            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(t => t.id == tid);
                            if (tindex !== -1) {
                                let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(s => s.id == rid);
                                if (sindex !== -1) {
                                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].Subtaskname = c.name;
                                }
                            }
                        }
                    }
                }
            }

        }).catch(function () {
            alert("Update failed");
        });
    };
    $scope.loadmilecompletiondetails = function (id) {

        assignprojectService.bindcompletiondetails_mile(id)
            .then(function (response) {

                var mile_listdetails = response.data.d;

                if (mile_listdetails) {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == id);
                    $scope.projectlist.miles[mindex].complete_value = mile_listdetails.complete_value;
                    $scope.projectlist.miles[mindex].pending_value = mile_listdetails.pending_value;
                    if (mile_listdetails.tasks_details && mile_listdetails.tasks_details.length > 0) {
                        angular.forEach(mile_listdetails.tasks_details, function (t) {
                            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == t.id);
                            if (tindex !== -1) {

                                // ================= Tasks =================
                                $scope.projectlist.miles[mindex].tasks[tindex].complete_value = t.complete_value;
                                $scope.projectlist.miles[mindex].tasks[tindex].pending_value = t.pending_value;

                                // ================= Subt tasks =================
                                if (t.subtasks_details && t.subtasks_details.length > 0) {

                                    angular.forEach(t.subtasks_details, function (s) {

                                        let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == s.id);
                                        if (sindex !== -1) {
                                            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].complete_value = s.complete_value;
                                            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].pending_value = s.pending_value;

                                        }
                                    });
                                }
                            }
                        });

                    }
                    $scope.$applyAsync();
                }
                if ($scope.view == "gantt") {
                    //updateGanttHierarchyCompletion('milestone',);
                    $scope.updateGanttCompletion(id);
                }
            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {
                // $scope.isLoading = false;
            });


    };
    $scope.loadtaskcompletiondetails = function (id) {

        assignprojectService.bindcompletiondetails_task(id)
            .then(function (response) {

                var task_listdetails = response.data.d;

                if (task_listdetails) {
                    var mid = task_listdetails.Milestoneid;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == task_listdetails.id);
                    if (tindex !== -1) {
                        $scope.projectlist.miles[mindex].tasks[tindex].complete_value = task_listdetails.complete_value;
                        $scope.projectlist.miles[mindex].tasks[tindex].pending_value = task_listdetails.pending_value;
                        if (task_listdetails.subtasks_details && task_listdetails.subtasks_details.length > 0) {

                            angular.forEach(task_listdetails.subtasks_details, function (s) {

                                let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == s.id);
                                if (sindex !== -1) {
                                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[tindex].complete_value = s.complete_value;
                                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].pending_value = s.pending_value;

                                }
                            });
                        }
                        if ($scope.view == "gantt") {
                            $scope.updateGanttCompletion(mid);
                        }
                    }

                    $scope.$applyAsync();
                }

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {
                // $scope.isLoading = false;
            });
    };
    $scope.loadsubtaskcompletiondetails = function (id) {

        assignprojectService.bindcompletiondetails_subtask(id)
            .then(function (response) {
                var subtask_listdetails = response.data.d;

                if (subtask_listdetails) {
                    var mid = subtask_listdetails.Milestoneid;
                    var tid = subtask_listdetails.Task;
                    var sid = subtask_listdetails.id;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == tid);
                    if (tindex !== -1) {
                        let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == sid);
                        if (sindex !== -1) {
                            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[tindex].complete_value = subtask_listdetails.complete_value;
                            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[tindex].pending_value = subtask_listdetails.pending_value;

                        }
                    }
                    if ($scope.view == "gantt") {
                        $scope.updateGanttCompletion(mid);
                    }
                    $scope.$applyAsync();
                }

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {
                // $scope.isLoading = false;
            });
    };
    $scope.updateGanttCompletion = function (mileId) {

        // ================= FIND MILESTONE =================
        let mile = ($scope.projectlist.miles || []).find(m => m.id == mileId);
        if (!mile) return;

        // =====================================================
        // MILESTONE
        // =====================================================
        let gmindex = $scope.ganttRows.findIndex(r =>
            r.type === "milestone" &&
            r.id == mile.id
        );

        if (gmindex !== -1) {

            $scope.ganttRows[gmindex].status_id = mile.P_Status;
            $scope.ganttRows[gmindex].status_name = mile.status_name;
            $scope.ganttRows[gmindex].status_color = mile.status_color;
            $scope.ganttRows[gmindex].progress = $scope.toProgress(mile.complete_value);
            // UPDATE RAW VALUES
            $scope.ganttRows[gmindex].raw.status_id = mile.P_Status;
            $scope.ganttRows[gmindex].raw.status_name = mile.status_name;
            $scope.ganttRows[gmindex].raw.status_color = mile.status_color;
            $scope.ganttRows[gmindex].raw.complete_value = mile.complete_value;
            $scope.ganttRows[gmindex].raw.pending_value = mile.pending_value;

            // UPDATE PROGRESS BAR
            $scope.ganttRows[gmindex].progress = $scope.toProgress(mile.complete_value || mile.iscompletepercentage);
        }

        // =====================================================
        // TASKS
        // =====================================================

        angular.forEach((mile.tasks || []), function (t) {
            let gtindex = $scope.ganttRows.findIndex(r =>
                r.type === "task" &&
                r.id == t.id
            );
            if (gtindex !== -1) {
                $scope.ganttRows[gtindex].status_id = t.P_Status;
                $scope.ganttRows[gtindex].status_name = t.status_name;
                $scope.ganttRows[gtindex].status_color = t.status_color;
                $scope.ganttRows[gtindex].progress = $scope.toProgress(t.complete_value);
                // UPDATE RAW VALUES
                $scope.ganttRows[gtindex].raw.status_id = t.P_Status;
                $scope.ganttRows[gtindex].raw.status_name = t.status_name;
                $scope.ganttRows[gtindex].raw.status_color = t.status_color;
                $scope.ganttRows[gtindex].raw.complete_value = t.complete_value;
                $scope.ganttRows[gtindex].raw.pending_value = t.pending_value;
                $scope.ganttRows[gtindex].progress = $scope.toProgress(t.complete_value || t.iscompletepercentage);


            }

            // =====================================================
            // SUBTASKS
            // =====================================================

            angular.forEach((t.subtasks || []), function (s) {

                let gsindex = $scope.ganttRows.findIndex(r =>
                    r.type === "subtask" &&
                    r.id == s.id
                );

                if (gsindex !== -1) {

                    $scope.ganttRows[gsindex].status_id = s.P_Status;
                    $scope.ganttRows[gsindex].status_name = s.status_name;
                    $scope.ganttRows[gsindex].status_color = s.status_color;
                    $scope.ganttRows[gsindex].progress = $scope.toProgress(s.complete_value);
                    // UPDATE RAW VALUES
                    $scope.ganttRows[gsindex].raw.status_id = s.P_Status;
                    $scope.ganttRows[gsindex].raw.status_name = s.status_name;
                    $scope.ganttRows[gsindex].raw.status_color = s.status_color;
                    $scope.ganttRows[gsindex].raw.complete_value = s.complete_value;
                    $scope.ganttRows[gsindex].raw.pending_value = s.pending_value;

                    // UPDATE PROGRESS
                    $scope.ganttRows[gsindex].progress =
                        $scope.toProgress(
                            s.complete_value ||
                            s.iscompletepercentage
                        );
                }
            });
        });

        $scope.$applyAsync();
    };
    $scope.updateGanttHierarchyCompletion = function (type, item) {

        function updateRow(rowType, rowId) {

            // FIND GANTT ROW
            let gindex = $scope.ganttRows.findIndex(r =>
                r.type === rowType &&
                r.id == rowId
            );

            if (gindex === -1) return;

            // FIND ACTUAL DATA
            var data = null;

            // ================= MILESTONE =================
            if (rowType === "milestone") {

                data = ($scope.projectlist.miles || [])
                    .find(m => m.id == rowId);
            }

            // ================= TASK =================
            else if (rowType === "task") {

                angular.forEach(($scope.projectlist.miles || []), function (m) {

                    let t =
                        (m.tasks || [])
                            .find(x => x.id == rowId);

                    if (t) {
                        data = t;
                    }
                });
            }

            // ================= SUBTASK =================
            else if (rowType === "subtask") {

                angular.forEach(($scope.projectlist.miles || []), function (m) {

                    angular.forEach((m.tasks || []), function (t) {

                        let s =
                            (t.subtasks || [])
                                .find(x => x.id == rowId);

                        if (s) {
                            data = s;
                        }
                    });
                });
            }

            if (!data) return;

            // UPDATE RAW VALUES
            $scope.ganttRows[gindex].raw.complete_value =
                data.complete_value;

            $scope.ganttRows[gindex].raw.pending_value =
                data.pending_value;

            // UPDATE PROGRESS
            $scope.ganttRows[gindex].progress =
                $scope.toProgress(
                    data.complete_value ||
                    data.iscompletepercentage
                );
        }

        // =====================================================
        // MILESTONE
        // =====================================================

        if (type === "milestone") {

            updateRow(
                "milestone",
                item.id
            );
        }

        // =====================================================
        // TASK
        // =====================================================

        else if (type === "task") {

            // TASK
            updateRow(
                "task",
                item.id
            );

            // MILESTONE
            updateRow(
                "milestone",
                item.Milestoneid
            );
        }

        // =====================================================
        // SUBTASK
        // =====================================================

        else if (type === "subtask") {

            // SUBTASK
            updateRow(
                "subtask",
                item.id
            );

            // TASK
            updateRow(
                "task",
                item.Task
            );

            // MILESTONE
            updateRow(
                "milestone",
                item.Milestoneid
            );
        }

        $scope.$applyAsync();
    };
    $scope.makeGanttRow = function (item, type, level, parentUid) {
        //debugger
        var _start = null;
        var _end = null;
        var _name = "";
        var _owners = [];
        var _status_id = "";
        var _status_name = "";
        var _status_color = "";
        var _progress = 0;
        var _uid = "";
        var _istask = 0;
        var _issubtask = 0;
        var _ProjectId = "";
        var _Milestoneid = "";
        var _Task = "";
        var _ownerid = "0";
        var _hasChildren = false;

        if (type === "milestone") {
            _start = normalizeHierarchyDateOnly(item.StartDate || item.Startdate || item.startDate || item.startdate);
            _end = $scope.getTaskEnd(item, "milestone");
            _name = item.Milestone || "";
            _istask = item.istask || 0;
            _ownerid = item.Owner || "0";
            _owners = item.owners || [];
            _status_id = item.id;
            _status_name = item.status_name || "";
            _status_color = item.status_color || "";
            _progress = $scope.toProgress(item.complete_value || item.iscompletepercentage);
            _uid = "m_" + item.id;
            _ProjectId = item.ProjectId;
            _Milestoneid = item.id;

        }
        else if (type === "task") {
            _start = normalizeHierarchyDateOnly(item.Startdate || item.StartDate || item.startdate || item.startDate);
            _end = $scope.getTaskEnd(item, "task");
            _name = item.taskname || "";
            _issubtask = item.issubtask || 0;
            _ownerid = item.owner || "0";
            _owners = item.owners || [];
            _status_id = item.id;
            _status_name = item.status_name || "";
            _status_color = item.status_color || "";
            _progress = $scope.toProgress(item.complete_value || item.iscompletepercentage);
            _uid = "t_" + item.id;
            _ProjectId = item.ProjectId;
            _Milestoneid = item.Milestoneid;

        }
        else {
            _start = normalizeHierarchyDateOnly(item.Startdate || item.StartDate || item.startdate || item.startDate);
            _end = $scope.getTaskEnd(item, "subtask");
            _name = item.Subtaskname || "";
            _ownerid = item.owner || "0";
            _owners = item.owners || [];
            _status_id = item.id;
            _status_name = item.status_name || "";
            _status_color = item.status_color || "";
            _progress = $scope.toProgress(item.complete_value || item.iscompletepercentage);
            _uid = "s_" + item.id;
            _ProjectId = item.ProjectId;
            _Milestoneid = item.Milestoneid;
            _Task = item.Task;

        }

        // Gantt's Assigned To column must represent the scheduler assignment,
        // not the hierarchy Owner/team.  Falling back to item.owner caused the
        // project owner (for example PA) to appear on unassigned rows.
        _owners = $scope.resolveGanttAssignedOwners(item, type);
        _ownerid = _owners.length ? _owners.map(function (x) { return x.id; }).join(',') : '0';

        return {
            uid: _uid,
            id: item.id,
            code: item.code || item.Code || item.taskcode || item.TaskCode || item.subtaskcode || item.MilestoneCode || '',
            parentUid: parentUid || null,
            type: type,
            level: level,
            name: _name,
            start: _start,
            end: _end,
            owners: _owners,
            status_id: _status_id,
            status_name: _status_name,
            status_color: _status_color,
            progress: _progress,
            expanded: true,
            visible: true,
            hasChildren: false,
            ProjectId: _ProjectId,
            Milestoneid: _Milestoneid,
            Task: _Task,
            Owner: _ownerid,
            istask: _istask,
            issubtask: _issubtask,
            raw: item
        };
    };

    $scope.resolveGanttAssignedOwners = function (item, type) {
        item = item || {};
        var result = [];
        var seen = {};
        var normalizedType = $scope.normalizeAssignTaskType(type);

        function addEmployee(emp) {
            if (!emp) return;
            var id = emp.id || emp.Id || emp.ownerid || emp.OwnerId || '';
            var key = String(id || emp.name || emp.DisplayName || '');
            if (!key || seen[key]) return;
            seen[key] = true;
            var name = emp.name || emp.DisplayName || emp.Name || '';
            result.push({
                id: id,
                name: name,
                image: emp.profileimage || emp.fileupload1 || emp.ProfileImage || emp.image || '',
                profileimage: emp.profileimage || emp.fileupload1 || emp.ProfileImage || emp.image || '',
                shortname: emp.shortname || emp.splinameone || (name ? name.substring(0, 2).toUpperCase() : '')
            });
        }

        function addEmployeeId(value) {
            angular.forEach(String(value || '').split(','), function (rawId) {
                var id = String(rawId || '').trim();
                if (!id || id === '0') return;
                var emp = ($scope.owners || []).find(function (x) {
                    return String(x.id || x.Id) === id;
                }) || ($scope.currenttaskteam || []).find(function (x) {
                    return String(x.id || x.Id) === id;
                });
                if (emp) addEmployee(emp);
            });
        }

        // First use the assignment id already bound to the hierarchy row.
        addEmployeeId(item.assignto !== undefined ? item.assignto : item.AssignTo);

        // The flat assignment list can contain the assignee even when the
        // hierarchy row was loaded without assignto.
        angular.forEach($scope.currenttasklist || [], function (assigned) {
            if (String(assigned.id || assigned.taskid || '') !== String(item.id || '')) return;
            if ($scope.normalizeAssignTaskType(assigned.tasktype || assigned.taskType) !== normalizedType) return;
            addEmployeeId(assigned.assignto !== undefined ? assigned.assignto : assigned.AssignTo);
        });

        // Finally resolve the actual employee row that owns a scheduler task
        // segment. This is the authoritative source used by Assigned view.
        angular.forEach($scope.currenttaskteam || [], function (emp) {
            var matched = false;
            angular.forEach((emp.taskSegments || []).concat(emp.usertasks || []), function (assigned) {
                if (matched) return;
                if (String(assigned.taskid || assigned.id || '') === String(item.id || '') &&
                    $scope.normalizeAssignTaskType(assigned.tasktype || assigned.taskType) === normalizedType) {
                    matched = true;
                }
            });
            if (matched) addEmployee(emp);
        });

        return result;
    };

    $scope.buildGanttRows = function () {

        var rows = [];

        angular.forEach(($scope.projectlist.miles || []), function (m, milestoneIndex) {
            if (!$scope.hasMatchingMilestoneBranch(m)) return;

            var mr = $scope.makeGanttRow(m, "milestone", 0, null);
            mr.serialNo = m.orderno || String(milestoneIndex + 1);
            var matchingTasks = (m.tasks || []).filter(function (t) {
                return $scope.hasMatchingTaskBranch(t);
            });
            mr.hasChildren = matchingTasks.length > 0;
            rows.push(mr);
            angular.forEach(matchingTasks, function (t, taskIndex) {
                var tr = $scope.makeGanttRow(t, "task", 1, mr.uid);
                var taskOrder = t.orderno || String(taskIndex + 1);
                tr.serialNo = mr.serialNo + "." + taskOrder;
                var matchingSubtasks = (t.subtasks || []).filter(function (s) {
                    return $scope.matchesTopLeafFilters(s);
                });
                tr.hasChildren = matchingSubtasks.length > 0;
                rows.push(tr);

                angular.forEach(matchingSubtasks, function (s, subtaskIndex) {
                    var sr = $scope.makeGanttRow(s, "subtask", 2, tr.uid);
                    var subtaskOrder = s.orderno || String(subtaskIndex + 1);
                    sr.serialNo = tr.serialNo + "." + subtaskOrder;
                    rows.push(sr);
                });
            });
        });

        return rows;
    };

    // Determine hierarchy arrows from the actual parent/child relationship.
    // This remains reliable even after switching directly to Quarter/Year,
    // where a stale hasChildren flag previously hid the Task arrow.
    $scope.hasGanttChildRows = function (row) {
        if (!row || !row.uid) return false;
        if (row.hasChildren) return true;
        var rawChildren = row.type === 'milestone'
            ? ((row.raw && row.raw.tasks) || [])
            : (row.type === 'task' ? ((row.raw && row.raw.subtasks) || []) : []);
        var hasMatchingRawChild = rawChildren.some(function (child) {
            return row.type === 'milestone'
                ? $scope.hasMatchingTaskBranch(child)
                : $scope.matchesTopLeafFilters(child);
        });
        if (hasMatchingRawChild) {
            row.hasChildren = true;
            return true;
        }
        var sources = [$scope.originalRows || [], $scope.ganttRows || []];
        for (var s = 0; s < sources.length; s++) {
            for (var i = 0; i < sources[s].length; i++) {
                if (sources[s][i].parentUid === row.uid) {
                    row.hasChildren = true;
                    return true;
                }
            }
        }
        return false;
    };

    // Kept only for reference. The active five-scale renderer is declared
    // later in the Gantt section. Giving the legacy renderer a separate name
    // prevents it from being used when the scale dropdown changes.
    $scope.buildLegacyGanttTimeline = function () {
        var rows = $scope.ganttRows;
        var minDate = null;
        var maxDate = null;

        angular.forEach(rows, function (r) {
            if (!r.start || !r.end) return;

            var s = $scope.startOfDay(r.start);
            var e = $scope.startOfDay(r.end);

            if (!minDate || s < minDate) minDate = s;
            if (!maxDate || e > maxDate) maxDate = e;
        });

        if (!minDate || !maxDate) {
            $scope.ganttCells = [];
            $scope.ganttHeaderGroups = [];
            return;
        }

        minDate = $scope.addDays(minDate, -2);
        maxDate = $scope.addDays(maxDate, 2);

        $scope.ganttStart = minDate;
        $scope.ganttEnd = maxDate;

        var cells = [];
        var groups = [];
        var cursor = new Date(minDate);
        //if ($scope.ganttScale === "month") {

        //    $scope.ganttCellWidth = 90;

        //    cells = [];
        //    groups = [];

        //    // current year
        //    var year = new Date().getFullYear();

        //    // OR use project start year
        //    // var year = minDate.getFullYear();

        //    for (var m = 0; m < 12; m++) {

        //        var first = new Date(year, m, 1);

        //        cells.push({
        //            date: first,
        //            label: first.toLocaleDateString('en-GB', {
        //                month: 'short',
        //                year: 'numeric'
        //            })
        //        });
        //    }

        //    groups = [{
        //        label: year,
        //        width: cells.length * $scope.ganttCellWidth
        //    }];
        //}
        //else
        if ($scope.ganttScale === "month") {
            $scope.ganttCellWidth = 90;
            while (cursor <= maxDate) {
                var first = new Date(cursor.getFullYear(), cursor.getMonth(), 1);
                var next = new Date(cursor.getFullYear(), cursor.getMonth() + 1, 1);

                cells.push({
                    date: new Date(first),
                    label: first.toLocaleDateString('en-GB', { month: 'short', year: 'numeric' })
                });

                cursor = next;
            }

            groups = [{
                label: "Timeline",
                width: cells.length * $scope.ganttCellWidth
            }];
        }
        else if ($scope.ganttScale === "week") {

            $scope.ganttCellWidth = 70;

            cells = [];
            groups = [];

            var current = new Date(
                minDate.getFullYear(),
                minDate.getMonth(),
                1
            );

            while (current <= maxDate) {

                var year = current.getFullYear();
                var month = current.getMonth();

                var monthName = current.toLocaleDateString('en-GB', {
                    month: 'long',
                    year: 'numeric'
                });

                var daysInMonth = new Date(year, month + 1, 0).getDate();

                var firstDay = new Date(year, month, 1).getDay();

                var totalWeeks = Math.ceil(
                    (daysInMonth + firstDay) / 7
                );

                for (var w = 1; w <= totalWeeks; w++) {

                    cells.push({
                        date: new Date(year, month, ((w - 1) * 7) + 1),
                        label: "W" + w
                    });
                }

                groups.push({
                    label: monthName,
                    width: totalWeeks * $scope.ganttCellWidth
                });

                current = new Date(year, month + 1, 1);
            }
        }
        else {
            $scope.ganttCellWidth = 40;
            while (cursor <= maxDate) {
                cells.push({
                    date: new Date(cursor),
                    label: cursor.getDate()
                });
                cursor = $scope.addDays(cursor, 1);
            }

            var gi = 0;
            while (gi < cells.length) {
                var firstCellDate = cells[gi].date;
                var y = firstCellDate.getFullYear();
                var m = firstCellDate.getMonth();
                var count = 0;

                while ((gi + count) < cells.length) {
                    var tempDate = cells[gi + count].date;
                    if (tempDate.getFullYear() !== y || tempDate.getMonth() !== m) break;
                    count++;
                }

                groups.push({
                    label: firstCellDate.toLocaleDateString('en-GB', { month: 'long', year: 'numeric' }),
                    width: count * $scope.ganttCellWidth
                });

                gi += count;
            }
        }

        $scope.ganttCells = cells;
        $scope.ganttHeaderGroups = groups;
    };
    $scope.getMonthWeekIndex = function (date) {

        if (!date) return 0;

        var total = 0;

        var startMonth = new Date(
            $scope.ganttStart.getFullYear(),
            $scope.ganttStart.getMonth(),
            1
        );

        var currentMonth = new Date(
            date.getFullYear(),
            date.getMonth(),
            1
        );

        // count previous month weeks
        while (startMonth < currentMonth) {

            var y = startMonth.getFullYear();
            var m = startMonth.getMonth();

            var daysInMonth = new Date(y, m + 1, 0).getDate();

            var firstDay = new Date(y, m, 1).getDay();

            var totalWeeks = Math.ceil(
                (daysInMonth + firstDay) / 7
            );

            total += totalWeeks;
            startMonth.setMonth(startMonth.getMonth() + 1);
        }

        // current month week
        var firstDayCurrent = new Date(
            date.getFullYear(),
            date.getMonth(),
            1
        ).getDay();

        var weekNo = Math.ceil(
            (date.getDate() + firstDayCurrent) / 7
        );

        total += (weekNo - 1);

        return total;
    };
    $scope.buildGantt11 = function () {
        // alert('hi');
        var allRows = $scope.buildGanttRows();
        $scope.originalRows = allRows;
        $scope.ganttRows = $scope.getVisibleGanttRows(allRows);

        $scope.buildGanttTimeline();
    };
    $scope.buildGantt = function () {

        var allRows = $scope.buildGanttRows();

        $scope.ganttRows = $scope.initializeGanttState(allRows);

        ganttAutoScrollPending = true;
        $scope.buildGanttTimeline();
        $scope.loadGanttChartDependencies();
        scrollGanttToFirstScheduledRow();
        //setTimeout(syncGanttRowHeight, 100);
    };
    $scope.initializeTableState = function () {
        //return;
        angular.forEach($scope.projectlist.miles, function (r) {

            if (r.isExpanded) {
                r.isExpanded = false;
            }
            if (r.type == "milestone") {
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === r.id);
                var mr = $scope.projectlist.miles[mindex];
                if (mr.isExpanded) {
                    let gmindex = allRows.findIndex(x => x.type === "milestone" && x.uid == "m_" + r.id);
                    if (gmindex !== -1) {
                        allRows[gmindex].hasChildren = true;
                        allRows[gmindex].raw.hasChildren = true;
                        allRows[gmindex].expanded = true;
                        allRows[gmindex].raw.expanded = true;
                    }
                }
            }

        });

    };
    $scope.initializeGanttState = function (allRows) {
        angular.forEach(allRows, function (r) {
            // On the first Gantt load show the complete hierarchy regardless
            // of which scale (Day/Week/Month/Quarter/Year) is selected first.
            // After the user clicks an arrow, retain that explicit choice.
            if (r.hasChildren) {
                r.expanded = r.raw && r.raw._ganttExpandedTouched
                    ? r.raw.isExpanded !== false
                    : true;
            } else {
                r.expanded = true;
            }
            if (r.type == "milestone") {
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === r.id);
                var mr = $scope.projectlist.miles[mindex];
                if (mr.isExpanded) {
                    let gmindex = allRows.findIndex(x => x.type === "milestone" && x.uid == "m_" + r.id);
                    if (gmindex !== -1) {
                        allRows[gmindex].hasChildren = true;
                        allRows[gmindex].raw.hasChildren = true;
                        allRows[gmindex].expanded = true;
                        allRows[gmindex].raw.expanded = true;
                    }
                }
            }
            else if (r.type == "task") {
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === r.Milestoneid);
                let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === r.id);
                var tr = $scope.projectlist.miles[mindex].tasks[tindex];
                if (tr.isExpanded) {
                    let gtindex = allRows.findIndex(x => x.type === "task" && x.uid == "t_" + r.id);
                    if (gtindex !== -1) {
                        allRows[gtindex].hasChildren = true;
                        allRows[gtindex].raw.hasChildren = true;
                        allRows[gtindex].expanded = true;
                        allRows[gtindex].raw.expanded = true;
                    }
                }
            }
        });
        $scope.originalRows = allRows;
        return $scope.getVisibleGanttRows(allRows);
    };
    $scope.getVisibleGanttRows = function (allRows) {
        var rowMap = {};

        angular.forEach(allRows, function (r) {
            rowMap[r.uid] = r;
        });

        function isVisible(row) {
            var parentUid = row.parentUid;

            while (parentUid) {
                var parent = rowMap[parentUid];
                if (!parent) break;
                if (parent.expanded === false) return false;
                parentUid = parent.parentUid;
            }

            return true;
        }

        return allRows.filter(function (r) {
            return isVisible(r);
        });
    };


    //#region Gantt row soft drag and drop
    $scope.ganttDraggingRow = null;
    $scope.ganttDragOverRow = null;
    $scope.ganttInvalidDropRow = null;
    $scope.ganttReorderSaving = false;
    $scope.ganttReorderMessage = '';

    $scope.clearGanttSoftDragMarks = function () {
        $scope.ganttDragOverRow = null;
        $scope.ganttInvalidDropRow = null;
        try {
            var rows = document.querySelectorAll('.gantt-row-over,.gantt-row-before,.gantt-row-after,.gantt-row-invalid,.gantt-row-moving');
            angular.forEach(rows, function (r) {
                r.classList.remove('gantt-row-over');
                r.classList.remove('gantt-row-before');
                r.classList.remove('gantt-row-after');
                r.classList.remove('gantt-row-invalid');
                r.classList.remove('gantt-row-moving');
            });
        } catch (e) { }
    };

    $scope.canDropGanttRow = function (sourceRow, targetRow) {
        if (!sourceRow || !targetRow) return false;
        if (sourceRow.uid === targetRow.uid) return false;

        // Same hierarchy validation as table view:
        // 1) Milestone can be reordered only with milestone rows.
        // 2) Task can be reordered only with task rows inside the SAME milestone.
        // 3) Subtask can be reordered only with subtask rows inside the SAME task.
        if (sourceRow.type === "milestone") {
            return targetRow.type === "milestone";
        }

        if (sourceRow.type === "task") {
            return targetRow.type === "task" &&
                String(sourceRow.parentId || sourceRow.Milestoneid || sourceRow.milestoneid || "") ===
                String(targetRow.parentId || targetRow.Milestoneid || targetRow.milestoneid || "");
        }

        if (sourceRow.type === "subtask") {
            return targetRow.type === "subtask" &&
                String(sourceRow.parentId || sourceRow.Task || sourceRow.task || "") ===
                String(targetRow.parentId || targetRow.Task || targetRow.task || "");
        }

        return false;
    };

    $scope.startSoftGanttRowDrag = function ($event, sourceRow) {
        var e = $event && ($event.originalEvent || $event);
        if (!e || !sourceRow || !sourceRow.uid || $scope.ganttReorderSaving || !$scope.canEdit(sourceRow.type)) return;
        if (e.type === 'mousedown' && e.button !== 0) return;

        var p = getDragPoint(e);
        if (e.preventDefault) e.preventDefault();
        if (e.stopPropagation) e.stopPropagation();

        var startEl = closestGanttRow(e.target);
        $scope.ganttDraggingRow = sourceRow;
        $scope.ganttDragOverRow = null;
        $scope.ganttInvalidDropRow = null;

        var ghost = document.createElement('div');
        ghost.className = 'gantt-soft-ghost';
        ghost.innerHTML = '<i class="ri-drag-move-2-line"></i> ' + htmlEncode(sourceRow.name || sourceRow.type || 'Move row');
        document.body.appendChild(ghost);
        document.body.classList.add('gantt-soft-dragging');
        if (startEl) startEl.classList.add('gantt-row-moving');

        function getDragPoint(ev) {
            if (ev.touches && ev.touches.length) return { x: ev.touches[0].clientX, y: ev.touches[0].clientY };
            if (ev.changedTouches && ev.changedTouches.length) return { x: ev.changedTouches[0].clientX, y: ev.changedTouches[0].clientY };
            return { x: ev.clientX, y: ev.clientY };
        }
        function htmlEncode(v) {
            return String(v || '').replace(/[&<>"]/g, function (c) {
                return ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' })[c];
            });
        }
        function closestGanttRow(el) {
            while (el && el !== document) {
                if (el.getAttribute && el.getAttribute('data-gantt-row')) return el;
                el = el.parentNode;
            }
            return null;
        }
        function findRowByUid(uid) {
            var found = null;
            angular.forEach($scope.ganttRows || [], function (r) {
                if (!found && String(r.uid) === String(uid)) found = r;
            });
            return found;
        }
        function markPair(uid, cls) {
            var els = document.querySelectorAll('[data-gantt-row="' + String(uid).replace(/"/g, '\\"') + '"]');
            angular.forEach(els, function (el) { el.classList.add(cls); });
        }
        function clearDomMarks() {
            var rows = document.querySelectorAll('.gantt-row-over,.gantt-row-before,.gantt-row-after,.gantt-row-invalid,.gantt-row-moving');
            angular.forEach(rows, function (r) {
                r.classList.remove('gantt-row-over');
                r.classList.remove('gantt-row-before');
                r.classList.remove('gantt-row-after');
                r.classList.remove('gantt-row-invalid');
                r.classList.remove('gantt-row-moving');
            });
            if (sourceRow && sourceRow.uid) markPair(sourceRow.uid, 'gantt-row-moving');
        }
        function setGhost(x, y) {
            ghost.style.left = (x + 14) + 'px';
            ghost.style.top = (y + 14) + 'px';
        }

        var targetRow = null, dropAfter = false;
        function onMove(ev) {
            if (ev.cancelable !== false && ev.preventDefault) ev.preventDefault();
            var pt = getDragPoint(ev);
            setGhost(pt.x, pt.y);
            clearDomMarks();

            var el = document.elementFromPoint(pt.x, pt.y);
            var rowEl = closestGanttRow(el);
            targetRow = null;
            $scope.ganttDragOverRow = null;
            $scope.ganttInvalidDropRow = null;

            if (!rowEl) return;
            var uid = rowEl.getAttribute('data-gantt-row');
            var tr = findRowByUid(uid);
            if (!tr || tr.uid === sourceRow.uid) return;

            targetRow = tr;
            dropAfter = pt.y >= rowEl.getBoundingClientRect().top + rowEl.getBoundingClientRect().height / 2;
            if ($scope.canDropGanttRow(sourceRow, tr)) {
                markPair(tr.uid, 'gantt-row-over');
                markPair(tr.uid, dropAfter ? 'gantt-row-after' : 'gantt-row-before');
                if (ghost) ghost.classList.remove('invalid-drop');
                $scope.ganttDragOverRow = tr;
            } else {
                markPair(tr.uid, 'gantt-row-invalid');
                if (ghost) ghost.classList.add('invalid-drop');
                $scope.ganttInvalidDropRow = tr;
            }
        }

        function finish(ev) {
            if (ev && ev.cancelable !== false && ev.preventDefault) ev.preventDefault();
            document.removeEventListener('mousemove', onMove, true);
            document.removeEventListener('mouseup', finish, true);
            document.removeEventListener('touchmove', onMove, true);
            document.removeEventListener('touchend', finish, true);
            document.removeEventListener('touchcancel', finish, true);
            document.body.classList.remove('gantt-soft-dragging');
            if (ghost && ghost.parentNode) ghost.parentNode.removeChild(ghost);
            clearDomMarks();

            var dropTarget = targetRow, afterTarget = dropAfter;
            $scope.$applyAsync(function () {
                if (!$scope.ganttReorderSaving && dropTarget && $scope.canDropGanttRow(sourceRow, dropTarget)) {
                    var change = $scope.moveGanttSibling(sourceRow, dropTarget, afterTarget);
                    if (change) {
                        $scope.rebuildGanttAfterRowDrop();
                        $scope.saveGanttSiblingOrder(change);
                    }
                }
                $scope.ganttDraggingRow = null;
                $scope.clearGanttSoftDragMarks();
            });
        }

        setGhost(p.x, p.y);
        document.addEventListener('mousemove', onMove, true);
        document.addEventListener('mouseup', finish, true);
        document.addEventListener('touchmove', onMove, { capture: true, passive: false });
        document.addEventListener('touchend', finish, true);
        document.addEventListener('touchcancel', finish, true);
    };

    $scope.findGanttMilestoneIndex = function (mid) {
        if (!$scope.projectlist || !$scope.projectlist.miles) return -1;
        return $scope.projectlist.miles.findIndex(function (m) { return String(m.id) === String(mid); });
    };

    $scope.findGanttTaskLocation = function (tid) {
        var result = { mIndex: -1, tIndex: -1, milestone: null, task: null };
        angular.forEach(($scope.projectlist.miles || []), function (m, mi) {
            if (result.task) return;
            var ti = (m.tasks || []).findIndex(function (t) { return String(t.id) === String(tid); });
            if (ti !== -1) { result.mIndex = mi; result.tIndex = ti; result.milestone = m; result.task = m.tasks[ti]; }
        });
        return result;
    };

    $scope.findGanttSubtaskLocation = function (sid) {
        var result = { mIndex: -1, tIndex: -1, sIndex: -1, milestone: null, task: null, subtask: null };
        angular.forEach(($scope.projectlist.miles || []), function (m, mi) {
            if (result.subtask) return;
            angular.forEach((m.tasks || []), function (t, ti) {
                if (result.subtask) return;
                var si = (t.subtasks || []).findIndex(function (s) { return String(s.id) === String(sid); });
                if (si !== -1) { result.mIndex = mi; result.tIndex = ti; result.sIndex = si; result.milestone = m; result.task = t; result.subtask = t.subtasks[si]; }
            });
        });
        return result;
    };

    $scope.moveGanttMilestone = function (sourceRow, targetRow) {
        var list = $scope.projectlist.miles || [];
        var fromIndex = $scope.findGanttMilestoneIndex(sourceRow.id);
        var toIndex = $scope.findGanttMilestoneIndex(targetRow.id);
        if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;
        var item = list.splice(fromIndex, 1)[0];
        if (fromIndex < toIndex) toIndex--;
        list.splice(toIndex, 0, item);
        $scope.refreshOrderNo();
    };

    $scope.moveGanttTask = function (sourceRow, targetRow) {
        var from = $scope.findGanttTaskLocation(sourceRow.id);
        var target = $scope.findGanttTaskLocation(targetRow.id);
        if (!from.task || !target.task) return;
        if (String(from.milestone.id) !== String(target.milestone.id)) return;

        var list = from.milestone.tasks || [];
        var fromIndex = from.tIndex;
        var toIndex = target.tIndex;
        if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;

        var moved = list.splice(fromIndex, 1)[0];
        if (fromIndex < toIndex) toIndex--;
        list.splice(toIndex, 0, moved);
        $scope.refreshOrderNo_tasks(from.milestone);
    };

    $scope.moveGanttSubtask = function (sourceRow, targetRow) {
        var from = $scope.findGanttSubtaskLocation(sourceRow.id);
        var target = $scope.findGanttSubtaskLocation(targetRow.id);
        if (!from.subtask || !target.subtask) return;
        if (String(from.task.id) !== String(target.task.id)) return;

        var list = from.task.subtasks || [];
        var fromIndex = from.sIndex;
        var toIndex = target.sIndex;
        if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;

        var moved = list.splice(fromIndex, 1)[0];
        if (fromIndex < toIndex) toIndex--;
        list.splice(toIndex, 0, moved);
        $scope.refreshOrderNo_subtasks(from.task);
    };

    // Reorder only the sibling array. Keeping the original array makes a failed
    // multi-row save recoverable without changing the task/milestone hierarchy.
    $scope.moveGanttSibling = function (sourceRow, targetRow, afterTarget) {
        var list;
        if (sourceRow.type === 'milestone') list = $scope.projectlist.miles;
        else if (sourceRow.type === 'task') {
            var taskLocation = $scope.findGanttTaskLocation(sourceRow.id);
            list = taskLocation.milestone && taskLocation.milestone.tasks;
        } else {
            var subtaskLocation = $scope.findGanttSubtaskLocation(sourceRow.id);
            list = subtaskLocation.task && subtaskLocation.task.subtasks;
        }
        if (!list || !$scope.canDropGanttRow(sourceRow, targetRow)) return null;
        var from = list.findIndex(function (x) { return String(x.id) === String(sourceRow.id); });
        var to = list.findIndex(function (x) { return String(x.id) === String(targetRow.id); });
        if (from < 0 || to < 0 || from === to) return null;
        var original = list.slice();
        var originalOrder = {};
        original.forEach(function (item, i) { originalOrder[String(item.id)] = item.orderno == null ? i + 1 : item.orderno; });
        var insert = to + (afterTarget ? 1 : 0);
        var moved = list.splice(from, 1)[0];
        if (from < insert) insert--;
        list.splice(insert, 0, moved);
        if (list.every(function (x, i) { return x === original[i]; })) return null;
        list.forEach(function (x, i) { x.orderno = i + 1; });
        return { list: list, original: original, originalOrder: originalOrder, type: sourceRow.type };
    };

    $scope.saveGanttSiblingOrder = function (change) {
        $scope.ganttReorderSaving = true;
        var save = change.type === 'milestone' ? assignprojectService.updateorderno_milestone
            : (change.type === 'task' ? assignprojectService.updateorderno_task : assignprojectService.updateorderno_subtask);
        var modified = change.list.filter(function (item, i) {
            return item !== change.original[i];
        });
        var originalNumbers = change.originalOrder;
        var sequence = Promise.resolve();
        modified.forEach(function (item) {
            sequence = sequence.then(function () {
                return save(item.id, item.orderno).then(function (response) {
                    if (!response || !response.data || Number(response.data.d) !== 1) throw new Error('Order was not saved');
                });
            });
        });
        sequence.then(function () {
            $scope.$applyAsync(function () {
                $scope.ganttReorderSaving = false;
                $scope.ganttReorderMessage = 'Order saved';
                $timeout(function () { $scope.ganttReorderMessage = ''; }, 2500);
            });
        }).catch(function (error) {
            console.error('Gantt reorder failed', error);
            // Restore the screen immediately; also restore database values for
            // requests that may have completed before the failure.
            $scope.$applyAsync(function () {
                change.list.splice.apply(change.list, [0, change.list.length].concat(change.original));
                change.list.forEach(function (item) { item.orderno = originalNumbers[String(item.id)]; });
                $scope.rebuildGanttAfterRowDrop();
            });
            Promise.all(modified.map(function (item) {
                return save(item.id, originalNumbers[String(item.id)]).then(function (response) {
                    return !!(response && response.data && Number(response.data.d) === 1);
                }, function () { return false; });
            })).then(function (restored) {
                $scope.$applyAsync(function () {
                    $scope.ganttReorderSaving = false;
                    $scope.ganttReorderMessage = restored.every(Boolean)
                        ? 'Could not save order. Previous order restored.'
                        : 'Could not save order. Reload to confirm database order.';
                    $timeout(function () { $scope.ganttReorderMessage = ''; }, 4500);
                });
            });
        });
    };

    $scope.rebuildGanttAfterRowDrop = function () {
        var expandedMap = {};
        angular.forEach($scope.originalRows || [], function (r) { expandedMap[r.uid] = r.expanded; });
        var allRows = $scope.buildGanttRows();
        angular.forEach(allRows, function (r) { if (expandedMap.hasOwnProperty(r.uid)) r.expanded = expandedMap[r.uid]; });
        $scope.originalRows = allRows;
        $scope.ganttRows = $scope.getVisibleGanttRows(allRows);
        $scope.buildGanttTimeline();
        $timeout(function () { initTableUI(); }, 0);
    };
    //#endregion Gantt row soft drag and drop

    $scope.originalRows = [];
    function setupGanttVerticalScrollSync() {
        var root = document.getElementById('pmsGanttRoot');
        if (!root) return;
        var left = root.querySelector('.pms-gantt-left');
        var right = root.querySelector('.pms-gantt-right');
        if (!left || !right || left._pmsGanttScrollSyncBound) return;

        var syncing = false;
        function sync(source, target) {
            if (syncing || target.scrollTop === source.scrollTop) return;
            syncing = true;
            target.scrollTop = source.scrollTop;
            window.requestAnimationFrame(function () { syncing = false; });
        }
        left.addEventListener('scroll', function () { sync(left, right); }, { passive: true });
        right.addEventListener('scroll', function () { sync(right, left); }, { passive: true });
        left._pmsGanttScrollSyncBound = true;
        right._pmsGanttScrollSyncBound = true;
    }
    function normalizeGanttScale(type) {
        var value = String(type || 'day').toLowerCase().replace(/s$/, '');
        return ['year', 'quarter', 'month', 'week', 'day'].indexOf(value) >= 0 ? value : 'day';
    }

    function scrollGanttToFirstScheduledRow() {
        $timeout(function () {
            var right = document.querySelector('.pms-gantt-right');
            if (!right || !$scope.ganttStart) return;
            var firstLeft = Infinity;
            angular.forEach($scope.ganttRows || [], function (row) {
                if (!row.start || !row.end) return;
                var position = parseFloat($scope.getBarStyle(row).left);
                if (isFinite(position) && position >= 0) firstLeft = Math.min(firstLeft, position);
            });
            if (isFinite(firstLeft)) right.scrollLeft = Math.max(0, firstLeft - Math.min(120, right.clientWidth * 0.15));
        }, 80);
    }

    $scope.changeGanttScale = function (type) {
        $scope.ganttIsLoading = true;
        $scope.ganttScale = normalizeGanttScale(type);
        ganttAutoScrollPending = true;
        $scope.ganttPrependCount = 0;
        $scope.ganttPostpendCount = 0;
        $scope.isLoading = true;
        // A scale change only changes the timeline resolution. Rebuilding the
        // hierarchy here reset milestone/task expansion to its default state,
        // so already-visible subtasks disappeared in Quarter/Year views.
        // Preserve the current visible rows and only recalculate the timeline.
        if (($scope.originalRows || []).length) {
            // Recreate the visible collection from the authoritative complete
            // hierarchy. This prevents stale states such as a collapsed
            // milestone still showing its task, or an expanded task missing
            // its subtasks, when Year/Quarter is selected.
            $scope.ganttRows = $scope.getVisibleGanttRows($scope.originalRows);
            $scope.buildGanttTimeline();
        } else {
            $scope.buildGantt();
        }
        $scope.isLoading = false;
        scrollGanttToFirstScheduledRow();
        $timeout(function () {
            initTableUI();
            setupGanttVerticalScrollSync();
            $scope.ganttIsLoading = false;
        }, 50);
    };
    $scope.$watch('ganttRows', function () {
        $scope.buildGanttDependencyPaths();
        setTimeout(function () {
            initTableUI();
            setupGanttVerticalScrollSync();
        }, 300);
    }, true);

    $scope.toggleGanttRow = function (row, $event) {
        if ($event) $event.stopPropagation();

        var nextExpanded = !row.expanded;
        var expandedMap = {};
        angular.forEach(($scope.originalRows || []).concat($scope.ganttRows || []), function (existingRow) {
            expandedMap[existingRow.uid] = existingRow.expanded !== false;
        });
        expandedMap[row.uid] = nextExpanded;

        row.expanded = nextExpanded;
        // Keep the hierarchy state on the existing source object as well.
        // This prevents Quarter/Year timeline rebuilds from restoring the
        // milestone/task to its old state and hiding its child subtasks.
        if (row.raw) {
            row.raw._ganttExpandedTouched = true;
            row.raw.isExpanded = row.expanded;
            row.raw.expanded = row.expanded;
        }

        // Rebuild the complete hierarchy from projectlist before filtering.
        // Subtasks can arrive after the first Gantt render, leaving
        // originalRows stale and causing the Task arrow to disappear.
        var refreshedRows = $scope.buildGanttRows();
        angular.forEach(refreshedRows, function (freshRow) {
            freshRow.expanded = expandedMap.hasOwnProperty(freshRow.uid)
                ? expandedMap[freshRow.uid]
                : true;
        });
        $scope.originalRows = refreshedRows;
        $scope.ganttRows = $scope.getVisibleGanttRows(refreshedRows);

        $scope.buildGanttTimeline();
        $scope.buildGanttDependencyPaths();

        if (!$scope.$$phase) {
            $scope.$applyAsync();
        }
    };
    $scope.toggleGanttRowold = function (row, $event) {
        if ($event) $event.stopPropagation();
        row.expanded = !row.expanded;
        var allRows = $scope.buildGanttRows();
        var collapseMap = {};
        function isVisible(r) {
            var p = r.parentUid;
            while (p) {
                if (collapseMap[p] === true) return false;
                var parent = null;
                for (var i = 0; i < allRows.length; i++) {
                    if (allRows[i].uid === p) {
                        parent = allRows[i];
                        break;
                    }
                }
                p = parent ? parent.parentUid : null;
            }
            return true;
        }

        angular.forEach(allRows, function (r) {
            if (r.uid === row.uid) {
                r.expanded = row.expanded;
            }
        });

        angular.forEach(allRows, function (r) {
            if (!r.expanded) collapseMap[r.uid] = true;
        });

        $scope.ganttRows = allRows.filter(function (r) {
            return isVisible(r);
        });

        $scope.buildGanttTimeline();
    };


    $scope.getBarStyle = function (row) {
        if (!row.start || !row.end || !$scope.ganttStart) return {};

        var leftIndex = 0;
        var widthUnits = 1;

        if ($scope.ganttScale === "month") {
            leftIndex = (row.start.getFullYear() - $scope.ganttStart.getFullYear()) * 12 +
                (row.start.getMonth() - $scope.ganttStart.getMonth());

            widthUnits = ((row.end.getFullYear() - row.start.getFullYear()) * 12) +
                (row.end.getMonth() - row.start.getMonth()) + 1;
        }
        else if ($scope.ganttScale === "week11") {
            leftIndex = Math.floor($scope.diffDays(row.start, $scope.ganttStart) / 7);
            widthUnits = Math.floor($scope.diffDays(row.end, row.start) / 7) + 1;
        }
        else if ($scope.ganttScale === "week") {

            var startIndex = $scope.getMonthWeekIndex(row.start);

            var endIndex = $scope.getMonthWeekIndex(row.end);

            leftIndex = startIndex;

            widthUnits = (endIndex - startIndex) + 1;
        }
        else {
            leftIndex = $scope.diffDays(row.start, $scope.ganttStart);
            widthUnits = $scope.diffDays(row.end, row.start) + 1;
        }

        if (widthUnits < 1) widthUnits = 1;
        var left = (leftIndex * $scope.ganttCellWidth);

        var width = (widthUnits * $scope.ganttCellWidth);

        // total gantt width
        var maxWidth = $scope.ganttCells.length * $scope.ganttCellWidth;

        // STOP OVERFLOW RIGHT
        if ((left + width) > maxWidth) {

            width = maxWidth - left;
        }

        // STOP NEGATIVE
        if (width < $scope.ganttCellWidth) {

            width = $scope.ganttCellWidth;
        }

        return {
            left: left + "px",
            width: width + "px"
        };
        //return {
        //    left: (leftIndex * $scope.ganttCellWidth) + "px",
        //    width: (widthUnits * $scope.ganttCellWidth) + "px"
        //};
    };

    // Gantt-only UI state. Existing table/timesheet/calendar data is never mutated here
    // except for the same TaskDuration fields used by the existing duration editor.
    $scope.ganttLeftWidth = 560;

    // Gantt-only row selection. Kept in a UID map so selection survives
    // Quarter/Year rebuilds and collapse/expand without changing raw records.
    $scope.ganttSelectedMap = {};

    $scope.isGanttRowSelected = function (row) {
        return !!(row && $scope.ganttSelectedMap[row.uid]);
    };

    $scope.getGanttSelectedCount = function () {
        return Object.keys($scope.ganttSelectedMap || {}).filter(function (uid) {
            return $scope.ganttSelectedMap[uid] === true;
        }).length;
    };

    $scope.toggleGanttRowSelection = function (row, $event) {
        if ($event) {
            $event.stopPropagation();
        }
        if (!row || !row.uid || row.type !== 'subtask') return;
        if ($scope.ganttSelectedMap[row.uid]) delete $scope.ganttSelectedMap[row.uid];
        else $scope.ganttSelectedMap[row.uid] = true;
    };

    $scope.areAllVisibleGanttRowsSelected = function () {
        var rows = ($scope.ganttRows || []).filter(function (row) {
            return row && row.type === 'subtask';
        });
        return rows.length > 0 && rows.every(function (row) {
            return $scope.isGanttRowSelected(row);
        });
    };

    $scope.toggleAllVisibleGanttRows = function ($event) {
        if ($event) {
            $event.stopPropagation();
        }
        var selectAll = !$scope.areAllVisibleGanttRowsSelected();
        angular.forEach($scope.ganttRows || [], function (row) {
            if (!row || !row.uid || row.type !== 'subtask') return;
            if (selectAll) $scope.ganttSelectedMap[row.uid] = true;
            else delete $scope.ganttSelectedMap[row.uid];
        });
    };

    $scope.clearGanttSelection = function ($event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        $scope.ganttSelectedMap = {};
    };

    $scope.getGanttSelectedRows = function () {
        var rows = ($scope.originalRows && $scope.originalRows.length)
            ? $scope.originalRows
            : ($scope.ganttRows || []);
        return rows.filter(function (row) {
            return $scope.isGanttRowSelected(row);
        });
    };

    // UI hook only: exposes the requested action and selected rows without
    // mutating data. Existing single-row move/delete/owner/status/tag flows
    // remain untouched until each bulk operation is connected explicitly.
    $scope.ganttBulkAction = function (action, $event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        $scope.ganttBulkRequestedAction = action;
        $scope.ganttBulkRequestedRows = $scope.getGanttSelectedRows();
        $scope.$emit('pmsGanttBulkActionRequested', {
            action: action,
            rows: $scope.ganttBulkRequestedRows
        });
    };

    // Resolve a visible Gantt row back to the original hierarchy object.
    // All edits below deliberately reuse the existing List-view handlers.
    function getGanttHierarchyContext(row) {
        if (!row || !row.raw) return null;
        var miles = ($scope.projectlist && $scope.projectlist.miles) || [];
        var context = { row: row, item: row.raw, type: row.type, mile: null, task: null, mileIndex: -1, taskIndex: -1, subtaskIndex: -1 };

        if (row.type === 'milestone') {
            context.mileIndex = miles.findIndex(function (m) { return String(m.id) === String(row.id); });
            context.mile = context.mileIndex > -1 ? miles[context.mileIndex] : row.raw;
            context.item = context.mile;
            return context;
        }

        context.mileIndex = miles.findIndex(function (m) {
            return String(m.id) === String(row.Milestoneid || (row.raw && row.raw.Milestoneid));
        });
        context.mile = context.mileIndex > -1 ? miles[context.mileIndex] : null;

        if (row.type === 'task') {
            var tasks = (context.mile && context.mile.tasks) || [];
            context.taskIndex = tasks.findIndex(function (t) { return String(t.id) === String(row.id); });
            context.task = context.taskIndex > -1 ? tasks[context.taskIndex] : row.raw;
            context.item = context.task;
            return context;
        }

        var parentTasks = (context.mile && context.mile.tasks) || [];
        context.taskIndex = parentTasks.findIndex(function (t) {
            return String(t.id) === String(row.Task || (row.raw && (row.raw.Task || row.raw.task)));
        });
        context.task = context.taskIndex > -1 ? parentTasks[context.taskIndex] : null;
        var subtasks = (context.task && context.task.subtasks) || [];
        context.subtaskIndex = subtasks.findIndex(function (s) { return String(s.id) === String(row.id); });
        context.item = context.subtaskIndex > -1 ? subtasks[context.subtaskIndex] : row.raw;
        return context;
    }

    // Gantt row menus use Angular state because the Gantt grid stops and
    // manages its own pointer events for resize, selection and popup actions.
    $scope.ganttOpenRowMenu = null;

    $scope.toggleGanttRowMenu = function (row, menu, $event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        var key = row && row.uid ? row.uid + ':' + menu : null;
        $scope.ganttOpenRowMenu = $scope.ganttOpenRowMenu === key ? null : key;
    };

    $scope.isGanttRowMenuOpen = function (row, menu) {
        return !!(row && row.uid && $scope.ganttOpenRowMenu === row.uid + ':' + menu);
    };

    $scope.isGanttAnyRowMenuOpen = function (row) {
        return $scope.isGanttRowMenuOpen(row, 'add') || $scope.isGanttRowMenuOpen(row, 'more');
    };

    $scope.closeGanttRowMenu = function () {
        $scope.ganttOpenRowMenu = null;
    };

    var ganttOutsideMenuClick = function (event) {
        var target = event && event.target;
        if (target && target.closest && target.closest('.pms-gantt-row-actions')) return;
        if (!$scope.ganttOpenRowMenu) return;
        $scope.$applyAsync(function () {
            $scope.closeGanttRowMenu();
        });
    };

    angular.element(document).on('click', ganttOutsideMenuClick);
    $scope.$on('$destroy', function () {
        angular.element(document).off('click', ganttOutsideMenuClick);
    });

    $scope.updateGanttRowName = function (row) {
        var context = getGanttHierarchyContext(row);
        if (!context || !$scope.canEdit(row.type)) return;
        if (row.type === 'milestone') {
            context.item.Milestone = row.name;
            $scope.updatedetails_mile(context.item, 'Milestone');
        } else if (row.type === 'task') {
            context.item.taskname = row.name;
            $scope.updatedetails_task(context.item, 'taskname');
        } else {
            context.item.Subtaskname = row.name;
            $scope.updatedetails_subtask(context.item, 'Subtaskname');
        }
    };

    $scope.hideGanttNameTooltip = function ($event) {
        var element = $event && $event.currentTarget;
        if (element && window.bootstrap && bootstrap.Tooltip) {
            var tooltip = bootstrap.Tooltip.getInstance(element);
            if (tooltip) tooltip.hide();
        }
    };

    $scope.selectGanttStatus = function (row, newstatus, $event) {
        if ($event) { $event.preventDefault(); $scope.closeListActionDropdown($event); }
        var context = getGanttHierarchyContext(row);
        if (!context || !$scope.canEdit(row.type)) return;
        var statusUpdate = $scope.selectStatus(context.item, newstatus, row.type);
        if (!statusUpdate || !statusUpdate.then) return statusUpdate;
        return statusUpdate.then(function () {
            var savedStatusId = context.item.P_Status !== undefined ? context.item.P_Status :
                (context.item.statusid !== undefined ? context.item.statusid : context.item.StatusId);
            if (String(savedStatusId) !== String(newstatus.id)) return;
            row.status_id = newstatus.id;
            row.status_name = newstatus.name;
            row.status_color = newstatus.colorname;
        });
    };

    $scope.openGanttAssignEditor = function (row, $event) {
        if ($event) $event.stopPropagation();
        var context = getGanttHierarchyContext(row);
        if (!context || !$scope.canAssign(row.type)) return;
        var index = row.type === 'milestone' ? context.mileIndex : (row.type === 'task' ? context.taskIndex : context.subtaskIndex);
        $scope.openAssignTask(context.item, row.type, index);
    };

    $scope.ganttAddRow = async function (row, direction) {
        $scope.closeGanttRowMenu();
        var context = getGanttHierarchyContext(row);
        if (!context) return;
        if (row.type === 'milestone') await $scope.addRowUpDown_mile(context.mileIndex, context.item, direction);
        else if (row.type === 'task') await $scope.addRowUpDown_task(context.taskIndex, context.mile, context.item, direction);
        else await $scope.addRowUpDown_subtask(context.subtaskIndex, context.task, context.item, direction);
        if ($scope.view === 'gantt') $scope.rebuildGanttAfterRowDrop();
    };

    $scope.ganttAddChild = async function (row) {
        $scope.closeGanttRowMenu();
        var context = getGanttHierarchyContext(row);
        if (!context) return;
        if (row.type === 'milestone') await $scope.addRowUpDown_taskdefault(context.item);
        else if (row.type === 'task') await $scope.addRowUpDown_subtaskdefault(context.item);
        if ($scope.view === 'gantt') $scope.rebuildGanttAfterRowDrop();
    };

    $scope.ganttUploadDocuments = function (row) {
        $scope.closeGanttRowMenu();
        var context = getGanttHierarchyContext(row);
        if (!context || !$scope.canEdit(row.type)) return;
        var index = row.type === 'milestone' ? context.mileIndex : (row.type === 'task' ? context.taskIndex : context.subtaskIndex);
        $scope.loadSrcFiles(context.item, row.type, index);
    };

    $scope.ganttEditDetails = function (row) {
        $scope.closeGanttRowMenu();
        var context = getGanttHierarchyContext(row);
        if (!context || !$scope.canEdit(row.type)) return;
        var url = '';
        var title = '';
        if (row.type === 'milestone') {
            url = '/pms/editmile.aspx?id=' + context.item.id + '&pid=' + context.item.ProjectId + '&ctype=ang';
            title = 'Milestone';
        } else if (row.type === 'task') {
            url = '/pms/edittask.aspx?id=' + context.item.id + '&mid=' + context.item.Milestoneid + '&ctype=ang';
            title = 'Task';
        } else {
            url = '/pms/addnewsubtask.aspx?id=' + context.item.id + '&tid=' + context.item.Task + '&ctype=ang';
            title = 'Sub Task';
        }
        $scope.loadformedit(context.item, row.type, 'editdetails', url, title);
    };

    $scope.ganttCloneRow = function (row) {
        $scope.ganttAddRow(row, 'down');
    };

    $scope.ganttDeleteRow = function (row) {
        $scope.closeGanttRowMenu();
        var context = getGanttHierarchyContext(row);
        if (!context) return;
        if (row.type === 'milestone') $scope.deletedatamile(context.mileIndex, context.item);
        else if (row.type === 'task') $scope.deletedatatask(context.taskIndex, context.item, context.mile);
        else $scope.deletedatasubtask(context.subtaskIndex, context.item, context.task);
    };

    $scope.getGanttOwnerTitle = function (row) {
        return ((row && row.owners) || []).map(function (x) { return x.name || ''; }).filter(Boolean).join(', ');
    };

    // Kept as a separate public helper because the Gantt template uses this
    // name for both the owner tooltip and the task-detail popup.
    $scope.getGanttOwnerNames = function (row) {
        return $scope.getGanttOwnerTitle(row);
    };

    $scope.getGanttDurationText = function (row) {
        var src = row && row.raw ? row.raw : {};
        return $scope.getTaskDurationText(src) || '0:00 hrs';
    };

    // Gantt-only subtask detail popup. Existing list/kanban popup and save
    // behavior are intentionally reused and remain unchanged.
    $scope.ganttDetailOpen = false;
    $scope.ganttDetailRow = {};
    $scope.ganttDependencyView = 'modern';
    $scope.ganttModernDependencyChoice = null;
    $scope.ganttDependencyTab = 'predecessor';
    $scope.ganttDependencyRows = [];
    $scope.ganttChartLinks = [];
    $scope.ganttDependencyPaths = [];
    var ganttChartLinkRequest = 0;
    $scope.loadGanttChartDependencies = function () {
        var row = ($scope.originalRows || $scope.ganttRows || [])[0];
        var requestId = ++ganttChartLinkRequest;
        if (!row || !row.id) {
            $scope.ganttChartLinks = [];
            $scope.ganttDependencyPaths = [];
            return;
        }
        assignprojectService.ganttDependency('listall', ganttDependencyPayload(row)).then(function (res) {
            if (requestId !== ganttChartLinkRequest || !res.data || !res.data.d || res.data.d.error) return;
            $scope.ganttChartLinks = res.data.d.rows || [];
            $scope.buildGanttDependencyPaths();
        }).catch(function () {
            if (requestId === ganttChartLinkRequest) {
                $scope.ganttChartLinks = [];
                $scope.ganttDependencyPaths = [];
            }
        });
    };
    $scope.buildGanttDependencyPaths = function () {
        var rows = $scope.ganttRows || [], byKey = {}, paths = [];
        rows.forEach(function (row, index) { byKey[row.type + ':' + row.id] = { row: row, index: index }; });
        ($scope.ganttChartLinks || []).forEach(function (link) {
            var from = byKey[link.PredecessorType + ':' + link.PredecessorId];
            var to = byKey[link.SuccessorType + ':' + link.SuccessorId];
            if (!from || !to || !from.row.start || !from.row.end || !to.row.start || !to.row.end) return;
            var fromStyle = $scope.getGanttDisplayBarStyle(from.row), toStyle = $scope.getGanttDisplayBarStyle(to.row);
            var type = String(link.LinkType || 'FS').toUpperCase();
            var fromStart = parseFloat(fromStyle.left), fromWidth = parseFloat(fromStyle.width);
            var toStart = parseFloat(toStyle.left), toWidth = parseFloat(toStyle.width);
            if (![fromStart, fromWidth, toStart, toWidth].every(isFinite)) return;
            var x1 = fromStart + (type.charAt(0) === 'F' ? fromWidth : 0);
            var x2 = toStart + (type.charAt(1) === 'F' ? toWidth : 0);
            var y1 = from.index * 35 + 18, y2 = to.index * 35 + 18;
            var bend = x2 >= x1 + 24 ? x1 + Math.min(18, (x2 - x1) / 2) : Math.max(x1, x2) + 18;
            var approach = x2 - 9;
            paths.push({ id: link.Id, type: type,
                d: 'M ' + x1 + ' ' + y1 + ' H ' + bend + ' V ' + y2 + ' H ' + approach + ' L ' + x2 + ' ' + y2 });
        });
        $scope.ganttDependencyPaths = paths;
    };
    $scope.ganttDependencyBusy = false;
    $scope.ganttDependencyError = '';
    $scope.ganttAddingDependency = false;
    $scope.ganttEditingDependency = null;
    $scope.ganttEditingDependencyItem = null;
    $scope.ganttDependencyForm = { other: '', linkType: 'FS', lagHours: 0, search: '' };
    $scope.ganttDependencyChoicesOpen = false;
    $scope.ganttDependencyChoicesStyle = {};
    $scope.ganttDependencyHighlighted = 0;
    function closeGanttChoicesOnOutsideClick(event) {
        if (!$scope.ganttDetailOpen || !$scope.ganttDependencyChoicesOpen) return;
        var target = event.target;
        if (target && target.closest &&
            (target.closest('.pms-dependency-search-wrap') ||
             target.closest('.pms-gantt-dependency-grid .pms-dependency-choices') ||
             target.closest('.pms-gantt-modern-choices-portal'))) return;
        $scope.$applyAsync(function () { $scope.ganttDependencyChoicesOpen = false; });
    }
    $document.on('mousedown', closeGanttChoicesOnOutsideClick);
    function closeGanttMenuOnOutsideClick(event) {
        if (!$scope.ganttDependencyMenuId) return;
        var target = event.target;
        if (target && target.closest &&
            (target.closest('.pms-gantt-floating-menu') || target.closest('.pms-dependency-row-actions'))) return;
        $scope.$applyAsync(function () { $scope.ganttDependencyMenuId = null; $scope.ganttDependencyMenuLink = null; });
    }
    $document.on('mousedown', closeGanttMenuOnOutsideClick);
    $scope.$on('$destroy', function () {
        $document.off('mousedown', closeGanttChoicesOnOutsideClick);
        $document.off('mousedown', closeGanttMenuOnOutsideClick);
    });
    $scope.ganttDependencyNotice = '';
    var ganttDependencyNoticeTimer = null;
    function clearGanttDependencyNotice() {
        if (ganttDependencyNoticeTimer) $timeout.cancel(ganttDependencyNoticeTimer);
        ganttDependencyNoticeTimer = null;
        $scope.ganttDependencyNotice = '';
    }
    function showGanttDependencyNotice(message) {
        clearGanttDependencyNotice();
        $scope.ganttDependencyNotice = message;
        ganttDependencyNoticeTimer = $timeout(function () {
            $scope.ganttDependencyNotice = '';
            ganttDependencyNoticeTimer = null;
        }, 3000);
    }
    $scope.$on('$destroy', clearGanttDependencyNotice);
    $scope.ganttDependencyMenuId = null;
    $scope.ganttDependencyMenuLink = null;
    $scope.ganttDependencyMenuStyle = {};
    $scope.toggleGanttDependencyMenu = function (link, event) {
        if (event) event.stopPropagation();
        if ($scope.ganttDependencyMenuId === link.Id) {
            $scope.ganttDependencyMenuId = null;
            $scope.ganttDependencyMenuLink = null;
            return;
        }
        var rect = event.currentTarget.getBoundingClientRect();
        var menuWidth = 144;
        var menuHeight = 82;
        var gap = 4;
        var spaceBelow = Math.max(0, window.innerHeight - 8 - rect.bottom - gap);
        var spaceAbove = Math.max(0, rect.top - 8 - gap);
        var opensBelow = spaceBelow >= menuHeight || spaceBelow >= spaceAbove;
        var availableSpace = opensBelow ? spaceBelow : spaceAbove;
        var visibleMenuHeight = Math.min(menuHeight, availableSpace);
        var left = Math.max(8, Math.min(rect.left, window.innerWidth - menuWidth - 8));
        var top = opensBelow ? rect.bottom + gap : rect.top - visibleMenuHeight - gap;
        $scope.ganttDependencyMenuStyle = {
            position: 'fixed',
            left: left + 'px',
            top: Math.max(8, Math.min(top, window.innerHeight - visibleMenuHeight - 8)) + 'px',
            maxHeight: Math.max(48, visibleMenuHeight) + 'px',
            overflowY: 'auto'
        };
        $scope.ganttDependencyMenuLink = link;
        $scope.ganttDependencyMenuId = link.Id;
    };
    $scope.runGanttDependencyMenuAction = function (action) {
        var link = $scope.ganttDependencyMenuLink;
        $scope.ganttDependencyMenuId = null;
        $scope.ganttDependencyMenuLink = null;
        if (!link) return;
        if (action === 'view') $scope.viewGanttDependencyItem(link);
        else if (action === 'edit') $scope.beginEditGanttDependency(link);
        else if (action === 'delete') $scope.removeGanttDependency(link);
    };
    $scope.beginEditGanttDependency = function (link) {
        if (!link || $scope.ganttDependencyBusy) return;
        $scope.ganttEditingDependency = link;
        $scope.ganttEditingDependencyItem = $scope.getGanttDependencyItem(link);
        $scope.ganttAddingDependency = true;
        $scope.ganttModernDependencyChoice = null;
        $scope.ganttDependencyForm.other = '';
        $scope.ganttDependencyForm.search = '';
        $scope.ganttDependencyForm.linkType = link.LinkType || 'FS';
        $scope.ganttDependencyForm.lagHours = Number(link.LagHours || 0);
        $scope.ganttDependencyChoicesOpen = false;
        $scope.ganttDependencyError = '';
    };
    $scope.ganttSuppressPopupUntil = 0;

    // The redesigned dependency view is optional; the original popup remains
    // available and both views use the same dependency data and save actions.
    $scope.setGanttDependencyView = function (view) {
        $scope.ganttDependencyView = view === 'modern' ? 'modern' : 'classic';
        $scope.ganttDependencyChoicesOpen = false;
        $scope.ganttDependencyMenuId = null;
        $scope.ganttDependencyMenuLink = null;
    };

    $scope.chooseModernGanttDependency = function (row) {
        if (!row || $scope.ganttDependencyBusy) return;
        $scope.ganttModernDependencyChoice = row;
        $scope.ganttDependencyForm.other = row.type + ':' + row.id;
        $scope.ganttDependencyForm.search = [row.serialNo, row.name].filter(Boolean).join(' ');
        $scope.ganttDependencyChoicesOpen = false;
        $scope.ganttDependencyHighlighted = 0;
    };

    $scope.saveModernGanttDependency = function () {
        if ($scope.ganttEditingDependency) {
            var editLink = $scope.ganttEditingDependency;
            var editLag = Number($scope.ganttDependencyForm.lagHours);
            if (!Number.isInteger(editLag)) {
                $scope.ganttDependencyError = 'Lag must be a whole number of hours.';
                return;
            }
            $scope.ganttDependencyBusy = true;
            $scope.ganttDependencyError = '';
            return assignprojectService.updateGanttDependency({
                projectId: Number(editLink.ProjectId || $scope.ganttDetailRow.ProjectId || (($scope.projectlist || {}).id) || 0),
                linkId: Number(editLink.Id),
                linkType: $scope.ganttDependencyForm.linkType || 'FS',
                lagHours: editLag
            }).then(function (res) {
                if (!res || !res.data || !res.data.d || res.data.d.error) {
                    throw new Error((res && res.data && res.data.d && res.data.d.error) || 'Could not update dependency.');
                }
                showGanttDependencyNotice('Dependency updated');
                $scope.ganttAddingDependency = false;
                $scope.ganttEditingDependency = null;
                $scope.ganttEditingDependencyItem = null;
                $scope.ganttDependencyForm = { other: '', linkType: 'FS', lagHours: 0, search: '' };
                $scope.loadGanttChartDependencies();
                return $scope.refreshGanttDependencies();
            }).catch(function (error) {
                $scope.ganttDependencyError = (error.data && error.data.Message) || error.message || 'Could not update dependency.';
            }).finally(function () { $scope.ganttDependencyBusy = false; });
        }
        if (!$scope.ganttModernDependencyChoice || !$scope.ganttDependencyForm.other) {
            $scope.ganttDependencyError = 'Select an item.';
            return;
        }
        $scope.saveGanttDependency();
    };

    function ganttDependencyPayload(row) {
        return { projectId: Number(row.ProjectId || (row.raw && row.raw.ProjectId) || (($scope.projectlist || {}).id) || 0),
            itemType: row.type, itemId: Number(row.id), otherType: '', otherId: 0,
            relation: $scope.ganttDependencyTab, linkType: 'FS', lagHours: 0, linkId: 0 };
    }

    function ganttDependencyKey(type, id) {
        return String(type || '').toLowerCase() + ':' + String(id == null ? '' : id);
    }

    $scope.getGanttDependencyChoices = function () {
        var selected = $scope.ganttDetailRow;
        var projectId = selected && selected.ProjectId;
        if (!selected) return [];

        var selectedKey = ganttDependencyKey(selected.type, selected.id);
        var alreadyLinkedKeys = [];
        // A dependency is unique for this selected item across both tabs.
        // Exclude the other endpoint whether the selected item is currently
        // the predecessor or the successor.
        ($scope.ganttChartLinks || []).forEach(function (link) {
            var predecessorKey = ganttDependencyKey(link.PredecessorType, link.PredecessorId);
            var successorKey = ganttDependencyKey(link.SuccessorType, link.SuccessorId);
            if (predecessorKey === selectedKey) alreadyLinkedKeys.push(successorKey);
            if (successorKey === selectedKey) alreadyLinkedKeys.push(predecessorKey);
        });

        return ($scope.originalRows || []).filter(function (r) {
            if (!r || String(r.ProjectId) !== String(projectId)) return false;
            var candidateKey = ganttDependencyKey(r.type, r.id);
            return candidateKey !== selectedKey && alreadyLinkedKeys.indexOf(candidateKey) < 0;
        });
    };

    $scope.getFilteredGanttDependencyChoices = function () {
        var query = String($scope.ganttDependencyForm.search || '').toLowerCase().trim();
        var choices = $scope.getGanttDependencyChoices();
        var selected = $scope.ganttModernDependencyChoice;
        var selectedLabel = selected ? [selected.serialNo, selected.name].filter(Boolean).join(' ').toLowerCase().trim() : '';
        if ($scope.ganttDependencyView === 'modern' && selected && query === selectedLabel) {
            return choices.filter(function (r) { return r.type === selected.type && String(r.id) === String(selected.id); });
        }
        return choices.filter(function (r) {
            var name = String(r.name || '').toLowerCase();
            var number = String(r.serialNo || '').toLowerCase();
            return !query || name.indexOf(query) >= 0 || number.indexOf(query) >= 0;
        }).slice(0, 30);
    };

    $scope.showGanttDependencyChoices = function () {
        $scope.ganttDependencyChoicesOpen = true;
        // Keep the options outside the horizontally scrolling table.
        $timeout(function () {
            var input = document.querySelector('.pms-gantt-detail .pms-gantt-modern-search input');
            if (!input) input = document.querySelector('.pms-gantt-detail .pms-dependency-search-cell input');
            if (!input) return;
            var target = input.getBoundingClientRect();
            var desiredHeight = Math.min(180, Math.max(48, $scope.getFilteredGanttDependencyChoices().length * 42 + 10));
            var spaceBelow = Math.max(0, window.innerHeight - target.bottom - 12);
            var spaceAbove = Math.max(0, target.top - 12);
            var below = spaceBelow >= desiredHeight || spaceBelow >= spaceAbove;
            var availableSpace = below ? spaceBelow : spaceAbove;
            var height = Math.min(desiredHeight, availableSpace);
            var width = Math.min(390, Math.max(280, target.width), window.innerWidth - 16);
            var left = Math.max(8, Math.min(target.left, window.innerWidth - width - 8));
            var top = below ? target.bottom : target.top - height;
            $scope.ganttDependencyChoicesStyle = {
                left: left + 'px',
                top: Math.max(8, Math.min(top, window.innerHeight - height - 8)) + 'px',
                width: width + 'px',
                maxHeight: Math.max(48, height) + 'px'
            };
        }, 0);
    };

    $scope.ganttDependencySearchChanged = function () {
        var selected = $scope.ganttModernDependencyChoice;
        if ($scope.ganttDependencyView === 'modern' && selected) {
            var selectedLabel = [selected.serialNo, selected.name].filter(Boolean).join(' ').toLowerCase().trim();
            var query = String($scope.ganttDependencyForm.search || '').toLowerCase().trim();
            if (query !== selectedLabel) {
                $scope.ganttModernDependencyChoice = null;
                $scope.ganttDependencyForm.other = '';
            }
        }
        $scope.ganttDependencyHighlighted = 0;
        $scope.showGanttDependencyChoices();
    };

    $scope.setGanttDependencyHighlighted = function (index) {
        $scope.ganttDependencyHighlighted = index;
    };

    $scope.onGanttDependencySearchKeydown = function (event) {
        var options = $scope.getFilteredGanttDependencyChoices();
        if (event.keyCode === 27) {
            event.preventDefault();
            $scope.ganttDependencyChoicesOpen = false;
        } else if (event.keyCode === 40 || event.keyCode === 38) {
            event.preventDefault();
            if (!$scope.ganttDependencyChoicesOpen) $scope.showGanttDependencyChoices();
            else if (options.length) $scope.ganttDependencyHighlighted =
                (Number($scope.ganttDependencyHighlighted || 0) + (event.keyCode === 40 ? 1 : -1) + options.length) % options.length;
            $timeout(function () {
                var active = document.querySelector('.pms-gantt-detail .pms-dependency-choices .is-active');
                if (active) active.scrollIntoView({ block: 'nearest' });
            }, 0);
        } else if (event.keyCode === 13 && $scope.ganttDependencyChoicesOpen && options.length) {
            event.preventDefault();
            var highlightedChoice = options[$scope.ganttDependencyHighlighted] || options[0];
            if ($scope.ganttDependencyView === 'modern') $scope.chooseModernGanttDependency(highlightedChoice);
            else $scope.chooseGanttDependency(highlightedChoice);
        }
    };

    $scope.chooseGanttDependency = function (row) {
        if (!row || $scope.ganttDependencyBusy) return;
        $scope.ganttDependencyForm.other = row.type + ':' + row.id;
        $scope.ganttDependencyChoicesOpen = false;
        $scope.ganttDependencyHighlighted = 0;
        $scope.saveGanttDependency();
    };

    $scope.beginGanttDependencySearch = function () {
        $scope.ganttAddingDependency = true;
        $scope.ganttEditingDependency = null;
        $scope.ganttEditingDependencyItem = null;
        $scope.ganttModernDependencyChoice = null;
        $scope.ganttDependencyForm.other = '';
        $scope.ganttDependencyForm.search = '';
        $scope.ganttDependencyForm.linkType = 'FS';
        $scope.ganttDependencyForm.lagHours = 0;
        $scope.ganttDependencyError = '';
        $timeout(function () {
            var input = document.querySelector('.pms-gantt-detail .pms-gantt-modern-search input');
            if (!input) input = document.querySelector('.pms-gantt-detail .pms-dependency-search-cell input');
            if (input) input.focus();
        }, 0);
    };

    $scope.cancelGanttDependencySearch = function () {
        $scope.ganttAddingDependency = false;
        $scope.ganttEditingDependency = null;
        $scope.ganttEditingDependencyItem = null;
        $scope.ganttModernDependencyChoice = null;
        $scope.ganttDependencyForm.other = '';
        $scope.ganttDependencyForm.linkType = 'FS';
        $scope.ganttDependencyForm.lagHours = 0;
        $scope.ganttDependencyChoicesOpen = false;
        $scope.ganttDependencyForm.search = '';
    };

    $scope.viewGanttDependencyItem = function (link) {
        var item = $scope.getGanttDependencyItem(link);
        if (item) $scope.openGanttSubtaskPopup(item);
    };

    function ganttLinkedRow(type, id) {
        return ($scope.originalRows || []).find(function (r) {
            return r.type === type && String(r.id) === String(id);
        });
    }

    $scope.getGanttDependencyItem = function (link) {
        if (!link) return null;
        return $scope.ganttDependencyTab === 'predecessor'
            ? ganttLinkedRow(link.PredecessorType, link.PredecessorId)
            : ganttLinkedRow(link.SuccessorType, link.SuccessorId);
    };

    $scope.refreshGanttDependencies = function () {
        if (!$scope.ganttDetailRow || !$scope.ganttDetailRow.id) return;
        var row = $scope.ganttDetailRow, tab = $scope.ganttDependencyTab;
        $scope.ganttDependencyBusy = true;
        $scope.ganttDependencyError = '';
        return assignprojectService.ganttDependency('list', ganttDependencyPayload(row)).then(function (res) {
            if ($scope.ganttDetailRow !== row || $scope.ganttDependencyTab !== tab) return;
            if (res.data.d.error) throw new Error(res.data.d.error);
            $scope.ganttDependencyRows = res.data.d.rows || [];
        }).catch(function (error) {
            if ($scope.ganttDetailRow === row && $scope.ganttDependencyTab === tab)
                $scope.ganttDependencyError = (error.data && error.data.Message) || error.message || 'Could not load dependencies.';
        }).finally(function () { $scope.ganttDependencyBusy = false; });
    };

    $scope.saveGanttDependency = function () {
        var choice = String($scope.ganttDependencyForm.other || '').split(':');
        if (choice.length !== 2) { $scope.ganttDependencyError = 'Select an item.'; return; }
        var request = ganttDependencyPayload($scope.ganttDetailRow);
        request.otherType = choice[0]; request.otherId = Number(choice[1]);
        request.linkType = $scope.ganttDependencyForm.linkType;
        request.lagHours = Number($scope.ganttDependencyForm.lagHours);
        if (!Number.isInteger(request.lagHours)) { $scope.ganttDependencyError = 'Lag must be a whole number of hours.'; return; }
        $scope.ganttDependencyBusy = true;
        $scope.ganttDependencyError = '';
        assignprojectService.ganttDependency('add', request).then(function (res) {
            if (res.data.d.error) throw new Error(res.data.d.error);
            showGanttDependencyNotice('Dependency added');
            $scope.ganttDependencyForm.search = '';
            $scope.ganttAddingDependency = false;
            $scope.ganttEditingDependency = null;
            $scope.ganttEditingDependencyItem = null;
            $scope.ganttModernDependencyChoice = null;
            $scope.ganttDependencyForm = { other: '', linkType: 'FS', lagHours: 0, search: '' };
            $scope.loadGanttChartDependencies();
            return $scope.refreshGanttDependencies();
        }).catch(function (error) {
            $scope.ganttDependencyError = (error.data && error.data.Message) || error.message || 'Could not save dependency.';
        }).finally(function () { $scope.ganttDependencyBusy = false; });
    };

    $scope.removeGanttDependency = function (link) {
        if (!link) return;
        var request = ganttDependencyPayload($scope.ganttDetailRow);
        request.linkId = Number(link.Id);
        $scope.ganttDependencyBusy = true;
        $scope.ganttDependencyError = '';
        assignprojectService.ganttDependency('delete', request).then(function (res) {
            if (res.data.d.error) throw new Error(res.data.d.error);
            var affected = (res.data.d.rows || [])[0];
            if (affected && Number(affected.Affected) === 0) throw new Error('Dependency was not found. Refresh and try again.');
            $scope.ganttDependencyRows = $scope.ganttDependencyRows.filter(function (item) {
                return Number(item.Id) !== request.linkId;
            });
            showGanttDependencyNotice('Dependency deleted');
            $scope.loadGanttChartDependencies();
            return $scope.refreshGanttDependencies();
        }).catch(function (error) {
            $scope.ganttDependencyError = (error.data && error.data.Message) || error.message || 'Could not remove dependency.';
        }).finally(function () { $scope.ganttDependencyBusy = false; });
    };

    $scope.suppressGanttResizeClick = function ($event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        $scope.ganttSuppressPopupUntil = Date.now() + 350;
    };

    $scope.openGanttSubtaskPopup = function (row, $event) {
        if (!row || ['milestone', 'task', 'subtask'].indexOf(row.type) < 0) return;
        var nativeEvent = $event && ($event.originalEvent || $event);
        var target = nativeEvent && nativeEvent.target;
        if (target && target.closest && target.closest('.pms-gantt-resize')) return;
        if (Date.now() < Number($scope.ganttSuppressPopupUntil || 0)) return;
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        $scope.ganttDetailRow = row;
        $scope.ganttDependencyView = 'modern';
        $scope.ganttModernDependencyChoice = null;
        $scope.ganttDependencyTab = 'predecessor';
        $scope.ganttDetailOpen = true;
        $scope.ganttAddingDependency = false;
        $scope.ganttEditingDependency = null;
        $scope.ganttEditingDependencyItem = null;
        $scope.ganttDependencyForm.search = '';
        clearGanttDependencyNotice();
        $scope.ganttDependencyChoicesOpen = false;
        $scope.ganttDependencyMenuId = null;
        $scope.ganttDependencyRows = [];
        $scope.refreshGanttDependencies();
        // Refresh the complete project link set so candidates already used on
        // either the Predecessor or Successor tab are hidden immediately.
        $scope.loadGanttChartDependencies();
    };

    $scope.closeGanttSubtaskPopup = function ($event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        $scope.ganttDetailOpen = false;
        $scope.ganttDetailRow = {};
        $scope.ganttAddingDependency = false;
        $scope.ganttEditingDependency = null;
        $scope.ganttEditingDependencyItem = null;
    };

    $scope.setGanttDependencyTab = function (tab) {
        $scope.ganttDependencyTab = tab === 'successor' ? 'successor' : 'predecessor';
        $scope.ganttAddingDependency = false;
        $scope.ganttEditingDependency = null;
        $scope.ganttEditingDependencyItem = null;
        $scope.ganttModernDependencyChoice = null;
        $scope.ganttDependencyForm.other = '';
        $scope.ganttDependencyForm.search = '';
        clearGanttDependencyNotice();
        $scope.ganttDependencyChoicesOpen = false;
        $scope.ganttDependencyMenuId = null;
        $scope.ganttDependencyRows = [];
        $scope.refreshGanttDependencies();
    };

    function ganttPopupDateTime(row, useEnd) {
        if (!row) return '';
        var raw = row.raw || {};
        var value = useEnd ? row.end : row.start;
        if (!value) return '';
        var d = new Date(value);
        var timeText = useEnd
            ? (raw.EndTime || raw.endtime || raw.endTime || '')
            : (raw.StartTime || raw.starttime || raw.startTime || '');
        var match = String(timeText).match(/(\d{1,2}):(\d{2})/);
        if (match) d.setHours(Number(match[1]), Number(match[2]), 0, 0);
        var dateText = String(d.getDate()).padStart(2, '0') + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + d.getFullYear();
        var hour = d.getHours(), minute = String(d.getMinutes()).padStart(2, '0');
        var clock = String((hour % 12) || 12).padStart(2, '0') + ':' + minute + ' ' + (hour >= 12 ? 'PM' : 'AM');
        return dateText + ' ' + clock;
    }

    $scope.getGanttPopupDateRange = function (row) {
        var start = ganttPopupDateTime(row, false);
        var end = ganttPopupDateTime(row, true);
        return start && end ? start + ' to ' + end : (start || end || 'Date not set');
    };

    $scope.getGanttPopupDayText = function (row) {
        if (!row || !row.start || !row.end) return $scope.getGanttDurationText(row);
        var days = Math.max(1, $scope.diffDays(row.end, row.start) + 1);
        return days + ' day' + (days === 1 ? '' : 's') + ' / ' + $scope.getGanttDurationText(row) + ' to go';
    };

    // Completion rules use the existing dependency editor. Scheduling links
    // remain in the Gantt row detail's Predecessor and Successor tabs.
    $scope.openGanttCompletionDependency = function (row, $event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        if (!row || !row.id) return;
        var source = row.raw || row;
        var projectId = row.ProjectId || source.ProjectId || (($scope.projectlist || {}).id) || 0;
        $scope.showCompletionDependencyPopup(row.id, row.type, projectId, row.name || 'item', source);
    };

    $scope.getGanttDisplayBarStyle = function (row) {
        if (row && row._resizeStyle) return row._resizeStyle;
        var style = $scope.getBarStyle(row);
        if (!style['background-color']) style['background-color'] = '#70cbea';
        return style;
    };

    // User-facing scale names: Hours=>hour cells, Days=>day cells,
    // Weeks=>week cells, Months=>month cells, Quarters=>quarter cells.
    // Internal scale values remain unchanged so bar positions and resize logic
    // use the existing timeline calculations.
    $scope.ganttZoomLevel = 1;
    $scope.ganttPrependCount = 0;
    $scope.ganttPostpendCount = 0;
    $scope.ganttNonWorkingCells = [];
    var ganttNonWorkingRequest = 0;
    var ganttNonWorkingCache = {};
    var ganttAutoScrollPending = false;
    function ganttDateKey(date) {
        return date.getFullYear() + '-' + String(date.getMonth() + 1).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0');
    }
    function updateGanttNonWorkingCells() {
        var scale = normalizeGanttScale($scope.ganttScale);
        var request = ++ganttNonWorkingRequest;
        $scope.ganttNonWorkingCells = [];
        if ((scale !== 'week' && scale !== 'day') || !$scope.ganttCells.length) {
            ganttAutoScrollPending = false;
            return;
        }
        var cells = $scope.ganttCells;
        var first = ganttDateKey(cells[0].date), last = ganttDateKey(cells[cells.length - 1].date);
        var cacheKey = first + ':' + last;
        function applyDates(dates) {
            if (request !== ganttNonWorkingRequest) return;
            // Keep every date visible, including week offs and holidays, and
            // retain their existing background highlights.
            var visible = [];
            var visibleGroups = [];
            var offset = 0;
            angular.forEach($scope.ganttHeaderGroups || [], function (header) {
                var groupCells = Math.round(header.width / $scope.ganttCellWidth);
                var kept = 0;
                for (var i = offset; i < offset + groupCells && i < cells.length; i++) {
                    var kind = dates[ganttDateKey(cells[i].date)] || '';
                    cells[i].nonWorkingKind = kind;
                    visible.push(cells[i]);
                    kept++;
                }
                if (kept) visibleGroups.push({ label: header.label, width: kept * $scope.ganttCellWidth });
                offset += groupCells;
            });
            $scope.ganttCells = visible;
            $scope.ganttHeaderGroups = visibleGroups;
            var highlights = [];
            angular.forEach($scope.ganttCells, function (cell, index) {
                var kind = cell.nonWorkingKind;
                if (kind) highlights.push({ index: index, kind: kind });
            });
            $scope.ganttNonWorkingCells = highlights;
            if (ganttAutoScrollPending) {
                ganttAutoScrollPending = false;
                scrollGanttToFirstScheduledRow();
            }
            if ($scope.ganttPrependAnchor) {
                var anchor = $scope.ganttPrependAnchor;
                $scope.ganttPrependAnchor = null;
                $timeout(function () {
                    var timeline = document.querySelector('#pmsGanttRoot .pms-gantt-right');
                    if (timeline) timeline.scrollLeft = ganttVisiblePosition(anchor.date, scale) * $scope.ganttCellWidth + anchor.offset;
                }, 0);
            }
        }
        if (ganttNonWorkingCache[cacheKey]) {
            applyDates(ganttNonWorkingCache[cacheKey]);
            return;
        }
        assignprojectService.getGanttNonWorkingDates(first, last).then(function (response) {
            var dates = response && response.data && response.data.d || {};
            ganttNonWorkingCache[cacheKey] = dates;
            applyDates(dates);
        }, function () {
            // Leave the timeline usable if the calendar request fails.
            if (request === ganttNonWorkingRequest) ganttAutoScrollPending = false;
        });
    }

    function ganttMonthDiff(a, b) {
        return (a.getFullYear() - b.getFullYear()) * 12 + a.getMonth() - b.getMonth();
    }
    function ganttStartDateTime(row) {
        if (!row || !row.start) return null;
        var d = new Date(row.start), raw = row.raw || {};
        var m = String(raw.StartTime || raw.starttime || raw.startTime || '').match(/(\d{1,2}):(\d{2})/);
        d.setHours(m ? Number(m[1]) : 0, m ? Number(m[2]) : 0, 0, 0);
        return d;
    }
    // Position a real date on the compressed Days/Hours grid. A hidden date
    // occupies zero pixels. The returned coordinate is in visible cells.
    function ganttVisiblePosition(date, scale) {
        var cells = $scope.ganttCells || [];
        if (!date || !cells.length) return 0;
        var target = date.getTime();
        var low = 0, high = cells.length;
        while (low < high) {
            var mid = (low + high) >>> 1;
            if (cells[mid].date.getTime() < target) low = mid + 1;
            else high = mid;
        }
        if (!low) return 0;
        var index = low - 1, start = cells[index].date.getTime();
        var next = scale === 'day' ? start + 3600000 : $scope.addDays(cells[index].date, 1).getTime();
        return target < next ? index + (target - start) / (next - start) : low;
    }
    function ganttShortDate(d) {
        return String(d.getDate()).padStart(2, '0') + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + d.getFullYear();
    }
    function ganttIsoWeek(d) {
        var x = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate()));
        var day = x.getUTCDay() || 7;
        x.setUTCDate(x.getUTCDate() + 4 - day);
        var yearStart = new Date(Date.UTC(x.getUTCFullYear(), 0, 1));
        return Math.ceil((((x - yearStart) / 86400000) + 1) / 7);
    }

    $scope.ganttZoom = function (direction) {
        $scope.ganttZoomLevel = Math.max(0.6, Math.min(2, Number($scope.ganttZoomLevel || 1) + Number(direction || 0) * 0.2));
        $scope.buildGanttTimeline();
    };

    $scope.ganttExtendRange = function (side) {
        if (side !== 'prepend' && side !== 'postpend') return;
        var right = document.querySelector('#pmsGanttRoot .pms-gantt-right');
        var oldWidth = $scope.ganttCells.length * $scope.ganttCellWidth;
        var oldScroll = right ? right.scrollLeft : 0;
        if (side === 'prepend' && right && $scope.ganttCells.length &&
            ($scope.ganttScale === 'week' || $scope.ganttScale === 'day')) {
            var index = Math.min($scope.ganttCells.length - 1, Math.floor(right.scrollLeft / $scope.ganttCellWidth));
            $scope.ganttPrependAnchor = {
                date: new Date($scope.ganttCells[index].date),
                offset: right.scrollLeft - index * $scope.ganttCellWidth
            };
        }
        if (side === 'prepend') $scope.ganttPrependCount = Number($scope.ganttPrependCount || 0) + 1;
        else $scope.ganttPostpendCount = Number($scope.ganttPostpendCount || 0) + 1;
        $scope.buildGanttTimeline();
        if (side === 'prepend' && $scope.ganttScale !== 'week' && $scope.ganttScale !== 'day') {
            var addedWidth = $scope.ganttCells.length * $scope.ganttCellWidth - oldWidth;
            $timeout(function () {
                if (right) right.scrollLeft = oldScroll + Math.max(0, addedWidth);
            }, 0);
        }
    };

    $scope.buildGanttTimeline = function () {
        var rows = ($scope.ganttRows || []).filter(function (r) { return r.start && r.end; });
        var today = new Date(); today.setHours(0, 0, 0, 0);
        var minDate = rows.length ? new Date(Math.min.apply(null, rows.map(function (r) { return r.start.getTime(); }))) : new Date(today);
        var maxDate = rows.length ? new Date(Math.max.apply(null, rows.map(function (r) { return r.end.getTime(); }))) : new Date(today);
        var scale = normalizeGanttScale($scope.ganttScale), zoom = Number($scope.ganttZoomLevel || 1);
        $scope.ganttScale = scale;
        var prependCount = Number($scope.ganttPrependCount || 0), postpendCount = Number($scope.ganttPostpendCount || 0);
        var cells = [], groups = [], cursor, count = 0, groupStart;
        function group(label, units) { groups.push({ label: label, width: units * $scope.ganttCellWidth }); }

        if (scale === 'day') {
            $scope.ganttCellWidth = Math.round(27 * zoom);
            minDate = $scope.addDays(minDate, -1 - prependCount); minDate.setHours(0, 0, 0, 0);
            maxDate = $scope.addDays(maxDate, 1 + postpendCount); maxDate.setHours(23, 0, 0, 0);
            cursor = new Date(minDate);
            while (cursor <= maxDate) {
                var hour = cursor.getHours();
                // Match the old 12-hour header: AM, 01..11, PM, 01..11.
                cells.push({ date: new Date(cursor), label: hour === 0 ? 'AM' : (hour === 12 ? 'PM' : String(hour % 12).padStart(2, '0')) });
                count++;
                var nextHour = new Date(cursor.getTime() + 3600000);
                if (nextHour.getDate() !== cursor.getDate()) {
                    group(cursor.toLocaleDateString('en-GB', { weekday: 'short' }).toUpperCase() + '  ' + ganttShortDate(cursor), count);
                    count = 0;
                }
                cursor = nextHour;
            }
        }
        else if (scale === 'week') {
            $scope.ganttCellWidth = Math.round(40 * zoom);
            // Monday-to-Sunday ISO weeks, with one complete week on each side.
            var minMondayOffset = (minDate.getDay() + 6) % 7;
            minDate = $scope.addDays(minDate, -minMondayOffset - (7 * (1 + prependCount)));
            var maxSundayOffset = 6 - ((maxDate.getDay() + 6) % 7);
            maxDate = $scope.addDays(maxDate, maxSundayOffset + (7 * (1 + postpendCount)));
            cursor = new Date(minDate); groupStart = new Date(cursor);
            while (cursor <= maxDate) {
                cells.push({ date: new Date(cursor), label: cursor.getDate() }); count++;
                var nextDay = $scope.addDays(cursor, 1);
                if (count === 7 || nextDay > maxDate) {
                    var sameMonth = groupStart.getMonth() === cursor.getMonth() && groupStart.getFullYear() === cursor.getFullYear();
                    var weekRange = sameMonth
                        ? String(groupStart.getDate()).padStart(2, '0') + '-' + String(cursor.getDate()).padStart(2, '0') + ' ' + cursor.toLocaleDateString('en-US', { month: 'short' }).toUpperCase() + ', ' + cursor.getFullYear()
                        : ganttShortDate(groupStart) + ' - ' + ganttShortDate(cursor);
                    group('W' + ganttIsoWeek(groupStart) + '  ' + weekRange, count);
                    count = 0; groupStart = new Date(nextDay);
                }
                cursor = nextDay;
            }
        }
        else if (scale === 'month') {
            $scope.ganttCellWidth = Math.round(52 * zoom);
            minDate = new Date(minDate.getFullYear(), minDate.getMonth() - 1 - prependCount, 1);
            // Align the weekly markers to Monday so the dates continue across months.
            minDate = $scope.addDays(minDate, -((minDate.getDay() + 6) % 7));
            maxDate = new Date(maxDate.getFullYear(), maxDate.getMonth() + 2 + postpendCount, 0);
            cursor = new Date(minDate);
            while (cursor <= maxDate) {
                cells.push({ date: new Date(cursor), label: cursor.getDate() }); count++;
                var nextWeek = $scope.addDays(cursor, 7);
                if (nextWeek.getMonth() !== cursor.getMonth() || nextWeek > maxDate) {
                    group(cursor.toLocaleDateString('en-US', { month: 'short', year: 'numeric' }).toUpperCase(), count);
                    count = 0;
                }
                cursor = nextWeek;
            }
        }
        else if (scale === 'quarter') {
            $scope.ganttCellWidth = Math.round(64 * zoom);
            minDate = new Date(minDate.getFullYear(), Math.floor(minDate.getMonth() / 3) * 3 - 3 - (prependCount * 3), 1);
            maxDate = new Date(maxDate.getFullYear(), Math.floor(maxDate.getMonth() / 3) * 3 + 6 + (postpendCount * 3), 0);
            cursor = new Date(minDate); groupStart = new Date(cursor);
            while (cursor <= maxDate) {
                cells.push({ date: new Date(cursor), label: cursor.toLocaleDateString('en-US', { month: 'short' }).toUpperCase() }); count++;
                var nextMonth = new Date(cursor.getFullYear(), cursor.getMonth() + 1, 1);
                if (count === 3 || nextMonth > maxDate) {
                    group('Q' + (Math.floor(groupStart.getMonth() / 3) + 1) + "' " + groupStart.getFullYear(), count);
                    count = 0; groupStart = new Date(nextMonth);
                }
                cursor = nextMonth;
            }
        }
        else {
            $scope.ganttCellWidth = Math.round(42 * zoom);
            minDate = new Date(minDate.getFullYear() - 2 - prependCount, 0, 1);
            maxDate = new Date(maxDate.getFullYear() + 3 + postpendCount, 0, 0);
            cursor = new Date(minDate);
            while (cursor <= maxDate) {
                cells.push({ date: new Date(cursor), label: 'Q' + (Math.floor(cursor.getMonth() / 3) + 1) }); count++;
                var nextQuarter = new Date(cursor.getFullYear(), cursor.getMonth() + 3, 1);
                if (nextQuarter.getFullYear() !== cursor.getFullYear() || nextQuarter > maxDate) {
                    group(String(cursor.getFullYear()), count); count = 0;
                }
                cursor = nextQuarter;
            }
        }
        $scope.ganttCells = cells;
        $scope.ganttHeaderGroups = groups;
        $scope.ganttStart = minDate;
        $scope.ganttEnd = maxDate;
        updateGanttNonWorkingCells();
        $scope.buildGanttDependencyPaths();
    };

    $scope.getBarStyle = function (row) {
        if (!row || !row.start || !$scope.ganttStart) return {};
        var scale = normalizeGanttScale($scope.ganttScale), leftUnits, widthUnits;
        var minutes = $scope.getDurationMinutesForValidation ? $scope.getDurationMinutesForValidation(row.raw || {}) : 0;
        if (scale === 'day') {
            var hourStart = ganttStartDateTime(row);
            var hourEnd = minutes > 0 ? new Date(hourStart.getTime() + minutes * 60000)
                : $scope.addDays(row.end, 1);
            leftUnits = ganttVisiblePosition(hourStart, scale);
            widthUnits = ganttVisiblePosition(hourEnd, scale) - leftUnits;
        }
        else if (scale === 'week') {
            leftUnits = ganttVisiblePosition(row.start, scale);
            widthUnits = ganttVisiblePosition($scope.addDays(row.end, 1), scale) - leftUnits;
        }
        else if (scale === 'month') { leftUnits = $scope.diffDays(row.start, $scope.ganttStart) / 7; widthUnits = Math.max(.35, ($scope.diffDays(row.end, row.start) + 1) / 7); }
        else if (scale === 'quarter') { leftUnits = ganttMonthDiff(row.start, $scope.ganttStart); widthUnits = Math.max(.25, ganttMonthDiff(row.end, row.start) + 1); }
        else { leftUnits = ganttMonthDiff(row.start, $scope.ganttStart) / 3; widthUnits = Math.max(.18, (ganttMonthDiff(row.end, row.start) + 1) / 3); }
        var color = row.status_color;
        if (!color || String(color).toLowerCase() === '#fff' || String(color).toLowerCase() === '#ffffff') color = '#70cbea';
        if (widthUnits <= 0) return { display: 'none' };
        // Keep a usable centre click target even when a task only spans a few
        // minutes/hours at a wide timeline scale.  The real dates/duration are
        // unchanged; this is only the minimum rendered interaction width.
        return { left: (leftUnits * $scope.ganttCellWidth) + 'px', width: Math.max(30, widthUnits * $scope.ganttCellWidth) + 'px', 'background-color': color, border: '1px solid rgba(0,0,0,.08)' };
    };

    $scope.getGanttTodayStyle = function () {
        if (!$scope.ganttStart || !$scope.ganttEnd) {
            $scope._ganttTodayStyleCache = null;
            return null;
        }
        var now = new Date(); now.setHours(0, 0, 0, 0);
        if (now < $scope.ganttStart || now > $scope.ganttEnd) {
            $scope._ganttTodayStyleCache = null;
            return null;
        }
        var scale = normalizeGanttScale($scope.ganttScale), units;
        if (scale === 'day' || scale === 'week') {
            if (!($scope.ganttCells || []).some(function (cell) { return ganttDateKey(cell.date) === ganttDateKey(now); })) {
                $scope._ganttTodayStyleCache = null;
                return null;
            }
            units = ganttVisiblePosition(now, scale);
        }
        else if (scale === 'month') units = $scope.diffDays(now, $scope.ganttStart) / 7;
        else if (scale === 'quarter') units = ganttMonthDiff(now, $scope.ganttStart);
        else units = ganttMonthDiff(now, $scope.ganttStart) / 3;
        var leftValue = (units * $scope.ganttCellWidth) + 'px';
        // ng-if/ng-style repeatedly evaluate this method. Returning a new
        // object every time causes AngularJS 1.8 to report $rootScope:infdig,
        // especially after switching to Quarter or Year. Reuse the same
        // object until the calculated position actually changes.
        if (!$scope._ganttTodayStyleCache || $scope._ganttTodayStyleCache.left !== leftValue) {
            $scope._ganttTodayStyleCache = { left: leftValue };
        }
        return $scope._ganttTodayStyleCache;
    };

    $scope.ganttPanelCollapsed = false;
    $scope.ganttExpandedLeftWidth = Number($scope.ganttLeftWidth || 560);

    $scope.toggleGanttPanel = function ($event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        if ($scope.ganttPanelCollapsed) {
            $scope.ganttPanelCollapsed = false;
            $scope.ganttLeftWidth = Number($scope.ganttExpandedLeftWidth || 560);
        } else {
            $scope.ganttExpandedLeftWidth = Number($scope.ganttLeftWidth || 560);
            $scope.ganttPanelCollapsed = true;
        }
        $timeout(function () {
            var right = document.querySelector('.pms-gantt-right');
            if (right) right.dispatchEvent(new Event('scroll'));
        }, 0);
    };

    $scope.startGanttPanelResize = function ($event) {
        var e = $event && ($event.originalEvent || $event);
        if (!e) return;
        e.preventDefault();
        if ($scope.ganttPanelCollapsed) return;
        var startX = e.clientX;
        var startWidth = Number($scope.ganttLeftWidth || 560);
        var root = document.getElementById('pmsGanttRoot');
        var maxWidth = root ? Math.max(520, root.clientWidth * 0.75) : 900;

        function move(ev) {
            var width = Math.max(360, Math.min(maxWidth, startWidth + ev.clientX - startX));
            $scope.$evalAsync(function () {
                $scope.ganttLeftWidth = Math.round(width);
                $scope.ganttExpandedLeftWidth = Math.round(width);
            });
        }
        function end() {
            document.removeEventListener('mousemove', move, true);
            document.removeEventListener('mouseup', end, true);
        }
        document.addEventListener('mousemove', move, true);
        document.addEventListener('mouseup', end, true);
    };

    $scope.startGanttBarResize = function ($event, row, edge) {
        var e = $event && ($event.originalEvent || $event);
        if (!e || !row || !row.raw || !$scope.canEdit(row.type)) return;
        e.preventDefault();
        e.stopPropagation();
        $scope.ganttSuppressPopupUntil = Number.MAX_SAFE_INTEGER || 9007199254740991;

        var startX = e.clientX;
        var originalStyle = $scope.getBarStyle(row);
        var originalLeft = parseFloat(originalStyle.left) || 0;
        var originalWidth = parseFloat(originalStyle.width) || $scope.ganttCellWidth;
        var originalHours = row.raw.TaskDuration;
        var originalMinutes = row.raw.TaskDuration_min;
        var perDayMinutes = Math.max(1, Number($scope.perdayhrs || 8)) * 60;
        var minutesPerStep = $scope.ganttScale === 'day' ? 60
            : ($scope.ganttScale === 'week' ? perDayMinutes
            : ($scope.ganttScale === 'month' ? 7 * perDayMinutes
            : ($scope.ganttScale === 'quarter' ? 30 * perDayMinutes : 90 * perDayMinutes)));
        var baseMinutes = $scope.getDurationMinutesForValidation ? $scope.getDurationMinutesForValidation(row.raw) : 0;
        if (!baseMinutes) baseMinutes = Math.max(1, Math.round(originalWidth / $scope.ganttCellWidth)) * minutesPerStep;
        var lastUnits = 0;

        function move(ev) {
            var rawDelta = ev.clientX - startX;
            var units = Math.round(rawDelta / $scope.ganttCellWidth);
            if (edge === 'left') units = -units;
            var newMinutes = baseMinutes + (units * minutesPerStep);
            if (newMinutes < 1) {
                units = -Math.floor((baseMinutes - 1) / minutesPerStep);
                newMinutes = Math.max(1, baseMinutes + (units * minutesPerStep));
            }
            lastUnits = units;
            var pixelDelta = units * $scope.ganttCellWidth;
            var newWidth = Math.max(12, originalWidth + pixelDelta);
            var newLeft = edge === 'left' ? originalLeft - pixelDelta : originalLeft;
            if (newLeft < 0) { newWidth += newLeft; newLeft = 0; }
            var display = { left:newLeft+'px', width:newWidth+'px', 'background-color':row.status_color || '#9eead8' };
            $scope.$evalAsync(function () {
                row._resizeStyle = display;
                row._pendingResizeMinutes = newMinutes;
            });
        }

        function end() {
            document.removeEventListener('mousemove', move, true);
            document.removeEventListener('mouseup', end, true);
            $scope.ganttSuppressPopupUntil = Date.now() + 350;
            $scope.$evalAsync(function () {
                var total = Number(row._pendingResizeMinutes || baseMinutes);
                delete row._pendingResizeMinutes;
                delete row._resizeStyle;
                if (!lastUnits) return;
                row.raw._oldTaskDuration = originalHours;
                row.raw._oldTaskDuration_min = originalMinutes;
                row.raw.TaskDuration = String(Math.floor(total / 60));
                row.raw.TaskDuration_min = String(total % 60).padStart(2, '0');
                $scope.update_hrs(row.raw, row.type);
            });
        }
        document.addEventListener('mousemove', move, true);
        document.addEventListener('mouseup', end, true);
    };


    // =========================
    // LIVE TIMESHEET VIEW
    // =========================
    // Builds the reference timeline layout from projectlist.miles -> tasks -> subtasks.
    // No additional API call is made and the source hierarchy objects are not modified.
    function parseTimesheetDate(value) {
        var date = normalizeHierarchyDateOnly(value);
        if (!date) return null;
        return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    }

    function timesheetAddDays(date, days) {
        return new Date(date.getFullYear(), date.getMonth(), date.getDate() + days);
    }

    function timesheetDiffDays(fromDate, toDate) {
        if (!fromDate || !toDate) return 0;
        var fromUtc = Date.UTC(fromDate.getFullYear(), fromDate.getMonth(), fromDate.getDate());
        var toUtc = Date.UTC(toDate.getFullYear(), toDate.getMonth(), toDate.getDate());
        return Math.round((toUtc - fromUtc) / 86400000);
    }

    function timesheetFormatDate(date) {
        if (!date) return '';
        var day = String(date.getDate()).padStart(2, '0');
        var month = String(date.getMonth() + 1).padStart(2, '0');
        return day + '/' + month + '/' + date.getFullYear();
    }

    function timesheetMonthName(date) {
        return date.toLocaleString('en-US', { month: 'long', year: 'numeric' });
    }

    function getTimesheetStatus(src) {
        src = src || {};
        var statusId = src.P_Status;
        if (statusId === undefined || statusId === null || statusId === '') {
            statusId = src.status_id || src.StatusId || src.statusid;
        }

        var statusName = src.status_name || src.StatusName || src.statusname || '';
        var statusColor = src.status_color || src.StatusColor || src.statuscolor || '';

        if ((!statusName || !statusColor) && $scope.statuslists) {
            var status = ($scope.statuslists || []).find(function (x) {
                return String(x.id) === String(statusId);
            });
            if (status) {
                statusName = statusName || status.name || '';
                statusColor = statusColor || status.colorname || '';
            }
        }

        return {
            id: statusId,
            name: statusName || 'Not Set',
            color: statusColor || '#6c757d'
        };
    }

    function getTimesheetProgress(src) {
        src = src || {};
        var value = src.complete_value;
        if (value === undefined || value === null || value === '') value = src.iscompletepercentage;
        if (value === undefined || value === null || value === '') value = src.CompleteValue;
        if (value === undefined || value === null || value === '') value = src.progress;
        if ((value === undefined || value === null || value === '') && Number(src.iscomplete) === 1) value = 100;

        value = parseFloat(value);
        if (isNaN(value)) value = 0;
        if (value < 0) value = 0;
        if (value > 100) value = 100;
        return Math.round(value * 100) / 100;
    }

    function normalizeTimesheetOwners(src) {
        src = src || {};
        var result = [];
        var seen = {};
        var directOwners = src.owners || src.Owners || [];

        function addOwner(owner) {
            if (!owner) return;
            var id = owner.id || owner.Id || owner.ownerid || owner.OwnerId || '';
            var key = String(id || owner.name || owner.DisplayName || result.length);
            if (seen[key]) return;
            seen[key] = true;
            result.push({
                id: id,
                name: owner.name || owner.DisplayName || owner.Name || '',
                profileimage: owner.profileimage || owner.fileupload1 || owner.ProfileImage || '',
                shortname: owner.shortname || owner.splinameone || ''
            });
        }

        if (angular.isArray(directOwners)) {
            angular.forEach(directOwners, addOwner);
        }

        var ownerIds = src.assignto;
        if (ownerIds === undefined || ownerIds === null || String(ownerIds).trim() === '' || String(ownerIds) === '0') {
            ownerIds = src.Owner !== undefined ? src.Owner : src.owner;
        }

        if (angular.isArray(ownerIds)) {
            angular.forEach(ownerIds, function (owner) {
                if (angular.isObject(owner)) addOwner(owner);
                else addOwnerFromId(owner);
            });
        }
        else if (angular.isObject(ownerIds)) {
            addOwner(ownerIds);
        }
        else {
            angular.forEach(String(ownerIds || '').split(','), addOwnerFromId);
        }

        function addOwnerFromId(ownerId) {
            ownerId = String(ownerId || '').trim();
            if (!ownerId || ownerId === '0') return;
            var owner = ($scope.owners || []).find(function (x) {
                return String(x.id) === ownerId;
            });
            if (owner) addOwner(owner);
        }

        return result;
    }

    function isTimesheetTaskVisible(task) {
        return $scope.hasMatchingTaskBranch(task);
    }

    function isTimesheetSubtaskVisible(subtask) {
        return $scope.matchesTopLeafFilters(subtask);
    }

    function createTimesheetRow(src, type, level, title, numberedTitle, milestoneIndex, taskIndex) {
        src = src || {};
        var status = getTimesheetStatus(src);
        var startDate = type === 'milestone'
            ? parseTimesheetDate(src.StartDate || src.startDate)
            : parseTimesheetDate(src.Startdate || src.StartDate || src.startdate);
        var endDate = type === 'milestone'
            ? parseTimesheetDate(src.EndDate || src.endDate || src.TargetDate)
            : parseTimesheetDate(src.Targetdate || src.TargetDate || src.EndDate || src.targetdate);

        if (startDate && endDate && endDate < startDate) {
            var swap = startDate;
            startDate = endDate;
            endDate = swap;
        }

        var startTime = src.StartTime || src.starttime || '';
        var workingMinutes = $scope.getDurationMinutesForValidation
            ? $scope.getDurationMinutesForValidation(src)
            : 0;
        var durationCapacity = getPerDayDateCapacity(src, startDate, endDate);

        return {
            key: type + '_' + String(src.id || '') + '_' + milestoneIndex + '_' + taskIndex,
            id: src.id,
            type: type,
            level: level,
            title: title || '',
            numberedTitle: numberedTitle || title || '',
            source: src,
            owners: normalizeTimesheetOwners(src),
            statusId: status.id,
            statusName: status.name,
            statusColor: status.color,
            progress: getTimesheetProgress(src),
            start: startDate,
            end: endDate,
            startLabel: timesheetFormatDate(startDate),
            startTime: startTime,
            startDateTimeLabel: timesheetFormatDate(startDate) +
                (startTime ? ' ' + startTime : ''),
            endLabel: timesheetFormatDate(endDate),
            hasDates: !!(startDate && endDate),
            durationDays: startDate && endDate ? timesheetDiffDays(startDate, endDate) + 1 : 0,
            workingMinutes: workingMinutes,
            workingHoursText: Math.floor(workingMinutes / 60) + ':' +
                String(workingMinutes % 60).padStart(2, '0'),
            capacityHours: durationCapacity.hours,
            perDayHours: durationCapacity.perDayHours,
            barStyle: {}
        };
    }

    $scope.buildTimesheet = function () {
        var rows = [];
        var hierarchy = ($scope.projectlist && $scope.projectlist.miles) || [];

        angular.forEach(hierarchy, function (mile, mileIndex) {
            var milestoneNumber = String(mileIndex + 1);
            var milestoneTitle = mile.Milestone || mile.milestone || ('Milestone ' + milestoneNumber);

            if ($scope.filterMilestoneAssignView(mile)) {
                rows.push(createTimesheetRow(
                    mile,
                    'milestone',
                    0,
                    milestoneTitle,
                    milestoneNumber + ' ' + milestoneTitle,
                    mileIndex,
                    -1
                ));
            }

            angular.forEach(mile.tasks || mile.Tasks || [], function (task, taskIndex) {
                var taskNumber = milestoneNumber + '.' + String(taskIndex + 1);
                var taskTitle = task.taskname || task.Taskname || task.TaskName || ('Task ' + taskNumber);

                if (isTimesheetTaskVisible(task)) {
                    rows.push(createTimesheetRow(
                        task,
                        'task',
                        1,
                        taskTitle,
                        taskNumber + ' ' + taskTitle,
                        mileIndex,
                        taskIndex
                    ));
                }

                angular.forEach(task.subtasks || task.Subtasks || task.subtasklist || [], function (subtask, subtaskIndex) {
                    if (!isTimesheetSubtaskVisible(subtask)) return;

                    var subtaskNumber = taskNumber + '.' + String(subtaskIndex + 1);
                    var subtaskTitle = subtask.Subtaskname || subtask.subtaskname || subtask.SubTaskName || ('Subtask ' + subtaskNumber);
                    rows.push(createTimesheetRow(
                        subtask,
                        'subtask',
                        2,
                        subtaskTitle,
                        subtaskNumber + ' ' + subtaskTitle,
                        mileIndex,
                        taskIndex + '_' + subtaskIndex
                    ));
                });
            });
        });

        var rangeDates = [];
        var projectStart = parseTimesheetDate(($scope.projectlist || {}).StartDate);
        var projectEnd = parseTimesheetDate(($scope.projectlist || {}).TargetDate);
        if (projectStart) rangeDates.push(projectStart);
        if (projectEnd) rangeDates.push(projectEnd);

        angular.forEach(rows, function (row) {
            if (row.start) rangeDates.push(row.start);
            if (row.end) rangeDates.push(row.end);
        });

        var today = new Date();
        today = new Date(today.getFullYear(), today.getMonth(), today.getDate());
        var start = rangeDates.length ? new Date(Math.min.apply(null, rangeDates.map(function (x) { return x.getTime(); }))) : today;
        var end = rangeDates.length ? new Date(Math.max.apply(null, rangeDates.map(function (x) { return x.getTime(); }))) : today;
        if (end < start) end = start;

        var days = [];
        var totalDays = timesheetDiffDays(start, end) + 1;
        for (var dayIndex = 0; dayIndex < totalDays; dayIndex++) {
            var dayDate = timesheetAddDays(start, dayIndex);
            days.push({
                key: dayDate.getFullYear() + '_' + (dayDate.getMonth() + 1) + '_' + dayDate.getDate(),
                label: String(dayDate.getDate()).padStart(2, '0'),
                date: dayDate,
                isWeekend: dayDate.getDay() === 0 || dayDate.getDay() === 6
            });
        }

        var months = [];
        var monthCursor = new Date(start.getFullYear(), start.getMonth(), 1);
        while (monthCursor <= end) {
            var nextMonth = new Date(monthCursor.getFullYear(), monthCursor.getMonth() + 1, 1);
            var monthStart = monthCursor < start ? start : monthCursor;
            var monthLastDate = timesheetAddDays(nextMonth, -1);
            var monthEnd = monthLastDate > end ? end : monthLastDate;
            var monthDays = timesheetDiffDays(monthStart, monthEnd) + 1;

            months.push({
                key: monthCursor.getFullYear() + '_' + monthCursor.getMonth(),
                name: timesheetMonthName(monthCursor),
                days: monthDays,
                width: (monthDays * $scope.timesheetDayWidth) + 'px'
            });
            monthCursor = nextMonth;
        }

        angular.forEach(rows, function (row) {
            if (!row.hasDates) return;
            var leftDays = Math.max(0, timesheetDiffDays(start, row.start));
            var visibleEnd = row.end > end ? end : row.end;
            var widthDays = Math.max(1, timesheetDiffDays(row.start, visibleEnd) + 1);
            row.barStyle = {
                left: (leftDays * $scope.timesheetDayWidth) + 'px',
                width: (widthDays * $scope.timesheetDayWidth) + 'px',
                'background-color': row.statusColor
            };
        });

        $scope.timesheetRows = rows;
        $scope.timesheetDays = days;
        $scope.timesheetMonths = months;
        $scope.timesheetTimelineWidth = totalDays * $scope.timesheetDayWidth;
        $scope.timesheetStart = start;
        $scope.timesheetEnd = end;
        $scope.timesheetRangeLabel = timesheetMonthName(start) + (start.getMonth() === end.getMonth() && start.getFullYear() === end.getFullYear()
            ? ''
            : ' - ' + timesheetMonthName(end));
    };

    $scope.openTimesheetView = function () {
        $scope.isLoading2 = true;
        $timeout(function () {
            $scope.buildTimesheet();
            $scope.isLoading2 = false;
        }, 0);
    };

    function calendarDateOnly(value) {
        var parsed = parseTimesheetDate(value);
        return parsed ? new Date(parsed.getFullYear(), parsed.getMonth(), parsed.getDate()) : null;
    }

    function calendarItemDates(item, type) {
        item = item || {};
        var start = type === 'milestone'
            ? calendarDateOnly(item.StartDate || item.startDate)
            : calendarDateOnly(item.Startdate || item.StartDate || item.startdate);
        var end = type === 'milestone'
            ? calendarDateOnly(item.EndDate || item.TargetDate || item.endDate)
            : calendarDateOnly(item.Targetdate || item.TargetDate || item.EndDate || item.targetdate);

        if (!start && end) start = new Date(end);
        if (start && !end) end = new Date(start);
        if (start && end && end < start) {
            var swap = start;
            start = end;
            end = swap;
        }
        return { start: start, end: end };
    }

    function calendarItemTitle(item, type) {
        if (type === 'milestone') return item.Milestone || item.milestone || 'Milestone';
        if (type === 'task') return item.taskname || item.Taskname || item.TaskName || 'Task';
        return item.Subtaskname || item.subtaskname || item.SubTaskName || 'Subtask';
    }

    function calendarStatusColor(item) {
        var color = item && (item.status_color || item.StatusColor);
        if (color) return color;

        var statusId = $scope.getTopStatusId(item);
        var status = ($scope.statuslists || []).find(function (x) {
            return String(x.id) === statusId;
        });
        return (status && status.colorname) || '#6f42c1';
    }

    function calendarStatusName(item) {
        var name = item && (item.status_name || item.StatusName);
        if (name) return name;
        var statusId = $scope.getTopStatusId(item);
        var status = ($scope.statuslists || []).find(function (x) {
            return String(x.id) === statusId;
        });
        return (status && status.name) || '';
    }

    function calendarHierarchyDetails(item, type) {
        var taskCount = 0;
        var subtaskCount = 0;
        var children = [];

        if (type === 'milestone') {
            var tasks = item.tasks || item.Tasks || [];
            taskCount = tasks.length;
            angular.forEach(tasks, function (task) {
                var subtasks = task.subtasks || task.Subtasks || [];
                subtaskCount += subtasks.length;
                children.push({
                    type: 'task',
                    title: calendarItemTitle(task, 'task'),
                    countText: subtasks.length ? subtasks.length + ' subtasks' : 'No subtasks'
                });
                angular.forEach(subtasks, function (subtask) {
                    children.push({
                        type: 'subtask',
                        title: calendarItemTitle(subtask, 'subtask'),
                        countText: ''
                    });
                });
            });
        }
        else if (type === 'task') {
            var taskSubtasks = item.subtasks || item.Subtasks || [];
            subtaskCount = taskSubtasks.length;
            angular.forEach(taskSubtasks, function (subtask) {
                children.push({
                    type: 'subtask',
                    title: calendarItemTitle(subtask, 'subtask'),
                    countText: ''
                });
            });
        }

        return {
            taskCount: taskCount,
            subtaskCount: subtaskCount,
            children: children
        };
    }

    $scope.buildCalendar = function () {
        var base = $scope.calendarDate ? new Date($scope.calendarDate) : null;
        if (!base || isNaN(base.getTime())) {
            base = calendarDateOnly(($scope.projectlist || {}).StartDate) || new Date();
        }
        base = new Date(base.getFullYear(), base.getMonth(), 1);
        $scope.calendarDate = base;
        $scope.calendarTitle = base.toLocaleDateString('en-GB', { month: 'long', year: 'numeric' });

        var gridStart = new Date(base);
        gridStart.setDate(gridStart.getDate() - gridStart.getDay());
        var gridEnd = new Date(gridStart);
        gridEnd.setDate(gridEnd.getDate() + 41);

        var events = [];
        function addEvent(item, type) {
            if (type === 'milestone' && !$scope.hasMatchingMilestoneBranch(item)) return;
            if (type === 'task' && !$scope.hasMatchingTaskBranch(item)) return;
            if (type === 'subtask' && !$scope.matchesTopLeafFilters(item)) return;
            var dates = calendarItemDates(item, type);
            if (!dates.start || !dates.end || dates.end < gridStart || dates.start > gridEnd) return;
            var hierarchy = calendarHierarchyDetails(item, type);
            events.push({
                id: item.id,
                type: type,
                title: calendarItemTitle(item, type),
                start: dates.start,
                end: dates.end,
                color: calendarStatusColor(item),
                statusName: calendarStatusName(item),
                taskCount: hierarchy.taskCount,
                subtaskCount: hierarchy.subtaskCount,
                children: hierarchy.children,
                source: item
            });
        }

        angular.forEach((($scope.projectlist || {}).miles || []), function (mile) {
            addEvent(mile, 'milestone');
            angular.forEach(mile.tasks || mile.Tasks || [], function (task) {
                addEvent(task, 'task');
                angular.forEach(task.subtasks || task.Subtasks || [], function (subtask) {
                    addEvent(subtask, 'subtask');
                });
            });
        });

        var today = new Date();
        today = new Date(today.getFullYear(), today.getMonth(), today.getDate());
        var weeks = [];
        for (var weekIndex = 0; weekIndex < 6; weekIndex++) {
            var week = [];
            for (var dayIndex = 0; dayIndex < 7; dayIndex++) {
                var date = new Date(gridStart);
                date.setDate(gridStart.getDate() + (weekIndex * 7) + dayIndex);
                var dayEvents = events.filter(function (event) {
                    return date >= event.start && date <= event.end;
                });
                var counts = { milestone: 0, task: 0, subtask: 0 };
                angular.forEach(dayEvents, function (event) {
                    if (counts[event.type] !== undefined) counts[event.type]++;
                });
                week.push({
                    key: date.getFullYear() + '_' + (date.getMonth() + 1) + '_' + date.getDate(),
                    date: date,
                    day: date.getDate(),
                    isCurrentMonth: date.getMonth() === base.getMonth(),
                    isToday: date.getTime() === today.getTime(),
                    events: dayEvents,
                    counts: counts
                });
            }
            weeks.push(week);
        }
        $scope.calendarWeeks = weeks;
        var selectedKey = $scope.selectedCalendarDay && $scope.selectedCalendarDay.key;
        var selectedDay = null;
        angular.forEach(weeks, function (week) {
            angular.forEach(week, function (day) {
                if (!selectedDay && selectedKey && day.key === selectedKey) selectedDay = day;
                if (!selectedDay && !selectedKey && day.isToday) selectedDay = day;
            });
        });
        if (!selectedDay) {
            angular.forEach(weeks, function (week) {
                angular.forEach(week, function (day) {
                    if (!selectedDay && day.isCurrentMonth) selectedDay = day;
                });
            });
        }
        $scope.selectedCalendarDay = selectedDay;
    };

    $scope.selectCalendarDay = function (day, $event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }
        $scope.selectedCalendarDay = day || null;
    };

    $scope.openCalendarView = function () {
        $scope.isLoading2 = true;
        $timeout(function () {
            $scope.buildCalendar();
            $scope.isLoading2 = false;
        }, 0);
    };

    $scope.changeCalendarMonth = function (offset) {
        var current = $scope.calendarDate || new Date();
        $scope.calendarDate = new Date(current.getFullYear(), current.getMonth() + Number(offset || 0), 1);
        $scope.buildCalendar();
    };

    $scope.goCalendarToday = function () {
        var today = new Date();
        $scope.calendarDate = new Date(today.getFullYear(), today.getMonth(), 1);
        $scope.buildCalendar();
    };

    $scope.getTimesheetOwnerTitle = function (row) {
        return ((row && row.owners) || []).map(function (owner) {
            return owner.name || '';
        }).filter(Boolean).join(', ');
    };

    $scope.changeView = function (type, $event) {
        debugger
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }

        if (type === "gantt") $scope.ganttIsLoading = true;
        $scope.view = type;
        if (type === "kanban") {
            $scope.openKanbanView();
        }
        else if (type === "timesheet") {
            $scope.openTimesheetView();
        }
        else if (type === "calendar") {
            $scope.openCalendarView();
        }
        // load gantt when needed
        if (type === "gantt") {
            $timeout(function () {
                $scope.changeGanttScale($scope.ganttScale);
            }, 0);
        }
        else {
            $timeout(function () {
                initTableUI();
            }, 150);
        }
        let dropdown = bootstrap.Dropdown.getInstance(
            document.getElementById('btnGroupDrop1')
        );
        if (dropdown) {
            dropdown.hide();
        }
        console.log("View changed:", $scope.view); // debug
    };
    $scope.showTooltip = false;
    $scope.toggleTooltip = function ($event) {
        $event.stopPropagation();
        $scope.showTooltip = !$scope.showTooltip;
    };


    $document.on("click", function () {
        $scope.$apply(function () {
            $scope.showTooltip = false;
        });
    });


    $scope.$on('$destroy', function () {
        $document.off("click");
    });


    function cloneHierarchyCalendarDate(value) {
        var date = normalizeHierarchyDateOnly(value);
        if (!date) return null;
        return new Date(date.getFullYear(), date.getMonth(), date.getDate(), 12, 0, 0, 0);
    }

    function hierarchyDateInputValue(value) {
        var date = cloneHierarchyCalendarDate(value);
        if (!date) return "";

        var month = String(date.getMonth() + 1).padStart(2, "0");
        var day = String(date.getDate()).padStart(2, "0");
        return date.getFullYear() + "-" + month + "-" + day;
    }

    $scope.getHierarchyItemLabel = function (type, item) {
        item = item || {};
        var normalizedType = (type || "").toLowerCase();
        var label = '';
        //var label = normalizedType === "milestone"
        //    ? "Milestone"
        //    : normalizedType === "task"
        //        ? "Task"
        //        : "Subtask";

        var name = normalizedType === "milestone"
            ? (item.Milestone || item.milestone)
            : normalizedType === "task"
                ? (item.taskname || item.Taskname || item.TaskName)
                : (item.Subtaskname || item.subtaskname || item.SubTaskName);

        name = (name || "").toString().trim();
        return name ? label + ' ' + name + '' : label;
    };

    function hasValidHierarchyDurationDays(item) {
        if (!item) return false;

        var value = item.txtdays;
        if (value === null || value === undefined) return false;

        // 0 is valid. Only a truly blank duration-day value is blocked.
        return value.toString().trim() !== "";
    }

    function alertInvalidHierarchyDuration(type, item) {
        alert(
            $scope.getHierarchyItemLabel(type, item) +
            " duration days cannot be blank."
        );
    }

    function getHierarchyDurationDays(item) {
        if (!item || item.txtdays === null || item.txtdays === undefined) {
            return 0;
        }

        var value = Number(String(item.txtdays).trim());
        return isFinite(value) && value > 0 ? value : 0;
    }

    function getHierarchyDurationDaysTotal(items) {
        var total = 0;

        angular.forEach(items || [], function (item) {
            total += getHierarchyDurationDays(item);
        });

        return Math.round(total * 100) / 100;
    }

    function formatHierarchyDurationDays(value) {
        var number = Number(value || 0);
        if (!isFinite(number)) number = 0;
        return String(Math.round(number * 100) / 100);
    }

    function showSubtaskParentDurationAlert(changedItem, task, subtaskDays, taskDays) {
        alert(
            $scope.getHierarchyItemLabel('subtask', changedItem) +
            ' cannot be updated. Duration is ' +
            formatHierarchyDurationDays(subtaskDays) +
            ' days, which cannot be greater than ' +
            $scope.getHierarchyItemLabel('task', task) +
            ' duration of ' +
            formatHierarchyDurationDays(taskDays) +
            ' days.'
        );
    }

    function showTaskParentDurationAlert(changedItem, milestone, taskDays, milestoneDays) {
        alert(
            $scope.getHierarchyItemLabel('task', changedItem) +
            ' cannot be updated. Duration is ' +
            formatHierarchyDurationDays(taskDays) +
            ' days, which cannot be greater than ' +
            $scope.getHierarchyItemLabel('milestone', milestone) +
            ' duration of ' +
            formatHierarchyDurationDays(milestoneDays) +
            ' days.'
        );
    }

    function showMilestoneProjectDurationAlert(
        changedMilestone,
        milestoneDays,
        projectDays
    ) {
        alert(
            $scope.getHierarchyItemLabel('milestone', changedMilestone) +
            ' cannot be updated. Duration is ' +
            formatHierarchyDurationDays(milestoneDays) +
            ' days, which cannot be greater than Project duration of ' +
            formatHierarchyDurationDays(projectDays) +
            ' days.'
        );
    }

    function validateSubtaskAgainstTask(subtask, task) {
        var epsilon = 0.0001;
        var subtaskDays = getHierarchyDurationDays(subtask);
        var taskDays = getHierarchyDurationDays(task);

        if (subtaskDays > taskDays + epsilon) {
            showSubtaskParentDurationAlert(
                subtask,
                task,
                subtaskDays,
                taskDays
            );
            return false;
        }

        return true;
    }

    function validateTaskAgainstMilestone(task, milestone) {
        var epsilon = 0.0001;
        var taskDays = getHierarchyDurationDays(task);
        var milestoneDays = getHierarchyDurationDays(milestone);

        if (taskDays > milestoneDays + epsilon) {
            showTaskParentDurationAlert(
                task,
                milestone,
                taskDays,
                milestoneDays
            );
            return false;
        }

        return true;
    }

    function validateMilestoneAgainstProject(milestone) {
        var epsilon = 0.0001;
        var milestoneDays = getHierarchyDurationDays(milestone);
        var projectDays = Number($scope.projectDurationDays || 0);

        // Keep the existing safe behaviour when Project duration could not be
        // calculated. When available, validate only this milestone, not a total.
        if (
            projectDays > 0 &&
            milestoneDays > projectDays + epsilon
        ) {
            showMilestoneProjectDurationAlert(
                milestone,
                milestoneDays,
                projectDays
            );
            return false;
        }

        return true;
    }

    // Validates only the edited item against its direct parent before the
    // existing end-date calculation and database update continue.
    // Sibling duration days are intentionally not added together.
    function validateHierarchyDurationDayTotals(
        item,
        type,
        parent,
        updatefrom,
        restoreOldValue
    ) {
        if (updatefrom !== 'duration') return true;

        var normalizedType = String(type || '').toLowerCase();

        function restore() {
            if (typeof restoreOldValue === 'function') {
                restoreOldValue();
            }
        }

        if (normalizedType === 'subtask') {
            if (!validateSubtaskAgainstTask(item, parent)) {
                restore();
                return false;
            }
        }

        if (normalizedType === 'task') {
            var ownSubtasks = item && item.subtasks ? item.subtasks : [];

            for (var subtaskIndex = 0; subtaskIndex < ownSubtasks.length; subtaskIndex++) {
                if (!validateSubtaskAgainstTask(ownSubtasks[subtaskIndex], item)) {
                    restore();
                    return false;
                }
            }

            if (!validateTaskAgainstMilestone(item, parent)) {
                restore();
                return false;
            }
        }

        if (normalizedType === 'milestone') {
            var ownTasks = item && item.tasks ? item.tasks : [];

            for (var taskIndex = 0; taskIndex < ownTasks.length; taskIndex++) {
                if (!validateTaskAgainstMilestone(ownTasks[taskIndex], item)) {
                    restore();
                    return false;
                }
            }

            if (!validateMilestoneAgainstProject(item)) {
                restore();
                return false;
            }
        }

        return true;
    }

    // Validates every subtask separately against its parent task.
    function validateTaskDurationHierarchy(task) {
        var subtasks = task && task.subtasks ? task.subtasks : [];

        for (var subtaskIndex = 0; subtaskIndex < subtasks.length; subtaskIndex++) {
            if (!validateSubtaskAgainstTask(subtasks[subtaskIndex], task)) {
                return false;
            }
        }

        return true;
    }

    // Validates every task separately against its milestone and every subtask
    // separately against its task. No sibling totals are calculated.
    function validateMilestoneDurationHierarchy(milestone) {
        var tasks = milestone && milestone.tasks ? milestone.tasks : [];

        for (var taskIndex = 0; taskIndex < tasks.length; taskIndex++) {
            var task = tasks[taskIndex];

            if (!validateTaskDurationHierarchy(task)) {
                return false;
            }

            if (!validateTaskAgainstMilestone(task, milestone)) {
                return false;
            }
        }

        return true;
    }

    // Checks the complete affected hierarchy before any database update starts.
    // Blank-day validation and the existing reset/update flow remain unchanged.
    $scope.validateHierarchyDurationDays = function (type, parent, startIndex) {
        var normalizedType = String(type || '').toLowerCase();
        var index = parseInt(startIndex, 10);
        if (isNaN(index) || index < 0) index = 0;

        function validateSubtasksExist(subtasks, fromIndex) {
            subtasks = subtasks || [];
            for (var s = fromIndex || 0; s < subtasks.length; s++) {
                if (!hasValidHierarchyDurationDays(subtasks[s])) {
                    alertInvalidHierarchyDuration('subtask', subtasks[s]);
                    return false;
                }
            }
            return true;
        }

        function validateTasksExist(tasks, fromIndex) {
            tasks = tasks || [];

            for (var t = fromIndex || 0; t < tasks.length; t++) {
                var task = tasks[t];

                if (!hasValidHierarchyDurationDays(task)) {
                    alertInvalidHierarchyDuration('task', task);
                    return false;
                }

                if (!validateSubtasksExist(task.subtasks || [], 0)) {
                    return false;
                }

                if (!validateTaskDurationHierarchy(task)) {
                    return false;
                }
            }

            return true;
        }

        if (normalizedType === 'milestone') {
            var milestones = parent && parent.miles ? parent.miles : [];

            for (var m = index; m < milestones.length; m++) {
                var milestone = milestones[m];

                if (!hasValidHierarchyDurationDays(milestone)) {
                    alertInvalidHierarchyDuration('milestone', milestone);
                    return false;
                }

                if (!validateTasksExist(milestone.tasks || [], 0)) {
                    return false;
                }

                if (!validateMilestoneDurationHierarchy(milestone)) {
                    return false;
                }

                if (!validateMilestoneAgainstProject(milestone)) {
                    return false;
                }
            }

            return true;
        }

        if (normalizedType === 'task') {
            var tasks = parent && parent.tasks ? parent.tasks : [];

            if (!validateTasksExist(tasks, index)) {
                return false;
            }

            // Preserve the existing complete-parent check, but validate every
            // task and subtask individually instead of adding sibling days.
            if (!validateMilestoneDurationHierarchy(parent)) {
                return false;
            }

            return true;
        }

        if (normalizedType === 'subtask') {
            var subtasks = parent && parent.subtasks
                ? parent.subtasks
                : [];

            if (!validateSubtasksExist(subtasks, index)) {
                return false;
            }

            // Check each subtask separately against the same parent task.
            for (var subtaskIndex = 0; subtaskIndex < subtasks.length; subtaskIndex++) {
                if (!validateSubtaskAgainstTask(subtasks[subtaskIndex], parent)) {
                    return false;
                }
            }

            return true;
        }

        return false;
    };

    // Opens only the browser calendar. No input box is displayed to the user.
    $scope.openHierarchyStartDateCalendar = function (item, type, parent, index, $event) {
        
        if (!$scope.requireEdit(type)) return;
        if (!item || !$event || !$event.currentTarget) return;

        var normalizedType = (type || "").toLowerCase();
        var currentDate = normalizedType === "milestone"
            ? item.StartDate
            : item.Startdate;

        if (normalizedType === "milestone") {
            item._oldStartDate = cloneHierarchyCalendarDate(currentDate);
        } else {
            item._oldStartdate = cloneHierarchyCalendarDate(currentDate);
        }

        item._pendingHierarchyStartDate = cloneHierarchyCalendarDate(currentDate);

        var picker = $event.currentTarget.querySelector(
            "input.hierarchy-startdate-picker"
        );

        if (!picker) return;

        picker.value = hierarchyDateInputValue(currentDate);

        try {
            if (typeof picker.showPicker === "function") {
                picker.showPicker();
            } else {
                picker.focus();
                picker.click();
            }
        } catch (ex) {
            picker.focus();
            picker.click();
        }
    };

    // Update only the selected item's start date.
    // Hierarchy reset methods are intentionally NOT called here;
    // they continue to run only when the user clicks the reset button manually.
    $scope.applyHierarchyStartDate = function (
        item,
        type,
        parent,
        index,
        selectedDateValue
    ) {
        if (!$scope.requireEdit(type)) return;
        if (!item) return;

        var normalizedType = (type || "").toLowerCase();
        var selectedDate = cloneHierarchyCalendarDate(
            selectedDateValue || item._pendingHierarchyStartDate
        );
        if (!selectedDate) return;

        var currentDate = normalizedType === "milestone"
            ? item.StartDate
            : item.Startdate;

        // Do nothing when the calendar is only opened/navigated or the user
        // commits the same date. Validation and save must run only for a new date.
        if (
            hierarchyDateInputValue(currentDate) ===
            hierarchyDateInputValue(selectedDate)
        ) {
            item._pendingHierarchyStartDate = cloneHierarchyCalendarDate(currentDate);
            return;
        }

        item._pendingHierarchyStartDate = cloneHierarchyCalendarDate(selectedDate);

        if (normalizedType === "milestone") {
            item.StartDate = selectedDate;

            // Existing validation:
            // Project StartDate <= Milestone StartDate/EndDate <= Project TargetDate
            $scope.update_duration(item, "milestone", "startdate");
        } else if (normalizedType === "task") {
            item.Startdate = selectedDate;

            // Existing validation:
            // Milestone StartDate <= Task StartDate/TargetDate <= Milestone EndDate
            $scope.update_duration_task(item, parent, "startdate");
        } else if (normalizedType === "subtask") {
            item.Startdate = selectedDate;

            // Existing validation:
            // Task Startdate <= Subtask Startdate/Targetdate <= Task Targetdate
            $scope.update_duration_subtask(item, parent, "startdate");
        }
    };

    $scope.resetStartDateHierarchy = function (node) {

        function reset(item) {
            if (!item) return;
            item.StartDate = baseDate;
            item.Startdate = baseDate;
            if (item.tasks && item.tasks.length > 0) {
                item.tasks.forEach(function (t) {
                    // baseDate = t.StartDate || t.Startdate;
                    reset(t);
                });
            }
            if (item.subtasks && item.subtasks.length > 0) {
                item.subtasks.forEach(function (s) {
                    //baseDate = s.StartDate || s.Startdate;
                    reset(s);
                });
            }


        }
        var baseDate = node.StartDate || node.Startdate;
        if (!baseDate) return;
        reset(node);


    };


    $scope.resetStartDateHierarchy_subtask = async function (node, subtasks, index, updatefrom) {
        if (!$scope.requireEdit('subtask')) return;
        if (!node || !subtasks || !subtasks.subtasks || subtasks.subtasks.length === 0) return;

        var startIndex = Math.max(0, parseInt(index, 10) || 0);

        // Working hours are checked first, before duration days or any database update.
        if (!validateHierarchyResetBranchHours(
            'subtask',
            subtasks.subtasks,
            startIndex
        )) {
            return;
        }

        if (!$scope.validateHierarchyDurationDays('subtask', subtasks, index)) return;

        var parentTaskLabel = $scope.getHierarchyItemLabel('task', subtasks);
        var parentStart = normalizeHierarchyDateOnly(subtasks.Startdate);
        var parentEnd = normalizeHierarchyDateOnly(subtasks.Targetdate);
        var baseCursor = createHierarchyResetCursor(
            node.Startdate || node.StartDate,
            node
        );

        if (!parentStart || !parentEnd || !baseCursor) {
            alert(parentTaskLabel + ' or selected Subtask date is invalid.');
            return;
        }

        for (let z = startIndex; z < subtasks.subtasks.length; z++) {
            let currentSubTask = subtasks.subtasks[z];
            let itemLabel = $scope.getHierarchyItemLabel('subtask', currentSubTask);
            let resetStart = applyHierarchyResetCursor(
                currentSubTask,
                'subtask',
                baseCursor
            );
            let start = resetStart && resetStart.date;
            let startTime = resetStart && resetStart.time;

            function restoreCurrentSubtask() {
                currentSubTask._isUpdating = true;
                if (updatefrom === 'duration') {
                    currentSubTask.txtdays = angular.copy(currentSubTask._oldtxtdays);
                } else {
                    currentSubTask.Startdate = angular.copy(currentSubTask._oldStartdate);
                    currentSubTask.Targetdate = angular.copy(currentSubTask._oldTargetdate);
                    currentSubTask.StartTime = angular.copy(currentSubTask._oldStartTime);
                    currentSubTask.starttime = angular.copy(currentSubTask._oldStartTime);
                }
                currentSubTask._isUpdating = false;
            }

            if (!start || start < parentStart) {
                alert(itemLabel + ' start date cannot be before ' + parentTaskLabel + ' start date.');
                restoreCurrentSubtask();
                return;
            }

            if (start > parentEnd) {
                alert(itemLabel + ' start date cannot be after ' + parentTaskLabel + ' end date.');
                restoreCurrentSubtask();
                return;
            }

            var startText = toServerDateOnly(start);
            try {
                var endResponse = await assignprojectService.getnew_enddate(
                    startText,
                    currentSubTask.txtdays,
                    resolveHierarchyProjectId(currentSubTask)
                );
                var end = hierarchyResponseDate(endResponse);
                if (!end) throw new Error('Unable to calculate end date.');

                if (end > parentEnd) {
                    alert(itemLabel + ' end date cannot be after ' + parentTaskLabel + ' end date.');
                    restoreCurrentSubtask();
                    return;
                }

                if (end < start) {
                    alert(itemLabel + ' end date cannot be before Start date.');
                    restoreCurrentSubtask();
                    return;
                }

                currentSubTask.Targetdate = cloneHierarchyDateOnly(end);

                var updateResponse = await assignprojectService.updatetimeline_subtask(
                    currentSubTask.id,
                    currentSubTask.txtdays,
                    startText,
                    startTime,
                    $scope.perdayhrs,
                    ''
                );

                var savedValue = updateResponse && updateResponse.data
                    ? updateResponse.data.d
                    : null;
                if (savedValue && String(savedValue).indexOf('ERROR|') === 0) {
                    throw new Error(String(savedValue).substring(6));
                }

                var savedEnd = hierarchyResponseDate(updateResponse);
                if (savedEnd) {
                    currentSubTask.Targetdate = cloneHierarchyDateOnly(savedEnd);
                    end = savedEnd;
                }

                currentSubTask._oldStartdate = angular.copy(currentSubTask.Startdate);
                currentSubTask._oldTargetdate = angular.copy(currentSubTask.Targetdate);
                currentSubTask._oldStartTime = angular.copy(currentSubTask.StartTime);
                currentSubTask._oldtxtdays = angular.copy(currentSubTask.txtdays);

                await refreshHierarchyLeftDays(currentSubTask, 'subtask');
                baseCursor = await advanceHierarchyResetCursorByHours(
                    baseCursor,
                    currentSubTask
                );
            } catch (error) {
                console.error('Subtask hierarchy reset failed', error);
                alert(error && error.message
                    ? error.message
                    : 'Failed to update ' + itemLabel + '.');
                restoreCurrentSubtask();
                return;
            }
        }

        $scope.syncListAndKanbanViews();
    };

    $scope.resetStartDateHierarchy_task = async function (node, tasks, index, updatefrom) {
        if (!$scope.requireEdit('task')) return;
        if (!node || !tasks || !tasks.tasks || tasks.tasks.length === 0) return;

        var startIndex = Math.max(0, parseInt(index, 10) || 0);

        // Working hours are checked first for tasks and their subtasks.
        if (!validateHierarchyResetBranchHours(
            'task',
            tasks.tasks,
            startIndex
        )) {
            return;
        }

        if (!$scope.validateHierarchyDurationDays('task', tasks, index)) return;

        var milestoneLabel = $scope.getHierarchyItemLabel('milestone', tasks);
        var milestoneStart = normalizeHierarchyDateOnly(tasks.StartDate);
        var milestoneEnd = normalizeHierarchyDateOnly(tasks.EndDate);
        var baseCursor = createHierarchyResetCursor(
            node.Startdate || node.StartDate,
            node
        );

        if (!milestoneStart || !milestoneEnd || !baseCursor) {
            alert(milestoneLabel + ' or selected Task date is invalid.');
            return;
        }

        for (let y = startIndex; y < tasks.tasks.length; y++) {
            let currentTask = tasks.tasks[y];
            let taskLabel = $scope.getHierarchyItemLabel('task', currentTask);
            let resetTaskStart = applyHierarchyResetCursor(
                currentTask,
                'task',
                baseCursor
            );
            let taskStart = resetTaskStart && resetTaskStart.date;
            let taskStartTime = resetTaskStart && resetTaskStart.time;

            function restoreCurrentTask() {
                currentTask._isUpdating = true;
                if (updatefrom === 'duration') {
                    currentTask.txtdays = angular.copy(currentTask._oldtxtdays);
                } else {
                    currentTask.Startdate = angular.copy(currentTask._oldStartdate);
                    currentTask.Targetdate = angular.copy(currentTask._oldTargetdate);
                    currentTask.StartTime = angular.copy(currentTask._oldStartTime);
                    currentTask.starttime = angular.copy(currentTask._oldStartTime);
                }
                currentTask._isUpdating = false;
            }

            if (!taskStart || taskStart < milestoneStart) {
                alert(taskLabel + ' start date cannot be before ' + milestoneLabel + ' start date.');
                restoreCurrentTask();
                return;
            }

            if (taskStart > milestoneEnd) {
                alert(taskLabel + ' start date cannot be after ' + milestoneLabel + ' end date.');
                restoreCurrentTask();
                return;
            }

            var taskStartText = toServerDateOnly(taskStart);
            var taskEnd;
            try {
                var taskEndResponse = await assignprojectService.getnew_enddate(
                    taskStartText,
                    currentTask.txtdays,
                    resolveHierarchyProjectId(currentTask)
                );
                taskEnd = hierarchyResponseDate(taskEndResponse);
                if (!taskEnd) throw new Error('Unable to calculate task end date.');

                if (taskEnd > milestoneEnd) {
                    alert(taskLabel + ' end date cannot be after ' + milestoneLabel + ' end date.');
                    restoreCurrentTask();
                    return;
                }

                if (taskEnd < taskStart) {
                    alert(taskLabel + ' end date cannot be before Start date.');
                    restoreCurrentTask();
                    return;
                }

                currentTask.Targetdate = cloneHierarchyDateOnly(taskEnd);

                var taskUpdateResponse = await assignprojectService.updatetimeline_task(
                    currentTask.id,
                    currentTask.txtdays,
                    taskStartText,
                    taskStartTime,
                    $scope.perdayhrs,
                    ''
                );

                var taskSavedValue = taskUpdateResponse && taskUpdateResponse.data
                    ? taskUpdateResponse.data.d
                    : null;
                if (taskSavedValue && String(taskSavedValue).indexOf('ERROR|') === 0) {
                    throw new Error(String(taskSavedValue).substring(6));
                }

                var savedTaskEnd = hierarchyResponseDate(taskUpdateResponse);
                if (savedTaskEnd) {
                    currentTask.Targetdate = cloneHierarchyDateOnly(savedTaskEnd);
                    taskEnd = savedTaskEnd;
                }

                currentTask._oldStartdate = angular.copy(currentTask.Startdate);
                currentTask._oldTargetdate = angular.copy(currentTask.Targetdate);
                currentTask._oldStartTime = angular.copy(currentTask.StartTime);
                currentTask._oldtxtdays = angular.copy(currentTask.txtdays);

                await refreshHierarchyLeftDays(currentTask, 'task');
            } catch (error) {
                console.error('Task hierarchy reset failed', error);
                alert(error && error.message
                    ? error.message
                    : 'Failed to update ' + taskLabel + '.');
                restoreCurrentTask();
                return;
            }

            if (currentTask.subtasks && currentTask.subtasks.length > 0) {
                var subtaskBaseCursor = createHierarchyResetCursor(
                    currentTask.Startdate,
                    currentTask
                );
                var parentTaskStart = cloneHierarchyDateOnly(currentTask.Startdate);
                var parentTaskEnd = cloneHierarchyDateOnly(currentTask.Targetdate);

                for (let z = 0; z < currentTask.subtasks.length; z++) {
                    let currentSubTask = currentTask.subtasks[z];
                    let subtaskLabel = $scope.getHierarchyItemLabel('subtask', currentSubTask);
                    let resetSubtaskStart = applyHierarchyResetCursor(
                        currentSubTask,
                        'subtask',
                        subtaskBaseCursor
                    );
                    let subtaskStart = resetSubtaskStart && resetSubtaskStart.date;
                    let subtaskStartTime = resetSubtaskStart && resetSubtaskStart.time;

                    if (!subtaskStart || subtaskStart < parentTaskStart) {
                        alert(subtaskLabel + ' start date cannot be before ' + taskLabel + ' start date.');
                        return;
                    }

                    if (subtaskStart > parentTaskEnd) {
                        alert(subtaskLabel + ' start date cannot be after ' + taskLabel + ' end date.');
                        return;
                    }

                    var subtaskStartText = toServerDateOnly(subtaskStart);
                    try {
                        var subtaskEndResponse = await assignprojectService.getnew_enddate(
                            subtaskStartText,
                            currentSubTask.txtdays,
                            resolveHierarchyProjectId(currentSubTask)
                        );
                        var subtaskEnd = hierarchyResponseDate(subtaskEndResponse);
                        if (!subtaskEnd) throw new Error('Unable to calculate subtask end date.');

                        if (subtaskEnd > parentTaskEnd) {
                            alert(subtaskLabel + ' end date cannot be after ' + taskLabel + ' end date.');
                            return;
                        }

                        if (subtaskEnd < subtaskStart) {
                            alert(subtaskLabel + ' end date cannot be before Start date.');
                            return;
                        }

                        currentSubTask.Targetdate = cloneHierarchyDateOnly(subtaskEnd);

                        var subtaskUpdateResponse = await assignprojectService.updatetimeline_subtask(
                            currentSubTask.id,
                            currentSubTask.txtdays,
                            subtaskStartText,
                            subtaskStartTime,
                            $scope.perdayhrs,
                            ''
                        );

                        var subtaskSavedValue = subtaskUpdateResponse && subtaskUpdateResponse.data
                            ? subtaskUpdateResponse.data.d
                            : null;
                        if (subtaskSavedValue && String(subtaskSavedValue).indexOf('ERROR|') === 0) {
                            throw new Error(String(subtaskSavedValue).substring(6));
                        }

                        var savedSubtaskEnd = hierarchyResponseDate(subtaskUpdateResponse);
                        if (savedSubtaskEnd) {
                            currentSubTask.Targetdate = cloneHierarchyDateOnly(savedSubtaskEnd);
                            subtaskEnd = savedSubtaskEnd;
                        }

                        currentSubTask._oldStartdate = angular.copy(currentSubTask.Startdate);
                        currentSubTask._oldTargetdate = angular.copy(currentSubTask.Targetdate);
                        currentSubTask._oldStartTime = angular.copy(currentSubTask.StartTime);
                        currentSubTask._oldtxtdays = angular.copy(currentSubTask.txtdays);

                        await refreshHierarchyLeftDays(currentSubTask, 'subtask');
                        subtaskBaseCursor = await advanceHierarchyResetCursorByHours(
                            subtaskBaseCursor,
                            currentSubTask
                        );
                    } catch (error) {
                        console.error('Subtask hierarchy reset failed', error);
                        alert(error && error.message
                            ? error.message
                            : 'Failed to update ' + subtaskLabel + '.');
                        return;
                    }
                }
            }

            // The next task start is hour based. Duration days control only
            // the current task Targetdate; they must not force the next task
            // to the date after Targetdate.
            baseCursor = await advanceHierarchyResetCursorByHours(
                baseCursor,
                currentTask
            );
        }

        $scope.syncListAndKanbanViews();
    };
    $scope.resetStartDateHierarchy_mile = async function (
        node,
        projectlist,
        index,
        updatefrom
    ) {
        debugger
        if (!$scope.requireEdit('milestone')) return;

        if (
            !node ||
            !projectlist ||
            !projectlist.miles ||
            projectlist.miles.length === 0
        ) {
            return;
        }

        var startIndex = parseInt(index, 10);
        if (isNaN(startIndex) || startIndex < 0) {
            startIndex = 0;
        }

        // Working hours are checked first for the complete affected branch.
        if (!validateHierarchyResetBranchHours(
            'milestone',
            projectlist.miles,
            startIndex
        )) {
            return;
        }

        if (
            !$scope.validateHierarchyDurationDays(
                'milestone',
                projectlist,
                index
            )
        ) {
            return;
        }

        var projectStart = normalizeHierarchyDateOnly(
            projectlist.StartDate ||
            ($scope.projectlist && $scope.projectlist.StartDate)
        );

        var projectEnd = normalizeHierarchyDateOnly(
            projectlist.TargetDate ||
            ($scope.projectlist && $scope.projectlist.TargetDate)
        );

        var baseCursor = createHierarchyResetCursor(
            node.StartDate || node.Startdate,
            node
        );

        if (!projectStart || !projectEnd || !baseCursor) {
            alert('Project or selected Milestone date is invalid.');
            return;
        }

        /*
         * Date-only display formatter.
         * It avoids browser/global timezone date shifting.
         */
        function formatPopupDate(value) {
            var dateValue = normalizeHierarchyDateOnly(value);

            if (!dateValue) return '';

            var day = String(dateValue.getDate()).padStart(2, '0');
            var month = String(dateValue.getMonth() + 1).padStart(2, '0');
            var year = dateValue.getFullYear();

            return day + '/' + month + '/' + year;
        }

        for (
            let x = startIndex;
            x < projectlist.miles.length;
            x++
        ) {
            let currentMile = projectlist.miles[x];

            let milestoneLabel =
                $scope.getHierarchyItemLabel(
                    'milestone',
                    currentMile
                );

            let resetMilestoneStart =
                applyHierarchyResetCursor(
                    currentMile,
                    'milestone',
                    baseCursor
                );

            let milestoneStart =
                resetMilestoneStart &&
                resetMilestoneStart.date;

            let milestoneStartTime =
                resetMilestoneStart &&
                resetMilestoneStart.time;

            function restoreCurrentMilestone() {
                currentMile._isUpdating = true;

                if (updatefrom === 'duration') {
                    currentMile.txtdays =
                        angular.copy(
                            currentMile._oldtxtdays
                        );
                } else {
                    currentMile.StartDate =
                        angular.copy(
                            currentMile._oldStartDate
                        );

                    currentMile.EndDate =
                        angular.copy(
                            currentMile._oldEndDate
                        );

                    currentMile.StartTime =
                        angular.copy(
                            currentMile._oldStartTime
                        );

                    currentMile.starttime =
                        angular.copy(
                            currentMile._oldStartTime
                        );
                }

                currentMile._isUpdating = false;
            }

            currentMile.StartDate =
                cloneHierarchyDateOnly(milestoneStart);

            if (milestoneStart < projectStart) {
                alert(
                    milestoneLabel +
                    ' start date cannot be before Project start date.'
                );

                restoreCurrentMilestone();
                return;
            }

            if (milestoneStart > projectEnd) {
                alert(
                    milestoneLabel +' start date cannot be after Project end date.'
                );

                restoreCurrentMilestone();
                return;
            }
            //if (milestoneStart > projectEnd) {
            //    alert(
            //        milestoneLabel +' start date cannot be after Project end date.'
            //    );
            //    var milestoneEndResponse1 =await assignprojectService.getnew_enddate(milestoneStartText,currentMile.txtdays);
            //    var milestoneEnd1 =hierarchyResponseDate(milestoneEndResponse1);
            //    var existingLastDate1 = formatPopupDate(projectEnd);
            //    var newLastDate1 = formatPopupDate(milestoneEnd1);

            //    var confirmResult1 = await Swal.fire({
            //        title: "Extend Timeline?",
            //        html:
            //            "The existing last date is <b>" +
            //            existingLastDate1 +
            //            "</b> and the new last date shall be <b>" +
            //            newLastDate1 +
            //            "</b>.<br><br>" +
            //            "Do you want to extend timeline?",
            //        icon: "warning",
            //        showCancelButton: true,
            //        confirmButtonText: "Yes",
            //        cancelButtonText: "No",
            //        allowOutsideClick: false,
            //        allowEscapeKey: false
            //    });
            //    if (!confirmResult1 ||!confirmResult1.isConfirmed) {
            //        restoreCurrentMilestone();
            //        return;
            //    }
            //    var projectId =
            //        projectlist.id ||
            //        projectlist.ProjectId ||
            //        currentMile.ProjectId ||
            //        ($scope.projectlist &&
            //            (
            //                $scope.projectlist.id ||
            //                $scope.projectlist.ProjectId
            //            ));

            //    if (!projectId) {
            //        await Swal.fire({
            //            title: "Timeline Not Extended",
            //            text: "Project ID was not found.",
            //            icon: "error",
            //            confirmButtonText: "OK"
            //        });

            //        restoreCurrentMilestone();
            //        return;
            //    }
            //    debugger
            //    try {
            //        var projectstartdate = toServerDateOnly(projectlist.StartDate);
            //        var newProjectTargetDate =toServerDateOnly(milestoneEnd1);

            //        var extendResponse = await assignprojectService.extendProjectTimeline(projectId, projectstartdate, newProjectTargetDate);

            //        var extendResult = extendResponse && extendResponse.data ? extendResponse.data.d : null;

            //        if (
            //            extendResult === null ||
            //            extendResult === undefined ||
            //            extendResult === '' ||
            //            extendResult === '0' ||
            //            extendResult === 0 ||
            //            extendResult === false
            //        ) {
            //            throw new Error(
            //                'Project timeline was not updated.'
            //            );
            //        }

            //        /*
            //         * WebMethod may return yyyy-MM-dd or
            //         * stored procedure history identity.
            //         */
            //        debugger
            //        var updatedProjectEnd;

            //        updatedProjectEnd = cloneHierarchyDateOnly(milestoneEnd);


            //        if (!updatedProjectEnd) {
            //            updatedProjectEnd = cloneHierarchyDateOnly(milestoneEnd);
            //        }

            //        /*
            //         * Update Angular project date after
            //         * successful database update.
            //         */
            //        projectEnd = cloneHierarchyDateOnly(updatedProjectEnd);

            //        projectlist.TargetDate = toServerDateOnly(updatedProjectEnd);

            //        if ($scope.projectlist) {
            //            $scope.projectlist.TargetDate =
            //                toServerDateOnly(
            //                    updatedProjectEnd
            //                );
            //        }

            //    } catch (extendError) {
            //        console.error(
            //            'Project timeline extension failed',
            //            extendError
            //        );
            //        debugger
            //        await Swal.fire({
            //            title: "Timeline Not Extended",
            //            text:
            //                "Project TargetDate could not be updated. " +
            //                "The milestone update has been cancelled.",
            //            icon: "error",
            //            confirmButtonText: "OK"
            //        });

            //        restoreCurrentMilestone();
            //        return;
            //    }
            //    //restoreCurrentMilestone();
            //    //return;
            //}
            var milestoneStartText =toServerDateOnly(milestoneStart);

            var milestoneEnd;
            try {
                var milestoneEndResponse =
                    await assignprojectService.getnew_enddate(
                        milestoneStartText,
                        currentMile.txtdays,
                        resolveHierarchyProjectId(currentMile)
                    );

                milestoneEnd =
                    hierarchyResponseDate(
                        milestoneEndResponse
                    );

                if (!milestoneEnd) {
                    throw new Error(
                        'Unable to calculate milestone end date.'
                    );
                }

                if (milestoneEnd < milestoneStart) {
                    alert(
                        milestoneLabel +
                        ' end date cannot be before Start date.'
                    );

                    restoreCurrentMilestone();
                    return;
                }

                /*
                 * Third condition:
                 * If milestone end date exceeds project TargetDate,
                 * ask whether project timeline should be extended.
                 */
                if (milestoneEnd > projectEnd) {
                    var existingLastDate =formatPopupDate(projectEnd);

                    var newLastDate =formatPopupDate(milestoneEnd);

                    var confirmResult = await Swal.fire({
                        title: "Extend Timeline?",
                        html:
                            "The existing last date is <b>" +
                            existingLastDate +
                            "</b> and the new last date shall be <b>" +
                            newLastDate +
                            "</b>.<br><br>" +
                            "Do you want to extend timeline?",
                        icon: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Yes",
                        cancelButtonText: "No",
                        allowOutsideClick: false,
                        allowEscapeKey: false
                    });

                    /*
                     * No clicked:
                     * restore milestone values and stop.
                     */
                    if (
                        !confirmResult ||
                        !confirmResult.isConfirmed
                    ) {
                        restoreCurrentMilestone();
                        return;
                    }
                    debugger
                    var projectId =
                        projectlist.id ||
                        projectlist.ProjectId ||
                        currentMile.ProjectId ||
                        ($scope.projectlist &&
                            (
                                $scope.projectlist.id ||
                                $scope.projectlist.ProjectId
                            ));
                    
                    if (!projectId) {
                        await Swal.fire({
                            title: "Timeline Not Extended",
                            text: "Project ID was not found.",
                            icon: "error",
                            confirmButtonText: "OK"
                        });

                        restoreCurrentMilestone();
                        return;
                    }
                    debugger
                    try {
                        var projectstartdate = toServerDateOnly(projectlist.StartDate);
                        var newProjectTargetDate =
                            toServerDateOnly(milestoneEnd);

                        /*
                         * This service must:
                         *
                         * 1. Update Pms_tbl_AddProject.TargetDate
                         * 2. Insert history using
                         *    pmstbl_Extnddate_DeatilsUpdate
                         * 3. Return date, history ID or another
                         *    non-zero success value.
                         */
                        var extendResponse =await assignprojectService.extendProjectTimeline(projectId,projectstartdate,newProjectTargetDate);

                        var extendResult =extendResponse &&extendResponse.data? extendResponse.data.d: null;

                        if (
                            extendResult === null ||
                            extendResult === undefined ||
                            extendResult === '' ||
                            extendResult === '0' ||
                            extendResult === 0 ||
                            extendResult === false
                        ) {
                            throw new Error(
                                'Project timeline was not updated.'
                            );
                        }

                        /*
                         * WebMethod may return yyyy-MM-dd or
                         * stored procedure history identity.
                         */
                        debugger
                        var updatedProjectEnd;

                          updatedProjectEnd =cloneHierarchyDateOnly(milestoneEnd);
                        

                        if (!updatedProjectEnd) {
                            updatedProjectEnd =cloneHierarchyDateOnly(milestoneEnd);
                        }

                        /*
                         * Update Angular project date after
                         * successful database update.
                         */
                        projectEnd =cloneHierarchyDateOnly(updatedProjectEnd);

                        projectlist.TargetDate =toServerDateOnly(updatedProjectEnd);

                        if ($scope.projectlist) {
                            $scope.projectlist.TargetDate =
                                toServerDateOnly(
                                    updatedProjectEnd
                                );
                        }

                    } catch (extendError) {
                        console.error(
                            'Project timeline extension failed',
                            extendError
                        );
                        debugger
                        await Swal.fire({
                            title: "Timeline Not Extended",
                            text:
                                "Project TargetDate could not be updated. " +
                                "The milestone update has been cancelled.",
                            icon: "error",
                            confirmButtonText: "OK"
                        });

                        restoreCurrentMilestone();
                        return;
                    }
                }

                currentMile.EndDate =
                    cloneHierarchyDateOnly(
                        milestoneEnd
                    );

                var milestoneUpdateResponse =
                    await assignprojectService.updatetimeline_mile(
                        currentMile.id,
                        currentMile.txtdays,
                        milestoneStartText,
                        milestoneStartTime,
                        $scope.perdayhrs,
                        ''
                    );

                var savedMilestoneEnd =
                    hierarchyResponseDate(
                        milestoneUpdateResponse
                    );

                if (savedMilestoneEnd) {
                    currentMile.EndDate =
                        cloneHierarchyDateOnly(
                            savedMilestoneEnd
                        );

                    milestoneEnd =
                        savedMilestoneEnd;
                }

                currentMile._oldStartDate =
                    angular.copy(
                        currentMile.StartDate
                    );

                currentMile._oldEndDate =
                    angular.copy(
                        currentMile.EndDate
                    );

                currentMile._oldStartTime =
                    angular.copy(
                        currentMile.StartTime
                    );

                currentMile._oldtxtdays =
                    angular.copy(
                        currentMile.txtdays
                    );

                await refreshHierarchyLeftDays(
                    currentMile,
                    'milestone'
                );

            } catch (error) {
                console.error(
                    'Milestone hierarchy reset failed',
                    error
                );

                alert(
                    'Failed to update ' +
                    milestoneLabel +
                    '.'
                );

                restoreCurrentMilestone();
                return;
            }

            /*
             * Update all tasks of the milestone.
             */
            if (
                currentMile.tasks &&
                currentMile.tasks.length > 0
            ) {
                var taskBaseCursor =
                    createHierarchyResetCursor(
                        currentMile.StartDate,
                        currentMile
                    );

                var parentMileStart =
                    cloneHierarchyDateOnly(
                        currentMile.StartDate
                    );

                var parentMileEnd =
                    cloneHierarchyDateOnly(
                        currentMile.EndDate
                    );

                for (
                    let y = 0;
                    y < currentMile.tasks.length;
                    y++
                ) {
                    let currentTask =
                        currentMile.tasks[y];

                    let taskLabel =
                        $scope.getHierarchyItemLabel(
                            'task',
                            currentTask
                        );

                    let resetTaskStart =
                        applyHierarchyResetCursor(
                            currentTask,
                            'task',
                            taskBaseCursor
                        );

                    let taskStart =
                        resetTaskStart &&
                        resetTaskStart.date;

                    let taskStartTime =
                        resetTaskStart &&
                        resetTaskStart.time;

                    if (taskStart < parentMileStart) {
                        alert(
                            taskLabel +
                            ' start date cannot be before ' +
                            milestoneLabel +
                            ' start date.'
                        );

                        return;
                    }

                    if (taskStart > parentMileEnd) {
                        alert(
                            taskLabel +
                            ' start date cannot be after ' +
                            milestoneLabel +
                            ' end date.'
                        );

                        return;
                    }

                    var taskStartText =
                        toServerDateOnly(taskStart);

                    var taskEnd;
                    try {
                        var taskEndResponse =
                            await assignprojectService.getnew_enddate(
                                taskStartText,
                                currentTask.txtdays,
                                resolveHierarchyProjectId(currentTask)
                            );

                        taskEnd =
                            hierarchyResponseDate(
                                taskEndResponse
                            );

                        if (!taskEnd) {
                            throw new Error(
                                'Unable to calculate task end date.'
                            );
                        }

                        if (taskEnd > parentMileEnd) {
                            alert(
                                taskLabel +
                                ' end date cannot be after ' +
                                milestoneLabel +
                                ' end date.'
                            );

                            return;
                        }

                        if (taskEnd < taskStart) {
                            alert(
                                taskLabel +
                                ' end date cannot be before Start date.'
                            );

                            return;
                        }

                        currentTask.Targetdate =
                            cloneHierarchyDateOnly(
                                taskEnd
                            );

                        var taskUpdateResponse =
                            await assignprojectService
                                .updatetimeline_task(
                                    currentTask.id,
                                    currentTask.txtdays,
                                    taskStartText,
                                    taskStartTime,
                                    $scope.perdayhrs,
                                    ''
                                );

                        var savedTaskEnd =
                            hierarchyResponseDate(
                                taskUpdateResponse
                            );

                        if (savedTaskEnd) {
                            currentTask.Targetdate =
                                cloneHierarchyDateOnly(
                                    savedTaskEnd
                                );

                            taskEnd =
                                savedTaskEnd;
                        }

                        currentTask._oldStartdate =
                            angular.copy(
                                currentTask.Startdate
                            );

                        currentTask._oldTargetdate =
                            angular.copy(
                                currentTask.Targetdate
                            );

                        currentTask._oldStartTime =
                            angular.copy(
                                currentTask.StartTime
                            );

                        currentTask._oldtxtdays =
                            angular.copy(
                                currentTask.txtdays
                            );

                        await refreshHierarchyLeftDays(
                            currentTask,
                            'task'
                        );

                    } catch (error) {
                        console.error(
                            'Task hierarchy reset failed',
                            error
                        );

                        alert(
                            'Failed to update ' +
                            taskLabel +
                            '.'
                        );

                        return;
                    }

                    /*
                     * Update all subtasks of the task.
                     */
                    if (
                        currentTask.subtasks &&
                        currentTask.subtasks.length > 0
                    ) {
                        var subtaskBaseCursor =
                            createHierarchyResetCursor(
                                currentTask.Startdate,
                                currentTask
                            );

                        var parentTaskStart =
                            cloneHierarchyDateOnly(
                                currentTask.Startdate
                            );

                        var parentTaskEnd =
                            cloneHierarchyDateOnly(
                                currentTask.Targetdate
                            );

                        for (
                            let z = 0;
                            z < currentTask.subtasks.length;
                            z++
                        ) {
                            let currentSubTask =
                                currentTask.subtasks[z];

                            let subtaskLabel =
                                $scope.getHierarchyItemLabel(
                                    'subtask',
                                    currentSubTask
                                );

                            let resetSubtaskStart =
                                applyHierarchyResetCursor(
                                    currentSubTask,
                                    'subtask',
                                    subtaskBaseCursor
                                );

                            let subtaskStart =
                                resetSubtaskStart &&
                                resetSubtaskStart.date;

                            let subtaskStartTime =
                                resetSubtaskStart &&
                                resetSubtaskStart.time;

                            if (
                                subtaskStart <
                                parentTaskStart
                            ) {
                                alert(
                                    subtaskLabel +
                                    ' start date cannot be before ' +
                                    taskLabel +
                                    ' start date.'
                                );

                                return;
                            }

                            if (
                                subtaskStart >
                                parentTaskEnd
                            ) {
                                alert(
                                    subtaskLabel +
                                    ' start date cannot be after ' +
                                    taskLabel +
                                    ' end date.'
                                );

                                return;
                            }

                            var subtaskStartText =
                                toServerDateOnly(
                                    subtaskStart
                                );
                            try {
                                var subtaskEndResponse =
                                    await assignprojectService
                                        .getnew_enddate(
                                            subtaskStartText,
                                            currentSubTask.txtdays,
                                        resolveHierarchyProjectId(currentSubTask)
                                        );

                                var subtaskEnd =
                                    hierarchyResponseDate(
                                        subtaskEndResponse
                                    );

                                if (!subtaskEnd) {
                                    throw new Error(
                                        'Unable to calculate subtask end date.'
                                    );
                                }

                                if (
                                    subtaskEnd >
                                    parentTaskEnd
                                ) {
                                    alert(
                                        subtaskLabel +
                                        ' end date cannot be after ' +
                                        taskLabel +
                                        ' end date.'
                                    );

                                    return;
                                }

                                if (
                                    subtaskEnd <
                                    subtaskStart
                                ) {
                                    alert(
                                        subtaskLabel +
                                        ' end date cannot be before Start date.'
                                    );

                                    return;
                                }

                                currentSubTask.Targetdate =
                                    cloneHierarchyDateOnly(
                                        subtaskEnd
                                    );

                                var subtaskUpdateResponse =
                                    await assignprojectService
                                        .updatetimeline_subtask(
                                            currentSubTask.id,
                                            currentSubTask.txtdays,
                                            subtaskStartText,
                                            subtaskStartTime,
                                            $scope.perdayhrs,
                                            ''
                                        );

                                var savedSubtaskEnd =
                                    hierarchyResponseDate(
                                        subtaskUpdateResponse
                                    );

                                if (savedSubtaskEnd) {
                                    currentSubTask.Targetdate =
                                        cloneHierarchyDateOnly(
                                            savedSubtaskEnd
                                        );

                                    subtaskEnd =
                                        savedSubtaskEnd;
                                }

                                currentSubTask._oldStartdate =
                                    angular.copy(
                                        currentSubTask.Startdate
                                    );

                                currentSubTask._oldTargetdate =
                                    angular.copy(
                                        currentSubTask.Targetdate
                                    );

                                currentSubTask._oldStartTime =
                                    angular.copy(
                                        currentSubTask.StartTime
                                    );

                                currentSubTask._oldtxtdays =
                                    angular.copy(
                                        currentSubTask.txtdays
                                    );

                                await refreshHierarchyLeftDays(
                                    currentSubTask,
                                    'subtask'
                                );

                                subtaskBaseCursor =
                                    await advanceHierarchyResetCursorByHours(
                                        subtaskBaseCursor,
                                        currentSubTask
                                    );

                            } catch (error) {
                                console.error(
                                    'Subtask hierarchy reset failed',
                                    error
                                );

                                alert(
                                    'Failed to update ' +
                                    subtaskLabel +
                                    '.'
                                );

                                return;
                            }
                        }
                    }

                    // Continue the next task from cumulative working hours.
                    // The task's txtdays is used only for its Targetdate.
                    taskBaseCursor =
                        await advanceHierarchyResetCursorByHours(
                            taskBaseCursor,
                            currentTask
                        );
                }
            }

            // Continue the next milestone from cumulative working hours.
            // The milestone's txtdays is used only for its EndDate.
            baseCursor =
                await advanceHierarchyResetCursorByHours(
                    baseCursor,
                    currentMile
                );
        }

        $scope.syncListAndKanbanViews();
    };
    // Reset icons use Bootstrap tooltips whose popup is appended to <body>.
    // If Angular removes the icon while it is hovered, the popup can remain
    // visible. Dispose it before showing the spinner, then recreate it after
    // reset completes. This changes UI state only; reset calculations remain
    // untouched.
    function disposeHierarchyResetTooltip($event) {
        var trigger = $event && ($event.currentTarget || $event.target);
        if (trigger && trigger.closest) {
            trigger = trigger.closest('[data-bs-toggle="tooltip"]') || trigger;
        }

        if (trigger) {
            try {
                if (window.bootstrap && bootstrap.Tooltip) {
                    var instance = bootstrap.Tooltip.getInstance(trigger);
                    if (instance) {
                        instance.hide();
                        instance.dispose();
                    }
                }
            } catch (e) { }

            try { trigger.removeAttribute('aria-describedby'); } catch (e1) { }
            try { trigger.blur(); } catch (e2) { }
        }

        // Remove only rendered Bootstrap tooltip popups. This is required
        // because their DOM lives outside the Angular hierarchy row.
        try {
            document.querySelectorAll('.tooltip').forEach(function (tooltip) {
                if (tooltip && tooltip.parentNode) {
                    tooltip.parentNode.removeChild(tooltip);
                }
            });
        } catch (e3) { }

        return trigger;
    }

    function rebindHierarchyResetTooltip(trigger) {
        if (!trigger) return;

        $timeout(function () {
            try {
                if (!document.body.contains(trigger) ||
                    !window.bootstrap || !bootstrap.Tooltip) return;

                var existing = bootstrap.Tooltip.getInstance(trigger);
                if (existing) existing.dispose();

                new bootstrap.Tooltip(trigger, {
                    trigger: 'hover',
                    placement: trigger.getAttribute('data-bs-placement') || 'top'
                });
            } catch (e) { }
        }, 25, false);
    }


    // Reset Start Date uses a dry-run first. No row is saved until the complete
    // affected hierarchy has been calculated and any required parent extension
    // has been confirmed with the same detailed Yes/No popup used by
    // saveHierarchyEndDate().
    function maxHierarchyResetPreviewDate(left, right) {
        var a = normalizeHierarchyDateOnly(left);
        var b = normalizeHierarchyDateOnly(right);
        if (!a) return b;
        if (!b) return a;
        return a > b ? a : b;
    }

    function findHierarchyResetItemIndex(list, item) {
        list = list || [];
        var itemId = item && item.id !== undefined && item.id !== null
            ? String(item.id)
            : '';

        for (var i = 0; i < list.length; i++) {
            if (list[i] === item) return i;
            if (itemId && list[i] && String(list[i].id) === itemId) return i;
        }
        return -1;
    }

    async function previewHierarchyResetItem(item, type, cursor) {
        debugger
        var start = getHierarchyResetCursorStart(cursor);
        if (!start || !start.date) {
            throw new Error($scope.getHierarchyItemLabel(type, item) +
                ' start date is invalid.');
        }
        var projectid = item.ProjectId;
       
        var response = await assignprojectService.getnew_enddate(
            toServerDateOnly(start.date),
            item.txtdays,resolveHierarchyProjectId(item)
        );
        var endDate = hierarchyResponseDate(response);
        if (!endDate) {
            throw new Error('Unable to calculate ' +
                $scope.getHierarchyItemLabel(type, item) + ' end date.');
        }

        var nextCursor = await advanceHierarchyResetCursorByHours(cursor, item);
        if (!nextCursor) {
            throw new Error('Unable to calculate the next hierarchy start date/time.');
        }

        return {
            item: item,
            type: type,
            startDate: cloneHierarchyDateOnly(start.date),
            startTime: start.time,
            ownEndDate: cloneHierarchyDateOnly(endDate),
            requiredEndDate: cloneHierarchyDateOnly(endDate),
            nextCursor: nextCursor
        };
    }

    async function previewHierarchyResetTask(task, cursor) {
        var taskPreview = await previewHierarchyResetItem(task, 'task', cursor);
        var requiredEnd = cloneHierarchyDateOnly(taskPreview.ownEndDate);
        var subtaskCursor = createHierarchyResetCursor(
            taskPreview.startDate,
            { StartTime: taskPreview.startTime, starttime: taskPreview.startTime }
        );

        var subtasks = task.subtasks || [];
        for (var s = 0; s < subtasks.length; s++) {
            var subtaskPreview = await previewHierarchyResetItem(
                subtasks[s],
                'subtask',
                subtaskCursor
            );
            requiredEnd = maxHierarchyResetPreviewDate(
                requiredEnd,
                subtaskPreview.requiredEndDate
            );
            subtaskCursor = subtaskPreview.nextCursor;
        }

        taskPreview.requiredEndDate = requiredEnd;
        return taskPreview;
    }

    async function previewHierarchyResetMilestone(milestone, cursor) {
        var milestonePreview = await previewHierarchyResetItem(
            milestone,
            'milestone',
            cursor
        );
        var requiredEnd = cloneHierarchyDateOnly(milestonePreview.ownEndDate);
        var taskCursor = createHierarchyResetCursor(
            milestonePreview.startDate,
            { StartTime: milestonePreview.startTime, starttime: milestonePreview.startTime }
        );

        var tasks = milestone.tasks || [];
        for (var t = 0; t < tasks.length; t++) {
            var taskPreview = await previewHierarchyResetTask(tasks[t], taskCursor);
            requiredEnd = maxHierarchyResetPreviewDate(
                requiredEnd,
                taskPreview.requiredEndDate
            );
            taskCursor = taskPreview.nextCursor;
        }

        milestonePreview.requiredEndDate = requiredEnd;
        return milestonePreview;
    }

    function addHierarchyResetPreviewEntry(plan, type, item, requiredEnd) {
        if (!item) return;
        var oldEnd = getHierarchyEndDateValue(item, type);
        var newEnd = maxHierarchyResetPreviewDate(oldEnd, requiredEnd);
        var isExtension = !!(oldEnd && newEnd && newEnd > oldEnd);

        plan.push({
            type: type,
            item: item,
            label: $scope.getHierarchyItemLabel(type, item),
            oldEnd: cloneHierarchyDateOnly(oldEnd),
            newEnd: cloneHierarchyDateOnly(newEnd),
            isExtension: isExtension,
            needsSave: isExtension
        });
    }

    async function buildHierarchyResetPreview(type, node, parent, grandparent, index) {
        var normalizedType = String(type || '').toLowerCase();
        var project = $scope.projectlist || {};
        var milestones = project.miles || [];
        var selectedPreview = null;
        var taskRequiredEnd = null;
        var milestoneRequiredEnd = null;
        var projectRequiredEnd = normalizeHierarchyDateOnly(project.TargetDate);

        if (!projectRequiredEnd) {
            throw new Error('Project end date is invalid.');
        }

        if (normalizedType === 'subtask') {
            var task = parent;
            var milestone = grandparent;
            if (!task || !milestone) {
                throw new Error('Task or milestone hierarchy was not found.');
            }

            var subtasks = task.subtasks || [];
            var subtaskIndex = findHierarchyResetItemIndex(subtasks, node);
            if (subtaskIndex < 0) subtaskIndex = Math.max(0, parseInt(index, 10) || 0);

            var subtaskCursor = createHierarchyResetCursor(
                node.Startdate || node.StartDate,
                node
            );
            var affectedSubtaskEnd = null;
            for (var s = subtaskIndex; s < subtasks.length; s++) {
                var subPreview = await previewHierarchyResetItem(
                    subtasks[s],
                    'subtask',
                    subtaskCursor
                );
                if (!selectedPreview) selectedPreview = subPreview;
                affectedSubtaskEnd = maxHierarchyResetPreviewDate(
                    affectedSubtaskEnd,
                    subPreview.requiredEndDate
                );
                subtaskCursor = subPreview.nextCursor;
            }

            taskRequiredEnd = maxHierarchyResetPreviewDate(
                getHierarchyEndDateValue(task, 'task'),
                affectedSubtaskEnd
            );
            milestoneRequiredEnd = maxHierarchyResetPreviewDate(
                getHierarchyEndDateValue(milestone, 'milestone'),
                taskRequiredEnd
            );

            var taskIndex = findHierarchyResetItemIndex(milestone.tasks || [], task);
            var taskCursor = createHierarchyResetCursor(task.Startdate, task);
            taskCursor = await advanceHierarchyResetCursorByHours(taskCursor, task);
            for (var t = taskIndex + 1; t < (milestone.tasks || []).length; t++) {
                var laterTaskPreview = await previewHierarchyResetTask(
                    milestone.tasks[t],
                    taskCursor
                );
                milestoneRequiredEnd = maxHierarchyResetPreviewDate(
                    milestoneRequiredEnd,
                    laterTaskPreview.requiredEndDate
                );
                taskCursor = laterTaskPreview.nextCursor;
            }

            var milestoneIndex = findHierarchyResetItemIndex(milestones, milestone);
            var milestoneCursor = createHierarchyResetCursor(
                milestone.StartDate,
                milestone
            );
            milestoneCursor = await advanceHierarchyResetCursorByHours(
                milestoneCursor,
                milestone
            );
            projectRequiredEnd = maxHierarchyResetPreviewDate(
                projectRequiredEnd,
                milestoneRequiredEnd
            );
            for (var m = milestoneIndex + 1; m < milestones.length; m++) {
                var laterMilestonePreview = await previewHierarchyResetMilestone(
                    milestones[m],
                    milestoneCursor
                );
                projectRequiredEnd = maxHierarchyResetPreviewDate(
                    projectRequiredEnd,
                    laterMilestonePreview.requiredEndDate
                );
                milestoneCursor = laterMilestonePreview.nextCursor;
            }
        } else if (normalizedType === 'task') {
            var parentMilestone = parent;
            if (!parentMilestone) {
                throw new Error('Milestone hierarchy was not found.');
            }

            var tasks = parentMilestone.tasks || [];
            var selectedTaskIndex = findHierarchyResetItemIndex(tasks, node);
            if (selectedTaskIndex < 0) selectedTaskIndex = Math.max(0, parseInt(index, 10) || 0);

            var selectedTaskCursor = createHierarchyResetCursor(
                node.Startdate || node.StartDate,
                node
            );
            for (var y = selectedTaskIndex; y < tasks.length; y++) {
                var resetTaskPreview = await previewHierarchyResetTask(
                    tasks[y],
                    selectedTaskCursor
                );
                if (!selectedPreview) selectedPreview = resetTaskPreview;
                milestoneRequiredEnd = maxHierarchyResetPreviewDate(
                    milestoneRequiredEnd,
                    resetTaskPreview.requiredEndDate
                );
                selectedTaskCursor = resetTaskPreview.nextCursor;
            }

            milestoneRequiredEnd = maxHierarchyResetPreviewDate(
                getHierarchyEndDateValue(parentMilestone, 'milestone'),
                milestoneRequiredEnd
            );

            var parentMilestoneIndex = findHierarchyResetItemIndex(
                milestones,
                parentMilestone
            );
            var nextMilestoneCursor = createHierarchyResetCursor(
                parentMilestone.StartDate,
                parentMilestone
            );
            nextMilestoneCursor = await advanceHierarchyResetCursorByHours(
                nextMilestoneCursor,
                parentMilestone
            );
            projectRequiredEnd = maxHierarchyResetPreviewDate(
                projectRequiredEnd,
                milestoneRequiredEnd
            );
            for (var my = parentMilestoneIndex + 1; my < milestones.length; my++) {
                var nextMilestonePreview = await previewHierarchyResetMilestone(
                    milestones[my],
                    nextMilestoneCursor
                );
                projectRequiredEnd = maxHierarchyResetPreviewDate(
                    projectRequiredEnd,
                    nextMilestonePreview.requiredEndDate
                );
                nextMilestoneCursor = nextMilestonePreview.nextCursor;
            }
        } else if (normalizedType === 'milestone') {
            var selectedMilestoneIndex = findHierarchyResetItemIndex(milestones, node);
            if (selectedMilestoneIndex < 0) {
                selectedMilestoneIndex = Math.max(0, parseInt(index, 10) || 0);
            }

            var selectedMilestoneCursor = createHierarchyResetCursor(
                node.StartDate || node.Startdate,
                node
            );
            for (var x = selectedMilestoneIndex; x < milestones.length; x++) {
                var resetMilestonePreview = await previewHierarchyResetMilestone(
                    milestones[x],
                    selectedMilestoneCursor
                );
                if (!selectedPreview) selectedPreview = resetMilestonePreview;
                projectRequiredEnd = maxHierarchyResetPreviewDate(
                    projectRequiredEnd,
                    resetMilestonePreview.requiredEndDate
                );
                selectedMilestoneCursor = resetMilestonePreview.nextCursor;
            }
        } else {
            throw new Error('Invalid hierarchy type.');
        }

        if (!selectedPreview) {
            throw new Error('Selected hierarchy item could not be calculated.');
        }

        var selectedOldEnd = getHierarchyEndDateValue(node, normalizedType);
        var selectedNewEnd = selectedPreview.requiredEndDate ||
            selectedPreview.ownEndDate;
        var plan = [{
            type: normalizedType,
            item: node,
            label: $scope.getHierarchyItemLabel(normalizedType, node),
            oldEnd: cloneHierarchyDateOnly(selectedOldEnd),
            newEnd: cloneHierarchyDateOnly(selectedNewEnd),
            isCurrent: true,
            isExtension: !!(selectedOldEnd && selectedNewEnd && selectedNewEnd > selectedOldEnd),
            needsSave: true
        }];

        if (normalizedType === 'subtask') {
            addHierarchyResetPreviewEntry(plan, 'task', parent, taskRequiredEnd);
            addHierarchyResetPreviewEntry(plan, 'milestone', grandparent, milestoneRequiredEnd);
        } else if (normalizedType === 'task') {
            addHierarchyResetPreviewEntry(plan, 'milestone', parent, milestoneRequiredEnd);
        }

        var projectOldEnd = normalizeHierarchyDateOnly(project.TargetDate);
        var projectNewEnd = maxHierarchyResetPreviewDate(
            projectOldEnd,
            projectRequiredEnd
        );
        plan.push({
            type: 'project',
            item: project,
            label: project.ProjectName
                ? 'Project "' + project.ProjectName + '"'
                : 'Project',
            oldEnd: cloneHierarchyDateOnly(projectOldEnd),
            newEnd: cloneHierarchyDateOnly(projectNewEnd),
            isExtension: !!(projectOldEnd && projectNewEnd && projectNewEnd > projectOldEnd),
            needsSave: !!(projectOldEnd && projectNewEnd && projectNewEnd > projectOldEnd)
        });

        return {
            plan: plan,
            currentEntry: plan[0],
            startDate: selectedPreview.startDate,
            startTime: selectedPreview.startTime
        };
    }

    function collectHierarchyResetPendingPromises(type, parent, index) {
        var normalizedType = String(type || '').toLowerCase();
        var startIndex = Math.max(0, parseInt(index, 10) || 0);
        var promises = [];

        function addItem(item) {
            if (!item) return;
            if (item._durationDaysSavePromise && item._durationDaysSavePromise.then) {
                promises.push(item._durationDaysSavePromise);
            }
            if (item._workingHoursSavePromise && item._workingHoursSavePromise.then) {
                promises.push(item._workingHoursSavePromise);
            }
        }

        function addTask(task) {
            addItem(task);
            angular.forEach((task && task.subtasks) || [], addItem);
        }

        if (normalizedType === 'milestone') {
            angular.forEach(((parent && parent.miles) || []).slice(startIndex), function (milestone) {
                addItem(milestone);
                angular.forEach(milestone.tasks || [], addTask);
            });
        } else if (normalizedType === 'task') {
            angular.forEach(((parent && parent.tasks) || []).slice(startIndex), addTask);
        } else if (normalizedType === 'subtask') {
            angular.forEach(((parent && parent.subtasks) || []).slice(startIndex), addItem);
        }

        return promises;
    }

    async function waitForHierarchyResetPendingEdits(type, parent, index) {
        var promises = collectHierarchyResetPendingPromises(type, parent, index);
        if (!promises.length) return true;

        try {
            await Promise.all(promises);
            // Allow Angular's promise callback/digest to apply the saved values
            // before the synchronous reset validations read them.
            await new Promise(function (resolve) {
                setTimeout(resolve, 0);
            });
            return true;
        } catch (error) {
            // The original blur handler already restores the old value and shows
            // its validation message. Stop Reset so it cannot continue with an
            // unsuccessfully saved edit.
            return false;
        }
    }

    async function executeHierarchyResetWithExtendPopup(
        type,
        node,
        parent,
        grandparent,
        index
    ) {
        debugger
        var normalizedType = String(type || '').toLowerCase();
        if (!$scope.requireEdit(normalizedType)) return;
        if (!node) return;

        var branchParent = normalizedType === 'milestone'
            ? ($scope.projectlist || parent)
            : parent;

        // Clicking Reset causes the active Duration Days/Working Hours input to
        // blur first. Wait for that save to finish, then validate once using the
        // current value. This removes the old "first click alerts, second click
        // works" behaviour for Milestone, Task and Subtask.
        if (!await waitForHierarchyResetPendingEdits(
            normalizedType,
            branchParent,
            index
        )) {
            return;
        }

        if (!validateHierarchyResetBranchHours(
            normalizedType,
            normalizedType === 'milestone'
                ? ((branchParent && branchParent.miles) || [])
                : normalizedType === 'task'
                    ? ((parent && parent.tasks) || [])
                    : ((parent && parent.subtasks) || []),
            index
        )) {
            return;
        }

        if (!$scope.validateHierarchyDurationDays(
            normalizedType,
            branchParent,
            index
        )) {
            return;
        }

        var preview;
        try {
            preview = await buildHierarchyResetPreview(
                normalizedType,
                node,
                parent,
                grandparent,
                index
            );
        } catch (previewError) {
            console.error('Reset Start Date preview failed', previewError);
            alert(previewError && previewError.message
                ? previewError.message
                : 'Unable to calculate Reset Start Date hierarchy.');
            return;
        }

        var confirmed = await confirmHierarchyEndDatePlan(preview.plan);
        if (!confirmed) return;

        try {
            await saveHierarchyExtensionCascade(
                preview.plan,
                preview.currentEntry,
                {
                    startDate: preview.startDate,
                    durationDays: node.txtdays,
                    startTime: preview.startTime,
                    perdayhrs: $scope.perdayhrs,
                    changeSource: 'reset'
                }
            );

            if ($scope.refreshProjectDurationDays) {
                await Promise.resolve($scope.refreshProjectDurationDays())
                    .catch(function () { return null; });
            }

            $scope.syncListAndKanbanViews();
            $scope.$applyAsync();
        } catch (saveError) {
            console.error('Reset Start Date save failed', saveError);
            if (window.Swal && Swal.fire) {
                await Swal.fire({
                    title: 'Timeline Not Updated',
                    text: saveError && saveError.message
                        ? saveError.message
                        : 'Reset Start Date could not be saved.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            } else {
                alert(saveError && saveError.message
                    ? saveError.message
                    : 'Reset Start Date could not be saved.');
            }
        }
    }

    // Replace only the Reset Start Date save path. Existing validations,
    // permissions, spinner handling and all unrelated functionality remain
    // unchanged.
    $scope.resetStartDateHierarchy_subtask = async function (node, task, index) {
        var context = findHierarchyTimelineContext(node, 'subtask', task, null);
        return executeHierarchyResetWithExtendPopup(
            'subtask',
            node,
            context.parent || task,
            context.grandparent,
            index
        );
    };

    $scope.resetStartDateHierarchy_task = async function (node, milestone, index) {
        var context = findHierarchyTimelineContext(node, 'task', milestone, null);
        return executeHierarchyResetWithExtendPopup(
            'task',
            node,
            context.parent || milestone,
            null,
            index
        );
    };

    $scope.resetStartDateHierarchy_mile = async function (node, projectlist, index) {
        return executeHierarchyResetWithExtendPopup(
            'milestone',
            node,
            projectlist || $scope.projectlist,
            null,
            index
        );
    };

    // Show the same saving spinner used by the end-date modal while any
    // hierarchy reset is running. The original reset implementation is kept
    // unchanged; this wrapper only manages UI state and duplicate clicks.
    function wrapHierarchyResetWithSpinner(functionName) {
        var originalReset = $scope[functionName];
        if (typeof originalReset !== 'function') return;

        $scope[functionName] = async function (node) {
            if (!node || node._resettingStartDate) return;

            var possibleEvent = arguments.length
                ? arguments[arguments.length - 1]
                : null;
            var resetTrigger = disposeHierarchyResetTooltip(
                possibleEvent && possibleEvent.currentTarget ? possibleEvent : null
            );

            node._resettingStartDate = true;
            $scope.$applyAsync();

            try {
                return await originalReset.apply(this, arguments);
            } finally {
                node._resettingStartDate = false;
                $scope.$applyAsync();
                rebindHierarchyResetTooltip(resetTrigger);
            }
        };
    }

    wrapHierarchyResetWithSpinner('resetStartDateHierarchy_mile');
    wrapHierarchyResetWithSpinner('resetStartDateHierarchy_task');
    wrapHierarchyResetWithSpinner('resetStartDateHierarchy_subtask');

    $scope.resetStartDateHierarchy_mile_old = async function (node, projectlist, index, updatefrom) {
        if (!$scope.requireEdit('milestone')) return;
        if (!$scope.validateHierarchyDurationDays('milestone', projectlist, index)) return;
        if (!node || !projectlist || !projectlist.miles || projectlist.miles.length === 0) return;

        var projectStart = normalizeHierarchyDateOnly(projectlist.StartDate || ($scope.projectlist && $scope.projectlist.StartDate));
        var projectEnd = normalizeHierarchyDateOnly(projectlist.TargetDate || ($scope.projectlist && $scope.projectlist.TargetDate));
        var baseDate = normalizeHierarchyDateOnly(node.StartDate || node.Startdate);
        var startIndex = parseInt(index, 10);
        if (isNaN(startIndex) || startIndex < 0) startIndex = 0;

        if (!projectStart || !projectEnd || !baseDate) {
            alert('Project or selected Milestone date is invalid.');
            return;
        }

        for (let x = startIndex; x < projectlist.miles.length; x++) {
            let currentMile = projectlist.miles[x];
            let milestoneLabel = $scope.getHierarchyItemLabel('milestone', currentMile);
            let milestoneStart = cloneHierarchyDateOnly(baseDate);

            function restoreCurrentMilestone() {
                currentMile._isUpdating = true;
                if (updatefrom === 'duration') {
                    currentMile.txtdays = angular.copy(currentMile._oldtxtdays);
                } else {
                    currentMile.StartDate = angular.copy(currentMile._oldStartDate);
                    currentMile.EndDate = angular.copy(currentMile._oldEndDate);
                }
                currentMile._isUpdating = false;
            }
            currentMile.StartDate = cloneHierarchyDateOnly(milestoneStart);
            if (milestoneStart < projectStart) {
                alert(milestoneLabel + ' start date cannot be before Project start date.');
                restoreCurrentMilestone();
                return;
            }
            if (milestoneStart > projectEnd) {
                alert(milestoneLabel + ' start date cannot be after Project end date.');
                restoreCurrentMilestone();
                return;
            }
            var milestoneStartText = toServerDateOnly(milestoneStart);
            var milestoneEnd;

            try {
                var milestoneEndResponse = await assignprojectService.getnew_enddate(
                    milestoneStartText,
                    currentMile.txtdays,
resolveHierarchyProjectId(currentMile)
                );
                milestoneEnd = hierarchyResponseDate(milestoneEndResponse);
                if (!milestoneEnd) throw new Error('Unable to calculate milestone end date.');

                if (milestoneEnd > projectEnd) {
                    alert(milestoneLabel + ' end date cannot be after Project end date.');
                    restoreCurrentMilestone();
                    return;
                }

                if (milestoneEnd < milestoneStart) {
                    alert(milestoneLabel + ' end date cannot be before Start date.');
                    restoreCurrentMilestone();
                    return;
                }

                currentMile.EndDate = cloneHierarchyDateOnly(milestoneEnd);
                var milestoneUpdateResponse = await assignprojectService.updatetimeline_mile(
                    currentMile.id,
                    currentMile.txtdays,
                    milestoneStartText
                );
                var savedMilestoneEnd = hierarchyResponseDate(milestoneUpdateResponse);
                if (savedMilestoneEnd) {
                    currentMile.EndDate = cloneHierarchyDateOnly(savedMilestoneEnd);
                    milestoneEnd = savedMilestoneEnd;
                }
                currentMile._oldStartDate = angular.copy(currentMile.StartDate);
                currentMile._oldEndDate = angular.copy(currentMile.EndDate);
                currentMile._oldtxtdays = angular.copy(currentMile.txtdays);
                await refreshHierarchyLeftDays(currentMile, 'milestone');
            } catch (error) {
                console.error('Milestone hierarchy reset failed', error);
                alert('Failed to update ' + milestoneLabel + '.');
                restoreCurrentMilestone();
                return;
            }
            if (currentMile.tasks && currentMile.tasks.length > 0) {
                var taskBaseDate = cloneHierarchyDateOnly(currentMile.StartDate);
                var parentMileStart = cloneHierarchyDateOnly(currentMile.StartDate);
                var parentMileEnd = cloneHierarchyDateOnly(currentMile.EndDate);

                for (let y = 0; y < currentMile.tasks.length; y++) {
                    let currentTask = currentMile.tasks[y];
                    let taskLabel = $scope.getHierarchyItemLabel('task', currentTask);
                    let taskStart = cloneHierarchyDateOnly(taskBaseDate);

                    currentTask.Startdate = cloneHierarchyDateOnly(taskStart);

                    if (taskStart < parentMileStart) {
                        alert(taskLabel + ' start date cannot be before ' + milestoneLabel + ' start date.');
                        return;
                    }

                    if (taskStart > parentMileEnd) {
                        alert(taskLabel + ' start date cannot be after ' + milestoneLabel + ' end date.');
                        return;
                    }

                    var taskStartText = toServerDateOnly(taskStart);
                    var taskEnd;

                    try {
                        var taskEndResponse = await assignprojectService.getnew_enddate(
                            taskStartText,
                            currentTask.txtdays,
                            resolveHierarchyProjectId(currentTask)
                        );
                        taskEnd = hierarchyResponseDate(taskEndResponse);
                        if (!taskEnd) throw new Error('Unable to calculate task end date.');

                        if (taskEnd > parentMileEnd) {
                            alert(taskLabel + ' end date cannot be after ' + milestoneLabel + ' end date.');
                            return;
                        }

                        if (taskEnd < taskStart) {
                            alert(taskLabel + ' end date cannot be before Start date.');
                            return;
                        }

                        currentTask.Targetdate = cloneHierarchyDateOnly(taskEnd);

                        var taskUpdateResponse = await assignprojectService.updatetimeline_task(
                            currentTask.id,
                            currentTask.txtdays,
                            taskStartText
                        );

                        var savedTaskEnd = hierarchyResponseDate(taskUpdateResponse);
                        if (savedTaskEnd) {
                            currentTask.Targetdate = cloneHierarchyDateOnly(savedTaskEnd);
                            taskEnd = savedTaskEnd;
                        }

                        currentTask._oldStartdate = angular.copy(currentTask.Startdate);
                        currentTask._oldTargetdate = angular.copy(currentTask.Targetdate);
                        currentTask._oldtxtdays = angular.copy(currentTask.txtdays);

                        await refreshHierarchyLeftDays(currentTask, 'task');
                    } catch (error) {
                        console.error('Task hierarchy reset failed', error);
                        alert('Failed to update ' + taskLabel + '.');
                        return;
                    }

                    if (currentTask.subtasks && currentTask.subtasks.length > 0) {
                        var subtaskBaseDate = cloneHierarchyDateOnly(currentTask.Startdate);
                        var parentTaskStart = cloneHierarchyDateOnly(currentTask.Startdate);
                        var parentTaskEnd = cloneHierarchyDateOnly(currentTask.Targetdate);

                        for (let z = 0; z < currentTask.subtasks.length; z++) {
                            let currentSubTask = currentTask.subtasks[z];
                            let subtaskLabel = $scope.getHierarchyItemLabel('subtask', currentSubTask);
                            let subtaskStart = cloneHierarchyDateOnly(subtaskBaseDate);

                            currentSubTask.Startdate = cloneHierarchyDateOnly(subtaskStart);

                            if (subtaskStart < parentTaskStart) {
                                alert(subtaskLabel + ' start date cannot be before ' + taskLabel + ' start date.');
                                return;
                            }

                            if (subtaskStart > parentTaskEnd) {
                                alert(subtaskLabel + ' start date cannot be after ' + taskLabel + ' end date.');
                                return;
                            }

                            var subtaskStartText = toServerDateOnly(subtaskStart);

                            try {
                                var subtaskEndResponse = await assignprojectService.getnew_enddate(
                                    subtaskStartText,
                                    currentSubTask.txtdays,
                                    resolveHierarchyProjectId(currentSubTask)
                                );
                                var subtaskEnd = hierarchyResponseDate(subtaskEndResponse);
                                if (!subtaskEnd) throw new Error('Unable to calculate subtask end date.');

                                if (subtaskEnd > parentTaskEnd) {
                                    alert(subtaskLabel + ' end date cannot be after ' + taskLabel + ' end date.');
                                    return;
                                }

                                if (subtaskEnd < subtaskStart) {
                                    alert(subtaskLabel + ' end date cannot be before Start date.');
                                    return;
                                }

                                currentSubTask.Targetdate = cloneHierarchyDateOnly(subtaskEnd);

                                var subtaskUpdateResponse = await assignprojectService.updatetimeline_subtask(
                                    currentSubTask.id,
                                    currentSubTask.txtdays,
                                    subtaskStartText
                                );

                                var savedSubtaskEnd = hierarchyResponseDate(subtaskUpdateResponse);
                                if (savedSubtaskEnd) {
                                    currentSubTask.Targetdate = cloneHierarchyDateOnly(savedSubtaskEnd);
                                    subtaskEnd = savedSubtaskEnd;
                                }

                                currentSubTask._oldStartdate = angular.copy(currentSubTask.Startdate);
                                currentSubTask._oldTargetdate = angular.copy(currentSubTask.Targetdate);
                                currentSubTask._oldtxtdays = angular.copy(currentSubTask.txtdays);

                                await refreshHierarchyLeftDays(currentSubTask, 'subtask');
                                subtaskBaseDate = addHierarchyCalendarDays(subtaskEnd, 1);
                            } catch (error) {
                                console.error('Subtask hierarchy reset failed', error);
                                alert('Failed to update ' + subtaskLabel + '.');
                                return;
                            }
                        }
                    }

                    taskBaseDate = addHierarchyCalendarDays(taskEnd, 1);
                }
            }

            baseDate = addHierarchyCalendarDays(milestoneEnd, 1);
        }
    };

    $scope.zoomLevel = 100;
    $scope.applyZoom = function (zoomValue) {
        debugger
        var value = parseInt(zoomValue, 10);

        if (isNaN(value)) {
            value = 100;
        }

        value = Math.max(50, Math.min(150, value));
        $scope.zoom.level = value;

        var scale = value / 100;
        var table = document.querySelector(".tbl_new");

        if (table) {
            table.style.transform = "scale(" + scale + ")";
            table.style.transformOrigin = "top left";
            table.style.width = (100 / scale) + "%";
        }
    };


    $scope.activePopover = null;



    $scope.togglePopover = function ($event, item, itemP, level, index) {
        var el = $event.currentTarget;
        if ($scope.activePopover) {
            $scope.activePopover.dispose();
            $scope.activePopover = null;
        }

        var old = bootstrap.Popover.getInstance(el);
        if (old) {
            old.dispose();
            return;
        }

        $scope.popContext = {
            item: item,
            itemP: itemP,
            level: level,
            index: index
        };
        function closePopover() {
            if ($scope.activePopover) {
                $scope.activePopover.dispose();
                $scope.activePopover = null;
            }
            setTimeout(function () {
                document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                    new bootstrap.Tooltip(el);
                });
            }, 100);
        }
        window.addRowUpDown_mile_wrap = function (dir) {
            //  alert($scope.popContext.item);
            $scope.$applyAsync(function () {
                $scope.addRowUpDown_mile(
                    $scope.popContext.index,
                    $scope.popContext.item,
                    dir
                );

            });
            closePopover();
        };

        window.addRowUpDown_task_wrap = function (dir) {
            $scope.$applyAsync(function () {
                $scope.addRowUpDown_task(
                    $scope.popContext.index,
                    $scope.popContext.itemP,
                    $scope.popContext.item,
                    dir
                );
            });
            closePopover();
        };

        window.addRowUpDown_taskdefault_wrap = function () {
            $scope.$applyAsync(function () {
                $scope.addRowUpDown_taskdefault($scope.popContext.item);
            });
            closePopover();
        };

        window.addRowUpDown_subtaskdefault_wrap = function () {
            $scope.$applyAsync(function () {
                $scope.addRowUpDown_subtaskdefault($scope.popContext.item);
            });
            closePopover();
        };

        window.addRowUpDown_subtask_wrap = function (dir) {
            $scope.$applyAsync(function () {
                $scope.addRowUpDown_subtask(
                    $scope.popContext.index,
                    $scope.popContext.itemP,
                    $scope.popContext.item,
                    dir,

                );
            });
            closePopover();
        };

        var content = "";
        if (level === "milestone") {
            var TaskButton = '';

            if (item.istask == 1 || item.istask == '1') {
                TaskButton = `
        <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_taskdefault_wrap()"
                       class="dropdown-item p-1">
                       <i class="ri-list-unordered label-icon align-middle me-1"  title="Add Task"></i>
                    </a>
                </li>
    `;
            }
            content = `
            <ul class="dropdown-menu dropdownmenu-secondary show" style="position:static;display:block;height:35px;padding:2px;">
                <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_mile_wrap('up')"
                       class="dropdown-item p-1"> <i class='bi bi-arrow-up-square'  title='Add Above'></i></a>
                </li>

                <li style="display:inline-block; margin:0 15px;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_mile_wrap('down')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-down-square'  title='Add Below'></i></a>
                </li>
            ${TaskButton}
                
            </ul>
        `;
        }
        else if (level === "task") {
            var subTaskButton = '';

            if (item.issubtask == 1 || item.issubtask == '1') {
                subTaskButton = `
        <li style="display:inline-block;" ng-if="item.issubtask==1">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_subtaskdefault_wrap()"
                       class="dropdown-item p-1">
                       <i class="ri-list-unordered label-icon align-middle me-1"  title="Add Sub Task"></i>
                    </a>
                </li>
    `;
            }
            content = `
            <ul class="dropdown-menu dropdownmenu-secondary show" style="position:static;display:block;;height:35px;padding:2px;">
                <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_task_wrap('up')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-up-square'  title='Add Above'></i></a>
                </li>

                <li style="display:inline-block; margin:0 15px;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_task_wrap('down')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-down-square' title='Add Below'></i></a>
                </li>
${subTaskButton}
                
            </ul>
        `;
        }
        else if (level === "subtask") {
            content = `
            <ul class="dropdown-menu dropdownmenu-secondary show" style="position:static;display:block;;height:35px;padding:2px;">
                <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_subtask_wrap('up')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-up-square'  title='Add Above'></i></a>
                </li>

                <li style="display:inline-block; margin:0 15px;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_subtask_wrap('down')"
                       class="dropdown-item p-1"><span  title="Add Below"><i class='bi bi-arrow-down-square'></i></span></a>
                </li>
            </ul>
        `;
        }



        var pop = new bootstrap.Popover(el, {
            html: true,
            trigger: 'manual',
            placement: 'bottom',
            sanitize: false,
            content: content
        });

        pop.show();
        $scope.activePopover = pop;
        setTimeout(function () {
            document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                new bootstrap.Tooltip(el);
            });
        }, 100);

    };

    $scope.togglePopover_action = function ($event, item, itemP, level, index, ProjectName) {
        var el = $event.currentTarget;
        if ($scope.activePopover) {
            $scope.activePopover.dispose();
            $scope.activePopover = null;
        }

        var old = bootstrap.Popover.getInstance(el);
        if (old) {
            old.dispose();
            return;
        }

        $scope.popContext = {
            item: item,
            itemP: itemP,
            level: level,
            index: index
        };
        function closePopover_action() {
            if ($scope.activePopover) {
                $scope.activePopover.dispose();
                $scope.activePopover = null;
            }
            setTimeout(function () {
                document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                    new bootstrap.Tooltip(el);
                });
            }, 100);
        }

        // wrapper function
        window.loadSrcFiles_wrap = function (dir) {
            $scope.$applyAsync(function () {
                $scope.loadSrcFiles(
                    $scope.popContext.item,
                    dir,
                    $scope.popContext.index
                );
            });
            closePopover_action();
        };
        window.loadform_wrap = function (dir, formtype, ifname, urls, urldetails) {
            //alert(formtype);
            if (urls != "") {
                if (formtype == "dependency") {
                    if (dir == "milestone")
                        urls += "?id=" + $scope.popContext.item.id + "&mprojectid=" + $scope.popContext.item.ProjectId + "&type=milestone&pid=" + $scope.popContext.item.ProjectId;
                    if (dir == "task")
                        urls += "?id=" + $scope.popContext.item.id + "&mid=" + $scope.popContext.item.MilestonId + "&mprojectid=" + $scope.popContext.item.ProjectId + "&type=task&pid=" + $scope.popContext.item.ProjectId;
                    if (dir == "subtask")
                        urls += "?id=" + $scope.popContext.item.id + "&tid=" + $scope.popContext.item.task + "&mid=" + $scope.popContext.item.MilestonId + "&pid=" + $scope.popContext.item.ProjectId + "&mprojectid=" + $scope.popContext.item.ProjectId + "&type=subtask";
                }
                else if (formtype == "issue") {
                    if (dir == "milestone")
                        urls += "?id=" + $scope.popContext.item.ProjectId + "&milstoneid=" + $scope.popContext.item.id + "&mprojectid=" + $scope.popContext.item.ProjectId + "&type=milstone&pid=" + $scope.popContext.item.ProjectId;
                    if (dir == "task")
                        urls += "?id=" + $scope.popContext.item.ProjectId + "&milstoneid=" + $scope.popContext.item.MilestonId + "$taskid=" + $scope.popContext.item.id + "&mprojectid=" + $scope.popContext.item.ProjectId + "&type=task";
                    if (dir == "subtask")
                        urls += "?id=" + $scope.popContext.item.ProjectId + "&taskid=" + $scope.popContext.item.task + "&milstoneid=" + $scope.popContext.item.MilestonId + "&subtaskid=" + $scope.popContext.item.id + "&tasklistid=0&mprojectid=" + $scope.popContext.item.ProjectId + "&type=subtask";

                }
            }
            //alert(urls); 
            $scope.$applyAsync(function () {
                $scope.loadform(
                    $scope.popContext.item,
                    dir,
                    formtype,
                    ifname,
                    urls,
                    urldetails

                );
            });
            closePopover_action();
        };

        var fileCount = (item.viewfiles && item.viewfiles.length) ? item.viewfiles.length : 0;
        var dependancyCount = (item.totaldependency) ? item.totaldependency : 0;
        var issueCount = (item.pendingissue) ? item.pendingissue : 0;

        var content = "";


        content = `
            <div class='action-dropdown1' style='padding:6px 6px 6px 0px;'>

                <a href="javascript:void(0);" onclick="loadSrcFiles_wrap('${level}')" class="position-relative" data-bs-toggle="modal" data-bs-target="#addfiles"> <span class="btn-plus2"  title="Upload Documents" style="font-size:12px;height:22px;width:22px;margin-left:9px"><i class="bi bi-file-earmark-arrow-up"></i></span><span class="files_count">${fileCount}</span></a>
                <a href='javascript:void(0);'  class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_dependency' onclick="loadform_wrap('${level}','dependency','inner_popupD','/pms/AddSTDependency.aspx',' Dependency of ${ProjectName}')"><span class='btn-plus2'  title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span><span class='dependency_count' >${dependancyCount}</span></a>
                <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_issue' onclick="loadform_wrap('${level}','issue','inner_popup','/pms/AddIssuesPopup.aspx',' Issues of ${ProjectName}')"><span class='btn-plus2'  title='+ Issues' style='font-size:12px;height:22px;width:22px;margin-left:9px'>I</span><span class='issue_count' >${issueCount}</span></a>

            </div>
        `;


        var pop = new bootstrap.Popover(el, {
            html: true,
            trigger: 'manual',
            placement: 'bottom',
            sanitize: false,
            content: content
        });
        $scope.activePopover = pop;
        //$scope.activePopover = pop;
        //$scope.activePopover = null;
        pop.show();


        setTimeout(function () {
            document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                new bootstrap.Tooltip(el);
            });
        }, 100);
    };


    $document.on('click', function (e) {
        //debugger
        // if no popover → do nothing
        if (!$scope.activePopover) return;
        var popEl = document.querySelector('.popover');
        // clicked inside popover → ignore
        if (popEl && popEl.contains(e.target)) return;
        // clicked on toggle button → ignore

        if (e.target.closest('.btn-plus2')) return;
        if (e.target.closest('.bi-three-dots-vertical')) return;
        // otherwise → close
        $scope.$applyAsync(function () {
            $scope.activePopover.dispose();
            $scope.activePopover = null;
        });
    });


    $scope.dropMilestone = function (event) {
        moveItemInArray($scope.projectlist.miles, event.previousIndex, event.currentIndex);
    };

    $scope.dropTask = function (event, milestone) {
        moveItemInArray(milestone.tasks, event.previousIndex, event.currentIndex);
    };

    $scope.dropSubTask = function (event, task) {
        moveItemInArray(task.subtasks, event.previousIndex, event.currentIndex);
    };

    function moveItemInArray(arr, from, to) {
        const item = arr.splice(from, 1)[0];
        arr.splice(to, 0, item);
    }


    /* ==========================================================
       V10 Angular row drag/drop - Chrome safe
       Start is called by ng-mousedown, so it does not depend on
       angular.element(row).scope() (which can fail when debugInfo is off).
    ========================================================== */
    $scope.startPmsRowDrag = function ($event, rowType, dragItem, dragList, parentItem) {
        var e = $event && ($event.originalEvent || $event);
        if (!e || (e.button !== undefined && e.button !== 0)) return;
        if (!dragItem || !dragList || !dragList.length) return;
        if (e.preventDefault) e.preventDefault();
        if (e.stopPropagation) e.stopPropagation();

        function closest(el, selector) {
            while (el && el !== document) {
                if (el.matches && el.matches(selector)) return el;
                el = el.parentNode;
            }
            return null;
        }
        var startRow = closest(e.target, 'tr[data-pms-row]');
        if (!startRow) return;
        var startIndex = dragList.indexOf(dragItem);
        if (startIndex < 0) return;

        var requiredParent = rowType === 'milestone'
            ? String(($scope.projectlist && $scope.projectlist.id) || (parentItem && parentItem.id) || 'project')
            : String((parentItem && parentItem.id) || '');

        var ghost = startRow.cloneNode(true);
        ghost.className = 'pms-drag-ghost-v10';
        ghost.style.width = startRow.offsetWidth + 'px';
        document.body.appendChild(ghost);

        if (!document.getElementById('pms-row-drag-v10-style')) {
            var style = document.createElement('style');
            style.id = 'pms-row-drag-v10-style';
            style.innerHTML =
                '.pms-row-drag-handle{display:inline-flex;align-items:center;justify-content:center;width:20px;height:20px;margin-right:4px;cursor:grab;color:#64748b;vertical-align:middle;user-select:none;-webkit-user-drag:none;}' +
                '.pms-row-drag-handle *{pointer-events:none;}' +
                'body.pms-row-dragging-v10{user-select:none;cursor:grabbing;}' +
                '.pms-row-drop.drag-row-moving{opacity:.55;}' +
                '.pms-row-drop.drag-row-over td{box-shadow:inset 0 3px 0 #198754;background:rgba(25,135,84,.08)!important;}' +
                '.pms-row-drop.drag-row-invalid td{box-shadow:inset 0 3px 0 #dc3545;background:rgba(220,53,69,.08)!important;}' +
                '.pms-drag-ghost-v10{position:fixed;z-index:2147483647;opacity:.88;background:#fff;border:1px solid #b6c2d2;box-shadow:0 12px 30px rgba(15,23,42,.22);border-radius:6px;overflow:hidden;pointer-events:none;}' +
                '.pms-drag-ghost-v10 td{padding:6px 8px;white-space:nowrap;}';
            document.head.appendChild(style);
        }

        function clearMark() {
            var rows = document.querySelectorAll('.pms-row-drop.drag-row-over,.pms-row-drop.drag-row-invalid,.pms-row-drop.drag-row-moving');
            angular.forEach(rows, function (r) {
                r.classList.remove('drag-row-over');
                r.classList.remove('drag-row-invalid');
                r.classList.remove('drag-row-moving');
            });
        }
        function setGhost(x, y) {
            ghost.style.left = (x + 12) + 'px';
            ghost.style.top = (y + 12) + 'px';
        }
        function validTarget(row) {
            if (!row || row === startRow) return false;
            if (row.getAttribute('data-pms-row') !== rowType) return false;
            if (rowType !== 'milestone' && String(row.getAttribute('data-pms-parent-id') || '') !== requiredParent) return false;
            return true;
        }
        function getTargetIndex(row) {
            var idx = parseInt(row.getAttribute('data-pms-index'), 10);
            if (!isNaN(idx) && idx >= 0 && idx < dragList.length) return idx;
            var id = String(row.getAttribute('data-pms-id') || '');
            for (var i = 0; i < dragList.length; i++) {
                if (String(dragList[i].id || '') === id) return i;
            }
            return -1;
        }

        var lastTargetRow = null;
        function onMove(ev) {
            if (ev.preventDefault) ev.preventDefault();
            setGhost(ev.clientX, ev.clientY);
            var el = document.elementFromPoint(ev.clientX, ev.clientY);
            var row = closest(el, 'tr[data-pms-row]');
            clearMark();
            startRow.classList.add('drag-row-moving');
            lastTargetRow = null;
            if (!row || row === startRow) return;
            if (validTarget(row)) { row.classList.add('drag-row-over'); lastTargetRow = row; }
            else { row.classList.add('drag-row-invalid'); }
        }
        function onUp(ev) {
            if (ev && ev.preventDefault) ev.preventDefault();
            document.removeEventListener('mousemove', onMove, true);
            document.removeEventListener('mouseup', onUp, true);
            document.body.classList.remove('pms-row-dragging-v10');
            var targetRow = lastTargetRow;
            if (ghost && ghost.parentNode) ghost.parentNode.removeChild(ghost);
            clearMark();
            if (!targetRow || !validTarget(targetRow)) return;
            var fromIndex = dragList.indexOf(dragItem);
            var toIndex = getTargetIndex(targetRow);
            if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;
            $scope.$applyAsync(function () {
                var moved = dragList.splice(fromIndex, 1)[0];
                if (fromIndex < toIndex) toIndex = toIndex - 1;
                dragList.splice(toIndex, 0, moved);
                if (rowType === 'milestone' && $scope.refreshOrderNo) $scope.refreshOrderNo();
                if (rowType === 'task' && $scope.refreshOrderNo_tasks) $scope.refreshOrderNo_tasks(parentItem);
                if (rowType === 'subtask' && $scope.refreshOrderNo_subtasks) $scope.refreshOrderNo_subtasks(parentItem);
            });
        }
        document.body.classList.add('pms-row-dragging-v10');
        startRow.classList.add('drag-row-moving');
        setGhost(e.clientX, e.clientY);
        document.addEventListener('mousemove', onMove, true);
        document.addEventListener('mouseup', onUp, true);
    };



    //#region manage gantt resise
    $scope.dragInfo = null;

    $scope.startDragBar = function ($event, row) {

        if ($($event.target).hasClass('resize-handle')) {
            return;
        }

        $event.preventDefault();

        $scope.dragInfo = {
            mode: 'move',
            row: row,
            startX: $event.pageX,
            startDate: new Date(row.start),
            endDate: new Date(row.end)
        };

        angular.element(document).on('mousemove', onBarMouseMove);
        angular.element(document).on('mouseup', onBarMouseUp);
    };

    $scope.startResizeBar = function ($event, row, side) {
        $event.stopPropagation();
        $event.preventDefault();

        $scope.dragInfo = {
            mode: 'resize',
            side: side,
            row: row,
            startX: $event.pageX,
            startDate: new Date(row.start),
            endDate: new Date(row.end)
        };

        angular.element(document).on('mousemove', onBarMouseMove);
        angular.element(document).on('mouseup', onBarMouseUp);
    };

    function onBarMouseMove(e) {
        if ($scope.ganttScale == "week" ||
            $scope.ganttScale == "month") {
            return;
        }
        if (!$scope.dragInfo) return;

        var info = $scope.dragInfo;

        var dx = e.pageX - info.startX;

        var days = Math.round(dx / $scope.ganttCellWidth);

        $scope.$applyAsync(function () {
            //alert(info.mode);
            if (info.mode === 'move') {

                info.row.start = $scope.addDays(info.startDate, days);
                info.row.end = $scope.addDays(info.endDate, days);

            }

            else if (info.mode === 'resize') {

                if (info.side === 'left') {

                    var newStart = $scope.addDays(info.startDate, days);

                    if (newStart < info.row.end) {
                        info.row.start = newStart;
                    }
                }

                if (info.side === 'right') {

                    var newEnd = $scope.addDays(info.endDate, days);

                    if (newEnd > info.row.start) {
                        info.row.end = newEnd;

                    }
                }
            }
        });
    }
    function getDaysBetween(start, end) {

        if (!start || !end) return 0;

        // REMOVE TIME
        var s = new Date(start.getFullYear(), start.getMonth(), start.getDate());
        var e = new Date(end.getFullYear(), end.getMonth(), end.getDate());

        // DIFFERENCE
        var diff = e - s;

        // DAYS
        return Math.floor(diff / (1000 * 60 * 60 * 24));
    }
    function onBarMouseMove2(e) {

        if (!$scope.dragInfo) return;

        var info = $scope.dragInfo;

        var dx = e.pageX - info.startX;

        var units = Math.round(dx / $scope.ganttCellWidth);

        function moveByScale(date, value) {

            var d = new Date(date);

            // DAY SCALE
            if ($scope.ganttScale === "day") {

                d.setDate(d.getDate() + value);
            }

            // WEEK SCALE
            else if ($scope.ganttScale === "week") {

                d.setDate(d.getDate() + (value * 7));
            }

            // MONTH SCALE
            else if ($scope.ganttScale === "month") {

                d.setMonth(d.getMonth() + value);
            }

            return d;
        }

        function isBeyondMax(date) {

            return date > $scope.ganttEnd;
        }

        function isBeforeMin(date) {

            return date < $scope.ganttStart;
        }

        $scope.$applyAsync(function () {

            // ================= MOVE =================
            if (info.mode === 'move') {

                var newStart = moveByScale(info.startDate, units);

                var newEnd = moveByScale(info.endDate, units);

                // STOP AT MAX
                if (isBeyondMax(newEnd)) {
                    return;
                }

                // STOP AT MIN
                if (isBeforeMin(newStart)) {
                    return;
                }

                info.row.start = newStart;

                info.row.end = newEnd;
            }

            // ================= RESIZE =================
            else if (info.mode === 'resize') {

                // LEFT RESIZE
                if (info.side === 'left') {

                    var leftDate = moveByScale(info.startDate, units);

                    if (leftDate < info.row.end &&
                        !isBeforeMin(leftDate)) {

                        info.row.start = leftDate;
                    }
                }

                // RIGHT RESIZE
                else if (info.side === 'right') {

                    var rightDate = moveByScale(info.endDate, units);

                    if (rightDate > info.row.start &&
                        !isBeyondMax(rightDate)) {

                        info.row.end = rightDate;
                    }
                }
            }
        });
    }
    function onBarMouseMove3(e) {

        if (!$scope.dragInfo) return;

        var info = $scope.dragInfo;

        var dx = e.pageX - info.startX;

        var units = Math.round(dx / $scope.ganttCellWidth);

        // ================= DATE MOVE BY SCALE =================
        function moveByScale(date, value) {

            var d = new Date(date);

            // DAY
            if ($scope.ganttScale === "day") {

                d.setDate(d.getDate() + value);
            }

            // WEEK
            else if ($scope.ganttScale === "week") {

                d.setDate(d.getDate() + (value * 7));
            }

            // MONTH
            else if ($scope.ganttScale === "month") {

                d.setMonth(d.getMonth() + value);
            }

            return d;
        }

        // ================= MIN GAP =================
        function getMinGap() {

            if ($scope.ganttScale === "day") {
                return 1;
            }

            if ($scope.ganttScale === "week") {
                return 7;
            }

            return 30;
        }

        // ================= APPLY =================
        $scope.$applyAsync(function () {

            // =====================================================
            // MOVE BAR
            // =====================================================
            if (info.mode === 'move') {

                var newStart = moveByScale(info.startDate, units);

                var newEnd = moveByScale(info.endDate, units);

                // STOP LEFT
                if (newStart < $scope.ganttStart) {

                    var diff1 = Math.ceil(
                        ($scope.ganttStart - newStart) / (1000 * 60 * 60 * 24)
                    );

                    newStart = $scope.addDays(newStart, diff1);

                    newEnd = $scope.addDays(newEnd, diff1);
                }

                // STOP RIGHT
                if (newEnd > $scope.ganttEnd) {

                    var diff2 = Math.ceil(
                        (newEnd - $scope.ganttEnd) / (1000 * 60 * 60 * 24)
                    );

                    newStart = $scope.addDays(newStart, -diff2);

                    newEnd = $scope.addDays(newEnd, -diff2);
                }

                info.row.start = newStart;

                info.row.end = newEnd;
            }

            // =====================================================
            // RESIZE BAR
            // =====================================================
            else if (info.mode === 'resize') {

                // ================= LEFT RESIZE =================
                if (info.side === 'left') {

                    var leftDate = moveByScale(info.startDate, units);

                    // MIN LIMIT
                    if (leftDate < $scope.ganttStart) {

                        leftDate = new Date($scope.ganttStart);
                    }

                    // SHOULD NOT CROSS END
                    if (leftDate >= info.row.end) {

                        leftDate = new Date(info.row.end);

                        if ($scope.ganttScale === "day") {

                            leftDate.setDate(leftDate.getDate() - 1);
                        }
                        else if ($scope.ganttScale === "week") {

                            leftDate.setDate(leftDate.getDate() - 7);
                        }
                        else if ($scope.ganttScale === "month") {

                            leftDate.setMonth(leftDate.getMonth() - 1);
                        }
                    }

                    info.row.start = leftDate;
                }

                // ================= RIGHT RESIZE =================
                else if (info.side === 'right') {
                    var rightDate = moveByScale(info.endDate, units);
                    // MAX LIMIT
                    if (rightDate > $scope.ganttEnd) {

                        rightDate = new Date($scope.ganttEnd);
                    }
                    // SHOULD NOT CROSS START
                    if (rightDate <= info.row.start) {
                        rightDate = new Date(info.row.start);
                        if ($scope.ganttScale === "day") {
                            rightDate.setDate(rightDate.getDate() + 1);
                        }
                        else if ($scope.ganttScale === "week") {

                            rightDate.setDate(rightDate.getDate() + 7);
                        }
                        else if ($scope.ganttScale === "month") {

                            rightDate.setMonth(rightDate.getMonth() + 1);
                        }
                    }

                    info.row.end = rightDate;
                }
            }
        });
    }

    function onBarMouseUp() {
        if ($scope.ganttScale == "week" ||
            $scope.ganttScale == "month") {
            return;
        }
        angular.element(document).off('mousemove', onBarMouseMove);
        angular.element(document).off('mouseup', onBarMouseUp);
        //alert("ok");
        if ($scope.dragInfo) {

            var row = $scope.dragInfo.row;
            var chkmode = $scope.dragInfo.mode;
            var nofdays = 0;
            if (chkmode == "resize") {
                nofdays = getDaysBetween(row.start, row.end);
            }
            if (row.type === "milestone") {
                row.raw.StartDate = new Date(row.start);
                row.raw.EndDate = new Date(row.end);
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === row.id);
                if (mindex !== -1) {
                    let c1 = $scope.projectlist.miles[mindex];
                    if (chkmode == "resize") { c1.txtdays = nofdays; }
                    $scope.resetStartDateHierarchy_mile(c1, $scope.projectlist, mindex);
                }
            }

            else if (row.type === "task") {

                row.raw.Startdate = new Date(row.start);
                row.raw.Targetdate = new Date(row.end);
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === row.Milestoneid);
                let c = $scope.projectlist.miles[mindex];
                let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === row.id);
                if (tindex !== -1) {
                    let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
                    if (chkmode == "resize") { c1.txtdays = nofdays; }
                    $scope.resetStartDateHierarchy_task(c1, c, tindex);
                }
            }

            else if (row.type === "subtask") {

                row.raw.Startdate = new Date(row.start);
                row.raw.Targetdate = new Date(row.end);
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === row.Milestoneid);
                let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === row.Task);
                if (tindex !== -1) {
                    let c = $scope.projectlist.miles[mindex].tasks[tindex];
                    let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === row.id);
                    if (sindex !== -1) {
                        let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                        if (chkmode == "resize") { c1.txtdays = nofdays; }
                        $scope.resetStartDateHierarchy_subtask(c1, c, tindex);
                    }
                }
            }
            console.log("UPDATED:", row.start, row.end);

            // save api here
            // updatedetails_gant(row);

            $scope.dragInfo = null;

            $scope.$applyAsync();
        }
    }
//#endregion
//// end scopes
});

function initTableUI() {

    // ===== 1. DESTROY TOOLTIP FIRST =====
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {

        let instance = bootstrap.Tooltip.getInstance(el);

        if (instance) {
            instance.dispose();
        }

    });

    // ===== 2. RECREATE TOOLTIP AFTER CLEAN =====
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
        new bootstrap.Tooltip(el);
    });


    // ===== 3. HANDLE DATATABLE SAFELY =====
    try {

        let tableEl = document.querySelector('.datatable');

        if (tableEl) {

            if ($.fn.DataTable.isDataTable(tableEl)) {
                $(tableEl).DataTable().clear().destroy();
            }

            setTimeout(function () {

                $(tableEl).DataTable({
                    scrollY: '300px',
                    scrollCollapse: true,
                    paging: false,
                    searching: true,
                    info: false
                });

            }, 50);
        }

    } catch (e) {

        console.log(e);
    }
    finally {

    }
    initProjectTableColumnDragDrop();

}
function getProjectTableElement() {
    return document.querySelector('.tbl_new');
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
    // alert("ok");
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
        return document.querySelector('.tbl_new');
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
            .tbl_new thead th{ position:sticky; }
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
            .tbl_new thead th.column-draggable{ cursor:move; }
            .tbl_new thead th.column-draggable::after{ content:"" !important; }
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
                b1.innerHTML = '<i class="bx bx-caret-left me-1" title="Add Before"></i> ';

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
                b2.innerHTML = ' <i class="bx bx-caret-right ms-1" title="Add After"></i>';

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


/* ==========================================================
   V9 - Chrome + Firefox Smooth Hierarchy Row Drag & Drop
   Milestone  -> move only inside project milestone list
   Task       -> move only inside the same milestone
   Sub Task   -> move only inside the same task
   Important: this version does NOT use HTML5 drag/drop. Chrome often
   shows a red no-drop icon inside tables with native drag/drop.
   This mouse/pointer based version is smoother and browser safe.
========================================================== */
(function () {
    var dragState = null;
    var lastTargetRow = null;

    function closestRow(el) {
        while (el && el !== document) {
            if (el.getAttribute && el.getAttribute('data-pms-row')) return el;
            el = el.parentNode;
        }
        return null;
    }

    function getRowScope(row) {
        try { return angular.element(row).scope(); } catch (e) { return null; }
    }

    function rowInfo(row) {
        var sc = getRowScope(row);
        if (!sc) return null;

        var type = row.getAttribute('data-pms-row');
        var item = null, parent = null, list = null, index = -1;

        if (type === 'milestone') {
            item = sc.c;
            parent = sc.projectlist || (sc.$parent && sc.$parent.projectlist);
            list = parent && parent.miles;
        } else if (type === 'task') {
            item = sc.c1;
            parent = sc.c;
            list = parent && parent.tasks;
        } else if (type === 'subtask') {
            item = sc.c2;
            parent = sc.c1;
            list = parent && parent.subtasks;
        }

        if (!item || !list) return null;
        index = list.indexOf(item);
        return { scope: sc, row: row, type: type, item: item, parent: parent, list: list, index: index };
    }

    function sameParent(a, b) {
        if (!a || !b || a.type !== b.type) return false;
        if (a.type === 'milestone') return true;
        if (!a.parent || !b.parent) return false;
        return String(a.parent.id || '') === String(b.parent.id || '');
    }

    function clearRowClasses() {
        var rows = document.querySelectorAll('.pms-row-drop.drag-row-over, .pms-row-drop.drag-row-invalid, .pms-row-drop.drag-row-moving');
        for (var i = 0; i < rows.length; i++) {
            rows[i].classList.remove('drag-row-over');
            rows[i].classList.remove('drag-row-invalid');
            rows[i].classList.remove('drag-row-moving');
        }
    }

    function saveOrder(info) {
        if (!info || !info.scope) return;
        if (info.type === 'milestone' && info.scope.refreshOrderNo) {
            info.scope.refreshOrderNo();
        } else if (info.type === 'task' && info.scope.refreshOrderNo_tasks) {
            info.scope.refreshOrderNo_tasks(info.parent);
        } else if (info.type === 'subtask' && info.scope.refreshOrderNo_subtasks) {
            info.scope.refreshOrderNo_subtasks(info.parent);
        }
    }

    function moveRow(from, to) {
        if (!from || !to || !sameParent(from, to)) return false;
        from.index = from.list.indexOf(from.item);
        to.index = to.list.indexOf(to.item);
        if (from.index < 0 || to.index < 0 || from.index === to.index) return false;
        var moved = from.list.splice(from.index, 1)[0];
        var newIndex = to.index;
        if (from.index < to.index) newIndex = to.index - 1;
        from.list.splice(newIndex, 0, moved);
        saveOrder(from);
        return true;
    }

    function makeGhost(row, startX, startY) {
        var ghost = row.cloneNode(true);
        ghost.className = 'pms-drag-ghost';
        ghost.style.width = row.offsetWidth + 'px';
        ghost.style.left = startX + 12 + 'px';
        ghost.style.top = startY + 12 + 'px';
        ghost.style.pointerEvents = 'none';
        document.body.appendChild(ghost);
        return ghost;
    }

    function getPoint(e) {
        if (e.touches && e.touches.length) return { x: e.touches[0].clientX, y: e.touches[0].clientY };
        if (e.changedTouches && e.changedTouches.length) return { x: e.changedTouches[0].clientX, y: e.changedTouches[0].clientY };
        return { x: e.clientX, y: e.clientY };
    }

    function updateTargetAt(x, y) {
        if (!dragState) return;
        var el = document.elementFromPoint(x, y);
        var targetRow = closestRow(el);
        clearRowClasses();
        dragState.row.classList.add('drag-row-moving');
        lastTargetRow = null;
        if (!targetRow || targetRow === dragState.row) return;
        var target = rowInfo(targetRow);
        if (!target) return;
        lastTargetRow = targetRow;
        if (sameParent(dragState.info, target)) targetRow.classList.add('drag-row-over');
        else targetRow.classList.add('drag-row-invalid');
    }

    function onMove(e) {
        if (!dragState) return;
        if (e.cancelable !== false) e.preventDefault();
        var p = getPoint(e);
        dragState.ghost.style.left = (p.x + 12) + 'px';
        dragState.ghost.style.top = (p.y + 12) + 'px';
        updateTargetAt(p.x, p.y);
    }

    function onUp(e) {
        if (!dragState) return;
        if (e.cancelable !== false) e.preventDefault();
        var targetRow = lastTargetRow;
        var target = targetRow ? rowInfo(targetRow) : null;
        var from = dragState.info;
        var sc = (target && target.scope) || from.scope;
        if (dragState.ghost && dragState.ghost.parentNode) dragState.ghost.parentNode.removeChild(dragState.ghost);
        document.body.classList.remove('pms-row-dragging-active');
        document.removeEventListener('mousemove', onMove, true);
        document.removeEventListener('mouseup', onUp, true);
        document.removeEventListener('touchmove', onMove, true);
        document.removeEventListener('touchend', onUp, true);
        document.removeEventListener('touchcancel', onUp, true);
        if (target && sameParent(from, target)) {
            sc.$applyAsync(function () {
                moveRow(from, target);
                dragState = null; lastTargetRow = null; clearRowClasses();
            });
        } else {
            dragState = null; lastTargetRow = null; clearRowClasses();
        }
    }

    function startDrag(e) {
        if (e.type === 'mousedown' && e.button !== 0) return;
        if (e.cancelable !== false) e.preventDefault();
        e.stopPropagation();
        var row = closestRow(this);
        var info = rowInfo(row);
        if (!row || !info || info.index < 0) return;
        var p = getPoint(e);
        dragState = { row: row, info: info, ghost: makeGhost(row, p.x, p.y) };
        document.body.classList.add('pms-row-dragging-active');
        row.classList.add('drag-row-moving');
        document.addEventListener('mousemove', onMove, true);
        document.addEventListener('mouseup', onUp, true);
        document.addEventListener('touchmove', onMove, { capture: true, passive: false });
        document.addEventListener('touchend', onUp, true);
        document.addEventListener('touchcancel', onUp, true);
    }

    function bindRowDragDrop() {
        return;// disabled in v10
        var table = document.querySelector('.tbl_new');
        if (!table) return;
        var handles = table.querySelectorAll('.pms-row-drag-handle');
        for (var i = 0; i < handles.length; i++) {
            var h = handles[i];
            h.setAttribute('draggable', 'false');
            if (h.dataset.rowDragBoundV9 === '1') continue;
            h.dataset.rowDragBoundV9 = '1';
            h.addEventListener('dragstart', function (e) { e.preventDefault(); return false; }, true);
            h.addEventListener('mousedown', startDrag, true);
            h.addEventListener('touchstart', startDrag, true);
        }
    }

    function addRowDragStyles() {
        if (document.getElementById('pms-row-drag-v9-style')) return;
        var css = document.createElement('style');
        css.id = 'pms-row-drag-v9-style';
        css.innerHTML =
            '.pms-row-drag-handle{display:inline-flex;align-items:center;justify-content:center;width:18px;height:18px;margin-right:4px;cursor:grab;color:#64748b;vertical-align:middle;touch-action:none;user-select:none;-webkit-user-drag:none;}' +
            '.pms-row-drag-handle *{pointer-events:none;}' +
            '.pms-row-drag-handle:active{cursor:grabbing;}' +
            'body.pms-row-dragging-active{user-select:none;cursor:grabbing;}' +
            '.pms-row-drop.drag-row-moving{opacity:.55;}' +
            '.pms-row-drop.drag-row-over td{box-shadow:inset 0 3px 0 #0d6efd;background:rgba(13,110,253,.05);}' +
            '.pms-row-drop.drag-row-invalid td{box-shadow:inset 0 3px 0 #dc3545;background:rgba(220,53,69,.06);}' +
            '.pms-drag-ghost{position:fixed;z-index:2147483647;opacity:.86;background:#fff;border:1px solid #b6c2d2;box-shadow:0 12px 30px rgba(15,23,42,.22);border-radius:6px;overflow:hidden;}' +
            '.pms-drag-ghost td{padding:6px 8px;white-space:nowrap;}';
        document.head.appendChild(css);
    }

    function initRowDragV9() { addRowDragStyles(); bindRowDragDrop(); }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', initRowDragV9);
    else initRowDragV9();
    if (window.MutationObserver) {
        var mo = new MutationObserver(function () { setTimeout(initRowDragV9, 0); });
        mo.observe(document.documentElement, { childList: true, subtree: true });
    }
})();
