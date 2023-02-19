using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using GraphicsForYouShopApi.Models;

namespace GraphicsForYouShopApi.Data
{
    public class CommunicatorHub : Hub
    {
        private GraphicsDbContext context { get; }

        public CommunicatorHub(GraphicsDbContext context)
        {
            this.context = context;
        }

        public void SendChatMessage(int senderId, int who, string textMessage, string files)
        {
            var name = Context.User.Identity.Name;

            int id = who;
            var user = context.Users.Where(k => k.Id == id).FirstOrDefault();
            var sender = context.Users.Where(k => k.Id == senderId).FirstOrDefault();
            if (user != null)
            {
                Message message = new Message();

                message.SenderId = senderId;
                message.MessageText = textMessage;
                message.ReceiverId = who;
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

                context.Entry(user)
                    .Collection(u => u.Connections)
                    .Query()
                    .Where(c => c.Connected == true)
                    .Load();

                context.Entry(sender)
                    .Collection(u => u.Connections)
                    .Query()
                    .Where(c => c.Connected == true)
                    .Load();

                if (user.Connections != null)
                {

                    foreach (var connection in user.Connections)
                    {
                        Clients.Client(connection.ConnectionID)
                            .SendAsync("ReceiveMessage", id, senderId, textMessage);


                    }
                }

                if (sender.Connections != null)
                {

                    foreach (var connection in sender.Connections)
                    {
                        Clients.Client(connection.ConnectionID)
                            .SendAsync("ReceiveMessage", id, senderId, textMessage);

                    }
                }
            }

        }

        [HubMethodName("OnConnected")]
        public string OnConnected(int senderId)
        {
            var user = context.Users
                    .Include(u => u.Connections)
                    .SingleOrDefault(u => u.Id == senderId);

            var conn = Context.ConnectionId;

            user.Connections.Add(new Connection
            {
                ConnectionID = conn,
                Connected = true
            });
            context.SaveChanges();

            return conn;
        }

        public void OnDisconnected(int id)
        {
            var deleteConnection = context.Connections.Where(c => c.UserId == id).ToList();
            foreach (var conn in deleteConnection)
            {
                context.Connections.Remove(conn);
            }
            context.SaveChanges();
        }

    }
}
