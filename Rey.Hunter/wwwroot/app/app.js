(function () {
    'use strict';

    angular
        .module('app', [
            //! angular
            'ngResource',
            'ngAnimate',

            //! third party.
            'angular-growl',
            'ui.bootstrap',
            'ui.checkbox',
            'ngBootbox',
            'ngFileUpload',
            'ngImgCrop',

            //! directives
            'reyTreeView',
            'rey-sidebar',
            'rey-date-picker',
            'rey-enum-select',
            'rey-single-select',
            'rey-multi-select',
            'rey-selector',
            'rey-filter'
        ])
        .constant('options', {
            api: {
                url: [
                    { name: 'Role', url: '/api/role' },
                    { name: 'User', url: '/api/user' },
                    { name: 'Talent', url: '/api/talent' },
                    { name: 'Company', url: '/api/company' },
                    { name: 'CompanyGroup', url: '/api/companygroup' },
                    { name: 'Project', url: '/api/project' },
                ]
            }
        })
        .constant('modal_options', {
            animation: true,
            size: 'lg',
            backdrop: false
        })
        .value('', {})
        .config(['growlProvider', function (growlProvider) {
            growlProvider.globalTimeToLive(5000);
        }]);
})();