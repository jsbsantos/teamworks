(function (pages) {
    pages.ActivityViewModel = function (json) {
        var errorCallback = function (data) {
            tw.bindings.alerts.push({ message: ((data && data.statusText) || 'An error as ocurred.') });
        };
        var mapping = {
            'startDate': {
                create: function (options) {
                    return ko.observable(options.data).extend({
                        isoDate: 'dd/MM/yyyy'
                    });
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
                            tw.utils.remove(self.discussions,
                                tw.utils.location + '/discussions/' + discussion.id() + '/delete',
                                'You are about to delete ' + discussion.name() + '.',
                                errorCallback).call(discussion);
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

                        var m = {
                            'duration': {
                                create: function (o) {
                                    return ko.observable(o.data).extend({ duration: "" });
                                }
                            },
                            'startDate': {
                                create: function (o) {
                                    return ko.observable(o.data).extend({
                                        isoDate: 'dd/MM/yyyy'
                                    });
                                }
                            }
                        };
                        ko.mapping.fromJS(options.data, m, this);
                    })());
                }
            },
            'todos': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (o) {
                    return ko.mapping.fromJS(o.data); ;
                }
            },
            'duration': {
                create: function (o) {
                    return ko.observable(o.data).extend({ duration: "" });
                }
            },
            'totalTimeLogged': {
                create: function (o) {
                    return ko.observable(o.data).extend({ duration: "" });
                }
            },
            'title': {
                create: function (o) {
                    return "teste";
                }
            }
        };

        var self = ko.mapping.fromJS(json || [], mapping);
        self.completionPercent = ko.computed(function () {
            return ((self.totalTimeLogged() / self.duration()) * 100).toPrecision(3);
        });

        /***************************************
        *               Discussion             *
        ****************************************/
        self.discussions.input = {
            name: ko.observable().extend({ required: "Discussion title." }),
            content: ko.observable().extend({ required: "Discussion message." }),
            updating: ko.observable(false)
        };

        self.discussions.input.reset = function () {
            self.discussions.input.name("");
            self.discussions.input.content("");
            self.discussions.input.updating(false);
        };

        self.discussions.editing = ko.observable(false);
        self.discussions._create = function () {
            self.discussions.input.updating(true);
            $.ajax({
                type: 'post',
                url: tw.utils.location + '/discussions',
                data: ko.toJSON(self.discussions.input)
            }).done(function (data) {
                self.discussions.mappedCreate(data);
            }).fail(errorCallback)
                .always(function () {
                    $('#addDiscussionModal').modal('hide');
                    self.discussions.input.reset();
                });
        };

        /***************************************
        *                 Todos                *
        ****************************************/
        self.todos.input = {
            name: ko.observable().extend({ required: "Todo Name." }),
            description: ko.observable().extend({ required: "Todo Description." }),
            updating: ko.observable(false)
        };

        self.todos.input.reset = function () {
            self.todos.input.name("");
            self.todos.input.description("");
            self.todos.input.updating(false);
        };

        self.todos.editing = ko.observable(false);
        self.todos._create = function () {
            self.todos.input.updating(true);
            $.ajax({
                type: 'post',
                url: tw.utils.location + '/todos',
                data: ko.toJSON(self.todos.input)
            }).done(function (data) {
                self.todos.mappedCreate(data);
            }).fail(errorCallback)
                .always(function () {
                    $('#addTodoModal').modal('hide');
                    self.todos.input.reset();
                });
        };

        self.todos.todoToggle = function (e) {
            $.ajax({
                type: 'post',
                url: tw.utils.location + '/todos/' + e.id(),
                data: ko.toJSON({ state: e.completed() })
            }).done(function (data) {
                self.todos.input.reset();
            }).fail(errorCallback)
                .always(function () {
                    $('#addTodoModal').modal('hide');
                });
            return true;
        };

        /***************************************
        *            Self/Dependecies          *
        ****************************************/
        self.discardChanges = function () {
            ko.mapping.fromJS(json.dependencies, self);
        };
        self.editing_dependencies = ko.observable(false);
        self.updating = ko.observable(false);
        self.dependenciesChanged = ko.observable(false);
        self._update = function () {
           self.updating(true);
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
                self.updating(false);
                $('#editActivityModal').modal('hide');
            });
        };


        /***************************************
        *                People                *
        ****************************************/
        self.people._add = function () {
            var email = self.people.input();
            $.ajax({
                url: tw.utils.location + '/people',
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


        /***************************************
        *               Typeahead              *
        ****************************************/
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