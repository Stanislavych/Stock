using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;
using Stock.Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Stock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            User user = await _authService.Register(request);

            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            string token = await _authService.Login(request);

            if (token.StartsWith("Ошибка:"))
            {
                return BadRequest(token);
            }

            return Ok(token);
        }
    }
}
