using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServiceWCF;
using AuthServiceWCF.Helpers;
using AuthServiceWCF.Models;
using ChatFurie.Models;
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

        public AccountController() { }

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
            _logger?.LogInformation($"Login for {loginModel.Login}");
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                _logger?.LogInformation($"Start login for {loginModel.Login}");
                AuthService authService = new AuthService();
                LoginActionResult resultCode = authService.Login(loginModel) as LoginActionResult;
                if (resultCode.Status == ResultCode.Success)
                {
                    Login(loginModel, resultCode.Id);
                    return RedirectToAction("Index", "Home");
                }
                _logger?.LogWarning($"Invalid login/password");
            }
            _logger?.LogWarning($"Invalid login {loginModel.Login}");
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
                _logger?.LogInformation($"Start register for {registerModel.Email}");
                AuthService authService = new AuthService();
                var resultCode = authService.Register(registerModel) as RegisterActionResult;
                if (resultCode.Status == ResultCode.Success)
                {
                    Login(new LoginModel { Login = resultCode.Email, Password = registerModel.Password }, resultCode.Id);
                    return RedirectToAction("Index", "Home");
                }
            }
            _logger?.LogWarning($"Invalid register for {registerModel.Email}");
            return View(registerModel);
        }
    }
}