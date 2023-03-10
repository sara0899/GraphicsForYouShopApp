using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace GraphicsForYouShopApi.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<User> Users { get; set; }
    }
}
