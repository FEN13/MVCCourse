using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using PhotoGalery.Data;
using PhotoGalery.Services.Interfaces;
using Album = PhotoGalery.DTOs.Album;

namespace PhotoGalery.Services
{
    public class AlbumsServices : IAlbumsServices
    {
        private readonly IPhotoGaleryContextFactory _photoGaleryContextFactory;

        public AlbumsServices(IPhotoGaleryContextFactory photoGaleryContextFactory)
        {
            _photoGaleryContextFactory = photoGaleryContextFactory;
        }

        public List<Album> GetAlbumsForUser(string email)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var userAlbums = ctx.UserAlbums.Where(u => u.User.Email == email).Select(a => a.Album).Include("Photos").ToList();
                var result = Mapper.Map<List<Data.Models.Album>, List<Album>>(userAlbums);
                return result;
            }
        }

        public Album GetAlbum(string name)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                var album = ctx.Albums.Include("Photos").SingleOrDefault(a => a.Name == name);
                return Mapper.Map<Data.Models.Album, Album>(album);
            }

        }

        public int CreateAlbum(string name, string newName, string email)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                try
                {
                    var user = ctx.Users.SingleOrDefault(u => u.Email == email);
                    if (user == null) 
                        return -1;
                    int userAlbums = ctx.UserAlbums.Count(u => u.User.Id == user.Id);
                    if (user is Data.Models.RegularUser && userAlbums == (user as Data.Models.RegularUser).AlbumLimit)
                        return -1;

                    if (name == newName)
                    {
                        var album = new Data.Models.Album { Name = name, CoverId = 0 };
                        ctx.Albums.Add(album);
                        ctx.UserAlbums.Add(new Data.Models.UserAlbum { Album = album, CreatedAt = DateTime.Now, User = user });
                    }
                    else
                    {
                        var album = ctx.Albums.SingleOrDefault(a => a.Name == name);
                        if (album != null)
                        {
                            album.Name = newName;
                        }
                    }
                    ctx.SaveChanges();
                    return ctx.UserAlbums.Count(u => u.User.Id == user.Id);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public Enums.BasicStatus DeleteAlbum(string name)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                try
                {
                    var album = ctx.Albums.SingleOrDefault(a => a.Name == name);
                    if (album != null)
                    {
                        ctx.Albums.Remove(album);
                        var ua = ctx.UserAlbums.SingleOrDefault(a => a.Album.Id == album.Id);
                        ctx.UserAlbums.Remove(ua);
                        ctx.SaveChanges();
                        return Enums.BasicStatus.Success;
                    }
                    return Enums.BasicStatus.Failure;
                }
                catch (Exception)
                {
                    return Enums.BasicStatus.Failure;
                }
            }
        }

        public bool IsAlbumExist(string name)
        {
            using (var ctx = _photoGaleryContextFactory.Create())
            {
                return ctx.Albums.Any(a => a.Name == name);
            }
        }
    }
}
