namespace PhotoGalery.DTOs
{
	public class ForgotPass: IValidatable
    {
        public string Email { get; set; }
        public bool IsValid()
        {
            return true;
        }
    }
	
}
