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
        private readonly ILogger<ItemController> _logger;
        public ItemController(IItemService itemService, IUsersService usersService, ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _usersService = usersService;
			_logger = logger;
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
            return View(items);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddItem([FromForm] Item item)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetUserByName(username);
            await _itemService.AddItemAsync(item, user);
            _logger.LogInformation($"Был добавлен предмет \"{item.Name}\" под номером \"{item.Id}\" пользователем {username}");
            return RedirectToAction("MyItems");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditItem([FromForm] Item item)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetUserByName(username);
            await _itemService.EditItemAsync(item, user);
            _logger.LogInformation($"Был изменен предмет \"{item.Name}\" под номером \"{item.Id}\" пользователем {username}");
            if (HttpContext.Request.Headers["Referer"].ToString().Contains("MyItems"))
                return RedirectToAction("MyItems");
            else
                return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveItem([FromForm] int itemId)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetUserByName(username);
            await _itemService.RemoveItemAsync(itemId, user);
            _logger.LogInformation($"Был удален предмет под номером \"{itemId}\" пользователем {username}");
            if (HttpContext.Request.Headers["Referer"].ToString().Contains("MyItems"))
                return RedirectToAction("MyItems");
            else
                return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        public async Task<IActionResult> FilteringItems([FromForm] string name, [FromForm] DateTime? receiptDate, [FromForm] string manufacturer)
        {
            var filteredItems = await _itemService.GetFilteredItemsAsync(name, receiptDate, manufacturer);
            return View("Index", filteredItems);
        }
        [HttpPost]
        public async Task<IActionResult> FilteringUserItems([FromForm] string name, [FromForm] DateTime? receiptDate, [FromForm] string manufacturer)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetUserByName(username);
            var filteredItems = await _itemService.GetFilteredUserItemsAsync(user.Id, name, receiptDate, manufacturer);
            return View("MyItems", filteredItems);
        }
    }
}
