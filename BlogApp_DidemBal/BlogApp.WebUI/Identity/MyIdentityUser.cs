using Microsoft.AspNetCore.Identity;

namespace BlogApp.WebUI.Identity
{
    public class MyIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
