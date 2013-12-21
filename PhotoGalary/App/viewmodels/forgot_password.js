define(['plugins/router', 'services/security', 'services/notification'], function (router, security, notify) {
    return {
        email: ko.observable(),

        reset: function () {
            security.forgot(this.email())
                .done(function (status) {
                    switch (status) {
                        case 0:
                            notify.success('Instructions to reset your password were sent to your email.');
                            router.navigate('reset_password');
                            break;
                        case 1:
                            notify.error('Reset password failed.');
                            break;
                    }
                });
        }
    };
});