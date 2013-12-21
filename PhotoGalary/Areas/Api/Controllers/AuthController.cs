using System.Web;
using System.Web.Http;
using System.Web.Security;
using PhotoGalery.DTOs;
using PhotoGalery.Mailer;
using PhotoGalery.Services;
using PhotoGalery.Services.Interfaces;

namespace PhotoGalery.Areas.Api.Controllers
{
    public class AuthController : ApiController
    {
		private readonly IAuthService _authenticationService;
		private readonly IMailer _userMailer;
		private readonly IUsersService _userService;

		public AuthController(IAuthService authenticationService, IMailer userMailer, IUsersService userListService)
		{
			_authenticationService = authenticationService;
			_userMailer = userMailer;
			_userService = userListService;
		}

		[HttpGet]
		public bool IsAuthenticated()
	    {
			return HttpContext.Current.Request.IsAuthenticated;
	    }

		[HttpPost]
		public object Login(Login dto)
		{
			var result = _authenticationService.Login(dto);

			var user = new UserListItem();
			if (result == Enums.UserLoginStatus.Success)
			{
				user = _userService.GetUser(dto.Email);
				FormsAuthentication.SetAuthCookie(dto.Email, true);
			}

            return new { result, role = user.Role, user.Email, user.PhotoLimit, user.AlbumLimit };
		}

		[HttpPost]
		public bool Logout()
		{
			if (HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session.Abandon();
			}
			FormsAuthentication.SignOut();
			return true;
		}

		[HttpPost]
		public Enums.UserRegistrationStatus Register(Register dto)
		{
			string activationToken;
			var status = _authenticationService.Register(dto, out activationToken);
			if (status == Enums.UserRegistrationStatus.Success && !string.IsNullOrEmpty(activationToken))
			{
				_userMailer.Activate(dto, activationToken).Send();
			}
			return status;
		}

		[HttpPost]
		public Enums.BasicStatus Forgot(ForgotPass dto)
		{
			DTOs.User user;
			string resetToken;
			var result = _authenticationService.Forgot(dto, out user, out resetToken);

			if (result == Enums.BasicStatus.Success)
			{
				_userMailer.PasswordReset(user, resetToken).Send();
			}

			return result;
		}

		[HttpPost]
		public Enums.UserResetPasswordStatus Reset(Reset dto)
		{
			var result = _authenticationService.Reset(dto);
			return result;
		}

		[HttpPost]
		public Enums.UserResetPasswordStatus Activate(Activate dto)
		{
			User user;
			var result = _authenticationService.Activate(dto, out user);

			if (result == Enums.UserResetPasswordStatus.Success)
			{
				_userMailer.Welcome(user).Send();
			}

			return result;
		}
    }
}
