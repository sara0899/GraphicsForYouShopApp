using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa promocji")]
        public string Name { get; set; }
        public int GraphicId { get; set; }
        public virtual Graphic Graphic { get; set; }

        [Display(Name = "Data początku promocji")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Data końca promocji")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Obniżka [%]")]
        public int Percentage   { get; set; }
    }
}
