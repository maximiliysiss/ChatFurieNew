using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatFurie.Models;
using ChatFurie.Models.AccountModel;
using ChatFurie.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ChatFurie.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        private async void Login(LoginModel loginModel, string id)
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, loginModel.Login),
                            new Claim(ClaimTypes.NameIdentifier, id)
                        };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ChatFurieLogin");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                ChatContext chatContext = new ChatContext();
                var user = chatContext.Users.Where(x => x.Login == loginModel.Login).FirstOrDefault();
                if (user != null)
                    if (CryptService.VerifyHash(loginModel.Password, user.PasswordHash))
                    {
                        Login(loginModel, user.ID.ToString());
                        return RedirectToAction("Index", "Home");
                    }
            }
            return View(loginModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                Models.ChatModel.User newUser = new Models.ChatModel.User
                {
                    Email = registerModel.Email,
                    LastEnter = DateTime.Now,
                    Login = registerModel.Email,
                    Name = registerModel.Email,
                    PasswordHash = CryptService.GetMd5Hash(registerModel.Password)
                };

                ChatContext chatContext = new ChatContext();
                chatContext.Entry(newUser).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                chatContext.SaveChanges();

                Login(new LoginModel
                {
                    Login = registerModel.Email,
                    Password = registerModel.Password
                }, newUser.ID.ToString());
                return RedirectToAction("Index", "Home");
            }
            return View(registerModel);
        }
    }
}