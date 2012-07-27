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
    * Edited to change parse to parseISOString
    */
    (function(Date, undefined) {
        var orig_parse = Date.parse, numeric_keys = [1, 4, 5, 6, 7, 10, 11];
        Date.parseISOString = function(date) {
            var timestamp, struct, minutes_offset = 0;

            // ES5 §15.9.4.2 states that the string should attempt to be parsed as a Date Time String Format string
            // before falling back to any implementation-specific date parsing, so that’s what we do, even if native
            // implementations could be faster
            //              1 YYYY                2 MM       3 DD           4 HH    5 mm       6 ss        7 msec        8 Z 9 ±    10 tzHH    11 tzmm
            if ((struct = /^(\d{4}|[+\-]\d{6})(?:-(\d{2})(?:-(\d{2}))?)?(?:T(\d{2}):(\d{2})(?::(\d{2})(?:\.(\d{3}))?)?(?:(Z)|([+\-])(\d{2})(?::(\d{2}))?)?)?$/ .exec(date))) {
                // avoid NaN timestamps caused by “undefined” values being passed to Date.UTC
                for (var i = 0, k; (k = numeric_keys[i]); ++i) {
                    struct[k] = +struct[k] || 0;
                }

                // allow undefined days and months
                struct[2] = (+struct[2] || 1) - 1;
                struct[3] = +struct[3] || 1;

                if (struct[8] !== 'Z' && struct[9] !== undefined) {
                    minutes_offset = struct[10] * 60 + struct[11];

                    if (struct[9] === '+') {
                        minutes_offset = 0 - minutes_offset;
                    }
                }

                timestamp = Date.UTC(struct[1], struct[2], struct[3], struct[4], struct[5] + minutes_offset, struct[6], struct[7]);
            } else {
                timestamp = orig_parse ? orig_parse(date) : NaN;
            }

            return timestamp;
        };
    }(Date));

    $.ajaxSetup({
        type: 'get',
        contentType: 'application/json; charset=utf-8',
        cache: false
    });

}());