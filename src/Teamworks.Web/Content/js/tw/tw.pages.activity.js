(function (pages) {
    pages.ActivityViewModel = function (endpoint, json) {
        var mapping = {
            'startDate': {
                update: function (options) {
                    options.observable.extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                    return options.data;
                }
            },
            'timelogs': {
                create: function (options) {
                    return (new (function () {
                        this.editing = ko.observable(false);
                        this._remove = function () {
                            var timelog = this;
                            var message = 'You are about to remove the time log for activity ' + self.name() + '.';
                            if (confirm(message)) {
                                alert("todo: remove timelog id:" + timelog.id());
                            }
                        };
                        this._update = function () {
                            var timelog = this;
                            alert("todo: update timelog id:" + timelog.id());
                        };

                        var m = {
                            'date': {
                                update: function (o) {
                                    o.observable.extend({
                                        isoDate: 'dd/MM/yyyy'
                                    });
                                    return o.data;
                                }
                            }  
                        };

                        ko.mapping.fromJS(options.data, m, this);
                    })());
                }
            },
            'dependencies': {
                create: function (options) {
                    return (new (function () {
                        this._remove = function () {
                            var activity = this;
                            var message = 'You are about to remove the dependency on ' + activity.name() + '.';
                            if (confirm(message)) {
                                $.ajax(endpoint + '/' + viewmodel().id() + "/precedences/" + activity.id(),
                                    {
                                        type: 'delete',
                                        statusCode: {
                                            204: /*no content*/function () {
                                                self.activities.mappedRemove(activity);
                                            }
                                        }
                                    });
                            }
                        };
                        ko.mapping.fromJS(options.data, {}, this);
                    })());
                }
            }
        };

        var self = ko.mapping.fromJS(json || [], mapping);

        self.completionPercent = ko.computed(function () {
            return ((self.totalTimeLogged() / self.duration()) * 100).toPrecision(3);
        });

        self.editing_timelog = ko.observable(false);
        self.editing_state = ko.observable(false);
        self.editing_dependencies = ko.observable(false);

        self.discardChanges = function () {
            ko.mapping.fromJS(json.dependencies, self);
        };

        self.dependenciesChanged = ko.observable(false);

        self._update = function () {
            $.ajax(endpoint + '/edit/',
                {
                    type: 'post',
                    data: ko.toJSON({
                        id: self.id(),
                        name: self.name(),
                        StartDate: self.startDate(),
                        duration: self.duration(),
                        description: self.description(),
                        project: self.projectReference.id(),
                        dependencies: $.map($.grep(self.dependencies(), function (e, i) { return e.dependency(); }), function (e) { return e.id(); })
                    })
                }
            ).success(function (d) {
                self.editing_description(false);
                json = d;
            }).error(function () {
                tw.page.alerts.push({ message: 'An error as ocurred.' });
                self.editing_description(false);
            });
        };
        return self;
    };
} (tw.pages));