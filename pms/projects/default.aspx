<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="myprojects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        #zoomBar input[type="range"] {
    -webkit-appearance: none;
    appearance: none;
    width: 150px;
    height: 4px;
    background: #d0d0d0;
    border-radius: 5px;
    outline: none;
}

/* ===== Chrome / Edge / Safari ===== */
#zoomBar input[type="range"]::-webkit-slider-runnable-track {
    height: 4px;
    background: #cfcfcf;
    border-radius: 5px;
}

#zoomBar input[type="range"]::-webkit-slider-thumb {
    -webkit-appearance: none !important;
    appearance: none !important;
    width: 14px;
    height: 14px;
    background: #888 !important;   /* ðŸ”´ TEST COLOR (RED) */
    border: 0px solid #000;
    border-radius: 10px;
    cursor: pointer;
    margin-top: -6px;
}

/* ===== Firefox ===== */
#zoomBar input[type="range"]::-moz-range-thumb {
    width: 14px;
    height: 14px;
    background: #888 !important;   /* ðŸ”´ TEST COLOR */
    border: 2px solid #000;
    border-radius: 10px;
    cursor: pointer;
}
        .zoom-bar {
    position: sticky;
    bottom: 0;
    
    padding: 8px 12px;
    display: flex;
    align-items: center;
    gap: 10px;
    z-index: 100;
}
    </style>
    <script src='/pms/angularmaster/jsapp/pmsapp.js?v=<%=GetAssetVersion("/pms/angularmaster/jsapp/pmsapp.js") %>'></script> 
     <script>
        window.PmsAssignPermissions = {
            ischeckEdit: <%= ischeckEdit %>,            
        };
        // PERFORMANCE: saved date filter rendered server-side (same value as getfilter WebMethod).
        window.PmsSavedProjectFilter = <%= SavedProjectFilterJs %>;
    </script>
<script src='/pms/projects/app/myproject.service.js?v=<%=GetAssetVersion("/pms/projects/app/myproject.service.js") %>'></script>
<script src='/pms/projects/app/myproject.main.js?v=<%=GetAssetVersion("/pms/projects/app/myproject.main.js") %>'></script>
    <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script> 
    <link rel="stylesheet" href="/pms/new/assets/css/daterangepicker.css" type="text/css" />
   <style>
       .pms-no-edit{pointer-events:none !important;cursor:not-allowed !important;opacity:.65 !important;}
.toast-position-20 {    
    z-index: 9999 !important;
}

       .pms-toast {
position: fixed;
top: 20px !important;
    right: 20px !important;
background: #23a566;
color: #fff;
padding: 14px 22px;
border-radius: 6px;
font-size: 15px;
z-index: -9999;
box-shadow: 0 4px 10px rgba(0,0,0,0.2);
opacity: 0;
transform: translateY(-20px);
transition: all .3s ease-in-out;
width: 260px;
}

.pms-toast.show {
opacity: 1;
transform: translateY(0);
}
.pms-progress {
height: 4px;
background: rgba(255,255,255,0.6);
position: absolute;
bottom: 0;
left: 0;
width: 100%;
}

.pms-progress-bar {
height: 100%;
background: white;
width: 100%;
transition: width linear;
}
/*top: 20px;
right: 20px;*/
.pms-toasterror {
position: fixed;
top: 20px !important;
    right: 20px !important;
background: #f77b7b;
color: #fff;
padding: 14px 22px;
border-radius: 6px;
font-size: 15px;
z-index: -9999;
box-shadow: 0 4px 10px rgba(0,0,0,0.2);
opacity: 0;
transform: translateY(-20px);
transition: all .3s ease-in-out;
width: 260px;
}

.pms-toasterror.show {
opacity: 1;
transform: translateY(0);
}
.pms-progresserror {
height: 4px;
background: rgba(255,255,255,0.6);
position: absolute;
bottom: 0;
left: 0;
width: 100%;
}

