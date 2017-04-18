(function () {
    'use strict';

    angular
        .module('app')
        .service('modal', [
            'modal_talent',
            'modal_company',
            'modal_project',
            'modal_candidate',
            'modal_portrait',
            'modal_export',
            'modal_import',
            function (
                modal_talent,
                modal_company,
                modal_project,
                modal_candidate,
                modal_portrait,
                modal_export,
                modal_import
            ) {
                this.talent = modal_talent;
                this.company = modal_company;
                this.project = modal_project;
                this.candidate = modal_candidate;
                this.portrait = modal_portrait;
                this.export = modal_export;
                this.import = modal_import;
            }]);
})();