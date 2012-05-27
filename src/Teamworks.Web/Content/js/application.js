/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />
/// <reference path="~/Content/js/application.viewmodels.js" />
/// <reference path="~/Content/js/application.viewmodels.tasks.js" />
/// <reference path="~/Content/js/application.viewmodels.projects.js" />
/// <reference path="~/Content/js/application.viewmodels.timelog.js" />
/// <reference path="~/Content/js/application.viewmodels.discussions.js" />

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
    
    //$('#task_list').length && ko.applyBindings(new task_list_viewmodel(data), document.getElementById('task_list'));
    $('#task').length && ko.applyBindings(new task_viewmodel(data), document.getElementById('task'));

    //$('#discussion_list').length && ko.applyBindings(new discussion_list_viewmodel(data), document.getElementById('discussion_list'));
    $('#discussion').length && ko.applyBindings(new discussion_viewmodel(data), document.getElementById('discussion'));

})();