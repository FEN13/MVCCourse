namespace PhotoGalery.Data.Models
{
	public class RegularUser: User
	{
		public int PhotoLimit { get; set; }
        public int AlbumLimit { get; set; }

		public RegularUser()
		{
			PhotoLimit = 30;
		    AlbumLimit = 5;
		}
	}
}
