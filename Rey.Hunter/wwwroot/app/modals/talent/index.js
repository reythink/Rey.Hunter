(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_talent', [
            '$uibModal', 'modal_options',
            function ($uibModal, modal_options) {
                this.open = function (model) {
                    var options = {
                        templateUrl: '/app/modals/talent/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'transfer', 'generator',
                            function ($scope, $uibModalInstance, transfer, generator) {
                                $scope.model = transfer(model, {
                                    birthday: function (value) { return value ? new Date(value) : value; }
                                });
                                $scope.years = generator.range((new Date()).getFullYear(), -50, true);

                                $scope.experience_create = function () {
                                    $scope.model.experiences.push({});
                                };

                                $scope.experience_remove = function (item) {
                                    var index = $scope.model.experiences.indexOf(item);
                                    $scope.model.experiences.splice(index, 1);
                                };

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