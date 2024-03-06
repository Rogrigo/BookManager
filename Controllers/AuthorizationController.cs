using BookManager.Interfaces;
using BookManager.Models;
using BookManager.Utils;
using BookManager.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManager.Controllers
{
    public class AuthorizationController : Controller
    {
        private IConfiguration _config;
        private readonly IUserReporitory _userReporitory;

        public AuthorizationController(IConfiguration config, IUserReporitory userReporitory)
        {
            _config = config;
            _userReporitory = userReporitory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            var existingUser = await _userReporitory.GetUserByEmail(loginVM.Email);
            if (existingUser == null) 
            {
                TempData["Error"] = "Wrong credetilas. Please, try again!";
                return View(loginVM);
            }

            if(PasswordHasher.HashPassword(loginVM.Password) != existingUser.Password)
            {
                TempData["Error"] = "Wrong credetilas. Please, try again!";
                return View(loginVM);
            }

            var tokenGenerator = new TokenGenerator(_config);

            var token = tokenGenerator.GenerateToken(existingUser);

            AppendTokenToCookie.AppendToCookie(token, HttpContext);

            return RedirectToAction("Index", "Book");
        }

        public IActionResult Logout()
        {
            if (Request.Cookies["token"] != null)
            {
                Response.Cookies.Delete("token");
            }
            return Redirect("~/Book/Index");
        }
    }
}
