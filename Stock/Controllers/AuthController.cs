using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;

namespace Stock.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] UserDto request)
        {
            await _authService.RegisterAsync(request);
            return RedirectToAction("Login", "Auth");
        }
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromForm] UserDto request)
        {
            string token = await _authService.LoginAsync(request);

            // сохраняем токен в куки
            Response.Cookies.Append("jwt", token);
            return RedirectToAction("Index", "Item");
        }
        public IActionResult Logout()
        {
            // Удаляем куки с токеном
            Response.Cookies.Delete("jwt");

            return RedirectToAction("Index", "Item");
        }
    }
}
