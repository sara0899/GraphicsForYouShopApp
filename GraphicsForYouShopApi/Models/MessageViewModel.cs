namespace GraphicsForYouShopApi.Models
{
    public class MessageViewModel
    {
        public string MessageText { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public ICollection<IFormFile> Files { get; set; }
      
    }
}
