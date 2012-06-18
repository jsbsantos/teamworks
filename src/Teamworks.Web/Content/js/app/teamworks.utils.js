/// <reference path="~/Content/js/libs/jquery-1.7.2.min.js" />
/// <reference path="~/Content/js/libs/knockout-2.0.0.js" />

if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

var TW = TW || {};
TW.utils = TW.utils || {};

$(document).ajaxStart(console.log('ajax start'));
$(document).ajaxStop(console.log('ajax stop'));