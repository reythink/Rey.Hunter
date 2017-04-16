(function () {
    'use strict';

    angular
        .module('app')
        .service('generator', ['', function () {
            this.range = function (from, size, reverse) {
                var type = typeof (from);
                if (type !== 'number' && type !== 'string') {
                    console.error('from must be a number or string!');
                }

                var min = type === 'string' ? parseInt(from, 10) : from;
                var max = min + size;

                if (min > max) {
                    var value = min;
                    min = max;
                    max = value;
                }

                var result = [];
                var push = function (i) {
                    result.push(type === 'string' ? i.toString() : type === 'number' ? i : null);
                };

                if (reverse === true) {
                    for (var i = max; i >= min; --i) {
                        push(i);
                    }
                } else {
                    for (var i = min; i <= max; ++i) {
                        push(i);
                    }
                }

                return result;
            };
        }]);
})();