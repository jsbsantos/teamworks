/// <reference path="~/Views/Home/post/Content/js/application.viewmodels.js" />

(function () {
    'use strict';
    var ENTER_KEY = 13;
    var ESCAPE_KEY = 27;

    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    var projects_viewmodel = function () {
        var self = this;

        /* new project */
        self.project = new Project();
        /* projects interactions */
        self.create = function () {
            var request = $.ajax("/api/projects", {
                data: ko.toJSON(self.project),
                type: 'post',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    201: /*created*/function (data) {
                        // push a new project
                        data.url = request.getResponseHeader("Location");
                        self.projects.push(new Project(data));
                        $('#project-modal').hide();
                    }
                }
            });
        };
        self.update = function () {
            var project = this;
            if (confirm('You are about to update ' + project.name() + '.')) {
                $.ajax(project.url(), {
                    data: ko.toJSON(project),
                    type: 'put',
                    contentType: "application/json; charset=utf-8",
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function () {
                            project.editing(false);
                        }
                    }
                });
            }
        };
        self.remove = function () {
            var project = this;
            if (confirm('You are about to delete ' + project.name() + '.')) {
                $.ajax("/api/projects/" + project.id(), {
                    type: 'delete',
                    contentType: "application/json; charset=utf-8",
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function () {
                            self.projects.destroy(project);
                        }
                    }
                });
            }
        };

        /* projects */
        self.projects = ko.observableArray([]);
        $.getJSON("/api/projects", function (data) {
            self.projects($.map(data, function (item) {
                return new Project(item);
            }));
        });
    };

    ko.applyBindings(new projects_viewmodel());
})();