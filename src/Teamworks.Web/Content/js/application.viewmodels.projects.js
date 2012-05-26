/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />
/// <reference path="~/Content/js/application.viewmodels.js" />
/// <reference path="~/Content/js/application.viewmodels.tasks.js" />

var project_viewmodel = function (project) {
    var self = this;
    self.tasks = new task_list_viewmodel(project.id, project.tasks);
    self.discussions = new discussions_list_viewmodel(project.id, project.discussions);
};

var project_list_viewmodel = function (projects) {
    var self = this;
    self.project = new Project();
    self.endpoint = "api/projects/";
    self.projects = ko.observableArray(
        $.map(projects, function (item) {
             return new Project(item);
         })
    );
    
    /* interact methods */
    self.create = function () {
        var request = $.ajax(self.endpoint, {
            data: ko.toJSON(self.project),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    // push a new project
                    data.url = request.getResponseHeader("Location");
                    self.projects.push(new Project(data));
                    $('#project-modal').modal('hide');
                    self.project.clear();
                }
            }
        });
    };
    self.update = function () {
        var project = this;
        if (confirm('You are about to update ' + project.name() + '.')) {
            $.ajax(project.url(), {
                data: ko.toJSON(project),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function () {
                        project.editing(false);
                    }
                }
            });
        }
    };
    self.remove = function () {
        var project = this;
        if (confirm('You are about to delete ' + project.name() + '.')) {
            $.ajax(self.endpoint + project.id(), {
                type: 'delete',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function () {
                        self.projects.destroy(project);
                    }
                }
            });
        }
    };
};