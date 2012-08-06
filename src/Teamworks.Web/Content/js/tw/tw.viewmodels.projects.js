/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />
/// <reference path="~/Content/js/libs/knockout.mapping.debug.js" />

(function(obj) {
    obj.Project = function(endpoint) {
        var peopleEndpoint = endpoint + '/people/';
        var activitiesEndpoint = endpoint + '/activities/';
        var discussionsEndpoint = endpoint + '/discussions/';

        var self = this;

        self.project = ko.mapping.fromJS({ });

        self.project.person = {
            text: ko.observable(),
            editing: ko.observable()
        };

        // people
        self.project.people = ko.mapping.fromJS([]);
        self.project.people._add = function() {
            var model = {
                emails: [],
                ids: []
            };

            var value = self.project.person.text();
            if (isNaN(value)) {
                model.emails.push(value);
            } else {
                model.ids.push(value);
            }

            $.ajax(peopleEndpoint,
                {
                    type: 'post',
                    data: ko.toJSON(model),
                    statusCode: {
                        201: /*created*/function(data) {
                            self.project.people.push(ko.mapping.fromJS(data));
                            self.project.person.text("");
                        },
                        400: /*bad request*/function() {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };
        self.project.people.typeahead = function(typeahead) {
            var data = { };
            var endpoint = '/api/people';
            var filter = function(items) {
                data = { };
                var labels = $(items).map(function(i, item) {
                    data[item.id] = item;
                    return item.id;
                });
                return labels;
            };

            typeahead.updater = function(item) {
                self.project.person.text(data[item].id);
                self.project.people._add();
                return "";
            };
            typeahead.matcher = function() {
                return true;
            };

            typeahead.render = function(items) {
                var that = this;

                items = $(items).map(function(i, item) {
                    var o = data[item];
                    i = $(that.options.item).attr('data-value', item);

                    var block = $(document.createElement('div'))
                        .append('<div>' + that.highlighter(o.name) + '</div>')
                        .append('<div>(@@' + that.highlighter(o.username) + ')</div>');

                    i.find('a')
                        .append('<img style="padding-right: 4px" class="pull-left" src=' + o.gravatar + '&s=36' + '/>')
                        .append(block);

                    return i[0];
                });

                items.first().addClass('active');
                this.$menu.html(items);
                return this;
            };

            var g = 0;
            var $elem = typeahead.$element;
            $elem.on('keyup', function() {
                var t = g = setTimeout(function() {
                    if (t !== g) return;
                    var query = typeahead.query;
                    if (!query) {
                        typeahead.source = filter([]);
                        return;
                    };
                    $.get(endpoint, { q: query }, function(data) {
                        if (query !== typeahead.query) return;
                        typeahead.source = filter(data);
                        typeahead.lookup();
                    });
                }, 200);
            });

        };

        // activities
        self.project.activity = {
            name: ko.observable(),
            editing: ko.observable()
        };

        self.project.activities = ko.mapping.fromJS([]);
        self.project.activities._create = function() {
            $.ajax(activitiesEndpoint,
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.project.activity.name() }),
                    statusCode: {
                        201: /*created*/function(data) {
                            self.project.activities.push(ko.mapping.fromJS(data));
                            self.project.activity.name("");
                        },
                        400: /*bad request*/function() {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };
        self.project.activities._remove = function() {
            var activity = this;
            var message = 'You are about to delete ' + activity.name() + '.';
            if (confirm(message)) {
                $.ajax(activitiesEndpoint + activity.id(),
                    {
                        type: 'delete',
                        statusCode: {
                            204: /*no content*/function() {
                                self.project.activities.remove(activity);
                            }
                        }
                    });
            }
        };

        // discussions
        self.project.discussion = {
            name: ko.observable(),
            editing: ko.observable()
        };

        self.project.discussions = ko.mapping.fromJS([]);
        self.project.discussions._create = function() {
            $.ajax(discussionsEndpoint,
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.project.discussion.name() }),
                    statusCode: {
                        201: /*created*/function(data) {
                            self.project.discussions.push(ko.mapping.fromJS(data));
                            self.project.discussions.name("");
                        },
                        400: /*bad request*/function() {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };
        self.project.discussions._remove = function() {
            var discussion = this;
            var message = 'You are about to delete ' + discussion.name() + '.';
            if (confirm(message)) {
                $.ajax(discussionsEndpoint + discussion.id(),
                    {
                        type: 'delete',
                        statusCode: {
                            204: /*no content*/function() {
                                self.project.discussions.remove(discussion);
                            }
                        }
                    });
            }
        };

        $.ajax(endpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project);
                }
            }
        });

        $.ajax(peopleEndpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project.people);
                }
            }
        });

        $.ajax(activitiesEndpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project.activities);
                }
            }
        });

        $.ajax(discussionsEndpoint, {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.project.discussions);
                }
            }
        });
    };
}(tw.viewmodels));

(function(obj) {
    obj.Projects = function(endpoint) {
        var self = this;

        self.name = ko.observable().extend({ required: "" });
        self.editing = ko.observable(false);

        self.projects = ko.mapping.fromJS([]);
        self.projects._create = function() {
            $.ajax(endpoint,
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.name() }),
                    statusCode: {
                        201: /*created*/function(data) {
                            self.projects.push(ko.mapping.fromJS(data));
                            self.editing(false);
                            self.name('');
                        },
                        400: /*bad request*/function(data) {
                            tw.helpers.bad_format(self.project, data.responseText);
                        }
                    }
                }
            );
        };

        self.projects._remove = function() {
            var project = this;
            var message = 'You are about to delete ' + project.name() + '.';
            if (confirm(message)) {
                $.ajax(endpoint + project.id(),
                    {
                        type: 'delete',
                        statusCode: {
                            204: /*no content*/function() {
                                self.projects.remove(project);
                            }
                        }
                    });
            }
        };

        $.getJSON(endpoint, function(projects) {
            ko.mapping.fromJS(projects, self.projects);
        });
    };
}(tw.viewmodels));