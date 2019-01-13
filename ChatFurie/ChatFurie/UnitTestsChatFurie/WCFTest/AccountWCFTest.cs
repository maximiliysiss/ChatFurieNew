using AuthServiceWCF;
using AuthServiceWCF.Helpers;
using AuthServiceWCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTestsChatFurie.ControllerTests
{
    /// <summary>
    /// Класс тестирования системы регистрации и авторизации
    /// </summary>
    public class AccountWCFTest : IDisposable
    {
        /// <summary>
        /// Модель регистрации
        /// </summary>
        readonly RegisterModel registerModel = new RegisterModel
        {
            Birthday = DateTime.MinValue,
            Email = "testingRegister@mail.com",
            Password = "123456Qw!",
            ConfirmedPassword = "123456Qw!"
        };

        /// <summary>
        /// Метод очистки после выполнения теста
        /// </summary>
        public void Dispose()
        {
            ChatWCF.Models.ChatContextWCF chatContextWCF = new ChatWCF.Models.ChatContextWCF();
            chatContextWCF.Users.Remove(chatContextWCF.Users.FirstOrDefault(x => x.Login == registerModel.Email));
            chatContextWCF.SaveChanges();
        }

        /// <summary>
        /// Первый тест, проверяющий регистрацию
        /// </summary>
        [Fact]
        public void RegisterTest()
        {
            AuthService authService = new AuthService();
            var resultCode = authService.Register(registerModel);
            Assert.Equal(ResultCode.Success, resultCode.Status);
        }

        /// <summary>
        /// Второй тест проверяющий авторизацию
        /// </summary>
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
