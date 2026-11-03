<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="setnotification.aspx.cs" Inherits="setnotification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.3/angular.min.js"></script>

    <style type="text/css">
        [ng-cloak] { display:none !important; }

        .nt-page { padding:0; background:#fff; min-height:620px; }
        .nt-row { display:flex; width:100%; min-height:620px; }

        .nt-left {
            width:210px;
            border-right:1px solid #ddd;
            padding:12px 10px;
            background:#fff;
            flex:0 0 210px;
        }
        .nt-left-title { font-size:15px; font-weight:600; margin:0 0 12px 4px; }
        .nt-nav { list-style:none; padding:0; margin:0; }
        .nt-nav li { margin-bottom:4px; }
        .nt-nav a {
            display:block;
            padding:9px 10px;
            text-decoration:none;
            border-radius:4px;
            color:#444;
            border-left:3px solid transparent;
        }
        .nt-nav a:hover { background:#f7f4ff; color:#6f42c1; }
        .nt-nav a.active {
            color:#6f42c1;
            background:#f3edff;
            border-left-color:#6f42c1;
            font-weight:600;
        }

        .nt-main { flex:1; min-width:0; padding:14px 16px; }
        .nt-heading { font-size:18px; font-weight:600; margin:0 0 14px 0; }

        .nt-content-grid {
            display:grid;
            grid-template-columns:minmax(0, 1fr) 315px;
            gap:16px;
            align-items:start;
        }

        .nt-card {
            border:1px solid #ddd;
            border-radius:5px;
            background:#fff;
            padding:14px;
        }
        .nt-card-title { font-size:14px; font-weight:600; margin-bottom:8px; }

        .nt-form-row {
            display:grid;
            grid-template-columns:1fr 1fr;
            gap:14px;
        }

        .nt-label { display:block; font-size:13px; font-weight:600; margin-bottom:6px; }
        .nt-required { color:#d9534f; }

        .nt-label-status-row {
            display:flex;
            align-items:center;
            justify-content:space-between;
            gap:12px;
            margin-bottom:6px;
        }
        .nt-label-status-row .nt-label {
            margin-bottom:0;
        }
        .nt-active-status {
            display:flex;
            align-items:center;
            gap:7px;
            white-space:nowrap;
            font-size:12px;
            font-weight:600;
            color:#444;
        }
        .nt-select, .nt-input {
            width:100%;
            height:36px;
            border:1px solid #ccc;
            border-radius:3px;
            padding:6px 9px;
            background:#fff;
        }
        .nt-field { margin-bottom:14px; }

        /* WHATSAPP LABEL + SCREEN POPUP ON SAME LABEL ROW */
        .nt-whatsapp-field {
            margin-bottom:14px;
        }
        .nt-whatsapp-label-row {
            display:flex;
            align-items:center;
            justify-content:space-between;
            gap:20px;
            margin-bottom:7px;
        }
        .nt-whatsapp-label-row .nt-label {
            margin-bottom:0;
        }
        .nt-whatsapp-switches {
            display:flex;
            align-items:center;
            gap:22px;
            flex:0 0 auto;
        }
        .nt-whatsapp-switches .nt-switch-item {
            white-space:nowrap;
        }
        @media (max-width:900px) {
            .nt-whatsapp-label-row {
                align-items:flex-start;
                flex-direction:column;
                gap:8px;
            }
        }

        /* PLACEHOLDERS ABOVE EDITOR */
        .nt-placeholder-panel {
            border:1px solid #e1d8f5;
            border-radius:5px;
            margin:2px 0 14px 0;
            overflow:hidden;
        }
        .nt-placeholder-head {
            display:flex;
            align-items:center;
            justify-content:space-between;
            gap:10px;
            padding:9px 11px;
            background:#faf8ff;
            border-bottom:1px solid #e8e0f8;
            cursor:pointer;
            user-select:none;
        }
        .nt-placeholder-head-left { min-width:0; }
        .nt-placeholder-title { font-size:13px; font-weight:600; color:#333; }
        .nt-placeholder-subtitle { font-size:11px; color:#777; margin-top:2px; }
        .nt-collapse-btn {
            border:1px solid #d8caef;
            background:#fff;
            color:#6f42c1;
            border-radius:3px;
            height:28px;
            min-width:82px;
            padding:0 8px;
            cursor:pointer;
            font-size:11px;
        }
        .nt-placeholder-body { padding:10px 11px 5px 11px; }
        .nt-placeholder-wrap { max-height:205px; overflow:auto; padding-right:4px; }
        .nt-placeholder {
            display:inline-block;
            margin:0 5px 6px 0;
            padding:5px 7px;
            border:1px solid #dfd3fa;
            background:#f6f0ff;
            color:#6f42c1;
            border-radius:3px;
            cursor:pointer;
            font-size:11px;
            word-break:break-all;
        }
        .nt-placeholder:hover { background:#eee3ff; }
        .nt-no-placeholder { color:#999; font-size:12px; padding:6px 0 10px; }

        /* RICHER EDITOR */
        .nt-toolbar {
            min-height:40px;
            display:flex;
            align-items:center;
            flex-wrap:wrap;
            gap:3px;
            border:1px solid #ccc;
            border-bottom:0;
            background:#f7f7f7;
            padding:4px 6px;
        }
        .nt-tool {
            min-width:30px;
            height:28px;
            border:1px solid transparent;
            background:transparent;
            cursor:pointer;
            border-radius:3px;
            font-size:12px;
            padding:0 6px;
        }
        .nt-tool:hover { background:#fff; border-color:#ccc; }
        .nt-tool-select {
            height:28px;
            border:1px solid #ccc;
            border-radius:3px;
            background:#fff;
            font-size:11px;
            padding:2px 5px;
        }
        .nt-color-wrap {
            height:28px;
            border:1px solid #ccc;
            border-radius:3px;
            background:#fff;
            display:inline-flex;
            align-items:center;
            gap:3px;
            padding:0 4px;
            font-size:11px;
        }
        .nt-color-wrap input[type=color] {
            width:22px;
            height:22px;
            border:0;
            padding:0;
            background:transparent;
            cursor:pointer;
        }
        .nt-separator { width:1px; height:22px; background:#d7d7d7; margin:0 3px; }

        .nt-editor {
            min-height:100px;
            max-height:150px;
            overflow:auto;
            border:1px solid #ccc;
            padding:8px;
            outline:none;
            font-size:12px;
            line-height:1.55;
            background:#fff;
        }
        .nt-editor:empty:before {
            content:'Enter notification message template...';
            color:#aaa;
        }

        .nt-preview {
            min-height:300px;
            max-height:590px;
            overflow:auto;
            border:1px solid #c9d9f0;
            background:#f7fbff;
            padding:10px;
            font-size:12px;
            line-height:1.5;
            white-space:normal;
        }

        .nt-actions { display:flex; gap:8px; align-items:center; margin-top:14px; flex-wrap:wrap; }
        .nt-btn {
            border:0;
            border-radius:3px;
            padding:8px 14px;
            cursor:pointer;
            font-size:13px;
        }
        .nt-btn-primary { background:#6f42c1; color:#fff; }
        .nt-btn-success { background:#28a745; color:#fff; }
        .nt-btn-default { background:#eee; color:#333; border:1px solid #d0d0d0; }
        .nt-btn:disabled { opacity:.55; cursor:not-allowed; }

        .nt-message { margin-top:10px; padding:8px 10px; border-radius:3px; font-size:12px; }
        .nt-message.error { color:#a94442; background:#f2dede; border:1px solid #ebccd1; }
        .nt-message.info { color:green; background:#d9edf7; border:1px solid #bce8f1; }

        .nt-heading-row {
            display:flex;
            align-items:center;
            gap:14px;
            flex-wrap:wrap;
            margin-bottom:12px;
        }
        .nt-heading-row .nt-heading {
            margin-bottom:0;
        }
        .nt-top-info {
            margin:0;
            padding:7px 12px;
            font-size:12px;
            line-height:16px;
            white-space:nowrap;
            color:#2e7d32 !important;
            background:#edf9ef !important;
            border:1px solid #b7dfbc !important;
            border-radius:5px;
            box-shadow:0 2px 7px rgba(0,0,0,.08);
            animation:ntTopAlertIn .18s ease-out;
        }
        .nt-top-info:before {
            
            display:inline-block;
            margin-right:6px;
            font-weight:700;
        }
        @keyframes ntTopAlertIn {
            from { opacity:0; transform:translateY(-5px); }
            to { opacity:1; transform:translateY(0); }
        }
        .nt-label-tools {
            display:flex;
            align-items:center;
            gap:12px;
        }
        .nt-loading-inline {
            display:inline-flex;
            align-items:center;
            gap:6px;
            color:#6f42c1;
            background:#f5f0ff;
            border:1px solid #ded2f7;
            border-radius:12px;
            padding:2px 8px;
            font-size:12px;
            font-weight:600;
            white-space:nowrap;
        }
        .nt-spinner {
            width:14px;
            height:14px;
            border:2px solid #d9ccf7;
            border-top-color:#6f42c1;
            border-radius:50%;
            display:inline-block;
            box-sizing:border-box;
            animation:ntSpin .65s linear infinite;
        }
        .nt-btn-spinner {
            width:13px;
            height:13px;
            border:2px solid rgba(255,255,255,.55);
            border-top-color:#fff;
            border-radius:50%;
            display:inline-block;
            box-sizing:border-box;
            animation:ntSpin .65s linear infinite;
            vertical-align:-2px;
            margin-right:6px;
        }
        @keyframes ntSpin {
            to { transform:rotate(360deg); }
        }

        /* SCREEN POPUP / FREEZE SCREEN SWITCHES */
        .nt-switch-row {
            display:flex;
            align-items:center;
            gap:28px;
            flex-wrap:nowrap;
            margin-bottom:14px;
        }

        .nt-switch-row .nt-switch-item {
            white-space:nowrap;
        }
        .nt-switch-item {
            display:flex;
            align-items:center;
            gap:7px;
            margin:0;
            font-size:13px;
            line-height:18px;
            font-weight:600;
            color:#333;
        }
        .nt-switch {
            position:relative;
            display:inline-block;
            width:38px;
            height:18px;
            flex:0 0 38px;
            vertical-align:middle;
        }
        .nt-switch input {
            opacity:0;
            width:0;
            height:0;
            margin:0;
        }
        .nt-switch-slider {
            position:absolute;
            cursor:pointer;
            top:0;
            left:0;
            right:0;
            bottom:0;
            background:#c7c7c7;
            border-radius:18px;
            transition:.2s;
        }
        .nt-switch-slider:before {
            content:'';
            position:absolute;
            width:14px;
            height:14px;
            left:2px;
            top:2px;
            background:#fff;
            border-radius:50%;
            transition:.2s;
            box-shadow:0 1px 2px rgba(0,0,0,.25);
        }
        .nt-switch input:checked + .nt-switch-slider {
            background:#6f42c1;
        }
        .nt-switch input:checked + .nt-switch-slider:before {
            transform:translateX(20px);
        }
        .nt-arrow {
    display: inline-block;
    width: 7px;
    height: 7px;
    border-right: 1.5px solid currentColor;
    border-bottom: 1.5px solid currentColor;
    margin-left: 6px;
    vertical-align: 2px;
}

.nt-arrow-up {
    transform: rotate(225deg);
    vertical-align: -1px;
}

.nt-arrow-down {
    transform: rotate(45deg);
    vertical-align: 2px;
}
        @media (max-width:1000px) {
            .nt-content-grid { grid-template-columns:1fr; }
            .nt-row { display:block; }
            .nt-left { width:100%; border-right:0; border-bottom:1px solid #ddd; }
            .nt-nav { display:flex; gap:4px; }
            .nt-main { width:100%; }
        }
        @media (max-width:700px) {
            .nt-form-row { grid-template-columns:1fr; gap:0; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="notificationTemplateApp"
         class="nt-page"
         ng-app="notificationTemplateApp"
         ng-controller="NotificationTemplateController as vm"
         ng-cloak>

        <div class="nt-row">

            <div class="nt-left">
                <div class="nt-left-title">Notifications</div>
                <ul class="nt-nav">
                    <li>
                        <a href="setnotification.aspx?type=222"
                           ng-class="{active: vm.type === '222'}">
                            Critical
                        </a>
                    </li>
                    <li>
                        <a href="setnotification.aspx?type=223"
                           ng-class="{active: vm.type === '223'}">
                            QC
                        </a>
                    </li>
                     <li>
                        <a href="setnotification.aspx?type=224"
                           ng-class="{active: vm.type === '224'}">
                            QC Action
                        </a>
                    </li>
                </ul>
            </div>

            <div class="nt-main">
                <div class="nt-heading-row">
                    <div class="nt-heading">{{vm.pageTitle}}</div>
                    <div class="nt-message info nt-top-info"
                         ng-if="vm.info">
                        {{vm.info}}
                    </div>
                </div>
                <%--class="nt-content-grid"--%>
                <div>

                    <div class="nt-card">

                        <div class="nt-form-row">
                            <div class="nt-field">
                                <label class="nt-label">
                                    Template For (Type) <span class="nt-required">*</span>
                                </label>
                                <select class="nt-select"
                                        ng-model="vm.entityType"
                                        ng-options="x.Id as x.Name for x in vm.entityTypes"
                                        ng-change="vm.onEntityTypeChanged()">
                                </select>
                            </div>

                            <div class="nt-field">
                                <div class="nt-label-status-row">
                                    <label class="nt-label">
                                        {{vm.selectorLabel}} <span class="nt-required">*</span>
                                    </label>

                                    <div class="nt-label-tools">
                                        <span class="nt-loading-inline"
                                              ng-if="vm.loadingTemplate">
                                            <span class="nt-spinner"></span>
                                            Loading...
                                        </span>

                                        <label class="nt-active-status">
                                            <span>{{vm.isActive ? 'Active' : 'Deactive'}}</span>
                                            <span class="nt-switch">
                                                <input type="checkbox"
                                                       ng-model="vm.isActive" />
                                                <span class="nt-switch-slider"></span>
                                            </span>
                                        </label>
                                    </div>
                                </div>

                                <select class="nt-select"
                                        ng-model="vm.selectedId"
                                        ng-options="x.Id as x.Name for x in vm.items track by x.Id"
                                        ng-change="vm.onMasterChanged()"
                                        ng-disabled="vm.loadingTemplate || vm.savingTemplate">
                                    <option value="">{{vm.selectorPlaceholder}}</option>
                                </select>
                            </div>
                        </div>

                        <!-- Placeholders are intentionally ABOVE the editor -->
                        <div class="nt-placeholder-panel">
                            <div class="nt-placeholder-head"
                                 ng-click="vm.placeholdersExpanded = !vm.placeholdersExpanded">
                                <div class="nt-placeholder-head-left">
                                    <div class="nt-placeholder-title">
                                        {{vm.entityTypeName}} Placeholders
                                    </div>
                                    <div class="nt-placeholder-subtitle">
                                        <%--Fields from {{vm.placeholderTableName || 'selected PMS table'}}.--%> Click any field to insert it into the message.
                                    </div>
                                </div>

                                <button type="button"
                                        class="nt-collapse-btn"
                                        ng-click="$event.stopPropagation(); vm.placeholdersExpanded = !vm.placeholdersExpanded">
                                    {{vm.placeholdersExpanded ? 'Collapse' : 'Expand'}}
                                     <span class="nt-arrow"
          ng-class="vm.placeholdersExpanded ? 'nt-arrow-up' : 'nt-arrow-down'">
    </span>
                                </button>
                            </div>

                            <div class="nt-placeholder-body" ng-show="vm.placeholdersExpanded">
                                <div class="nt-placeholder-wrap" ng-if="!vm.loadingPlaceholders && vm.placeholders.length">
                                    <span class="nt-placeholder"
                                          ng-repeat="p in vm.placeholders track by p.Token"
                                          ng-mousedown="$event.preventDefault()"
                                          ng-click="vm.insertPlaceholder(p.Token)"
                                          title="{{p.DisplayName}} - {{p.Token}}">
                                        {{p.DisplayName}}
                                    </span>
                                </div>

                                <div class="nt-no-placeholder" ng-if="vm.loadingPlaceholders">
                                    Loading {{vm.entityTypeName}} fields...
                                </div>

                                <div class="nt-no-placeholder"
                                     ng-if="!vm.loadingPlaceholders && !vm.placeholders.length">
                                    No fields found for selected type.
                                </div>
                            </div>
                       
                           
                            <div class="nt-toolbar">
                                <button type="button" class="nt-tool" title="Bold" ng-click="vm.format('bold')"><b>B</b></button>
                                <button type="button" class="nt-tool" title="Italic" ng-click="vm.format('italic')"><i>I</i></button>
                                <button type="button" class="nt-tool" title="Underline" ng-click="vm.format('underline')"><u>U</u></button>
                                <button type="button" class="nt-tool" title="Strike through" ng-click="vm.format('strikeThrough')"><s>S</s></button>

                                <span class="nt-separator"></span>

                                <select class="nt-tool-select"
                                        ng-model="vm.fontName"
                                        ng-change="vm.formatWithValue('fontName', vm.fontName)">
                                    <option value="Arial">Arial</option>
                                    <option value="Calibri">Calibri</option>
                                    <option value="Segoe UI">Segoe UI</option>
                                    <option value="Tahoma">Tahoma</option>
                                    <option value="Verdana">Verdana</option>
                                    <option value="Times New Roman">Times New Roman</option>
                                </select>

                                <select class="nt-tool-select"
                                        ng-model="vm.fontSize"
                                        ng-change="vm.formatWithValue('fontSize', vm.fontSize)">
                                    <option value="1">10</option>
                                    <option value="2">12</option>
                                    <option value="3">14</option>
                                    <option value="4">16</option>
                                    <option value="5">18</option>
                                    <option value="6">24</option>
                                    <option value="7">32</option>
                                </select>

                                <span class="nt-color-wrap" title="Text color">
                                    A
                                    <input type="color" ng-model="vm.foreColor" ng-change="vm.formatWithValue('foreColor', vm.foreColor)" />
                                </span>

                                <span class="nt-color-wrap" title="Highlight color">
                                    H
                                    <input type="color" ng-model="vm.backColor" ng-change="vm.formatWithValue('hiliteColor', vm.backColor)" />
                                </span>

                                <span class="nt-separator"></span>

                                <button type="button" class="nt-tool" title="Align left" ng-click="vm.format('justifyLeft')">&#8801;</button>
                                <button type="button" class="nt-tool" title="Align center" ng-click="vm.format('justifyCenter')">&#8801;</button>
                                <button type="button" class="nt-tool" title="Align right" ng-click="vm.format('justifyRight')">&#8801;</button>
                                <button type="button" class="nt-tool" title="Justify" ng-click="vm.format('justifyFull')">&#9776;</button>

                                <span class="nt-separator"></span>

                                <button type="button" class="nt-tool" title="Bulleted list" ng-click="vm.format('insertUnorderedList')">&#8226; List</button>
                                <button type="button" class="nt-tool" title="Numbered list" ng-click="vm.format('insertOrderedList')">1. List</button>
                                <button type="button" class="nt-tool" title="Decrease indent" ng-click="vm.format('outdent')">&#8592;</button>
                                <button type="button" class="nt-tool" title="Increase indent" ng-click="vm.format('indent')">&#8594;</button>

                                <span class="nt-separator"></span>

                                <button type="button" class="nt-tool" title="Undo" ng-click="vm.format('undo')">&#8630;</button>
                                <button type="button" class="nt-tool" title="Redo" ng-click="vm.format('redo')">&#8631;</button>
                                <button type="button" class="nt-tool" title="Remove formatting" ng-click="vm.format('removeFormat')">Clear</button>
                            </div>

                            <div id="notificationEditor"
                                 class="nt-editor"
                                 contenteditable="true"
                                 nt-contenteditable
                                 ng-model="vm.templateHtml">
                            </div>
                        </div>

                        <div class="nt-field nt-whatsapp-field">
                            <div class="nt-whatsapp-label-row">
                                <label class="nt-label">Whatsapp Template Id</label>

                                <div class="nt-whatsapp-switches" style="display:none;">
                                    <label class="nt-switch-item">
                                        <span>Do you want to screen popup?</span>
                                        <span class="nt-switch">
                                            <input type="checkbox"
                                                   ng-model="vm.screenPopup"
                                                   ng-change="vm.onScreenPopupChanged()" />
                                            <span class="nt-switch-slider"></span>
                                        </span>
                                    </label>

                                    <label class="nt-switch-item" ng-if="vm.screenPopup">
                                        <span>Do you want to freeze screen?</span>
                                        <span class="nt-switch">
                                            <input type="checkbox"
                                                   ng-model="vm.freezeScreen" />
                                            <span class="nt-switch-slider"></span>
                                        </span>
                                    </label>
                                </div>
                            </div>

                            <input type="text"
                                   class="nt-input"
                                   ng-model="vm.templateCode"
                                   placeholder="Enter Template Id" />
                        </div>
                        <div class="nt-actions">
                            <button type="button"
                                    class="nt-btn nt-btn-primary"
                                    ng-click="vm.downloadTemplate()"
                                    ng-disabled="!vm.templateHtml" style="display:none;">
                                Download Template
                            </button>

                            <button type="button"
                                    class="nt-btn nt-btn-success"
                                    ng-click="vm.showSaveNextStep()"
                                    ng-disabled="vm.savingTemplate || vm.loadingTemplate">
                                <span class="nt-btn-spinner"
                                      ng-if="vm.savingTemplate"></span>
                                {{vm.savingTemplate ? 'Saving...' : 'Save Template'}}
                            </button>

                            <button type="button"
                                    class="nt-btn nt-btn-default"
                                    ng-click="vm.resetTemplate()">
                                Reset
                            </button>
                        </div>

                        <div class="nt-message error" ng-if="vm.error">{{vm.error}}</div>
                    </div>

                    <div style="display:none;">
                        <div class="nt-card">
                            <div class="nt-card-title">Template Preview</div>
                            <div class="nt-preview" ng-bind-html="vm.getPreviewHtml()"></div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        (function () {
            'use strict';

            var app = angular.module('notificationTemplateApp', []);
            app.directive('ntContenteditable', function () {
                return {
                    require: 'ngModel',
                    link: function (scope, element, attrs, ngModel) {
                        function read() {
                            scope.$evalAsync(function () {
                                ngModel.$setViewValue(element.html());
                            });
                        }

                        // Keep the latest caret/selection inside the editor.
                        function saveSelection() {
                            var editor = element[0];
                            var selection = window.getSelection();

                            if (!selection || selection.rangeCount === 0)
                                return;

                            var range = selection.getRangeAt(0);

                            if (editor.contains(range.commonAncestorContainer))
                                editor._ntSavedRange = range.cloneRange();
                        }

                        function readAndSaveSelection() {
                            saveSelection();
                            read();
                        }

                        ngModel.$render = function () {
                            var newHtml = ngModel.$viewValue || '';

                            // Do not rebuild the editor DOM when content is already same.
                            // Rebuilding it destroys the saved browser Range and moves
                            // the caret to the beginning after placeholder insertion.
                            if (element.html() !== newHtml) {
                                element.html(newHtml);
                                element[0]._ntSavedRange = null;
                            }
                        };

                        element.on('keyup mouseup click focus input paste', readAndSaveSelection);
                        element.on('blur', read);

                        scope.$on('$destroy', function () {
                            element.off('keyup mouseup click focus input paste', readAndSaveSelection);
                            element.off('blur', read);
                        });
                    }
                };
            });

            app.controller('NotificationTemplateController', ['$http', '$sce', '$timeout', function ($http, $sce, $timeout) {
                var vm = this;

                vm.type = '<%= NotificationType %>';
                vm.pageTitle = vm.type === '224'
                    ? 'Set Notification Template - QC Action Workflows'
                    : (vm.type === '223'
                        ? 'Set Notification Template - QC Workflows'
                        : 'Set Notification Template - Critical Workflows');

                vm.selectorLabel = (vm.type === '223' || vm.type === '224')
                    ? 'For'
                    : 'Critical Label';

                vm.selectorPlaceholder = vm.type === '224'
                    ? 'Select QC Action'
                    : (vm.type === '223'
                        ? 'Select QC Status'
                        : 'Select Critical Label');

                vm.entityTypes = [
                    { Id: 'PROJECT', Name: 'Project' },
                    { Id: 'MILESTONE', Name: 'Milestone' },
                    { Id: 'TASK', Name: 'Task' },
                    { Id: 'SUBTASK', Name: 'Sub Task' }
                ];

                // Existing form was Sub Task based, so keep Sub Task as the initial selection.
                vm.entityType = 'SUBTASK';
                vm.entityTypeName = 'Sub Task';
                vm.placeholderTableName = 'Pms_tbl_AddSubTask';

                vm.items = [];
                vm.placeholders = [];
                vm.selectedId = '';
                vm.templateRecordId = 0;
                vm.templateHtml = '';
                vm.templateCode = '';
                vm.isActive = true;
                vm.screenPopup = false;
                vm.freezeScreen = false;
                vm.error = '';
                vm.info = '';
                vm.loadingPlaceholders = false;
                vm.loadingTemplate = false;
                vm.savingTemplate = false;
                vm.placeholdersExpanded = true;

                vm.fontName = 'Segoe UI';
                vm.fontSize = '2';
                vm.foreColor = '#000000';
                vm.backColor = '#ffff00';

                vm.getEntityTypeName = function () {
                    for (var i = 0; i < vm.entityTypes.length; i++) {
                        if (vm.entityTypes[i].Id === vm.entityType)
                            return vm.entityTypes[i].Name;
                    }
                    return '';
                };

                vm.loadMasters = function () {
                    vm.error = '';

                    return $http.post('setnotification.aspx/GetTemplateMasters', {
                        type: vm.type
                    }).then(function (response) {
                        var data = response.data && response.data.d ? response.data.d : response.data;

                        if (!data || data.Success === false) {
                            vm.error = data && data.Message
                                ? data.Message
                                : 'Unable to load notification template data.';
                            return;
                        }

                        vm.items = data.Items || [];
                    }, function () {
                        vm.error = 'Unable to load Critical/QC master data from server.';
                    });
                };

                vm.loadPlaceholders = function () {
                    vm.error = '';
                    vm.loadingPlaceholders = true;
                    vm.placeholders = [];
                    vm.entityTypeName = vm.getEntityTypeName();

                    return $http.post('setnotification.aspx/GetTemplatePlaceholders', {
                        entityType: vm.entityType
                    }).then(function (response) {
                        var data = response.data && response.data.d ? response.data.d : response.data;

                        vm.loadingPlaceholders = false;

                        if (!data || data.Success === false) {
                            vm.error = data && data.Message
                                ? data.Message
                                : 'Unable to load placeholders.';
                            return;
                        }

                        vm.placeholders = data.Placeholders || [];
                        vm.placeholderTableName = data.TableName || '';
                        vm.placeholdersExpanded = true;
                    }, function () {
                        vm.loadingPlaceholders = false;
                        vm.error = 'Unable to load fields for selected type.';
                    });
                };

                vm.clearSavedTemplateFields = function () {
                    vm.loadingTemplate = false;
                    vm.templateRecordId = 0;
                    vm.templateHtml = '';
                    vm.templateCode = '';
                    vm.isActive = true;
                    vm.screenPopup = false;
                    vm.freezeScreen = false;

                    var editor = document.getElementById('notificationEditor');
                    if (editor) {
                        editor.innerHTML = '';
                        editor._ntSavedRange = null;
                    }
                };

                vm.loadSavedTemplate = function () {
                    vm.error = '';
                    vm.info = '';
                    vm.loadingTemplate = false;

                    if (!vm.entityType || !vm.selectedId) {
                        vm.clearSavedTemplateFields();
                        return;
                    }

                    vm.loadingTemplate = true;

                    return $http.post('setnotification.aspx/GetSavedNotificationTemplate', {
                        type: vm.type,
                        entityType: vm.entityType,
                        ntypeid: parseInt(vm.selectedId, 10)
                    }).then(function (response) {
                        vm.loadingTemplate = false;

                        var data = response.data && response.data.d ? response.data.d : response.data;

                        if (!data || data.Success === false) {
                            vm.error = data && data.Message
                                ? data.Message
                                : 'Unable to load saved template.';
                            return;
                        }

                        if (!data.Found) {
                            vm.clearSavedTemplateFields();
                            return;
                        }

                        vm.templateRecordId = data.Id || 0;
                        vm.templateHtml = data.TempText || '';
                        vm.templateCode = data.WhatsappTemplateId || '';
                        vm.isActive = data.IsActive === 1 || data.IsActive === true;
                        vm.screenPopup = data.IsScreenPopup === 1 || data.IsScreenPopup === true;
                        vm.freezeScreen = vm.screenPopup &&
                            (data.IsScreenFreeze === 1 || data.IsScreenFreeze === true);
                    }, function () {
                        vm.loadingTemplate = false;
                        vm.error = 'Unable to load saved notification template.';
                    });
                };

                vm.onEntityTypeChanged = function () {
                    vm.info = '';
                    vm.loadPlaceholders();
                    vm.loadSavedTemplate();
                };

                vm.onMasterChanged = function () {
                    vm.loadSavedTemplate();
                };

                vm.onScreenPopupChanged = function () {
                    if (!vm.screenPopup)
                        vm.freezeScreen = false;
                };

                vm.syncEditorModel = function () {
                    var editor = document.getElementById('notificationEditor');
                    if (editor)
                        vm.templateHtml = editor.innerHTML;
                };

                vm.restoreEditorSelection = function (editor) {
                    if (!editor) return null;

                    editor.focus();

                    var selection = window.getSelection();
                    if (!selection) return null;

                    var savedRange = editor._ntSavedRange;

                    if (savedRange &&
                        editor.contains(savedRange.commonAncestorContainer)) {
                        selection.removeAllRanges();
                        selection.addRange(savedRange);
                        return savedRange;
                    }

                    // No saved caret: use the end of the current editor content.
                    var range = document.createRange();
                    range.selectNodeContents(editor);
                    range.collapse(false);

                    selection.removeAllRanges();
                    selection.addRange(range);
                    editor._ntSavedRange = range.cloneRange();

                    return range;
                };

                vm.saveEditorSelection = function (editor) {
                    if (!editor) return;

                    var selection = window.getSelection();

                    if (selection && selection.rangeCount > 0) {
                        var range = selection.getRangeAt(0);

                        if (editor.contains(range.commonAncestorContainer))
                            editor._ntSavedRange = range.cloneRange();
                    }
                };

                vm.format = function (command) {
                    var editor = document.getElementById('notificationEditor');
                    if (!editor) return;

                    vm.restoreEditorSelection(editor);
                    document.execCommand(command, false, null);
                    vm.saveEditorSelection(editor);
                    vm.syncEditorModel();
                };

                vm.formatWithValue = function (command, value) {
                    var editor = document.getElementById('notificationEditor');
                    if (!editor || !value) return;

                    vm.restoreEditorSelection(editor);

                    // Chrome/Edge support hiliteColor. BackColor is a safe fallback.
                    var success = document.execCommand(command, false, value);
                    if (!success && command === 'hiliteColor')
                        document.execCommand('backColor', false, value);

                    vm.saveEditorSelection(editor);
                    vm.syncEditorModel();
                };

                vm.insertPlaceholder = function (token) {
                    if (!token) return;

                    var editor = document.getElementById('notificationEditor');
                    if (!editor) return;

                    var range = vm.restoreEditorSelection(editor);
                    var selection = window.getSelection();

                    if (!range || !selection)
                        return;

                    range.deleteContents();

                    var node = document.createTextNode(token);
                    range.insertNode(node);

                    // Keep the blinking cursor immediately after the inserted token.
                    range.setStartAfter(node);
                    range.collapse(true);

                    selection.removeAllRanges();
                    selection.addRange(range);

                    editor._ntSavedRange = range.cloneRange();

                    // Update ngModel from the current editor DOM.
                    // This preserves the caret immediately after the inserted token.
                    angular.element(editor).triggerHandler('input');
                };

                vm.getPreviewHtml = function () {
                    var html = vm.templateHtml || '<span style="color:#999">Template preview will appear here.</span>';
                    return $sce.trustAsHtml(html);
                };

                vm.downloadTemplate = function () {
                    if (!vm.templateHtml) return;

                    var text = vm.templateHtml
                        .replace(/<br\s*\/?>/gi, '\r\n')
                        .replace(/<\/p>/gi, '\r\n')
                        .replace(/<\/div>/gi, '\r\n')
                        .replace(/<[^>]+>/g, '');

                    // Download only the placeholders used in the template.
                    // Example:
                    // {####TaskName####} {####SubTaskName####} {####ProjectName####}
                    // becomes:
                    // {{1}}{{2}}{{3}}
                    var placeholders = text.match(/\{####[^{}]+####\}/g) || [];

                    if (!placeholders.length) {
                        vm.error = 'No placeholder found in template.';
                        return;
                    }

                    vm.error = '';

                    var parameterText = '';
                    for (var i = 0; i < placeholders.length; i++) {
                        parameterText += '{{' + (i + 1) + '}}';
                    }

                    var blob = new Blob([parameterText], { type: 'text/plain;charset=utf-8' });
                    var url = URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = (vm.templateCode ||
                        (vm.type === '224'
                            ? 'qc_action_workflow_template'
                            : (vm.type === '223' ? 'qc_workflow_template' : 'critical_workflow_template'))) + '.txt';
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                    URL.revokeObjectURL(url);
                };

                vm.resetTemplate = function () {
                    vm.templateRecordId = 0;
                    vm.templateHtml = '';
                    vm.templateCode = '';
                    vm.isActive = true;
                    vm.screenPopup = false;
                    vm.freezeScreen = false;
                    vm.error = '';
                    vm.info = '';
                    if (vm._infoTimer) {
                        $timeout.cancel(vm._infoTimer);
                        vm._infoTimer = null;
                    }
                    vm.loadingTemplate = false;
                    vm.savingTemplate = false;
                    vm.selectedId = '';

                    var editor = document.getElementById('notificationEditor');
                    if (editor) {
                        editor.innerHTML = '';
                        editor._ntSavedRange = null;
                    }
                };

                vm._infoTimer = null;

                vm.showTopSuccess = function (message) {
                    vm.info = message || 'Template saved successfully.';

                    if (vm._infoTimer) {
                        $timeout.cancel(vm._infoTimer);
                    }

                    // Auto hide the in-page success alert.
                    vm._infoTimer = $timeout(function () {
                        vm.info = '';
                        vm._infoTimer = null;
                    }, 3000);
                };

                vm.showSaveNextStep = function () {
                    if (vm.savingTemplate)
                        return;
debugger
                    vm.error = '';
                    vm.info = '';
                    vm.savingTemplate = false;
                    vm.syncEditorModel();

                    if (!vm.entityType) {
                        vm.error = 'Please select Template For (Type).';
                        return;
                    }

                    if (!vm.selectedId&&parseInt(vm.type, 10)!=223) {
                        vm.error = 'Please select ' + vm.selectorLabel + '.';
                        return;
                    }

                    if (!vm.templateHtml || !vm.templateHtml.replace(/<[^>]*>/g, '').replace(/&nbsp;/gi, ' ').trim()) {
                        vm.error = 'Please enter notification template text.';
                        return;
                    }

                    var model = {
                        Id: vm.templateRecordId || 0,
                        TypeId: parseInt(vm.type, 10),
                        NtFor: vm.entityType,
                        NTypeId: parseInt(vm.selectedId, 10),
                        TempText: vm.templateHtml,
                        IsActive: vm.isActive ? 1 : 0,
                        IsScreenPopup: vm.screenPopup ? 1 : 0,
                        IsScreenFreeze: (vm.screenPopup && vm.freezeScreen) ? 1 : 0,
                        WhatsappTemplateId: vm.templateCode || ''
                    };

                    vm.savingTemplate = true;

                    $http.post('setnotification.aspx/SaveNotificationTemplate', {
                        model: model
                    }).then(function (response) {
                        vm.savingTemplate = false;
                        var data = response.data && response.data.d ? response.data.d : response.data;

                        if (!data || data.Success === false) {
                            vm.error = data && data.Message
                                ? data.Message
                                : 'Unable to save notification template.';
                            return;
                        }

                        vm.templateRecordId = data.Id || vm.templateRecordId;
                        vm.showTopSuccess(data.Message || 'Template saved successfully.');
                    }, function () {
                        vm.savingTemplate = false;
                        vm.error = 'Unable to save notification template.';
                    });
                };

                vm.loadMasters();
                vm.loadPlaceholders();
            }]);
        })();
    </script>

</asp:Content>
