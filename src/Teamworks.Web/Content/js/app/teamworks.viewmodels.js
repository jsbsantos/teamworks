/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />

var TW = TW || { };
TW.viewmodels = TW.viewmodels || { };

TW.viewmodels.Project = function(data) {
    var self = this;
    var map = function(data) {
        self.id(data.id);
        self.name(data.name);
        self.description(data.description);
    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.description = ko.observable();

    self.clear = function() {
        map({ });
    };
    map(data || { });
};

TW.viewmodels.Projects = function() {
    var self = this;
    self.endpoint = "/api/projects/";
    self.project = new TW.viewmodels.Project();
    self.projects = ko.observableArray([]);

    /*interact methods*/
    self.create = function() {
        var request = $.ajax(
            self.endpoint,
            {
                type: 'post',
                data: ko.toJSON(self.project),
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    201: /*created*/function(data) {
                        self.projects.push(new TW.viewmodels.Project(data));
                        self.project.clear();
                    }
                }
            }
        );
    };
    self.remove = function() {
        var project = this;
        var message = 'You are about to delete ' + project.name() + '.';
        if (confirm(message)) {
            $.ajax(
                self.endpoint + project.id(),
                {
                    type: 'delete',
                    contentType: "application/json; charset=utf-8",
                    cache: 'false',
                    statusCode: {
                        204: /*no content*/function() {
                            self.projects.destroy(project);
                        }
                    }
                });
        }
    };

    $.getJSON(self.endpoint, function(projects) {
        self.projects($.map(projects, function(item) {
            return new TW.viewmodels.Project(item);
        }));
    });
};