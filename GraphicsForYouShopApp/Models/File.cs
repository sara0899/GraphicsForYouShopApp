using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        public byte Name { get; set; }

    }
}
