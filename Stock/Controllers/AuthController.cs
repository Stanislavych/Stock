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
        public async Task<IActionResult> RegisterPost([FromForm] UserDto request)
        {
            await _authService.Register(request);
            return RedirectToAction("Index", "Item");
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
            string token = await _authService.Login(request);

            // сохраняем токен в куки
            Response.Cookies.Append("jwt", token);
            return RedirectToAction("Index", "Item");
        }
    }
}
