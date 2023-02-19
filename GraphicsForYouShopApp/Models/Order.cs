using GraphicsForYouShopApp.Data;
using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        
        public decimal TotalAmount { get; set; }

        //Relationships
        public virtual List<OrderItem> OrderItems { get; set; }

        //Enum
        public OrderStatus OrderStatus { get; set; }

    }
}
