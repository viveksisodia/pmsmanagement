<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="mytaskuser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/pms/angularmaster/jsapp/pmsapp.js?v=<%=DateTime.Now.Ticks %>"></script> 
<script src="/pms/users/app/usertask.main.js?v=<%=DateTime.Now.Ticks %>"></script>
    <script src="/pms/users/app/usertask.service.js?v=<%=DateTime.Now.Ticks %>"></script>
     <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script> 
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
   <link rel="stylesheet" href="/pms/new/assets/css/daterangepicker.css" type="text/css" />
     <style>
        .mytask-project-head-avatar {
            display: inline-flex; position: relative; vertical-align: middle;
            flex: 0 0 28px; width: 28px; height: 28px; margin-right: 6px;
            align-items: center; justify-content: center; border-radius: 50%;
            background: #e8edff; color: #405189; font-size: 11px; font-weight: 600;
        }
        .mytask-project-head-avatar > img {
            width: 28px; height: 28px; border-radius: 50%; object-fit: cover;
        }
        .mytask-project-head-avatar > span {
            display: flex;
            width: 100%; height: 100%; align-items: center; justify-content: center;
        }

        a.mytask-task-page-link, a.mytask-task-page-link span {
            color: #0d6efd !important;
            /*text-decoration: underline !important;*/
            cursor: pointer !important;
            pointer-events: auto !important;
        }

        .mytask-query-highlight {
            outline: 2px solid #e0a800;
            outline-offset: -2px;
        }
        .mytask-query-highlight > td:not([ng-style]),
        .mytask-query-highlight .card-body {
            background-color: #fff3cd !important;
        }

         .tag-popup {
    width: 220px;
    background: #fff;
    border-radius: 10px;
    box-shadow: 0 3px 15px rgba(0,0,0,.18);
    padding: 10px;
}

.tag-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 15px;
}

.tag-header span {
    font-size: 14px;
    color: #7b8794;
}

.tag-actions {
    display: flex;
    gap: 6px;
}

.icon-btn {
    width: 22px;
    height: 22px;
    border-radius: 50%;
    border: 1px solid #d7dce2;
    background: #fff;
    cursor: pointer;
    padding: 0;
}

.icon-btn i {
    font-size: 12px;
}

.tag-list {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
}

.tag-item {
    padding: 7px 14px;
    border-radius: 20px;
    font-size: 13px;
    cursor: pointer;
    display: inline-block;
}

.tag-yellow {
    background: #ffc400;
    color: #000;
}

.tag-red {
    background: #ff0000;
    color: #fff;
}
     </style>
    <style>
        .badge-border {	border-left: 2px solid}
        .badge-label {margin-right:5px;border-radius:20px 4px 4px 20px;	position:relative}
        body {background-color: #f1f5f7;}
        input#daterange {
            border: 0px !important;
            padding: 3px 0 0 0;
            width: 145px;
        }
        input#daterange:focus {
            outline: 0;
        }
