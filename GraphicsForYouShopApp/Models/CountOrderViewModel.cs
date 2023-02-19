namespace GraphicsForYouShopApp.Models
{
    public class CountOrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InProgressOrders { get; set; }

        public int UnpaidOrders { get; set; }

        public int RealisedOrders { get; set; }

        public int CancelledOrders { get; set; }

        public int MessagesCount { get; set; }
    }
}
