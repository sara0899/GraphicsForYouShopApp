using Microsoft.AspNetCore.Mvc;
using GraphicsForYouShopApp.Models;
using GraphicsForYouShopApp.Data;
using System.Security.Claims;
using System.Dynamic;
using GraphicsForYouShopApp.Services;
using Newtonsoft.Json;

namespace GraphicsForYouShopApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IGraphicsApiService _graphicsForYouApiService;

        public OrderController(IGraphicsApiService graphicsForYouApiService)
        {
            _graphicsForYouApiService = graphicsForYouApiService;
        }

        public async Task<IActionResult> FinishOrder(int orderId)
        {
            var order = await _graphicsForYouApiService.GetOrder(orderId);
            return View(order);
        }

        public async Task<IActionResult> GraphicProjectsCart()
        {
            ShopViewModel model = new ShopViewModel();
            var GraphicProjectsCartCookie = Request.Cookies["GraphicsInCart"];
            var graphicList = new List<Graphic>();
            if (GraphicProjectsCartCookie != null && !string.IsNullOrEmpty(GraphicProjectsCartCookie))
            {
                model.GraphicsIdsInCart = GraphicProjectsCartCookie.Split('-').Select(id => int.Parse(id)).ToList();
                foreach (var productId in model.GraphicsIdsInCart)
                {
                    var graphic = await _graphicsForYouApiService.GetSingleGraphic(productId);
                    graphicList.Add(graphic);
                }
                model.GraphicsInCart = graphicList;
            }
            return View(model);
        }

        public async Task<IActionResult> UserOrders()
        {

            int id = await _graphicsForYouApiService.GetUserId(User.FindFirstValue(ClaimTypes.Email));
            var orderList = await _graphicsForYouApiService.GetUserOrders(id);
            return View(orderList);
        }
        public async Task<int> PlaceOrder(string orderr)
        {
            var obiektList = JsonConvert.DeserializeObject<List<OrderItem>>(orderr);
            int orderId = 0;
            Order newOrder = new Order();

            newOrder.UserId = await _graphicsForYouApiService.GetUserId(User.FindFirstValue(ClaimTypes.Email));
            newOrder.OrderDate = DateTime.Now;
            newOrder.OrderStatus = OrderStatus.Nieopłacone;


           var productQuantities = new List<int>();
            var boughtProducts = new List<Graphic>();
            foreach (var productId in obiektList)
            {
                productId.Price = productId.Price / 100;
                var graphic = await _graphicsForYouApiService.GetSingleGraphic(productId.GraphicId);
                boughtProducts.Add(graphic);
                productQuantities.Add(productId.GraphicId);
            }

            newOrder.TotalAmount = boughtProducts.Sum(x => x.PromotionPrice * productQuantities.Where(productId => productId == x.Id).Count());

            newOrder.OrderItems = new List<OrderItem>();
            newOrder.OrderItems.AddRange(obiektList.Select(x => new OrderItem() { GraphicId = x.GraphicId, OrderId = x.GraphicId, Price = x.Price, Description = x.Description, Contact = x.Contact, Format = x.Format}));
            try
            {
                orderId = await _graphicsForYouApiService.AddOrder(newOrder);
            }
            catch (Exception)
            {
            }

            return orderId;
        
        }

        public async Task<IActionResult> Summary()
        {

            ShopViewModel model = new ShopViewModel();
            var GraphicProjectsCartCookie = Request.Cookies["GraphicsInCart"];

            if (GraphicProjectsCartCookie != null && !string.IsNullOrEmpty(GraphicProjectsCartCookie))
            {
            
                model.GraphicsIdsInCart = GraphicProjectsCartCookie.Split('-').Select(i => int.Parse(i)).ToList();
                var graphicList = new List<Graphic>();
                foreach (var productId in model.GraphicsIdsInCart)
                {
                    var graphic = await _graphicsForYouApiService.GetSingleGraphic(productId);
                    graphicList.Add(graphic);
                }
                model.GraphicsInCart = graphicList;
             
                model.User = await _graphicsForYouApiService.GetUser(User.FindFirstValue(ClaimTypes.Email));
                if(model.User == null)
                {
                    return RedirectToAction("Login", "Account", new { ifCheckout = true });
                }
            }
            return View(model);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _graphicsForYouApiService.GetOrder(id);
          
            var user = new User();
            user = await _graphicsForYouApiService.GetUserById(order.UserId);

            var orderItems = await _graphicsForYouApiService.GetOrderItems(id);

            dynamic dy = new ExpandoObject();
            dy.orderItemsList = orderItems;

            dy.order = order;
            dy.user = user;

            var orderFiles = await _graphicsForYouApiService.GetOrderFiles(order.Id);
            dy.orderFiles = orderFiles;
            return View(dy);
        }

        public async Task<IActionResult> OrderDetailsDesigner(int id)
        {
            var order = await _graphicsForYouApiService.GetOrder(id);

            var user = new User();
            user = await _graphicsForYouApiService.GetUserById(order.UserId);

            var orderItems = await _graphicsForYouApiService.GetOrderItems(id);

            dynamic dy = new ExpandoObject();
            dy.orderItemsList = orderItems;

            dy.order = order;
            dy.user = user;
            var orderFiles = await _graphicsForYouApiService.GetOrderFiles(order.Id);
            dy.orderFiles = orderFiles;
            return View(dy);
        }

        public async Task<IActionResult> OrdersManagement()
        {
            var orders = await _graphicsForYouApiService.GetOrders();
            return View(orders);
        }

        public async Task<IActionResult> ChangeOrderStatus(Order order)
        {
            _graphicsForYouApiService.ChangeOrderStatus(order);
            return RedirectToAction("OrdersManagement");
        }

        public async Task<IActionResult> AddFilesToOrder(OrderFileViewModel model)
        {
            foreach (var file in model.Files)
            {
                if(file != null)
                {
                    if(file.Length > 0)
                    {
                        var orderFile = new OrderFile();
                        using (var reader = new MemoryStream())
                        {
                            file.CopyTo(reader);
                            orderFile.Name = file.FileName;
                            orderFile.GraphicFile = reader.ToArray();
                            orderFile.Type = file.ContentType;
                            orderFile.OrderId = model.OrderId;
                            orderFile.Date = DateTime.Now;
                        }

                        await _graphicsForYouApiService.AddOrderFile(orderFile);
                    }
                }
            }
            return RedirectToAction("OrdersManagement");

        }
        public async Task<IActionResult> FilesSite(int id)
        {
            var files = new OrderFileViewModel();
            files.OrderId = id;
            return View(files);
        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            var orderFile = await _graphicsForYouApiService.GetOrderFile(id);
            if (orderFile == null)
            {
                return NotFound();
            }
            else
            {
                byte[] byteTab = orderFile.GraphicFile;
                string contentType = orderFile.Type;
                return new FileContentResult(byteTab, contentType)
                {
                    FileDownloadName = orderFile.Name
                };     
            }
        }

        public async Task<IActionResult> DownloadMessageFile(int id)
        {
            var file = await _graphicsForYouApiService.GetMessage(id);
            if (file == null)
            {
                return NotFound();
            }
            else
            {
                byte[] byteTab = file.FileBytes;
                string contentType = file.FileType;
                return new FileContentResult(byteTab, contentType)
                {
                    FileDownloadName = file.FileName
                };
            }
        }

    }
}
