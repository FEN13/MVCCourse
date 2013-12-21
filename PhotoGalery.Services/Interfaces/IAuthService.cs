using PhotoGalery.DTOs;

namespace PhotoGalery.Services.Interfaces
{
	public interface IAuthService
	{
		Enums.UserResetPasswordStatus Reset(Reset resetDTO);
		Enums.UserLoginStatus Login(Login loginDTO);
		Enums.UserRegistrationStatus Register(Register registerDto, out string activationToken);
		Enums.BasicStatus Forgot(ForgotPass forgotDTO, out User user, out string resetToken);
		Enums.UserResetPasswordStatus Activate(Activate activateDTO, out User user);
	}
}
