define(['plugins/http'], function (http) {
    var baseUri = window.AppRootPath;
    return {
        getAllAlbums: function (email) {
            return http.get(baseUri + '/api/albums/getalbums', { email: email });
        },
        saveAlbum: function (albumName, albumNewName, email) {
            return http.post(baseUri + '/api/albums/saveAlbum?albumName=' + encodeURIComponent(albumName) + '&newName=' + encodeURIComponent(albumNewName) + '&email=' + email);
        },
        getAlbum: function (albumName) {
            return http.get(baseUri + '/api/albums/getalbum', { albumName: encodeURIComponent(albumName) });
        },
        checkalbumName: function (albumName) {
            return http.get(baseUri + '/api/albums/checkAlbumName', { albumName: encodeURIComponent(albumName) });
        },
        deleteAlbum: function (albumName) {
            return http.get(baseUri + '/api/albums/deleteAlbum', { albumName: encodeURIComponent(albumName) });
        },
        search: function (query, currentAlbum) {
            return http.post(baseUri + '/api/photo/GetPhotosByName?name=' + encodeURIComponent(query) + '&currentAlbum=' + encodeURIComponent(currentAlbum));
        },
        advancedSearch: function (searchParams) {
            return http.post(baseUri + '/api/photo/AdvancedSearch',  searchParams);
        }
    };
});