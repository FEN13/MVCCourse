namespace PhotoGalery.DTOs
{
	public class UploadImage
	{
		public string Album { get; set; }
		public string UserEmail { get; set; }
		public byte[] ImageData { get; set; }
		public string ImageName { get; set; }
	}
}
