(function () {
    'use strict';

    class ListPage {
        constructor(services, options) {
            this._services = angular.extend({}, ListPage.defaults.services, services);
            this._options = angular.extend({}, ListPage.defaults.options, options);
        }

        get bootbox() { return this._services.bootbox; }
        get order() { return this._services.order; }

        get scope() { return this._options.scope; }

        //! angular resource of model.
        get Model() { return this._options.Model; }

        //! default model value.
        get model() { return this._options.model || new this.Model(); }

        //! dialog box.
        get modal() { return this._options.modal; }

        initCreate(options) {
            options = angular.extend({}, ListPage.defaults.create, options);
            var modal = this.modal;
            var model = this.model;
            this.scope[options.name] = function () {
                modal.open(model).result.then(options.then);
            };
            return this;
        }

        initUpdate(options) {
            options = angular.extend({}, ListPage.defaults.update, options);
            var Model = this.Model;
            var modal = this.modal;
            this.scope[options.name] = function (id) {
                Model.get({ id: id }, function (model) {
                    modal.open(model).result.then(options.then);
                });
            };
            return this;
        }

        initDelete(options) {
            options = angular.extend({}, ListPage.defaults.delete, options);
            var Model = this.Model;
            var bootbox = this.bootbox;
            this.scope[options.name] = function (id) {
                options.bootbox.buttons.success.callback = function () {
                    Model.delete({ id: id }, function (resp) {
                        if (resp.err_code === 0) {
                            options.then();
                        }
                    });
                };
                bootbox.customDialog(options.bootbox);
            };
            return this;
        }

        initBatchDelete(options) {
            options = angular.extend({}, ListPage.defaults.batchDelete, options);
            var Model = this.Model;
            var bootbox = this.bootbox;
            this.scope[options.name] = function (idList) {
                options.bootbox.buttons.success.callback = function () {
                    Model.batch_delete(idList, function (resp) {
                        if (resp.data.err_code === 0) {
                            options.then();
                        }
                    });
                };
                bootbox.customDialog(options.bootbox);
            };
            return this;
        }

        initOrder(options, orderOptions) {
            options = angular.extend({}, ListPage.defaults.order, options);
            var order = this.order;
            order.init(orderOptions);
            this.scope[options.name] = order;
            return this;
        }

        initAuth(options) {
            options = angular.extend({}, ListPage.defaults.auth, options);
            var Model = this.Model;
            this.scope[options.name] = function (id) {
                Model.get({ id: id }, function (model) {
                    options.modal.open(model).result.then(options.then);
                });
            };
            return this;
        }

        static get defaults() {
            return {
                services: { bootbox: null, order: null },
                options: { scope: null, Model: null, model: null, modal: null },
                create: { name: 'create', then: function () { location.reload(); } },
                update: { name: 'update', then: function () { location.reload(); } },
                delete: {
                    name: 'delete', then: function () { location.reload(); },
                    bootbox: {
                        title: 'Warning',
                        message: 'Are you sure delete this item?',
                        buttons: {
                            warning: { label: "No" },
                            success: {
                                label: "Yes",
                                className: "btn-danger"
                            }
                        }
                    }
                },
                batchDelete: {
                    name: 'batchDelete', then: function () { location.reload(); },
                    bootbox: {
                        title: 'Warning',
                        message: 'Are you sure delete selected items?',
                        buttons: {
                            warning: { label: "No" },
                            success: {
                                label: "Yes",
                                className: "btn-danger"
                            }
                        }
                    }
                },
                order: { name: 'order' },
                auth: { name: 'auth', modal: null, then: function () { location.reload(); } },
            }
        };
    }

    class ItemPage {
        constructor(services, options) {
            this._services = services;
            this._options = options;
        }

        get scope() { return this._options.scope; }
        get Model() { return this._options.Model; }
        get modal() { return this._options.modal; }

        initInit(options) {
            options = angular.extend({}, ItemPage.defaults.init, options);
            var Model = this.Model;
            var scope = this.scope;
            scope[options.name] = function (id) {
                Model.get({ id: id }, function (model) {
                    scope[options.modelName] = model;
                });
            };
            return this;
        }

        initUpdate(options) {
            options = angular.extend({}, ItemPage.defaults.update, options);
            var Model = this.Model;
            var modal = this.modal;
            this.scope[options.name] = function (id) {
                Model.get({ id: id }, function (model) {
                    modal.open(model).result.then(options.then);
                });
            };
            return this;
        }

        static get defaults() {
            return {
                services: {},
                options: { scope: null, Model: null },
                init: { name: 'init', modelName: 'model' },
                update: { name: 'update', then: function () { location.reload(); } },
            }
        }
    }

    angular
        .module('app')
        .service('page', [
            '$ngBootbox', 'order',
            function ($ngBootbox, order) {
                this.list = function (options) {
                    var services = {
                        bootbox: $ngBootbox,
                        order: order,
                    };
                    return new ListPage(services, options);
                };

                this.item = function (options) {
                    var services = {
                    };
                    return new ItemPage(services, options);
                };
            }]);
})();