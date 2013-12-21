define(['plugins/router', 'services/security', 'durandal/app', 'services/albums', 'services/photos', 'services/notification'], function (router, security, app, album, photos, notify) {
    return {
        albumPhotos: ko.observableArray([]),
        loadPhoto: ko.observable(false),
        query: ko.observable(''),
        showFilters: ko.observable(false),
        name: ko.observable(''),
        diafragm: ko.observable(0),
        iso: ko.observable(0),
        focusDist: ko.observable(0),
        flash: ko.observable(false),
        location: ko.observable(''),
        shutter: ko.observable(0),
        deviceManufacturer: ko.observable(),
        deviceModel: ko.observable(),
        addDate: ko.observable(''),
        cameraModels: ko.observableArray([]),
        selectedModels: ko.observableArray([]),
        deviceManufacturers: ko.observableArray([]),
        activate: function () {
            var self = this;
            security.checkAuth().done(function (result) {
                if (!result) {
                    router.navigate("log_in");
                }
                self.albumPhotos.removeAll();
                app.on("search", function (params) {
                    self.query(params.query);
                    if (params.query == null) {
                        params.query = "";
                    }
                    self.query(params.query);
                    self.showFilters(params.showFilters);
                    if (!params.showFilters) {
                        self.searchQ(params);
                    }
                });
                self.searchQ = function(params) {
                    album.search(params.query, params.albumName).done(function (res) {
                        self.albumPhotos.removeAll();
                        if (res != null && res.length > 0) {
                            self.loadPhoto(true);
                            self.showNoData(false);
                            for (var i = 0; i < res.length; i++) {
                                var item = res[i];
                                item.link = 'album/' + item.albumName + '/photo/' + item.id;
                                item.imgLink = 'data:image/jpeg;base64,' + item.thumb;
                                item.imgLinkFull = 'data:image/jpeg;base64,' + item.image;
                                item.addDate = item.addDate.split('T')[0] + " " + item.addDate.split('T')[1].substr(1, 4);
                                item.likes = ko.observable(item.likes);
                                item.active = (i === 0);
                                self.albumPhotos.push(item);
                            }
                        } else {
                            self.showNoData(true);
                            self.loadPhoto(false);
                            notify.info("Nothing found");
                        }
                    });
                },
                self.adSearch = function (params) {
                    album.advancedSearch(params).done(function (res) {
                        self.albumPhotos.removeAll();
                        if (res != null && res.length > 0) {
                            self.showNoData(false);
                            self.loadPhoto(true);
                            for (var i = 0; i < res.length; i++) {
                                var item = res[i];
                                item.link = 'album/' + item.albumName + '/photo/' + item.id;
                                item.imgLink = 'data:image/jpeg;base64,' + item.thumb;
                                item.imgLinkFull = 'data:image/jpeg;base64,' + item.image;
                                item.addDate = item.addDate.split('T')[0] + " " + item.addDate.split('T')[1].substr(1, 4);
                                item.likes = ko.observable(item.likes);
                                item.active = (i === 0);
                                self.albumPhotos.push(item);
                            }
                            console.log(self.loadPhoto());
                        } else {
                            self.showNoData(true);
                            self.loadPhoto(false);
                            notify.info("Nothing found");
                        }
                    });
                },
                app.on("advancedSearch", function(params) {
                    self.adSearch(params);
                });
            });
            photos.getManuFacturers().done(function(res) {
                if (res.length > 0) {
                    self.deviceManufacturers.removeAll();
                    self.cameraModels.removeAll();
                    for (var i = 0; i < res.length; i++) {
                        var manufacturer = { id: res[i].id, name: res[i].name, models: res[i].models };
                        self.deviceManufacturers.push(manufacturer);
                        for (var j = 0; j < manufacturer.models.length; j++) {
                            var model = { manufacturer: manufacturer.id, name: manufacturer.models[j].name, id: manufacturer.models[j].id };
                            self.cameraModels.push(model);
                        }
                    }
                }
            });
            self.selectedModels = ko.dependentObservable(function () {
                if (self.deviceManufacturer()) {
                    var dev = self.deviceManufacturer().id;
                    if (dev) {
                        return ko.utils.arrayFilter(self.cameraModels(), function (item) {
                            return item.manufacturer === dev;
                        });
                    }
                }
                return [];
            });
            self.showNoData = function(show) {
                if (show) {
                    $("#nodata").show();
                } else {
                    $("#nodata").hide();
                }
            };

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
            $("#fdate").datepicker().on('changeDate', function(ev) {
                $(this).datepicker('hide');
            });

        },

        likePhoto: function (item) {
            item.likes = item.likes + 1;
            photos.likeDislikePhotos(item.id, item.likes).done(function (res) {
            });
        },
        unlikePhoto: function (item) {
            item.likes = item.likes - 1;
            photos.likeDislikePhotos(item.id, item.likes).done(function (res) {
            });
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
            if (self.query() == null) {
                self.query("");
            }
            self.searchQ({ query: self.query() });
        },
        advancedSearch: function() {
            var self = this;
            self.showFilters(true);
        },
        advancedFilter: function () {
            var self = this;
            var params = {
                name: self.name(),
                date: new Date(self.addDate()),
                location: self.location(),
                deviceId: self.deviceManufacturer()? self.deviceManufacturer.id : 0,
                model: self.deviceModel(),
                focusDistance: self.focusDist(),
                diafragm: self.diafragm(),
                shutterSpeed: self.shutter(),
                ISO: self.iso(),
                isUsedFlash: self.flash()
            };
            self.adSearch(params);
        }
    };
});