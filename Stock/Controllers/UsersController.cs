using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;

namespace Stock.Controllers
{
	public class UsersController : Controller
	{
		private readonly IUsersService _usersService;
		public UsersController(IUsersService usersService)
		{
			_usersService = usersService;
		}
		[HttpGet]
		//[Authorize(Roles ="Admin")]
		public IActionResult AllUsers()
		{
			var users = _usersService.GetAllUsers();
			return View(users);
		}
	}
}
