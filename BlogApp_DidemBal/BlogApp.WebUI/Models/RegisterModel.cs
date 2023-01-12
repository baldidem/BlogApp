using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebUI.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "This field is mandatory.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This field is mandatory.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This field is mandatory.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field is mandatory.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is mandatory.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is mandatory.")]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public string RePassword { get; set; }
    }
}
