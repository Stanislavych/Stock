using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;
using Stock.Models.Models;

namespace Stock.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IUsersService _usersService;
        public ItemController(IItemService itemService, IUsersService usersService)
        {
            _itemService = itemService;
            _usersService = usersService;
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
            var username = User.Identity.Name;
            var user = _usersService.GetUserByName(username);
            var items = _itemService.GetUserItems(user.Id);
            var filteredItems = items.Where(item => item.UserId == user.Id).ToList();
            return View(filteredItems);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItem([FromForm] Item item)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetUserByName(username);
            await _itemService.AddItemAsync(item, user);
            return RedirectToAction("MyItems");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditItem([FromForm] Item item)
        {
            await _itemService.EditItemAsync(item);
            return RedirectToAction("MyItems");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveItem([FromForm] int itemId)
        {
            await _itemService.RemoveItemAsync(itemId);
            return RedirectToAction("MyItems");
        }
    }
}
