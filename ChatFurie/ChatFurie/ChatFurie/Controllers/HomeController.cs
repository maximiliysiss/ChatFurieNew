using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatFurie.Models;
using Microsoft.AspNetCore.Authorization;

namespace ChatFurie.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

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
            return PartialView(chatService.ConversationUserList(user));
        }

        [HttpPost]
        public IActionResult OpenContact(int user, int another)
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            return PartialView(chatService.GetUser(another, user));
        }
    }
}
