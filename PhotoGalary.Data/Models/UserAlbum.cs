using System;

namespace PhotoGalery.Data.Models
{
	public class UserAlbum
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }

		public virtual User User { get; set; }
		public virtual Album Album { get; set; }
	}
}
