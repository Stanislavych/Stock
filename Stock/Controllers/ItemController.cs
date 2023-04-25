using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLogic.Interfaces;
using Stock.Models;
using Stock.Models.Models;

namespace Stock.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IUsersService _usersService;
        private readonly ILogger<ItemController> _logger;
        private readonly ApplicationContext _context;
        public ItemController(IItemService itemService, IUsersService usersService, ILogger<ItemController> logger, ApplicationContext context)
        {
            _itemService = itemService;
            _usersService = usersService;
            _logger = logger;
            _context = context;
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
            var file = Request.Form.Files.FirstOrDefault();
            using (var fileStream = file.OpenReadStream())
            {
                await _itemService.AddItemAsync(item, user, fileStream);
            }
            _logger.LogInformation($"Был добавлен предмет \"{item.Name}\" под номером \"{item.Id}\" пользователем {User.Identity.Name}");
            return RedirectToAction("MyItems");
        }
        [HttpGet]
        public IActionResult GetImage(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);
            if (item != null && item.Image != null)
            {
                return File(item.Image, "image/png");
            }
            return NotFound();
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> EditItem(int itemId, [FromForm] Item item)
        {
            item.Id = itemId;
            var user = await _usersService.GetUserByName(User.Identity.Name);
            item.UserId = user.Id;
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null)
            {
                using (var fileStream = file.OpenReadStream())
                {
                    await _itemService.EditItemAsync(item, user, fileStream);
                }
            }
            else
            {
                await _itemService.EditItemAsync(item, user);
            }

            _logger.LogInformation($"Был изменен предмет \"{item.Name}\" под номером \"{item.Id}\" пользователем {User.Identity.Name}");
            return HttpContext.Request.Headers["Referer"].ToString().Contains("MyItems") ?
       RedirectToAction("MyItems") :
       Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> RemoveItem(int itemId)
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
