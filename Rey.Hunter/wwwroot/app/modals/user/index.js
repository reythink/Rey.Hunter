(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_user', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model) {
                    model = model || new api.User({ enabled: true });
                    var options = {
                        templateUrl: '/app/modals/user/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'selector',
                            function ($scope, $uibModalInstance, selector) {
                                $scope.model = model;
                                $scope.selector = selector.create();

                                api.Role.query(function (resp) {
                                    $scope.roles = resp;
                                    if (model.roles) {
                                        _.map(model.roles, (role) => {
                                            $scope.selector.register(role.id, true);
                                        });
                                    }
                                });

                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.ok = function () {
                                    $scope.model.roles = _.map($scope.selector.items(true), (item) => { return { id: item.value } });

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