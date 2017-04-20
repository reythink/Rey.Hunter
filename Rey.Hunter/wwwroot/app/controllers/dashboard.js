(function () {
    'use strict';

    angular
        .module('app')
        .controller('dashboard-ctrl', [
            '$scope', 'modal',
            function ($scope, modal) {
                $scope.addTalent = function () {
                    modal.talent.open().result.then(function (model) {
                        location.href = '/Talent/' + model.id;
                    });
                };

                $scope.addCompany = function () {
                    modal.company.open().result.then(function (model) {
                        location.href = '/Company/' + model.id;
                    });
                };

                $scope.addProject = function () {
                    modal.project.open().result.then(function (model) {
                        location.href = '/Project/' + model.id;
                    });
                };
            }]);
})();
