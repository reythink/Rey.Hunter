(function () {
    'use strict';

    angular
        .module('app')
        .directive('reyAttachmentList', [function () {
            return {
                restrict: 'AE',
                template: `
<ul class="attachment-list">
    <li ng-repeat="item in ngModel">
        <a class="text" ng-bind="item.name" ng-href="{{item.url}}"></a>
        <span class="remove" ng-click="remove(item)">
            <i class="fa fa-times-circle-o"></i>
        </span>
    </li>
    <li class="add" ngf-select="selectFile($files)" ngf-multiple="true">
        <i class="fa fa-plus"></i>
    </li>
</ul>`,
                scope: {
                    ngModel: '='
                },
                link: function (scope, element, attrs) {
                },
                controller: ['$scope', 'Upload', function ($scope, Upload) {
                    $scope.selectFile = function ($files) {
                        Upload.upload({
                            url: '/api/upload/file',
                            data: { file: $files },
                        }).then(function (resp) {
                            for (var i = 0; i < resp.data.models.length; ++i) {
                                $scope.ngModel.push(resp.data.models[i]);
                            }
                        });
                    };

                    $scope.remove = function (item) {
                        $scope.ngModel.splice($scope.ngModel.indexOf(item), 1);
                    };
                }]
            };
        }]);
})();