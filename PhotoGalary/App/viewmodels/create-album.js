define(['plugins/dialog', 'services/security', 'services/albums', 'services/notification'], function (dialog, security, album, notify) {
    return {
        name: ko.observable(''),
        activate: function (name) {
            security.checkAuth().done(function (auth) {
                if (!auth) {
                    router.navigate('log_in');
                }
            });
            this.name(name);
        },
        closeModal: function () {
            dialog.close(this, '');
        },
        createAlbum: function () {
            var self = this;
            album.checkalbumName(self.name()).done(function (result) {
                if (!result) {
                    dialog.close(self, self.name());
                } else {
                    notify.error("Album with the same name already exists!");
                }
            });
            
        }
    };
});