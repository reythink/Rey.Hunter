(function () {
    'use strict';

    class Sidebar {
        constructor(options) {
            this._options = options;
        }

        get sidebar() { return $(this._options.selector); }
        get slide() { return this._options.slide; }

        open() {
            if (this.slide) {
                this.sidebar.addClass('control-sidebar-open');
            } else {
                $('body').addClass('control-sidebar-open');
            }
        }

        close() {
            if (this.slide) {
                this.sidebar.removeClass('control-sidebar-open');
            } else {
                $('body').removeClass('control-sidebar-open');
            }
        }

        toggle() {
            if (!this.sidebar.hasClass('control-sidebar-open')
                && !$('body').hasClass('control-sidebar-open')) {
                this.open();
            } else {
                this.close();
            }
        }
    }

    angular.module('app')
        .directive('reySidebar', function () {
            return {
                restrict: 'AE',
                template: `
<aside class="control-sidebar control-sidebar-dark">
    <div class="tab-content" ng-transclude></div>
</aside>`,
                transclude: true,
                scope: {
                },
                link: function (scope, element, attrs, ctrl, transclude) {
                    //! need reactivate controlsidebar here, because of 
                    //! content of template load after then AdminLTE app script.
                    //$.AdminLTE.controlSidebar.activate();
                    var options = $.AdminLTE.options.controlSidebarOptions;
                    var bar = new Sidebar(options);
                    element.find(options.toggleBtnSelector).click(function () {
                        bar.toggle();
                    });
                }
            };
        });
})();