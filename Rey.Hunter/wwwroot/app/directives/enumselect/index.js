(function () {
    'use strict';

    angular.module('app')
        .directive('reyEnumSelect', ['$http', function ($http) {
            return {
                restrict: 'E',
                template: `
<select class="form-control" ng-model="ngModel">
    <option ng-if="reyDefault" value="" ng-bind="reyDefault"></option>
    <option ng-repeat="model in models" ng-value="getValue(model)" ng-bind="getName(model)"></option>
</select>`,
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
        }]);
})();