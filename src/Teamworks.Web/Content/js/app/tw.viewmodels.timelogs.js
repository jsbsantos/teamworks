/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />
/// <reference path="~/Content/js/libs/knockout.mapping.debug.js" />

(function (obj) {
    obj.Timelogs = function () {
        var self = this;
        
        self.tabs = [
            { description: "2 days ago", date: Date.today().add(-2).days().toISOString(), calendar: false },
            { description: "Yesterday", date: Date.today().add(-1).days().toISOString(), calendar: false },
            { description: "Today", date: Date.today().toISOString(), calendar: false },
            { description: "Calendar", date: Date.today().toISOString(), calendar: true}];

        self.selected = ko.observable(self.tabs[2]);
        self._select = function (item) {
            self.selected(item);
            self.timelog.date(item.date);
        };

        var now = new Date().toString('dd/MM/yyyy');

        self.timelog = {
            project_id: ko.observable(),
            activity_id: ko.observable(),
            description: ko.observable(),
            duration: ko.observable(),
            date: ko.observable(now).extend({
                isoDate: 'dd/MM/yyyy'
            })
        };
        self.timelog.create = function () {
            $.ajax('/api/projects/' + self.activity.project_id() + '/activities/' + self.activity.id() + '/timelogs/',
                {
                    type: 'post',
                    data: ko.toJSON(self.timelog),
                    statusCode: {
                        201: /*created*/function (data) {
                            self.activity.timelogs.push(ko.mapping.fromJS(data));
                            self.timelog.clear();
                        }
                    }
                }
            );
        };

        
    };
} (TW.viewmodels));