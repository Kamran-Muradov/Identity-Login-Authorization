using Educal_MVC.ViewModels.Categories;
using Educal_MVC.ViewModels.Courses;

namespace Educal_MVC.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<CourseVM> Courses { get; set; }
    }
}
