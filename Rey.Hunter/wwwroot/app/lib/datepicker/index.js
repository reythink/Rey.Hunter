(function () {
    var app = angular.module('rey-date-picker', []);

    app.directive('reyDatePicker', function ($http) {
        return {
            restrict: 'E',
            templateUrl: '/app/lib/datepicker/index.html?r=' + Math.random(),
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
    });
})();