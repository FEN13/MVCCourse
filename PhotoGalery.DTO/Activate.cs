namespace PhotoGalery.DTOs
{
	public class Activate : IValidatable
	{
		public string Token { get; set; }
		public bool IsValid()
		{
			return true;
		}
	}
}
