(function () {
    var app = angular.module('rey-single-select', ['ui.select', 'ngSanitize']);

    app.filter('textLevel', function () {
        return function (input, item) {
            var level = item.level || 0;
            var ret = input;
            for (var i = 0; i < level; ++i) {
                ret = '&nbsp;&nbsp;&nbsp;&nbsp;' + ret;
            }
            return ret;;
        };
    });

    app.filter('propsFilter', function () {
        return function (items, props) {
            var out = [];

            if (angular.isArray(items)) {
                var keys = Object.keys(props);

                items.forEach(function (item) {
                    var itemMatches = false;

                    for (var i = 0; i < keys.length; i++) {
                        var prop = keys[i];
                        var text = props[prop].toLowerCase();
                        if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                            itemMatches = true;
                            break;
                        }
                    }

                    if (itemMatches) {
                        out.push(item);
                    }
                });
            } else {
                // Let the output be the input untouched
                out = items;
            }

            return out;
        };
    });

    app.directive('reySingleSelect', function ($http) {
        return {
            restrict: 'E',
            templateUrl: '/app/lib/singleselect/index.html?r=' + Math.random(),
            scope: {
                reyPlaceholder: '@',
                reyAttrSelect: '@',
                reyAttrSelected: '@',
                reyUri: '@',
                reyTree: '@',
                ngModel: '='
            },
            link: function (scope, element, attrs) {
                var options = scope.options = {
                    tree: function () {
                        return typeof (scope.reyTree) !== 'undefined';
                    }
                };

                scope.search = function (text) {
                    $http.get(scope.reyUri).then(function (resp) {
                        if (options.tree()) {
                            var items = [];
                            var stack = resp.data.model.children;

                            while (stack.length > 0) {
                                var node = stack.shift();
                                items.push(node);

                                var models = scope.ngModel || [];
                                for (var i = 0, len = models.length; i < len; ++i) {
                                    if (models[i].id === node.id) {
                                        models[i] = node;
                                    }
                                }

                                for (var i = 0, len = node.children.length; i < len; ++i) {
                                    var child = node.children[i];
                                    child.level = (node.level || 0) + 1;
                                    stack = [child].concat(stack);
                                }
                            }

                            scope.items = items;
                        } else {
                            scope.items = resp.data.models;
                        }
                    });
                };

                scope.getSelect = function (item) {
                    if (typeof (item) === 'undefined' || item === null) { return item; }
                    return item[scope.reyAttrSelect || 'name'];
                };

                scope.getSelected = function (item) {
                    if (typeof (item) === 'undefined' || item === null) { return item; }
                    return item[scope.reyAttrSelected || 'name'];
                };
            }
        };
    });
})();