/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />

(function () {
    /* validation */
    var validate = function (target, fn) {
        var change_message = function (msg) {
            target.has_error(msg.length ? true : false);
        };

        target.has_error = ko.observable();
        target.validation_message = ko.observableArray([]);

        target.subscribe(fn);
        target.validation_message.subscribe(change_message);
        fn(target());
        return target;
    };
    /* extenders */
    ko.extenders.required = function (target, message) {
        var fn = function (value) {
            var valid = value && value.length > 0;
            target.validation_message(valid ? [] : [message]);
        };
        return validate(target, fn);
    };

    ko.extenders.isoDate = function (target, pattern) {
        target.formatted = ko.computed({
            read: function () {
                if (!target()) {
                    return;
                }
                var dt = new Date(Date.parseISOString(target()));
                return dt.toString(pattern);
            },
            write: function (value) {
                if (value) {
                    target(new Date(Date.parseExact(value, pattern)).toISOString());
                }
            }
        });
        target.formatted(target());
        return target;
    };
    /* binders */
    ko.bindingHandlers.datepicker = {
        init: function (element, valueAccessor) {
            var elem = $(element);
            var value = valueAccessor();
            var datepicker = elem.datepicker(elem.data());
            datepicker.on('changeDate', function (e) {
                value(e.date.toString(datepicker.data().dateFormat));
            });
        }
    };
    ko.bindingHandlers.typeahead = {
        init: function (element, valueAccessor) {
            var g = 0;
            var $elem = $(element);
            if ($elem.data('typeahead')) return;

            var data = $elem.data();
            $elem.typeahead(data);
            var typeahead = $elem.data('typeahead');
            var value = ko.utils.unwrapObservable(valueAccessor());
            for (var prop in typeahead) {
                if (value[prop]) {
                    typeahead[prop] = value[prop];
                }
            }
            $elem.on('keyup', function () {
                var t = g = setTimeout(function () {
                    if (t !== g) return;

                    var query = typeahead.query;
                    if (!query) {
                        if (value.filter) {
                            typeahead.source = value.filter([]);
                        } else {
                            typeahead.source = [];
                        }
                        return;
                    };
                    $.get(value.endpoint, { q: query }, function (data) {
                        if (query !== typeahead.query) return;
                        if (value.filter) {
                            typeahead.source = value.filter(data);
                        } else {
                            typeahead.source = data;
                        }
                        typeahead.lookup();
                    });
                }, 200);
            });
        }
    };

} ());