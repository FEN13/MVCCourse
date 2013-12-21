define(['plugins/router', 'services/security', 'services/notification', 'durandal/app'], function (router, security, notify, app) {

    function setData(email,role, plimit, alimit) {
        store.set("user", { email: email, role: role, plimit: plimit, alimit: alimit });
    }
    return {
        email: ko.observable(),
        password: ko.observable(),
        isLogingIn: ko.observable(false),
        isDisabled: ko.observable(),

        login: function () {
            var self = this;
            $("#password").trigger("change");
            self.isLogingIn(true);
            self.isDisabled('disabled');
            security.login(this.email(), this.password())
             .done(function (data) {
                 switch (data.result) {
                     case 0:
                         notify.success('Welcome back!');
                         setData(data.email, data.role, data.photoLimit, data.albumLimit);
                         app.trigger('onlogin', data.role);
                         router.navigate('home');
                         break;
                     case 1:
                         notify.error('User does not exist or password is wrong.');
                         break;
                     case 2:
                         notify.error('User was blocked by administrator.');
                         break;
                     case 3:
                         notify.warning('User was not activated. Please find instructions in email.');
                         router.navigate('activate');
                         break;
                 }
                 self.isLogingIn(false);
                 self.isDisabled(null);
             });

        },
    };
});