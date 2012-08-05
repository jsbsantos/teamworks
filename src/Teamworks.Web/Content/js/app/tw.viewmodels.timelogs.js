﻿/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />
/// <reference path="~/Content/js/libs/knockout.mapping.debug.js" />

(function (obj) {
    ko.extenders.notEmpty = function (target, message) {
        var fn = function (value) {
            var valid = $.isEmptyObject(value);
            target.validation_message(valid ? "" : message);
        };

        return tw.extenders.validation(target, fn);
    };

    obj.Time = function (source, timelogs) {
        var self = this;

        self._select = function (item) {
            self.selected(item);
            self.timelog.date(item.date);
        };

        self.tabs = [
            { description: "2 days ago", date: Date.today().add(-2).days().toISOString(), calendar: false },
            { description: "Yesterday", date: Date.today().add(-1).days().toISOString(), calendar: false },
            { description: "Today", date: Date.today().toISOString(), calendar: false },
            { description: "Calendar", date: Date.today().toISOString(), calendar: true}];
        self.selected = ko.observable(self.tabs[2]);

        var now = Date.now().toString('dd/MM/yyyy');
        self.timelog = {
            date: ko.observable(now).extend({
                isoDate: 'dd/MM/yyyy'
            }),
            description: ko.observable().extend({ required: "Describe what have you done." }),
            duration: ko.observable().extend({ required: "How much time do you used." })
        };

        var mapping = {
            'date': {
                update: function (options) {
                    options.observable.extend({
                        isoDate: 'dd/MM/yyyy'
                    });
                    return options.data;
                }
            }
        };

        self.timelogs = ko.mapping.fromJS(timelogs || [], mapping);
        self.timelogs._create = function () {
            var entity = ko.mapping.toJS(self.typeahead.entity);
            var projectId = entity.projectId;
            var activityId = entity.activityId;

            $.ajax(
                '/api/projects/' + projectId + '/activities/' + activityId + '/timelogs/',
                {
                    type: 'post',
                    data: ko.toJSON(self.timelog),
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

        self.source = ko.mapping.fromJS(source);
        self.typeahead = function (typeahead) {
            var source = [];
            for (var i = 0; i < self.source().length; i++) {
                source.push(i);
            }

            typeahead.source = source;
            typeahead.matcher = function (item) {
                var o = self.source()[item];
                var composed = o.project() + ', ' + o.activity();
                return ~o.activity().toLowerCase().indexOf(this.query.toLowerCase())
                    || ~o.project().toLowerCase().indexOf(this.query.toLowerCase())
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
                    var o = self.source()[item];
                    i = $(that.options.item).attr('data-value', item);
                    var composed = o.project() + ', ' + o.activity();
                    i.find('a').html(that.highlighter(composed));
                    return i[0];
                });

                items.first().addClass('active');
                this.$menu.html(items);
                return this;
            };
            typeahead.updater = function (item) {
                var o = self.source()[item];
                ko.mapping.fromJS(o, self.typeahead.entity);
                return o.project() + ', ' + o.activity();
            };
        };

        self.typeahead.validation_message = ko.observable("");
        self.typeahead.has_error = ko.computed(function () {
            return self.typeahead.validation_message().length > 0;
        });

        self.typeahead.entity = ko.mapping.fromJS({});
        self.typeahead.reset = function () {
            if (self.typeahead.has_error) return;
            ko.mapping.fromJS({}, self.typeahead.entity);
        };
    };
} (tw.pages));