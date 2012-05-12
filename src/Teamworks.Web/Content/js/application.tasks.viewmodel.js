/// <reference path="~/Content/js/application.viewmodels.js" />

(function () {
    'use strict';
    var ENTER_KEY = 13;
    var ESCAPE_KEY = 27;

    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    var tasks_viewmodel = function () {
        var self = this;

        /* new Task */
        self.task = new Task();
        self.requestUrl = "/api/project/" + self.task.projectid() + "/tasks/";
        
        /* Task interactions */
        
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
                        self.tasks.push(new Task(data));
                        $('#task-modal').modal('hide');
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
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function () {
                            task.editing(false);
                        }
                    }
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
                            self.tasks.destroy(task);
                        }
                    }
                });
            }
        };

        /* tasks */
        self.tasks = ko.observableArray([]);
        $.getJSON(self.requestUrl, function (data) {
            self.tasks($.map(data, function (item) {
                return new Task(item);
            }));
        });
    };

    ko.applyBindings(new tasks_viewmodel());
})();