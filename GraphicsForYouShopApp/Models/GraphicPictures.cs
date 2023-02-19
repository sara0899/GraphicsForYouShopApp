using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class GraphicPictures
    {
        [Key]
        public int Id { get; set; }
        public int GraphicId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public Graphic Graphic { get; set; }
    }
}
