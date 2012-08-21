(function (pages) {
    pages.ActivityViewModel = function (json) {
        var errorCallback = function (message) {
            tw.page.alerts.push({ message: 'An error as ocurred.' + message && ('\'' + message + '\'') });
        };

        var mapping = {
            'startDate': {
                create: function (options) {
                    return ko.observable(options.data).extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                }
            },
            'timelogs': {
                create: function (options) {
                    return (new (function () {
                        this.editing = ko.observable(false);
                        this._remove = function () {
                            var timelog = this;
                            tw.utils.remove(self.timelogs,
                                tw.utils.location + '/timelogs/' + timelog.id() + '/delete',
                                'You are about to remove the time log for activity ' + self.name() + '.',
                                errorCallback).call(timelog);
                        };
                        this._update = function () {
                            var timelog = this;
                            $.ajax({
                                type: 'post',
                                url: tw.utils.location + '/timelogs/edit',
                                data: ko.toJSON(timelog)
                            }).success(function () {

                            }).error(errorCallback);
                        };

                        var m = {
                            'date': {
                                create: function (options) {
                                    return ko.observable(options.data).extend({
                                        isoDate: 'dd/MM/yyyy'
                                    });
                                }
                            }
                        };

                        ko.mapping.fromJS(options.data, m, this);
                    })());
                }
            },
            'people': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    return (new (function () {
                        this._remove = (function () {
                            var obj = this;
                            var message = 'You are about to delete ' + obj.name() + '.';
                            if (confirm(message)) {
                                $.ajax({
                                    type: 'post',
                                    url: tw.utils.location + '/people/' + obj.id() + '/delete'
                                }).done(function (data) {
                                    var i = self.people.mappedIndexOf(obj);
                                    self.people()[i].assigned(false);
                                    self.people.notifySubscribers(self.people);
                                }).fail(errorCallback);
                            }
                        });

                        var m = {
                            'gravatar': {
                                create: function (opt) {
                                    return opt.data + '&s=64';
                                }
                            },
                            'assigned': {
                                update: function (opt) {
                                    options.parent.notifySubscribers(options.parent);
                                    return opt.data;
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
                            tw.utils.remove(self.precedences,
                                tw.utils.location + '/precedences/' + activity.id() + '/delete',
                                'You are about to remove the dependency on ' + activity.name() + '.',
                                errorCallback).call(activity);
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

        self.discussions.input = ko.observable();
        self.discussions.editing = ko.observable(false);
        self.discussions._create = function () {
            $.ajax({
                type: 'post',
                url: tw.utils.location + '/discussions',
                data: ko.toJSON({ 'name': self.discussions.input() })
            }).done(function (data) {
                self.discussions.mappedCreate(data);
                self.discussions.input("");
            }).fail(errorCallback);
        };

        self.discardChanges = function () {
            ko.mapping.fromJS(json.dependencies, self);
        };

        self.dependenciesChanged = ko.observable(false);
        self._update = function () {
            $.ajax(tw.utils.location + '/edit',
                {
                    type: 'post',
                    data: ko.toJSON({
                        id: self.id(),
                        name: self.name(),
                        StartDate: self.startDate(),
                        duration: self.duration(),
                        description: self.description(),
                        project: self.project.id(),
                        dependencies: $.map($.grep(self.dependencies(), function (item) { return item.dependency(); }), function (e) { return e.id(); })
                    })
                }
            ).done(function (d) {
                json = d;
            }).fail(function (jq, textStatus, errorThrown) {
                errorCallback(errorThrown);
            }).always(function () {
                self.dependenciesChanged(false);
                self.editing_description(false);
            });
        };

        self.people._add = function () {
            var email = self.people.input();
            $.ajax({
                url: tw.utils.location + '/people/add',
                type: 'post',
                data: ko.toJSON({ 'email': email })
            }).done(function (data) {
                var i = self.people.mappedIndexOf(data);
                self.people()[i].assigned(true);
                self.people.input("");
                self.people.notifySubscribers(self.people);
            }).fail(function (jk, textStatus, errorThrown) {
                errorCallback(errorThrown);
            });
        };

        self.people.input = ko.observable();
        self.people.editing = ko.observable();
        self.people.assigned = ko.computed(function () {
            return $.grep(self.people(), function (item) {
                return item.assigned();
            });
        });
        self.people.unassigned = ko.computed(function () {
            return $.grep(self.people(), function (item) {
                return !item.assigned();
            });
        });

        // typeahead 
        self.people.typeahead = function (typeahead) {
            var refill = function () {
                var source = [];
                for (var i = 0; i < self.people.unassigned().length; i++) {
                    source.push(i);
                }
                typeahead.source = source;
                typeahead.lookup();
            };

            self.people.subscribe(function () {
                refill();
            });

            refill();

            typeahead.options.item = '<li class="row"><a class="span2" href="#"></a></li>';
            typeahead.matcher = function (item) {
                var o = self.people.unassigned()[item];
                return ~o.name().toLowerCase().indexOf(this.query.toLowerCase())
                    || ~o.username().toLowerCase().indexOf(this.query.toLowerCase())
                    || ~o.email().toLowerCase().indexOf(this.query.toLowerCase());
            };
            typeahead.updater = function (item) {
                self.people.input(self.people.unassigned()[item].email);
                self.people._add();
                return "";
            };
            typeahead.render = function (items) {
                var that = this;

                items = $(items).map(function (i, item) {
                    var o = self.people.unassigned()[item];
                    i = $(that.options.item).attr('data-value', item);
                    var block = $(document.createElement('div'))
                        .append('<div>' + that.highlighter(o.name()) + '</div>')
                        .append('<div>(@' + that.highlighter(o.username()) + ')</div>');

                    i.find('a')
                        .append('<img style="padding-right: 4px" class="pull-left" src=' + o.gravatar + '&s=36' + '/>')
                        .append(block);

                    return i[0];
                });

                items.first().addClass('active');
                this.$menu.html(items);
                return this;
            };
            typeahead.sorter = function (items) {
                // if a sorter is needed this means
                // a change as been made reset entity
                return items;
            };
        };


        return self;
    };
} (tw.pages));