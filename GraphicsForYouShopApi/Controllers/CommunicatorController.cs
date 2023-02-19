using Microsoft.AspNetCore.Mvc;
using GraphicsForYouShopApi.Data;
using GraphicsForYouShopApi.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GraphicsForYouShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicatorController : ControllerBase
    {
        HubConnection connection;
        private readonly GraphicsDbContext context;

        private readonly IHubContext<CommunicatorHub> _hubContext;

        public CommunicatorController(GraphicsDbContext _context, IHubContext<CommunicatorHub> hubContext)
        {
            context = _context;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Route("GetUsersList/{status}")]
        public IActionResult GetUsersList(OrderStatus status)
        {
            var tempUsersList = new List<int>();
            tempUsersList = context.Orders.Where(order => order.OrderStatus == status).Select(order => order.UserId).Distinct().ToList();
            return Ok(tempUsersList);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var tempUsersList = context.Users.Where(u => u.Id != 1).Select(u => u.Id).ToList();
          
            return Ok(tempUsersList);
        }


        [HttpGet]
        [Route("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var email = context.Users.Where(u => u.Id == id).Select(u => u.Email).FirstOrDefault();

            return Ok(email);
        }

        
        [HttpGet]
        [Route("GetCountOrder/{user},{status}")]
        public IActionResult GetCountOrder(int user, OrderStatus status)
        {
            int countOrders = context.Orders.Where(order => order.UserId == user && order.OrderStatus == status).Count();

            return Ok(countOrders);
        }

        [HttpGet]
        [Route("GetMessage/{id}")]
        public IActionResult GetMessage(int id)
        {
            var message = context.Messages.Where(m => m.Id == id).FirstOrDefault();

            return Ok(message);
        }

        [HttpGet]
        [Route("GetUsersListUnreadMessages")]
        public IActionResult GetUsersListUnreadMessages()
        {
            var tempUsersList = new List<int>();
            tempUsersList = context.Messages.Where(user => user.ReceiverId == 1 && user.Read == false).Select(user => user.SenderId).Distinct().ToList();
            return Ok(tempUsersList);
        }

        [Route("SendMessageFiles")]
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm] MessageViewModel message)
        {
            var user = context.Users.Where(k => k.Id == message.ReceiverId).FirstOrDefault();
            var sender = context.Users.Where(k => k.Id == message.SenderId).FirstOrDefault();
            if (user != null)
            {
                if (!string.IsNullOrEmpty(message.MessageText))
                {
                    Message newTextMessage = new Message();

                    newTextMessage.SenderId = message.SenderId;
                    newTextMessage.MessageText = message.MessageText;
                    newTextMessage.ReceiverId = message.ReceiverId;
                    newTextMessage.Read = false;
                    newTextMessage.SendDateTime = DateTime.Now;

                    try
                    {
                        context.Messages.Add(newTextMessage);
                        context.SaveChanges();
                    }
                    catch (Exception)
                    {

                    }
                }

                context.Entry(user)
                .Collection(u => u.Connections)
                .Query()
                .Where(c => c.Connected == true).Load();

                context.Entry(sender)
                    .Collection(u => u.Connections)
                    .Query()
                    .Where(c => c.Connected == true).Load();

                if (user.Connections != null)
                {
                    foreach (var connection in user.Connections)
                    {
                        await _hubContext.Clients.Client(connection.ConnectionID)
                            .SendAsync("ReceiveMessage", message.ReceiverId, message.SenderId, message.MessageText, null, 0);
                    }
                }

                if (sender.Connections != null)
                {
                    foreach (var connection in sender.Connections)
                    {
                        await _hubContext.Clients.Client(connection.ConnectionID)
                            .SendAsync("ReceiveMessage", message.ReceiverId, message.SenderId, message.MessageText, null, 0);
                    }
                }

            }

            //zacząć od wiadomości a potem pliki
            if (message.Files != null)
            {
                foreach (var file in message.Files)
                {
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            Message newFileMessage = new Message();
                            using (var reader = new MemoryStream())
                            {
                                file.CopyTo(reader);
                                newFileMessage.FileName = file.FileName;
                                newFileMessage.FileType = file.ContentType;
                                newFileMessage.FileBytes = reader.ToArray();
                                newFileMessage.SenderId = message.SenderId;
                                newFileMessage.ReceiverId = message.ReceiverId;
                                newFileMessage.Read = false;
                                newFileMessage.SendDateTime = DateTime.Now;
                            }

                            context.Messages.Add(newFileMessage);
                            context.SaveChanges();

                            if (user.Connections != null)
                            {
                                foreach (var connection in user.Connections)
                                {
                                    await _hubContext.Clients.Client(connection.ConnectionID)
                                        .SendAsync("ReceiveMessage", message.ReceiverId, message.SenderId, null, file.FileName, newFileMessage.Id);
                                }
                            }

                            if (sender.Connections != null)
                            {
                                foreach (var connection in sender.Connections)
                                {
                                    await _hubContext.Clients.Client(connection.ConnectionID)
                                        .SendAsync("ReceiveMessage", message.ReceiverId, message.SenderId, null, file.FileName, newFileMessage.Id);
                                }
                            }
                        }
                    }
                }
            }

            return Ok();
        }

        
        
    }
    
}
           
            
  
        



    

