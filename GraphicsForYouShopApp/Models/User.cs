using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphicsForYouShopApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }

        public int? NIP { get; set; }

        public string Street { get; set; }
        public string BuildingNumber { get; set; }

        public string City { get; set; }
        public string ZIPCode { get; set; }

        public int PhoneNumber { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]

        public UserRole Role { get; set; }

        public User()
        {
            RoleId = 1;
        }

        //Relationships
      //  public List<Order> Orders { get; set; }

        public ICollection<Connection> Connections { get; set; }



    }
}
