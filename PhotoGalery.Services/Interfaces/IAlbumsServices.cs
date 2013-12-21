using System.Collections.Generic;
using PhotoGalery.DTOs;

namespace PhotoGalery.Services.Interfaces
{
	public interface IAlbumsServices
	{
		List<Album> GetAlbumsForUser(string email);
		Album GetAlbum(string name);
		int CreateAlbum(string name, string newName, string email);
		Enums.BasicStatus DeleteAlbum(string name);
		bool IsAlbumExist(string name);
	}
}