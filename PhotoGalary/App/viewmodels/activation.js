define(['plugins/router', 'services/security', 'services/notification'], function (router, security, notify) {
    var activationStatus = function (status) {
        switch (status) {
            case 0:
                notify.success('Activation successful.');
                router.navigate('home');
                break;
            case 1:
                notify.error('Activation failed.');
                break;
            case 2:
                notify.error('Activation code you specified is wrong.');
                break;
            case 3:
                notify.error('Activation code you specified has already expired.');
                break;
        }
    };

    return {
        token: ko.observable(),

        activateUser: function () {
            security.activate(this.token())
                .done(function (status) {
                    activationStatus(status);
                });
        },

        activate: function (token) {
            if (token) {
                this.token(token);
                security.activate(token)
                    .done(function (status) {
                        activationStatus(status);
                    });
            }
        },
        deactivate: function () {
            this.token('');
        }
    };
});