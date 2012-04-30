(function () {
    'use strict'
    var ENTER_KEY = 13;
    var ESCAPE_KEY = 27;

    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    var project = function (data) {
        /* self region */
        var self = this;
        self.id = ko.observable();
        self.name = ko.observable();
        self.description = ko.observable();
        self.editing = ko.observable();
        self.url = ko.computed(function () {
            return "/projects/view/" + self.id();
        });
        var map = function (other) {
            self.id(other.id || self.id() || "0");
            self.name(other.name || self.name() || "");
            self.description(other.description || self.description() || "");
        };
        map(data || {});

        /* interactions */
        var old;
        self.edit = function () {
            if (!self.editing()) {
                self.editing(true);
                old = ko.toJSON(self);
            }
        };
        self.cancel = function () {
            map(old);
        };
    };

    var projects_viewmodel = function () {
        var self = this;

        /* new project */
        self.project = new project();

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
                        self.projects.push(new project(data));
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
                return new project(item);
            }));
        });
    };

    ko.applyBindings(new projects_viewmodel());
})();