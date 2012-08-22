    (function (pages) {
        pages.RegisterTimelogsViewModel = function (data, json) {
            var self = this;
            var errorCallback = function (message) {
                tw.page.alerts.push({ message: 'An error as ocurred.' + message && ('\'' + message + '\'') });
            };

            self._select = function (i) {
                self.selected(i);
                self.timelogs.input.date(i.date);
            };

            self.tabs = [
                { description: "2 days ago", date: Date.today().add(-2).days().toISOString(), calendar: false },
                { description: "Yesterday", date: Date.today().add(-1).days().toISOString(), calendar: false },
                { description: "Today", date: Date.today().toISOString(), calendar: false },
                { description: "Calendar", date: Date.today().toISOString(), calendar: true}];
            self.selected = ko.observable(self.tabs[2]);

            var mapping = {
                create: function (options) {
                    return (new (function () {
                        this.editing = ko.observable(false);

                        this._remove = function () {
                            var timelog = this;
                            tw.utils.remove(self.timelogs,
                               '/projects/' + timelog.project.id() + '/activities/' + timelog.activity.id() + '/timelogs/' + timelog.id() + '/delete',
                                'You are about to remove the time log for activity ' + timelog.activity.name() + ', of project ' + timelog.project.name() + '.',
                                errorCallback).call(timelog);
                        };

                        this._update = function () {
                            var timelog = this;
                            $.ajax({
                                type: 'post',
                                url: '/projects/' + timelog.project.id() + '/activities/' + timelog.activity.id() + '/timelogs/edit',
                                data: ko.toJSON(timelog)
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
            };
            self.discardChanges = function () {
                ko.mapping.fromJS(json, self.timelogs);
            };

            var now = Date.today().toISOString();
            self.timelogs = ko.mapping.fromJS(json || [], mapping);
            self.timelogs._create = function () {
                if (self.timelogs.input.has_error()) {
                    return;
                }
                self.timelogs.input.editing(true);
                var entity = ko.mapping.toJS(self.typeahead.entity);
                $.ajax('/projects/' + entity.project.id + '/activities/' + entity.activity.id + '/timelogs/create',
                    {
                        type: 'post',
                        data: ko.toJSON(self.timelogs.input)
                    }).done(function (d) {
                        self.timelogs.mappedCreate(d);
                        self.timelogs.input.editing(false);
                    })
                    .error(errorCallback);
            };

            self.timelogs.input = {
                date: ko.observable(now).extend({
                    isoDate: 'dd/MM/yyyy'
                }),
                description: ko.observable().extend({ required: "Describe what have you done." }),
                duration: ko.observable().extend({ required: "How much time do you used.", duration: "" })
            };
            self.timelogs.input.editing = ko.observable(false);
            self.timelogs.input.has_error = ko.computed(function () {
                return self.timelogs.input.description.has_error() || self.timelogs.input.duration() == undefined;
            });

            self.source = data;
            self.typeahead = function (typeahead) {
                var source = [];
                for (var i = 0; i < self.source.length; i++) {
                    source.push(i);
                }

                typeahead.source = source;
                typeahead.matcher = function (item) {
                    var o = self.source[item];
                    var composed = o.project.name + ', ' + o.activity.name;
                    return ~o.activity.name.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~o.project.name.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~composed.toLowerCase().indexOf(this.query.toLowerCase());
                };
                typeahead.sorter = function (items) {
                    // if a sorter is needed this means
                    // a change as been made reset entity
                    self.typeahead.reset();
                    return items;
                };
                typeahead.render = function (items) {
                    var that = this;
                    items = $(items).map(function (i, item) {
                        var o = self.source[item];
                        i = $(that.options.item).attr('data-value', item);
                        var composed = o.project.name + ', ' + o.activity.name;
                        i.find('a').html(that.highlighter(composed));
                        return i[0];
                    });

                    items.first().addClass('active');
                    this.$menu.html(items);
                    return this;
                };
                typeahead.updater = function (item) {
                    var o = self.source[item];
                    self.typeahead.entity = o;
                    self.typeahead.has_error(false);
                    return o.project.name + ', ' + o.activity.name;
                };
            };

            self.typeahead.has_error = ko.observable(true);
            self.typeahead.validation_message = ko.computed(function () {
                return self.typeahead.has_error() ? "Specify the activity you worked." : "";
            });

            self.typeahead.entity = {};
            self.typeahead.reset = function () {
                if (self.typeahead.has_error) return;
                self.typeahead.entity = {};
                self.typeahead.has_error(true);

            };

            var sortFunction = function (a, b) {
                var result = a[self.sortProperty()]() > b[self.sortProperty()]() ? 1 : -1;
                return self.sortAsc() ? result : result * -1;
            };
            self.sortProperty = ko.observable("duration");
            self.sortAsc = ko.observable(true);
            self.sort = function (property) {
                if (self.sortProperty() == property)
                    self.sortAsc(!self.sortAsc());
                else {
                    self.sortProperty(property);
                    self.sortAsc(true);
                }
            };
            self.sortedTimelog = ko.computed(function () { return self.timelogs().sort(sortFunction); });

            return self;
        };
    } (tw.pages));