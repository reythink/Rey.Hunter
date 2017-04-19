(function () {
    'use strict';

    class TreeViewNodeCheckbox {
        constructor(node) {
            this._node = node;
            this._checked = false;
        }

        get node() { return this._node; }
        get isChecked() {
            if (this._node.isLeaf) { return this._checked; }
            var children = this._node.children;
            for (var i = 0, len = children.length; i < len; ++i) {
                if (children[i].checkbox.isChecked === false) {
                    return false;
                }
            }
            return true;
        }

        get classes() {
            let options = this._node.treeView.options;
            if (this.isChecked) { return options.icon_checked; }
            return options.icon_unchecked;
        }

        check() {
            if (this.isChecked) { return; }
            if (this._node.isLeaf) {
                this._checked = true;
            } else {
                var children = this._node.children;
                for (var i = 0, len = children.length; i < len; ++i) {
                    children[i].checkbox.check();
                }
            }
            this._node.treeView.events._callChecked(this._node);
        }

        uncheck() {
            if (!this.isChecked) { return; }
            if (this._node.isLeaf) {
                this._checked = false;
            } else {
                var children = this._node.children;
                for (var i = 0, len = children.length; i < len; ++i) {
                    children[i].checkbox.uncheck();
                }
            }
            this._node.treeView.events._callUnchecked(this._node);
        }

        toggle() {
            if (this.isChecked) { this.uncheck(); }
            else { this.check(); }
        }
    }

    class TreeViewNodeIcon {
        constructor(node) {
            this._node = node;
        }

        get node() { return this._node; }

        get classes() {
            let options = this._node.treeView.options;
            if (this._node.isLeaf)
                return options.icon_leaf;

            if (this._node.isExpanded)
                return options.icon_expand;

            return options.icon_collapse;
        }

        get style() {
            let result = {};
            if (!this._node.isLeaf) {
                angular.extend(result, { cursor: 'pointer' });
            }
            return result;
        }
    }

    class TreeViewNode {
        static foreach(nodes, each) {
            if (!nodes || !angular.isArray(nodes) || nodes.length === 0)
                return;

            if (!each || !angular.isFunction(each))
                return;

            var stack = [];
            for (var i = 0, len = nodes.length; i < len; ++i) {
                stack.push(nodes[i]);
            }

            while (stack.length > 0) {
                var node = stack.shift();
                if (each(node) === false) { break; }

                var children = node.children;
                if (!children || !angular.isArray(children) || children.length === 0)
                    continue;

                stack = children.concat(stack);
            }
        }

        constructor(treeView, item, parent) {
            this._treeView = treeView;
            this._item = item;
            this._id = item.id;
            this._name = item.name || item;
            this._parent = parent;
            this._level = parent ? parent.level + 1 : 1;
            this._children = [];
            this._icon = new TreeViewNodeIcon(this);
            this._checkbox = new TreeViewNodeCheckbox(this);

            this._expanded = false;
            this._selected = false;

            if (parent) {
                parent.children.push(this);
            }
        }

        get treeView() { return this._treeView; }
        get item() { return this._item; }

        get id() { return this._id; }
        get parent() { return this._parent; }
        get level() { return this._level; }
        get children() { return this._children; }
        get classes() {
            var result = ['level-' + this._level];
            if (this._treeView.canSelect) { result.push("selectable"); }
            if (this._selected) { result.push('active'); }
            return result;
        }

        get icon() { return this._icon; }
        get checkbox() { return this._checkbox; }
        get buttons() { return this._treeView.buttons; }

        set name(value) { this._name = value; }
        get name() { return this._name; }
        set visible(value) { this._visible = value; }
        get visible() { return this._visible; }

        get isRoot() { return this._parent ? false : true; }
        get isLeaf() { return (this._children && angular.isArray(this._children) && this._children.length > 0) ? false : true; }
        get isExpanded() { return this._expanded; }
        get isSelected() { return this._selected; }
        get isVisible() {
            if (this._visible === false) { return false; }

            for (var p = this._parent; p; p = p._parent) {
                if (!p._expanded) { return false; }
            }

            return true;
        }
        get isChecked() { return this._checkbox.isChecked; }

        expand() {
            if (this._expanded) { return; }
            this._expanded = true;
            this.treeView.events._callExpanded(this);
        }

        collapse() {
            if (!this._expanded) { return; }
            this._expanded = false;
            this.treeView.events._callCollapsed(this);
        }

        toggle() {
            if (this._expanded) { this.collapse(); }
            else { this.expand(); }
        }

        select() {
            if (!this._treeView.canSelect || this._selected) { return; }
            this._treeView.foreach(function (node) {
                if (node.isSelected) {
                    node._selected = false;
                    return false;
                }
            });
            this._selected = true;
            this.treeView.events._callSelected(this);
        }

        unselect() {
            if (!this._selected) { return; }
            this._selected = false;
            this.treeView.events._callUnselected(this);
        }

        toggleSelect() {
            if (this._selected) { this.unselect(); }
            else { this.select(); }
        }

        check() { this._checkbox.check(); }
        uncheck() { this._checkbox.uncheck(); }
        toggleCheck() { this._checkbox.toggle(); }

        remove() {
            this.treeView.removeNode(this);
        }
    }

    class TreeViewButton {
        static get defaults() {
            return {
                html: '',
                classes: '',
                style: '',
                visible: true,
                click: function (node) { }
            };
        }

        constructor(treeView, options) {
            this._treeView = treeView;
            this._options = angular.extend({}, TreeViewButton.defaults, options);
        }

        get treeView() { return this._treeView; }
        get options() { return this._options; }

        get html() { return this._options.html; }
        get classes() { return this._options.classes; }
        get style() { return this._options.style; }

        visible(node) {
            let value = this._options.visible;
            if (angular.isFunction(value)) {
                return value.call(this, node);
            }
            return value;
        }

        click(node) { this._options.click.call(this, node); }
    }

    class TreeViewEvents {
        static get defaults() {
            return {
                expanded: function (node) { },
                collapsed: function (node) { },
                selected: function (node) { },
                unselected: function (node) { },
                checked: function (node) { },
                unchecked: function (node) { }
            };
        }

        constructor(treeView, options) {
            this._treeView = treeView;
            this._options = angular.extend({}, TreeViewEvents.defaults, options);
        }

        get treeView() { return this._treeView; }
        get options() { return this._options; }

        _callExpanded(node) { this._options.expanded.call(this, node); }
        _callCollapsed(node) { this._options.collapsed.call(this, node); }
        _callSelected(node) { this._options.selected.call(this, node); }
        _callUnselected(node) { this._options.unselected.call(this, node); }
        _callChecked(node) { this._options.checked.call(this, node); }
        _callUnchecked(node) { this._options.unchecked.call(this, node); }
    }

    class TreeView {
        static get defaults() {
            return {
                show_checkbox: false,
                show_icon: true,
                expanded: false,                                    //! boolean or function
                selectable: false,
                icon_leaf: 'glyphicon glyphicon-file',
                icon_expand: 'glyphicon glyphicon-minus',
                icon_collapse: 'glyphicon glyphicon-plus',
                icon_checked: 'glyphicon glyphicon-check',
                icon_unchecked: 'glyphicon glyphicon-unchecked',
                buttons: [],
                events: {}
            };
        }

        constructor(options) {
            this._roots = [];
            this._options = angular.extend({}, TreeView.defaults, options);
            this._buttons = [];
            this._events = new TreeViewEvents(this, this._options.events);

            //! initialize buttons.
            for (let i = 0, len = this._options.buttons.length; i < len; ++i) {
                this.createButton(this._options.buttons[i]);
            }
        }

        get roots() { return this._roots; }
        get options() { return this._options; }
        get buttons() { return this._buttons; }
        get events() { return this._events; }

        get canSelect() { return this._options.selectable; }
        get selectedNode() {
            let selected = null;
            this.foreach(function (node) {
                if (node.isSelected) {
                    selected = node;
                    return false;
                }
            });
            return selected;
        }

        foreach(each) {
            TreeViewNode.foreach(this._roots, each);
        }

        toList() {
            let result = [];
            this.foreach(function (node) { result.push(node); });
            return result;
        }

        toJson(convert) {
            let result = [];

            var stack = [];
            for (var i = 0, len = this._roots.length; i < len; ++i) {
                let node = this._roots[i];
                let json = { id: node.id, name: node.name, children: [] };
                if (convert && angular.isFunction(convert)) {
                    json = convert(node);
                }
                stack.push({ node: node, json: json });
                result.push(json);
            }

            while (stack.length > 0) {
                var parent = stack.shift();

                var children = parent.node.children;
                if (!children || !angular.isArray(children) || children.length === 0)
                    continue;

                for (let i = 0, len = children.length; i < len; ++i) {
                    let node = children[i];
                    let json = { id: node.id, name: node.name, children: [] };
                    if (convert && angular.isFunction(convert)) {
                        json = convert(node);
                    }
                    parent.json.children.push(json);
                    stack.push({ node: node, json: json });
                }
            }

            return result;
        }

        createNode(item, parent) {
            let node = new TreeViewNode(this, item, parent);

            let expanded = this._options.expanded;
            if (angular.isFunction(expanded) ? expanded.call(this, node) : expanded) { node.expand(); }

            if (!parent) {
                this._roots.push(node);
            }
            return node;
        }

        removeNode(node) {
            let arr = node.isRoot ? this._roots : node.parent.children;
            let index = arr.indexOf(node);
            if (index !== -1) {
                arr.splice(index, 1);
            }
        }

        clear() {
            this._roots = [];
        }

        createButton(options) {
            let button = new TreeViewButton(this, options);
            this._buttons.push(button);
            return button;
        }

        removeButton(button) {
            let index = this._buttons.indexOf(button);
            if (index !== -1) {
                this._buttons.splice(index, 1);
            }
        }

        parse(items) {
            var stack = [];

            if (angular.isArray(items)) {
                for (var i = 0, len = items.length; i < len; ++i) {
                    stack.push(this.createNode(items[i]));
                }
            } else {
                stack.push(this.createNode(items));
            }

            while (stack.length > 0) {
                var parent = stack.shift();

                var children = parent.item.children;
                if (!children || !angular.isArray(children) || children.length === 0)
                    continue;

                for (var i = 0, len = children.length; i < len; ++i) {
                    stack.push(this.createNode(children[i], parent));
                }
            }

            return this._roots;
        }

        expand() {
            this.foreach(function (node) { node.expand(); });
        }

        collapse() {
            this.foreach(function (node) { node.collapse(); });
        }
    }

    angular.module('app')
        .directive('reyTreeView', [function () {
            return {
                restrict: 'E',
                template: `
<ul class="nav nav-pills nav-stacked rey-tree-view">
    <li ng-repeat="node in treeView.toList()" ng-class="node.classes" ng-if="node.isVisible" class="tree-row">
        <a>
            <i ng-class="node.icon.classes" ng-style="node.icon.style" ng-click="node.toggle()" ng-if="treeView.options.show_icon"></i>
            <i ng-class="node.checkbox.classes" ng-style="row.checkbox.style" class="tree-node-checkbox" ng-click="node.checkbox.toggle()" ng-if="treeView.options.show_checkbox"></i>
            <span ng-bind="node.name" class="tree-node-name" ng-click="node.select()"></span>
            <span class="pull-right tree-node-buttons" ng-if="node.buttons.length > 0">
                <button ng-repeat="button in node.buttons" ng-class="button.classes" ng-style="button.style" ng-bind-html="button.html" ng-click="button.click(node)" ng-if="button.visible(node)"></button>
            </span>
        </a>
    </li>
</ul>`,
                scope: {
                    treeItems: '=',
                    treeOptions: '=',
                    treeView: '='
                },
                link: function (scope, element, attrs) {
                    if (!scope.treeItems) {
                        console.error('no tree-items defined for the tree.');
                        return;
                    }

                    if (!angular.isArray(scope.treeItems)) {
                        scope.treeItems = [scope.treeItems];
                    }

                    var treeView = scope.treeView = new TreeView(scope.treeOptions);

                    scope.$watch('treeItems', function () {
                        treeView.clear();
                        treeView.parse(scope.treeItems);
                    });
                }
            };
        }]);
})();