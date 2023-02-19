using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApp.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
