using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;

namespace Stock.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var items = _itemService.GetAllItems();
            return View(items);
        }
    }
}
