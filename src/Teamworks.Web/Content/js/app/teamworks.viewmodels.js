var TW = TW || { };
TW.viewmodels = TW.viewmodels || { };

TW.viewmodels.Project = function (endpoint) {
    var self = this;

    self.project = new TW.viewmodels.models.Project();

    self.activity = new TW.viewmodels.models.Activity();
    self.activity.editing = ko.observable(false);

    self.discussion = new TW.viewmodels.models.Discussion();
    self.discussion.editing = ko.observable(false);

    self.project.discussions.create = function () {
        $.ajax(endpoint + '/discussions/',
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
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    self.project.discussions.remove = function () {
        var discussion = this;
        var message = 'You are about to delete ' + discussion.name() + '.';
        if (confirm(message)) {
            $.ajax(endpoint + '/discussions/' + discussion.id(),
                {
                    type: 'delete',
                    contentType: 'application/json; charset=utf-8',
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function () {
                            self.project.discussions.destroy(discussion);
                        }
                    }
                });
        }
    };
    self.project.activities.create = function () {
        $.ajax(endpoint + '/activities/',
            {
                type: 'post',
                data: ko.toJSON(self.activity),
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
                statusCode: {
                    201: /*created*/function (data) {
                        self.project.activities.push(new TW.viewmodels.models.Activity(data));
                        self.activity.editing(false);
                        self.activity.clear();
                    },
                    400: /*bad request*/function () {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };

    self.people = ko.observableArray([]);
    self.people.editing = ko.observable(false);
    self.people.ids = ko.computed(function () {
        return self.people().map(function (item) {
            var int = parseInt(item.id());
            return isNaN(int) ? 0 : int;
        });
    });
    self.people.more = function () {
        for (var i in [0, 1, 2]) {
            self.people.push({ id: ko.observable("") });
        }
    };

    self.project.people.add = function () {
        var data = {
            ids: self.people.ids()
        };

        $.ajax(endpoint + '/people/',
            {
                type: 'post',
                data: ko.toJSON(data),
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
                statusCode: {
                    204: /*created*/function () {
                        $.getJSON(endpoint + '/people', function (people) {
                            self.project.people(
                                $.map(people, function (item) {
                                    return new TW.viewmodels.models.Person(item);
                                }));
                        });
                        self.people.editing(false);
                        self.people([]);
                        self.people.more();
                    },
                    400: /*bad request*/function () {
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
                    }
                }
            }
        );
    };
    self.project.people.remove = function () {
        var person = this;
        var message = 'You are about to remove ' + person.name() + ' from the project.';
        if (confirm(message)) {
            $.ajax(endpoint + '/people/' + person.id(),
                {
                    type: 'delete',
                    contentType: 'application/json; charset=utf-8',
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function () {
                            self.project.people.destroy(person);
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
                    TW.app.alerts.push({ message: 'The project you requested doesn\'t exist.' });
                }
            }
        });
    self.people.more();
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
                        TW.app.alerts.push({ message: 'An error as ocurred.' });
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
    self.message = new TW.viewmodels.models.Message();
    self.message.editing = ko.observable(false);

    self.discussion = new TW.viewmodels.models.Discussion();
    self.discussion.messages.create = function() {
        $.ajax(endpoint + '/messages/',
            {
                type: 'post',
                data: ko.toJSON(self.message),
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
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
            contentType: 'application/json; charset=utf-8',
            cache: 'false',
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
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
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
            contentType: 'application/json; charset=utf-8',
            cache: 'false',
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