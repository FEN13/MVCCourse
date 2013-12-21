using System;

namespace PhotoGalery.Data.Models
{
	public class Photo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreateDate { get; set; }
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
	   
	    public virtual Device Device { get; set; }
	    public virtual Album Album { get; set; }
	}
}
