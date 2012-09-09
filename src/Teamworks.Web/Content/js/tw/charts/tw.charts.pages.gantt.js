(function (pages) {
    pages.GanttViewmodel = function (d) {
        var self = this;
        d = d || {};

        var mapping = {
            'startDate': {
                create: function (o) {
                    return ko.observable(o.data).extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                }
            },
            'endDate': {
                create: function (o) {
                    return ko.observable(o.data).extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                }
            },
            'totalEstimatedTime': {
                create: function (o) {
                    return ko.observable(o.data).extend({ duration: "0" });
                }
            },
            'totalTimeLogged': {
                create: function (o) {
                    return ko.observable(o.data).extend({ duration: "0" });
                }
            },
            'activitySummary': {
                create: function (options) {
                    return (new (function () {
                        var tthis = this;
                        tthis._duration = ko.observable(options.data.duration * 3600).extend({ duration: "0" });
                        tthis._logged = ko.observable(options.data.timeUsed * 3600).extend({ duration: "0" });
                        tthis._progress = ko.computed(function () {
                            return ((tthis._logged() / tthis._duration()) * 100).toPrecision(3) + '%';
                        });
                        tthis.exceededByDependecy = ko.computed(function () {
                            var dt = new Date(options.data.startDate);
                            var val = Math.floor(Math.max(options.data.duration, options.data.timeUsed) / 8);
                            dt.setDate(dt.getDate() + val);
                            return new Date(options.data.endDate) > dt;
                        });
                        tthis.exceededEstimate = ko.computed(function () {
                            return tthis._logged() > tthis._duration();
                        });

                        var m = {
                            'startDate': {
                                create: function (o) {
                                    return ko.observable(o.data).extend({
                                        isoDate: 'dd/MM/yyyy'
                                    });
                                }
                            },
                            'endDate': {
                                create: function (o) {
                                    return ko.observable(o.data).extend({
                                        isoDate: 'dd/MM/yyyy'
                                    });
                                }
                            }
                        };
                        ko.mapping.fromJS(options.data, m, this);
                    })());
                }
            }
        };
        self = ko.mapping.fromJS(d || [], mapping);


        self.progress = ko.computed(function () {
            return ((self.totalTimeLogged() / self.totalEstimatedTime()) * 100).toPrecision(3) + '%';
        });

        /***************************************
        *                Tooltip               *
        ****************************************/
        self.tooltip = {};
        self.tooltip.description = ko.observable("");
        self.tooltip.duration = ko.observable().extend({ duration: "0" });
        self.tooltip.name = ko.observable("");
        self.tooltip.startDate = ko.observable("").extend({
            isoDate: 'dd/MM/yyyy'
        });
        ;
        self.tooltip.timeUsed = ko.observable("").extend({ duration: "0" }); ;

        self.tooltip.map = function (data) {
            self.tooltip.description(data.Description || "N/A");
            self.tooltip.duration(data.Duration * 3600);
            self.tooltip.name(data.Name);
            self.tooltip.startDate(data.StartDate || "N/A");
            self.tooltip.timeUsed(data.TimeUsed * 3600);
        };

        self.tooltip.map(d);


        /***************************************
        *                Timelog               *
        ****************************************/
        self.timelogs = new tw.pages.RegisterTimelogsViewModel({}, d.timelogs);
        return self;
    };
} (tw.pages));