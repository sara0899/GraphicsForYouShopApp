namespace GraphicsForYouShopApp.Models
{
    public class OrderFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public DateTime Date { get; set; }
        public byte[] GraphicFile { get; set; }
    }
}
