using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Http;
using PhotoGalery.Data;
using PhotoGalery.DTOs;
using PhotoGalery.Services;
using PhotoGalery.Services.Interfaces;

namespace PhotoGalery.Areas.Api.Controllers
{
    public class PhotoController : ApiController
    {
        private readonly IPhotoService _photoService;
        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        public Photo GetPhoto(int photoId)
        {
            var photo = _photoService.GetPhoto(photoId);
            photo.IsCover = _photoService.IsCover(photo.AlbumName, photoId);
            return photo;
        }

        [HttpGet]
        public List<Photo> GetPhotos(string albumName)
        {
            return _photoService.GetPhotosForAlbum(albumName);
        }

        [HttpGet]
        public bool CheckMime(string mimeType, int fileSize)
        {
            return (mimeType == Constants.Jpeg || mimeType == Constants.Jpg) && fileSize <= 512000;
        }

        [HttpPost]
        public int UploadImage(UploadImage model)
        {
            try
            {
                using (var ms = new MemoryStream(model.ImageData))
                {
                    Image img = Image.FromStream(ms);
                    ImageFormat format = img.RawFormat;
                    ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == format.Guid);
                    if (codec.MimeType == Constants.Jpeg || codec.MimeType == Constants.Jpg && ms.Length <= 512000)
                    {
                        return _photoService.CreatePhoto(model, User.Identity.Name);
                    }
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        [HttpPost]
        public Enums.BasicStatus SaveMetadata(PhotoMetadata photo)
        {
            return _photoService.SaveMetadata(photo);
        }

        [HttpPost]
        public int DeleteImage(int imageId)
        {
            return _photoService.DeletePhoto(imageId);
        }

        [HttpPost]
        public Enums.BasicStatus Likes(int imageId, int act)
        {
            return act > 1 ? Enums.BasicStatus.Failure : _photoService.LikePhoto(imageId, (Enums.LikeAction)act);
        }

        [HttpGet]
        public List<Device> GetDevices()
        {
            return _photoService.GetDevices();
        }

        [HttpGet]
        public List<Manufacturer> GetManufacturers()
        {
            return _photoService.GetManufacturers();
        }

        [HttpPost]
        public List<Photo> GetPhotosByName(string name, string currentAlbum)
        {
            return _photoService.GetPhotosByName(name, currentAlbum);
        }

        [HttpPost]
        public List<Photo> AdvancedSearch(AdvancedSearch searchParams)
        {
            return _photoService.AdvancedSearch(searchParams, User.Identity.Name);
        }
    }
}
