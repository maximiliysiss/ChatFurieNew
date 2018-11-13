using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ChatFurie.Controllers;
using System.Linq;
using AuthServiceWCF.Models;

namespace UnitTestsChatFurie.ControllerTests
{
    public class AccountControllerUTest : IDisposable
    {
        public RegisterModel registerModel = new RegisterModel
        {
            Birthday = DateTime.MinValue,
            Password = "123456Qw!",
            ConfirmedPassword = "123456Qw!",
            Email = "testOne@yandex.ru"
        };

        public int? userID;

        public void Dispose()
        {
            ChatContext chatContext = new ChatContext();
            if (userID != null)
                chatContext.Users.Remove(chatContext.Users.Find(userID.Value));
            chatContext.SaveChanges();
        }

        [Fact]
        public void RegisterTest()
        {
            AccountController accountController = new AccountController();
            var regPage = accountController.Register(registerModel);
            ChatContext chatContext = new ChatContext();
            var user = chatContext.Users.FirstOrDefault(x => x.Email == registerModel.Email);
            this.userID = user.ID;
            Assert.True(registerModel == user);
        }

    }
}
