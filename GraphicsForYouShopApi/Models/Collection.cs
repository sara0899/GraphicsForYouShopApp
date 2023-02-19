using System.ComponentModel.DataAnnotations;
namespace GraphicsForYouShopApi.Models
{
    public class Collection
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Graphic> Graphics { get; set; }

    }
}
