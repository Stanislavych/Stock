using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
	public interface IUsersService
	{
		List<User> GetAllUsers();
	}
}
