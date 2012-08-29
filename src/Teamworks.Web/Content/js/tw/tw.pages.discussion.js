(function (pages) {
    pages.DiscussionViewModel = function (json) {
        var removeMessage = function () {
            return function () {
                var discussion = this;
                if (!discussion.editable()) return;

                var message = 'You are about to delete a message.';
                if (confirm(message)) {
                    $.ajax({
                        type: 'post',
                        url: tw.utils.location + '/messages/' + discussion.id() + '/delete'
                    }).done(function () {
                        self.messages.mappedRemove(discussion);
                    }).fail(errorCallback);
                }
            };
        };

        var errorCallback = function () {
            tw.page.alerts.push({ message: 'An error as ocurred.' });
        };

        var mapping = {
            'people': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                }
            },
            'messages': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    return (new (function () {
                        this._remove = removeMessage();
                        this._update = function () {
                            var message = this;
                            $.ajax({
                                type: 'post',
                                url: tw.utils.location + '/messages/' + self.id() + '/edit'
                            }).fail(errorCallback).always(function () {
                                $('#addDiscussionModal').modal('hide');
                            }); ;
                        };
                        var m = {
                            'date': {
                                create: function (dateOptions) {
                                    var pattern = 'dd/MM/yyyy HH:mm:ss';
                                    var date = new Date(dateOptions.data).toString(pattern);
                                    return ko.observable(date).extend({
                                        isoDate: pattern
                                    });
                                }
                            }
                        };
                        ko.mapping.fromJS(options.data, m, this);
                    })());
                }
            }
        };

        var self = ko.mapping.fromJS(json, mapping);
        self.messages.input = ko.observable().extend({ required: "" });
        self.messages.input("");

        self.messages._create = function () {
            if (self.messages.input.has_error()) return;

            $.ajax({
                type: 'post',
                data: ko.toJSON({ 'content': self.messages.input() }),
                url: tw.utils.location + '/messages'
            }).done(function (data) {
                var item = self.messages.mappedCreate(data);
                if (self.people.mappedIndexOf(item.person) == -1)
                    self.people.mappedCreate(item.person);
                self.messages.input("");
            }).fail(errorCallback);
        };

        self.toggleWatch = function () {
            var endpoint = tw.utils.location + (self.watching() ? "/unwatch" : "/watch");
            $.ajax({
                type: 'post',
                url: endpoint
            }).done(function (data) {
                self.watching(!self.watching());
            }).fail(errorCallback);
        };

        return self;
    };
} (tw.pages));