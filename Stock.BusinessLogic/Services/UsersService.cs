using Stock.BusinessLogic.Interfaces;
using Stock.Models;
using Stock.Models.Models;

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
    }
}
