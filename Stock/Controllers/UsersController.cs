using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;

namespace Stock.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet("allUsers"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _usersService.GetAllUsers();
            return View(users);
        }
        [HttpPost("editUser"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser([FromForm] UserDto request, [FromForm] string password)
        {
            await _usersService.EditUserAsync(request, password);
            return RedirectToAction("AllUsers", "Users");
        }
        [HttpPost("removeUser"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUser([FromForm] int userId)
        {
            await _usersService.RemoveUserAsync(userId);
            return RedirectToAction("AllUsers", "Users");
        }
        [HttpPost("changePassword"), Authorize]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto model)
        {
            if (model.NewPassword != model.ConfirmPassword)
                throw new Exception("Новые пароли не совпадают!");
            var user = await _usersService.GetUserByName(User.Identity.Name);
            _usersService.UpdatePassword(user, model.OldPassword, model.NewPassword);
            return RedirectToAction("Index", "Item");
        }
    }
}
