/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />

var TW = TW || { };
TW.viewmodels = TW.viewmodels || { };

TW.viewmodels.Project = function (data) {
    var self = this;
    self.base = "/api/projects/";
    self.load = function (data) {
        self.id(data.id);
        self.name(data.name);
        self.description(data.description);
        self.discussions.data(data.discussions);
    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.description = ko.observable();
    self.discussions = new TW.viewmodels.Discussions();
    self.discussions.endpoint = ko.computed(function () {
        return self.base + self.id() + "/discussions/";
    });

    self.endpoint = ko.computed(function () {
        return self.base + self.id();
    }, self);

    self.clear = function () {
        self.load({});
    };
    self.load(data || {});
};

TW.viewmodels.Projects = function() {
    var self = this;
    self.endpoint = "/api/projects/";

    self.data = ko.observableArray([]);
    /*interact methods*/
    self.create = function(project, callback) {
        var request = $.ajax(
            self.endpoint,
            {
                type: 'post',
                data: ko.toJSON(project),
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
                statusCode: {
                    201: /*created*/function(data) {
                        self.data.push(new TW.viewmodels.Project(data));
                        callback && callback();
                    },
                    400: /*bad request*/function(data) {
                        TW.app.messages.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    self.remove = function(element, callback) {
        var project = this;
        var message = 'You are about to delete ' + project.name() + '.';
        if (confirm(message)) {
            $.ajax(
                self.endpoint + project.id(),
                {
                    type: 'delete',
                    contentType: 'application/json; charset=utf-8',
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function() {
                            self.data.destroy(project);
                            callback && callback();
                        }
                    }
                });
        }
    };
};

TW.viewmodels.Discussion = function (data) {
    var self = this;
    self.load = function (data) {
        self.id(data.id);
        self.name(data.name);
        self.content(data.content);
    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.content = ko.observable();
    self.endpoint = ko.computed(function () {
        return self.base + self.id();
    }, self);
    self.clear = function () {
        self.load({});
    };
    self.load(data || {});
};


TW.viewmodels.Discussions = function () {
    var self = this;
    self.data = ko.observableArray([]);
    
    /*interact methods*/
    self.create = function (discussion, callback) {
        var request = $.ajax(
            self.endpoint(),
            {
                type: 'post',
                data: ko.toJSON(discussion),
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
                statusCode: {
                    201: /*created*/function (data) {
                        self.data.push(new TW.viewmodels.Discussion(data));
                        self.base = self.endpoint;
                        callback && callback();
                    },
                    400: /*bad request*/function (data) {
                        TW.app.messages.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    self.remove = function (element, callback) {
        var discussion = this;
        var message = 'You are about to delete ' + discussion.name() + '.';
        if (confirm(message)) {
            $.ajax(
                self.endpoint + discussion.id(),
                {
                    type: 'delete',
                    contentType: 'application/json; charset=utf-8',
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function () {
                            self.data.destroy(discussion);
                            callback && callback();
                        }
                    }
                });
        }
    };
};