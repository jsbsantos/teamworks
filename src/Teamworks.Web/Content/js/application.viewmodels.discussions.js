var discussion_viewmodel = function (discussion) {
    var self = this;

    self.message = new Message();
    self.endpoint = "/api/" + discussion.project + "/discussions/" + discussion.id + "/messages/";
    self.message_list = ko.observableArray(
        $.map(discussion.messages, function (item) {
            return new Message(item);
        })
    );

    /* interact methods */
    self.create = function () {
        var request = $.ajax(self.endpoint, {
            data: ko.toJSON(self.message),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    data.url = request.getResponseHeader("Location");
                    self.message_list.push(new Message(data));
                    $('#discussion-modal').modal('hide');
                    self.message.clear();
                }
            }
        });
    };
    
    self.update = function () {
        var message = this;
        if (confirm('You are about to update ' + discussion.name() + '.')) {
            $.ajax(message.url(), {
                data: ko.toJSON(message),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false'
            });
        }
    };
    
    self.remove = function () {
        var message = this;
        if (confirm('You are about to delete a timelog entry.')) {
            $.ajax(self.endpoint + message.id(), {
                type: 'delete',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function () {
                        self.message_list.remove(message);
                    }
                }
            });
        }
    };
};

var discussion_list_viewmodel = function (projectid, discussions) {
    var self = this;
    self.discussion = new Discussion();
    
    self.endpoint = "/api/projects/" + projectid + "/discussions/";
    self.discussions = ko.observableArray(
        $.map(discussions, function (item) {
            return new Discussion(item);
        })
    );

    /* interact methods */
    self.create = function () {
        var request = $.ajax(self.endpoint, {
            data: ko.toJSON(self.discussion),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            cache: 'false',
            statusCode: {
                201: /*created*/function (data) {
                    // push a new task
                    data.url = request.getResponseHeader("Location");
                    self.discussions.push(new Discussion(data));
                    $('#discussion-modal').modal('hide');
                    self.discussion.clear();
                }
            }
        });
    };
    self.update = function () {
        var discussion = this;
        if (confirm('You are about to update ' + discussion.name() + '.')) {
            $.ajax(discussion.url(), {
                data: ko.toJSON(discussion),
                type: 'put',
                contentType: "application/json; charset=utf-8",
                cache: 'false'
            });
        }
    };
    self.remove = function () {
        var discussion = this;
        if (confirm('You are about to delete ' + discussion.name() + '.')) {
            $.ajax(self.endpoint + discussion.id(), {
                type: 'delete',
                contentType: "application/json; charset=utf-8",
                cache: 'false',
                statusCode: {
                    204: /*no content*/function () {
                        self.discussions.remove(task);
                    }
                }
            });
        }
    };
};