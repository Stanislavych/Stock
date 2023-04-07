using Stock.Common.Dto;
using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserDto request);
        Task<string> LoginAsync(UserDto request);
        Task RegisterAdminAsync(UserDto request);

    }
}
