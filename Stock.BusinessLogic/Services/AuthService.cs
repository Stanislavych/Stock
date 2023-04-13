using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;
using Stock.Models;
using Stock.Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Stock.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _context;
        private readonly IPasswordHashService _passwordHash;

        public AuthService(IConfiguration configuration, ApplicationContext context, IPasswordHashService passwordHash)
        {
            _configuration = configuration;
            _context = context;
            _passwordHash = passwordHash;
        }

        public async Task RegisterAsync(UserDto request, string confirmPassword)
        {
            var existingUserByName = await _context.Users.AnyAsync(u => u.Username == request.Username);
            if (existingUserByName)
                throw new Exception("Пользователь с таким именем уже существует");

            var existingUserByEmail = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (existingUserByEmail)
                throw new Exception("Пользователь с такой почтой уже существует");

            if (request.Password != confirmPassword)
                throw new Exception("Пароли не совпадают!");

            _passwordHash.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = request.Email,
                RoleId = 2
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task RegisterAdminAsync(UserDto request)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Username == request.Username);
            if (existingUser)
                throw new Exception("Пользователь с таким именем уже существует");

            _passwordHash.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User admin = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 1
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(UserDto request)
        {
            User user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                throw new Exception("Пользователь не найден!");
            if (!await _passwordHash.VerifyPasswordHashAsync(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Неверный пароль!");
            string token = CreateToken(user);
            return token;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}

