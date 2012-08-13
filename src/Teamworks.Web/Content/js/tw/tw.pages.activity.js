(function (pages) {
    pages.ActivityViewModel = function (endpoint, json) {
        var self = ko.mapping.fromJS(json, {});

        self.completionPercent = ko.computed(function () {
            return ((self.totalTimeLogged() / self.duration()) * 100).toPrecision(3);
        });
        
        return self;
    };
} (tw.pages));