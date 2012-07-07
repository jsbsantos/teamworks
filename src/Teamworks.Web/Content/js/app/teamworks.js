/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />

var TW = TW || {};

(function () {
    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }
} ());

/// <see cref="https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/Date/toISOString" />
(function() {
    if (!Date.prototype.toISOString) {

        (function() {

            function pad(number) {
                var r = String(number);
                if (r.length === 1) {
                    r = '0' + r;
                }
                return r;
            }

            Date.prototype.toISOString = function() {
                return this.getUTCFullYear()
                    + '-' + pad(this.getUTCMonth() + 1)
                        + '-' + pad(this.getUTCDate())
                            + 'T' + pad(this.getUTCHours())
                                + ':' + pad(this.getUTCMinutes())
                                    + ':' + pad(this.getUTCSeconds())
                                        + '.' + String((this.getUTCMilliseconds() / 1000).toFixed(3)).slice(2, 5)
                                            + 'Z';
            };

        }());
    }
}());

(function () {
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

(function(tw) {
    tw.app = TW.app || {};

    tw.app.ready = ko.observable(false);
    tw.app.alerts = ko.observableArray([]);

    tw.helpers = TW.helpers || {};
    tw.helpers.md5 = CryptoJS.MD5;
    tw.helpers.count = (function () {
        var count = 0;
        return {
            curr: function () {
                return count;
            },
            next: function() {
                return ++count;
            }
        };
    }());
}(TW));



