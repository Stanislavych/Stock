using Stock.BusinessLogic.Interfaces;
using Stock.Models;
using Stock.Models.Models;

namespace Stock.BusinessLogic.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationContext _context;
        public ItemService(ApplicationContext context)
        {
            _context = context;
        }
        public List<Item> GetAllItems()
        {
            var items = _context.Items.ToList();
            return items;
        }
    }
}
