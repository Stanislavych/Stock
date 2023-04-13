
namespace Stock.BusinessLogic.Interfaces
{
    public interface IPasswordHashService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task<bool> VerifyPasswordHashAsync(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
