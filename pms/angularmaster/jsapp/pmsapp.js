var app = angular.module("PMSerp", ['angularUtils.directives.dirPagination', 'ngSanitize','ui.mask']);
app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix('');
}]);
app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function(scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function() {
                scope.$apply(function() {
                    modelSetter(scope, element[0].files);
                });
            });
        }
    };
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
app.directive('ngPopover', function () {
    try {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.on('click', function () {
                    var pop = new bootstrap.Popover(element[0], {
                        content: attrs.content || '',
                        html: true,
                        placement: attrs.placement || 'top',
                        trigger: 'focus'
                    });
                    pop.toggle();
                });
            }
        };
    }
    catch{}
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
app.directive("milestoneStart", function () {
    return {

        require: "ngModel",
        scope: {
            projectStart: "="
        },
        link: function (scope, elem, attr, ngModel) {
debugger
            ngModel.$validators.milestoneStart = function (value) {

                if (!value || !scope.projectStart) return true;

                return new Date(value) >= new Date(scope.projectStart);
            };

            // revalidate if project date changes
            scope.$watch("projectStart", function () {
                ngModel.$validate();
            });
        }
    };
});
app.directive("milestoneEnd", function () {
    return {
        require: "ngModel",
        scope: {
            startDate: "=",
            projectEnd: "="
        },
        link: function (scope, elem, attr, ngModel) {

            ngModel.$validators.milestoneEnd = function (value) {

                if (!value) return true;

                let end = new Date(value);
                let start = new Date(scope.startDate);
                let projectEnd = new Date(scope.projectEnd);

                // 1. End >= Start
                if (scope.startDate && end < start) return false;

                // 2. End <= Project End
                if (scope.projectEnd && end > projectEnd) return false;

                return true;
            };

            scope.$watchGroup(["startDate", "projectEnd"], function () {
                ngModel.$validate();
            });
        }
    };
});
app.directive("clickOutside", function ($document) {
    return {
        restrict: "A",
        scope: {
            clickOutside: "&"
        },
        link: function (scope, element) {
            var handler = function (event) {
                if (!element[0].contains(event.target)) {
                    scope.$apply(scope.clickOutside);
                }
            };

            $document.on("click", handler);

            scope.$on("$destroy", function () {
                $document.off("click", handler);
            });
        }
    };
});
app.directive('bsTooltip', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            $timeout(function () {
                new bootstrap.Tooltip(element[0]);
            });

            scope.$watch(attrs.bsTooltip, function () {
                $timeout(function () {
                    let instance = bootstrap.Tooltip.getInstance(element[0]);
                    if (instance) instance.dispose();
                    new bootstrap.Tooltip(element[0]);
                });
            });
        }
    };
});






