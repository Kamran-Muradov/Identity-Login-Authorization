using System.ComponentModel.DataAnnotations;

namespace Educal_MVC.ViewModels.Categories
{
    public class CategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
