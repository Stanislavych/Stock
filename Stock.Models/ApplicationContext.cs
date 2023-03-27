using Microsoft.EntityFrameworkCore;
using Stock.Models.Models;

namespace Stock.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasData(new Item
            {
                Id = 1,
                Name = "Banana",
                ItemType = "Food",
                Manufacturer = "Belarus",
                ManufacturerAddress = "Pinsk",
                ReceiptDate = DateTime.Now
            });
        }
    }
}
