var timelog = function (data, taskid, projectid) {
    var self = this;
    self.id = ko.observable(data.id);
    self.description = ko.observable(data.description);
    self.date = ko.observable(data.date);
    self.duration = ko.observable(data.duration);
    self.person = ko.observable(data.person);
    self.url = ko.computed(function () {
        return "/" + projectid + "/tasks/" + taskid + "/timelog/" + self.id() + "/view";
    }, this);
};

var task_viewmodel = function (task) {
    var self = this;

    self.timelog = new timelog();
    self.requestUrl = "/api/projects/" + task.project + "/tasks/" + task.id + "/timelog/";

    self.create = function () {
        var request = $.ajax(self.requestUrl, {
            data: ko.toJSON(self.timelog),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    data.url = request.getResponseHeader("Location");
                    self.timelog_list.push(new timelog(data));
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
            $.ajax(self.requestUrl + timelog.id(), {
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

    /* timelog */
    self.timelog_list = ko.observableArray($.map(task.timelog, function (item) {
        return new timelog(item);
    }));
};