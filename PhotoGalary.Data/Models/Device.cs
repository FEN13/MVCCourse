namespace PhotoGalery.Data.Models
{
	public class Device
	{
		public int Id { get; set; }

		public virtual Manufacturer Manufacturer { get; set; }
		public virtual Model Model { get; set; }
	}
}
