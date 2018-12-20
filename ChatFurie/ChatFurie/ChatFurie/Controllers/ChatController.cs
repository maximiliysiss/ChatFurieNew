using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatFurie.Middleware.Sockets;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ChatFurie.Controllers
{
    public class InputFrame
    {
        public int Conversation { get; set; }
        public int User { get; set; }
        public int SegmentNumber { get; set; }
    }

    public class ChatController : Controller
    {
        public async void SendImage([FromForm]InputFrame inputFrame)
        {
            if (Request.Form.Files.Count > 0)
            {
                var fileFirst = Request.Form.Files[0];
                if (fileFirst.Length > 0)
                {
                    JObject jObject = new JObject();
                    jObject["conversation"] = inputFrame.Conversation;
                    jObject["index"] = inputFrame.SegmentNumber;
                    jObject["user"] = inputFrame.User;
                    jObject["type"] = "video-message";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await fileFirst.CopyToAsync(stream);
                        jObject["data"] = Convert.ToBase64String(stream.ToArray());
                    }
                    SocketHandler.VideoMessage("", jObject, inputFrame.User, default(CancellationToken));
                }
            }
        }
    }
}