(function (pages) {
    pages.DiscussionViewModel = function (json) {
        var removeMessage = function () {
            return function() {
                var discussion = this;
                if (!discussion.editable()) return;

                var message = 'You are about to delete a message.';
                if (confirm(message)) {
                    $.ajax({
                        type: 'post',
                        url: tw.utils.location + '/messages/' + discussion.id() + '/delete'
                    }).success(function() {
                        self.messages.mappedRemove(discussion);
                    }).error(errorCallback);
                }
            };
        };

        var errorCallback = function () {
            tw.page.alerts.push({ message: 'An error as ocurred.' });
        };

        var mapping = {
            'messages': {
                key: function (data) {
                    return ko.utils.unwrapObservable(data.id);
                },
                create: function (options) {
                    return (new (function () {
                        this._remove = removeMessage();
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
            }).success(function (data) {
                self.messages.mappedCreate(data);
                self.messages.input("");
            }).error(errorCallback);
        };

        return self;
    };
} (tw.pages));