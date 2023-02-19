using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public List<Graphic> Graphics { get; set; }

    }
}
