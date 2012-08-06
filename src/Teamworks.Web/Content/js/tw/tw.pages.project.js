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
                    return (new (function () {
                        this._remove = function () {
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

                        var mapping = {
                            'gravatar': {
                                create: function (options) {
                                    return options.data + '&s=64';
                                }
                            }
                        };

                        ko.mapping.fromJS(options.data, mapping, this);
                    })());
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

        self.people._add = function () {
            var model = {
                emails: [],
                ids: []
            };

            var value = self.people.input();
            if (isNaN(value)) {
                model.emails.push(value);
            } else {
                model.ids.push(value);
            }

            $.ajax(endpoint + '/people/',
                {
                    type: 'post',
                    data: ko.toJSON(model),
                    statusCode: {
                        201: /*created*/function (data) {
                            self.people.mappedCreate(data);
                            self.people.input("");
                        },
                        400: /*bad request*/function () {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                }
            );
        };

        self.people.input = ko.observable();
        self.people.editing = ko.observable();

        self.people.typeahead = function (typeahead) {
            var data = {};
            var endpoint = '/api/people';
            var filter = function (items) {
                data = {};
                var labels = $(items).map(function (i, item) {
                    data[item.id] = item;
                    return item.id;
                });
                return labels;
            };

            typeahead.options.item = '<li class="row"><a class="span2" href="#"></a></li>';
            typeahead.matcher = function () {
                return true;
            };
            typeahead.updater = function (item) {
                self.people.input(data[item].id);
                self.people._add();
                return "";
            };
            typeahead.render = function (items) {
                var that = this;

                items = $(items).map(function (i, item) {
                    var o = data[item];
                    i = $(that.options.item).attr('data-value', item);
                    var block = $(document.createElement('div'))
                        .append('<div>' + that.highlighter(o.name) + '</div>')
                        .append('<div>(@' + that.highlighter(o.username) + ')</div>');

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
            $elem.on('keyup', function () {
                var t = g = setTimeout(function () {
                    if (t !== g) return;
                    var query = typeahead.query;
                    if (!query) {
                        typeahead.source = filter([]);
                        return;
                    }
                    ;
                    $.get(endpoint, { q: query }, function (data) {
                        if (query !== typeahead.query) return;
                        typeahead.source = filter(data);
                        typeahead.lookup();
                    });
                }, 200);
            });

        };


        return self;
    };
} (tw.pages));