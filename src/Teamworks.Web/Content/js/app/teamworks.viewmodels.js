var TW = TW || { };
TW.viewmodels = TW.viewmodels || { };

TW.viewmodels.Project = function (endpoint) {
    var self = this;
    self.project = new TW.viewmodels.models.Project();
    self.discussion = new TW.viewmodels.models.Discussion();
    self.activities = new TW.viewmodels.models.Activity();

    self.discussion.editing = ko.observable(false);
    self.activities.editing = ko.observable(false);

    self.project.discussions.create = function () {
        $.ajax(endpoint + self.project.id() + '/discussions/',
            {
                type: 'post',
                data: ko.toJSON(self.discussion),
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
                statusCode: {
                    201: /*created*/function (data) {
                        self.project.discussions.push(new TW.viewmodels.models.Discussion(data));
                        self.discussion.editing(false);
                        self.discussion.clear();
                    },
                    400: /*bad request*/function () {
                        TW.app.messages.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    self.project.discussions.remove = function () {
        var discussion = this;
        var message = 'You are about to delete ' + discussion.name() + '.';
        if (confirm(message)) {
            $.ajax(endpoint + self.project.id() + '/discussions/' + discussion.id(),
                {
                    type: 'delete',
                    contentType: 'application/json; charset=utf-8',
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function () {
                            self.project.discussions.destroy(project);
                        }
                    }
                });
        }
    };

    $.ajax(endpoint,
        {
            async: false,
            type: 'get',
            contentType: 'application/json; charset=utf-8',
            cache: 'false',
            statusCode: {
                200: /*ok*/function (data) {
                    self.project.load(data);
                },
                404: /*not found*/function () {
                    TW.app.messages.push({ message: 'The project you requested doesn\'t exist.' });
                }
            }
        });
};

TW.viewmodels.Projects = function(endpoint) {

    var self = this;
    self.project = new TW.viewmodels.models.Project();
    self.project.editing = ko.observable(false);
    self.projects = ko.observableArray([]);
    self.projects.create = function() {
        $.ajax(endpoint,
            {
                type: 'post',
                data: ko.toJSON(self.project),
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
                statusCode: {
                    201: /*created*/function(data) {
                        self.projects.push(new TW.viewmodels.models.Project(data));
                        self.project.editing(false);
                        self.project.clear();
                    },
                    400: /*bad request*/function(data) {
                        TW.app.messages.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };

    self.projects.remove = function() {
        var project = this;
        var message = 'You are about to delete ' + project.name() + '.';
        if (confirm(message)) {
            $.ajax(endpoint + project.id(),
                {
                    type: 'delete',
                    contentType: 'application/json; charset=utf-8',
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function() {
                            self.projects.destroy(project);
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
    self.discussion = new TW.viewmodels.models.Discussion();

    $.ajax(endpoint,
        {
            async: false,
            type: 'get',
            contentType: 'application/json; charset=utf-8',
            cache: 'false',
            statusCode: {
                200: /*ok*/function(data) {
                    self.discussion.load(data);
                },
                404: /*not found*/function() {
                    TW.app.messages.push({ message: 'The project you requested doesn\'t exist.' });
                }
            }
        });


};