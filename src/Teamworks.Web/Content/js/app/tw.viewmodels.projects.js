﻿/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />
/// <reference path="~/Content/js/libs/knockout.mapping.debug.js" />

(function(obj) {
    obj.Project = function(endpoint) {
        var people_endpoint = endpoint + '/people/';
        var activities_endpoint = endpoint + '/activities/';
        var discussions_endpoint = endpoint + '/discussions/';

        var self = this;

        self.project = ko.mapping.fromJS({ });
        
        // people
        self.project.person = {
            name: ko.observable(),
            editing: ko.observable(),
            invites: ko.observableArray([]),
            _add: function () {
                alert('');
            }
        };

        self.project.people = ko.mapping.fromJS([]);
        // activities
        self.project.activity = {
            name: ko.observable(),
            editing: ko.observable()
        };
        
        self.project.activities = ko.mapping.fromJS([]);
        self.project.activities._create = function() {
            $.ajax(activities_endpoint,
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.project.activity.name() }),
                    statusCode: {
                        201: /*created*/function(data) {
                            self.project.activities.push(ko.mapping.fromJS(data));
                            self.project.activity.name("");
                        },
                        400: /*bad request*/function() {
                            TW.app.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };
        self.project.activities._remove = function() {
            var activity = this;
            var message = 'You are about to delete ' + activity.name() + '.';
            if (confirm(message)) {
                $.ajax(activities_endpoint + activity.id(),
                    {
                        type: 'delete',
                        statusCode: {
                            204: /*no content*/function() {
                                self.project.activities.remove(activity);
                            }
                        }
                    });
            }
        };
        
        // discussions
        self.project.discussion = {
            name: ko.observable(),
            editing: ko.observable()
        };
        
        self.project.discussions = ko.mapping.fromJS([]);
        self.project.discussions._create = function() {
            $.ajax(discussions_endpoint,
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.project.discussion.name() }),
                    statusCode: {
                        201: /*created*/function(data) {
                            self.project.discussions.push(ko.mapping.fromJS(data));
                            self.project.discussions.name("");
                        },
                        400: /*bad request*/function() {
                            TW.app.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };
        self.project.discussions._remove = function() {
            var discussion = this;
            var message = 'You are about to delete ' + discussion.name() + '.';
            if (confirm(message)) {
                $.ajax(discussions_endpoint + discussion.id(),
                    {
                        type: 'delete',
                        statusCode: {
                            204: /*no content*/function() {
                                self.project.discussions.remove(discussion);
                            }
                        }
                    });
            }
        };

        $.ajax(endpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project);
                }
            }
        });

        $.ajax(people_endpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project.people);
                }
            }
        });

        $.ajax(activities_endpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project.activities);
                }
            }
        });
        
        $.ajax(discussions_endpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project.discussions);
                }
            }
        });
    };
}(TW.viewmodels));

(function(obj) {
    obj.Projects = function(endpoint) {
        var self = this;

        self.name = ko.observable().extend({ required: "" });
        self.editing = ko.observable(false);

        self.projects = ko.observableArray([]);
        self.projects._create = function() {
            $.ajax(endpoint,
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.name() }),
                    statusCode: {
                        201: /*created*/function(data) {
                            self.projects.push(new TW.viewmodels.models.Project(data));
                            self.editing(false);
                            self.name('');
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
}(TW.viewmodels));