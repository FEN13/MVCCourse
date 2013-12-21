define(['plugins/router', 'services/security', 'services/notification'], function (router, security, notify) {
    var resetStatus = function (status) {
        switch (status) {
            case 0:
                notify.success('Password was successfuly reset.');
                router.navigate('home');
                break;
            case 1:
                notify.error('Reset password failed.');
                break;
            case 2:
                notify.error('Reset code you specified is wrong.');
                break;
            case 3:
                notify.error('Reset code you specified has expired.');
                break;
        }
    };

    return {
        token: ko.observable(),
        password: ko.observable(),
        showToken: ko.observable(true),

        reset: function () {
            if (this.password().length >= 6) {
                security.reset(this.token(), this.password())
               .done(function (status) {
                   resetStatus(status);
               });
            } else {
                notify.error("Password length should be 6 or more symbols.");
            }

        },
        activate: function (token) {
            if (token) {
                this.token(token);
                this.showToken(false);
            }
        },
        deactivate: function () {
            this.token('');
            this.password('');
            this.showToken(true);
        }
    };
});