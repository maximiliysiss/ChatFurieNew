using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatFurie.Models;
using ChatFurie.Models.AccountModel;
using ChatFurie.Services;
using ChatWCF.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ChatFurie.Controllers
{
    public class AccountController : Controller
    {
        protected readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            if (_logger == null)
                _logger = logger;
        }

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
            _logger.LogInformation($"Login for {loginModel.Login}");
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Start login for {loginModel.Login}");
                ChatContext chatContext = new ChatContext();
                var user = chatContext.Users.Where(x => x.Login == loginModel.Login).FirstOrDefault();
                if (user != null)
                    if (CryptService.VerifyHash(loginModel.Password, user.PasswordHash))
                    {
                        Login(loginModel, user.ID.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                _logger.LogWarning($"Invalid login/password");
            }
            _logger.LogWarning($"Invalid login {loginModel.Login}");
            return View(loginModel);
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
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
                _logger.LogInformation($"Start register for {registerModel.Email}");
                User newUser = new User
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
            _logger.LogWarning($"Invalid register for {registerModel.Email}");
            return View(registerModel);
        }
    }
}