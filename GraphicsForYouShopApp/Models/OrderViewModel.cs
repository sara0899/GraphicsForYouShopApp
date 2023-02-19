namespace GraphicsForYouShopApp.Models
{
    public class OrderViewModel
    {
        public List<Graphic> GraphicsInCart { get; set; }

        public List<int> GraphicsIdsInCart { get; set; }

        public User User { get; set; }
    }
}
