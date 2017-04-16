(function () {
    'use strict';

    angular
        .module('app')
        .filter('gender', function () {
            return function (input) {
                return input === 1 ? 'Male'
                    : input === 2 ? 'Female'
                    : 'Unknown';
            };
        });
})();