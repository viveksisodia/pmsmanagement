<%@ Page Title="PMS User Dashboard" Language="C#" ResponseEncoding="utf-8" CodeFile="default.aspx.cs" Inherits="newuserdashboard" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" %>
<asp:Content ID="DashboardHead" ContentPlaceHolderID="head" Runat="Server">
<style>
/* Layout: app bar → filter row → KPI strip → 2-col analytic rows (wide | narrow) → task table. One dataset feeds every card. */
:root{
  --bg:#f4f3f7; --surface:#ffffff; --sunk:#efedf3; --line:#e1dde8; --fg:#1d1a24; --muted:#6a6475;
  --accent:#5a2a7a; --accent-soft:#efe6f5;
  --s-open:#3b7ddd; --s-prog:#d99a1e; --s-reop:#8a5cd6; --s-done:#2e9a5c;
  --p-high:#d1453b; --p-med:#d99a1e; --p-low:#5f8f7a;
  --bad:#c93a31; --bad-soft:#fbe6e4; --warn:#a86a00; --warn-soft:#fbf0d9; --ok:#23804b; --ok-soft:#e1f2e7;
  --f-display:"Sora",system-ui,sans-serif; --f-body:"Figtree",system-ui,-apple-system,"Segoe UI",sans-serif;
  --f-mono:"JetBrains Mono",ui-monospace,Menlo,monospace;
}
@media (prefers-color-scheme:dark){:root:not([data-theme="light"]){
  --bg:#141218; --surface:#1d1a23; --sunk:#16141b; --line:#2f2b38; --fg:#ece8f2; --muted:#a39cb0;
  --accent:#c79be6; --accent-soft:#2e2238;
  --s-open:#6aa3f2; --s-prog:#e9b44c; --s-reop:#ad8bef; --s-done:#55c487;
  --p-high:#f0756b; --p-med:#e9b44c; --p-low:#86b8a2;
  --bad:#ff8077; --bad-soft:#3a1d1b; --warn:#e9b44c; --warn-soft:#3a2d12; --ok:#55c487; --ok-soft:#173022; color-scheme:dark}}
:root[data-theme="dark"]{
  --bg:#141218; --surface:#1d1a23; --sunk:#16141b; --line:#2f2b38; --fg:#ece8f2; --muted:#a39cb0;
  --accent:#c79be6; --accent-soft:#2e2238;
  --s-open:#6aa3f2; --s-prog:#e9b44c; --s-reop:#ad8bef; --s-done:#55c487;
  --p-high:#f0756b; --p-med:#e9b44c; --p-low:#86b8a2;
  --bad:#ff8077; --bad-soft:#3a1d1b; --warn:#e9b44c; --warn-soft:#3a2d12; --ok:#55c487; --ok-soft:#173022; color-scheme:dark}

*{box-sizing:border-box}
body{background:var(--bg);color:var(--fg);font-family:var(--f-body);font-size:14px;line-height:1.45}
button,select,input{font:inherit;color:inherit}
button{cursor:pointer}
:focus-visible{outline:2px solid var(--accent);outline-offset:2px}
.num{font-family:var(--f-mono);font-variant-numeric:tabular-nums}
.muted{color:var(--muted)}
h1,h2,h3{margin:0;text-wrap:balance}

/* app bar */
.appbar{background:var(--surface);border-bottom:1px solid var(--line);padding-inline:20px}
.appbar .in{max-width:1320px;margin:0 auto;display:flex;align-items:center;gap:20px;flex-wrap:wrap;padding-block:10px 0}
.brand{display:flex;align-items:center;gap:10px;font-weight:700;font-size:15px}
.tabs{display:flex;gap:4px;margin-left:12px;flex:1;min-width:0;overflow-x:auto}
.tabs a{padding:10px 12px;color:var(--muted);text-decoration:none;border-bottom:2px solid transparent;white-space:nowrap;font-weight:500}
.tabs a.on{color:var(--accent);border-bottom-color:var(--accent)}
.user{display:flex;align-items:center;gap:10px;padding-bottom:8px}
.avatar{width:32px;height:32px;border-radius:50%;background:var(--accent-soft);color:var(--accent);display:grid;place-items:center;font-weight:700;font-size:13px}
.user small{display:block;color:var(--muted);font-size:12px}

.wrap{max-width:1320px;margin:0 auto;padding-inline:20px;padding-block:20px 56px;display:grid;gap:16px}
.head{display:flex;flex-wrap:wrap;gap:12px 20px;align-items:flex-end;justify-content:space-between}
.head h1{font-family:var(--f-display);font-size:22px;font-weight:600}
.head p{margin:2px 0 0;color:var(--muted)}
.filters{display:flex;gap:8px;flex-wrap:wrap;align-items:center}
.filters label{font-size:12px;color:var(--muted)}
select{background:var(--surface);border:1px solid var(--line);border-radius:8px;padding:7px 10px}

/* KPI strip */
.kpis{display:grid;grid-template-columns:repeat(6,minmax(0,1fr));background:var(--surface);border:1px solid var(--line);border-radius:12px;overflow:hidden}
.kpi{padding:14px 16px;border-left:1px solid var(--line);display:grid;gap:2px;text-align:left;background:none;border-top:0;border-right:0;border-bottom:3px solid transparent;min-width:0}
.kpi:first-child{border-left:0}
button.kpi:hover{background:var(--sunk)}
.kpi.on{border-bottom-color:var(--accent);background:var(--accent-soft)}
.kpi .l{font-size:12px;color:var(--muted);display:flex;align-items:center;gap:6px}
.kpi .l i{width:8px;height:8px;border-radius:2px;display:inline-block}
.kpi .v{font-family:var(--f-display);font-size:26px;font-weight:600;line-height:1.15}
.kpi .n{font-size:12px;color:var(--muted)}
.kpi.alert .v{color:var(--bad)}

.row{display:grid;gap:16px;grid-template-columns:minmax(0,1.55fr) minmax(0,1fr)}
.row3{display:grid;gap:16px;grid-template-columns:repeat(3,minmax(0,1fr))}
.card{background:var(--surface);border:1px solid var(--line);border-radius:12px;padding:16px 18px;display:grid;gap:14px;align-content:start;min-width:0}
.ch{display:flex;justify-content:space-between;align-items:baseline;gap:10px;flex-wrap:wrap}
.ch h2{font-size:15px;font-weight:600}
.ch .hint{font-size:12px;color:var(--muted)}
.def{font-size:12px;color:var(--muted);margin:0}

/* stacked bar */
.stack{display:flex;height:14px;border-radius:7px;overflow:hidden;background:var(--sunk);gap:2px}
.stack span{display:block;height:100%}
.legend{display:flex;flex-wrap:wrap;gap:6px 16px;font-size:13px}
.legend span{display:flex;align-items:center;gap:6px}
.legend i{width:10px;height:10px;border-radius:3px}
.legend b{font-family:var(--f-mono);font-weight:500}

/* matrix */
.mx{width:100%;border-collapse:separate;border-spacing:4px;font-size:13px}
.mx th{font-weight:500;color:var(--muted);font-size:12px;text-align:left;padding:2px 4px}
.mx td{text-align:center;border-radius:6px;padding:9px 4px;font-family:var(--f-mono);font-variant-numeric:tabular-nums}
.mx td.tot{color:var(--muted);background:none}
.pdot{display:inline-block;width:8px;height:8px;border-radius:50%;margin-right:6px}

/* attention list */
.att{display:grid}
.att .it{display:grid;grid-template-columns:minmax(0,1fr) auto;gap:2px 10px;padding:9px 0;border-top:1px solid var(--line)}
.att .it:first-child{border-top:0}
.att .t{font-weight:500;overflow-wrap:anywhere}
.att .m{font-size:12px;color:var(--muted)}
.pill{font-size:11px;font-weight:600;padding:3px 8px;border-radius:999px;white-space:nowrap;align-self:start}
.pill.bad{background:var(--bad-soft);color:var(--bad)} .pill.warn{background:var(--warn-soft);color:var(--warn)} .pill.ok{background:var(--ok-soft);color:var(--ok)}
.empty{color:var(--muted);font-size:13px;padding:12px 0}

/* meters */
.meter{display:grid;gap:6px}
.meter .top{display:flex;justify-content:space-between;align-items:baseline;gap:8px}
.meter .big{font-family:var(--f-display);font-size:28px;font-weight:600}
.track{position:relative;height:10px;background:var(--sunk);border-radius:5px}
.track .fill{height:100%;border-radius:5px}
.track .tgt{position:absolute;top:-4px;bottom:-4px;width:2px;background:var(--fg);opacity:.55}
.split{display:grid;gap:18px}

/* tags */
.hbars{display:grid;gap:8px}
.hb{display:grid;grid-template-columns:90px minmax(0,1fr) 32px;gap:10px;align-items:center;font-size:13px}
.hb .b{height:10px;background:var(--sunk);border-radius:5px;overflow:hidden}
.hb .b i{display:block;height:100%;background:var(--accent);opacity:.75;border-radius:5px}

/* table */
.tools{display:flex;gap:6px;flex-wrap:wrap}
.chipbtn{border:1px solid var(--line);background:var(--surface);border-radius:999px;padding:5px 12px;font-size:13px}
.chipbtn.on{background:var(--accent);border-color:var(--accent);color:var(--surface)}
.tbl{overflow-x:auto}
table.t{width:100%;border-collapse:collapse;font-size:13px;min-width:720px}
.t th{text-align:left;font-weight:500;color:var(--muted);font-size:12px;padding:8px 10px;border-bottom:1px solid var(--line);white-space:nowrap}
.t td{padding:9px 10px;border-bottom:1px solid var(--line);vertical-align:top}
.t tr:last-child td{border-bottom:0}
.st{display:inline-flex;align-items:center;gap:6px;white-space:nowrap}
.st i{width:8px;height:8px;border-radius:50%}
.over{color:var(--bad);font-weight:600}
svg text{font-family:var(--f-mono);font-size:11px;fill:var(--muted)}

@media (max-width:1100px){.kpis{grid-template-columns:repeat(3,minmax(0,1fr))}.kpi:nth-child(4){border-left:0}.kpi:nth-child(n+4){border-top:1px solid var(--line)}.row3{grid-template-columns:minmax(0,1fr) minmax(0,1fr)}}
@media (max-width:860px){.row,.row3{grid-template-columns:minmax(0,1fr)}}
@media (max-width:520px){.wrap,.appbar{padding-inline:16px}.kpis{grid-template-columns:repeat(2,minmax(0,1fr))}.kpi{border-left:0!important;border-top:1px solid var(--line)}.kpi:nth-child(-n+2){border-top:0}.kpi:nth-child(even){border-left:1px solid var(--line)!important}.user small{display:none}}
@media (prefers-reduced-motion:reduce){*{transition:none!important}}


        body{background:#f1f5f7;color:#202633;font-family:Arial,sans-serif}.ud-header{height:56px;background:#fff;border-bottom:1px solid #e6e9ed;display:flex;align-items:center;justify-content:space-between;padding:0 18px}.ud-brand{display:flex;align-items:center;gap:10px;color:#19242f;font-size:15px;font-weight:600}.ud-brand img{width:25px;height:25px;object-fit:contain}.ud-user{display:flex;align-items:center;gap:9px;font-size:12px}.ud-user img{width:32px;height:32px;border-radius:50%;object-fit:cover}.ud-nav{height:38px;background:#fff;border-bottom:1px solid #e3e7eb;display:flex;align-items:stretch;gap:22px;padding:0 18px}.ud-nav a{display:flex;align-items:center;color:#303a45;text-decoration:none;font-size:12px;padding:0 2px}.ud-nav a.active{color:#5928a5;border-bottom:2px solid #5928a5}.ud-main{padding:10px 14px 18px}.ud-toolbar{display:flex;justify-content:space-between;align-items:center;gap:12px;margin-bottom:10px}.ud-title{font-size:17px;font-weight:600;margin:0}.ud-kpis{display:flex;align-items:center;gap:14px;flex-wrap:wrap}.ud-kpi{display:flex;align-items:center;gap:7px;min-width:90px;color:#343d48;font-size:11px}.ud-kpi i{font-size:16px}.ud-kpi strong{display:block;font-size:13px;color:#111}.ud-date{font-size:12px;color:#3d4650;white-space:nowrap;display:flex;align-items:center;gap:7px;position:relative}.ud-range-trigger{height:32px;min-width:206px;padding:4px 9px;border:1px solid #d7dce2;border-radius:4px;background:#fff;color:#3d4650;font-size:11px;text-align:left;display:flex;align-items:center;justify-content:space-between;gap:9px}.ud-range-popup{position:absolute;z-index:20;top:38px;right:0;width:142px;padding:8px;background:#fff;border:1px solid #dfe3e8;border-radius:5px;box-shadow:0 6px 20px rgba(28,39,50,.16);white-space:normal;display:block}.ud-range-popup.custom-open{width:342px;display:grid;grid-template-columns:132px minmax(0,1fr);gap:8px}.ud-range-preset-list{display:flex;flex-direction:column;align-items:stretch}.ud-range-preset{border:0;border-radius:0;background:#fff;color:#202633;padding:5px 5px;text-align:left;font-size:11px;line-height:16px}.ud-range-preset:hover,.ud-range-preset.active{background:#0788c9;color:#fff}.ud-range-details{padding:1px 0 0 0}.ud-range-custom label{display:block;color:#111;font-size:13px;font-weight:600;margin:0 0 4px}.ud-range-custom input{width:100%;height:34px;padding:4px 8px;margin:0 0 8px;border:1px solid #cfd5dc;border-radius:4px;background:#fff;font-size:12px}.ud-range-apply{width:100%;height:34px;padding:4px 12px;border:0;border-radius:3px;background:#42133f;color:#fff;font-size:12px}.ud-range-error{color:#c0392b;font-size:11px;margin-top:7px}.ud-grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:8px;position:relative;transition:opacity .18s ease}.ud-refresh-overlay{position:absolute;inset:0;z-index:12;display:flex;align-items:center;justify-content:center;background:rgba(245,248,250,.68);border-radius:6px;backdrop-filter:blur(1px);animation:ud-fade-in .18s ease}.ud-refresh-indicator{display:flex;align-items:center;gap:10px;padding:10px 15px;border:1px solid #e3e8ed;border-radius:6px;background:#fff;color:#3d4650;font-size:12px;box-shadow:0 3px 12px rgba(32,38,51,.08)}.ud-refresh-spinner{width:17px;height:17px;border:2px solid #e6dcf1;border-top-color:#5928a5;border-radius:50%;animation:ud-spin .7s linear infinite}@keyframes ud-spin{to{transform:rotate(360deg)}}@keyframes ud-fade-in{from{opacity:0}to{opacity:1}}@media(prefers-reduced-motion:reduce){.ud-refresh-spinner{animation:none}.ud-refresh-overlay{animation:none}.ud-grid{transition:none}}.ud-card{background:#fff;border:1px solid #edf0f2;border-radius:5px;min-height:148px;padding:10px;position:relative;overflow:hidden}.ud-card.tall{min-height:240px}.ud-card-title{font-size:12px;color:#111;margin:0 0 8px}.ud-status-card{height:148px}.ud-total{position:absolute;left:11px;top:34px;font-size:24px;font-weight:700;color:#111}.ud-total small{display:block;font-size:10px;font-weight:400}.ud-legend{position:absolute;left:11px;bottom:10px;display:flex;flex-direction:column;gap:4px;font-size:10px;color:#333;z-index:2}.ud-legend-line{display:flex;align-items:center;gap:5px;white-space:nowrap}.ud-dot{width:5px;height:5px;border-radius:50%;flex:0 0 5px}.ud-bubbles{position:absolute;right:11px;top:15px;width:58%;height:120px}.ud-bubble{position:absolute;border-radius:50%;display:flex;align-items:center;justify-content:center;font-weight:600}.ud-ring-area{height:calc(100% - 18px);display:flex;align-items:center;justify-content:center;padding-left:50px}.ud-ring{width:116px;height:116px;border-radius:50%;position:relative;flex:none}.ud-ring:after{content:"";position:absolute;inset:17px;border-radius:50%;background:#fff}.ud-ring-center{position:absolute;inset:0;z-index:1;display:flex;flex-direction:column;align-items:center;justify-content:center;font-size:19px;font-weight:700;color:#101820}.ud-ring-center small{font-size:9px;font-weight:400}.ud-bars{height:calc(100% - 18px);display:flex;align-items:stretch;padding:26px 6px 8px 26px;gap:8px;position:relative}.ud-hours-legend{position:absolute;top:0;right:2px;display:flex;align-items:center;gap:10px;font-size:9px}.ud-hours-legend .ud-dot{width:5px;height:5px}.ud-hours-filter{height:23px;width:82px;padding:1px 18px 1px 5px;border:1px solid #d7dce2;border-radius:3px;background:#fff;font-size:10px;color:#3d4650}.ud-hours-overlay{position:absolute;inset:22px 0 0;z-index:5;display:flex;align-items:center;justify-content:center;background:rgba(255,255,255,.76);animation:ud-fade-in .16s ease}.ud-hours-indicator{display:flex;align-items:center;gap:7px;padding:7px 10px;border:1px solid #e3e8ed;border-radius:5px;background:#fff;color:#3d4650;font-size:10px;box-shadow:0 2px 8px rgba(32,38,51,.07)}.ud-hours-spinner{width:13px;height:13px;border:2px solid #e6dcf1;border-top-color:#5928a5;border-radius:50%;animation:ud-spin .7s linear infinite}.ud-hours-error{position:absolute;left:30px;right:5px;bottom:0;z-index:6;text-align:center;color:#b33a3a;background:#fff;padding:3px;font-size:9px}.ud-axis{position:absolute;left:0;top:25px;bottom:8px;display:flex;flex-direction:column;justify-content:space-between;font-size:8px;color:#59616a}.ud-bar-day{flex:1;display:flex;flex-direction:column;justify-content:flex-end;align-items:center;gap:4px;position:relative;border-bottom:1px solid #edf0f4}.ud-bar-pair{height:100%;display:flex;align-items:flex-end;gap:2px;width:100%;justify-content:center}.ud-bar{width:min(40%,13px);min-width:5px;min-height:0;border-radius:1px 1px 0 0}.ud-day-label{font-size:8px;color:#59616a;position:absolute;bottom:-13px}.ud-gauge-wrap{height:190px;display:flex;justify-content:center;align-items:flex-end;position:relative}.ud-gauge{width:180px;height:90px;position:relative;overflow:hidden}.ud-gauge-track{position:absolute;width:180px;height:180px;border-radius:50%;background:conic-gradient(from 270deg,#ff999a 0 25%,#ffe696 25% 50%,#a5c7ff 50% 75%,#c5f8c8 75% 100%);top:0;left:0}.ud-gauge-track:after{content:"";position:absolute;inset:15px;border-radius:50%;background:#fff}.ud-gauge-needle{position:absolute;bottom:0;left:50%;height:79px;width:2px;background:#444;transform-origin:bottom center;z-index:2}.ud-gauge-needle:after{content:"";position:absolute;bottom:-4px;left:-4px;width:10px;height:10px;background:#222;border-radius:50%}.ud-gauge-value{position:absolute;bottom:-1px;left:0;right:0;text-align:center;font-size:15px;font-weight:700;z-index:3;color:#101820}.ud-table{font-size:10px;margin-bottom:0}.ud-table th{background:#edf2f5;font-weight:500}.ud-table td,.ud-table th{padding:4px 6px!important}.ud-tag{font-size:9px;color:#fff;padding:2px 6px;border-radius:3px}.ud-error{margin:12px}.ud-loading{padding:30px;text-align:center;color:#68717a}.ud-empty{text-align:center;color:#84909b;padding:22px;font-size:12px}
        @media(max-width:900px){.ud-grid{grid-template-columns:repeat(2,minmax(0,1fr))}.ud-toolbar{align-items:flex-start;flex-direction:column}.ud-kpis{gap:9px}}
        @media(max-width:600px){.ud-grid{grid-template-columns:1fr}.ud-main{padding:8px}.ud-header{padding:0 10px}.ud-nav{padding:0 10px;gap:15px}.ud-card.tall{min-height:220px}.ud-range-popup{width:min(94vw,142px);right:-5px}.ud-range-popup.custom-open{width:min(94vw,342px);display:grid;grid-template-columns:122px minmax(0,1fr);gap:6px}.ud-range-custom input{width:100%}}
    .ud-card-heading{display:flex;align-items:center;justify-content:space-between;margin:0 0 8px;position:relative;z-index:3}.ud-card-heading .ud-card-title{margin:0}.ud-report-link{font-size:9px;color:#5928a5;text-decoration:none;padding:3px 7px;border:1px solid #e1d7ee;border-radius:3px;background:#fff}.ud-report-link:hover{background:#f5effc;color:#42133f}
        /* Reserve space for day labels inside the Task Hours card. */
        .ud-bars{height:190px;box-sizing:border-box;padding:28px 6px 25px 38px;gap:8px;min-width:0}
        .ud-axis{top:28px;bottom:25px;font-size:10px;line-height:12px;justify-content:space-between}
        .ud-axis span{position:absolute;left:0;transform:translateY(-50%);font-size:9px;line-height:9px}
        .ud-bar-day{min-width:0;gap:0}
        .ud-day-label{bottom:-20px;font-size:10px;line-height:14px;white-space:nowrap;max-width:100%;overflow:hidden;text-overflow:ellipsis}
        .ud-hours-legend .ud-dot{display:inline-block;margin-right:3px}
    
body{background:#f4f3f7}.nud{--bg:#f4f3f7;--surface:#fff;--sunk:#efedf3;--line:#e1dde8;--fg:#1d1a24;--muted:#6a6475;--accent:#5a2a7a;--accent-soft:#efe6f5;color:var(--fg);font-family:Arial,sans-serif}.nud .wrap{max-width:1320px}.nud .ud-bars{height:280px;padding-top:32px;padding-bottom:30px}.nud .ud-bar{width:min(40%,34px)}.nud .ud-axis{top:32px;bottom:30px}.nud .ud-hours-legend{font-size:12px}.nud .att{max-height:310px;overflow:auto}.nud .hbars{max-height:180px;overflow:auto}.nud .tbl{max-height:430px;overflow:auto}.nud .t th{position:sticky;top:0;background:var(--surface);z-index:1}.nud .refresh{position:sticky;top:8px;z-index:10;background:#efe6f5;color:#5a2a7a;padding:10px;border-radius:8px}.nud .ud-range-popup{z-index:30}.nud .kpi{color:var(--fg)}.nud .kpi .l{display:block}.nud .ud-range-trigger{font-size:12px}.nud .ch a{color:var(--accent);font-size:12px}.nud .meter .top{align-items:center}.nud .ring{width:88px;height:88px;border-radius:50%;display:grid;place-items:center;flex:none}.nud .ring>div{background:var(--surface);width:65px;height:65px;border-radius:50%;display:grid;place-content:center;text-align:center}.nud .ring small{font-size:10px;color:var(--muted)}.nud .issues-content{display:flex;align-items:center;gap:20px}
.nud .row{display:grid;margin:0}.nud .kpis{grid-template-columns:repeat(auto-fit,minmax(140px,1fr))}.nud .ud-bar-pair{flex:1;min-height:0}.nud .tgt{pointer-events:none}
.nud .tools button.chipbtn.on{background:#5a2a7a!important;border-color:#5a2a7a!important;color:#fff!important;font-weight:600!important;box-shadow:0 0 0 1px #5a2a7a!important}
.nud .tools button.chipbtn:not(.on){background:#fff!important;color:#1d1a24!important;border-color:#e1dde8!important}
.nud .kpis button.kpi.nud-selected{background:#efe6f5!important;border-bottom:3px solid #5a2a7a!important;box-shadow:none!important}.nud .kpis button.kpi:not(.nud-selected){background:#fff!important;border-bottom:3px solid transparent!important}

/* Issue chart: old dashboard layout; statuses and colours come from the issue-status table. */
.nud .nud-issues-card .issues-content{justify-content:center;min-height:124px;padding:4px 0 8px}.nud .nud-issues-card .ring{width:116px;height:116px}.nud .nud-issues-card .ring>div{width:82px;height:82px}.nud .nud-issues-card .ring b{font-size:20px}.nud .nud-issues-card .ring small{font-size:9px}.nud .nud-issues-legend{display:flex;flex-direction:column;align-items:flex-start;gap:5px;margin-top:5px;font-size:10px}
</style>
<script src="app/userdashboard.main.js?v=20261009-1720"></script><script src="app/userdashboard.service.js?v=20261009-1720"></script>
</asp:Content>
<asp:Content ID="DashboardContent" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main-wrapper nud" ng-app="pmsUserDashboardApp" ng-controller="UserDashboardController as vm" ng-cloak>
<main class="wrap">
<div class="head"><div><h1>{{vm.greeting}}, {{vm.data.userName || 'User'}}</h1><p>{{vm.todayLabel}}</p></div><div class="filters"><label for="nudProject">Project</label><select id="nudProject" ng-model="vm.projectId" ng-options="p.id as p.name for p in vm.projects" ng-change="vm.changeProject()"></select>                <div class="ud-date">
                    <i class="ri-calendar-line"></i>
                    <button type="button" class="ud-range-trigger" ng-click="vm.toggleRangeMenu()" aria-haspopup="true" aria-expanded="{{vm.rangeMenuOpen}}">
                        <span>{{vm.rangeDisplay}}</span><i class="ri-arrow-down-s-line"></i>
                    </button>
                    <div class="ud-range-popup" ng-if="vm.rangeMenuOpen" ng-class="{'custom-open':vm.customMode}" ng-click="$event.stopPropagation()">
                        <div class="ud-range-preset-list">
                            <button type="button" class="ud-range-preset" ng-repeat="preset in vm.presets" ng-class="{'active':vm.selectedPreset===preset}" ng-click="vm.selectPreset(preset)">{{preset}}</button>
                        </div>
                        <div class="ud-range-details" ng-if="vm.customMode">
                            <div class="ud-range-custom">
                                <label for="udFromDate">From Date</label><input id="udFromDate" type="date" ng-model="vm.customFromDate" ng-click="vm.openCalendar($event)" />
                                <label for="udToDate">To Date</label><input id="udToDate" type="date" ng-model="vm.customToDate" ng-click="vm.openCalendar($event)" />
                                <button type="button" class="ud-range-apply" ng-click="vm.applyCustomRange()">Apply</button>
                            </div>
                            <div class="ud-range-error" ng-if="vm.rangeError">{{vm.rangeError}}</div>
                        </div>
                    </div>
                </div>
</div></div>
<div class="ud-loading" ng-if="vm.loading && !vm.hasLoaded">Loading dashboard...</div><div class="alert alert-danger" ng-if="vm.error">{{vm.error}}</div><div class="refresh" ng-if="vm.loading && vm.hasLoaded" role="status">Updating dashboard...</div>
<div ng-if="vm.hasLoaded" style="display:grid;gap:16px">
<section class="kpis" aria-label="Task summary" >
<button type="button" class="kpi" nud-selected="vm.taskFilter==='All'" ng-class="{'on':vm.taskFilter==='All'}" ng-click="vm.selectTaskFilter('All',true)"><span class="l">Total tasks</span><span class="v">{{vm.data.taskStatus.total}}</span><span class="n">All statuses</span></button>
<button type="button" class="kpi" ng-repeat="s in vm.data.taskStatus.items" nud-selected="vm.taskFilter===s.name" ng-class="{'on':vm.taskFilter===s.name}" ng-click="vm.selectTaskFilter(s.name,true)"><span class="l"><i ng-style="{'background':s.color}"></i> {{s.name}}</span><span class="v">{{s.count}}</span><span class="n">{{s.percent}}% of total</span></button>
<button type="button" class="kpi alert" nud-selected="vm.taskFilter==='Overdue'" ng-class="{'on':vm.taskFilter==='Overdue'}" ng-click="vm.selectTaskFilter('Overdue',true)"><span class="l">Overdue</span><span class="v">{{vm.overdueCount}}</span><span class="n">Past due, not completed</span></button></section>
<div class="row"><article class="card"><div class="ch"><h2>Status and priority</h2><span class="hint">{{vm.data.taskStatus.total}} tasks <a ng-href="{{vm.reportUrl('status')}}" target="_blank">View Report</a></span></div>
<div class="stack"><span ng-repeat="s in vm.data.taskStatus.items" ng-style="{'width':s.percent+'%','background':s.color}"></span></div><div class="legend"><span ng-repeat="s in vm.data.taskStatus.items"><i ng-style="{'background':s.color}"></i>{{s.name}} <b>{{s.count}}</b> <small class="muted">{{s.percent}}%</small></span></div>
<div style="overflow:auto"><table class="mx"><thead><tr><th>Priority</th><th ng-repeat="s in vm.data.taskStatus.items">{{s.name}}</th><th>Total</th></tr></thead><tbody><tr ng-repeat="p in vm.matrix"><th><span class="pdot" ng-style="{'background':p.color}"></span>{{p.name}}</th><td ng-repeat="c in p.cells" ng-style="{'background':c.count ? p.color : '#efedf3','color':'#111'}">{{c.count}}</td><td class="tot">{{p.total}} - {{vm.percent(p.total,vm.data.taskStatus.total)}}%</td></tr></tbody></table><p class="empty" ng-if="!vm.matrix.length">No assigned tasks in this period.</p></div></article>
<article class="card"><div class="ch"><h2>Needs attention</h2><span class="hint">Overdue or due within 3 days</span></div><div class="att"><div class="it" ng-repeat="t in vm.attention"><span class="t">{{t.title}}</span><span class="pill" ng-class="vm.overdue(t)?'bad':'warn'">{{vm.dueLabel(t)}}</span><span class="m">{{t.serialNo || '—'}} - {{t.project}} - {{t.priority}} - {{t.status}}</span></div><div class="empty" ng-if="!vm.attention.length">No tasks need attention.</div></div></article></div>
<div class="row"><article class="card"><div class="ch"><h2>{{(vm.hoursPeriod==='week' || (vm.hoursPeriod==='all' && vm.selectedPreset==='This Week')) ? 'Hours this week' : vm.hoursPeriod==='today' ? 'Hours today' : 'Task hours'}}</h2><span class="hint num">{{vm.hoursSummary()}}</span><a ng-href="{{vm.reportUrl('hours')}}" target="_blank">View Report</a></div>
<div class="ud-bars"><div class="ud-hours-legend"><span><i class="ud-dot" style="background:#e1dde8"></i> Planned</span><span><i class="ud-dot" style="background:#5a2a7a"></i> Spent</span><select class="ud-hours-filter" aria-label="Task hours period" ng-model="vm.hoursPeriod" ng-options="period.value as period.label for period in vm.hoursPeriods" ng-change="vm.loadTaskHours(vm.hoursPeriod)"></select></div><div class="ud-axis"><span ng-repeat="tick in vm.hoursAxisTicks track by $index" ng-style="{'top':tick.top+'%'}">{{tick.label}}</span></div><div class="ud-bar-day" ng-repeat="d in vm.data.hours.days"><div class="ud-bar-pair"><div class="ud-bar" ud-hours-tooltip data-bs-title="{{d.assignedTooltip}}" ng-style="{'height':vm.hoursBarHeight(d.assigned),'background':'#e1dde8'}"></div><div class="ud-bar" ud-hours-tooltip data-bs-title="{{d.spentTooltip}}" ng-style="{'height':vm.hoursBarHeight(d.spent),'background':'#5a2a7a'}"></div></div><span class="ud-day-label">{{d.day}}</span></div><div class="ud-hours-overlay" ng-if="vm.hoursLoading"><div class="ud-hours-indicator">Updating hours...</div></div><div class="ud-hours-error" ng-if="vm.hoursError">{{vm.hoursError}}</div></div><p class="def">Planned hours follow your shift, lunch, week off, holidays, and approved leave. Spent hours come from task timers.</p></article>
<article class="card"><div class="ch"><h2>Efficiency</h2><span class="hint">Target line at {{vm.data.taskEfficiency || 0}}%</span></div><div class="split"><div class="meter"><div class="top"><span>Task efficiency</span><svg width="110" height="36" viewBox="0 0 110 36" role="img" aria-label="Task efficiency six-week trend"><title>{{vm.efficiencyTrendTitle('task')}}</title><polyline ng-attr-points="{{vm.efficiencyPoints('task')}}" fill="none" stroke="#6a6475" stroke-width="1.5"></polyline><circle ng-repeat="point in vm.efficiencySparkline('task')" ng-attr-cx="{{point.x}}" ng-attr-cy="{{point.y}}" r="2" fill="#23804b"><title>{{point.week}}: {{point.value}}%</title></circle></svg><a ng-href="{{vm.reportUrl('taskEfficiency')}}" target="_blank">View Report</a></div><div class="big" style="color:var(--ok)">{{vm.data.taskEfficiency}}% <small style="font-size:11px;font-weight:400;color:var(--muted)">6-week trend</small></div><div class="track"><div class="fill" style="background:var(--ok)" ng-style="{'width':vm.meterWidth(vm.data.taskEfficiency)}"></div><span class="tgt" ng-style="{'left':vm.meterWidth(vm.data.taskEfficiency)}"></span></div><p class="def">{{vm.taskEfficiencyDetails}}</p></div><div class="meter"><div class="top"><span>Time efficiency</span><svg width="110" height="36" viewBox="0 0 110 36" role="img" aria-label="Time efficiency six-week trend"><title>{{vm.efficiencyTrendTitle('time')}}</title><polyline ng-attr-points="{{vm.efficiencyPoints('time')}}" fill="none" stroke="#6a6475" stroke-width="1.5"></polyline><circle ng-repeat="point in vm.efficiencySparkline('time')" ng-attr-cx="{{point.x}}" ng-attr-cy="{{point.y}}" r="2" fill="#23804b"><title>{{point.week}}: {{point.value}}%</title></circle></svg><a ng-href="{{vm.reportUrl('timeEfficiency')}}" target="_blank">View Report</a></div><div class="big" style="color:var(--ok)">{{vm.data.timeEfficiency}}% <small style="font-size:11px;font-weight:400;color:var(--muted)">6-week trend</small></div><div class="track"><div class="fill" style="background:var(--ok)" ng-style="{'width':vm.meterWidth(vm.data.timeEfficiency)}"></div><span class="tgt" style="left:85%"></span></div><p class="def">{{vm.hourNumber(vm.completedPlanned)}} h planned vs {{vm.hourNumber(vm.completedSpent)}} h spent on completed tasks. Above 100% means work took less time than planned.</p></div></div></article></div>
<div class="row3"><article class="card"><div class="ch"><h2>Task situation</h2><a ng-href="{{vm.reportUrl('situation')}}" target="_blank">View Report</a></div><div class="stack"><span ng-repeat="s in vm.data.situation.items" ng-style="{'width':s.percent+'%','background':s.color}"></span></div><div class="legend"><span ng-repeat="s in vm.data.situation.items"><i ng-style="{'background':s.color}"></i>{{s.name}} <b>{{s.count}}</b> <small>{{s.percent}}%</small></span></div><p class="def">Situation labels from the database for assigned tasks.</p></article>
<article class="card nud-issues-card"><div class="ch"><h2>Issues</h2><a ng-href="{{vm.reportUrl('issues')}}" target="_blank">View Report</a></div><div class="issues-content"><div class="ring" ng-style="{'background':vm.data.issues.gradient}" aria-label="{{vm.data.issues.total}} total issues"><div><b>{{vm.data.issues.total}}</b><small>Total Issues</small></div></div></div><div class="legend nud-issues-legend"><span ng-repeat="item in vm.data.issues.items"><i ng-style="{'background':item.color}"></i>{{item.name}} ({{item.count}})</span><span class="hint" ng-if="!vm.data.issues.total">No issues in the selected period.</span></div></article>
<article class="card"><div class="ch"><h2>Tasks by tag</h2><a ng-href="{{vm.reportUrl('tag')}}" target="_blank">View Report</a></div><div class="hbars"><div class="hb" ng-repeat="t in vm.data.tags"><span>{{t.name}}</span><div class="b"><i ng-style="{'width':(t.count/vm.maxTag*100)+'%'}"></i></div><span class="num">{{t.count}}</span></div><div class="empty" ng-if="!vm.data.tags.length">No tag data</div></div></article></div>
<article class="card" id="nudMyTasks" style="scroll-margin-top:90px"><div class="ch"><h2>My tasks</h2><div class="tools"><button type="button" class="chipbtn" nud-selected="vm.taskFilter==='All'" ng-class="{'on':vm.taskFilter==='All'}" ng-click="vm.selectTaskFilter('All',false)" aria-pressed="{{vm.taskFilter==='All'}}">All</button><button type="button" class="chipbtn" ng-repeat="s in vm.data.taskStatus.items" nud-selected="vm.taskFilter===s.name" ng-class="{'on':vm.taskFilter===s.name}" ng-click="vm.selectTaskFilter(s.name,false)" aria-pressed="{{vm.taskFilter===s.name}}">{{s.name}}</button><button type="button" class="chipbtn" nud-selected="vm.taskFilter==='Overdue'" ng-class="{'on':vm.taskFilter==='Overdue'}" ng-click="vm.selectTaskFilter('Overdue',false)" aria-pressed="{{vm.taskFilter==='Overdue'}}">Overdue</button></div></div>
<div class="tbl"><table class="t"><thead><tr><th>Serial No.</th><th>Task</th><th>Type</th><th>Project</th><th>Priority</th><th>Status</th><th>Due</th><th>Hours (spent / planned)</th><th>Issues (resolved / total)</th></tr></thead><tbody><tr ng-repeat="t in vm.data.tasks | filter:vm.visibleTask as visibleTasks"><td class="num">{{t.serialNo || '—'}}</td><td>{{t.title}}</td><td>{{t.type}}</td><td>{{t.project}}</td><td><span class="pdot" ng-style="{'background':t.priorityColor}"></span>{{t.priority}}</td><td><span class="st"><i ng-style="{'background':t.statusColor}"></i>{{t.status}}</span></td><td ng-class="{'over':vm.overdue(t)}">{{vm.dueLabel(t)}}</td><td class="num">{{vm.hoursText(t.spent)}} / {{vm.hoursText(t.planned)}}</td><td class="num">{{t.resolved}} / {{t.issues}}</td></tr><tr ng-if="!visibleTasks.length"><td colspan="9" class="empty">No tasks match the selected filters.</td></tr></tbody></table></div></article>
</div></main></div></asp:Content>
