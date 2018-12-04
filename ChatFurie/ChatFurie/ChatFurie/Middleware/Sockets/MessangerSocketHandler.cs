using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatFurie.Middleware.Sockets
{
    public class SocketHandler
    {
        public static ChatWCF.ChatService ChatService = new ChatWCF.ChatService();

        public static async void Messanger(string method, JObject jObject, int userId, CancellationToken ct)
        {
            switch (method)
            {
                case "sendMessage":
                    {
                        ChatWCF.Models.ChatContextWCF chatContextWCF = new ChatWCF.Models.ChatContextWCF();
                        int conversation = jObject["conversation"].Value<int>();
                        string content = jObject["content"].Value<string>();
                        var usersList = ChatService.GetUsersInConversation(conversation, userId);

                        int id = await ChatService.SendMessageToUser(conversation, userId, content);
                        jObject["date"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        jObject["user"] = chatContextWCF.Users.Find(userId).Name;
                        jObject["author"] = userId;
                        jObject["id"] = id;
                        foreach (var userForSending in MessageSocketTransform.Sockets.Where(x => usersList.Contains(x.Key)))
                        {
                            await MessageSocketTransform.SendStringAsync(userForSending.Value, jObject.ToString(), ct);
                        }
                    }
                    break;
            }
        }

        public static async void Notification(string method, JObject jObject, int userId, CancellationToken ct)
        {
            int userTo = jObject["userTo"].Value<int>();
            if (MessageSocketTransform.Sockets.TryGetValue(userTo, out var socket))
                await MessageSocketTransform.SendStringAsync(socket, jObject.ToString(), ct);
        }

        public static async void VideoMessage(string method, JObject jObject, int userId, CancellationToken ct)
        {

        }
    }
}
