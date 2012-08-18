(function (pages) {
    pages.ProjectViewModel = function (json) {
        var errorCallback = function () {
            tw.page.alerts.push({ message: 'An error as ocurred.' });
        };

        var remove = function (name, endpoint) {
            return function () {
                var obj = this;
                var message = 'You are about to delete ' + obj.name() + '.';
                if (confirm(message)) {
                    var collection = self[name];
                    $.ajax({
                            type: 'post',
                            url: endpoint + obj.id()
                        }).success(function () {
                            collection.mappedRemove(obj);
                        }).error(errorCallback);
                }
            };
        };

        var mapping = {
            'activities': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    return (new (function () {
                        this._remove = remove('activities', tw.utils.location + '/activities/remove/');
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
                        this._remove = remove('discussions', tw.utils.location + '/discussions/remove/');
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
                        this._remove = remove('people', tw.utils.location + '/people/remove/');
                        var m = {
                            'gravatar': {
                                create: function (opt) {
                                    return opt.data + '&s=64';
                                }
                            }
                        };
                        ko.mapping.fromJS(options.data, m, this);
                    })());
                }
            }
        };

        var self = ko.mapping.fromJS(json, mapping);

        self.activities.input = ko.observable();
        self.activities.editing = ko.observable();

        self.activities._create = function () {
            $.ajax(tw.utils.location + '/activities/',
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
            $.ajax(tw.utils.location + '/discussions/',
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
            var email = self.people.input();
            $.ajax({
                url: tw.utils.location + '/people/add',
                type: 'post',
                data: ko.toJSON({ 'email': email })
            }).success(function(data) {
                    self.people.mappedCreate(data);
                    self.people.input("");
                }).error(errorCallback);
        };

        self.people.input = ko.observable();
        self.people.editing = ko.observable();

        self.people.typeahead = function (typeahead) {
            var data = {};
            var endpoint = '/people';
            var filter = function (items) {
                data = {};
                var labels = $(items).map(function (i, item) {
                    data[item.id] = item;
                    return item.id.toString();
                });
                return labels;
            };

            typeahead.options.item = '<li class="row"><a class="span2" href="#"></a></li>';
            typeahead.matcher = function () {
                return true;
            };
            typeahead.updater = function (item) {
                self.people.input(data[item].email);
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