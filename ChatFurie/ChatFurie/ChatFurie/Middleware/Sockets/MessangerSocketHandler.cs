using ChatFurie.Middleware.Models;
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
        public enum NotificationType
        {
            Common, Conversation, CallStart, CallEnd, CallAccess, CallDecline, Backway, RoomIsReady
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

        public static void Notification(string method, JObject jObject, int userId, CancellationToken ct)
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
                        ChatWCF.Models.ChatContextWCF chatContextWCF = new ChatWCF.Models.ChatContextWCF();
                        jObject["name"] = chatContextWCF.Conversation.Find(conversation).Name;
                        var users = chatContextWCF.UserInConversation
                            .Where(x => x.ConversationID == conversation && x.UserID != userId).Select(x => x.UserID);
                        List<int> usersList = new List<int>();
                        bool isStart = false;
                        VideoChatRoom videoChatRoom = null;
                        if (!MessageSocketTransform.ChatRooms.TryGetValue(conversation, out videoChatRoom))
                        {
                            videoChatRoom = new VideoChatRoom() { Conversation = conversation };
                            MessageSocketTransform.ChatRooms.Add(conversation, videoChatRoom);
                        }
                        foreach (var user in users)
                            if (MessageSocketTransform.Sockets.TryGetValue(user, out var socket))
                            {
                                MessageSocketTransform.SendStringAsync(socket, jObject.ToString(), ct);
                                isStart = true;
                                usersList.Add(user);
                                videoChatRoom.AddUser(socket, user);
                            }
                        if (MessageSocketTransform.Sockets.TryGetValue(userId, out var userSocket))
                        {
                            var client = videoChatRoom.AddUser(userSocket, userId);
                            videoChatRoom.SetActive(userId);
                            videoChatRoom.Creator = client;
                            jObject["method"] = (int)NotificationType.Backway;
                            jObject["isStart"] = isStart;
                            jObject["users"] = string.Join(",", usersList);
                            if (!isStart)
                                MessageSocketTransform.SendStringAsync(userSocket, jObject.ToString(), ct);
                        }
                        break;
                    }
                case NotificationType.CallEnd:
                    {
                        int conversation = jObject["conversation"].Value<int>();
                        int user = jObject["userStart"].Value<int>();
                        int userFrom = jObject["userFrom"].Value<int>();
                        if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                            room.DeleteUser(userFrom);
                        MessageSocketTransform.ValidateRoom(conversation, ct);
                        break;
                    }
                case NotificationType.CallAccess:
                    {
                        int conversation = jObject["conversation"].Value<int>();
                        int user = jObject["userStart"].Value<int>();
                        int userFrom = jObject["userFrom"].Value<int>();
                        if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                            room.SetActive(userFrom);
                    }
                    break;
                case NotificationType.CallDecline:
                    {
                        int conversation = jObject["conversation"].Value<int>();
                        int user = jObject["userDecliner"].Value<int>();
                        if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                            room.DeleteUser(user);
                        MessageSocketTransform.ValidateRoom(conversation, ct);
                    }
                    break;
            }
        }

        public static void VideoMessage(string method, JObject jObject, int userId, CancellationToken ct)
        {
            int conversation = jObject["conversation"].Value<int>();
            if (MessageSocketTransform.ChatRooms.TryGetValue(conversation, out var room))
                room.VideoChat(jObject, ct, userId);
        }
    }
}
