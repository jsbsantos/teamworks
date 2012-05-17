var Task = function(data) {
    var self = this;
    self.id = ko.observable("0");
    self.name = ko.observable();
    self.projectid = ko.observable();
    self.description = ko.observable();
    self.editing = ko.observable();
    self.url = ko.computed(function() {
        return "/projects/" + self.projectid() + "/tasks/" + self.id();
    });
    var map = function(other) {
        self.id(other.id || self.id());
        self.name(other.name || self.name());
        self.description(other.description || self.description());
        self.projectid(other.projectid || self.projectid());
    };
    map(data || { });
};

var Project = function(data) {
    /* self region */
    var self = this;
    self.id = ko.observable("0");
    self.name = ko.observable();
    self.description = ko.observable();
    self.editing = ko.observable();
    self.url = ko.computed(function() {
        return "/projects/view/" + self.id();
    });
    self.tasks = ko.observableArray();
    var map = function(other) {
        self.id(other.id || self.id());
        self.name(other.name || self.name());
        self.description(other.description || self.description());
        self.tasks($.map(other.tasks || self.tasks() || [], function(data) {
            return new Task(data);
        }));
    };
    map(data || { });

    /* interactions */
    var old;
    self.edit = function() {
        if (!self.editing()) {
            self.editing(true);
            old = ko.toJSON(self);
        }
    };
    self.cancel = function() {
        map(old);
    };
};

var ProjectsViewmodel = function() {
    var self = this;

    /* new project */
    self.project = new Project();
    /* projects interactions */

    self.create = function() {
        var request = $.ajax("/api/projects", {
            data: ko.toJSON(self.project),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function(data) {
                    // push a new project
                    data.url = request.getResponseHeader("Location");
                    self.projects.push(new Project(data));
                    $('#project-modal').modal('hide');
                }
            }
        });
    };
    self.update = function() {
        var project = this;
        if (confirm('You are about to update ' + project.name() + '.')) {
            $.ajax(project.url(), {
                data: ko.toJSON(project),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function() {
                        project.editing(false);
                    }
                }
            });
        }
    };
    self.remove = function() {
        var project = this;
        if (confirm('You are about to delete ' + project.name() + '.')) {
            $.ajax("/api/projects/" + project.id(), {
                type: 'delete',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function() {
                        self.projects.destroy(project);
                    }
                }
            });
        }
    };

    /* projects */
    self.projects = ko.observableArray([]);
    $.getJSON("/api/projects", function(data) {
        self.projects($.map(data, function(item) {
            return new Project(item);
        }));
    });
};

var TasksViewmodel = function(projectid) {
    var self = this;

    /* new TaskModel */
    self.task = new Task(projectid);
    self.requestUrl = "/api/projects/" + self.task.projectid() + "/tasks/";

    /* TaskModel interactions */

    self.create = function() {
        var request = $.ajax(self.requestUrl, {
            data: ko.toJSON(self.task),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function(data) {
                    // push a new task
                    data.url = request.getResponseHeader("Location");
                    self.tasks.push(new Task(data));
                    $('#task-modal').modal('hide');
                }
            }
        });
    };
    self.update = function() {
        var task = this;
        if (confirm('You are about to update ' + task.name() + '.')) {
            $.ajax(task.url(), {
                data: ko.toJSON(task),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function() {
                        task.editing(false);
                    }
                }
            });
        }
    };
    self.remove = function() {
        var task = this;
        if (confirm('You are about to delete ' + task.name() + '.')) {
            $.ajax(self.requestUrl + task.id(), {
                type: 'delete',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function() {
                        self.tasks.destroy(task);
                    }
                }
            });
        }
    };

    /* tasks */
    self.tasks = ko.observableArray([]);
    $.getJSON(self.requestUrl, function(data) {
        self.tasks($.map(data, function(item) {
            return new Task(item);
        }));
    });
};