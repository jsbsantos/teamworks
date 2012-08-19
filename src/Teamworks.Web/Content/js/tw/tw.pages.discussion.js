(function (pages) {
    pages.DiscussionViewModel = function (json) {
        var errorCallback = function () {
            tw.page.alerts.push({ message: 'An error as ocurred.' });
        };

        var mapping = {
            'messages': {
                update: function(options) {
                    return ko.mapping.fromJS(options.data);
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
                url: tw.utils.location + '/messages/create'
            }).success(function (data) {
                self.messages.mappedCreate(data);
                self.messages.input("");
            }).error(errorCallback);
        };

        return self;
    };
} (tw.pages));