using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GraphicsForYouShopApi.Models
{
    public class RegisterViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [Display(Name = "E-mail*")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Wprowadź adres e-mail w poprawnym formacie")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Powtórzenie hasła jest wymagane")]
        [Display(Name = "Powtórz hasło*")]
        [Compare("Password", ErrorMessage = ("Podane hasła nie są identyczne"))]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [Display(Name = "Imię*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [Display(Name = "Nazwisko*")]
        public string Surname { get; set; }

        [Display(Name = "Nazwa firmy")]
        public string Company { get; set; }

        [Display(Name = "NIP")]
        public int? NIP { get; set; }

        [Required(ErrorMessage = "Ulica jest wymagana")]
        [Display(Name = "Ulica*")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Numer budynku jest wymagany")]
        [Display(Name = "Nr domu* / lokalu")]
        public string BuildingNumber { get; set; }

        [Required(ErrorMessage = "Miejscowość jest wymagana")]
        [Display(Name = "Miejscowość*")]
        public string City { get; set; }

        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [Display(Name = "Kod pocztowy*")]
        public string ZIPCode { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [Display(Name = "Numer telefonu*")]
        public int PhoneNumber { get; set; }

    }
}
