namespace GraphicsForYouShopApp.Models
{
    public class PromotionViewModdel
    {
        public int Id { get; set; }
        public string PromotionName { get; set; }
        public string GraphicName { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public int GraphicId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Percentage { get; set; }
        public string MainPictureUrl { get; set; }
    }
}
