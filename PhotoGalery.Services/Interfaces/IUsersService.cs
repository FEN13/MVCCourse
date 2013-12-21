using System.Collections.Generic;
using PhotoGalery.DTOs;
using Role = PhotoGalery.DTOs.Role;

namespace PhotoGalery.Services.Interfaces
{
	public interface IUsersService
	{
		List<UserListItem> GetAllUsers();
		Enums.BasicStatus ChangeActivationStatus(Block dto);
		UserListItem GetUser(string email);
		List<Role> GetRoles();
		bool IsEmailExists(string email);
	}
}