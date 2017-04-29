(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_company', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model) {
                    model = model || new api.Company({ status: 1 });
                    var options = {
                        templateUrl: '/app/modals/company/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'Upload',
                            function ($scope, $uibModalInstance, Upload) {
                                $scope.model = model;

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

                                //! Contact information.
                                $scope.contact_remove = function (item) {
                                    var index = $scope.model.contacts.indexOf(item);
                                    $scope.model.contacts.splice(index, 1);
                                    if ($scope.current_contact && $scope.current_contact === item) {
                                        $scope.current_contact = null;
                                    }
                                };

                                $scope.contact_select = function (item) {
                                    $scope.current_contact = item;
                                };

                                $scope.contact_class = function (item) {
                                    if ($scope.current_contact === item)
                                        return 'selected';

                                    return '';
                                };

                                $scope.contact_save = function () {
                                    if ($scope.model.contacts.indexOf($scope.current_contact) === -1) {
                                        $scope.model.contacts.push($scope.current_contact);
                                    }
                                    $scope.current_contact = null;
                                };

                                $scope.verifyIndustry = function (model) {
                                    return model.industries && model.industries.length > 0;
                                };

                                $scope.disabled = function (form, model) {
                                    return form.$invalid
                                        || !$scope.verifyIndustry(model);
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();