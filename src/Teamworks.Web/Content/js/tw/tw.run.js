$(function () {
    'use strict';

    var vm = { };
    if (typeof window.viewmodel !== 'undefined') {
        vm = window.viewmodel();
    }

    tw.utils.applyBindings(vm);
    if (typeof gantt !== 'undefined')
        tw.graphics.gantt = gantt();
});

