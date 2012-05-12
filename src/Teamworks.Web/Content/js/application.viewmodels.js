var Task = function(data) {
    var self = this;
    self.id = ko.observable("0");
    self.name = ko.observable();
    self.project = ko.observable();
    self.description = ko.observable();
    self.url = ko.computed(function() {
        return "/projects/" + self.project() + "/tasks/" + self.id();
    });
    var map = function(other) {
        self.id(other.id || self.id());
        self.name(other.name || self.name());
        self.description(other.description || self.description());
    };
    map(data || { });
};

var Project = function(data) {
    /* self region */
    var self = this;
    self.id = ko.observable("0");
    self.name = ko.observable();
    self.description = ko.observable();
    self.editing = ko.observable();
    self.url = ko.computed(function() {
        return "/projects/view/" + self.id();
    });
    self.tasks = ko.observableArray();
    var map = function(other) {
        self.id(other.id || self.id());
        self.name(other.name || self.name());
        self.description(other.description || self.description());
        self.tasks($.map(other.tasks || self.tasks() || [], function(data) {
            return new Task(data);
        }));
    };
    map(data || { });

    /* interactions */
    var old;
    self.edit = function() {
        if (!self.editing()) {
            self.editing(true);
            old = ko.toJSON(self);
        }
    };
    self.cancel = function() {
        map(old);
    };
};