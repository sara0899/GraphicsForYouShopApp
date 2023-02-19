using GraphicsForYouShopApp.Data;
using GraphicsForYouShopApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class Graphic
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

        [Display(Name = "Kategoria")]
        public Category Category { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "Seria")]
        public Collection Collection { get; set; }

        public int CollectionId { get; set; }
        public string MainPictureUrl { get; set; }

        public ICollection<GraphicPictures> graphicPictures { get; set; }

        //Enum
        public GraphicAvailability Availability { get; set; }
    }
}
