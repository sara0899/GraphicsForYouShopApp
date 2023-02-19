using GraphicsForYouShopApp.Data;
using GraphicsForYouShopApp.Models;

namespace GraphicsForYouShopApp.Services
{
    public interface IGraphicsApiService
    {
        Task<IEnumerable<Category>> GetCategoryList();
        Task<GraphicViewModel> GetGraphicById(int id);

        Task<IEnumerable<Graphic>> GetGraphicsByCategoryId(int id);
        Task<IEnumerable<Graphic>> GetGraphicsByCollectionId(int id);

        Task<IEnumerable<Graphic>> GetGraphicsFromCollection(int id);

        Task<IEnumerable<Collection>> GetCollectionList();
        Task AddGraphic(GraphicViewModel graphic);
        Task<IEnumerable<Graphic>> GetAllGraphics();
        void DeletePicture(int id);

        Task<int> GetUserId(string email);
        Task<User> GetUser(string email);

        Task<string> GetRoleName(int id);
        Task<User> GetUserById(int id);

        Task Register(User user);

        Task<Order> GetOrder(int id);
        Task<List<Order>> GetUserOrders(int id);

        Task<List<Graphic>> GetOrderItems(int id);

        Task<Graphic> GetSingleGraphic(int id);
        Task<List<Order>> GetOrders();
        Task<int> AddOrder(Order order);
        Task EditProfile(RegisterViewModel user);

        void ChangeOrderStatus(Order order);

        Task<int> GetUnreadMessagesCount(string email);
        Task ChangeMessageReading(string email);

        Task CreateCollection(Collection collection);
        Task<Collection> GetCollection(int id);

        Task EditCollection(Collection collection);
        Task AddOrderFile(OrderFile file);
        Task<List<OrderFile>> GetOrderFiles(int id);
        Task<OrderFile> GetOrderFile(int id);
        Task<Message> GetMessage(int id);
        Task<List<Graphic>> Search(string key);
        Task<List<PromotionViewModel>> GetPromotions();

        Task AddPromotionToAll(Promotion promotion);
        Task AddPromotionToGraphic(Promotion promotion);
        Task<List<Message>> GetMessages(int receiverId, int senderId);
        Task<List<int>> GetUsersListUnreadMessages();
        Task<List<int>> GetUsersList(OrderStatus status);

        Task<List<int>> GetAllUsers();
        Task<int> GetCountOrder(int id, OrderStatus status);
        Task<int> GetUnreadMessagesToDesigner(int id);

        Task Edit(GraphicViewModel graphic);
        Task EditPassword(RegisterViewModel user);
    }
}
