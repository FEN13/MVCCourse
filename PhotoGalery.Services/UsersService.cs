using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PhotoGalery.Data;
using PhotoGalery.Data.Models;
using PhotoGalery.DTOs;
using PhotoGalery.Services.Interfaces;
using Role = PhotoGalery.DTOs.Role;

namespace PhotoGalery.Services
{
	public class UsersService : IUsersService
	{
		private readonly IPhotoGaleryContextFactory _photoGaleryContextFactory;

		public UsersService(IPhotoGaleryContextFactory photoGaleryContextFactory)
        {
			_photoGaleryContextFactory = photoGaleryContextFactory;
        }

		public UserListItem GetUser(string email)
		{
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				var user = ctx.Users.Include(Constants.Role).FirstOrDefault(u => u.Email == email);
			    if (user is RegularUser)
			    {
                    return Mapper.Map<RegularUser, UserListItem>(user as RegularUser);
			    }
				return Mapper.Map<Data.Models.User, UserListItem>(user);
			}
		}

		public List<UserListItem> GetAllUsers()
		{
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				var users = ctx.Users.Include(Constants.Role).OrderByDescending(u => u.CreatedAt).ToList();
				var result = Mapper.Map<List<Data.Models.User>, List<UserListItem>>(users);
				return result;
			}
		}

		public List<Role> GetRoles()
		{
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				var roles = ctx.Roles.ToList();
				return Mapper.Map<List<Data.Models.Role>, List<Role>>(roles);
			}
		}

		public bool IsEmailExists(string email)
		{
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				return ctx.Users.Any(u => u.Email == email);
			}
		}

		public Enums.BasicStatus ChangeActivationStatus(Block dto)
		{
			using (var ctx = _photoGaleryContextFactory.Create())
			{
				try
				{
					Data.Models.User user = ctx.Users.Single(t => t.Id == dto.UserId);
					user.IsActivated = dto.Status;
					ctx.SaveChanges();
					return Enums.BasicStatus.Success;
				}
				catch
				{
					return Enums.BasicStatus.Failure;
				}
			}
		}
	}
}
