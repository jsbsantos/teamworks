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

    $('#tasks').length && ko.applyBindings(new TasksViewmodel(projectid));
    $('#projects').length && ko.applyBindings(new ProjectsViewmodel());
})();