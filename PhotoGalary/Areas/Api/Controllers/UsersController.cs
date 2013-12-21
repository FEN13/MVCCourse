using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PhotoGalery.DTOs;
using PhotoGalery.Services;
using PhotoGalery.Services.Interfaces;

namespace PhotoGalery.Areas.Api.Controllers
{
    public class UsersController : ApiController
    {
		 private readonly IUsersService _userService;

		 public UsersController(IUsersService userListService)
        {
            _userService = userListService;
        }

        [HttpGet]
        public IList<UserListItem> Index()
        {
	        var users = _userService.GetAllUsers();
	        var user =users.First(u=>u.Email == User.Identity.Name);
	        users.Remove(user);
            return users;
        }

		[HttpGet]
	    public UserListItem GetUser(string email)
	    {
		    return _userService.GetUser(email.Trim());
	    }

		[HttpGet]
		public List<Role> GetRoles()
		{
			return _userService.GetRoles();
		}

		[HttpGet]
		public bool CheckEmail(string email)
		{
			return _userService.IsEmailExists(email.Trim());
		}
    }
}
