var Project = function (data) {
    var self = this;
    data = data || { };

    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.description = ko.observable(data.description);

    self.clear = function () {
        self.id("");
        self.name("");
        self.description("");
    };

    self.url = ko.computed(function () {
        return "/projects/" + self.id();
    });
};

var Task = function (data) {
    var self = this;
    data = data || {};
    
    self.id = ko.observable(data.id);
    self.name = ko.observable(data.name);
    self.project = ko.observable(data.project);
    self.description = ko.observable(data.description);

    self.clear = function () {
        self.id("");
        self.name("");
        self.project("");
        self.description("");
    };

    self.url = ko.computed(function () {
        return "/projects/" + self.project() + "/tasks/" + self.id();
    }, this);
};

var Timelog = function (data, taskid, projectid) {
    var self = this;
    data = data || {};
    
    self.id = ko.observable(data.id);
    self.description = ko.observable(data.description);
    self.date = ko.observable(data.date);
    self.duration = ko.observable(data.duration);
    self.person = ko.observable(data.person);
    self.url = ko.computed(function () {
        return "/" + projectid + "/tasks/" + taskid + "/timelog/" + self.id();
    }, this);
};

