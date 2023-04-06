using Stock.Common.Dto;
using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<User> Register(UserDto request);
        Task<string> Login(UserDto request);
    }
}
