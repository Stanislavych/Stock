using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
    public interface IItemService
    {
        List<Item> GetAllItems();
        List<Item> GetUserItems(int userId);
        Task AddItemAsync(Item item, User user);
        Task EditItemAsync(Item item);
        Task RemoveItemAsync(int itemId);
    }
}
