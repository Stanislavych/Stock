using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Item>> GetFilteredItemsAsync(string name, DateTime? receiptDate, string manufacturer)
        {
            var filteredItems = _context.Items.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                filteredItems = filteredItems.Where(item => item.Name.Contains(name));

            if (receiptDate.HasValue)
            {
                var date = receiptDate.Value.Date;
                filteredItems = filteredItems.Where(item => item.ReceiptDate.Date == date);
            }

            if (!string.IsNullOrEmpty(manufacturer))
                filteredItems = filteredItems.Where(item => item.Manufacturer.Contains(manufacturer));

            return await filteredItems.ToListAsync();
        }
        public async Task<List<Item>> GetFilteredUserItemsAsync(int userId, string name, DateTime? receiptDate, string manufacturer)
        {
            var filteredUserItems = _context.Items.Where(item => item.UserId == userId).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                filteredUserItems = filteredUserItems.Where(item => item.Name.Contains(name));

            if (receiptDate.HasValue)
            {
                var date = receiptDate.Value.Date;
                filteredUserItems = filteredUserItems.Where(item => item.ReceiptDate.Date == date);
            }

            if (!string.IsNullOrEmpty(manufacturer))
                filteredUserItems = filteredUserItems.Where(item => item.Manufacturer.Contains(manufacturer));

            return await filteredUserItems.ToListAsync();
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
            if (currentItem == null)
                throw new Exception("Предмет не найден");

            if (user.RoleId != 1 && currentItem.UserId != user.Id || item.UserId != currentItem.UserId)
                throw new Exception("Вы не можете изменить этот предмет");

            item.ReceiptDate = DateTime.Now;
            _context.Entry(currentItem).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveItemAsync(int itemId, User user)
        {
            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
                throw new Exception("Предмет не найден");

            if (user.RoleId == 1 || item.UserId == user.Id)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception("Вы не можете удалить чужой предмет");
        }
    }
}
