$(function () {
    'use strict';

    if (typeof viewmodel !== 'undefined') {
        tw.page.viewmodel = viewmodel();
        /* apply bindings only if tw.page is setted */
        !$.isEmptyObject(tw.page) && ko.applyBindings(tw.page);
    }
    tw.page.ready(true);


    if (typeof gantt !== 'undefined')
        tw.graphics.gantt = gantt();
});

