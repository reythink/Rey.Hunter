(function () {
    'use strict';

    angular
        .module('app')
        .controller('profile-ctrl', [
            '$scope', '$http', 'modal', 'page', 'api', 'growl',
            function ($scope, $http, modal, page, api, growl) {
                $scope.portraitSelected = function ($files) {
                    if ($files && $files.length > 0) {
                        var $file = $files[0];
                        modal.portrait.open($file);
                    }
                };

                page.item({ scope: $scope, Model: api.User, modal: modal.user })
                    .initInit();

                $scope.saveSetting = function () {
                    $scope.model.$save(function (resp) {
                        if (resp.err.code === 0) {
                            growl.success('Setting save success!');
                        } else {
                            growl.error(resp.err.msg);
                        }
                    });
                };

                var password = $scope.password = {};
                $scope.savePassword = function () {
                    $http.post('/api/security/savepassword', password).then(function (resp) {
                        if (resp.data.err_code === 0) {
                            growl.success('Password save success!');
                            password = $scope.password = {};
                        } else {
                            growl.error(resp.data.err_msg);
                        }
                    });
                };
            }]);
})();
