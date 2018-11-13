using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AuthServiceWCF.Helpers;
using AuthServiceWCF.Models;
using AuthServiceWCF.Services;
using ChatWCF.Models;

namespace AuthServiceWCF
{
    public class AuthService : IAuthService
    {
        public IActionResultWCF Login(LoginModel loginModel)
        {
            ChatContext chatContext = new ChatContext();
            var user = chatContext.Users.Where(x => x.Login == loginModel.Login).FirstOrDefault();
            if (user != null)
                if (CryptService.VerifyHash(loginModel.Password, user.PasswordHash))
                    return new LoginActionResult { Status = ResultCode.Success, Id = user.ID.ToString() };
                else
                    return new LoginActionResult { Status = ResultCode.ErrorLoginPassword };
            return new LoginActionResult { Status = ResultCode.Error };
        }

        public IActionResultWCF Register(RegisterModel registerModel)
        {
            ChatContext chatContext = new ChatContext();
            if (chatContext.Users.FirstOrDefault(x => x.Email == registerModel.Email) != null)
                return new RegisterActionResult { Status = ResultCode.ErrorAccess };

            User newUser = new User
            {
                Email = registerModel.Email,
                LastEnter = DateTime.Now,
                Login = registerModel.Email,
                Name = registerModel.Email,
                PasswordHash = CryptService.GetMd5Hash(registerModel.Password)
            };

            chatContext.Entry(newUser).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            chatContext.SaveChanges();
            return new RegisterActionResult { Status = ResultCode.Success, Id = newUser.ID.ToString(), Email = newUser.Email };
        }
    }
}
