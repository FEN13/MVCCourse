toastr.options.timeOut = 4000;
toastr.options.positionClass = 'toast-bottom-right';

requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    }
});

define('jquery', function () { return jQuery; });
define('knockout', ko);
define('store', store);

define(['durandal/system', 'durandal/app', 'durandal/viewLocator'], function (system, app, viewLocator) {

    //>>excludeStart("build", true);
    system.debug(true);
    //>>excludeEnd("build");

    app.title = 'Photo Gallery';

    app.configurePlugins({
        router: true,
        dialog: true,
        widget: true
    });

    app.start().then(function () {
        viewLocator.useConvention();
        app.setRoot('viewmodels/body', 'entrance');
    });
});
