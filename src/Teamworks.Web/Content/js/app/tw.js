var TW = TW || {};
TW.viewmodels = TW.viewmodels || {};

$(function () {
    'use strict';
    /* app */
    TW.page = {
        ready: ko.observable(false),
        alerts: ko.observableArray([])
    };
    TW.page.alerts._remove = function (item) {
        TW.page.alerts.remove(item);
    };
    /* helpers */
    TW.helpers = {
        md5: CryptoJS.MD5,
        copy: function (text) {
            return function () {
                window.prompt("ctrl+c, Enter", text);
            };
        },
        bad_format: function (obj, data) {
            var d = JSON.parse(data);
            for (var attr in d) {
                if (obj[attr] && obj[attr].validation_message) {
                    obj[attr].validation_message(d[attr]);
                }
            }
        },
        gravatar: function (value, size) {
            return '//www.gravatar.com/avatar/' + TW.helpers.md5((value || "").trim()) + '?s=' + size + '&d=mm&r=g';
        }
    };

    $(function () {
        $('body').on('focus.datepicker.data-api', '[data-provide="datepicker"]', function (e) {
            var $this = $(this);
            if ($this.data('datepicker')) return;
            e.preventDefault();
            $this.datepicker($this.data());
        });
    });

    if (typeof viewmodel !== 'undefined') {
        TW.page.viewmodel = viewmodel();
        /* apply bindings only if TW.app is setted */
        !$.isEmptyObject(TW.page) && ko.applyBindings(TW.page);

        if (TW.page.viewmodel.Gantt)
            viewmodel().Gantt();
    }
    TW.page.ready(true);
});