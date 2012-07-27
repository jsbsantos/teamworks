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
            target.validation_message(valid ? [] : ["" + min]);
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
                    target(new Date(Date.parse(value, pattern)).toISOString());
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
    gravatar: function (value, size) {
        return '//www.gravatar.com/avatar/' + TW.helpers.md5((value || "").trim()) + '?s=' + size + '&d=mm&r=g';
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

$(function () {
    'use strict';
    if (typeof viewmodel !== 'undefined') {
        TW.app.viewmodel = viewmodel();
        /* apply bindings only if TW.app is setted */
        !$.isEmptyObject(TW.app) && ko.applyBindings(TW.app);
    }
    TW.app.ready(true);

    /* TODO remove this please =) */
    $('body').tooltip({
        placement: 'bottom',
        selector: '[rel="tooltip-bottom"]'
    });

    $('body').on('focus.typeahead.data-api', '[data-provide="people-typeahead"]', function (e) {
        var $this = $(this);
        if ($this.data('typeahead')) return;
        e.preventDefault();

        var obj, labels;
        $this.typeahead({
            source: function (query, process) {
                $.get('/api/people', { q: query }, function (data) {
                    process(data);
                });
            },
            sorter: function (items) {
                return items;
            },
            matcher: function (item) {
                return true;
            },
            updater: function (item) {
                return item.id;
            },
            menu: '<ul class="typeahead dropdown-menu"></ul>',
            item: '<li><a href="#awesome"></a></li>'
        });
        $this.data('typeahead').render = function (items) {
            var that = this;

            items = $(items).map(function (i, item) {
                i = $(that.options.item).attr('data-value', item);
                i.find('a').html(that.highlighter(item.name)).append('<img href="' + TW.helpers.gravatar(item.email, 16) + '"/>');
                return i[0];
            });

            items.first().addClass('active');
            this.$menu.html(items);
            return this;
        };
    });

    $(function () {
        $('body').on('focus.datepicker.data-api', '[data-provide="datepicker"]', function(e) {
            var $this = $(this);
            if ($this.data('datepicker')) return;
            e.preventDefault();
            $this.datepicker($this.data());
        });
    });
});