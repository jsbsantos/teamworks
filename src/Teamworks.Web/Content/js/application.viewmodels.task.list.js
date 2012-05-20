var task = function (data) {
    var self = this;
    self.id = ko.observable("0");
    self.name = ko.observable();
    self.project = ko.observable();
    self.description = ko.observable();
    self.timelog = ko.observableArray();
    
    self.url = ko.computed(function () {
        return "/" + self.project() + "/tasks/" + self.id() + "/view";
    }, this);

    var map = function (other) {
        self.id(other.id || self.id());
        self.name(other.name || self.name());
        self.description(other.description || self.description());
        self.project(other.project || self.project());
        if (other.timelog)
            self.timelog($.map(other.timelog));
    };

    self.clear = function () {
        map({
            id: "0",
            name: "",
            project: "",
            description: "",
            timelog: []
        });
    };

    map(data || {});
};


var task_list_viewmodel = function (projectid, tasks) {
    var self = this;

    self.task = new task(projectid);

    self.create = function () {
        var request = $.ajax(self.requestUrl, {
            data: ko.toJSON(self.task),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    // push a new task
                    data.url = request.getResponseHeader("Location");
                    self.tasks.push(new task(data));
                    $('#task-modal').modal('hide');
                    self.task.clear();
                }
            }
        });
    };

    /* new taskModel */
    self.requestUrl = "/api/projects/" + projectid + "/tasks/";
    /* taskModel interactions */

    self.update = function () {
        var task = this;
        if (confirm('You are about to update ' + task.name() + '.')) {
            $.ajax(task.url(), {
                data: ko.toJSON(task),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false'
            });
        }
    };

    self.remove = function () {
        var task = this;
        if (confirm('You are about to delete ' + task.name() + '.')) {
            $.ajax(self.requestUrl + task.id(), {
                type: 'delete',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function () {
                        self.tasks.remove(task);
                    }
                }
            });
        }
    };

    /* tasks */
    self.tasks = ko.observableArray($.map(tasks, function(item) {
        return new task(item);
    }));
};