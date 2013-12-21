namespace PhotoGalery.DTOs
{
	public class UserListItem
	{
		public int Id { get; set; }
		public string Role { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string CreatedAt { get; set; }
		public string LastActiveDate { get; set; }
		public bool IsLocked { get; set; }
		public bool IsActivated { get; set; }
		public string Password { get; set; }
	    public int PhotoLimit { get; set; }
        public int AlbumLimit { get; set; }
	}
}
