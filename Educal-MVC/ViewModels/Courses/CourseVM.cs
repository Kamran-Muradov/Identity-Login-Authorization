namespace Educal_MVC.ViewModels.Courses
{
    public class CourseVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string MainImage { get; set; }
    }
}
