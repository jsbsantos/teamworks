/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />
/// <reference path="~/Content/js/application.viewmodels.project.list.js" />

(function () {
    'use strict';
    var ENTER_KEY = 13;
    var ESCAPE_KEY = 27;

    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    $('#project_list').length && ko.applyBindings(new project_list_viewmodel(data), document.getElementById('project_list'));
    $('#project').length && ko.applyBindings(new project_viewmodel(data), document.getElementById('projects'));
})();