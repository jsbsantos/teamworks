(function() {
    (function(String, undefined) {
        if (!String.prototype.trim) {
            String.prototype.trim = function() {
                return this.replace( /^\s+|\s+$/g , '');
            };
        }
    }(String));

    /**
    * Date.parse with progressive enhancement for ISO 8601 <https://github.com/csnover/js-iso8601>
    * © 2011 Colin Snover <http://zetafleet.com>
    * Released under MIT license.
    *
    * ---
    *
    * Edited to change method name to parseISOString
    */
    (function(Date, undefined) {
        var origParse = Date.parse, numericKeys = [1, 4, 5, 6, 7, 10, 11];
        Date.parseISOString = function(date) {
            var timestamp, struct, minutesOffset = 0;

            // ES5 §15.9.4.2 states that the string should attempt to be parsed as a Date Time String Format string
            // before falling back to any implementation-specific date parsing, so that’s what we do, even if native
            // implementations could be faster
            //              1 YYYY                2 MM       3 DD           4 HH    5 mm       6 ss        7 msec        8 Z 9 ±    10 tzHH    11 tzmm
            if ((struct = /^(\d{4}|[+\-]\d{6})(?:-(\d{2})(?:-(\d{2}))?)?(?:T(\d{2}):(\d{2})(?::(\d{2})(?:\.(\d{3}))?)?(?:(Z)|([+\-])(\d{2})(?::(\d{2}))?)?)?$/ .exec(date))) {
                // avoid NaN timestamps caused by “undefined” values being passed to Date.UTC
                for (var i = 0, k; (k = numericKeys[i]); ++i) {
                    struct[k] = +struct[k] || 0;
                }

                // allow undefined days and months
                struct[2] = (+struct[2] || 1) - 1;
                struct[3] = +struct[3] || 1;

                if (struct[8] !== 'Z' && struct[9] !== undefined) {
                    minutesOffset = struct[10] * 60 + struct[11];

                    if (struct[9] === '+') {
                        minutesOffset = 0 - minutesOffset;
                    }
                }

                timestamp = Date.UTC(struct[1], struct[2], struct[3], struct[4], struct[5] + minutesOffset, struct[6], struct[7]);
            } else {
                timestamp = origParse ? origParse(date) : NaN;
            }

            return timestamp;
        };
    }(Date));

    $.ajaxSetup({
        type: 'get',
        contentType: 'application/json; charset=utf-8',
        cache: false
    });
} ());

var tw = tw || {};
tw.pages = tw.pages || {};
tw.mappings = tw.mappings || {};
tw.viewmodels = tw.viewmodels || {};
tw.graphics = tw.graphics || {};

(function (obj) {
    'use strict';
    /* page */
    obj.page = {
        ready: ko.observable(false),
        alerts: ko.observableArray([])
    };
    obj.page.alerts._remove = function (item) {
        tw.page.alerts.remove(item);
    };
    
    $(function () {
        $('body').on('focus.datepicker.data-api', '[data-provide="datepicker"]', function (e) {
            var $this = $(this);
            if ($this.data('datepicker')) return;
            e.preventDefault();
            $this.datepicker($this.data());
        });
    });
} (tw));