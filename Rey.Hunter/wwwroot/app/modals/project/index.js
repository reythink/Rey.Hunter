(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_project', [
            '$uibModal', 'modal_options',
            function ($uibModal, modal_options) {
                this.open = function (model) {
                    var options = {
                        templateUrl: '/app/modals/project/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance',
                            function ($scope, $uibModalInstance) {
                                $scope.model = model;

                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.ok = function () {
                                    $scope.model.$save(function (resp) {
                                        if (resp.err.code === 0) {
                                            $uibModalInstance.close('save');
                                        }
                                    });
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();