using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebUI.Models
{
    public class PasswordModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "This field is mandatory.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public string RePassword { get; set; }
    }
}
