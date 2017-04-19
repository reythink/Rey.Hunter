(function () {
    'use strict';

    angular
        .module('app', [
            //! angular
            'ngResource',
            'ngAnimate',
            'ngSanitize',

            //! third party.
            'angular-growl',
            'ui.bootstrap',
            'ui.checkbox',
            'ui.select',
            'ngBootbox',
            'ngFileUpload',
            'ngImgCrop',
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