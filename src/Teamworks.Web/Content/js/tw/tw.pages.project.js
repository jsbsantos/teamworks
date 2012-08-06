(function (pages) {
    pages.ProjectViewModel = function (endpoint, json) {
        var mapping = {
            'activities': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    return (new (function () {
                        this._remove = function () {
                            var activity = this;
                            var message = 'You are about to delete ' + activity.name() + '.';
                            if (confirm(message)) {
                                $.ajax(endpoint + '/activities/' + activity.id(),
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
            },
            'discussions': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    return (new (function () {
                        this._remove = function () {
                            var discussion = this;
                            var message = 'You are about to delete ' + discussion.name() + '.';
                            if (confirm(message)) {
                                $.ajax(endpoint + '/discussions/' + discussion.id(),
                                {
                                    type: 'delete',
                                    statusCode: {
                                        204: /*no content*/function () {
                                            self.discussions.mappedRemove(discussion);
                                        }
                                    }
                                });
                            }
                        };
                        ko.mapping.fromJS(options.data, {}, this);
                    })());
                }
            },
            'people': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    var o = ko.mapping.fromJS(options.data);
                    o._remove = function () {
                        var person = this;
                        var message = 'You are about to delete ' + person.name() + '.';
                        if (confirm(message)) {
                            $.ajax(endpoint + '/people/' + person.id(),
                                {
                                    type: 'delete',
                                    statusCode: {
                                        204: /*no content*/function () {
                                            self.people.mappedRemove(person);
                                        }
                                    }
                                });
                        }
                    };
                    return o;
                }
            }
        };

        var self = ko.mapping.fromJS(json, mapping);

        self.activities.input = ko.observable();
        self.activities.editing = ko.observable();

        self.activities._create = function () {
            $.ajax(endpoint + '/activities/',
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.activities.input() }),
                    statusCode: {
                        201: /*created*/function (data) {
                            self.activities.mappedCreate(data);
                            self.activities.input("");
                        },
                        400: /*bad request*/function () {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };

        self.discussions.input = ko.observable();
        self.discussions.editing = ko.observable();

        self.discussions._create = function () {
            $.ajax(endpoint + '/discussions/',
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.discussions.input() }),
                    statusCode: {
                        201: /*created*/function (data) {
                            self.discussions.mappedCreate(data);
                            self.discussions.input("");
                        },
                        400: /*bad request*/function () {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };
        return self;
    };
} (tw.pages));



