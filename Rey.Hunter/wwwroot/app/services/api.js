(function () {
    'use strict';

    angular
        .module('app')
        .service('api', ['$http', '$resource', 'options', function ($http, $resource, options) {
            var check_resp = function (data, result) {
                if (data.err_code !== 0) {
                    console.error(data.err_code, data.err_msg);
                }

                if (typeof (result) !== 'undefined') {
                    result['err'] = { code: data.err_code, msg: data.err_msg };
                    return result;
                }

                return data;
            };

            var init_batch_delete = function (r, url) {
                r.batch_delete = function (id_list, success) {
                    return $http.delete(url, {
                        data: id_list,
                        headers: { 'Content-type': 'application/json' }
                    }).then(success);
                };
                return r;
            };

            var init_concrete = function (r) {
                var get_children = function (nodes, prop) {
                    var children = [];
                    for (var i = 0, ilen = nodes.length; i < ilen; ++i) {
                        var node = nodes[i];
                        var model = node.model[prop];
                        if (angular.isArray(model)) {
                            for (var j = 0, jlen = model.length; j < jlen; ++j) {
                                children.push({ parent: node.model, prop: prop, model: model[j] });
                            }
                        } else {
                            children.push({ parent: node.model, prop: prop, model: model });
                        }
                    }
                    return children;
                };

                r.concrete = function (model, props, success) {
                    if (angular.isString(props)) { props = [props]; }
                    var nodes = [{ parent: null, prop: null, model: model }];

                    while (props.length > 0) {
                        var prop = props.shift();
                        nodes = get_children(nodes, prop);
                    }

                    for (var i = 0, len = nodes.length; i < len; ++i) {
                        var node = nodes[i];
                        if (node.model && node.model.id) {
                            node.parent[node.prop] = r.get({ id: node.model.id }, success);
                        }
                    }
                };
                return r;
            };

            var actions = {
                'get': {
                    method: 'GET',
                    transformResponse: function (data, headers, status) {
                        data = angular.fromJson(data);
                        return check_resp(data, data.model);
                    }
                },
                'query': {
                    method: 'GET',
                    isArray: true,
                    transformResponse: function (data, headers, status) {
                        data = angular.fromJson(data);
                        return check_resp(data, data.models);
                    }
                },
                'save': {
                    method: 'POST',
                    transformResponse: function (data, headers, status) {
                        data = angular.fromJson(data);
                        return check_resp(data, data.model || {});
                    }
                },
                'delete': {
                    method: 'DELETE',
                    transformResponse: function (data, headers, status) {
                        data = angular.fromJson(data);
                        return check_resp(data);
                    }
                }
            };

            var init = function (url) {
                url = url.trimRight('/')
                var r = $resource(url + '/:id', { id: '@id' }, actions);
                init_batch_delete(r, url);
                init_concrete(r);
                return r;
            };

            //! initialize with app options.
            for (var i = 0, len = options.api.url.length; i < len; ++i) {
                var item = options.api.url[i];
                this[item.name] = init(item.url);
            }
        }]);
})();