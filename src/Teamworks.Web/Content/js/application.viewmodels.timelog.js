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
};