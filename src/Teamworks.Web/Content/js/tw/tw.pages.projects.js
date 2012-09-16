(function (pages) {
    pages.ProjectsViewModel = function (json) {
        var errorCallback = function (data) {
            tw.bindings.alerts.push({ message: ((data && data.statusText) || 'An error as ocurred.') });
        };
        
        var mapping = {
            'projects': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    return (new (function () {
                        this._remove = function () {
                            var project = this;
                            var message = 'You are about to delete ' + project.name() + '.';
                            if (confirm(message)) {
                                $.ajax(project.id(),
                                    {
                                        type: 'post',
                                        statusCode: {
                                            204: /*no content*/function () {
                                                self.projects.mappedRemove(project);
                                            }
                                        }
                                    });
                            }
                        };

                        var mapping = {
                            'gravatar': {
                                create: function (options) {
                                    return options.data + '&s=32';
                                }
                            }
                        };
                        ko.mapping.fromJS(options.data, mapping, this);
                    })());
                }
            }
        };

        var self = ko.mapping.fromJS(json, mapping);
        self.projects.input = ko.observable().extend({
            required: "You should provide a name for the project."
        });
        self.projects.editing = ko.observable();
        self.projects.updating = ko.observable(false);
        self.projects._create = function () {
            self.projects.updating(true);
            $.ajax("",
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.projects.input() })
                }).done(function (data) {
                    self.projects.mappedCreate(data);
                }).fail(errorCallback)
                .always(function () {
                    self.projects.input("");
                    self.projects.editing(false);
                });
            ;
        };
        return self;
    };
} (tw.pages));