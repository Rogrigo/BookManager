using System.ComponentModel.DataAnnotations;

namespace BookManager.ViewModels.UserViewModels
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
