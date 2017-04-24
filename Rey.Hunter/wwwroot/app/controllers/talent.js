(function () {
    'use strict';



    angular
        .module('app')
        .controller('talent-list-ctrl', [
            'page', '$scope', 'api', 'modal', 'selector', 'filter',
            function (page, $scope, api, modal, selector, filter) {
                var options = {
                    scope: $scope,
                    Model: api.Talent,
                    model: new api.Talent({ experiences: [] }),
                    modal: modal.talent
                };
                page.list(options)
                    .initCreate()
                    .initUpdate()
                    .initDelete()
                    .initBatchDelete()
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