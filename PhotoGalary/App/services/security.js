define(['plugins/http'], function (http) {
    var baseUri = window.AppRootPath;
    return {
        checkAuth: function () {
            return http.get(baseUri + '/api/auth/isauthenticated');
        },
        getUser: function (email) {
            return http.get(baseUri + '/api/users/GetUser', { email: email });
        },
        login: function (email, password) {
            return http.post(baseUri + '/api/auth/login', { email: email, password: password });
        },
        logout: function () {
            return http.post(baseUri + '/api/auth/logout');
        },
        register: function (firstName, lastName, email, password, creditCard) {
            return http.post(baseUri + '/api/auth/register', { firstName: firstName, lastName: lastName, email: email, password: password, creditCardNumber: creditCard });
        },
        forgot: function (email) {
            return http.post(baseUri + '/api/auth/forgot', { email: email });
        },
        reset: function (token, password) {
            return http.post(baseUri + '/api/auth/reset', { token: token, password: password });
        },
        validateEmail: function (email) {
            return http.get(baseUri + '/api/users/checkEmail', { email: email });
        },
        validateAdmins: function (role) {
            return http.get(baseUri + '/api/users/checkAdmins', { role: role });
        },
        validateEmailFormat: function (email) {
            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            return emailPattern.test(email);
        },
        activate: function (token) {
            return http.post('/api/auth/activate', { token: token });
        },
    };
});