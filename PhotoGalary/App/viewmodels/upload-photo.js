define(['plugins/router', 'services/security', 'plugins/dialog', 'services/notification', 'services/photos'], function (router, security, dialog, notify, photo) {
    var windowUrl = window.URL || window.webkitURL;

    ko.bindingHandlers.file = {
        init: function (element, valueAccessor) {
            $(element).change(function () {
                var file = this.files[0];
                if (ko.isObservable(valueAccessor())) {
                    valueAccessor()(file);
                }
            });
        },

        update: function (element, valueAccessor, allBindingsAccessor) {
            var file = ko.utils.unwrapObservable(valueAccessor());
            var bindings = allBindingsAccessor();

            if (bindings.fileObjectURL && ko.isObservable(bindings.fileObjectURL)) {
                var oldUrl = bindings.fileObjectURL();
                if (oldUrl) {
                    windowUrl.revokeObjectURL(oldUrl);
                }
                bindings.fileObjectURL(file && windowUrl.createObjectURL(file));
            }

            if (bindings.fileBinaryData && ko.isObservable(bindings.fileBinaryData)) {
                if (!file) {
                    bindings.fileBinaryData(null);
                } else {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        bindings.fileBinaryData(e.target.result);
                    };
                    reader.readAsArrayBuffer(file);
                }
            }
        }
    };
    var user = store.get('user');
    return {
        albumName: ko.observable(),
        images: ko.observableArray([]),
        activate: function (name) {
            var self = this;
            security.checkAuth().done(function (result) {
                if (!result) {
                    router.navigate('log_in');
                }

            });

            self.images.removeAll();
            self.albumName(name.name());

            self.imageFile = ko.observable();
            self.imageObjectURL = ko.observable();
            self.imageBinary = ko.observable();
            self.imageType = ko.computed(function () {
                var file = this.imageFile();
                return file ? file.type : "mime";
            }, self);

            self.fileSize = ko.computed(function () {
                var file = this.imageFile();
                return file ? file.size : 0;
            }, self);

            self.resBytes = ko.computed(function () {
                if (self.imageBinary()) {
                    var buf = new Uint8Array(self.imageBinary());
                    var bytes = [];
                    for (var i = 0; i < buf.length; ++i) {
                        bytes.push(buf[i]);
                    }
                    return bytes;
                } else {
                    return '';
                }
            }, self);

            self.images.push(self);
        },

        closeModal: function () {
            dialog.close(this, '');
        },

        uploadPhoto: function (model) {
            var self = this;
            photo.checkPhotoMimeType(model.imageType(), model.resBytes().length).done(function (res) {
                if (res) {
                    var image = model.imageFile();
                    photo.uploadPhoto(self.albumName(), user.email, self.resBytes(), image.name).done(function (result) {
                        if (result == -1) {
                            notify.error("Failed to upload image.");
                        } else {
                            dialog.close(self, result);
                        }
                    });
                } else {
                    notify.error("Failed to upload image. Wrong mime type or file is to big");
                }
            });
        }
    };
});