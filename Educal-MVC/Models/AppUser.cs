using Microsoft.AspNetCore.Identity;

namespace Educal_MVC.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
