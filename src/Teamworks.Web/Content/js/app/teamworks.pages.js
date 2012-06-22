var TW = TW || { };
TW.pages = TW.pages || { };

TW.pages.Project = function () {
    TW.viewmodels.Discussion.prototype.editing = ko.observable(false);

    var self = this;
    self.project = new TW.viewmodels.Project();
    self.discussion = new TW.viewmodels.Discussion();
    self.create_discussion = function() {
        self.project.discussions.create(self.discussion, function () {
            self.discussion.clear();
            self.discussion.editing(false);
        });
    };
    self.load = function (identifier) {
        $.ajax(
            '/api/projects/' + identifier,
            {
                type: 'get',
                contentType: 'application/json; charset=utf-8',
                cache: 'false',
                statusCode: {
                    200: /*ok*/function (data) {
                        self.project.load(data);
                    },
                    404: /*not found*/function () {
                        TW.app.messages.push({ message: 'The project you requested doesn\'t exist.' });
                    }
                }
            });
        return self;
    };
    return self;
};

TW.pages.Projects = function () {
    TW.viewmodels.Project.prototype.editing = ko.observable(false);
    
    var self = this;
    self.projects = new TW.viewmodels.Projects();
    self.project = new TW.viewmodels.Project();
    self.create = function () {
        self.projects.create(self.project, function () {
            self.project.clear();
            self.project.editing(false);
        });
    };

    $.getJSON('api/projects', function (projects) {
        self.projects.data(
            $.map(projects, function (item) {
                return new TW.viewmodels.Project(item);
            }));
    });
    return self;
}