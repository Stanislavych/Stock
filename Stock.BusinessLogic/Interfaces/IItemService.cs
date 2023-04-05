using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
    public interface IItemService
    {
        List<Item> GetAllItems();
        Task AddItemAsync(Item item);
        Task EditItemAsync(Item item);
        Task RemoveItemAsync(int itemId);
    }
}
