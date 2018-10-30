using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatFurie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ChatFurie.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        protected ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            if (_logger == null)
                _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public IActionResult FindResult(string find, int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView(chatService.FindFriends(find, user));
        }

        [HttpPost]
        public bool AddFriend(int user, int newFriend)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return chatService.AddFriend(user, newFriend);
        }

        [HttpPost]
        public IActionResult GetConversations(int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView("ConversationList", chatService.ConversationUserList(user));
        }

        [HttpPost]
        public IActionResult GetNotifications(int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView("NotificationList", chatService.GetNotifications(user));
        }

        [HttpPost]
        public IActionResult OpenContact(int user, int another)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView(chatService.GetUser(another, user));
        }

        [HttpPost]
        public IActionResult OpenNotification(int user, int notification)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView(chatService.GetNotification(user, notification));
        }

        [HttpPost]
        public int AcceptFriend(int user, int notification)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return chatService.AcceptFriend(user, notification);
        }

        [HttpPost]
        public bool DeclineFriend(int user, int notification)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return chatService.DeclineFriend(user, notification);
        }

        [HttpPost]
        public IActionResult OpenConversation(int user, int conversation)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView("Conversation", chatService.GetConversation(user, conversation));
        }

        [HttpPost]
        public IActionResult ConversationInfo(int user, int conversation)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView(chatService.ConversationInfo(conversation, user));
        }
    }
}
