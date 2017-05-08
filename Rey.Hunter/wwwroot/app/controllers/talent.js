(function () {
    'use strict';

    angular
        .module('app')
        .controller('talent-list-ctrl', [
            'page', '$scope', 'api', 'modal', 'selector', 'filter', 'growl',
            function (page, $scope, api, modal, selector, filter, growl) {
                var options = {
                    scope: $scope,
                    Model: api.Talent,
                    modal: modal.talent
                };
                page.list(options)
                    .initCreate()
                    .initUpdate()
                    .initDelete()
                    .initBatchDelete()
                    .initJoin({ modal: modal.join, then: () => growl.success('candidate join success!') })
                    .initBatchJoin({ modal: modal.join, then: () => growl.success('candidates join success!') })
                    .initOrder();

                $scope.selector = selector.create();
                $scope.filter = filter;
                $scope.modal = modal;
            }])
        .controller('talent-item-ctrl', [
            'page', '$scope', 'api', 'modal',
            function (page, $scope, api, modal) {
                var options = {
                    scope: $scope,
                    Model: api.Talent,
                    modal: modal.talent
                };
                page.item(options)
                    .initInit()
                    .initUpdate();
            }]);
})();