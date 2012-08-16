(function (pages) {
    pages.GanttViewmodel = function (d) {
        var self = this;
        d = d || {};

        self.description = ko.observable("");
        self.duration = ko.observable("");
        self.name = ko.observable("");
        self.startDate = ko.observable("").extend({
            isoDate: 'dd/MM/yyyy'
        }); ;
        self.timeUsed = ko.observable("");

        self.map = function (data) {
            self.description(data.Description || "N/A");
            self.duration(data.Duration || "N/A");
            self.name(data.Name || "");
            self.startDate(data.StartDate || "N/A");
            self.timeUsed(data.TimeUsed || "0");
        };

        //        var mapping = {
        //            'startDate': {
        //                create: function (options) {
        //                    options.observable.extend({
        //                        isoDate: 'dd/MM/yyyy'
        //                    });
        //                    return options.data;
        //                }
        //            }
        //        };

        self.map(d);
        return self;
    };
} (tw.pages));