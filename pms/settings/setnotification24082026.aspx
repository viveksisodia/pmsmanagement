<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="setnotification24082026.aspx.cs" Inherits="setnotification" %>

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
            padding:5px;
        }
        .nt-card-title { font-size:14px; font-weight:600; margin-bottom:8px; }

        .nt-form-row {
            display:grid;
            grid-template-columns:1fr 1fr;
            gap:14px;
        }

        .nt-label { display:block; font-size:13px; font-weight:600; margin-bottom:6px; }
        .nt-required { color:#d9534f; }
        .nt-select, .nt-input {
            width:100%;
            height:36px;
            border:1px solid #ccc;
            border-radius:3px;
            padding:6px 9px;
            background:#fff;
        }
        .nt-field { margin-bottom:14px; }

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
            min-height:300px;
            max-height:480px;
            overflow:auto;
            border:1px solid #ccc;
            padding:10px;
            outline:none;
            font-size:13px;
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
        .nt-message.info { color:#31708f; background:#d9edf7; border:1px solid #bce8f1; }

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
                            Critical Workflows
                        </a>
                    </li>
                    <li>
                        <a href="setnotification.aspx?type=223"
                           ng-class="{active: vm.type === '223'}">
                            QC Workflows
                        </a>
                    </li>
                </ul>
            </div>

            <div class="nt-main">
                <div class="nt-heading">{{vm.pageTitle}}</div>
                <%--class="nt-content-grid"--%>
                <div >

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
                                <label class="nt-label">
                                    {{vm.selectorLabel}} <span class="nt-required">*</span>
                                </label>
                                <select class="nt-select"
                                        ng-model="vm.selectedId"
                                        ng-options="x.Id as x.Name for x in vm.items track by x.Id">
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
                                        Fields from {{vm.placeholderTableName || 'selected PMS table'}}. Click any field to insert it into the message.
                                    </div>
                                </div>

                                <button type="button"
                                        class="nt-collapse-btn"
                                        ng-click="$event.stopPropagation(); vm.placeholdersExpanded = !vm.placeholdersExpanded">
                                    {{vm.placeholdersExpanded ? 'Collapse ' : 'Expand '}}
                                </button>
                            </div>

                            <div class="nt-placeholder-body" ng-show="vm.placeholdersExpanded">
                                <div class="nt-placeholder-wrap" ng-if="!vm.loadingPlaceholders && vm.placeholders.length">
                                    <span class="nt-placeholder"
                                          ng-repeat="p in vm.placeholders track by p.Token"
                                          ng-click="vm.insertPlaceholder(p.Token)"
                                          title="{{p.DisplayName}} ({{p.ColumnName}})">
                                        {{p.Token}}
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

                        <div class="nt-field">
                            <label class="nt-label">Whatsapp Template Id</label>
                            <input type="text"
                                   class="nt-input"
                                   ng-model="vm.templateCode"
                                   placeholder="Enter Template Id" />
                        </div>

                        <div class="nt-actions">
                            <button type="button"
                                    class="nt-btn nt-btn-primary"
                                    ng-click="vm.downloadTemplate()"
                                    ng-disabled="!vm.templateHtml">
                                Download Template
                            </button>

                            <button type="button"
                                    class="nt-btn nt-btn-success"
                                    ng-click="vm.showSaveNextStep()">
                                Save Template
                            </button>

                            <button type="button"
                                    class="nt-btn nt-btn-default"
                                    ng-click="vm.resetTemplate()">
                                Reset
                            </button>
                        </div>

                        <div class="nt-message error" ng-if="vm.error">{{vm.error}}</div>
                        <div class="nt-message info" ng-if="vm.info">{{vm.info}}</div>
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

                        ngModel.$render = function () {
                            element.html(ngModel.$viewValue || '');
                        };

                        element.on('blur keyup paste input', read);

                        scope.$on('$destroy', function () {
                            element.off('blur keyup paste input', read);
                        });
                    }
                };
            });

            app.controller('NotificationTemplateController', ['$http', '$sce', function ($http, $sce) {
                var vm = this;

                vm.type = '<%= NotificationType %>';
                vm.pageTitle = vm.type === '223'
                    ? 'Set Notification Template - QC Workflows'
                    : 'Set Notification Template - Critical Workflows';

                vm.selectorLabel = vm.type === '223' ? 'QC Workflow' : 'Critical Label';
                vm.selectorPlaceholder = vm.type === '223' ? 'Select QC Workflow' : 'Select Critical Label';

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
                vm.templateHtml = '';
                vm.templateCode = '';
                vm.error = '';
                vm.info = '';
                vm.loadingPlaceholders = false;
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

                vm.onEntityTypeChanged = function () {
                    vm.info = '';
                    vm.loadPlaceholders();
                };

                vm.syncEditorModel = function () {
                    var editor = document.getElementById('notificationEditor');
                    if (editor)
                        vm.templateHtml = editor.innerHTML;
                };

                vm.format = function (command) {
                    var editor = document.getElementById('notificationEditor');
                    if (!editor) return;

                    editor.focus();
                    document.execCommand(command, false, null);
                    vm.syncEditorModel();
                };

                vm.formatWithValue = function (command, value) {
                    var editor = document.getElementById('notificationEditor');
                    if (!editor || !value) return;

                    editor.focus();

                    // Chrome/Edge support hiliteColor. BackColor is a safe fallback.
                    var success = document.execCommand(command, false, value);
                    if (!success && command === 'hiliteColor')
                        document.execCommand('backColor', false, value);

                    vm.syncEditorModel();
                };

                vm.insertPlaceholder = function (token) {
                    if (!token) return;

                    var editor = document.getElementById('notificationEditor');
                    if (!editor) return;

                    editor.focus();

                    var selection = window.getSelection();
                    var inserted = false;

                    if (selection && selection.rangeCount > 0) {
                        var range = selection.getRangeAt(0);

                        if (editor.contains(range.commonAncestorContainer)) {
                            range.deleteContents();
                            var node = document.createTextNode(token);
                            range.insertNode(node);
                            range.setStartAfter(node);
                            range.setEndAfter(node);
                            selection.removeAllRanges();
                            selection.addRange(range);
                            inserted = true;
                        }
                    }

                    if (!inserted) {
                        if (editor.innerHTML && editor.innerHTML.trim() !== '')
                            editor.appendChild(document.createTextNode(' '));

                        editor.appendChild(document.createTextNode(token));
                    }

                    vm.templateHtml = editor.innerHTML;
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

                    var blob = new Blob([text], { type: 'text/plain;charset=utf-8' });
                    var url = URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = (vm.templateCode ||
                        (vm.type === '223' ? 'qc_workflow_template' : 'critical_workflow_template')) + '.txt';
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                    URL.revokeObjectURL(url);
                };

                vm.resetTemplate = function () {
                    vm.templateHtml = '';
                    vm.templateCode = '';
                    vm.error = '';
                    vm.info = '';
                    vm.selectedId = '';

                    var editor = document.getElementById('notificationEditor');
                    if (editor)
                        editor.innerHTML = '';
                };

                vm.showSaveNextStep = function () {
                    vm.error = '';
                    vm.info = 'Form UI is ready. Database save/update can be connected in the next step.';
                };

                vm.loadMasters();
                vm.loadPlaceholders();
            }]);
        })();
    </script>

</asp:Content>
