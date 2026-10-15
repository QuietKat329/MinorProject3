namespace MinorProject3.Data.Models
{
    public class ShopItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public int PriceGold { get; set; }
        public int StockQuantity { get; set; }
    }
}