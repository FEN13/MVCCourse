namespace PhotoGalery.DTOs
{
	public class Register:IValidatable
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string CreditCardNumber { get; set; }

		public bool IsValid()
		{
			return true;
		}
	}
}
