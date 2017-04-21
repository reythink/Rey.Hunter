(function () {
    'use strict';

    class Selector {
        constructor() {
            this._items = [];

        }

        register(value, checked) {
            var item = this.find(value);
            if (item !== null) {
                return item;
            }

            item = new SelectorItem(this, value, checked);
            this._items.push(item);
            return item;
        }

        check(checked, value) {
            if (typeof (value) === 'undefined') {
                for (var i = 0, len = this._items.length; i < len; ++i) {
                    this.check(checked, this._items[i].value);
                }
                return;
            }

            this.find(value).check(checked);
        }

        checked(value) {
            if (typeof (value) === 'undefined') {
                for (var i = 0, len = this._items.length; i < len; ++i) {
                    if (this._items[i].checked === false) {
                        return false;
                    }
                }
                return true;
            }

            return this.find(value).checked;
        }

        toggle(value) {
            this.check(!this.checked(value), value);
        }

        find(value) {
            for (var i = 0, len = this._items.length; i < len; ++i) {
                if (this._items[i].value === value) {
                    return this._items[i];
                }
            }
            return null;
        }

        items(checked) {
            if (typeof (checked) === 'undefined') {
                return this._items;
            }

            var temp = [];
            for (var i = 0, len = this._items.length; i < len; ++i) {
                if (this._items[i].checked === checked) {
                    temp.push(this._items[i]);
                }
            }
            return temp;
        }

        values(checked) {
            var temp = [];
            for (var i = 0, len = this._items.length; i < len; ++i) {
                if (typeof (checked) === 'undefined'
                    || this._items[i].checked === checked) {
                    temp.push(this._items[i].value);
                }
            }
            return temp;
        }
    }

    class SelectorItem {
        constructor(selector, value, checked) {
            this._selector = selector;
            this._value = value;
            this._checked = checked || false;
            this._onChanged = [];
        }

        get selector() { return this._selector; }
        get value() { return this._value; }
        get checked() { return this._checked; }

        onChanged(func) {
            this._onChanged.push(func);
            return this;
        }

        check(checked) {
            if (this._checked === checked) { return; }
            this._checked = checked;
            for (var i = 0, len = this._onChanged.length; i < len; ++i) {
                this._onChanged[i].call(this, checked);
            }
        }
    }

    angular.module('app')
        .service('selector', function () {
            this.create = function () {
                return new Selector();
            };
        })
        .directive('reySelectorAll', [function () {
            return {
                restrict: 'E',
                transclude: true,
                template: `
<button type="button" ng-class ="reyClass" ng-click="toggle()">
    <i ng-class ="icon_cls()"></i>
    <span ng-transclude></span>
</button>`,
                scope: {
                    reySelector: '=',
                    reyClass: '@'
                },
                link: function (scope, element, attrs) {
                    var selector = scope.reySelector;
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
        .directive('reySelectorItem', [function () {
            return {
                restrict: 'E',
                template: `<checkbox ng-model="checked" ng-change="change()" class="select-item"></checkbox>`,
                scope: {
                    reySelector: '=',
                    reyValue: '@'
                },
                link: function (scope, element, attrs) {
                    var selector = scope.reySelector;
                    var value = scope.reyValue;

                    var item = selector.register(value).onChanged(function (checked) {
                        scope.checked = checked;
                    });
                    scope.checked = item.checked;

                    scope.change = function () {
                        item.check(scope.checked);
                    };
                }
            };
        }]);
})();