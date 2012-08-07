(function (pages) {
    pages.ActivityViewModel = function (endpoint, json) {
        var self = ko.mapping.fromJS(json, {});

        return self;
    };
} (tw.pages));