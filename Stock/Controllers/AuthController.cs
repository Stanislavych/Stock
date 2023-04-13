using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;

namespace Stock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("register")]
        public IActionResult Register() => View();

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserDto request, [FromForm] string confirmPassword)
        {
            await _authService.RegisterAsync(request, confirmPassword);
            return RedirectToAction("login");
        }

        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserDto request)
        {
            string token = await _authService.LoginAsync(request);

            Response.Cookies.Append("jwt", token);

            return RedirectToAction("index", "item");
        }

        [HttpGet("logout"), Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return RedirectToAction("index", "item");
        }
    }
}
