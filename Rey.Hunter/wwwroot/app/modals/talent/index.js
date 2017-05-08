(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_talent', [
            '$uibModal', 'modal_options', 'api', 'growl',
            function ($uibModal, modal_options, api, growl) {
                this.open = function (model) {
                    model = model || new api.Talent({ experiences: [{ currentJob: true }], language: 1, nationality: 1 });
                    var options = {
                        templateUrl: '/app/modals/talent/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'transfer', 'generator',
                            function ($scope, $uibModalInstance, transfer, generator) {
                                $scope.model = transfer(model, {
                                    birthday: function (value) { return value ? new Date(value) : value; }
                                });
                                var year = (new Date()).getFullYear();
                                $scope.years = generator.range(year, 1960 - year, true);

                                $scope.experience_create = function () {
                                    $scope.model.experiences.push({});
                                };

                                $scope.experience_remove = function (experience) {
                                    if ($scope.model.experiences.length <= 1) {
                                        growl.warning('Cannot remove last experience!');
                                        return;
                                    }

                                    var index = $scope.model.experiences.indexOf(experience);
                                    $scope.model.experiences.splice(index, 1);

                                    if (experience.currentJob === true) {
                                        $scope.model.experiences[0].currentJob = true;
                                    }
                                };

                                $scope.experience_current_changed = function (experience) {
                                    $scope.model.experiences.forEach(function (item) {
                                        if (item !== experience) {
                                            item.currentJob = false;
                                        }
                                    });
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

                                $scope.verifyExperienceCompany = function (experience) {
                                    return experience.company && experience.company.id;
                                };

                                $scope.verifyExperienceTitle = function (experience) {
                                    if (!experience.currentJob) { return true; }
                                    return experience.title;
                                };

                                $scope.verifyExperience = function (model) {
                                    for (var i = 0, len = model.experiences.length; i < len; ++i) {
                                        var item = model.experiences[i];
                                        if (!$scope.verifyExperienceCompany(item)) { return false; }
                                        if (!$scope.verifyExperienceTitle(item)) { return false; }
                                    }
                                    return true;
                                };

                                $scope.disabled = function (form, model) {
                                    return form.$invalid
                                        || !$scope.verifyCurrentLocation(model)
                                        || !$scope.verifyExperience(model);
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();