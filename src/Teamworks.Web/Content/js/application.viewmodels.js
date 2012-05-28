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
        return "/projects/" + self.id()  + "/view";
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
    
    self.id = ko.observable((data && data.id) || "0");
    self.description = ko.observable(data && data.description);
    self.date = ko.observable(data && data.date);
    self.duration = ko.observable(data && data.duration);
    self.person = ko.observable(data && data.person);
    
    self.url = ko.computed(function () {
        return "/" + projectid + "/tasks/" + taskid + "/timelog/" + self.id() + "/view";
    }, this);

    self.clear = function () {
        self.id("");
        self.description("");
        self.date("");
        self.duration("");
    };
};

var Discussion = function (data) {
    var self = this;
    data = data || {};
    
    self.id = ko.observable((data && data.id) || "0");
    self.name = ko.observable(data && data.name);
    self.date = ko.observable(data && data.date);
    self.text = ko.observable(data && data.text);
    self.person = ko.observable(data && data.person);
    self.project = ko.observable(data.project);
    
    self.url = ko.computed(function () {
        return "/" + self.project() + "/discussions/" + self.id() + "/view";
    }, this);

    self.clear = function () {
        self.id("");
        self.name("");
        self.date("");
        self.text("");
        self.person("");
    };
};

var Message = function (data, discussionid, projectid) {
    var self = this;
    data = data || {};

    self.id = ko.observable((data && data.id) || "0");
    self.text = ko.observable(data && data.text);
    self.date = ko.observable(data && data.date || new Date().toString());
    self.person = ko.observable(data && data.person);
    
    self.formatted_date = ko.computed(function () {
        return new Date(parseInt(self.date().substr(6)));
    }, this);
        
    self.url = ko.computed(function () {
        return "/projects/" + projectid + "/discussion/" + discussionid + "/message/" + self.id() + "/view";
    }, this);

    self.clear = function () {
        self.id("");
        self.text("");
        self.date("");
        self.person("");
    };
};


