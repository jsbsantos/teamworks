﻿/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />
/// <reference path="~/Content/js/libs/knockout.mapping.debug.js" />

var TW = TW || { };
TW.viewmodels = TW.viewmodels || { };

TW.viewmodels.Project = function (endpoint) {
    var self = this;
    /* new entities */
    self.project = new TW.viewmodels.models.Project();
    self.people = ko.observable().extend({ throttle: 500 });
    self.activity = new TW.viewmodels.models.Activity();
    self.discussion = new TW.viewmodels.models.Discussion();

    self.people.editing = ko.observable(false);
    self.activity.editing = ko.observable(false);
    self.discussion.editing = ko.observable(false);
    self.DependencyGraph = ko.observable(false);

    self.Gantt = function () { TW.Gantt(endpoint + '/precedences'); };

    self.project.discussions._create = function () {
        $.ajax(endpoint + '/discussions/',
            {
                type: 'post',
                data: ko.toJSON(self.discussion),
                statusCode: {
                    201: /*created*/function (data) {
                        self.project.discussions.push(ko.mapping.fromJS(data));
                        self.discussion.clear();
                    },
                    400: /*bad request*/function () {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    self.project.discussions._remove = function () {
        var discussion = this;
        var message = 'You are about to delete ' + discussion.name() + '.';
        if (confirm(message)) {
            $.ajax(endpoint + '/discussions/' + discussion.id(),
                {
                    type: 'delete',
                    statusCode: {
                        204: /*no content*/function () {
                            self.project.discussions.remove(discussion);
                        }
                    }
                });
        }
    };
    self.project.activities._create = function () {
        $.ajax(endpoint + '/activities/',
            {
                type: 'post',
                data: ko.toJSON(self.activity),
                statusCode: {
                    201: /*created*/function (data) {
                        self.project.activities.push(new TW.viewmodels.models.Activity(data));
                        self.activity.clear();
                    },
                    400: /*bad request*/function () {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    self.project.activities._remove = function () {
        var activity = this;
        var message = 'You are about to delete ' + activity.name() + '.';
        if (confirm(message)) {
            $.ajax(endpoint + '/activities/' + activity.id(),
                {
                    type: 'delete',
                    statusCode: {
                        204: /*no content*/function () {
                            self.project.activities.remove(activity);
                        }
                    }
                });
        }
    };

    self.people.list = ko.observableArray([]);
    self.people._add = function (project, event, person) {
        if (person) {
            self.people.list.push(person);
            self.people("");
        }
    };
    self.people._ids = ko.computed(function () {
        return self.people.list().map(function (item) {
            var i = parseInt(item.id);
            return isNaN(i) ? 0 : i;
        });
    });

    self.project.people._add = function () {
        $.ajax(endpoint + '/people', {
            type: 'post',
            data: ko.toJSON({ ids: self.people._ids() }),
            statusCode: {
                204: /*created*/function () {
                    $.ajax(endpoint + '/people',
                            {
                                statusCode: {
                                    200: /*ok*/function (data) {
                                        self.project.people(
                                            $.map(data, function (item) {
                                                return new TW.viewmodels.models.Person(item);
                                            }));
                                    }
                                }
                            }
                        );
                    self.people.list([]);
                    self.people.editing(false);
                    self.people("");
                },
                400: /*bad request*/function () {
                    TW.app.alerts.push({ message: 'An error as ocurred.' });
                }
            }
        }
        );
    };
    self.project.people._remove = function () {
        var person = this;
        var message = 'You are about to remove ' + person.name() + ' from the project.';
        if (confirm(message)) {
            $.ajax(endpoint + '/people/' + person.id(),
                {
                    type: 'delete',
                    statusCode: {
                        204: /*no content*/function () {
                            self.project.people.remove(person);
                        }
                    }
                });
        }
    };

    self.Dependency = function () {
        $.ajax(endpoint + '/precedences',
            {
                type: 'get',
                statusCode: {
                    200: /*ok*/function (data) {
                        self.DependencyGraph(data);

                    },
                    404: /*bad request*/function () {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    },
                    400: /*bad request*/function () {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            });
    };

    $.ajax(endpoint, {
        async: false,
        statusCode: {
            200: /*ok*/function (data) {
                self.project.load(data);
            },
            404: /*not found*/function () {
                TW.app.alerts.push({ message: 'The project you requested doesn\'t exist.' });
            }
        }
    });

    self.Dependency();

};

TW.viewmodels.Projects = function(endpoint) {
    var self = this;
    self.project = new TW.viewmodels.models.Project();
    self.project.editing = ko.observable(false);

    self.projects = ko.observableArray([]);
    self.projects._create = function() {
        $.ajax(endpoint,
            {
                type: 'post',
                data: ko.toJSON(self.project),
                statusCode: {
                    201: /*created*/function(data) {
                        self.projects.push(new TW.viewmodels.models.Project(data));
                        self.project.editing(false);
                        self.project.clear();
                    },
                    400: /*bad request*/function(data) {
                        TW.helpers.bad_format(self.project, data.responseText);
                    }
                }
            }
        );
    };

    self.projects._remove = function() {
        var project = this;
        var message = 'You are about to delete ' + project.name() + '.';
        if (confirm(message)) {
            $.ajax(endpoint + project.id(),
                {
                    type: 'delete',
                    statusCode: {
                        204: /*no content*/function() {
                            self.projects.remove(project);
                        }
                    }
                });
        }
    };

    $.getJSON(endpoint, function(projects) {
        self.projects(
            $.map(projects, function(item) {
                return new TW.viewmodels.models.Project(item);
            }));
    });
};

TW.viewmodels.Discussion = function(endpoint) {

    var self = this;
    self.message = new TW.viewmodels.models.Message();
    self.message.editing = ko.observable(false);

    self.discussion = new TW.viewmodels.models.Discussion();
    self.discussion.messages.create = function() {
        $.ajax(endpoint + '/messages/',
            {
                type: 'post',
                data: ko.toJSON(self.message),
                statusCode: {
                    201: /*created*/function(data) {
                        self.discussion.messages.push(new TW.viewmodels.models.Message(data));
                        self.message.editing(false);
                        self.message.clear();
                    },
                    400: /*bad request*/function() {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };

    $.ajax(endpoint,
        {
            async: false,
            type: 'get',
            statusCode: {
                200: /*ok*/function(data) {
                    self.discussion.load(data);
                },
                404: /*not found*/function() {
                    TW.app.alerts.push({ message: 'The project you requested doesn\'t exist.' });
                }
            }
        });
};

TW.viewmodels.Activity = function(endpoint) {

    var self = this;
    self.timelog = new TW.viewmodels.models.Timelog();
    self.activity = new TW.viewmodels.models.Activity();

    self.timelog.create = function() {
        $.ajax(endpoint + '/timelogs/',
            {
                type: 'post',
                data: ko.toJSON(self.timelog),
                statusCode: {
                    201: /*created*/function(data) {
                        self.activity.timelogs.push(new TW.viewmodels.models.Timelog(data));
                        self.timelog.clear();
                    },
                    400: /*bad request*/function() {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };

    $.ajax(endpoint,
        {
            async: false,
            type: 'get',
            statusCode: {
                200: /*ok*/function(data) {
                    self.activity.load(data);
                },
                404: /*not found*/function() {
                    TW.app.alerts.push({ message: 'The project you requested doesn\'t exist.' });
                }
            }
        });
};

TW.viewmodels.Timelogs = function() {
    var self = this;
    self.activity = {
        id: ko.observable(),
        name: ko.observable(),
        project_id: ko.observable(),
        project_name: ko.observable()
    };

    self.activity_description = ko.computed(function() {
        return self.activity.project_name + self.activity.name;
    });
    self.timelog = new TW.viewmodels.models.Timelog();
    self.timelog.create = function() {
        $.ajax('/api/projects/' + self.activity.project_id() + '/activities/' + self.activity.id() + '/timelogs/',
            {
                type: 'post',
                data: ko.toJSON(self.timelog),
                statusCode: {
                    201: /*created*/function(data) {
                        self.activity.timelogs.push(new TW.viewmodels.models.Timelog(data));
                        self.timelog.clear();
                    },
                    400: /*bad request*/function() {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    
    self.tabs = [
        { description: "2 days ago", date: Date.today().add(-2).days().toISOString(), calendar: false },
        { description: "Yesterday", date: Date.today().add(-1).days().toISOString(), calendar: false },
        { description: "Today", date: Date.today().toISOString(), calendar: false },
        { description: "Calendar", date: Date.today().toISOString(), calendar: true }];

    self.selected = ko.observable(self.tabs[2]);
    self.select = function(item) {
        self.selected(item);
        self.timelog.date(item.date);
    };
}