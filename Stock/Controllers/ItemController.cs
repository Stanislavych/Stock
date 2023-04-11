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
        public async Task<IActionResult> Index() => View(await _itemService.GetAllItemsAsync());
        [HttpGet, Authorize]
        public async Task<IActionResult> MyItems()
        {
            var user = await _usersService.GetUserByName(User.Identity.Name);
            var userItems = await _itemService.GetUserItemsAsync(user.Id);
            return View(userItems);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> AddItem([FromForm] Item item)
        {
            var user = await _usersService.GetUserByName(User.Identity.Name);
            await _itemService.AddItemAsync(item, user);
            _logger.LogInformation($"Был добавлен предмет \"{item.Name}\" под номером \"{item.Id}\" пользователем {User.Identity.Name}");
            return RedirectToAction("MyItems");
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> EditItem([FromForm] Item item)
        {
            var user = await _usersService.GetUserByName(User.Identity.Name);
            await _itemService.EditItemAsync(item, user);
            _logger.LogInformation($"Был изменен предмет \"{item.Name}\" под номером \"{item.Id}\" пользователем {User.Identity.Name}");
            return HttpContext.Request.Headers["Referer"].ToString().Contains("MyItems") ?
       RedirectToAction("MyItems") :
       Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> RemoveItem([FromForm] int itemId)
        {
            var user = await _usersService.GetUserByName(User.Identity.Name);
            await _itemService.RemoveItemAsync(itemId, user);
            _logger.LogInformation($"Был удален предмет под номером \"{itemId}\" пользователем {User.Identity.Name}");
            return HttpContext.Request.Headers["Referer"].ToString().Contains("MyItems") ?
       RedirectToAction("MyItems") :
       Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        public async Task<IActionResult> FilteringItems([FromForm] string name, [FromForm] DateTime? receiptDate, [FromForm] string manufacturer)
    => View("Index", await _itemService.GetFilteredItemsAsync(name, receiptDate, manufacturer));
        [HttpPost, Authorize]
        public async Task<IActionResult> FilteringUserItems([FromForm] string name, [FromForm] DateTime? receiptDate, [FromForm] string manufacturer)
        {
            var user = _usersService.GetUserByName(User.Identity.Name);
            return View("MyItems", await _itemService.GetFilteredUserItemsAsync(user.Id, name, receiptDate, manufacturer));
        }
    }
}
