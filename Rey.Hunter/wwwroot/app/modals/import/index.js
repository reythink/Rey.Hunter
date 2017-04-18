(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_import', [
            '$uibModal', 'modal_options', 'Upload',
            function ($uibModal, modal_options, Upload) {
                this.open = function (uri) {
                    var options = {
                        templateUrl: '/app/modals/import/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', 'transfer', 'generator',
                            function ($scope, $uibModalInstance, transfer, generator) {
                                $scope.type = 'json';
                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.selected = function ($files) {
                                    if ($files && $files.length > 0) {
                                        var $file = $files[0];
                                        Upload.upload({
                                            url: uri + '/import',
                                            data: {
                                                file: $file
                                            },
                                        }).then(function (resp) {
                                            console.log(resp);
                                            window.location.reload();
                                        });
                                    }
                                };
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }]);
})();