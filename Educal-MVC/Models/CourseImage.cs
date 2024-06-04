using System.ComponentModel.DataAnnotations.Schema;

namespace Educal_MVC.Models
{
    public class CourseImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
