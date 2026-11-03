(function () {
    'use strict';

    angular.module('pmsUserDashboardApp', ['pmsUserDashboard.services'])
        .directive('nudSelected', function () {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    scope.$watch(attrs.nudSelected, function (selected) {
                        var node = element[0], isCard = node.classList.contains('kpi');
                        node.setAttribute('aria-pressed', selected ? 'true' : 'false');
                        node.style.setProperty('background-color', selected ? (isCard ? '#efe6f5' : '#5a2a7a') : '#ffffff', 'important');
                        if (isCard) {
                            node.style.setProperty('border-bottom', '3px solid ' + (selected ? '#5a2a7a' : 'transparent'), 'important');
                        } else {
                            node.style.setProperty('color', selected ? '#ffffff' : '#1d1a24', 'important');
                            node.style.setProperty('border-color', selected ? '#5a2a7a' : '#e1dde8', 'important');
                        }
                    });
                }
            };
        })
        .directive('udHoursTooltip', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var timer = $timeout(function () {
                        var node = element[0];
                        var title = attrs.dataBsTitle || node.getAttribute('data-bs-title') || '';
                        node.setAttribute('title', title);
                        var bootstrapApi = window.bootstrap;
                        if (!title || !bootstrapApi || !bootstrapApi.Tooltip) return;
                        node.setAttribute('data-bs-title', title);
                        node.setAttribute('data-bs-original-title', title);
                        if (bootstrapApi.Tooltip.getOrCreateInstance) {
                            bootstrapApi.Tooltip.getOrCreateInstance(node, {
                                title: title, trigger: 'hover', placement: 'top', container: 'body', boundary: 'viewport'
                            });
                        } else {
                            new bootstrapApi.Tooltip(node, {
                                title: title, trigger: 'hover', placement: 'top', container: 'body', boundary: 'viewport'
                            });
                        }
                    }, 0, false);
                    scope.$on('$destroy', function () {
                        $timeout.cancel(timer);
                        var bootstrapApi = window.bootstrap;
                        if (bootstrapApi && bootstrapApi.Tooltip) {
                            var instance = bootstrapApi.Tooltip.getInstance(element[0]);
                            if (instance) instance.dispose();
                        }
                    });
                }
            };
        }])
        .controller('UserDashboardController', ['$scope', 'UserDashboardService', function ($scope, UserDashboardService) {
            var vm = this;
            vm.loading = true;
            vm.hasLoaded = false;
            vm.error = '';
            var requestSequence = 0;
            var hoursRequestSequence = 0;
            var mainFilterHours = { days: [] };
            var hoursPeriodSource = 'main';
            vm.hoursAxisMax = 9;
            vm.hoursAxisTicks = [];
            vm.hoursLoading = false;
            vm.hoursLoaded = false;
            vm.hoursError = '';
            vm.hoursPeriod = 'all';
            vm.hoursPeriods = [
                { value: 'all', label: 'All' },
                { value: 'week', label: 'This Week' },
                { value: 'today', label: 'Today' }
            ];
            vm.rangeMenuOpen = false;
            vm.customMode = false;
            vm.rangeError = '';
            vm.presets = ['All', 'Today', 'This Week', 'This Month', 'This Quarter', 'This Year', 'Yesterday', 'Previous Week', 'Previous Month', 'Previous Quarter', 'Previous Year', 'Custom Range'];
            vm.selectedPreset = 'Today';
            vm.data = {};

            var dashboardTimeZone = 'Asia/Dubai';
            function dateOnly(date) {
                return new Date(date.getFullYear(), date.getMonth(), date.getDate());
            }
            function globalToday() {
                var parts = new Intl.DateTimeFormat('en-US', {
                    timeZone: dashboardTimeZone,
                    year: 'numeric', month: '2-digit', day: '2-digit'
                }).formatToParts(new Date());
                var values = {};
                parts.forEach(function (part) { values[part.type] = part.value; });
                return new Date(Number(values.year), Number(values.month) - 1, Number(values.day));
            }
            function addDays(date, count) {
                var result = dateOnly(date);
                result.setDate(result.getDate() + count);
                return result;
            }
            function startOfWeek(date) {
                return addDays(date, -date.getDay());
            }
            function startOfHoursWeek(date) {
                return addDays(date, -((date.getDay() + 6) % 7));
            }
            function endOfMonth(date) {
                return new Date(date.getFullYear(), date.getMonth() + 1, 0);
            }
            function startOfQuarter(date) {
                return new Date(date.getFullYear(), Math.floor(date.getMonth() / 3) * 3, 1);
            }
            function endOfQuarter(date) {
                return new Date(date.getFullYear(), Math.floor(date.getMonth() / 3) * 3 + 3, 0);
            }
            function toApiDate(date) {
                var d = dateOnly(date);
                return ('0' + d.getDate()).slice(-2) + '/' + ('0' + (d.getMonth() + 1)).slice(-2) + '/' + d.getFullYear();
            }
            function toDateKey(date) {
                var d = dateOnly(date);
                return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
            }
            vm.reportUrl = function (reportType) {
                var titles = { status: 'Task Status Report', priority: 'Task Priority Report', situation: 'Task Situation Report', tag: 'Tag Report', hours: 'Task Hours Report', taskEfficiency: 'Task Efficiency Report', timeEfficiency: 'Time Efficiency Report', issues: 'Issues Report' };
                var title = titles[reportType] || 'Dashboard Report';
                var filterType = vm.selectedPreset || 'Today';
                //var url = '/pms/userreports/default.aspx?type=' + encodeURIComponent(reportType) +
                var url = '/pms/userreports/?type=' + encodeURIComponent(reportType) +
                    '&filterType=' + encodeURIComponent(filterType) +
                    '&title=' + encodeURIComponent(title);
                if (filterType === 'Custom Range' && (reportType !== 'hours' || hoursPeriodSource === 'main')) {
                    url += '&fromDate=' + encodeURIComponent(toDateKey(vm.rangeFrom || globalToday())) +
                        '&toDate=' + encodeURIComponent(toDateKey(vm.rangeTo || globalToday()));
                }
                return url +
                    '&hoursPeriod=' + encodeURIComponent(vm.hoursPeriod || 'all') +
                    '&hoursSource=' + encodeURIComponent(hoursPeriodSource || 'main');
            };
            function timeToHours(value) {
                var parts = String(value || '00:00').split(':');
                return (parseInt(parts[0], 10) || 0) + ((parseInt(parts[1], 10) || 0) / 60);
            }
            function updateHoursAxis(shiftHours) {
                var hours = Number(shiftHours);
                if (!isFinite(hours) || hours <= 0) hours = vm.hoursAxisMax;
                vm.hoursAxisMax = hours;
                vm.hoursAxisTicks = [];
                var maximumMinutes = Math.round(hours * 60);
                function addTick(minutes) {
                    vm.hoursAxisTicks.push({
                        label: ('0' + Math.floor(minutes / 60)).slice(-2) + ':' + ('0' + minutes % 60).slice(-2),
                        top: (1 - minutes / maximumMinutes) * 100
                    });
                }
                addTick(maximumMinutes);
                var tickStep = Math.max(60, Math.ceil(maximumMinutes / 360) * 60);
                for (var minutes = Math.floor((maximumMinutes - 1) / tickStep) * tickStep; minutes >= 0; minutes -= tickStep) {
                    addTick(minutes);
                }
            }
            updateHoursAxis(vm.hoursAxisMax);
            function leaveTooltip(days, key, fromKey, toKey) {
                return (days || []).filter(function (leave) {
                    return key ? leave.key === key : leave.key >= fromKey && leave.key <= toKey;
                }).map(function (leave) { return (key ? '' : leave.key + ' : ') + leave.label; }).join('; ');
            }
            function addHoursTooltips(hours) {
                function formatHours(value) {
                    var minutes = Math.max(0, Math.round((Number(value) || 0) * 60));
                    return ('0' + Math.floor(minutes / 60)).slice(-Math.max(2, String(Math.floor(minutes / 60)).length)) +
                        ':' + ('0' + (minutes % 60)).slice(-2);
                }
                function formatSpentHours(value) {
                    var seconds = Math.max(0, Math.round((Number(value) || 0) * 3600));
                    return ('0' + Math.floor(seconds / 3600)).slice(-Math.max(2, String(Math.floor(seconds / 3600)).length)) + ':' +
                        ('0' + Math.floor(seconds / 60) % 60).slice(-2) + ':' + ('0' + seconds % 60).slice(-2);
                }
                (hours.days || []).forEach(function (day) {
                    day.assignedTooltip = 'Assigned Time : ' + (day.assignedText || formatHours(day.assigned)) + ' hrs' +
                        (Number(hours.lunchHours) > 0 && Number(day.assigned) > 0 ? ' | Lunch : ' + formatHours(hours.lunchHours) + ' hrs' : '');
                    day.leaveLabel = leaveTooltip(hours.leaveDays, day.key, day.fromKey, day.toKey);
                    if (day.leaveLabel) day.assignedTooltip += ' | Leave : ' + day.leaveLabel;
                    day.spentTooltip = 'Spent Time : ' + (day.spentText || formatSpentHours(day.spent)) + ' hrs' + (day.leaveLabel ? ' | Leave : ' + day.leaveLabel : '');
                });
                return hours;
            }
            function weeklyReportToHours(report, period, todayKey, leaveData) {
                var days = report && report.days ? report.days : [];
                if (period === 'today') days = days.filter(function (day) { return day.key === todayKey; });
                return days.map(function (day) {
                    var assignedText = day.totalTime || '00:00';
                    // Weekly View totalTime includes lunch; its shiftTime is the full shift.
                    var lunchHours = Math.max(0, timeToHours(report.shiftTime) - Number(mainFilterHours.shiftHours || vm.hoursAxisMax));
                    var netMinutes = Math.max(0, Math.round((timeToHours(assignedText) - lunchHours) * 60));
                    var leave = (leaveData && leaveData.days || []).filter(function (item) { return item.key === day.key; })[0];
                    if (leave) netMinutes = Math.min(netMinutes, Math.max(0, Math.round(Number(mainFilterHours.shiftHours || vm.hoursAxisMax) * 60 * (1 - leave.fraction))));
                    assignedText = ('0' + Math.floor(netMinutes / 60)).slice(-2) + ':' + ('0' + netMinutes % 60).slice(-2);
                    var spentText = day.actualTime || '00:00';
                    var label = period === 'today' ? 'Today' : day.key;
                    if (period !== 'today' && day.key) {
                        var parts = day.key.split('-');
                        label = new Date(Number(parts[0]), Number(parts[1]) - 1, Number(parts[2])).toLocaleString('en-US', { weekday: 'short' });
                    }
                    return {
                        day: label,
                        key: day.key,
                        leaveLabel: leave ? leave.label : '',
                        assigned: timeToHours(assignedText),
                        spent: timeToHours(spentText),
                        assignedText: assignedText,
                        spentText: spentText,
                        assignedTooltip: 'Assigned Time : ' + assignedText + ' hrs' + (netMinutes > 0 && lunchHours > 0 ? ' | Lunch : ' + ('0' + Math.floor(lunchHours)).slice(-2) + ':' + ('0' + Math.round(lunchHours * 60) % 60).slice(-2) + ' hrs' : '') + (leave ? ' | Leave : ' + leave.label : ''),
                        spentTooltip: 'Spent Time : ' + spentText + ' hrs' + (leave ? ' | Leave : ' + leave.label : '')
                    };
                });
            }
            function hoursPeriodForMainPreset(preset) {
                if (preset === 'Today') return 'today';
                if (preset === 'This Week') return 'week';
                return 'all';
            }
            function setRange(start, end, label, closeMenu) {
                vm.rangeFrom = dateOnly(start);
                vm.rangeTo = dateOnly(end);
                vm.customFromDate = new Date(vm.rangeFrom.getTime());
                vm.customToDate = new Date(vm.rangeTo.getTime());
                vm.rangeDisplay = toApiDate(vm.rangeFrom) + ' - ' + toApiDate(vm.rangeTo);
                vm.selectedPreset = label || '';
                vm.customMode = label === 'Custom Range';
                vm.rangeError = '';
                if (closeMenu) vm.rangeMenuOpen = false;
                vm.load(toApiDate(vm.rangeFrom), toApiDate(vm.rangeTo));
            }

            vm.load = function (fromDate, toDate) {
                var thisRequest = ++requestSequence;
                var thisHoursRequest = ++hoursRequestSequence;
                vm.hoursPeriod = 'all';
                hoursPeriodSource = 'main';
                vm.hoursLoading = true;
                vm.hoursError = '';
                vm.loading = true;
                vm.error = '';
                UserDashboardService.getDashboard(fromDate, toDate, vm.projectId).then(function (data) {
                    if (thisRequest !== requestSequence) return;
                    var currentHours = vm.data && vm.data.hours;
                    vm.data = data || {};
                    mainFilterHours = addHoursTooltips(vm.data.hours || { days: [] });
                    updateHoursAxis(Math.max.apply(Math, [Number(mainFilterHours.shiftHours)||9].concat((mainFilterHours.days||[]).map(function(d){return Math.max(Number(d.assigned)||0,Number(d.spent)||0);}))));
                    if (vm.hoursPeriod === 'all' || hoursPeriodSource === 'main') {
                        vm.data.hours = mainFilterHours;
                        vm.hoursLoaded = true;
                    } else if (currentHours) {
                        vm.data.hours = currentHours;
                    }
                    vm.hasLoaded = true;
                    vm.refreshDisplay();
                }).catch(function () {
                    if (thisRequest === requestSequence) vm.error = 'Unable to load dashboard data. Please try again.';
                }).finally(function () {
                    if (thisRequest !== requestSequence) return;
                    vm.loading = false;
                    if (thisHoursRequest === hoursRequestSequence) vm.hoursLoading = false;
                    $scope.$evalAsync();
                });
            };

            vm.loadTaskHours = function (period) {
                var thisRequest = ++hoursRequestSequence;
                vm.hoursPeriod = period;
                hoursPeriodSource = 'user';
                vm.hoursError = '';
                if (period === 'all') {
                    vm.hoursLoading = false;
                    vm.data = vm.data || {};
                    vm.data.hours = mainFilterHours;
                    vm.hoursLoaded = true;
                    $scope.$evalAsync();
                    return;
                }

                var today = globalToday();
                var weekStart = startOfHoursWeek(today);
                var weekEnd = addDays(weekStart, 6);
                var todayKey = toDateKey(today);
                vm.hoursLoading = true;
                vm.hoursError = '';
                UserDashboardService.getTaskHours(toApiDate(period === 'today' ? today : weekStart), toApiDate(period === 'today' ? today : weekEnd), vm.projectId).then(function (hours) {
                    if (thisRequest !== hoursRequestSequence) return;
                    vm.data = vm.data || {};
                    // Keep the net working-hour scale returned by the dashboard.
                    vm.data.hours = addHoursTooltips(hours || { days: [] });
                    updateHoursAxis(Math.max.apply(Math, [Number(vm.data.hours.shiftHours)||9].concat((vm.data.hours.days||[]).map(function(d){return Math.max(Number(d.assigned)||0,Number(d.spent)||0);}))));
                    vm.hoursLoaded = true;
                }).catch(function () {
                    if (thisRequest === hoursRequestSequence) vm.hoursError = 'Unable to update Task Hours.';
                }).finally(function () {
                    if (thisRequest !== hoursRequestSequence) return;
                    vm.hoursLoading = false;
                    $scope.$evalAsync();
                });
            };

            vm.hoursBarHeight = function (value) {
                var hours = Number(value) || 0;
                if (hours <= 0) return '0%';
                return Math.max(2, Math.min(100, hours / vm.hoursAxisMax * 100)) + '%';
            };

            vm.timeEfficiencyNeedle = function (value) {
                var percent = Math.max(0, Math.min(100, Number(value) || 0));
                return 'rotate(' + ((percent - 50) * 1.8) + 'deg)';
            };

            vm.toggleRangeMenu = function () {
                vm.rangeMenuOpen = !vm.rangeMenuOpen;
                vm.customMode = vm.rangeMenuOpen && vm.selectedPreset === 'Custom Range';
                vm.rangeError = '';
            };

            vm.openCalendar = function (event) {
                var input = event && event.currentTarget;
                if (!input) return;
                try {
                    if (typeof input.showPicker === 'function') {
                        input.showPicker();
                        return;
                    }
                } catch (ignore) { }
                input.focus();
            };

            vm.selectPreset = function (preset) {
                if (preset === 'Custom Range') {
                    vm.selectedPreset = preset;
                    vm.customMode = true;
                    vm.rangeError = '';
                    return;
                }
                var today = globalToday();
                var start = today;
                var end = today;
                var weekStart = startOfWeek(today);
                if (preset === 'All') start = new Date(2020, 0, 1);
                else if (preset === 'This Week') { start = weekStart; end = addDays(weekStart, 6); }
                else if (preset === 'This Month') { start = new Date(today.getFullYear(), today.getMonth(), 1); end = endOfMonth(today); }
                else if (preset === 'This Quarter') { start = startOfQuarter(today); end = endOfQuarter(today); }
                else if (preset === 'This Year') { start = new Date(today.getFullYear(), 0, 1); end = new Date(today.getFullYear(), 11, 31); }
                else if (preset === 'Yesterday') { start = addDays(today, -1); end = start; }
                else if (preset === 'Previous Week') { start = addDays(weekStart, -7); end = addDays(weekStart, -1); }
                else if (preset === 'Previous Month') { start = new Date(today.getFullYear(), today.getMonth() - 1, 1); end = new Date(today.getFullYear(), today.getMonth(), 0); }
                else if (preset === 'Previous Quarter') { start = new Date(startOfQuarter(today).getFullYear(), startOfQuarter(today).getMonth() - 3, 1); end = addDays(startOfQuarter(today), -1); }
                else if (preset === 'Previous Year') { start = new Date(today.getFullYear() - 1, 0, 1); end = new Date(today.getFullYear() - 1, 11, 31); }
                setRange(start, end, preset, true);
            };

            vm.applyCustomRange = function () {
                if (!(vm.customFromDate instanceof Date) || isNaN(vm.customFromDate.getTime()) || !(vm.customToDate instanceof Date) || isNaN(vm.customToDate.getTime())) {
                    vm.rangeError = 'Select both From Date and To Date.';
                    return;
                }
                if (dateOnly(vm.customToDate) < dateOnly(vm.customFromDate)) {
                    vm.rangeError = 'To Date must be on or after From Date.';
                    return;
                }
                setRange(vm.customFromDate, vm.customToDate, 'Custom Range', true);
            };


            vm.projectId = '';
            vm.projects = [{ id: '', name: 'All projects' }];
            vm.taskFilter = 'All';
            vm.percent = function (a,b) { return b ? Math.round(a / b * 100) : 0; };
            vm.hoursText = function(value) { var seconds=Math.max(0,Math.round((Number(value)||0)*3600)); return Math.floor(seconds/3600)+':'+('0'+Math.floor(seconds/60)%60).slice(-2)+':'+('0'+seconds%60).slice(-2); };
            vm.greeting = (new Date().getHours()<12?'Good morning':new Date().getHours()<17?'Good afternoon':'Good evening');
            vm.todayLabel = new Date().toLocaleDateString('en-GB',{weekday:'long',day:'numeric',month:'long',year:'numeric'});
            vm.changeProject = function() { vm.load(toApiDate(vm.rangeFrom),toApiDate(vm.rangeTo)); };
            vm.dueDays = function(t) { if(!t.due) return null; var parts=t.due.split('-'); return Math.round((new Date(+parts[0],+parts[1]-1,+parts[2])-globalToday())/86400000); };
            vm.overdue = function(t) { return !t.completed && t.due && vm.dueDays(t)<0; };
            vm.dueLabel = function(t) { var d=vm.dueDays(t); if(d===null) return 'No due date'; if(t.completed) return t.due; return d<0 ? t.due+' - '+(-d)+'d late' : d===0 ? 'Due today' : d===1 ? 'Due tomorrow' : t.due; };
            vm.selectTaskFilter = function (filter, scrollToGrid) {
                vm.taskFilter = filter;
                if (!scrollToGrid) return;
                // Wait for Angular to filter rows and finish updating the grid height.
                window.setTimeout(function () {
                    var grid = document.getElementById('nudMyTasks');
                    if (!grid) return;
                    var table = grid.querySelector('.tbl');
                    if (table) table.scrollTop = 0;
                    // The master page may scroll inside a wrapper instead of the window.
                    var parent = grid.parentElement;
                    while (parent && parent !== document.body && parent !== document.documentElement) {
                        var overflow = window.getComputedStyle(parent).overflowY;
                        if (parent.scrollHeight > parent.clientHeight && /auto|scroll|overlay|hidden/.test(overflow)) {
                            parent.scrollTop += grid.getBoundingClientRect().top - parent.getBoundingClientRect().top - parent.clientTop - 90;
                        }
                        parent = parent.parentElement;
                    }
                    var page = document.scrollingElement || document.documentElement;
                    var top = Math.max(0, grid.getBoundingClientRect().top + page.scrollTop - 90);
                    window.scrollTo({ top: top, behavior: 'instant' });
                }, 50);
            };
            vm.visibleTask = function(t) { return vm.taskFilter==='All' || (vm.taskFilter==='Overdue' ? vm.overdue(t) : t.status===vm.taskFilter); };
            vm.refreshDisplay = function() {
                var tasks=vm.data.tasks||[];
                if(!vm.projectId) { var seen={}; vm.projects=[{id:'',name:'All projects'}]; tasks.forEach(function(t){if(!seen[t.projectId]){seen[t.projectId]=true;vm.projects.push({id:t.projectId,name:t.project});}}); }
                var completed=tasks.filter(function(t){return t.completed;});
                var dated=completed.filter(function(t){return t.due && t.completedDate;});
                var onTime=dated.filter(function(t){return t.completedDate<=t.due;}).length;
                vm.taskEfficiencyDetails = !completed.length ? 'No completed tasks in the selected period.' :
                    dated.length === completed.length ? onTime+' of '+completed.length+' completed tasks finished on or before their due date.' :
                    !dated.length ? completed.length+' completed tasks. On-time completion dates are unavailable.' :
                    onTime+' of '+completed.length+' completed tasks confirmed finished on or before their due date. '+(completed.length-dated.length)+' tasks have missing completion or due dates.';
                vm.completedPlanned=completed.reduce(function(sum,t){return sum+(Number(t.planned)||0);},0);
                vm.completedSpent=completed.reduce(function(sum,t){return sum+(Number(t.spent)||0);},0);
                vm.hourNumber=function(value){return (Math.round((Number(value)||0)*100)/100).toString();};
                vm.overdueCount=tasks.filter(vm.overdue).length;
                vm.attention=tasks.filter(function(t){return !t.completed && t.due && vm.dueDays(t)<=3;}).sort(function(a,b){return a.due.localeCompare(b.due);});
                vm.matrix=(vm.data.priority.items||[]).map(function(p){ var row=tasks.filter(function(t){return t.priority===p.name;}); return {name:p.name,color:p.color,total:row.length,cells:(vm.data.taskStatus.items||[]).map(function(st){return {count:row.filter(function(t){return t.status===st.name;}).length,color:st.color};})}; });
                vm.maxTag=Math.max.apply(Math,[1].concat((vm.data.tags||[]).map(function(t){return t.count;})));
                var issues=vm.data.issues||{}, resolved=(issues.items||[]).filter(function(t){return t.name==='Resolved';})[0];
                vm.resolvedIssues=resolved?resolved.count:0;vm.openIssues=Math.max(0,(issues.total||0)-vm.resolvedIssues);
            };
            vm.efficiencySparkline = function(field) {
                var weeks=vm.data.efficiencyTrend||[];
                var values=weeks.map(function(w){return w[field];}).filter(function(v){return v!==null && v!==undefined;});
                var low=Math.min.apply(Math,[0].concat(values)), high=Math.max.apply(Math,[100].concat(values));
                return weeks.map(function(w,i){var value=w[field];return value===null || value===undefined ? null :
                    {x:5+i*100/Math.max(1,weeks.length-1),y:30-(Number(value)-low)/Math.max(1,high-low)*24,week:w.week,value:value};}).filter(Boolean);
            };
            vm.efficiencyPoints = function(field){return vm.efficiencySparkline(field).map(function(p){return p.x+','+p.y;}).join(' ');};
            vm.efficiencyTrendTitle = function(field){return (vm.data.efficiencyTrend||[]).map(function(w){return w.week+': '+(w[field]===null?'No data':w[field]+'%');}).join('; ');};
            vm.hoursSummary = function () {
                var days = (vm.data.hours && vm.data.hours.days) || [];
                var today = toDateKey(globalToday());
                var spent = days.reduce(function(sum,d){return sum+(Number(d.spent)||0);},0);
                var planned = days.reduce(function(sum,d){return sum+(Number(d.assigned)||0);},0);
                var isWeek = vm.hoursPeriod==='week' || (vm.hoursPeriod==='all' && vm.selectedPreset==='This Week');
                var number = function(value){return String(Math.round(value*100)/100);};
                if (!isWeek) return number(spent)+' h spent of '+number(planned)+' h planned in the selected period';
                var plannedToDate = days.filter(function(d){return d.key && d.key<=today;})
                    .reduce(function(sum,d){return sum+(Number(d.assigned)||0);},0);
                return number(spent)+' h spent of '+number(plannedToDate)+' h planned to date - '+number(planned)+' h planned this week';
            };
            vm.hoursSum=function(days,field){return vm.hoursText((days||[]).reduce(function(sum,d){return sum+(Number(d[field])||0);},0));};
            vm.meterWidth=function(value){return Math.max(0,Math.min(100,Number(value)||0))+'%';};

            var today = globalToday();
            vm.selectPreset('Today');
        }]);
}());
