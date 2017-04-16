(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_portrait', [
            '$uibModal', 'modal_options', '$http', 'Upload',
            function ($uibModal, modal_options, $http, Upload) {
                this.open = function ($file) {
                    var options = {
                        templateUrl: '/app/modals/portrait/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance',
                            function ($scope, $uibModalInstance) {
                                $scope.model = { file: $file };
                                $scope.data_cropped = '';

                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.ok = function () {
                                    Upload.upload({
                                        url: '/api/upload/file',
                                        data: {
                                            file: Upload.dataUrltoBlob($scope.data_cropped)
                                        },
                                    }).then(function (resp) {
                                        var url = resp.data.models[0].url;
                                        $http.post('/api/current/portrait', {}, { params: { url: url } }).then(function (resp) {
                                            if (resp.data.err_code === 0) {
                                                window.location.reload();
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