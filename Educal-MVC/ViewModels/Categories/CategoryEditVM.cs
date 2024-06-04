using System.ComponentModel.DataAnnotations;

namespace Educal_MVC.ViewModels.Categories
{
    public class CategoryEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile NewImage { get; set; }
    }
}
