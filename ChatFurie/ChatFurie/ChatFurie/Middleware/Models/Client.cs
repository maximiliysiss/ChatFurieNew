using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatFurie.Middleware.Models
{
    public class Client
    {
        public WebSocket Socket { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
