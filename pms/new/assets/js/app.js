var app = angular.module("app", []);

app.controller("quotationCtrl", function ($scope) {

    $scope.showLeft = true;
    $scope.showRight = true;

    $scope.items = [{ name:"", qty:1, rate:0 }];
    $scope.total = 0;

    $scope.addRow = function () {
        $scope.items.push({ name:"", qty:1, rate:0 });
    };

    $scope.removeRow = function (i) {
        $scope.items.splice(i,1);
        $scope.calculate();
    };

    $scope.calculate = function () {
        var t = 0;
        angular.forEach($scope.items, function (x) {
            t += (x.qty||0)*(x.rate||0);
        });
        $scope.total = t;
    };

    $scope.toggleLeft = function () {
        $scope.showLeft = !$scope.showLeft;
    };

    $scope.toggleRight = function () {
        $scope.showRight = !$scope.showRight;
    };

});
