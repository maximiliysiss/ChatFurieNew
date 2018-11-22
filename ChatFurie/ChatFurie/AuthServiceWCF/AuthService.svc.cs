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
        public UserPageModel GetUserSettings(int id)
        {
            ChatContext chatContext = new ChatContext();
            var user = chatContext.Users.Find(id);
            return new UserPageModel
            {
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                ID = user.ID,
                Login = user.Login,
                Name = user.Name
            };
        }



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

        public IActionResultWCF SaveSettingsUser(UserPageModel userPageModel)
        {
            ChatContext chatContext = new ChatContext();
            var user = chatContext.Users.Find(userPageModel.ID);
            if (user.PasswordHash != CryptService.GetMd5Hash(userPageModel.VerifyPassword))
                return new LoginActionResult { Id = user.ID.ToString(), Status = ResultCode.ErrorLoginPassword };
            if (userPageModel.ChangePassword.Trim().Length > 0)
                user.PasswordHash = CryptService.GetMd5Hash(userPageModel.ChangePassword);
            if (userPageModel.Login.Trim().Length > 0)
                user.Login = userPageModel.Login;
            if (userPageModel.Email.Trim().Length > 0)
                user.Email = userPageModel.Email;
            if (userPageModel.Name.Trim().Length > 0)
                user.Name = userPageModel.Name;

            chatContext.Entry(userPageModel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            chatContext.SaveChanges();
            return new LoginActionResult { Id = user.ID.ToString(), Status = ResultCode.Success };
        }
    }
}
