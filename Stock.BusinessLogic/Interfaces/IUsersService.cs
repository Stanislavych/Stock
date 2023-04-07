using Stock.Common.Dto;
using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
	public interface IUsersService
	{
		List<User> GetAllUsers();
		User GetUserByName(string username);
		Task EditUserAsync(UserDto request, string password);
		Task RemoveUserAsync(int userId);
	}
}
