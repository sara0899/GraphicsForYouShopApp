using Microsoft.AspNetCore.Mvc;
using System.Data;
using GraphicsForYouShopApi.Models;
using System.Text;
using System.Security.Cryptography;

namespace GraphicsForYouShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly GraphicsDbContext _graphicsDbContext;
        public AccountController(GraphicsDbContext graphicsDbContext)
        {
            _graphicsDbContext = graphicsDbContext;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] User userModel)
        {
            try
            {
                if (_graphicsDbContext.Roles.Count() == 0)
                {
                    var userRole = new UserRole();
                    var designerRole = new UserRole();
                    userRole.RoleName = "User";
                    designerRole.RoleName = "Designer";
                    _graphicsDbContext.Roles.Add(userRole);
                    _graphicsDbContext.Roles.Add(designerRole);
                    _graphicsDbContext.SaveChanges();
                }

                await _graphicsDbContext.Users.AddAsync(userModel);
                await _graphicsDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetUser/{email}")]
        public IActionResult GetUser(string email)
        {
            try
            {
                var user = _graphicsDbContext.Users.Where(u => u.Email == email).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetUserId/{email}")]
        public IActionResult GetUserId(string email)
        {
            try
            {
                var userId = _graphicsDbContext.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, userId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _graphicsDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetRoleName/{id}")]
        public IActionResult GetRoleName(int id)
        {
            try
            {
                string roleName = (from user in _graphicsDbContext.Users
                                    join role in _graphicsDbContext.Roles on user.RoleId equals role.Id
                                    where user.Id == id
                                    select role.RoleName).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, roleName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetMessages/{id},{senderId}")]
        public IActionResult GetMessages(int id, int senderId)
        {
            try
            {
                var messagesFromUser = _graphicsDbContext.Messages
                                        .Where(message => message.ReceiverId == id && message.SenderId == senderId).ToList();
                return StatusCode(StatusCodes.Status200OK, messagesFromUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpPut]
        [Route("ChangeMessageReading")]
        public IActionResult ChangeMessageReading([FromBody] string email)
        {
            try
            {
                var userModel = _graphicsDbContext.Users.SingleOrDefault(user => user.Email == email);
                var messagesFromUser = _graphicsDbContext.Messages
                                        .Where(message => message.ReceiverId == userModel.Id && message.Read == false).ToList();

                if (messagesFromUser != null)
                {
                    foreach (var message in messagesFromUser)
                    {
                        message.Read = true;
                        _graphicsDbContext.SaveChanges();
                    }
                }

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }
        

        [HttpGet]
        [Route("GetUnreadMessagesCount/{email}")]
        public IActionResult GetUnreadMessagesCount(string email)
        {
            try
            {
                int userId = _graphicsDbContext.Users.Where(user => user.Email == email).Select(user => user.Id).FirstOrDefault();
                int messagesCount = _graphicsDbContext.Messages
                                     .Where(message => message.ReceiverId == userId && message.Read == false).Count();

                return StatusCode(StatusCodes.Status200OK, messagesCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetUnreadMessagesToDesigner/{id}")]
        public IActionResult GetUnreadMessagesToDesigner(int id)
        {
            try
            {
                int messagesCount = _graphicsDbContext.Messages
                                     .Where(message => message.SenderId == id && message.Read == false).Count();

                return StatusCode(StatusCodes.Status200OK, messagesCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpPost]
        [Route("AddMessage")]
        public async Task<IActionResult> AddMessage(Message message)
        {
            try
            {
                await _graphicsDbContext.Messages.AddAsync(message);
                await _graphicsDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }

        }

        [HttpPut]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(User userModel)
        {
            var newUser = _graphicsDbContext.Users.Where(user => user.Email == userModel.Email).FirstOrDefault();
            if (newUser != null)
            {
                newUser.Name = userModel.Name;
                newUser.Surname = userModel.Surname;
                newUser.Company = userModel.Company;
                newUser.NIP = userModel.NIP;
                newUser.Street = userModel.Street;
                newUser.BuildingNumber = userModel.BuildingNumber;
                newUser.City = userModel.City;
                newUser.ZIPCode = userModel.ZIPCode;
                newUser.PhoneNumber = userModel.PhoneNumber;
                newUser.Email = userModel.Email;
            }
            await _graphicsDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new { message = "Edytowano użytkownika" });
        }


        [HttpPut]
        [Route("EditPassword")]
        public async Task<IActionResult> EditPassword(User userModel)
        {
            var newUser = _graphicsDbContext.Users.Where(user => user.Email == userModel.Email).FirstOrDefault();
            if (newUser != null)
            {
                newUser.Password = userModel.Password;
            }
            await _graphicsDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK, new { message = "Hasło zostało zmienione" });
        }


    }
}
