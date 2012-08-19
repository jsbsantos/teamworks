    (function(pages) {
    pages.RegisterTimelogsViewModel = function(data, json) {
        var self = this;

        self._select = function(i) {
            self.selected(i);
            self.timelogs.input.date(i.date);
        };

        self.tabs = [
            { description: "2 days ago", date: Date.today().add(-2).days().toISOString(), calendar: false },
            { description: "Yesterday", date: Date.today().add(-1).days().toISOString(), calendar: false },
            { description: "Today", date: Date.today().toISOString(), calendar: false },
            { description: "Calendar", date: Date.today().toISOString(), calendar: true }];
        self.selected = ko.observable(self.tabs[2]);

        var mapping = {
            'date': {
                create: function(options) {
                    return ko.observable(options.data).extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                }
            }
        };
        
        var now = Date.today().toString('dd/MM/yyyy');
        self.timelogs = ko.mapping.fromJS(json || [], mapping);
        self.timelogs._create = function () {
            var entity = ko.mapping.toJS(self.typeahead.entity);
            $.ajax(
                '/api/projects/' + entity.project.id
                    + '/activities/' + entity.activity.id + '/timelogs/',
                {
                    type: 'post',
                    data: ko.toJSON(self.timelogs.input),
                    statusCode: {
                        201: /*created*/function (data) {
                            alert(data);
                        },
                        400: /*bad request*/function () {
                            tw.page.alerts.push({ message: 'An error as ocurred.' });
                        }
                    }
                });
        };
        self.timelogs.input = {
            date: ko.observable(now).extend({
                isoDate: 'dd/MM/yyyy'
            }),
            description: ko.observable().extend({ required: "Describe what have you done." }),
            duration: ko.observable().extend({ required: "How much time do you used." })
        };

        self.source = data;
        self.typeahead = function(typeahead) {
            var source = [];
            for (var i = 0; i < self.source.length; i++) {
                source.push(i);
            }

            typeahead.source = source;
            typeahead.matcher = function(item) {
                var o = self.source[item];
                var composed = o.project.name + ', ' + o.activity.name;
                return ~o.activity.name.toLowerCase().indexOf(this.query.toLowerCase())
                    || ~o.project.name.toLowerCase().indexOf(this.query.toLowerCase())
                        || ~composed.toLowerCase().indexOf(this.query.toLowerCase());
            };
            typeahead.sorter = function(items) {
                // if a sorter is needed this means
                // a change as been made reset entity
                self.typeahead.reset();
                return items;
            };
            typeahead.render = function(items) {
                var that = this;
                items = $(items).map(function(i, item) {
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
            typeahead.updater = function(item) {
                var o = self.source[item];
                self.typeahead.entity = o;
                self.typeahead.has_error(false);
                return o.project.name + ', ' + o.activity.name;
            };
        };

        self.typeahead.has_error = ko.observable(true);
        self.typeahead.validation_message = ko.computed(function() {
            return self.typeahead.has_error() ? "Specify the activity you worked." : "";
        });

        self.typeahead.entity = { };
        self.typeahead.reset = function() {
            if (self.typeahead.has_error) return;
            self.typeahead.entity = { };
            self.typeahead.has_error(true);
        };

        return self;
    };
}(tw.pages));