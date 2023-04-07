using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;

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
		[HttpPost]
		[Route("editUser")]
		public async Task<ActionResult<string>> EditUser([FromForm] UserDto request, [FromForm] string password)
		{
			await _usersService.EditUserAsync(request, password);
			return RedirectToAction("AllUsers", "Users");
		}
		[HttpPost]
		[Route("removeUser")]
		public async Task<IActionResult> RemoveUser([FromForm] int userId)
		{
			var username = User.Identity.Name;
			var user = _usersService.GetUserByName(username);
			await _usersService.RemoveUserAsync(userId);
			return RedirectToAction("AllUsers", "Users");
		}
	}
}
