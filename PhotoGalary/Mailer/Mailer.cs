using Mvc.Mailer;
using PhotoGalery.DTOs;

namespace PhotoGalery.Mailer
{
	public class Mailer : MailerBase, IMailer
	{
		public Mailer()
        {
            MasterName = "_Layout";
        }

        public MvcMailMessage Welcome(User user)
        {
            ViewBag.User = user;
            return Populate(x =>
            {
                x.Subject = "Welcome";
                x.ViewName = "Welcome";
                x.To.Add(user.Email);
            });
        }

        public MvcMailMessage Activate(Register user, string activationToken)
        {
            ViewBag.User = user;
            ViewBag.Token = activationToken;
            return Populate(x =>
            {
                x.Subject = "Activate";
                x.ViewName = "Activate";
                x.To.Add(user.Email);
            });
        }

        public MvcMailMessage PasswordReset(User user, string resetToken)
        {
            ViewBag.User = user;
            ViewBag.Token = resetToken;
            return Populate(x =>
            {
                x.Subject = "Password Reset";
                x.ViewName = "PasswordReset";
                x.To.Add(user.Email);
            });
        }
	}
}