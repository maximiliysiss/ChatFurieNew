using ChatFurie.Middleware.Sockets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatFurie.Middleware.Models
{
    public class Client
    {
        public WebSocket Socket { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }


    public class VideoChatRoom
    {
        private List<Client> Users { get; set; } = new List<Client>();
        public Client Creator { get; set; }
        public int Conversation { get; set; }
        public Client AddUser(WebSocket ws, int id)
        {
            var client = new Client { Id = id, Socket = ws, IsActive = false };
            Users.Add(client);
            return client;
        }

        public void SetActive(int user)
        {
            Users.FirstOrDefault(x => x.Id == user).IsActive = true;
            var userActives = Users.Where(x => x.IsActive);
            if (userActives.Count() > 0)
                foreach (var userActive in userActives)
                {
                    JObject jObject = new JObject();
                    jObject["conversation"] = Conversation;
                    jObject["method"] = (int)SocketHandler.NotificationType.RoomIsReady;
                    MessageSocketTransform.SendStringAsync(userActive.Socket, jObject.ToString());
                }
        }

        public void DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
                Users.Remove(user);
        }

        public bool NeedToDelete()
        {
            if (Count == 0 || Count == 1)
                return true;

            if (Users.Where(x => x.IsActive).Count() <= 1
                && Users.FirstOrDefault(x => x.IsActive) != Creator)
                return true;

            return false;
        }

        public void Deleting(int conversation, CancellationToken ct)
        {
            JObject jObject = new JObject();
            jObject["conversation"] = conversation;
            jObject["method"] = (int)SocketHandler.NotificationType.CallEnd;
            foreach (var user in Users)
                MessageSocketTransform.SendStringAsync(user.Socket, jObject.ToString(), ct);
        }

        public void VideoChat(JObject jObject, CancellationToken ct, int userFrom = 0)
        {
            foreach (var user in Users)
                if (user.Id != userFrom)
                    MessageSocketTransform.SendStringAsync(user.Socket, jObject.ToString(), ct);
        }
        public bool IsUser(int id)
        {
            return Users.FirstOrDefault(x => x.Id == id) != null;
        }
        public int Count { get => Users.Count; }
    }
}
