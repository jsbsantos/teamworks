(function (pages) {
    pages.DiscussionViewModel = function (json) {
        var errorCallback = function () {
            tw.page.alerts.push({ message: 'An error as ocurred.' });
        };

        var self = ko.mapping.fromJS(json, {});
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