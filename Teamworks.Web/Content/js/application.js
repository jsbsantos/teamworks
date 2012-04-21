(function () {
    'use strict';

    var Project = function (data) {
        var self = this;

        data = data || {};
        self.name = ko.observable(data.name || "");
        self.description = ko.observable(data.description || "");
    };

    var Model = function (projects) {
        var self = this;

        self.projects = ko.observableArray($.map(projects, function (e) {
            return new Project(e);
        }));

        self.project = new Project();
        self.new_project = function () {
            $.ajax("/api/projects", {
                data: ko.toJSON(self.project),
                type: 'post', contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    201: /*created*/ function (data) {
                        // push a new project
                        self.projects.push(new Project(data));
                        // clean the project
                        self.project.name("");
                        self.project.description("");
                    }    
                }
            });
        };
    };

    ko.applyBindings(new Model(data || []));
})()
