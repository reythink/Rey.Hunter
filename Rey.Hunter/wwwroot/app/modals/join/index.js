(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_join', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (idList) {
                    var options = {
                        templateUrl: '/app/modals/join/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance',
                            function ($scope, $uibModalInstance) {
                                $scope.model = null;

                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.ok = function () {
                                    api.Project.get({ id: $scope.model.id }, function (model) {
                                        idList = _.pullAll(idList, _.map(model.candidates, (item) => item.talent.id));
                                        model.candidates = model.candidates.concat(_.map(idList, (id) => { return { talent: { id: id } } }));
                                        model.$save(function (resp) {
                                            if (resp.err.code === 0) {
                                                $uibModalInstance.close(model);
                                            }
                                        });
                                    });
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();