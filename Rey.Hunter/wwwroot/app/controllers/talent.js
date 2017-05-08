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

                $scope.expComparator = function (item1, item2) {
                    var value1 = item1.value;
                    var value2 = item2.value;
                    value1 = value1.currentJob ? 1000000 : 0 + value1.fromYear * 100 + value1.fromMonth;
                    value2 = value2.currentJob ? 1000000 : 0 + value2.fromYear * 100 + value2.fromMonth;
                    return value1 - value2;
                };
            }]);
})();