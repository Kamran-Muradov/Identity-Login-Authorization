namespace Educal_MVC.ViewModels.Courses
{
    public class CourseDetailVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string Category { get; set; }
        public List<CourseImageVM> Images { get; set; }
    }
}
