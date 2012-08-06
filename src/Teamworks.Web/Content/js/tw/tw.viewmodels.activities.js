/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />
/// <reference path="~/Content/js/libs/knockout.mapping.debug.js" />

(function (obj) {
    obj.Activity = function (endpoint) {
        var timelogs_endpoint = endpoint + '/timelogs/';

        var self = this;

        self.activity = ko.mapping.fromJS({});
        self.activity.timelogs = ko.mapping.fromJS([]);
        
        $.ajax(endpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function (data) {
                    ko.mapping.fromJS(data, self.activity);
                }
            }
        });

        $.ajax(timelogs_endpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function (data) {
                    ko.mapping.fromJS(data, self.activity.timelogs);
                }
            }
        });
    };
} (tw.viewmodels));