namespace PhotoGalery.DTOs
{
	public class Reset: IValidatable
	{
		public string Token { get; set; }
		public string Password { get; set; }

		public bool IsValid()
		{
			return true;
		}
	}
}
