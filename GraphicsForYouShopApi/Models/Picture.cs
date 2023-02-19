using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApi.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
