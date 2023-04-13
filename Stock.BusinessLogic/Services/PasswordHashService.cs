using Stock.BusinessLogic.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Stock.BusinessLogic.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<bool> VerifyPasswordHashAsync(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            return await Task.Run(() =>
            {
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return computedHash.SequenceEqual(passwordHash);
                }
            });
        }
    }
}
