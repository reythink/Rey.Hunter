(function () {
    'use strict';

    angular
        .module('app')
        .controller('role-list-ctrl', [
            'page', '$scope', 'api', 'modal', 'selector', 'filter',
            function (page, $scope, api, modal, selector, filter) {
                var options = {
                    scope: $scope,
                    Model: api.Role,
                    model: new api.Role({ enabled: true }),
                    modal: modal.role
                };
                page.list(options)
                    .initCreate()
                    .initUpdate()
                    .initDelete()
                    .initBatchDelete()
                    .initOrder()
                    .initAuth({ modal: modal.role_auth });

                $scope.selector = selector.create();
                $scope.filter = filter;
            }])
        .controller('role-item-ctrl', [
            'page', '$scope', 'api', 'modal',
            function (page, $scope, api, modal) {
                var options = {
                    scope: $scope,
                    Model: api.Role,
                    modal: modal.role
                };
                page.item(options)
                    .initInit()
                    .initUpdate();
            }]);
})();
