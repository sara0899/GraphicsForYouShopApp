namespace GraphicsForYouShopApp.Models
{
    public class OrderItem
    { 
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int GraphicId { get; set; }
        public virtual Graphic Graphic { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Format { get; set; }
        public string Contact { get; set; }
    }
}
