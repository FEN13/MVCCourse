namespace PhotoGalery.Data.Models
{
	public class Model
	{
		public int Id { get; set; }
		public string Name { get; set; }

        public virtual Manufacturer Manufacturer { get; set; } 
	}
}
