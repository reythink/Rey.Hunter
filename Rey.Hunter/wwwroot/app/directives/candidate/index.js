(function () {
    'use strict';

    const STATUS_TABLE = [
        {
            name: 'All', value: undefined, color: 'gray', actions: []
        },
        {
            name: 'Approching', value: 1, color: 'aqua', actions: [
                { name: 'Approch', value: 2 },
                { name: 'Shortlist', value: 3 }
            ]
        },
        {
            name: 'Approched', value: 2, color: 'blue', actions: [
                { name: 'Shortlist', value: 3 }
            ]
        },
        {
            name: 'Shortlisted', value: 3, color: 'blue', actions: [
                { name: 'Interview', value: 4 },
                { name: 'Fail', value: 9999 }
            ]
        },
        {
            name: 'Interviewed', value: 4, color: 'purple', actions: [
                { name: 'Interview', value: 4 },
                { name: 'Offer', value: 5 },
                { name: 'Fail', value: 9999 }
            ]
        },
        {
            name: 'Offering', value: 5, color: 'yellow', actions: [
                { name: 'Accept Offer', value: 6 },
                { name: 'Fail', value: 9999 }
            ]
        },
        {
            name: 'Offer Accepted', value: 6, color: 'yellow', actions: [
                { name: 'On Board', value: 7 },
                { name: 'Fail', value: 9999 }
            ]
        },
        {
            name: 'On Board', value: 7, color: 'green', actions: [
                { name: 'Fail', value: 9999 }
            ]
        },
        {
            name: 'Failed', value: 9999, color: 'red', actions: [
                { name: 'Interview', value: 4 },
                { name: 'Offer', value: 5 }
            ]
        },
    ];

    const INTERVIEW_TABLE = [
        { name: 'All', value: undefined, match: function () { return true; } },
        { name: '1st Round', value: 1, match: function (rounds) { return rounds === 1; } },
        { name: '2nd Round', value: 2, match: function (rounds) { return rounds === 2; } },
        { name: '3rd Round', value: 3, match: function (rounds) { return rounds === 3; } },
        { name: '4th Round', value: 4, match: function (rounds) { return rounds === 4; } },
        { name: '5th Round', value: 5, match: function (rounds) { return rounds === 5; } },
        { name: '5th+ Round', value: -1, match: function (rounds) { return rounds > 5; } },
    ];

    class CandidatePanel {
        constructor(model) {
            this._model = model;
            this._statusPanels = [];
            this._interviewPanels = [];
            this._tables = [];
            this._status = null;
            this._interview = null;
        }

        get status() { return this._status; }
        get candidates() { return this._model.candidates; }

        createStatusPanel() {
            var statusPanel = new CandidateStatusPanel(this);
            this._statusPanels.push(statusPanel);
            return statusPanel;
        }

        createInterviewPanel() {
            var interviewPanel = new CandidateInterviewPanel(this);
            this._interviewPanels.push(interviewPanel);
            return interviewPanel;
        }

        createTable() {
            var table = new CandidateTable(this);
            this._tables.push(table);
            return table;
        }

        getCandidates(status, interview) {
            if (typeof (status) === 'undefined'
                || status === null
                || typeof (status.value) === 'undefined') {
                return this.candidates;
            }

            var temp = [];
            this.candidates.forEach(function (item) {
                if (status.value === item.status) {
                    if (status.value !== 4
                        || typeof (interview) === 'undefined'
                        || interview === null
                        || typeof (interview.value) === 'undefined') {
                        temp.push(item);
                    } else {
                        if (interview.value >= 0
                            && item.interviews.length === interview.value) {
                            temp.push(item);
                        } else {
                            if (interview.match(item.interviews.length)) {
                                temp.push(item);
                            }
                        }
                    }
                }
            });

            return temp;
        }

        getTableCandidates() {
            return this.getCandidates(this._status, this._interview);
        }

        getStatusSelected(status) {
            if (this._status === null
                && typeof (status.value) === 'undefined') {
                return true;
            }

            return this._status === status;
        }

        getInterviewSelected(interview) {
            if (this._interview === null
                && typeof (interview.value) === 'undefined') {
                return true;
            }

            return this._interview === interview;
        }

        selectStatus(status) {
            if (this._status !== status) {
                this._status = status;
            }
            return this;
        }

        selectInterview(interview) {
            if (this._interview !== interview) {
                this._interview = interview;
            }
            return this;
        }

        saveModel() {
            this._model.$save();
        }
    }

    class CandidateStatusPanel {
        constructor(panel) {
            this._panel = panel;
            this._statuses = [];
        }

        get panel() { return this._panel; }
        get statuses() { return this._statuses; }

        createStatus(name, value, color) {
            var status = new CandidateStatus(this, name, value, color);
            this._statuses.push(status);
            return status;
        }
    }

    class CandidateStatus {
        constructor(statusPanel, name, value, color) {
            this._statusPanel = statusPanel;
            this._name = name;
            this._value = value;
            this._color = color;
        }

        get statusPanel() { return this._statusPanel; }
        get panel() { return this._statusPanel.panel; }
        get name() { return this._name; }
        get value() { return this._value; }
        get color() { return this._color; }

        get class() { return this.selected ? "active" : ''; }
        get badgeClass() { return 'badge bg-' + (this._color || 'gray'); }
        get count() { return this.panel.getCandidates(this).length; }
        get selected() { return this.panel.getStatusSelected(this); }

        select() {
            this.panel.selectStatus(this);
        }
    }

    class CandidateInterviewPanel {
        constructor(panel) {
            this._panel = panel;
            this._interviews = [];
        }

        get panel() { return this._panel; }
        get interviews() { return this._interviews; }

        createInterview(name, value, match) {
            var interview = new CandidateInterview(this, name, value, match);
            this._interviews.push(interview);
            return interview;
        }
    }

    class CandidateInterview {
        constructor(interviewPanel, name, value, match) {
            this._interviewPanel = interviewPanel;
            this._name = name;
            this._value = value;
            this._match = match;
        }

        get interviewPanel() { return this._interviewPanel; }
        get panel() { return this._interviewPanel.panel; }
        get name() { return this._name; }
        get value() { return this._value; }
        get match() { return this._match; }

        get class() { return this.selected ? "active" : ''; }
        get count() { return this.panel.getCandidates(this.panel.status, this).length; }

        get selected() { return this.panel.getInterviewSelected(this); }

        select() {
            this.panel.selectInterview(this);
        }
    }

    class CandidateTable {
        constructor(panel) {
            this._panel = panel;
        }

        get panel() { return this._panel; }
        get candidates() { return this._panel.getTableCandidates(); }
    }

    angular
        .module('app')
        .directive('reyCandidatePanel', [function () {
            return {
                restrict: 'AE',
                template: `<div ng-transclude></div>`,
                transclude: true,
                scope: {
                    reyModel: '=ngModel'
                },
                link: function (scope, element, attrs, ctrl, transclude) {
                },
                controller: ['$scope', 'api', function ($scope, api) {
                    this.panel = new CandidatePanel($scope.reyModel);
                }]
            };
        }])
        .directive('reyCandidateStatusPanel', [function () {
            return {
                restrict: 'AE',
                templateUrl: '/app/directives/candidate/status-panel.html?r=' + Math.random(),
                scope: {
                },
                link: function (scope, element, attrs, ctrl, transclude) {
                },
                controller: ['$scope', '$element', function ($scope, $element) {
                    var statusPanel = $scope.statusPanel = $element
                        .parent()
                        .controller('reyCandidatePanel')
                        .panel
                        .createStatusPanel();

                    for (var i = 0, len = STATUS_TABLE.length; i < len; ++i) {
                        var item = STATUS_TABLE[i];
                        statusPanel.createStatus(item.name, item.value, item.color);
                    }
                }]
            };
        }])
        .directive('reyCandidateInterviewPanel', [function () {
            return {
                restrict: 'AE',
                templateUrl: '/app/directives/candidate/interview-panel.html?r=' + Math.random(),
                scope: {
                },
                link: function (scope, element, attrs, ctrl, transclude) {
                },
                controller: ['$scope', '$element', function ($scope, $element) {
                    var interviewPanel = $scope.interviewPanel = $element
                        .parent()
                        .controller('reyCandidatePanel')
                        .panel
                        .createInterviewPanel();

                    for (var i = 0, len = INTERVIEW_TABLE.length; i < len; ++i) {
                        var item = INTERVIEW_TABLE[i];
                        interviewPanel.createInterview(item.name, item.value, item.match);
                    }
                }]
            };
        }])
        .directive('reyCandidateTable', [function () {
            return {
                restrict: 'AE',
                templateUrl: '/app/directives/candidate/table.html?r=' + Math.random(),
                scope: {
                },
                link: function (scope, element, attrs, ctrl, transclude) {
                    scope.getCandidateActions = function (statusValue) {
                        for (var i = 0, len = STATUS_TABLE.length; i < len; ++i) {
                            var item = STATUS_TABLE[i];
                            if (item.value === statusValue) {
                                return item.actions;
                            }
                        }
                        return null;
                    };

                    scope.getStatusName = function (candidate) {
                        for (var i = 0, len = STATUS_TABLE.length; i < len; ++i) {
                            var item = STATUS_TABLE[i];
                            if (item.value === candidate.status) {
                                if (candidate.status === 4) {
                                    return item.name + ' (' + candidate.interviews.length + ')';
                                } else {
                                    return item.name;
                                }
                            }
                        }
                        return null;
                    };
                },
                controller: ['$scope', '$element', function ($scope, $element) {
                    var table = $scope.table = $element
                        .parent()
                        .controller('reyCandidatePanel')
                        .panel
                        .createTable();

                    $scope.changeStatus = function (candidate, statusValue) {
                        candidate.status = statusValue;
                        if (statusValue === 4) {
                            candidate.interviews.push({});
                        }
                        table.panel.saveModel();
                    };

                    $scope.currentExperience = function (talent) {
                        for (var i = 0, len = talent.experiences.length; i < len; ++i) {
                            var item = talent.experiences[i];
                            if (item.currentJob === true) {
                                return item;
                            }
                        }
                        return null;
                    };

                    $scope.companyName = function (talent) {

                    };
                }]
            };
        }]);
})();