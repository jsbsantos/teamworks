(function () {
    (function (String, undefined) {
        if (!String.prototype.trim) {
            String.prototype.trim = function () {
                return this.replace(/^\s+|\s+$/g, '');
            };
        }
    } (String));

    /**
    * Date.parse with progressive enhancement for ISO 8601 <https://github.com/csnover/js-iso8601>
    * © 2011 Colin Snover <http://zetafleet.com>
    * Released under MIT license.
    *
    * ---
    *
    * Edited to change method name to parseISOString
    */
    (function (Date, undefined) {
        var origParse = Date.parse, numericKeys = [1, 4, 5, 6, 7, 10, 11];
        Date.parseISOString = function (date) {
            var timestamp, struct, minutesOffset = 0;

            // ES5 §15.9.4.2 states that the string should attempt to be parsed as a Date Time String Format string
            // before falling back to any implementation-specific date parsing, so that’s what we do, even if native
            // implementations could be faster
            //              1 YYYY                2 MM       3 DD           4 HH    5 mm       6 ss        7 msec        8 Z 9 ±    10 tzHH    11 tzmm
            if ((struct = /^(\d{4}|[+\-]\d{6})(?:-(\d{2})(?:-(\d{2}))?)?(?:T(\d{2}):(\d{2})(?::(\d{2})(?:\.(\d{3}))?)?(?:(Z)|([+\-])(\d{2})(?::(\d{2}))?)?)?$/.exec(date))) {
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

        Date.prototype.toStringLocal = function (format) {
            var local = new Date(this);
            local.setHours(local.getHours() - local.getTimezoneOffset() / 60);
            return local.toString(format);
        };
    } (Date));

    $.ajaxSetup({
        type: 'get',
        contentType: 'application/json; charset=utf-8',
        cache: false
    });
} ());

var tw = {
    pages: { },
    utils: { },
    bindings: { },
    graphics: { }
};

(function(obj) {
    'use strict';

    obj.ready = ko.observable(false);
    obj.bindings.alerts = ko.observableArray([]);
    obj.bindings.alerts._remove = function(item) {
        tw.bindings.alerts.remove(item);
    };

    var raw = window.location.href;
    var i = raw.length - 1;
    if (raw.charAt(i) == '/') {
        raw = raw.substring(0, i);
    }
    raw = raw.replace("#", "");
    obj.utils.location = raw;
    obj.utils.applyBindings = function(model) {
        tw.bindings.vm = model;
        ko.applyBindings(model);
        tw.ready(true);
    };

    obj.utils.remove = function(collection, endpoint, message, errorCallback) {
        return function() {
            var _obj = this;
            if (confirm(message)) {
                $.ajax({
                    type: 'post',
                    url: endpoint
                }).success(function(data) {
                    collection.mappedRemove(_obj);
                }).error(errorCallback);
            }
        };
    };
    obj.utils.now = function() {
        var x = Date.today();
        x.setHours(Math.abs(x.getTimezoneOffset()) / 60);
        return x;
    };
    $(function() {
        $('body').on('focus.datepicker.data-api', '[data-provide="datepicker"]', function(e) {
            var $this = $(this);
            if ($this.data('datepicker')) return;
            e.preventDefault();
            $this.datepicker($this.data());
        });
    });
}(tw));

// knockout extensions
(function() {
    /* validation */
    tw.extenders = { };
    tw.extenders.validation = function(target, fn) {
        var changeMessage = function(msg) {
            target.has_error(msg.length ? true : false);
        };

        target.has_error = ko.observable();
        target.validation_message = ko.observable();

        target.subscribe(fn);
        target.validation_message.subscribe(changeMessage);
        fn(target());
        return target;
    };
    /* extenders */
    ko.extenders.required = function(target, message) {
        var fn = function(value) {
            var valid = value && value.length > 0;
            target.validation_message(valid ? "" : message);
        };

        return tw.extenders.validation(target, fn);
    };
    ko.extenders.isoDate = function(target, pattern) {
        target.formatted = ko.computed({
            read: function() {
                if (!target()) {
                    return;
                }
                var dt = new Date(Date.parseISOString(target()));
                return dt.toString(pattern);
            },
            write: function(value) {
                if (value) {
                    var dt = Date.parseExact(value, pattern);
                    if (dt)
                        target(new Date(dt).toISOString());
                    else {
                        dt = new Date(value);
                        target(dt.toISOString());
                    }
                }
            }
        });
        return target;
    };
    ko.extenders.duration = function(target, message) {
        target.duration = ko.computed({
            read: function() {
                var val = parseInt(target());
                if (isNaN(val) || val == 0)
                    return message;
                return juration.stringify(val, { format: 'long' });
            },
            write: function(value) {
                if (value) {
                    try {
                        if (typeof (value)=='string') {
                            if (value.indexOf('h') == -1 && value.indexOf('m') == -1 )
                                value = value.trim() + "h";
                            target(juration.parse(value));
                        } else 
                            target(value);
                    } catch(e) {
                        target(message);
                    }
                }
            }
        });
        target.duration(target());
        return target;
    };
    /* binders */
    ko.bindingHandlers.highlight = {
        init: function(element) {
            var $elem = $(element);
            $elem.addClass('');
        },
        update: function(element, valueAccessor, allBindingsAccessor) {
            var $elem = $(element);
            var all = allBindingsAccessor();
            var value = ko.utils.unwrapObservable(valueAccessor());
            var name = value || 'highlight';
            $elem.removeClass('out');
            $elem.addClass(name);
            var duration = all.duration || 250;

            setTimeout(function() {
                $elem.addClass('out');
                $elem.removeClass(name);
            }, duration);
        }
    };
    ko.bindingHandlers.datepicker = {
        init: function(element, valueAccessor) {
            var elem = $(element);
            var value = valueAccessor();
            var datepicker = elem.datepicker(elem.data());
            datepicker.on('changeDate', function(e) {
                //value(e.date.toString(datepicker.data().dateFormat));
                value(e.date.toStringLocal());
            });
        }
    };
    ko.bindingHandlers.timeago = {
        update: function(element) {
            var $elem = $(element);
            $elem.timeago();
        }
    };
    ko.bindingHandlers.typeahead = {
        init: function(element, valueAccessor) {
            var isFunction = function(fn) {
                var getType = { };
                return fn && getType.toString.call(fn) == '[object Function]';
            };

            var $elem = $(element);
            if ($elem.data('typeahead')) return;

            var data = $elem.data();
            $elem.typeahead(data);
            var typeahead = $elem.data('typeahead');

            var value = ko.utils.unwrapObservable(valueAccessor());
            if (isFunction(value)) {
                value(typeahead);
            }
        }
    };

}());