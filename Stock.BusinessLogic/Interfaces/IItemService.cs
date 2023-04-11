using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
    public interface IItemService
    {
        Task<List<Item>> GetAllItemsAsync();
        Task<List<Item>> GetUserItemsAsync(int userId);
        Task AddItemAsync(Item item, User user);
        Task EditItemAsync(Item item, User user);
        Task RemoveItemAsync(int itemId, User user);
        Task<List<Item>> GetFilteredItemsAsync(string name, DateTime? receiptDate, string manufacturer);
        Task<List<Item>> GetFilteredUserItemsAsync(int userId, string name, DateTime? receiptDate, string manufacturer);
    }
}
