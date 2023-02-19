namespace GraphicsForYouShopApi.Models
{
    public class ShopViewModel
    {
        public List<Graphic> GraphicsInCart { get; set; }

        public List<int> GraphicsIdsInCart { get; set; }

        public User User { get; set; }
    }
}
