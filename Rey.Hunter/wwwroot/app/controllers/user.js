(function () {
    'use strict';

    angular
        .module('app')
        .controller('user-list-ctrl', [
            'page', '$scope', 'api', 'modal', 'selector', 'filter',
            function (page, $scope, api, modal, selector, filter) {
                var options = {
                    scope: $scope,
                    Model: api.User,
                    model: new api.User({ enabled: true }),
                    modal: modal.user
                };
                page.list(options)
                    .initCreate()
                    .initUpdate()
                    .initDelete()
                    .initBatchDelete()
                    .initOrder();

                $scope.selector = selector;
                $scope.filter = filter;
            }])
        .controller('user-item-ctrl', [
            'page', '$scope', 'api', 'modal',
            function (page, $scope, api, modal) {
                var options = {
                    scope: $scope,
                    Model: api.User,
                    modal: modal.user
                };
                page.item(options)
                    .initInit()
                    .initUpdate();
            }]);
})();
