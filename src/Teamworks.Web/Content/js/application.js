var Project = function (data) {
    var old;
    var self = this;

    self.url = ko.observable();
    self.name = ko.observable();
    self.description = ko.observable();
    self.editing = ko.observable();
    self.start_editing = function () {
        old = $.parseJSON(ko.toJSON(self));
        self.editing(true);
    };
    self.stop_editing = function () {
        self.editing(false);
    };
    self.key_editing = function (project, event) {
        var keyCode = (event.which ? event.which : event.keyCode);
        if (keyCode === 27 && self.editing()) {
            map(old);
            self.editing(false);
            return false;
        }
    };
    var map = function (other) {
        self.url(other.url || self.url() || "");
        self.name(other.name || self.name() || "");
        self.description(other.description || self.description() || "");
    };
    map(data || {});
};


(function() {
    var self = this;
    var validate = function() {
        var n = self.project.name().length;
        var d = self.project.description().length;
        self.is_valid(n ? d : !d);
    };
    self.project = new Project(),
    self.project.name.subscribe(validate);
    self.project.description.subscribe(validate);
    self.projects = ko.observableArray($.map(data, function(d) {
        return new Project(d);
    }));
    self.is_valid = ko.observable(true);
    self.clear = function() {
        self.project.name("");
        self.project.description("");
    };
    self.new_project = function() {
        if (!is_valid) return;
        var request = $.ajax("/api/projects", {
            data: ko.toJSON(self.project),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function(data) {
                    // push a new project
                    data.url = "http://" + request.getResponseHeader("Location");
                    self.projects.push(new Project(data));
                    // clean the project
                    self.clear();
                }
            }
        });
    };
    self.save = function() {
        var project = this;
        $.ajax(project.url(), {
            data: ko.toJSON(project),
            type: 'put',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                204: /*no content*/function() {
                    alert('changes saved');
                    project.editing(false);
                }
            }
        });
    };
    self.remove = function() {
        var project = this;
        $.ajax(project.url(), {
            type: 'delete',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                204: /*no content*/function() {
                    alert('project deleted');
                    self.projects.destroy(project);
                }
            }
        });
    };

    ko.applyBindings(self);
})(data || []);