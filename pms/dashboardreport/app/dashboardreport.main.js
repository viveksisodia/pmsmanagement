(function(){'use strict';angular.module('pmsDashboardReportApp').directive('drTeamTooltip', ['$timeout', function ($timeout) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var timer, instance;
                    function hideTooltip() {
                        if (instance) instance.hide();
                    }
                    element.on('mouseleave', hideTooltip);
                    var stop = attrs.$observe('drTeamTooltip', function (title) {
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
        }]).controller('ReportController',['$scope','$timeout','ReportService',function($scope,$timeout,svc){
var vm=this,q=new URLSearchParams(window.location.search),version=0,detailVersion=0,timer=null,searchTimer=null;
vm.types=[{id:'projects',name:'Projects'},{id:'milestones',name:'Milestones'},{id:'tasks',name:'Tasks'},{id:'subtasks',name:'Subtasks'},{id:'completion',name:'Project Completion'},{id:'milestoneprogress',name:'Project with Milestone'},{id:'situation',name:'Project Situation'},{id:'taskprojects',name:'Project Status / Priority'},{id:'issues',name:'Project Issues'},{id:'tags',name:'Tags'}];
function num(k){var n=Number(q.get(k));return Number.isInteger(n)&&n>=0?n:0;}
vm.f={employeeId:num('employeeId'),projectId:num('projectId'),startDate:q.get('startDate')||null,endDate:q.get('endDate')||null,reportType:q.get('reportType')||'projects',groupBy:q.get('groupBy')||'Status',statusId:num('statusId'),priorityId:num('priorityId'),situationId:num('situationId'),tagId:0,issueState:'All',search:''};
if(!vm.types.some(function(t){return t.id===vm.f.reportType;}))vm.f.reportType='projects';
if(['Status','Priority','Situation','Project','Completion','Tag'].indexOf(vm.f.groupBy)<0)vm.f.groupBy='Status';
if(vm.f.reportType==='issues')vm.f.groupBy='Complete / Not Complete';
vm.presets=['All','Today','This Week','This Month','This Quarter','This Year','Yesterday','Previous Week','Previous Month','Previous Quarter','Previous Year','Custom'];
vm.preset=q.get('preset')||(vm.f.startDate?'Custom':'All');if(vm.presets.indexOf(vm.preset)<0)vm.preset='Custom';
function parseDate(s){if(!s)return null;var a=s.split('-').map(Number);return new Date(a[0],a[1]-1,a[2]);}
vm.from=parseDate(vm.f.startDate);vm.to=parseDate(vm.f.endDate);vm.groups=[];vm.rows=[];vm.page=1;vm.size=20;vm.total=0;vm.selected=null;vm.look={employees:[],projects:[],statuses:[],priorities:[],situations:[],tags:[]};
var initialFilters=angular.copy(vm.f),initialPreset=vm.preset,initialFrom=vm.from?new Date(vm.from.getTime()):null,initialTo=vm.to?new Date(vm.to.getTime()):null;
vm.projectOpen=false;vm.projectText='All Projects';vm.projectQuery='';vm.projectIndex=0;
vm.projectLabel=function(){var p=vm.look.projects.filter(function(p){return p.id===vm.f.projectId;})[0];return p?p.ProjectName:'All Projects';};
vm.projectMatches=function(){var term=vm.projectQuery.toLowerCase();return vm.look.projects.filter(function(p){return String(p.ProjectName||'').toLowerCase().indexOf(term)>=0;});};
vm.projectFocus=function(){vm.projectOpen=true;vm.projectQuery='';vm.projectText='';vm.projectIndex=0;};
vm.projectSearch=function(){vm.projectQuery=vm.projectText||'';vm.projectIndex=0;vm.projectOpen=true;};
vm.projectPick=function(p){vm.f.projectId=p.id;vm.projectText=p.ProjectName;vm.projectQuery='';vm.projectOpen=false;vm.changed();};
vm.projectBlur=function(){vm.projectOpen=false;vm.projectText=vm.projectLabel();vm.projectQuery='';};
vm.projectKey=function(e){var rows=vm.projectMatches();if(e.key==='ArrowDown'||e.key==='ArrowUp'){e.preventDefault();if(!vm.projectOpen){vm.projectOpen=true;vm.projectQuery='';rows=vm.projectMatches();vm.projectIndex=0;}else if(rows.length){vm.projectIndex=(vm.projectIndex+(e.key==='ArrowDown'?1:-1)+rows.length)%rows.length;}$timeout(function(){var el=document.getElementById('dr-project-option-'+vm.projectIndex);if(el)el.scrollIntoView({block:'nearest'});},0);}else if(e.key==='Enter'&&vm.projectOpen){e.preventDefault();if(rows[vm.projectIndex])vm.projectPick(rows[vm.projectIndex]);}else if(e.key==='Escape'){e.preventDefault();vm.projectBlur();}};
vm.employeeOpen=false;vm.employeeText='All Employees';vm.employeeQuery='';vm.employeeIndex=0;
vm.employeeLabel=function(){var p=vm.look.employees.filter(function(p){return p.id===vm.f.employeeId;})[0];return p?p.DisplayName:'All Employees';};
vm.employeeMatches=function(){var term=vm.employeeQuery.toLowerCase();return vm.look.employees.filter(function(p){return String(p.DisplayName||'').toLowerCase().indexOf(term)>=0;});};
vm.employeeFocus=function(){vm.employeeOpen=true;vm.employeeQuery='';vm.employeeText='';vm.employeeIndex=0;};
vm.employeeSearch=function(){vm.employeeQuery=vm.employeeText||'';vm.employeeIndex=0;vm.employeeOpen=true;};
vm.employeePick=function(p){vm.f.employeeId=p.id;vm.employeeText=p.DisplayName;vm.employeeQuery='';vm.employeeOpen=false;vm.changed();};
vm.employeeBlur=function(){vm.employeeOpen=false;vm.employeeText=vm.employeeLabel();vm.employeeQuery='';};
vm.employeeKey=function(e){var rows=vm.employeeMatches();if(e.key==='ArrowDown'||e.key==='ArrowUp'){e.preventDefault();if(!vm.employeeOpen){vm.employeeOpen=true;vm.employeeQuery='';rows=vm.employeeMatches();vm.employeeIndex=0;}else if(rows.length){vm.employeeIndex=(vm.employeeIndex+(e.key==='ArrowDown'?1:-1)+rows.length)%rows.length;}$timeout(function(){var el=document.getElementById('dr-employee-option-'+vm.employeeIndex);if(el)el.scrollIntoView({block:'nearest'});},0);}else if(e.key==='Enter'&&vm.employeeOpen){e.preventDefault();if(rows[vm.employeeIndex])vm.employeePick(rows[vm.employeeIndex]);}else if(e.key==='Escape'){e.preventDefault();vm.employeeBlur();}};
vm.groupPage=1;vm.groupSize=10;
vm.groupPages=function(){return Math.max(1,Math.ceil(vm.groups.length/vm.groupSize));};
vm.groupRows=function(){var start=(vm.groupPage-1)*vm.groupSize;return vm.groups.slice(start,start+vm.groupSize);};
vm.groupGo=function(page){vm.groupPage=Math.max(1,Math.min(page,vm.groupPages()));};
vm.teamMembers=function(r){if(!r._team){var members=r.TeamJson;try{if(typeof members==='string')members=JSON.parse(members||'[]');}catch(e){members=[];}r._team=Array.isArray(members)?members:[];}return r._team;};
vm.teamNames=function(r){return vm.teamMembers(r).map(function(e){return e.DisplayName;}).join(', ');};
vm.teamInitials=function(name){return String(name||'?').trim().split(/\s+/).slice(0,2).map(function(p){return p.charAt(0);}).join('').toUpperCase();};
vm.title=function(){var t=vm.types.filter(function(t){return t.id===vm.f.reportType;})[0];return (t?t.name:'Dashboard')+' Report';};
vm.groupOptions=function(){if(vm.f.reportType==='issues')return ['Complete / Not Complete'];if(vm.f.reportType==='tags')return ['Tag'];if(vm.f.reportType==='completion')return ['Completion'];if(vm.f.reportType==='milestoneprogress')return ['Project'];if(vm.f.reportType==='taskprojects')return ['Status','Priority'];return ['Status','Priority','Situation','Project'];};
vm.detailSearch='';vm.detailSearchText='';
function request(action){var f=angular.extend({},vm.f,{action:action,groupKey:vm.selected?vm.selected.GroupKey:null,page:vm.page,pageSize:vm.size});['employeeId','projectId','statusId','priorityId','situationId','tagId'].forEach(function(k){f[k]=Number(f[k])||0;});if(f.reportType==='issues'){f.situationId=0;f.tagId=0;}f.search=action==='groups'?'':(vm.detailSearch||'');return f;}
function err(e){return e&&e.data&&e.data.Message||'Unable to load report. Please retry.';}
vm.reload=function(){var v=++version;++detailVersion;vm.selected=null;vm.rows=[];vm.detailTotal=0;vm.loading=true;vm.error='';svc.report(request('groups')).then(function(d){if(v!==version)return;vm.groups=d.rows;vm.total=d.total;vm.groupPage=1;},function(e){if(v!==version)return;vm.groups=[];vm.total=0;vm.groupPage=1;vm.error=err(e);}).finally(function(){if(v===version)vm.loading=false;});};
vm.changed=function(){if(timer)$timeout.cancel(timer);++version;++detailVersion;vm.selected=null;vm.detailSearch='';vm.detailSearchText='';vm.rows=[];vm.loading=true;timer=$timeout(function(){timer=null;vm.reload();},180);};
vm.changeType=function(){vm.f.groupBy=vm.groupOptions()[0];vm.changed();};
vm.openCalendar=function(event){var input=event.currentTarget;if(input && typeof input.showPicker==='function'){try{input.showPicker();}catch(e){input.focus();}}else if(input){input.focus();}};
vm.changeDates=function(){try{var r=svc.range(vm.preset,vm.from,vm.to);vm.f.startDate=r.startDate;vm.f.endDate=r.endDate;vm.error='';vm.changed();}catch(e){vm.error=e.message;}};
vm.detailSearchChanged=function(){if(searchTimer)$timeout.cancel(searchTimer);searchTimer=$timeout(function(){searchTimer=null;vm.applyDetailSearch();},350);};
vm.applyDetailSearch=function(){if(searchTimer){$timeout.cancel(searchTimer);searchTimer=null;}var text=(vm.detailSearchText||'').trim();if(text===vm.detailSearch)return;vm.detailSearch=text;vm.searchDetails();};
vm.detailSearchKey=function(event){if(event.key==='Enter'){event.preventDefault();vm.applyDetailSearch();}else if(event.key==='Tab'){vm.applyDetailSearch();}};
vm.searchDetails=function(){if(!vm.selected)return;vm.page=1;vm.details();};
vm.open=function(g){vm.detailSearch='';vm.detailSearchText='';vm.selected=g;vm.page=1;vm.details();};
vm.details=function(){var v=++detailVersion,base=version,shell=document.getElementById('dashboard-report-shell'),grid=document.getElementById('dr-details-grid'),shellTop=shell?shell.scrollTop:0,gridTop=grid?grid.scrollTop:0,gridLeft=grid?grid.scrollLeft:0;vm.detailLoading=true;vm.detailError='';svc.report(request('details')).then(function(d){if(v!==detailVersion||base!==version)return;vm.rows=d.rows;vm.detailTotal=d.total;vm.pages=Math.max(1,Math.ceil(d.total/vm.size));},function(e){if(v===detailVersion)vm.detailError=err(e);}).finally(function(){if(v===detailVersion){vm.detailLoading=false;$timeout(function(){if(v!==detailVersion||base!==version)return;if(shell)shell.scrollTop=shellTop;if(grid){grid.scrollTop=gridTop;grid.scrollLeft=gridLeft;}},0);}});};
vm.go=function(p){if(p<1||p>vm.pages)return;vm.page=p;vm.details();};
vm.back=function(){++detailVersion;vm.detailSearch='';vm.detailSearchText='';vm.selected=null;vm.rows=[];};
vm.clear=function(){vm.f=angular.copy(initialFilters);vm.preset=initialPreset;vm.from=initialFrom?new Date(initialFrom.getTime()):null;vm.to=initialTo?new Date(initialTo.getTime()):null;vm.projectBlur();vm.employeeBlur();vm.page=1;vm.groupPage=1;vm.error='';vm.exportError='';vm.detailError='';vm.changed();};
vm.exportExcel=function(){if(vm.loading||vm.detailLoading||vm.exporting)return;var exportDetails=!!vm.selected,exportType=vm.f.reportType;vm.exporting=true;vm.exportError='';var columns=['ProjectName','Kind','Name','Description','StatusName','PriorityName','SituationName','Completion','OwnerName','Team','StartDate','EndDate'];if(vm.f.reportType==='milestoneprogress')columns.push('TotalTasks','CompletedTasks');
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
svc.report(request(exportDetails?'export':'groups')).then(function(d){if(exportDetails)d.rows.forEach(function(r){r.Team=vm.teamNames(r);});if(!exportDetails){columns=['Group','Count'];d.rows=d.rows.map(function(g){return {Group:g.GroupName,Count:g.Count};});d.rows.push({Group:'Total',Count:d.total});}var url=URL.createObjectURL(excelWorkbook(columns,d.rows)),a=document.createElement('a');a.href=url;a.download='Dashboard_'+exportType+(exportDetails?'_Details':'_Groups')+'.xlsx';document.body.appendChild(a);a.click();a.remove();$timeout(function(){URL.revokeObjectURL(url);},1000);},function(e){vm.exportError=err(e);}).finally(function(){vm.exporting=false;});};
svc.options().then(function(d){vm.look=d;['employees','projects','statuses','priorities','situations','tags'].forEach(function(k){var o={id:0,Name:'All '+k};if(k==='employees')o.DisplayName='All Employees';if(k==='projects')o.ProjectName='All Projects';vm.look[k].unshift(o);});vm.projectText=vm.projectLabel();vm.employeeText=vm.employeeLabel();},function(e){vm.error=err(e);});vm.reload();
var reportTop=null;function sizeReport(){var shell=document.getElementById('dashboard-report-shell');if(!shell)return;if(reportTop===null)reportTop=shell.getBoundingClientRect().top;var parent=shell.parentElement.getBoundingClientRect();shell.style.position='fixed';shell.style.top=reportTop+'px';shell.style.left=Math.max(0,parent.left)+'px';shell.style.width=parent.width+'px';shell.style.height=Math.max(120,window.innerHeight-reportTop-8)+'px';}
window.addEventListener('resize',sizeReport);$timeout(sizeReport,0);
$scope.$on('$destroy',function(){window.removeEventListener('resize',sizeReport);++version;++detailVersion;if(timer)$timeout.cancel(timer);if(searchTimer)$timeout.cancel(searchTimer);});
}]);}());
