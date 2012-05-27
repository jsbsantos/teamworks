var task_viewmodel = function (task) {
    var self = this;

    self.timelog = new Timelog();
    self.endpoint = "/api/projects/" + task.project() + "/tasks/" + task.id() + "/timelog/";
    self.timelog_list = ko.observableArray(
        $.map(task.timelog, function (item) {
            return new Timelog(item);
        })
    );

    /* interact methods */
    self.create = function () {
        var request = $.ajax(self.endpoint, {
            data: ko.toJSON(self.timelog),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    data.url = request.getResponseHeader("Location");
                    self.timelog_list.push(new Timelog(data));
                    $('#timelog-modal').modal('hide');
                    self.timelog.clear();
                }
            }
        });
    };
    self.update = function () {
        var timelog = this;
        if (confirm('You are about to update ' + task.name() + '.')) {
            $.ajax(timelog.url(), {
                data: ko.toJSON(timelog),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false'
            });
        }
    };
    self.remove = function () {
        var timelog = this;
        if (confirm('You are about to delete a timelog entry.')) {
            $.ajax(self.endpoint + timelog.id(), {
                type: 'delete',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function () {
                        self.timelog_list.remove(timelog);
                    }
                }
            });
        }
    };
};

var task_list_viewmodel = function (projectid, tasks) {
    var self = this;
    self.task = new Task();
    
    self.endpoint = "/api/projects/" + projectid + "/tasks/";
    self.tasks = ko.observableArray(
        $.map(tasks, function (item) {
            return new Task(item);
        })
    );
    
    /* interact methods */
    self.create = function () {
        var request = $.ajax(self.endpoint, {
            data: ko.toJSON(self.task),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    // push a new task
                    data.url = request.getResponseHeader("Location");
                    self.tasks.push(new Task(data));
                    $('#task-modal').modal('hide');
                    self.task.clear();
                }
            }
        });
    };
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
            $.ajax(self.endpoint + task.id(), {
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
};