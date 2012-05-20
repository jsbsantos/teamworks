var project = function(data) {
    /* self region */
    var self = this;
    self.id = ko.observable("0");
    self.name = ko.observable();
    self.description = ko.observable();
    self.url = ko.computed(function() {
        return "/projects/view/" + self.id();
    });
    var clear = function() { map({ id: "0", name: "", description: "" }); };
    var map = function(other) {
        self.id(other.id || self.id());
        self.name(other.name || self.name());
        self.description(other.description || self.description());
    };
    map(data || { });

};

var project_list_viewmodel = function(projects) {
    var self = this;
    self.project = new project();
    self.create = function() {
        var request = $.ajax("/api/projects", {
            data: ko.toJSON(self.project),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function(data) {
                    // push a new project
                    data.url = request.getResponseHeader("Location");
                    self.projects.push(new project(data));
                    $('#project-modal').modal('hide');
                    self.project.clear();
                }
            }
        });
    };
    self.update = function() {
        var project = this;
        if (confirm('You are about to update ' + project.name() + '.')) {
            $.ajax(project.url(), {
                data: ko.toJSON(project),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function() {
                        project.editing(false);
                    }
                }
            });
        }
    };
    self.remove = function() {
        var project = this;
        if (confirm('You are about to delete ' + project.name() + '.')) {
            $.ajax("/api/projects/" + project.id(), {
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

    /* projects */
    self.projects = ko.observableArray($.map(projects, function (item) { return new project(item); }));
};