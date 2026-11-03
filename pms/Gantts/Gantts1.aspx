<%@ Page Title="" Language="C#" MasterPageFile="~/popup.master" AutoEventWireup="true" CodeFile="Gantts1.aspx.cs" Inherits="pms_Gantts_Gantts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!DOCTYPE html>
<html>
<head>
    <title>Odoo Gantt with Toolbar</title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular.min.js"></script>

    <style>
        body {
            margin: 0;
            font-family: Arial;
            background: #f5f6fa;
        }

        /* TOP TOOLBAR */
        .toolbar {
            background: #1f2d3d;
            color: white;
            padding: 10px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .toolbar input,
        .toolbar select {
            padding: 6px;
            border-radius: 4px;
            border: none;
            outline: none;
        }

        .btn {
            padding: 6px 10px;
            border: none;
            cursor: pointer;
            border-radius: 4px;
            background: #3b82f6;
            color: white;
        }

        .btn-green { background: #22c55e; }

        /* LAYOUT */
        .container {
            display: flex;
            height: calc(100vh - 50px);
        }

        .left {
            width: 280px;
            background: white;
            border-right: 1px solid #ddd;
            overflow: auto;
        }

        .right {
            flex: 1;
            overflow: auto;
            position: relative;
        }

        .row {
            height: 50px;
            border-bottom: 1px solid #eee;
            display: flex;
            align-items: center;
            padding-left: 10px;
            font-size: 13px;
        }

        .timeline-header {
            display: flex;
            position: sticky;
            top: 0;
            background: #e9edf2;
            z-index: 2;
        }

        .cell {
            border-right: 1px solid #ddd;
            text-align: center;
            font-size: 11px;
            padding: 5px 0;
        }

        .gantt-area {
            position: relative;
        }

        .gantt-row {
            height: 50px;
            border-bottom: 1px solid #eee;
            position: relative;
        }

        .bar {
            position: absolute;
            height: 24px;
            top: 13px;
            border-radius: 5px;
            color: white;
            font-size: 11px;
            padding: 3px 6px;
        }

        .p1 { background: #22c55e; }
        .p2 { background: #f59e0b; }
        .p3 { background: #06b6d4; }
    </style>
</head>

<body ng-app="app" ng-controller="ctrl">

<!-- TOOLBAR -->
<div class="toolbar">

    <b>Gantt View</b>

    <input type="text" ng-model="searchText" placeholder="Search task...">

    <select ng-model="filterOwner">
        <option value="">All Owners</option>
        <option ng-repeat="o in owners">{{o}}</option>
    </select>

    <select ng-model="filterProject">
        <option value="">All Projects</option>
        <option ng-repeat="p in projects">{{p}}</option>
    </select>

    <button class="btn" ng-click="zoom('day')">Day</button>
    <button class="btn" ng-click="zoom('week')">Week</button>

    <button class="btn btn-green" ng-click="addTask()">+ Add Task</button>

</div>

<div class="container">

    <!-- LEFT PANEL -->
    <div class="left">
        <div class="row" ng-repeat="t in filteredTasks()">
            <div>
                <b>{{t.name}}</b><br>
                <small>{{t.owner}} | {{t.project}}</small>
            </div>
        </div>
    </div>

    <!-- RIGHT PANEL -->
    <div class="right">

        <div class="timeline-header">
            <div class="cell"
                 ng-style="{'width': cellWidth + 'px'}"
                 ng-repeat="d in timeline">
                {{d}}
            </div>
        </div>

        <div class="gantt-area">

            <div class="gantt-row" ng-repeat="t in filteredTasks()">

                <div class="bar {{t.color}}"
                     ng-style="getStyle(t)">
                    {{t.name}}
                </div>

            </div>

        </div>

    </div>

</div>

<script>
var app = angular.module("app", []);

app.controller("ctrl", function($scope) {

    $scope.mode = "day";
    $scope.cellWidth = 50;

    $scope.searchText = "";
    $scope.filterOwner = "";
    $scope.filterProject = "";

    $scope.owners = ["Amit", "Rahul", "Sneha"];
    $scope.projects = ["ERP", "CRM", "Website"];

    function buildTimeline() {
        $scope.timeline = [];
        let count = ($scope.mode === "day") ? 30 : 12;

        for (let i = 1; i <= count; i++) {
            $scope.timeline.push(i);
        }
    }

    buildTimeline();

    $scope.tasks = [
        { name:"UI Design", owner:"Amit", project:"ERP", start:2, end:6, color:"p1" },
        { name:"API Dev", owner:"Rahul", project:"CRM", start:5, end:14, color:"p2" },
        { name:"Testing", owner:"Sneha", project:"Website", start:10, end:20, color:"p3" }
    ];

    $scope.zoom = function(type) {
        $scope.mode = type;
        $scope.cellWidth = (type === "day") ? 50 : 80;
        buildTimeline();
    };

    $scope.getStyle = function(t) {
        return {
            left: (t.start * $scope.cellWidth) + "px",
            width: ((t.end - t.start) * $scope.cellWidth) + "px"
        };
    };

    // FILTER LOGIC (like Odoo domain filter)
    $scope.filteredTasks = function() {
        return $scope.tasks.filter(t => {

            let matchSearch =
                !$scope.searchText ||
                t.name.toLowerCase().includes($scope.searchText.toLowerCase());

            let matchOwner =
                !$scope.filterOwner || t.owner === $scope.filterOwner;

            let matchProject =
                !$scope.filterProject || t.project === $scope.filterProject;

            return matchSearch && matchOwner && matchProject;
        });
    };

    // ADD TASK
    $scope.addTask = function() {
        let newTask = {
            name: "New Task " + ($scope.tasks.length + 1),
            owner: "Amit",
            project: "ERP",
            start: 3,
            end: 8,
            color: "p1"
        };

        $scope.tasks.push(newTask);
    };

});
</script>

</body>
</html>
</asp:Content>

