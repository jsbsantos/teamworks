/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />
/// <reference path="~/Content/js/app/teamworks.viewmodels.js" />

(function () {
    'use strict';

    $('#viewmodel-projects').length && ko.applyBindings(
        new TW.viewmodels.Projects(),
        document.getElementById('viewmodel-projects'));
})();