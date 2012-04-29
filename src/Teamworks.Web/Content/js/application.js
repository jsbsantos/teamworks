(function () {
    'use strict';
    var ENTER_KEY = 13;
    var ESCAPE_KEY = 27;

    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    var project = function (data) {
        var self = this;
        self.id = ko.observable(data.id);
        self.name = ko.observable(data.name);
        self.description = ko.observable(data.description);
        self.url = ko.computed(function() {
            return "/projects/view" + self.id();
        });
    };
    
    var projects_viewmodel = function () {
        var self = this;
        self.projects = ko.observableArray([]);
        //load initial state
        $.getJSON("/api/projects", function (data) {
            self.projects($.map(data, function (item) {
                return new project(item);
            }));
        });
    };

    ko.applyBindings(new projects_viewmodel());
})();