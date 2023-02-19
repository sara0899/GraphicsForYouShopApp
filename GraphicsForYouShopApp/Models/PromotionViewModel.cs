namespace GraphicsForYouShopApp.Models
{
    public class PromotionViewModel
    {
        public string GraphicName { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public int GraphicId { get; set; }
        public string MainPictureUrl { get; set; }
        public List<Promotion> Promotions { get; set; }
    }
}
