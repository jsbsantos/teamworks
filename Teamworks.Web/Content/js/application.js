(function () {
    'use strict';

    var Project = function (data) {
        var self = this;

        data = data || {};
        self.url = ko.observable(data.url || "");
        self.name = ko.observable(data.name || "");
        self.description = ko.observable(data.description || "");

        self.editing = ko.observable(false);
        self.toggle_edit = function () {
            self.editing(!self.editing());
        };
    };

    var Model = function (projects) {
        var self = this;

        self.projects = ko.observableArray($.map(projects, function (e) {
            return new Project(e);
        }));

        self.project = new Project();

        self.clear = function () {
            self.project.name("");
            self.project.description("");
        };

        self.new_project = function () {
            var request = $.ajax("/api/projects", {
                data: ko.toJSON(self.project),
                type: 'post', contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    201: /*created*/function (data) {
                        // push a new project
                        data.url = request.getResponseHeader("Location");
                        self.projects.push(new Project(data));
                        // clean the project
                        self.project.name("");
                        self.project.description("");
                    }
                }
            });
        };

        self.save = function (project) {
            $.ajax(project.url(), {
                data: ko.toJSON(project),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function () {
                        alert('changes saved');
                        project.toggle_edit();
                    }
                }
            });
        };
    };

    ko.applyBindings(new Model(data || []));
})()
