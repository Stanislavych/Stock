namespace Stock.Models.Models
{
    public class Item
    {
        public int Id { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string ManufacturerAddress { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty ;
        public byte[]? Image { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
