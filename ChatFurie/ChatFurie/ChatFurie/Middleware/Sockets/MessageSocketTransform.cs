using ChatFurie.Middleware.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatFurie.Middleware.Sockets
{

    public class MessageSocketTransform
    {
        private static ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>();
        private static Dictionary<int, VideoChatRoom> _chatRooms = new Dictionary<int, VideoChatRoom>();

        public static Dictionary<int, VideoChatRoom> ChatRooms => _chatRooms;
        public static ConcurrentDictionary<int, WebSocket> Sockets => _sockets;
        private ChatWCF.ChatService ChatService = new ChatWCF.ChatService();

        private readonly RequestDelegate _next;

        public static void DeleteUserFromChatRoom(int ws)
        {
            foreach (var room in ChatRooms.Where(x => x.Value.IsUserInRoom(ws)).ToList())
            {
                room.Value.DeleteUser(ws);
                if (room.Value.NeedToDelete)
                {
                    room.Value.Deleting(default(CancellationToken));
                    ChatRooms.Remove(room.Key);
                }
            }
        }

        public static void ValidateRoom(int conversation, CancellationToken ct)
        {
            if (ChatRooms.TryGetValue(conversation, out var room))
            {
                if (room.NeedToDelete)
                {
                    room.Deleting(ct);
                    ChatRooms.Remove(conversation);
                }
            }
        }

        public static void DeleteUserFromChatRoom(int ws, int conversation)
        {
            if (ChatRooms.TryGetValue(conversation, out var room))
            {
                room.DeleteUser(ws);
                if (room.Count == 0)
                    ChatRooms.Remove(conversation);
            }
        }

        public MessageSocketTransform(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            CancellationToken ct = context.RequestAborted;
            WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
            var response = await ReceiveStringAsync(currentSocket, 0, ct);
            JObject jObject = JObject.Parse(response);
            int userId = jObject["user"].Value<int>();

            _sockets.TryAdd(userId, currentSocket);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                response = await ReceiveStringAsync(currentSocket, userId, ct);
                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }

                    continue;
                }

                JObject jResponse = JObject.Parse(response);
                string type = jResponse["type"].Value<string>();
                string method = jResponse["method"].Value<string>();
                switch (type)
                {
                    case "messanger":
                        SocketHandler.Messanger(method, jResponse, userId, ct);
                        break;
                    case "notification":
                        SocketHandler.Notification(method, currentSocket, jResponse, userId, ct);
                        break;
                    case "video-message":
                        SocketHandler.VideoMessage(method, jResponse, userId, ct);
                        break;
                }

            }

            DeleteUserFromChatRoom(userId);
            WebSocket dummy;
            _sockets.TryRemove(userId, out dummy);
            if (currentSocket.State == WebSocketState.Connecting || currentSocket.State == WebSocketState.Open)
            {
                await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
                currentSocket.Dispose();
            }
        }

        public static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, int userID, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[512000]);
            using (var ms = new MemoryStream())
            {
                try
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        ct.ThrowIfCancellationRequested();
                        result = await socket.ReceiveAsync(buffer, ct);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);
                    if (result.MessageType != WebSocketMessageType.Text)
                    {
                        return null;
                    }

                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }
    }
}
