<%@ Page Title="PMS Team Dashboard" Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="pms_team_dashboard" MasterPageFile="~/pms/angularmaster/angular_head.master" %>
<asp:Content ID="TeamDashboardHead" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Figtree:wght@400;500;600;700&amp;family=Sora:wght@500;600;700&amp;family=JetBrains+Mono:wght@400;500&amp;display=swap">
<style>.team-dashboard{
  --bg:#f4f3f7; --surface:#ffffff; --sunk:#efedf3; --line:#e1dde8; --fg:#1d1a24; --muted:#6a6475;
  --accent:#5a2a7a; --accent-soft:#efe6f5;
  --s-open:#3b7ddd; --s-prog:#d99a1e; --s-reop:#8a5cd6; --s-done:#2e9a5c;
  --p-high:#d1453b; --p-med:#d99a1e; --p-low:#5f8f7a;
  --bad:#c93a31; --bad-soft:#fbe6e4; --warn:#a86a00; --warn-soft:#fbf0d9; --ok:#23804b; --ok-soft:#e1f2e7; --info:#2f6fc4; --info-soft:#e3edfa;
  --f-display:"Sora",system-ui,sans-serif; --f-body:"Figtree",system-ui,-apple-system,"Segoe UI",sans-serif;
  --f-mono:"JetBrains Mono",ui-monospace,Menlo,monospace;
}
.team-dashboard[data-theme="dark"]{
  --bg:#141218; --surface:#1d1a23; --sunk:#16141b; --line:#2f2b38; --fg:#ece8f2; --muted:#a39cb0;
  --accent:#c79be6; --accent-soft:#2e2238;
  --s-open:#6aa3f2; --s-prog:#e9b44c; --s-reop:#ad8bef; --s-done:#55c487;
  --p-high:#f0756b; --p-med:#e9b44c; --p-low:#86b8a2;
  --bad:#ff8077; --bad-soft:#3a1d1b; --warn:#e9b44c; --warn-soft:#3a2d12; --ok:#55c487; --ok-soft:#173022; --info:#7fb0f5; --info-soft:#1a2a40; color-scheme:dark}
.team-dashboard *{box-sizing:border-box}
.team-dashboard{background:var(--bg);color:var(--fg);font-family:var(--f-body);font-size:14px;line-height:1.45}
.team-dashboard button,.team-dashboard select,.team-dashboard input{font:inherit;color:inherit}
.team-dashboard button{cursor:pointer}
.team-dashboard :focus-visible{outline:2px solid var(--accent);outline-offset:2px}
.team-dashboard .num{font-family:var(--f-mono);font-variant-numeric:tabular-nums}
.team-dashboard .muted{color:var(--muted)}
.team-dashboard h1,.team-dashboard h2{margin:0;text-wrap:balance}
.team-dashboard .scroll-x{overflow-x:auto}
.team-dashboard .appbar{background:var(--surface);border-bottom:1px solid var(--line);padding-inline:20px}
.team-dashboard .appbar .in{max-width:1360px;margin:0 auto;display:flex;align-items:center;gap:20px;flex-wrap:wrap;padding-block:10px 0}
.team-dashboard .brand{display:flex;align-items:center;gap:10px;font-weight:700;font-size:15px;padding-bottom:8px}
.team-dashboard .tabs{display:flex;gap:4px;margin-left:12px;flex:1;min-width:0;overflow-x:auto}
.team-dashboard .tabs a{padding:10px 12px;color:var(--muted);text-decoration:none;border-bottom:2px solid transparent;white-space:nowrap;font-weight:500}
.team-dashboard .tabs a.on{color:var(--accent);border-bottom-color:var(--accent)}
.team-dashboard .user{display:flex;align-items:center;gap:10px;padding-bottom:8px}
.team-dashboard .user small{display:block;color:var(--muted);font-size:12px}
.team-dashboard .av{width:30px;height:30px;flex:none;border-radius:50%;background:var(--accent-soft);color:var(--accent);display:inline-grid;place-items:center;font-weight:700;font-size:12px}
.team-dashboard .av.sm{width:22px;height:22px;font-size:10px}
.team-dashboard .wrap{max-width:1360px;margin:0 auto;padding-inline:20px;padding-block:20px 56px;display:grid;gap:16px}
.team-dashboard .head{display:flex;flex-wrap:wrap;gap:12px 20px;align-items:flex-end;justify-content:space-between}
.team-dashboard .head h1{font-family:var(--f-display);font-size:22px;font-weight:600}
.team-dashboard .head p{margin:2px 0 0;color:var(--muted)}
.team-dashboard .filters{display:flex;gap:8px 12px;flex-wrap:wrap;align-items:center}
.team-dashboard .filters label{font-size:12px;color:var(--muted);display:flex;gap:6px;align-items:center}
.team-dashboard select,.team-dashboard input[type=search]{background:var(--surface);border:1px solid var(--line);border-radius:8px;padding:7px 10px}
.team-dashboard .focus{display:inline-flex;align-items:center;gap:8px;background:var(--accent-soft);color:var(--accent);border-radius:999px;padding:4px 6px 4px 4px;font-weight:600;font-size:13px}
.team-dashboard .focus button{background:none;border:0;color:inherit;padding:0 6px;font-size:15px;line-height:1}
.team-dashboard .kpis{display:grid;grid-template-columns:repeat(7,minmax(0,1fr));background:var(--surface);border:1px solid var(--line);border-radius:12px;overflow:hidden}
.team-dashboard .kpi{padding:14px 16px;border:0;border-left:1px solid var(--line);border-bottom:3px solid transparent;display:grid;gap:2px;text-align:left;background:none;min-width:0}
.team-dashboard .kpi:first-child{border-left:0}
.team-dashboard button.kpi:hover{background:var(--sunk)}
.team-dashboard .kpi.on{border-bottom-color:var(--accent);background:var(--accent-soft)}
.team-dashboard .kpi .l{font-size:12px;color:var(--muted);display:flex;align-items:center;gap:6px}
.team-dashboard .kpi .l i{width:8px;height:8px;border-radius:2px;display:inline-block}
.team-dashboard .kpi .v{font-family:var(--f-display);font-size:26px;font-weight:600;line-height:1.15}
.team-dashboard .kpi .n{font-size:12px;color:var(--muted)}
.team-dashboard .kpi.alert .v{color:var(--bad)}
.team-dashboard .row{display:grid;gap:16px;grid-template-columns:minmax(0,1.4fr) minmax(0,1fr)}
.team-dashboard .row3{display:grid;gap:16px;grid-template-columns:repeat(3,minmax(0,1fr))}
.team-dashboard .card{background:var(--surface);border:1px solid var(--line);border-radius:12px;padding:16px 18px;display:grid;gap:14px;align-content:start;min-width:0}
.team-dashboard .ch{display:flex;justify-content:space-between;align-items:baseline;gap:10px;flex-wrap:wrap}
.team-dashboard .ch h2{font-size:15px;font-weight:600}
.team-dashboard .hint{font-size:12px;color:var(--muted)}
.team-dashboard .def{font-size:12px;color:var(--muted);margin:0}
.team-dashboard .pill{font-size:11px;font-weight:600;padding:3px 8px;border-radius:999px;white-space:nowrap;display:inline-block}
.team-dashboard .pill.bad{background:var(--bad-soft);color:var(--bad)}
.team-dashboard .pill.warn{background:var(--warn-soft);color:var(--warn)}
.team-dashboard .pill.ok{background:var(--ok-soft);color:var(--ok)}
.team-dashboard .pill.info{background:var(--info-soft);color:var(--info)}
.team-dashboard .pill.mut{background:var(--sunk);color:var(--muted)}
.team-dashboard .pdot{display:inline-block;width:8px;height:8px;border-radius:50%;margin-right:6px;vertical-align:middle}
.team-dashboard table.wl{width:100%;border-collapse:collapse;font-size:13px;min-width:1000px}
.team-dashboard .wl th{text-align:left;font-weight:500;color:var(--muted);font-size:12px;padding:0;border-bottom:1px solid var(--line);white-space:nowrap}
.team-dashboard .wl th button{background:none;border:0;padding:8px 10px;color:inherit;font-weight:500;width:100%;text-align:inherit}
.team-dashboard .wl th button:hover{color:var(--fg)}
.team-dashboard .wl th.r,.team-dashboard .wl td.r{text-align:right}
.team-dashboard .wl td{padding:9px 10px;border-bottom:1px solid var(--line);vertical-align:middle;white-space:nowrap}
.team-dashboard .wl tbody tr{cursor:pointer}
.team-dashboard .wl tbody tr:hover td{background:var(--sunk)}
.team-dashboard .wl tbody tr.sel td{background:var(--accent-soft)}
.team-dashboard .wl tr:last-child td{border-bottom:0}
.team-dashboard .dev{display:flex;align-items:center;gap:10px}
.team-dashboard .dev small{display:block;color:var(--muted);font-size:11px}
.team-dashboard .loadcell{display:flex;align-items:center;gap:8px;min-width:150px}
.team-dashboard .lbar{position:relative;flex:1;height:8px;background:var(--sunk);border-radius:4px}
.team-dashboard .lbar i{position:absolute;left:0;top:0;bottom:0;border-radius:4px}
.team-dashboard .lbar b{position:absolute;left:66.67%;top:-3px;bottom:-3px;width:2px;background:var(--fg);opacity:.45}
.team-dashboard .zero{color:var(--muted)}
.team-dashboard .bad-t{color:var(--bad);font-weight:600}
.team-dashboard .warn-t{color:var(--warn);font-weight:600}
.team-dashboard .loads{display:grid;gap:7px}
.team-dashboard .ld{display:grid;grid-template-columns:120px minmax(0,1fr) 56px;gap:10px;align-items:center;font-size:13px}
.team-dashboard .ld .nm{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
.team-dashboard .ld .tr{position:relative;height:16px;background:var(--sunk);border-radius:4px}
.team-dashboard .ld .tr i{position:absolute;left:0;top:0;bottom:0;border-radius:4px}
.team-dashboard .ld .tr .cap{position:absolute;top:-4px;bottom:-4px;width:2px;background:var(--fg);opacity:.55}
.team-dashboard .ld .pc{text-align:right}
.team-dashboard .scale{display:grid;grid-template-columns:120px minmax(0,1fr) 56px;gap:10px;font-size:11px;color:var(--muted)}
.team-dashboard .scale div{position:relative;height:14px}
.team-dashboard .scale span{position:absolute;transform:translateX(-50%);font-family:var(--f-mono)}
.team-dashboard .moves{display:grid;gap:8px}
.team-dashboard .mv{display:grid;grid-template-columns:minmax(0,1fr) auto;gap:4px 10px;padding:10px 12px;border:1px solid var(--line);border-radius:8px;align-items:center}
.team-dashboard .mv .t{font-weight:500;overflow-wrap:anywhere}
.team-dashboard .mv .m{font-size:12px;color:var(--muted)}
.team-dashboard .mv .arrow{display:flex;align-items:center;gap:6px;font-size:13px;flex-wrap:wrap}
.team-dashboard .btn{border:1px solid var(--line);background:var(--surface);border-radius:7px;padding:5px 12px;font-size:13px;font-weight:500}
.team-dashboard .btn:hover{border-color:var(--accent);color:var(--accent)}
.team-dashboard .btn.primary{background:var(--accent);border-color:var(--accent);color:var(--surface)}
.team-dashboard .btn.primary:hover{filter:brightness(1.08);color:var(--surface)}
.team-dashboard .btnrow{display:flex;gap:8px;flex-wrap:wrap;align-items:center}
.team-dashboard .empty{color:var(--muted);font-size:13px;padding:8px 0;margin:0}
.team-dashboard .done-note{font-size:12px;color:var(--ok)}
.team-dashboard .att{display:grid}
.team-dashboard .att .it{display:grid;grid-template-columns:auto minmax(0,1fr) auto;gap:2px 10px;padding:9px 0;border-top:1px solid var(--line);align-items:start}
.team-dashboard .att .it:first-child{border-top:0}
.team-dashboard .att .t{font-weight:500;overflow-wrap:anywhere}
.team-dashboard .att .m{font-size:12px;color:var(--muted)}
.team-dashboard table.ts{width:100%;border-collapse:separate;border-spacing:3px;font-size:12px;min-width:420px}
.team-dashboard .ts th{font-weight:500;color:var(--muted);text-align:center;padding:2px}
.team-dashboard .ts th.nm{text-align:left;white-space:nowrap}
.team-dashboard .ts td{text-align:center;border-radius:5px;padding:7px 2px;font-family:var(--f-mono);font-variant-numeric:tabular-nums}
.team-dashboard .ts td.miss{background:var(--bad-soft);color:var(--bad);font-weight:600}
.team-dashboard .ts td.leave{background:repeating-linear-gradient(135deg,var(--sunk) 0 4px,var(--surface) 4px 8px);color:var(--muted);font-family:var(--f-body)}
.team-dashboard .ts td.fut{background:none;color:var(--muted)}
.team-dashboard .ts td.tot{background:none;text-align:right;color:var(--muted)}
.team-dashboard .ts th.today{color:var(--fg);font-weight:700}
.team-dashboard .stack{display:flex;height:14px;border-radius:7px;overflow:hidden;background:var(--sunk);gap:2px}
.team-dashboard .stack span{display:block;height:100%}
.team-dashboard .legend{display:flex;flex-wrap:wrap;gap:6px 16px;font-size:13px}
.team-dashboard .legend span{display:flex;align-items:center;gap:6px}
.team-dashboard .legend i{width:10px;height:10px;border-radius:3px}
.team-dashboard .legend b{font-family:var(--f-mono);font-weight:500}
.team-dashboard .mx{width:100%;border-collapse:separate;border-spacing:4px;font-size:13px}
.team-dashboard .mx th{font-weight:500;color:var(--muted);font-size:12px;text-align:left;padding:2px 4px;white-space:nowrap}
.team-dashboard .mx td{text-align:center;border-radius:6px;padding:9px 4px;font-family:var(--f-mono)}
.team-dashboard .mx td.tot{color:var(--muted);background:none;white-space:nowrap}
.team-dashboard .meter{display:grid;gap:6px}
.team-dashboard .meter .top{display:flex;justify-content:space-between;align-items:baseline;gap:8px}
.team-dashboard .meter .big{font-family:var(--f-display);font-size:28px;font-weight:600}
.team-dashboard .track{position:relative;height:10px;background:var(--sunk);border-radius:5px}
.team-dashboard .track .fill{height:100%;border-radius:5px}
.team-dashboard .track .tgt{position:absolute;top:-4px;bottom:-4px;width:2px;background:var(--fg);opacity:.55}
.team-dashboard .split{display:grid;gap:18px}
.team-dashboard .qrow{display:grid;grid-template-columns:minmax(0,1fr) auto;gap:4px 10px;font-size:13px;padding:7px 0;border-top:1px solid var(--line)}
.team-dashboard .qrow:first-child{border-top:0}
.team-dashboard .tools{display:flex;gap:6px;flex-wrap:wrap;align-items:center}
.team-dashboard .chipbtn{border:1px solid var(--line);background:var(--surface);border-radius:999px;padding:5px 12px;font-size:13px}
.team-dashboard .chipbtn.on{background:var(--accent);border-color:var(--accent);color:var(--surface)}
.team-dashboard table.t{width:100%;border-collapse:collapse;font-size:13px;min-width:860px}
.team-dashboard .t th{text-align:left;font-weight:500;color:var(--muted);font-size:12px;padding:8px 10px;border-bottom:1px solid var(--line);white-space:nowrap}
.team-dashboard .t td{padding:9px 10px;border-bottom:1px solid var(--line);vertical-align:top}
.team-dashboard .t tr:last-child td{border-bottom:0}
.team-dashboard .st{display:inline-flex;align-items:center;gap:6px;white-space:nowrap}
.team-dashboard .st i{width:8px;height:8px;border-radius:50%}
.team-dashboard .over{color:var(--bad);font-weight:600}
.team-dashboard .moved{font-size:11px;color:var(--info)}
.team-dashboard .more{font-size:12px;color:var(--muted);margin:6px 0 0}
@media (max-width:1180px){.team-dashboard .kpis{grid-template-columns:repeat(4,minmax(0,1fr))}
.team-dashboard .kpi:nth-child(5){border-left:0}
.team-dashboard .kpi:nth-child(n+5){border-top:1px solid var(--line)}
.team-dashboard .row3{grid-template-columns:repeat(2,minmax(0,1fr))}
}
@media (max-width:900px){.team-dashboard .row,.team-dashboard .row3{grid-template-columns:minmax(0,1fr)}
}
@media (max-width:560px){.team-dashboard .wrap,.team-dashboard .appbar{padding-inline:16px}
.team-dashboard .kpis{grid-template-columns:repeat(2,minmax(0,1fr))}
.team-dashboard .kpi{border-left:0!important;border-top:1px solid var(--line)}
.team-dashboard .kpi:nth-child(-n+2){border-top:0}
.team-dashboard .kpi:nth-child(even){border-left:1px solid var(--line)!important}
.team-dashboard .user small{display:none}
.team-dashboard .ld,.team-dashboard .scale{grid-template-columns:84px minmax(0,1fr) 46px}
}
@media (prefers-reduced-motion:reduce){.team-dashboard *{transition:none!important;scroll-behavior:auto!important}
}

.team-dashboard {color-scheme:light;min-width:0}
.team-dashboard .kpis{grid-template-columns:repeat(auto-fit,minmax(140px,1fr))}
.team-dashboard .kpi{min-width:0}
.team-dashboard .load-status{padding:10px 20px;display:flex;align-items:center;gap:12px;min-height:45px}
.team-dashboard.loading #dashboardContent{opacity:.5;pointer-events:none}
.team-dashboard #dashboardContent{display:grid;gap:16px;min-width:0}
.team-dashboard .wrap{display:block}
.team-dashboard .head{margin-bottom:16px}
.team-dashboard #paging{display:flex;gap:10px;align-items:center;justify-content:flex-end;margin-top:12px}
.team-dashboard [hidden]{display:none!important}
.team-dashboard button:disabled{cursor:default;opacity:.45}
.team-dashboard input[type=date]{padding:7px;border:1px solid var(--line);border-radius:8px;background:var(--surface);color:var(--fg)}
.team-dashboard .scroll-x{max-width:100%}
.team-dashboard .row,.team-dashboard .row3{margin:0}
.team-dashboard #wl thead th,.team-dashboard #tbl thead th{position:sticky;top:0;background:var(--surface)}
.team-dashboard #taskCard{scroll-margin-top:90px}

.team-dashboard .sub-header .row{display:flex;flex-wrap:wrap;gap:0}
.team-dashboard .sub-header{background:#fff;border-bottom:1px solid #dfe3e8;padding:12px 16px}
.team-dashboard .sub-header .sub-page-title{font-size:18px;font-weight:600;color:#333}
.team-dashboard .sub-header .md-main-filter-row{display:flex;align-items:center;gap:10px;flex-wrap:wrap}
.team-dashboard .sub-header .md-date{display:flex;align-items:center;gap:7px;position:relative;font-size:12px}
.team-dashboard .sub-header .md-range-trigger{height:32px;min-width:155px;max-width:100%;padding:4px 9px;border:1px solid #d7dce2;border-radius:4px;background:#fff;color:#3d4650;font-size:11px}
.team-dashboard .sub-header #period{min-width:120px}
.team-dashboard .sub-header #customDates{display:flex;justify-content:flex-end;flex-wrap:wrap;gap:8px;margin-top:10px}
.team-dashboard .sub-header #customDates label{display:flex;align-items:center;gap:5px;font-size:12px}
.team-dashboard .load-status{padding:0 0 8px;min-height:28px}
.team-dashboard .head p{margin:0}
.team-dashboard #dashboardContent{max-width:none}
@media(max-width:767px){.team-dashboard .sub-header .row>div{margin-bottom:8px}.team-dashboard .sub-header .justify-content-end{justify-content:flex-start!important}}
</style>
<script src="<%= ResolveUrl("~/pms/teamdashboard/app/teamdashboard.main.js") %>?v=20261009-team-diagnostic-v6"></script>
<script src="<%= ResolveUrl("~/pms/teamdashboard/app/teamdashboard.service.js") %>?v=20261009-team-diagnostic-v6"></script>
<script>
window.addEventListener('load', function () {
    window.setTimeout(function () {
        var panel = document.getElementById('teamDashboard');
        var message = document.getElementById('loadMessage');
        if (panel && message && !panel.hasAttribute('data-team-started')) {
            message.textContent = 'Dashboard could not start. Refresh the page after installing all files from the latest package.';
        }
    }, 1500);
});
</script>
</asp:Content>
<asp:Content ID="TeamDashboardContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="main-wrapper team-dashboard" id="teamDashboard" ng-app="pmsTeamDashboardApp" ng-controller="TeamDashboardController">
<div class="sub-header">
 <div class="row">
  <div class="col-xxl-3 col-lg-3 col-md-3 col-sm-12"><div class="sub-left"><div class="sub-page-title" id="teamTitle">Team Dashboard</div></div></div>
  <div class="col-xxl-6 col-lg-6 col-md-6 col-sm-12"><div class="md-main-filter-row">
   <select class="md-range-trigger" id="projSel" aria-label="Project filter"><option value="0">All projects</option></select>
   <select class="md-range-trigger" id="devSel" aria-label="Employee filter"><option value="">All employees</option></select>
  </div></div>
  <div class="col-xxl-3 col-lg-3 col-md-3 col-sm-12"><div class="d-flex justify-content-end align-items-center gap-1">
   <div class="md-date"><i class="ri-calendar-line"></i><select class="md-range-trigger" id="period" aria-label="Date period"><option value="all">All</option><option value="today" selected>Today</option><option value="week">This Week</option><option value="month">This Month</option><option value="quarter">This Quarter</option><option value="year">This Year</option><option value="yesterday">Yesterday</option><option value="previousWeek">Previous Week</option><option value="previousMonth">Previous Month</option><option value="previousQuarter">Previous Quarter</option><option value="previousYear">Previous Year</option><option value="custom">Custom Range</option></select></div>
   <button type="button" class="sub-soft-btn" id="refresh" title="Refresh dashboard" aria-label="Refresh dashboard"><i class="ri-refresh-line"></i></button>
  </div></div>
 </div>
 <div id="customDates" hidden><label>From Date <input type="date" id="fromDate"></label><label>To Date <input type="date" id="toDate"></label><button type="button" class="btn" id="applyDates">Apply</button></div>
</div>
<div class="wrapper_inner p-3"><div class="load-status" role="status"><span id="loadMessage">Loading team data…</span></div>
<div class="head"><p id="asOf"></p><span id="focusChip"></span></div>
<span id="userName" hidden></span><span id="userAvatar" hidden></span>
  <div id="dashboardContent" hidden>
  <section class="kpis" id="kpis" aria-label="Team summary"></section>

  <section class="card">
    <div class="ch"><h2>Team workload</h2><span class="hint">Click an employee to focus the whole dashboard on them. Click a column to sort.</span></div>
    <div class="scroll-x"><table class="wl" id="wl"></table></div>
    <p class="def">Load = remaining assigned hours ÷ employee capacity for the next 10 working dates, after lunch, approved leave, holidays and weekoff. The tick marks 100% capacity. Missing timesheet means recorded hours are below the expected capacity for a past workday this week.</p>
  </section>

  <div class="row">
    <section class="card">
      <div class="ch"><h2>Load vs capacity</h2><span class="hint" id="teamLoadHint"></span></div>
      <div class="loads" id="loads"></div>
      <div class="scale" id="loadScale"></div>
    </section>
    <section class="card">
      <div class="ch"><h2>Suggested reassignments</h2><span class="hint">Review suggestions in Assign Project</span></div>
      <div class="moves" id="moves"></div>
    </section>
  </div>

  <div class="row">
    <section class="card">
      <div class="ch"><h2>Needs attention</h2><span class="hint">Overdue or due within 3 days</span></div>
      <div class="att" id="att"></div>
    </section>
    <section class="card">
      <div class="ch"><h2>Timesheets this week</h2><span class="hint" id="tsHint"></span></div>
      <div class="scroll-x"><table class="ts" id="ts"></table></div>
      <div class="legend"><span><i style="background:color-mix(in srgb,var(--accent) 45%,var(--sunk))"></i>Hours logged</span><span><i style="background:var(--bad-soft);outline:1px solid var(--bad)"></i>Below expected capacity</span><span><i style="background:repeating-linear-gradient(135deg,var(--sunk) 0 3px,var(--surface) 3px 6px);outline:1px solid var(--line)"></i>Leave</span></div>
    </section>
  </div>

  <div class="row3">
    <section class="card">
      <div class="ch"><h2>Status and priority</h2><span class="hint" id="stTotal"></span></div>
      <div class="stack" id="stStack"></div>
      <div class="legend" id="stLegend"></div>
      <div class="scroll-x"><table class="mx" id="matrix"></table></div>
    </section>
    <section class="card">
      <div class="ch"><h2>Team efficiency</h2><span class="hint">Target line at 85%</span></div>
      <div class="split" id="eff"></div>
    </section>
    <section class="card">
      <div class="ch"><h2>Quality</h2><span class="hint">Reopened work and issues</span></div>
      <div id="quality"></div>
    </section>
  </div>

  <section class="card" id="taskCard">
    <div class="ch"><h2 id="taskTitle">Team tasks</h2>
      <div class="tools"><input type="search" id="q" placeholder="Search task or ID" aria-label="Search tasks"><span id="chips" class="tools"></span></div></div>
    <div class="scroll-x"><table class="t" id="tbl"></table></div><div id="paging"></div>
  </section>
</div>
</div>
</div>
</asp:Content>
