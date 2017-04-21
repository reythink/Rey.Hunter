(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_project', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model) {
                    model = model || new api.Project();
                    var options = {
                        templateUrl: '/app/modals/project/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'transfer',
                            function ($scope, $uibModalInstance, transfer) {
                                $scope.model = transfer(model, {
                                    startDate: function (value) { return value ? new Date(value) : value; },
                                    assignmentDate: function (value) { return value ? new Date(value) : value; },
                                    onBoardDate: function (value) { return value ? new Date(value) : value; },
                                });

                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.ok = function () {
                                    $scope.model.$save(function (resp) {
                                        if (resp.err.code === 0) {
                                            $uibModalInstance.close($scope.model);
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