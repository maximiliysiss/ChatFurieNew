using AuthServiceWCF;
using AuthServiceWCF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTestsChatFurie.ControllerTests
{
    public class AccountWCFTest
    {
        readonly RegisterModel registerModel = new RegisterModel
        {
            Birthday = DateTime.MinValue,
            Email = "testing@mail.com",
            Password = "123456Qw!",
            ConfirmedPassword = "123456Qw!"
        };

        readonly LoginModel loginModel = new 

        [Fact]
        public void RegisterTest()
        {
            AuthService authService = new AuthService();
            var resultCode = authService.Register(new AuthServiceWCF.Models.RegisterModel { })
        }
    }
}
