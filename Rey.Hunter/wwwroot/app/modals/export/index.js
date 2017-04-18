(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_export', [
            '$uibModal', 'modal_options',
            function ($uibModal, modal_options) {
                this.open = function (uri) {
                    var options = {
                        templateUrl: '/app/modals/export/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'transfer', 'generator',
                            function ($scope, $uibModalInstance, transfer, generator) {
                                $scope.type = 'json';
                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.ok = function () {
                                    location.href = uri + '/export?type' + $scope.type;
                                    $uibModalInstance.close('ok');
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();