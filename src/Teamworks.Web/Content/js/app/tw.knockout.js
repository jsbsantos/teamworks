/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />

(function () {
    /* validation */
    tw.extenders = { };
    tw.extenders.validation = function (target, fn) {
        var changeMessage = function (msg) {
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
    ko.extenders.required = function (target, message) {
        var fn = function (value) {
            var valid = value && value.length > 0;
            target.validation_message(valid ? "" : message);
        };

        return tw.extenders.validation(target, fn);
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
            var isFunction = function (fn) {
                var getType = {};
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

} ());