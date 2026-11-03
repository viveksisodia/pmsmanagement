(function () {
    'use strict';
    angular.module('pmsManagerDashboardApp', [])
        .controller('ManagerDashboardController', ['$scope', '$timeout', '$element', '$document', 'DashboardService',
        function ($scope, $timeout, $element, $document, DashboardService) {
            var charts = [], tooltips = [], chartMap = {}, requestVersion = 0, destroyed = false, reloadTimer = null;
            var teamTooltip=null;
            $scope.hideTeamTooltip=function(){if(teamTooltip){teamTooltip.remove();teamTooltip=null;}};
            $scope.showTeamTooltip=function(event,text){
                $scope.hideTeamTooltip();if(!text)return;
                var doc=$document[0],anchor=event.currentTarget.getBoundingClientRect();
                teamTooltip=doc.createElement('div');teamTooltip.className='md-dashboard-team-tooltip';teamTooltip.setAttribute('role','tooltip');teamTooltip.textContent=text;teamTooltip.style.position='fixed';teamTooltip.style.left='0px';teamTooltip.style.top='0px';teamTooltip.style.visibility='hidden';teamTooltip.style.maxWidth=Math.max(80,Math.min(260,doc.documentElement.clientWidth-32))+'px';doc.body.appendChild(teamTooltip);
                var rect=teamTooltip.getBoundingClientRect(),width=doc.documentElement.clientWidth,height=doc.documentElement.clientHeight;
                var left=Math.max(8,Math.min(anchor.left+anchor.width/2-rect.width/2,width-rect.width-8));
                var top=anchor.top-rect.height-8;if(top<8)top=anchor.bottom+8;
                teamTooltip.style.left=left+'px';teamTooltip.style.top=Math.max(8,Math.min(top,height-rect.height-8))+'px';teamTooltip.style.visibility='visible';
            };
            function hideTeamOnScroll(){$scope.hideTeamTooltip();}
            if(window.addEventListener){window.addEventListener('scroll',hideTeamOnScroll,true);window.addEventListener('resize',hideTeamOnScroll);}
            $scope.projectState={id:0, group:"Status", situationId:0};
            $scope.itemState={Milestone:"Status",Task:"Status",Subtask:"Status",taskProjectGroup:"Priority"};
            $scope.itemLegends={};
            $scope.paging={milestone:{page:1,size:5,total:0,pages:1},situation:{page:1,size:5,total:0,pages:1}};
            $scope.pageSizes=[5,10,20];
            $scope.listSearch={milestone:'',situation:''};
            $scope.refreshPage=function(kind,reset){
                $scope.hideTeamTooltip();
                var pager=$scope.paging[kind],rows=kind==='milestone'?$scope.projects:$scope.criticalProjects;
                var query=($scope.listSearch[kind]||'').trim().toLowerCase();
                if(query)rows=rows.filter(function(p){
                    var text=[p.ProjectName,p.OwnerName,p.TeamNames,p.Deadline,p.SituationName];
                    if(kind==='milestone')(p.milestones||[]).forEach(function(m){text.push(m.Name);});
                    return text.join(' ').toLowerCase().indexOf(query)>=0;
                });
                if(reset)pager.page=1;
                pager.size=Number(pager.size)||5;pager.total=rows.length;pager.pages=Math.max(1,Math.ceil(pager.total/pager.size));
                pager.page=Math.max(1,Math.min(pager.page,pager.pages));
                pager.from=pager.total?(pager.page-1)*pager.size+1:0;pager.to=Math.min(pager.page*pager.size,pager.total);
                $scope[kind+'PageRows']=rows.slice((pager.page-1)*pager.size,pager.page*pager.size);
            };
            $scope.goPage=function(kind,page){$scope.paging[kind].page=page;$scope.refreshPage(kind,false);};
            $scope.dateState={from:null,to:null};
            $scope.projects=[]; $scope.projectOptions=[]; $scope.projectLegend=[]; $scope.criticalProjects=[];
            $scope.employees = [];
            $scope.selectedEmployee = {id:0,name:'All Employees'};
            $scope.employeeState = { search: '', index: 0 };
            $scope.employeeLoading = true;
            $scope.projectDropdowns={main:{open:false,search:'',index:0,selected:{id:0,ProjectName:'All Projects'},options:[]},issue:{open:false,search:'',index:0,selected:{id:0,ProjectName:'All Projects'},options:[]}};
            $scope.filterProjectOptions=function(kind){var state=$scope.projectDropdowns[kind],query=(state.search||'').trim().toLowerCase();state.options=[{id:0,ProjectName:'All Projects'}].concat($scope.projectOptions||[]).filter(function(p){return String(p.ProjectName||'').toLowerCase().indexOf(query)>=0;});state.index=0;};
            $scope.toggleProjectDropdown=function(kind){var state=$scope.projectDropdowns[kind];state.open=!state.open;$scope.employeeOpen=false;$scope.projectDropdowns[kind==='main'?'issue':'main'].open=false;if(state.open){state.search='';$scope.filterProjectOptions(kind);$timeout(function(){var input=$element[0].querySelector('#mdProjectSearch'+kind);if(input)input.focus({preventScroll:true});},0);}};
            $scope.selectProjectOption=function(kind,project){var state=$scope.projectDropdowns[kind];state.selected=project;state.open=false;if(kind==='main'){$scope.projectState.id=project.id;reloadProjects();}else{$scope.bindIssues();}};
            $scope.projectKeydown=function(kind,event){var state=$scope.projectDropdowns[kind],options=state.options;if(event.key==='ArrowDown'||event.key==='ArrowUp'){event.preventDefault();if(options.length)state.index=(state.index+(event.key==='ArrowDown'?1:-1)+options.length)%options.length;$timeout(function(){var active=$element[0].querySelector('#mdProjects'+kind+' .md-employee-option.active');if(active)active.scrollIntoView({block:'nearest'});},0);}else if(event.key==='Enter'){event.preventDefault();if(options[state.index])$scope.selectProjectOption(kind,options[state.index]);}else if(event.key==='Escape'){event.preventDefault();state.open=false;}};
            $scope.bindIssues=function(){var selected=$scope.projectDropdowns.issue.selected.id,total=0,complete=0;($scope.issueProjects||[]).forEach(function(p){if(!selected||p.ProjectId==selected){total+=p.Total;complete+=p.Completed;}});$scope.issueLegend=[{name:'Open',count:total-complete,color:'#f7c948'},{name:'Resolved',count:complete,color:'#4ade80'}];var chart=chartMap.issuesChart;if(chart){chart.data.labels=$scope.issueLegend.map(function(g){return g.name;});chart.data.datasets[0].data=$scope.issueLegend.map(function(g){return g.count;});chart.data.datasets[0].backgroundColor=$scope.issueLegend.map(function(g){return g.color;});chart.options.plugins.centerText.number=total;chart.update();}};
            $scope.filterEmployees = function(value) {
                if (typeof value === 'string') $scope.employeeState.search=value;
                var query = ($scope.employeeState.search || '').trim().toLowerCase();
                $scope.employeeOptions = [{id:0,name:'All Employees'}].concat($scope.employees).filter(function(e) { return String(e.name || '').toLowerCase().indexOf(query) >= 0; });
                $scope.employeeState.index = 0;
            };
            $scope.toggleEmployees = function() {
                $scope.employeeOpen = !$scope.employeeOpen;
                if ($scope.employeeOpen) {
                    $scope.employeeState.search = ''; $scope.filterEmployees();
                    $timeout(function() { var input=$element[0].querySelector('#mdEmployeeSearch'); if(input) input.focus(); },0);
                }
            };
            $scope.selectEmployee = function(employee) {
                $scope.selectedEmployee = employee;
                $scope.employeeOpen = false;
                $scope.projectState.id=0; if($scope.projectDropdowns)$scope.projectDropdowns.main.selected={id:0,ProjectName:'All Projects'}; reloadProjects();
            };
            $scope.employeeKeydown = function(event) {
                var options=$scope.employeeOptions || [];
                if(event.key === 'ArrowDown' || event.key === 'ArrowUp') {
                    event.preventDefault();
                    if(options.length) $scope.employeeState.index = ($scope.employeeState.index + (event.key === 'ArrowDown' ? 1 : -1) + options.length) % options.length;
                    $timeout(function() { var active=$element[0].querySelector('.md-employee-option.active'); if(active) active.scrollIntoView({block:'nearest'}); },0);
                } else if(event.key === 'Enter') {
                    event.preventDefault(); if(options[$scope.employeeState.index]) $scope.selectEmployee(options[$scope.employeeState.index]);
                } else if(event.key === 'Escape') { event.preventDefault(); $scope.employeeOpen=false; }
            };
            DashboardService.getEmployees().then(function(items) {
                $scope.employees=items; $scope.filterEmployees();
            },function() { $scope.employeeError='Unable to load employees. Please reload the page.'; }).finally(function() { $scope.employeeLoading=false; });
            $scope.datePresets = ['All','Today','This Week','This Month','This Quarter','This Year','Yesterday','Previous Week','Previous Month','Previous Quarter','Previous Year','Custom'];
            $scope.datePreset = 'Today';
            $scope.rangeMenuOpen = false;
            $scope.customMode = false;
            function displayDate(value) { return value ? value.split('-').reverse().join('/') : ''; }
            function commitRange(range) {
                $scope.dateFilter = range;
                $scope.rangeDisplay = range.preset === 'All' ? 'All' : displayDate(range.startDate) + ' - ' + displayDate(range.endDate);
                reloadProjects();
            }
            $scope.selectPreset = function(preset) {
                $scope.datePreset = preset;
                $scope.customMode = preset === 'Custom';
                $scope.dateError = '';
                if ($scope.customMode) {
                    if (!$scope.dateState.from) $scope.dateState.from = new Date();
                    if (!$scope.dateState.to) $scope.dateState.to = new Date();
                    return;
                }
                commitRange(DashboardService.dateRange(preset));
                $scope.rangeMenuOpen = false;
            };
            $scope.openCalendar = function(event) {
                var input = event.currentTarget;
                input.focus();
                if (typeof input.showPicker === 'function') {
                    try { input.showPicker(); } catch (e) { /* Keep native date input available. */ }
                }
            };
            $scope.applyCustomRange = function() {
                try {
                    commitRange(DashboardService.dateRange('Custom', $scope.dateState.from, $scope.dateState.to));
                    $scope.dateError = '';
                    $scope.rangeMenuOpen = false;
                } catch(e) { $scope.dateError = e.message; }
            };
            $scope.reportUrl=function(type,group){
                var range=$scope.dateFilter||{},project=Number($scope.projectState.id)||0;
                if(type==='issues')project=Number($scope.projectDropdowns.issue.selected.id)||project;
                var f={reportType:type,groupBy:group||'Status',employeeId:$scope.selectedEmployee.id,projectId:project,startDate:range.startDate||'',endDate:range.endDate||'',preset:$scope.datePreset};
                if(type==='situation')f.situationId=Number($scope.projectState.situationId)||0;
                return '../dashboardreport/?'+Object.keys(f).map(function(k){return encodeURIComponent(k)+'='+encodeURIComponent(f[k]);}).join('&');
            };
            $scope.reloadProjects = reloadProjects;
            $scope.bindProjectGroups = function() {
                var kind=$scope.projectState.group, field={Status:'StatusId',Priority:'PriorityId',Situation:'SituationId'}[kind];
                var groups=($scope.masters || []).filter(function(m){return m.Kind===kind;}).map(function(m){return {id:m.id,name:m.Name,color:m.Color || '#607d8b',count:0};});
                var byId={}; groups.forEach(function(g){byId[g.id]=g;});
                $scope.projects.forEach(function(p){var g=byId[p[field]]; if(!g){g={id:p[field],name:'Unspecified',color:'#9ca3af',count:0};byId[p[field]]=g;groups.push(g);}g.count++;});
                groups.forEach(function(g){g.percentage=$scope.projects.length ? Math.round(g.count*1000/$scope.projects.length)/10 : 0;});
                $scope.projectLegend=groups;
                var chart=chartMap.projectChart;
                if(chart){chart.data.labels=groups.map(function(g){return g.name;});chart.data.datasets[0].data=groups.map(function(g){return g.count;});chart.data.datasets[0].backgroundColor=groups.map(function(g){return g.color;});chart.options.plugins.centerText.number=$scope.projects.length;chart.update();}
            };
            $scope.bindItemCharts=function(){
                ['Milestone','Task','Subtask'].forEach(function(kind){
                    var group=$scope.itemState[kind],field={Status:'StatusId',Priority:'PriorityId',Situation:'SituationId'}[group];
                    var masters=($scope.masters||[]).filter(function(m){return m.Kind===group;}), groups=masters.map(function(m){return {id:m.id,name:m.Name,color:m.Color||'#607d8b',count:0};}),byId={},total=0;
                    groups.forEach(function(g){byId[g.id]=g;});
                    ($scope.itemGroups||[]).filter(function(i){return i.Kind===kind;}).forEach(function(i){var g=byId[i[field]];if(!g){g={id:i[field],name:'Unspecified',color:'#9ca3af',count:0};byId[i[field]]=g;groups.push(g);}g.count+=i.Count;total+=i.Count;});
                    groups.forEach(function(g){g.percentage=total?Math.round(g.count*1000/total)/10:0;});$scope.itemLegends[kind]=groups;
                    var chart=chartMap[kind.toLowerCase()+'Chart'];if(chart){chart.data.labels=groups.map(function(g){return g.name;});chart.data.datasets[0].data=groups.map(function(g){return g.count;});chart.data.datasets[0].backgroundColor=groups.map(function(g){return g.color;});chart.options.plugins.centerText.number=total;chart.update();}
                });
                var kind=$scope.itemState.taskProjectGroup,field=kind==='Priority'?'PriorityId':'StatusId',projects=[],ids={},groups=($scope.masters||[]).filter(function(m){return m.Kind===kind;}).map(function(m){return {id:m.id,name:m.Name,color:m.Color||'#607d8b'};});
                ($scope.taskProjects||[]).forEach(function(i){if(ids[i.ProjectId]===undefined){ids[i.ProjectId]=projects.length;projects.push(i);}if(!groups.some(function(g){return g.id==i[field];}))groups.push({id:i[field],name:'Unspecified',color:'#9ca3af'});});
                $scope.taskChartWidth=Math.max(400,projects.length*110);var chart=chartMap.priorityChart;if(chart){chart.data.labels=projects.map(function(p){return p.ProjectName;});chart.data.datasets=groups.map(function(g){var values=projects.map(function(){return 0;});($scope.taskProjects||[]).forEach(function(i){if(i[field]==g.id)values[ids[i.ProjectId]]+=i.Count;});return {label:g.name,data:values,backgroundColor:g.color,barThickness:20};});chart.update();}
            };
            $scope.bindCriticalProjects=function(){
                var masters=($scope.masters || []).filter(function(m){return m.Kind==='Situation';});
                $scope.situations=masters;$scope.situationOptions=[{id:0,Name:'All Situations'}].concat(masters);
                $scope.criticalProjects=$scope.projects.filter(function(p){return !$scope.projectState.situationId || p.SituationId==$scope.projectState.situationId;}).map(function(p){
                    var m=masters.filter(function(m){return m.id==p.SituationId;})[0];p.SituationName=m?m.Name:'Unspecified';p.SituationColor=m?m.Color:'#607d8b';
                    var colors=['#f56550','#0788c9','#8b5cf6'];
                    p.teamAvatars=String(p.TeamNames||'').split(',').map(function(name){return name.trim();}).filter(function(name){return !!name;}).map(function(name,index){
                        var parts=name.split(/\s+/);return {name:name,initials:(parts[0].charAt(0)+(parts.length>1?parts[parts.length-1].charAt(0):'')).toUpperCase(),color:colors[index%colors.length]};
                    });
                    p.moreTeamNames=p.teamAvatars.slice(3).map(function(member){return member.name;}).join(', ');
                    return p;
                });
                $scope.refreshPage('situation',true);
            };
            function reloadProjects(){
                if(!$scope.dateFilter || destroyed) return;
                requestVersion++;
                if(reloadTimer)$timeout.cancel(reloadTimer);
                $scope.hideTeamTooltip();
                $scope.projectLoading=true;$scope.projectError='';
                reloadTimer=$timeout(function(){reloadTimer=null;loadProjects();},180);
            }
            function loadProjects(){
                if(destroyed)return;
                $scope.paging.milestone.page=1;$scope.paging.situation.page=1;
                var version=++requestVersion;
                $scope.projectLoading=true;$scope.projectError='';
                DashboardService.getProjects({employeeId:$scope.selectedEmployee.id,projectId:Number($scope.projectState.id)||0,startDate:$scope.dateFilter.startDate,endDate:$scope.dateFilter.endDate}).then(function(data){
                    if(destroyed || version!==requestVersion)return;
                    $scope.projects=data.projects;$scope.masters=data.masters;$scope.projectOptions=data.projectOptions;$scope.itemGroups=data.itemGroups;$scope.taskProjects=data.taskProjects;$scope.issueProjects=data.issueProjects;$scope.tags=data.tags||[];
                    ['main','issue'].forEach(function(kind){$scope.filterProjectOptions(kind);});
                    $scope.bindIssues();
                    var milestonesByProject={};
                    data.milestones.forEach(function(m){(milestonesByProject[m.ProjectId]||(milestonesByProject[m.ProjectId]=[])).push(m);});
                    $scope.projects.forEach(function(p){p.milestones=(milestonesByProject[p.id]||[]).map(function(m,index){
                        m.Label=m.Name||'Unnamed milestone';m.Completion=Math.max(0,Math.min(100,Number(m.Completion)||0));
                        m.TotalTasks=Number(m.TotalTasks)||0;m.CompletedTasks=Number(m.CompletedTasks)||0;
                        m.Color=m.Completion>=100?'#85ff8d':m.Completion>0?'#ffde73':'#b8c2cc';
                        m.Tooltip='Milestone: '+m.Label+'\nTasks: '+m.TotalTasks+'/'+m.CompletedTasks;return m;
                    });});
                    $scope.bindProjectGroups();$scope.bindCriticalProjects();$scope.bindItemCharts();$scope.refreshPage('milestone',true);
                    var buckets=[0,0,0,0];data.projects.forEach(function(p){var v=Number(p.Completion)||0;buckets[v<=25?0:v<=50?1:v<=75?2:3]++;});
                    if(chartMap.barChart){chartMap.barChart.data.datasets[0].data=buckets;chartMap.barChart.update();}
                },function(error){if(destroyed || version!==requestVersion)return;
                    $scope.tags=[];$scope.issueProjects=[];$scope.bindIssues();$scope.projects=[];$scope.criticalProjects=[];$scope.itemGroups=[];$scope.taskProjects=[];$scope.bindProjectGroups();$scope.bindItemCharts();$scope.refreshPage('milestone',true);$scope.refreshPage('situation',true);
                    if(chartMap.barChart){chartMap.barChart.data.datasets[0].data=[0,0,0,0];chartMap.barChart.update();}
                    $scope.projectError=(error.data && error.data.Message)||'Unable to load projects.';
                }).finally(function(){if(!destroyed && version===requestVersion)$scope.projectLoading=false;});
            }
            function outsideClick() { $scope.$evalAsync(function() { $scope.rangeMenuOpen = false; $scope.employeeOpen = false;$scope.projectDropdowns.main.open=false;$scope.projectDropdowns.issue.open=false; }); }
            function escapeKey(event) { if(event.key === 'Escape') outsideClick(); }
            $document.on('click', outsideClick);
            $document.on('keydown', escapeKey);
            commitRange(DashboardService.dateRange('Today'));
        const centerTextPlugin = {
            id: 'centerText',
            beforeDraw(chart) {
                const { width, height, ctx } = chart;
                const text = chart.config.options.plugins.centerText;
                if (!text) return;
                ctx.save();
                ctx.font = 'bold 22px Arial';
                ctx.fillStyle = '#000';
                ctx.textAlign = 'center';
                ctx.textBaseline = 'middle';
                ctx.fillText(text.number, width / 2, height / 2 - 6);
                ctx.font = '12px Arial';
                ctx.fillStyle = '#000';
                ctx.fillText(text.label, width / 2, height / 2 + 14);
                ctx.restore();
            }
        };

            var pending = $timeout(function () {
                if (window.Chart) {
                    DashboardService.getChartDefinitions().forEach(function (definition) {
                        var canvas = $element[0].querySelector('#' + definition.id);
                        if (canvas) {
                            definition.config.plugins = [centerTextPlugin];
                            var chart=new window.Chart(canvas, definition.config);
                            charts.push(chart); chartMap[definition.id]=chart;
                        }
                    });
                }
                $scope.bindProjectGroups(); $scope.bindItemCharts();$scope.bindIssues();
                if(chartMap.barChart){var buckets=[0,0,0,0];$scope.projects.forEach(function(p){var v=Number(p.Completion)||0;buckets[v<=25?0:v<=50?1:v<=75?2:3]++;});chartMap.barChart.data.datasets[0].data=buckets;chartMap.barChart.update();}
                if(!window.Chart)$scope.chartError='Chart library did not load. Project data can still be filtered.';
                if (window.bootstrap && window.bootstrap.Tooltip) {
                    Array.prototype.forEach.call($element[0].querySelectorAll('[data-bs-toggle="tooltip"]'), function (el) {
                        tooltips.push(window.bootstrap.Tooltip.getOrCreateInstance(el));
                    });
                }
            }, 0);
            $scope.$on('$destroy', function () {
                destroyed=true; requestVersion++;if(reloadTimer)$timeout.cancel(reloadTimer);$scope.hideTeamTooltip();
                if(window.removeEventListener){window.removeEventListener('scroll',hideTeamOnScroll,true);window.removeEventListener('resize',hideTeamOnScroll);}
                $timeout.cancel(pending);
                $document.off('click', outsideClick);
                $document.off('keydown', escapeKey);
                charts.forEach(function (chart) { chart.destroy(); });
                tooltips.forEach(function (tooltip) { tooltip.dispose(); });
            });
        }]);
}());