.avatar-group .avatar-group-item:nth-child(1) .avatar-title {background:#fef4e4!important;color:#f7b84b!important;}
.avatar-group .avatar-group-item:nth-child(2) .avatar-title {background:#ddfbf9!important;color:#18ddcd!important;}
.avatar-group .avatar-group-item:nth-child(3) .avatar-title {background:#f8f0f7!important;color:#804574!important;}
    </style>
    <style>
      

.spent-time-cell .start-timer{
    cursor:pointer;
    display:inline-flex;
    align-items:center;
    gap:6px;
}

.spent-time-cell .running-timer{
    position:relative;
    display:inline-flex;
    align-items:center;
    gap:6px;
}

.spent-time-cell .timer-text{
    color:#ff6a4d;
    font-weight:500;
}
.spent-time-cell .timer-text-green{
    color:#0cd71a !important;
    font-weight:500;
}

.spent-time-cell .timer-controls{
    position:absolute;
    top:50%;
    
    transform:translateY(-50%);
    margin-left:10px;

    background:#222;
    border-radius:6px;
    padding:3px 4px;

    display:none;
    align-items:center;
    gap:10px;

    box-shadow:0 2px 10px rgba(0,0,0,.2);
    z-index:99;
}

.spent-time-cell .running-timer:hover .timer-controls{
    display:flex;
}

.spent-time-cell .timer-controls button{
    border:0;
    background:none;
    padding:0;
}

.spent-time-cell .timer-controls i{
    font-size:22px;
    cursor:pointer;
}

.loader-overlay {
position: fixed;
top: 0px; left: 0px;
width: 100%; height: 100%;
background: rgba(255,255,255,1);
display: flex;
justify-content: center;
align-items: center;
z-index: 9999;
}

.loader {
border: 8px solid #f3f3f3;
border-top: 2px solid #3498db;
border-radius: 50%;
width: 60px;
height: 60px;
animation: spin 1s linear infinite;
}
    </style>
    
    <style>
        .preview-item {
    width: 30px;
    height: 30px;
    border-radius: 50%;
    object-fit: cover;
    margin: 5px;
    display: inline-block;
    box-shadow: 0 0 2px rgba(0,0,0,0.2);
    text-align:center;
    line-height:30px;
    float: left;
    overflow:hidden;
    transition: transform 0.3s ease;
}
        .preview-item:hover {transform: scale(1.25);}
.preview-item img {width: 30px;height: 30px;border-radius: 50%;}
        .tag-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:8px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}
a.tag-wrp {position:relative;}
.tag-wrp:hover .tag-del {display:block;}
    </style>
    <style>
        .search_box {position:relative;}
.search_box i {position:absolute;top:4px;left:7px;font-size:16px;}
.search_box input.form-control {padding-left:28px;color:#000;border-radius:4px;height:32px;border: 1px solid #ddd !important;}
.search_box input.form-control:focus {border-color:#804574 !important;}
    </style>
    <style>
  .table-fix-wrap {
    width: 100%;
    overflow: auto;
    position: relative;


}

.project-table th,
.project-table td {
    white-space: nowrap;
    vertical-align: middle;
    background: #fff;
    border-style:solid;
}

.project-table thead th {
   
    background: #f8f9fa;
    border: 1px solid #e5e7eb;
}

  
    </style>
    
    <style>

.custom-range-box{
    width:260px;
    float:left;
    padding:10px;
}

.custom-range-box .form-control{
    height:34px;
    font-size:13px;
}

.custom-range-box label{
    display:block;
    margin-bottom:4px;
    font-weight:600;
}

</style>
    <style>
        .project-table td {
    white-space: nowrap;
    vertical-align: middle;
    background: #fff;
    border-style:solid;
}
.project-table thead th {
   
    background: #f8f9fa;
    border: 1px solid #e5e7eb;
}
        .project-table thead th.column-draggable {
    cursor: move;
    user-select: none;
}
.project-table thead th.column-draggable::after {
    content: " ↕";
    font-size: 11px;
    color: #94a3b8;
}
.project-table thead th.column-fixed {
    cursor: default;
}
.project-table thead th.dragging-col {
    opacity: .55;
    background: #f3e8ff !important;
}
.project-table thead th.drag-over-col {
    box-shadow: inset 0 -3px 0 #7c3aed;
}

    </style>

    <style>
        /* =========================================================
           MY TASK - INTEGRATED VIEW SWITCHER
           UI only. Existing list, filters, timer and server logic stay intact.
           ========================================================= */
        .mytask-view-pane { background:#f1f5f7; min-height:calc(100vh - 210px); }
        .mytask-view-empty { padding:22px; text-align:center; color:#7b8794; background:#fff; border:1px solid #e5e7eb; }

        /* KANBAN */
        .mytask-kanban-board {
            font:13px/18px system-ui,-apple-system,"Segoe UI",Roboto,"Helvetica Neue","Noto Sans","Liberation Sans",Arial,sans-serif,"Apple Color Emoji","Segoe UI Emoji","Segoe UI Symbol","Noto Color Emoji" !important;
            display:flex;
            gap:10px;
            width:100%;
            overflow-x:auto;
            align-items:flex-start;
            padding-bottom:8px;
            scrollbar-width:thin;
            scrollbar-color:#ccc #f5f5f5;
        }
        .mytask-kanban-column {
            flex:1 0 245px;
            min-width:245px;
            max-width:320px;
            border-right:1px solid #e5e7eb;
            padding-right:9px;
        }
        .mytask-kanban-column:last-child { border-right:0; padding-right:0; }
        .mytask-kanban-heading {
            background:#fff;
            border:1px solid #e7eef1;
            padding:9px 10px;
            margin-bottom:8px;
            font-size:13px;
            font-weight:600;
            display:flex;
            align-items:center;
            justify-content:space-between;
        }
        .mytask-kanban-column-body { max-height:calc(100vh - 290px); min-height:120px; overflow-y:auto; padding-right:2px; scrollbar-width:thin; transition:background-color .12s ease, box-shadow .12s ease; }
        .mytask-kanban-column-body.kanban-drag-over { background:#f7fbff; box-shadow:inset 0 0 0 2px #9fc2df; }
        .mytask-kanban-card[draggable="true"] { cursor:grab; }
        .mytask-kanban-card.kanban-card-dragging { opacity:.45; cursor:grabbing; }
        .mytask-kanban-card {
            position:relative;
            background:#fff;
            border:0;
            border-left:4px solid #3978de;
            box-shadow:0 2px 5px rgba(0,0,0,.08);
            margin-bottom:8px;
            border-radius:4px;
            overflow:visible;
            cursor:default;
        }
        .mytask-kanban-card .card-body { padding:8px 9px 6px; }
        .mytask-kanban-card h4 {
            font-size:16px;
            line-height:20px;
            font-weight:600;
            margin:5px 0 3px;
            white-space:nowrap;
            overflow:hidden;
            text-overflow:ellipsis;
        }
        .mytask-kanban-card h5 {
            font-size:14px;
            line-height:18px;
            font-weight:400;
            margin:0;
            white-space:nowrap;
            overflow:hidden;
            text-overflow:ellipsis;
        }
        .mytask-kanban-card .card-footer { padding:5px 8px; background:#fff; min-height:32px; }
        .mytask-kanban-status { font-size:12px; line-height:18px; font-weight:600; white-space:nowrap; }
        .mytask-kanban-priority { font-size:12px; line-height:18px; font-weight:600; padding:2px 6px; border-radius:3px; }
        .mytask-kanban-count { color:#777; font-weight:500; }
        .mytask-kanban-owner-name {
            max-width:105px;
            font-size:11px;
            color:#6b7280;
            white-space:nowrap;
            overflow:hidden;
            text-overflow:ellipsis;
        }
        .mytask-kanban-description {
            margin:0 0 4px;
            font-size:13px;
            line-height:18px;
            font-weight:400;
            color:#000;
            display:-webkit-box;
            -webkit-line-clamp:2;
            -webkit-box-orient:vertical;
            overflow:hidden;
            min-height:36px;
        }
        .mytask-kanban-meta {
            display:flex;
            align-items:center;
            gap:5px;
        }
        .mytask-kanban-meta-item {
            display:inline-flex;
            align-items:center;
            justify-content:center;
            width:26px;
            height:26px;
            border-radius:50%;
            background:#f8f0f7;
            color:#804574;
            cursor:default;
            flex:0 0 26px;
        }
        .mytask-kanban-meta-item i { font-size:15px; }
        .mytask-kanban-meta-item .avatar-xxs { width:26px; height:26px; }
        .mytask-kanban-meta-item .avatar-xxs img { width:26px; height:26px; object-fit:cover; }
        /* Kanban timer: same compact footer behaviour as the reference Kanban screen. */
        .mytask-kanban-meta-item.mytask-kanban-timer {
            width:auto;
            min-width:26px;
            height:26px;
            flex:0 0 auto;
            margin-left:auto;
            border-radius:0;
            background:transparent;
            color:#ff654e;
            position:relative;
            overflow:visible;
            cursor:default;
        }
        .mytask-kanban-timer-display {
            display:inline-flex;
            align-items:center;
            gap:5px;
            height:26px;
            white-space:nowrap;
        }
        .mytask-kanban-timer-display.timer-start-display {
            cursor:pointer;
            color:#00d81b;
        }
        .mytask-kanban-timer-display.timer-start-display i {
            color:#00d81b;
            font-size:22px;
        }
        .mytask-kanban-timer-display.timer-running-display i {
            color:#ff654e;
            font-size:20px;
        }
        .mytask-kanban-timer-display .kanban-timer-time,
        .mytask-kanban-timer-controls .kanban-timer-time {
            color:#ff654e;
            font-size:12px;
            font-weight:500;
            white-space:nowrap;
        }
        .mytask-kanban-timer-controls {
            display:none;
            align-items:center;
            gap:5px;
            height:26px;
            padding:0;
            margin:0;
            background:transparent;
            border:0;
            box-shadow:none;
            white-space:nowrap;
        }
        .mytask-kanban-timer.is-running:hover .mytask-kanban-timer-display,
        .mytask-kanban-timer.is-paused:hover .mytask-kanban-timer-display {
            display:none;
        }
        .mytask-kanban-timer.is-running:hover .mytask-kanban-timer-controls,
        .mytask-kanban-timer.is-paused:hover .mytask-kanban-timer-controls,
        .mytask-kanban-timer-controls:hover {
            display:inline-flex;
        }
        .mytask-kanban-timer-controls button {
            width:20px;
            height:20px;
            border:0;
            border-radius:50%;
            background:transparent;
            padding:0;
            margin:0;
            display:inline-flex;
            align-items:center;
            justify-content:center;
            cursor:pointer;
            line-height:20px;
        }
        .mytask-kanban-timer-controls button i { font-size:20px; line-height:20px; }
        .mytask-kanban-timer-controls .timer-pause { color:#f7b84b; }
        .mytask-kanban-timer-controls .timer-resume { color:#00d81b; }
        .mytask-kanban-timer-controls .timer-stop { color:#ff654e; }
        /* Kanban: change TEXT color only. Keep all existing backgrounds, borders and icon colors. */
        .mytask-kanban-card h4,
        .mytask-kanban-card h5,
        .mytask-kanban-description,
        .mytask-kanban-priority,
        .mytask-kanban-status { color:#000 !important; }
        .mytask-kanban-group-select { width:112px; }
        .mytask-kanban-group-select .input-group-text,
        .mytask-kanban-group-select .form-select {
            height:30px;
            font-size:12px;
            padding-top:3px;
            padding-bottom:3px;
        } 
        .mytask-kanban-text-muted {
            color:lightgray;
        }


        /* Keep all Bootstrap tooltips fully visible and consistently black. */
        .tooltip { z-index:10850 !important; }
        .tooltip .tooltip-inner {
            max-width:320px !important;
            background:#000 !important;
            color:#fff !important;
            white-space:normal !important;
            text-align:left;
            line-height:1.35;
            padding:6px 9px;
            font-size:12px;
        }
        .tooltip.bs-tooltip-top .tooltip-arrow::before { border-top-color:#000 !important; }
        .tooltip.bs-tooltip-bottom .tooltip-arrow::before { border-bottom-color:#000 !important; }
        .tooltip.bs-tooltip-start .tooltip-arrow::before { border-left-color:#000 !important; }
        .tooltip.bs-tooltip-end .tooltip-arrow::before { border-right-color:#000 !important; }

        /* TIMESHEET - only the time grid scrolls horizontally.
           Task / Project / Assigned Time and Actual Time remain fixed. */
        .mytask-timesheet-scroll {
            width:100%;
            overflow:hidden;
            background:#fff;
        }
        .mytask-timesheet-table {
            width:100%;
            min-width:0;
            background:#fff;
        }
        .mytask-timesheet-layout {
            display:grid;
            grid-template-columns:580px minmax(0,1fr) 120px;
            width:100%;
            min-width:0;
            background:#fff;
        }
        .mytask-timesheet-fixed-left,
        .mytask-timesheet-fixed-right {
            min-width:0;
            background:#fff;
            position:relative;
            z-index:2;
        }
        .mytask-timesheet-fixed-left {
            border-right:1px solid #dfe7ec;
        }
        .mytask-timesheet-fixed-right {
            border-left:1px solid #dfe7ec;
        }
        .mytask-timesheet-left-row {
            display:grid;
            grid-template-columns:40px 210px 210px 120px;
            min-height:50px;
            border-bottom:1px solid #dfe7ec;
            align-items:stretch;
        }
        .mytask-timesheet-right-row {
            min-height:50px;
            border-bottom:1px solid #dfe7ec;
            display:flex;
            align-items:stretch;
        }
        .mytask-timesheet-left-row.header,
        .mytask-timesheet-right-row.header,
        .mytask-timesheet-time-row.header {
            min-height:48px;
            height:48px;
            background:#ecf3f7;
            font-weight:600;
        }
        .mytask-timesheet-cell {
            display:flex;
            align-items:center;
            padding:6px 10px;
            border-right:1px solid #dfe7ec;
            min-width:0;
            background:#fff;
        }
        .mytask-timesheet-left-row.header .mytask-timesheet-cell,
        .mytask-timesheet-right-row.header .mytask-timesheet-cell {
            background:#ecf3f7;
        }
        .mytask-timesheet-cell:last-child { border-right:0; }
        .mytask-timesheet-cell.center { justify-content:center; text-align:center; }

        /* This is the ONLY horizontal scroller in Timesheet view. */
        .mytask-timesheet-time-scroll {
            width:100%;
            min-width:0;
            overflow-x:auto;
            overflow-y:hidden;
            background:#fff;
            scrollbar-width:thin;
            scrollbar-color:#999 #f5f5f5;
        }
        .mytask-timesheet-time-inner {
            width:1440px;
            min-width:1440px;
            background:#fff;
        }
        .mytask-timesheet-time-row {
            position:relative;
            width:1440px;
            min-width:1440px;
            min-height:50px;
            height:50px;
            border-bottom:1px solid #dfe7ec;
            overflow:hidden;
        }
        .mytask-timesheet-hours {
            display:grid;
            grid-template-columns:repeat(16, 90px);
            width:1440px;
            height:48px;
        }
        .mytask-timesheet-hours span {
            display:flex;
            align-items:center;
            justify-content:center;
            border-right:1px solid #dfe7ec;
            font-size:12px;
            font-weight:500;
            background:#ecf3f7;
        }
        .mytask-timesheet-track {
            position:relative;
            width:1440px;
            height:49px;
            min-height:49px;
            background:repeating-linear-gradient(to right,#fff 0,#fff 89px,#e5eef3 89px,#e5eef3 90px);
        }
        .mytask-timesheet-track:after {
            content:"";
            position:absolute;
            inset:0;
            background:repeating-linear-gradient(to right,transparent 0,transparent 21.5px,#edf2f5 21.5px,#edf2f5 22.5px);
            pointer-events:none;
        }
        .mytask-timesheet-bar {
            position:absolute;
            top:12px;
            height:25px;
            min-width:42px;
            border-radius:5px;
            color:black;
            font-size:11px;
            font-weight:600;
            display:flex;
            align-items:center;
            justify-content:center;
            z-index:2;
            overflow:hidden;
            white-space:nowrap;
            text-overflow:ellipsis;
            padding:0 8px;
        }
        .mytask-timesheet-dot { width:8px; height:8px; border-radius:50%; flex:0 0 8px; margin-right:8px; }
        .mytask-timesheet-name { overflow:hidden; text-overflow:ellipsis; white-space:nowrap; }

        @media (max-width:991px) {
            .mytask-timesheet-layout {
                grid-template-columns:480px minmax(260px,1fr) 110px;
            }
            .mytask-timesheet-left-row {
                grid-template-columns:40px 170px 170px 100px;
            }
        }

        /* WEEKLY */
        .mytask-weekly-wrap { width:100%; overflow-x:auto; background:#fff; scrollbar-width:thin; }
        .mytask-weekly-table { min-width:1100px; margin:0; }
        .mytask-weekly-table th,
        .mytask-weekly-table td { white-space:nowrap; vertical-align:middle; border-color:#dfe7ec; padding:5px 8px; }
        .mytask-weekly-table thead th { background:#ecf3f7; font-weight:600; text-align:center; }
        .mytask-weekly-table tfoot th { background:#ecf3f7; font-weight:600; text-align:center; }
        .mytask-weekly-table .task-col { min-width:210px; text-align:left; }
        .mytask-weekly-table .project-col { min-width:200px; text-align:left; }
        .mytask-weekly-day-value { color:#111; text-align:center; }
        .mytask-weekly-toolbar { display:flex; align-items:center; justify-content:space-between; gap:12px; padding:8px 10px; border-bottom:1px solid #dfe7ec; background:#fff; }
        .mytask-weekly-nav { display:flex; align-items:center; gap:8px; font-weight:600; }
        .mytask-weekly-nav button { border:0; background:#f7f3f7; color:#804574; width:28px; height:28px; border-radius:4px; cursor:pointer; }
        .mytask-weekly-shift { font-size:12px; color:#526173; white-space:nowrap; }
        .mytask-weekly-day-head { line-height:1.15; }
        .mytask-weekly-day-head .dow { display:block; font-size:11px; color:#6b7280; font-weight:500; margin-bottom:2px; }
        .mytask-weekly-day-head .off { display:block; margin-top:3px; font-size:10px; font-weight:600; color:#9b4d3f; }
        .mytask-weekly-nonworking { background:#faf7f7 !important; }

        /* Selected top task filter only; existing tile layout and filtering stay unchanged. */
        .top-list-sales .tile.mytask-filter-selected {
            border:1px solid #804574;
            border-radius:4px;
            background:#faf5fa;
            box-shadow:0 0 0 1px rgba(128,69,116,.08);
        }

        /* CALENDAR */
        .mytask-calendar-layout {
            display:grid;
            grid-template-columns:minmax(0, 2fr) minmax(360px, 1fr);
            gap:8px;
            background:#fff;
        }
        .mytask-calendar-main,
        .mytask-calendar-side { background:#fff; border:1px solid #e5e7eb; }
        .mytask-calendar-toolbar {
            height:38px; display:flex; align-items:center; gap:8px; padding:0 10px; border-bottom:1px solid #e5e7eb;
        }
        .mytask-calendar-toolbar button { border:0; background:transparent; padding:0; font-size:20px; line-height:1; }
        .mytask-calendar-title { font-size:14px; font-weight:600; }
        .mytask-calendar-days-header,
        .mytask-calendar-grid { display:grid; grid-template-columns:repeat(7, 1fr); }
        .mytask-calendar-days-header div {
            background:#f3f6f9; border-right:1px solid #e5e7eb; border-bottom:1px solid #e5e7eb;
            text-align:center; font-weight:600; padding:8px; font-size:12px;
        }
        .mytask-calendar-days-header div:last-child { border-right:0; }
        .mytask-calendar-day {
            min-height:105px; padding:5px; border-right:1px solid #e5e7eb; border-bottom:1px solid #e5e7eb;
            cursor:pointer; background:#fff; overflow:hidden;
        }
        .mytask-calendar-day:nth-child(7n) { border-right:0; }
        .mytask-calendar-day.other-month { background:#fafafa; color:#b5bdc6; }
        .mytask-calendar-day.selected { box-shadow:inset 0 0 0 2px #804574; }
        .mytask-calendar-day-number { font-weight:600; width:28px; height:28px; display:flex; align-items:center; justify-content:center; border-radius:50%; }
        .mytask-calendar-event {
            margin-top:6px; background:#eef4ff; border-left:3px solid #3978de; padding:5px 6px; font-size:11px; line-height:16px; color:#111;
        }
        .mytask-calendar-event .assigned { color:#2563eb; display:block; }
        .mytask-calendar-event .actual { color:#16a34a; display:block; }
        .mytask-calendar-side-head {
            min-height:38px; padding:7px 10px; border-bottom:1px solid #e5e7eb; display:flex; align-items:center; justify-content:space-between;
        }
        .mytask-calendar-side-head h4 { font-size:14px; font-weight:600; margin:0; }
        .mytask-calendar-side .table th { background:#ecf3f7; font-weight:600; }
        .mytask-calendar-side .table th,
        .mytask-calendar-side .table td { border-color:#cddce4; vertical-align:middle; padding:7px 8px; }
        .mytask-calendar-taskdot { display:inline-block; width:8px; height:8px; border-radius:50%; margin-right:7px; }

        @media (max-width:1199px) {
            .mytask-calendar-layout { grid-template-columns:1fr; }
            .mytask-kanban-column { flex-basis:260px; }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main-wrapper" ng-app="PMSerp" ng-controller="usertaskCtrl">
        <div class="sub-header">
            <div class="row">
                <%--<div class="col-xxl-3 col-lg-3 col-md-4 col-sm-12">
                    <div class="sub-left">
                        <div class="sub-page-title">Task</div>
                        <div class="d-flex align-items-center gap-1 ms-3">
                            
                           <div class="d-flex align-items-center">
                                <i class="ri-calendar-line text-muted me-1" style="font-size:16px;line-height: 1;"></i>
                                <input type="text" id="daterange" readonly/>
                                <i class="ri-arrow-down-s-line" style="font-size:18px;line-height: 1;"></i>
                            </div> 
                            
                        </div>
                    </div>
                </div>--%>
                <div class="col-xxl-3 col-lg-3 col-md-4 col-sm-12">
    <div class="sub-left">

        <div class="sub-page-title">Task</div>

        <div class="d-flex align-items-center gap-2 ms-3">

          <select class="form-select" ng-show="taskView=='list'" ng-model="pageSize"  ng-options="size for size in pageSizes" style="width:55px;"></select>

            <!-- CALENDAR ICON -->
            <i class="ri-calendar-line text-muted ms-1"
               style="font-size:16px;line-height:1;"></i>

            <!-- DATE RANGE INPUT -->
            <input type="text" id="daterange" readonly
                   style="border:none; background:transparent;cursor:pointer" />

            <!-- ARROW -->
            <i class="ri-arrow-down-s-line"
               style="font-size:18px;line-height:1;"></i>

        </div>
    </div>
</div>
                <div class="col-xxl-6 col-lg-6 col-md-8 col-sm-12">
                    <ul class="top-list-sales">
                        <li>
                            <a href="javascript:void(0);"
                               ng-click="filterTaskCalendarByStatus(''); $event.preventDefault();">
                                <div class="tile" ng-class="{'mytask-filter-selected': !taskViewCalendarStatusId}">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <div class="tile-icon"><i class="ri-folder-2-line"></i></div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>Total</span></div>
                                            <div class="tile-body">{{mytasklist.length}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        <li ng-repeat="c in mytasklist_bystatus">
                            <a href="javascript:void(0);"
                               ng-click="filterTaskCalendarByStatus(c.statusid); $event.preventDefault();">
                                <div class="tile" ng-class="{'mytask-filter-selected': taskViewCalendarStatusId == c.statusid}">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <%--<div class="tile-icon" ng-style="{'background-color':getContrastColor(c.statuscolor),'color':c.statuscolor }"><i class="ri-folder-2-line"></i></div>--%>
                                            <div class="tile-icon" ng-if="c.statusicon==''" ng-style="{'color':c.statuscolor }"><i class="ri-folder-2-line"></i></div>
                                            <div class="tile-icon" ng-if="c.statusicon" ng-style="{'color':c.statuscolor }"><i ng-class="c.statusicon"></i>
</div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>{{c.statusname}}</span></div>
                                            <%--<div class="tile-body" ng-style="{'color': c.statuscolor}">{{c.total}}</div>--%>
                                            <div class="tile-body">{{c.total}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        
                    </ul>
                </div>
                <div class="col-xxl-3 col-lg-3 col-md-6 col-sm-12" ng-show="multiselect=='1'">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                        <div class="btn-group d-block mt-1">
                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="d-block" aria-expanded="false" ng-style="{'color':(selectedStatus.length > 0 &&selectedStatus[selectedStatus.length-1].colorname &&selectedStatus[selectedStatus.length-1].colorname != '#FFFFFF')? selectedStatus[selectedStatus.length-1].colorname: '#000'}">

    {{ selectedStatus.length >0? selectedStatus[selectedStatus.length-1].name: 'Status' }}<i class="ri-arrow-down-s-line"></i></a>
                            <div class="dropdown-menu p-0" style="border-radius: 0px;">
                                <ul class="sttus-list">
                                     <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus_multi(newstatus); $event.preventDefault()">
                 <div class='dropdown-item p-0'>
                    <%--<a class='' href='javascript:void(0);'  ng-style="{'color':(newstatus.colorname && newstatus.colorname != '') ?newstatus.colorname:'#000'}" ng-click="selectStatus_multi(newstatus); $event.preventDefault()">
                        <i class='bx bxs-circle' ></i>{{newstatus.name }}
                    </a>--%>
                     <a href="javascript:void(0);" >

    <i class="bx bxs-circle"  ng-style="{'color':newstatus.colorname}"></i>{{newstatus.name}}
</a>
                    </div>
    </li>
                                </ul>
                            </div>
                        </div>
                       
                        <div class="btn-group d-block mt-1">
                            <a href="javascript:void(0);" type="button" data-bs-boundary="window" class="d-block" aria-expanded="false"  data-bs-toggle="modal" data-bs-target="#addtags_multi" ng-click="clearselectedtag()">Tag<i class="ri-arrow-down-s-line"></i></a>
                            
                        </div>
                       
                    </div>
                </div>

                <div class="col-xxl-3 col-lg-3 col-md-6 col-sm-12" ng-show="multiselect=='0'">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                       <div class="search_box me-2"  ng-show="freesearch=='1'">                           
                           <input type="text" class="form-control" ng-model="searchText" placeholder="Search..." />
                            <i class="ri-search-line"></i>
                        </div>

                        <button class="sub-soft-btn" data-bs-toggle="tooltip" title="Search" ng-click="viewsearch()"><i class="ri-search-line"></i></button>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active':taskView=='list'}" ng-click="setTaskView('list')" data-bs-toggle="tooltip" title="List View"><i class="ri-list-check"></i></a>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active':taskView=='kanban'}" ng-click="setTaskView('kanban')" data-bs-toggle="tooltip" title="Kanban View"><i class="las la-chart-bar2"></i></a>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active':taskView=='timesheet'}" ng-click="setTaskView('timesheet')" data-bs-toggle="tooltip" title="Timesheet"><i class="bx bx-server"></i></a>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active':taskView=='weekly'}" ng-click="setTaskView('weekly')" data-bs-toggle="tooltip" title="Weekly View"><i class="ri-time-line"></i></a>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active':taskView=='calendar'}" ng-click="setTaskView('calendar')" data-bs-toggle="tooltip" title="Calendar"><i class="bx bx-calendar"></i></a>
                        <button class="sub-soft-btn" data-bs-toggle="tooltip" title="Filter"><i class="ri-filter-line"></i></button>
                        <div class="btn-group d-block">
                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="sub-soft-btn"><i class="ri-more-fill"></i></a>
                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                <ul class='sttus-list'>
                                    <li><a class='' href='#'><i class="bx bx-download fs-16 align-middle me-1"></i> Download Report</a></li>
                                </ul>
                            </div>
                        </div>
                        <div class="input-group mytask-kanban-group-select" ng-if="taskView=='kanban'">
                            <span class="input-group-text"><i class="ri-equalizer-line"></i></span>
                            <select class="form-select" ng-model="kanban.groupBy" ng-change="setKanbanGroupBy(kanban.groupBy)">
                                <option value="status">Status</option>
                                <option value="priority">Priority</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div  ng-if="isload=='1'">
        <div class="wrapper_inner p-0" ng-show="taskView=='list'">
            <div class="table-responsive-new">
                <table id='tbDetails' class="table table-hover table-striped tbl_new project-table">
                    <thead>
                        <tr>
                            <th class="color_border" style="text-align:right;">
                                  <input type="checkbox" class="form-check-input" id="chkAll"   onclick="angular.element(this).scope().selectall(this.checked)">

                            </th>
                            <th>Task</th>
                            <th>Description</th>
                            <th>Project Name</th>
                            <th>Priority</th>
                            <th>Status</th>
                            <th>Total Completion</th>
                            <th>Tags</th>
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Timeline</th>
                            <th>Assigned Time</th>
                            <th style="min-width:160px;">Spent Time</th>
                            <th>Project Head</th>
                            <th style="min-width:120px;">Team</th>
                        </tr>
                    </thead>
                    <tbody>
                       
                       <%--<tr dir-paginate="c in mytasklist|orderBy:sortColumn:reverseSort| itemsPerPage: pageSize | filter:searchText track by $index" current-page="currentPage" ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}">--%>
                        <%--<tr dir-paginate="c in (mytasklist | filter:searchText | orderBy:sortColumn:reverseSort)| itemsPerPage: pageSize track by $index" current-page="currentPage" ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}">--%>
       <tr dir-paginate="
        c in mytasklist
        | filter:taskTopStatusFilter
        | filter:searchText
        | orderBy:sortColumn:reverseSort
        | itemsPerPage:pageSize
        "
    current-page="currentPage" ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}">

                            <td class="color_border" style="border-left-color:#ffc300 !important;" >
                                <input class="form-check-input" type="checkbox" ng-model="c.selected" ng-change="checkselect(c)">
                            </td>
                            <td>
                                <span style="display:none">{{ ((currentPage - 1) * pageSize) + $index + 1 }}</span>
                                <span ng-if="getTaskViewName(c) && getTaskViewName(c).length <= 30">{{getTaskViewName(c)}}</span>
                                <span ng-if="getTaskViewName(c) && getTaskViewName(c).length > 30"
                                      bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                      data-bs-container="body" data-bs-boundary="viewport"
                                      data-bs-title="{{getTaskViewName(c)}}">{{getTaskViewName(c) | limitTo:30}}...</span>
                            </td>
                            <td>{{c.SubTaskDescription}}</td>
                            <td>{{c.ProjectName}}</td>
                            
            <td style="text-align:center;" ng-style="{'background-color': c.priority_color}">
   <div class="priority" >{{c.priority_name}}</div>

            </td>
       

                           
                          
                            <td ng-style="{'background-color': c.status_color}">
                               
                              <div style="position:relative;text-align:center">     
                                     <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class=''  style="font-size:12px;font-weight:600;color:black" > {{ c.status_name }}</a>
                                    <%--<a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="d-block" ng-style="{'color':c.status_color}" ng-click="
      c._oldcomplete_value = c.complete_value;
      c._oldpending_value = c.pending_value;
      
   "><i class='bx bxs-circle'></i>{{c.status_name }}</a>--%>
                                    <div class="dropdown-menu p-0" style="border-radius:0;">
                                        <ul class='sttus-list'>
                                            <%--<li><a class='text-warning' href='#'><i class='bx bxs-circle' ng-style="{'color':c.status_color}"></i>{{c.status_name }}</a></li>--%>
                                           <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c,newstatus); $event.preventDefault()" >
                 <div class='dropdown-item p-0'>
                    <%--<a class='' href='javascript:void(0);' ng-style="{'color':(newstatus.colorname && newstatus.colorname != '') ? newstatus.colorname:'#000'}" ng-click="selectStatus(c,newstatus); $event.preventDefault()">
                        <i class='bx bxs-circle' ></i>{{newstatus.name }}
                    </a>--%>
                     <a href="javascript:void(0);">

    <i class="bx bxs-circle" ng-style="{'color':newstatus.colorname}" ></i>{{newstatus.name}}
</a>
                    </div>
    </li>
                                        </ul>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div class="d-block mb-0">
                                        <div style="font-size:11px !important;color:#777;">{{c.complete_value}}%</div>
                                    </div>
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar bg-success" role="progressbar" style="width:{{c.complete_value}}%;"></div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <%--<span class="badge bg-danger priority" style="background:#ff6a00;">Important</span>--%>
                               <%-- <a ng-repeat="tag in c.tags|limitTo:1" ng-click="viewtags(c,'subtask'); $event.preventDefault()" class="badge priority tag-wrp"  ng-style="{'background':'#' + tag.tagcolor}">
                                </a>--%>
                                <a ng-repeat="tag in c.tags | limitTo:1"
   class="badge priority tag-wrp"
   ng-style="{
      'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor,'color':'black'
   }">
    {{tag.tagname}}
                                    <i class="ri-close-line tag-del" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="Remove" ng-click="removetags(c,tag.id)"></i>
</a>
                                
                              
                                <span  ng-if="c.tags.length>1" class="cursor-pointer" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingtags(c.tags) }}">{{c.tags.length-1}}</span>
                               
                                

                                <a href="javascript:void(0);" ng-click="Addtags(c,c.tasktype)" data-bs-toggle="modal" data-bs-target="#addtags"><span class='btn-plus2' data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' title='+ Add Tags'>+</span></a>
                            </td>
                            
                            <td>{{formatDateOnly(c.Startdate) }}</td>
                            <td>{{formatDateOnly(c.Targetdate) }}</td>
                            <td>{{formatDateOnly(c.Startdate) }}-{{ formatDateOnly(c.Targetdate) }}</td>
                            <td>{{ c.TaskDuration}} hrs</td>
                            <td class="spent-time-cell">

    <!-- before start -->
  
    <div class="start-timer"
         ng-if="!c.isrun"
         ng-click="timerstart(c)">

        <i class="bx bx-play-circle text-success"
           style="font-size:18px;margin-top:2px;"></i>

        <span class="text-success">
            Start Timer
        </span>

    </div>

    <!-- after start -->
    <div class="running-timer"
         ng-if="c.isrun">

        <!-- timer icon -->
        <i class="bx bx-stopwatch text-danger"
           style="font-size:18px;"></i>

        <!-- timer text -->
         <%--<span class="timer-text">
            {{(c.timerText || '00:00:00').split(':').slice(0, 3).join(':')}}
        </span>--%>
        
        <span ng-if="c.isexceed == 1" class="timer-text">
    {{(c.timerText || '00:00:00').split(':').slice(0, 3).join(':')}}
</span>

<span ng-if="c.isexceed == 0" class="timer-text-green">
    {{(c.timerText || '00:00:00').split(':').slice(0, 3).join(':')}}
</span>

        <!-- hover buttons -->
        <div class="timer-controls">

    <!-- timer text inside hover -->
    <span class="text-white fw-bold"
          style="font-size:13px;">
        {{(c.timerText || '00:00:00').split(':').slice(0, 3).join(':')}}
    </span>

    <!-- pause -->
    <button type="button"
            ng-if="!c.ispause"
            ng-click="timerpause(c)">

        <i class="ri-pause-circle-fill text-warning"
           title="Pause"></i>

    </button>

    <!-- play -->
    <button type="button"
            ng-if="c.ispause"
            ng-click="timerresume(c)">

        <i class="ri-play-circle-fill text-success"
           title="Play"></i>

    </button>

    <!-- stop -->
    <button type="button"
            ng-click="timersstop(c)">

        <i class="ri-stop-circle-fill text-danger"
           title="Stop"></i>

    </button>

</div>

    </div>
<!-- view history -->
                                <div class="start-history" style="display:inline !important" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="History">
   <a href="javascript:void(0);"  ng-click="showhistory(c.id,c.tasktype)" data-bs-toggle="modal" data-bs-target="#timerhistory">
   <i class="bx bx-history"></i>
  </a>
</div>
                                </br> 
                                 <span ng-if="c.isend==1"
      ng-class="c.isexceed == 1 ? 'timer-text' : 'timer-text-green'" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="Spent Time">
    {{c.timerText_spent || '00:00:00'}}
</span>

                                
</td>
                            <td style="text-align: center;">
                                <%--{{c.Powner}}--%>
<span class="mytask-project-head-avatar" ng-if="c.Powner"
    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top"
    data-bs-container="body" data-bs-boundary="viewport"
    data-bs-title="{{getOwnerName(c.Powner) || 'Project Head'}}"
    aria-label="Project Head: {{getOwnerName(c.Powner)}}">
    <img ng-if="getProjectHeadAvatarImage(c.Powner)" ng-src="{{getProjectHeadAvatarImage(c.Powner)}}"
         alt="" onerror="this.style.display='none';this.nextElementSibling.classList.remove('ng-hide');this.nextElementSibling.style.display='flex';" />
    <span ng-show="!getProjectHeadAvatarImage(c.Powner)">{{getProjectHeadInitials(c.Powner)}}</span>
</span>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div class="avatar-group">
                                        <a href="javascript: void(0);"  ng-repeat="newteam in c.teams|limitTo:2"  class="avatar-group-item material-shadow" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}">
                                            <%--<div class="avatar-xxs">

                                                <div class="avatar-title rounded-circle bg-danger">
                                                    D
                                                </div>
                                                </div>--%>
                                                <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle img-fluid" />

    <!-- Initial -->
   
      <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary" >
        {{newteam.shortname}}
      </div>
    

    </div>

                                            
                                        </a>
                                        <div ng-if="c.teams && c.teams.length > 2"
     class="avatar-group-item owner-wrp"
     bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingOwners(c.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle bg-primary">
        +{{ c.teams.length - 2 }}
    </div>
    </div>
</div>
                                       <%-- <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="Lee Winton">
                                            <div class="avatar-xxs">
                                                <img src="/pms/new/assets/img/avatar-2.jpg" alt="" class="rounded-circle img-fluid">
                                            </div>
                                        </a>
                                        <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="Johnny Shorter">
                                            <div class="avatar-xxs">
                                                <img src="/pms/new/assets/img/avatar-3.jpg" alt="" class="rounded-circle img-fluid">
                                            </div>
                                        </a>--%>
                                        <%--<a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="2 More">
                                            <div class="avatar-xxs">
                                                <div class="avatar-title rounded-circle bg-primary">
                                                    +2
                                                </div>
                                            </div>
                                        </a>--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                       
                    </tbody>
                </table>
            </div>
            
        </div>
            <div ng-show="isLoading" class="loader-overlay">
<div class="loader"></div>
            </div>
             <!-- Paging Controls -->
<div class="pull-right" ng-show="taskView=='list'">
<dir-pagination-controls
max-size="8" 
direction-links="true"
boundary-links="true">
</dir-pagination-controls>
</div>


        <!-- =====================================================
             INTEGRATED KANBAN VIEW
             Group by Status or Priority. Cards can be moved by drag/drop.
             ===================================================== -->
        <div class="wrapper_inner p-2 mytask-view-pane" ng-if="taskView=='kanban'">
            <div class="mytask-kanban-board">
                <div class="mytask-kanban-column" ng-repeat="grp in getKanbanGroups() track by grp.id">
                    <div class="mytask-kanban-heading" ng-style="{'border-top':'3px solid ' + getKanbanGroupColor(grp)}">
                        <span>{{grp.name}} <span class="mytask-kanban-count">({{getKanbanGroupCount(grp.id)}})</span></span>
                    </div>

                    <div class="mytask-kanban-column-body"
                         kanban-drop-group="grp">
                        <div class="mytask-kanban-card card"
                             ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}" ng-repeat="c in (getKanbanGroupTasks(grp.id) | filter:searchText) track by $index"
                             kanban-card-drag="c"
                             ng-style="{'border-left-color':getTaskViewBorderColor(c)}">

                            <div class="card-body">
                                <div class="d-flex align-items-center justify-content-between gap-2">
                                    <span class="mytask-kanban-priority"
                                          ng-style="{'background-color':getTaskViewSoftColor(c.priority_color),'color':'#000'}">
                                        {{c.priority_name || 'Normal'}}
                                    </span>

                                    <span class="mytask-kanban-status" style="color:#000 !important;">
                                        <i class="bx bxs-circle" ng-style="{'color':getTaskViewStatusTextColor(c.status_color)}"></i> {{c.status_name || '-'}}
                                    <input class="form-check-input ng-pristine ng-valid ng-empty ng-touched" type="checkbox" ng-model="c.selected" ng-change="checkselect(c)">
                                    </span>
                                </div>

                                <h4 ng-if="getTaskViewName(c) && getTaskViewName(c).length <=30" >{{getTaskViewName(c)}}</h4>
                                <%--<h4 title="{{getTaskViewName(c)}}">{{getTaskViewName(c)}}</h4>--%>
                                <h4 ng-if="getTaskViewName(c) && getTaskViewName(c).length >30" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                          data-bs-title="{{getTaskViewName(c)}}">{{getTaskViewName(c) | limitTo:30}}...</h4>
                                <!-- Description: show max 60 chars; full description in tooltip only when > 60 chars -->
                                <p class="mytask-kanban-description"
                                   ng-if="!c.SubTaskDescription || c.SubTaskDescription.length <= 60">
                                    {{c.SubTaskDescription || 'No description'}}
                                </p>
                                <p class="mytask-kanban-description"
                                   ng-if="c.SubTaskDescription && c.SubTaskDescription.length > 60"
                                   bs-tooltip
                                   data-bs-toggle="tooltip"
                                   data-bs-placement="top"
                                   data-bs-container="body"
                                   data-bs-boundary="viewport"
                                   data-bs-title="{{c.SubTaskDescription}}">
                                    {{c.SubTaskDescription | limitTo:60}}...
                                </p>
                                 <span class="mytask-kanban-text-muted" ng-if="c.ProjectName && c.ProjectName.length <=30" >{{c.ProjectName}}</span>
                                <span class="mytask-kanban-text-muted" ng-if="c.ProjectName && c.ProjectName.length >30" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                          data-bs-title="{{c.ProjectName}}">{{c.ProjectName | limitTo:30}}...</span>
                            </div>

                            <div class="card-footer border-top">
                                <div class="mytask-kanban-meta">
                                    <!-- Owner: display only -->
<span class="mytask-project-head-avatar" ng-if="c.Powner"
    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top"
    data-bs-container="body" data-bs-boundary="viewport"
    data-bs-title="{{getOwnerName(c.Powner) || 'Project Head'}}"
    aria-label="Project Head: {{getOwnerName(c.Powner)}}">
    <img ng-if="getProjectHeadAvatarImage(c.Powner)" ng-src="{{getProjectHeadAvatarImage(c.Powner)}}"
         alt="" onerror="this.style.display='none';this.nextElementSibling.classList.remove('ng-hide');this.nextElementSibling.style.display='flex';" />
    <span ng-show="!getProjectHeadAvatarImage(c.Powner)">{{getProjectHeadInitials(c.Powner)}}</span>
</span>

                                    <!-- Task date range: display only -->
                                    <span class="mytask-kanban-meta-item"
                                          bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                          data-bs-title="{{formatDateOnly(c.Startdate) || '-'}} - {{formatDateOnly(c.Targetdate) || '-'}}">
                                        <i class="bx bx-calendar-week"></i>
                                    </span>

                                    <!-- Assigned/task duration: display only -->
                                    <span class="mytask-kanban-meta-item"
                                          bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                          data-bs-title="{{c.TaskDuration || '00:00'}} Hrs">
                                        <i class="bx bx-time"></i>
                                    </span>
                                    <div class="start-history" style="display:inline !important" bs-tooltip="" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="History">
   <a href="javascript:void(0);" ng-click="showhistory(c.id,c.tasktype)" data-bs-toggle="modal" data-bs-target="#timerhistory">
   <i class="bx bx-history"></i>
  </a>
</div>

                                    <!-- Timer: compact normal state; actions appear inline on hover. Existing timer methods/modals reused. -->
                                    <span class="mytask-kanban-meta-item mytask-kanban-timer"
                                          ng-class="{'is-running':c.isrun && !c.ispause,'is-paused':c.isrun && c.ispause,'is-not-started':!c.isrun}">

                                        <!-- Not started: only green Start icon -->
                                        <span class="mytask-kanban-timer-display timer-start-display"
                                              ng-if="!c.isrun"
                                              ng-click="timerstart(c); $event.stopPropagation()">
                                            

                                            <span class="kanban-timer-time"
                                                  ng-if="hasKanbanSpentTime(c)"
                                                  bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                                  data-bs-title="Spent Time">{{getKanbanSpentTime(c)}}</span>
                                            
                                            <i class="ri-play-circle-line" bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                               data-bs-title="Start Timer"></i>
                                        </span>

                                        <!-- Running/paused normal display: stopwatch + current time -->
                                        <span class="mytask-kanban-timer-display timer-running-display"
                                              ng-if="c.isrun">
                                            <i class="bx bx-stopwatch"></i>
                                            <span class="kanban-timer-time">{{(c.timerText || '00:00:00').split(':').slice(0, 3).join(':')}}</span>
                                            
                                        </span>
                                       
                                        <!-- Hover display exactly like Kanban reference: Stop + Pause/Resume + time -->
                                        <span class="mytask-kanban-timer-controls"
                                              ng-if="c.isrun"
                                              ng-click="$event.stopPropagation()">

                                            <button type="button"
                                                    class="timer-stop"
                                                    ng-click="timersstop(c); $event.stopPropagation()"
                                                    aria-label="Stop Timer"
                                                    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                                    data-bs-title="Stop Timer">
                                                <i class="ri-stop-circle-fill"></i>
                                            </button>

                                            <button type="button"
                                                    class="timer-pause"
                                                    ng-if="!c.ispause"
                                                    ng-click="timerpause(c); $event.stopPropagation()"
                                                    aria-label="Pause Timer"
                                                    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                                    data-bs-title="Pause Timer">
                                                <i class="ri-pause-circle-fill"></i>
                                            </button>

                                            <button type="button"
                                                    class="timer-resume"
                                                    ng-if="c.ispause"
                                                    ng-click="timerresume(c); $event.stopPropagation()"
                                                    aria-label="Resume Timer"
                                                    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-container="body" data-bs-boundary="viewport"
                                                    data-bs-title="Resume Timer">
                                                <i class="ri-play-circle-fill"></i>
                                            </button>

                                            <span class="kanban-timer-time">{{(c.timerText || '00:00:00').split(':').slice(0, 3).join(':')}}</span>
                                        </span>
                                    </span>
                                </div>
                            </div>
                        </div>

                        <div class="mytask-view-empty" ng-if="getKanbanGroupCount(grp.id)==0">No tasks</div>
                    </div>
                </div>
            </div>
        </div>

        <!-- =====================================================
             INTEGRATED TIMESHEET VIEW
             Read-only alternate presentation; existing timer operations remain unchanged.
             ===================================================== -->
        <div class="wrapper_inner p-0 mytask-view-pane" ng-if="taskView=='timesheet'">
            <div class="mytask-timesheet-scroll">
                <div class="mytask-timesheet-table">

                    <!-- Keep all non-time columns fixed. Only the middle time grid scrolls. -->
                    <div class="mytask-timesheet-layout" ng-if="getTaskTopStatusFilteredTasks().length">

                        <!-- FIXED LEFT: checkbox, Task, Project Name, Assigned Time -->
                        <div class="mytask-timesheet-fixed-left">
                            <div class="mytask-timesheet-left-row header">
                                <div class="mytask-timesheet-cell center"><input class="form-check-input" type="checkbox" onclick="angular.element(this).scope().selectall(this.checked)" /></div>
                                <div class="mytask-timesheet-cell">Task</div>
                                <div class="mytask-timesheet-cell">Project Name</div>
                                <div class="mytask-timesheet-cell center">Assigned Time</div>
                            </div>

                            <div class="mytask-timesheet-left-row" ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}" ng-repeat="c in (mytasklist | filter:taskTopStatusFilter | filter:searchText | limitTo:pageSize) track by $index">
                                <div class="mytask-timesheet-cell center"><input class="form-check-input" type="checkbox" ng-model="c.selected" ng-change="checkselect(c)" /></div>
                                <div class="mytask-timesheet-cell">
                                    <span class="mytask-timesheet-dot" ng-style="{'background-color':getTaskViewBorderColor(c)}"></span>
                                    <span class="mytask-timesheet-name" ng-if="getTaskViewName(c) && getTaskViewName(c).length <= 30">{{getTaskViewName(c)}}</span>
                                    <span class="mytask-timesheet-name" ng-if="getTaskViewName(c) && getTaskViewName(c).length > 30"
                                          bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                          data-bs-container="body" data-bs-boundary="viewport"
                                          data-bs-title="{{getTaskViewName(c)}}">{{getTaskViewName(c) | limitTo:30}}...</span>
                                </div>
                                <div class="mytask-timesheet-cell"><span class="mytask-project-head-avatar" ng-if="c.Powner"
    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top"
    data-bs-container="body" data-bs-boundary="viewport"
    data-bs-title="{{getOwnerName(c.Powner) || 'Project Head'}}"
    aria-label="Project Head: {{getOwnerName(c.Powner)}}">
    <img ng-if="getProjectHeadAvatarImage(c.Powner)" ng-src="{{getProjectHeadAvatarImage(c.Powner)}}"
         alt="" onerror="this.style.display='none';this.nextElementSibling.classList.remove('ng-hide');this.nextElementSibling.style.display='flex';" />
    <span ng-show="!getProjectHeadAvatarImage(c.Powner)">{{getProjectHeadInitials(c.Powner)}}</span>
</span><span class="mytask-timesheet-name">{{c.ProjectName}}</span></div>
                                <div class="mytask-timesheet-cell center">{{getTaskViewAssignedText(c)}}</div>
                            </div>
                        </div>

                        <!-- SCROLLABLE MIDDLE: hours + timeline bars -->
                        <div class="mytask-timesheet-time-scroll">
                            <div class="mytask-timesheet-time-inner"
                                 ng-style="{'width':taskViewTimesheetWidth+'px','min-width':taskViewTimesheetWidth+'px'}">
                                <div class="mytask-timesheet-time-row header"
                                     ng-style="{'width':taskViewTimesheetWidth+'px','min-width':taskViewTimesheetWidth+'px'}">
                                    <div class="mytask-timesheet-hours"
                                         ng-style="{'width':taskViewTimesheetWidth+'px','grid-template-columns':'repeat('+taskViewHours.length+', '+taskViewTimesheetCellWidth+'px)'}">
                                        <span ng-repeat="h in taskViewHours track by $index">{{h}}</span>
                                    </div>
                                </div>

                                <div class="mytask-timesheet-time-row"
                                     ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}" ng-repeat="c in (mytasklist | filter:taskTopStatusFilter | filter:searchText | limitTo:pageSize) track by $index"
                                     ng-style="{'width':taskViewTimesheetWidth+'px','min-width':taskViewTimesheetWidth+'px'}">
                                    <div class="mytask-timesheet-track"
                                         ng-style="{'width':taskViewTimesheetWidth+'px'}">
                                        <div class="mytask-timesheet-bar"
                                             ng-style="getTaskViewTimesheetBarStyle(c)"
                                             title="Assigned duration: {{getTaskViewAssignedText(c)}}">
                                            {{c.isrun ? (c.timerText || '00:00:00').split(':').slice(0, 3).join(':') : getTaskViewActualText(c)}}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- FIXED RIGHT: Actual Time -->
                        <div class="mytask-timesheet-fixed-right">
                            <div class="mytask-timesheet-right-row header">
                                <div class="mytask-timesheet-cell center" style="width:100%;">Actual Time</div>
                            </div>

                            <div class="mytask-timesheet-right-row" ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}" ng-repeat="c in (mytasklist | filter:taskTopStatusFilter | filter:searchText | limitTo:pageSize) track by $index">
                                <div class="mytask-timesheet-cell center" style="width:100%;">
                                    <span ng-class="c.isrun ? 'text-success' : (c.isend==1 ? 'text-danger' : 'text-muted')">
                                        <i class="bx bx-stopwatch me-1"></i>{{c.isrun ? (c.timerText || '00:00:00').split(':').slice(0, 3).join(':') : getTaskViewActualText(c)}}
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mytask-view-empty" ng-if="!getTaskTopStatusFilteredTasks().length">No tasks</div>
                </div>
            </div>
        </div>

        <!-- =====================================================
             INTEGRATED WEEKLY VIEW
             Existing My Task data remains unchanged. This view adds:
             current Monday-Sunday week, login-user shift capacity,
             day-wise timer spent time, Week Off and Holiday handling.
             ===================================================== -->
        <div class="wrapper_inner p-0 mytask-view-pane" ng-if="taskView=='weekly'">
            <div class="mytask-weekly-wrap">
                <div class="mytask-weekly-toolbar">
                    <div class="mytask-weekly-nav">
                        <button type="button" ng-click="changeTaskViewWeek(-1)" title="Previous Week"><i class="ri-arrow-left-s-line"></i></button>
                        <span>{{taskViewWeekRangeLabel}}</span>
                        <button type="button" ng-click="changeTaskViewWeek(1)" title="Next Week"><i class="ri-arrow-right-s-line"></i></button>
                    </div>
                    <div class="mytask-weekly-shift">
                        Shift: {{taskViewWeekShiftStart || '-'}} - {{taskViewWeekShiftEnd || '-'}}
                        &nbsp;|&nbsp; Working Days: {{taskViewWeekWorkingDays}}
                        &nbsp;|&nbsp; Week Capacity: {{taskViewWeekCapacityTotal}}
                    </div>
                </div>
                <table class="table table-hover table-striped tbl_new mytask-weekly-table mb-0">
                    <thead>
                        <tr>
                            <th style="width:40px;"><input class="form-check-input" type="checkbox" onclick="angular.element(this).scope().selectall(this.checked)" /></th>
                            <th class="task-col">Task</th>
                            <th class="project-col">Project Name</th>
                            <th ng-repeat="d in taskViewWeekDays track by d.key" ng-class="{'mytask-weekly-nonworking':d.isHoliday || d.isWeekOff}">
                                <div class="mytask-weekly-day-head">
                                    <span class="dow">{{d.dayName}}</span>
                                    <span>{{d.label}}</span>
                                    <span class="off" ng-if="d.isHoliday">Holiday</span>
                                    <span class="off" ng-if="!d.isHoliday && d.isWeekOff">Week Off</span>
                                </div>
                            </th>
                            <th>Assigned Time</th>
                            <th>Actual Time</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}" ng-repeat="c in (mytasklist | filter:taskTopStatusFilter | filter:searchText | limitTo:pageSize) track by $index">
                            <td style="border-left:4px solid #3978de;" ng-style="{'border-left-color':getTaskViewBorderColor(c)}"><input class="form-check-input" type="checkbox" ng-model="c.selected" ng-change="checkselect(c)" /></td>
                            <td>
                                <a ng-href="{{getUserTaskPageUrl(c)}}" target="_blank" rel="noopener" ng-click="openUserTaskPage(c,$event)" class="mytask-task-page-link"><span ng-if="getTaskViewName(c) && getTaskViewName(c).length <= 45">{{getTaskViewName(c)}}</span>
                                <span ng-if="getTaskViewName(c) && getTaskViewName(c).length > 45"
                                      bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                      data-bs-container="body" data-bs-boundary="viewport"
                                      data-bs-title="{{getTaskViewName(c)}}">{{getTaskViewName(c) | limitTo:45}}...</span></a>
                            </td>
                            <td><span class="mytask-project-head-avatar" ng-if="c.Powner"
    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top"
    data-bs-container="body" data-bs-boundary="viewport"
    data-bs-title="{{getOwnerName(c.Powner) || 'Project Head'}}"
    aria-label="Project Head: {{getOwnerName(c.Powner)}}">
    <img ng-if="getProjectHeadAvatarImage(c.Powner)" ng-src="{{getProjectHeadAvatarImage(c.Powner)}}"
         alt="" onerror="this.style.display='none';this.nextElementSibling.classList.remove('ng-hide');this.nextElementSibling.style.display='flex';" />
    <span ng-show="!getProjectHeadAvatarImage(c.Powner)">{{getProjectHeadInitials(c.Powner)}}</span>
</span> {{c.ProjectName}}</td>
                            <td class="mytask-weekly-day-value"
                                ng-repeat="d in taskViewWeekDays track by d.key"
                                style="cursor:help;"
                                ng-class="{'mytask-weekly-nonworking':d.isHoliday || d.isWeekOff}"
                                bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                      data-bs-container="body" data-bs-boundary="viewport"
                                      data-bs-title="{{getTaskViewWeekTooltip(c,d)}}">{{getTaskViewWeekCell(c,d)}}</td>
                            <td class="text-center">{{getTaskViewWeekAssignedText(c)}}</td>
                            <td class="text-center">{{getTaskViewWeekActualText(c)}}</td>
                        </tr>
                    </tbody>
                    <tfoot ng-if="getTaskTopStatusFilteredTasks().length > 0">
                        <tr>
                            <th colspan="3" style="text-align:right;">Total Day Hours</th>
                            <th ng-repeat="d in taskViewWeekDays track by d.key"
                                ng-class="{'mytask-weekly-nonworking':d.isHoliday || d.isWeekOff}"
                                title="{{getTaskViewWeekTotalTooltip(d)}}">{{getTaskViewWeekFilteredTotal(d.key)}}</th>
                            <th title="Cumulative weekly shift capacity of all displayed tasks">{{getTaskViewWeekAssignedGrandTotal()}}</th>
                            <th>{{getTaskViewWeekFilteredActualTotal()}}</th>
                        </tr>
                    </tfoot>
                </table>
                <div class="mytask-view-empty" ng-if="!getTaskTopStatusFilteredTasks().length">No tasks</div>
            </div>
        </div>

        <!-- =====================================================
             INTEGRATED CALENDAR VIEW
             ===================================================== -->
        <div class="wrapper_inner p-0 mytask-view-pane" ng-if="taskView=='calendar'">
            <div class="mytask-calendar-layout">
                <div class="mytask-calendar-main">
                    <div class="mytask-calendar-toolbar">
                        <button type="button" ng-click="changeTaskViewCalendarMonth(-1)"><i class="ri-arrow-left-s-line"></i></button>
                        <i class="ri-calendar-line text-muted"></i>
                        <span class="mytask-calendar-title">{{taskViewCalendarTitle}}</span>
                        <button type="button" ng-click="changeTaskViewCalendarMonth(1)"><i class="ri-arrow-right-s-line"></i></button>
                    </div>
                    <div class="mytask-calendar-days-header">
                        <div>Sun</div><div>Mon</div><div>Tue</div><div>Wed</div><div>Thu</div><div>Fri</div><div>Sat</div>
                    </div>
                    <div class="mytask-calendar-grid">
                        <div class="mytask-calendar-day"
                             ng-repeat="d in taskViewCalendarDays track by d.key"
                             ng-class="{'other-month':!d.inMonth,'selected':d.key==taskViewSelectedCalendarKey}"
                             ng-click="selectTaskViewCalendarDay(d)">
                            <div class="mytask-calendar-day-number">{{d.day}}</div>
                            <div class="mytask-calendar-event" ng-if="d.count>0">
                                <strong>{{d.count}} Task<span ng-if="d.count!=1">s</span></strong>
                                <span class="assigned">Assigned Time: {{d.assigned}}</span>
                                <span class="actual">Actual Hrs: {{d.actual}}</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mytask-calendar-side">
                    <div class="mytask-calendar-side-head">
                        <h4>{{taskViewSelectedCalendarLabel}}</h4>
                    </div>
                    <div class="table-responsive">
                        <table class="table align-middle mb-0">
                            <thead><tr><th>Task</th><th>Project Name</th><th>Assigned Hours</th><th>Status</th></tr></thead>
                            <tbody>
                                <tr ng-class="{'mytask-query-highlight':isQueryTaskHighlighted(c)}" ng-repeat="c in (taskViewSelectedCalendarTasks | filter:searchText) track by $index">
                                    <td>
                                        <span class="mytask-calendar-taskdot" ng-style="{'background-color':getTaskViewBorderColor(c)}"></span>
                                        <a ng-href="{{getUserTaskPageUrl(c)}}" target="_blank" rel="noopener" ng-click="openUserTaskPage(c,$event)" class="mytask-task-page-link"><span ng-if="getTaskViewName(c) && getTaskViewName(c).length <= 40">{{getTaskViewName(c)}}</span>
                                        <span ng-if="getTaskViewName(c) && getTaskViewName(c).length > 40"
                                              bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                              data-bs-container="body" data-bs-boundary="viewport"
                                              data-bs-title="{{getTaskViewName(c)}}">{{getTaskViewName(c) | limitTo:40}}...</span></a>
                                    </td>
                                    <td><span class="mytask-project-head-avatar" ng-if="c.Powner"
    bs-tooltip data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top"
    data-bs-container="body" data-bs-boundary="viewport"
    data-bs-title="{{getOwnerName(c.Powner) || 'Project Head'}}"
    aria-label="Project Head: {{getOwnerName(c.Powner)}}">
    <img ng-if="getProjectHeadAvatarImage(c.Powner)" ng-src="{{getProjectHeadAvatarImage(c.Powner)}}"
         alt="" onerror="this.style.display='none';this.nextElementSibling.classList.remove('ng-hide');this.nextElementSibling.style.display='flex';" />
    <span ng-show="!getProjectHeadAvatarImage(c.Powner)">{{getProjectHeadInitials(c.Powner)}}</span>
</span> {{c.ProjectName}}</td>
                                    <td>{{getTaskViewAssignedText(c)}}</td>
                                    <td><span >{{c.status_name}}</span></td><%--ng-style="{'color':getTaskViewStatusTextColor(c.status_color)}"--%>
                                </tr>
                                <tr ng-if="!taskViewSelectedCalendarTasks.length"><td colspan="4" class="text-center text-muted p-3">No tasks</td></tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <input type="hidden" id="hdnid" value="0" />
        <div id="timerhistory" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title"> {{mytasklist_history.taskname}} (Total Spent Time -{{mytasklist_history.spent}})</h5>
                <button type="button" class="btn-close" id="btnclshistory" data-bs-dismiss="modal" aria-label="Close"></button>
                
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                        <%--{{mytasklist_history.taskname}}--%>
<div class="table-responsive-new">
                <table class="table table-hover table-striped">
                    <thead>
                        <tr>
                            
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Spent Time</th>
                            <th>Remark</th>
                            <%--<th>Status</th> --%>                           
                            
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="history in mytasklist_history.history track by $index">
                            <td>{{ history.startdate | date:'dd/MM/yyyy'}}</td>
                            <td>{{ history.enddate | date:'dd/MM/yyyy'}}</td>
                            <td>{{ history.spenttime}}</td>
                            <td>{{ history.remark}}</td>

                        </tr>
                        </tbody>
                    </table>
    </div>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</div>
<div id="addnotes" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Add Notes</h5>
                <button type="button" class="btn-close" id="btnclsNotes" data-bs-dismiss="modal" aria-label="Close" ng-click="closepause();"></button>
                <button type="button" style="display:none" class="btn-close" id="btnclsNotes2" data-bs-dismiss="modal" aria-label="Close" ></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

                        <div class="col-sm-12 mb-1">
                            <textarea id="Notes" runat="server" class="form-control"></textarea>
                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <div id="pnlimgpause" style="display:none;">
                        <img src="/pms/Image/loading8.gif" runat="server" />
                    </div>
                <div class="btn-group" id="divpause" style="display: block;">
               <button type="button" id="btnpausesaveNotes" class="btn btn-primary"  ng-click="SavePauseDetials();">Submit</button>
             </div>

            </div>
        </div>
    </div>
</div>

<div id="timer_stop" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Timer has stopped</h5>
                <button type="button" class="btn-close" id="btnclsTimer" data-bs-dismiss="modal" aria-label="Close" ng-click="closestop();"></button>
                <button type="button" style="display:none" class="btn-close" id="btnclsTimer2" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <%--<h5 class="text-center">Timer has stopped</h5>--%>

                    <asp:HiddenField ID="hdnhourcount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnminutecount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnsecondcount" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnmilisecondcount" runat="server" Value="0" />
                   

                    <div class="row"> 
                        <div class="col-sm-12 mb-2">
                            <div class="d-flex align-items-center">
                            </div>
                            <div class="timer-stopped">
                                <div class="startendhrs">
                                    <h4 class="mb-0"><span id="spnhr">{{(currentlist.timerText || '00:00:00').split(':').slice(0, 3).join(':')}}</span></h4>
                                    <span>Hours</span>
                                </div>
                                <div class="startendtime" style="display:none">
                                   Start Time - <span id="spnStarttime"></span><br />
                                    End Time - <span id="spnEndtime"></span>

                                </div>
                                
                            </div>
                        </div>
                        <div class="col-xxl-3 col-lg-3" style="display:none">
                            <div class="form-check form-radio-primary">
                                <label class="form-check-label">
                                    <input value="" name="Billable" type="radio" id="" class="form-check-input" checked="checked">
                                    Billable
                                </label>
                            </div>
                        </div>
                        <div class="col-xxl-3 col-lg-3" style="display:none">
                            <div class="form-check form-radio-primary">
                                <label class="form-check-label">
                                    <input value="" name="Billable" type="radio" id="" class="form-check-input">
                                    Non Billable
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1 mt-2">
                            <div class="d-flex align-items-center">
                                <label class="form-label flex-grow-1">Notes</label>
                            </div>
                      
                                <textarea id="TimerNotes" runat="server" class="form-control"></textarea>

                        </div>                        
                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                 <div id="pnlimgstop" style="display:none;">
                        <img src="/pms/Image/loading8.gif" runat="server" />
                    </div>
                <div class="btn-group" id="divstop" style="display: block;">
               <button type="button" id="btnsaveTimer" class="btn btn-primary" ng-click="SaveStopDetials();">Submit</button>
               <%-- <button type="button" id="btnRemoveTimer" style="display:none" onclick="RemoveDataTimer('removeTimer');" class="btn btn-dark">Remove Timer</button>--%>
          
             </div>
                  </div>

        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>
            <div id="addtags" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            
            <div class="modal-header d-flex justify-content-between align-items-center" style="border-bottom:1px solid #ddd;padding:10px;">
                    <h4 style="font-size:18px;margin:0;">Add Tags</h4>
                    <button type="button" id="btnclstags" class="sub-soft-btn danger" data-bs-dismiss="modal" aria-label="Close" ng-click="closetagwindow();"><i class="ri-close-line"></i></button>
                
                </div>
           
             <div class="modal-body p-3 pb-3" click-outside="closelist()">
                 <div  id="tagDropdown" class="input-line" style="position:relative; width:100%;" >

                 <div class="input-tags" ng-click="closelist()">
        <span class="tag" ng-repeat="tag in getSelectedTags()" ng-style="{'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}} <span class="remove" ng-click="removetagsnew(tag.id); $event.stopPropagation()"><i class="ri-close-fill"></i></span>
        </span>
        
    </div>
                 <div style="width:100%;">
                                <input type="text" class="search__input"
               ng-model="searchTag"
               placeholder="Select Tag..."
               ng-click="openTagDropdown($event);stopTagEvent($event)"
               ng-change="filterTagList()"
               style="width:100%;" />                                
                            </div>
            
    <div class="dropdown-menu"
         ng-show="showTagDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

        <!-- Owner list -->
        <div ng-repeat="tag in filteredTagList | filter:searchTag"
             ng-click="toggleTag(tag)"
             ng-class="{'selected-tag': selectedtagIds.includes(tag.id)}"
             style="padding:3px; cursor:pointer;"
            ng-style="{
      'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}}
        </div>

        <div ng-if="(mytasktaglist | filter:searchTag).length == 0" style="padding:3px; color:#888;">
            No owners found
        </div>
    </div>

</div>
             </div>
            <div class="modal-body p-3 pb-2" id="dvnewtag" style="display:none">
                <asp:HiddenField ID="hdntagprjid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdntagshtml" runat="server" Value="0" />
<div class="d-flex gap-2 align-items-center justify-content-between">
                        <div class="d-flex gap-2 justify-content-between" style="width:100%;">
                            <div style="width:100%;">
                               
                                 <input type="text" id="tags" placeholder="Tag Name" runat="server" class="form-control" required/>
                            </div>
                            <div>
                                <div class="input-group">
                                    <span class="input-group-text"><i class="bx bx-color-fill"></i></span>
                                   <%-- <input type="text" data-jscolor="{}" class="jscolor form-control">--%>
                                    <input id="tagscolor" class="form-control">
                                </div>
                            </div>
                        </div>
                        <div class="d-flex gap-1">
                            
                       <button type="button" id="btnsaveTags" class="sub-soft-btn" data-bs-toggle="tooltip" title="+ Add" ng-click="SvaeadTags();"><i class="ri-add-line"></i></button>
                            <button type="button" class="sub-soft-btn danger" ng-click="closenewtabgdiv()"><i class="ri-close-line"></i></button>
                            </div>
                    </div>
<div class="pt-3 pb-2">
                        <%--<div class="d-flex gap-1">
                            <span class="badge priority" style="background:#ff6a00;">Important</span>
                            <span class="badge priority" style="background:#3978de;">Urgent</span>
                        </div>--%>
                    </div>
                
            </div>
            
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
                <style>
                   .custom-dropdown{
    display:block;
    position:absolute;
    top:100%;
    left:0;
    right:0;
    background:#fff;
    border:1px solid #ddd;
    border-radius:6px;
    max-height:250px;
    overflow-y:auto;
    z-index:9999;
    box-shadow:0 2px 10px rgba(0,0,0,.1);
}

.dropdown-item-custom{
    padding:8px 12px;
    cursor:pointer;
}

.dropdown-item-custom:hover{
    background:#f5f5f5;
}

.selected-tag{
    background:#e7f1ff;
    font-weight:600;
}

.no-record{
    padding:10px;
    color:#888;
}

.input-tags{
    display:flex;
    flex-wrap:wrap;
    gap:5px;
    margin-bottom:5px;
}

.input-tags .tag{
    background:#eef2ff;
    border-radius:20px;
    padding:4px 10px;
    font-size:12px;
}

.input-tags .tag .remove {
    margin-left:5px;
    cursor:pointer;
    font-weight:bold;
}
.dropdown-menu div:hover {
    background-color: #e0e0e0;
}
                </style>
</div>
            <div id="addtags_multi" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            
            <div class="modal-header d-flex justify-content-between align-items-center" style="border-bottom:1px solid #ddd;padding:10px;">
                    <h4 style="font-size:18px;margin:0;">Add Tags</h4>
                    <button type="button" id="btnclstags2" class="sub-soft-btn danger" data-bs-dismiss="modal" aria-label="Close" ng-click="closetagwindow();"><i class="ri-close-line"></i></button>
                
                </div>
           
             <div class="modal-body p-3 pb-3" click-outside="closelist2()">
                 <div  id="tagDropdown2" class="input-line" style="position:relative; width:100%;" >

                 <div class="input-tags" ng-click="closelist()">
        <span class="tag" ng-repeat="tag in getSelectedTags()" ng-style="{'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}} <span class="remove" ng-click="removetagsnew_multi(tag.id); $event.stopPropagation()"><i class="ri-close-fill"></i></span>
        </span>
        
    </div>
                 <div style="width:100%;">
                                <input type="text" class="search__input"
               ng-model="searchTag"
               placeholder="Select Tag..."
               ng-click="openTagDropdown($event);stopTagEvent($event)"
               ng-change="filterTagList()"
               style="width:100%;" />
                                
                            </div>
            
    <div class="dropdown-menu"
         ng-show="showTagDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

        <!-- Owner list -->
        <div ng-repeat="tag in filteredTagList | filter:searchTag"
             ng-click="toggleTag_multi(tag)"
             ng-class="{'selected-tag': selectedtagIds.includes(tag.id)}"
             style="padding:3px; cursor:pointer;"
            ng-style="{
      'background-color':
      (tag.tagcolor && tag.tagcolor.indexOf('#') === 0)
      ? tag.tagcolor
      : '#' + tag.tagcolor
   }">
            {{tag.tagname}}
        </div>

        <div ng-if="(mytasktaglist | filter:searchTag).length == 0" style="padding:3px; color:#888;">
            No owners found
        </div>
    </div>

</div>
             </div>
            <div class="modal-body p-3 pb-2" id="dvnewtag2" style="display:none">
                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                          <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
<div class="d-flex gap-2 align-items-center justify-content-between">
                        <div class="d-flex gap-2 justify-content-between" style="width:100%;">
                            <div style="width:100%;">
                               
                                 <input type="text" id="tags2" placeholder="Tag Name" runat="server" class="form-control" required/>
                            </div>
                            <div>
                                <div class="input-group">
                                    <span class="input-group-text"><i class="bx bx-color-fill"></i></span>
                                   <%-- <input type="text" data-jscolor="{}" class="jscolor form-control">--%>
                                    <input id="tagscolor2" class="form-control">
                                </div>
                            </div>
                        </div>
                        
                    </div>

                
            </div>
            <div class="d-flex justify-content-center p-2">                            
                       <button type="button" id="btnsaveTags2" class="btn btn-primary" data-bs-toggle="tooltip" title="Save" ng-click="SvaeadTags_Multi();">Save</button>
                            
                            </div>
            
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
                <style>
                   .custom-dropdown{
    display:block;
    position:absolute;
    top:100%;
    left:0;
    right:0;
    background:#fff;
    border:1px solid #ddd;
    border-radius:6px;
    max-height:250px;
    overflow-y:auto;
    z-index:9999;
    box-shadow:0 2px 10px rgba(0,0,0,.1);
}

.dropdown-item-custom{
    padding:8px 12px;
    cursor:pointer;
}

.dropdown-item-custom:hover{
    background:#f5f5f5;
}

.selected-tag{
    background:#e7f1ff;
    font-weight:600;
}

.no-record{
    padding:10px;
    color:#888;
}

.input-tags{
    display:flex;
    flex-wrap:wrap;
    gap:5px;
    margin-bottom:5px;
}

.input-tags .tag{
    background:#eef2ff;
    border-radius:20px;
    padding:4px 10px;
    font-size:12px;
}

.input-tags .tag .remove {
    margin-left:5px;
    cursor:pointer;
    font-weight:bold;
}
.dropdown-menu div:hover {
    background-color: #e0e0e0;
}
                </style>
</div>
    </div>
        
    <script src="/pms/new/assets/js/jquery.min.js"></script>
    <script src="/pms/new/assets/js/moment.min.js"></script>
    <script src="/pms/new/assets/js/daterangepicker.min.js"></script>
      
        <script>

$(function () {

    // =========================================
    // MAIN DATERANGE
    // =========================================
    var taskLinkQuery = new URLSearchParams(window.location.search || '');
    var taskLinkFrom = moment(taskLinkQuery.get('fromdate') || '', 'YYYY-MM-DD', true);
    var taskLinkTo = moment(taskLinkQuery.get('todate') || '', 'YYYY-MM-DD', true);
    var validTaskLinkRange = taskLinkFrom.isValid() && taskLinkTo.isValid()
        && !taskLinkFrom.isAfter(taskLinkTo, 'day');
    $('#daterange').daterangepicker({

    autoUpdateInput: true,
    autoApply: true,

    // DEFAULT THIS MONTH
    startDate: validTaskLinkRange ? taskLinkFrom : moment().startOf('month'),
    endDate: validTaskLinkRange ? taskLinkTo : moment().endOf('month'),

    locale: {
        format: 'DD/MM/YYYY'
    },

    
     ranges: {

            'All': [moment('2020-01-01'), moment()],
            'Today': [moment(), moment()],
            'This Week': [moment().startOf('week'), moment().endOf('week')],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'This Quarter': [moment().startOf('quarter'), moment().endOf('quarter')],
            'This Year': [moment().startOf('year'), moment().endOf('year')],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Previous Week': [moment().subtract(1, 'week').startOf('week'), moment().subtract(1, 'week').endOf('week')],
            'Previous Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],            
            'Previous Quarter': [moment().subtract(1, 'quarter').startOf('quarter'), moment().subtract(1, 'quarter').endOf('quarter')],            
            'Previous Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
        }
});




    // =========================================
    // GET PICKER
    // =========================================
    var picker = $('#daterange').data('daterangepicker');
    if (validTaskLinkRange) {
        var linkedLabel = taskLinkQuery.get('range');
        picker.chosenLabel = linkedLabel && Object.prototype.hasOwnProperty.call(picker.ranges, linkedLabel)
            && picker.ranges[linkedLabel][0].isSame(taskLinkFrom, 'day')
            && picker.ranges[linkedLabel][1].isSame(taskLinkTo, 'day')
            ? linkedLabel : 'Custom Range';
    }




    // =========================================
    // HIDE DEFAULT CALENDAR
    // =========================================
    picker.container
        .find('.drp-calendar')
        .hide();




    // =========================================
    // DEFAULT VALUE
    // =========================================
    $('#daterange').val(

        picker.startDate.format('DD/MM/YYYY') +
        ' - ' +
        picker.endDate.format('DD/MM/YYYY')

    );




    // =========================================
    // NORMAL RANGE APPLY
    // =========================================
    $('#daterange').on(
        'apply.daterangepicker',
        function (ev, picker) {

            var fromdate =
                picker.startDate.format('DD/MM/YYYY');

            var todate =
                picker.endDate.format('DD/MM/YYYY');

            $(this).val(
                fromdate + ' - ' + todate
            );

            // CUSTOM RANGE SKIP
            if (
                picker.chosenLabel !== 'Custom Range'
            ) {

                callAngularBind(
                    fromdate,
                    todate
                );
            }

        }
    );




    // =========================================
    // CUSTOM RANGE HTML
    // =========================================
    var customHtml = `

        <div class="custom-range-box p-2"
             style="display:none;width:200px;">

            <div class="mb-2">

                <label class="mb-1 d-block">
                    From Date
                </label>

                <input type="text"
                       id="customFromDate"
                       class="form-control"
                       placeholder="DD/MM/YYYY"
                       maxlength="10">

            </div>

            <div class="mb-2">

                <label class="mb-1 d-block">
                    To Date
                </label>

                <input type="text"
                       id="customToDate"
                       class="form-control"
                       placeholder="DD/MM/YYYY"
                       maxlength="10">

            </div>

            <button type="button"
                    id="btnCustomApply"
                    class="btn btn-primary btn-sm w-100">

                Apply

            </button>

        </div>

    `;

    picker.container.append(customHtml);




    // =========================================
    // CUSTOM SINGLE-DATE CALENDARS
    // Create only once. Re-initializing these controls was the reason a
    // selected date sometimes appeared only after a second Apply click.
    // =========================================
    initCustomCalendar();



    // =========================================
    // RANGE CLICK
    // =========================================
    picker.container.find('.ranges li').on(
        'click.mytaskCustomRange',
        function () {

            var label =
                $(this).attr('data-range-key');



            // =====================================
            // CUSTOM RANGE
            // =====================================
            if (label === 'Custom Range') {

                // Keep Custom Range visibly selected while From/To are edited.
                picker.chosenLabel = 'Custom Range';
                picker.container.find('.ranges li').removeClass('active');
                $(this).addClass('active');

                // HIDE MAIN CALENDAR / DEFAULT BUTTONS
                picker.container
                    .find('.drp-calendar')
                    .hide();

                picker.container
                    .find('.drp-buttons')
                    .hide();

                // SHOW CUSTOM BOX
                picker.container
                    .find('.custom-range-box')
                    .show();

                // Fill the two boxes from the currently selected main range.
                var fromText = picker.startDate.format('DD/MM/YYYY');
                var toText = picker.endDate.format('DD/MM/YYYY');

                $('#customFromDate').val(fromText);
                $('#customToDate').val(toText);

                // Synchronize the single-date pickers without recreating them.
                var fromPicker = $('#customFromDate').data('daterangepicker');
                var toPicker = $('#customToDate').data('daterangepicker');

                if (fromPicker) {
                    fromPicker.setStartDate(moment(fromText, 'DD/MM/YYYY'));
                    fromPicker.setEndDate(moment(fromText, 'DD/MM/YYYY'));
                }

                if (toPicker) {
                    toPicker.setStartDate(moment(toText, 'DD/MM/YYYY'));
                    toPicker.setEndDate(moment(toText, 'DD/MM/YYYY'));
                }
            }



            // =====================================
            // NORMAL RANGE
            // =====================================
            else {

                picker.container
                    .find('.custom-range-box')
                    .hide();

                picker.container
                    .find('.drp-calendar')
                    .hide();

                picker.container
                    .find('.drp-buttons')
                    .hide();
            }

        }
    );



    // =========================================
    // ALLOW ONLY DATE CHARACTERS
    // =========================================
    $(document).on(
        'input',
        '#customFromDate, #customToDate',
        function () {

            this.value =
                this.value.replace(
                    /[^0-9\/]/g,
                    ''
                );

        }
    );




    // =========================================
    // CUSTOM APPLY
    // =========================================
    $(document).on(
        'click',
        '#btnCustomApply',
        function () {

            var fromdate =
                $('#customFromDate')
                    .val()
                    .trim();

            var todate =
                $('#customToDate')
                    .val()
                    .trim();



            // EMPTY CHECK
            if (
                fromdate === '' ||
                todate === ''
            ) {

                alert(
                    'Please enter both dates'
                );

                return;
            }



            // FORMAT CHECK
            if (
                !isValidDateFormat(
                    fromdate
                )
            ) {

                alert(
                    'Invalid From Date. Use DD/MM/YYYY'
                );

                $('#customFromDate')
                    .focus();

                return;
            }

            if (
                !isValidDateFormat(
                    todate
                )
            ) {

                alert(
                    'Invalid To Date. Use DD/MM/YYYY'
                );

                $('#customToDate')
                    .focus();

                return;
            }



            // DATE CHECK
            if (
                toDateObj(fromdate) >
                toDateObj(todate)
            ) {

                alert(
                    'From Date cannot be greater than To Date'
                );

                return;
            }



            // SET VALUE
            $('#daterange').val(

                fromdate +
                ' - ' +
                todate

            );



            // UPDATE PICKER
            picker.setStartDate(
                moment(
                    fromdate,
                    'DD/MM/YYYY'
                )
            );

            picker.setEndDate(
                moment(
                    todate,
                    'DD/MM/YYYY'
                )
            );

            // Keep main filter marked as Custom Range after Apply.
            picker.chosenLabel = 'Custom Range';
            picker.container.find('.ranges li').removeClass('active');
            picker.container
                .find('.ranges li[data-range-key="Custom Range"]')
                .addClass('active');



            // CLOSE
            picker.hide();



            // CALL ANGULAR
            callAngularBind(
                fromdate,
                todate
            );
        }
    );

});




// =========================================
// VALIDATE DATE
// =========================================
function isValidDateFormat(dateString) {

    var regex =
        /^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/\d{4}$/;

    if (!regex.test(dateString)) {

        return false;
    }

    var parts =
        dateString.split('/');

    var day =
        parseInt(parts[0], 10);

    var month =
        parseInt(parts[1], 10) - 1;

    var year =
        parseInt(parts[2], 10);

    var date =
        new Date(year, month, day);

    return (

        date.getFullYear() === year &&
        date.getMonth() === month &&
        date.getDate() === day

    );
}




// =========================================
// STRING TO DATE
// =========================================
function toDateObj(str) {

    var p = str.split('/');

    return new Date(

        parseInt(p[2]),
        parseInt(p[1]) - 1,
        parseInt(p[0])

    );
}




// =========================================
// CALL ANGULAR
// =========================================
function callAngularBind(
    fromdate,
    todate
) {

    var scope =
        angular.element(
            $('#daterange')
        ).scope();



    // FALLBACK
    if (!scope) {

        scope = angular.element(
            document.querySelector(
                '[ng-controller]'
            )
        ).scope();
    }



    // CALL FUNCTION
    if (scope) {

        scope.$applyAsync(function () {

            scope.fromdate = fromdate;

            scope.todate = todate;



            if (
                typeof scope.bindtasklist2
                === 'function'
            ) {

                scope.bindtasklist2(
                    fromdate,
                    todate
                );
            }

        });
    }
}
function initCustomCalendar() {

    function ensureSingleDatePicker(selector) {

        var $input = $(selector);
        if (!$input.length) return;

        // IMPORTANT: create each picker only once.
        if (!$input.data('daterangepicker')) {
            $input.daterangepicker({
                singleDatePicker: true,
                autoApply: true,
                autoUpdateInput: false,
                showDropdowns: true,
                // IMPORTANT: keep this calendar OUTSIDE the main daterange popup.
                // If it is appended inside .custom-range-box, the existing code that
                // hides the main .drp-calendar also hides this single-date calendar.
                parentEl: 'body',
                locale: {
                    format: 'DD/MM/YYYY'
                }
            });

            var createdPicker = $input.data('daterangepicker');
            if (createdPicker && createdPicker.container) {
                createdPicker.container.addClass('mytask-custom-single-date-picker');
                createdPicker.container.css('z-index', 100000);
            }
        }

        // Clicking the From/To textbox must always open its calendar on the first click.
        $input.off('click.mytaskCustomDateOpen mousedown.mytaskCustomDateOpen')
            .on('mousedown.mytaskCustomDateOpen click.mytaskCustomDateOpen', function (ev) {
                ev.stopPropagation();

                var input = $(this);
                var datePicker = input.data('daterangepicker');
                if (!datePicker) return;

                var typed = input.val().trim();
                if (isValidDateFormat(typed)) {
                    var m = moment(typed, 'DD/MM/YYYY', true);
                    if (m.isValid()) {
                        datePicker.setStartDate(m);
                        datePicker.setEndDate(m);
                    }
                }

                // Defer show until the current click stack completes so the main
                // daterangepicker's document handler cannot immediately hide it.
                setTimeout(function () {
                    datePicker.show();
                    if (datePicker.container) {
                        datePicker.container.css('z-index', 100000);
                    }
                }, 0);
            });

        // Prevent clicks inside the single-date popup from being treated as clicks
        // on/outside the main range picker.
        var singlePicker = $input.data('daterangepicker');
        if (singlePicker && singlePicker.container) {
            singlePicker.container.off('mousedown.mytaskCustomDate click.mytaskCustomDate')
                .on('mousedown.mytaskCustomDate click.mytaskCustomDate', function (ev) {
                    ev.stopPropagation();
                });
        }

        // Namespace + off() makes the handler idempotent even if this helper
        // is called again later by existing page code.
        $input.off('apply.daterangepicker.mytaskCustomDate')
            .on('apply.daterangepicker.mytaskCustomDate', function (ev, datePicker) {
                var selected = datePicker.startDate.format('DD/MM/YYYY');
                $(this).val(selected).trigger('change');

                // Do not close the main range popup. User should be able to
                // select From Date, then click To Date, then press final Apply.
                var mainPicker = $('#daterange').data('daterangepicker');
                if (mainPicker) {
                    // The child calendar lives under <body>. Selecting a child date can
                    // make the main picker think the click happened outside it. Re-open
                    // the main Custom Range box immediately so the user can choose the
                    // second date without selecting Custom Range again.
                    setTimeout(function () {
                        mainPicker.chosenLabel = 'Custom Range';
                        mainPicker.show();
                        mainPicker.container.find('.custom-range-box').show();
                        // These selectors are now safe because the child calendar is
                        // no longer nested inside mainPicker.container.
                        mainPicker.container.find('.drp-calendar, .drp-buttons').hide();
                        mainPicker.container.find('.ranges li').removeClass('active');
                        mainPicker.container
                            .find('.ranges li[data-range-key="Custom Range"]')
                            .addClass('active');
                    }, 0);
                }
            });
    }

    ensureSingleDatePicker('#customFromDate');
    ensureSingleDatePicker('#customToDate');
}
</script>
        
  <%--ranges: {

            'All': [moment('2020-01-01'), moment()],

            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],

            'This Week': [moment().startOf('week'), moment().endOf('week')],
            'Previous Week': [moment().subtract(1, 'week').startOf('week'), moment().subtract(1, 'week').endOf('week')],

            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],

            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Previous Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],

            'This Quarter': [moment().startOf('quarter'), moment().endOf('quarter')],
            'Previous Quarter': [moment().subtract(1, 'quarter').startOf('quarter'), moment().subtract(1, 'quarter').endOf('quarter')],

            'This Year': [moment().startOf('year'), moment().endOf('year')],
            'Previous Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
        }--%>  
        
<script src="/pms/assets/js/jscolor.js"></script>

<script>

$(document).on('shown.bs.modal', '#addtags', function () {

    if (!document.getElementById('tagscolor').jscolor) {

        new jscolor(document.getElementById('tagscolor'));

    }

});
$(document).on('shown.bs.modal', '#addtags_multi', function () {

    if (!document.getElementById('tagscolor2').jscolor) {

        new jscolor(document.getElementById('tagscolor2'));

    }

});
</script>

<script>
        document.addEventListener('DOMContentLoaded', function () {

            document.querySelectorAll('.search__wrap').forEach(wrapper => {

                const tagInput = wrapper.querySelector('.tag-input'); // optional
                const input = wrapper.querySelector('.search__input');
                const dropdown = wrapper.querySelector('.search__dropdown');
                const arrow = wrapper.querySelector('.search__arrow');
                const items = [...wrapper.querySelectorAll('.dropdown_bl__link:not(.search-more)')];
                const noRecord = wrapper.querySelector('.dropdown_bl__no-record');

                /* ✅ PLACEHOLDER FIX */
                const originalPlaceholder = input.getAttribute('placeholder');
                function updatePlaceholder() {
                    if (!tagInput) return;
                    const hasTags = tagInput.querySelectorAll('.tag').length > 0;
                    input.placeholder = hasTags ? 'Search' : originalPlaceholder;
                }

                function openDropdown() {
                    dropdown.style.display = 'block';
                }

                function closeDropdown() {
                    dropdown.style.display = 'none';
                }

                function updateNoRecord() {
                    const visible = items.some(i => i.style.display !== 'none');
                    if (noRecord) {
                        noRecord.classList.toggle('d-none', visible);
                    }
                }

                function toggleTag(value) {
                    if (!tagInput) return false;

                    const existing = [...tagInput.querySelectorAll('.tag')]
                        .find(tag => tag.dataset.value === value);

                    const item = items.find(i => i.textContent.trim() === value);

                    /* 🔁 REMOVE TAG */
                    if (existing) {
                        existing.remove();
                        if (item) item.style.display = 'block';
                        updateNoRecord();
                        updatePlaceholder(); // ✅
                        return true;
                    }

                    /* ➕ ADD TAG */
                    const tag = document.createElement('span');
                    tag.className = 'tag';
                    tag.dataset.value = value;
                    tag.innerHTML = `${value} <i class="ri-close-line"></i>`;

                    tag.querySelector('i').onclick = () => {
                        tag.remove();
                        if (item) item.style.display = 'block';
                        updateNoRecord();
                        updatePlaceholder(); // ✅
                    };

                    tagInput.insertBefore(tag, input);
                    if (item) item.style.display = 'none';

                    updateNoRecord();
                    updatePlaceholder(); // ✅
                    return true;
                }

                /* Open dropdown */
                (tagInput || input).addEventListener('click', () => {
                    input.focus();
                    openDropdown();
                });

                if (arrow) {
                    arrow.addEventListener('click', e => {
                        e.stopPropagation();
                        dropdown.style.display === 'block' ? closeDropdown() : openDropdown();
                    });
                }

                /* 🔍 Search filter */
                input.addEventListener('input', () => {
                    const value = input.value.toLowerCase();
                    let visibleCount = 0;

                    items.forEach(item => {
                        if (item.style.display === 'none') return;

                        const match = item.textContent.toLowerCase().includes(value);
                        item.style.display = match ? 'block' : 'none';
                        if (match) visibleCount++;
                    });

                    if (noRecord) {
                        noRecord.classList.toggle('d-none', visibleCount !== 0);
                    }
                });

                /* Select / Unselect */
                items.forEach(item => {
                    item.addEventListener('click', e => {
                        e.preventDefault();
                        const value = item.textContent.trim();

                        if (tagInput) {
                            toggleTag(value);
                            input.value = '';
                            input.dispatchEvent(new Event('input'));
                        } else {
                            input.value = value;
                            closeDropdown();
                        }
                    });
                });

                /* Click outside */
                document.addEventListener('click', e => {
                    if (!wrapper.contains(e.target)) {
                        closeDropdown();
                    }
                });

                updatePlaceholder(); // ✅ initial state

            });

        });
    </script>
</asp:Content>
