(function(pages) {
    pages.ProjectViewModel = function(json) {
        var errorCallback = function() {
            tw.page.alerts.push({ message: 'An error as ocurred.' });
        };
        var self = { };
        var mapping = {
            'activities': {
                key: function(data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function(options) {
                    return (new (function() {
                        this._remove = function() {
                            var activity = this;
                            tw.utils.remove(self.activities,
                                tw.utils.location + '/activities/' + activity.id() + '/delete',
                                'You are about to delete ' + activity.name() + '.',
                                errorCallback).call(activity);
                        };
                        ko.mapping.fromJS(options.data, { }, this);
                    })());
                }
            },
            'discussions': {
                key: function(data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function(options) {
                    return (new (function() {
                        this._remove = function() {
                            var discussion = this;
                            tw.utils.remove(self.discussions,
                                tw.utils.location + '/discussions/' + discussion.id() + '/delete',
                                'You are about to delete ' + discussion.name() + '.',
                                errorCallback).call(discussion);
                        };
                        ko.mapping.fromJS(options.data, { }, this);
                    })());
                }
            },
            'people': {
                key: function(data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function(options) {
                    return (new (function() {
                        this._remove = function() {
                            var discussion = this;
                            tw.utils.remove(self.people,
                                tw.utils.location + '/people/' + this.id() + '/delete',
                                'You are about to delete ' + this.name() + '.',
                                errorCallback).call(discussion);
                        };
                        var m = {
                            'gravatar': {
                                create: function(opt) {
                                    return opt.data + '&s=64';
                                }
                            }
                        };
                        ko.mapping.fromJS(options.data, m, this);
                    })());
                }
            }
        };

        self = ko.mapping.fromJS(json || [], mapping);
        self.activities.input = ko.observable();
        self.activities.editing = ko.observable();

        self.activities._create = function() {
            $.ajax(tw.utils.location + '/activities/',
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.activities.input() }),
                }
            ).done(function(data) {
                self.activities.mappedCreate(data);
                self.activities.input("");
            }).fail(errorCallback);
        };

        self.discussions.input = ko.observable();
        self.discussions.editing = ko.observable();

        self.discussions._create = function() {
            $.ajax(tw.utils.location + '/discussions/',
                {
                    type: 'post',
                    data: ko.toJSON({ 'name': self.discussions.input() }),
                }
            ).done(function(data) {
                self.discussions.mappedCreate(data);
                self.discussions.input("");
            }).fail(errorCallback);
        };

        self.people._add = function() {
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

        self.people.typeahead = function(typeahead) {
            var data = { };
            var endpoint = '/people';
            var filter = function(items) {
                data = { };
                var labels = $(items).map(function(i, item) {
                    data[item.id] = item;
                    return item.id.toString();
                });
                return labels;
            };

            typeahead.options.item = '<li class="row"><a class="span2" href="#"></a></li>';
            typeahead.matcher = function() {
                return true;
            };
            typeahead.updater = function(item) {
                self.people.input(data[item].email);
                self.people._add();
                return "";
            };
            typeahead.render = function(items) {
                var that = this;

                items = $(items).map(function(i, item) {
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
            $elem.on('keyup', function() {
                var t = g = setTimeout(function() {
                    if (t !== g) return;
                    var query = typeahead.query;
                    if (!query) {
                        typeahead.source = filter([]);
                        return;
                    }
                    ;
                    $.get(endpoint, { q: query }, function(data) {
                        if (query !== typeahead.query) return;
                        typeahead.source = filter(data);
                        typeahead.lookup();
                    });
                }, 200);
            });
        };

        self.timelogs = new tw.pages.RegisterTimelogsViewModel({ }, json.timelogs);

        self.timelogs.filter.clear = function() {
            self.timelogs.filter.reset();
            self.timelogs.filter.project({ id: self.id(), name: self.name() });

        };
        self.timelogs.filter.project({ id: self.id(), name: self.name() });
        
        $('body').ready(function(){
            $('#tabs a[href="#tab-discussions"]').tab('show');
        });
        return self;
    };
}(tw.pages));