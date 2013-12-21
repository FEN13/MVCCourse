define(['plugins/http'], function (http) {
    var baseUri = window.AppRootPath;
    return {
        getPhotosForAlbum: function (albumName) {
            return http.get(baseUri + '/api/photo/getphotos', { albumName: encodeURIComponent(albumName) });
        },
        uploadPhoto: function (albumName, email, binaryData, imgName) {
            return http.post(baseUri + '/api/photo/uploadImage', { Album: encodeURIComponent(albumName), UserEmail: email, ImageData: binaryData, ImageName: imgName });
        },
        getPhoto: function (photoId) {
            return http.get(baseUri + '/api/photo/getphoto?photoId=' + photoId);
        },
        checkPhotoMimeType: function (mimeType, fileSize) {
            return http.get(baseUri + '/api/photo/checkMime', { mimeType: mimeType, fileSize: fileSize });
        },
        getImage: function (imgId) {
            return http.get(baseUri + '/api/photo/ImageFile', { imgId: imgId });
        },
        removePhoto: function (phid) {
            return http.post(baseUri + '/api/photo/deleteImage?imageId=' + phid);
        },
        likeDislikePhotos: function (itemid, action) {
            return http.post(baseUri + '/api/photo/Likes?imageId=' + itemid + '&act=' + action); //, { imageId: itemid, act: action });
        },
        savePhotoMetadata: function (photo) {
            return http.post(baseUri + '/api/photo/SaveMetadata', photo);
        },
        getManuFacturers: function () {
            return http.get(baseUri + '/api/photo/GetManufacturers');
        }
    };
});