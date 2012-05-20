/// <reference path="~/Content/js/application.viewmodels.task.list.js" />

var project_viewmodel = function (project) {
    var self = this;
    self.tasks = new task_list_viewmodel(project.id, project.tasks);
    /* projects */
};