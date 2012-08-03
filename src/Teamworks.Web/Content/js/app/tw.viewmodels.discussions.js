/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.debug.js" />
/// <reference path="~/Content/js/libs/knockout.mapping.debug.js" />

(function(obj) {
    obj.Discussion = function(endpoint) {
        var messages_endpoint = endpoint + '/messages/';
        
        var self = this;
        
        var mapping = {
            'messages': {
                update: function(options) {
                    return options.data;
                }
            }
        };

        self.discussion = ko.mapping.fromJS({ }, mapping);
        self.discussion.message = {
            content: ko.observable(),
            _create: function() {
                $.ajax(endpoint + '/messages/',
                    {
                        type: 'post',
                        data: ko.toJSON(self.discussion.message),
                        statusCode: {
                            201: /*created*/function(data) {
                                self.discussion.messages.push(ko.mapping.fromJS(data));
                                self.message.content("");
                            },
                            400: /*bad request*/function() {
                                TW.page.alerts.push({ message: 'An error as ocurred.' });
                            }
                        }
                    }
                );
            }
        };

        $.ajax(endpoint,
            {
            async: false,
            statusCode: {
                200: /*ok*/function(data) {
                    ko.mapping.fromJS(data, self.discussion);
                }
            }
        })
        ;
    };
}(TW.viewmodels));