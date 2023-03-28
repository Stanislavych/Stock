using Stock.Models.Models;

namespace Stock.BusinessLogic.Interfaces
{
    public interface IItemService
    {
        List<Item> GetAllItems();
    }
}
