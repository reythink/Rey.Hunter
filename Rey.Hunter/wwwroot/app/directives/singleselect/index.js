(function () {
    'use strict';

    angular.module('app')
        .directive('reySingleSelect', ['$http', function ($http) {
            return {
                restrict: 'E',
                template: `
<ui-select ng-model="$parent.ngModel">
    <ui-select-match placeholder="{{reyPlaceholder}}">{{getSelected($select.selected)}}</ui-select-match>
    <ui-select-choices repeat="item in items | propsFilter: {name: $select.search}"
                       refresh="search($select.search)"
                       refresh-delay="0">
        <div ng-bind-html="getSelect(item) | highlight: $select.search | textLevel: item: options.tree()"></div>
    </ui-select-choices>
</ui-select>`,
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
                                    }

                                    stack = node.children.concat(stack);
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
        }]);
})();