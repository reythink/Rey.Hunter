(function () {
    'use strict';

    angular
        .module('app')
        .controller('location-ctrl', [
            '$scope', '$http', 'modal',
            function ($scope, $http, modal) {
                $scope.modal = modal;
                var _url = '/api/location/';

                var init = function () {
                    $http.get(_url).then(function (resp) {
                        tree.items = resp.data.model.children;
                    });
                };

                var removeNode = function (node) {
                    $http.delete(_url + node.id).then((resp) => {
                        node.remove();
                        if (node === data.current) {
                            data.current = null;
                        }
                    });
                };

                var addNode = function () {
                    var item = { name: data.name_add };
                    if (data.current) {
                        $http.post(_url + data.current.id, item).then((resp) => {
                            tree.view.createNode(resp.data.model, data.current);
                        });
                    } else {
                        $http.post(_url, item).then((resp) => {
                            tree.view.createNode(resp.data.model);
                        });
                    }

                    data.name_add = null;
                    if (data.current) {
                        data.current.expand();
                    }
                };

                var editNode = function () {
                    if (data.current) {
                        $http.put(_url + data.current.id, { name: data.name_edit }).then((resp) => {
                            data.current.name = data.name_edit;
                            data.current.unselect();
                            data.current = data.name_edit = null;
                        });
                    }
                };

                var unselectNode = function () {
                    if (data.current) {
                        data.current.unselect();
                        data.current = data.name_edit = null;
                    }
                };

                var data = $scope.data = {
                    current: null,
                    name_add: null,
                    name_edit: null,
                    add: addNode,
                    edit: editNode,
                    unselect: unselectNode,
                    expand: function () { tree.view.expand(); },
                    collapse: function () { tree.view.collapse(); }
                };

                var tree = $scope.tree = {
                    items: [],
                    options: {
                        selectable: true,
                        buttons: [{
                            classes: 'btn btn-xs btn-danger',
                            html: '<span class="glyphicon glyphicon-trash"></span>',
                            click: removeNode
                        }],
                        events: {
                            selected: function (node) {
                                data.current = node;
                                data.name_edit = node.name;
                            }
                        }
                    },
                    view: null
                };

                init();
            }]);
})();
