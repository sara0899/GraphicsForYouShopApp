using GraphicsForYouShopApi.Data;
using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApi.Models
{


    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderFile> OrderFiles { get; set; }
    }
}
