using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Security.Cryptography;
using System.Dynamic;
using GraphicsForYouShopApp.Models;
using GraphicsForYouShopApp.Services;

namespace GraphicsForYouShopApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGraphicsApiService _graphicsApiService;

        public AccountController(IGraphicsApiService graphicsApiService)
        {
            _graphicsApiService = graphicsApiService;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpGet]
        public async Task<int> GetUnreadMessagesCount()
        {
            int unreadMessagesCount = await _graphicsApiService
                                        .GetUnreadMessagesCount(User.FindFirstValue(ClaimTypes.Email));
            return unreadMessagesCount;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                var userHasThisEmail = await _graphicsApiService.GetUser(model.Email);
                if (userHasThisEmail == null)
                {
                    if (ModelState.IsValid)
                    {
                        var user = new User()
                        {
                            Name = model.Name,
                            Surname = model.Surname,
                            Company = model.Company,
                            NIP = model.NIP,
                            Street = model.Street,
                            BuildingNumber = model.BuildingNumber,
                            City = model.City,
                            ZIPCode = model.ZIPCode,
                            PhoneNumber = model.PhoneNumber,
                            Email = model.Email,
                            Password = HashUserPassword(model.Password)
                        };

                        await _graphicsApiService.Register(user);
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["errorMessage"] = "Formularz nie został poprawnie wypełniony";
                        return View(model);
                    }
                }
                else
                {
                    TempData["errorMessage"] = $"Użytkownik o podanym adres e-mail {model.Email} już istnieje";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Wystąpił błąd podczas rejestracji użytkownika";
                return View(model);
            }
        }

        public async Task<IActionResult> Login(string ifCheckout)
        {
            ViewBag.ifCheckout = ifCheckout;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _graphicsApiService.GetUser(model.Email);
                    if (user != null)
                    {
                        bool isCorrectPassword = (user.Email == model.Email && CheckPasswords(model.Password, user.Password));
                        if (isCorrectPassword)
                        {
                            var roleName = await _graphicsApiService.GetRoleName(user.Id);
                            var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Name),
                                            new Claim(ClaimTypes.Role, roleName), new Claim(ClaimTypes.Email, user.Email)}, 
                                            CookieAuthenticationDefaults.AuthenticationScheme);
                            
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                            HttpContext.Session.SetString("E-mail", user.Email);
                            
                            string ifCheckout = Request.Form["ifCheckout"];
                            if (ifCheckout == "True")
                                return RedirectToAction("Summary", "Order");
                            else
                                return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["errorMessage"] = "Podano nieprawidłowe hasło";
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Użytkownik o podanym adresie e-mail nie istnieje";
                        return View(model);
                    }

                    return View(model);
                }
                else
                {
                    TempData["errorMessage"] = "Formularz nie został poprawnie wypełniony";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("Index", "Home");
        }

        public async void ChangeMessageReading()
        {
            await _graphicsApiService.ChangeMessageReading(User.FindFirstValue(ClaimTypes.Email));
        }

        public async Task<IActionResult> Communicator(int id)
        {
            dynamic dy = new ExpandoObject();
            if (id == 0) id = 1;
            dy.receiverId = id;
            int senderId = await _graphicsApiService.GetUserId(User.FindFirstValue(ClaimTypes.Email));
            dy.senderId = senderId;
            var messagesFromUser = await _graphicsApiService.GetMessages(id, senderId);
            var messagesFromAdmin = await _graphicsApiService.GetMessages(senderId, id);
            var messages = new List<Message>();
            messages.AddRange(messagesFromUser);
            messages.AddRange(messagesFromAdmin);
            var SortedList = messages.OrderBy(o => o.Id).ToList();
            dy.messagesList = SortedList;
            return PartialView(dy);
        }
       

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _graphicsApiService.GetUser(User.FindFirstValue(ClaimTypes.Email));
            var newUser = new RegisterViewModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Company = user.Company,
                NIP = user.NIP,
                Street = user.Street,
                BuildingNumber = user.BuildingNumber,
                City = user.City,
                ZIPCode = user.ZIPCode,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
                
            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(RegisterViewModel user)
        {
           
            await _graphicsApiService.EditProfile(user);
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> EditPassword(RegisterViewModel user)
        {

            if(user.Password == user.ConfirmPassword) {
                user.Password = HashUserPassword(user.Password);
                user.Email = User.FindFirstValue(ClaimTypes.Email);
            }
            await _graphicsApiService.EditPassword(user);
            return RedirectToAction("Index", "Home");

        }


        private string HashUserPassword(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5ComputeHash = md5.ComputeHash(passwordBytes);
                for (int i = 0; i < md5ComputeHash.Length; i++)
                {
                    stringBuilder.Append(md5ComputeHash[i].ToString("x2"));
                }
            }
            return stringBuilder.ToString();
        }
        private bool CheckPasswords(string password, string hash)
        {
            bool theSamePasswords;
            string passwordHash = HashUserPassword(password);
            if (string.Compare(passwordHash, hash) == 0)
                theSamePasswords = true;
            else
                theSamePasswords = false;
            return theSamePasswords;
        }

    }
}
