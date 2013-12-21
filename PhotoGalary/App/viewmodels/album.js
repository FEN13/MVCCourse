define(['plugins/router', 'services/security', 'durandal/app', 'services/albums', 'services/photos', 'services/notification'], function (router, security, app, album, photos, notify) {
    return {
        oldName: ko.observable(),
        name: ko.observable(),
        coverId: ko.observable(),
        albumPhotos: ko.observableArray([]),
        loadPhoto: ko.observable(false),
        enableEdit: ko.observable(false),
        photoLimitReached: ko.observable(true),
        query: ko.observable(),
        activate: function (name) {
            var self = this;
            security.checkAuth().done(function (result) {
                if (result) {
                    self.enableEdit(true);
                } else {
                    self.enableEdit(false);
                }
                self.oldName(name);
                self.name(name);
                app.on('onEditAlbum', function () {
                    self.loadPhotos();
                });
                self.loadPhotos();
            });
        },
        loadPhotos: function () {
            var self = this;
            photos.getPhotosForAlbum(self.name()).done(function (items) {
                self.albumPhotos.removeAll();
                if (items != null && items.length > 0) {
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];
                        item.link = 'album/' + item.albumName + '/photo/' + item.id;
                        item.imgLink = 'data:image/jpeg;base64,' + item.thumb;
                        item.imgLinkFull = 'data:image/jpeg;base64,' + item.image;
                        item.addDate = item.addDate.split('T')[0] + " " + item.addDate.split('T')[1].substr(1, 4);
                        item.likes = ko.observable(item.likes);
                        item.active = (i === 0);
                        self.albumPhotos.push(item);
                    }

                    if (window.user.plimit > items.length || window.user.plimit < 0) {
                        self.photoLimitReached(true);
                    } else {
                        self.photoLimitReached(false);
                    }
                    self.loadPhoto(true);
                }
            });
        },
        updateAlbum: function () {
            var self = this;
            app.showDialog('viewmodels/create-album', self.name()).then(function (name) {
                if (name === '') {
                    return;
                }
                self.name(name);
                album.saveAlbum(self.oldName(), name, window.user.email).done(function (result) {
                    if (result.code != 200 || result.data.segments.length === 0) {
                        return;
                    }
                });
            });
        },

        uploadPhoto: function () {
            app.showDialog('viewmodels/upload-photo', this).then(function (itemsCount) {
                if (user.plimit == itemsCount) {
                    notify.warning('Limit of photos in album reached. Please buy vip account to upload more photos!');
                } else {
                    notify.success("Successfuly uploaded");
                }
                app.trigger('onEditAlbum');
            });
        },

        deleteAlbum: function () {
            var self = this;
            app.showMessage('Are you sure you want to delete this album?', 'Confirmation', ['Yes', 'No']).done(function (res) {
                if (res == "Yes") {
                    album.deleteAlbum(self.name()).done(function (result) {
                        if (result != -1) {
                            notify.success("Album '" + self.name() + "' was removed successfuly.");
                            router.navigate('home');
                        } else {
                            notify.error("Failed to remove '" + self.name() + "' album. Try again later.");
                        }
                    });
                }
            });
        },

        deletePhoto: function (item) {
            app.showMessage('Are you sure you want to to delete this photo?', 'Confirmation', ['Yes', 'No']).done(function (res) {
                if (res == "Yes") {
                    photos.removePhoto(item.id, item.albumName).done(function (result) {
                        if (result != -1) {
                            notify.success("Photo '" + item.name + "' was removed successfuly.");
                            app.trigger('onEditAlbum');
                        } else {
                            notify.error("Failed to remove '" + item.name + "' photo. Try again later.");
                        }
                    });
                }
            });
        },

        likePhoto: function (item) {
            item.likes(item.likes() + 1);
            photos.likeDislikePhotos(item.id, 1).done(function (res) {
                if (res == 0) {
                    item.likes(item.likes - 1);
                }
            });
        },

        unlikePhoto: function (item) {
            if (item.likes() > 0) {
                item.likes(item.likes() - 1);
                photos.likeDislikePhotos(item.id, 0).done(function (res) {
                    if (res == 0) {
                        item.likes(item.likes() + 1);
                    }
                });
            }
        },

        getEmbededLink: function () {
            alert(document.location);
        },

        fullSize: function (item) {
            $.fancybox({
                'href': 'data:image/jpeg;base64,' + item.image,
                'title': item.name,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic',
                'centerOnScroll': true,

            });
        },

        search: function () {
            var self = this;
            app.trigger("searchNavigate", { query: self.query(), aldumName: self.name(), showFilters:false });
        },
        
        advancedSearch: function () {
            var self = this;
            app.trigger("searchNavigate", { query: self.query(), aldumName: self.name(), showFilters: true });
        }
    };
});