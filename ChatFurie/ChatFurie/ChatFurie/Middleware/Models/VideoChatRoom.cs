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
        public bool InChat { get; set; }
    }


    public class VideoChatRoom
    {
        public VideoChatRoom(int conversation)
        {
            Conversation = conversation;
        }

        private List<Client> Users { get; set; } = new List<Client>();
        public Client Creator { get; set; }
        public int Conversation { get; set; }

        public Client AddUser(WebSocket ws, int id)
        {
            var client = new Client { Id = id, Socket = ws, IsActive = false };
            Users.Add(client);
            return client;
        }

        public void StartVideoChat()
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            var initUser = Users.First().Id;
            foreach (var user in chatService.GetUsersInConversation(Conversation, initUser))
                if (MessageSocketTransform.Sockets.TryGetValue(user, out var socket) && user != initUser)
                    this.AddUser(socket, user);
            JObject jObject = new JObject();
            jObject["conversation"] = Conversation;
            jObject["type"] = "notification";
            jObject["method"] = (int)SocketHandler.NotificationType.CallStart;
            foreach (var users in Users.Where(x => !x.IsActive))
            {
                jObject["name"] = chatService.GetConversation(users.Id, Conversation).Name;
                MessageSocketTransform.SendStringAsync(users.Socket, jObject.ToString());
            }
        }

        public void SetActive(int user)
        {
            if (Users.FirstOrDefault(x => x.Id == user) == null)
                return;
            JObject jObject = new JObject();
            jObject["conversation"] = Conversation;
            jObject["type"] = "notification";
            jObject["user"] = user;
            jObject["method"] = (int)SocketHandler.NotificationType.UserIdReady;
            foreach (var userActive in Users.Where(x => x.IsActive && !x.InChat))
                MessageSocketTransform.SendStringAsync(userActive.Socket, jObject.ToString());
            Users.FirstOrDefault(x => x.Id == user).IsActive = true;
        }

        public void DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
                Users.Remove(user);
        }

        public bool NeedToDelete => Count < 2;

        public void Deleting(CancellationToken ct)
        {
            JObject jObject = new JObject();
            jObject["conversation"] = Conversation;
            jObject["method"] = (int)SocketHandler.NotificationType.Stop;
            jObject["type"] = "notification";
            foreach (var user in Users)
                MessageSocketTransform.SendStringAsync(user.Socket, jObject.ToString(), ct);
        }

        public void VideoChat(JObject jObject, CancellationToken ct, int userFrom = 0)
        {
            var user = jObject["user"].Value<int>();
            foreach (var users in Users.Where(x => x.IsActive && x.Id != user))
                if (users.Id != userFrom)
                    MessageSocketTransform.SendStringAsync(users.Socket, jObject.ToString(), ct);
        }

        public bool IsUserInRoom(int id) => Users.FirstOrDefault(x => x.Id == id) != null;
        public int Count => Users.Count;
        public int ActiveCount => Users.Where(x => x.IsActive).Count();
    }
}
