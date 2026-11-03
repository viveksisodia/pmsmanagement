<%--<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="products_default" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/pms/angularmaster/angular_head.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="products_default" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script src="/pms/angularmaster/jsapp/pmsapp.js?v=<%=DateTime.Now.Ticks %>"></script>    
    <script>
        window.PmsAssignPermissions = {
            ischeckEdit: <%= ischeckEdit %>,
            ischeckEdit_task: <%= ischeckEdit_task %>,
            ischeckEdit_stask: <%= ischeckEdit_stask %>,
            ischeckAssign: <%= ischeckAssign%>,
            ischeckAssign_task: <%= ischeckAssign_task %>,
            ischeckAssign_stask: <%= ischeckAssign_stask %>
        };
    </script>
     <link rel="stylesheet" href="/pms/new/assets/css/dragula.min.css" />
    <script src="/pms/assignprojects/app/assignproject.main.js?v=<%=DateTime.Now.Ticks %>"></script> 
    <script src="/pms/assignprojects/app/assignproject.service.js?v=<%=DateTime.Now.Ticks %>"></script> 
    	<script src="/pms/assets/js/jquery.dataTables.min.js"></script>
    <script src="/pms/assets/js/angular_ui_mask_1_8_7.js"></script>
     
   <style>
        
        #addowners .pms-team-person-option { display:block!important;width:100%;padding:8px 12px!important;line-height:20px;font-size:13px;text-decoration:none;color:#334155;white-space:normal;cursor:pointer;border-radius:3px; }
        #addowners .pms-team-person-option:hover, #addowners .pms-team-person-option.pms-team-key-active { background:#e4edff!important;color:#183f85!important; }
        #addowners .pms-team-person-selected::after { content:'✓';float:right;color:#3578df;font-weight:bold; }

        .pms-person-initials { color:#fff;background:#3578df;width:100%;height:100%;display:flex;align-items:center;justify-content:center;font-size:11px;font-weight:600; }
        .dropdown-item.pms-owner-key-active { background:#dceaff!important;color:#182c48!important; }
        /* Compact employee / owner avatars, independent of source image dimensions. */
        .pms-person-avatar { width:28px!important;height:28px!important;min-width:28px;max-width:28px;flex:0 0 28px;display:inline-flex!important;align-items:center;justify-content:center;border-radius:50%;overflow:hidden;background:#edf1f5;border:2px solid #fff;box-shadow:0 1px 3px rgba(0,0,0,.15);vertical-align:middle; }
        .pms-person-avatar img { width:100%!important;height:100%!important;max-width:100%!important;object-fit:cover;display:block; }
        .pms-owner-avatar-trigger { display:inline-flex;align-items:center;justify-content:center; }
        .pms-kanban-meta .pms-person-avatar { width:26px!important;height:26px!important;min-width:26px;max-width:26px;flex-basis:26px; }

    .assign-center-tooltip-popup { opacity: 1 !important; }
    .assign-center-tooltip-popup .tooltip-inner {
        width: 300px;
        max-width: 300px;
        box-sizing: border-box;
        background-color: #d9f2d9 !important;
        color: #000 !important;
        text-align: left;
        overflow-wrap: anywhere;
        padding: 10px 12px;
    }
    .assign-center-tooltip-popup .tooltip-inner * { color: #000 !important; }
    .assign-center-tooltip-popup .tooltip-inner strong { font-weight: 700; }
    .assign-center-tooltip-popup.bs-tooltip-top .tooltip-arrow::before { border-top-color: #d9f2d9 !important; }
    .assign-center-tooltip-popup.bs-tooltip-bottom .tooltip-arrow::before { border-bottom-color: #d9f2d9 !important; }
    .assign-center-tooltip-popup.bs-tooltip-start .tooltip-arrow::before { border-left-color: #d9f2d9 !important; }
    .assign-center-tooltip-popup.bs-tooltip-end .tooltip-arrow::before { border-right-color: #d9f2d9 !important; }

        body {
            background-color: #f1f5f7;
        }
        .header-date {
            display: inline-block;
            cursor: pointer;
            user-select: none;
        }
        #datePicker {
            position: absolute;
            opacity: 0;
            pointer-events: none;
            left:0;
            bottom:0;
        }
        #datePicker_mile {
            position: absolute;
            opacity: 0;
            pointer-events: none;
            left:0;
            bottom:0;
        }
         #datePicker_task {
            position: absolute;
            opacity: 0;
            pointer-events: none;
            left:0;
            bottom:0;
        }
          #datePicker_subtask {
            position: absolute;
            opacity: 0;
            pointer-events: none;
            left:0;
            bottom:0;
        }

        /* Start-date editor: only the formatted date div is visible.
           The native date input remains transparent only to open the browser calendar. */
        .hierarchy-startdate-click {
            position: relative;
            display: inline-block;
            cursor: pointer;
            user-select: none;
            min-width: 78px;
        }
        .hierarchy-startdate-click.is-readonly {
            cursor: default;
        }
        .hierarchy-startdate-picker {
            position: absolute !important;
            width: 1px !important;
            height: 1px !important;
            min-width: 1px !important;
            padding: 0 !important;
            margin: 0 !important;
            border: 0 !important;
            opacity: 0 !important;
            pointer-events: none !important;
            left: 0;
            bottom: 0;
            overflow: hidden;
        }
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

 .loader-overlay2 {
position: absolute;
top: 0px; left: 0px;
width: 100%; height: 100%;
background: rgba(255,255,255,1);
display: flex !important;
justify-content: center;
align-items: center;
z-index: 9999;
}

.loader2 {
border: 8px solid #f3f3f3;
border-top: 2px solid #3498db;
border-radius: 50%;
width: 60px;
height: 60px;
animation: spin 1s linear infinite;
}
    
        .assign-no-permission {
            opacity: .55;
            cursor: not-allowed !important;
        }
        .assign-no-permission * {
            cursor: not-allowed !important;
        }
   </style>
    <style>
        .toggle {
            cursor: pointer;
            display: inline-block;
            width: 20px;
            transition: .3s;
            font-weight: bold;
        }

        .rotate {
            transform: rotate(-90deg);
        }

        .tbl_new td.level-1 {
            padding-left: 20px !important;
        }

        .tbl_new td.level-2 {
            padding-left: 55px !important;
        }

        .tbl_new td.level-3 {
            padding-left: 80px !important;
        }

        /* HIDE ALL CHILD ROWS FIRST */
        .hidden-row {
            display: none;
        }
    </style>
    <style>
       .owner-del {color:#fff;background:#ff0000;width:12px;line-height:12px;font-size:9px;border-radius:50%;text-align: center;display:none;position:absolute;top:-4px;right:-4px;cursor:pointer;}
       a.owner-wrp {position:relative;}
a.owner-wrp:hover .owner-del {display:block !important;}
       .files_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.issue_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}
.dependency_count {color:#fff;background:#ff0000;line-height:12px;font-size:9px;border-radius:8px;text-align: center;position:absolute;top:-4px;right:-7px;padding:0 3px;}

    </style>
    
    <style>
        .pmsinput {border:0px;outline:none;background-color:transparent;padding:4px;border-bottom: 1px solid;
    border-color: transparent;
    line-height: 21px;text-align:left}
.pmsinput:hover {border-bottom:1px solid #ccc;}
.pmsinput:focus {border-bottom:1px solid #72075d;}
.duration-box {
    display: inline-flex;
    align-items: center;
    flex-wrap: nowrap;
    width: 80px;
    overflow: hidden;
}

.duration-input {
    border: none !important;
    outline: none;
    height: 24px;
    font-size: 13px;
    background: transparent;
}

.duration-input:hover {
    border-bottom: 1px solid #ddd !important;
}

.duration-input:focus {
    border-bottom: 1px solid #72075d !important;
}

/* Hours */
.duration-input.left {
    width: 35px;
    text-align: right;
    padding-right: 4px;
}

/* Separator */
.sls_2 {
    flex: 0 0 auto;
    padding: 0 2px;
    font-weight: 600;
    line-height: 24px;
}

/* Minutes */
.duration-input.right {
    width: 25px;
    text-align: left;
    padding-left: 4px;
}
    </style>

    <style>
/* === Corrected Assign Task Timeline UI === */
.wrapper{display:flex;width:100%;height:452px;overflow:hidden;background:#fff;}
.user-panel{width:220px;flex:0 0 220px;border-right:1px solid #ddd;background:#fff;overflow:hidden;}
.timeline-wrapper{flex:1;min-width:0;height:452px;overflow:auto;background:#fff;position:relative;}
.task-panel{width:280px;flex:0 0 280px;border-left:1px solid #ddd;background:#fff;overflow:hidden;}
.user-header,.task-header{height:52px;display:flex;align-items:center;padding:0 12px;border-bottom:1px solid #ddd;background:#fff;box-sizing:border-box;}
.user-row{height:40px;display:flex;align-items:center;gap:10px;padding:0 10px;border-bottom:1px solid #eee;white-space:nowrap;box-sizing:border-box;}
.user-row img{width:26px;height:26px;border-radius:50%;object-fit:cover;}
.user-id{margin-left:auto;font-size:11px;color:#666;}
.timeline-header{height:52px;display:flex;position:sticky;top:0;z-index:20;background:#fff;border-bottom:1px solid #ddd;min-width:max-content;box-sizing:border-box;}
.time-col{width:100px;min-width:100px;flex:0 0 100px;height:52px;text-align:center;border-right:1px solid #e5e5e5;font-size:12px;padding-top:7px;box-sizing:border-box;background:#fff;}
.time-col span{display:block;margin-top:2px;color:#111;}

.available-time-head{width:85px;min-width:85px;flex:0 0 85px;height:52px;display:flex;align-items:center;justify-content:center;text-align:center;border-right:1px solid #ddd;background:#fff;font-size:12px;font-weight:600;color:#334155;box-sizing:border-box;position:sticky;left:0;z-index:30;box-shadow:2px 0 0 #ddd;}
.available-time-cell{position:sticky;left:0;top:0;width:85px;min-width:85px;height:40px;display:flex;align-items:center;justify-content:center;border-right:1px solid #e5e7eb;background:#fff;z-index:15;box-sizing:border-box;box-shadow:2px 0 0 #e5e7eb;}
.available-time-pill{display:inline-flex;align-items:center;justify-content:center;min-width:78px;padding:2px 4px;border-radius:12px;background:#eaffea;border:1px solid #88d18a;color:#176c2e;font-size:12px;font-weight:600;line-height:18px;}
.available-time-pill.zero{background:#fff1f2;border-color:#f4a6b2;color:#b4232f;}
.timeline-row{position:relative;height:40px;min-width:max-content;border-bottom:1px solid #e9e9e9;background:#fff;}
.available-strip{position:absolute;left:0;top:2px;height:35px;background:#fff;border:1px dashed #bfc7ce;box-sizing:border-box;z-index:1;}
.available-strip:after{content:'';}
.timeline-row.shift-closed .available-strip{background:#fff;border-color:#f0f0f0;}
.planned-time{margin-left:2px;}
.grid{position:absolute;left:0;top:0;height:100%;display:flex;z-index:2;pointer-events:none;}
.grid div{width:25px;min-width:25px;height:100%;border-right:1px solid #e6eef4;box-sizing:border-box;}
.planned-time,.task-bar{position:absolute;top:10px;height:20px;line-height:20px;background:#9fb5c7;border:1px solid #7f9aae;border-radius:2px;font-size:12px;color:#111;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;text-align:center;z-index:3;box-sizing:border-box;}
.center-planned-time{background:#cbd9e5;border-color:#9fb5c7;color:#111;}
.center-planned-time .planned-pill{background:#fff;color:#111;}
.assign-drag-preview{background:#fff !important;border:1px solid #9ec5fe !important;color:#111 !important;box-shadow:0 3px 10px rgba(0,0,0,.18) !important;border-radius:4px !important;}
.assign-legend{display:none;}
.assign-legend span{display:inline-flex;align-items:center;gap:4px;}
.legend-box{width:14px;height:10px;border-radius:2px;display:inline-block;}
.legend-assigned{background:#5aa7ff;border:1px solid #2580dd;}
.legend-available{background:#eaffea;border:1px dashed #63c76a;}
.legend-busy{background:#9fb5c7;border:1px solid #7f9aae;}
.bottom-legend-box{display:inline-block;width:14px;height:10px;margin-right:5px;vertical-align:middle;border-radius:2px;}
.bottom-planned{background:#cbd9e5;border:1px solid #9fb5c7;}
.bottom-available{background:#fff;border:1px dashed #bfc7ce;}
.assign-bottom-legend{font-size:12px;color:#444;}
.planned-time{padding:0 18px 0 3px;display:flex;align-items:center;justify-content:center;}
.planned-pill{max-width:100%;height:18px;line-height:18px;background:#fff;border-radius:3px;padding:0 6px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;}
.planned-pill:before{content:"\2807";color:#9eb0bd;margin-right:5px;font-weight:bold;}
.planned-close{position:absolute;right:6px;top:50%;transform:translateY(-50%);color:#ff4d4f;font-size:13px;cursor:pointer;font-weight:bold;z-index:30;background:#fff;border-radius:50%;width:14px;height:14px;line-height:14px;text-align:center;display:inline-flex;align-items:center;justify-content:center;}
.planned-close:hover{background:#ffecec;color:#d90000;}
.task-bar{display:flex;align-items:center;justify-content:space-between;cursor:move;z-index:4;background:#d7e6f2;}
.task-bar.dragging{z-index:99999;opacity:.92;box-shadow:0 3px 10px rgba(0,0,0,.18);}
.assign-drag-preview,.assign-center-drag-preview{user-select:none;-webkit-user-select:none;position:fixed!important;z-index:2147483647!important;pointer-events:none!important;margin:0!important;transform:none!important;}
.task-list{padding:0 10px 10px 10px;height:400px;overflow:auto;}
.task-card{min-height:27px;margin:6px 0;border:1px solid #b9d5ee;border-radius:3px;background:#fafcff;display:flex;align-items:center;gap:6px;padding:3px 6px;font-size:12px;cursor:move;}
.task-card .task-title{flex:1;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;}
.task-icons{display:flex;gap:8px;align-items:center;}
.resize-handle{width:6px;cursor:ew-resize;height:100%;}


/* === Center panel move + resize time option === */
.center-planned-time{cursor:move;overflow:visible;user-select:none;touch-action:none;}
.center-planned-time .planned-pill{display:flex;align-items:center;gap:4px;max-width:calc(100% - 22px);padding:0 5px;box-sizing:border-box;}
.center-move-icon{color:#7d92a1;font-size:13px;line-height:1;flex:0 0 auto;cursor:move;}
.center-task-title{min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;}
.center-task-time{font-size:10px;color:#52616b;font-weight:600;margin-left:4px;white-space:nowrap;display:none;}
.center-planned-time:hover .center-task-time{display:inline;}
.center-resize-handle{position:absolute;top:-1px;width:7px;height:20px;background:#6f8fa6;border-radius:2px;z-index:8;opacity:.75;cursor:ew-resize;}
.center-resize-left{left:-4px;}
.center-resize-right{right:-4px;}
.center-resize-handle:hover{opacity:1;background:#3f6f8f;}
.center-segment-time-editing,.center-segment-time-editing *{cursor:ew-resize !important;user-select:none !important;}


/* === Table alignment fix: Available Time is a real fixed table column === */
.wrapper{--assign-head-h:52px;--assign-row-h:40px;--available-col-w:85px;}
.user-header,.task-header,.timeline-header{height:var(--assign-head-h) !important;min-height:var(--assign-head-h) !important;}
.user-row,.timeline-row{height:var(--assign-row-h) !important;min-height:var(--assign-row-h) !important;}
#users11{height:400px !important;}
.timeline-wrapper{height:452px !important;overflow:auto !important;}
.timeline-header{align-items:stretch !important;width:max-content !important;}
.available-time-head{width:var(--available-col-w) !important;min-width:var(--available-col-w) !important;flex:0 0 var(--available-col-w) !important;border-right:1px solid #ddd !important;border-bottom:1px solid #ddd !important;box-shadow:2px 0 0 #ddd;}
.available-time-cell{width:var(--available-col-w) !important;min-width:var(--available-col-w) !important;flex:0 0 var(--available-col-w) !important;height:var(--assign-row-h) !important;border-right:1px solid #ddd !important;border-bottom:1px solid #e9e9e9 !important;box-shadow:2px 0 0 #e5e7eb;}
.timeline-row{display:block !important;border-bottom:1px solid #e9e9e9 !important;}
.time-col{height:var(--assign-head-h) !important;border-bottom:1px solid #ddd !important;}
.grid{height:var(--assign-row-h) !important;}
/* keep original timing strip view: only borders/alignment changed, strip top/height/background are not overridden */
.planned-time,.task-bar{top:10px !important;}

/* === Assign right panel hierarchy compact view === */
.task-list.assign-tree-list{padding:6px 8px 10px 8px;height:400px;overflow:auto;}
.assign-tree-row{min-height:28px;margin:3px 0;border:1px solid #b9d5ee;border-radius:3px;background:#fafcff;display:flex;align-items:center;gap:5px;padding:2px 6px;font-size:12px;cursor:move;box-sizing:border-box;}
.assign-tree-row input[type="checkbox"]{width:13px;height:13px;margin:0;flex:0 0 auto;}
.assign-tree-title{flex:1;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;}
.assign-tree-duration{font-weight:600;color:#111;margin-right:3px;flex:0 0 auto;}
.assign-tree-icon{width:14px;min-width:14px;height:18px;display:inline-flex;align-items:center;justify-content:center;color:#111;cursor:pointer;font-size:14px;}
.assign-tree-spacer{width:14px;min-width:14px;}
.assign-tree-remove{color:#ff4d4f;cursor:pointer;font-size:14px;line-height:1;flex:0 0 auto;}
.assign-tree-ok{color:#10c957;font-size:13px;line-height:1;flex:0 0 auto;}
.assign-tree-assigned{background:#f0fff4 !important;}
.assign-tree-assigned input[type=checkbox]{cursor:not-allowed;opacity:.55;}
.top-list-sales a.pms-top-filter-active .tile{
    background:#f3efff;
    box-shadow:inset 0 -2px 0 #6f42c1;
    border-radius:6px;
}
.top-list-sales a.pms-top-filter-active .tile-heading,
.top-list-sales a.pms-top-filter-active .tile-body{color:#6f42c1;}
.pms-task-situation-filter{min-width:150px;height:30px;border:1px solid #e1e4e8;border-radius:6px;background:#fff;padding:0 28px 0 10px;font-size:12px;color:#26313c;}
.assign-tree-task{margin-left:16px;}
.assign-tree-subtask{margin-left:32px;}
.assign-tree-milestone{font-weight:600;}
.assign-tree-task .assign-tree-title{font-weight:500;}
.pms-no-edit{pointer-events:none !important;cursor:not-allowed !important;opacity:.65 !important;}
.pmsinput[readonly],.duration-input[readonly]{cursor:not-allowed !important;opacity:.75 !important;}

</style>
    <style>
        #addcopy .dropdown {
    display: block !important;
    width: 100% !important;
}

#addcopy .dropdown-toggle {
    display: flex !important;
    align-items: center;
    justify-content: space-between;
    width: 100% !important;
}

#addcopy .dropdown-menu {
    width: 100% !important;
    min-width: 100% !important;
    box-sizing: border-box;
}

#addcopy .dropdown-item {
    white-space: normal;
    word-break: break-word;
}
    </style>
     <script>
document.addEventListener('DOMContentLoaded', function () {

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

                visibleItems[selectedIndex].scrollIntoView({
                    block: 'nearest'
                });
            }
        }

        function updatePlaceholder() {

            if (!tagInput) return;

            const hasTags =
                tagInput.querySelectorAll('.tag').length > 0;

            input.placeholder = hasTags
                ? 'Search tag'
                : originalPlaceholder;
        }

        function openDropdown() {

            selectedIndex = -1;

            dropdown.style.display = 'block';
        }

        function closeDropdown() {

            selectedIndex = -1;

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

            selectedIndex = -1;

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

            const visibleItems = getVisibleItems();

            if (!visibleItems.length) return;

            // DOWN
            if (e.key === 'ArrowDown') {

                e.preventDefault();

                if (dropdown.style.display !== 'block') {
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

                if (dropdown.style.display !== 'block') {
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

    <style>
        .pms-timesheet-view {
            width: 100%;
            background: #fff;
            border-top: 1px solid #e5e9ee;
        }
        .pms-timesheet-range {
            display: flex;
            align-items: center;
            gap: 6px;
            min-height: 38px;
            padding: 7px 12px;
            border-bottom: 1px solid #e5e9ee;
            background: #fbfcfd;
            font-size: 13px;
            font-weight: 600;
        }
        .pms-timesheet-app {
            display: grid;
            grid-template-columns: minmax(560px, 42%) minmax(0, 58%);
            width: 100%;
            min-height: 180px;
        }
        .pms-timesheet-left {
            position: relative;
            z-index: 2;
            background: #fff;
            border-right: 1px solid #dfe4e8;
            box-shadow: 2px 0 5px rgba(0,0,0,.035);
        }
        .pms-timesheet-left-head,
        .pms-timesheet-left-row {
            display: grid;
            grid-template-columns: minmax(210px, 2fr) minmax(105px, .9fr) minmax(105px, .8fr) minmax(125px, 1fr);
            align-items: center;
        }
        .pms-timesheet-left-head {
            height: 64px;
            background: #f6f8fa;
            border-bottom: 1px solid #dfe4e8;
            font-size: 11px;
            font-weight: 700;
            color: #4c5864;
            text-transform: uppercase;
        }
        .pms-timesheet-left-head > div,
        .pms-timesheet-left-row > div {
            min-width: 0;
            padding: 0 9px;
        }
        .pms-timesheet-left-row {
            height: 52px;
            border-bottom: 1px solid #edf0f2;
            font-size: 12px;
        }
        .pms-timesheet-left-row:hover {
            background: #fafcff;
        }
        .pms-timesheet-task-title {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            font-weight: 600;
        }
        .pms-timesheet-task-title.level-0 { font-weight: 700; }
        .pms-timesheet-task-title.level-1 { font-weight: 600; }
        .pms-timesheet-task-title.level-2 { font-weight: 500; }
        .pms-timesheet-owners {
            display: flex;
            align-items: center;
            min-height: 28px;
            padding-left: 7px;
        }
        .pms-timesheet-owner-avatar {
            width: 28px;
            height: 28px;
            min-width: 28px;
            margin-left: -7px;
            overflow: hidden;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            border: 2px solid #fff;
            border-radius: 50%;
            background: #edf1f5;
            color: #606b75;
            font-size: 10px;
            text-transform: uppercase;
        }
        .pms-timesheet-owner-avatar:first-child { margin-left: 0; }
        .pms-timesheet-owner-avatar img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
        .pms-timesheet-no-owner { color: #a0a7ae; font-size: 11px; }
        .pms-timesheet-status {
            display: inline-flex;
            align-items: center;
            max-width: 100%;
            padding: 3px 7px;
            border: 1px solid;
            border-radius: 3px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            font-size: 10px;
            font-weight: 600;
        }
        .pms-timesheet-progress-box {
            display: flex;
            align-items: center;
            gap: 6px;
        }
        .pms-timesheet-progress-value {
            width: 38px;
            font-size: 10px;
            font-weight: 600;
            text-align: right;
        }
        .pms-timesheet-progress-bar {
            position: relative;
            flex: 1;
            height: 6px;
            overflow: hidden;
            border-radius: 6px;
            background: #e9edf1;
        }
        .pms-timesheet-progress-fill {
            height: 100%;
            border-radius: 6px;
            background: #f3b400;
        }
        .pms-timesheet-progress-fill.is-complete { background: #00a82d; }
        .pms-timesheet-right {
            min-width: 0;
            overflow-x: auto;
            overflow-y: hidden;
            background: #fff;
        }
        .pms-timesheet-timeline-head {
            position: sticky;
            top: 0;
            z-index: 1;
            height: 64px;
            background: #f6f8fa;
            border-bottom: 1px solid #dfe4e8;
        }
        .pms-timesheet-months {
            display: flex;
            height: 32px;
        }
        .pms-timesheet-month {
            flex: 0 0 auto;
            display: flex;
            align-items: center;
            justify-content: center;
            border-right: 1px solid #dfe4e8;
            border-bottom: 1px solid #e6eaee;
            font-size: 11px;
            font-weight: 700;
            color: #4c5864;
        }
        .pms-timesheet-days {
            display: flex;
            height: 32px;
        }
        .pms-timesheet-day {
            flex: 0 0 24px;
            width: 24px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-right: 1px solid #e8ecef;
            font-size: 10px;
            color: #68737d;
        }
        .pms-timesheet-day.is-weekend { background: #f0f3f6; }
        .pms-timesheet-timeline-row {
            position: relative;
            height: 52px;
            border-bottom: 1px solid #edf0f2;
            background-image: repeating-linear-gradient(
                to right,
                transparent 0,
                transparent 23px,
                #edf0f2 23px,
                #edf0f2 24px
            );
        }
        .pms-timesheet-timeline-row:hover { background-color: #fafcff; }
        .pms-timesheet-bar {
            position: absolute;
            top: 14px;
            height: 24px;
            min-width: 20px;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 0 5px;
            overflow: hidden;
            border-radius: 4px;
            box-shadow: 0 1px 2px rgba(0,0,0,.12);
            color: #000;
            font-size: 10px;
            font-weight: 600;
            white-space: nowrap;
            text-overflow: ellipsis;
        }
        .pms-timesheet-empty {
            padding: 28px 15px;
            text-align: center;
            color: #929aa2;
            background: #fff;
        }
        @media (max-width: 1100px) {
            .pms-timesheet-app { grid-template-columns: 520px minmax(520px, 1fr); }
            .pms-timesheet-view { overflow-x: auto; }
        }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div ng-app="PMSerp" ng-controller="assignprojectCtrl">
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
            <header class="pms-issue-head"><div><h2>{{issuePanel.mode=='list' ? 'Issues' : (issueForm.id ? 'Edit Issue' : 'Add Issue')}}</h2><small>{{issuePanel.context.title}} · {{issuePanel.context.type}}</small></div><button type="button" aria-label="Close issues" ng-click="closeIssueList()">×</button></header>
            <div ng-if="issuePanel.mode=='list'" style="display:flex;flex-direction:column;min-height:0;flex:1">
                <div class="pms-issue-toolbar"><button type="button" class="pms-issue-add" ng-click="openIssueEditor()"><i class="ri-add-line"></i> Add Issue</button><span class="pms-issue-count">{{issueFilteredRows().length}} issue(s)</span><input type="search" ng-model="issuePanel.search" ng-change="issuePanel.page=1" placeholder="Search issues" aria-label="Search issues" /></div>
                <div class="pms-issue-table-wrap"><div ng-if="issuePanel.loading" class="pms-issue-empty">Loading issues…</div><div ng-if="issuePanel.error" class="pms-issue-empty" role="alert">{{issuePanel.error}} <button type="button" ng-click="refreshIssueList()">Retry</button></div>
                    <table class="pms-issue-table" ng-if="!issuePanel.loading && !issuePanel.error"><thead><tr><th>S.No</th><th>Issue</th><th>Owner</th><th>Team</th><th>Status</th><th>Priority</th><th>Assign To</th><th>Create Date</th><th>Target Date</th><th>Issue Type</th><th>Milestone</th><th>Actions</th></tr></thead><tbody><tr ng-repeat="issue in issuePageRows() track by $index"><td>{{issue.number || ((issuePanel.page-1)*issuePanel.pageSize+$index+1)}}</td><td class="issue-title">{{issue.name}}</td><td class="pms-issue-people-cell"><div class="pms-issue-people" ng-if="issue.owners.length"><span class="pms-issue-person" ng-repeat="name in issue.owners | limitTo:3 track by $index" tabindex="0" aria-label="Owner: {{name}}">{{name.slice(0,2)}}<span class="pms-issue-person-tooltip" role="tooltip">{{name}}</span></span><span class="pms-issue-person more" ng-if="issue.owners.length>3" tabindex="0" aria-label="{{issue.owners.length-3}} more owners">+{{issue.owners.length-3}}<span class="pms-issue-person-tooltip" role="tooltip"><span ng-repeat="name in issue.owners.slice(3) track by $index">{{name}}{{$last ? '' : ', '}}</span></span></span></div><span ng-if="!issue.owners.length">—</span></td><td class="pms-issue-people-cell"><div class="pms-issue-people" ng-if="issue.teams.length"><span class="pms-issue-person" ng-repeat="name in issue.teams | limitTo:3 track by $index" tabindex="0" aria-label="Team: {{name}}">{{name.slice(0,2)}}<span class="pms-issue-person-tooltip" role="tooltip">{{name}}</span></span><span class="pms-issue-person more" ng-if="issue.teams.length>3" tabindex="0" aria-label="{{issue.teams.length-3}} more team members">+{{issue.teams.length-3}}<span class="pms-issue-person-tooltip" role="tooltip"><span ng-repeat="name in issue.teams.slice(3) track by $index">{{name}}{{$last ? '' : ', '}}</span></span></span></div><span ng-if="!issue.teams.length">—</span></td><td>{{issue.status || '—'}}</td><td>{{issue.priority || '—'}}</td><td>{{issue.assignee || '—'}}</td><td>{{issue.created || '—'}}</td><td>{{issue.target || '—'}}</td><td>{{issue.IssueTypeName || '—'}}</td><td>{{issue.milestone || '—'}}</td><td class="pms-issue-actions"><button type="button" ng-if="issue.canEdit" title="Edit issue" aria-label="Edit {{issue.name}}" ng-click="editIssue(issue)" ng-disabled="issuePanel.deletingId==issue.id"><i class="ri-edit-line"></i></button><button type="button" ng-if="issue.canDelete" class="delete" title="Delete issue" aria-label="Delete {{issue.name}}" ng-click="deleteIssue(issue)" ng-disabled="issuePanel.deletingId==issue.id"><i class="ri-delete-bin-line"></i></button></td></tr></tbody></table><div ng-if="!issuePanel.loading && !issuePanel.error && !issueFilteredRows().length" class="pms-issue-empty">No issues found for this {{issuePanel.context.type}}.</div></div>
                <footer class="pms-issue-footer"><span>Showing {{issueFilteredRows().length ? ((issuePanel.page-1)*issuePanel.pageSize+1) : 0}}–{{issuePanel.page*issuePanel.pageSize < issueFilteredRows().length ? issuePanel.page*issuePanel.pageSize : issueFilteredRows().length}} of {{issueFilteredRows().length}}</span><span><button type="button" ng-disabled="issuePanel.page<=1" ng-click="issuePanel.page=issuePanel.page-1">Previous</button> Page {{issuePanel.page}} <button type="button" ng-disabled="issuePanel.page*issuePanel.pageSize>=issueFilteredRows().length" ng-click="issuePanel.page=issuePanel.page+1">Next</button></span></footer>
            </div>
            <div ng-if="issuePanel.mode=='add'" style="display:flex;flex-direction:column;min-height:0;flex:1">
                <div class="pms-issue-form" ng-click="closeIssuePersonList($event)">
                    <div class="pms-issue-form-context"><div><small>PROJECT</small><strong>{{projectlist.ProjectName || projectlist.projectname || projectlist.name}}</strong></div><div><small>MILESTONE / TASK / SUBTASK</small><strong>{{issuePanel.context.breadcrumb}}</strong></div><span class="kind">{{issuePanel.context.type}} issue</span></div>
                    <div ng-if="issuePanel.loadingOptions" class="pms-issue-empty">Loading form options…</div>
                    <div class="pms-issue-field"><label for="pmsIssueTitle">Issue title *</label><input id="pmsIssueTitle" type="text" ng-model="issueForm.Issues" maxlength="250" placeholder="Enter a short issue title" /></div>
                    <div class="pms-issue-field" style="margin-top:12px"><label for="pmsIssueDescription">Description</label><textarea id="pmsIssueDescription" ng-model="issueForm.Description" placeholder="Describe the problem and expected outcome"></textarea></div>
                    <h3>People &amp; schedule</h3>
                    <div class="pms-issue-form-grid">
                        <div class="pms-issue-field"><label for="pmsIssueOwnerSearch">Owner</label><div class="pms-issue-single"><input id="pmsIssueOwnerSearch" type="search" placeholder="Search owner" ng-model="issueMulti.search.Owner" ng-focus="openIssuePeople('Owner')" ng-change="issueMulti.active=-1;issueForm.Owner=[]" ng-keydown="issuePersonKeydown('Owner',$event)" autocomplete="off" role="combobox" aria-autocomplete="list" aria-expanded="{{issueMulti.open=='Owner'}}" /><button type="button" ng-if="issueForm.Owner.length" title="Clear owner" aria-label="Clear owner" ng-click="issueForm.Owner=[];issueMulti.search.Owner='';issueMulti.open=''">×</button><span ng-if="!issueForm.Owner.length" aria-hidden="true">⌄</span></div><div class="pms-issue-person-list" ng-if="issueMulti.open=='Owner'"><button type="button" ng-repeat="person in issuePersonMatches('Owner') track by person.id" ng-class="{'active':issueMulti.active==$index}" ng-mouseenter="issueMulti.active=$index" ng-click="chooseIssuePerson('Owner',person)">{{person.name}} <span ng-if="issueForm.Owner.indexOf(''+person.id)>=0">✓</span></button><div class="pms-issue-person-empty" ng-if="!issuePersonMatches('Owner').length">No matching owners</div></div></div>
                        <div class="pms-issue-field"><label>Team</label><div class="pms-issue-multi" ng-click="openIssuePeople('team')"><span class="pms-issue-person-chip" ng-repeat="id in issueForm.team track by id">{{issuePersonName(id)}} <button type="button" ng-click="toggleIssuePerson('team',{id:id});$event.stopPropagation()">×</button></span><input type="search" placeholder="Search team" aria-label="Search team" ng-model="issueMulti.search.team" ng-focus="openIssuePeople('team')" ng-change="issueMulti.active=-1" ng-keydown="issuePersonKeydown('team',$event)" autocomplete="off" /></div><div class="pms-issue-person-list" ng-if="issueMulti.open=='team'"><button type="button" ng-repeat="person in issuePersonMatches('team') track by person.id" ng-class="{'active':issueMulti.active==$index}" ng-mouseenter="issueMulti.active=$index" ng-click="chooseIssuePerson('team',person)">{{person.name}} <span ng-if="issueForm.team.indexOf(person.id)>=0">✓</span></button><div class="pms-issue-person-empty" ng-if="!issuePersonMatches('team').length">No matching team members</div></div></div>
                        <div class="pms-issue-field"><label for="pmsIssueAssigneeSearch">Assign to *</label><div class="pms-issue-single"><input id="pmsIssueAssigneeSearch" type="search" placeholder="Search assignee" ng-model="issueMulti.search.AssignTo" ng-focus="openIssuePeople('AssignTo')" ng-change="issueMulti.active=-1;issueForm.AssignTo=''" ng-keydown="issuePersonKeydown('AssignTo',$event)" autocomplete="off" role="combobox" aria-autocomplete="list" aria-expanded="{{issueMulti.open=='AssignTo'}}" /><button type="button" ng-if="issueForm.AssignTo" title="Clear assignee" aria-label="Clear assignee" ng-click="issueForm.AssignTo='';issueMulti.search.AssignTo=''">×</button><span ng-if="!issueForm.AssignTo" aria-hidden="true">⌄</span></div><div class="pms-issue-person-list" ng-if="issueMulti.open=='AssignTo'"><button type="button" ng-repeat="person in issuePersonMatches('AssignTo') track by person.id" ng-class="{'active':issueMulti.active==$index}" ng-mouseenter="issueMulti.active=$index" ng-click="chooseIssuePerson('AssignTo',person)">{{person.name}} <span ng-if="issueForm.AssignTo==person.id">✓</span></button><div class="pms-issue-person-empty" ng-if="!issuePersonMatches('AssignTo').length">No matching assignees</div></div></div>
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
            <div class="pms-completion-heading"><span>{{completionDependency.mode=='list' ? 'Dependencies' : (completionDependency.mode=='edit' ? 'Edit completion dependency' : 'Add completion dependency')}}</span><button type="button" aria-label="Close" ng-click="closeCompletionDependencyPopup()">×</button></div>
            <div class="pms-completion-body">
                <div ng-if="completionDependency.mode=='list'">
                    <div class="pms-completion-list-top"><span>For: <strong>{{completionDependency.name}}</strong> ({{completionDependency.type}})</span><button type="button" ng-click="addCompletionDependency()" ng-disabled="!completionDependency.milestones.length">+ Add dependency</button></div>
                <div style="margin-top:23px;border-top:1px solid #dce7f3;padding-top:12px"><strong>Existing dependencies</strong><div ng-if="completionDependency.loadingExisting" style="margin-top:8px">Loading…</div><div class="pms-completion-table-wrap" ng-if="!completionDependency.loadingExisting"><table class="pms-completion-table"><thead><tr><th>Milestone</th><th>Task</th><th>Subtask</th><th>Dependency on</th><th>Action</th></tr></thead><tbody><tr ng-repeat="existing in completionDependency.existing track by existing.id"><td>{{existing.milestone || '—'}}</td><td>{{existing.task || '—'}}</td><td>{{existing.subtask || '—'}}</td><td class="pms-completion-linked-paths" title="{{existing.name}}"><span ng-repeat="path in existing.names track by $index">{{path}}</span></td><td style="white-space:nowrap"><button type="button" class="btn btn-sm btn-light" ng-click="editCompletionDependency(existing)" title="Edit dependency" aria-label="Edit dependency"><i class="ri-edit-line"></i></button> <button type="button" class="btn btn-sm btn-light" ng-click="deleteCompletionDependency(existing)" title="Delete dependency" aria-label="Delete dependency"><i class="ri-delete-bin-line"></i></button></td></tr><tr ng-if="!completionDependency.existing.length"><td colspan="5" style="text-align:center;color:#6b7f91">No dependencies added.</td></tr></tbody></table></div></div>
                </div>
                <div ng-if="completionDependency.mode!='list'">
                <div>For: <strong>{{completionDependency.name}}</strong> ({{completionDependency.type}}) · Current project: {{(projectlist.ProjectName || projectlist.projectname || projectlist.name)}}</div>
                <label>Milestone *</label>
                <div class="pms-completion-picker" ng-click="$event.stopPropagation()">
                    <div class="pms-completion-control" ng-click="completionDependency.menu='milestone'">
                        <span class="pms-completion-chip" ng-repeat="option in completionDependency.selected.milestone track by option.Value"><span>{{option.Text}}</span><button type="button" title="Remove {{option.Text}}" aria-label="Remove {{option.Text}}" ng-click="toggleCompletionDependency('milestone',option);$event.stopPropagation()">×</button></span>
                        <input type="text" ng-model="completionDependency.search.milestone" placeholder="Select milestones..." aria-label="Search milestones" ng-focus="completionDependency.menu='milestone'" autocomplete="off" />
                    </div>
                    <div class="pms-completion-choices" ng-if="completionDependency.menu=='milestone'"><button type="button" class="pms-completion-option" ng-repeat="option in completionDependency.milestones | filter:completionDependency.search.milestone track by option.Value" ng-class="{'is-selected':completionDependencySelected('milestone',option.Value)}" ng-click="toggleCompletionDependency('milestone',option);$event.stopPropagation()">{{option.Text}} <span ng-if="completionDependencySelected('milestone',option.Value)" style="float:right">✓</span></button><div ng-if="!(completionDependency.milestones | filter:completionDependency.search.milestone).length" class="pms-completion-empty">No matches</div></div>
                </div>
                <div ng-if="completionDependency.tasks.length"><label>Task</label>
                <div class="pms-completion-picker" ng-click="$event.stopPropagation()">
                    <div class="pms-completion-control" ng-click="completionDependency.menu='task'">
                        <span class="pms-completion-chip" ng-repeat="option in completionDependency.selected.task track by option.Value"><span>{{option.Text}}</span><button type="button" title="Remove {{option.Text}}" aria-label="Remove {{option.Text}}" ng-click="toggleCompletionDependency('task',option);$event.stopPropagation()">×</button></span>
                        <input type="text" ng-model="completionDependency.search.task" placeholder="Select tasks..." aria-label="Search tasks" ng-focus="completionDependency.menu='task'" autocomplete="off" />
                    </div>
                    <div class="pms-completion-choices" ng-if="completionDependency.menu=='task'"><button type="button" class="pms-completion-option" ng-repeat="option in completionDependency.tasks | filter:completionDependency.search.task track by option.Value" ng-class="{'is-selected':completionDependencySelected('task',option.Value)}" ng-click="toggleCompletionDependency('task',option);$event.stopPropagation()">{{option.Text}} <span ng-if="completionDependencySelected('task',option.Value)" style="float:right">✓</span></button><div ng-if="!(completionDependency.tasks | filter:completionDependency.search.task).length" class="pms-completion-empty">No matches</div></div>
                </div>
                </div>
                <div ng-if="completionDependency.subtasks.length"><label>Subtask</label>
                <div class="pms-completion-picker" ng-click="$event.stopPropagation()">
                    <div class="pms-completion-control" ng-click="completionDependency.menu='subtask'">
                        <span class="pms-completion-chip" ng-repeat="option in completionDependency.selected.subtask track by option.Value"><span>{{option.Text}}</span><button type="button" title="Remove {{option.Text}}" aria-label="Remove {{option.Text}}" ng-click="toggleCompletionDependency('subtask',option);$event.stopPropagation()">×</button></span>
                        <input type="text" ng-model="completionDependency.search.subtask" placeholder="Select subtasks..." aria-label="Search subtasks" ng-focus="completionDependency.menu='subtask'" autocomplete="off" />
                    </div>
                    <div class="pms-completion-choices" ng-if="completionDependency.menu=='subtask'"><button type="button" class="pms-completion-option" ng-repeat="option in completionDependency.subtasks | filter:completionDependency.search.subtask track by option.Value" ng-class="{'is-selected':completionDependencySelected('subtask',option.Value)}" ng-click="toggleCompletionDependency('subtask',option);$event.stopPropagation()">{{option.Text}} <span ng-if="completionDependencySelected('subtask',option.Value)" style="float:right">✓</span></button><div ng-if="!(completionDependency.subtasks | filter:completionDependency.search.subtask).length" class="pms-completion-empty">No matches</div></div>
                </div>
                </div>
                </div>
                <p ng-if="completionDependency.error" style="color:#b42318;margin-top:16px" role="alert">{{completionDependency.error}}</p>
                <p ng-if="completionDependency.notice" style="color:#18834b;margin-top:16px" role="status">{{completionDependency.notice}}</p>
            </div>
            <div class="pms-completion-actions"><button type="button" ng-if="completionDependency.mode!='list'" ng-click="backToCompletionDependencyList()">Back to dependencies</button><button type="button" ng-if="completionDependency.mode=='list'" ng-click="closeCompletionDependencyPopup()">Close</button><button type="button" ng-if="completionDependency.mode!='list'" class="primary" ng-disabled="completionDependency.saving || completionDependency.loadingTasks || completionDependency.loadingSubtasks" ng-click="saveCompletionDependency()">{{completionDependency.saving?'Saving…':(completionDependency.editingId?'Update':'Save')}}</button></div>
        </div>
    </div>
    <div class="sales-nav-project" ng-if="isload=='1'">
        <div class="row">
            <div class="col-xxl-6 col-lg-6 col-md-6 col-sm-12">
                <div class="sub-left">
                    <a href="/pms/projects/" id="goproject" data-bs-toggle="tooltip" class="sub-icon-btn me-2" title="Back to Home" style="display:none"></a>
                    <a href="/pms/projects/" data-bs-toggle="tooltip" class="sub-icon-btn me-2" title="Back to Home"><i class="ri-arrow-left-s-line"></i></a>
                    <div class="sub-page-title"><a href="javascript:void(0);" style="color:#000;" ng-click="assignprojecttask('0')">{{projectlist.ProjectName}}</a></div>
                    <div class="btn-group d-block me-3">
                          <%if (ischeckCreate == 1)
                                          {%>
                        <a href="javascript:void(0);"  ng-click="addRowUpDown_default()" type="button" class="sub-soft-btn"><i class="ri-add-line"></i></a>
                       <%} %>
                        <%--  <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="sub-soft-btn"><i class="ri-add-line"></i></a>
                        <div class="dropdown-menu p-0" style="border-radius:0;">
                            <ul class="sttus-list">
                                <li><a class="" href="javascript:void(0);" ng-click="addRowUpDown_default()" data-bs-toggle="offcanvas" data-bs-target="#addnewmilestone">Add Milestone</a></li>
                                <li><a class="" href="javascript:void(0);" data-bs-toggle="offcanvas" data-bs-target="#addnewtask">Add Task</a></li>
                                <li><a class="" href="javascript:void(0);" data-bs-toggle="offcanvas" data-bs-target="#addnewsubtask">Add Subtask</a></li>
                            </ul>
                        </div>--%>
                    </div>
                </div>
            </div>
       <div class="col-xxl-5 col-lg-5 col-md-5 col-sm-12">
            <div class="d-flex justify-content-end">
            <%-- <%if (ischeckCreate == 1)
                                          {%>--%>
                         <a href="javascript:void(0);" id="btncopy" class="sub-soft-btn" data-bs-toggle="modal" data-bs-target="#addcopy" ng-click="loadprojectlist(projectlist.id)" ng-if="(!projectlist.miles || projectlist.miles.length == 0) || projectlist.isadmin"><span data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="Copy From" data-bs-original-title="Copy From"><i class="bx bx-copy"></i></span></a>
                      <%-- <%} %>--%>
            </div>
        </div>
            <div class="col-xxl-1 col-lg-1 col-md-1 col-sm-12"></div>
            </div>
    </div>
    <div class="main-wrapper" ng-if="isload=='1'">
        <div class="sub-header pb-0">
            <div class="row">
                <div class="col-xxl-2 col-lg-2 col-md-2 col-sm-12">
                    <div class="assign_nav">
                        <%--<a href="projects-unassign.html" ng-click="assignprojecttask('1');">Unassign</a>
                        <a href="projects-assign.html" class="active" ng-click="assignprojecttask('2');">Assigned</a>--%>
                        <a href="javascript:void(0);"
   ng-class="{'active': activetype == '1'}"
   ng-click="assignprojecttask('1');">
    Unassign
</a>

<a href="javascript:void(0);"
   ng-class="{'active': activetype == '2'}"
   ng-click="assignprojecttask('2');">
    Assigned
</a>
                    </div>
                </div>
               
                <div class="col-xxl-5 col-lg-5 col-md-5 col-sm-12">
                    <div class="sub-left">
                    <ul class="top-list-sales">
                        <li class="d-flex align-items-center">
                            <select class="pms-task-situation-filter"
                                    ng-model="taskSituationFilter"
                                    ng-change="applyTaskSituationFilter(taskSituationFilter)"
                                    title="Task Situation">
                                <option value="all">All Task Situations</option>
                                <option value="none">No Task Situation</option>
                                <option ng-repeat="situation in taskSituationLists track by situation.id" value="{{situation.id}}">
                                    {{situation.name}}
                                </option>
                            </select>
                        </li>
                        
                    </ul>
                </div>
                </div>
                <div class="col-xxl-5 col-lg-5 col-md-5 col-sm-12">
                    <div class="d-flex justify-content-end align-items-center gap-1">
                        
                        <!--<div class="search_box me-2">
                            <input type="text" class="form-control" id="" placeholder="Search...">
                            <i class="ri-search-line"></i>
                        </div>-->
                        <%--<button class="sub-soft-btn" data-bs-toggle="tooltip" title="Search"><i class="ri-search-line"></i></button>--%>
                        <a href="javascript:void(0);" ng-click="changeView('table')" class="sub-soft-btn active" data-bs-toggle="tooltip" title="List View"  ng-class="{'active':view=='table'}"><i class="ri-list-check"></i></a>
                        <!-- Keep Gantt as its own permanent view option. Only the
                             columns inside the Gantt view use the Timesheet layout. -->
                        <a id="btnGanttView" href="javascript:void(0);" ng-click="changeView('gantt', $event)" class="sub-soft-btn" style="display:inline-flex!important" data-bs-toggle="tooltip" title="Gantt View" aria-label="Gantt View" ng-class="{'active':view=='gantt'}"><i class="ri-bar-chart-horizontal-line"></i></a>
                        <a href="javascript:void(0);" ng-click="changeView('kanban', $event)" class="sub-soft-btn" data-bs-toggle="tooltip" title="Kanban View" ng-class="{'active':view=='kanban'}"><i class="las la-chart-bar2"></i></a>
                        <a href="javascript:void(0);" ng-click="changeView('timesheet')" class="sub-soft-btn" data-bs-toggle="tooltip" title="Timesheet"  ng-class="{'active':view=='timesheet'}"><i class="bx bx-server"></i></a>
                        
                        <a href="javascript:void(0);" ng-click="changeView('calendar')" class="sub-soft-btn" data-bs-toggle="tooltip" aria-label="Calendar" ng-class="{'active':view=='calendar'}" data-bs-original-title="Calendar"><i class="bx bx-calendar"></i></a>
                        <button class="sub-soft-btn" data-bs-toggle="tooltip" title="Filter"><i class="ri-filter-line"></i></button>
                        <div class="btn-group d-block">
                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="sub-soft-btn"><i class="ri-more-fill"></i></a>
                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                <ul class="sttus-list">
                                    <li><a class="" href="#"><i class="bx bx-download fs-16 align-middle me-1"></i> Download Report</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
                <div class="row">
                <div class="col-xxl-2 col-lg-2 col-md-2 col-sm-12"></div>
                <div class="col-xxl-8 col-lg-8 col-md-8 col-sm-12" style="margin-top:2px;margin-bottom:2px;">
                    <ul class="top-list-sales">
                       
                        <li>
                            <a href="javascript:void(0);" ng-click="applyTopStatusFilter('all')" ng-class="{'pms-top-filter-active': topStatusFilter == 'all'}">
                                <div class="tile">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <div class="tile-icon text-secondary"><i class="ri-user-add-line"></i></div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>Total</span></div>
                                            <div class="tile-body">{{topFilterCounts.total || 0}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                        <li ng-repeat="status in statuslists track by status.id">
                            <a href="javascript:void(0);"
                               ng-click="applyTopStatusFilter(status.id)"
                               ng-class="{'pms-top-filter-active': topStatusFilter == status.id}">
                                <div class="tile">
                                    <div class="d-flex align-items-center gap-1">
                                        <div>
                                            <div class="tile-icon" ng-style="{'color': status.colorname}">
                                                <i class="ri-checkbox-blank-circle-fill"></i>
                                            </div>
                                        </div>
                                        <div>
                                            <div class="tile-heading"><span>{{status.name}}</span></div>
                                            <div class="tile-body">{{topFilterCounts.byStatus[status.id] || 0}}</div>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>
                    </ul>
                </div>
                <div class="col-xxl-2 col-lg-2 col-md-2 col-sm-12"></div>
            </div>
        </div>
       
        <div class="wrapper_inner p-0" ng-show="view=='table'">
            <div class="table-responsive-new">
                <table id="projectHierarchyTable" class="table table-hover table-striped tbl_new">
                    <thead>
                        <tr>
                            <th class="color_border"><input class="form-check-input" type="checkbox"></th>
                            <th style="min-width:350px;">Milestone</th>
                            <th style="min-width:120px;">Status</th>
                            <th>Total Completion</th>
                            <th>Task Situation</th>
                            <th>Priority</th>
                            <th style="min-width:120px;">Assign to</th>
                            <th>Duration Days</th>
                            <th>Task Description</th>
                            <th>Start Date</th>
                            <th>HH:MM</th>
                            <th>Timeline</th>
                            <th>Owner</th>
                            <th style="min-width:120px;">Team</th>
                            <th>Weightage</th>
                            <th>Issues</th>
                            <th>Files</th>
                        </tr>
                    </thead>
                    <tbody>
                        <%--<tr class="parent" data-id="milestone{{c.id}}" ng-repeat-start="c in projectlist.miles | filter:searchText | filter:filterMilestoneAssignView track by $index">--%>
                        <tr ng-repeat-start="c in projectlist.miles | filter:searchText | filter:filterMilestoneAssignView track by $index" ng-init="c.isExpanded = c.isExpanded || false"  class="parent-row pms-row-drop" data-pms-row="milestone" data-pms-id="{{c.id}}" data-pms-parent-id="{{projectlist.id}}" data-pms-index="{{}}">
                            <td class="color_border" style="border-left-color:#ffc300 !important;">
                                <input class="form-check-input" type="checkbox">
                            </td>
                            <td>
                                <div class="mile_st">
                                    <div style="width:100%;padding-right:35px;">
                                        <span class="toggle"
      ng-if="c.tasks && c.tasks.length > 0"
      ng-class="{ 'rotate': c.isExpanded }"
      ng-click="toggleMilestone(c)">

    <i class="ri-arrow-down-s-line"
       style="font-size:18px;font-weight:400;">
    </i>
</span>
                                       <%-- <span class="toggle"
      
    ng-if="c.tasks.length>0"  ng-class="{c.isExpanded?'rotate' : 'toggle'}"
      ng-click="toggleMilestone(c)">
    <i class="ri-arrow-down-s-line" style="font-size:18px;font-weight:400;"></i>
</span>--%>
                                        <b>{{$index + 1 }}. </b><input type="text"  ng-model="c.Milestone" ng-model-options="{ updateOn: 'blur' }" ng-blur="saveListNameOnBlur($event, c, 'Milestone', 'milestone')" ng-readonly="!canEdit('milestone')" ng-disabled="!canEdit('milestone')" class="pmsinput w-100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.Milestone}}" ></div>
                                    <div class="d-flex">
                                        <div>
                                            
                                            <div class="btn-group d-block">
                                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-add-line"></i></a>
                                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                                <ul class='sttus-list'>

                                                     <%if (ischeckCreate == 1)
                                          {%>
                        <li><a class='' href='javascript:void(0);' ng-click="addRowUpDown_mile($index,c,'up')"><i class="ri-add-line me-1" tooltip="Add Above"></i>Add Above</a></li>
                                                    <li><a class='' href='javascript:void(0);' ng-click="addRowUpDown_mile($index,c,'down')"><i class="ri-add-line me-1" tooltip="Add Below"></i>Add Bellow</a></li>

                       <%} %>
                                                    
<%if (ischeckCreate_task == 1)
                                          {%>
                        <li><a class='' href='javascript:void(0);' ng-if="c.istask==1" ng-click="addRowUpDown_taskdefault(c)"><i class="ri-add-line me-1" tooltip="Add Task"></i>Add Task</a></li>

                       <%} %>
                                                    
                                                  
                                                </ul>
                                            </div>
                                        </div>
                                        </div>
                                       
                                        <div class="btn-group d-block">
                                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-more-2-fill"></i></a>
                                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                                <ul class='sttus-list'>
                                                    <li><a class='' href='javascript:void(0);' ng-click="openListCompletionDependency(c,'milestone',$event)"><i class="ri-parent-line fs-16 align-middle me-1"></i> Dependency</a></li>
                                                    <li><a class='' href='javascript:void(0);' ng-click="openIssueList(c,'milestone',c.Milestone,$event)"><i class="bx bx-error-circle fs-16 align-middle me-1"></i> Add Issue</a></li>
                                                   <%if (ischeckEdit == 1)
                                          {%>
                                                    <li><a class='' href="javascript:void(0);" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadSrcFiles(c,'milestone',$index)" ><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Documents</a></li>
                                                    <li><a class='' href='javascript:void(0);' data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="loadformedit(c,'milestone','editdetails','/pms/editmile.aspx?id='+c.id+'&pid='+c.ProjectId+'&ctype=ang','Milestone')"><i class="bx bx-edit fs-16 align-middle me-1"></i> Edit Milestone</a></li>
                                                    <%} %>
                                                    
                                                    <%if (ischeckCreate == 1)
                                          {%>
                                                    <li><a class='' href='javascript:void(0);' ng-click="addRowUpDown_mile($index,c,'down')"><i class="bx bx-copy fs-16 align-middle me-1"></i> Clone</a></li>
                                                     <%} %>

                                                    <%--<li><a class='' href='javascript:void(0);' id="text"><i class="bx bx-palette fs-16 align-middle me-1"></i> Change Color</a></li>--%>
                                                    <%--<li><a class='' href='#'><i class="bx bx-archive fs-16 align-middle me-1"></i> Hold</a></li>--%>
                                                     <%if (ischeckDelete == 1)
                                          {%>
                                                    <li><a class='text-danger' href='javascript:void(0);'  ng-click="deletedatamile($index,c)"><i class="bx bx-trash fs-16 align-middle me-1"></i> Delete</a></li>
                                               <%} %>
                                                    </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td ng-style="{'background-color': c.status_color}">
                              <div style="position:relative;text-align:center">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='d-flex align-items-center justify-content-center w-100 h-100' ng-class="{'pms-no-edit': !canEdit('milestone')}" style="font-size:12px;font-weight:600;color:black;" > {{ c.status_name }}</a>
    
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="closeListActionDropdown($event); selectStatus(c,newstatus,'milestone'); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='ri-checkbox-blank-fill' ng-style="{'color':newstatus.colorname}" ></i> {{newstatus.name }}
                    </a>
                    </div>
    </li>
</ul>
</div>
    </div> 
                              <%--<div style="position:relative;text-align:center">     
                                <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='' ng-class="{'pms-no-edit': !canEdit('milestone')}" style="font-size:12px;font-weight:600;color:black;" > {{ c.status_name }}</a>
                                

                                    <div class="dropdown-menu p-0" style="border-radius:0;">
                                        <ul class='sttus-list'>                                           
                                           <li ng-repeat="newstatus in statuslists|filter:name" ng-click="selectStatus(c,newstatus); $event.preventDefault()" >
                 <div class='dropdown-item p-0'>
                   
                     <a href="javascript:void(0);"  ng-click="selectStatus(c,newstatus,'milestone'); $event.preventDefault()">

    
                         <i class='ri-checkbox-blank-fill' ng-style="{'color':newstatus.colorname}" ></i> {{newstatus.name }}
</a>
                    </div>
    </li>
                                        </ul>
                                    </div>
                                



                                </div>--%>
                                
                                
                                </td>
                            <td>
                                <div>
                                    <div class="d-block mb-0">
                                        <div style="font-size:11px !important;color:#777;">{{ c.complete_value }}%</div>
                                    </div>
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar bg-success" role="progressbar" style="width:{{ c.complete_value }}%;"></div>
                                    </div>
                                </div>
                            </td>
                            <td ng-style="{'background-color': c.CriticalLabelcolor}">
                              <div style="text-align:center">{{c.CriticalLabel }}</div>
                            </td>
                            
                                <td ng-style="{'background-color': c.priority_color}">
                                <%--<div class="badge bg-danger-subtle text-danger priority">High</div>--%>
                                <div style="position:relative;text-align:center">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='d-flex align-items-center justify-content-center w-100 h-100' ng-class="{'pms-no-edit': !canEdit('milestone')}" style="font-size:12px;font-weight:600;color:black;" > {{ c.priority_name }}</a>
    <%--<a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;" > {{ c.priority_name }}</a>--%>
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name" ng-click="selectPriority(c,newpriority,'milestone'); $event.preventDefault()" >
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

                            
                            <td>
                                <div class="mile_st">

                                    <div class="avatar-group" ng-if="c.assignto!=0">
                                        <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="{{getOwnerName(c.assignto)}}">
                                            <span class="pms-person-avatar" pms-person-avatar person-name="{{getOwnerName(c.assignto)}}" person-photo="{{getOwnerProfilenew(c.assignto)}}"></span>
                                        </a>                                        
                                    </div>

                                    <div><a href="javascript:void(0);" ng-if="canAssign('milestone')"  class="click-link" data-bs-toggle="modal" data-bs-target="#assignedtask" ng-click="openAssignTask(c,'milestone',$index)"><i class="ri-add-line"></i></a></div>
                                </div></td>
                            <td><input type="text" class="pmsinput" ng-model="c.txtdays" ng-readonly="!canEdit('milestone')" ng-disabled="!canEdit('milestone')" 
     ng-focus="c._oldtxtdays = c.txtdays"
   
     ng-blur="update_duration(c,'milestone','duration')"
    style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)"></td>
                            <td><input type="text" class="pmsinput" ng-model="c.miledescription" ng-readonly="!canEdit('milestone')" ng-disabled="!canEdit('milestone')" ng-blur="!c.isDescPopupOpen && updatedetails_mile(c,'miledescription')" ng-dblclick="c.isDescPopupOpen=true; openDescriptionPopup($event,c,'miledescription','milestone')"
           
           style="width:100%;background-color:{{projectlist.editcolor}}" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.miledescription}}"></td>
                            <td class="position-relative">
                                <div class="d-flex gap-2">
                                <div class="hierarchy-startdate-click"
                                     ng-class="{'is-readonly': !canEdit('milestone')}"
                                     ng-click="openHierarchyStartDateCalendar(c,'milestone',projectlist,$index,$event)"
                                     title="Click to change milestone start date">
                                    <span>{{formatDateOnly(c.StartDate) }}</span>
                                    <input type="date"
                                           class="hierarchy-startdate-picker"
                                           tabindex="-1"
                                           aria-hidden="true"
                                           hierarchy-date-select="applyHierarchyStartDate(c,'milestone',projectlist,$index,$selectedDateValue)">
                                </div>  
                                <span style="">
                                    <i class="bx bx-refresh text-primary fs-18"
                                       style="cursor:pointer;"
                                       data-bs-toggle="tooltip"
                                       data-bs-placement="top"
                                       data-bs-trigger="hover"
                                       title="Reset Start Date"
                                       ng-show="canEdit('milestone') && !c._resettingStartDate"
                                       ng-click="resetStartDateHierarchy_mile(c,projectlist,$index,$event)">
                                    </i>
                                    <span class="spinner-border spinner-border-sm text-primary"
                                          role="status"
                                          aria-hidden="true"
                                          title="Resetting Start Date..."
                                          ng-if="canEdit('milestone') && c._resettingStartDate"></span>
                                </span>
                                </div>
                            </td>
                            <td>
                                <div class="d-flex align-items-center gap-1">
                                    <%--<i class="bx bx-stopwatch" style="font-size:18px;margin-top: 2px;"></i>--%>
                                    <div class="duration-box">
    <input type="text" class="duration-input left" ng-model="c.TaskDuration"  ng-focus="storeOldDuration(c)" ng-readonly="!canEdit('milestone')" ng-disabled="!canEdit('milestone')"
           ng-blur="update_hrs(c,'milestone')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

    <div class="sls_2">:</div>

    <input type="text"
           class="duration-input right"          
           maxlength="2"
        ng-model="c.TaskDuration_min"  ng-focus="storeOldDuration(c)" ng-readonly="!canEdit('milestone')" ng-disabled="!canEdit('milestone')"
           ng-blur="validateTime(c,$index,'milestone');update_hrs(c,'milestone')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>
        </div>
                                </div>
                            </td>
                            <td>
                                <div class="pms-table-timeline"
                                     ng-mouseenter="openHierarchyTimelineHover($event,c,'milestone',null,null)"
                                     ng-mouseleave="scheduleHierarchyTimelineHide()"
                                     ng-attr-data-timeline-revision="{{c._timelineRevision || 0}}">
                                    <div class="pms-kanban-progress-track">
                                        <div class="pms-kanban-progress-fill timeline"
                                             ng-style="{'width': getKanbanTimelinePercent(c) + '%'}"></div>
                                    </div>
                                   <%-- <a href="javascript:void(0);"
       type="button"             
       data-bs-toggle="modal"
       data-bs-target="#extendedhistory"
       ng-click="Bindextenddate_history(c,'milestone',$index)">
       <i class="ri-eye-line" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" aria-label="View" data-bs-original-title="History"></i>
       </a>--%>
                                    <span class="pms-table-timeline-value">{{getKanbanTimelinePercent(c)}}%</span>
                                </div>
                            </td>
                            <td style="text-align: center;">
                                <div class="dropdown w-100">

  <div class="dropdown"
         
          data-bs-toggle="dropdown" 
        ng-click="focusOwnerSearch($event, c)" ng-class="{'pms-no-edit': !canEdit('milestone')}" style="cursor:pointer;">
      <div class="pms-owner-avatar-trigger" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)" data-bs-title="{{getOwnerName(c.Owner) || 'Select Owner'}}" aria-label="{{getOwnerName(c.Owner) || 'Select Owner'}}">
       <span class="pms-person-avatar" pms-person-avatar person-name="{{getOwnerName(c.Owner)}}" person-photo="{{getOwnerProfilenew(c.Owner)}}"></span>
      </div>
  </div>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2 search__input owner-search"
           placeholder="Search..."
           pms-owner-keys ng-model="c.ownerSearch" ng-disabled="!canEdit('milestone')">

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: c.ownerSearch"
         ng-class="{'active': c.Owner == o.id}"
         ng-click="selectOwner(c,o,'milestone')">
        {{o.name}}
      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="(owners | filter: c.ownerSearch).length == 0">
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
<a href="javascript: void(0);"  ng-repeat="newteam in c.teams|limitTo:3" class="avatar-group-item material-shadow owner-wrp" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary" >
        {{newteam.shortname}}
      </div>
    </div>   
    <i class="ri-close-line owner-del" ng-click="removeteams(c,newteam,'milestone')" ng-if="canEdit('project')"></i>

</a>
   <div ng-if="c.teams && c.teams.length > 3"
     class="avatar-group-item owner-wrp"     bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingOwners(c.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle bg-primary">
        +{{ c.teams.length - 3 }}
    </div>
    </div>
</div>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center" ng-if="canEdit('milestone')"><a href="javascript:void(0);"
     ng-click="loadSrcOwnersnew(c,'team','milestone',$index)" 
     data-bs-toggle="modal" data-bs-target="#addowners">
     <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
                                <%--<div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c.teams|limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-original-title="{{newteam.name}}" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary">
        {{newteam.shortname}}
      </div>
    </div>

    </div>
    <i class="ri-close-line owner-del" ng-if="canEdit('milestone')" 
      ng-click="removeteams(c,newteam,'milestone')">
    </i>

</a>
    <div ng-if="c.teams && c.teams.length > 3"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-trigger="hover"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle">
        +{{ c.teams.length - 3 }}
    </div>
    </div>
</div>
 
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center" ng-if="canEdit('milestone')"><a href="javascript:void(0);"
     ng-click="loadSrcOwnersnew(c,'team','milestone',$index)" 
     data-bs-toggle="modal" data-bs-target="#addowners">
     <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>--%>
                                
                            </td>
                            <td><input type="text" class="pmsinput" ng-readonly="!canEdit('milestone')" ng-disabled="!canEdit('milestone')"
           ng-model="c.Weightage"
        ng-blur="updatedetails_mile(c, 'Weightage')"
          
           style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)"></td>
                            <td><div class="d-flex">
       <div class="flex-grow-1 align-self-center">
       <div class="progress progress-xl cursor-pointer">
           <div class="progress-bar bg-success" role="progressbar" style="width:{{c.completeissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.completeissue_rate}}% [{{c.completeissue}} of {{c.totalissue}} Issues Completed]">{{c.completeissue}}</div>
           <div class="progress-bar bg-warning" role="progressbar" style="width:{{c.pendingissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c.pendingissue_rate}}% [{{c.pendingissue}} of {{c.totalissue}} Issues Not Completed]">{{c.pendingissue}}</div>

       </div>
       </div>
       
   </div></td>
                            <td>
                                <a href="javascript:void(0);" style="font-size:22px;" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadSrcFiles(c,'milestone',$index)" class="position-relative"><span class="btn-plus2" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="Upload Documents" ><i class="ri-file-text-line"></i></span><span class="files_count">{{ c.viewfiles.length || 0 }}</span></a>
                            </td>
                        </tr>
                        <%--<tr class="child-of-milestone{{c.id}} parent" data-id="task{{c1.id}}" ng-repeat-start="c1 in c.tasks | filter:filterTaskAssignView track by $index">--%>
                        <tr ng-repeat-start="c1 in c.tasks | filter:filterTaskAssignView track by $index" class="child-row pms-row-drop" ng-if="c.isExpanded" data-pms-row="task" data-pms-id="{{c1.id}}" data-pms-parent-id="{{c.id}}" data-pms-index="{{}}">
                            <td class="color_border" style="border-left-color:#ffc300 !important;">
                                <input class="form-check-input" type="checkbox">
                            </td>
                            <td class="level-1">
                                <div class="mile_st">
                                    <div style="width:100%;padding-right:35px;">
                                           <span class="toggle"
          ng-if="c1.subtasks && c1.subtasks.length > 0"
          ng-class="c1.isExpanded ? 'rotate' : 'toggle'"
          ng-click="toggleTask(c1)">
                                               <i class="ri-arrow-down-s-line" style="font-size: 18px;font-weight: 400;"></i>
    </span>
                                        
                                        
                                        <b>{{$parent.$parent.$index+1}}.{{ $index + 1 }} </b><input type="text"
         ng-model="c1.taskname" ng-model-options="{ updateOn: 'blur' }"
        
        ng-blur="saveListNameOnBlur($event, c1, 'taskname', 'task')" ng-readonly="!canEdit('task')" ng-disabled="!canEdit('task')" class="pmsinput w-100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.taskname}}" ></div>
                                    <div class="d-flex">
                                        <div>
                                            <div class="btn-group d-block">
                                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-add-line"></i></a>
                                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                                <ul class='sttus-list'>
                                                     <%if (ischeckCreate_task == 1)
                                          {%>
                     <li><a class='' href='javascript:void(0);'  ng-click="addRowUpDown_task($index,c,c1,'up')"><i class="ri-add-line me-1" tooltip="Add Above"></i>Add Above</a></li>
                                                    <li><a class='' href='javascript:void(0);' ng-click="addRowUpDown_task($index,c,c1,'down')"><i class="ri-add-line me-1" tooltip="Add Below"></i>Add Bellow</a></li>
                       <%} %>
                                                          <%if (ischeckCreate_stask == 1)
                                          {%>
                      <li><a class='' href='javascript:void(0);' ng-if="c1.issubtask===1" ng-click="addRowUpDown_subtaskdefault(c1)"><i class="ri-add-line me-1" tooltip="Add Task"></i>Add Sub Task</a></li>
                       <%} %>
                                                    
                                                   
                                                  
                                                </ul>
                                            </div>
                                        </div>

                                        </div>


                                        <div class="btn-group d-block">
                                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-more-2-fill"></i></a>
                                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                                <ul class='sttus-list'>
                                                    <li><a class='' href='javascript:void(0);' ng-click="openListCompletionDependency(c1,'task',$event)"><i class="ri-parent-line fs-16 align-middle me-1"></i> Dependency</a></li>
                                                    <li><a class='' href='javascript:void(0);' ng-click="openIssueList(c1,'task',c1.TaskName || c1.taskname,$event)"><i class="bx bx-error-circle fs-16 align-middle me-1"></i> Add Issue</a></li>
                                                     <%if (ischeckEdit_task == 1)
                                          {%>
                                                    <li><a class='' href="javascript:void(0);" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadSrcFiles(c1,'task',$index)" ><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Documents</a></li>                                                   
                                                    <li><a class='' href='javascript:void(0);' ng-click="loadformedit(c1,'task','editdetails','/pms/edittask.aspx?id='+c1.id+'&mid='+c1.Milestoneid+'&ctype=ang','Task')" data-bs-toggle="offcanvas" data-bs-target="#addmile"><i class="bx bx-edit fs-16 align-middle me-1"></i> Edit Task</a></li>
                                                   <%} %>
                                                    <%if (ischeckCreate_task == 1)
                                          {%>
                                                    <li><a class='' href='javascript:void(0);' ng-click="addRowUpDown_task($index,c,c1,'down')"><i class="bx bx-copy fs-16 align-middle me-1"></i> Clone</a></li>
                                                     <%} %>
                                                    <%--<li><a class='' href='javascript:void(0);' id="text"><i class="bx bx-palette fs-16 align-middle me-1"></i> Change Color</a></li>--%>
                                                    <%--<li><a class='' href='#'><i class="bx bx-archive fs-16 align-middle me-1"></i> Hold</a></li>--%>
                                                    <%if (ischeckDelete_task == 1)
                                          {%>
                                                    <li><a class='text-danger' href='javascript:void(0);' ng-click="deletedatatask($index,c1,c)"><i class="bx bx-trash fs-16 align-middle me-1"></i> Delete</a></li>
                                                <%} %>
                                                    </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td ng-style="{'background-color': c1.status_color}">
                              
                             <div style="position:relative;text-align:center">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='d-flex align-items-center justify-content-center w-100 h-100' ng-class="{'pms-no-edit': !canEdit('task')}" style="font-size:12px;font-weight:600;color:black;" > {{ c1.status_name }}</a>
    
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="closeListActionDropdown($event); selectStatus(c1,newstatus,'task'); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='ri-checkbox-blank-fill' ng-style="{'color':newstatus.colorname}" ></i> {{newstatus.name }}
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
                                        <div style="font-size:11px !important;color:#777;">{{ c1.complete_value }}%</div>
                                    </div>
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar bg-success" role="progressbar" style="width:{{ c1.complete_value }}%;"></div>
                                    </div>
                                </div>
                            </td>
                            <td ng-style="{'background-color': c1.CriticalLabelcolor}"><div style="text-align:center">{{c1.CriticalLabel }}</div></td>
                           
                                <td ng-style="{'background-color': c1.priority_color}">
                                <%--<div class="badge bg-danger-subtle text-danger priority">High</div>--%>
                                <div style="position:relative;text-align:center">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='d-flex align-items-center justify-content-center w-100 h-100' ng-class="{'pms-no-edit': !canEdit('task')}" style="font-size:12px;font-weight:600;color:black;" > {{ c1.priority_name }}</a>
    
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name" ng-click="selectPriority(c1,newpriority,'task'); $event.preventDefault()" >
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

                            
                            <td>
                                <div class="mile_st">
                                    <div class="avatar-group" ng-if="c1.assignto!=0">
                                        <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="{{getOwnerName(c1.assignto)}}">
                                            <span class="pms-person-avatar" pms-person-avatar person-name="{{getOwnerName(c1.assignto)}}" person-photo="{{getOwnerProfilenew(c1.assignto)}}"></span>
                                        </a>                                        
                                    </div>
                                    <div><a href="javascript:void(0);" class="click-link"  ng-if="canAssign('task')" data-bs-toggle="modal" data-bs-target="#assignedtask" ng-click="openAssignTask(c1,'task',$index)"><i class="ri-add-line"></i></a></div>
                                </div>
                            </td>
                            <td><input type="text" class="pmsinput" ng-model="c1.txtdays" ng-readonly="!canEdit('task')" ng-disabled="!canEdit('task')" 
     ng-focus="c1._oldtxtdays = c1.txtdays"
   
     ng-blur="update_duration_task(c1,c,'duration')"
    style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)"></td>
                            <td class="position-relative"><input type="text" class="pmsinput" ng-model="c1.taskdescripton" ng-readonly="!canEdit('task')" ng-disabled="!canEdit('task')" ng-blur="updatedetails_task(c1, 'taskdescripton')"
           
           style="width:100%;background-color:{{projectlist.editcolor}}" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.taskdescripton}}"></td>
                            <td>
                                <div class="d-flex gap-2">
                                <div class="hierarchy-startdate-click"
                                     ng-class="{'is-readonly': !canEdit('task')}"
                                     ng-click="openHierarchyStartDateCalendar(c1,'task',c,$index,$event)"
                                     title="Click to change task start date">
                                    <span>{{formatDateOnly(c1.Startdate) }}</span>
                                    <input type="date"
                                           class="hierarchy-startdate-picker"
                                           tabindex="-1"
                                           aria-hidden="true"
                                           hierarchy-date-select="applyHierarchyStartDate(c1,'task',c,$index,$selectedDateValue)">
                                </div>  
    <i class="bx bx-refresh ms-2 text-primary fs-18"
       style="cursor:pointer;"
       data-bs-toggle="tooltip"
       data-bs-placement="top"
       data-bs-trigger="hover"
       title="Reset Start Date"
       ng-show="canEdit('task') && !c1._resettingStartDate"
       ng-click="resetStartDateHierarchy_task(c1,c,$index,$event)">
    </i>
    <span class="spinner-border spinner-border-sm text-primary ms-2"
          role="status"
          aria-hidden="true"
          title="Resetting Start Date..."
          ng-if="canEdit('task') && c1._resettingStartDate"></span>
</div></td>
                            <td>
                                <div class="d-flex align-items-center gap-1">
                                    <%--<i class="bx bx-stopwatch" style="font-size:18px;margin-top: 2px;"></i>--%><div class="duration-box">
    <input type="text" class="duration-input left" ng-model="c1.TaskDuration"  ng-focus="storeOldDuration(c1)" ng-readonly="!canEdit('task')" ng-disabled="!canEdit('task')"
           ng-blur="update_hrs(c1,'task')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

    <div class="sls_2">:</div>

    <input type="text"
           class="duration-input right"          
           maxlength="2"
        ng-model="c1.TaskDuration_min"  ng-focus="storeOldDuration(c1)" ng-readonly="!canEdit('task')" ng-disabled="!canEdit('task')"
           ng-blur="validateTime(c1,$index,'task');update_hrs(c1,'task')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>
        </div>
                                </div>
                            </td>
                            <td>
                                <div class="pms-table-timeline"
                                     ng-mouseenter="openHierarchyTimelineHover($event,c1,'task',c,null)"
                                     ng-mouseleave="scheduleHierarchyTimelineHide()"
                                     ng-attr-data-timeline-revision="{{c1._timelineRevision || 0}}">
                                    <div class="pms-kanban-progress-track">
                                        <div class="pms-kanban-progress-fill timeline"
                                             ng-style="{'width': getKanbanTimelinePercent(c1) + '%'}"></div>
                                    </div>
                                    <span class="pms-table-timeline-value">{{getKanbanTimelinePercent(c1)}}%</span>
                                </div>
                            </td>
                            <td style="text-align: center;">
                              <div class="dropdown w-100">

  <div class="dropdown"
         
          data-bs-toggle="dropdown" 
        ng-click="focusOwnerSearch($event, c1)" ng-class="{'pms-no-edit': !canEdit('task')}" style="cursor:pointer;">
      <div class="pms-owner-avatar-trigger" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)" data-bs-title="{{getOwnerName(c1.owner) || 'Select Owner'}}" aria-label="{{getOwnerName(c1.owner) || 'Select Owner'}}">
       <span class="pms-person-avatar" pms-person-avatar person-name="{{getOwnerName(c1.owner)}}" person-photo="{{getOwnerProfilenew(c1.owner)}}"></span>
      </div>
  </div>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2 search__input owner-search"
           placeholder="Search..."
           pms-owner-keys ng-model="c1.ownerSearch" ng-disabled="!canEdit('task')">

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: c1.ownerSearch"
         ng-class="{'active': c1.owner == o.id}"
         ng-click="selectOwner(c1, o,'task')">
        {{o.name}}
      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="(owners | filter: c1.ownerSearch).length == 0">
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
<a href="javascript: void(0);"  ng-repeat="newteam in c1.teams|limitTo:3" class="avatar-group-item material-shadow owner-wrp" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary" >
        {{newteam.shortname}}
      </div>
    </div>   
    <i class="ri-close-line owner-del" ng-click="removeteams(c1,newteam,'task')" ng-if="canEdit('task')"></i>

</a>
   <div ng-if="c1.teams && c1.teams.length > 3"
     class="avatar-group-item owner-wrp"     bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingOwners(c1.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle bg-primary">
        +{{ c1.teams.length - 3 }}
    </div>
    </div>
</div>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center" ng-if="canEdit('task')"><a href="javascript:void(0);"
     ng-click="loadSrcOwnersnew(c1,'team','task',$index)" 
     data-bs-toggle="modal" data-bs-target="#addowners">
     <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
                                <%--<div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c1.teams|limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-original-title="{{newteam.name}}" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary">
        {{newteam.shortname}}
      </div>
    </div>

    </div>
   
     <i class="ri-close-line owner-del" ng-if="canEdit('task')" 
      ng-click="removeteams(c1,newteam,'task')">
    </i>

</a>
    <div ng-if="c1.teams && c1.teams.length > 3"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c1.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle">
        +{{ c1.teams.length - 3 }}
    </div>
    </div>
</div>
 
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center" ng-if="canEdit('task')"><a href="javascript:void(0);"
     ng-click="loadSrcOwnersnew(c1,'team','task',$index)" 
     data-bs-toggle="modal" data-bs-target="#addowners">
     <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>--%>
                                
                            </td>
                            <td> <input type="text" class="pmsinput" ng-readonly="!canEdit('task')" ng-disabled="!canEdit('task')"
           ng-model="c1.Weightage"
        ng-blur="updatedetails_task(c1, 'Weightage')"
          
           style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)"></td>
                            <td><div class="d-flex">
       <div class="flex-grow-1 align-self-center">
       <div class="progress progress-xl cursor-pointer">
           <div class="progress-bar bg-success" role="progressbar" style="width:{{c1.completeissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.completeissue_rate}}% [{{c1.completeissue}} of {{c1.totalissue}} Issues Completed]">{{c1.completeissue}}</div>
           <div class="progress-bar bg-warning" role="progressbar" style="width:{{c1.pendingissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c1.pendingissue_rate}}% [{{c1.pendingissue}} of {{c1.totalissue}} Issues Not Completed]">{{c1.pendingissue}}</div>

       </div>
       </div>
      
   </div></td>
                            <td>
                                <a href="javascript:void(0);" style="font-size:22px;" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadSrcFiles(c1,'task',$index)" class="position-relative"><span class="btn-plus2" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="Upload Documents" ><i class="ri-file-text-line"></i></span><span class="files_count">{{ c1.viewfiles.length || 0 }}</span></a>
                            </td>
                        </tr>
                       <%-- <tr class="child-of-task{{c1.id}}" ng-repeat-start="c2 in c1.subtasks | filter:filterSubtaskAssignView track by $index">--%>
                        <tr ng-repeat="c2 in c1.subtasks | filter:filterSubtaskAssignView track by $index" class="child-row pms-row-drop" ng-if="c1.isExpanded" data-pms-row="subtask" data-pms-id="{{c2.id}}" data-pms-parent-id="{{c1.id}}" data-pms-index="{{}}">
                            <td class="color_border" style="border-left-color:#ffc300 !important;">
                                <input class="form-check-input" type="checkbox">
                            </td>
                            <td class="level-2">
                                <div class="mile_st">
                                    <div style="width:100%;padding-right:25px;"><b>{{$parent.$parent.$parent.$index+1}}.{{$parent.$parent.$index+1}}.{{ $index + 1 }} </b><input type="text"
         ng-model="c2.Subtaskname" ng-model-options="{ updateOn: 'blur' }"
        
        ng-blur="saveListNameOnBlur($event, c2, 'Subtaskname', 'subtask')" ng-readonly="!canEdit('subtask')" ng-disabled="!canEdit('subtask')" class="pmsinput w-100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.Subtaskname}}" ></div>
                                    <div class="d-flex">
                                        <div>
                                            <div class="btn-group d-block">
                                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-add-line"></i></a>
                                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                                <ul class='sttus-list'>

                                                    <%if (ischeckCreate_stask == 1)
                                          {%>
                     <li><a class='' href='javascript:void(0);'  ng-click="addRowUpDown_subtask($index,c1,c2,'up')"><i class="ri-add-line me-1" tooltip="Add Above"></i>Add Above</a></li>
                                                    <li><a class='' href='javascript:void(0);'  ng-click="addRowUpDown_subtask($index,c1,c2,'down')"><i class="ri-add-line me-1" tooltip="Add Below"></i>Add Bellow</a></li>
                       <%} %>
                                                    
                                                    
                                                  
                                                </ul>
                                            </div>
                                        </div>

                                        </div>
                                        <div class="btn-group d-block">
                                            <a href="javascript:void(0);" type="button" data-bs-toggle="dropdown" data-bs-auto-close="true" data-bs-boundary="window" class="click-link"><i class="ri-more-2-fill"></i></a>
                                            <div class="dropdown-menu p-0" style="border-radius:0;">
                                                <ul class='sttus-list'>
                                                    <li><a class='' href='javascript:void(0);' ng-click="openListCompletionDependency(c2,'subtask',$event)"><i class="ri-parent-line fs-16 align-middle me-1"></i> Dependency</a></li>
                                                    <li><a class='' href='javascript:void(0);' ng-click="openIssueList(c2,'subtask',c2.Subtaskname,$event)"><i class="bx bx-error-circle fs-16 align-middle me-1"></i> Add Issue</a></li>
                                                    <%if (ischeckEdit_stask == 1)
                                          {%>
                                                    <li><a class='' href="javascript:void(0);" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadSrcFiles(c2,'subtask',$index)" ><i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Documents</a></li>
                                                    <li><a class='' href='javascript:void(0);' ng-click="loadformedit(c2,'subtask','editdetails','/pms/addnewsubtask.aspx?id='+c2.id+'&tid='+c2.Task+'&ctype=ang','Sub Task')" data-bs-toggle="offcanvas" data-bs-target="#addmile"><i class="bx bx-edit fs-16 align-middle me-1"></i> Edit Sub Task</a></li>
                                                     <%} %>
                                                    <%if (ischeckCreate_stask == 1)
                                          {%>
                                                    <li><a class='' href='javascript:void(0);' ng-click="addRowUpDown_subtask($index,c1,c2,'down')"s><i class="bx bx-copy fs-16 align-middle me-1"></i> Clone</a></li>
                                                     <%} %>
                                                   <%-- <li><a class='' href='javascript:void(0);' id="text"><i class="bx bx-palette fs-16 align-middle me-1"></i> Change Color</a></li>--%>
                                                   
                                                    <%--<li><a class='' href='#'><i class="bx bx-archive fs-16 align-middle me-1"></i> Hold</a></li>--%>
                                                    
                                               <%if (ischeckDelete_stask == 1)
                                          {%>
                                                    <li><a class='text-danger' href='javascript:void(0);'  ng-click="deletedatasubtask($index,c2,c1)"><i class="bx bx-trash fs-16 align-middle me-1"></i> Delete</a></li>
                                                <%} %>
                                                    
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td ng-style="{'background-color': c2.status_color}">
                              <div style="position:relative;text-align:center">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='d-flex align-items-center justify-content-center w-100 h-100' ng-class="{'pms-no-edit': !canEdit('subtask')}" style="font-size:12px;font-weight:600;color:black;" > {{ c2.status_name }}</a>
    
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newstatus in statuslists|filter:name" ng-click="closeListActionDropdown($event); selectStatus(c2,newstatus,'subtask'); $event.preventDefault()" >
         <div class='dropdown-item p-0'>
                    <a class='' href='javascript:void(0);'  >
                    <i class='ri-checkbox-blank-fill' ng-style="{'color':newstatus.colorname}" ></i> {{newstatus.name }}
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
                                        <div style="font-size:11px !important;color:#777;">{{ c2.complete_value }}%</div>
                                    </div>
                                    <div class="progress progress-md animated-progress">
                                        <div class="progress-bar bg-success" role="progressbar" style="width:{{ c2.complete_value }}%;"></div>
                                    </div>
                                </div>
                            </td>
                            <td ng-style="{'background-color': c2.CriticalLabelcolor}"><div style="text-align:center">{{c2.CriticalLabel }}</div></td>
                            
                                <td ng-style="{'background-color': c2.priority_color}">
                                <%--<div class="badge bg-danger-subtle text-danger priority">High</div>--%>
                                <div style="position:relative;text-align:center">
    <a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='d-flex align-items-center justify-content-center w-100 h-100' ng-class="{'pms-no-edit': !canEdit('subtask')}" style="font-size:12px;font-weight:600;color:black;" > {{ c2.priority_name }}</a>
    <%--<a href ='javascript:void(0);' type='button'  data-bs-toggle='dropdown' data-bs-auto-close='true' class='badge text-uppercase new_badge' style="font-size:10px;font-weight:500;" > {{ c.priority_name }}</a>--%>
   <div class='dropdown-menu lead_form_body p-0' style="display:">
<ul class='sttus-list' data-simplebar style='max-height: 160px'>
    <li ng-repeat="newpriority in prioritylists|filter:name" ng-click="selectPriority(c2,newpriority,'subtask'); $event.preventDefault()" >
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

                            
                            <td>
                                <div class="mile_st">
                                    <div class="avatar-group" ng-if="c2.assignto!=0">
                                        <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="{{getOwnerName(c2.assignto)}}">
                                            <span class="pms-person-avatar" pms-person-avatar person-name="{{getOwnerName(c2.assignto)}}" person-photo="{{getOwnerProfilenew(c2.assignto)}}"></span>
                                        </a>                                        
                                    </div>
                                    <div><a href="javascript:void(0);"   ng-if="canAssign('subtask')" class="click-link" data-bs-toggle="modal" data-bs-target="#assignedtask" ng-click="openAssignTask(c2,'subtask',$index)"><i class="ri-add-line"></i></a></div>
                                </div>
                            </td>
                            <td><input type="text" class="pmsinput" ng-model="c2.txtdays" ng-readonly="!canEdit('subtask')" ng-disabled="!canEdit('subtask')" 
     ng-focus="c2._oldtxtdays = c2.txtdays"
   
     ng-blur="update_duration_subtask(c2,c1,'duration')"
    style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)"></td>
                            <td>
                                <input type="text" class="pmsinput" ng-model="c2.SubTaskDescription" ng-readonly="!canEdit('subtask')" ng-disabled="!canEdit('subtask')" ng-blur="updatedetails_subtask(c2, 'SubTaskDescription')"
           
           style="width:100%;background-color:{{projectlist.editcolor}}" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.SubTaskDescription}}"></td>
                            <td class="position-relative">
                               <div class="d-flex gap-2">
                                <div class="hierarchy-startdate-click"
                                     ng-class="{'is-readonly': !canEdit('subtask')}"
                                     ng-click="openHierarchyStartDateCalendar(c2,'subtask',c1,$index,$event)"
                                     title="Click to change subtask start date">
                                    <span>{{formatDateOnly(c2.Startdate) }}</span>
                                    <input type="date"
                                           class="hierarchy-startdate-picker"
                                           tabindex="-1"
                                           aria-hidden="true"
                                           hierarchy-date-select="applyHierarchyStartDate(c2,'subtask',c1,$index,$selectedDateValue)">
                                </div>  
    <i class="bx bx-refresh ms-2 text-primary fs-18"
       style="cursor:pointer;"
       data-bs-toggle="tooltip"
       data-bs-placement="top"
       data-bs-trigger="hover"
       title="Reset Start Date"
       ng-show="canEdit('subtask') && !c2._resettingStartDate"
       ng-click="resetStartDateHierarchy_subtask(c2,c1,$index,$event)">
    </i>
    <span class="spinner-border spinner-border-sm text-primary ms-2"
          role="status"
          aria-hidden="true"
          title="Resetting Start Date..."
          ng-if="canEdit('subtask') && c2._resettingStartDate"></span>
</div></td>
                            <td>
                                <div class="d-flex align-items-center gap-1">
                                    <%--<i class="bx bx-stopwatch" style="font-size:18px;margin-top: 2px;"></i>--%><div class="duration-box">
    <input type="text" class="duration-input left" ng-model="c2.TaskDuration"  ng-focus="storeOldDuration(c2)" ng-readonly="!canEdit('subtask')" ng-disabled="!canEdit('subtask')"
           ng-blur="update_hrs(c2,'subtask')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>

    <div class="sls_2">:</div>

    <input type="text"
           class="duration-input right"          
           maxlength="2"
        ng-model="c2.TaskDuration_min"  ng-focus="storeOldDuration(c2)" ng-readonly="!canEdit('subtask')" ng-disabled="!canEdit('subtask')"
           ng-blur="validateTime(c2,$index,'subtask');update_hrs(c2,'subtask')"
           ng-keypress="isnumeric($event)"
           style="width:100%;background-color:{{projectlist.editcolor}}"/>
        </div>
                                </div>
                            </td>
                            <td>
                                <div class="pms-table-timeline"
                                     ng-mouseenter="openHierarchyTimelineHover($event,c2,'subtask',c1,c)"
                                     ng-mouseleave="scheduleHierarchyTimelineHide()"
                                     ng-attr-data-timeline-revision="{{c2._timelineRevision || 0}}">
                                    <div class="pms-kanban-progress-track">
                                        <div class="pms-kanban-progress-fill timeline"
                                             ng-style="{'width': getKanbanTimelinePercent(c2) + '%'}"></div>
                                    </div>
                                    <span class="pms-table-timeline-value">{{getKanbanTimelinePercent(c2)}}%</span>
                                </div>
                            </td>
                            <td style="text-align: center;">

                                <div class="dropdown w-100">

  <div class="dropdown"
         
          data-bs-toggle="dropdown" 
        ng-click="focusOwnerSearch($event, c2)" ng-class="{'pms-no-edit': !canEdit('subtask')}" style="cursor:pointer;">
      <div class="pms-owner-avatar-trigger" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)" data-bs-title="{{getOwnerName(c2.owner) || 'Select Owner'}}" aria-label="{{getOwnerName(c2.owner) || 'Select Owner'}}">
       <span class="pms-person-avatar" pms-person-avatar person-name="{{getOwnerName(c2.owner)}}" person-photo="{{getOwnerProfilenew(c2.owner)}}"></span>
      </div>
  </div>

  <div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control form-control-sm mb-2 search__input owner-search"
           placeholder="Search..."
           pms-owner-keys ng-model="c2.ownerSearch" ng-disabled="!canEdit('subtask')">

    <div style="max-height:200px; overflow:auto;">

      <a href="javascript:void(0);"
         class="dropdown-item"
         ng-repeat="o in owners | filter: c2.ownerSearch"
         ng-class="{'active': c2.owner == o.id}"
         ng-click="selectOwner(c2, o,'subtask')">
        {{o.name}}
      </a>

      <!-- NO RESULT -->
      <div class="text-muted p-2"
           ng-if="(owners | filter: c2.ownerSearch).length == 0">
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
<a href="javascript: void(0);"  ng-repeat="newteam in c2.teams|limitTo:3" class="avatar-group-item material-shadow owner-wrp" bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{newteam.name}}" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-title rounded-circle bg-primary" >
        {{newteam.shortname}}
      </div>
    </div>   
    <i class="ri-close-line owner-del" ng-click="removeteams(c2,newteam,'subtask')" ng-if="canEdit('subtask')"></i>

</a>
   <div ng-if="c2.teams && c2.teams.length > 3"
     class="avatar-group-item owner-wrp"     bs-tooltip data-bs-toggle="tooltip"  data-bs-placement="top" data-bs-title="{{ getRemainingOwners(c2.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle bg-primary">
        +{{ c2.teams.length - 3 }}
    </div>
    </div>
</div>
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center" ng-if="canEdit('subtask')"><a href="javascript:void(0);"
     ng-click="loadSrcOwnersnew(c2,'team','subtask',$index)" 
     data-bs-toggle="modal" data-bs-target="#addowners">
     <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>
                                <%--<div class="d-flex" style="width:100%;">
 <div class="flex-grow-1">
<div class="avatar-group">
<a href="javascript: void(0);"  ng-repeat="newteam in c2.teams|limitTo:3" class="avatar-group-item owner-wrp" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-original-title="{{newteam.name}}" data-bs-trigger="hover" ng-mouseleave="hideTeamAvatarTooltip($event)">
    <div class="avatar-xxs">
      <img ng-if="newteam.profileimage"
         ng-src="{{newteam.profileimage}}"
         class="rounded-circle avatar-xxs" />

    <!-- Initial -->
    <div ng-if="!newteam.profileimage" class="avatar-xxs">
      <div class="avatar-title rounded-circle bg-light text-primary">
        {{newteam.shortname}}
      </div>
    </div>

    </div>
    
     <i class="ri-close-line owner-del" ng-if="canEdit('subtask')" 
      ng-click="removeteams(c2,newteam,'subtask')">
    </i>

</a>
    <div ng-if="c2.teams && c2.teams.length > 3"
     class="avatar-group-item owner-wrp"
     data-bs-toggle="tooltip"
     data-bs-placement="top"
     data-bs-original-title="{{ getRemainingOwners(c2.teams) }}">
<div class="avatar-xxs">
    <div class="avatar-title rounded-circle">
        +{{ c2.teams.length - 3 }}
    </div>
    </div>
</div>
 
</div>

 </div>
 <div class="gap-1 flex-shrink-0 align-self-center" ng-if="canEdit('subtask')"><a href="javascript:void(0);"
     ng-click="loadSrcOwnersnew(c2,'team','subtask',$index)" 
     data-bs-toggle="modal" data-bs-target="#addowners">
     <span class="btn-plus2" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="+ Add Team">+</span></a>
</div>

</div>--%>
                                
                            </td>
                            <td> <input type="text" class="pmsinput" ng-readonly="!canEdit('subtask')" ng-disabled="!canEdit('subtask')"
           ng-model="c2.Weightage"
        ng-blur="updatedetails_subtask(c2, 'Weightage')"
          
           style="width:100%;background-color:{{projectlist.editcolor}}" ng-keypress="isnumeric($event)"></td>
                            <td><div class="d-flex">
       <div class="flex-grow-1 align-self-center">
       <div class="progress progress-xl cursor-pointer">
           <div class="progress-bar bg-success" role="progressbar" style="width:{{c2.completeissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.completeissue_rate}}% [{{c2.completeissue}} of {{c2.totalissue}} Issues Completed]">{{c2.completeissue}}</div>
           <div class="progress-bar bg-warning" role="progressbar" style="width:{{c2.pendingissue_rate}}%" aria-valuemin="0" aria-valuemax="100" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{c2.pendingissue_rate}}% [{{c2.pendingissue}} of {{c2.totalissue}} Issues Not Completed]">{{c2.pendingissue}}</div>

       </div>
       </div>
      
   </div></td>
                           <td> <a href="javascript:void(0);" style="font-size:22px;" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="loadSrcFiles(c2,'subtask',$index)" class="position-relative"><span class="btn-plus2" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="Upload Documents" ><i class="ri-file-text-line"></i></span><span class="files_count">{{ c2.viewfiles.length || 0 }}</span></a></td>
                        </tr>
                         <tr ng-repeat-end></tr>
                         <tr ng-repeat-end></tr>
                            <tr ng-repeat-end></tr>
                    </tbody>
                    <tfoot>
                        <tr class="fw-semibold project-duration-footer-row">
                            <td data-original-index="0"></td>
                            <td data-original-index="1"></td>
                            <td data-original-index="2"></td>
                            <td data-original-index="3"></td>
                            <td data-original-index="4"></td>
                            <td data-original-index="5"></td>
                            <td data-original-index="6"></td>
                            <td data-original-index="7" class="text-center project-duration-footer-cell">
                                <%--<span class="d-block text-nowrap">Project Total Duration Days</span>--%>
                                <strong>Total-{{projectDurationDays || 0}} Days</strong>
                            </td>
                            <td data-original-index="8"></td>
                            <td data-original-index="9"></td>
                            <td data-original-index="10"></td>
                            <td data-original-index="11"></td>
                            <td data-original-index="12"></td>
                            <td data-original-index="13"></td>
                            <td data-original-index="14"></td>
                            <td data-original-index="15"></td>
                            <td data-original-index="16"></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="d-flex align-items-center gap-2" style="padding-left:10px;">
                        <select class="form-select" ng-model="pageSize" ng-options="size for size in pageSizes" style="width:55px;"><option label="10" value="number:10" selected="selected">10</option><option label="20" value="number:20">20</option><option label="50" value="number:50">50</option><option label="100" value="number:100">100</option></select>
                        <span class="text-muted" style="font-size:11px;">Records per page</span>
                    </div>
                </div>
                <div class="col-sm-6 text-center" style="padding-top:7px;">
                    
                   <%-- <span class="text-muted" style="font-size:11px;">Showing 1 to 2 of 2 entries</span>--%>
                </div>
                <div class="col-sm-2">
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
            </div>
        </div>
        <!-- Gantt is intentionally isolated from the existing views. -->
        <div class="wrapper_inner p-0" ng-show="view=='gantt'">
            <style>
                .pms-gantt{display:flex;position:relative;height:calc(100vh - 165px);min-height:475px;border-top:1px solid #dfe5ea;background:#fff;overflow:hidden;--gantt-left:560px}
                .pms-gantt-loading{position:absolute;inset:0;z-index:500;display:flex;align-items:center;justify-content:center;background:rgba(255,255,255,.88);backdrop-filter:blur(1px)}
                .pms-gantt-loading-spinner{width:34px;height:34px;border:3px solid #e3eaf0;border-top-color:#7b3f86;border-radius:50%;animation:pms-gantt-spin .7s linear infinite}
                @keyframes pms-gantt-spin{to{transform:rotate(360deg)}}
                .pms-gantt-left{flex:0 0 var(--gantt-left);min-width:300px;max-width:65%;overflow:auto;background:#fff;scrollbar-width:none;-ms-overflow-style:none}
                .pms-gantt-left::-webkit-scrollbar{display:none;width:0;height:0}
                .pms-gantt-divider{flex:0 0 6px;background:#edf1f4;border-left:1px solid #d9e0e5;border-right:1px solid #d9e0e5;position:relative;z-index:8}
                .pms-gantt-divider:hover,.pms-gantt-divider.dragging{background:#9ebcf0}
                .pms-gantt-divider-resize{position:absolute;inset:0;cursor:col-resize;z-index:9}
                .pms-gantt-divider-toggle{position:absolute;top:210px;left:50%;transform:translateX(-50%);width:25px;height:34px;padding:0;border:1px solid #d7e0e6;border-radius:5px;background:#fff;color:#40505e;display:flex;align-items:center;justify-content:center;box-shadow:0 2px 7px rgba(0,0,0,.13);z-index:12;cursor:pointer}
                .pms-gantt.left-collapsed{--gantt-left:0px!important}.pms-gantt.left-collapsed .pms-gantt-left{min-width:0;max-width:0;border:0}.pms-gantt.left-collapsed .pms-gantt-divider{flex-basis:6px}.pms-gantt.left-collapsed .pms-gantt-divider-toggle{left:100%;transform:none}
                .pms-gantt-right{flex:1;min-width:0;overflow:auto;background:#fff;scrollbar-width:thin}
                .pms-gantt-head,.pms-gantt-row{display:grid;grid-template-columns:36px minmax(400px,1fr) minmax(105px,115px);height:35px;border-bottom:1px solid #e5eaee;align-items:center}
                .pms-gantt-head{height:40px;background:#f4f7f9;font-size:11px;font-weight:700;color:#34404b;text-transform:uppercase;position:sticky;top:0;z-index:70}
                .pms-gantt-head>div,.pms-gantt-row>div{height:100%;display:flex;align-items:center;padding:0 9px;border-right:1px solid #e5eaee;min-width:0}
                .pms-gantt-row:hover{background:#f8fbff}.pms-gantt-name{gap:5px;white-space:nowrap;overflow:hidden}.pms-gantt-name.level-0{font-weight:700}.pms-gantt-name.level-1{padding-left:27px}.pms-gantt-name.level-2{padding-left:49px}
                .pms-gantt-check{justify-content:center!important;padding:0!important}.pms-gantt-check input{width:14px;height:14px;margin:0;cursor:pointer;accent-color:#2878ed}.pms-gantt-bulk{height:42px;display:flex;align-items:center;gap:5px;padding:0 8px;background:#fff9df;border-bottom:1px solid #eadfb2;white-space:nowrap;overflow:hidden}.pms-gantt-bulk-close{border:0;background:transparent;color:#2878ed;font-size:20px;line-height:1;padding:0 4px}.pms-gantt-bulk-count{font-size:11px;font-weight:600;min-width:18px}.pms-gantt-bulk-btn{height:25px;border:1px solid #b9d1f7;border-radius:4px;background:#fff;color:#1768d5;padding:2px 10px;font-size:10px}.pms-gantt-bulk-btn.trash{border-color:#ff7777;color:#e02d2d}.pms-gantt-bulk-spacer{height:42px;background:#fff9df;border-bottom:1px solid #eadfb2}.pms-gantt-time-row.is-selected{background-color:#eaf3ff}
                .pms-gantt-toggle{width:16px;flex:0 0 16px;color:#536273;text-decoration:none}.pms-gantt-title{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.pms-gantt-name{overflow:visible;position:relative}.pms-gantt-row.menu-open{position:relative;z-index:80;background:#fff}.pms-gantt-name-input{border:0;background:transparent;outline:0;min-width:0;flex:1;font-size:12px;font-weight:inherit;padding:4px 2px}.pms-gantt-name-input:focus{background:#fff;border:1px solid #84b6ff;border-radius:2px}.pms-gantt-row-actions{display:flex;align-items:center;gap:2px;margin-left:auto;position:relative;z-index:25}.pms-gantt-row.menu-open .pms-gantt-row-actions{z-index:200}.pms-gantt-row-actions .btn-group{position:relative}.pms-gantt-row-action{width:22px;height:25px;border:0;border-radius:3px;background:transparent;color:#667583;padding:0;font-size:15px;display:flex;align-items:center;justify-content:center}.pms-gantt-row-action:hover,.pms-gantt-row-action:focus{background:#edf4ff;color:#1768d5}.pms-gantt-row-actions .dropdown-menu{top:calc(100% + 4px)!important;min-width:190px;padding:5px 0!important;background:#fff;border:1px solid #dfe5ea;border-radius:5px;box-shadow:0 8px 22px rgba(31,45,61,.18);z-index:300}.pms-gantt-row-actions .dropdown-menu-end{right:0;left:auto}.pms-gantt-row-actions .dropdown-item{display:flex;align-items:center;gap:4px;font-size:11px;line-height:18px;padding:7px 12px;color:#27333d}.pms-gantt-row-actions .dropdown-item:hover{background:#f1f6ff;color:#1768d5}.pms-gantt-row-actions .dropdown-item.text-danger{color:#e13b3b}.pms-gantt-owner-edit{cursor:pointer}.pms-gantt-status-edit{cursor:pointer;width:100%;justify-content:center;background:transparent}.pms-gantt-status-menu{max-height:180px;overflow:auto;min-width:155px}.pms-gantt-status-menu .dropdown-item{font-size:11px;padding:6px 10px}
                .pms-gantt-row-actions .dropdown-menu{max-height:180px;overflow-y:auto;overscroll-behavior:contain;scrollbar-width:thin}
                .pms-gantt-row:nth-last-child(-n+2) .pms-gantt-row-actions .dropdown-menu{top:auto!important;bottom:calc(100% + 4px)!important}
                .pms-gantt-owners{gap:3px;overflow:hidden}.pms-gantt-avatar{width:25px;height:25px;border-radius:50%;background:#e9eef3;color:#62707d;font-size:9px;display:inline-flex;align-items:center;justify-content:center;overflow:hidden;flex:0 0 25px}.pms-gantt-avatar img{width:100%;height:100%;object-fit:cover}
                .pms-gantt-status{font-size:10px;font-weight:600;border:1px solid;padding:3px 7px;border-radius:3px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
                .pms-gantt-progress{width:100%}.pms-gantt-progress-text{font-size:10px;font-weight:600;margin-bottom:4px}.pms-gantt-progress-track{height:6px;background:#e8edf1;border-radius:4px;overflow:hidden}.pms-gantt-progress-fill{height:100%;background:#13a33b;border-radius:4px}
                /* Controls sit over the existing Gantt header and do not add a layout row. */
                .pms-gantt-controls{position:sticky;top:0;left:0;width:100%;height:0;z-index:90;pointer-events:none}
                .pms-gantt-controls button,.pms-gantt-controls select{pointer-events:auto;cursor:pointer}
                .pms-gantt-range-button{position:absolute;top:1px;width:23px;height:17px;padding:0;border:0;background:#f4f7f9;color:#334d68;font-size:15px;line-height:16px}
                .pms-gantt-range-button.prepend{left:0}.pms-gantt-range-button.postpend{right:0}
                .pms-gantt-range-button:hover,.pms-gantt-range-button:focus-visible{background:#dceaff;color:#1659a9}
                .pms-gantt-scale{position:absolute;top:43px;right:30px;display:flex;align-items:center;gap:4px;padding:2px;background:#fff;border:1px solid #dfe5ea;border-radius:3px;box-shadow:0 1px 4px rgba(40,60,85,.08)}
                .pms-gantt-scale select,.pms-gantt-scale button{height:25px;border:1px solid #d9e0e5;background:#fff;padding:2px 9px;font-size:10px}
                .pms-gantt-scale select{min-width:92px}.pms-gantt-scale button{width:31px;font-size:14px;padding:0}
                .pms-gantt-timeline{position:relative;min-width:100%;width:max-content}.pms-gantt-groups,.pms-gantt-cells{display:flex;position:sticky;z-index:70}.pms-gantt-groups{height:18px;top:0}.pms-gantt-cells{height:22px;top:18px}.pms-gantt-group,.pms-gantt-cell{flex:0 0 auto;display:flex;align-items:center;justify-content:center;border-right:1px solid #dde4e9;border-bottom:1px solid #dde4e9;background:#f4f7f9;font-size:10px;font-weight:600}.pms-gantt-cell{font-weight:500;background:#fff}
                .pms-gantt-time-row{height:35px;position:relative;border-bottom:1px solid #e5eaee;background-image:linear-gradient(to right,#e9eef2 1px,transparent 1px);background-size:var(--gantt-cell) 100%}
                .pms-gantt-cell.is-weekoff,.pms-gantt-shade.is-weekoff{background-color:#e8f3ff}.pms-gantt-cell.is-holiday,.pms-gantt-shade.is-holiday{background-color:#fff0da}.pms-gantt-cell.is-weekoff{box-shadow:inset 0 3px #549bea}.pms-gantt-cell.is-holiday{box-shadow:inset 0 3px #ef9b39}.pms-gantt-shade{position:absolute;top:0;bottom:0;pointer-events:none;z-index:0}.pms-gantt-bar{z-index:2}
                .pms-gantt-bar{position:absolute;top:5px;height:26px;min-width:30px;border-radius:3px;color:#17212b;font-size:10px;font-weight:600;display:flex;align-items:center;justify-content:center;white-space:nowrap;box-shadow:0 1px 2px rgba(0,0,0,.12);user-select:none}
                .pms-gantt-bar-label{overflow:hidden;text-overflow:ellipsis;padding:0 7px;max-width:100%;pointer-events:none}.pms-gantt-resize{position:absolute;top:0;bottom:0;width:6px;cursor:ew-resize;z-index:3}.pms-gantt-resize.left{left:0}.pms-gantt-resize.right{right:0}.pms-gantt-bar:hover .pms-gantt-resize{background:rgba(255,255,255,.45)}
                .pms-gantt-empty{padding:35px;text-align:center;color:#7b8792}.pms-gantt-today{position:absolute;top:32px;bottom:0;border-left:1px dashed #ff3b30;z-index:4;pointer-events:none}
                .pms-gantt-row.is-selected{background:#eaf3ff}.pms-gantt-row[data-gantt-type="subtask"] .pms-gantt-title,.pms-gantt-bar.is-subtask{cursor:pointer}
                .pms-gantt-drag-handle{flex:0 0 15px;width:15px;height:24px;padding:0;border:0;background:transparent;color:#8090a0;cursor:grab;display:inline-flex;align-items:center;justify-content:center}.pms-gantt-drag-handle:active{cursor:grabbing}.pms-gantt-drag-handle:hover{color:#168ff1}
                .gantt-soft-ghost{position:fixed;z-index:10000;pointer-events:none;background:#fff;border:1px solid #64b7f1;border-radius:5px;box-shadow:0 6px 18px rgba(25,55,85,.22);padding:7px 12px;font-size:12px;color:#253747;max-width:320px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.gantt-soft-ghost.invalid-drop{border-color:#e66b6b}.pms-gantt-row.gantt-row-moving{opacity:.55}.pms-gantt-row.gantt-row-before,.pms-gantt-time-row.gantt-row-before{box-shadow:inset 0 3px 0 #09a8ef;background-color:#edf8ff}.pms-gantt-row.gantt-row-after,.pms-gantt-time-row.gantt-row-after{box-shadow:inset 0 -3px 0 #09a8ef;background-color:#edf8ff}.pms-gantt-row.gantt-row-invalid{background:#fff2f2}.pms-gantt-reorder-toast{position:absolute;top:6px;left:50%;transform:translateX(-50%);z-index:500;background:#fff;border-left:3px solid #30bf68;border-radius:3px;box-shadow:0 3px 12px rgba(20,40,60,.22);padding:8px 16px;font-size:12px;white-space:nowrap}
                .pms-gantt-detail{position:absolute;z-index:450;top:74px;right:20px;width:min(650px,calc(100% - 40px));max-height:calc(100% - 94px);background:#fff;border:2px solid #13a9f4;border-radius:15px;box-shadow:0 10px 28px rgba(38,63,85,.18);color:#1d2730;overflow:auto;overscroll-behavior:contain;scrollbar-width:thin}
                .pms-gantt-detail-close{position:absolute;right:13px;top:10px;border:0;background:transparent;color:#258bd4;font-size:20px;line-height:1;padding:2px 5px;z-index:2}
                .pms-gantt-detail-top{display:flex;gap:14px;padding:24px 24px 15px}.pms-gantt-detail-progress{flex:0 0 42px;width:42px;height:42px;border:4px solid #e5edf3;border-radius:50%;display:flex;align-items:center;justify-content:center;color:#0aa8ec;font-size:11px;font-weight:700}
                .pms-gantt-detail-main{min-width:0;flex:1}.pms-gantt-detail-badges{display:flex;align-items:center;gap:8px;margin-top:-25px;margin-bottom:8px}.pms-gantt-type-badge{background:#08a8ed;color:#fff;padding:4px 11px;border-radius:0 0 5px 5px;font-size:11px;font-weight:600}.pms-gantt-code-badge{border:1px solid #dde4e9;color:#8995a0;padding:3px 10px;border-radius:3px;font-size:10px}.pms-gantt-detail-title{font-size:14px;font-weight:600;margin-bottom:7px}.pms-gantt-detail-meta{display:flex;align-items:center;gap:8px;flex-wrap:wrap;color:#64717c;font-size:11px}.pms-gantt-detail-status{color:#fff;padding:2px 7px;border-radius:2px;font-weight:600}.pms-gantt-detail-duration{margin-top:7px;color:#36a550;font-size:11px}.pms-gantt-detail-owner{color:#65727d;margin-left:10px}
                .pms-gantt-detail-tabs{display:flex;gap:20px;margin:0 24px;border-bottom:1px solid #dbe3e9}.pms-gantt-detail-tab{border:0;background:transparent;padding:8px 0 7px;color:#17212b;font-size:12px}.pms-gantt-detail-tab.active{border-bottom:2px solid #168ff1}
                .pms-gantt-dependency-grid{margin:14px 24px 24px;overflow-x:auto;border-bottom:1px solid #dbe3e9}.pms-gantt-dependency-grid table{width:100%;min-width:610px;border-collapse:collapse;font-size:11px}.pms-gantt-dependency-grid th,.pms-gantt-dependency-grid td{height:32px;padding:7px 11px;text-align:left;border:1px solid #dbe3e9;font-weight:400}.pms-gantt-dependency-grid th{background:#fff}.pms-gantt-add-dependency{color:#168ff1;text-decoration:none}.pms-gantt-dependency-empty{color:#8b98a4}
                .pms-gantt-dependency-grid .pms-dependency-search-cell{position:relative;min-width:225px}.pms-dependency-search-cell input{width:100%;border:1px solid #cbdcf3;padding:6px 9px;font:inherit}.pms-dependency-choices{position:absolute;z-index:100;top:100%;left:0;width:max(100%,320px);max-height:180px;overflow-y:auto;background:#fff;border:1px solid #cbdcf3;box-shadow:0 6px 18px #253a581f}.pms-dependency-choices button{display:block;width:100%;padding:8px;text-align:left;border:0;background:#fff;color:#263849}.pms-dependency-choices button:hover,.pms-dependency-choices button:focus{background:#e9f3ff}.pms-dependency-added{color:#21864b;padding:5px 0}
                .pms-dependency-row-actions{position:relative;display:inline-block}.pms-dependency-row-menu{position:absolute;z-index:105;top:100%;left:0;min-width:140px;background:white;border:1px solid #dbe3e9;box-shadow:0 6px 16px #253a5826}.pms-dependency-row-menu button{display:block;border:0;background:white;width:100%;padding:9px 12px;text-align:left}.pms-dependency-row-menu button:hover{background:#e9f3ff}
                .pms-gantt-dependency-grid .pms-dependency-menu-cell{width:25px;min-width:25px;max-width:25px;padding:0 2px;text-align:center}.pms-dependency-menu-cell>button{border:0;background:transparent;color:#2788cf;padding:3px;font-size:16px;line-height:1}
                .pms-dependency-search-cell .pms-dependency-search-wrap .pms-dependency-project{width:105px;max-width:105px;border:0;border-left:1px solid #9ebbe0;padding:5px 2px;font:inherit;background:#fff;color:#283d56}
                .pms-gantt-detail{overflow:visible}.pms-gantt-dependency-grid{max-height:calc(100vh - 330px);overflow:auto}.pms-gantt-dependency-grid .pms-dependency-choices{position:fixed;z-index:800;max-height:180px;overflow-y:auto;background:#fff;border:1px solid #cbdcf3;box-shadow:0 6px 18px #253a5833}.pms-dependency-search-cell .pms-dependency-search-wrap{display:flex;align-items:center;border:1px solid #9ebbe0;background:#fff}.pms-dependency-search-wrap input{border:0;min-width:0;flex:1}.pms-dependency-search-wrap button{border:0;background:#fff;padding:5px 8px;color:#67809b}
                .pms-gantt-dependency-grid .pms-dependency-choices button.is-active{background:#e9f3ff;color:#0877ca}
                /* Keep the dependency editor aligned with the Gantt reference grid. */
                .pms-gantt-detail{position:fixed;top:clamp(160px,26vh,230px);right:clamp(16px,10vw,160px);width:min(650px,calc(100vw - 32px));max-height:calc(100vh - 190px);z-index:1050}
                .pms-gantt-detail-top{padding-top:22px;padding-bottom:13px}
                .pms-gantt-dependency-grid{margin:14px 24px 20px;max-height:calc(100vh - 370px);overflow:auto}
                .pms-gantt-dependency-grid table{width:1195px;min-width:1195px;table-layout:fixed;font-size:12px}
                .pms-gantt-dependency-grid th,.pms-gantt-dependency-grid td{height:37px;box-sizing:border-box;padding:7px 10px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
                .pms-gantt-dependency-grid .pms-dependency-search-cell{overflow:visible}
                .pms-gantt-dependency-grid .pms-dependency-menu-cell{width:25px;min-width:25px;max-width:25px}
                .pms-gantt-dependency-grid .pms-dependency-search-wrap{width:100%;height:32px}
                .pms-gantt-dependency-grid .pms-dependency-search-wrap input{height:30px;box-sizing:border-box}
                .pms-gantt-dependency-grid .pms-dependency-search-wrap .pms-dependency-project{width:120px;max-width:120px;height:30px}
                .pms-gantt-dependency-grid .pms-dependency-row-menu{position:fixed}
                .pms-gantt-dependency-grid .pms-dependency-menu-cell .pms-dependency-row-actions>button{appearance:none;-webkit-appearance:none;display:inline-flex;align-items:center;justify-content:center;width:21px;height:26px;margin:0;padding:0;border:0!important;border-radius:0;outline:none;background:transparent!important;box-shadow:none!important;color:#66809b;font-size:17px;line-height:1;cursor:pointer}
                .pms-gantt-dependency-grid .pms-dependency-menu-cell .pms-dependency-row-actions>button:hover,.pms-gantt-dependency-grid .pms-dependency-menu-cell .pms-dependency-row-actions>button:focus-visible{color:#168ff1;background:transparent!important;box-shadow:none!important}
                .pms-gantt-link-layer{position:absolute;left:0;z-index:4;overflow:visible;pointer-events:none}.pms-gantt-link-layer>path{fill:none;stroke:#447ba9;stroke-width:1.6;stroke-linejoin:round;stroke-linecap:round}.pms-gantt-link-layer marker path{fill:#447ba9;stroke:none}
                .pms-gantt-view-switch{position:absolute;right:47px;top:11px;z-index:3;border:1px solid #cfe0ef;background:#f7fbff;color:#2678b9;border-radius:7px;padding:5px 9px;font-size:11px;cursor:pointer}
                .pms-gantt-modern-choices-portal{position:fixed;z-index:2200;max-height:180px;overflow-y:auto;overscroll-behavior:contain;background:#fff;border:1px solid #d5e1ee;border-radius:10px;box-shadow:0 12px 30px rgba(22,45,72,.18);padding:5px}.pms-gantt-modern-choices-portal button{display:flex;align-items:center;gap:8px;width:100%;min-height:38px;padding:7px 9px;text-align:left;border:0;border-radius:7px;background:#fff;color:#263849;font-size:12px;cursor:pointer}.pms-gantt-modern-choice-code{flex:0 0 auto;padding:5px 7px;border-radius:6px;background:#eef5ff;color:#2d75b5;font-size:10px;font-weight:700}.pms-gantt-modern-choice-name{flex:1;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.pms-gantt-modern-choice-type{flex:0 0 auto;padding:4px 6px;border-radius:10px;background:#f1f4f8;color:#718198;font-size:9px;text-transform:capitalize}.pms-gantt-modern-choices-portal button:hover,.pms-gantt-modern-choices-portal button:focus,.pms-gantt-modern-choices-portal button.is-active{background:#edf6ff;color:#0877ca;outline:0}.pms-gantt-modern-view .pms-dependency-row-menu{position:fixed;z-index:2300;min-width:132px;padding:5px;background:#fff;border:1px solid #dbe5ef;border-radius:10px;box-shadow:0 10px 26px rgba(20,42,68,.2)}.pms-gantt-modern-view .pms-dependency-row-menu button{display:block;width:100%;padding:9px 11px;border:0;border-radius:6px;background:transparent;color:#33465b;text-align:left;font-size:12px;cursor:pointer}.pms-gantt-modern-view .pms-dependency-row-menu button:hover,.pms-gantt-modern-view .pms-dependency-row-menu button:focus-visible{background:#eef6ff;color:#176eaf;outline:0}.pms-gantt-modern-view .pms-dependency-row-menu button:last-child{color:#b42318}.pms-gantt-modern-view .pms-dependency-row-menu button:last-child:hover{background:#fff1f0;color:#b42318}.pms-gantt-modern-menu-portal{position:fixed!important;z-index:2300;width:144px;min-width:144px;box-sizing:border-box;margin:0;padding:5px;background:#fff;border:1px solid #dbe5ef;border-radius:10px;box-shadow:0 10px 26px rgba(20,42,68,.2)}.pms-gantt-modern-menu-portal button{display:block;width:100%;padding:9px 11px;border:0;border-radius:6px;background:transparent;color:#33465b;text-align:left;font-size:12px;cursor:pointer}.pms-gantt-modern-menu-portal button:hover,.pms-gantt-modern-menu-portal button:focus-visible{background:#eef6ff;color:#176eaf;outline:0}.pms-gantt-modern-menu-portal button:last-child{color:#b42318}.pms-gantt-modern-menu-portal button:last-child:hover{background:#fff1f0;color:#b42318}
                .pms-gantt-detail.pms-gantt-detail-modern{top:50%;right:auto;left:50%;transform:translate(-50%,-50%);width:min(840px,calc(100vw - 32px));max-height:calc(100vh - 40px);overflow:hidden;border:1px solid #dce6f0;border-radius:18px;box-shadow:0 24px 70px rgba(10,25,45,.28);color:#172b45}
                .pms-gantt-modern-view{position:relative;max-height:calc(100vh - 42px);overflow:auto;background:#fff;border-radius:17px}.pms-gantt-modern-header{display:flex;align-items:center;justify-content:space-between;padding:17px 24px;border-bottom:1px solid #e8eef5;background:#fbfdff}.pms-gantt-modern-title{font-size:20px;font-weight:700;letter-spacing:-.2px;color:#172b45}.pms-gantt-modern-subtitle{font-size:12px;color:#75869b;margin-top:5px}.pms-gantt-modern-close{border:0;background:#f5f8fb;color:#8191a3;border-radius:10px;width:40px;height:40px;font-size:24px;line-height:1;cursor:pointer}
                .pms-gantt-modern-content{padding:16px 24px 18px}.pms-gantt-modern-item{display:flex;align-items:center;justify-content:space-between;padding:12px 14px;border:1px solid #e5edf5;border-radius:12px;background:#f9fbfd}.pms-gantt-modern-type{display:inline-block;margin-right:10px;background:#eaf3ff;color:#1874cc;border-radius:7px;padding:6px 9px;font-size:10px;font-weight:700;text-transform:uppercase}.pms-gantt-modern-item strong{font-size:14px}.pms-gantt-modern-meta{font-size:11px;color:#7c8da1;margin-top:5px}.pms-gantt-modern-progress{font-size:11px;font-weight:700;color:#117b6b;background:#e6f7f3;border-radius:20px;padding:7px 11px}
                .pms-gantt-modern-tabs{display:flex;gap:7px;width:max-content;margin:13px 0;padding:4px;background:#f1f5f9;border-radius:10px}.pms-gantt-modern-tabs button{border:0;background:transparent;color:#73839a;font-weight:700;font-size:12px;padding:9px 14px;border-radius:8px;cursor:pointer}.pms-gantt-modern-tabs button.active{background:#fff;color:#1d68b3;box-shadow:0 1px 4px rgba(20,48,78,.12)}.pms-gantt-modern-tabs button span{display:inline-block;min-width:16px;margin-left:4px;color:#4b8fd1}
                .pms-gantt-modern-section-head{display:flex;align-items:center;justify-content:space-between;margin:4px 0 8px}.pms-gantt-modern-section-head strong{font-size:13px}.pms-gantt-modern-section-head span,.pms-gantt-modern-footer span{font-size:11px;color:#8594a6}.pms-gantt-modern-section-head button,.pms-gantt-modern-add-head button,.pms-gantt-modern-save{border:0;background:#2479c5;color:#fff;border-radius:8px;padding:9px 13px;font-size:11px;font-weight:700;cursor:pointer}.pms-gantt-modern-list{max-height:190px;overflow:auto}.pms-gantt-modern-link{display:grid;grid-template-columns:25px 55px minmax(130px,1fr) 90px 220px;align-items:center;gap:10px;padding:10px 11px;border:1px solid #e7edf4;border-radius:10px;margin-bottom:8px;background:#fff}.pms-gantt-modern-menu{border:0;background:transparent;color:#8292a5;font-size:18px;cursor:pointer}.pms-gantt-modern-code{width:max-content;border-radius:7px;background:#eff6ff;color:#377db8;padding:7px 8px;font-size:10px;font-weight:700}.pms-gantt-modern-linkname strong{display:block;font-size:12px}.pms-gantt-modern-linkname span{display:block;font-size:10px;color:#8493a6;margin-top:4px}.pms-gantt-modern-relation{text-align:center}.pms-gantt-modern-relation span{display:inline-block;padding:5px 8px;border-radius:6px;background:#eef6ff;color:#2876b9;font-size:11px;font-weight:700}.pms-gantt-modern-relation small{display:block;margin-top:4px;font-size:9px;color:#8594a6}.pms-gantt-modern-dates{font-size:10px;color:#596c82}.pms-gantt-modern-empty,.pms-gantt-modern-loading{padding:17px;text-align:center;color:#8493a6;font-size:12px;border:1px dashed #d7e2ed;border-radius:9px}
                .pms-gantt-modern-add{margin-top:4px;padding:12px;border:1px dashed #bad1e8;background:#f8fbff;border-radius:12px}.pms-gantt-modern-add-head{display:flex;align-items:center;justify-content:space-between}.pms-gantt-modern-add-head strong{display:block;font-size:12px;text-transform:capitalize}.pms-gantt-modern-add-head span{display:block;margin-top:4px;font-size:10px;color:#8191a3}.pms-gantt-modern-add-controls{display:grid;grid-template-columns:minmax(170px,1fr) 135px 68px 72px 88px 65px;gap:7px;margin-top:10px;align-items:center}.pms-gantt-modern-edit-target{grid-column:span 2;min-width:0;padding:0 10px;color:#31445a;font-size:11px;font-weight:600}.pms-gantt-modern-search{position:relative;display:flex;align-items:center;border:1px solid #d7e2ed;border-radius:8px;background:#fff;height:38px}.pms-gantt-modern-search input{width:100%;height:36px;padding:0 30px 0 10px;border:0;background:transparent;font-size:11px;outline:0}.pms-gantt-modern-search>span{position:absolute;right:9px;color:#73869b}.pms-gantt-modern-select,.pms-gantt-modern-lag{height:38px;min-width:0;border:1px solid #d7e2ed;border-radius:8px;background:#fff;padding:0 8px;color:#31445a;font-size:11px}.pms-gantt-modern-lag{width:100%;padding:0 7px}.pms-gantt-modern-cancel{border:0;background:transparent;color:#61738a;font-size:11px;cursor:pointer}.pms-gantt-modern-selected{margin-top:8px;font-size:10px;color:#6383a1}.pms-gantt-modern-error{color:#b42318;margin-top:8px;font-size:11px}.pms-gantt-modern-footer{display:flex;align-items:center;justify-content:space-between;margin-top:12px;padding-top:10px;border-top:1px solid #e8eef5}.pms-gantt-modern-footer button{border:0;background:transparent;color:#2876b9;font-size:11px;cursor:pointer}
                @media(max-width:720px){.pms-gantt-modern-content{padding:12px}.pms-gantt-modern-link{grid-template-columns:22px 48px minmax(100px,1fr) 58px}.pms-gantt-modern-dates{grid-column:3/5}.pms-gantt-modern-add-controls{grid-template-columns:1fr 1fr}.pms-gantt-modern-search{grid-column:1/3}.pms-gantt-modern-list{max-height:calc(100vh - 450px)}}
            </style>
            <div class="pms-gantt" id="pmsGanttRoot" ng-class="{'left-collapsed':ganttPanelCollapsed}" ng-style="{'--gantt-left':ganttLeftWidth+'px'}">
                <div class="pms-gantt-loading" ng-if="ganttIsLoading" aria-label="Loading Gantt"><span class="pms-gantt-loading-spinner"></span></div>
                <div class="pms-gantt-reorder-toast" ng-if="ganttReorderSaving || ganttReorderMessage" role="status">{{ganttReorderSaving ? 'Saving order...' : ganttReorderMessage}}</div>
                <section class="pms-gantt-left">
                    <div class="pms-gantt-head">
                        <div class="pms-gantt-check" >
                            <input type="checkbox" style="display:none" ng-checked="areAllVisibleGanttRowsSelected()" ng-click="toggleAllVisibleGanttRows($event)" aria-label="Select all visible Gantt rows" />

                        </div>
                        <div>Task</div><div>Status</div></div>
                    <div class="pms-gantt-bulk" ng-if="getGanttSelectedCount()">
                        <button type="button" class="pms-gantt-bulk-close" ng-click="clearGanttSelection($event)" title="Clear selection">×</button>
                        <input type="checkbox" ng-checked="getGanttSelectedCount()>0" ng-click="clearGanttSelection($event)" aria-label="Clear selected subtasks" />
                        <span class="pms-gantt-bulk-count">{{getGanttSelectedCount()}}</span>
                        <%--<button type="button" class="pms-gantt-bulk-btn" ng-click="ganttBulkAction('move',$event)">Move</button>
                        <button type="button" class="pms-gantt-bulk-btn trash" ng-click="ganttBulkAction('trash',$event)">Trash</button>
                        <button type="button" class="pms-gantt-bulk-btn" ng-click="ganttBulkAction('owner',$event)">Owner</button>--%>
                        <button type="button" class="pms-gantt-bulk-btn" ng-click="ganttBulkAction('status',$event)">Status</button>
                        <button type="button" class="pms-gantt-bulk-btn" ng-click="ganttBulkAction('tags',$event)">Tags</button>
                        <%--<button type="button" class="pms-gantt-bulk-btn" ng-click="ganttBulkAction('more',$event)">•••</button>--%>
                    </div>
                    <div class="pms-gantt-row" ng-repeat="row in ganttRows track by row.uid" data-gantt-row="{{row.uid}}" data-gantt-type="{{row.type}}" ng-class="{'is-selected':ganttDetailRow.uid==row.uid || isGanttRowSelected(row),'menu-open':isGanttAnyRowMenuOpen(row)}">
                        <div class="pms-gantt-check" ><button type="button" class="pms-gantt-drag-handle" ng-if="canEdit(row.type)" ng-mousedown="startSoftGanttRowDrag($event,row)" ng-click="$event.stopPropagation()" title="Drag to reorder" aria-label="Drag {{row.name}} to reorder"><i class="ri-draggable"></i></button><input type="checkbox" style="display:none;" ng-if="row.type=='subtask'" ng-checked="isGanttRowSelected(row)" ng-click="toggleGanttRowSelection(row,$event)" aria-label="Select {{row.name}}" /></div>
                        <div class="pms-gantt-name level-{{row.level}}">
                            <a href="javascript:void(0)" class="pms-gantt-toggle" ng-click="hasGanttChildRows(row) && toggleGanttRow(row,$event)"><i ng-if="hasGanttChildRows(row)" ng-class="row.expanded ? 'ri-arrow-down-s-line' : 'ri-arrow-right-s-line'"></i></a>
                            <b>{{row.serialNo}}</b>
                            <input type="text" class="pms-gantt-name-input" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" data-bs-original-title="{{row.name}}" ng-mouseleave="hideGanttNameTooltip($event)" ng-model="row.name" ng-readonly="!canEdit(row.type)" ng-disabled="!canEdit(row.type)" ng-blur="updateGanttRowName(row)" ng-click="$event.stopPropagation()" />
                            <div class="pms-gantt-row-actions">
                                <div class="btn-group">
                                    <button type="button" class="pms-gantt-row-action" ng-click="toggleGanttRowMenu(row,'add',$event)" title="Add"><i class="ri-add-line"></i></button>
                                    <div class="dropdown-menu p-0" ng-class="{'show':isGanttRowMenuOpen(row,'add')}" ng-show="isGanttRowMenuOpen(row,'add')">
                                        <%if (ischeckCreate == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='milestone'" ng-click="ganttAddRow(row,'up')"><i class="ri-add-line me-1"></i>Add Above</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='milestone'" ng-click="ganttAddRow(row,'down')"><i class="ri-add-line me-1"></i>Add Below</a><%} %>
                                        <%if (ischeckCreate_task == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='task'" ng-click="ganttAddRow(row,'up')"><i class="ri-add-line me-1"></i>Add Above</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='task'" ng-click="ganttAddRow(row,'down')"><i class="ri-add-line me-1"></i>Add Below</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='milestone' && row.istask==1" ng-click="ganttAddChild(row)"><i class="ri-add-line me-1"></i>Add Task</a><%} %>
                                        <%if (ischeckCreate_stask == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='subtask'" ng-click="ganttAddRow(row,'up')"><i class="ri-add-line me-1"></i>Add Above</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='subtask'" ng-click="ganttAddRow(row,'down')"><i class="ri-add-line me-1"></i>Add Below</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='task' && row.issubtask==1" ng-click="ganttAddChild(row)"><i class="ri-add-line me-1"></i>Add Sub Task</a><%} %>
                                    </div>
                                </div>
                                <div class="btn-group">
                                    <button type="button" class="pms-gantt-row-action" ng-click="toggleGanttRowMenu(row,'more',$event)" title="More"><i class="ri-more-2-fill"></i></button>
                                    <div class="dropdown-menu dropdown-menu-end p-0" ng-class="{'show':isGanttRowMenuOpen(row,'more')}" ng-show="isGanttRowMenuOpen(row,'more')">
                                        <%--<a class="dropdown-item" href="javascript:void(0)" ng-click="openGanttCompletionDependency(row,$event);closeGanttRowMenu()"><i class="ri-parent-line me-1"></i>Dependency</a>--%>
                                        <a class="dropdown-item" href="javascript:void(0)" ng-click="openGanttSubtaskPopup(row,$event);closeGanttRowMenu()"><i class="ri-parent-line me-1"></i>Dependency</a>
                                        <a class="dropdown-item" href="javascript:void(0)" ng-click="openGanttIssue(row,$event)"><i class="bx bx-error-circle me-1"></i>Add Issue</a>
                                        <%if (ischeckEdit == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='milestone'" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="ganttUploadDocuments(row)"><i class="bx bx-upload me-1"></i>Upload Documents</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='milestone'" data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="ganttEditDetails(row)"><i class="bx bx-edit me-1"></i>Edit Milestone</a><%} %>
                                        <%if (ischeckEdit_task == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='task'" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="ganttUploadDocuments(row)"><i class="bx bx-upload me-1"></i>Upload Documents</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='task'" data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="ganttEditDetails(row)"><i class="bx bx-edit me-1"></i>Edit Task</a><%} %>
                                        <%if (ischeckEdit_stask == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='subtask'" data-bs-toggle="offcanvas" data-bs-target="#addfiles" ng-click="ganttUploadDocuments(row)"><i class="bx bx-upload me-1"></i>Upload Documents</a><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='subtask'" data-bs-toggle="offcanvas" data-bs-target="#addmile" ng-click="ganttEditDetails(row)"><i class="bx bx-edit me-1"></i>Edit Sub Task</a><%} %>
                                        <%if (ischeckCreate == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='milestone'" ng-click="ganttCloneRow(row)"><i class="bx bx-copy me-1"></i>Clone</a><%} %><%if (ischeckCreate_task == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='task'" ng-click="ganttCloneRow(row)"><i class="bx bx-copy me-1"></i>Clone</a><%} %><%if (ischeckCreate_stask == 1) {%><a class="dropdown-item" href="javascript:void(0)" ng-if="row.type=='subtask'" ng-click="ganttCloneRow(row)"><i class="bx bx-copy me-1"></i>Clone</a><%} %>
                                        <%if (ischeckDelete == 1) {%><a class="dropdown-item text-danger" href="javascript:void(0)" ng-if="row.type=='milestone'" ng-click="ganttDeleteRow(row)"><i class="bx bx-trash me-1"></i>Delete</a><%} %><%if (ischeckDelete_task == 1) {%><a class="dropdown-item text-danger" href="javascript:void(0)" ng-if="row.type=='task'" ng-click="ganttDeleteRow(row)"><i class="bx bx-trash me-1"></i>Delete</a><%} %><%if (ischeckDelete_stask == 1) {%><a class="dropdown-item text-danger" href="javascript:void(0)" ng-if="row.type=='subtask'" ng-click="ganttDeleteRow(row)"><i class="bx bx-trash me-1"></i>Delete</a><%} %>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="btn-group"><button type="button" class="pms-gantt-status pms-gantt-status-edit" data-bs-toggle="dropdown" data-bs-auto-close="true" ng-disabled="!canEdit(row.type)" ng-style="{'color':'black','border-color':row.status_color || '#b8c3cc'}">{{row.status_name || 'Open'}}</button><div class="dropdown-menu pms-gantt-status-menu p-0"><a class="dropdown-item" href="javascript:void(0)" ng-repeat="newstatus in statuslists" ng-click="selectGanttStatus(row,newstatus,$event)"><i class="ri-checkbox-blank-fill me-1" ng-style="{'color':newstatus.colorname}"></i>{{newstatus.name}}</a></div></div>
                    </div>
                </section>
                <div class="pms-gantt-divider"><span class="pms-gantt-divider-resize" ng-mousedown="startGanttPanelResize($event)"></span><button type="button" class="pms-gantt-divider-toggle" ng-mousedown="$event.stopPropagation()" ng-click="toggleGanttPanel($event)" title="{{ganttPanelCollapsed ? 'Expand' : 'Collapse'}}" aria-label="{{ganttPanelCollapsed ? 'Expand left panel' : 'Collapse left panel'}}"><i ng-class="ganttPanelCollapsed ? 'ri-arrow-right-s-line' : 'ri-arrow-left-s-line'"></i></button></div>
                <section class="pms-gantt-right">
                    <div class="pms-gantt-controls">
                        <button type="button" class="pms-gantt-range-button prepend" ng-click="ganttExtendRange('prepend')" title="Prepend earlier period" aria-label="Show earlier dates"><i class="bx bx-chevrons-left"></i></button>
                        <button type="button" class="pms-gantt-range-button postpend" ng-click="ganttExtendRange('postpend')" title="Postpend future period" aria-label="Show later dates"><i class="bx bx-chevrons-right"></i></button>
                        <div class="pms-gantt-scale"><select ng-model="ganttScale" ng-change="changeGanttScale(ganttScale)" aria-label="Gantt time scale"><option value="year">Quarters</option><option value="quarter">Months</option><option value="month">Weeks</option><option value="week">Days</option><option value="day">Hours</option></select><button type="button" ng-click="ganttZoom(-1)" title="Zoom out" aria-label="Zoom out">−</button><button type="button" ng-click="ganttZoom(1)" title="Zoom in" aria-label="Zoom in">+</button></div>
                    </div>
                    <div class="pms-gantt-timeline" ng-style="{'width':(ganttCells.length*ganttCellWidth)+'px','--gantt-cell':ganttCellWidth+'px'}">
                        <div class="pms-gantt-groups"><div class="pms-gantt-group" ng-repeat="group in ganttHeaderGroups track by $index" ng-style="{'width':group.width+'px'}">{{group.label}}</div></div>
                        <div class="pms-gantt-cells"><div class="pms-gantt-cell" ng-repeat="cell in ganttCells track by $index" ng-class="{'is-weekoff':cell.nonWorkingKind=='weekoff','is-holiday':cell.nonWorkingKind=='holiday'}" ng-style="{'width':ganttCellWidth+'px'}" title="{{cell.nonWorkingKind=='holiday' ? 'Holiday' : (cell.nonWorkingKind=='weekoff' ? 'Week off' : '')}}">{{cell.label}}</div></div>
                        <div class="pms-gantt-today" ng-if="getGanttTodayStyle()" ng-style="getGanttTodayStyle()"></div>
                        <div class="pms-gantt-bulk-spacer" ng-if="getGanttSelectedCount()"></div>
                        <svg class="pms-gantt-link-layer" aria-label="Gantt dependency arrows" ng-attr-width="{{ganttCells.length*ganttCellWidth}}" ng-attr-height="{{ganttRows.length*35}}" ng-style="{'top':(40+(getGanttSelectedCount() ? 42 : 0))+'px'}" ng-if="ganttDependencyPaths.length"><defs><marker id="pmsGanttLinkArrow" markerWidth="7" markerHeight="7" refX="6" refY="3.5" orient="auto" markerUnits="userSpaceOnUse"><path d="M 0 0 L 7 3.5 L 0 7 Z" fill="#447ba9" stroke="none"></path></marker></defs><path ng-repeat="link in ganttDependencyPaths track by link.id" ng-attr-d="{{link.d}}" marker-end="url(#pmsGanttLinkArrow)"><title>{{link.type}} dependency</title></path></svg>
                        <div class="pms-gantt-time-row" ng-repeat="row in ganttRows track by row.uid" data-gantt-row="{{row.uid}}" ng-class="{'is-selected':isGanttRowSelected(row)}">
                            <div class="pms-gantt-shade" ng-repeat="shade in ganttNonWorkingCells track by $index" ng-class="{'is-weekoff':shade.kind=='weekoff','is-holiday':shade.kind=='holiday'}" ng-style="{'left':(shade.index*ganttCellWidth)+'px','width':ganttCellWidth+'px'}"></div>
                            <div class="pms-gantt-bar" ng-if="row.start && row.end" ng-class="{'is-subtask':row.type=='subtask'}" ng-style="getGanttDisplayBarStyle(row)" title="{{row.name}} | {{getGanttDurationText(row)}}" ng-click="openGanttSubtaskPopup(row,$event)">
                                <span class="pms-gantt-resize left" ng-if="canEdit(row.type)" ng-mousedown="startGanttBarResize($event,row,'left')" ng-click="suppressGanttResizeClick($event)"></span><span class="pms-gantt-bar-label">{{getGanttDurationText(row)}}</span><span class="pms-gantt-resize right" ng-if="canEdit(row.type)" ng-mousedown="startGanttBarResize($event,row,'right')" ng-click="suppressGanttResizeClick($event)"></span>
                            </div>
                        </div>
                    </div>
                    <div class="pms-gantt-empty" ng-if="!ganttIsLoading && !ganttRows.length">No records found.</div>
                    
                </section>
            </div>
        </div>
        <div class="pms-gantt-detail" ng-class="{'pms-gantt-detail-modern':ganttDependencyView=='modern'}" ng-show="ganttDetailOpen && ganttDetailRow" ng-click="$event.stopPropagation()">
                        <div class="pms-gantt-detail-classic" ng-if="ganttDependencyView!='modern'">
                        <button type="button" class="pms-gantt-detail-close" ng-click="closeGanttSubtaskPopup($event)" aria-label="Close">×</button>
                        <div class="pms-gantt-detail-top">
                            <div class="pms-gantt-detail-progress">{{ganttDetailRow.progress || 0}}%</div>
                            <div class="pms-gantt-detail-main">
                                <div class="pms-gantt-detail-badges"><span class="pms-gantt-type-badge">{{ganttDetailRow.type}}</span><span class="pms-gantt-code-badge">{{ganttDetailRow.serialNo}}</span></div>
                                <div class="pms-gantt-detail-title">{{ganttDetailRow.name}}</div>
                                <div class="pms-gantt-detail-meta"><span class="pms-gantt-detail-status" ng-style="{'color':'black','background-color':ganttDetailRow.status_color || '#57b66c'}">{{ganttDetailRow.status_name || 'Open'}}</span><span><i class="ri-calendar-line"></i> {{getGanttPopupDateRange(ganttDetailRow)}}</span></div>
                                <div class="pms-gantt-detail-duration">{{getGanttPopupDayText(ganttDetailRow)}} <span class="pms-gantt-detail-owner"><i class="ri-user-line"></i> {{getGanttOwnerNames(ganttDetailRow) || 'Unassigned'}}</span></div>
                            </div>
                        </div>
                        <div class="pms-gantt-detail-tabs"><button type="button" class="pms-gantt-detail-tab" ng-class="{'active':ganttDependencyTab=='predecessor'}" ng-click="setGanttDependencyTab('predecessor')">Predecessor{{ganttDependencyTab=='predecessor' && ganttDependencyRows.length ? ' ('+ganttDependencyRows.length+')' : ''}}</button><button type="button" class="pms-gantt-detail-tab" ng-class="{'active':ganttDependencyTab=='successor'}" ng-click="setGanttDependencyTab('successor')">Successor{{ganttDependencyTab=='successor' && ganttDependencyRows.length ? ' ('+ganttDependencyRows.length+')' : ''}}</button></div>
                        <div class="pms-gantt-dependency-grid">
                            <table><colgroup><col style="width:25px" /><col style="width:55px" /><col style="width:380px" /><col style="width:130px" /><col style="width:130px" /><col style="width:130px" /><col style="width:175px" /><col style="width:170px" /></colgroup><thead><tr><th class="pms-dependency-menu-cell"></th><th>ID</th><th>Task Name</th><th>Dependency</th><th>Lag (hours)</th><th>Link Type</th><th>Start Date</th><th>Due Date</th></tr></thead>
                                <tbody>
                                    <tr ng-repeat="link in ganttDependencyRows track by link.Id" ng-init="linked=getGanttDependencyItem(link)">
                                        <td class="pms-dependency-menu-cell"><span class="pms-dependency-row-actions"><button type="button" ng-click="toggleGanttDependencyMenu(link,$event)" title="Dependency options" aria-label="Dependency options" aria-expanded="{{ganttDependencyMenuId==link.Id}}">⋮</button></span></td><td>{{linked.serialNo || linked.id || '—'}}</td><td>{{linked.name || 'Item unavailable in current filter'}}</td>
                                        <td>{{link.LinkType}}</td><td>{{link.LagHours}}</td><td>{{ganttDependencyTab=='predecessor' ? 'Predecessor' : 'Successor'}}</td>
                                        <td>{{linked ? getGanttPopupDateRange(linked).split(' to ')[0] : '—'}}</td><td>{{linked ? getGanttPopupDateRange(linked).split(' to ')[1] || '—' : '—'}}</td>
                                    </tr>
                                    <tr ng-if="ganttDependencyBusy"><td colspan="8">Loading…</td></tr>
                                    <tr ng-if="!ganttAddingDependency"><td class="pms-dependency-menu-cell"></td><td></td><td><a href="javascript:void(0)" class="pms-gantt-add-dependency" ng-click="beginGanttDependencySearch()">Add {{ganttDependencyTab=='predecessor' ? 'Predecessor' : 'Successor'}}</a></td><td colspan="5"></td></tr>
                                    <tr ng-if="ganttAddingDependency"><td class="pms-dependency-menu-cell"></td><td></td><td class="pms-dependency-search-cell">
                                        <span class="pms-dependency-search-wrap"><input type="text" ng-model="ganttDependencyForm.search" ng-focus="showGanttDependencyChoices()" ng-change="ganttDependencySearchChanged()" ng-keydown="onGanttDependencySearchKeydown($event)" placeholder="Enter name and search" aria-label="Search predecessor or successor" autocomplete="off" role="combobox" aria-autocomplete="list" aria-expanded="{{ganttDependencyChoicesOpen}}" /><select class="pms-dependency-project" aria-label="Search in current project"><option>{{(projectlist && (projectlist.ProjectName || projectlist.projectname || projectlist.name)) || 'Current project'}}</option></select></span>
                                        <div class="pms-dependency-choices" ng-if="ganttDependencyChoicesOpen && getFilteredGanttDependencyChoices().length" ng-style="ganttDependencyChoicesStyle" role="listbox">
                                            <button type="button" ng-repeat="choice in getFilteredGanttDependencyChoices() track by choice.uid" ng-class="{'is-active':$index==ganttDependencyHighlighted}" ng-mouseenter="setGanttDependencyHighlighted($index)" ng-click="chooseGanttDependency(choice)" role="option" aria-selected="{{$index==ganttDependencyHighlighted}}">{{choice.serialNo}} &nbsp; {{choice.name}} ({{choice.type}})</button>
                                        </div>
                                    </td><td><select ng-model="ganttDependencyForm.linkType" aria-label="Dependency type"><option value="FS">FS</option><option value="SS">SS</option><option value="FF">FF</option><option value="SF">SF</option></select></td>
                                    <td><input type="number" step="1" min="-8760" max="8760" style="width:60px" ng-model="ganttDependencyForm.lagHours" aria-label="Lag in hours" title="Lag in hours; negative is lead" /></td>
                                    <td colspan="3"><button type="button" ng-click="cancelGanttDependencySearch()" aria-label="Cancel adding dependency">Cancel</button></td></tr>
                                </tbody></table>
                            <div ng-if="ganttDependencyNotice" class="pms-dependency-added" role="status">✓ {{ganttDependencyNotice}}</div>
                            <div ng-if="ganttDependencyError" role="alert" style="color:#b42318;padding:6px">{{ganttDependencyError}}</div>
                        </div>
                        <span class="pms-dependency-row-menu pms-gantt-floating-menu" ng-if="ganttDependencyMenuLink && ganttDependencyMenuId==ganttDependencyMenuLink.Id" ng-style="ganttDependencyMenuStyle"><button type="button" ng-disabled="!getGanttDependencyItem(ganttDependencyMenuLink)" ng-click="runGanttDependencyMenuAction('view')">View Details</button><button type="button" ng-disabled="ganttDependencyBusy" ng-click="runGanttDependencyMenuAction('delete')">Delete</button></span>
                        </div>

                        <div class="pms-gantt-modern-view" ng-if="ganttDependencyView=='modern'">
                            <div class="pms-gantt-modern-header"><div><div class="pms-gantt-modern-title">Task dependencies</div><div class="pms-gantt-modern-subtitle">Manage predecessor and successor links for this item</div></div><button type="button" class="pms-gantt-modern-close" ng-click="closeGanttSubtaskPopup($event)" aria-label="Close">×</button></div>
                            <div class="pms-gantt-modern-content">
                                <div class="pms-gantt-modern-item"><div><span class="pms-gantt-modern-type">{{ganttDetailRow.type}}</span><strong>{{ganttDetailRow.name}}</strong><div class="pms-gantt-modern-meta">{{ganttDetailRow.serialNo}} <span ng-if="ganttDetailRow.status_name">· {{ganttDetailRow.status_name}}</span> · {{getGanttPopupDateRange(ganttDetailRow)}}</div></div><span class="pms-gantt-modern-progress">{{ganttDetailRow.progress || 0}}%</span></div>
                                <div class="pms-gantt-modern-tabs"><button type="button" ng-class="{'active':ganttDependencyTab=='predecessor'}" ng-click="setGanttDependencyTab('predecessor')">Predecessors <span>{{ganttDependencyTab=='predecessor' ? ganttDependencyRows.length : ''}}</span></button><button type="button" ng-class="{'active':ganttDependencyTab=='successor'}" ng-click="setGanttDependencyTab('successor')">Successors <span>{{ganttDependencyTab=='successor' ? ganttDependencyRows.length : ''}}</span></button></div>
                                <div ng-if="!ganttAddingDependency">
                                    <div class="pms-gantt-modern-section-head"><strong>Linked {{ganttDependencyTab=='predecessor' ? 'predecessors' : 'successors'}}</strong><span>Link type and lag are shown for each relationship</span><button type="button" ng-click="beginGanttDependencySearch()">＋ Add</button></div>
                                    <div class="pms-gantt-modern-list">
                                    <div class="pms-gantt-modern-empty" ng-if="!ganttDependencyRows.length && !ganttDependencyBusy">No {{ganttDependencyTab=='predecessor' ? 'predecessors' : 'successors'}} linked yet.</div>
                                    <div class="pms-gantt-modern-link" ng-repeat="link in ganttDependencyRows track by link.Id" ng-init="linked=getGanttDependencyItem(link)">
                                        <button type="button" class="pms-gantt-modern-menu" ng-click="toggleGanttDependencyMenu(link,$event)" aria-label="Dependency options">⋮</button><div class="pms-gantt-modern-code">{{linked.serialNo || linked.id || '—'}}</div><div class="pms-gantt-modern-linkname"><strong>{{linked.name || 'Item unavailable in current filter'}}</strong><span>{{linked.type || 'Task'}} <span ng-if="linked.status_name">· {{linked.status_name}}</span></span></div><div class="pms-gantt-modern-relation"><span>{{link.LinkType}}</span><small>{{link.LagHours || 0}}h lag</small></div><div class="pms-gantt-modern-dates">{{linked ? getGanttPopupDateRange(linked) : '—'}}</div>
                                    </div>
                                    <div class="pms-gantt-modern-loading" ng-if="ganttDependencyBusy">Loading dependencies…</div>
                                    </div>
                                </div>
                                <div class="pms-gantt-modern-add" ng-if="ganttAddingDependency">
                                    <div class="pms-gantt-modern-add-head"><div><strong>{{ganttEditingDependency ? 'Edit' : 'Add'}} {{ganttDependencyTab=='predecessor' ? 'predecessor' : 'successor'}}</strong><span>{{ganttEditingDependency ? 'Update the link type or lag for this item' : 'Choose a task or milestone in this project'}}</span></div></div>
                                    <div class="pms-gantt-modern-add-controls">
                                        <div class="pms-gantt-modern-edit-target" ng-if="ganttEditingDependency">{{ganttEditingDependencyItem.serialNo}} · {{ganttEditingDependencyItem.name}}</div>
                                        <div class="pms-gantt-modern-search" ng-if="!ganttEditingDependency"><input type="text" ng-model="ganttDependencyForm.search" ng-focus="showGanttDependencyChoices()" ng-change="ganttDependencySearchChanged()" ng-keydown="onGanttDependencySearchKeydown($event)" placeholder="Search by name or ID…" aria-label="Search predecessor or successor" autocomplete="off" role="combobox" aria-autocomplete="list" aria-expanded="{{ganttDependencyChoicesOpen}}" /><span>⌕</span></div>
                                        <select class="pms-gantt-modern-select" ng-if="!ganttEditingDependency" aria-label="Search in current project"><option>{{(projectlist && (projectlist.ProjectName || projectlist.projectname || projectlist.name)) || 'Current project'}}</option></select>
                                        <select class="pms-gantt-modern-select pms-gantt-modern-linktype" ng-model="ganttDependencyForm.linkType" aria-label="Dependency type"><option value="FS">FS</option><option value="SS">SS</option><option value="FF">FF</option><option value="SF">SF</option></select>
                                        <input class="pms-gantt-modern-lag" type="number" step="1" min="-8760" max="8760" ng-model="ganttDependencyForm.lagHours" aria-label="Lag in hours" title="Lag in hours; negative is lead" />
                                        <button type="button" class="pms-gantt-modern-save" ng-disabled="ganttDependencyBusy || (!ganttEditingDependency && !ganttModernDependencyChoice)" ng-click="saveModernGanttDependency()">{{ganttEditingDependency ? 'Save changes' : 'Add link'}}</button><button type="button" class="pms-gantt-modern-cancel" ng-click="cancelGanttDependencySearch()">Cancel</button>
                                    </div>
                                </div>
                                <div ng-if="ganttDependencyNotice" class="pms-dependency-added" role="status">✓ {{ganttDependencyNotice}}</div><div ng-if="ganttDependencyError" role="alert" class="pms-gantt-modern-error">{{ganttDependencyError}}</div>
                                <div class="pms-gantt-modern-footer"><span>Links are scoped to this selected item.</span></div>
                            </div>
                            
                        </div>
                    </div>
                <span class="pms-gantt-modern-menu-portal pms-dependency-row-menu pms-gantt-floating-menu" ng-if="ganttDetailOpen && ganttDependencyView=='modern' && ganttDependencyMenuLink && ganttDependencyMenuId==ganttDependencyMenuLink.Id" ng-style="ganttDependencyMenuStyle"><button type="button" ng-disabled="ganttDependencyBusy" ng-click="runGanttDependencyMenuAction('edit')">Edit</button><button type="button" ng-disabled="ganttDependencyBusy" ng-click="runGanttDependencyMenuAction('delete')">Delete</button></span>
        <div class="pms-gantt-modern-choices-portal" ng-if="ganttDetailOpen && ganttDependencyView=='modern' && ganttAddingDependency && ganttDependencyChoicesOpen && getFilteredGanttDependencyChoices().length" ng-style="ganttDependencyChoicesStyle" role="listbox" aria-label="Dependency choices"><button type="button" ng-repeat="choice in getFilteredGanttDependencyChoices() track by choice.uid" ng-class="{'is-active':$index==ganttDependencyHighlighted}" ng-mouseenter="setGanttDependencyHighlighted($index)" ng-click="chooseModernGanttDependency(choice)" role="option" aria-selected="{{$index==ganttDependencyHighlighted}}"><span class="pms-gantt-modern-choice-code">{{choice.serialNo}}</span><span class="pms-gantt-modern-choice-name">{{choice.name}}</span><span class="pms-gantt-modern-choice-type">{{choice.type}}</span></button></div>
        <div class="wrapper_inner p-0" ng-show="view=='kanban'">
            <style>
                .pms-kanban-board{display:flex;gap:14px;padding:15px;overflow-x:auto;align-items:flex-start;min-height:480px;background:#f1f5f7}
                .pms-kanban-column{flex:0 0 285px;min-width:285px}
                /*.pms-kanban-heading{display:flex;align-items:center;justify-content:space-between;margin-bottom:8px;padding:0 3px;font-weight:600}*/
                .pms-kanban-heading{display:flex;align-items:center;justify-content:space-between;padding:10px;font-weight:600;border:1px solid #e7eef1;background:#fff;}
                .pms-kanban-count{display:inline-flex;align-items:center;justify-content:center;min-width:22px;height:22px;padding:0 6px;border-radius:11px;background:#fff;border:1px solid #e3e7ec;font-size:11px;color:#555}
                .pms-kanban-body{min-height:410px;max-height:calc(100vh - 270px);overflow-y:auto;padding:3px}
                .pms-kanban-card{background:#fff;border:1px solid #e5e8ec;border-left:4px solid #3978de;margin-bottom:10px;box-shadow:0 2px 6px rgba(0,0,0,.06);cursor:pointer}
                .pms-kanban-card .card-body{padding:10px}
                .pms-kanban-id{color:#3978de;font-size:12px;font-weight:600}
                .pms-kanban-type{text-transform:capitalize;font-size:10px;color:#77808a;background:#f0f3f6;padding:2px 6px;border-radius:9px}
                .pms-kanban-title{font-size:14px;font-weight:600;color:#20252b;margin:7px 0 3px;word-break:break-word}
                .pms-kanban-desc{font-size:12px;color:#5e6872;display:-webkit-box;-webkit-line-clamp:2;-webkit-box-orient:vertical;overflow:hidden;min-height:18px}
                .pms-kanban-parent{font-size:11px;color:#8a929b;margin-top:6px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
                .pms-kanban-footer{display:flex;align-items:center;justify-content:space-between;padding:6px 9px;border-top:1px solid #edf0f2;font-size:11px}
                .pms-kanban-meta{display:flex;align-items:center;gap:5px}
                .pms-kanban-meta .sub-soft-btn{width:26px;height:26px;min-width:26px;padding:0;display:inline-flex;align-items:center;justify-content:center}
                .pms-kanban-priority{font-size:10px;padding:2px 7px;border-radius:3px;color:#222}
                .pms-kanban-status{font-size:10px;padding:2px 7px;border-radius:3px;color:#222}
                .pms-kanban-inline-editor{display:inline-flex}
                .pms-kanban-inline-editor .pms-kanban-editable{display:inline-flex;align-items:center;gap:2px;text-decoration:none;cursor:pointer;border:1px solid rgba(0,0,0,.08)}
                .pms-kanban-inline-editor .dropdown-menu{min-width:165px;max-height:220px;overflow-y:auto}
                .pms-kanban-inline-editor .dropdown-item{font-size:12px;display:flex;align-items:center;justify-content:space-between;gap:8px;padding:6px 10px}
                .pms-kanban-inline-editor .dropdown-item:hover{background:#f3f6f9}
                .pms-kanban-inline-editor .pms-kanban-selected{font-weight:600;background:#f7f9fb}
                .pms-kanban-meta-action{display:inline-flex;align-items:center;gap:5px;max-width:135px;padding:4px 7px;border:1px solid #e5e9ee;border-radius:14px;background:#f8fafc;color:#56606a;text-decoration:none;cursor:pointer;white-space:nowrap;overflow:hidden}
                .pms-kanban-meta-action:hover{background:#eef4fb;color:#2764ae}
                .pms-kanban-meta-action span{overflow:hidden;text-overflow:ellipsis}
                .pms-kanban-meta-action.is-static{cursor:default}
                .pms-kanban-duration-action{color:#dc3545;background:#fff8f8}
                .pms-kanban-popup-list{max-height:245px;overflow-y:auto;border:1px solid #e1e5ea;border-radius:4px;background:#fff}
                .pms-kanban-popup-owner{display:flex;align-items:center;justify-content:space-between;gap:10px;padding:8px 10px;border-bottom:1px solid #f0f2f4;cursor:pointer}
                .pms-kanban-popup-owner:last-child{border-bottom:0}
                .pms-kanban-popup-owner:hover{background:#f5f8fb}
                .pms-kanban-popup-owner.selected-owner{background:#eaf3ff;color:#185fa9;font-weight:600}
                .pms-kanban-owner-avatar{width:28px;height:28px;border-radius:50%;display:inline-flex;align-items:center;justify-content:center;background:#edf1f5;font-size:11px;text-transform:uppercase;flex:0 0 28px;overflow:hidden}
                .pms-kanban-owner-avatar img{width:100%;height:100%;object-fit:cover}
                .pms-kanban-empty{padding:18px 8px;text-align:center;color:#9aa1a8;font-size:12px;background:rgba(255,255,255,.55);border:1px dashed #dce1e5}
                /* Compact Timeline / Allotted Time / Spent Time bars shown at the bottom of every Kanban card. */
                .pms-kanban-progress-section{padding:8px 10px 9px;border-top:1px solid #edf0f2;background:#fff}
                .pms-kanban-progress-row{display:grid;grid-template-columns:58px minmax(0,1fr) 62px;align-items:center;gap:7px;min-height:19px}
                .pms-kanban-progress-row + .pms-kanban-progress-row{margin-top:3px}
                .pms-kanban-progress-label{font-size:11px;font-weight:600;color:#434a52;white-space:nowrap}
                .pms-kanban-progress-track{display:block!important;position:relative!important;height:10px;overflow:hidden;border-radius:6px;background:#e3e5e8;box-shadow:inset 0 1px 2px rgba(0,0,0,.08)}
                .pms-kanban-progress-track > .pms-kanban-progress-fill{display:block!important;position:absolute!important;top:0!important;bottom:0!important;left:0!important;right:auto!important;margin:0!important;transform:none!important;transform-origin:left center!important;height:100%;min-width:0;border-radius:inherit;transition:width .2s ease}
                .pms-kanban-progress-fill.timeline{background:#59a84f}
                .pms-kanban-progress-fill.allotted{background:#59a84f}
                .pms-kanban-progress-fill.spent{background:#59a84f}
                .pms-kanban-progress-value{font-size:11px;color:#626a73;text-align:right;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}
                /* Compact table Timeline bar. Reuses the existing Kanban date calculation and tooltip. */
                .pms-table-timeline{display:grid;grid-template-columns:minmax(90px,1fr) 38px;align-items:center;gap:7px;min-width:135px;width:100%}
                .pms-table-timeline .pms-kanban-progress-track{width:100%}
                .pms-table-timeline-value{font-size:11px;color:#626a73;text-align:right;white-space:nowrap}
            </style>
            <style>

            </style>
            <div class="pms-kanban-board" id="kanbanboard">
                <div class="pms-kanban-column"
                     ng-repeat="status in statuslists track by status.id"
                     ng-if="topStatusFilter == 'all' || topStatusFilter == status.id">
                    <%--<div class="pms-kanban-heading" ng-style="{'color': status.colorname}">--%>
                    <div class="pms-kanban-heading" >
                        <span>{{status.name}}</span>
                        <span class="pms-kanban-count" ng-style="{'color': status.colorname}">{{getKanbanCount(status)}}</span>
                    </div>
                    <div class="tasks-list" id="kanban_{{status.id}}">
                    <div class="pms-kanban-body">
                        <div class="card pms-kanban-card"
                             ng-repeat="item in getKanbanItemsByStatus(status) track by item.tasktype + '-' + item.id"
                             ng-style="{'border-left-color': getKanbanStatusColor(item,status)}"
                             ng-click="openKanbanItem(item)">
                            <div class="card-body">
                                <div class="d-flex align-items-center justify-content-between gap-2">
                                    <div class="d-flex align-items-center gap-1">
                                        <span class="pms-kanban-id">{{item.serialNo}}</span>
                                        <%--<span class="pms-kanban-type">{{item.tasktype}}</span>--%>
                                    </div>
                                    <div class="d-flex align-items-center gap-1">
                                        <!-- Priority: editable only when the current item type has edit permission. -->
                                        <div class="dropdown pms-kanban-inline-editor"
                                             ng-if="canEdit(item.tasktype)"
                                             ng-click="$event.stopPropagation()">
                                            <a href="javascript:void(0);"
                                               class="pms-kanban-priority pms-kanban-editable"
                                               data-bs-toggle="dropdown"
                                               data-bs-auto-close="true"
                                               data-bs-boundary="window"
                                               aria-expanded="false"
                                               title="Edit priority"
                                               ng-style="{'background-color':getKanbanPriorityColor(item)}">
                                                <span>{{getKanbanPriorityName(item) || ''}}</span>
                                                <i class="ri-arrow-down-s-line"></i>
                                            </a>
                                            <%--Set Priority--%>
                                            <div class="dropdown-menu dropdown-menu-end p-0">
                                                <a href="javascript:void(0);"
                                                   class="dropdown-item"
                                                   ng-repeat="newpriority in prioritylists track by newpriority.id"
                                                   ng-class="{'pms-kanban-selected': getKanbanPriorityId(item) == newpriority.id}"
                                                   ng-click="selectKanbanPriority(item,newpriority,$event)">
                                                    <span><i class="ri-checkbox-blank-fill me-1" ng-style="{'color':newpriority.colorname}"></i>{{newpriority.name}}</span>
                                                    <i class="ri-check-line" ng-if="getKanbanPriorityId(item) == newpriority.id"></i>
                                                </a>
                                            </div>
                                        </div>
                                        <span class="pms-kanban-priority"
                                              ng-if="!canEdit(item.tasktype) && getKanbanPriorityName(item)"
                                              bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                              data-bs-title="Priority Name"
                                              ng-style="{'background-color':getKanbanPriorityColor(item)}">
                                            {{getKanbanPriorityName(item)}}
                                        </span>

                                        <!-- Status: the existing status API still performs completion/dependency validation. -->
                                        <div style="display:none;" class="dropdown pms-kanban-inline-editor"
                                             ng-if="canEdit(item.tasktype)"
                                             ng-click="$event.stopPropagation()">
                                            <a href="javascript:void(0);"
                                               class="pms-kanban-status pms-kanban-editable"
                                               data-bs-toggle="dropdown"
                                               data-bs-auto-close="true"
                                               data-bs-boundary="window"
                                               aria-expanded="false"
                                               title="Edit status"
                                               ng-style="{'background-color':getKanbanStatusColor(item)}">
                                                <span>{{getKanbanStatusName(item) || 'Set Status'}}</span>
                                                <i class="ri-arrow-down-s-line"></i>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end p-0">
                                                <a href="javascript:void(0);"
                                                   class="dropdown-item"
                                                   ng-repeat="newstatus in statuslists track by newstatus.id"
                                                   ng-class="{'pms-kanban-selected': getKanbanStatusId(item) == newstatus.id}"
                                                   ng-click="selectKanbanStatus(item,newstatus,$event)">
                                                    <span><i class="bx bxs-circle me-1" ng-style="{'color':newstatus.colorname}"></i>{{newstatus.name}}</span>
                                                    <i class="ri-check-line" ng-if="getKanbanStatusId(item) == newstatus.id"></i>
                                                </a>
                                            </div>
                                        </div>
                                        <span class="pms-kanban-status"
                                              ng-if="!canEdit(item.tasktype) && getKanbanStatusName(item)"
                                              bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                              data-bs-title="Status Name"
                                              ng-style="{'background-color':getKanbanStatusColor(item)}">
                                            {{getKanbanStatusName(item)}}
                                        </span>

                                        <a href="javascript:void(0);"
                                           class="click-link"
                                           ng-if="canAssign(item.tasktype)"
                                            data-bs-toggle="modal" data-bs-target="#assignedtask"
                                           bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                              data-bs-title="Assign {{item.tasktype}}"
                                           ng-click="openKanbanAssignTask(item, $index, $event)">
                                            <i class="ri-user-add-line"></i>
                                        </a>

                                        <div class="btn-group d-block" ng-click="$event.stopPropagation()">
                                            <a href="javascript:void(0);"
                                               type="button"
                                               data-bs-toggle="dropdown"
                                               data-bs-auto-close="true"
                                               data-bs-boundary="window"
                                               class="click-link"
                                               aria-expanded="false"
                                               title="More actions">
                                                <i class="ri-more-2-fill"></i>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-end p-0" style="border-radius:0;">
                                                <ul class="sttus-list">
                                                    <li>
                                                        <a href="javascript:void(0);"
                                                           ng-click="openKanbanDependency(item,$event)">
                                                            <i class="ri-parent-line fs-16 align-middle me-1"></i> Dependency
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="javascript:void(0);"
                                                           ng-click="openKanbanIssue(item,$event)">
                                                            <i class="bx bx-error-circle fs-16 align-middle me-1"></i> Add Issue
                                                        </a>
                                                    </li>
                                                    <li ng-if="canEdit(item.tasktype)">
                                                        <a href="javascript:void(0);"
                                                           data-bs-toggle="offcanvas"
                                                           data-bs-target="#addfiles"
                                                           ng-click="openKanbanFiles(item,$event)">
                                                            <i class="bx bx-upload fs-16 align-middle me-1"></i> Upload Documents
                                                        </a>
                                                    </li>
                                                    <li ng-if="canEdit(item.tasktype)">
                                                        <a href="javascript:void(0);"
                                                           data-bs-toggle="offcanvas"
                                                           data-bs-target="#addmile"
                                                           ng-click="editKanbanItem(item,$event)">
                                                            <i class="bx bx-edit fs-16 align-middle me-1"></i> Edit {{item.tasktype}}
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="javascript:void(0);" ng-click="cloneKanbanItem(item,$event)">
                                                            <i class="bx bx-copy fs-16 align-middle me-1"></i> Clone
                                                        </a>
                                                    </li>
                                                   <%-- <li>
                                                        <a href="javascript:void(0);" class="text-muted" title="No existing action is connected to Change Color">
                                                            <i class="bx bx-palette fs-16 align-middle me-1"></i> Change Color
                                                        </a>
                                                    </li>--%>
                                                   <%-- <li>
                                                        <a href="javascript:void(0);" class="text-muted" title="No existing action is connected to Hold">
                                                            <i class="bx bx-archive fs-16 align-middle me-1"></i> Hold
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <a class="text-danger" href="javascript:void(0);" ng-click="deleteKanbanItem(item,$event)">
                                                            <i class="bx bx-trash fs-16 align-middle me-1"></i> Delete
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="pms-kanban-title">{{getKanbanItemTitle(item)}}</div>
                                <div class="pms-kanban-desc" ng-if="getKanbanDescription(item)"
                                    bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="{{getKanbanDescription(item)}}">{{ getKanbanDescription(item) | limitTo:40 }}
    <span ng-if="getKanbanDescription(item).length >40">..</span></div>
                                
                                <div class="pms-kanban-parent" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="{{item.tasktype}}">
                                    {{item.milestoneName}}
                                    <span ng-if="item.tasktype=='subtask' && item.taskName"> / {{item.taskName}}</span>
                                </div>
                            </div>

                            <!-- Timeline / AT / ST summary. Existing Kanban footer actions remain unchanged below. -->
                            <div class="pms-kanban-progress-section" ng-click="$event.stopPropagation()">
                                <div class="pms-kanban-progress-row"
                                     bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                     data-bs-title="{{getKanbanTimelineTooltip(item)}}">
                                    <span class="pms-kanban-progress-label">Timeline</span>
                                    <div class="pms-kanban-progress-track">
                                        <div class="pms-kanban-progress-fill timeline"
                                             ng-style="{'width': getKanbanTimelinePercent(item) + '%'}"></div>
                                    </div>
                                    <span class="pms-kanban-progress-value">{{getKanbanTimelinePercent(item)}}%</span>
                                </div>

                                <div class="pms-kanban-progress-row"
                                     bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                     data-bs-title="{{getKanbanAllottedTooltip(item)}}">
                                    <span class="pms-kanban-progress-label">AT</span>
                                    <div class="pms-kanban-progress-track">
                                        <div class="pms-kanban-progress-fill allotted"
                                             ng-style="{'width': getKanbanAllottedPercent(item) + '%'}"></div>
                                    </div>
                                    <span class="pms-kanban-progress-value">{{getKanbanAllottedPercent(item)}}%</span>
                                </div>

                                <div class="pms-kanban-progress-row"
                                     bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                     data-bs-title="{{getKanbanSpentTooltip(item)}}">
                                    <span class="pms-kanban-progress-label">ST</span>
                                    <div class="pms-kanban-progress-track">
                                        <div class="pms-kanban-progress-fill spent"
                                             ng-style="{'width': getKanbanSpentPercent(item) + '%'}"></div>
                                    </div>
                                    <span class="pms-kanban-progress-value">{{getKanbanDuration_spent(item)}}</span>
                                </div>
                            </div>

                           <div class="pms-kanban-footer">
                                <div class="pms-kanban-meta">
                                     <span class="sub-soft-btn rounded-circle"
                                          ng-if="getKanbanOwnerName(item)"
                                         ng-click="openKanbanOwnerPopup(item,$event)"
                                          bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="Owner Name :{{getKanbanOwnerName(item)}}">
                                        <span class="pms-person-avatar" pms-person-avatar person-name="{{getKanbanOwnerName(item)}}" person-photo="{{getOwnerProfilenew(getKanbanOwnerId(item))}}"></span>

                                    </span>
                                    <%--<a href="javascript:void(0);"
                                       class="pms-kanban-meta-action"
                                       ng-if="canEdit(item.tasktype)"
                                       title="Change owner"
                                       ng-click="openKanbanOwnerPopup(item,$event)">
                                        <i class="ri-user-line"></i>
                                        <span>{{getKanbanOwnerName(item) || 'Set Owner'}}</span>
                                    </a>
                                    <span class="pms-kanban-meta-action is-static"
                                          ng-if="!canEdit(item.tasktype)">
                                        <i class="ri-user-line"></i>
                                        <span>{{getKanbanOwnerName(item) || 'No Owner'}}</span>
                                    </span>--%>

                                  <%--  <span class="sub-soft-btn rounded-circle"
                                          ng-if="getKanbanStartDate(item) || getKanbanEndDate(item)"
                                          bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                          data-bs-title="{{formatDateOnly(getKanbanStartDate(item))}} - {{formatDateOnly(getKanbanEndDate(item))}}">
                                        <i class="bx bx-calendar-week"></i>
                                    </span>--%>
                                   <%--<span class="sub-soft-btn rounded-circle"
                                          ng-if="getKanbanDuration(item)"
                                          ng-click="openKanbanDurationPopup(item,$event)"
                                          bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="Duration Time: {{getKanbanDuration(item)}}">
                                        <i class="bx bx-time"></i>
                                    </span>--%>
                                    <a href="javascript:void(0);"
                                       class="sub-soft-btn rounded-circle position-relative"
                                       data-bs-toggle="offcanvas"
                                       data-bs-target="#addfiles"
                                       ng-click="openKanbanFiles(item,$event)">
                                        <span bs-tooltip
                                              data-bs-toggle="tooltip"
                                              data-bs-placement="top"
                                              data-bs-title="Upload {{item.tasktype}} Documents">
                                            <i class="ri-file-text-line"></i>
                                        </span>
                                        <span class="files_count">{{item.source.viewfiles.length || 0}}</span>
                                    </a>
                                     <span class="sub-soft-btn rounded-circle"
                                          ng-if="getKanbanAssignName(item)"
                                          bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                           data-bs-title="Assign To :{{getKanbanAssignName(item)}}">

                                         <i class="ri-user-line" ng-if="getKanbanassignto(item)==0"></i>
                                        <%-- <div class="avatar-group" ng-if="getKanbanassignto(item)!=0">
                                        <a href="javascript: void(0);" class="avatar-group-item material-shadow" data-bs-toggle="tooltip" title="{{getOwnerName(c.assignto)}}">
                                            <div class="avatar-xxs">--%>
                                                <span class="pms-person-avatar" ng-if="getKanbanassignto(item)!=0" pms-person-avatar person-name="{{getKanbanAssignName(item)}}" person-photo="{{getOwnerProfilenew(getKanbanassignto(item))}}"></span>
                                            <%--</div>
                                        </a>                                        
                                    </div>--%>
                                         
                                    </span>
                                </div>
                               <%--<div class="text-danger d-flex align-items-center gap-1" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top"
                                          data-bs-title="Spent Time">
                                <i class="bx bx-stopwatch" style="font-size:18px;margin-top: 2px;"></i>
                                <div class="duration-box">
                                   <span ng-if="getKanbanDuration(item)">{{getKanbanDuration_spent(item)}}</span>
                              
                                   </div>
                                   </div>--%>
                                <%--<a href="javascript:void(0);"
                                   class="pms-kanban-meta-action pms-kanban-duration-action"
                                   ng-if="canEdit(item.tasktype)"
                                   title="Change duration"
                                   ng-click="openKanbanDurationPopup(item,$event)">
                                    <i class="bx bx-stopwatch"></i>
                                    <span>{{getKanbanDuration(item) || 'Set Duration'}}</span>
                                </a>
                                <span class="pms-kanban-meta-action pms-kanban-duration-action is-static"
                                      ng-if="!canEdit(item.tasktype)">
                                    <i class="bx bx-stopwatch"></i>
                                    <span>{{getKanbanDuration(item) || 'No Duration'}}</span>
                                </span>--%>

                           </div>
                        </div>

                        <div class="pms-kanban-empty" ng-if="getKanbanCount(status)==0">
                            No {{activetype=='1' ? 'unassigned' : (activetype=='2' ? 'assigned' : '')}} items
                        </div>
                    </div>
                        </div>
                </div>
            </div>
        </div>

        <!-- Kanban single-owner popup -->
        <div class="modal fade" id="kanbanOwnerModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm">
                <div class="modal-content">
                    <div class="modal-header p-3 pt-2 pb-2 bg-light">
                        <h5 class="modal-title">Update Owner</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" ng-disabled="kanbanOwnerEdit.saving"></button>
                    </div>
                    <div class="modal-body p-3">
                        <div class="mb-2 text-muted small">
                            Select one owner for <strong>{{getKanbanItemTitle(kanbanOwnerEdit.wrapper)}}</strong>
                        </div>
                        <input type="text"
                               class="form-control form-control-sm mb-2"
                               ng-model="kanbanOwnerEdit.searchText"
                               placeholder="Search owner name..."
                               ng-disabled="kanbanOwnerEdit.saving" />

                        <div class="pms-kanban-popup-list">
                            <div class="pms-kanban-popup-owner"
                                 ng-repeat="owner in owners | filter:kanbanOwnerEdit.searchText track by owner.id"
                                 ng-class="{'selected-owner': kanbanOwnerEdit.selectedOwnerId == owner.id}"
                                 ng-click="selectKanbanPopupOwner(owner,$event)">
                                <div class="d-flex align-items-center gap-2 overflow-hidden">
                                    <span class="pms-kanban-owner-avatar">
                                        <img ng-if="owner.profileimage" ng-src="{{owner.profileimage}}" alt="" />
                                        <span ng-if="!owner.profileimage">{{owner.shortname || (owner.name | limitTo:2)}}</span>
                                    </span>
                                    <span class="text-truncate">{{owner.name}}</span>
                                </div>
                                <i class="ri-radio-button-line" ng-if="kanbanOwnerEdit.selectedOwnerId != owner.id"></i>
                                <i class="ri-checkbox-circle-fill text-primary" ng-if="kanbanOwnerEdit.selectedOwnerId == owner.id"></i>
                            </div>
                            <div class="p-3 text-center text-muted"
                                 ng-if="(owners | filter:kanbanOwnerEdit.searchText).length == 0">
                                No owner found
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer p-2">
                        <button type="button" class="btn btn-light btn-sm" data-bs-dismiss="modal" ng-disabled="kanbanOwnerEdit.saving">Cancel</button>
                        <button type="button"
                                class="btn btn-primary btn-sm"
                                ng-click="saveKanbanOwner()"
                                ng-disabled="kanbanOwnerEdit.saving || !kanbanOwnerEdit.selectedOwnerId">
                            <span ng-if="!kanbanOwnerEdit.saving"><i class="ri-save-line me-1"></i>Save Owner</span>
                            <span ng-if="kanbanOwnerEdit.saving"><span class="spinner-border spinner-border-sm me-1"></span>Saving...</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Kanban duration popup -->
        <div class="modal fade" id="kanbanDurationModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm">
                <div class="modal-content">
                    <div class="modal-header p-3 pt-2 pb-2 bg-light">
                        <h5 class="modal-title">Update Duration</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" ng-disabled="kanbanDurationEdit.saving"></button>
                    </div>
                    <div class="modal-body p-3">
                        <div class="mb-3 text-muted small">
                            Update duration for <strong>{{getKanbanItemTitle(kanbanDurationEdit.wrapper)}} </strong>
                        </div>
                        
                        <div class="row g-2">
                            <div class="col-6">
                                <label class="form-label mb-1">Hours</label>
                                
                                <input type="number"
                                       min="0"
                                       step="1"
                                       class="form-control"
                                       ng-model="kanbanDurationEdit.hours"
                                       ng-disabled="kanbanDurationEdit.saving"
                                       ng-keydown="$event.keyCode == 13 && saveKanbanDuration()" />
                            </div>
                            <div class="col-6">
                                <label class="form-label mb-1">Minutes</label>
                                <input type="number"
                                       min="0"
                                       max="59"
                                       step="1"
                                       class="form-control"
                                       ng-model="kanbanDurationEdit.minutes"
                                       ng-disabled="kanbanDurationEdit.saving"
                                       ng-keydown="$event.keyCode == 13 && saveKanbanDuration()" />
                            </div>
                        </div>
                        <div class="form-text mt-2">Minutes must be between 0 and 59.</div>
                    </div>
                    <div class="modal-footer p-2">
                        <button type="button" class="btn btn-light btn-sm" data-bs-dismiss="modal" ng-disabled="kanbanDurationEdit.saving">Cancel</button>
                        <button type="button"
                                class="btn btn-primary btn-sm"
                                ng-click="saveKanbanDuration()"
                                ng-disabled="kanbanDurationEdit.saving">
                            <span ng-if="!kanbanDurationEdit.saving"><i class="ri-save-line me-1"></i>Save Duration</span>
                            <span ng-if="kanbanDurationEdit.saving"><span class="spinner-border spinner-border-sm me-1"></span>Saving...</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div class="wrapper_inner p-0" ng-show="view=='timesheet'">
            <div class="pms-timesheet-view">
                <div class="pms-timesheet-range">
                    <i class="ri-calendar-line"></i>
                    <span>{{timesheetRangeLabel || 'Project timeline'}}</span>
                </div>

                <div class="pms-timesheet-app" ng-if="timesheetRows.length > 0">
                    <!-- LEFT: hierarchy details -->
                    <div class="pms-timesheet-left">
                        <div class="pms-timesheet-left-head">
                            <div>Task</div>
                            <div>Assigned to</div>
                            <div>Status</div>
                            <div>Task Completion%</div>
                        </div>

                        <div class="pms-timesheet-left-row"
                             ng-repeat="row in timesheetRows track by row.key">
                            <div class="pms-timesheet-task-title level-{{row.level}}"
                                 ng-style="{'padding-left': ((row.level * 20) + 9) + 'px'}"
                                 bs-tooltip
                                 data-bs-toggle="tooltip"
                                 data-bs-placement="top"
                                 data-bs-title="{{row.title}}">
                                {{row.numberedTitle}}
                            </div>

                            <div>
                                <div class="pms-timesheet-owners"
                                     bs-tooltip
                                     data-bs-toggle="tooltip"
                                     data-bs-placement="top"
                                     data-bs-title="{{getTimesheetOwnerTitle(row)}}">
                                    <span class="pms-timesheet-owner-avatar"
                                          ng-repeat="owner in row.owners | limitTo:3 track by $index">
                                        <img ng-if="owner.profileimage"
                                             ng-src="{{owner.profileimage}}"
                                             onerror="this.onerror=null;this.src='/pms/Image/user.jpg';"
                                             alt="{{owner.name}}" />
                                        <span ng-if="!owner.profileimage">
                                            {{owner.shortname || (owner.name | limitTo:2)}}
                                        </span>
                                    </span>
                                    <span class="pms-timesheet-no-owner" ng-if="!row.owners.length">
                                        <i class="ri-user-line"></i>
                                    </span>
                                </div>
                            </div>

                            <div>
                                <span class="pms-timesheet-status"
                                      ng-style="{'border-color':row.statusColor, 'color':black}">
                                    {{row.statusName}}
                                </span>
                            </div>

                            <div>
                                <div class="pms-timesheet-progress-box">
                                    <div class="pms-timesheet-progress-value">{{row.progress}}%</div>
                                    <div class="pms-timesheet-progress-bar">
                                        <div class="pms-timesheet-progress-fill"
                                             ng-class="{'is-complete':row.progress >= 100}"
                                             ng-style="{'width':row.progress + '%'}">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- RIGHT: date timeline -->
                    <div class="pms-timesheet-right">
                        <div ng-style="{'width':timesheetTimelineWidth + 'px'}">
                            <div class="pms-timesheet-timeline-head"
                                 ng-style="{'width':timesheetTimelineWidth + 'px'}">
                                <div class="pms-timesheet-months">
                                    <div class="pms-timesheet-month"
                                         ng-repeat="month in timesheetMonths track by month.key"
                                         ng-style="{'width':month.width}">
                                        {{month.name}}
                                    </div>
                                </div>
                                <div class="pms-timesheet-days">
                                    <span class="pms-timesheet-day"
                                          ng-repeat="day in timesheetDays track by day.key"
                                          ng-class="{'is-weekend':day.isWeekend}">
                                        {{day.label}}
                                    </span>
                                </div>
                            </div>

                            <div class="pms-timesheet-timeline-row"
                                 ng-repeat="row in timesheetRows track by row.key"
                                 ng-style="{'width':timesheetTimelineWidth + 'px'}">
                                <div class="pms-timesheet-bar"
                                     ng-if="row.hasDates"
                                     ng-style="row.barStyle"
                                     bs-tooltip
                                     data-bs-toggle="tooltip"
                                     data-bs-placement="top"
                                     data-bs-title="{{row.title}}: {{row.startDateTimeLabel}} - {{row.endLabel}} | Working Hrs: {{row.workingHoursText}} | Reset Basis: {{row.perDayHours}} hrs/day">
                                    {{row.durationDays}} days · {{row.workingHoursText}} hrs
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="pms-timesheet-empty" ng-if="timesheetRows.length == 0">
                    No milestone, task or subtask found for this view.
                </div>
            </div>
        </div>
        <div class="wrapper_inner p-0" ng-show="view=='calendar'">
            <style>
                .pms-calendar{padding:12px;background:#f6f8fb;min-height:520px}
                .pms-calendar-toolbar{display:flex;align-items:center;justify-content:space-between;margin-bottom:10px}
                .pms-calendar-title{font-size:17px;font-weight:600;color:#26313c}
                .pms-calendar-actions{display:flex;gap:5px}
                .pms-calendar-week-head,.pms-calendar-week{display:grid;grid-template-columns:repeat(7,minmax(120px,1fr))}
                .pms-calendar-week-head{background:#eef1f5;border:1px solid #dfe4e8;border-bottom:0}
                .pms-calendar-week-head div{padding:8px;text-align:center;font-size:12px;font-weight:600;color:#59636e;border-right:1px solid #dfe4e8}
                .pms-calendar-week-head div:last-child{border-right:0}
                .pms-calendar-grid{min-width:840px}
                .pms-calendar-body{display:grid;grid-template-columns:minmax(0,1fr) 440px;align-items:start;gap:0}
                .pms-calendar-scroll{overflow:auto;flex:1;min-width:0}
                .pms-calendar-day{min-height:105px;padding:5px;border-right:1px solid #e1e5e9;border-bottom:1px solid #e1e5e9;background:#fff}
                .pms-calendar-week .pms-calendar-day:first-child{border-left:1px solid #e1e5e9}
                .pms-calendar-day.other-month{background:#f7f8fa;color:#a5adb5}
                .pms-calendar-day.today{background:#fffbea;box-shadow:inset 0 0 0 1px #f1c40f}
                .pms-calendar-day.selected{background:#eef4ff;box-shadow:inset 0 0 0 2px #4b82f0}
                .pms-calendar-day-number{font-size:11px;font-weight:600;margin-bottom:4px}
                .pms-calendar-summary{width:100%;padding:5px 7px;border:0;border-left:3px solid #4b82f0;background:#eef4ff;text-align:left;cursor:pointer;font-size:10px;line-height:16px}
                .pms-calendar-summary span{display:block}
                .pms-calendar-summary .mile{color:#26313c;font-weight:600}
                .pms-calendar-summary .task{color:#2878d0}
                .pms-calendar-summary .subtask{color:#18a86b}
                .pms-calendar-panel{width:440px;background:#fff;border:1px solid #dfe4e8;overflow:hidden;min-height:650px}
                .pms-calendar-panel-head{display:flex;align-items:center;justify-content:space-between;padding:10px;border-bottom:1px solid #e6e9ec;background:#f8f9fb}
                .pms-calendar-panel-title{font-size:13px;font-weight:600;color:#26313c;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
                .pms-calendar-panel-body{font-size:11px;color:#59636e;max-height:610px;overflow:auto}
                .pms-calendar-table{width:100%;border-collapse:collapse}
                .pms-calendar-table th,.pms-calendar-table td{padding:7px 6px;border-right:1px solid #dfe4e8;border-bottom:1px solid #dfe4e8;vertical-align:top}
                .pms-calendar-table th{background:#eef3f6;color:#1f2933;font-weight:600;white-space:nowrap}
                .pms-calendar-table th:last-child,.pms-calendar-table td:last-child{border-right:0}
                .pms-calendar-row-name{font-weight:600;color:#26313c}
                .pms-calendar-row-type{font-size:9px;text-transform:capitalize;color:#7a838c}
                .pms-calendar-row-count{font-size:9px;color:#2878d0}
                .pms-calendar-status-dot{display:inline-block;width:10px;height:10px;border-radius:50%}
                .pms-calendar-no-data{text-align:center;padding:25px;color:#7a838c}
                @media(max-width:1100px){.pms-calendar-body{display:block}.pms-calendar-panel{width:100%;margin-top:10px;min-height:0}}
            </style>
            <div class="pms-calendar">
                <div class="pms-calendar-toolbar">
                    <div class="pms-calendar-title">{{calendarTitle}}</div>
                    <div class="pms-calendar-actions">
                        <button type="button" class="sub-soft-btn" ng-click="changeCalendarMonth(-1)" title="Previous month"><i class="ri-arrow-left-s-line"></i></button>
                        <button type="button" class="btn btn-sm btn-light" ng-click="goCalendarToday()">Today</button>
                        <button type="button" class="sub-soft-btn" ng-click="changeCalendarMonth(1)" title="Next month"><i class="ri-arrow-right-s-line"></i></button>
                    </div>
                </div>
                <div class="pms-calendar-body">
                <div class="pms-calendar-scroll">
                    <div class="pms-calendar-grid">
                        <div class="pms-calendar-week-head">
                            <div ng-repeat="weekDay in calendarWeekDays track by $index">{{weekDay}}</div>
                        </div>
                        <div class="pms-calendar-week" ng-repeat="week in calendarWeeks track by $index">
                            <div class="pms-calendar-day"
                                 ng-repeat="day in week track by day.key"
                                 ng-click="selectCalendarDay(day,$event)"
                                 ng-class="{'other-month':!day.isCurrentMonth,'today':day.isToday,'selected':selectedCalendarDay.key==day.key}">
                                <div class="pms-calendar-day-number">{{day.day}}</div>
                                <button type="button" class="pms-calendar-summary" ng-if="day.events.length" ng-click="selectCalendarDay(day,$event)">
                                    <span class="mile">Milestone: {{day.counts.milestone}}</span>
                                    <span class="task">Task: {{day.counts.task}}</span>
                                    <span class="subtask">Subtask: {{day.counts.subtask}}</span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <aside class="pms-calendar-panel">
                    <div class="pms-calendar-panel-head">
                        <div class="pms-calendar-panel-title">{{selectedCalendarDay.date | date:'dd MMMM yyyy'}}</div>
                        <span class="text-muted">{{selectedCalendarDay.events.length || 0}} items</span>
                    </div>
                    <div class="pms-calendar-panel-body">
                        <table class="pms-calendar-table" ng-if="selectedCalendarDay.events.length">
                            <thead>
                                <tr><th>Sr</th><th>Work Name</th><th>Start Date</th><th>End Date</th><th>Status</th></tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="event in selectedCalendarDay.events track by event.type + '_' + event.id">
                                    <td>{{$index + 1}}</td>
                                    <td>
                                        <div class="pms-calendar-row-name">{{event.title}}</div>
                                        <div class="pms-calendar-row-type">{{event.type}}</div>
                                        <div class="pms-calendar-row-count" ng-if="event.type=='milestone'">Tasks: {{event.taskCount}} | Subtasks: {{event.subtaskCount}}</div>
                                        <div class="pms-calendar-row-count" ng-if="event.type=='task'">Subtasks: {{event.subtaskCount}}</div>
                                    </td>
                                    <td>{{event.start | date:'dd/MM/yyyy'}}</td>
                                    <td>{{event.end | date:'dd/MM/yyyy'}}</td>
                                    <td title="{{event.statusName}}"><span class="pms-calendar-status-dot" ng-style="{'background-color':event.color}"></span></td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="pms-calendar-no-data" ng-if="!selectedCalendarDay.events.length">
                            No milestone, task or subtask found for this date.
                        </div>
                    </div>
                </aside>
                </div>
            </div>
        </div>
    </div>

    <style>
        .pms-hierarchy-timeline-hover {
            position: fixed;
            z-index: 1095;
            width: 300px;
            max-width: calc(100vw - 16px);
            background: #fff;
            border: 1px solid #dfe4e8;
            border-radius: 8px;
            box-shadow: 0 10px 30px rgba(26, 36, 46, .18);
            padding: 0;
            overflow: hidden;
            font-size: 12px;
        }
        .pms-hierarchy-timeline-hover__head {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
            padding: 9px 11px;
            background: #f6f8fa;
            border-bottom: 1px solid #e8ecef;
        }
        .pms-hierarchy-timeline-hover__title {
            min-width: 0;
            font-weight: 600;
            color: #25313c;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .pms-hierarchy-timeline-hover__body { padding: 9px 11px 11px; }
        .pms-hierarchy-timeline-hover__row {
            display: grid;
            grid-template-columns: 105px minmax(0, 1fr);
            gap: 10px;
            align-items: center;
            min-height: 28px;
            border-bottom: 1px dashed #edf0f2;
        }
        .pms-hierarchy-timeline-hover__row:last-child { border-bottom: 0; }
        .pms-hierarchy-timeline-hover__label { color: #6a747e; }
        .pms-hierarchy-timeline-hover__value { color: #25313c; font-weight: 500; text-align: right; }
        .pms-hierarchy-timeline-end-link {
            border: 0;
            padding: 2px 4px;
            background: transparent;
            color: #0d6efd;
            font-weight: 600;
            text-decoration: underline;
            cursor: pointer;
        }
        .pms-hierarchy-timeline-end-link:hover { color: #084298; }
        .pms-table-timeline { cursor: default; }
    </style>

    <!-- One shared clickable hover popup for milestone, task and subtask timelines. -->
    <div class="pms-hierarchy-timeline-hover"
         ng-show="hierarchyTimelineHover.visible"
         ng-style="{'top': hierarchyTimelineHover.top + 'px', 'left': hierarchyTimelineHover.left + 'px'}"
         ng-mouseenter="cancelHierarchyTimelineHide()"
         ng-mouseleave="scheduleHierarchyTimelineHide()"
         ng-click="$event.stopPropagation()">
        <div class="pms-hierarchy-timeline-hover__head">
            <div class="pms-hierarchy-timeline-hover__title">{{hierarchyTimelineHover.label}}</div>
            <button type="button" class="btn-close" aria-label="Close" ng-click="closeHierarchyTimelineHover()"></button>
        </div>
        <div class="pms-hierarchy-timeline-hover__body">
            <div class="pms-hierarchy-timeline-hover__row">
                <span class="pms-hierarchy-timeline-hover__label">Start Date</span>
                <span class="pms-hierarchy-timeline-hover__value">{{hierarchyTimelineHover.startText || '-'}}</span>
            </div>
            <div class="pms-hierarchy-timeline-hover__row">
                <span class="pms-hierarchy-timeline-hover__label">End Date</span>
                <span class="pms-hierarchy-timeline-hover__value">
                    <button type="button"
                            class="pms-hierarchy-timeline-end-link"
                            ng-if="hierarchyTimelineHover.canEdit"
                            ng-click="openHierarchyEndDatePopup($event)">
                        {{hierarchyTimelineHover.endText || '-'}}
                    </button>
                    <span ng-if="!hierarchyTimelineHover.canEdit">{{hierarchyTimelineHover.endText || '-'}}</span>
                </span>
            </div>
            <div class="pms-hierarchy-timeline-hover__row">
                <span class="pms-hierarchy-timeline-hover__label">Duration</span>
                <span class="pms-hierarchy-timeline-hover__value">{{hierarchyTimelineHover.metrics.totalDays || 0}} days</span>
            </div>
            <div class="pms-hierarchy-timeline-hover__row">
                <span class="pms-hierarchy-timeline-hover__label">Spent Days</span>
                <span class="pms-hierarchy-timeline-hover__value">{{hierarchyTimelineHover.metrics.elapsedDays || 0}}</span>
            </div>
            <div class="pms-hierarchy-timeline-hover__row">
                <span class="pms-hierarchy-timeline-hover__label">Left Days</span>
                <span class="pms-hierarchy-timeline-hover__value">{{hierarchyTimelineHover.metrics.leftDays || 0}}</span>
            </div>
           <%-- <div class="pms-hierarchy-timeline-hover__row" ng-if="hierarchyTimelineHover.metrics.startsInDays > 0">
                <span class="pms-hierarchy-timeline-hover__label">Starts In</span>
                <span class="pms-hierarchy-timeline-hover__value">{{hierarchyTimelineHover.metrics.startsInDays}} days</span>
            </div>--%>
        </div>
    </div>

    <!-- Dedicated hierarchy end-date editor; existing duration/start-date handlers are untouched. -->
    <div class="modal fade" id="hierarchyEndDateModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm">
            <div class="modal-content">
                <div class="modal-header p-3 pt-2 pb-2 bg-light">
                    <h5 class="modal-title">Update End Date</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" ng-disabled="hierarchyEndDateEdit.saving"></button>
                </div>
                <div class="modal-body p-3">
                    <div class="mb-3 small text-muted">
                        {{hierarchyEndDateEdit.label}}
                    </div>
                    <div class="row g-2 mb-3">
                        <div class="col-6">
                            <label class="form-label mb-1">Start Date</label>
                            <input type="date" class="form-control form-control-sm" ng-model="hierarchyEndDateEdit.startDate" disabled />
                        </div>
                        <div class="col-6">
                            <label class="form-label mb-1">Existing End Date</label>
                            <input type="date" class="form-control form-control-sm" ng-model="hierarchyEndDateEdit.oldEndDate" disabled />
                        </div>
                    </div>
                    <label class="form-label mb-1">New End Date</label>
                    <input type="date"
                           class="form-control"
                           ng-model="hierarchyEndDateEdit.newEndDate"
                           ng-attr-min="{{hierarchyEndDateEdit.minDate}}"
                           ng-disabled="hierarchyEndDateEdit.saving" />
                    <%--<div class="form-text">
                        Parent task, milestone and project dates are validated before saving.
                    </div>--%>
                </div>
                <div class="modal-footer p-2">
                    <button type="button" class="btn btn-light btn-sm" data-bs-dismiss="modal" ng-disabled="hierarchyEndDateEdit.saving">Cancel</button>
                    <button type="button"
                            class="btn btn-primary btn-sm"
                            ng-click="saveHierarchyEndDate()"
                            ng-disabled="hierarchyEndDateEdit.saving || !hierarchyEndDateEdit.newEndDate">
                        <span ng-if="!hierarchyEndDateEdit.saving"><i class="ri-save-line me-1"></i>Save</span>
                        <span ng-if="hierarchyEndDateEdit.saving"><span class="spinner-border spinner-border-sm me-1"></span>Saving...</span>
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="assignedtask" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-dialog-scrollable modal-dialog-centered modal-xxl">
            <div class="modal-content" style="position:relative;">
                <div class="modal-header d-flex justify-content-between align-items-center" style="border-bottom:1px solid #ddd;padding-top:10px;padding-bottom:10px;">
                    <h4 style="font-size:18px;margin:0;">Assign Task</h4>
                    <div class="d-flex align-items-center gap-3 assign-bottom-legend">
                        <div><span class="bottom-legend-box bottom-planned"></span> Planned</div>
                        <div><span class="bottom-legend-box bottom-available"></span> Available</div>
                    </div>
                    <button type="button" class="sub-soft-btn danger" data-bs-dismiss="modal" aria-label="Close"><i class="ri-close-line"></i></button>
                </div>
                <div class="modal-body p-0">
                    <div class="wrapper">
                        <!-- LEFT -->
                        <div class="user-panel">

                            <div class="user-header" style="position:relative">
                                <div class="d-flex align-items-center gap-1" >

    <i class="ri-arrow-left-s-line"
       onclick="changeDay(-1)"
       style="font-size:20px;line-height:1;cursor:pointer;"></i>
<div onclick="openCalendar()">
    <i class="ri-calendar-line"       
       style="font-size:16px;cursor:pointer;"></i>

   <%-- <span id="selectedDate"
          style="font-size:15px;cursor:pointer;">
        {{selectedDate | date:'dd/MM/yyyy'}}
    </span>--%>
     <span id="selectedDate"
          style="font-size:15px;cursor:pointer;">
        {{formatDateOnly(selectedDate)}}
    </span>

    </div>
    <i class="ri-arrow-right-s-line"
       onclick="changeDay(1)"
       style="font-size:20px;line-height:1;cursor:pointer;"></i>
<input type="date" id="datePicker">
</div>



                            </div>
                         <%-- <div id="users"></div>--%>
                            <div id="users11" style="height:400px;overflow-y:auto;overflow-x:hidden;">
                           <div class="user-row" ng-repeat="users in currenttaskteam">
    
    <img ng-if="!users.profileimage || users.profileimage == ''"   src="/pms/Image/user.jpg" />

    <img ng-if="users.profileimage && users.profileimage != ''"  ng-src="{{users.profileimage}}" onerror="this.onerror=null;this.src='/pms/Image/user.jpg';"/>

    <div>{{users.name}}</div>
    <div class="user-id">{{users.empid}}</div>

</div>
                        
                            
                        </div>
                        </div>

                        <!-- CENTER -->

                        <div class="timeline-wrapper">


                            <%--<div class="timeline-header" id="header"></div>--%>
                            <div class="timeline-header" ng-style="{width: (timelineWidth + availableTimeColWidth) + 'px'}">
                                <div class="available-time-head">Available<br />Hrs</div>
                                <div class="time-col" ng-repeat="t in hours track by $index">
    {{t.hour}}
    <span>{{t.period}}</span>
</div>
                                
                            </div>

                            <%--<div id="timeline"></div>--%>
                            <div class="timeline-row" ng-repeat="u in currenttaskteam track by u.id"  data-empid="{{u.id}}"  data-shiftstart="{{u.ShiftStart}}" data-shiftend="{{u.ShiftEnd}}" ng-class="{'shift-closed': !u.availWidth}" ng-style="{width: (timelineWidth + availableTimeColWidth) + 'px'  }">

    <div class="available-time-cell" title="{{u.AvailableTitle}}">
        <span class="available-time-pill" ng-class="{'zero': (u.AvailableMinutes || 0) <= 0}">{{u.AvailableTime || '00:00'}} Hrs</span>
    </div>
    <div class="available-strip" ng-style="{left:((availableTimeColWidth || 0) + (u.availLeft || 0)) + 'px', width:(u.availWidth || timelineWidth) + 'px'}"></div>
    <div class="grid" ng-style="{left:(availableTimeColWidth || 0) + 'px'}" style="width:auto !important">
        <div ng-repeat="g in gridColumns track by $index"></div>
    </div>
  
<%--<div class="planned-time"
     ng-repeat="t in (u.usertasks || []) track by t.id"
     ng-style="{
        left:t.tskleft + 'px',
        width:t.tskwidth + 'px'
     }">
</div>--%>
                             
     <div class="planned-time center-planned-time"
     ng-repeat="t in (u.taskSegments || []) track by $index"
     ng-if="isSegmentVisible(t.workDateKey || t.WorkDateKey || t.workDate)"
     data-taskid="{{t.taskid || t.id}}"
     data-tasktype="{{t.tasktype}}"
     data-starttime="{{t.StartTime}}"
     data-endtime="{{t.EndTime}}"
     data-duration="{{t.DurationMinutes}}"
     data-assignid="{{t.assignid || t.AssignId || t.assignmentid}}"
     data-bs-toggle="tooltip"
     data-bs-html="true"
     data-bs-placement="top"
     data-bs-trigger="hover"
     data-bs-title="{{t.taskname}}"
     ng-mouseenter="refreshCenterTaskTooltip($event, t)"
     ng-class="{'assign-no-permission': !canAssign(t.tasktype)}"
     ng-mousedown="canAssign(t.tasktype) && startCenterSegmentDrag($event, u, t, 'move')"
     ng-style="{
        left:((availableTimeColWidth || 0) + (t.left || 0)) + 'px',
        width:(t.width || 100) + 'px'
     }">

    <span class="center-resize-handle center-resize-left"
          title="Resize start time"
          ng-if="canAssign(t.tasktype)"
          ng-mousedown="startCenterSegmentDrag($event, u, t, 'resize-left')"></span>

    <span class="planned-pill">
        <span class="center-move-icon"><i class="ri-drag-move-2-line"></i></span>
        <span class="center-task-title">{{t.taskname}}</span>
        <small class="center-task-time">{{t.StartTime}} - {{t.EndTime}}</small>
        <span class="planned-close" title="Remove" ng-if="canAssign(t.tasktype)" ng-mousedown="$event.preventDefault(); $event.stopPropagation();" ng-click="removeAssignedTaskFromCenter(u, t, $event); $event.preventDefault(); $event.stopPropagation();"><i class="ri-close-line"></i></span>
    </span>

    <span class="center-resize-handle center-resize-right"
          title="Resize end time"
          ng-if="canAssign(t.tasktype)"
          ng-mousedown="startCenterSegmentDrag($event, u, t, 'resize-right')"></span>

</div>



    <%-- Existing assigned tasks are drawn by taskSegments above. Do not draw usertasks again here, otherwise text appears without correct left/width. --%>
</div>

                        </div>

                        <!-- RIGHT -->

                        <div class="task-panel">

                            <div class="task-header">
                                <div>Tasks</div>
                            </div>

                            <%--<div class="task-list" id="taskList"></div>--%>
                                <div class="task-list assign-tree-list">

    <!-- Hierarchy from projectlist.miles/current selected node. Task always appears first, then its subtasks. -->
    <div ng-repeat="mile in assignTree track by (mile.type + '_' + mile.id)" ng-if="visibleAssignNode(mile)&&!isCompletedAssignNode(mile,'milestone')">

        <div ng-class="getAssignTreeRowClass(mile)"
             class="external-task"
             data-id="{{mile.id}}" data-type="{{mile.type}}" data-title="{{mile.name}}" data-owner="{{mile.source.owner}}" data-duration="{{mile.source.TaskDuration}}" data-assigned="{{isAssignTreeNodeAssigned(mile)}}">

            <span class="assign-tree-icon"
                  ng-if="mile.children && mile.children.length"
                  ng-click="toggleAssignTreeExpand(mile, $event)">
                <i ng-class="mile.expanded ? 'ri-arrow-down-s-line' : 'ri-arrow-right-s-line'"></i>
            </span>
            <span class="assign-tree-spacer" ng-if="!mile.children || !mile.children.length"></span>

            <input type="checkbox"
                   ng-model="mile.isSelected"
                   ng-disabled="isAssignTreeNodeAssigned(mile) || !canAssign(mile.type)"
                   ng-click="$event.stopPropagation()"
                   ng-change="toggleAssignTreeSelection(mile, null, mile.isSelected)">

            
            <span class="assign-tree-title">{{mile.name}}</span>
            <span class="assign-tree-duration" ng-if="mile.source.TaskDuration">({{mile.source.TaskDuration}} hrs)</span>
            <span class="assign-tree-remove" title="Remove"
                  ng-if="canAssign(mile.type)"
                  ng-click="removeAssignTreeNode(mile, $event)"><i class="ri-close-line"></i></span>
        </div>

        <div ng-show="mile.expanded">
            <div ng-repeat="task in mile.children track by (task.type + '_' + task.id)" ng-if="visibleAssignNode(task)&&!isCompletedAssignNode(mile,'task')">

                <div ng-class="getAssignTreeRowClass(task)"
                     class="external-task"
                     data-id="{{task.id}}" data-type="{{task.type}}"  data-title="{{task.name}}"  data-owner="{{task.source.owner}}" data-duration="{{task.source.TaskDuration}}" data-assigned="{{isAssignTreeNodeAssigned(task)}}">

                    <span class="assign-tree-icon"
                          ng-if="task.children && task.children.length"
                          ng-click="toggleAssignTreeExpand(task, $event)">
                        <i ng-class="task.expanded ? 'ri-arrow-down-s-line' : 'ri-arrow-right-s-line'"></i>
                    </span>
                    <span class="assign-tree-spacer" ng-if="!task.children || !task.children.length"></span>

                    <input type="checkbox"
                           ng-model="task.isSelected"
                           ng-disabled="isAssignTreeNodeAssigned(task) || !canAssign(task.type)"
                           ng-click="$event.stopPropagation()"
                           ng-change="toggleAssignTreeSelection(task, null, task.isSelected)">

                    
                    <span class="assign-tree-title">{{task.name}}</span></span>
                    <span class="assign-tree-duration" ng-if="task.source.TaskDuration">({{task.source.TaskDuration}} hrs)</span>
                    <span class="assign-tree-remove" title="Remove"
                          ng-if="canAssign(task.type)"
                          ng-click="removeAssignTreeNode(task, $event)"><i class="ri-close-line"></i></span>
                </div>

                <div ng-show="task.expanded">
                    <div ng-repeat="sub in task.children track by (sub.type + '_' + sub.id)" ng-if="visibleAssignNode(sub)&& !isCompletedAssignNode(sub,'subtask')">
                        <div ng-class="getAssignTreeRowClass(sub)"
                             class="external-task"
                             data-id="{{sub.id}}" data-type="{{sub.type}}"   data-title="{{sub.name}}"  data-owner="{{sub.source.owner}}" data-duration="{{sub.source.TaskDuration}}" data-assigned="{{isAssignTreeNodeAssigned(sub)}}">
                            
                            <span class="assign-tree-spacer"></span>

                            <input type="checkbox"
                                   ng-model="sub.isSelected"
                                   ng-disabled="isAssignTreeNodeAssigned(sub) || !canAssign(sub.type)"
                                   ng-click="$event.stopPropagation()"
                                   ng-change="toggleAssignTreeSelection(sub, null, sub.isSelected)">

                            
                            <span class="assign-tree-title">{{sub.name}}</span>
                            <span class="assign-tree-duration" ng-if="sub.source.TaskDuration">({{sub.source.TaskDuration}} hrs)</span>
                            <span class="assign-tree-remove" title="Remove"
                                  ng-if="canAssign(sub.type)"
                                  ng-click="removeAssignTreeNode(sub, $event)"><i class="ri-close-line"></i></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Safety fallback only if hierarchy is empty -->
    <div class="task-card external-task"
         ng-repeat="c in currenttasklist track by (c.tasktype + '_' + c.id)"
         ng-if="(!assignTree || assignTree.length == 0) && !c.isAssignedTemp && !isAssignSourceAssigned(c)"
         data-id="{{c.id}}" data-type="{{c.tasktype}}" data-owner="{{c.owner}}" data-duration="{{c.TaskDuration}}">
        <input type="checkbox" ng-model="c.isSelected" ng-disabled="!canAssign(c.tasktype)" ng-click="$event.stopPropagation()" ng-change="syncSelectedAssignTasks()">
        <strong>{{c.TaskDuration}}</strong>
        <div class="task-title">{{c.Subtaskname}}</div>
        <div class="task-icons">
            <span class="text-success" ng-if="c.isAssignedTemp"><i class="bx bx-check-circle"></i></span>
            <span class="text-danger" ng-if="canAssign(c.tasktype)" ng-click="removeassigntask(c.id, c.tasktype, $index); $event.stopPropagation();" style="cursor:pointer;"><i class="ri-close-line"></i></span>
        </div>
    </div>

</div>
                            
                        </div>

                    </div>
                </div>
               <%-- <div class="modal-footer d-flex justify-content-between align-items-center" style="border-top:1px solid #ddd;padding-top:10px;padding-bottom:10px;">
                    <div class="d-flex align-items-center gap-3 assign-bottom-legend">
                        <div><span class="bottom-legend-box bottom-planned"></span> Planned</div>
                        <div><span class="bottom-legend-box bottom-available"></span> Available</div>
                    </div>
                    <button type="button" class="btn btn-primary" data-bs-dismiss="modal" aria-label="Close" id="borderedToast2Btn"><i class="ri-user-add-line"></i> Assign Task</button>
                </div>--%>
          <div ng-show="isLoading2" class="loader-overlay2">
<div class="loader2"></div>
            </div>
                </div><!-- /.modal-content -->
        </div><!-- /.modal-dialog -->
   
           </div>
    
    <div class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 1100">
        <div id="borderedToast2" class="toast toast-border-success overflow-hidden"
             role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-body border-0">
                <div class="d-flex align-items-center justify-content-between">
                    <div class="d-flex align-items-center">
                        <i class="ri-checkbox-circle-fill me-2"></i>
                        <div>
                            <h6 class="mb-0">Successfully Assigned!</h6>
                            <p class="small text-muted mb-0">5 Task assigned to 2 Users.</p>
                        </div>
                    </div>
                    <!-- Cancel button -->
                    <button type="button" class="btn-close ms-3" id="toastCancelBtn"></button>
                </div>
            </div>
            <!-- Progress bar -->
            <div class="toast-progress">
                <div class="toast-progress-bar"></div>
            </div>
        </div>
    </div>
       <div id="addowners" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
    <div class="modal-dialog modal-samll">
        <div class="modal-content" style="min-height:225px;height:auto;">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title" id="lblowners">{{ modalTitle }}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body p-3 pb-2">

    <div class="input-line">
        <div class="pms-team-search-wrap position-relative" ng-init="teamPicker={search:'',open:false}">

            <div class="tag-input">

                <span class="tag"
                      ng-repeat="owner in getSelectedOwners() track by owner.id">

                    {{owner.name}}

                    <i class="ri-close-line"
                       ng-click="removeOwner(owner.id)">
                    </i>

                </span>

                <input type="text"
                       pms-team-picker-keys class="pms-team-search-input" style="width:100%;border:0;border-bottom:1px solid #84447e;outline:none;"
                       ng-model="teamPicker.search"
                       ng-model-options="{updateOn: 'default', debounce: 0}"
                       ng-focus="teamPicker.open=true"
                       ng-change="teamPicker.open=true"
                       ng-blur="teamPicker.open=false; teamPicker.search=''"
                       ng-keydown="($event.keyCode==9 || $event.keyCode==27) && (teamPicker.open=false); $event.keyCode==9 && (teamPicker.search='')"
                       autocomplete="off"
                       placeholder="Select teams...">

            </div>

            <div class="pms-team-search-results" ng-show="teamPicker.open"
                 style="position:relative;width:100%;max-height:200px;margin-top:6px;border-radius:5px;padding:4px;overflow-y:auto;background:#fff;border:1px solid #ddd;box-shadow:0 4px 12px rgba(0,0,0,.12);z-index:1060;">

                <a href="javascript:void(0);"
                   ng-repeat="owner in owners | filter:isAvailableTeamPerson | filter:teamPicker.search"
                   class="dropdown-item pms-team-person-option" ng-class="{'pms-team-person-selected': selectedOwnerIds.includes(owner.id)}" ng-mousedown="$event.preventDefault()"
                   data-id="{{owner.id}}"
                   ng-click="toggleOwner(owner); teamPicker.search=''">

                    {{owner.name}}

                </a>

                <div ng-if="(owners | filter:isAvailableTeamPerson | filter:teamPicker.search).length == 0"
                     class="text-muted p-2">
                    No owners found
                </div>

            </div>

        </div>
    </div>

</div>
            
        </div>
    </div>
</div>
       <div id="addowners222" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
    <div class="modal-dialog modal-samll">
        <div class="modal-content" style="height:225px;">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title" id="lblowners">{{ modalTitle }}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body p-3 pb-2" >                
               <div  id="ownerDropdown" class="dropdown" style="position:relative; width:100%;" click-outside="closelist()">

    <!-- Input box showing tags -->
    <div class="input-tags" >
        <span class="tag" ng-repeat="owner in getSelectedOwners()">
            {{owner.name}} <span class="remove" ng-if="canEdit(currentFileType)" ng-click="removeOwner(owner.id); $event.stopPropagation()"><i class="ri-close-fill"></i></span>
        </span>
        
    </div>
<input type="text" class="pmsinput"
               ng-model="searchOwner"
               ng-disabled="!canEdit(currentFileType)"
               placeholder="Select owners..."
               ng-focus="showOwnerDropdown = true"
               style="width:100%;" />
    <!-- Dropdown -->
    <div class="dropdown-menu"
         ng-show="showOwnerDropdown"
         ng-click="$event.stopPropagation()"
         style="display:block; max-height:200px; overflow:auto; border:1px solid #ccc; padding:5px; background:#fff; position:absolute; width:100%; z-index:1000;">

        <!-- Search input -->
        <%--<input type="text"
               ng-model="searchOwner"
               placeholder="Search..."
             ng-keydown="handleOwnerKeydown($event)" ng-focus="openOwnerDropdown()" 
           placeholder="Search owners..."
               style="border:0px; width:100%; margin-bottom:5px; padding:3px;" />--%>

        <!-- Owner list -->
        <div ng-repeat="owner in owners | filter:searchOwner"
             ng-click="toggleOwner(owner)"
             ng-class="{'selected-owner': selectedOwnerIds.includes(owner.id), 'pms-no-edit': !canEdit(currentFileType)}"
             style="padding:3px; cursor:pointer;">
            {{owner.name}}
        </div>


        <div ng-if="(owners | filter:searchOwner).length == 0" style="padding:3px; color:#888;">
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
.dropdown-menu div:hover {background-color: #e0e0e0;}

                </style>

            </div>
        </div>
    </div>
</div>

 <div id="addcopy" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" role="dialog">
    <div class="modal-dialog modal-samll" style="height:150px;width:300px!important">
        <div class="modal-content">
            <div class="modal-header p-3 pt-1 pb-1 bg-light" style="border-bottom:1px solid #ccc;">
                <h5 class="modal-title">Copy From</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" id="btnclosecopy"></button>
            </div>
            <div class="modal-body p-3 pb-2" >                
               <div class="dropdown w-100 ">
                   <button class="btn btn-sm dropdown-toggle w-100 text-start no-caret"
        type="button"
        data-bs-toggle="dropdown">

    {{ selectedprojectname || 'Select Project' }}

</button>
<div class="dropdown-menu p-2" style="width:260px;">

    <!-- SEARCH -->
    <input type="text"
           class="form-control mb-2"
           placeholder="Search..."
           ng-model="searchText">

    <!-- LIST -->
    <div style="max-height:200px; overflow:auto;">

        <a href="javascript:void(0);"
           class="dropdown-item"
           ng-repeat="p in projectmasterlist | filter:searchText"
           ng-click="selectProject(p)">

            {{p.ProjectName}}

        </a>

        <!-- NO RESULT -->
        <div class="text-muted p-2"
             ng-if="(projectmasterlist | filter:searchText).length == 0">
            No results found
        </div>

    </div>

</div>

</div>
                <div class="row" style="text-align:center;margin-top:20px">
                    <div class="col-4"></div>
                    <div class="col-4">
                        <a type="button" class="btn btn-primary" ng-click="confirmCopy()">Submit</a>

                    </div>
                    <div class="col-4"></div>
                    
                </div>
            </div>
        </div>
    </div>
</div>
<style>
        

#dropZone {border:2px dashed #ccc;padding:20px;text-align:center;border-radius:10px;cursor:pointer;transition: 0.3s;}
    </style>
       <div class="offcanvas offcanvas-end" tabindex="-1" id="addfiles">
        <div class="offcanvas-header justify-content-between border-bottom p-2">
             <h5 class="modal-title" id="lblfiles">{{ modalTitle }}</h5>
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

                    <div id="dropZone" class="dropzone" ng-show="canEdit(currentFileType)"
                     ng-click="openFilePicker($event)"
                     ng-class="{'highlight': isDragging}">
                    <i class="ri-upload-cloud-2-fill fs-6xl"></i><br />
                    Drag & Drop files here or click to upload
                </div>
                      <div class="preview-container" style="margin-top:10px; display:none; flex-wrap:wrap; gap:8px;">
                    <div ng-repeat="file in uploadedFiles" class="file-preview" style="width:60px; text-align:center;">
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
                     
<tr ng-repeat="file in showfileslist | orderBy:'-id' track by file.id">

                          <td width="45px">
                              <div class="attch_icon" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}">

                                  <i class="ri"
                                      ng-class="{
                                      'ri-image-2-fill': file.fext && (file.fext.toLowerCase() == '.jpg' || file.fext.toLowerCase() == '.png'),
                                      ' ri-file-pdf-fill': file.fext && file.fext.toLowerCase() == '.pdf',
                                      'ri-file-excel-fill': file.fext && (file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx'),
                                      'ri-file-word-fill': file.fext && (file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.doc')
         }"></i>
                              </div>
                          </td>

                          <td><span bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}"> {{ file.filedes.replace(file.fext,'').length > 40? (file.filedes.replace(file.fext,'') | limitTo:40) + '...': file.filedes.replace(file.fext,'')}}</span></td>
    <td>{{file.Uploadby}}</td>
                          <td class='text-right' style='width:30px;vertical-align:middle;'>
                             
                              <a href="javascript:void(0);" class="text-dark" bs-tooltip  data-bs-toggle="tooltip" data-bs-title="{{file.ftypename}}" ng-href="{{ (file.fext.toLowerCase() == '.pdf' || file.fext.toLowerCase() == '.doc' || file.fext.toLowerCase() == '.docx' || file.fext.toLowerCase() == '.xls' || file.fext.toLowerCase() == '.xlsx') 
                      ? '/pms/ValidateDocument.aspx?isview=1&count=1&id=' + file.id 
                      : file.fileurl }}" 
       target="_blank"><i class="ri-eye-line" data-bs-toggle="tooltip"
         data-bs-placement="top"
         data-bs-trigger="hover"
         title="View"></i></a>
                              <a href="javascript:void(0);" class="text-dark" ng-click="downloadFile(file)"><i class="ri-download-2-line"></i></a>
                              <a href="javascript:void(0);" data-index="0" class="delete text-danger"  ng-click="removedocumentnew(file.id)"><i class="ri-delete-bin-line" data-bs-toggle='tooltip' data-bs-placement='top' data-bs-trigger='hover' data-bs-original-title='Delete'></i></a>
                        
                              </td>

                      </tr>
                           
</table>
                </div>
                </div>
            </div>
        </div>
    </div>
      <input type="hidden" id="hdneditid" value="0" />
    <div class="offcanvas offcanvas-end" id="addmile" data-bs-backdrop="static" tabindex="-1" data-bs-scroll="false">
    <div class="offcanvas-header border-bottom" style="padding:5px 15px;justify-content:space-between;">
        <h5 class="offcanvas-title">
             <label class="m-0" id="lbltitlemile">{{ modalTitle }}</label>  

        </h5>
        <div class="hstack gap-2">
            <button style="display:none" type="button" onclick="fsFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button style="display:none" type="button" onclick="fseFunctionmile()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" id="btncloseedit"  ng-click="closemile()" aria-label="Close"><i class="ri-close-line"></i></button>
            <div style="display:none"><button type="button" id="btnclsmile" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" data-bs-dismiss="offcanvas"  aria-label="Close"><i class="ri-close-line"></i></button></div>
        </div>
    </div>
    <iframe id='editdetails' width="100%" height="100%" style="padding:0px 0px;" border="0px"></iframe>      

</div>
       <div ng-show="isLoading" class="loader-overlay">
<div class="loader"></div>
            </div>

</div>
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
    

    
    <script src="/pms/new/assets/js/interact.min.js"></script>
    <script src="/pms/new/assets/js/app.js"></script>

    <script src="/pms/new/assets/js/moment.min.js"></script>

   <%-- <script src="/pms/new/assets/js/jquery.min.js"></script>
<script src="/pms/new/assets/js/moment.min.js"></script>
<script src="/pms/new/assets/js/daterangepicker.min.js"></script>
<link rel="stylesheet"  href="/pms/new/assets/css/daterangepicker.css" />--%>

 
   
    
       <style>
        .hidden-row {
    display: none;
}
    </style>



    

    <script>
        /* Corrected timeline drag/drop only. Removed old demo renderer because its missing ids stopped the page script. */
        function getHourWidth() {
            var col = document.querySelector('.time-col');
            return (col && col.offsetWidth) ? col.offsetWidth : 100;
        }
        function getClientPoint(event) {
            if (event && event.client && typeof event.client.x === 'number') return event.client;
            if (event && typeof event.clientX === 'number') return { x: event.clientX, y: event.clientY };
            if (event && event.originalEvent && typeof event.originalEvent.clientX === 'number') return { x: event.originalEvent.clientX, y: event.originalEvent.clientY };
            if (event && event.page && typeof event.page.x === 'number') return event.page;
            return { x: 0, y: 0 };
        }

       
function positionAssignDragPreview(task, event) {
    if (!task) return;

    var pt = getClientPoint(event);
    if (!pt) return;

    var w = task.offsetWidth || 240;
    var h = task.offsetHeight || 24;

    var left = pt.x;
    var top = pt.y - h - 3; // cursor ke just top me helper

    if (left + w > window.innerWidth - 6) {
        left = Math.max(6, window.innerWidth - w - 6);
    }

    if (top < 6) {
        top = pt.y + 2;
    }

    task.style.position = 'fixed';
    task.style.pointerEvents = 'none';
    task.style.zIndex = '2147483647';

    task.style.setProperty('left', left + 'px', 'important');
    task.style.setProperty('top', top + 'px', 'important');
}

        function disposeAssignBootstrapTooltips() {
            if (!window.bootstrap || !bootstrap.Tooltip) return;
            document.querySelectorAll('.tooltip').forEach(function (x) { if (x && x.parentNode) x.parentNode.removeChild(x); });
            document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                var inst = bootstrap.Tooltip.getInstance(el);
                if (inst) inst.dispose();
            });
        }

        function initAssignBootstrapTooltips() {
            if (!window.bootstrap || !bootstrap.Tooltip) return;
            document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                var inst = bootstrap.Tooltip.getInstance(el);
                if (inst) inst.dispose();
                new bootstrap.Tooltip(el, {
                    html: true,
                    container: 'body',
                    trigger: 'hover',
                    boundary: 'window'
                });
            });
        }
        function getDropConflictInfo(row, left, width, ignoreEl) {
            // Alert only when the DROP START point is not available.
            // Do not block a long task just because its duration continues after shift end;
            // server GenerateTaskSegments will continue remaining duration to next day.
            var info = { ok: false, reason: '', overlap: false, outsideShift: false };
            if (!row) { info.reason = 'No employee row found.'; return info; }

            var taskLeft = Math.max(0, left || 0);

            var strip = row.querySelector('.available-strip');
            if (!strip) { info.reason = 'Employee shift/free strip not found.'; info.outsideShift = true; return info; }

            var stripLeft = parseFloat(strip.style.left || strip.offsetLeft || 0);
            var stripWidth = parseFloat(strip.style.width || strip.offsetWidth || 0);
            var stripRight = stripLeft + stripWidth;

            if (taskLeft < stripLeft || taskLeft >= stripRight) {
                info.reason = 'Drop is outside employee shift time.';
                info.outsideShift = true;
                return info;
            }

            var probeWidth = Math.max(6, getHourWidth() / 16);
            var probeRight = Math.min(taskLeft + probeWidth, stripRight);

            var planned = row.querySelectorAll('.planned-time,.task-bar');
            for (var i = 0; i < planned.length; i++) {
                var el = planned[i];
                if (ignoreEl && el === ignoreEl) continue;
                if (el.classList && (el.classList.contains('assign-drag-preview') || el.classList.contains('assign-center-drag-preview'))) continue;

                var elLeft = parseFloat(el.style.left || el.offsetLeft || 0);
                var elWidth = parseFloat(el.style.width || el.offsetWidth || 0);
                var elRight = elLeft + elWidth;

                if (taskLeft < elRight && probeRight > elLeft) {
                    info.reason = 'Drop overlaps existing task.';
                    info.overlap = true;
                    info.overlapElement = el;
                    return info;
                }
            }

            info.ok = true;
            return info;
        }

        function isAvailableArea(row, left, width, ignoreEl) {
            return getDropConflictInfo(row, left, width, ignoreEl).ok;
        }

        function isStartPointFree(row, left, ignoreEl) {
            // Free-slot allocation is allowed even if the full task duration continues to next date.
            // So here we only check that the START point is inside shift and not inside an existing planned task.
            var b = getShiftBounds(row);
            if (!b) return false;

            var step = Math.max(10, getHourWidth() / 4); // 15 minute approx
            var taskLeft = Math.max(b.left, left || b.left);
            var probeRight = Math.min(taskLeft + step, b.right);
            if (taskLeft < b.left || taskLeft >= b.right || probeRight <= taskLeft) return false;

            var planned = row.querySelectorAll('.planned-time,.task-bar');
            for (var i = 0; i < planned.length; i++) {
                var el = planned[i];
                if (ignoreEl && el === ignoreEl) continue;

                var elLeft = parseFloat(el.style.left || el.offsetLeft || 0);
                var elWidth = parseFloat(el.style.width || el.offsetWidth || 0);
                var elRight = elLeft + elWidth;

                if (taskLeft < elRight && probeRight > elLeft) return false;
            }
            return true;
        }

        function findFreeLeftInRow(row, width, preferredLeft) {
            // Return a free START position. Full width does not need to fit in the same date;
            // server GenerateTaskSegments will split the remaining duration into next date.
            var b = getShiftBounds(row);
            if (!b) return null;
            var step = Math.max(10, getHourWidth() / 4); // 15 minute approx
            var start = Math.max(b.left, Math.min(preferredLeft || b.left, b.right - step));

            for (var left = start; left < b.right; left += step) {
                if (isStartPointFree(row, left, null)) return left;
            }
            for (var left2 = b.left; left2 < start; left2 += step) {
                if (isStartPointFree(row, left2, null)) return left2;
            }
            return null;
        }

        function getFreeIntervalsInRow(row, ignoreEl) {
            var b = getShiftBounds(row);
            if (!b) return [];

            var intervals = [{ left: b.left, right: b.right }];
            var planned = row.querySelectorAll('.planned-time,.task-bar');

            for (var i = 0; i < planned.length; i++) {
                var el = planned[i];
                if (ignoreEl && el === ignoreEl) continue;

                var elLeft = parseFloat(el.style.left || el.offsetLeft || 0);
                var elWidth = parseFloat(el.style.width || el.offsetWidth || 0);
                var elRight = elLeft + elWidth;

                var next = [];
                for (var j = 0; j < intervals.length; j++) {
                    var g = intervals[j];
                    if (elRight <= g.left || elLeft >= g.right) {
                        next.push(g);
                    } else {
                        if (elLeft > g.left) next.push({ left: g.left, right: Math.max(g.left, elLeft) });
                        if (elRight < g.right) next.push({ left: Math.min(g.right, elRight), right: g.right });
                    }
                }
                intervals = next;
            }

            return intervals.filter(function (g) { return (g.right - g.left) >= Math.max(6, getHourWidth() / 16); })
                            .sort(function (a, b) { return a.left - b.left; });
        }

        function findFreeLeftNearestInRow(row, width, preferredLeft) {
            // Find the nearest visible free GAP from drop point.
            // Full task width is NOT required to fit in the gap; remaining duration will be split after next task / next date.
            var gaps = getFreeIntervalsInRow(row, null);
            if (!gaps.length) return null;

            var step = Math.max(6, getHourWidth() / 16); // approx 3.75/4 min on 100px hour; supports tiny visible gaps
            var p = typeof preferredLeft === 'number' ? preferredLeft : gaps[0].left;
            var best = null;
            var bestScore = Number.MAX_VALUE;

            for (var i = 0; i < gaps.length; i++) {
                var g = gaps[i];
                var start;

                if (p >= g.left && p < g.right) {
                    // Drop point is already in a free gap. Use exactly this gap start point.
                    start = Math.min(Math.max(p, g.left), g.right - Math.min(step, g.right - g.left));
                    return start;
                }

                // If pointer is on an existing task, choose the closest free gap.
                // IMPORTANT: for previous gap use gap START, not gap end.
                // Example: only 30 min free before next task => assign 30 min there,
                // remaining will continue after next task / next date by server GenerateTaskSegments().
                start = p < g.left ? g.left : g.left;
                var dist = p < g.left ? (g.left - p) : (p - g.right);
                var tieBias = p >= g.right ? -0.25 : 0; // previous gap preference
                var score = dist + tieBias;
                if (score < bestScore) {
                    bestScore = score;
                    best = start;
                }
            }
            return best;
        }

        function getShiftStartLeft(row) {
            var b = getShiftBounds(row);
            return b ? b.left : 0;
        }

        function showOverlapPlanPopup(callback, planInfo) {
            planInfo = planInfo || {};
            var popupTitle = planInfo.forecastTitle || 'Task overlap found';
            var planHtml = '';
            /*if (planInfo.totalText || planInfo.daysText) {
                planHtml = '<div style="text-align:left;font-size:13px;line-height:1.55">' +
                    '<div><b>Total task time:</b> ' + (planInfo.totalText || '-') + '</div>' +
                    '<div><b>Free slot used today:</b> ' + (planInfo.todayFreeText || '00:00 Hrs') + '</div>' +
                    '<div><b>Forecast:</b> ' + (planInfo.daysText || '-') + '</div>' +
                    (planInfo.completeDateText ? '<div><b>Till:</b> ' + planInfo.completeDateText + '</div>' : '') +
                    (planInfo.note ? '<div style="margin-top:6px;color:#64748b">' + planInfo.note + '</div>' : '') +
                    '</div>';
            }*/

            if (window.Swal && Swal.fire) {
                Swal.fire({
                    title: popupTitle,
                    html: planHtml,
                    icon: 'warning',
                    showDenyButton: true,
                    showCancelButton: true,
                    confirmButtonText: 'Yes, find free slot',
                    denyButtonText: 'Yes, shift existing task',
                    cancelButtonText: 'Cancel',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then(function (r) {
                    if (r.isConfirmed) callback('findfree');
                    else if (r.isDenied) callback('shift');
                    else callback('cancel');
                });
            } else {
                var msg = popupTitle;
                if (planInfo.totalText || planInfo.daysText) {
                    msg += '\n\nTotal task time: ' + (planInfo.totalText || '-') +
                           '\nFree slot used today: ' + (planInfo.todayFreeText || '00:00 Hrs') +
                           '\nForecast: ' + (planInfo.daysText || '-');
                    if (planInfo.completeDateText) msg += '\nTill: ' + planInfo.completeDateText;
                }
                msg += '\n\nOK = Continue with this plan and find free slot.\nCancel = Cancel drop.\n\nFor Shift Existing Task, please enable SweetAlert popup.';
                callback(confirm(msg) ? 'findfree' : 'cancel');
            }
        }

        function durationTextToMinutes(text) {
            if (!text) return 60;
            text = String(text).trim();
            if (text.indexOf(':') > -1) {
                var p = text.split(':');
                return ((parseInt(p[0], 10) || 0) * 60) + (parseInt(p[1], 10) || 0);
            }
            if (text.indexOf('/') > -1) {
                var q = text.split('/');
                return ((parseInt(q[0], 10) || 0) * 60) + (parseInt(q[1], 10) || 0);
            }
            var n = parseFloat(text);
            return isNaN(n) ? 60 : Math.max(15, n * 60);
        }

        function minutesToPlanText(minutes) {
            minutes = Math.max(0, parseInt(minutes || 0, 10));
            var h = Math.floor(minutes / 60);
            var m = minutes % 60;
            return (('0' + h).slice(-2)) + ':' + (('0' + m).slice(-2)) + ' Hrs';
        }

        function getDropMinutes(scope, taskid, tasktype, original) {
            var minutes = 0;
            try {
                var selected = scope && scope.getSelectedTasksForDrop
                    ? scope.getSelectedTasksForDrop(taskid, tasktype)
                    : [];

                if (selected && selected.length) {
                    selected.forEach(function (x) {
                        if (scope && scope.getDurationMinutesFromTask) {
                            minutes += scope.getDurationMinutesFromTask(x) || 0;
                        } else {
                            minutes += durationTextToMinutes(x.TaskDuration || x.taskduration || x.Duration);
                        }
                    });
                }
            } catch (e) { }

            if (!minutes && original) {
                minutes = durationTextToMinutes(original.getAttribute('data-duration'));
            }
            return minutes || 60;
        }

        function parseWorkDateForPlan(scope, seg) {
            try {
                if (scope && scope.getSegmentDateMoment) return scope.getSegmentDateMoment(seg).clone().startOf('day');
                var d = seg ? (seg.workDateKey || seg.WorkDateKey || seg.workDate) : new Date();
                if (typeof d === 'string' && d.indexOf('/Date(') === 0) {
                    d = new Date(parseInt(d.replace(/[^0-9-]/g, ''), 10));
                }
                return moment(d).startOf('day');
            } catch (e) { return moment().startOf('day'); }
        }

        function getEmployeeForPlan(scope, empid) {
            var emp = null;
            try {
                angular.forEach((scope && scope.currenttaskteam) || [], function (u) {
                    if (!emp && String(u.id) === String(empid)) emp = u;
                });
            } catch (e) { }
            return emp;
        }

        function getShiftMinutesForPlan(scope, emp, day) {
            try {
                var ss = scope && scope.parseTimeOnDate ? scope.parseTimeOnDate(emp.ShiftStart, day) : moment(day).startOf('day');
                var se = scope && scope.parseTimeOnDate ? scope.parseTimeOnDate(emp.ShiftEnd, day) : moment(day).startOf('day').add(parseInt(emp.ShiftHours || 8, 10), 'hours');
                if (se.isSameOrBefore(ss)) se.add(1, 'day');
                return Math.max(0, se.diff(ss, 'minutes'));
            } catch (e) {
                return Math.max(0, parseInt((emp && emp.ShiftHours) || 8, 10) * 60);
            }
        }

        function getBookedMinutesForPlan(scope, emp, day) {
            var booked = 0;
            var key = moment(day).format('YYYY-MM-DD');
            try {
                angular.forEach((emp && emp.taskSegments) || [], function (seg) {
                    if (parseWorkDateForPlan(scope, seg).format('YYYY-MM-DD') === key) {
                        booked += parseInt(seg.DurationMinutes || 0, 10) || 0;
                    }
                });
            } catch (e) { }
            return booked;
        }

        function getTodayFreeAfterStartMinutes(row, startLeft) {
            var total = 0;
            var gaps = getFreeIntervalsInRow(row, null) || [];
            var hw = getHourWidth();
            for (var i = 0; i < gaps.length; i++) {
                var g = gaps[i];
                var l = Math.max(g.left, startLeft || g.left);
                if (g.right > l) total += Math.round(((g.right - l) / hw) * 60);
            }
            return Math.max(0, total);
        }

        function parsePlanMomentOnDay(scope, day, timeText) {
            try {
                if (scope && scope.parseTimeOnDate) return scope.parseTimeOnDate(timeText, day);
            } catch (e) { }
            return moment(day).startOf('day').add(moment(timeText || '09:00 AM', ['hh:mm A','h:mm A','HH:mm']).hours(), 'hours').add(moment(timeText || '09:00 AM', ['hh:mm A','h:mm A','HH:mm']).minutes(), 'minutes');
        }

        function getTodayFreeIntervalsForPlan(scope, row, day, startLeft) {
            var intervals = [];
            var gaps = getFreeIntervalsInRow(row, null) || [];
            var b = getShiftBounds(row);
            if (!b) return intervals;
            var hw = getHourWidth();
            var shiftStartText = row.getAttribute('data-shiftstart') || '09:00 AM';
            var shiftStart = parsePlanMomentOnDay(scope, day, shiftStartText);

            for (var i = 0; i < gaps.length; i++) {
                var g = gaps[i];
                var l = Math.max(g.left, startLeft || g.left);
                if (g.right <= l) continue;
                var sm = Math.round(((l - b.left) / hw) * 60);
                var em = Math.round(((g.right - b.left) / hw) * 60);
                if (em > sm) intervals.push({ start: shiftStart.clone().add(sm, 'minutes'), end: shiftStart.clone().add(em, 'minutes') });
            }
            return intervals;
        }

        function getFutureFreeIntervalsForPlan(scope, emp, day) {
            var intervals = [];
            if (!emp) return intervals;

            var shiftStart = parsePlanMomentOnDay(scope, day, emp.ShiftStart || '09:00 AM');
            var shiftEnd = parsePlanMomentOnDay(scope, day, emp.ShiftEnd || '06:00 PM');
            if (shiftEnd.isSameOrBefore(shiftStart)) shiftEnd.add(1, 'day');

            var free = [{ start: shiftStart, end: shiftEnd }];
            var key = moment(day).format('YYYY-MM-DD');
            var booked = [];
            try {
                angular.forEach((emp && emp.taskSegments) || [], function (seg) {
                    if (parseWorkDateForPlan(scope, seg).format('YYYY-MM-DD') !== key) return;
                    var bs = parsePlanMomentOnDay(scope, day, seg.StartTime || seg.starttime || '09:00 AM');
                    var be = parsePlanMomentOnDay(scope, day, seg.EndTime || seg.endtime || '09:00 AM');
                    if (be.isSameOrBefore(bs)) be.add(1, 'day');
                    booked.push({ start: bs, end: be });
                });
            } catch (e) { }
            booked.sort(function (a, b) { return a.start.diff(b.start); });

            for (var i = 0; i < booked.length; i++) {
                var ob = booked[i];
                var next = [];
                for (var j = 0; j < free.length; j++) {
                    var fr = free[j];
                    if (ob.end.isSameOrBefore(fr.start) || ob.start.isSameOrAfter(fr.end)) {
                        next.push(fr);
                    } else {
                        if (ob.start.isAfter(fr.start)) next.push({ start: fr.start.clone(), end: ob.start.clone() });
                        if (ob.end.isBefore(fr.end)) next.push({ start: ob.end.clone(), end: fr.end.clone() });
                    }
                }
                free = next;
            }
            return free.filter(function (x) { return x.end.diff(x.start, 'minutes') > 0; });
        }

        function getForecastDayText(daysUsed) {
            if (!daysUsed || daysUsed < 1) return 'No free slot found';
            if (daysUsed === 1) return 'same day';
            if (daysUsed === 2) return 'next day';
            return 'next ' + daysUsed + ' working days';
        }

        function buildOverlapPlanInfo(scope, row, taskid, tasktype, original, startLeft, width) {
            var nearestFreeLeft = findFreeLeftNearestInRow(row, width || 25, startLeft);
            if (nearestFreeLeft !== null && nearestFreeLeft !== undefined) startLeft = nearestFreeLeft;

            var totalMinutes = getDropMinutes(scope, taskid, tasktype, original);
            var empid = row ? row.getAttribute('data-empid') : '';
            var emp = getEmployeeForPlan(scope, empid);
            var selectedDay = moment((scope && scope.selectedDate) || new Date()).startOf('day');
            var remaining = totalMinutes;
            var daysUsed = 0;
            var completeAt = null;
            var todayUsed = 0;
            var note = '';

            for (var i = 0; i < 90 && remaining > 0; i++) {
                var day = selectedDay.clone().add(i, 'days');
                var intervals = i === 0
                    ? getTodayFreeIntervalsForPlan(scope, row, day, startLeft)
                    : getFutureFreeIntervalsForPlan(scope, emp, day);

                var usedThisDay = 0;
                for (var j = 0; j < intervals.length && remaining > 0; j++) {
                    var freeMin = Math.max(0, intervals[j].end.diff(intervals[j].start, 'minutes'));
                    if (freeMin <= 0) continue;

                    if (remaining <= freeMin) {
                        usedThisDay += remaining;
                        if (i === 0) todayUsed += remaining;
                        completeAt = intervals[j].start.clone().add(remaining, 'minutes');
                        remaining = 0;
                        break;
                    } else {
                        usedThisDay += freeMin;
                        if (i === 0) todayUsed += freeMin;
                        remaining -= freeMin;
                    }
                }
                if (usedThisDay > 0) daysUsed++;
            }

            if (remaining > 0) note = 'Estimate checked next 90 days only.';

            var forecastDayText = getForecastDayText(daysUsed);
            var completeDateText = completeAt ? completeAt.format('DD-MMM-YYYY hh:mm A') : '';
            var forecastTitle = completeAt
                ? 'The selected shall be completed in ' + forecastDayText + ', till ' + completeAt.format('DD-MMM-YYYY hh:mm A')
                : 'No free slot found for this task';

            return {
                totalMinutes: totalMinutes,
                totalText: minutesToPlanText(totalMinutes),
                todayFreeText: minutesToPlanText(todayUsed),
                daysText: completeAt ? (forecastDayText + ', till ' + completeAt.format('DD-MMM-YYYY hh:mm A')) : forecastDayText,
                completeDateText: completeDateText,
                forecastTitle: forecastTitle,
                note: note
            };
        }

        function getDropWidth(scope, taskid, tasktype, original, fallbackWidth) {
            var minutes = 0;
            try {
                var selected = scope && scope.getSelectedTasksForDrop
                    ? scope.getSelectedTasksForDrop(taskid, tasktype)
                    : [];

                if (selected && selected.length) {
                    selected.forEach(function (x) {
                        if (scope && scope.getDurationMinutesFromTask) {
                            minutes += scope.getDurationMinutesFromTask(x) || 0;
                        } else {
                            minutes += durationTextToMinutes(x.TaskDuration || x.taskduration || x.Duration);
                        }
                    });
                }
            } catch(e) {}

            if (!minutes && original) {
                minutes = durationTextToMinutes(original.getAttribute('data-duration'));
            }

            if (!minutes) minutes = 60;
            return Math.max(25, (minutes / 60) * getHourWidth() - 4);
        }

        function findDroppedRow(event) {
            var pt = getClientPoint(event);
            var rows = document.querySelectorAll('.timeline-row');
            var droppedRow = null;
            rows.forEach(function (row) {
                var rect = row.getBoundingClientRect();
                if (pt.y >= rect.top && pt.y <= rect.bottom && pt.x >= rect.left && pt.x <= rect.right) droppedRow = row;
            });
            return droppedRow;
        }

        function getAssignDropLeft(row, event, dragPreview) {
            if (!row) return 0;

            var rowRect = row.getBoundingClientRect();
            var pt = getClientPoint(event);
            var eventLeft = (pt && typeof pt.x === 'number') ? (pt.x - rowRect.left) : 0;

            // Interact drag-end can sometimes give a stale/client point. The helper div is
            // positioned from the latest mouse move, so use its left when it is available.
            var previewLeft = null;
            try {
                if (dragPreview && dragPreview.getBoundingClientRect) {
                    var pr = dragPreview.getBoundingClientRect();
                    if (isFinite(pr.left) && pr.left > 0) {
                        previewLeft = pr.left - rowRect.left;
                    }
                }
            } catch (e) { previewLeft = null; }

            var left = (previewLeft !== null) ? previewLeft : eventLeft;

            if (!isFinite(left)) left = eventLeft || 0;
            return Math.max(0, left);
        }
        function getShiftBounds(row) {
            var strip = row ? row.querySelector('.available-strip') : null;
            if (!strip) return null;

            var stripLeft = parseFloat(strip.style.left || strip.offsetLeft || 0);
            var stripWidth = parseFloat(strip.style.width || strip.offsetWidth || 0);

            return {
                left: stripLeft,
                right: stripLeft + stripWidth,
                width: stripWidth
            };
        }

        function normalizeDropLeftToShift(row, left, width) {
            var b = getShiftBounds(row);
            if (!b) return Math.max(0, left || 0);

            // For multi-day split scheduling, the full task width does not need to fit in the current shift.
            // We only normalize the START position inside the employee shift.
            if (left < b.left) return b.left;
            if (left >= b.right) return Math.max(b.left, b.right - Math.max(10, getHourWidth() / 4));

            return left;
        }

        function pxToDropTime(row, left) {
            var hourWidth = getHourWidth();
            var shiftStart = row.getAttribute('data-shiftstart') || '09:00 AM';
            var b = getShiftBounds(row);

            // left is timeline-left. Convert it to employee-shift relative minutes.
            // Example: if employee shift strip starts after header start, strip-left must be deducted.
            var relativeLeft = b ? Math.max(0, left - b.left) : Math.max(0, left || 0);

            return moment(shiftStart, ['hh:mm A','h:mm A','HH:mm'])
                .add((relativeLeft / hourWidth) * 60, 'minutes')
                .format('hh:mm A');
        }
        function enableSheetTask() {
            if (typeof interact === 'undefined') return;
            try { interact('.drag-item').unset(); } catch(e) {}
            interact('.drag-item').draggable({ listeners: {
                start: function (event) { event.target.classList.add('dragging'); },
                move: function (event) {
                    var target = event.target;
                    var x = (parseFloat(target.dataset.x) || 0) + event.dx;
                    var y = (parseFloat(target.dataset.y) || 0) + event.dy;
                    target.style.transform = 'translate(' + x + 'px,' + y + 'px)';
                    target.dataset.x = x; target.dataset.y = y;
                },
                end: function (event) {
                    var target = event.target;
                    var droppedRow = findDroppedRow(event);
                    target.classList.remove('dragging');
                    target.style.transform = 'translate(0px,0px)';
                    target.dataset.x = 0; target.dataset.y = 0;
                    if (!droppedRow) return;
                    var rowRect = droppedRow.getBoundingClientRect();
                    var pt = getClientPoint(event);
                    var left = Math.max(0, pt.x - rowRect.left);
                    var width = target.offsetWidth || 120;
                    if (!isAvailableArea(droppedRow, left, width, target)) { alert('Drop only in available area'); return; }
                    droppedRow.appendChild(target);
                    target.style.position = 'absolute';
                    target.style.left = left + 'px';
                    target.style.top = '10px';
                }
            }}).resizable({ edges: { right: true }, listeners: { move: function (event) {
                var target = event.target, row = target.parentElement;
                var left = parseFloat(target.style.left || 0), newWidth = event.rect.width;
                if (isAvailableArea(row, left, newWidth, target)) target.style.width = newWidth + 'px';
            }}});
        }
        function enableRightDrag(scope) {
            if (typeof interact === 'undefined') return;

            function canAssignType(taskType) {
                try { return !scope || !scope.canAssign || scope.canAssign(taskType); }
                catch (e) { return false; }
            }

            function showAssignPermissionAlert(taskType) {
                try {
                    if (scope && scope.requireAssign) { scope.requireAssign(taskType); }
                    else { alert('You do not have assign permission.'); }
                } catch (e) { alert('You do not have assign permission.'); }
            }

            function findExternalTask(el) {
                if (!el) return null;
                if (el.closest) return el.closest('.external-task');
                while (el && el !== document) {
                    if (el.classList && el.classList.contains('external-task')) return el;
                    el = el.parentNode;
                }
                return null;
            }

            try { interact('.external-task').unset(); } catch(e) {}

            interact('.external-task').draggable({
                ignoreFrom: 'input,button,a,.task-icons,.text-danger',
                listeners: {
                start: function (event) {
                    var original = findExternalTask(event.target);
                    if (!original) return;
                    if (String(original.getAttribute('data-assigned') || '').toLowerCase() === 'true') {
                        if (event.interaction) event.interaction.stop();
                        return;
                    }

                    var startTaskType = original.getAttribute('data-type');
                    if (!canAssignType(startTaskType)) {
                        if (event.interaction) event.interaction.stop();
                        showAssignPermissionAlert(startTaskType);
                        return;
                    }

                    event.interaction.original = original;
                    event.interaction.taskId = original.getAttribute('data-id');
                    event.interaction.tasktype = startTaskType;
                    event.interaction.taskowner = original.getAttribute('data-owner');
                    event.interaction.tasktitle = original.getAttribute('data-title');

                    if (!event.interaction.taskId || !event.interaction.tasktype) return;

                    var task = document.createElement('div');
                    task.className = 'task-bar dragging assign-drag-preview'; // do not use drag-item here; old sheet drag handler can leave preview at top
                    task.style.position = 'fixed';
                    task.style.width = '240px';
                    task.style.pointerEvents = 'none';
                    task.style.zIndex = '2147483647';
                    task.style.userSelect = 'none';
                    task.style.webkitUserSelect = 'none';

                    var titleEl = original.querySelector('.task-title');
                    var dragTitle = titleEl ? titleEl.innerText : event.interaction.tasktitle;


                    try {
                        var selected = scope && scope.getSelectedTasksForDrop
                            ? scope.getSelectedTasksForDrop(event.interaction.taskId, event.interaction.tasktype)
                            : [];

                        if (selected.length > 1) {
                            dragTitle = selected.length + ' tasks selected';
                        }
                    } catch(e) {}

                    task.innerHTML =
                        '<div class="task-name">' + dragTitle + '</div>' +
                        '<div class="text-danger"><i class="ri-close-line"></i></div>' +
                        '<div class="resize-handle"></div>';

                    document.body.appendChild(task);
                    event.interaction.task = task;
setTimeout(function () {
    positionAssignDragPreview(task, event);
}, 0);
                    //positionAssignDragPreview(task, event);
                },

                move: function (event) {
                    var task = event.interaction.task;
                    if (!task) return;
                    positionAssignDragPreview(task, event);
                },

                end: function (event) {
                    var task = event.interaction.task;
                    var taskid = event.interaction.taskId;
                    var tasktype = event.interaction.tasktype;
                    var taskowner = event.interaction.taskowner;

                    // Remove floating drag preview immediately on drop/end.
                    // Center task should be redrawn only from Angular taskSegments after save.
                    function removeDragPreview() {
                        disposeAssignBootstrapTooltips();
                        if (task && task.parentNode) task.parentNode.removeChild(task);
                        event.interaction.task = null;
                    }

                    if (!taskid || !tasktype) {
                        removeDragPreview();
                        return;
                    }

                    if (!canAssignType(tasktype)) {
                        removeDragPreview();
                        showAssignPermissionAlert(tasktype);
                        return;
                    }

                    var droppedRow = findDroppedRow(event);
                    if (!droppedRow) {
                        removeDragPreview();
                        return;
                    }

                    var left = getAssignDropLeft(droppedRow, event, task);
                    var original = event.interaction.original || null;
                    var width = getDropWidth(scope, taskid, tasktype, original, task ? (task.offsetWidth || 240) : 240);

                    var empid = droppedRow.getAttribute('data-empid');

                    function executeAssign(finalLeft, mode, assignDate) {
                        var dropTime = pxToDropTime(droppedRow, finalLeft);
                        removeDragPreview();
                        if (!scope) return;
                        if (!canAssignType(tasktype)) {
                            showAssignPermissionAlert(tasktype);
                            return;
                        }
                        scope.$applyAsync(function () {
                            var tasks = scope.getSelectedTasksForDrop
                                ? scope.getSelectedTasksForDrop(taskid, tasktype)
                                : [];

                            var finalTasks = tasks.length ? tasks : [{ id: taskid, tasktype: tasktype, owner: taskowner }];

                            if (mode === 'shift' && scope.changeassignWithShift) {
                                scope.changeassignWithShift(empid, finalTasks, dropTime, assignDate);
                            }
                            else if (finalTasks.length > 1 && scope.changeassignMultiple) {
                                scope.changeassignMultiple(empid, finalTasks, dropTime, assignDate);
                            }
                            else if (finalTasks.length === 1 && scope.changeassign) {
                                scope.changeassign(empid, finalTasks[0].id, finalTasks[0].tasktype, finalTasks[0].owner, dropTime, false, assignDate);
                            }
                        });
                    }

                    var conflict = getDropConflictInfo(droppedRow, left, width, null);

                    if (!conflict.ok) {
                        removeDragPreview();
                        alert('Drop only in available area');
                        return;
                    }

                    executeAssign(left, 'normal');
                }
            }});
        }

    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {

            const toastEl = document.getElementById("borderedToast2");

            const toast = new bootstrap.Toast(toastEl, {
                autohide: true,
                delay: 3000
            });

            const progressBar = toastEl.querySelector(".toast-progress-bar");

            function resetProgress() {
                progressBar.style.animation = "none";
                progressBar.offsetHeight; // force reflow
                progressBar.style.animation = "shrink 3s linear forwards";
            }

            document.getElementById("borderedToast2Btn").addEventListener("click", function () {
                resetProgress();
                toast.show();
            });

            // Cancel button
            document.getElementById("toastCancelBtn").addEventListener("click", function () {
                toast.hide(); // close toast
                progressBar.style.animation = "none"; // stop progress
            });

        });
    </script>
    <script>
        document.addEventListener("DOMContentLoaded", () => {

            const dropzone = document.getElementById("dropzone");
            const fileInput = document.getElementById("fileInput");
            const fileTable = document.getElementById("fileTable");

            let filesArray = [];

            //dropzone.addEventListener("click", () => fileInput.click());

            //fileInput.addEventListener("change", (e) => {
            //    handleFiles(e.target.files);
            //});

            //dropzone.addEventListener("dragover", (e) => {
            //    e.preventDefault();
            //    dropzone.classList.add("dragover");
            //});

            //dropzone.addEventListener("dragleave", () => {
            //    dropzone.classList.remove("dragover");
            //});

            //dropzone.addEventListener("drop", (e) => {
            //    e.preventDefault();
            //    dropzone.classList.remove("dragover");
            //    handleFiles(e.dataTransfer.files);
            //});

            function getIcon(file) {
                if (file.type.startsWith("image/")) return "<i class='ri-image-fill'></i>";
                if (file.type.includes("pdf")) return "<i class='bx bxs-file-pdf' style='color:red;'></i>";
                if (file.type.includes("zip")) return "<i class='ri-folder-zip-line'></i>";
                if (file.type.includes("video")) return "<i class='ri-film-line'></i>";
                if (file.type.includes("audio")) return "<i class='ri-volume-up-fill'></i>";
                return "📁";
            }

            function handleFiles(files) {
                Array.from(files).forEach(file => {
                    filesArray.push(file);
                });

                renderTable();
            }

            function renderTable() {
                fileTable.innerHTML = "";

                filesArray.forEach((file, index) => {

                    const row = document.createElement("tr");

                    row.innerHTML = `
                    <td>
                      <span class="icon">${getIcon(file)}</span>
                      ${file.name}
                    </td>
                    <td>
                        <div class="d-flex align-items-center gap-2" style="display:inline-block;">
                            <div class="avatar-xxs">
                                <img src="/pms/new/assets/img/avatar.jpg" alt="" class="rounded-circle img-fluid">
                            </div>
                            <div style="color:#000;line-height:1;">Erica Kernan<br><small>34534</small></div>
                        </div>
                    </td>
                    <td><div class="d-flex align-items-center gap-2"><a href="javascript:void(0);" class="text-dark"><i class="ri-download-2-line"></i></a><a href="javascript:void(0);" class="text-dark"><i class="ri-eye-line"></i></a><a href="javascript:void(0);" data-index="${index}" class="delete text-danger"><i class="ri-delete-bin-line"></i></a></div></td>
                  `;

                    fileTable.appendChild(row);

                    uploadFile(file, row);
                });

                document.querySelectorAll(".delete").forEach(btn => {
                    btn.addEventListener("click", (e) => {
                        const index = e.target.dataset.index;
                        filesArray.splice(index, 1);
                        renderTable();
                    });
                });
            }

            function uploadFile(file, row) {

                const formData = new FormData();
                formData.append("file", file);

                fetch("/upload", {
                    method: "POST",
                    body: formData
                })
                    .then(res => res.json())
                    .then(() => {
                        row.querySelector(".status").textContent = "✔ Uploaded";
                        row.querySelector(".status").style.color = "green";
                    })
                    .catch(() => {
                        row.querySelector(".status").textContent = "✖ Failed";
                        row.querySelector(".status").style.color = "red";
                    });
            }

        });
    </script>
    <script>
        // Initial date
        let currentDate = new Date(); // Month is 0-based (4 = May)

        updateDisplay();

        function openCalendar() {
            const picker = document.getElementById("datePicker");

            if (picker.showPicker) {
                picker.showPicker();
            } else {
                picker.click();
            }
        }
        function openCalendar_mile() {
            const picker = document.getElementById("datePicker_mile");

            if (picker.showPicker) {
                picker.showPicker();
            } else {
                picker.click();
            }
        }
        function openCalendar_task() {
            const picker = document.getElementById("datePicker_task");

            if (picker.showPicker) {
                picker.showPicker();
            } else {
                picker.click();
            }
        }
        function openCalendar_subtask() {
            const picker = document.getElementById("datePicker_subtask");

            if (picker.showPicker) {
                picker.showPicker();
            } else {
                picker.click();
            }
        }
        function updateDisplay() {
            const day = String(currentDate.getDate()).padStart(2, '0');
            const month = String(currentDate.getMonth() + 1).padStart(2, '0');
            const year = currentDate.getFullYear();

            document.getElementById("selectedDate").textContent =
                `${day}/${month}/${year}`;

            document.getElementById("datePicker").value =
                `${year}-${month}-${day}`;
 var scope = angular.element(
        document.querySelector('[ng-controller="assignprojectCtrl"]')
    ).scope();

    if (scope) {

        scope.$applyAsync(function () {

            scope.selectedDate = new Date(currentDate);
            //alert(scope.selectedDate);
           if (scope.dateChanged) {
            scope.dateChanged();
            }

        });
    }
        }

        function changeDay(days) {
            currentDate.setDate(currentDate.getDate() + days);
  
            updateDisplay();
        }

        document.getElementById("datePicker").addEventListener("change", function () {
            if (this.value) {
                currentDate = new Date(this.value);
                updateDisplay();
            }
        });
    </script>
    <script>

       <%-- function openCalendar() {
            const picker = document.getElementById("datePicker");

            if (picker.showPicker) {
                picker.showPicker();
            } else {
                picker.click(); 
            }
        }

        document.getElementById("datePicker").addEventListener("change", function () {
            const value = this.value;

            if (value) {
                const [year, month, day] = value.split("-");
                document.getElementById("selectedDate").textContent =
                    `${day}/${month}/${year}`;
            }
        });--%>
    </script>
    <script src="/pms/assignprojects/app/js/dragula.min.js"></script>
    <script src="/pms/assignprojects/app/js/tasks-kanban.init.js"></script>
</asp:Content>
