(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_project', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model) {
                    model = model || new api.Project({ headcount: 1, assignmentDate: new Date() });
                    var options = {
                        templateUrl: '/app/modals/project/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'transfer',
                            function ($scope, $uibModalInstance, transfer) {
                                $scope.model = transfer(model, {
                                    startDate: function (value) { return value ? new Date(value) : value; },
                                    offerSignedDate: function (value) { return value ? new Date(value) : value; },
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

                                $scope.verifyClient = function (model) {
                                    return model.client && model.client.id;
                                };

                                $scope.verifyFunction = function (model) {
                                    return model.functions && model.functions.length > 0;
                                };

                                $scope.verifyLocation = function (model) {
                                    return model.locations && model.locations.length > 0;
                                };

                                $scope.verifyAssignmentDate = function (model) {
                                    return model.assignmentDate;
                                };

                                $scope.verifyManager = function (model) {
                                    return model.manager && model.manager.id;
                                };

                                $scope.verifyConsultant = function (model) {
                                    return model.consultants && model.consultants.length > 0;
                                };

                                $scope.disabled = function (form, model) {
                                    return form.$invalid
                                        || !$scope.verifyClient(model)
                                        || !$scope.verifyFunction(model)
                                        || !$scope.verifyLocation(model)
                                        || !$scope.verifyAssignmentDate(model)
                                        || !$scope.verifyManager(model)
                                        || !$scope.verifyConsultant(model);
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();