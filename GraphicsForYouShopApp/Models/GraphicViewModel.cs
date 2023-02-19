using GraphicsForYouShopApp.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphicsForYouShopApp.Models
{
    public class GraphicViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [Display(Name = "Nazwa")]
        public string? Name { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "Opis jest wymagany")]
        public string? Description { get; set; }

        [Display(Name = "Cena")]
        [Required(ErrorMessage = "Cena jest wymagana")]
        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

        [Display(Name = "Kategoria")]
        public Category? Category { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Kategoria")]
        [Required(ErrorMessage = "Kategoria jest wymagana")]
        public int CategoryId { get; set; }

        [Display(Name = "Kolekcja")]
        public Collection? Collection { get; set; }

        [ForeignKey("Collection")]
        [Display(Name = "Kolekcja")]
        [Required(ErrorMessage = "Kolekcja jest wymagana")]
        public int CollectionId { get; set; }

        [Display(Name = "Wybierz główne zdjęcie")]
        [Required(ErrorMessage = "Główne zdjęcie jest wymagane")]
        public IFormFile? MainPicture { get; set; }
        public string? MainPictureUrl { get; set; }

        [Display(Name = "Wybierz zdjęcia do galerii")]
        [Required(ErrorMessage = "Zdjęcia do galerii są wymagane")]
        public IFormFileCollection? Pictures { get; set; }
        public List<Picture>? PicturesList { get; set; }

        //Enum
        [Display(Name = "Określ dostępność na sklepie")]
        public GraphicAvailability? Availability { get; set; }
    }
}
