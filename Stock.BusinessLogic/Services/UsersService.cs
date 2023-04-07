using Microsoft.EntityFrameworkCore;
using Stock.BusinessLogic.Interfaces;
using Stock.Common.Dto;
using Stock.Models;
using Stock.Models.Models;
using System.Security.Cryptography;
using System.Text;

namespace Stock.BusinessLogic.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationContext _context;
        public UsersService(ApplicationContext context)
        {
            _context = context;
        }
        public List<User> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }
        public User GetUserByName(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        public async Task EditUserAsync(UserDto request, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
            {
                throw new Exception("Пользователь не найден!");
            }

            if (!string.Equals(user.Username, request.Username, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (existingUser != null)
                {
                    throw new Exception("Пользователь с таким именем уже существует");
                }
                user.Username = request.Username;
            }

            if (!string.IsNullOrEmpty(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            if (user.RoleId != request.RoleId)
            {
                user.RoleId = request.RoleId;
            }

            _context.Users.Update(user);
            _context.SaveChanges();

        }
        public async Task RemoveUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
