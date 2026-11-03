(function () {
'use strict';
angular.module('pmsTeamDashboardApp', []).controller('TeamDashboardController', ['$scope', '$timeout', '$q', 'TeamDashboardService', function ($scope, $timeout, $q, service) {
const root=document.getElementById('teamDashboard'), $=id=>root.querySelector('#'+id);
root.setAttribute('data-team-started','true');
const esc=x=>String(x==null?'':x).replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const num=x=>Number(x)||0, hrs=x=>Number.isFinite(x)?x.toFixed(x>0&&x<0.01?4:2).replace(/\.00$/,'').replace(/(\.\d)0$/,'$1'):'–';
const pct=(a,b)=>b>0?Math.round(a/b*100):null, percent=x=>x==null?'–':x+'%';
const key=d=>d.getFullYear()+'-'+String(d.getMonth()+1).padStart(2,'0')+'-'+String(d.getDate()).padStart(2,'0');
const date=s=>s?new Date(String(s).slice(0,10)+'T00:00:00'):null;
const isoToday=()=>key(new Date()), localToday=()=>date(isoToday());
const initials=name=>String(name).split(/\s+/).filter(Boolean).slice(0,2).map(x=>x[0]).join('').toUpperCase();
const avatar=e=>'<span class="av sm" title="'+esc(e.name)+'">'+esc(initials(e.name))+'</span>';
const empty=(text='No records in the selected period.')=>'<p class="empty">'+esc(text)+'</p>';
const fmt=d=>d?d.toLocaleDateString('en-GB',{day:'numeric',month:'short'}):'–';
let data={employees:[],tasks:[],issues:[],projects:[]},focus='',project=0,filter='All',query='',sort='load',dir=-1,page=1;
let token=0,cancel=null,loading=false,failed=false;
const pageSize=50;
const isOpen=t=>!t.complete;
const overdue=t=>isOpen(t)&&t.due&&t.due<localToday();
const daysTo=t=>t.due?Math.round((Date.UTC(t.due.getFullYear(),t.due.getMonth(),t.due.getDate())-Date.UTC(localToday().getFullYear(),localToday().getMonth(),localToday().getDate()))/864e5):Infinity;
const unique=arr=>Array.from(new Map(arr.map(t=>[t.key,t])).values());
const avg=values=>values.length?Math.round(values.reduce((a,b)=>a+b,0)/values.length):null;
function tasksFor(id){return data.tasks.filter(t=>!id||t.dev===String(id));}
function issuesFor(tasks){const keys=new Set(tasks.map(t=>t.key));return data.issues.filter(i=>keys.has(i.key));}
function stats(e){
 const all=tasksFor(e.id),open=all.filter(isOpen),done=all.filter(t=>t.complete),known=done.filter(t=>t.done&&t.due);
 const remaining=open.reduce((a,t)=>a+Math.max(t.planned-(t.spent==null?0:t.spent),0),0);
 const logged=e.days.filter(d=>!d.future).reduce((a,d)=>a+num(d.spent),0);
 const issues=issuesFor(all), reopens=all.filter(t=>t.reopened).length;
 const load=e.shiftConfigured?pct(remaining,num(e.nextCapacity)):null;
 const atRisk=open.some(overdue), today=e.days.find(d=>d.key===isoToday());
 const flag=!e.shiftConfigured?'Shift not configured':num(e.nextCapacity)<=0?'No capacity':load>100?'Overloaded':atRisk?'At risk':e.missing?'Missing timesheet':today&&today.leave===1?'On leave':load<60?'Available':'On track';
 return {e,open:open.length,overdue:open.filter(overdue).length,high:open.filter(t=>/high/i.test(t.priority)).length,remaining,load,
 logged,expected:e.expected,taskEff:pct(known.filter(t=>t.done<=t.due).length,known.length),
 timeEff:avg(done.filter(t=>t.spent>0&&t.planned>0).map(t=>t.planned/t.spent*100)),
 reopen:pct(reopens,all.length),openIssues:issues.filter(i=>!i.complete).length,flag};
}
function selected(){return tasksFor(focus);}
function combined(tasks){
 const groups=new Map();tasks.forEach(t=>{let g=groups.get(t.key);if(!g){g=Object.assign({},t,{assignees:[],planned:0,spent:0,hasSpent:true});groups.set(t.key,g);}
  g.assignees.push(data.employees.find(e=>String(e.id)===t.dev));g.planned+=t.planned;if(t.spent==null)g.hasSpent=false;else g.spent+=t.spent;});
 return Array.from(groups.values());
}
function meter(name,value,description){return '<div class="meter"><div class="top">'+esc(name)+'</div><div class="big num" style="color:var(--ok)">'+percent(value)+'</div><div class="track"><div class="fill" style="width:'+Math.min(value||0,100)+'%;background:var(--ok)"></div><span class="tgt" style="left:85%"></span></div><p class="def">'+esc(description)+'</p></div>';}
function render(){
 const allStats=data.employees.map(stats),scope=allStats.filter(s=>!focus||String(s.e.id)===focus),raw=selected(),tasks=combined(raw);
 const statuses=[...new Set(tasks.map(t=>t.status))],count=s=>tasks.filter(t=>t.status===s).length;
 const total=tasks.length,remaining=scope.reduce((a,s)=>a+s.remaining,0),cap=scope.reduce((a,s)=>a+num(s.e.nextCapacity),0);
 const capacityKnown=scope.length>0&&scope.every(s=>s.e.shiftConfigured);
 const statusColor=s=>(tasks.find(t=>t.status===s)||{}).statusColor||'#6a6475';
 $('devSel').value=focus;$('projSel').value=String(project);
 const employee=data.employees.find(e=>String(e.id)===focus);
 $('focusChip').innerHTML=employee?'<span class="focus">'+avatar(employee)+' '+esc(employee.name)+'<button type="button" data-clear aria-label="Show whole team">×</button></span>':'';
 const kpis=[{key:'All',label:'Team tasks',value:total,note:scope.length+' employees'},
 ...statuses.map(s=>({key:s,label:s,value:count(s),note:percent(pct(count(s),total))+' of total',color:statusColor(s)})),
 {key:'Overdue',label:'Overdue',value:tasks.filter(overdue).length,note:'Past due, not completed',color:'var(--bad)'},
 {key:null,label:'Team load',value:percent(capacityKnown?pct(remaining,cap):null),note:hrs(remaining)+' h remaining / '+(capacityKnown?hrs(cap):'unknown')+' h capacity'}];
 $('kpis').innerHTML=kpis.map(k=>'<'+(k.key===null?'div':'button type="button"')+' class="kpi '+(filter===k.key?'on':'')+'" '+(k.key===null?'':'data-filter="'+esc(k.key)+'"')+'><span class="l">'+(k.color?'<i style="background:'+esc(k.color)+'"></i>':'')+esc(k.label)+'</span><span class="v num">'+esc(k.value)+'</span><span class="n">'+esc(k.note)+'</span></'+(k.key===null?'div':'button')+'>').join('');
 const columns=[['name','Developer'],['flag','Status'],['open','Open'],['overdue','Overdue'],['high','High open'],['load','Load'],['logged','Logged this week'],['taskEff','On time'],['timeEff','Time eff.'],['reopen','Reopened'],['openIssues','Open issues']];
 const sorted=[...allStats].sort((a,b)=>{const x=sort==='name'?a.e.name:a[sort],y=sort==='name'?b.e.name:b[sort];return (typeof x==='string'?x.localeCompare(y):(x==null?-1:x)-(y==null?-1:y))*dir;});
 $('wl').innerHTML='<thead><tr>'+columns.map(([k,l])=>'<th><button type="button" data-sort="'+k+'">'+l+(sort===k?(dir===1?' ▴':' ▾'):'')+'</button></th>').join('')+'</tr></thead><tbody>'+sorted.map(s=>'<tr tabindex="0" data-dev="'+s.e.id+'" class="'+(String(s.e.id)===focus?'sel':'')+'"><td><div class="dev">'+avatar(s.e)+'<div>'+esc(s.e.name)+'</div></div></td><td><span class="pill '+(s.load>100?'bad':s.overdue?'warn':'ok')+'">'+esc(s.flag)+'</span></td><td class="num">'+s.open+'</td><td class="num '+(s.overdue?'bad-t':'')+'">'+s.overdue+'</td><td class="num">'+s.high+'</td><td><div class="loadcell"><div class="lbar"><i style="width:'+Math.min(s.load||0,150)/150*100+'%;background:var(--'+(s.load>100?'bad':'ok')+')"></i><b></b></div><span class="num">'+percent(s.load)+'</span></div></td><td class="num">'+hrs(s.logged)+' / '+(s.e.shiftConfigured?hrs(s.expected):'–')+' h</td><td class="num">'+percent(s.taskEff)+'</td><td class="num">'+percent(s.timeEff)+'</td><td class="num">'+percent(s.reopen)+'</td><td class="num">'+s.openIssues+'</td></tr>').join('')+'</tbody>';
 $('teamLoadHint').textContent='Selected open work / next 10 working days';
 $('loads').innerHTML=[...scope].sort((a,b)=>(b.load||0)-(a.load||0)).map(s=>'<div class="ld"><span class="nm">'+esc(s.e.name)+'</span><div class="tr" title="'+esc(hrs(s.remaining)+' h remaining / '+hrs(s.e.nextCapacity)+' h capacity')+'"><i style="width:'+Math.min(s.load||0,150)/150*100+'%;background:var(--'+(s.load>100?'bad':'ok')+')"></i><span class="cap" style="left:66.67%"></span></div><span class="pc num">'+percent(s.load)+'</span></div>').join('')||empty('No permitted employees.');
 $('loadScale').innerHTML='<span></span><div>'+[0,50,100,150].map(x=>'<span style="left:'+x/150*100+'%">'+x+'%</span>').join('')+'</div><span></span>';
 renderSuggestions(allStats);
 const attention=tasks.filter(t=>isOpen(t)&&daysTo(t)<=3).sort((a,b)=>a.due-b.due);
 $('att').innerHTML=attention.slice(0,8).map(t=>'<div class="it"><span>'+t.assignees.map(avatar).join('')+'</span><div><div class="t">'+esc(t.title)+'</div><div class="m">'+esc(t.id+' · '+t.assignees.map(e=>e.name).join(', ')+' · '+t.priority+' · '+t.status)+'</div></div><span class="pill '+(overdue(t)?'bad':'warn')+'">'+esc(overdue(t)?(-daysTo(t))+' days overdue':daysTo(t)===0?'Due today':'Due in '+daysTo(t)+' days')+'</span></div>').join('')||empty('No overdue work or deadlines within 3 days.');
 if(attention.length>8)$('att').innerHTML+='<button type="button" class="btn" data-filter="Overdue">Show overdue tasks below</button>';
 renderTimesheets(scope);
 $('stTotal').textContent=total+' assigned items';
 $('stStack').innerHTML=statuses.map(s=>'<span style="width:'+count(s)/total*100+'%;background:'+esc(statusColor(s))+'" title="'+esc(s)+': '+count(s)+'"></span>').join('');
 $('stLegend').innerHTML=statuses.map(s=>'<span><i style="background:'+esc(statusColor(s))+'"></i>'+esc(s)+' <b>'+count(s)+'</b> '+percent(pct(count(s),total))+'</span>').join('');
 const priorities=[...new Set(tasks.map(t=>t.priority))];
 $('matrix').innerHTML='<tr><th></th>'+statuses.map(s=>'<th>'+esc(s)+'</th>').join('')+'<th>Total</th></tr>'+priorities.map(p=>'<tr><th>'+esc(p)+'</th>'+statuses.map(s=>'<td>'+tasks.filter(t=>t.priority===p&&t.status===s).length+'</td>').join('')+'<td>'+tasks.filter(t=>t.priority===p).length+'</td></tr>').join('');
 const complete=tasks.filter(t=>t.complete), dated=complete.filter(t=>t.done&&t.due),onTime=dated.filter(t=>t.done<=t.due).length;
 const eligible=complete.filter(t=>t.hasSpent&&t.planned>0&&t.spent>0), planned=eligible.reduce((a,t)=>a+t.planned,0),spent=eligible.reduce((a,t)=>a+t.spent,0);
 $('eff').innerHTML=meter('Task efficiency',pct(onTime,dated.length),dated.length?onTime+' of '+dated.length+' completed items with recorded completion dates finished on or before the due date. '+(complete.length-dated.length)+' have no comparable completion date.':'No completed items have both a recorded completion date and a due date.')+
 meter('Time efficiency',avg(eligible.map(t=>t.planned/t.spent*100)),hrs(planned)+' h planned vs '+hrs(spent)+' h spent on '+eligible.length+' completed items with recorded time. Average of each item’s planned / spent percentage. Above 100% means work took less time than planned.');
 const issues=issuesFor(raw),resolved=issues.filter(i=>i.complete).length,reopened=tasks.filter(t=>t.reopened).length;
 $('quality').innerHTML='<div class="qrow"><span>Currently reopened items</span><b class="num">'+reopened+' · '+percent(pct(reopened,total))+'</b></div><div class="qrow"><span>Issues resolved</span><b class="num">'+resolved+' of '+issues.length+' · '+percent(pct(resolved,issues.length))+'</b></div><div class="qrow"><span>Open issues</span><b class="num">'+(issues.length-resolved)+'</b></div><p class="def">Issues are counted once across selected assigned items. Historical reopen events are not available in the supplied schema.</p>';
 renderTasks(tasks,statuses);
}
function renderTimesheets(scope){
 const days=(data.employees[0]||{}).days||[];
 $('tsHint').textContent='Recorded timer hours; approved leave and shift capacity';
 $('ts').innerHTML='<tr><th class="nm">Employee</th>'+days.map(d=>'<th class="'+(d.key===isoToday()?'today':'')+'">'+esc(d.label)+(d.key===isoToday()?' · today':'')+'</th>').join('')+'<th>Total</th></tr>'+scope.map(s=>'<tr><th class="nm">'+avatar(s.e)+' '+esc(s.e.name)+'</th>'+s.e.days.map(d=>{
 const title=hrs(num(d.spent))+' h spent · '+(s.e.shiftConfigured?hrs(d.capacity):'unknown')+' h capacity'+(d.leaveLabel?' · '+d.leaveLabel:'')+(d.holiday?' · Holiday':d.weekOff?' · Weekoff':'');
 const value=d.future?'–':d.leave===1&&!d.spent?'Leave':(d.holiday||d.weekOff)&&!d.spent?'Off':hrs(num(d.spent))+(d.leave>0&&d.leave<1?' · ½':'');
 return '<td class="'+(d.leave?'leave':d.future?'fut':d.capacity>0&&d.spent<d.capacity?'miss':'')+'" title="'+esc(title)+'">'+esc(value)+'</td>';
 }).join('')+'<td class="tot num" title="'+esc(hrs(s.e.weekCapacity)+' h capacity this week')+'">'+hrs(s.logged)+' / '+(s.e.shiftConfigured?hrs(s.expected):'–')+'</td></tr>').join('');
}
function renderSuggestions(allStats){
 const receivers=allStats.filter(s=>s.load!=null&&s.load<85&&!(s.e.days.find(d=>d.key===isoToday())||{}).leave).sort((a,b)=>a.load-b.load);
 const suggestions=[];
 allStats.filter(s=>s.load>100).forEach(s=>{const candidates=tasksFor(s.e.id).filter(t=>isOpen(t)&&/^open$|^to start$|^not started$/i.test(t.status)&&!/high/i.test(t.priority)&&t.spent===0&&t.planned>0);
 candidates.forEach(t=>{const receiver=receivers.find(r=>r.e.id!==s.e.id&&r.e.nextCapacity>0&&(r.remaining+t.planned)/r.e.nextCapacity<=0.85);
 if(receiver&&suggestions.length<6&&(!focus||String(s.e.id)===focus||String(receiver.e.id)===focus))suggestions.push({t,from:s,to:receiver});});});
 $('moves').innerHTML=suggestions.length?suggestions.map(m=>'<div class="mv"><div><div class="t">'+esc(m.t.title)+'</div><div class="m">'+esc(m.t.id+' · '+m.t.priority+' · '+hrs(m.t.planned)+' h · '+m.t.project)+'</div></div><div class="arrow">'+avatar(m.from.e)+esc(m.from.e.name)+' → '+avatar(m.to.e)+esc(m.to.e.name)+'</div></div>').join('')+'<p class="def">Each suggestion is evaluated independently. Review team/project eligibility and apply changes through Assign Project. This dashboard does not save reassignments.</p>':empty('No suitable reassignment suggestions for the selected work.');
}
function renderTasks(tasks,statuses){
 $('taskTitle').textContent=focus?(data.employees.find(e=>String(e.id)===focus)||{}).name+' — assigned work':'Team tasks';
 $('chips').innerHTML=['All',...statuses,'Overdue'].map(s=>'<button type="button" class="chipbtn '+(filter===s?'on':'')+'" data-filter="'+esc(s)+'">'+esc(s)+'</button>').join('');
 const q=query.trim().toLowerCase(),rows=tasks.filter(t=>filter==='All'||(filter==='Overdue'?overdue(t):t.status===filter)).filter(t=>!q||(t.id+' '+t.title+' '+t.project).toLowerCase().includes(q)).sort((a,b)=>Number(a.complete)-Number(b.complete)||(a.due||Infinity)-(b.due||Infinity));
 page=Math.min(page,Math.max(1,Math.ceil(rows.length/pageSize)));
 $('tbl').innerHTML='<thead><tr><th>ID / Type</th><th>Task</th><th>Assignee</th><th>Project</th><th>Priority</th><th>Status</th><th>Due</th><th>Hours (spent / planned)</th><th>Issues</th></tr></thead><tbody>'+rows.slice((page-1)*pageSize,page*pageSize).map(t=>{
 const issues=issuesFor([t]);return '<tr><td class="num muted">'+esc(t.id)+'<small style="display:block">'+esc(t.kind)+'</small></td><td>'+esc(t.title)+'</td><td>'+t.assignees.map(e=>avatar(e)+' '+esc(e.name)).join('<br>')+'</td><td>'+esc(t.project)+'</td><td><span class="pdot" style="background:'+esc(t.priorityColor)+'"></span>'+esc(t.priority)+'</td><td><span class="st"><i style="background:'+esc(t.statusColor)+'"></i>'+esc(t.status)+'</span></td><td class="num '+(overdue(t)?'over':'')+'">'+fmt(t.due)+(overdue(t)?' · '+(-daysTo(t))+'d late':'')+'</td><td class="num">'+(t.hasSpent?hrs(t.spent):'–')+' / '+hrs(t.planned)+'</td><td class="num">'+issues.filter(i=>i.complete).length+' / '+issues.length+'</td></tr>';
 }).join('')+(rows.length?'':'<tr><td colspan="9">No tasks match this filter.</td></tr>')+'</tbody>';
 $('paging').innerHTML='<span>'+rows.length+' items · Page '+page+' of '+Math.max(1,Math.ceil(rows.length/pageSize))+'</span> <button type="button" class="btn" data-page="-1" '+(page<=1?'disabled':'')+'>Previous</button> <button type="button" class="btn" data-page="1" '+(page*pageSize>=rows.length?'disabled':'')+'>Next</button>';
}
function range(){
 const t=localToday(),s=new Date(t),e=new Date(t),mode=$('period').value;
 if(mode==='all')return {fromDate:null,toDate:null};
 if(mode==='week'||mode==='previousWeek'){s.setDate(s.getDate()-((s.getDay()+6)%7)-(mode==='previousWeek'?7:0));e.setTime(s.getTime());e.setDate(e.getDate()+6);}
 else if(mode==='month'||mode==='previousMonth'){s.setDate(1);if(mode==='previousMonth')s.setMonth(s.getMonth()-1);e.setTime(s.getTime());e.setMonth(e.getMonth()+1,0);}
 else if(mode==='quarter'||mode==='previousQuarter'){s.setDate(1);s.setMonth(Math.floor(t.getMonth()/3)*3-(mode==='previousQuarter'?3:0));e.setTime(s.getTime());e.setMonth(e.getMonth()+3,0);}
 else if(mode==='year'||mode==='previousYear'){s.setFullYear(t.getFullYear()-(mode==='previousYear'?1:0),0,1);e.setFullYear(s.getFullYear(),11,31);}
 else if(mode==='yesterday'){s.setDate(s.getDate()-1);e.setTime(s.getTime());}
 else if(mode==='custom'){return {fromDate:$('fromDate').value,toDate:$('toDate').value};}
 return {fromDate:key(s),toDate:key(e)};
}
async function reload(){
 const r=range();if($('period').value!=='all'&&(!r.fromDate||!r.toDate||r.toDate<r.fromDate)){$('loadMessage').textContent='Select a valid date range.';return;}
 failed=false;const request=++token;if(cancel)cancel.resolve();cancel=$q.defer();loading=true;root.setAttribute('aria-busy','true');root.classList.add('loading');$('loadMessage').textContent='Loading team data…';
 try{
  const result=await service.load(Object.assign(r,{today:isoToday(),projectId:project}),cancel.promise);if(request!==token)return;
  if(!result||!Array.isArray(result.employees)||!Array.isArray(result.tasks)||!Array.isArray(result.issues)||!Array.isArray(result.projects))throw new Error('The server returned an invalid team dashboard response.');
  data=result;
  data.employees=data.employees||[];data.projects=data.projects||[];
  data.tasks=(data.tasks||[]).map(t=>({key:t.Kind+':'+t.ItemId+':'+t.ProjectId,id:t.Kind.toUpperCase()+'-'+t.ItemId,kind:t.Kind,title:t.Title||'Unnamed item',dev:String(t.EmployeeId),project:t.ProjectName,projectId:num(t.ProjectId),status:t.StatusName||'Unspecified',statusColor:color(t.StatusColor),priority:t.PriorityName||'Unspecified',priorityColor:color(t.PriorityColor),complete:t.IsComplete===true||num(t.IsComplete)===1,reopened:/reopen/i.test(t.StatusName||''),due:date(t.DueDate),done:date(t.CompletedDate),planned:num(t.PlannedHours),spent:t.SpentHours==null?null:num(t.SpentHours)}));
  data.issues=(data.issues||[]).map(i=>({id:i.IssueId,key:i.Kind+':'+i.ItemId+':'+i.ProjectId,complete:i.IsComplete===true||num(i.IsComplete)===1}));
  if(focus&&!data.employees.some(e=>String(e.id)===focus))focus='';
  $('devSel').innerHTML='<option value="">All '+data.employees.length+' employees</option>'+data.employees.map(e=>'<option value="'+e.id+'">'+esc(e.name)+'</option>').join('');
  $('projSel').innerHTML='<option value="0">All projects</option>'+data.projects.map(p=>'<option value="'+num(p.id)+'">'+esc(p.ProjectName)+'</option>').join('');
  if(project&&!data.projects.some(p=>num(p.id)===project)){project=0;reload();return;}
  $('userName').textContent=data.userName||'Team leader';$('userAvatar').textContent=initials(data.userName||'User');
  $('teamTitle').textContent=data.isAdmin?'Team dashboard':'My team dashboard';
  $('asOf').textContent=localToday().toLocaleDateString('en-GB',{weekday:'long',day:'numeric',month:'long',year:'numeric'})+' · '+(r.fromDate?r.fromDate+' to '+r.toDate:'All dates');
  render();$('loadMessage').textContent=data.employees.length?'Updated — '+data.employees.length+' permitted employees.':'No permitted employees.';
 }catch(err){if(request===token&&!err.cancelled){failed=true;$('loadMessage').textContent=err.message||'Unable to load team data.';$('dashboardContent').hidden=true;}}
 finally{if(request===token){loading=false;root.removeAttribute('aria-busy');root.classList.remove('loading');}}
 if(request===token&&!loading&&!failed)$('dashboardContent').hidden=false;
}
function color(value){return /^#[0-9a-f]{3,8}$/i.test(value||'')?value:'#6a6475';}
function clicked(event){
 const b=event.target.closest('button,[data-dev]');if(!b||!root.contains(b))return;
 if(b.dataset.sort){dir=sort===b.dataset.sort?-dir:-1;sort=b.dataset.sort;render();}
 else if(b.dataset.filter){filter=b.dataset.filter;page=1;render();$('taskCard').scrollIntoView({behavior:'smooth',block:'start'});}
 else if(b.dataset.dev){focus=focus===b.dataset.dev?'':b.dataset.dev;page=1;render();}
 else if(b.hasAttribute('data-clear')){focus='';page=1;render();}
 else if(b.dataset.page){page+=Number(b.dataset.page);render();}
}
root.addEventListener('click',clicked);
root.addEventListener('keydown',event=>{const row=event.target.closest('tr[data-dev]');if(row&&(event.key==='Enter'||event.key===' ')){event.preventDefault();row.click();}});
$('devSel').addEventListener('change',event=>{focus=event.target.value;page=1;render();});
$('projSel').addEventListener('change',event=>{project=num(event.target.value);page=1;reload();});
$('q').addEventListener('input',event=>{query=event.target.value;page=1;render();});
$('period').addEventListener('change',()=>{$('customDates').hidden=$('period').value!=='custom';if($('period').value!=='custom'){page=1;reload();}});
$('applyDates').addEventListener('click',()=>{page=1;reload();});
$('refresh').addEventListener('click',()=>reload());
$('fromDate').value=$('toDate').value=isoToday();
$scope.$on('$destroy',()=>{if(cancel)cancel.resolve();root.removeEventListener('click',clicked);});
$timeout(reload,0);
}]);
}());
