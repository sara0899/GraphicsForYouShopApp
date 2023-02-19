using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class Collection
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        public List<Graphic> Graphics { get; set; }
    }
}
