using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;
using Stock.Models.Models;

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
		[HttpGet]
		[Authorize]
		public IActionResult MyItems()
        {
            var items = _itemService.GetAllItems();
            return View(items);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItem([FromForm]Item item)
        {
            await _itemService.AddItemAsync(item);
            return RedirectToAction("MyItems");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditItem([FromForm]Item item)
        {
            await _itemService.EditItemAsync(item);
            return RedirectToAction("MyItems");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveItem([FromForm]int itemId)
        {
            await _itemService.RemoveItemAsync(itemId);
            return RedirectToAction("MyItems");
        }
    }
}
