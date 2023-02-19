using GraphicsForYouShopApi.Data;
using GraphicsForYouShopApi.Data.Enums;
using GraphicsForYouShopApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraphicsForYouShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GraphicsDbContext context;

        public OrderController(GraphicsDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        [Route("GetOrder/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = context.Orders.Where(order => order.Id == id).FirstOrDefault();
            return Ok(order);
        }

        [HttpGet]
        [Route("GetUserOrders/{id}")]
        public async Task<IActionResult> GetUserOrders(int id)
        {
            var orderList = context.Orders.Where(order => order.UserId == id).ToList();
            return Ok(orderList);
        }

        [HttpGet]
        [Route("GetOrderItems/{id}")]
        public async Task<IActionResult> GetOrderItems(int id)
        {
            var graphicList = context.OrderItems.Where(order => order.OrderId == id).Select(order => order.Graphic).ToList();
            var priceList = context.OrderItems.Where(order => order.OrderId == id).Select(order => order.Price).ToList();
            for (int i = 0; i < graphicList.Count; i++)
            {
                graphicList[i].Price = priceList[i];
            }
            return Ok(graphicList);
        }

        private async Task<List<Graphic>> GetOrderGraphics(int id)
        {
            var graphicList = context.OrderItems.Where(order => order.OrderId == id).Select(order => order.Graphic).ToList();
            var priceList = context.OrderItems.Where(order => order.OrderId == id).Select(order => order.Price).ToList();
            for (int i = 0; i < graphicList.Count; i++)
            {
                graphicList[i].Price = priceList[i];
            }
            return graphicList;
        }

        [HttpGet]
        [Route("GetOrderFiles/{id}")]
        public async Task<IActionResult> GetOrderFiles(int id)
        {
            var orderFiles = context.OrderFiles.Where(o => o.OrderId == id).ToList();
            return Ok(orderFiles);
        }

        [HttpGet]
        [Route("GetOrderFile/{id}")]
        public async Task<IActionResult> GetOrderFile(int id)
        {
            var orderFile = context.OrderFiles.Where(o => o.Id == id).FirstOrDefault();
            return Ok(orderFile);
        }

        [HttpGet]
        [Route("GetSingleGraphic/{id}")]
        public async Task<IActionResult> GetSingleGraphic(int id)
        {
            var graphic = context.Graphics.Where(p => p.Id == id).FirstOrDefault();
            return Ok(graphic);
        }

        [HttpGet]
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orderList = context.Orders.ToList();
            return Ok(orderList);
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<IActionResult> AddOrder(Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            Message message = new Message();

            message.SenderId = 1;
            if (order.OrderItems.Count > 1)
            {
                message.MessageText = AutomaticMessage.NewOrder2;
            }
            else
            {
                message.MessageText = AutomaticMessage.NewOrder;
            }
            message.ReceiverId = order.UserId;
            message.Read = false;
            message.SendDateTime = DateTime.Now;

            try
            {
                context.Messages.Add(message);
                context.SaveChanges();
            }
            catch (Exception)
            {

            }

            var orderItems = await GetOrderGraphics(order.Id);

            foreach (var item in orderItems)
            {
                var categoryId = context.Graphics.Where(g => g.Id == item.Id).Select(g => g.CategoryId).FirstOrDefault();
                if (categoryId == 6)
                {
                    var messageText = string.Empty;
                    messageText = AutomaticMessage.Logo;
                    Message messageOrderItem = new Message();
                    messageOrderItem.SenderId = 1;
                    messageOrderItem.MessageText = messageText;
                    messageOrderItem.ReceiverId = order.UserId;
                    messageOrderItem.Read = false;
                    messageOrderItem.SendDateTime = DateTime.Now;
                    context.Messages.Add(messageOrderItem);
                    context.SaveChanges();
                }
            }

            return Ok(order.Id);
        }

        [HttpPut]
        [Route("ChangeOrderStatus")]
        public async Task<IActionResult> ChangeOrderStatus(Order order)
        {
            var existingOrder = context.Orders.Where(o => o.Id == order.Id).FirstOrDefault();
            if (existingOrder != null)
            {
                existingOrder.OrderStatus = order.OrderStatus;
                context.SaveChanges();
            }
            var messageText = string.Empty;
            switch (order.OrderStatus)
            {
                case OrderStatus.Realizowane:
                    messageText = AutomaticMessage.InProgress;
                    break;
                case OrderStatus.Zrealizowane:
                    messageText = AutomaticMessage.Completed;
                    break;
                case OrderStatus.Anulowane:
                    messageText = AutomaticMessage.Cancelled;
                    break;
            }

            Message message = new Message();

            message.SenderId = 1;
            message.MessageText = messageText;
            message.ReceiverId = order.UserId;
            message.Read = false;
            message.SendDateTime = DateTime.Now;
            context.Messages.Add(message);
            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("AddOrderFile")]
        public async Task<IActionResult> AddOrderFile(OrderFile file)
        {
            
            context.OrderFiles.Add(file);
            context.SaveChanges();

            return Ok();
        }
    }
}
