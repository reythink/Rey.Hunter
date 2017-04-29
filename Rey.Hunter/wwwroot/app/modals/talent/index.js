(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_talent', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model) {
                    model = model || new api.Talent({ experiences: [], language: 1, nationality: 1 });
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
                                            $uibModalInstance.close($scope.model);
                                        }
                                    });
                                };

                                $scope.verifyCurrentLocation = function (model) {
                                    return model.currentLocations && model.currentLocations.length > 0;
                                };

                                $scope.disabled = function (form, model) {
                                    return form.$invalid
                                        || !$scope.verifyCurrentLocation(model);
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();