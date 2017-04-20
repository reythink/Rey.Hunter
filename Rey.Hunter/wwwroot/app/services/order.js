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

        get(name) {
            return QueryParameter.findByName(this.parameters, name);
        }

        remove(name) {
            for (var i = 0; i < this._parameters.length; ++i) {
                var item = this._parameters[i];
                if (item.name === name) {
                    this._parameters.splice(i, 1);
                    return;
                }
            }
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

    class Order {
        constructor(options) {
            this._options = angular.extend({}, Order.defaults.options, options);
        }

        get search() { return location.search || this._options.search; }
        get nameBy() { return this._options.nameBy; }
        get nameDirection() { return this._options.nameDirection; }
        get nameText() { return this._options.nameText; }
        get query() {
            if (typeof (this._query) === 'undefined' || this._query === null) {
                this._query = QueryString.parse(this.search);
            }
            return this._query;
        }
        get current() {
            var by = this.query.get(this.nameBy);
            if (by === null) { return null; }
            var text = this.query.get(this.nameText);
            var direction = this.query.get(this.nameDirection);
            return {
                by: by.value,
                text: text === null ? by.value : text.value,
                direction: direction === null ? '' : direction.value === 'asc' ? 'Ascending' : direction.value === 'desc' ? 'Descending' : ''
            };
        }

        init(options) {
            this._options = angular.extend({}, Order.defaults.options, options);
            this._query = null;
        }

        by(name, text) {
            var by = this.query.get(this.nameBy);
            var direction = this.query.get(this.nameDirection);

            if (by !== null && by.value === name) {
                if (direction === null || direction.value === 'desc') {
                    this.query.parameter(this.nameDirection, 'asc');
                } else {
                    this.query.parameter(this.nameDirection, 'desc');
                }
            } else {
                this.query.parameter(this.nameBy, name);
                this.query.remove(this.nameDirection);
            }

            if (text) {
                this.query.parameter(this.nameText, text);
            }
            else {
                this.query.remove(this.nameText);
            }

            location.href = location.origin + location.pathname + this.query.search;
        }

        static get defaults() {
            return {
                options: { search: null, nameBy: 'orderBy', nameDirection: 'orderDirection', nameText: 'orderText' },
            }
        };
    }

    angular
        .module('app')
        .factory('order', [function () {
            return new Order();
        }]);
})();