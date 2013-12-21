using System.Collections.Generic;
using System.Web.Http;
using PhotoGalery.DTOs;
using PhotoGalery.Services;
using PhotoGalery.Services.Interfaces;

namespace PhotoGalery.Areas.Api.Controllers
{
    public class AlbumsController : ApiController
    {
	    private readonly IAlbumsServices _albumsServices;
		public AlbumsController(IAlbumsServices albumsServices)
		{
			_albumsServices = albumsServices;
		}

		[HttpGet]
	    public List<Album> GetAlbums(string email)
		{
			return _albumsServices.GetAlbumsForUser(email);
		}
		
		[HttpGet]
	    public Album GetAlbum(string albumName)
	    {
		    return _albumsServices.GetAlbum(albumName);
	    }

		[HttpPost]
		public int SaveAlbum(string albumName, string email, string newName)
	    {
			return _albumsServices.CreateAlbum(albumName, newName, email);
	    }

		[HttpGet]
	    public bool CheckAlbumName(string albumName)
	    {
		    return _albumsServices.IsAlbumExist(albumName);
	    }

		[HttpGet]
		public Enums.BasicStatus DeleteAlbum(string albumName)
		{
			return _albumsServices.DeleteAlbum(albumName);
		}
    }
}
