(function () {
    'use strict';

    angular
        .module('app')
        .factory('transfer', [function () {
            return function (model, config) {
                if (typeof (model) === 'undefined' || model === null) {
                    console.error('model is undefined or null!');
                }

                if (typeof (config) === 'undefined' || config === null) {
                    console.error('config is undefined or null!');
                }

                for (var name in config) {
                    var value = model[name];
                    if (typeof (value) === 'undefined') {
                        continue;
                    }

                    model[name] = config[name](value);
                }
                return model;
            };
        }]);
})();