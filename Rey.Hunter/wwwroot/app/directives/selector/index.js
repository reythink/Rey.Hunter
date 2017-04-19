(function () {
    'use strict';

    class Selector {
        constructor() {
            this._list = [];
        }

        register(id, changed, checked) {
            this._list.push({ id: id, changed: changed, checked: checked || false });
        }

        find(id) {
            for (var i = 0, len = this._list.length; i < len; ++i) {
                if (this._list[i].id === id) {
                    return { index: i, item: this._list[i] };
                }
            }
            return null;
        }

        checked(id) {
            if (typeof (id) === 'undefined') {
                for (var i = 0, len = this._list.length; i < len; ++i) {
                    if (this._list[i].checked === false) {
                        return false;
                    }
                }
                return true;
            }

            return this.find(id).item.checked;
        }

        notify(checked, id) {
            this.find(id).item.checked = checked;
        }

        checkItem(item, checked) {
            if (item.checked !== checked) {
                item.checked = checked;
                item.changed(checked);
            }
        }

        check(checked, id) {
            if (typeof (id) === 'undefined') {
                for (var i = 0, len = this._list.length; i < len; ++i) {
                    this.checkItem(this._list[i], checked);
                }
            } else {
                this.checkItem(this.find(id), checked);
            }
        }

        toggle(id) {
            this.check(!this.checked(id), id);
        }

        data(checked) {
            if (typeof (checked) === 'undefined') {
                return this._list;
            }

            var temp = [];
            for (var i = 0, len = this._list.length; i < len; ++i) {
                if (this._list[i].checked === checked) {
                    temp.push(this._list[i]);
                }
            }
            return temp;
        }

        idList(checked) {
            var temp = [];
            for (var i = 0, len = this._list.length; i < len; ++i) {
                if (typeof (checked) === 'undefined'
                    || this._list[i].checked === checked) {
                    temp.push(this._list[i].id);
                }
            }
            return temp;
        }
    }

    angular.module('app')
        .factory('selector', function () {
            return new Selector();
        })
        .directive('reySelectorAll', ['selector', function (selector) {
            return {
                restrict: 'E',
                transclude: true,
                template: `
<button type="button" ng-class ="reyClass" ng-click="toggle()">
    <i ng-class ="icon_cls()"></i>
    <span ng-transclude></span>
</button>`,
                scope: {
                    reyClass: '@'
                },
                link: function (scope, element, attrs) {
                    scope.toggle = function () {
                        selector.toggle();
                    };

                    scope.icon_cls = function () {
                        return selector.checked() ?
                            'fa fa-fw fa-check-square-o' :
                            'fa fa-fw fa-square-o';
                    };
                }
            };
        }])
        .directive('reySelectorItem', ['selector', function (selector) {
            return {
                restrict: 'E',
                template: `<checkbox ng-model="checked" ng-change="changed()" class="select-item"></checkbox>`,
                scope: {
                    reyId: '@'
                },
                link: function (scope, element, attrs) {
                    scope.checked = false;
                    selector.register(scope.reyId, function (checked) {
                        scope.checked = checked;
                    });

                    scope.changed = function () {
                        selector.notify(scope.checked, scope.reyId);
                    };
                }
            };
        }]);
})();