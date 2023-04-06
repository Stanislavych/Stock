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
        public List<Item> GetUserItems(int userId)
        {
            var userItems = _context.Items.Where(item => item.UserId == userId).ToList();
            return userItems;
        }
        public async Task AddItemAsync(Item item, User user)
        {
            item.UserId = user.Id;
            item.ReceiptDate = DateTime.Now;
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }
        public async Task EditItemAsync(Item item, User user)
        {
            var currentItem = await _context.Items.FindAsync(item.Id);
            item.ReceiptDate = DateTime.Now;
            if (currentItem == null)
                throw new Exception("Предмет не найден");

            if (item.UserId != user.Id)
                throw new Exception("Вы не можете изменить чужой предмет");
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveItemAsync(int itemId, User user)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
                throw new Exception("Предмет не найден");

            if (item.UserId != user.Id)
                throw new Exception("Вы не можете удалить чужой предмет");

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
