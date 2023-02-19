using GraphicsForYouShopApp.Data;
using GraphicsForYouShopApp.Data.Enums;
using GraphicsForYouShopApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace GraphicsForYouShopApp.Services
{
    public class GraphicsApiService : IGraphicsApiService
    {
        private readonly HttpClient _graphicsApiClient;
        public GraphicsApiService(HttpClient graphicsApiClient)
        {
            _graphicsApiClient = graphicsApiClient;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategoryList()
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetCategoryList");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(jsonString);
            return categoryList;
        }

        [HttpGet]
        public async Task<GraphicViewModel> GetGraphicById(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetGraphicById/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphic = JsonConvert.DeserializeObject<GraphicViewModel>(jsonString);
            return graphic;
        }

        [HttpGet]
        public async Task<IEnumerable<Graphic>> GetGraphicsByCategoryId(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetGraphicsByCategoryId/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphicList = JsonConvert.DeserializeObject<List<Graphic>>(jsonString);
            return graphicList;
            
        }

        [HttpGet]
        public async Task<IEnumerable<Graphic>> GetGraphicsByCollectionId(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetGraphicsByCollectionId/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphicList = JsonConvert.DeserializeObject<List<Graphic>>(jsonString);
            return graphicList;
        }

        [HttpGet]
        public async Task<IEnumerable<Graphic>> GetGraphicsFromCollection(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetGraphicsFromCollection/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphicList = JsonConvert.DeserializeObject<List<Graphic>>(jsonString);
            return graphicList;
        }

        [HttpGet]
        public async Task<IEnumerable<Collection>> GetCollectionList()
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetCollectionList");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var collectionList = JsonConvert.DeserializeObject<List<Collection>>(jsonString);
            return collectionList;
        }

        [HttpPost]
        public async Task AddPromotionToAll(Promotion promotion)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(promotion), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PostAsync("Graphic/AddPromotionToAll", stringContentToApi);
        }

        [HttpPost]
        public async Task AddPromotionToGraphic(Promotion promotion)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(promotion), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PostAsync("Graphic/AddPromotionToGraphic", stringContentToApi);
        }

        [HttpPost]
        public async Task AddGraphic(GraphicViewModel graphic)
        {
            var newGraphic = new Graphic()
            {
                Name = graphic.Name,
                Description = graphic.Description,
                Price = graphic.Price,
                PromotionPrice = graphic.Price,
                CategoryId = graphic.CategoryId,
                CollectionId = graphic.CollectionId,
                MainPictureUrl = graphic.MainPictureUrl,
                Availability = GraphicAvailability.Dostępna,
            };

            newGraphic.graphicPictures = new List<GraphicPictures>();
            if (graphic.PicturesList != null)
            {
                foreach (Picture file in graphic.PicturesList)
                {
                    newGraphic.graphicPictures.Add(new GraphicPictures()
                    {
                        Name = file.Name,
                        Url = file.Url
                    });
                }
            }

            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(newGraphic), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PostAsync("Graphic/AddGraphic", stringContentToApi);
        }

        [HttpGet]
        public async Task<IEnumerable<Graphic>> GetAllGraphics()
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetAllGraphics");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphicList = JsonConvert.DeserializeObject<List<Graphic>>(jsonString);
            return graphicList;
        }

        [HttpGet]
        public async void DeletePicture(int id)
        {
            await _graphicsApiClient.GetAsync($"Graphic/DeletePicture/{id}");
        }

        [HttpGet]
        public async Task<List<PromotionViewModel>> GetPromotions()
        {
            var responseGraphics = await _graphicsApiClient.GetAsync($"Graphic/GetAllGraphics");
            var responsePromotions = await _graphicsApiClient.GetAsync($"Graphic/GetPromotions");
            var promotionList = new List<Promotion>();
            var graphicList = new List<Graphic>();
            if (responseGraphics.IsSuccessStatusCode)
            {
                var jsonString = await responseGraphics.Content.ReadAsStringAsync();
                graphicList = JsonConvert.DeserializeObject<List<Graphic>>(jsonString);
            }

            if (responsePromotions.IsSuccessStatusCode)
            {
                var jsonString = await responsePromotions.Content.ReadAsStringAsync();
                promotionList = JsonConvert.DeserializeObject<List<Promotion>>(jsonString);
            }

            var promotionModels = new List<PromotionViewModel>();

            if (graphicList != null)
            {
                foreach (var graphic in graphicList)
                {
                    var promotionListForGraphic = promotionList.FindAll(g => g.GraphicId == graphic.Id);
                    if (promotionListForGraphic.Any())
                    {
                        promotionModels.Add(new PromotionViewModel()
                        {
                            GraphicName = graphic.Name,
                            Price = graphic.Price,
                            PromotionPrice = graphic.PromotionPrice,
                            GraphicId = graphic.Id,
                            MainPictureUrl = graphic.MainPictureUrl,
                            Promotions = promotionListForGraphic
                        });
                    }
                }
            }
            return promotionModels;
        }

        [HttpPut]
        public async void ChangeOrderStatus(Order order)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PutAsync("Order/ChangeOrderStatus", stringContentToApi);
        }

        //ACCOUNT METHODS

        [HttpPost]
        public async Task Register(User user)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PostAsync("Account/Register", stringContentToApi);
        }


        [HttpGet]
        public async Task<int> GetUserId(string email)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Account/GetUserId/{email}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<int>(jsonString);
            return id;
        }

        [HttpGet]
        public async Task<User> GetUser(string email)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Account/GetUser/{email}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(jsonString);
            return user;
        }

        [HttpGet]
        public async Task<string> GetRoleName(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Account/GetRoleName/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            return jsonString;
        }

        [HttpGet]
        public async Task<User> GetUserById(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Account/GetUserById/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(jsonString);
            return user;
        }

        [HttpGet]
        public async Task<int> GetUnreadMessagesCount(string email)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Account/GetUnreadMessagesCount/{email}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var count = JsonConvert.DeserializeObject<int>(jsonString);
            return count;
        }

        [HttpPut]
        public async Task EditProfile(RegisterViewModel user)
        {
            var newUser = new User()
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
                Email = user.Email,
            };

            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PutAsync("Account/EditProfile", stringContentToApi);
        }

        [HttpPut]
        public async Task EditPassword(RegisterViewModel user)
        {
            var newUser = new User()
            {
                Id = user.UserId,
                Email = user.Email,
                Password = user.Password,
            };

            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PutAsync("Account/EditPassword", stringContentToApi);
        }

        //ORDERS

        [HttpGet]
        public async Task<Order> GetOrder(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Order/GetOrder/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<Order>(jsonString);
            return order;
        }

        [HttpGet]
        public async Task<List<Order>> GetUserOrders(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Order/GetUserOrders/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var orderList = JsonConvert.DeserializeObject<List<Order>>(jsonString);
            return orderList;
        }

        [HttpGet]
        public async Task<List<Graphic>> GetOrderItems(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Order/GetOrderItems/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphicList = JsonConvert.DeserializeObject<List<Graphic>>(jsonString);
            return graphicList;
        }

        [HttpGet]
        public async Task<Graphic> GetSingleGraphic(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Order/GetSingleGraphic/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphic = JsonConvert.DeserializeObject<Graphic>(jsonString);
            return graphic;
        }

        [HttpGet]
        public async Task<List<Order>> GetOrders()
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Order/GetOrders");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var orderList = JsonConvert.DeserializeObject<List<Order>>(jsonString);
            return orderList;
        }

        [HttpPost]
        public async Task<int> AddOrder(Order order)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
            var response = await _graphicsApiClient.PostAsync("Order/AddOrder", stringContentToApi);
            var jsonString = await response.Content.ReadAsStringAsync();
            var orderId = JsonConvert.DeserializeObject<int>(jsonString);
            return orderId;
        }

        [HttpPut]
        public async Task ChangeMessageReading(string email)
        {
           var stringContentToApi = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PutAsync("Account/ChangeMessageReading", stringContentToApi);
        }

        [HttpPost]
        public async Task CreateCollection(Collection collection)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(collection), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PostAsync("Graphic/CreateCollection", stringContentToApi);
        }

        [HttpGet]
        public async Task<Collection> GetCollection(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/GetCollection/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var collection = JsonConvert.DeserializeObject<Collection>(jsonString);
            return collection;
        }

        [HttpGet]
        public async Task<List<OrderFile>> GetOrderFiles(int id)
        {
            var response = await _graphicsApiClient.GetAsync($"Order/GetOrderFiles/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var orderFileList = JsonConvert.DeserializeObject<List<OrderFile>>(jsonString);
            return orderFileList;
        }

        [HttpGet]
        public async Task<OrderFile> GetOrderFile(int id)
        {
            var response = await _graphicsApiClient.GetAsync($"Order/GetOrderFile/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var orderFile = JsonConvert.DeserializeObject<OrderFile>(jsonString);
            return orderFile;
        }

        [HttpPut]
        public async Task EditCollection(Collection collection)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(collection), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PutAsync("Graphic/EditCollection", stringContentToApi);
        }

        [HttpPost]
        public async Task AddOrderFile(OrderFile file)
        {
            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PostAsync("Order/AddOrderFile", stringContentToApi);
        }

        [HttpGet]
        public async Task<Message> GetMessage(int id)
        {
            var response = await _graphicsApiClient.GetAsync($"Communicator/GetMessage/{id}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<Message>(jsonString);
            return message;
        }

        [HttpGet]
        public async Task<List<Message>> GetMessages(int receiverId, int senderId)
        {
            var response = await _graphicsApiClient.GetAsync($"Account/GetMessages/{receiverId},{senderId}");
            var jsonString = await response.Content.ReadAsStringAsync();
            var message = JsonConvert.DeserializeObject<List<Message>>(jsonString);
            return message;
        }


        [HttpGet]
        public async Task<List<Graphic>> Search(string key)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Graphic/Search/{key}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var graphicList = JsonConvert.DeserializeObject<List<Graphic>>(jsonString);
            return graphicList;
        }

        [HttpGet]
        public async Task<List<int>> GetUsersListUnreadMessages()
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Communicator/GetUsersListUnreadMessages");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var userList = JsonConvert.DeserializeObject<List<int>>(jsonString);
            return userList;
        }

        [HttpGet]
        public async Task<List<int>> GetUsersList(OrderStatus status)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Communicator/GetUsersList/{status}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var userList = JsonConvert.DeserializeObject<List<int>>(jsonString);
            return userList;
        }

        [HttpGet]
        public async Task<List<int>> GetAllUsers()
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Communicator/GetAllUsers");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var userList = JsonConvert.DeserializeObject<List<int>>(jsonString);
            return userList;
        }

        [HttpGet]
        public async Task<int> GetCountOrder(int id, OrderStatus status)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Communicator/GetCountOrder/{id},{status}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var countOrder= JsonConvert.DeserializeObject<int>(jsonString);
            return countOrder;
        }

        [HttpGet]
        public async Task<int> GetUnreadMessagesToDesigner(int id)
        {
            var responseFromApi = await _graphicsApiClient.GetAsync($"Account/GetUnreadMessagesToDesigner/{id}");
            var jsonString = await responseFromApi.Content.ReadAsStringAsync();
            var countMessage = JsonConvert.DeserializeObject<int>(jsonString);
            return countMessage;
        }

        [HttpPut]
        public async Task Edit(GraphicViewModel graphic)
        {
            var newGraphic = await GetSingleGraphic(graphic.Id);

            if(newGraphic != null)
            {
                newGraphic.Name = graphic.Name;
                newGraphic.Description = graphic.Description;
                newGraphic.Price = graphic.Price;
                newGraphic.PromotionPrice = graphic.Price;
                newGraphic.CategoryId = graphic.CategoryId;
                newGraphic.CollectionId = graphic.CollectionId;
                newGraphic.MainPictureUrl = graphic.MainPictureUrl;
                newGraphic.Availability = GraphicAvailability.Dostępna;

                newGraphic.graphicPictures = new List<GraphicPictures>();
                if (graphic.PicturesList != null)
                {
                    foreach (Picture file in graphic.PicturesList)
                    {
                        newGraphic.graphicPictures.Add(new GraphicPictures()
                        {
                            Name = file.Name,
                            Url = file.Url
                        });
                    }
                }
            }

            var stringContentToApi = new StringContent(JsonConvert.SerializeObject(newGraphic), Encoding.UTF8, "application/json");
            await _graphicsApiClient.PutAsync("Graphic/Edit", stringContentToApi);
        }
    }
}

















