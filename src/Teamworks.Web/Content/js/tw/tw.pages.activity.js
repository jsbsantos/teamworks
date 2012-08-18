(function(pages) {
    pages.ActivityViewModel = function(endpoint, json) {
        var mapping = {
            'startDate': {
                update: function(options) {
                    options.observable.extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                    return options.data;
                }
            },
            'date': {
                update: function(options) {
                    options.observable.extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                    return options.data;
                }
            },
            'dependencies': {
                create: function(options) {
                    return (new (function() {
                        this._remove = function() {
                            var activity = this;
                            var message = 'You are about to remove the dependency on ' + activity.name() + '.';
                            if (confirm(message)) {
                                $.ajax(endpoint + '/' + viewmodel().id() + "/precedences/" + activity.id(),
                                    {
                                        type: 'delete',
                                        statusCode: {
                                            204: /*no content*/function() {
                                                self.activities.mappedRemove(activity);
                                            }
                                        }
                                    });
                            }
                        };
                        ko.mapping.fromJS(options.data, { }, this);
                    })());
                }
            }
        };

        var self = ko.mapping.fromJS(json || [], mapping);

        self.completionPercent = ko.computed(function() {
            return ((self.totalTimeLogged() / self.duration()) * 100).toPrecision(3);
        });

        self.editing_description = ko.observable(false);
        self.editing_duration = ko.observable(false);
        self.editing_dependencies = ko.observable(false);
        self.dependenciesChanged = ko.observable(false);

        self._update = function() {
            $.ajax(endpoint.replace("api/","") + "/update/" + self.id(),
                {
                    type: 'put',
                    data: ko.toJSON({
                        id: self.id(),
                        name: self.name(),
                        duration: self.duration(),
                        description: self.description()
                    }),
                    statusCode: {
                        201: /*created*/function() {
                            self.editing_description(false);
                        },
                        400: /*bad request*/function() {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                            self.editing_description(false);
                        }
                    }
                }
            );
        };
        return self;
    };
}(tw.pages));