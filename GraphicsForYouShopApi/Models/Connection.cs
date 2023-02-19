
using System.ComponentModel.DataAnnotations.Schema;
namespace GraphicsForYouShopApi.Models
{
    public class Connection
    {
        public string ConnectionID { get; set; }
        public bool Connected { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}
