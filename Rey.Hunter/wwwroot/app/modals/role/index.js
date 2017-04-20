(function () {
    'use strict';

    angular
        .module('app')
        .service('modal_role', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model) {
                    model = model || new api.Role({ enabled: true });
                    var options = {
                        templateUrl: '/app/modals/role/index.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance',
                            function ($scope, $uibModalInstance) {
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
                            }]
                    };

                    options = angular.extend({}, modal_options, options);
                    return $uibModal.open(options);
                };
            }])
        .service('modal_role_auth', [
            '$uibModal', 'modal_options', 'api',
            function ($uibModal, modal_options, api) {
                this.open = function (model) {
                    var options = {
                        templateUrl: '/app/modals/role/auth.html?r=' + Math.random(),
                        controller: [
                            '$scope', '$uibModalInstance', '$http',
                            function ($scope, $uibModalInstance, $http) {
                                $scope.model = model;
                                $scope.items = [];

                                //! 全选下拉列表中的选项。
                                $scope.activities = [];

                                $http.get('/api/auth/items').then(function (resp) {
                                    $scope.items = resp.data.models;

                                    //! 选择角色中包含的授权项。
                                    $(model.authItems).each(function () {
                                        let authItem = this;
                                        $($scope.items).each(function () {
                                            if (this.target.name !== authItem.target) { return; }
                                            $(this.activities).each(function () {
                                                if (this.name === authItem.activity) {
                                                    this.selected = true;
                                                }
                                            });
                                        });
                                    });

                                    //! 加载全选下拉列表中的选项。
                                    $($scope.items).each(function () {
                                        $(this.activities).each(function () {
                                            var activity = this.name;
                                            if (!_.some($scope.activities, function (item) { return item.name === activity; })) {
                                                $scope.activities.push(this);
                                            }
                                        });
                                    });
                                });

                                $scope.cancel = function () {
                                    $uibModalInstance.dismiss('cancel');
                                };

                                $scope.selected_all = function (activity) {
                                    var result = true;
                                    $($scope.items).each(function () {
                                        if (!result) { return false; }
                                        $(this.activities).each(function () {
                                            if (activity !== 'all' && this.name !== activity) { return true; }
                                            if (this.selected !== true) {
                                                result = false;
                                                return false;
                                            }
                                        });
                                    });
                                    return result;
                                };

                                $scope.toggle_select_all = function (activity) {
                                    var selected = $scope.selected_all(activity) === true;
                                    $($scope.items).each(function () {
                                        $(this.activities).each(function () {
                                            if (activity !== 'all' && this.name !== activity) { return true; }
                                            this.selected = !selected;
                                        });
                                    });
                                };

                                $scope.selected_item = function (item) {
                                    var result = true;
                                    $(item.activities).each(function () {
                                        if (this.selected !== true) {
                                            result = false;
                                            return false;
                                        }
                                    });
                                    return result;
                                };

                                $scope.toggle_select_item = function (item) {
                                    var selected = $scope.selected_item(item);
                                    $(item.activities).each(function () {
                                        this.selected = !selected;
                                    });
                                };

                                $scope.ok = function () {
                                    $scope.model.authItems = [];
                                    $($scope.items).each(function () {
                                        var item = this;
                                        $(item.activities).each(function () {
                                            if (this.selected) {
                                                $scope.model.authItems.push({ target: item.target.name, activity: this.name });
                                            }
                                        });
                                    });

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