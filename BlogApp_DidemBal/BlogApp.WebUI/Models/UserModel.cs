using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebUI.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
