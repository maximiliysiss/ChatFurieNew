﻿using Microsoft.AspNetCore.Http;
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

        public static ConcurrentDictionary<int, WebSocket> Sockets => _sockets;
        private ChatWCF.ChatService ChatService = new ChatWCF.ChatService();

        private readonly RequestDelegate _next;

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
            var response = await ReceiveStringAsync(currentSocket, ct);
            JObject jObject = JObject.Parse(response);
            int userId = jObject["user"].Value<int>();

            _sockets.TryAdd(userId, currentSocket);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                response = await ReceiveStringAsync(currentSocket, ct);
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
                        SocketHandler.Notification(method, jResponse, userId, ct);
                        break;
                    case "video-message":
                        SocketHandler.VideoMessage(method, jResponse, userId, ct);
                        break;
                }

            }

            WebSocket dummy;
            _sockets.TryRemove(userId, out dummy);

            if (currentSocket.State == WebSocketState.Connecting || currentSocket.State == WebSocketState.Open)
                await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }

        public static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
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

                // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
