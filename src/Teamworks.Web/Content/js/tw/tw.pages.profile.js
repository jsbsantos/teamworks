(function(pages) {
    pages.ProfileViewModel = function(endpoint, json) {

        var mapping = {
            'gravatar': {
                create: function(options) {
                    return options.data + '&s=256';
                }
            }
        };

        var self = ko.mapping.fromJS(json, mapping);
        return self;
    };
}(tw.pages));