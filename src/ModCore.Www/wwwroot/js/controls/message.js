(function ($) {

    $.fn.message = function (options) {

        var settings = $.extend({
            messageType: "info",
            text: ""
        }, options);

        var classNames = "alert";
        if (options.messageType === "error") {
            classNames += " alert-danger";
        }
        else if (options.messageType === "info") {
            classNames += " alert-info";
        }
        else if (options.messageType === "success") {
            classNames += " alert-success";
        }

        this.addClass(classNames);
        this.text(options.text);
        // this.append('<a href="#" class="close" title="close">×</a>');
        this.css("display", "none");

        return this;
    };

    $.fn.message.prototype.showBySlide = function (delay) {
        $(this).slideDown(delay);
    }


}(jQuery));