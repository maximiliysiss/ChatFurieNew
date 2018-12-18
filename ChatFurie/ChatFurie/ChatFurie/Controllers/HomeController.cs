using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatFurie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AuthServiceWCF.Models;
using AuthServiceWCF.Helpers;

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

        public IActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindResult(string find, int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView(chatService.FindFriends(find, user));
        }

        [HttpPost]
        public IActionResult LoadUserSettings(int user)
        {
            AuthServiceWCF.AuthService authService = new AuthServiceWCF.AuthService();
            return PartialView("UserPage", authService.GetUserSettings(user));
        }

        [HttpPost]
        public bool SaveUserSettings([FromForm]UserPageModel userPageModel)
        {
            AuthServiceWCF.AuthService authService = new AuthServiceWCF.AuthService();
            return authService.SaveSettingsUser(userPageModel).Status == ResultCode.Success ? true : false;
        }

        [HttpPost]
        public IActionResult FindOnlyFriends(string find, int user, int conversation)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView("FindFriendsAdd", chatService.FindOnlyFriends(find, user, conversation));
        }

        [HttpPost]
        public bool AddUserToConversation(int user, int friend, int conversation)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return chatService.AddFriendToConversation(user, friend, conversation);
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
        public bool ReadMessage(int message, int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return chatService.ReadMessage(message, user);
        }

        [HttpPost]
        public IActionResult ConversationInfo(int user, int conversation)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView(chatService.ConversationInfo(conversation, user));
        }

        [HttpPost]
        public bool DeleteUserFromConversation(int conversation, int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return chatService.DeleteUserFromConversation(user, conversation);
        }

        [HttpPost]
        public bool SetUserAdmin(int conversation, int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return chatService.ChangeConversationAdmin(conversation, null, user);
        }

        [HttpPost]
        public IActionResult GetAddingsMsg(int conversation, int first, int user)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView("MessageBoxGenerator", chatService.GetNewMessages(conversation, first, user));
        }
    }
}
