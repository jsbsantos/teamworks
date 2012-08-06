$(function () {
    'use strict';
    if (typeof gantt !== 'undefined')
        tw.graphics.gantt = gantt();
    
    if (typeof viewmodel !== 'undefined') {
        tw.page.viewmodel = viewmodel();
        /* apply bindings only if tw.page is setted */
        !$.isEmptyObject(tw.page) && ko.applyBindings(tw.page);
    }
    tw.page.ready(true);
});