/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />
/// <reference path="~/Content/js/app/teamworks.viewmodels.js" />

(function () {
    'use strict';
    if (typeof viewmodel !== 'undefined') {
        TW.app.viewmodel = viewmodel();
        /* apply bindings only if TW.app is setted */
        !$.isEmptyObject(TW.app) && ko.applyBindings(TW.app);
    }
    TW.app.ready(true);

    $('body').tooltip({
       placement: 'bottom',
       selector: '[rel="tooltip-bottom"]'
    });
})();

