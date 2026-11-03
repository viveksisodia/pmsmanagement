<%@ Page Title="" Language="C#" MasterPageFile="~/pms/MasterPage.master" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.8.2/angular.min.js"></script>

  <style>
    body { font-family: Arial; margin:0; }

    .grid-container {
      width: 100%;
    }

    /* HEADER */
    .grid-header {
      display: grid;
      grid-template-columns: 80px 100px 1fr 120px 1fr;
      background: #f5f5f5;
      border: 1px solid #ccc;
      font-weight: bold;
    }

    .grid-header div {
      padding: 8px;
      border-right: 1px solid #ccc;
      cursor: pointer;
    }

    /* ROW */
    .grid-row {
      display: grid;
      grid-template-columns: 80px 100px 1fr 120px 1fr;
      border: 1px solid #eee;
      border-top: none;
    }

    .grid-row div {
      padding: 8px;
      border-right: 1px solid #eee;
    }

    .grid-row:hover {
      background: #f9f9f9;
    }

    .sort-icon {
      margin-left: 5px;
      font-size: 12px;
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="table-fix-wrap" style="height: calc(100vh - 90px);overflow:auto;" id="projectTableContainer">

<div ng-app="app" ng-controller="Ctrl">

  <div class="grid-container">

    <!-- HEADER -->
    <div class="grid-header">

      <div ng-repeat="col in columns"
           ng-click="sortBy(col.key)">

        {{col.label}}

        <span class="sort-icon" ng-if="sortKey === col.key">
          {{reverse ? '▼' : '▲'}}
        </span>

      </div>

    </div>

    <!-- ROWS -->
    <div class="grid-row"
         ng-repeat="row in data | orderBy:sortKey:reverse">

      <div ng-repeat="col in columns">
        {{row[col.key]}}
      </div>

    </div>

  </div>

</div>
    </div>
<!-- SCRIPT OUTSIDE -->
<script>
var app = angular.module('app', []);

app.controller('Ctrl', function($scope) {

  $scope.columns = [
    { key: 'sno', label: 'S.No.' },
    { key: 'action', label: 'Action' },
    { key: 'client', label: 'Client Name' },
    { key: 'projectId', label: 'Project ID' },
    { key: 'projectName', label: 'Project Name' }
  ];

  $scope.data = [
    { sno:1, action:'Edit', client:'ABC Ltd', projectId:'P101', projectName:'Website Revamp' },
    { sno:2, action:'Edit', client:'XYZ Corp', projectId:'P102', projectName:'Mobile App' },
    { sno:3, action:'Edit', client:'TechSoft', projectId:'P103', projectName:'CRM System' }
  ];

  $scope.sortKey = 'sno';
  $scope.reverse = false;

  $scope.sortBy = function(key) {
    if ($scope.sortKey === key) {
      $scope.reverse = !$scope.reverse;
    } else {
      $scope.sortKey = key;
      $scope.reverse = false;
    }
  };

});
</script>

</asp:Content>

