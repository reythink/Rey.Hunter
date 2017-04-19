(function () {
    'use strict';

    /**************************************************
    * QueryParameter
    **************************************************/
    class QueryParameter {
        constructor(name, value) {
            this._name = name;
            this._value = value;
        }

        get name() { return this._name; }
        get value() { return this._value; }
        set value(value) { this._value = value; }

        get text() { return this._name + '=' + encodeURI(this._value); }

        static findByName(parameters, name) {
            for (var i = 0, len = parameters.length; i < len; ++i) {
                var parameter = parameters[i];
                if (parameter.name === name) {
                    return parameter;
                }
            }
            return null;
        }

        static parse(text) {
            var ret = text.match(/^(.*?)=(.*?)$/i);
            if (ret === null || ret.length < 2) {
                return null;
            }
            return new QueryParameter(ret[1], decodeURI(ret[2]));
        }
    }

    /**************************************************
    * QueryString
    **************************************************/
    class QueryString {
        constructor() {
            this._parameters = [];
        }

        get parameters() { return this._parameters; }
        get search() {
            var list = [];
            for (var i = 0, len = this._parameters.length; i < len; ++i) {
                list.push(this._parameters[i].text);
            }
            return '?' + list.join('&');
        }

        addParameter(parameter) {
            var found = QueryParameter.findByName(this._parameters, parameter.name);
            if (found === null) {
                this._parameters.push(parameter);
            } else {
                found.value = parameter.value;
            }
            return this;
        }

        addParameters(parameters) {
            for (var i = 0, len = parameters.length; i < len; ++i) {
                this.addParameter(parameters[i]);
            }
            return this;
        }

        parameter(name, value) {
            return this.addParameter(new QueryParameter(name, value));
        }

        static parse(search) {
            var query = new QueryString();

            if (typeof (search) === 'undefined'
                || search === null
                || search.length === 0) {
                return query;
            }

            search = search.replace(/^\?/, '');
            angular.forEach(search.split('&'), function (text) {
                query.addParameter(QueryParameter.parse(text));
            });
            return query;
        }

        static combine(query1, query2) {
            return new QueryString()
                .addParameters(query1.parameters)
                .addParameters(query2.parameters);
        }
    }

    /**************************************************
    * Filter
    **************************************************/
    class Filter {
        constructor() {
            this._categories = [];
            this._query = new QueryString();
        }

        get categories() { return this._categories; }
        get hasItem() {
            for (var i = 0, len = this._categories.length; i < len; ++i) {
                if (this._categories[i].hasItem) { return true; }
            }
            return false;
        }
        get href() {
            var query = new QueryString();
            for (var i = 0, len = this._categories.length; i < len; ++i) {
                query.addParameters(this._categories[i].parameters);
            }
            query.addParameters(this._query.parameters);
            return location.origin + location.pathname + query.search;
        }

        init(search) {
            search = search || location.search;
            this._query.addParameters(QueryString.parse(search).parameters);
            return this;
        }

        initCategory(name, text, textGetter) {
            if (typeof (name) === 'undefined' || name === null) {
                console.error('name is empty!');
                return this;
            }

            let category = FilterCategory.findByName(this._categories, name);
            if (category === null) {
                category = new FilterCategory(this, name, text);
                this._categories.push(category);
            }

            let reg = new RegExp('^' + name + '\\[\\d+\\]$', 'i');
            var items = [];

            for (var i = this._query.parameters.length - 1; i >= 0; --i) {
                var parameter = this._query.parameters[i];
                if (parameter.name.match(reg) != null) {
                    var text = parameter.value;
                    if (textGetter) { text = textGetter.getText(text); }
                    items.push({ value: parameter.value, text: text });
                    this._query.parameters.splice(i, 1);
                }
            }

            items.forEach((item) => {
                category.item(item.value, item.text);
            });

            if (this.hasItem) {
                $('body').addClass('control-sidebar-open');
            }

            return this;
        }

        getCategoryByName(name) {
            return FilterCategory.findByName(this._categories, name);
        }

        removeCategory(category) {
            FilterCategory.remove(this._categories, category);
        }

        get() {
            console.log(this.href);
            location.href = this.href;
        }
    }

    /**************************************************
    * FilterCategory
    **************************************************/
    class FilterCategory {
        constructor(filter, name, text) {
            this._filter = filter;
            this._name = name;
            this._text = text || name;
            this._items = [];
        }

        get filter() { return this._filter; }
        get name() { return this._name; }
        get text() { return this._text; }
        get items() { return this._items; }
        get hasItem() { return this._items.length > 0; }

        get parameters() {
            var list = [];
            for (var i = 0, len = this._items.length; i < len; ++i) {
                var item = this._items[i];
                list.push(new QueryParameter(this._name + '[' + i + ']', item.value));
            }
            return list;
        }

        item(value, text) {
            let item = FilterItem.findByValue(this._items, value);
            if (item === null) {
                item = new FilterItem(this, value, text);
                this._items.push(item);
            }
            return item;
        }

        removeItem(item) {
            FilterItem.remove(this._items, item);
        }

        clearItems() {
            this._items = [];
        }

        remove() {
            this._filter.removeCategory(this);
        }

        static findByName(categories, name) {
            for (var i = 0, len = categories.length; i < len; ++i) {
                let category = categories[i];
                if (category._name === name) {
                    return category;
                }
            }
            return null;
        }

        static remove(categories, category) {
            for (var i = 0, len = categories.length; i < len; ++i) {
                if (categories[i] === category) {
                    categories.splice(i, 1);
                    break;
                }
            }
        }
    }

    /**************************************************
    * FilterItem
    **************************************************/
    class FilterItem {
        constructor(category, value, text) {
            this._category = category;
            this._value = value;
            this._text = text || value;
        }

        get category() { return this._category; }
        get value() { return this._value; }
        get text() { return this._text; }

        remove() {
            this._category.removeItem(this);
        }

        static findByValue(items, value) {
            for (var i = 0, len = items.length; i < len; ++i) {
                let item = items[i];
                if (item._value == value) {
                    return item;
                }
            }
            return null;
        }

        static remove(items, item) {
            for (var i = 0, len = items.length; i < len; ++i) {
                if (items[i] === item) {
                    items.splice(i, 1);
                    break;
                }
            }
        }
    }

    /**************************************************
    * EnumTextGetter
    **************************************************/
    class EnumTextGetter {
        constructor(models, valueField, textField) {
            this._models = models;
            this._valueField = valueField || 'value';
            this._textField = textField || 'desc';
        }

        getText(value) {
            for (var i = 0, len = this._models.length; i < len; ++i) {
                if (this._models[i][this._valueField] == value) {
                    return this._models[i][this._textField];
                }
            }
            return null;
        }
    }

    /**************************************************
    * TreeTextGetter
    **************************************************/
    class TreeTextGetter {
        constructor(model, valueField, textField) {
            this._model = model;
            this._valueField = valueField || 'id';
            this._textField = textField || 'name';
        }

        getText(value) {
            var stack = [this._model];
            while (stack.length > 0) {
                var node = stack.shift();

                if (node[this._valueField] == value) {
                    return node[this._textField];
                }

                if (angular.isArray(node.children)) {
                    stack = node.children.concat(stack);
                }
            }
        }
    }

    angular.module('app')
        .factory('filter', [function () {
            return new Filter().init();
        }])
        .directive('reyFilterPanel', ['filter', function (filter) {
            return {
                restrict: 'AE',
                template: `<div class="filter-panel"></div>`,
                transclude: true,
                scope: {
                },
                link: function (scope, element, attrs, ctrl, transclude) {
                    transclude(function (clone, transcope) {
                        transcope.$filter = filter;
                        element.find('div[class="filter-panel"]').append(clone);
                    });
                }
            };
        }])
        .directive('reyFilterCategory', ['filter', '$http', function (filter, $http) {
            return {
                restrict: 'AE',
                template: `<div class="filter-category"></div>`,
                transclude: true,
                scope: {
                    reyName: '@',
                    reyText: '@',
                    reyUri: '@',
                    reyStructure: '@'           //! enum | tree
                },
                link: function (scope, element, attrs, ctrl, transclude) {
                    var getTextGetter = function (structure, data) {
                        if (structure === 'enum') { return new EnumTextGetter(data.models); }
                        if (structure === 'tree') { return new TreeTextGetter(data.model); }
                        return new EnumTextGetter(data.models);
                    };

                    var initFilter = function (uri, structure, textGetter) {
                        filter.initCategory(scope.reyName, scope.reyText, textGetter);
                        transclude(function (clone, transcope) {
                            transcope.$category = filter.getCategoryByName(scope.reyName);
                            element.find('div[class="filter-category"]').append(clone);
                        });
                    };

                    var init = function (uri, structure) {
                        if (typeof (uri) === 'undefined' || uri === null) {
                            initFilter(uri, structure);
                            return;
                        }

                        $http.get(uri).then(function (resp) {
                            initFilter(uri, structure, getTextGetter(structure, resp.data));
                        });
                    };

                    init(scope.reyUri, scope.reyStructure);
                }
            };
        }]);
})();