define(function () {
    return {
        info: function (message, title) {
            toastr.info(message, title);
        },
        success: function (message, title) {
            toastr.success(message, title);
        },
        warning: function (message, title) {
            toastr.warning(message, title);
        },
        error: function (message, title) {
            toastr.error(message, title);
        },
        clear: function () {
            toastr.clear();
        }
    };
});