define(['plugins/router', 'services/security', 'durandal/app', 'services/albums', 'services/notification'], function (router, security, app, album, notify) {
    function getUserEmail() {
        window.user = store.get('user');
    }
    return {
        source: ko.observable(),
        showAlbums: ko.observable(false),
        albumlist: ko.observableArray([]),
        name: ko.observable(''),
        limitReached: ko.observable(true),
        query: ko.observable(''),
        canActivate: function() {
            return security.checkAuth().then(function(status) {
                if (status === false) {
                    return {
                        redirect: 'log_in'
                    };
                }
                return true;
            });
        },

        activate: function() {
            var self = this;
            security.checkAuth().done(function(result) {
                self.showAlbums(true);
                self.albumlist.removeAll();
                if (result) {
                    getUserEmail();
                    album.getAllAlbums(window.user.email).done(function (data) {
                        if (window.user.alimit > data.length || window.user.alimit < 0) {
                            self.limitReached(true);
                        } else {
                            self.limitReached(false);
                        }
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            item.link = '/album/' + item.name;
                            item.imgLink = "data:image/jpeg;base64," + item.cover;
                            self.albumlist.push(item);
                        }
                    });
                } else {
                    router.navigate('log_in');
                }
            });
        },

        createAlbum: function() {
            var self = this;
            app.showDialog('viewmodels/create-album', this.name()).then(function(name) {
                if (name === '') {
                    return;
                }
                album.saveAlbum(name, name, window.user.email).done(function (result) {
                    if (result === -1) {
                        notify.error('Failed to Create album!');
                        return;
                    }
                    if (window.user.alimit === result) {
                        self.limitReached(false);
                        notify.warning('Limit of Albums reached. Please buy vip account to create more albums!');
                    } else {
                        self.limitReached(true);
                        notify.success('Album succsessfuly saved!');
                    }
                    self.albumlist.push({ name: name, link: '/album/' + name, imgLink: " " });
                    router.navigate('album/' + name);
                });
            });
        },
        search: function() {
            var self = this;
            app.trigger("searchNavigate", { query: self.query(), showFilters: false });
        },
        advancedSearch: function() {
            var self = this;
            app.trigger("searchNavigate", { query: self.query(), showFilters: true });
        }
    };
});