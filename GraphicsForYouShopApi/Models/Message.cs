using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphicsForYouShopApi.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public bool Read { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        public int SenderId { get; set; }

        [ForeignKey("ReceiverId")]

        public virtual User Receiver { get; set; }
        public int ReceiverId { get; set; }

        public DateTime SendDateTime { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileBytes { get; set; }

    }
}
