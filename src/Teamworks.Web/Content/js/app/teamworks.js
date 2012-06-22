/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />
/// <reference path="~/Content/js/app/teamworks.viewmodels.js" />

(function () {
    'use strict';
    if (typeof page !== 'undefined') {
        TW.app.page = page();
        /* apply bindings only if TW.app is setted */
        !$.isEmptyObject(TW.app) && ko.applyBindings(TW.app);
    }
})();