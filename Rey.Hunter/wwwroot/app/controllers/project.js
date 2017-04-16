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
                   .initBatchDelete();

               $scope.selector = selector;
               $scope.filter = filter;
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
