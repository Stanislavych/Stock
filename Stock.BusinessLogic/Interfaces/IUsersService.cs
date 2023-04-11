using Stock.Common.Dto;
using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
	public interface IUsersService
	{
		Task<List<User>> GetAllUsers();
		Task<User> GetUserByName(string username);
		Task EditUserAsync(UserDto request, string password);
		Task RemoveUserAsync(int userId);
		Task UpdatePassword(User user, string password, string newPassword);
	}
}
