namespace Stock.Models.Models
{
    public class Item
    {
        public int Id { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerAddress { get; set; }
        public string ItemType { get; set; }
        public byte[]? Image { get; set; }
    }
}
