using Mvc.Mailer;
using PhotoGalery.DTOs;

namespace PhotoGalery.Mailer
{
	public interface IMailer
	{
		MvcMailMessage Welcome(User user);
		MvcMailMessage Activate(Register user, string activationToken);
		MvcMailMessage PasswordReset(User user, string resetToken);
	}
}