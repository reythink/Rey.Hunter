(function () {
    'use strict';

    angular.module('app')
        .directive('reyDatePicker', [function () {
            return {
                restrict: 'E',
                template: `
<div class="input-group">
    <input type="text" class="form-control" uib-datepicker-popup="{{format}}" ng-model="ngModel" is-open="opened" datepicker-options="options" close-text="Close" />
    <span class="input-group-btn">
        <button type="button" class="btn btn-default" ng-click="opened = !opened"><i class="glyphicon glyphicon-calendar"></i></button>
    </span>
</div>`,
                scope: {
                    reyPlaceholder: '@',
                    reyFormat: '@',
                    reyBeginToday: '@',         //! 今天之后的日期有效
                    reyEndToday: '@',           //! 今天之前的日期有效
                    ngModel: '='
                },
                link: function (scope, element, attrs) {
                    var options = scope.options = {
                        startingDay: 1
                    };

                    scope.format = scope.reyFormat || 'yyyy/MM/dd';

                    if (typeof (scope.reyBeginToday) !== 'undefined') {
                        options.minDate = new Date();
                    }

                    if (typeof (scope.reyEndToday) !== 'undefined') {
                        options.maxDate = new Date();
                    }
                }
            };
        }]);
})();