using Educal_MVC.Services.Interfaces;
using Educal_MVC.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace Educal_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ICourseService _courseService;

        public HomeController(
            ICategoryService categoryService,
            ICourseService courseService)
        {
            _categoryService = categoryService;
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(new HomeVM
            {
                Categories = await _categoryService.GetAllAsync(),
                Courses = await _courseService.GetAllAsync()
            });
        }
    }
}
