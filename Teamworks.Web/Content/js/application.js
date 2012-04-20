(function () {
    'use strict';

    var Project = function (data) {
        var self = this;

        data = data || {};
        self.name = ko.observable(data.name || "");
        self.description = ko.observable(data.description || "");

        self.tasks = ko.observableArray([]);
    };

    var Model = function (projects) {
        var self = this;

        self.projects = ko.observableArray($.map(projects, function (e) {
            return new Project(e);
        }));

        self.project = new Project();
        self.new_project = function () {
            //create new project
            var p = new Project({
                name: self.project.name(),
                description: self.project.description()
            });
            // push a new project
            self.projects.push(p);
            // clean the project
            self.project.name("");
            self.project.description("");
        };
    };

    ko.applyBindings(new Model(data || []));

})()
