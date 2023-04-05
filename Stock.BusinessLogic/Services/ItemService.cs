﻿using Stock.BusinessLogic.Interfaces;
using Stock.Models;
using Stock.Models.Models;

namespace Stock.BusinessLogic.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationContext _context;
        public ItemService(ApplicationContext context)
        {
            _context = context;
        }
        public List<Item> GetAllItems()
        {
            var items = _context.Items.ToList();
            return items;
        }
        public List<Item> GetUserItems(int userId)
        {
            var userItems = _context.Items.Where(item => item.UserId == userId).ToList();
            return userItems;
        }
        public async Task AddItemAsync(Item item, User user)
        {
            item.UserId = user.Id;
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }
        public async Task EditItemAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveItemAsync(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
