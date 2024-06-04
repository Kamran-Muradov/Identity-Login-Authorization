namespace Educal_MVC.ViewModels.Categories
{
    public class CategoryDetailVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CreatedDate { get; set; }
        public ICollection<string> Courses { get; set; }
        public int CourseCount { get; set; }
    }
}
