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
        if (data.discussions) {
            self.discussions(
                $.map(data.discussions, function(item) {
                    return new TW.viewmodels.models.Discussion(item);
                }));
        }

        if (data.activities) {
            self.activities(
                $.map(data.activities, function(item) {
                    return new TW.viewmodels.models.Activity(item);
                }));
        }
    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.description = ko.observable();

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

        if (data.messages) {
            self.messages(
                $.map(data.messages, function (item) {
                    return new TW.viewmodels.models.Message(item);
                }));
        }
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

TW.viewmodels.models.Message = function (data) {
    var self = this;
    self.load = function (data) {
        self.id(data.id);
        self.content(data.content);
    };

    self.id = ko.observable();
    self.content = ko.observable();
    self.clear = function () {
        self.load({});
    };
    self.load(data || {});
};

TW.viewmodels.models.Activity = function(data) {
    var self = this;
    self.load = function(data) {
        self.id(data.id);
        self.name(data.name);
        self.description(data.description);
    };

    self.id = ko.observable();
    self.name = ko.observable();
    self.description = ko.observable();

    self.clear = function() {
        self.load({ });
    };
    self.load(data || { });
};