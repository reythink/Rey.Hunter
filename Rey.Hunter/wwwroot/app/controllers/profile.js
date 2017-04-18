(function () {
    'use strict';

    angular
        .module('app')
        .controller('profile-ctrl', [
            '$scope', 'modal',
            function ($scope, modal) {
                $scope.portraitSelected = function ($files) {
                    if ($files && $files.length > 0) {
                        var $file = $files[0];
                        modal.portrait.open($file);
                    }
                };
            }]);
})();
