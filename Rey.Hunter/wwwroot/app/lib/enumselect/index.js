(function () {
    var app = angular.module('rey-enum-select', []);

    app.directive('reyEnumSelect', function ($http) {
        return {
            restrict: 'E',
            templateUrl: '/app/lib/enumselect/index.html?r=' + Math.random(),
            scope: {
                reyDefault: '@',
                reyUri: '@',
                reyAttrName: '@',
                reyAttrValue: '@',
                ngModel: '='
            },
            link: function (scope, element, attrs) {
                $http.get(scope.reyUri).then(function (resp) {
                    scope.models = resp.data.models;
                });

                scope.getName = function (model) {
                    if (typeof (scope.reyAttrName) !== 'undefined') {
                        return model[scope.reyAttrName];
                    }
                    return model;
                };

                scope.getValue = function (model) {
                    if (typeof (scope.reyAttrValue) !== 'undefined') {
                        return model[scope.reyAttrValue];
                    }
                    return model;
                };
            }
        };
    });
})();