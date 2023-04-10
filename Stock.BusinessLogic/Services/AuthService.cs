﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;
using Stock.Models;
using Stock.Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Stock.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _context;

        public AuthService(IConfiguration configuration, ApplicationContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task RegisterAsync(UserDto request, string confirmPassword)
        {
            var existingUserByName = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUserByName != null)
                throw new Exception("Пользователь с таким именем уже существует");
            var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUserByEmail != null)
                throw new Exception("Пользователь с такой почтой уже существует");
            if (request.Password != confirmPassword)
                throw new Exception("Пароли не совпадают!");
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Email = request.Email;
            user.RoleId = 2;
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public async Task RegisterAdminAsync(UserDto request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
            {
                throw new Exception("Пользователь с таким именем уже существует");
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User admin = new User();
            admin.Username = request.Username;
            admin.PasswordHash = passwordHash;
            admin.PasswordSalt = passwordSalt;
            admin.RoleId = 1;
            _context.Users.Add(admin);
            _context.SaveChanges();

        }

        public async Task<string> LoginAsync(UserDto request)
        {
            User user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                throw new Exception("Пользователь не найден!");
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Неверный пароль!");
            }
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}

