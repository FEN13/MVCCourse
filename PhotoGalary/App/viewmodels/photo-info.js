define(['plugins/router', 'services/security', 'durandal/app', 'services/albums', 'services/photos', 'services/notification'], function (router, security, app, album, photos, notify) {
    return {
        albumLink: ko.observable(),
        imgLink: ko.observable(),
        name: ko.observable(),
        diafragm: ko.observable(),
        iso: ko.observable(),
        focusDist: ko.observable(),
        flash: ko.observable(),
        location: ko.observable(),
        shutter: ko.observable(),
        deviceManufacturer: ko.observable(),
        deviceModel: ko.observable(),
        views: ko.observable(),
        likes: ko.observable(),
        id: ko.observable(),
        addDate: ko.observable(),
        description: ko.observable(),
        isCover: ko.observable(false),
        cameraModels: ko.observableArray([]),
        selectedModels: ko.observableArray([]),
        deviceManufacturers: ko.observableArray([]),
        album: " ",
        activate: function (albumName, id) {
            var self = this;
            self.album = albumName;
            security.checkAuth().done(function (result) {
                if (!result) {
                    router.navigate('log_in');
                }

               
                photos.getManuFacturers().done(function (res) {
                    if (res.length > 0) {
                        self.deviceManufacturers.removeAll();
                        self.cameraModels.removeAll();
                        for (var i = 0; i < res.length; i++) {
                            var manufacturer = res[i];
                            self.deviceManufacturers.push(res[i]);
                            self.cameraModels.push(res[i].models);
                            for (var j = 0; j < manufacturer.models.length; j++) {
                                self.cameraModels.push(manufacturer.models[j]);
                            }
                        }
                    }
                    photos.getPhoto(id).done(function (pinfo) {
                        if (pinfo) {
                            self.id(id);
                            self.addDate(pinfo.addDate.split('T')[0] + ' ' + pinfo.addDate.split('T')[1]);
                            self.albumLink("/album/" + pinfo.albumName);
                            self.imgLink('data:image/jpeg;base64,' + pinfo.previewSize);
                            self.name(pinfo.name);
                            self.diafragm(pinfo.diafragm);
                            self.iso(pinfo.iso);
                            self.focusDist(pinfo.focusDistance);
                            self.flash(pinfo.isFlashUsed);
                            self.location(pinfo.location);
                            self.shutter(pinfo.shutterSpeed);
                            for (var k = 0; k < self.deviceManufacturers().length; k++) {
                                if (self.deviceManufacturers()[k].name === pinfo.deviceManufacturer) {
                                    self.deviceManufacturer(self.deviceManufacturers()[k]);
                                    break;
                                }
                            }
                            for (var l = 0; l < self.cameraModels().length; l++) {
                                if (self.cameraModels()[l].name === pinfo.deviceModel) {
                                    self.deviceModel(self.cameraModels()[l]);
                                    break;
                                }
                            }

                            self.views(pinfo.views);
                            self.likes(pinfo.likes);
                            self.description(pinfo.description);
                            self.isCover(pinfo.isCover);
                        }
                    });
                });
            });
            // change state selection based on country selection
            self.selectedModels = ko.dependentObservable(function () {
                console.log(self.deviceManufacturer());
                if (self.deviceManufacturer()) {
                    return ko.utils.arrayFilter(self.cameraModels(), function (item) {
                        return item.manufacturer === self.deviceManufacturer().id;
                    });
                }
                return [];
            });

        },
        attached: function () {
            $("#diafragm").numeric({ negative: false }, function () { this.value = ""; this.focus(); });
            $("#shutter").numeric({ negative: false }, function () { this.value = ""; this.focus(); });
            $("#iso").numeric({ negative: false }, function () { this.value = ""; this.focus(); });
            $("#fdist").numeric({ negative: false }, function () { this.value = ""; this.focus(); });
            $("#name").change(function () {
                var value = $.trim($(this).val());
                console.log(value);
                if (value == "") {
                    $("#name").addClass("error");
                } else {
                    $("#name").removeClass("error");
                }
            });
        },

        updatePhoto: function () {
            var self = this;
            if ($.trim(self.name()) != "") {
                var item = {
                    Id: self.id(),
                    Name: self.name(),
                    Location: self.location(),
                    FocusDistance: self.focusDist(),
                    Diafragm: self.diafragm(),
                    ShutterSpeed: self.shutter(),
                    ISO: self.iso(),
                    IsFlashUsed: self.flash(),
                    Description: self.description(),
                    DeviceManufacturer: self.deviceManufacturer() != null ? self.deviceManufacturer().id : -1,
                    DeviceModel: self.deviceModel() != null ? self.deviceModel() : -1,
                    IsCover: self.isCover(),
                    AlbumName: self.album
                };
                photos.savePhotoMetadata(item).done(function (res) {
                    if (res > 0) {
                        notify.success("Photo metadata sucessfully updated.");
                    } else {
                        notify.error("Failed to update photo metadata.");
                    }
                });
            } else {
                notify.error("Name is required field");
                $("#name").addClass("field-validation-error");
            }
        },
    };
});