namespace PhotoGalery.DTOs
{
	public class Login : IValidatable
	{
		public string Email { get; set; }
		public string Password { get; set; }

		public bool IsValid()
		{
			return true;
		}
	}
}
