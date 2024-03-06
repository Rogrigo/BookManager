using BookManager.Interfaces;
using BookManager.Models;
using BookManager.Utils;
using BookManager.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookManager.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserReporitory _userReporitory;

        public UserController(IUserReporitory userReporitory)
        {
           _userReporitory = userReporitory;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel userVM)
        {
            var hashedPassword = PasswordHasher.HashPassword(userVM.Password);
            if(ModelState.IsValid)
            {
                var newUser = new User
                {
                    UserName = userVM.UserName,
                    UserSurname = userVM.UserSurname,
                    Email = userVM.Email,
                    Password = hashedPassword,
                    DateOfBirth = userVM.DateOfBirth
                };
                _userReporitory.Add(newUser);
                return RedirectToAction("Index", "Book");
            }
            return View(userVM);
        }
    }
}
