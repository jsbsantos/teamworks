/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />
/// <reference path="~/Content/js/application.viewmodels.js" />

(function() {
    'use strict';
    var ENTER_KEY = 13;
    var ESCAPE_KEY = 27;

    if (!String.prototype.trim) {
        String.prototype.trim = function() {
            return this.replace( /^\s+|\s+$/g , '');
        };
    }

    $('#tasks').length && ko.applyBindings(new TasksViewmodel(tasks));
    $('#projects').length && ko.applyBindings(new ProjectsViewmodel());
})();