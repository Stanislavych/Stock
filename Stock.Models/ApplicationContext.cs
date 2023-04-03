using Microsoft.EntityFrameworkCore;
using Stock.Models.Models;

namespace Stock.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasData(new Item
            {
                Id = 1,
                Name = "Банан",
                ItemType = "Еда",
                Manufacturer = "Беларусь",
                ManufacturerAddress = "Пинск ул. Центральная 28",
                ReceiptDate = DateTime.Now
            },
            new Item
            {
                Id = 2,
                Name = "Дерево",
                ItemType = "Материал",
                Manufacturer = "Америка",
                ManufacturerAddress = "Groove Street 228",
                ReceiptDate = DateTime.Now
            },
            new Item
            {
                Id = 3,
                Name = "Апельсин",
                ItemType = "Еда",
                Manufacturer = "Россия",
                ManufacturerAddress = "Российская 16",
                ReceiptDate = DateTime.Now
            },
            new Item
            {
                Id = 4,
                Name = "Ручка",
                ItemType = "Канцелярия",
                Manufacturer = "Таджикистан",
                ManufacturerAddress = "Маяковская 2",
                ReceiptDate = DateTime.Now
            }
            );
        }
    }
}
