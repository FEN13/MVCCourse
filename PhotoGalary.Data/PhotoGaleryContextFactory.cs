
namespace PhotoGalery.Data
{
	public class PhotoGaleryContextFactory : IPhotoGaleryContextFactory
	{
		public PhotoGaleryDataContext Create()
		{
			var ctx = new PhotoGaleryDataContext();
			return ctx;
		}
	}
}
