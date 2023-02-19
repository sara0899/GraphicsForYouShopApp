using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GraphicsForYouShopApp.Data;
using GraphicsForYouShopApp.Models;
using Newtonsoft.Json;
using GraphicsForYouShopApp.Services;

namespace GraphicsForYouShopApp.Controllers
{

    public class CommunicatorController : Controller
    {
        private readonly IGraphicsApiService _graphicsApiService;

        public CommunicatorController(IGraphicsApiService graphicsApiService)
        {
            _graphicsApiService = graphicsApiService;
        }

        [Authorize(Roles = "Designer")]
        public async Task<IActionResult> CommunicatorSignal()
        {
            List<int> usersList = new List<int>();

            var users = await _graphicsApiService.GetUsersListUnreadMessages();
            usersList.AddRange(users);

            users = await _graphicsApiService.GetUsersList(OrderStatus.Realizowane);
            foreach (var u in users)
            {
                if (!usersList.Contains(u))
                {
                    usersList.Add(u);
                }
            };

            users = await _graphicsApiService.GetUsersList(OrderStatus.Nieopłacone);
            foreach (var u in users)
            {
                if (!usersList.Contains(u))
                {
                    usersList.Add(u);
                }
            };

            users = await _graphicsApiService.GetUsersList(OrderStatus.Zrealizowane);
            foreach (var u in users)
            {
                if (!usersList.Contains(u))
                {
                    usersList.Add(u);
                }
            };

            users = await _graphicsApiService.GetUsersList(OrderStatus.Anulowane);
            foreach (var u in users)
            {
                if (!usersList.Contains(u))
                {
                    usersList.Add(u);
                }
            };
          
            users = await _graphicsApiService.GetAllUsers();
            foreach (var u in users)
            {
                if (!usersList.Contains(u))
                {
                    usersList.Add(u);
                }
            };

            var countOrderList = new List<CountOrderViewModel>();
            foreach (var user in usersList)
            {
                var countOrder = new CountOrderViewModel();
                countOrder.Id = user;

                var userEmail = await _graphicsApiService.GetUserById(user);
                countOrder.Name = userEmail.Email;


                var countOrderWithStatus = await _graphicsApiService.GetCountOrder(user,OrderStatus.Realizowane);
                countOrder.InProgressOrders = countOrderWithStatus;

                countOrderWithStatus = await _graphicsApiService.GetCountOrder(user,OrderStatus.Nieopłacone);
                countOrder.UnpaidOrders = countOrderWithStatus;

                countOrderWithStatus = await _graphicsApiService.GetCountOrder(user, OrderStatus.Zrealizowane);
                countOrder.RealisedOrders = countOrderWithStatus;

                countOrderWithStatus = await _graphicsApiService.GetCountOrder(user, OrderStatus.Anulowane);
                countOrder.CancelledOrders = countOrderWithStatus;

                var messagesCount = await _graphicsApiService.GetUnreadMessagesToDesigner(user);
                countOrder.MessagesCount = messagesCount;

                countOrderList.Add(countOrder);
            }

            return View(countOrderList);
        }

    }
}
