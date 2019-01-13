using AuthServiceWCF.Helpers;
using AuthServiceWCF.Services;
using ChatWCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTestsChatFurie.WCFTest
{
    /// <summary>
    /// Класс для тестирования Чата
    /// </summary>
    public class ChatWCFTest : IDisposable
    {
        /// <summary>
        /// Id пользователей системы
        /// </summary>
        public int u1, u2;

        /// <summary>
        /// Создание пользователей через сервиса Auth
        /// </summary>
        public ChatWCFTest()
        {
            AuthServiceWCF.AuthService authService = new AuthServiceWCF.AuthService();
            u1 = int.Parse((authService.Register(new AuthServiceWCF.Models.RegisterModel
            {
                Birthday = new DateTime(1990, 1, 1),
                Password = "testingOne",
                ConfirmedPassword = "testingOne",
                Email = "test1@mail.ru"
            }) as RegisterActionResult).Id);
            u2 = int.Parse((authService.Register(new AuthServiceWCF.Models.RegisterModel
            {
                Birthday = new DateTime(1990, 1, 1),
                Password = "testingOne",
                ConfirmedPassword = "testingOne",
                Email = "test2@mail.ru"
            }) as RegisterActionResult).Id);
        }

        /// <summary>
        /// Проверка на добавление в друзья
        /// </summary>
        [Fact]
        public void AddFriend()
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            chatService.AddFriend(u1, u2);
            var notifications = chatService.GetNotifications(u2).FirstOrDefault();
            Assert.NotNull(notifications);
            Assert.Equal(u1, notifications.CreatorID);
            Assert.Contains("add", notifications.Context.ToLower());
            chatService.DeclineFriend(u2, notifications.ID);
        }

        /// <summary>
        /// Проверка на поиск человека
        /// </summary>
        [Fact]
        public void FindingUsers()
        {
            ChatWCF.ChatService chatService = new ChatWCF.ChatService();
            var user = chatService.FindFriends("test2@mail.ru", u1).FirstOrDefault();
            Assert.NotNull(user);
            Assert.Equal(u2, user.ID);
        }

        /// <summary>
        /// Удаление после тестирования
        /// </summary>
        public void Dispose()
        {
            AuthServiceWCF.Models.ChatContext chatContext = new AuthServiceWCF.Models.ChatContext();
            chatContext.Users.Remove(chatContext.Users.Find(u1));
            chatContext.Users.Remove(chatContext.Users.Find(u2));
            chatContext.SaveChanges();
        }
    }
}
