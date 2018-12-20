using ChatFurie.Middleware.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatFurie.Middleware.Sockets
{
    public class SocketHandler
    {
        public enum NotificationType
        {
            Common, Conversation, CallStart, CallAccess, Stop, UserIdReady
        }

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

        public static void Notification(string method, WebSocket userSocket, JObject jObject, int userId, CancellationToken ct)
        {
            switch ((NotificationType)Convert.ToInt32(method))
            {
                case NotificationType.Common:
                case NotificationType.Conversation:
                    {
                        int userTo = jObject["userTo"].Value<int>();
                        if (MessageSocketTransform.Sockets.TryGetValue(userTo, out var socket))
                            MessageSocketTransform.SendStringAsync(socket, jObject.ToString(), ct);
                        break;
                    }
                case NotificationType.CallStart:
                    {
                        int conversation = jObject["conversation"].Value<int>();
                        if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                        {
                            room.AddUser(userSocket, userId);
                            room.SetActive(userId);
                        }
                        else
                        {
                            VideoChatRoom videoChat = new VideoChatRoom(conversation);
                            videoChat.AddUser(userSocket, userId);
                            videoChat.SetActive(userId);
                            videoChat.StartVideoChat();
                            if (videoChat.NeedToDelete)
                                videoChat.Deleting(ct);
                            else
                                MessageSocketTransform.ChatRooms.Add(conversation, videoChat);
                        }
                        break;
                    }
                case NotificationType.Stop:
                    {
                        int conversation = jObject["conversation"].Value<int>();
                        int user = jObject["user"].Value<int>();
                        if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                        {
                            room.DeleteUser(user);
                            if (room.NeedToDelete)
                            {
                                room.Deleting(ct);
                                MessageSocketTransform.ChatRooms.Remove(conversation);
                            }
                        }
                        break;
                    }
                case NotificationType.CallAccess:
                    {
                        int conversation = jObject["conversation"].Value<int>();
                        int user = jObject["user"].Value<int>();
                        if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                            room.SetActive(user);
                        break;
                    }
            }
        }

        public static void VideoMessage(string method, JObject jObject, int userId, CancellationToken ct)
        {
            Console.WriteLine($"Input from user {userId}: {jObject["data"].Value<string>().Length}");
            int conversation = jObject["conversation"].Value<int>();
            if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                room.VideoChat(jObject, ct, userId);
        }
    }
}
