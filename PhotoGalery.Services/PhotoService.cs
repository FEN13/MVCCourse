using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using AutoMapper;
using ExifReader;
using PhotoGalery.Data;
using PhotoGalery.DTOs;
using PhotoGalery.Services.Interfaces;
using PhotoManager.Core.Helpers.Media;
using Photo = PhotoGalery.DTOs.Photo;

namespace PhotoGalery.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoGaleryContextFactory _photoGaleryContextFactory;

        public PhotoService(IPhotoGaleryContextFactory photoGaleryContextFactory)
        {
            _photoGaleryContextFactory = photoGaleryContextFactory;
        }

        public Photo GetPhoto(int id)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var photo = ctx.Photos.Include("Device").SingleOrDefault(p => p.Id == id);
                return Mapper.Map<Data.Models.Photo, Photo>(photo);
            }
        }

        public List<Photo> GetPhotosForAlbum(string albumName)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var albumPhotos = ctx.Albums.Include("Photos").SingleOrDefault(a => a.Name == albumName);
                if (albumPhotos == null)
                    return null;

                var photos = albumPhotos.Photos.ToList();
                return Mapper.Map<List<Data.Models.Photo>, List<Photo>>(photos);
            }
        }

        public int CreatePhoto(UploadImage image, string email)
        {
            var imu = new ImageMetadataUtility(image.ImageData);
            var diafragm = imu.GetMetadata(PropertyId.ExifFNumber);
            var iso = imu.GetMetadata(PropertyId.ExifISOSpeed);
            var focusDist = imu.GetMetadata(PropertyId.ExifFocalLength);
            var flash = imu.GetMetadata(PropertyId.ExifFlash);
            var location = imu.GetMetadata(PropertyId.ExifSubjectLoc);
            var sutter = imu.GetMetadata(PropertyId.ExifShutterSpeed);
            var deviceManufacturer = imu.GetMetadata(PropertyId.CameraMake);
            var cameraModel = imu.GetMetadata(PropertyId.CameraModel);

            try
            {
                using (var ctx = _photoGaleryContextFactory.Create())
                {
                    var user = ctx.Users.SingleOrDefault(u => u.Email == email);
                    if (user is Data.Models.RegularUser && user.Albums.Count == (user as Data.Models.RegularUser).PhotoLimit)
                    {
                        return -1;
                    }
                    var newPhoto = new Data.Models.Photo
                    {
                        Name = image.ImageName,
                        AddDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        Description = string.Empty,
                        Diafragm = !string.IsNullOrEmpty(diafragm) ? int.Parse(diafragm.Replace("F/", string.Empty)) : 0,
                        FocusDistance = !string.IsNullOrEmpty(focusDist) ? int.Parse(focusDist) : 0,
                        ISO = !string.IsNullOrEmpty(iso) ? int.Parse(iso.Replace("ISO-", string.Empty)) : 0,
                        Image = image.ImageData,
                        PreviewSize = Utils.GenerateImage(image.ImageData, 300),
                        Thumb = Utils.GenerateImage(image.ImageData, 150),
                        IsFlashUsed = !string.IsNullOrEmpty(flash) && int.Parse(flash) != 0,
                        Likes = 0,
                        Views = 0,
                        Location = !string.IsNullOrEmpty(location) ? location : string.Empty,
                        ShutterSpeed = !string.IsNullOrEmpty(sutter) ? float.Parse(sutter) : 0
                    };

                    if (!string.IsNullOrEmpty(deviceManufacturer) && !string.IsNullOrEmpty(cameraModel))
                    {
                        var manufacturer = ctx.Manufacturers.SingleOrDefault(dm => dm.Name == deviceManufacturer);
                        if (manufacturer == null)
                        {
                            ctx.Manufacturers.AddOrUpdate(new Data.Models.Manufacturer { Name = deviceManufacturer });
                            ctx.SaveChanges();
                            manufacturer = ctx.Manufacturers.SingleOrDefault(m => m.Name == deviceManufacturer);

                        }
                        var model = ctx.Models.SingleOrDefault(dm => dm.Name == cameraModel.Replace(deviceManufacturer, string.Empty));
                        if (model == null)
                        {
                            ctx.Models.AddOrUpdate(new Data.Models.Model { Name = cameraModel.Replace(deviceManufacturer, string.Empty) });
                            ctx.SaveChanges();
                            model = ctx.Models.SingleOrDefault(m => m.Name == cameraModel.Replace(deviceManufacturer, string.Empty));

                        }
                        var device = ctx.Devices.SingleOrDefault(d => d.Manufacturer.Id == manufacturer.Id && d.Model.Id == model.Id);
                        if (device == null)
                        {
                            device = new Data.Models.Device { Manufacturer = manufacturer, Model = model };
                            ctx.Devices.AddOrUpdate(device);
                            ctx.SaveChanges();
                        }
                        newPhoto.Device = ctx.Devices.SingleOrDefault(d => d.Manufacturer.Id == manufacturer.Id && d.Model.Id == model.Id);
                    }
                    var album = ctx.Albums.SingleOrDefault(a => a.Name == image.Album);

                    if (album == null)
                        return -1;

                    album.Photos.Add(newPhoto);
                    ctx.SaveChanges();
                    return album.Photos.Count();
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int DeletePhoto(int imageId)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var photo = ctx.Photos.SingleOrDefault(i => i.Id == imageId);
                if (photo == null)
                    return -1;

                var alb = photo.Album;
                ctx.Photos.Remove(photo);

                if (ctx.SaveChanges() <= 0)
                    return -1;


                return alb.Photos.Count();
            }
        }

        public Enums.BasicStatus LikePhoto(int imageId, Enums.LikeAction action)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var photo = ctx.Photos.SingleOrDefault(i => i.Id == imageId);
                if (photo == null)
                    return Enums.BasicStatus.Failure;

                switch (action)
                {
                    case Enums.LikeAction.Dislike:
                        photo.Likes = photo.Likes - 1;
                        break;
                    case Enums.LikeAction.Like:
                        photo.Likes = photo.Likes + 1;
                        break;
                }
                return ctx.SaveChanges() > 0 ? Enums.BasicStatus.Success : Enums.BasicStatus.Failure;
            }
        }

        public Enums.BasicStatus SaveMetadata(PhotoMetadata photo)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var ph = ctx.Photos.SingleOrDefault(i => i.Id == photo.Id);
                if (photo == null || ph == null)
                    return Enums.BasicStatus.Failure;

                ph.Name = photo.Name;
                ph.Description = photo.Description;
                if (photo.DeviceManufacturer != -1 && photo.DeviceModel != -1)
                {
                    var device = ctx.Devices.FirstOrDefault(d => d.Manufacturer.Id == photo.DeviceManufacturer && d.Model.Id == photo.DeviceModel);
                    if (device != null)
                    {
                        ph.Device = device;
                    }
                    else
                    {
                        var manuf = ctx.Manufacturers.SingleOrDefault(m => m.Id == photo.DeviceManufacturer);
                        var model = ctx.Models.SingleOrDefault(m => m.Id == photo.DeviceModel);
                        if (model != null && manuf != null)
                        {
                            device = new Data.Models.Device { Manufacturer = manuf, Model = model };
                            ph.Device = device;
                        }
                    }
                }

                ph.FocusDistance = photo.FocusDistance;
                ph.Diafragm = photo.Diafragm;
                ph.ISO = photo.ISO;
                ph.IsFlashUsed = photo.IsFlashUsed;
                ph.Location = photo.Location;
                ph.ShutterSpeed = photo.ShutterSpeed;
                ctx.Photos.AddOrUpdate(ph);
                if (photo.IsCover)
                {
                    var alb = ctx.Albums.SingleOrDefault(a => a.Name == photo.AlbumName);
                    if (alb != null)
                    {
                        alb.CoverId = ph.Id;
                    }
                }
                if (ctx.SaveChanges() > 0)
                {
                    return Enums.BasicStatus.Success;
                }
                return Enums.BasicStatus.Failure;
            }
        }

        public List<Device> GetDevices()
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var devices = ctx.Devices.ToList();
                return Mapper.Map<List<Data.Models.Device>, List<Device>>(devices);
            }
        }

        public List<Manufacturer> GetManufacturers()
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var manufacturers = ctx.Manufacturers.Include("Models").ToList();
                return Mapper.Map<List<Data.Models.Manufacturer>, List<Manufacturer>>(manufacturers);
            }
        }

        public bool IsCover(string album, int photoId)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var alb = ctx.Albums.SingleOrDefault(a => a.Name == album);
                return alb != null && alb.CoverId == photoId;
            }
        }

        public List<Photo> GetPhotosByName(string name, string currentAlbum)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                if (string.IsNullOrWhiteSpace(name))
                    return !string.IsNullOrWhiteSpace(currentAlbum) ? GetPhotosForAlbum(currentAlbum) : new List<Photo>();

                var photos = ctx.Photos.Where(p => p.Name.Contains(name)).ToList();
                return Mapper.Map<List<Data.Models.Photo>, List<Photo>>(photos);
            }
        }

        public List<Photo> AdvancedSearch(AdvancedSearch searchParams, string email)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                string queryString = string.Format("select *, us.Id, ua.Album_Id from Photo ph join [User] us on us.Email = '{0}' join UserAlbum ua on us.Id = ua.User_Id where ua.Album_Id= ph.Album_Id", email);
                var sb = new StringBuilder(queryString);
                if (!string.IsNullOrWhiteSpace(searchParams.Name))
                {
                    sb.Append(string.Format(" And ph.Name='{0}'", searchParams.Name));
                }
                if (searchParams.IsUsedFlash)
                {
                    sb.Append(string.Format(" And ph.IsFlashUsed={0}", searchParams.IsUsedFlash ? 1 : 0));
                }
                if (searchParams.Diafragm != 0)
                {
                    sb.Append(string.Format(" And ph.Diafragm={0}", searchParams.Diafragm));
                }
                if (searchParams.FocusDistance != 0)
                {
                    sb.Append(string.Format(" And ph.FocusDistance={0}", searchParams.FocusDistance));
                }
                if (searchParams.ISO != 0)
                {
                    sb.Append(string.Format(" And ph.ISO={0}", searchParams.ISO));
                }
                if (searchParams.ShutterSpeed != 0)
                {
                    sb.Append(string.Format(" And ph.ShutterSpeed={0}", searchParams.ShutterSpeed));
                }
                if (!string.IsNullOrWhiteSpace(searchParams.Location))
                {
                    sb.Append(string.Format(" And ph.Location='{0}'", searchParams.Location));
                }
                if (searchParams.Date != DateTime.MinValue)
                {
                    sb.Append(string.Format(" And ph.AddDate='{0}'", searchParams.Date));
                }
                if (searchParams.DeviceId != 0 && searchParams.Model != 0)
                {
                    var dev = ctx.Devices.SingleOrDefault(d => d.Manufacturer.Id == searchParams.DeviceId && d.Model.Id == searchParams.Model);
                    if (dev != null)
                    {
                        sb.Append(string.Format(" And ph.Name={0}", dev.Id));
                    }
                }
                var query = ctx.Database.SqlQuery<Data.Models.Photo>(sb.ToString());
                return Mapper.Map<List<Data.Models.Photo>, List<Photo>>(query.ToList());
            }
        }
    }
}
