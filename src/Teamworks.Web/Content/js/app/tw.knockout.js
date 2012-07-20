/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />

(function () {
    var validate = function (target, fn) {
        var change_message = function (msg) {
            target.has_error(msg.length ? true : false);
        };

        target.has_error = ko.observable();
        target.validation_message = ko.observableArray([]);

        fn(target());
        target.subscribe(fn);
        target.validation_message.subscribe(change_message);
        target.validation_message([]);
        return target;
    };

    ko.extenders.min_length = function (target, min) {
        var fn = function (value) {
            var valid = value && value.length > min;
            target.validation_message(valid ?[] : [""+ min]);
        };
        return validate(target, fn);
    };

    ko.extenders.isoDate = function (target, formatString) {
        target.formatted = ko.computed({
            read: function () {
                if (!target()) {
                    return;
                }
                var dt = new Date(Date.parse(target()));
                return dt;
            },
            write: function (value) {
                if (value) {
                    target(new Date(Date.parse(value)).toISOString());
                }
            }
        });
        target.formatted(target());
        return target;
    };
} ());

var TW = TW || { };

/* app */
TW.app = {
    ready: ko.observable(false),
    alerts: ko.observableArray([])
};
TW.app.alerts._remove = function(item) {
    TW.app.alerts.remove(item);
};

/* helpers */
TW.helpers = {
    md5: CryptoJS.MD5,
    copy: function(text) {
        return function() {
            window.prompt("ctrl+c, Enter", text);
        };
    },
    bad_format: function(obj, data) {
        var d = JSON.parse(data);
        for (var attr in d) {
            if (obj[attr] && obj[attr].validation_message) {
                obj[attr].validation_message(d[attr]);
            }
        }
    }
};

$(function() {
    'use strict';
    if (typeof viewmodel !== 'undefined') {
        TW.app.viewmodel = viewmodel();
        /* apply bindings only if TW.app is setted */
        !$.isEmptyObject(TW.app) && ko.applyBindings(TW.app);
    }
    TW.app.ready(true);

    /*
    /* TODO remove this please =) */
    $('body').tooltip({
        placement: 'bottom',
        selector: '[rel="tooltip-bottom"]'
    });
});