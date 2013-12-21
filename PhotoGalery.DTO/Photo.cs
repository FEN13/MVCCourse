using System;

namespace PhotoGalery.DTOs
{
	public class Photo
	{
		public int Id { get; set; }
	    public string Name { get; set; }
		public DateTime AddDate { get; set; }
		public string Location { get; set; }
		public int FocusDistance { get; set; }
		public int Diafragm { get; set; }
		public float ShutterSpeed { get; set; }
		public int ISO { get; set; }
		public bool IsFlashUsed { get; set; }
		public byte[] Image { get; set; }
        public byte[] Thumb { get; set; }
        public byte[] PreviewSize { get; set; }
		public int Likes { get; set; }
		public int Views { get; set; }
		public string Description { get; set; }
		public string DeviceManufacturer { get; set; }
		public string DeviceModel { get; set; }
	    public bool IsCover { get; set; }
	    public string AlbumName { get; set; }
	}
}
