using System.ComponentModel.DataAnnotations;

namespace Educal_MVC.ViewModels.Accounts
{
    public class LoginVM
    {
        [Required]
        public string EmailOrUserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
