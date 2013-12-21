using System.Collections.Generic;

namespace PhotoGalery.Data.Models
{
	public class Manufacturer
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ICollection<Model> Models { get; set; }

		public Manufacturer()
		{
			Models = new HashSet<Model>();
		}
	}
}
