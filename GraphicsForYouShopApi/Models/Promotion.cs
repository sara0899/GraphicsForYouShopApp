namespace GraphicsForYouShopApi.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GraphicId { get; set; }
        public virtual Graphic Graphic { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Percentage   { get; set; }
    }
}
