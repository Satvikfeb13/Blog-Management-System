using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="ConfirmPassword must be match with Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
