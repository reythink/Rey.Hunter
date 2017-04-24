(function () {
    'use strict';

    angular
       .module('app')
       .controller('project-list-ctrl', [
           'page', '$scope', 'api', 'modal', 'selector', 'filter',
           function (page, $scope, api, modal, selector, filter) {
               var options = {
                   scope: $scope,
                   Model: api.Project,
                   modal: modal.project
               };
               page.list(options)
                   .initCreate()
                   .initUpdate()
                   .initDelete()
                   .initBatchDelete()
                   .initOrder();

               $scope.selector = selector.create();
               $scope.filter = filter;
               $scope.modal = modal;
           }])
        .controller('project-item-ctrl', [
            'page', '$scope', 'api', 'modal',
            function (page, $scope, api, modal) {
                var options = {
                    scope: $scope,
                    Model: api.Project,
                    modal: modal.project
                };
                page.item(options)
                    .initInit()
                    .initUpdate();

                $scope.openCandidate = function (model) {
                    api.Project.get({ id: model.id }, function (model2) {
                        modal.candidate.open(model2, model.candidates);
                    });
                };
            }]);
})();
