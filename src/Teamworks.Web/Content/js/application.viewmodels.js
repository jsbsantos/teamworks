var Timelog = function (data) {
    var self = this;
    self.id = ko.observable("0");
    self.description = ko.observable();
    self.date = ko.observable();
    self.duration = ko.observable();
    self.person = ko.observable();

    self.task = ko.observable();
    
    self.editing = ko.observable();
    
    var map = function (other) {
        self.id(other.id || self.id());
        self.description(other.description || self.description());
        self.date(other.date || self.date());
        self.duration(other.duration || self.duration());
        self.person(other.person || self.person());
    };
    map(data || {});
};


var task = function (data) {
    var self = this;
    self.id = ko.observable("0");
    self.name = ko.observable();
    self.projectid = ko.observable();
    self.description = ko.observable();
    self.editing = ko.observable();
    self.timelog = ko.observableArray();
    
    self.url = ko.computed(function () {
        return "/" + self.projectid() + "/tasks/" + self.id();
    }, this);
    
    var map = function (other) {
        self.id(other.id || self.id());
        self.name(other.name || self.name());
        self.description(other.description || self.description());
        self.projectid(other.projectid || self.projectid());

        self.timelog($.map(other.timelog || self.timelog() || [], function (t) {
            return new Timelog(t);
        }));
    };
    map(data || {});
};

var project = function(data) {
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
        self.tasks($.map(other.tasks || self.tasks() || [], function(t) {
            return new task(t);
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

var project_list_viewmodel = function() {
    var self = this;

    /* new project */
    self.project = new project();
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
                    self.projects.push(new project(data));
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
            return new project(item);
        }));
    });
};

var task_list_viewmodel = function(projectid) {
    var self = this;

    /* new taskModel */
    self.task = new task(projectid);
    self.requestUrl = "/api/projects/" + projectid + "/tasks/";
    /* taskModel interactions */

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
                    self.tasks.push(new task(data));
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
            return new task(item);
        }));
    });
    

    
var TimelogViewmodel = function(task) {
    var self = this;

    /* new TimeEntryViewmodel */
    self.task = task;
    self.requestUrl = "/api/projects/" + task.projectid() + "/tasks/" + task.id() + "/timelog";
    /* TimeEntryViewmodel interactions */

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