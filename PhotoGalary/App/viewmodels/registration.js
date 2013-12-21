define(['plugins/router', 'services/security', 'services/notification'], function (router, security, notify) {
    return {
        firstName: ko.observable(''),
        lastName: ko.observable(''),
        email: ko.observable(''),
        password: ko.observable(''),
        creditCard: ko.observable(''),

        activate: function () {
            this.firstName('');
            this.lastName('');
            this.email('');
            this.password('');
            this.creditCard('');
        },

        attached: function () {
            $.mask.definitions['~'] = '[+-]';
            $('#creditcard').mask('9999-9999-9999-9999');
        },

        register: function () {
            var self = this;
            var emailValid, passwavild, userdataValid;
            if (security.validateEmailFormat(self.email())) {
                security.validateEmail(self.email()).done(function (result) {
                    emailValid = !result;
                    passwavild = $.trim(self.password()) != "" && self.password().length >= 6;
                    userdataValid = $.trim(self.firstName()) != "" && $.trim(self.lastName()) != "";

                    if (!emailValid) {
                        notify.error("Email already in use.");
                    } else if (!passwavild) {
                        notify.error("Password should have have more then 6 symbols");
                    } else if (!userdataValid) {
                        notify.error("First name and last name are required.");
                    }

                    if (emailValid && passwavild && userdataValid) {
                        security.register(self.firstName(), self.lastName(), self.email(), self.password(), self.creditCard()).done(function (data) {
                            switch (data) {
                                case 0:
                                    notify.success('Registration successful.');
                                    router.navigate('activation');
                                    break;
                                case 1:
                                    notify.error('User registration failed.');
                                    break;
                                case 2:
                                    notify.error('User with same email already registered.');
                                    break;
                            }
                        });
                    }
                });
            } else {
                notify.error("Wrong email format.");
            }
        }
    };
});