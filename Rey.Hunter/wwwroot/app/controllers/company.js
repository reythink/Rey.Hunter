﻿(function () {
    'use strict';

    angular
        .module('app')
        .controller('company-list-ctrl', [
            'page', '$scope', 'api', 'modal', 'selector', 'filter',
            function (page, $scope, api, modal, selector, filter) {
                var options = {
                    scope: $scope,
                    Model: api.Company,
                    modal: modal.company
                };
                page.list(options)
                    .initCreate()
                    .initUpdate()
                    .initDelete()
                    .initBatchDelete();

                $scope.selector = selector;
                $scope.filter = filter;
            }])
        .controller('company-item-ctrl', [
            'page', '$scope', 'api', 'modal',
            function (page, $scope, api, modal) {
                var options = {
                    scope: $scope,
                    Model: api.Company,
                    modal: modal.company
                };
                page.item(options)
                    .initInit()
                    .initUpdate();
            }]);
})();
