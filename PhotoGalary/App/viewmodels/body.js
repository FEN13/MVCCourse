define(['plugins/router', 'durandal/app', 'services/security', 'services/notification', 'services/photos'], function (router, app, security, notify, photo) {
    function cleanData() {
        store.remove('user');
    }
    function getUserEmail() {
        window.user = store.get('user');
    }
    return {
        email: ko.observable(),
        router: router,
        userFullName: ko.observable(),
        currentDate: ko.observable(new Date().getFullYear()),
        footertext: ko.observable(''),
        showLogedInBlock: ko.observable(false),
        logourl: ko.observable(window.AppRootPath + "/Images/logo.png"),
        isActive: ko.observable(false),

        activate: function () {
            var self = this;
            self.footertext('&copy; ' + self.currentDate() + ' - Photo Galary');
            getUserEmail();
            app.on('onlogin').then(function (role) {
                if (role != "None") {
                    self.setFullname(function (rolee) {
                        if (rolee != "None") {
                            self.showLogedInBlock(true);
                        }
                        if (rolee == "Administrator") {
                            self.isActive(true);
                        }
                    });
                }
            });

            app.on('searchNavigate', function (params) {
                app.trigger("search", params);
                router.navigate('search-results');
            });

            security.checkAuth().done(function (data) {
                if (!data) {
                    app.trigger('onlogin', "None");
                } else {
                    self.setFullname(function (role) {
                        if (role != "None") {
                            self.showLogedInBlock(true);
                        } else {
                            self.showLogedInBlock(false);
                            security.logout();
                        }
                        app.trigger('onlogin', role);
                    });

                }
            });

            self.logout = function () {
                security.logout().done(function () {
                    self.showLogedInBlock(false);
                    self.isActive(false);
                    cleanData();
                    notify.success('Goodbye!');
                    self.login();
                });
            },

            self.login = function () {
                router.navigate('log_in');
            },

            self.setFullname = function (callback) {
                var result = "None";
                if (user != null) {
                    security.getUser(window.user.email).done(function (usr) {
                        if (usr != null) {
                            self.userFullName = ko.computed(function () {
                                return usr.firstName + " " + usr.lastName;
                            });
                            result = usr.role;
                        } else {
                            self.logout();
                        }
                        callback(result);
                    });
                }
            },

            router.map([
                { route: '', title: 'Home', moduleId: 'viewmodels/home', onlyAuth: true },
                { route: 'registration', title: 'Create User', moduleId: 'viewmodels/registration', onlyAuth: true },
                { route: 'activation(/:token)', title: 'Activation', moduleId: 'viewmodels/activation', onlyAuth: false },
                { route: 'log_in', title: 'Log in', moduleId: 'viewmodels/log_in', role: "All" },
                { route: 'reset_password(/:token)', title: 'Reset password', moduleId: 'viewmodels/reset_password', onlyAuth: false },
                { route: 'forgot_password', title: 'Forgot password', moduleId: 'viewmodels/forgot_password', onlyAuth: false },
                { route: 'albums(/:email)', title: 'User List', moduleId: 'viewmodels/home', onlyAuth: true },
                { route: 'album(/:albumName)', title: 'Album', moduleId: 'viewmodels/album', onlyAuth: false },
                { route: 'album/(:albumName)/photo(/:photoId)', title: '', moduleId: 'viewmodels/photo-info', onlyAuth: true },
                { route: 'settings', title: 'Settings', moduleId: 'viewmodels/settings', onlyAuth: true, },
                { route: 'search-results', title: 'Search results', moduleId: 'viewmodels/search-results', onlyAuth: true }

            ])
                .buildNavigationModel()
                .mapUnknownRoutes('viewmodels/home', 'Home');


            return router.activate({ pushState: true, root: window.AppRootPath });
        }
    };
});