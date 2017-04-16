(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_company', [
            '$uibModal', 'modal_options',
            function ($uibModal, modal_options) {
                this.open = function (model) {
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
                                            $uibModalInstance.close('save');
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

                                //! Department Structure.
                                $scope.department_add = function ($files) {
                                    Upload.upload({
                                        url: '/api/upload/file',
                                        data: { file: $files },
                                    }).then(function (resp) {
                                        for (var i = 0; i < resp.data.models.length; ++i) {
                                            $scope.model.departmentStructures.push(resp.data.models[i]);
                                        }
                                    });
                                };

                                $scope.department_remove = function (item) {
                                    var index = $scope.model.departmentStructures.indexOf(item);
                                    $scope.model.departmentStructures.splice(index, 1);
                                };

                                //! NameList
                                $scope.namelist_add = function ($files) {
                                    Upload.upload({
                                        url: '/api/upload/file',
                                        data: { file: $files },
                                    }).then(function (resp) {
                                        for (var i = 0; i < resp.data.models.length; ++i) {
                                            $scope.model.nameList.push(resp.data.models[i]);
                                        }
                                    });
                                };

                                $scope.namelist_remove = function (item) {
                                    var index = $scope.model.nameList.indexOf(item);
                                    $scope.model.nameList.splice(index, 1);
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();