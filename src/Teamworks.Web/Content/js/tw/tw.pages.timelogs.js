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
            };

            self.discardChanges = function () {
                ko.mapping.fromJS(json, self.timelogs);
                self.timelogs.editing(undefined);
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
                description: ko.observable().extend({ required: "What tasks did you perform?" }),
                duration: ko.observable().extend({ required: "How long did it took?", duration: "" })
            };
            self.timelogs.input.editing = ko.observable(false);
            self.timelogs.input.has_error = ko.computed(function () {
                return self.timelogs.input.description.has_error() || self.timelogs.input.duration() == undefined;
            });
            self.timelogs.editing = ko.observable();
            self.timelogs.editing._update = function () {
                self.timelogs.editing()._update.call(self.timelogs.editing());
            };

            /***************************************
            *               Typeahead              *
            ****************************************/
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
                return self.typeahead.has_error() ? "What activity were you working on?" : "";
            });

            self.typeahead.entity = {};
            self.typeahead.reset = function () {
                if (self.typeahead.has_error) return;
                self.typeahead.entity = {};
                self.typeahead.has_error(true);

            };

            /***************************************
            *               Sort                   *
            ****************************************/
            var sortFunction = function (a, b) {
                var p1 = a[self.sort.Property()], p2 = b[self.sort.Property()];
                var result = 0;
                if (!($.isFunction(p1) && $.isFunction(p2))) {
                    p1 = p1.name();
                    p2 = p2.name();
                }
                else {
                    p1 = p1();
                    p2 = p2();
                }

                result = p1 > p2 ? 1 : -1;
                return self.sort.Asc() ? result : result * -1;
            };
            self.sort = function (property) {
                if (self.sort.Property() == property)
                    self.sort.Asc(!self.sort.Asc());

                else {
                    self.sort.Property(property);
                    self.sort.Asc(true);
                }
            };
            self.sort.Property = ko.observable();
            self.sort.Asc = ko.observable(false);

            self.sortedTimelog = ko.computed(function () {
                var result;
                if (self.sort.Property()) {
                    result = self.timelogs().sort(sortFunction);
                } else {
                    result = self.timelogs();
                }
                return result;
            });

            /***************************************
            *              Filtering               *
            ****************************************/
            var getUnique = function (collection) {
                var result = [{ name: "All", id: -1, project: -1}];
                $.each(collection, function (i, e) {
                    //hack:concat with "t" so the item wont show up on knockout observable iterations
                    if (!result[e.project + "t" + e.id]) {
                        result[e.project + "t" + e.id] = 1;
                        result.push(e);
                    }
                });
                return result;
            };
            self.filter = function (e) {
                return !(self.filter.byProject(e) && self.filter.byActivity(e) && self.filter.byDate(e) && self.filter.byPerson(e));
            };
            self.filter.distinctProjects = ko.computed(function () { return getUnique($.map(json, function (e) { return e.project; })); });
            self.filter.distinctActivities = ko.computed(function () { return getUnique($.map(json, function (e) { return { id: e.activity.id, name: e.activity.name, project: e.project.id }; })); });
            self.filter.distinctPeople = self.filter.distinctPeople = ko.computed(function () { return getUnique($.map(json, function (e) { return (e.person && { id: e.person.id, name: e.person.name }) || { id: 0, name: "" }; })); });

            //filter properties
            self.filter.project = ko.observable(self.filter.distinctProjects()[0]);
            self.filter.activity = ko.observable(self.filter.distinctActivities()[0]);
            self.filter.person = ko.observable(self.filter.distinctPeople()[0]);
            self.filter.dateFrom = ko.observable().extend({ isoDate: 'dd/MM/yyyy' });
            self.filter.dateTo = ko.observable().extend({ isoDate: 'dd/MM/yyyy' });

            //filter functions
            self.filter.byProject = function (e) {
                return self.filter.project().id == -1 || self.filter.project().id == e.project.id();
            };
            self.filter.byActivity = function (e) {
                return (self.filter.project().id == -1 && self.filter.activity().id == -1) ||
                    (self.filter.project().id != -1 && (
                        self.filter.activity().id == -1 ||
                            (self.filter.activity().id == e.activity.id() && self.filter.activity().project == e.project.id())));
            };
            self.filter.byPerson = function (e) {
                return self.filter.person().id == -1 || self.filter.person().id == e.person.id();
            };
            self.filter.byDate = function (e) {
                var di = new Date(self.filter.dateFrom() || '1900/01/01'),
                    df = new Date(self.filter.dateTo() || '9999/12/31'),
                    date = new Date(e.date());
                return date >= di && date <= df;
            };

            //filter reset
            self.filter.resetProject = function () {
                self.filter.project(self.filter.distinctProjects()[0]);
            };
            self.filter.resetActivity = function () {
                self.filter.activity(self.filter.distinctActivities()[0]);
            };
            self.filter.resetDates = function () {
                self.filter.dateFrom(undefined);
                self.filter.dateTo(undefined);
            };
            self.filter.resetPerson = function () {
                self.filter.person(self.filter.distinctPeople()[0]);
            };
            self.filter.reset = function () {
                self.filter.resetProject();
                self.filter.resetDates();
                self.filter.resetPerson();
                self.filter.resetEvent(!self.filter.resetEvent());
            };
            ///////////
            //HACK!!!//
            ///////////
            self.filter.resetEvent = ko.observable(false);

            //disable activities dropdown list when project changes to "All"
            self.filter.project.subscribe(function (newValue) {
                self.filter.resetActivity();
            });
            //needed to avoid wrong date intervals
            self.filter.dateTo.subscribe(function (newValue) {
                $('#datepickFrom').data('datepicker').endDate = new Date(newValue);
            });
            self.filter.dateFrom.subscribe(function (newValue) {
                $('#datepickTo').data('datepicker').startDate = new Date(newValue);
            });

            //done
            return self;
        };
    } (tw.pages));