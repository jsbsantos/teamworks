(function (pages) {
    pages.ProfileViewModel = function (json) {

        var mapping = {
            'ignore': ['editing'],
            'gravatar': {
                create: function (options) {
                    return ko.observable(options.data + '&s=256');
                }
            }
        };

        var self = ko.mapping.fromJS(json, mapping);
        self.editing = ko.observable();
        self._update = function () {
            $.ajax('edit',
                        {
                            data: ko.mapping.toJSON(self),
                            type: 'post',
                            statusCode: {
                                200: /*ok*/function (data) {
                                    ko.mapping.fromJS(data, self);
                                    self.editing(false);
                                }
                            }
                        });
        };
        return self;
    };
} (tw.pages));