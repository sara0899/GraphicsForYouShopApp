using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class OrderFileViewModel
    {
        public int OrderId { get; set; }

        [Display(Name = "Pliki do zamówienia")]
        public IFormFileCollection Files { get; set; }
        public List<File> FilesList { get; set; }
    }
}
