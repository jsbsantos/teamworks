$(function () {
    'use strict';
    if (typeof viewmodel !== 'undefined') {
        tw.page.viewmodel = viewmodel();
        /* apply bindings only if tw.page is setted */
        !$.isEmptyObject(tw.page) && ko.applyBindings(tw.page);

        if (tw.page.viewmodel.Gantt)
            viewmodel().Gantt();
    }
    tw.page.ready(true);
});