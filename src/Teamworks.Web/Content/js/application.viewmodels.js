var Project = function (data) {
    var self = this;
    data = data || { };

    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.description = ko.observable(data.description);

    self.clear = function () {
        self.id("");
        self.name("");
        self.description("");
    };

    self.url = ko.computed(function () {
        return "/projects/" + self.id();
    });
};

var Task = function (data) {
    var self = this;
    data = data || {};
    
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.project = ko.observable(data.project);
    self.description = ko.observable(data.description);

    self.clear = function () {
        self.id("");
        self.name("");
        self.project("");
        self.description("");
    };

    self.url = ko.computed(function () {
        return "/projects/" + self.project() + "/tasks/" + self.id();
    }, this);
};

var Timelog = function (data, taskid, projectid) {
    var self = this;
    self.id = ko.observable((data && data.id) || "0");
    self.description = ko.observable(data && data.description);
    self.date = ko.observable(data && data.date);
    self.duration = ko.observable(data && data.duration);
    self.person = ko.observable(data && data.person);
    self.url = ko.computed(function () {
        return "/" + projectid + "/tasks/" + taskid + "/timelog/" + self.id() + "/view";
    }, this);
};


//todo check
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

