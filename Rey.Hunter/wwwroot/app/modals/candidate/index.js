(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_candidate', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model, candidates) {
                    var options = {
                        windowTopClass: 'modal-fullscreen',
                        templateUrl: '/app/modals/candidate/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance',
                            function ($scope, $uibModalInstance) {
                                $scope.model = model;
                                $scope.talents = [];
                                $scope.candidates = candidates;

                                $scope.query = function (search) {
                                    api.Talent.query({ search: search }, function (resp) {
                                        $scope.talents = resp;
                                        for (var i = 0, len = model.candidates.length; i < len; ++i) {
                                            var id = model.candidates[i].talent.id;
                                            for (var j = $scope.talents.length - 1; j >= 0; --j) {
                                                if ($scope.talents[j].id === id) {
                                                    $scope.talents.splice(j, 1);
                                                    break;
                                                }
                                            }
                                        }
                                    });
                                };
                                $scope.query();

                                $scope.select = function (talent, index) {
                                    $scope.candidates.push({ talent: talent, status: 1 });
                                    $scope.talents.splice(index, 1);
                                };

                                $scope.unselect = function (candidate, index) {
                                    $scope.talents.push(candidate.talent);
                                    $scope.candidates.splice(index, 1);
                                }

                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.ok = function () {
                                    $scope.model.candidates = $scope.candidates;
                                    $scope.model.$save(function (resp) {
                                        if (resp.err.code === 0) {
                                            candidates = $scope.model.candidates;
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