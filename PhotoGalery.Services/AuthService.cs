using System;
using System.Linq;
using PhotoGalery.Data;
using PhotoGalery.Data.Models;
using PhotoGalery.DTOs;
using PhotoGalery.Services.Interfaces;
using User = PhotoGalery.Data.Models.User;

namespace PhotoGalery.Services
{
	public class AuthService: IAuthService
	{
		private readonly IPhotoGaleryContextFactory _photoGaleryContextFactory;

		public AuthService(IPhotoGaleryContextFactory photoGaleryContextFactory)
        {
			_photoGaleryContextFactory = photoGaleryContextFactory;
        }

		public Enums.UserResetPasswordStatus Reset(Reset resetDTO)
		{
			if (!resetDTO.IsValid())
			{
				return Enums.UserResetPasswordStatus.Failure;
			}
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				try
				{
					var token = ctx.Tokens.Include(Constants.User).Single(t => t.Value.Equals(resetDTO.Token, StringComparison.InvariantCulture));
					if (token == null)
					{
						return Enums.UserResetPasswordStatus.WrongCode;
					}

					if (!token.IsValid())
					{
						return Enums.UserResetPasswordStatus.Expired;
					}
					token.IsUsed = true;
					token.User.ChangePassword(resetDTO.Password);

					ctx.SaveChanges();

					return Enums.UserResetPasswordStatus.Success;
				}
				catch
				{
					return Enums.UserResetPasswordStatus.Failure;
				}
			}
		}

		public Enums.UserLoginStatus Login(Login loginDTO)
		{
			if (!loginDTO.IsValid())
			{
				return Enums.UserLoginStatus.Failure;
			}

			using (var ctx = _photoGaleryContextFactory.Create())
			{
				try
				{
					var user = ctx.Users.SingleOrDefault(u => u.Email.Equals(loginDTO.Email, StringComparison.InvariantCultureIgnoreCase));
					if (user == null || !user.CheckPassword(loginDTO.Password))
					{
						return Enums.UserLoginStatus.Failure;
					}

					if (!user.IsActivated)
					{
						return Enums.UserLoginStatus.NotActivated;
					}

					if (user.IsLocked)
					{
						return Enums.UserLoginStatus.Locked;
					}

					user.LastActiveDate = DateTime.UtcNow;
					ctx.SaveChanges();

					return Enums.UserLoginStatus.Success;
				}
				catch
				{
					return Enums.UserLoginStatus.Failure;
				}
			}
		}

		public Enums.UserRegistrationStatus Register(Register registerDto, out string activationToken)
		{
			activationToken = string.Empty;

			if (!registerDto.IsValid())
			{
				return Enums.UserRegistrationStatus.Failure;
			}

			using (var ctx = _photoGaleryContextFactory.Create())
			{
				try
				{
					var existingUser = ctx.Users.SingleOrDefault(u => u.Email.Equals(registerDto.Email));
					if (existingUser != null)
					{
						return Enums.UserRegistrationStatus.AlreadyExists;
					}
					var newUser = !string.IsNullOrEmpty(registerDto.CreditCardNumber) ? new User() : new RegularUser();
					newUser.FirstName = registerDto.FirstName;
					newUser.LastName = registerDto.LastName;
					newUser.Email = registerDto.Email;
					newUser.ChangePassword(registerDto.Password);
					newUser.CreatedAt = DateTime.UtcNow;
					newUser.LastActiveDate = DateTime.UtcNow;
					newUser.IsLocked = false;
					newUser.IsActivated = false;
					newUser.Role = ctx.Roles.SingleOrDefault(r => r.Name == "User");

					var newUserActivationToken = Token.Generate();
					newUser.Tokens.Add(newUserActivationToken);

					ctx.Users.Add(newUser);
					ctx.SaveChanges();

					activationToken = newUserActivationToken.Value;

					return Enums.UserRegistrationStatus.Success;
				}
				catch
				{
					return Enums.UserRegistrationStatus.Failure;
				}
			}
		}

		public Enums.BasicStatus Forgot(ForgotPass forgotDTO, out DTOs.User user, out string resetToken)
		{
			user = new DTOs.User();
			resetToken = string.Empty;

			if (!forgotDTO.IsValid())
			{
				return Enums.BasicStatus.Failure;
			}
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				try
				{
					User userToRecover = ctx.Users.Single(u => u.Email.Equals(forgotDTO.Email, StringComparison.InvariantCultureIgnoreCase));
					if (userToRecover == null)
					{
						return Enums.BasicStatus.Failure;
					}
					if (userToRecover.IsLocked || !userToRecover.IsActivated)
					{
						return Enums.BasicStatus.Failure;
					}
					var passwordResetToken = Token.Generate(true);
					userToRecover.Tokens.Add(passwordResetToken);

					ctx.SaveChanges();

					user.Id = userToRecover.Id;
					user.Email = userToRecover.Email;
					user.FullName = GetFullName(userToRecover);
					resetToken = passwordResetToken.Value;

					return Enums.BasicStatus.Success;
				}
				catch
				{
					return Enums.BasicStatus.Failure;
				}
			}
		}

		public Enums.UserResetPasswordStatus Activate(Activate activateDTO, out DTOs.User user)
		{
			user = new DTOs.User();
			if (!activateDTO.IsValid())
			{
				return Enums.UserResetPasswordStatus.Failure;
			}
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				Token token;
				try
				{
					token = ctx.Tokens.Include("User").Single(t => t.Value.Equals(activateDTO.Token, StringComparison.InvariantCulture));
				}
				catch (Exception)
				{
					return Enums.UserResetPasswordStatus.Failure;
				}

				if (token == null)
				{
					return Enums.UserResetPasswordStatus.WrongCode;
				}

				if (!token.IsValid())
				{
					return Enums.UserResetPasswordStatus.Expired;
				}

				try
				{
					token.IsUsed = true;
					token.User.IsActivated = true;

					ctx.SaveChanges();

					user.Id = token.User.Id;
					user.Email = token.User.Email;
					user.FullName = string.Format("{0} {1}", token.User.FirstName, token.User.LastName);

					return Enums.UserResetPasswordStatus.Success;
				}
				catch (Exception)
				{
					return Enums.UserResetPasswordStatus.Failure;
				}
			}
		}

		private string GetFullName(User user)
		{
			return user.FirstName + " " + user.LastName;
		}
	}
}
