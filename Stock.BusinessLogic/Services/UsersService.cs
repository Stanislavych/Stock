﻿using Microsoft.EntityFrameworkCore;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;
using Stock.Models;
using Stock.Models.Models;

namespace Stock.BusinessLogic.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationContext _context;
        private readonly IPasswordHashService _passwordHash;
        public UsersService(ApplicationContext context, IPasswordHashService passwordHash)
        {
            _context = context;
            _passwordHash = passwordHash;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUserByName(string username)
        {
            return await Task.FromResult(_context.Users.FirstOrDefault(u => u.Username == username));
        }
        public async Task EditUserAsync(UserDto request, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
                throw new Exception("Пользователь не найден!");

            if (!string.Equals(user.Username, request.Username, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (existingUser != null)
                    throw new Exception("Пользователь с таким именем уже существует");
                user.Username = request.Username;
            }

            if (!string.IsNullOrEmpty(password))
            {
                _passwordHash.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            if (user.RoleId != request.RoleId)
                user.RoleId = request.RoleId;

            _context.Users.Update(user);
            _context.SaveChanges();

        }
        public async Task UpdatePassword(User user, string password, string newPassword)
        {
            if (!await _passwordHash.VerifyPasswordHashAsync(password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Неверный старый пароль!");
            _passwordHash.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
