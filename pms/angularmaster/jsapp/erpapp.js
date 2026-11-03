var app = angular.module("Absoluteerp", ['angularUtils.directives.dirPagination', 'ngSanitize']);
app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix('');
}]);
app.directive('customOnChange', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.on('change', function (event) {
                scope.$apply(function () {
                    scope.$eval(attrs.customOnChange, {
                        $event: event
                    });
                });
            });
        }
    };
});
app.directive('customOnDrop', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.on('dragover', function (event) {
                event.preventDefault();
            });

            element.on('drop', function (event) {
                event.preventDefault();
                scope.$apply(function () {
                    scope.$eval(attrs.customOnDrop, {
                        $event: event
                    });
                });
            });
        }
    };
});
app.directive("dropdownAutoClear", function ($timeout) {
    return {
        restrict: "A",
        require: "ngModel",
        scope: {
            ngModel: "=",
            listData: "=",
            displayField: "@",
            selectedValue: "=",
            dropdownVisible: "="
        },
        link: function (scope, element) {

            element.on("blur", function () {
                $timeout(function () {
                    var text = (scope.ngModel || "").toString().trim().toLowerCase();

                    if (!text) {
                        scope.ngModel = "";
                        scope.selectedValue = "";
                        scope.dropdownVisible = false;
                        return;
                    }

                    var matchedItem = null;

                    angular.forEach(scope.listData, function (item) {
                        if (item && item[scope.displayField]) {
                            var value = item[scope.displayField].toString().trim().toLowerCase();
                            if (value === text) {
                                matchedItem = item;
                            }
                        }
                    });

                    if (matchedItem) {
                        scope.ngModel = matchedItem[scope.displayField];
                        scope.selectedValue = matchedItem.id || matchedItem.Id || matchedItem;
                    } else {
                        scope.ngModel = "";
                        scope.selectedValue = "";
                    }

                    scope.dropdownVisible = false;
                }, 200);
            });
        }
    };
});

 