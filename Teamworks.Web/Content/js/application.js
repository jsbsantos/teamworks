;
ko.bindingHandlers.submitFormOnEnter = {
    init: function (element) {
        $(element).keyup(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                $(this.form).trigger('submit');
                return false;
            }
            return true;
        });
    }
};

var models = {};
var Project = function (data) {
    var self = this;

    data = data || {};
    self.url = ko.observable(data.url || "");
    self.name = ko.observable(data.name || "");
    self.description = ko.observable(data.description || "");
    self.editing_name = ko.observable();
    self.editing_description = ko.observable();
    self.start_editing_name = function () {
        self.editing_name(true);
    };
    self.start_editing_description = function () {
        self.editing_description(true);
    };
};


models.Projects = (function () {
    var self = this;
    var validate = function () {
        var n = self.project.name().length;
        var d = self.project.description().length;
        self.is_valid(n ? d : !d);
    };
    self.project = new Project(),
    self.project.name.subscribe(validate);
    self.project.description.subscribe(validate);
    self.projects = ko.observableArray($.map(data, function (e) {
        return new Project(e);
    }));
    self.is_valid = ko.observable(true);
    self.clear = function () {
        self.project.name("");
        self.project.description("");
    };
    self.new_project = function () {
        if (!is_valid) return;
        var request = $.ajax("/api/projects", {
            data: ko.toJSON(self.project),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    // push a new project
                    data.url = "http://" + request.getResponseHeader("Location");
                    self.projects.push(new Project(data));
                    // clean the project
                    self.clear();
                }
            }
        });
    };
    self.save = function () {
        var project = this;
        $.ajax(project.url(), {
            data: ko.toJSON(project),
            type: 'put',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                204: /*no content*/function () {
                    alert('changes saved');
                    project.editing_name(false);
                    project.editing_description(false);
                }
            }
        });
    };
    self.remove = function () {
        var project = this;
        $.ajax(project.url(), {
            type: 'delete',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                204: /*no content*/function () {
                    alert('project deleted');
                    self.projects.destroy(project);
                }
            }
        });
    };

    ko.applyBindings(self);
})(data || []);