.pms-progress-barerror {
height: 100%;
background: white;
width: 100%;
transition: width linear;
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

        .priority-select-wrap .priority-option { display:inline-block; }
        .priority-select-wrap .priority-btn { position:relative; min-width:135px; text-align:left; border:1px solid transparent; }
        .priority-select-wrap .priority-btn .priority-tick { font-size:16px; font-weight:700; vertical-align:middle; }
        .priority-select-wrap .priority-btn.priority-selected { box-shadow:0 0 0 2px rgba(128,69,116,.28); border-color:rgba(0,0,0,.12); }

        .status-select-wrap .status-option { display:inline-block; }
        .status-select-wrap .status-btn { position:relative; min-width:135px; text-align:left; border:1px solid transparent; }
        .status-select-wrap .status-btn .status-tick { font-size:16px; font-weight:700; vertical-align:middle; }
        .status-select-wrap .status-btn.status-selected { box-shadow:0 0 0 2px rgba(128,69,116,.28); border-color:rgba(0,0,0,.12); }
    </style>
    <style>
    
        .search_box {position:relative;}
.search_box i {position:absolute;top:4px;left:7px;font-size:16px;}
.search_box input.form-control {padding-left:28px;color:#000;border-radius:4px;height:32px;border: 1px solid #ddd !important;}
.search_box input.form-control:focus {border-color:#804574 !important;}
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
    text-align:left;
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
        .files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.issue_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.dependency_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}

    </style>
    <style>
        .owner-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:9px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}
          a.owner-wrp {position:relative;}
a.owner-wrp:hover .owner-del {display:block !important;}
        .loader-overlay {
position: fixed;
top: 0px; left: 0px;
width: 100%; height: 100%;
background: #ffffff; /* keep page content completely hidden while loading */
display: flex;
justify-content: center;
align-items: center;
z-index: 99999;
opacity: 1;
pointer-events: all;
}

.loader {
border: 8px solid #f3f3f3;
border-top: 2px solid #3498db;
border-radius: 50%;
width: 60px;
height: 60px;
animation: spin 1s linear infinite;
}

/* Hide Angular-controlled project content until Angular has initialized it. */
[ng-cloak] {
    display: none !important;
}
    </style>
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
        .project-tiles-view .project-card { display:flex; }
        .project-tile-card {
            width:100%;
            border-left-width:4px;
            border-left-style:solid;
            box-shadow:0 2px 8px rgba(0,0,0,.06);
            transition:transform .18s ease, box-shadow .18s ease;
        }
        .project-tile-card:hover { transform:translateY(-2px); box-shadow:0 6px 18px rgba(0,0,0,.10); }
        .project-tile-id { color:#3978de; font-size:12px; font-weight:600; }
        .project-tile-priority { background:#fff; border:1px solid; font-size:10px; font-weight:600; cursor:pointer; color:#000 !important; }
        .project-tile-status { display:block; white-space:nowrap; font-size:11px; font-weight:600; color:#000 !important; }

        /* Keep Priority/Status labels black; the icon/border still uses its configured color. */
        .project-priority-dropdown a,
        .project-status-dropdown a,
        .project-list-priority-dropdown a,
        .project-list-status-dropdown a { color:#000 !important; }
        .project-tile-title {
            color:#202124 !important; font-size:16px; font-weight:700; line-height:1.35; min-height:44px;
            display:-webkit-box; -webkit-line-clamp:2; -webkit-box-orient:vertical; overflow:hidden;
        }
        .project-tile-client, .project-tile-owner { color:#6c757d; font-size:12px; }
        .project-tile-summary { display:grid; grid-template-columns:repeat(3, 1fr); gap:8px; }
        .project-tile-summary > div { background:#f7f8fa; border:1px solid #edf0f2; border-radius:5px; padding:7px 5px; text-align:left; }
        .project-tile-summary span { display:block; color:#7b8794; font-size:9px; margin-bottom:2px; }
        .project-tile-summary strong { color:#202124; font-size:12px; }
        .project-tile-file { color:#3978de; font-size:22px; line-height:1; }
        .project-tile-date { color:#5f6368; font-size:10px; white-space:nowrap; }
        .project-tile-progress { width:80px; }
        .project-tile-percent { color:#5f6368; font-size:10px; min-width:30px; text-align:right; }
        .project-calendar-view { padding:12px; }
        .project-calendar-shell { background:#fff; border:1px solid #e4e8ed; border-radius:7px; overflow:hidden; }
        .project-calendar-toolbar { display:flex; align-items:center; justify-content:space-between; gap:12px; padding:10px 12px; border-bottom:1px solid #e4e8ed; }
        .project-calendar-title { margin:0; color:#202124; font-size:16px; font-weight:600; }
        .project-calendar-actions { display:flex; align-items:center; gap:5px; }
        .project-calendar-action { border:1px solid #dfe3e8; background:#fff; color:#495057; border-radius:4px; min-width:31px; height:29px; padding:0 8px; font-size:11px; }
        .project-calendar-action:hover { background:#f3eef7; color:#6b246d; border-color:#d8c2dc; }
        .project-calendar-table { width:100%; table-layout:fixed; border-collapse:collapse; margin:0; }
        .project-calendar-table th { padding:7px 5px; background:#f7f8fa; border-right:1px solid #e8ebef; border-bottom:1px solid #e4e8ed; color:#5f6368; text-align:center; font-size:11px; font-weight:600; }
        .project-calendar-table th:last-child { border-right:0; }
        .project-calendar-cell { height:118px; padding:5px; vertical-align:top; border-right:1px solid #e8ebef; border-bottom:1px solid #e8ebef; background:#fff; }
        .project-calendar-cell:last-child { border-right:0; }
        .project-calendar-cell.other-month { background:#fafbfc; }
        .project-calendar-day { display:flex; align-items:center; justify-content:flex-end; height:22px; color:#495057; font-size:11px; font-weight:600; }
        .project-calendar-cell.other-month .project-calendar-day { color:#b3bac3; }
        .project-calendar-day.today span { display:inline-flex; align-items:center; justify-content:center; width:22px; height:22px; border-radius:50%; background:#6b246d; color:#fff; }
        .project-calendar-events { display:flex; flex-direction:column; gap:3px; }
        .project-calendar-event { display:block; min-width:0; padding:4px 5px; background:#f6f7f9; border-left:3px solid #3978de; border-radius:3px; color:#202124; font-size:10px; line-height:1.2; text-decoration:none; }
        .project-calendar-event:hover { background:#eceff3; color:#202124; }
        .project-calendar-event-id { color:#3978de; font-weight:600; margin-right:3px; }
        .project-calendar-event-name { display:block; overflow:hidden; text-overflow:ellipsis; white-space:nowrap; margin-top:1px; }
        .project-calendar-more { color:#6b246d; font-size:10px; padding:1px 3px; }
        .project-calendar-empty { padding:35px 10px; color:#7b8794; text-align:center; }
        .project-calendar-layout { display:grid; grid-template-columns:minmax(0, 2fr) minmax(360px, 1fr); }
        .project-calendar-main { min-width:0; }
        .project-calendar-side { border-left:1px solid #e4e8ed; background:#fff; min-width:0; }
        .project-calendar-side-title { display:flex; align-items:center; justify-content:space-between; min-height:43px; padding:8px 10px; border-bottom:1px solid #e4e8ed; font-size:12px; font-weight:600; color:#202124; }
        .project-calendar-cell { cursor:pointer; }
        .project-calendar-cell.selected { box-shadow:inset 0 0 0 2px #6b246d; background:#fcf8fc; }
        .project-calendar-count { margin-top:4px; padding:6px; border-left:3px solid #3978de; background:#edf3ff; border-radius:2px; color:#202124; font-size:10px; line-height:1.45; }
        .project-calendar-count strong { display:block; color:#202124; }
        .project-calendar-side-scroll { max-height:704px; overflow:auto; }
        .project-calendar-detail { width:100%; border-collapse:collapse; table-layout:fixed; }
        .project-calendar-detail th { padding:7px 6px; background:#f7f8fa; border-bottom:1px solid #e4e8ed; border-right:1px solid #e8ebef; color:#202124; font-size:10px; font-weight:600; text-align:left; }
        .project-calendar-detail td { padding:8px 6px; border-bottom:1px solid #e8ebef; border-right:1px solid #e8ebef; color:#202124; font-size:10px; vertical-align:top; word-break:break-word; }
        .project-calendar-detail th:last-child, .project-calendar-detail td:last-child { border-right:0; }
        .project-calendar-detail-name { color:#202124; font-weight:600; text-decoration:none; }
        .project-calendar-detail-id { display:block; color:#3978de; margin-bottom:2px; }
        .project-calendar-detail-summary { display:block; margin-top:3px; font-size:9px; font-weight:400; line-height:1.35; }
        .project-calendar-detail-task { color:#3978de; }
        .project-calendar-detail-subtask { color:#18a957; }
        .project-calendar-owner { display:flex; align-items:center; gap:5px; }
        .project-calendar-owner img { width:24px; height:24px; border-radius:50%; object-fit:cover; flex:0 0 24px; }
        .project-calendar-status { display:inline-block; padding:3px 6px; border-radius:10px; font-size:9px; font-weight:600; white-space:nowrap; }
        .project-calendar-no-selection { padding:28px 10px; color:#7b8794; text-align:center; font-size:11px; }
        .top-list-sales li .tile { cursor:pointer; border:1px solid transparent; border-radius:5px; transition:background-color .15s ease, border-color .15s ease; }
        .top-list-sales li.status-filter-active .tile { background:#f6edf7; border-color:#8a3c8d; }
        .pms-project-situation-filter { min-width:165px; height:30px; border:1px solid #dfe3e8; border-radius:6px; background:#fff; color:#26313c; padding:0 28px 0 10px; font-size:12px; }
        @media (max-width: 767px) {
            .project-calendar-view { padding:8px; overflow-x:auto; }
            .project-calendar-shell { min-width:1000px; }
            .project-calendar-cell { height:105px; }
        }


        /* Project table timeline display only. Project data and save/update flow are untouched. */
        .project-timeline-cell { min-width:220px; }
        .project-timeline-wrap {
            position:relative;
            width:100%;
            min-width:190px;
            height:16px;
            margin:0;
            overflow:hidden;
            border-radius:4px;
            background:#e5e7eb;
            box-shadow:none;
            cursor:default;
        }
        .project-timeline-fill {
            height:100%;
            border-radius:4px 0 0 4px;
            transition:width .25s ease;
            background:#a9c6ff !important;
        }
        .project-timeline-wrap .dayswrapper {
            position:absolute;
            inset:0;
            z-index:2;
            display:flex;
            align-items:center;
            justify-content:center;
            padding:0 6px;
            pointer-events:none;
            font-size:10px;
            font-weight:600;
            line-height:16px;
            white-space:nowrap;
        }
        /* White text remains visible on the dark unfilled section. */
        .project-timeline-wrap .timeline-text-light {
            color:#000;
        }
        /* Dark text is clipped to the pastel-blue completed section. */
        .project-timeline-wrap .timeline-text-dark {
            color:#1f2937;
        }
        .project-timeline-wrap .totaldays { display:none; }
        .project-timeline-wrap:hover .fromtodate { display:none; }
        .project-timeline-wrap:hover .totaldays { display:inline; }
        .project-timeline-unavailable {
            display:block;
            color:#8a9199;
            font-size:10px;
            white-space:nowrap;
        }
        .project-timeline-tooltip .tooltip-inner {
            min-width:auto;
            max-width:180px;
            padding:6px 10px;
            text-align:left;
            white-space:nowrap;
        }
        @media (max-width:575.98px) {
            .project-tile-date { white-space:normal; }
            .project-tile-progress { width:60px; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <div class="main-wrapper"  ng-app="PMSerp" ng-controller="myprojectCtrl">
    <style>
        .pms-completion-backdrop { position: fixed; inset: 0; z-index: 2147483000; background: rgba(14, 30, 51, .40); display: none; align-items: center; justify-content: center; padding: 20px; }
        .pms-completion-backdrop.is-open { display: flex; }
        .pms-completion-dialog { width: min(760px, 96vw); height: min(680px, 90vh); background: white; border: 1px solid #159af0; border-radius: 12px; box-shadow: 0 16px 45px rgba(0,0,0,.18); display: flex; flex-direction: column; overflow: hidden; }
        .pms-completion-heading { display: flex; align-items: center; justify-content: space-between; padding: 14px 18px; border-bottom: 1px solid #dae5f1; font-size: 17px; font-weight: 600; }
        .pms-completion-heading button { border: 0; background: none; color: #38516e; font-size: 25px; line-height: 1; cursor: pointer; }
        .pms-completion-dialog iframe { flex: 1; width: 100%; border: 0; background: white; }
        .pms-completion-body { padding: 18px 24px; overflow-y: auto; flex: 1; }
        .pms-completion-body label { display: block; margin: 15px 0 7px; font-weight: 600; }
        .pms-completion-picker { position: relative; }
        .pms-completion-control { display: flex; flex-wrap: wrap; align-items: center; gap: 6px; width: 100%; border-bottom: 1px solid #b6cbe2; min-height: 40px; padding: 5px 2px; cursor: text; }
        .pms-completion-control:focus-within { border-bottom-color: #1394e5; }
        .pms-completion-chip { display: inline-flex; align-items: center; gap: 7px; max-width: 100%; padding: 4px 8px; background: #edf0f3; border-radius: 4px; color: #344455; }
        .pms-completion-chip span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
        .pms-completion-chip button { border: 0; padding: 0; background: transparent; color: #536476; font-size: 17px; line-height: 1; cursor: pointer; }
        .pms-completion-control input[type=text] { flex: 1 1 140px; min-width: 90px; border: 0; outline: 0; padding: 5px 2px; background: transparent; }
        .pms-completion-choices { position: absolute; top: 100%; left: 0; right: 0; z-index: 5; background: #fff; box-shadow: 0 8px 20px #263d5830; border: 1px solid #bdd0e5; max-height: 190px; overflow: auto; padding: 5px; }
        .pms-completion-empty { padding: 9px; color: #657688; }
        .pms-completion-choices .pms-completion-option { display: block; width: 100%; margin: 0; padding: 8px; border: 0; text-align: left; background: white; color: #15559d; cursor: pointer; }
        .pms-completion-choices .pms-completion-option:hover, .pms-completion-choices .pms-completion-option.is-selected { background: #edf6ff; }
        .pms-completion-actions { display: flex; justify-content: flex-end; gap: 9px; border-top: 1px solid #e0e8f2; padding: 15px 24px; }
        .pms-completion-actions button { border: 1px solid #b8cbe0; background: white; border-radius: 4px; padding: 7px 20px; }
        .pms-completion-actions button.primary { background: #138ce2; border-color: #138ce2; color: white; }
        .pms-completion-table-wrap { margin-top: 9px; overflow-x: auto; border: 1px solid #d6e3f2; border-radius: 4px; }
        .pms-completion-table { width: 100%; min-width: 620px; border-collapse: collapse; font-size: 13px; }
        .pms-completion-table th { background: #f4f9ff; color: #334b63; font-weight: 600; text-align: left; }
        .pms-completion-table th, .pms-completion-table td { padding: 9px 10px; border-right: 1px solid #e4ebf4; border-bottom: 1px solid #e4ebf4; vertical-align: middle; }
        .pms-completion-list-top{display:flex;justify-content:space-between;align-items:center;gap:10px;margin-bottom:17px}.pms-completion-list-top button{border:0;border-radius:5px;background:#168ce7;color:#fff;padding:8px 14px;cursor:pointer}.pms-completion-list-top button:disabled{opacity:.5}
        .pms-completion-table tr:last-child td { border-bottom: 0; }
        .pms-completion-table th:last-child, .pms-completion-table td:last-child { border-right: 0; }
    </style>
    <style>
        .pms-issue-backdrop{position:fixed;inset:0;z-index:1070;background:rgba(24,38,58,.42);display:none;align-items:center;justify-content:center;padding:18px}.pms-issue-backdrop.is-open{display:flex}
        .pms-issue-dialog{width:min(1120px,calc(100vw - 36px));height:min(690px,calc(100vh - 36px));display:flex;flex-direction:column;background:#fff;border:1px solid #cde0ed;border-radius:12px;box-shadow:0 22px 65px #14253b38;overflow:hidden}
        .pms-issue-head{display:flex;align-items:center;justify-content:space-between;padding:18px 24px;border-bottom:1px solid #e6edf2}.pms-issue-head h2{font-size:19px;margin:0}.pms-issue-head small{display:block;color:#6b8091;margin-top:5px}.pms-issue-head button{border:0;background:transparent;font-size:25px;color:#587189;cursor:pointer}
        .pms-issue-toolbar{display:flex;align-items:center;gap:10px;padding:14px 24px;border-bottom:1px solid #e6edf2}.pms-issue-toolbar input{margin-left:auto;width:min(275px,40vw);padding:8px 11px;border:1px solid #cbd8e3;border-radius:6px}.pms-issue-add{background:#168ced;color:#fff;border:0;border-radius:6px;padding:9px 14px;cursor:pointer}.pms-issue-count{color:#688096;font-size:12px}
        .pms-issue-table-wrap{overflow:auto;flex:1;margin:0 24px}.pms-issue-table{border-collapse:collapse;width:100%;min-width:1050px;font-size:12px}.pms-issue-table th,.pms-issue-table td{padding:11px 10px;border-bottom:1px solid #e5ecf2;text-align:left;white-space:nowrap}.pms-issue-table th{position:sticky;top:0;background:#f3f7fa;color:#375168;z-index:1}.pms-issue-table td.issue-title{min-width:190px;font-weight:600;white-space:normal}.pms-issue-table tr:hover td{background:#f7fbff}.pms-issue-empty{padding:50px;text-align:center;color:#6d8091}
        .pms-issue-actions{white-space:nowrap}.pms-issue-actions button{border:0;background:transparent;color:#2678b9;padding:3px 5px;cursor:pointer;font-size:15px}.pms-issue-actions button.delete{color:#c84444}.pms-issue-actions button:hover{background:#e8f2fb;border-radius:4px}.pms-issue-actions button:disabled{opacity:.4;cursor:default}
        .pms-issue-people{display:flex;align-items:center;padding-left:5px;min-width:45px}.pms-issue-person{position:relative;display:inline-flex;align-items:center;justify-content:center;width:25px;height:25px;margin-left:-5px;border:2px solid #fff;border-radius:50%;background:#3278dc;color:#fff;font-size:9px;font-weight:600;cursor:default}.pms-issue-person:nth-child(3n+2){background:#19a9a5}.pms-issue-person:nth-child(3n){background:#7568c9}.pms-issue-person.more{background:#e9eff8;color:#345a83}.pms-issue-person-tooltip{position:absolute;left:50%;bottom:calc(100% + 7px);transform:translateX(-50%);z-index:20;max-width:230px;padding:5px 8px;border-radius:4px;background:#30343b;color:#fff;font-size:11px;font-weight:400;line-height:1.4;white-space:normal;text-align:center;box-shadow:0 3px 9px #0003;visibility:hidden;opacity:0;pointer-events:none}.pms-issue-person:hover .pms-issue-person-tooltip,.pms-issue-person:focus .pms-issue-person-tooltip{visibility:visible;opacity:1}.pms-issue-people-cell{overflow:visible}
        .pms-issue-footer{display:flex;justify-content:space-between;align-items:center;padding:11px 24px;border-top:1px solid #e6edf2;color:#607587;font-size:12px}.pms-issue-footer button,.pms-issue-back{background:#fff;border:1px solid #cbd8e3;border-radius:5px;padding:5px 10px;cursor:pointer}.pms-issue-footer button:disabled{opacity:.4;cursor:default}.pms-issue-frame{width:100%;border:0;flex:1}.pms-completion-table td.pms-completion-linked-paths{white-space:normal;min-width:240px;max-width:350px}.pms-completion-linked-paths span{display:block;padding:3px 0;overflow-wrap:anywhere}.pms-completion-linked-paths span+span{border-top:1px solid #edf1f4}
        .pms-issue-form{flex:1;overflow:auto;padding:16px 24px}.pms-issue-form-context{display:flex;gap:24px;align-items:center;justify-content:space-between;padding:11px 14px;margin-bottom:15px;background:#f3f8fc;border:1px solid #d9e7f3;border-radius:7px;color:#2e5474}.pms-issue-form-context small{display:block;color:#6d879b}.pms-issue-form-context strong{font-size:12px}.pms-issue-form-context .kind{border-radius:20px;background:#e0f4eb;color:#13794d;padding:5px 10px;white-space:nowrap}.pms-issue-field{display:flex;flex-direction:column;gap:5px;position:relative;min-width:0}.pms-issue-field label{font-size:12px;font-weight:600;color:#35516a}.pms-issue-field input,.pms-issue-field select,.pms-issue-field textarea{width:100%;border:1px solid #c9d9e7;border-radius:5px;padding:8px 10px;min-height:35px;background:#fff;color:#263a4c;box-sizing:border-box}.pms-issue-field textarea{min-height:64px;resize:vertical}.pms-issue-form-grid{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:12px 14px}.pms-issue-form h3{font-size:12px;color:#38526a;margin:19px 0 11px;padding-bottom:5px;border-bottom:1px solid #e4ebf2}.pms-issue-multi{border:1px solid #c9d9e7;border-radius:5px;min-height:35px;display:flex;align-items:center;flex-wrap:wrap;gap:4px;padding:3px 6px}.pms-issue-multi input{flex:1;min-width:90px;border:0;padding:4px;outline:0;min-height:22px}.pms-issue-single{display:flex;align-items:center;border:1px solid #c9d9e7;border-radius:5px;background:#fff;min-height:35px}.pms-issue-single input{flex:1;width:100%;min-width:0;border:0;outline:0;padding:7px 9px}.pms-issue-single button{border:0;background:transparent;color:#71899c;padding:4px 9px;cursor:pointer}.pms-issue-single>span{padding:0 10px;color:#71899c}.pms-issue-person-chip{background:#e9f2fb;border-radius:4px;padding:3px 5px;color:#315e80;white-space:nowrap}.pms-issue-person-chip button{border:0;background:none;color:#667c8e;cursor:pointer}.pms-issue-person-list{position:absolute;top:100%;left:0;right:0;z-index:10;max-height:155px;overflow:auto;background:#fff;border:1px solid #c9d9e7;box-shadow:0 7px 18px #1c385933}.pms-issue-person-list button{display:block;width:100%;border:0;background:#fff;padding:8px;text-align:left}.pms-issue-person-list button:hover,.pms-issue-person-list button.active{background:#e9f2fb}.pms-issue-person-empty{padding:9px;color:#6d8091;font-size:12px}.pms-issue-form-actions{border-top:1px solid #e3eaf0;padding:12px 24px;display:flex;justify-content:flex-end;gap:8px}.pms-issue-form-actions button{border:1px solid #97b8d9;border-radius:5px;background:#fff;color:#1b75b7;padding:7px 15px}.pms-issue-form-actions button.primary{background:#178ce6;border-color:#178ce6;color:#fff}.pms-issue-form-actions button:disabled{opacity:.55}.pms-issue-form-error{color:#b42318;font-size:12px;margin:10px 0}.pms-issue-upload{padding:10px;border:1px dashed #b5cee2;border-radius:6px;margin-top:14px}@media(max-width:740px){.pms-issue-form-grid{grid-template-columns:1fr}.pms-issue-form-context{flex-wrap:wrap}}
    </style>
    <div class="pms-issue-backdrop" ng-class="{'is-open':issuePanel.open}" ng-click="closeIssueList()">
        <section class="pms-issue-dialog" role="dialog" aria-modal="true" aria-label="Issues" ng-click="$event.stopPropagation()">
            <header class="pms-issue-head"><div><h2>{{issuePanel.mode=='list' ? 'Issues' : (issueForm.id ? 'Edit Issue' : 'Add Issue')}}</h2><small>{{issuePanel.context.title}}</small></div><button type="button" aria-label="Close issues" ng-click="closeIssueList()"><i class="ri-close-line"></i></button></header>
            <div ng-if="issuePanel.mode=='list'" style="display:flex;flex-direction:column;min-height:0;flex:1">
                <div class="pms-issue-toolbar"><button type="button" class="pms-issue-add" ng-click="openIssueEditor()"><i class="ri-add-line"></i> Add Issue</button><span class="pms-issue-count">{{issueFilteredRows().length}} issue(s)</span><input type="search" ng-model="issuePanel.search" ng-change="issuePanel.page=1" placeholder="Search issues" aria-label="Search issues" /></div>
                <div class="pms-issue-table-wrap"><div ng-if="issuePanel.loading" class="pms-issue-empty">Loading issues...</div><div ng-if="issuePanel.error" class="pms-issue-empty" role="alert">{{issuePanel.error}} <button type="button" ng-click="refreshIssueList()">Retry</button></div>
                    <table class="pms-issue-table" ng-if="!issuePanel.loading && !issuePanel.error"><thead><tr><th>S.No</th><th>Issue</th><th>Owner</th><th>Team</th><th>Status</th><th>Priority</th><th>Assign To</th><th>Create Date</th><th>Target Date</th><th>Issue Type</th><th>Milestone</th><th>Actions</th></tr></thead><tbody><tr ng-repeat="issue in issuePageRows() track by $index">
                        <td>{{issue.number || ((issuePanel.page-1)*issuePanel.pageSize+$index+1)}}</td><td class="issue-title">{{issue.name}}</td><td class="pms-issue-people-cell"><div class="pms-issue-people" ng-if="issue.owners.length"><span class="pms-issue-person" ng-repeat="name in issue.owners | limitTo:3 track by $index" tabindex="0" aria-label="Owner: {{name}}">{{name.slice(0,2)}}<span class="pms-issue-person-tooltip" role="tooltip">{{name}}</span></span><span class="pms-issue-person more" ng-if="issue.owners.length>3" tabindex="0" aria-label="{{issue.owners.length-3}} more owners">+{{issue.owners.length-3}}<span class="pms-issue-person-tooltip" role="tooltip"><span ng-repeat="name in issue.owners.slice(3) track by $index">{{name}}{{$last ? '' : ', '}}</span></span></span></div><span ng-if="!issue.owners.length">—</span></td><td class="pms-issue-people-cell"><div class="pms-issue-people" ng-if="issue.teams.length"><span class="pms-issue-person" ng-repeat="name in issue.teams | limitTo:3 track by $index" tabindex="0" aria-label="Team: {{name}}">{{name.slice(0,2)}}<span class="pms-issue-person-tooltip" role="tooltip">{{name}}</span></span><span class="pms-issue-person more" ng-if="issue.teams.length>3" tabindex="0" aria-label="{{issue.teams.length-3}} more team members">+{{issue.teams.length-3}}<span class="pms-issue-person-tooltip" role="tooltip"><span ng-repeat="name in issue.teams.slice(3) track by $index">{{name}}{{$last ? '' : ', '}}</span></span></span></div><span ng-if="!issue.teams.length">-</span></td><td>{{issue.status || '-'}}</td><td>{{issue.priority || '-'}}</td><td>{{issue.assignee || '-'}}</td><td>{{issue.created || '-'}}</td><td>{{issue.target || '-'}}</td><td>{{issue.issueType || '-'}}</td><td>{{issue.milestone || '-'}}</td><td class="pms-issue-actions"><button type="button" ng-if="issue.canEdit" title="Edit issue" aria-label="Edit {{issue.name}}" ng-click="editIssue(issue)" ng-disabled="issuePanel.deletingId==issue.id"><i class="ri-edit-line"></i></button><button type="button" ng-if="issue.canDelete" class="delete" title="Delete issue" aria-label="Delete {{issue.name}}" ng-click="deleteIssue(issue)" ng-disabled="issuePanel.deletingId==issue.id"><i class="ri-delete-bin-line"></i></button></td></tr></tbody></table><div ng-if="!issuePanel.loading && !issuePanel.error && !issueFilteredRows().length" class="pms-issue-empty">No issues found for this {{issuePanel.context.type}}.</div></div>
                <footer class="pms-issue-footer"><span>Showing {{issueFilteredRows().length ? ((issuePanel.page-1)*issuePanel.pageSize+1) : 0}}-{{issuePanel.page*issuePanel.pageSize < issueFilteredRows().length ? issuePanel.page*issuePanel.pageSize : issueFilteredRows().length}} of {{issueFilteredRows().length}}</span><span><button type="button" ng-disabled="issuePanel.page<=1" ng-click="issuePanel.page=issuePanel.page-1">Previous</button> Page {{issuePanel.page}} <button type="button" ng-disabled="issuePanel.page*issuePanel.pageSize>=issueFilteredRows().length" ng-click="issuePanel.page=issuePanel.page+1">Next</button></span></footer>
            </div>
            <div ng-if="issuePanel.mode=='add'" style="display:flex;flex-direction:column;min-height:0;flex:1">
                <div class="pms-issue-form" ng-click="closeIssuePersonList($event)">
                    <div class="pms-issue-form-context"><div><small>PROJECT</small><strong>{{issuePanel.context.title}}</strong></div><span class="kind">Project issue</span></div>
                    <div ng-if="issuePanel.loadingOptions" class="pms-issue-empty">Loading form options...</div>
                    <div class="pms-issue-field"><label for="pmsIssueTitle">Issue title *</label><input id="pmsIssueTitle" type="text" ng-model="issueForm.Issues" maxlength="250" placeholder="Enter a short issue title" /></div>
                    <div class="pms-issue-field" style="margin-top:12px"><label for="pmsIssueDescription">Description</label><textarea id="pmsIssueDescription" ng-model="issueForm.Description" placeholder="Describe the problem and expected outcome"></textarea></div>
                    <h3>People &amp; schedule</h3>
                    <div class="pms-issue-form-grid">
                        <div class="pms-issue-field"><label for="pmsIssueOwnerSearch">Owner</label><div class="pms-issue-single"><input id="pmsIssueOwnerSearch" type="search" placeholder="Search owner" ng-model="issueMulti.search.Owner" ng-focus="openIssuePeople('Owner')" ng-change="issueMulti.active=-1;issueForm.Owner=''" ng-keydown="issuePersonKeydown('Owner',$event)" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" role="combobox" aria-autocomplete="list" aria-expanded="{{issueMulti.open=='Owner'}}" /><button type="button" ng-if="issueForm.Owner" title="Clear owner" aria-label="Clear owner" ng-click="issueForm.Owner='';issueMulti.search.Owner=''"></button><span ng-if="!issueForm.Owner" aria-hidden="true"><i class="ri-arrow-down-s-line"></i></span></div><div class="pms-issue-person-list" ng-if="issueMulti.open=='Owner'"><button type="button" ng-repeat="person in issuePersonMatches('Owner') track by person.id" ng-class="{'active':issueMulti.active==$index}" ng-mouseenter="issueMulti.active=$index" ng-click="chooseIssuePerson('Owner',person)">{{person.name}} <span ng-if="issueForm.Owner==person.id"><i class="ri-check-line"></i></span></button><div class="pms-issue-person-empty" ng-if="!issuePersonMatches('Owner').length">No matching owners</div></div></div>
                        <div class="pms-issue-field"><label>Team</label><div class="pms-issue-multi" ng-click="openIssuePeople('team')"><span class="pms-issue-person-chip" ng-repeat="id in issueForm.team track by id">{{issuePersonName(id)}} <button type="button" ng-click="toggleIssuePerson('team',{id:id});$event.stopPropagation()"><i class="ri-close-line"></i></button></span><input type="search" placeholder="Search team" aria-label="Search team" ng-model="issueMulti.search.team" ng-focus="openIssuePeople('team')" ng-change="issueMulti.active=-1" ng-keydown="issuePersonKeydown('team',$event)" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" /></div><div class="pms-issue-person-list" ng-if="issueMulti.open=='team'"><button type="button" ng-repeat="person in issuePersonMatches('team') track by person.id" ng-class="{'active':issueMulti.active==$index}" ng-mouseenter="issueMulti.active=$index" ng-click="chooseIssuePerson('team',person)">{{person.name}} <span ng-if="issueForm.team.indexOf(person.id)>=0"><i class="ri-check-line"></i></span></button><div class="pms-issue-person-empty" ng-if="!issuePersonMatches('team').length">No matching team members</div></div></div>
                        <div class="pms-issue-field"><label for="pmsIssueAssigneeSearch">Assign to *</label><div class="pms-issue-single"><input id="pmsIssueAssigneeSearch" type="search" placeholder="Search assignee" ng-model="issueMulti.search.AssignTo" ng-focus="openIssuePeople('AssignTo')" ng-change="issueMulti.active=-1;issueForm.AssignTo=''" ng-keydown="issuePersonKeydown('AssignTo',$event)" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" role="combobox" aria-autocomplete="list" aria-expanded="{{issueMulti.open=='AssignTo'}}" /><button type="button" ng-if="issueForm.AssignTo" title="Clear assignee" aria-label="Clear assignee" ng-click="issueForm.AssignTo='';issueMulti.search.AssignTo=''"></button><span ng-if="!issueForm.AssignTo" aria-hidden="true"><i class="ri-arrow-down-s-line"></i></span></div><div class="pms-issue-person-list" ng-if="issueMulti.open=='AssignTo'"><button type="button" ng-repeat="person in issuePersonMatches('AssignTo') track by person.id" ng-class="{'active':issueMulti.active==$index}" ng-mouseenter="issueMulti.active=$index" ng-click="chooseIssuePerson('AssignTo',person)">{{person.name}} <span ng-if="issueForm.AssignTo==person.id"><i class="ri-check-line"></i></span></button><div class="pms-issue-person-empty" ng-if="!issuePersonMatches('AssignTo').length">No matching assignees</div></div></div>
                        <div class="pms-issue-field"><label for="pmsIssueStart">Start date</label><input id="pmsIssueStart" type="date" ng-model="issueForm.StartDate" ng-model-options="{timezone:'UTC'}" /></div>
                        <div class="pms-issue-field"><label for="pmsIssueEnd">End date</label><input id="pmsIssueEnd" type="date" ng-model="issueForm.EndDate" ng-model-options="{timezone:'UTC'}" /></div>
                        <div class="pms-issue-field"><label for="pmsIssueReminder">Reminder</label><select id="pmsIssueReminder" ng-model="issueForm.Reminder"><option value="0">No reminder</option><option value="1">Based on due date</option><option value="2">Specific date</option></select></div>
                        <div class="pms-issue-field" ng-if="issueForm.Reminder=='1'"><label>Reminder type</label><select ng-model="issueForm.ReminderType"><option value="">Select type</option><option value="1">Daily</option><option value="2">On due date</option><option value="3">Days before due date</option></select></div>
                        <div class="pms-issue-field" ng-if="issueForm.Reminder=='1' && issueForm.ReminderType=='3'"><label>Days before</label><input type="number" min="0" ng-model="issueForm.Days" /></div>
                        <div class="pms-issue-field" ng-if="issueForm.Reminder=='2'"><label>Choose date</label><input type="date" ng-model="issueForm.ChooseDate" ng-model-options="{timezone:'UTC'}" /></div>
                        <div class="pms-issue-field" ng-if="issueForm.Reminder!='0'"><label>Time</label><input type="time" ng-model="issueForm.Time" ng-model-options="{timezone:'UTC'}" /></div>
                        <div class="pms-issue-field" ng-if="issueForm.Reminder!='0'"><label>Notify user</label><select ng-model="issueForm.Notifyusers" ng-options="person.id as person.name for person in issueFormOptions.employees"><option value="">Select user</option></select></div>
                    </div>
                    <h3>Classification</h3>
                    <div class="pms-issue-form-grid"><div class="pms-issue-field"><label>Status *</label><select ng-model="issueForm.IssueStatus" ng-options="option.id as option.name for option in issueFormOptions.statuses"><option value="">Select status</option></select></div><div class="pms-issue-field"><label>Priority *</label><select ng-model="issueForm.Priority" ng-options="option.id as option.name for option in issueFormOptions.priorities"><option value="">Select priority</option></select></div><div class="pms-issue-field"><label>Issue type *</label><select ng-model="issueForm.IssueType" ng-options="option.id as option.name for option in issueFormOptions.types"><option value="">Select issue type</option></select></div><div class="pms-issue-field"><label>Replicability *</label><select ng-model="issueForm.Replicability" ng-options="option.id as option.name for option in issueFormOptions.replicabilities"><option value="">Select option</option></select></div></div>
                    <div class="pms-issue-upload"><label for="pmsIssueFiles">Add files</label><input id="pmsIssueFiles" type="file" multiple aria-label="Attach issue files" /></div>
                    <p class="pms-issue-form-error" ng-if="issuePanel.formError" role="alert">{{issuePanel.formError}}</p>
                </div>
                <footer class="pms-issue-form-actions"><button type="button" ng-click="backToIssueList()">Cancel</button><button type="button" class="primary" ng-disabled="issuePanel.saving || issuePanel.loadingOptions" ng-click="saveIssueForm(false)">{{issuePanel.saving ? 'Saving…' : 'Save'}}</button><button type="button" class="primary" ng-if="!issueForm.id" ng-disabled="issuePanel.saving || issuePanel.loadingOptions" ng-click="saveIssueForm(true)">Save &amp; Add</button></footer>
            </div>
        </section>
    </div>
    <div id="pmsCompletionDependencyPopup" class="pms-completion-backdrop" ng-class="{'is-open':completionDependency.open}" ng-click="closeCompletionDependencyPopup()">
        <div class="pms-completion-dialog" role="dialog" aria-modal="true" aria-label="Add completion dependency" ng-click="completionDependencyOutsideClick($event);$event.stopPropagation()">
            <div class="pms-completion-heading"><span>{{completionDependency.mode=='list' ? 'Dependencies' : (completionDependency.mode=='edit' ? 'Edit completion dependency' : 'Add completion dependency')}}</span><button type="button" aria-label="Close" ng-click="closeCompletionDependencyPopup()"><i class="ri-close-line"></i></button></div>
            <div class="pms-completion-body">
                <div ng-if="completionDependency.mode=='list'">
                    <div class="pms-completion-list-top"><span>For: <strong>{{completionDependency.name}}</strong> ({{completionDependency.type}})</span><button type="button" ng-click="addCompletionDependency()" ng-disabled="!completionDependency.milestones.length">+ Add dependency</button></div>
                <div style="margin-top:23px;border-top:1px solid #dce7f3;padding-top:12px"><strong>Existing dependencies</strong><div ng-if="completionDependency.loadingExisting" style="margin-top:8px">Loading...</div><div class="pms-completion-table-wrap" ng-if="!completionDependency.loadingExisting"><table class="pms-completion-table"><thead><tr><th>Project</th><th>Dependency on</th><th>Action</th></tr></thead><tbody><tr ng-repeat="existing in completionDependency.existing track by existing.id"><td>{{completionDependency.name}}</td><td class="pms-completion-linked-paths" title="{{existing.name}}"><span ng-repeat="path in existing.names track by $index">{{path}}</span></td><td style="white-space:nowrap"><button type="button" class="btn btn-sm btn-light" ng-click="editCompletionDependency(existing)" title="Edit dependency" aria-label="Edit dependency"><i class="ri-edit-line"></i></button> <button type="button" class="btn btn-sm btn-light" ng-click="deleteCompletionDependency(existing)" title="Delete dependency" aria-label="Delete dependency"><i class="ri-delete-bin-line"></i></button></td></tr><tr ng-if="!completionDependency.existing.length"><td colspan="3" style="text-align:center;color:#6b7f91">No dependencies added.</td></tr></tbody></table></div></div>
                </div>
                <div ng-if="completionDependency.mode!='list'">
                <div>Project: <strong>{{completionDependency.name}}</strong></div>
                <label>Milestone *</label>
                <div class="pms-completion-picker" ng-click="$event.stopPropagation()">
                    <div class="pms-completion-control" ng-click="completionDependency.menu='milestone'">
                        <span class="pms-completion-chip" ng-repeat="option in completionDependency.selected.milestone track by option.Value"><span>{{option.Text}}</span><button type="button" title="Remove {{option.Text}}" aria-label="Remove {{option.Text}}" ng-click="toggleCompletionDependency('milestone',option);$event.stopPropagation()">x</button></span>
                        <input type="text" ng-model="completionDependency.search.milestone" placeholder="Select milestones..." aria-label="Search milestones" ng-focus="completionDependency.menu='milestone'" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" />
                    </div>
                    <div class="pms-completion-choices" ng-if="completionDependency.menu=='milestone'"><button type="button" class="pms-completion-option" ng-repeat="option in completionDependency.milestones | filter:completionDependency.search.milestone track by option.Value" ng-class="{'is-selected':completionDependencySelected('milestone',option.Value)}" ng-click="toggleCompletionDependency('milestone',option);$event.stopPropagation()">{{option.Text}} <span ng-if="completionDependencySelected('milestone',option.Value)" style="float:right"><i class="ri-check-line"></i></span></button><div ng-if="!(completionDependency.milestones | filter:completionDependency.search.milestone).length" class="pms-completion-empty">No matches</div></div>
                </div>
                <div ng-if="completionDependency.tasks.length"><label>Task</label>
                <div class="pms-completion-picker" ng-click="$event.stopPropagation()">
                    <div class="pms-completion-control" ng-click="completionDependency.menu='task'">
                        <span class="pms-completion-chip" ng-repeat="option in completionDependency.selected.task track by option.Value"><span>{{option.Text}}</span><button type="button" title="Remove {{option.Text}}" aria-label="Remove {{option.Text}}" ng-click="toggleCompletionDependency('task',option);$event.stopPropagation()"><i class="ri-close-line"></i></button></span>
                        <input type="text" ng-model="completionDependency.search.task" placeholder="Select tasks..." aria-label="Search tasks" ng-focus="completionDependency.menu='task'" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" />
                    </div>
                    <div class="pms-completion-choices" ng-if="completionDependency.menu=='task'"><button type="button" class="pms-completion-option" ng-repeat="option in completionDependency.tasks | filter:completionDependency.search.task track by option.Value" ng-class="{'is-selected':completionDependencySelected('task',option.Value)}" ng-click="toggleCompletionDependency('task',option);$event.stopPropagation()">{{option.Text}} <span ng-if="completionDependencySelected('task',option.Value)" style="float:right"><i class="ri-check-line"></i></span></button><div ng-if="!(completionDependency.tasks | filter:completionDependency.search.task).length" class="pms-completion-empty">No matches</div></div>
                </div>
                </div>
                <div ng-if="completionDependency.subtasks.length"><label>Subtask</label>
                <div class="pms-completion-picker" ng-click="$event.stopPropagation()">
                    <div class="pms-completion-control" ng-click="completionDependency.menu='subtask'">
                        <span class="pms-completion-chip" ng-repeat="option in completionDependency.selected.subtask track by option.Value"><span>{{option.Text}}</span><button type="button" title="Remove {{option.Text}}" aria-label="Remove {{option.Text}}" ng-click="toggleCompletionDependency('subtask',option);$event.stopPropagation()"><i class="ri-close-line"></i></button></span>
                        <input type="text" ng-model="completionDependency.search.subtask" placeholder="Select subtasks..." aria-label="Search subtasks" ng-focus="completionDependency.menu='subtask'" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" />
                    </div>
                    <div class="pms-completion-choices" ng-if="completionDependency.menu=='subtask'"><button type="button" class="pms-completion-option" ng-repeat="option in completionDependency.subtasks | filter:completionDependency.search.subtask track by option.Value" ng-class="{'is-selected':completionDependencySelected('subtask',option.Value)}" ng-click="toggleCompletionDependency('subtask',option);$event.stopPropagation()">{{option.Text}} <span ng-if="completionDependencySelected('subtask',option.Value)" style="float:right"><i class="ri-check-line"></i></span></button><div ng-if="!(completionDependency.subtasks | filter:completionDependency.search.subtask).length" class="pms-completion-empty">No matches</div></div>
                </div>
                </div>
                </div>
                <p ng-if="completionDependency.error" style="color:#b42318;margin-top:16px" role="alert">{{completionDependency.error}}</p>
                <p ng-if="completionDependency.notice" style="color:#18834b;margin-top:16px" role="status">{{completionDependency.notice}}</p>
            </div>
            <div class="pms-completion-actions"><button type="button" ng-if="completionDependency.mode!='list'" ng-click="backToCompletionDependencyList()">Back to dependencies</button><button type="button" ng-if="completionDependency.mode=='list'" ng-click="closeCompletionDependencyPopup()">Close</button><button type="button" ng-if="completionDependency.mode!='list'" class="primary" ng-disabled="completionDependency.saving || completionDependency.loadingTasks || completionDependency.loadingSubtasks" ng-click="saveCompletionDependency()">{{completionDependency.saving?'Saving…':(completionDependency.editingId?'Update':'Save')}}</button></div>
        </div>
    </div>

        <div id="toast" class="pms-toast">
            <div id="toastMessage"></div>
            <div class="pms-progress">
                <div id="toastBar" class="pms-progress-bar"></div>
            </div>
        </div>
        <div id="toasterror" class="pms-toasterror">
            <div id="toastMessageerror"></div>
            <div class="pms-progresserror">
                <div id="toastBarerror" class="pms-progress-barerror"></div>
            </div>
        </div>
        <div class="sub-header">
            <div class="row">
                <div class="col-xxl-3 col-lg-3 col-md-4 col-sm-12">
                    <div class="sub-left">
                        <div class="sub-page-title">
                            Project
                        </div>
                       <%if (ischeckCreate == 1)
                                          {%>
                        <a href="javascript:void(0);" type="button" class="sub-soft-btn me-3" data-bs-toggle="offcanvas" data-bs-target="#addnewproject" ng-click="openproject('0','new')"><span data-bs-toggle="tooltip" title="+ Add Project"><i class="ri-add-line"></i></span></a>
                        <%} %>

                        <div class="d-flex align-items-center gap-1">
                            <!-- CALENDAR ICON -->
            <i class="ri-calendar-line text-muted ms-1"
               style="font-size:16px;line-height:1;"></i>

            <!-- DATE RANGE INPUT -->
            <input type="text" id="daterange" readonly
                   style="border:none; background:transparent;cursor:pointer" />

            <!-- ARROW -->
            <i class="ri-arrow-down-s-line"
               style="font-size:18px;line-height:1;"></i>
                            <%--<i class="ri-calendar-line text-muted" style="font-size: 20px;"></i>
                            <span style="font-size: 15px;">2025-2026</span>
                            <i class="ri-arrow-down-s-line" style="font-size: 18px;"></i>--%>
                        </div>
                    </div>
                </div>
                <div class="col-xxl-6 col-lg-6 col-md-8 col-sm-12">
                <div class="sub-left">
                    <ul class="top-list-sales">
                        <li class="d-flex align-items-center">
                            <select class="pms-project-situation-filter"
                                    ng-model="projectFilter.situationId"
                                    ng-change="applyProjectSituationFilter()"
                                    title="Project Situation">
                                <option value="all">All Project Situations</option>
                                <option value="none">No Project Situation</option>
                                <option ng-repeat="situation in projectSituationLists track by situation.id"
                                        value="{{situation.id}}">
                                    {{situation.name}}
                                </option>
                            </select>
                        </li>
                       </ul>
                    </div>
                </div>
                <div class="col-xxl-3 col-lg-3 col-md-6 col-sm-12">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                       <div class="search_box me-2"  ng-show="freesearch=='1'">                           
                           <input type="text" class="form-control" ng-model="searchText" id="txtsrch" placeholder="Search..." />
                            <i class="ri-search-line"></i>
                        </div>
                        <a href="javascript:void(0);" type="button" class="sub-soft-btn" data-bs-toggle="tooltip" title="Search" ng-click="viewsearch()"><i class="ri-search-line"></i></a>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active': view == 'table'}" ng-attr-aria-pressed="{{view == 'table'}}" data-bs-toggle="tooltip" title="List View" ng-click="setView('table')"><i class="ri-list-check"></i></a>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active': view == 'tiles'}" ng-attr-aria-pressed="{{view == 'tiles'}}" data-bs-toggle="tooltip" title="Tiles View" ng-click="setView('tiles')"><i class="ri-layout-grid-fill"></i></a>
                        <a href="javascript:void(0);" class="sub-soft-btn" ng-class="{'active': view == 'calendar'}" ng-attr-aria-pressed="{{view == 'calendar'}}" data-bs-toggle="tooltip" title="Calendar" ng-click="setView('calendar')"><i class="bx bx-calendar"></i></a>
                        <button class="sub-soft-btn" data-bs-toggle="tooltip" title="Filter"><i class="ri-filter-line"></i></button>
                        <div class="btn-group d-block">
                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="sub-soft-btn"><i class="ri-more-fill"></i></a>
                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                <ul class='sttus-list'>
                                    <li><a class='' href='#'><i class="bx bx-download fs-16 align-middle me-1"></i> Download Report</a></li>
                                    <li><a class='' href='javascript:void(0);'><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Project</a></li>
                                    <li><a class='' href='javascript:void(0);' data-bs-toggle="offcanvas" data-bs-target="#addnewproject" ng-click="openproject('0','new')"><i class="bx bx-plus fs-16 align-middle me-1"></i> Add Project</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row mt-1">
                 <div class="col-xxl-2 col-lg-2 col-md-2 col-sm-12">
                    
                </div>
                <div class="col-xxl-8 col-lg-8 col-md-8 col-sm-12">
                    <ul class="top-list-sales">
                       
                        <li ng-class="{'status-filter-active': !selectedProjectStatusId}">
                            <a href="javascript:void(0);" ng-click="setProjectStatusFilter(''); $event.preventDefault();">
                                <div class="tile">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <div class="tile-icon"><i class="ri-folder-2-line"></i></div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>Total Project</span></div>
                                            <div class="tile-body">{{mytotalprojectlist}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        <li ng-repeat="c in myprojectlist_bystatus"
                            ng-class="{'status-filter-active': selectedProjectStatusId == c.statusid}">
                            <a href="javascript:void(0);" ng-click="setProjectStatusFilter(c.statusid); $event.preventDefault();">
                                <div class="tile">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <div class="tile-icon" ng-if="c.statusicon==''" ng-style="{'color':c.statuscolor }"><i class="ri-folder-2-line"></i></div>
                                            <div class="tile-icon" ng-if="c.statusicon" ng-style="{'color':c.statuscolor }"><i ng-class="c.statusicon"></i>
</div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>{{c.statusname}}</span></div>
                                            <div class="tile-body">{{c.total}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        
                    </ul>
                </div>
                <div class="col-xxl-2 col-lg-2 col-md-2 col-sm-12">
                    
                </div>
            </div>
        </div>
        <div ng-if="view == 'table'">
        
        <div class="wrapper_inner p-0 project-data-wrapper"
             ng-cloak
             ng-show="projectDataReady && !isLoading && projectlist.length > 0">
            <div class="table-responsive-new">
                <table id='tbDetails' class="table table-hover table-striped tbl_new project-table">
                    <thead>
                        <tr>
                            <th class="color_border"></th>
                            <th style="width: 40px;">S.No.</th>
                            <th>Client Name</th>
                            <th>ID</th>
                            <th style="min-width:250px;">Project Name</th>
                            <th style="min-width:220px;">Timeline</th>
                            <th style="min-width:120px;">Status</th>
                            <th>Project Situation</th>
                            <th>Priority</th>
                            <th>Extended Timeline</th>
                            <th>Completion %</th>
                            <th style="min-width:120px;">Project Manager</th>
                            <th style="min-width:120px;">Team</th>
                            <th>Milestones</th>
                            <th>Task</th>
                            <th>Issues</th>
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Budget</th>
                            <th>Actual</th>
                            <th style="min-width:250px;">Project Address</th>
                            <th>Award Date</th>
                            <th>Approval</th>
                            <th>Approval Date</th>
                            <th>Files</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="c in projectlist | filter:projectStatusFilter | orderBy:sortColumn:reverseSort | filter:searchText | itemsPerPage: pageSize track by c.id " current-page="currentPage">
                            <td class="color_border" style="border-left-color:#ffc300 !important;">
                                <div class="btn-group d-block">
                                    <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-more-2-fill"></i></a>
                                    <div class="dropdown-menu p-0" style="border-radius:0;">
                                        <ul class='sttus-list'>
                                            <li><a href="javascript:void(0);" ng-click="openProjectDependency(c,$event)"><i class="ri-parent-line fs-16 align-middle me-1"></i> Dependency</a></li>
                                            <li><a href="javascript:void(0);" ng-click="openProjectIssue(c,$event)"><i class="bx bx-error-circle fs-16 align-middle me-1"></i> Add Issue</a></li>
                                             <%if (ischeckEdit == 1)
                                          {%>
                       <li><a class='' href='javascript:void(0);' data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadprojectfiles(c,$index)"><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Documents</a></li>
                                            <li><a href="javascript:void(0);" type="button" data-bs-toggle="offcanvas" data-bs-target="#addnewproject" ng-click="openproject(c.id,'update')"><i class="bx bx-edit fs-16 align-middle me-1"></i> Edit Project</a></li>
                        <%} %>
                                             <%if (ischeckCreate == 1)
                                          {%>
                                            <li><a class='' href='javascript:void(0);' ng-click="saveproject_clone(c)"><i class="bx bx-copy fs-16 align-middle me-1"></i> Clone</a></li>
                                            <%} %>


                                            <%--<li><a class='' href='javascript:void(0);' id="text"><i class="bx bx-palette fs-16 align-middle me-1"></i> Change Color</a></li>
                                            <li><a class='' href='#'><i class="bx bx-archive fs-16 align-middle me-1"></i> Hold</a></li>--%>
                                             <%if (ischeckDelete == 1)
                                          {%>
                       <li><a class='text-danger' href='javascript:void(0);' ng-click="deletedataproject(c)"><i class="bx bx-trash fs-16 align-middle me-1"></i> Delete</a></li>
                        <%} %>
                                            
                                        </ul>
                                        <input type="color" id="colorPicker">
                                    </div>
                                </div>
                            </td>
                            <td>{{ ((currentPage - 1) * pageSize) + $index + 1 }}</td>
                            <td>{{c.ClientName}}</td>
                            <%--<td ><a href="javascript:void(0);" ng-click="loadassignform('ifassign','+{{c.id}}+');$event.preventDefault()">{{c.ProjectID}}</a></td>--%>
                            <td ><a href="/pms/assignprojects/?id={{c.id}}">{{c.ProjectID}}</a></td>
                            <td><a href="/pms/assignprojects/?id={{c.id}}" style="color:#000 !important;"><b>{{c.ProjectName}}</b></a></td>
                            <td class="project-timeline-cell">
                                <div class="progress progress-xl position-relative project-timeline-wrap"
                                     ng-if="getProjectTimelineMetrics(c).valid"
                                     data-bs-toggle="tooltip"
                                     data-bs-html="true"
                                     data-bs-placement="top"
                                     data-bs-trigger="hover"
                                     data-bs-custom-class="project-timeline-tooltip"
                                     data-bs-title="{{getProjectTimelineTooltip(c)}}"
                                     ng-mouseenter="refreshProjectTimelineTooltip($event, c)">
                                    <div class="progress-bar bg-secondary project-timeline-fill"
                                         role="progressbar"
                                         ng-style="{'width': getProjectTimelinePercent(c) + '%'}"
                                         aria-valuenow="{{getProjectTimelinePercent(c)}}"
                                         aria-valuemin="0"
                                         aria-valuemax="100"></div>
                                    <div class="dayswrapper timeline-text-light">
                                        <span class="fromtodate">{{getProjectTimelineDateRange(c)}}</span>
                                        <span class="totaldays">{{getProjectTimelineMetrics(c).totalDays}} days</span>
                                    </div>
                                    <div class="dayswrapper timeline-text-dark"
                                         ng-style="{
                                             'clip-path':'inset(0 ' + (100 - getProjectTimelinePercent(c)) + '% 0 0)',
                                             '-webkit-clip-path':'inset(0 ' + (100 - getProjectTimelinePercent(c)) + '% 0 0)'
                                         }">
                                        <span class="fromtodate">{{getProjectTimelineDateRange(c)}}</span>
                                        <span class="totaldays">{{getProjectTimelineMetrics(c).totalDays}} days</span>
                                    </div>
                                </div>
                                <span class="project-timeline-unavailable"
                                      ng-if="!getProjectTimelineMetrics(c).valid">-</span>
                            </td>
                            <td ng-style="{'background-color': c.status_color}" style="padding:0;">
                                <%--<div class="btn-group d-block">--%>
                                <div style="position:relative;text-align:center;width:100%;height:100%;">
                                    <a href ='javascript:void(0);' type='button' data-bs-toggle='dropdown' data-bs-auto-close='true'
                                       class='d-flex align-items-center justify-content-center w-100 h-100'
                                       ng-class="{'pms-no-edit': !canEdit('project')}"
                                       style="min-height:31px;font-size:10px;font-weight:500;color:black;"
                                       ng-click="c._oldcomplete_value = c.complete_value;c._oldpending_value = c.pending_value;">
                                        {{ c.status_name }}
                                    </a>
                                    <%--<a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="d-block" ng-class="{'pms-no-edit': !canEdit('project')}" ng-style="{'color':c.status_color}" ng-click="
      c._oldcomplete_value = c.complete_value;
      c._oldpending_value = c.pending_value;
      
   "><i class='bx bxs-circle'></i>{{c.status_name }}</a>--%>
                                    <div class="dropdown-menu p-0 project-list-status-dropdown" style="border-radius:0;">
                                        <ul class='sttus-list'>
                                            <%--<li><a class='text-warning' href='#'><i class='bx bxs-circle' ng-style="{'color':c.status_color}"></i>{{c.status_name }}</a></li>--%>
                                           <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c,newstatus); $event.preventDefault()" >
                 <div class='dropdown-item p-0'>
                   
                     <a href="javascript:void(0);"
   style="color:#000 !important;"
   ng-click="selectStatus(c,newstatus); $event.preventDefault()">

    <i class="ri-checkbox-blank-fill"
       ng-style="{'color': (!newstatus.colorname || newstatus.colorname == '' || newstatus.colorname == '#FFFFFF') ? '#000' : newstatus.colorname}"></i>
    {{newstatus.name}}
</a>
                    </div>
    </li>
                                        </ul>
                                    </div>
                                </div>
                            </td>
                            <td ng-style="{'background-color': c.CriticalLabelcolor}">
                              <div style="text-align:center">{{c.CriticalLabel }}</div>
                            </td>
                            <td ng-style="{'background-color': c.priority_color}" style="padding:0;">
                                <%--<div class="badge bg-danger-subtle text-danger priority">High</div>--%>
                                <div style="position:relative;text-align:center;width:100%;height:100%;">
    <a href ='javascript:void(0);' type='button' data-bs-toggle='dropdown' data-bs-auto-close='true'
       class='d-flex align-items-center justify-content-center w-100 h-100'
       ng-class="{'pms-no-edit': !canEdit('project')}"
       style="min-height:31px;font-size:10px;font-weight:500;color:black;">
        {{ c.priority_name }}
    </a>
    <%--<a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;" > {{ c.priority_name }}</a>--%>
   <div class='dropdown-menu lead_form_body p-0 project-list-priority-dropdown' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name" ng-click="selectPriority(c,newpriority); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='ri-checkbox-blank-fill' ng-style="{'color':newpriority.colorname}" ></i> {{newpriority.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
    </div> 
                            </td>
                            <td><div class="d-flex" style="width:100%">
{{ formatDateOnly(c.lastextendstartdate) }} - {{ formatDateOnly(c.lastextendenddate) }}
    <!-- LEFT SIDE (Progress) -->
    <div class="flex-grow-1 align-self-center me-2" >
        <div class="extended_time" style="display:none">
            
            <a href="javascript:void(0);" class="timesheet-wrap" ng-repeat="etl in c.extendedlists">

                <div class="progress progress-lg bg-dark position-relative me-1"
                     data-bs-toggle="tooltip"                    
                     data-bs-title="{{etl.extended_from}} - {{etl.extended_to}} ({{etl.delayed_days}} {{etl.extended_type}})">

                    <div class="progress-bar bg-secondary"
                         role="progressbar"
                         ng-style="{'width':'100%'}"
                         aria-valuemax="100">

                        {{etl.extended_days}} days
                    </div>

                </div>

            </a>

        </div>
    </div>

    <!-- RIGHT SIDE (+ BUTTON) -->
    <div class="gap-1 flex-shrink-0 align-self-center">
<a href="javascript:void(0);"
       type="button"             
       data-bs-toggle="modal"
       data-bs-target="#extendedhistory"
       ng-click="Bindextenddate_history(c,$index)">
       <i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="View" data-bs-original-title="History"></i>
       </a>
    <a href="javascript:void(0);"
       type="button"             
       data-bs-toggle="modal"
       data-bs-target="#ExtendedTimline"
       ng-click="Bindextenddate_Details(c,$index)">

      <span class="btn-plus2"
            data-bs-toggle="tooltip"
            data-bs-title="+ Add Extended Day">
        +
      </span>
    </a>

    </div>

</div></td>
                            <td>
                                <div>


                                    <div class="d-block mb-0">
                                        <div style="font-size:11px !important;color:#777;">{{ (+c.complete_value > 100 ? 100 : +c.complete_value) }}%</div>
                                        <%--<div style="font-size:11px !important;color:#777;">{{ c.complete_value }}%</div>--%>
                                    </div>
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar bg-success" role="progressbar" style="width:{{ c.complete_value }}%;"></div>
                                    </div>
                                    
                                </div>
                            </td>
                            <td style="text-align: center;">
                                <%--<a href="javascript: void(0);" class="d-flex gap-2" style="display:inline-block;">
                                    <div class="avatar-xxs">
                                        <img src="/pms/new/assets/img/avatar.jpg" alt="" class="rounded-circle img-fluid">
                                    </div>
                                    <div style="color:#000;">Erica Kernan</div>
                                </a>--%>
                                <div class="dropdown w-100">

  <div class="dropdown"
         
          data-bs-toggle="dropdown"  ng-class="{'pms-no-edit': !canEdit('project')}" 
        ng-click="focusOwnerSearch($event, c)" style="cursor:pointer;">
      <div class="d-flex align-items-center gap-2">
   <div class="avatar-xxs">
       <img ng-src="{{getOwnerProfilenew(c.Owner)}}" onerror="this.onerror=null;this.src='/pms/Image/user.jpg';"  alt="" class="rounded-circle img-fluid">

   </div>                               
      <div>{{ getOwnerName(c.Owner) || 'Select Owner' }}</div>
      </div>
  </div>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2 search__input"
           placeholder="Search..."
           ng-model="c.ownerSearch" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other">

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in (ownerMenuOpened[c.id] ? owners : []) | filter: c.ownerSearch"
         ng-class="{'active': c.Owner == o.id}"
         ng-click="selectOwner(c, o)">
        {{o.name}}
      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="showHeadDropdown && (owners | filter: c.ownerSearch).length == 0">
        No results found
      </div>

    </div>

  </div>

</div>

                            </td>
                            <td>
                                <div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c.teams_all" class="avatar-group-item material-shadow owner-wrp" data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary" >
        {{newteam.shortname}}
      </div>
    </div>   
    <i class="ri-close-line owner-del" ng-click="removeteams_cumulative(c,newteam)" ng-if="canEdit('project')"></i>

</a>
   <%--<div ng-if="c.teams_all && c.teams_all.length > 2"
     class="avatar-group-item owner-wrp" data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingOwners(c.teams_all) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle bg-primary">
        +{{ c.teams_all.length - 2 }}
    </div>
    </div>
</div>--%>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center" ng-if="canEdit('project')"><a href="javascript:void(0);" style="display:" type="button"  ng-click="loadSrcOwnersnew(c,'team',$index)" data-bs-toggle="modal" data-bs-target="#addowners"><span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div style="min-width:85px;">
                                        <div class="d-block mb-0">
                                            <div style="font-size:11px !important;color:#777;">{{c.completemile}}/{{c.totalmile}}</div>
                                        </div>
                                        <div class="progress progress-md animated-progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completemile_rate}}%;"></div>
                                        </div>
                                    </div>
                                    <%--<div><a href="javascript:void(0);" class="click-link" ng-click="loadsrc(c,'inner_popup2','/pms/AddmilstonePopup.aspx?id='+c.id+'&type=Project','milestone')"><i class="ri-add-line"></i></a></div>--%>
                                </div>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div style="min-width:85px;">
                                        <div class="d-block mb-0">
                                            <div style="font-size:11px !important;color:#777;">{{c.completetask}}/{{c.totaltask}}</div>
                                        </div>
                                        <div class="progress progress-md animated-progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completetask_rate}}%;"></div>
                                        </div>
                                    </div>
                                    <%--<div><a href="javascript:void(0);" class="click-link"><i class="ri-add-line"></i></a></div>--%>
                                </div>
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div style="min-width:85px;">
                                        <div class="d-block mb-0">
                                            <div style="font-size:11px !important;color:#777;">{{c.completeissue}}/{{c.totalissue}}</div>
                                        </div>
                                        <div class="progress progress-md animated-progress">
                                            <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completeissue_rate}}%;"></div>
                                        </div>
                                    </div>
                                    <%--<div><a href="" class="click-link"><i class="ri-add-line"></i></a></div>--%>
                                </div>
                            </td>
                            <td>{{ formatDateOnly(c.StartDate) }}</td>
                            <td>{{ formatDateOnly(c.TargetDate) }}</td>
                            <td>{{c.Budget}}</td>
                            <td>{{c.Actual}}</td>
                            <td><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="{{c.Completeaddress}}">
    {{ c.Completeaddress && c.Completeaddress.length >35
        ? (c.Completeaddress | limitTo:50) + '...'
        : c.Completeaddress }}
</span></td>
                            <td>{{ formatDateOnly(c.AwardDate) }}</td>
                            <td>{{ c.ApprovalDrawing == '1' ? 'Yes' : 'No' }}</td>
                            <td>{{ formatDateOnly(c.DrawingApprovalDate) }}</td>
                            <td>
                                <%--<a href="javascript:void(0);" style="font-size:22px;" data-bs-toggle="offcanvas" data-bs-target="#uploaddocuments"><i class="ri-file-text-line"></i></a>--%>
                                <a href="javascript:void(0);" style="font-size:22px;" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadprojectfiles(c,$index)" class="position-relative"><span class="btn-plus2"  data-bs-toggle="tooltip" data-bs-title="Upload Documents" ><i class="ri-file-text-line"></i></span><span class="files_count">{{ c.viewfiles.length || 0 }}</span></a>
                                
                            </td>
                        </tr>
                        
                    </tbody>
                </table>
            </div>
           
            <div class="row">
                <div class="col-sm-4">
                    <div class="d-flex align-items-center gap-2" style="padding-left:10px;">
                        <select class="form-select" ng-model="pageSize"  ng-options="size for size in pageSizes" style="width:55px;"></select>
                        <span class="text-muted" style="font-size:11px;">Records per page</span>
                    </div>
                </div>
                <div class="col-sm-2 text-center" style="padding-top:7px;">
                    <%--<span class="text-muted" style="font-size:11px;">Showing 1 to 2 of 2 entries</span>--%>
                </div>
                <div class="col-sm-2" text-center" style="padding-top:7px;">
                    <div class="zoom-bar" id="zoomBar">
    <span>-</span>
    <input type="range"
       min="50"
       max="150"
       ng-model="zoom.level"
       ng-change="applyZoom(zoom.level)" />
    <span>+</span>
    <span>{{zoom.level}}%</span>
</div>
                   <%-- <div class="pull-right" style="padding-right:10px;">
                        <dir-pagination-controls max-size="8" direction-links="true" boundary-links="true" class="ng-isolate-scope"><ul class="pagination justify-content-end mb-0"><li class="page-item disabled" ng-class="{disabled: pagination.current == 1}"><a class="page-link" href="" ng-click="setCurrent(1); $event.preventDefault()">«</a></li><!-- ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope active" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">1</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">2</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">3</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">4</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">5</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">6</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope disabled" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">...</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item ng-scope" ng-repeat="pageNumber in pages track by $index" ng-class="{active: pagination.current == pageNumber, disabled: pageNumber == '...'}"><a class="page-link ng-binding" href="" ng-click="setCurrent(pageNumber); $event.preventDefault()">96</a></li><!-- end ngRepeat: pageNumber in pages track by $index --><li class="page-item" ng-class="{disabled: pagination.current == pagination.last}"><a class="page-link" href="" ng-click="setCurrent(pagination.last); $event.preventDefault()">»</a></li></ul></dir-pagination-controls>
                    </div>--%>
                </div>
                <div class="col-sm-4">
                    <div class="pull-right" style="padding-right:10px;">
                        <dir-pagination-controls
max-size="8" 
direction-links="true"
boundary-links="true">
</dir-pagination-controls>
                    </div>
                </div>
            </div>
             <!-- Paging Controls -->
        </div>

        <div ng-cloak
             ng-show="projectDataReady && !isLoading && projectlist.length == 0"
             class="text-center text-muted py-4">
            No projects found.
        </div>
        </div>
        <div ng-if="view == 'tiles'" class="wrapper_inner pt-3 project-tiles-view">
            <div class="row g-2">
                <div class="col-lg-3 project-card"
                     dir-paginate="c in projectlist | filter:projectStatusFilter | orderBy:sortColumn:reverseSort | filter:searchText | itemsPerPage:pageSize track by c.id"
                     current-page="currentPage">
                    <div class="card card-height-100 project-tile-card"
                         ng-style="{'border-left-color': (c.priority_color || c.status_color || '#3978de')}">
                        <div class="card-body p-2">
                            <div class="d-flex flex-column h-100">
                                <div class="d-flex align-items-start mb-2 gap-2">
                                    <div class="flex-grow-1">
                                        <div class="d-flex gap-1 align-items-center flex-wrap">
                                            <a href="/pms/assignprojects/?id={{c.id}}" class="project-tile-id">{{c.ProjectID}}</a>

                                            <!-- Uses the same priority list and selectPriority() handler as List View. -->
                                            <div class="dropdown d-inline-block">
                                                <a href="javascript:void(0);"
                                                   class="badge project-tile-priority"
                                                   ng-class="{'pms-no-edit': !canEdit('project')}"
                                                   ng-style="{'border-color': (c.priority_color || '#6c757d'), 'color': '#000'}"
                                                   data-bs-toggle="dropdown"
                                                   data-bs-auto-close="true"
                                                   data-bs-boundary="window"
                                                   aria-expanded="false"
                                                   title="Edit Priority">
                                                    {{c.priority_name || 'No Priority'}}
                                                    <i class="ri-arrow-down-s-line ms-1"></i>
                                                </a>
                                                <div class="dropdown-menu p-0 project-priority-dropdown" style="border-radius:0; min-width:145px; z-index:1060;">
                                                    <ul class="sttus-list" data-simplebar style="max-height:160px;">
                                                        <li ng-repeat="newpriority in prioritylists | filter:name"
                                                            ng-click="selectPriority(c,newpriority); $event.preventDefault();">
                                                            <div class="dropdown-item p-0">
                                                                <a href="javascript:void(0);" style="color:#000 !important;">
                                                                    <i class="ri-checkbox-blank-fill"
                                                                       ng-style="{'color': (!newpriority.colorname || newpriority.colorname == '#FFFFFF') ? '#000' : newpriority.colorname}"></i>
                                                                    {{newpriority.name}}
                                                                    <i class="ri-check-line float-end"
                                                                       ng-if="c.Priority == newpriority.id || c.priority_name == newpriority.name"></i>
                                                                </a>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="flex-shrink-0">
                                        <div class="d-flex gap-2 align-items-center">
                                            <div class="dropdown">
                                                <a href="javascript:void(0);"
                                                   class="project-tile-status"
                                                   ng-class="{'pms-no-edit': !canEdit('project')}"
                                                   style="color:#000 !important;"
                                                   data-bs-toggle="dropdown" data-bs-auto-close="true"
                                                   ng-click="c._oldcomplete_value = c.complete_value; c._oldpending_value = c.pending_value;">
                                                    <i class="bx bxs-circle"
                                                       ng-style="{'color': (!c.status_color || c.status_color == '#FFFFFF') ? '#000' : c.status_color}"></i>
                                                    {{c.status_name}}
                                                </a>
                                                <div class="dropdown-menu dropdown-menu-end p-0 project-status-dropdown" style="border-radius:0;">
                                                    <ul class="sttus-list">
                                                        <li ng-repeat="newstatus in statuslists | filter:name">
                                                            <a href="javascript:void(0);"
                                                               style="color:#000 !important;"
                                                               ng-click="selectStatus(c,newstatus); $event.preventDefault();">
                                                                <i class="bx bxs-circle"
                                                                   ng-style="{'color': (!newstatus.colorname || newstatus.colorname == '#FFFFFF') ? '#000' : newstatus.colorname}"></i>
                                                                {{newstatus.name}}
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>

                                            <div class="dropdown">
                                                <a href="javascript:void(0);" class="click-link" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window">
                                                    <i class="ri-more-2-fill"></i>
                                                </a>
                                                <div class="dropdown-menu dropdown-menu-end p-0" style="border-radius:0;min-width:175px;">
                                                    <ul class="sttus-list">
                                                        <li><a href="javascript:void(0);" ng-click="openProjectDependency(c,$event)"><i class="ri-parent-line fs-16 align-middle me-1"></i> Dependency</a></li>
                                                        <li><a href="javascript:void(0);" ng-click="openProjectIssue(c,$event)"><i class="bx bx-error-circle fs-16 align-middle me-1"></i> Add Issue</a></li>
                                                        <%if (ischeckEdit == 1) {%>
                                                        <li><a href="javascript:void(0);" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadprojectfiles(c,$index)"><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Documents</a></li>
                                                        <li><a href="javascript:void(0);" data-bs-toggle="offcanvas" data-bs-target="#addnewproject" ng-click="openproject(c.id,'update')"><i class="bx bx-edit fs-16 align-middle me-1"></i> Edit Project</a></li>
                                                        <%} %>
                                                        <%if (ischeckCreate == 1) {%>
                                                        <li><a href="javascript:void(0);" ng-click="saveproject_clone(c)"><i class="bx bx-copy fs-16 align-middle me-1"></i> Clone</a></li>
                                                        <%} %>
                                                        <li><a href="#"><i class="bx bx-archive fs-16 align-middle me-1"></i> Hold</a></li>
                                                        <%if (ischeckDelete == 1) {%>
                                                        <li><a class="text-danger" href="javascript:void(0);" ng-click="deletedataproject(c)"><i class="bx bx-trash fs-16 align-middle me-1"></i> Delete</a></li>
                                                        <%} %>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <a href="/pms/assignprojects/?id={{c.id}}" class="" data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="Project Name">{{c.ProjectName}}</a>
                                <div class="project-tile-client text-truncate" data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="Client Name">{{c.ClientName || 'No client'}}</div>

                                <div class="d-flex align-items-center gap-2 mt-1">
                                    <div class="avatar-xxs" data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="Project Manager">
                                        <img ng-src="{{getOwnerProfilenew(c.Owner)}}"
                                             onerror="this.onerror=null;this.src='/pms/Image/user.jpg';"
                                             alt="" class="rounded-circle img-fluid">
                                    </div>
                                    <div class="project-tile-owner text-truncate">{{getOwnerName(c.Owner) || 'No project manager'}}</div>
                                </div>

                                <%--<div class="project-tile-summary mt-3">
                                    <div><span>Milestones</span><strong>{{c.completemile}}/{{c.totalmile}}</strong></div>
                                    <div><span>Tasks</span><strong>{{c.completetask}}/{{c.totaltask}}</strong></div>
                                    <div><span>Issues</span><strong>{{c.completeissue}}/{{c.totalissue}}</strong></div>
                                </div>--%>

                                <%--<div class="d-flex align-items-center justify-content-between mt-3">
                                    <div class="avatar-group">
                                        <a href="javascript:void(0);" ng-repeat="newteam in c.teams | limitTo:3"
                                           class="avatar-group-item material-shadow owner-wrp" data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="{{newteam.name}}">
                                            <div class="avatar-xxs">
                                                <img ng-if="newteam.profileimage" ng-src="{{newteam.profileimage}}" class="rounded-circle avatar-xxs" />
                                                <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary">{{newteam.shortname}}</div>
                                            </div>
                                        </a>
                                        <div ng-if="c.teams && c.teams.length > 3"
                                             class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-placement="top"
                                             data-bs-title="{{getRemainingOwners(c.teams)}}">
                                            <div class="avatar-xxs">
                                                <div class="avatar-title rounded-circle bg-primary">+{{c.teams.length - 3}}</div>
                                            </div>
                                        </div>
                                    </div>

                                    <a href="javascript:void(0);" class="position-relative project-tile-file"
                                       data-bs-toggle="offcanvas" data-bs-target="#addfiles"
                                       ng-click="loadprojectfiles(c,$index)" data-bs-title="Upload Documents">
                                        <i class="ri-file-text-line"></i>
                                        <span class="files_count">{{c.viewfiles.length || 0}}</span>
                                    </a>
                                </div>--%>
                            </div>
                        </div>

                        <div class="card-footer bg-transparent border-top p-2">
                            <div class="d-flex align-items-center gap-3">
                                <div class="flex-grow-1 project-tile-date">
                                    <i class="ri-calendar-event-fill me-1 align-bottom"></i>
                                    {{ formatDateOnly(c.StartDate) }} - {{ formatDateOnly(c.TargetDate) }}
                                </div>
                                <div class="flex-shrink-0">
                                    <div class="d-flex gap-2 align-items-center">
                                        <div class="progress progress-md animated-progress project-tile-progress">
                                            <div class="progress-bar bg-success" role="progressbar"
                                                 style="width:{{((+c.complete_value || 0) > 100 ? 100 : (+c.complete_value || 0))}}%;"></div>
                                        </div>
                                        <div class="project-tile-percent">{{((+c.complete_value || 0) > 100 ? 100 : (+c.complete_value || 0))}}%</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-12 text-center py-4 text-muted" ng-if="(projectlist | filter:projectStatusFilter | filter:searchText).length == 0">
                    No projects found.
                </div>
            </div>

            <div class="row align-items-center mt-3">
                <div class="col-sm-4">
                    <div class="d-flex align-items-center gap-2">
                        <select class="form-select" ng-model="pageSize" ng-options="size for size in pageSizes" style="width:65px;"></select>
                        <span class="text-muted" style="font-size:11px;">Records per page</span>
                    </div>
                </div>
                <div class="col-sm-8">
                    <div class="pull-right">
                        <dir-pagination-controls max-size="8" direction-links="true" boundary-links="true"></dir-pagination-controls>
                    </div>
                </div>
            </div>
        </div>

        <div ng-if="view == 'calendar'" class="wrapper_inner project-calendar-view" ng-cloak>
            <div class="project-calendar-shell" ng-show="projectDataReady && !isLoading">
                <div class="project-calendar-toolbar">
                    <h5 class="project-calendar-title">{{projectCalendarTitle}}</h5>
                    <div class="project-calendar-actions">
                        <button type="button" class="project-calendar-action" ng-click="changeProjectCalendarMonth(-1)" title="Previous month">
                            <i class="ri-arrow-left-s-line"></i>
                        </button>
                        <button type="button" class="project-calendar-action" ng-click="showCurrentProjectCalendarMonth()">Today</button>
                        <button type="button" class="project-calendar-action" ng-click="changeProjectCalendarMonth(1)" title="Next month">
                            <i class="ri-arrow-right-s-line"></i>
                        </button>
                    </div>
                </div>

                <div class="project-calendar-layout" ng-if="projectlist.length > 0">
                <div class="project-calendar-main">
                <table class="project-calendar-table">
                    <thead>
                        <tr>
                            <th>Sun</th><th>Mon</th><th>Tue</th><th>Wed</th><th>Thu</th><th>Fri</th><th>Sat</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="week in projectCalendarWeeks track by $index">
                            <td ng-repeat="day in week track by day.key"
                                class="project-calendar-cell"
                                ng-class="{'other-month': !day.isCurrentMonth, 'selected': projectCalendarSelectedDay && projectCalendarSelectedDay.key == day.key}"
                                ng-click="selectProjectCalendarDay(day)">
                                <div class="project-calendar-day" ng-class="{'today': day.isToday}">
                                    <span>{{day.dayNumber}}</span>
                                </div>
                                <div class="project-calendar-count" ng-if="calendarVisibleProjects(day.projects).length > 0">
                                    <strong>Project: {{calendarVisibleProjects(day.projects).length}}</strong>
                                    <span class="project-calendar-detail-task">Task: {{projectCalendarTotal(day.projects, 'totaltask')}}</span><br />
                                    <span class="project-calendar-detail-subtask">Subtask: {{projectCalendarTotal(day.projects, 'totalsubtask')}}</span>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                </div>

                <aside class="project-calendar-side">
                    <div class="project-calendar-side-title">
                        <span>{{projectCalendarSelectedDay ? formatDateOnly(projectCalendarSelectedDay.date) : ''}}</span>
                        <span>{{projectCalendarSelectedProjects().length}} Project(s)</span>
                    </div>
                    <div class="project-calendar-side-scroll" ng-if="projectCalendarSelectedProjects().length > 0">
                        <table class="project-calendar-detail">
                            <thead>
                                <tr>
                                    <th style="width:32px;">Sr.</th>
                                    <th>Project Name</th>
                                    <th style="width:72px;">Start Date</th>
                                    <th style="width:72px;">End Date</th>
                                    <th style="width:105px;" ng-if="projectCalendarIsAdmin">Assign To</th>
                                    <th style="width:105px;" ng-if="!projectCalendarIsAdmin">Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="c in projectCalendarSelectedProjects() track by c.id"
                                    ng-style="{'border-left':'3px solid ' + (c.status_color || c.priority_color || '#3978de')}">
                                    <td>{{$index + 1}}</td>
                                    <td>
                                        <a href="/pms/assignprojects/?id={{c.id}}" class="project-calendar-detail-name"
                                           title="{{c.ProjectName}}">
                                            <span class="project-calendar-detail-id">{{c.ProjectID}}</span>
                                            {{c.ProjectName | limitTo:30}}{{c.ProjectName.length > 30 ? '...' : ''}}
                                            <span class="project-calendar-detail-summary project-calendar-detail-task">Total Task: {{c.totaltask || 0}}</span>
                                            <span class="project-calendar-detail-summary project-calendar-detail-subtask">Total Subtask: {{c.totalsubtask || 0}}</span>
                                        </a>
                                    </td>
                                    <td>{{formatDateOnly(c.StartDate)}}</td>
                                    <td>{{formatDateOnly(c.TargetDate)}}</td>
                                    <td ng-if="projectCalendarIsAdmin">
                                        <div class="project-calendar-owner">
                                            <img ng-src="{{getOwnerProfilenew(c.Owner)}}"
                                                 onerror="this.onerror=null;this.src='/pms/Image/user.jpg';" alt="" />
                                            <span>{{getOwnerName(c.Owner) || 'Not assigned'}}</span>
                                        </div>
                                    </td>
                                    <td ng-if="!projectCalendarIsAdmin">
                                        <span class="project-calendar-status"
                                              ng-style="{'color': c.status_color || '#495057', 'background-color': (c.status_color || '#6c757d') + '18'}">
                                            {{c.status_name || '-'}}
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="project-calendar-no-selection" ng-if="projectCalendarSelectedProjects().length == 0">
                        No projects for this date.
                    </div>
                </aside>
                </div>

                <div class="project-calendar-empty" ng-if="projectlist.length == 0">
                    No projects found.
                </div>
            </div>
        </div>

        <div ng-show="isLoading" class="loader-overlay">
            <div class="loader"></div>
        </div>
        <div class="offcanvas offcanvas-end" tabindex="-1" id="addnewproject" data-bs-backdrop="static">
        <div class="offcanvas-header justify-content-between border-bottom p-2">
            <h5 class="offcanvas-title">Add Project</h5>
            <!--<button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>-->
            <div class="hstack gap-1">
               
            <div id="pnlimg" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img2" />
                    </div>
            <div id="divsub" style="display:block;"  data-bs-toggle="tooltip" data-bs-title="Save Project">
                   
                   <a href="javascript:void(0);" type="button" style="display:inline;text-align:center;" ng-click="saveproject()" class="sub-soft-btn fsr fs-18"><i class="ri-save-3-line"></i></a>
                </div>
            
        
                <a href="javascript:void(0);" type="button" onclick="fsFunction()" class="sub-soft-btn fsr fs-18"><i class="ri-fullscreen-fill"></i></a>
                <a href="javascript:void(0);" type="button" onclick="fseFunction()" class="sub-soft-btn fsre fs-18" style="text-align:center;"><i class="ri-fullscreen-exit-fill"></i></a>
                <a href="javascript:void(0);" type="button" class="sub-soft-btn danger fs-18" data-bs-dismiss="offcanvas" aria-label="Close" id="btnsave" ng-click="closesave()"><i class="ri-close-fill"></i></a>
            </div>
        </div>
        <div class="offcanvas-body p-0">
            <div data-simplebar style="height: calc(100vh - 45px);">
                <div class="p-3">
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="input-line">
                                <input type="text" placeholder="Project ID" ng-model="newproject.ProjectID">
                            </div>
                        </div>
                        <div class="col-sm-9">
                            <div class="input-line">
                                <input type="text" placeholder="Project Name" ng-model="newproject.ProjectName">
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1">
                            <label style="font-weight:600;">Client Details</label>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        
                                        <div class="search__wrap position-relative">

    <input type="text"
           class="form-control search__input"
           placeholder="Client Name"
           ng-model="newproject.ClientName"
           autocomplete="off" name="pms_search_1" role="combobox" aria-autocomplete="list" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other"
       selected-value="newproject.customerid"
                   list-data="customerlist"
                   display-field="ClientName"
        
        ng-focus="showqcustomersDropdown= true;"
        ng-change="onSearchInputChange()"
        ng-click="showqcustomersDropdown= true;"
        ng-keyup="checkSelectAllOrType($event)"
        >

    <span class="search__arrow" >
        <i class="ri-arrow-down-s-fill"></i>
    </span>

    <div class="dropdown_bl search__dropdown shadow-sm" 
        ng-show="showqcustomersDropdown"
        ng-mousedown="$event.preventDefault(); $event.stopPropagation();">

        <a href="javascript:void(0);"
           class="dropdown_bl__link" 
           ng-repeat="c in (showqcustomersDropdown ? customerlist : []) | filter:{ClientName: newproject.ClientName}"
           ng-click="selectclient(c)"  ng-bind-html="highlightMatch(c.ClientName, newproject.ClientName)">
            {{c.ClientName}}
        </a>

        <div class="dropdown_bl__no-record"
             ng-if="showqcustomersDropdown && (customerlist | filter:{ClientName: newproject.ClientName}).length == 0">
            No record found
        </div>

    </div>

</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">

                                        <div class="search__wrap position-relative">

    <input type="text"
           class="form-control search__input"
           placeholder="Sales Order"
           ng-model="newproject.Estimateno"
           autocomplete="off" name="pms_search_2" role="combobox" aria-autocomplete="list" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other"  
        selected-value="newproject.salesorderid"
                   list-data="solist"
                   display-field="Estimateno"
             ng-focus="showSODropdown=true;"
        ng-change="showSODropdown = true;"
            ng-click="showSODropdown=true;">

    <span class="search__arrow">
        <i class="ri-arrow-down-s-fill"></i>
    </span>

    <div class="dropdown_bl search__dropdown shadow-sm" 
        ng-show="showSODropdown"
        ng-mousedown="$event.preventDefault(); $event.stopPropagation();">

        <a href="javascript:void(0);"
           class="dropdown_bl__link"
           ng-repeat="c in (showSODropdown ? solist : []) | filter:{Estimateno:(newproject.Estimateno || '')}"
           ng-click="selectSO(c)"  ng-bind-html="highlightMatch(c.Estimateno, newproject.Estimateno)">
            {{c.Estimateno}}
        </a>

        <div class="dropdown_bl__no-record"
             ng-if="showSODropdown && (solist | filter:{Estimateno:(newproject.Estimateno || '')}).length == 0">
            No record found
        </div>

    </div>

</div>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>
               
                       
                        <div class="col-sm-4">
                            <div class="d-flex align-items-center gap-2">
    <span style="font-weight:600;">Milestone</span>
    <label class="switch_dis" style="margin-bottom:0;">
        <input type="checkbox"
               ng-model="newproject.hasmile" />
        <div class="slider round">
            <span class="on">Yes</span>
            <span class="off">No</span>
        </div>
    </label>
</div>

<div id="dvmile"
     class="d-flex align-items-center gap-2 mt-2"
     ng-show="newproject.hasmile" >
    <span>Same weightage</span>
    <label class="switch_dis" style="margin-bottom:0;">
        <input type="checkbox"
               ng-model="newproject.checkmilesameWeightage" />
        <div class="slider round">
            <span class="on">Yes</span>
            <span class="off">No</span>
        </div>
    </label>
</div>
                        </div>
                        <div class="col-sm-4">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Task</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" ng-model="newproject.hastask"/>
                                    <div class="slider round" >
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                            <div id="dvtask"  class="d-flex align-items-center gap-2 mt-2" ng-show="newproject.hastask">
                                <span>Same weightage</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox"  ng-model="newproject.checktasksameWeightage"/>
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Subtask</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" ng-model="newproject.hassubtask"/>
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                            <div id="dvsubtask"  class="d-flex align-items-center gap-2 mt-2"  ng-show="newproject.hassubtask">
                                <span>Same weightage</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox"  ng-model="newproject.checksubtasksameWeightage"/>
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                       
                            <div class="col-sm-6 mt-3 mb-1">
                            <div class="d-flex align-items-center gap-2">
    <span style="font-weight:600;">Including Holydays</span>
    <label class="switch_dis" style="margin-bottom:0;">
        <input type="checkbox"
               ng-model="newproject.iscountholydays" />
        <div class="slider round">
            <span class="on">Yes</span>
            <span class="off">No</span>
        </div>
    </label>
</div>
 </div>
                        <div class="col-sm-6 mt-3 mb-1">
                            <div class="d-flex align-items-center gap-2">
    <span style="font-weight:600;">Including Weekoff</span>
    <label class="switch_dis" style="margin-bottom:0;">
        <input type="checkbox"
               ng-model="newproject.iscountweekoff" />
        <div class="slider round">
            <span class="on">Yes</span>
            <span class="off">No</span>
        </div>
    </label>
</div>
 </div>
                      
                        <div class="col-sm-12 mt-3">
                            <label style="font-weight:600;">Priority</label>
                            <div class="hstack gap-1 flex-wrap priority-select-wrap">
                                <div ng-repeat="c in prioritylists" class="priority-option">

                                    <input type="radio"
                                           class="btn-check"
                                           name="options-priority"
                                           id="rbt_{{c.id}}"
                                           ng-model="newproject.Priority"
                                           ng-value="c.id"
                                           ng-checked="newproject.Priority == c.id"
                                           ng-click="selectPopupPriority(c)">

                                    <label class="btn btn-stts priority-btn"
                                           for="rbt_{{c.id}}"
                                           ng-class="{'priority-selected': isPopupPrioritySelected(c)}"
                                           ng-style="{'background-color': c.colorname ? c.colorname : 'gray', 'color': 'black'}">
                                        <i class="ri-check-line me-1 priority-tick" ng-show="isPopupPrioritySelected(c)"></i>
                                        {{c.name}}
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 mt-2">
                            <label style="font-weight:600;">Status</label>
                            <div class="hstack gap-1 flex-wrap staus-select-wrap">
                                <div ng-repeat="c in statuslists" class="status-option">
                                    <input type="radio"
                                           class="btn-check"
                                           name="options-status"
                                           id="rbts_{{c.id}}"
                                           ng-model="newproject.ProjectStatus"
                                           ng-value="c.id"
                                           ng-checked="newproject.ProjectStatus == c.id"
                                           ng-click="selectPopupStatus(c)">

                                    <label class="btn btn-stts status-btn"
                                           for="rbts_{{c.id}}"
                                           ng-class="{'status-selected': isPopupStatusSelected(c)}"
                                           ng-style="{'background-color': c.colorname ? c.colorname : 'gray', 'color': 'black'}">
                                        <i class="ri-check-line me-1 status-tick" ng-show="isPopupStatusSelected(c)"></i>
                                        {{c.name}}
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 mt-2">
                            <div class="input-line">
                                <div class="search__wrap position-relative">

    <input type="text"
           class="form-control search__input"
           placeholder="Project Manager"
           ng-model="newproject.OwnerName"
           autocomplete="off" name="pms_search_3" role="combobox" aria-autocomplete="list" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" 
         selected-value="newproject.Owner"
                   list-data="owners"
                   display-field="name"
      ng-focus="showHeadDropdown = true;"
        ng-change="showHeadDropdown = true;"
      ng-click="showHeadDropdown = true;">

    <span class="search__arrow">
        <i class="ri-arrow-down-s-fill"></i>
    </span>

    <div class="dropdown_bl search__dropdown shadow-sm" 
        ng-show="showHeadDropdown"
        ng-mousedown="$event.preventDefault(); $event.stopPropagation();" >

        <a href="javascript:void(0);"
           class="dropdown_bl__link"
           ng-repeat="c in (showHeadDropdown ? owners : []) | filter:{name:newproject.OwnerName}"
           ng-click="selectowner(c)" ng-bind-html="highlightMatch(c.name, newproject.OwnerName)">
            {{c.name}}
        </a>

        <div class="dropdown_bl__no-record"
             ng-if="showHeadDropdown && (owners | filter:{name: newproject.OwnerName}).length == 0">
            No record found
        </div>

    </div>

</div>
                       
                            </div>
                        </div>
                        <div class="col-sm-12 mb-1">
                            <div class="input-line">
                                <div class="search__wrap position-relative">
                                    
                                    <div class="tag-input">

    <span class="tag"
          ng-repeat="team in selectedTeams track by team.id">

        {{team.name}}

        <i class="ri-close-line"
           ng-click="removeTeam(team)">
        </i>

    </span>

    <input type="text"
           class="search__input"
           placeholder="Team" ng-model="teamSearch" autocomplete="off" name="pms_search_6" role="combobox" aria-autocomplete="list" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other">

</div>
                                    <div class="dropdown_bl search__dropdown" ng-show="showProjectTeamDropdown">
                                        <a href="javascript:void(0);"  ng-repeat="newteam in (showProjectTeamDropdown ? selectedfilterTeams : []) | filter:teamSearch" class="dropdown_bl__link" data-id="{{newteam.id}}"   ng-click="addTeam(newteam)">{{newteam.name}}</a>
                                        <div ng-if="showProjectTeamDropdown && (selectedfilterTeams | filter:teamSearch).length == 0" class="dropdown_bl__no-record">No record found</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="input-line">
                                <label for="StartDate">Start Date</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="StartDate" class="form-control search__input" placeholder="dd/mm/yyyy" autocomplete="off"  ng-model="newproject.StartDate">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="input-line">
                                <label for="TargetDate">End Date</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="TargetDate" class="form-control search__input" placeholder="dd/mm/yyyy"  autocomplete="off"  ng-model="newproject.TargetDate">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="input-line">
                                <label for="AwardDate">Date of Award</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="AwardDate" class="form-control search__input" placeholder="dd/mm/yyyy"  autocomplete="off"  ng-model="newproject.AwardDate">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12 mt-1 mb-1">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="input-line">
                                        <label class="mb-2">Project Address</label>
                                        <input type="text" placeholder="Address Line 1"  ng-model="newproject.ProjectAddress1">
                                    </div>
                                    <div class="input-line">
                                        <input type="text" placeholder="Address Line 2"  ng-model="newproject.ProjectAddress2">
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <input type="text" placeholder="City"  ng-model="newproject.City">
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <div class="search__wrap position-relative">

    <input type="text"
           class="form-control search__input"
           placeholder="Country Name"
           ng-model="newproject.CountryName"
           autocomplete="off" name="pms_search_4" role="combobox" aria-autocomplete="list" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other"  
        selected-value="newproject.Countryid"
                   list-data="countrylist"
                   display-field="CountryName"
        ng-focus="showCountryDropdown = true;"
        ng-change="showCountryDropdown = true;"
                                                            ng-click="showCountryDropdown = true;">

    <span class="search__arrow">
        <i class="ri-arrow-down-s-fill"></i>
    </span>

    <div class="dropdown_bl search__dropdown shadow-sm" 
        ng-show="showCountryDropdown"
        ng-mousedown="$event.preventDefault(); $event.stopPropagation();">

        <a href="javascript:void(0);"
           class="dropdown_bl__link"
           ng-repeat="c in (showCountryDropdown ? countrylist : []) | filter:{CountryName:newproject.CountryName}"
           ng-click="selectcountry(c)"  ng-bind-html="highlightMatch(c.CountryName, newproject.CountryName)">
            {{c.CountryName}}
        </a>

        <div class="dropdown_bl__no-record"
             ng-if="showCountryDropdown && (countrylist | filter:{CountryName: newproject.CountryName}).length == 0">
            No record found
        </div>

    </div>

</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <div class="search__wrap position-relative">

    <input type="text"
           class="form-control search__input"
           placeholder="State Name"
           ng-model="newproject.StateName"
           autocomplete="off" name="pms_search_5" role="combobox" aria-autocomplete="list" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other" 
        selected-value="newproject.Stateid"
                   list-data="statelist"
                   display-field="StateName"
        ng-focus="showStateDropdown = true;"
        ng-change="showStateDropdown = true;"
        ng-click="showStateDropdown = true;">

    <span class="search__arrow">
        <i class="ri-arrow-down-s-fill"></i>
    </span>

    <div class="dropdown_bl search__dropdown shadow-sm" 
        ng-show="showStateDropdown"
        ng-mousedown="$event.preventDefault(); $event.stopPropagation();">

        <a href="javascript:void(0);"
           class="dropdown_bl__link"
           ng-repeat="c in (showStateDropdown ? statelist : []) | filter:{StateName:newproject.StateName}"
           ng-click="selectstate(c)"   ng-bind-html="highlightMatch(c.StateName, newproject.StateName)">
            {{c.StateName}}
        </a>

        <div class="dropdown_bl__no-record"
             ng-if="showStateDropdown && (statelist | filter:{StateName: newproject.StateName}).length == 0">
            No record found
        </div>

    </div>

</div>
                                       <%-- <div class="search__wrap position-relative">
                                            <input type="text" class="form-control search__input" placeholder="State" value="" autocomplete="off">
                                            <span class="search__arrow"><i class="ri-arrow-down-s-fill"></i></span>
                                            <div class="dropdown_bl search__dropdown">
                                                <a href="javascript:void(0);" class="dropdown_bl__link">Andaman and Nicobar Islands</a>
                                                
                                                <div class="dropdown_bl__no-record">No record found</div>
                                                <!--<a href="#" class="dropdown_bl__link search-more">Search more…</a>-->
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="input-line">
                                        <input type="text" placeholder="Zip/Postal Code" ng-model="newproject.PostalCode">
                                    </div>
                                </div>
                                
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Document Required</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox" ng-model="newproject.RequiredDrawing"/>
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <div class="col-sm-6" ng-show="newproject.RequiredDrawing">
                            <div class="d-flex align-items-center gap-2">
                                <span style="font-weight:600;">Approval of Document</span>
                                <label class="switch_dis" style="margin-bottom:0;">
                                    <input type="checkbox"  ng-model="newproject.ApprovalDrawing"/>
                                    <div class="slider round">
                                        <span class="on">Yes</span>
                                        <span class="off">No</span>
                                    </div>
                                </label>
                            </div>
                        </div>
                        <%--<div class="col-sm-4 mt-2">
                            <div class="input-line">
                                <label for="AwardDate">Date of Award</label>
                                <div class="search__wrap position-relative">
                                    <input type="text" id="AwardDate" class="form-control search__input" placeholder="dd/mm/yyyy" value="" autocomplete="off">
                                    <span class="search__arrow"><i class="ri-calendar-line"></i></span>
                                </div>
                            </div>
                        </div>--%>
                        <div class="col-sm-12 mb-1" ng-show="newproject.RequiredDrawing">
                            <label style="font-weight:600;">Upload Document</label>
                            <div class="row">
                                <div class="col-sm-12 mt-1">
                                   
                    <input type="file" id="fileupload2" multiple hidden  onchange="angular.element(this).scope().filesSelected2(this.files)">

                    <div id="dropZone2" class="dropzone"
                     ng-click="openFilePicker2($event)"
                     ng-class="{'highlight': isDragging}">
                    <i class="ri-upload-cloud-2-fill fs-6xl"></i><br />
                    Drag & Drop files here or click to upload
                </div>
                      <div class="preview-container" style="margin-top:10px; display:none; flex-wrap:wrap; gap:8px;">
                    <div ng-repeat="file in uploadedFiles" class="file-preview" style="width:60px; text-align:left;">
                        <img ng-if="file.type.startsWith('image/')" ng-src="{{file.data}}" 
                             style="width:60px; height:60px; object-fit:cover; border-radius:4px;">
                        <div ng-if="file.type==='application/pdf'" 
                             style="width:60px; height:60px; display:flex; justify-content:center; align-items:center; font-size:24px; background:#f0f0f0; border-radius:4px;">
                            📄
                        </div>
                        <span style="font-size:10px; display:block; overflow:hidden; text-overflow:ellipsis;">{{file.name}}</span>
                    </div>
                </div>
                    
                    
                    <table class="table align-middle table-striped table-hover table-bordered table-sm datatable tbl_new tbl_fileup">

    <thead>
        <tr>
            <th width="50">#</th>
            <th>File</th>
            <th>Uploaded by</th>
            <th width="120">Action</th>
        </tr>
    </thead>

    <tbody>

        <tr ng-repeat="file in uploadedFiles2 track by $index">

            <td>{{$index + 1}}</td>

            <td>
                <i class="ri-image-line"
   ng-if="getFileCategory(file)=='image'"></i>

<i class="ri-file-pdf-line text-danger"
   ng-if="getFileCategory(file)=='pdf'"></i>

<i class="ri-file-excel-2-line text-success"
   ng-if="getFileCategory(file)=='excel'"></i>

<i class="ri-file-word-2-line text-primary"
   ng-if="getFileCategory(file)=='word'"></i>

<i class="ri-file-text-line text-info"
   ng-if="getFileCategory(file)=='text'"></i>

<i class="ri-file-line"
   ng-if="getFileCategory(file)=='other'"></i>
               
                {{ file.name.replace(file.fext,'').length > 40? (file.name.replace(file.fext,'') | limitTo:40) + '...': file.name.replace(file.fext,'')}}
                <small class="text-muted d-block">
                    {{file.size}}
                </small>
            </td>

            <td>
                {{file.uploadedby}}
            </td>

            <td>

                <a href="javascript:void(0);" ng-if="!file.id" 
                        class="text-dark"
                        ng-click="viewFile2(file)">
                    <i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i>
                </a>
                <a href="javascript:void(0);" ng-if="file.id" 
                        class="text-dark"
                        ng-click="viewExistfile(file)">
                    <i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="View"></i>
                </a>

                <a href="javascript:void(0);" class="delete text-danger"
                        ng-click="removeFile2($index,file.id)">
                    <i class="ri-delete-bin-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="Delete"></i>
                </a>

            </td>

        </tr>

        <tr ng-if="uploadedFiles2.length==0">
            <td colspan="4" class="text-center">
                No files selected
            </td>
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
        <%--<div class="offcanvas-foorter border-top p-2 text-center">
            <div id="pnlimg" style="display: none;">
                        <img src="/pms/Image/loading8.gif" runat="server" id="img2" />
                    </div>
            <div id="divsub" style="display:block;">
                        <button type="button" class="sub-soft-btn fs-18" style="display:inline;" ng-click="saveproject()"><i class="ri-save-3-line"></i></button>
                    </div>
            
        </div>--%>
    </div>
        <div class="offcanvas offcanvas-end" tabindex="-1" id="addfiles">
        <div class="offcanvas-header justify-content-between border-bottom p-2">
            <h5 class="offcanvas-title">Upload Documents</h5>
            <!--<button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>-->
            <div class="hstack gap-1">
                <button type="button" onclick="fs1Function()" class="sub-soft-btn fsr fs-18"><i class="ri-fullscreen-fill"></i></button>
                <button type="button" onclick="fse1Function()" class="sub-soft-btn fsre fs-18"><i class="ri-fullscreen-exit-fill"></i></button>
                <button type="button" class="sub-soft-btn danger fs-18" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ri-close-fill"></i></button>
            </div>
        </div>
        <div class="offcanvas-body p-0 overflow-hidden">
            <div data-simplebar style="height: calc(100vh - 10px);">
                <div class="p-3">
                    <input type="file" id="fileupload" multiple hidden  onchange="angular.element(this).scope().filesSelected(this.files)">

                    <div id="dropZone" class="dropzone"
                     ng-click="openFilePicker($event)"
                     ng-class="{'highlight': isDragging}">
                    <i class="ri-upload-cloud-2-fill fs-6xl"></i><br />
                    Drag & Drop files here or click to upload
                </div>
                      <div class="preview-container" style="margin-top:10px; display:none; flex-wrap:wrap; gap:8px;">
                    <div ng-repeat="file in uploadedFiles" class="file-preview" style="width:60px; text-align:left;">
                        <img ng-if="file.type.startsWith('image/')" ng-src="{{file.data}}" 
                             style="width:60px; height:60px; object-fit:cover; border-radius:4px;">
                        <div ng-if="file.type==='application/pdf'" 
                             style="width:60px; height:60px; display:flex; justify-content:center; align-items:center; font-size:24px; background:#f0f0f0; border-radius:4px;">
                            📄
                        </div>
                        <span style="font-size:10px; display:block; overflow:hidden; text-overflow:ellipsis;">{{file.name}}</span>
                    </div>
                </div>
                    <div class="row" style="display:none">
                    <div class="col-4"></div>
                    <div class="col-4"></div>
                <div class="col-4" style="text-align:right;">
                    <span class="form-switch form-switch-lg"
      data-bs-toggle="tooltip"
      title="view all files">

    <input type="checkbox"
           class="form-check-input"
           ng-model="chkall"
           ng-change="viewallfiles()"
           role="switch">
</span></div>
                    </div>
                    <div style="max-height:400px;overflow-x:auto;">
                    <table class='table align-middle table-striped table-hover table-bordered table-sm datatable tbl_new tbl_fileup'>
<thead>
                                            <tr>
                                                <th></th>
                                                <th>File</th>
                                                <th>Uploaded by</th>
                                                <th>Action</th>
                                            </tr>
                                        </thead>
                     
<tr ng-repeat="file in showfileslist | orderBy:'-id' track by file.id"
    ng-if="file.ftype=='project' || file.isshow">

                          <td width="45px">
                              <div class="attch_icon"  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}">

                                  <i class="ri"
                                      ng-class="{
                                      'ri-image-2-fill': file.fext && (file.fext.toLowerCase() == '.jpg' || file.fext.toLowerCase() == '.jpeg'|| file.fext.toLowerCase() == '.png'),
                                      ' ri-file-pdf-fill': file.fext && file.fext.toLowerCase() == '.pdf',
                                      'ri-file-excel-fill': file.fext && (file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx'),
                                      'ri-file-word-fill': file.fext && (file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.doc')
         }"></i>
                              </div>
                          </td>

                          <td><span  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}"> {{ file.filedes.replace(file.fext,'').length > 40? (file.filedes.replace(file.fext,'') | limitTo:40) + '...': file.filedes.replace(file.fext,'')}}</span></td>
    <td>{{file.Uploadby}}</td>
                          <td class='text-right' style='width:30px;vertical-align:middle;'>
                             
                              <a href="javascript:void(0);" class="text-dark"  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}" ng-href="{{ (file.fext.toLowerCase() == '.pdf' || file.fext.toLowerCase() == '.doc' || file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx') 
                      ? '/pms/ValidateDocument.aspx?isview=1&count=1&id=' + file.id 
                      : file.fileurl }}" 
       target="_blank"><i class="ri-eye-line" data-bs-toggle="tooltip"
         data-bs-placement="top"
         data-bs-trigger="hover"
         title="View"></i></a>
                              <a href="javascript:void(0);" class="text-dark" ng-click="downloadFile(file)"><i class="ri-download-2-line"></i></a>
                              <a href="javascript:void(0);" data-index="0" class="delete text-danger" ng-if ="file.ftype=='project'" ng-click="removedocumentnew(file.id)"><i class="ri-delete-bin-line" data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='Delete'></i></a>
                          </td>

                      </tr>
                           
</table>
                </div>
                </div>
            </div>
        </div>
    </div>   
        <div id="addowners" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
    <div class="modal-dialog modal-samll">
        <div class="modal-content" style="height:225px;">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title" id="lblowners">{{ modalTitle }}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body p-3 pb-2">

    <div class="input-line">
        <div class="search__wrap position-relative">

            <div class="tag-input">

                <span class="tag"
                      ng-repeat="owner in getSelectedOwners() track by owner.id">

                    {{owner.name}}

                    <i class="ri-close-line"
                       ng-click="removeOwner(owner.id)">
                    </i>

                </span>

                <input type="text"
                       class="search__input"
                       ng-model="searchOwner"
                       placeholder="Select teams..." autocomplete="off" name="pms_search_7" role="combobox" aria-autocomplete="list" autocorrect="off" autocapitalize="off" spellcheck="false" data-form-type="other">

            </div>

            <div class="dropdown_bl search__dropdown" ng-show="showOwnerOptions">

                <a href="javascript:void(0);"
                   ng-repeat="owner in (showOwnerOptions ? owners : []) | filter:searchOwner"
                   class="dropdown_bl__link"
                   data-id="{{owner.id}}"
                   ng-click="toggleOwner(owner)">

                    {{owner.name}}

                </a>

                <div ng-if="showHeadDropdown && (owners | filter:searchOwner).length == 0"
                     class="dropdown_bl__no-record">
                    No owners found
                </div>

            </div>

        </div>
    </div>

</div>
            <%--<div class="modal-body p-3 pb-2" ng-click="closelist()">  
                
               <div  id="ownerDropdown" class="dropdown" style="position:relative; width:100%;" click-outside="closelist()">

    
    <div class="input-tags" ng-click="closelist()">
        <span class="tag" ng-repeat="owner in getSelectedOwners()">
            {{owner.name}} <span class="remove" ng-click="removeOwner(owner.id); $event.stopPropagation()"><i class="ri-close-fill"></i></span>
        </span>
        
    </div>

<input type="text" class="search__input"   ng-model="searchOwner"   placeholder="Select teams..."
    ng-click="openTagDropdown($event);stopTagEvent($event)"   ng-focus="showOwnerDropdown = true" style="width:100%;" />
    
    <div class="dropdown_bl search__dropdown"
         ng-show="showOwnerDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

       
        <div ng-repeat="owner in owners | filter:searchOwner"
             ng-click="toggleOwner(owner)"
             ng-class="{'selected-owner': selectedOwnerIds.includes(owner.id)}"
             style="padding:3px; cursor:pointer;">
            {{owner.name}}
        </div>


        <div ng-if="showHeadDropdown && (owners | filter:searchOwner).length == 0" style="padding:3px; color:#888;">
            No owners found
        </div>
    </div>
</div>
              
                
                <style>
                    .input-tags {
    border: px solid #ccc;
    padding: 3px;
    min-height:30px;
    display:flex;
    flex-wrap:wrap;
    cursor:text;
}
.input-tags .tag {
    background:#007bff;
    color:#fff;
    padding:3px 6px;
    border-radius:3px;
    margin:2px;
    display:flex;
    align-items:center;
}
.input-tags .tag .remove {
    margin-left:5px;
    cursor:pointer;
    font-weight:bold;
}
.selected-owner {
    background:#f0f0f0;
}
.dropdown-menu div:hover {
    background-color: #e0e0e0;
}
                </style>

            </div>--%>
        </div>
    </div>
</div>
    <div class="offcanvas offcanvas-end canvas_lg_screen canvas-fullscreen" id="canvas_popup_multi" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div style="display:none;">
     <button type="button" id="btnclosemainpopupmulti" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas" aria-label="Close"><i class="ph-x-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoominmulti" onclick="fse3Functionmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
      <button type="button" id="btnclosemainpopupzoomoutmulti" onclick="fs3Functionmulti()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
    </div>
    <iframe id='inner_popup_multi' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      
   </div>
    <div id="ExtendedTimline" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
                                 <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Extended Date</h5>
                <button type="button" class="btn-close" id="btnclsextended" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 

     <div style="display: none">
    <%--<asp:Button ID="Button2" runat="server" Text="" OnClick="Button2_Click" />--%>
   </div>
                      <asp:HiddenField ID="hdnextendid" runat="server" Value="0" />
                          <asp:HiddenField ID="hdnextnhtml" runat="server" Value="0" />

                        <div class="col-sm-12 mb-1">
                          


                              <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend Start Date", "2")%></label>
                                    </div>
                                    <input type="date" class="form-control datepicker" ng-model="extendObj.fromdate"    onclick="if(this.showPicker){this.showPicker();}">
                                   
                                        
                                </div>
                                <div class="col-xxl-12 col-lg-12">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1"><%=fns.getltext("Extend End Date", "2")%></label>
                                    </div>
                                    
                                    <input type="date" class="form-control datepicker" ng-model="extendObj.todate" onclick="if(this.showPicker){this.showPicker();}">
                                         
                                </div>




                        </div>   
                        

                          

                    </div>
                </div>
            </div>
            <div class="pb-3 text-center">
                <a href="javascript:void(0);"  ng-click="SvaeExtended()" class='btn btn-primary'>Save</a>
                
            </div>
        </div>
    </div>
</div>
    <input type="hidden" id="hdnid" value="0" />
        <div id="extendedhistory" class="modal fade bs-example-modal-center" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title"> {{historyprojectname}}</h5>
                <button type="button" class="btn-close" id="btnclshistory" data-bs-dismiss="modal" aria-label="Close"></button>
                
            </div>
            <div class="modal-body p-3 pb-2">
                <div class="lead_form_body m-0 p-0">
                    <div class="row"> 
                       
<div class="table-responsive-new">
                <table class="table table-hover table-striped">
                    <thead>
                        <tr>
                            
                            <th>Sr.No</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>On Date</th>
                            
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="history in projectextendedhistory track by $index">
                            <td>{{ ((currentPage - 1) * pageSize) + $index + 1 }}</td>
                            <td>{{formatDateOnly(history.fromdate)}}</td>
                            <td>{{formatDateOnly(history.todate)}}</td>
                            <td>{{formatDateOnly(history.createdate)}}</td>
                            

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
        </div>

    <script src="/pms/new/assets/js/jquery.min.js"></script>
    <script src="/pms/new/assets/js/moment.min.js"></script>
    <script src="/pms/new/assets/js/daterangepicker.min.js"></script>
  <script>
$(document).ready(function () {

    function initDatePicker(selector, modelField) {

        $(selector).daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            autoUpdateInput: false,
            locale: {
                format: 'DD/MM/YYYY',
                cancelLabel: 'Clear'
            }
        });

        // Calendar open on input click
        $(selector).on('click', function () {
            $(this).data('daterangepicker').show();
        });

        // Only numbers and /
        $(selector).on('input', function () {
            $(this).val(
                $(this).val().replace(/[^0-9\/]/g, '')
            );
        });

        $(selector).on('apply.daterangepicker', function (ev, picker) {

            var scope = angular.element(this).scope();
            var date = picker.startDate.format('DD/MM/YYYY');

            // StartDate Validation
            if (modelField === 'StartDate') {

                var targetDate = $('#TargetDate').val();

                if (targetDate) {

                    if (
                        moment(date, 'DD/MM/YYYY')
                        .isAfter(moment(targetDate, 'DD/MM/YYYY'))
                    ) {

                        alert('Start Date cannot be greater than Target Date');
                        return false;
                    }
                }

                // Restrict TargetDate minimum
                $('#TargetDate')
                    .data('daterangepicker')
                    .minDate = picker.startDate;
            }

            // TargetDate Validation
            if (modelField === 'TargetDate') {

                var startDate = $('#StartDate').val();

                if (startDate) {

                    if (
                        moment(date, 'DD/MM/YYYY')
                        .isBefore(moment(startDate, 'DD/MM/YYYY'))
                    ) {

                        alert('Target Date cannot be less than Start Date');
                        return false;
                    }
                }
            }

            $(this).val(date);

            if (scope) {
                scope.$applyAsync(function () {
                    scope.newproject[modelField] = date;
                });
            }
        });

        // Manual typing validation
        $(selector).on('blur', function () {

            var value = $(this).val().trim();

            if (value === '')
                return;

            if (!moment(value, 'DD/MM/YYYY', true).isValid()) {

                alert('Please enter valid date (DD/MM/YYYY)');
                $(this).val('');

                var scope = angular.element(this).scope();

                if (scope) {
                    scope.$applyAsync(function () {
                        scope.newproject[modelField] = null;
                    });
                }

                return;
            }

            // StartDate vs TargetDate
            if (modelField === 'StartDate') {

                var targetDate = $('#TargetDate').val();

                if (targetDate &&
                    moment(value, 'DD/MM/YYYY')
                    .isAfter(moment(targetDate, 'DD/MM/YYYY'))) {

                    alert('Start Date cannot be greater than Target Date');

                    $(this).val('');

                    if (scope) {
                        scope.$applyAsync(function () {
                            scope.newproject.StartDate = null;
                        });
                    }

                    return;
                }
            }

            // TargetDate vs StartDate
            if (modelField === 'TargetDate') {

                var startDate = $('#StartDate').val();

                if (startDate &&
                    moment(value, 'DD/MM/YYYY')
                    .isBefore(moment(startDate, 'DD/MM/YYYY'))) {

                    alert('Target Date cannot be less than Start Date');

                    $(this).val('');

                    if (scope) {
                        scope.$applyAsync(function () {
                            scope.newproject.TargetDate = null;
                        });
                    }

                    return;
                }
            }

            if (scope) {
                scope.$applyAsync(function () {
                    scope.newproject[modelField] = value;
                });
            }
        });

        $(selector).on('cancel.daterangepicker', function () {

            var scope = angular.element(this).scope();

            $(this).val('');

            if (scope) {
                scope.$applyAsync(function () {
                    scope.newproject[modelField] = null;
                });
            }
        });
    }

    initDatePicker('#StartDate', 'StartDate');
    initDatePicker('#TargetDate', 'TargetDate');
    initDatePicker('#AwardDate', 'AwardDate');

    // Calendar icon click
    $('.search__arrow').on('click', function () {

        var picker = $(this).closest('.search__wrap').find('input').data('daterangepicker');
        if (picker) picker.show();
    });

});
</script>
    <%--<script>
$(document).ready(function () {

    function initDatePicker(selector, modelField) {

        $(selector).daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            autoUpdateInput: false,
            locale: {
                format: 'DD/MM/YYYY',
                cancelLabel: 'Clear'
            }
        });

        $(selector).on('apply.daterangepicker', function (ev, picker) {

            var scope = angular.element(this).scope();
            var date = picker.startDate.format('DD/MM/YYYY');

            // ==========================
            // START DATE VALIDATION
            // ==========================
            if (modelField === 'StartDate') {

                var targetDate = $('#TargetDate').val();

                if (targetDate) {

                    var startMoment = moment(date, 'DD/MM/YYYY');
                    var targetMoment = moment(targetDate, 'DD/MM/YYYY');

                    if (startMoment.isAfter(targetMoment)) {

                        alert('Start Date cannot be greater than Target Date');

                        $(this).val('');

                        if (scope) {
                            scope.$applyAsync(function () {
                                scope.newproject.StartDate = null;
                            });
                        }

                        return false;
                    }
                }

                // TargetDate minimum = StartDate
                $('#TargetDate')
                    .data('daterangepicker')
                    .minDate = picker.startDate;
            }

            // ==========================
            // TARGET DATE VALIDATION
            // ==========================
            if (modelField === 'TargetDate') {

                var startDate = $('#StartDate').val();

                if (startDate) {

                    var startMoment = moment(startDate, 'DD/MM/YYYY');
                    var targetMoment = moment(date, 'DD/MM/YYYY');

                    if (targetMoment.isBefore(startMoment)) {

                        alert('Target Date cannot be less than Start Date');

                        $(this).val('');

                        if (scope) {
                            scope.$applyAsync(function () {
                                scope.newproject.TargetDate = null;
                            });
                        }

                        return false;
                    }
                }
            }

            $(this).val(date);

            if (scope) {
                scope.$applyAsync(function () {
                    scope.newproject[modelField] = date;
                });
            }
        });

        $(selector).on('cancel.daterangepicker', function () {

            var scope = angular.element(this).scope();

            $(this).val('');

            if (scope) {
                scope.$applyAsync(function () {
                    scope.newproject[modelField] = null;
                });
            }

            // StartDate clear => remove TargetDate restriction
            if (modelField === 'StartDate') {

                $('#TargetDate')
                    .data('daterangepicker')
                    .minDate = false;
            }
        });
    }

    initDatePicker('#AwardDate', 'AwardDate');
    initDatePicker('#StartDate', 'StartDate');
    initDatePicker('#TargetDate', 'TargetDate');

});
</script>--%>
    
    
       <script>
    function fsFunction(){
        var element = document.getElementById("addnewproject");
        element.classList.add("canvas-fullscreen");
    }
    function fseFunction(){
        var element = document.getElementById("addnewproject");
        element.classList.remove("canvas-fullscreen");
    }
    </script>
    <script>
    function fs1Function(){
        var element = document.getElementById("addfiles");
        element.classList.add("canvas-fullscreen");
    }
    function fse1Function(){
        var element = document.getElementById("addfiles");
        element.classList.remove("canvas-fullscreen");
    }
    </script>
        <script>

$(function () {

    // =========================================
    // MAIN DATERANGE
    // =========================================
    $('#daterange').daterangepicker({

    autoUpdateInput: true,
    autoApply: true,

    // Temporary UI default. Angular replaces it with the saved DB filter.
    // If no DB value exists, This Month remains the default.
    startDate: moment().startOf('month'),
    endDate: moment().endOf('month'),

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
            
            var searchkey = picker.chosenLabel;
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
                    todate,
                    searchkey
                );
                initCustomCalendar();
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
    // RANGE CLICK
    // =========================================
    picker.container.find('.ranges li').on(
        'click',
        function () {

            var label =
                $(this).attr('data-range-key');



            // =====================================
            // CUSTOM RANGE
            // =====================================
            if (label === 'Custom Range') {

                // HIDE MAIN CALENDAR
                picker.container
                    .find('.drp-calendar')
                    .hide();

                // HIDE DEFAULT BUTTON
                picker.container
                    .find('.drp-buttons')
                    .hide();

                // SHOW CUSTOM BOX
                picker.container
                    .find('.custom-range-box')
                    .show();

                // DEFAULT VALUE
                $('#customFromDate').val(
                    picker.startDate.format(
                        'DD/MM/YYYY'
                    )
                );

                $('#customToDate').val(
                    picker.endDate.format(
                        'DD/MM/YYYY'
                    )
                );



                // =====================================
                // FROM DATE PICKER
                // =====================================
                if (
                    !$('#customFromDate')
                        .data('daterangepicker')
                ) {

                    $('#customFromDate')
                        .daterangepicker({

                            singleDatePicker: true,
                            autoApply: true,
                            autoUpdateInput: true,
                            showDropdowns: true,
                            parentEl: '.custom-range-box',

                            locale: {
                                format: 'DD/MM/YYYY'
                            }

                        });

                    $('#customFromDate').on(
                        'apply.daterangepicker',
                        function (ev, p) {

                            $(this).val(
                                p.startDate.format(
                                    'DD/MM/YYYY'
                                )
                            );

                        }
                    );
                }



                // =====================================
                // TO DATE PICKER
                // =====================================
                if (
                    !$('#customToDate')
                        .data('daterangepicker')
                ) {

                    $('#customToDate')
                        .daterangepicker({

                            singleDatePicker: true,
                            autoApply: true,
                            autoUpdateInput: true,
                            showDropdowns: true,
                            parentEl: '.custom-range-box',

                            locale: {
                                format: 'DD/MM/YYYY'
                            }

                        });

                    $('#customToDate').on(
                        'apply.daterangepicker',
                        function (ev, p) {

                            $(this).val(
                                p.startDate.format(
                                    'DD/MM/YYYY'
                                )
                            );

                        }
                    );
                }

            }



            // =====================================
            // NORMAL RANGE
            // =====================================
            else {

                // HIDE CUSTOM BOX
                picker.container
                    .find('.custom-range-box')
                    .hide();

                // HIDE CALENDAR
                picker.container
                    .find('.drp-calendar')
                    .hide();

                // SHOW BUTTONS
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



            // CLOSE
            picker.hide();



            // CALL ANGULAR
            callAngularBind(
                fromdate,
                todate,
                'custom'
            );
            initCustomCalendar();
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
    todate,
    searchtype
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
        var searchby = searchtype;
        //alert(searchby);
        scope.$applyAsync(function () {
            
            scope.fromdate = fromdate;

            scope.todate = todate;



            if (
                typeof scope.bindtasklist2
                === 'function'
            ) {

                scope.bindtasklist2(
                    fromdate,
                    todate,
                    searchby
                );
            }

        });
    }
    initCustomCalendar();
}
function initCustomCalendar() {

    $('#customFromDate, #customToDate').daterangepicker({
        singleDatePicker: true,
        autoUpdateInput: false,
        locale: {
            format: 'DD/MM/YYYY'
        }
    });

    // set value on selection
    $('#customFromDate').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY'));
    });

    $('#customToDate').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY'));
    });
}
</script>
    <script src="/pms/assets/js/jscolor.js"></script>
    <script>
document.addEventListener('DOMContentLoaded', function () {

    // Close custom lists before the browser moves focus with Tab or Shift+Tab.
    // Use delegation so dynamically rendered issue/dependency pickers are covered.
    document.addEventListener('keydown', function (event) {
        if (event.key !== 'Tab' && event.keyCode !== 9) return;

        document.querySelectorAll('.search__dropdown').forEach(function (menu) {
            menu.style.display = 'none';
            var model = menu.getAttribute('ng-show');
            var scope = angular.element(menu).scope();
            if (model && scope) {
                scope.$eval(model + ' = false');
                scope.$applyAsync();
            }
        });

        var activeScope = angular.element(event.target).scope();
        if (activeScope) {
            if (activeScope.issueMulti) {
                activeScope.issueMulti.open = '';
                activeScope.issueMulti.active = -1;
            }
            if (activeScope.completionDependency) activeScope.completionDependency.menu = '';
            activeScope.$applyAsync();
        }

        if (window.bootstrap && window.bootstrap.Dropdown) {
            document.querySelectorAll('[data-bs-toggle="dropdown"]').forEach(function (trigger) {
                var instance = window.bootstrap.Dropdown.getInstance(trigger);
                if (instance) instance.hide();
            });
        }
        // Do not preventDefault: retain the browser's normal focus navigation.
    });


    document.querySelectorAll('.search__wrap').forEach(wrapper => {

        const tagInput = wrapper.querySelector('.tag-input');
        const input = wrapper.querySelector('.search__input');
        const dropdown = wrapper.querySelector('.search__dropdown');
        const arrow = wrapper.querySelector('.search__arrow');
        const noRecord = wrapper.querySelector('.dropdown_bl__no-record');

        let selectedIndex = -1;

        if (!input || !dropdown) return;

        const originalPlaceholder = input.getAttribute('placeholder');

        function getItems() {
            return [...wrapper.querySelectorAll('.dropdown_bl__link:not(.search-more)')];
        }

        function getVisibleItems() {
            return getItems().filter(function (item) {
                return item.style.display !== 'none';
            });
        }

        // MAC FIX: scroll only inside this dropdown. element.scrollIntoView() also
        // scrolls the offcanvas/page in Safari (and older Safari ignores {block:'nearest'}).
        function scrollItemIntoDropdown(item) {
            if (!item) return;
            var top = item.offsetTop;
            var bottom = top + item.offsetHeight;
            if (top < dropdown.scrollTop) {
                dropdown.scrollTop = top;
            } else if (bottom > dropdown.scrollTop + dropdown.clientHeight) {
                dropdown.scrollTop = bottom - dropdown.clientHeight;
            }
        }

        function highlightSelected() {

            getItems().forEach(function (item) {
                item.classList.remove('active-item');
            });

            const visibleItems = getVisibleItems();

            if (
                selectedIndex >= 0 &&
                selectedIndex < visibleItems.length
            ) {

                visibleItems[selectedIndex].classList.add('active-item');

                scrollItemIntoDropdown(visibleItems[selectedIndex]);
            }
        }

        function updatePlaceholder() {

            if (!tagInput) return;

            const hasTags =
                tagInput.querySelectorAll('.tag').length > 0;

            input.placeholder = hasTags
                ? 'Search team'
                : originalPlaceholder;
        }

        const visibilityModel = dropdown.getAttribute('ng-show');
        const wrapperScope = angular.element(wrapper).scope();
        function setVisible(visible) {
            if (visibilityModel && wrapperScope) {
                // The stylesheet hides menus by default; set display as well as Angular state.
                dropdown.style.display = visible ? 'block' : 'none';
                wrapperScope.$eval(visibilityModel + ' = ' + (visible ? 'true' : 'false'));
                wrapperScope.$applyAsync();
            } else {
                dropdown.style.display = visible ? 'block' : 'none';
            }
        }
        // MAC FIX: ng-focus/ng-change set the Angular flag only, while the stylesheet
        // keeps .search__dropdown hidden until display:block is set here. The flag could
        // therefore say "open" while nothing was visible (first arrow click closed it,
        // arrow keys highlighted invisible rows). Open = flag set AND really visible.
        function isActuallyVisible() {
            try {
                return window.getComputedStyle(dropdown).display !== 'none';
            } catch (ex) {
                return dropdown.style.display === 'block';
            }
        }
        function isOpen() {
            var modelOpen = visibilityModel && wrapperScope
                ? !!wrapperScope.$eval(visibilityModel)
                : dropdown.style.display === 'block';
            return modelOpen && isActuallyVisible();
        }
        function openDropdown() {

            selectedIndex = -1;

            setVisible(true);
        }

        function closeDropdown() {

            selectedIndex = -1;

            setVisible(false);
        }

        function updateNoRecord() {
            if (input.hasAttribute('ng-model')) return;

            const visible = getItems().some(function (i) {
                return i.style.display !== 'none';
            });

            if (noRecord) {
                noRecord.classList.toggle('d-none', visible);
            }
        }

        // MAC FIX: inputs whose ng-focus opens the list (Client, Sales Order, Project
        // Manager, Country, State) now also show it when focus arrives without a mouse
        // click (Tab, label, Safari/Chrome focus restore). Angular state was already "open".
        if (input.hasAttribute('ng-focus')) {
            input.addEventListener('focus', function () {
                if (visibilityModel && wrapperScope && wrapperScope.$eval(visibilityModel)) {
                    dropdown.style.display = 'block';
                }
            });
        }

        // INPUT CLICK
        (tagInput || input).addEventListener('click', function (e) {

            e.stopPropagation();

            document.querySelectorAll('.search__dropdown').forEach(function (dd) {

                if (dd !== dropdown) {
                    var otherModel = dd.getAttribute('ng-show');
                    var otherScope = angular.element(dd).scope();
                    if (otherModel && otherScope) {
                        dd.style.display = 'none';
                        otherScope.$eval(otherModel + ' = false');
                        otherScope.$applyAsync();
                    } else { dd.style.display = 'none'; }
                }
            });

            openDropdown();
            input.focus();
        });

        // ARROW CLICK
        if (arrow) {

            arrow.addEventListener('click', function (e) {

                e.stopPropagation();

                document.querySelectorAll('.search__dropdown').forEach(function (dd) {

                    if (dd !== dropdown) {
                        var otherModel = dd.getAttribute('ng-show');
                    var otherScope = angular.element(dd).scope();
                    if (otherModel && otherScope) {
                        dd.style.display = 'none';
                        otherScope.$eval(otherModel + ' = false');
                        otherScope.$applyAsync();
                    } else { dd.style.display = 'none'; }
                    }
                });

                if (isOpen()) {

                    closeDropdown();

                } else {

                    openDropdown();
                    input.focus();
                }
            });
        }

        // SEARCH
        input.addEventListener('input', function () {

            selectedIndex = -1;

            // Selection closes the menu without blurring the input.
            // Reopen it when the user edits the single-selection search.
            if (!tagInput) {
                openDropdown();
            }

            if (input.hasAttribute('ng-model')) {
                // Angular filters own these rows; do not leave stale inline hiding.
                getItems().forEach(function (item) { item.style.removeProperty('display'); });
                openDropdown();
                return;
            }
            const value = input.value.toLowerCase();

            let visibleCount = 0;

            getItems().forEach(function (item) {

                const match = item.textContent
                    .toLowerCase()
                    .includes(value);

                item.style.display = match ? 'block' : 'none';

                if (match) {
                    visibleCount++;
                }
            });

            if (noRecord) {

                noRecord.classList.toggle(
                    'd-none',
                    visibleCount !== 0
                );
            }

            highlightSelected();
        });

        // KEYBOARD NAVIGATION
        input.addEventListener('keydown', function (e) {

            // MAC FIX: while macOS composes text (press-and-hold accents, Hindi/Indic or
            // other input sources) Enter/arrows belong to the composition, not the list.
            if (e.isComposing || e.keyCode === 229) return;

            if ((e.key === 'ArrowDown' || e.key === 'ArrowUp') && !isOpen()) {
                e.preventDefault();
                openDropdown();
                // Lazy option rows appear during the Angular digest.
                if (wrapperScope) {
                    wrapperScope.$$postDigest(function () {
                        var items = getVisibleItems();
                        selectedIndex = e.key === 'ArrowUp' ? items.length - 1 : (items.length ? 0 : -1);
                        highlightSelected();
                    });
                    return;
                }
            }
            const visibleItems = getVisibleItems();

            if (!visibleItems.length) return;

            // DOWN
            if (e.key === 'ArrowDown') {

                e.preventDefault();

                if (!isOpen()) {
                    openDropdown();
                }

                selectedIndex++;

                if (selectedIndex >= visibleItems.length) {
                    selectedIndex = 0;
                }

                highlightSelected();
            }

            // UP
            else if (e.key === 'ArrowUp') {

                e.preventDefault();

                if (!isOpen()) {
                    openDropdown();
                }

                selectedIndex--;

                if (selectedIndex < 0) {
                    selectedIndex = visibleItems.length - 1;
                }

                highlightSelected();
            }

            // ENTER
            else if (e.key === 'Enter') {

                if (
                    selectedIndex >= 0 &&
                    visibleItems[selectedIndex]
                ) {

                    e.preventDefault();

                    visibleItems[selectedIndex].click();
                }
            }

            // ESC
            else if (e.key === 'Escape') {

                closeDropdown();
            }
        });

        // ITEM CLICK
        dropdown.addEventListener('click', function (e) {

            const item = e.target.closest('.dropdown_bl__link');

            if (!item) return;

            e.preventDefault();
            e.stopPropagation();

            const scope = angular.element(wrapper).scope();

            if (tagInput) {

                // Each option's ng-click already performs its own selection.
                // Clear the Angular query too, not only the displayed input value.
                var modelController = angular.element(input).controller('ngModel');
                if (scope && modelController) {
                    scope.$applyAsync(function () {
                        modelController.$setViewValue('');
                        modelController.$render();
                    });
                } else {
                    input.value = '';
                    input.dispatchEvent(new Event('input', { bubbles: true }));
                }

                updatePlaceholder();

            } else {

                input.value = item.textContent.trim();

                closeDropdown();
            }

            selectedIndex = -1;
        });

        // OUTSIDE CLICK
        document.addEventListener('click', function (e) {

            if (!wrapper.contains(e.target)) {

                closeDropdown();
            }
        });

        updatePlaceholder();
    });
});
</script>

<style>
.search__dropdown .active-item {
    background: #e9ecef;
    color: #000;
}
</style>
    <%--<script>
      document.addEventListener('DOMContentLoaded', function () {

    document.querySelectorAll('.search__wrap').forEach(wrapper => {

        const tagInput = wrapper.querySelector('.tag-input');
        const input = wrapper.querySelector('.search__input');
        const dropdown = wrapper.querySelector('.search__dropdown');
        const arrow = wrapper.querySelector('.search__arrow');
        const noRecord = wrapper.querySelector('.dropdown_bl__no-record');

        if (!input || !dropdown) return;

        const originalPlaceholder = input.getAttribute('placeholder');

        function getItems() {
            return [...wrapper.querySelectorAll('.dropdown_bl__link:not(.search-more)')];
        }

        function updatePlaceholder() {

            if (!tagInput) return;

            const hasTags = tagInput.querySelectorAll('.tag').length > 0;

            input.placeholder = hasTags
                ? 'Search tag'
                : originalPlaceholder;
        }

        function openDropdown() {
            dropdown.style.display = 'block';
        }

        function closeDropdown() {
            dropdown.style.display = 'none';
        }

        function updateNoRecord() {

            const visible = getItems().some(function (i) {
                return i.style.display !== 'none';
            });

            if (noRecord) {
                noRecord.classList.toggle('d-none', visible);
            }
        }

        // INPUT CLICK
        (tagInput || input).addEventListener('click', function (e) {

            e.stopPropagation();

            document.querySelectorAll('.search__dropdown').forEach(function (dd) {

                if (dd !== dropdown) {
                    dd.style.display = 'none';
                }
            });

            if (dropdown.style.display === 'block') {

                closeDropdown();

            } else {

                openDropdown();
                input.focus();
            }
        });

        // ARROW CLICK
        if (arrow) {

            arrow.addEventListener('click', function (e) {

                e.stopPropagation();

                document.querySelectorAll('.search__dropdown').forEach(function (dd) {

                    if (dd !== dropdown) {
                        dd.style.display = 'none';
                    }
                });

                if (dropdown.style.display === 'block') {

                    closeDropdown();

                } else {

                    openDropdown();
                    input.focus();
                }
            });
        }

        // SEARCH
        input.addEventListener('input', function () {

            const value = input.value.toLowerCase();

            let visibleCount = 0;

            getItems().forEach(function (item) {

                const match = item.textContent
                    .toLowerCase()
                    .includes(value);

                item.style.display = match ? 'block' : 'none';

                if (match) {
                    visibleCount++;
                }
            });

            if (noRecord) {

                noRecord.classList.toggle(
                    'd-none',
                    visibleCount !== 0
                );
            }
        });

        // ITEM CLICK
        dropdown.addEventListener('click', function (e) {

            const item = e.target.closest('.dropdown_bl__link');

            if (!item) return;

            e.preventDefault();
            e.stopPropagation();

            const scope = angular.element(wrapper).scope();

            if (tagInput) {

                if (scope) {

                    scope.$applyAsync(function () {

                        scope.addTeam({
                            id: item.dataset.id,
                            name: item.textContent.trim()
                        });
                    });
                }

                input.value = '';
                input.dispatchEvent(new Event('input'));

                updatePlaceholder();

                // MULTI SELECT => KEEP OPEN

            } else {

                input.value = item.textContent.trim();

                closeDropdown();
            }
        });

        // OUTSIDE CLICK
        document.addEventListener('click', function (e) {

            if (!wrapper.contains(e.target)) {

                closeDropdown();
            }
        });

        updatePlaceholder();
    });
});
    </script>--%>
    
</asp:Content>
