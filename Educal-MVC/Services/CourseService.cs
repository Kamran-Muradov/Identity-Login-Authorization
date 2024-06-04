using Educal_MVC.Data;
using Educal_MVC.Helpers.Extensions;
using Educal_MVC.Models;
using Educal_MVC.Services.Interfaces;
using Educal_MVC.ViewModels.Courses;
using Microsoft.EntityFrameworkCore;

namespace Educal_MVC.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseService(
            AppDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<CourseVM>> GetAllAsync()
        {
            return await _context.Courses
                .Include(m => m.CourseImages)
                .Include(m => m.Category)
                .Select(m => new CourseVM
                {
                    Name = m.Name,
                    Price = m.Price,
                    DiscountPrice = m.DiscountPrice,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category.Name,
                    MainImage = m.CourseImages.FirstOrDefault(i => i.IsMain).Name
                })
                .ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<Course> GetByIdWithAllDatasAsync(int id)
        {
            return await _context.Courses
                .Where(m => m.Id == id)
                .Include(m => m.Category)
                .Include(m => m.CourseImages)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Course>> GetAllPaginateAsync(int page, int take)
        {
            return await _context.Courses
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .Include(m => m.CourseImages)
                .Include(m => m.Category)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Courses.CountAsync();
        }

        public IEnumerable<CourseAdminVM> GetMappedDatas(IEnumerable<Course> courses)
        {
            return courses.Select(m => new CourseAdminVM
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                CategoryName = m.Category.Name,
                MainImage = m.CourseImages.FirstOrDefault(i => i.IsMain)?.Name
            });
        }

        public async Task CreateAsync(CourseCreateVM request)
        {
            List<CourseImage> images = new();

            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";

                string path = _env.GenerateFilePath("assets/images", fileName);

                await item.SaveFileToLocalAsync(path);

                images.Add(new CourseImage { Name = fileName });
            }

            images.FirstOrDefault().IsMain = true;

            Course course = new()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Price = decimal.Parse(request.Price),
                DiscountPrice = request.DiscountPrice != null ? decimal.Parse(request.DiscountPrice) : null,
                CourseImages = images
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            foreach (var item in course.CourseImages)
            {
                string path = _env.GenerateFilePath("assets/images", item.Name);
                path.DeleteFileFromLocal();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(Course course, CourseEditVM request)
        {
            if (request.NewImages is not null)
            {
                foreach (var item in request.NewImages)
                {
                    string fileName = $"{Guid.NewGuid()}-{item.FileName}";

                    string path = _env.GenerateFilePath("assets/images", fileName);

                    await item.SaveFileToLocalAsync(path);

                    course.CourseImages.Add(new CourseImage { Name = fileName });
                }
            }

            course.Name = request.Name;
            course.Description = request.Description;
            course.Price = decimal.Parse(request.Price);
            course.DiscountPrice = request.DiscountPrice != null ? decimal.Parse(request.DiscountPrice) : null;
            course.CategoryId = request.CategoryId;


            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseImageAsync(MainAndDeleteImageVM data)
        {
            var course = await _context.Courses
                .Where(m => m.Id == data.CourseId)
                .Include(m => m.CourseImages)
                .FirstOrDefaultAsync();

            var courseImage = course.CourseImages.FirstOrDefault(m => m.Id == data.ImageId);

            _context.CourseImages.Remove(courseImage);
            await _context.SaveChangesAsync();

            string path = _env.GenerateFilePath("assets/images", courseImage.Name);
            path.DeleteFileFromLocal();
        }

        public async Task SetMainImageAsync(MainAndDeleteImageVM data)
        {
            var course = await _context.Courses
                .Where(m => m.Id == data.CourseId)
                .Include(m => m.CourseImages)
                .FirstOrDefaultAsync();

            var courseImage = course.CourseImages.FirstOrDefault(m => m.Id == data.ImageId);

            course.CourseImages.FirstOrDefault(m => m.IsMain).IsMain = false;
            courseImage.IsMain = true;
            await _context.SaveChangesAsync();
        }
    }
}
