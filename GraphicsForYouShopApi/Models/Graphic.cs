
using GraphicsForYouShopApi.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace GraphicsForYouShopApi.Models
{
    public class Graphic
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Collection Collection { get; set; }
        public int CollectionId { get; set; }
        public string MainPictureUrl { get; set; }
        public ICollection<GraphicPictures> graphicPictures { get; set; }
        public GraphicAvailability Availability { get; set; }

    }
}
