using System.Collections.Generic;
using PhotoGalery.DTOs;

namespace PhotoGalery.Services.Interfaces
{
	public interface IPhotoService
	{
		Photo GetPhoto(int id);
		List<Photo> GetPhotosForAlbum(string albumName);
		int CreatePhoto(UploadImage image, string email);
	    int DeletePhoto(int imageId);
        Enums.BasicStatus LikePhoto(int imageId, Enums.LikeAction action);
        Enums.BasicStatus SaveMetadata(PhotoMetadata photo);
	    List<Device> GetDevices();
	    List<Manufacturer> GetManufacturers();
	    bool IsCover(string album, int photoId);
	    List<Photo> GetPhotosByName(string name, string currentAlbum);
	    List<Photo> AdvancedSearch(AdvancedSearch searchParams, string email);
	}
}