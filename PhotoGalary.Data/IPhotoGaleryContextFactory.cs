namespace PhotoGalery.Data
{
	public interface IPhotoGaleryContextFactory
	{
		PhotoGaleryDataContext Create();
	}
}
