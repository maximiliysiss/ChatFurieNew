﻿using AuthServiceWCF;
using AuthServiceWCF.Helpers;
using AuthServiceWCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTestsChatFurie.ControllerTests
{
    public class AccountWCFTest : IDisposable
    {
        readonly RegisterModel registerModel = new RegisterModel
        {
            Birthday = DateTime.MinValue,
            Email = "testingRegister@mail.com",
            Password = "123456Qw!",
            ConfirmedPassword = "123456Qw!"
        };

        public void Dispose()
        {
            ChatWCF.Models.ChatContextWCF chatContextWCF = new ChatWCF.Models.ChatContextWCF();
            chatContextWCF.Users.Remove(chatContextWCF.Users.FirstOrDefault(x => x.Login == registerModel.Email));
            chatContextWCF.SaveChanges();
        }

        [Fact]
        public void RegisterTest()
        {
            AuthService authService = new AuthService();
            var resultCode = authService.Register(registerModel);
            Assert.Equal(ResultCode.Success, resultCode.Status);
        }

        [Fact]
        public void LoginTest()
        {
            AuthService authService = new AuthService();
            var resultCode = authService.Register(registerModel);
            Assert.Equal(ResultCode.Success, resultCode.Status);

            resultCode = authService.Login(new LoginModel
            {
                Login = registerModel.Email,
                Password = registerModel.Password
            });
            Assert.Equal(ResultCode.Success, resultCode.Status);
        }
    }
}
