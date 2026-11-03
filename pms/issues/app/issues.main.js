(function () {
    'use strict';

    angular.module('pmsIssuesApp', [])
        .directive('issueEmployeeKeys', ['$parse', function($parse) {
            return {restrict:'A',link:function(scope,element,attrs){
                var action=$parse(attrs.issueEmployeeKeys);
                function handle(event){
                    var key=event.key || '',code=event.which || event.keyCode;
                    if(['ArrowDown','ArrowUp','Enter','Escape'].indexOf(key)<0 && [40,38,13,27].indexOf(code)<0)return;
                    event.preventDefault();event.stopPropagation();event.stopImmediatePropagation();
                    function run(){action(scope,{$event:event});}
                    if(scope.$root.$$phase)scope.$evalAsync(run);else scope.$apply(run);
                }
                element[0].addEventListener('keydown',handle,true);
                scope.$on('$destroy',function(){element[0].removeEventListener('keydown',handle,true);});
            }};
        }])
        .controller('IssuesController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
            var page = window.IssuesPage || {};
            var requestToken = 0;
            $scope.permissions = page.permissions || {};
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
                { Id: 'ALL', Name: 'All' }, { Id: 'TODAY', Name: 'Today' },
                { Id: 'THISWEEK', Name: 'This Week' }, { Id: 'THISMONTH', Name: 'This Month' },
                { Id: 'THISQUARTER', Name: 'This Quarter' }, { Id: 'THISYEAR', Name: 'This Year' },
                { Id: 'YESTERDAY', Name: 'Yesterday' }, { Id: 'PREVIOUSWEEK', Name: 'Previous Week' },
                { Id: 'PREVIOUSMONTH', Name: 'Previous Month' }, { Id: 'PREVIOUSQUARTER', Name: 'Previous Quarter' },
                { Id: 'PREVIOUSYEAR', Name: 'Previous Year' }, { Id: 'CUSTOM', Name: 'Custom Range' }
            ];
            $scope.issueStatusFilters = [{ Id: 'ALL', Name: 'All statuses' }];
            $scope.defaultIssueStatus = 'ALL';
            $scope.filter = { EntityType: 'ALL', DateFilter: 'ALL', FromDate: null, ToDate: null, IssueStatus: $scope.defaultIssueStatus };

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
                if ($scope.closeRowMenu) $scope.closeRowMenu();
                if ($scope.closeCellChoice) $scope.closeCellChoice();
                if ($scope.closePersonChoice) $scope.closePersonChoice();
                $scope.error = '';
                $http.post('default.aspx/GetIssuesGrid', { request: {
                    EntityType: $scope.filter.EntityType || 'ALL',
                    IssueStatus: $scope.filter.IssueStatus || 'ALL',
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
                $scope.filter = { EntityType: 'ALL', DateFilter: 'ALL', FromDate: null, ToDate: null, IssueStatus: $scope.defaultIssueStatus };
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
            $scope.statusOptions = []; $scope.priorityOptions = [];
            $scope.editor = null; $scope.saving = false; $scope.editError = '';
            $http.post('default.aspx/GetIssueEditOptions', {}).then(function(response) {
                var data = unwrap(response);
                if (!data.Success) { $scope.error = data.Message || 'Unable to load issue options.'; return; }
                $scope.statusOptions = data.Statuses || []; $scope.priorityOptions = data.Priorities || [];
                $scope.issueStatusFilters = [{Id:'ALL',Name:'All statuses'}].concat($scope.statusOptions.map(function(option){return {Id:String(option.Id),Name:option.Name};}));
                var pending=$scope.statusOptions.filter(function(option){return String(option.Name || '').trim().toLowerCase()==='pending';})[0];
                $scope.defaultIssueStatus=pending ? String(pending.Id) : 'ALL';
                $scope.filter.IssueStatus=$scope.defaultIssueStatus; $scope.currentPage=1; $scope.load();
            }, function() { $scope.error = 'Unable to load issue options.'; });
            $scope.people = function(row, kind) {
                var ids = kind === 'Team' ? row.TeamIds || [] : [kind === 'Owner' ? row.OwnerId : row.AssignToId];
                var fallback = kind === 'Team' ? row.TeamNames : kind === 'Owner' ? row.OwnerName : row.AssigneeName;
                return ids.filter(function(id){return id > 0;}).map(function(id, index) {
                    var employee = $scope.employeeOptions.filter(function(e){return e.Id === id;})[0];
                    return { Id:id, Name:employee ? employee.Name : (fallback ? (kind === 'Team' ? fallback.split(', ')[index] : fallback) || 'Employee ' + id : 'Employee ' + id) };
                });
            };
            $scope.initials = function(name) { return String(name || '?').split(/\s+/).filter(Boolean).slice(0,2).map(function(n){return n.charAt(0);}).join('').toUpperCase(); };
            $scope.avatarColor = function(id) { return '#3478df'; };
            $scope.statusStyle = function(row) { return { 'background-color': /^#[0-9a-f]{3}([0-9a-f]{3})?$/i.test(row.StatusColor || '') ? row.StatusColor : '#f1f3f5', color:'#000' }; };
            $scope.issueTooltip='';
            $scope.showIssueTooltip=function(text,event) {
                var rect=event.currentTarget.getBoundingClientRect();
                $scope.issueTooltip=text;
                $scope.issueTooltipStyle={left:Math.max(8,Math.min(window.innerWidth-288,rect.left))+'px',top:Math.max(8,rect.top-38)+'px'};
            };
            $scope.hideIssueTooltip=function(){$scope.issueTooltip='';};
            $scope.remainingTeamNames=function(row){return $scope.people(row,'Team').slice(3).map(function(p){return p.Name;}).join(', ');};
            $scope.selectedEditTeam=function(){return $scope.editEmployees().filter(function(e){return $scope.editor && $scope.editor.TeamIds.indexOf(e.Id)>=0;});};
            $scope.removeIssueMember=function(row,person,event) {
                event.stopPropagation(); $scope.hideIssueTooltip();
                if($scope.saving || $scope.loading || !$scope.permissions.Edit) return;
                var changed=angular.copy(row);
                changed.TeamIds=(changed.TeamIds || []).filter(function(id){return id!==person.Id;});
                persist(changed,'Team');
            };
            $scope.priorityStyle = function(row) {
                var option=$scope.priorityOptions.filter(function(p){return p.Id===row.PriorityId;})[0];
                var color=option && option.Color;
                if (!/^#[0-9a-f]{3}([0-9a-f]{3})?$/i.test(color || '')) {
                    var name=String(option ? option.Name : row.PriorityName || '').toLowerCase();
                    color=name==='low' ? '#00ed22' : name==='high' ? '#ffb4a2' : name==='medium' ? '#ffe39a' : '#eef3f7';
                }
                return {'background-color':color,color:'#000'};
            };
            $scope.personChoice=null;
            $scope.closePersonChoice=function(){$scope.personChoice=null;};
            $scope.personSelectedId=function(){return !$scope.personChoice ? 0 : ($scope.personChoice.Field==='Owner' ? $scope.personChoice.Row.OwnerId : $scope.personChoice.Row.AssignToId) || 0;};
            $scope.personChoiceOptions=function(){
                if(!$scope.personChoice)return [];
                var query=String($scope.personChoice.Search || '').trim().toLowerCase();
                return $scope.personChoice.Options.filter(function(e){return e.Id!==$scope.personSelectedId() && (!query || e.Name.toLowerCase().indexOf(query)>=0);});
            };
            $scope.personSearchChanged=function(){if($scope.personChoice)$scope.personChoice.Index=-1;};
            $scope.openPersonChoice=function(row,field,event){
                event.stopPropagation(); $scope.hideIssueTooltip();$scope.closeRowMenu();$scope.closeCellChoice();
                if($scope.personChoice && $scope.personChoice.Row.Id===row.Id && $scope.personChoice.Field===field){$scope.closePersonChoice();return;}
                var options=field==='AssignTo' ? $scope.assignOptions(row) : $scope.employeeOptions.slice();
                $scope.people(row,field).forEach(function(e){if(!options.some(function(o){return o.Id===e.Id;}))options.push(e);});
                options.unshift({Id:0,Name:field==='Owner' ? 'No owner' : 'Unassigned'});
                var rect=event.currentTarget.getBoundingClientRect(),height=Math.min(265,options.length*31+54);
                $scope.personChoiceStyle={left:Math.max(8,Math.min(window.innerWidth-276,rect.left))+'px',top:Math.max(8,rect.bottom+height>window.innerHeight ? rect.top-height : rect.bottom+3)+'px'};
                $scope.personChoice={Row:row,Field:field,Options:options,Search:'',Index:-1};
                $timeout(function(){var input=document.getElementById('issuePersonSearch');if(input)input.focus();},0);
            };
            $scope.selectPersonChoice=function(person){
                var choice=$scope.personChoice;if(!choice || !person || $scope.saving)return;
                $scope.closePersonChoice();
                if(choice.Field==='Owner'){choice.Row.OwnerId=person.Id;choice.Row.OwnerName=person.Id ? person.Name : '';}
                else {choice.Row.AssignToId=person.Id;choice.Row.AssigneeName=person.Id ? person.Name : '';}
                persist(choice.Row,choice.Field);
            };
            $scope.personChoiceKeydown=function(event){
                if(!$scope.personChoice)return;
                var key=event.key==='ArrowDown' ? 40 : event.key==='ArrowUp' ? 38 : event.key==='Enter' ? 13 : event.key==='Escape' ? 27 : (event.which || event.keyCode),options=$scope.personChoiceOptions();
                if(key===40 || key===38){event.preventDefault();event.stopPropagation();if(!options.length)return;
                    $scope.personChoice.Index=$scope.personChoice.Index<0 ? (key===40 ? 0 : options.length-1) : Math.max(0,Math.min(options.length-1,$scope.personChoice.Index+(key===40 ? 1 : -1)));
                    $timeout(function(){var item=document.getElementById('issue-person-option-'+$scope.personChoice.Index);var list=document.getElementById('issuePersonOptions');if(item && list){if(item.offsetTop<list.scrollTop)list.scrollTop=item.offsetTop;else if(item.offsetTop+item.offsetHeight>list.scrollTop+list.clientHeight)list.scrollTop=item.offsetTop+item.offsetHeight-list.clientHeight;}},0);
                }else if(key===13){event.preventDefault();event.stopPropagation();var index=$scope.personChoice.Index;if(index<0 && options.length===1)index=0;if(index>=0)$scope.selectPersonChoice(options[index]);}
                else if(key===27){event.preventDefault();event.stopPropagation();$scope.closePersonChoice();}
            };
            $scope.cellChoice=null;
            $scope.closeCellChoice=function(){$scope.cellChoice=null;};
            $scope.choiceColor=function(option,field){return field==='Priority' ? $scope.priorityStyle({PriorityId:option.Id,PriorityName:option.Name})['background-color'] : (/^#[0-9a-f]{3}([0-9a-f]{3})?$/i.test(option.Color || '') ? option.Color : '#d7dce3');};
            $scope.openCellChoice=function(row,field,event) {
                event.stopPropagation(); $scope.hideIssueTooltip(); $scope.closePersonChoice(); $scope.closeRowMenu();
                if($scope.cellChoice && $scope.cellChoice.Row.Id===row.Id && $scope.cellChoice.Field===field){$scope.closeCellChoice();return;}
                var options=field==='Status' ? $scope.statusOptions : $scope.priorityOptions;
                var rect=event.currentTarget.getBoundingClientRect(), height=Math.min(210,options.length*34+10);
                $scope.cellChoiceStyle={left:Math.max(8,Math.min(window.innerWidth-173,rect.left))+'px',top:Math.max(8,rect.bottom+height>window.innerHeight ? rect.top-height : rect.bottom)+'px'};
                $scope.cellChoice={Row:row,Field:field,Options:options}; $scope.choiceIndex=0;
                $timeout(function(){var menu=document.querySelector('.issue-choice-menu');if(menu)menu.focus();},0);
            };
            $scope.selectCellChoice=function(option) {
                var choice=$scope.cellChoice; if(!choice)return;
                $scope.closeCellChoice();
                if(choice.Field==='Status'){choice.Row.StatusId=option.Id;choice.Row.StatusName=option.Name;choice.Row.StatusColor=option.Color;}
                else {choice.Row.PriorityId=option.Id;choice.Row.PriorityName=option.Name;}
                persist(choice.Row,choice.Field);
            };
            $scope.choiceKeydown=function(event) {
                if(! $scope.cellChoice)return;
                var key=event.which || event.keyCode;
                if(key===40 || key===38){event.preventDefault();$scope.choiceIndex=Math.max(0,Math.min($scope.cellChoice.Options.length-1,$scope.choiceIndex+(key===40 ? 1 : -1)));}
                else if(key===13){event.preventDefault();$scope.selectCellChoice($scope.cellChoice.Options[$scope.choiceIndex]);}
                else if(key===27){event.preventDefault();$scope.closeCellChoice();}
            };
            $scope.rowMenu=null;
            $scope.closeRowMenu=function(){$scope.rowMenu=null;};
            $scope.toggleRowMenu=function(row,event) {
                event.stopPropagation(); $scope.closeCellChoice(); $scope.closePersonChoice();
                if($scope.rowMenu && $scope.rowMenu.Id===row.Id){$scope.closeRowMenu();return;}
                var box=event.currentTarget.getBoundingClientRect();
                var height=($scope.permissions.Edit && $scope.permissions.Delete) ? 90 : 50;
                $scope.rowMenuStyle={left:Math.max(8,Math.min(window.innerWidth-178,box.right-170))+'px',top:Math.max(8,box.bottom+height>window.innerHeight ? box.top-height : box.bottom+3)+'px'};
                $scope.rowMenu=row;
            };
            $scope.editFromMenu=function(){var row=$scope.rowMenu;$scope.closeRowMenu();$scope.openEditor(row,'Edit');};
            $scope.deleteFromMenu=function(){var row=$scope.rowMenu;$scope.closeRowMenu();$scope.deleteIssue(row);};
            function dismissMenu(event) {
                if(event.type==='scroll' && event.target.closest && event.target.closest('.issue-choice-menu,.issue-person-menu')) return;
                if(event.type==='scroll' || event.type==='resize') $scope.$evalAsync($scope.hideIssueTooltip);
                if((!$scope.rowMenu && !$scope.cellChoice && !$scope.personChoice) || (event.type==='keydown' && event.keyCode!==27)) return;
                if(event.type==='click' && event.target.closest && event.target.closest('.issue-row-menu,.issue-dots,.issue-choice-menu,.issue-cell-choice,.issue-person-menu,.issue-person-trigger')) return;
                $scope.$evalAsync(function(){$scope.closeRowMenu();$scope.closeCellChoice();$scope.closePersonChoice();});
            }
            document.addEventListener('click',dismissMenu);
            document.addEventListener('keydown',dismissMenu);
            window.addEventListener('resize',dismissMenu);
            window.addEventListener('scroll',dismissMenu,true);
            $scope.$on('$destroy',function(){document.removeEventListener('click',dismissMenu);document.removeEventListener('keydown',dismissMenu);window.removeEventListener('resize',dismissMenu);window.removeEventListener('scroll',dismissMenu,true);});
            $scope.openEditor = function(row, mode) {
                if ($scope.saving) return;
                $scope.closeRowMenu(); $scope.closeCellChoice(); $scope.closePersonChoice(); $scope.hideIssueTooltip();
                $scope.editor = angular.copy(row); $scope.editor.Field = mode || 'Edit';
                $scope.editor.TeamIds = ($scope.editor.TeamIds || []).slice();
                $scope.editor.TeamIndex=-1; $scope.editor.TeamSearch='';
                $scope.editError = ''; $scope.editSearch = '';
            };
            $scope.closeEditor = function() { if (!$scope.saving) $scope.editor = null; };
            $scope.editEmployees = function() {
                if (!$scope.editor) return [];
                var all = $scope.employeeOptions.slice();
                ['Owner','Team','AssignTo'].forEach(function(kind) {
                    $scope.people($scope.editor,kind).forEach(function(e){if (!all.some(function(a){return a.Id===e.Id;})) all.push(e);});
                });
                return all;
            };
            $scope.filteredEditEmployees = function() { var q=String(($scope.editor && $scope.editor.TeamSearch) || '').trim().toLowerCase(); if(!q)return []; return $scope.editEmployees().filter(function(e){return $scope.editor.TeamIds.indexOf(e.Id)<0 && (!q || e.Name.toLowerCase().indexOf(q)>=0);}); };
            $scope.assignOptions = function(row) {
                return ($scope.employeeOptions || []).filter(function(e){return (row.TeamIds || []).indexOf(e.Id)>=0;}).concat(
                    $scope.people(row,'Team').filter(function(e){return !$scope.employeeOptions.some(function(a){return a.Id===e.Id;});}));
            };
            $scope.teamChoiceKeydown=function(event){
                if(!$scope.editor || $scope.saving)return;
                var options=$scope.filteredEditEmployees(),key=event.key==='ArrowDown' ? 40 : event.key==='ArrowUp' ? 38 : event.key==='Enter' ? 13 : (event.which || event.keyCode);
                if(key===40 || key===38){event.preventDefault();event.stopPropagation();if(!options.length)return;
                    $scope.editor.TeamIndex=$scope.editor.TeamIndex<0 ? (key===40 ? 0 : options.length-1) : Math.max(0,Math.min(options.length-1,$scope.editor.TeamIndex+(key===40 ? 1 : -1)));
                    $timeout(function(){if(!$scope.editor)return;var item=document.getElementById('issue-team-option-'+$scope.editor.TeamIndex),list=document.getElementById('issueTeamOptions');if(item && list){if(item.offsetTop<list.scrollTop)list.scrollTop=item.offsetTop;else if(item.offsetTop+item.offsetHeight>list.scrollTop+list.clientHeight)list.scrollTop=item.offsetTop+item.offsetHeight-list.clientHeight;}},0);
                }else if(key===13){event.preventDefault();event.stopPropagation();var index=$scope.editor.TeamIndex;if(index<0 && options.length===1)index=0;if(index>=0 && options[index])$scope.toggleIssueTeam(options[index].Id);}
            };
            $scope.toggleIssueTeam = function(id) {
                if(!$scope.editor || $scope.saving)return;
                var editing=$scope.editor,ids=editing.TeamIds,index=ids.indexOf(id),previous=ids.slice();
                if(index<0) ids.push(id); else ids.splice(index,1);
                editing.TeamIndex=-1;
                if(editing.Field==='Team')persist(editing,'Team',null,function(){if($scope.editor===editing)editing.TeamIds=previous;});
            };
            function persist(row, field, done, failed) {
                if ($scope.saving) return;
                $scope.saving = true; $scope.editError = ''; $scope.error = '';
                var request = {Id:row.Id, Field:field, IssueName:row.IssueName, Description:row.Description,
                    StatusId:row.StatusId, PriorityId:row.PriorityId, OwnerId:row.OwnerId || 0,
                    AssignToId:row.AssignToId || 0, TeamIds:row.TeamIds || []};
                $http.post('default.aspx/SaveIssue', {request:request}).then(function(response) {
                    var data=unwrap(response);
                    if (!data.Success) { if(failed)failed(); if($scope.editor) $scope.editError=data.Message; else {$scope.load(); $scope.error=data.Message;} return; }
                    if(done) done(); $scope.load();
                }, function(){ if(failed)failed(); if($scope.editor) $scope.editError='Unable to save the issue.'; else {$scope.load(); $scope.error='Unable to save the issue.';} })
                .finally(function(){$scope.saving=false;});
            }
            $scope.saveEditor = function(){persist($scope.editor,$scope.editor.Field,function(){$scope.editor=null;});};
            $scope.inlineSave = function(row,field){persist(row,field);};
            $scope.deleteIssue = function(row) {
                if(window.confirm('Delete "' + row.IssueName + '"?')) persist(row,'Delete');
            };
            $scope.$watch('searchText', rebuildSearch);
            $scope.loadEmployeeOptions();
            $scope.load();
        }]);
}());
