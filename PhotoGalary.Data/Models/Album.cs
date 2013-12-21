using System.Collections.Generic;

namespace PhotoGalery.Data.Models
{
	public class Album
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CoverId { get; set; }

		public ICollection<Photo> Photos { get; set; }

		public Album()
		{
			Photos = new HashSet<Photo>();
		}
	}
}
