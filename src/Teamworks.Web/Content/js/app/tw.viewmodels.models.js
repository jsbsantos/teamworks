/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />

var TW = TW || { };
TW.viewmodels = TW.viewmodels || { };
TW.viewmodels.models = TW.viewmodels.models || { };

TW.viewmodels.models.Project = function(data) {
    var self = this;
    self.load = function(data) {
        self.id(data.id);
        self.name(data.name);
        self.description(data.description);
        self.token(data.token);

        self.people($.map(data.people || { }, function(item) {
            return new TW.viewmodels.models.Person(item);
        }));

        self.discussions($.map(data.discussions || { }, function(item) {
            return new TW.viewmodels.models.Discussion(item);
        }));

        self.activities($.map(data.activities || { }, function(item) {
            return new TW.viewmodels.models.Activity(item);
        }));
    };

    self.id = ko.observable();
    self.name = ko.observable().extend({ min_length: 6 });
    self.description = ko.observable();
    self.token = ko.observable();

    self.people = ko.observableArray([]);
    self.activities = ko.observableArray([]);
    self.discussions = ko.observableArray([]);
    self.clear = function() {
        self.load({ });
    };
    self.load(data || { });
};

TW.viewmodels.models.Discussion = function(data) {
    var self = this;
    self.load = function(data) {
        self.id(data.id);
        self.name(data.name);
        self.content(data.content);

        self.messages($.map(data.messages || { }, function(item) {
            return new TW.viewmodels.models.Message(item);
        }));

    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.content = ko.observable();
    self.messages = ko.observableArray([]);
    self.clear = function() {
        self.load({ });
    };
    self.load(data || { });
};

TW.viewmodels.models.Message = function(data) {
    var self = this;
    self.load = function(data) {
        self.id(data.id);
        self.date(data.date);
        self.content(data.content);
        self.person.load(data.person || { });
    };

    self.id = ko.observable();
    self.content = ko.observable();
    self.date = ko.observable();
    self.person = new TW.viewmodels.models.Person();

    self.clear = function() {
        self.load({ });
    };
    self.load(data || { });
};

TW.viewmodels.models.Activity = function(data) {
    var self = this;
    self.load = function(data) {
        self.id(data.id);
        self.name(data.name);
        self.description(data.description);
        self.token(data.token);

        self.timelogs($.map(data.timelogs || { }, function(item) {
            return new TW.viewmodels.models.Timelog(item);
        }));
    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.description = ko.observable();
    self.token = ko.observable();
    self.timelogs = ko.observableArray([]);

    self.clear = function() {
        self.load({ });
    };
    self.load(data || { });
};

TW.viewmodels.models.Person = function(data) {
    var self = this;
    self.load = function(data) {
        self.id(data.id);
        self.name(data.name);
        self.username(data.username);
        self.email(data.email);
    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.username = ko.observable();
    self.email = ko.observable();
    self.clear = function() {
        self.load({ });
    };


    self.gravatar = {
        small: ko.computed(function() {
            return TW.helpers.gravatar(self.email(), 32);
        }),
        medium: ko.computed(function() {
            return TW.helpers.gravatar(self.email(), 64);
        })
    };

    self.load(data || { });
};

TW.viewmodels.models.Timelog = function(data) {
    var now = new Date().format('dd/mm/yyyy');

    var self = this;
    self.load = function(data) {
        self.id(data.id);
        self.description(data.description);
        self.duration(data.duration);
        data.date ?
            self.date(data.date)
            : self.date.formatted(now);
    };

    self.id = ko.observable();
    self.description = ko.observable();
    self.duration = ko.observable();
    self.date = ko.observable().extend({
        isoDate: ''
    });

    self.clear = function() {
        self.load({ });
    };
    self.load(data || { });
};