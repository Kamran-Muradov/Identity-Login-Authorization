using Educal_MVC.Helpers;
using Educal_MVC.Helpers.Extensions;
using Educal_MVC.Services.Interfaces;
using Educal_MVC.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Educal_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var categories = await _categoryService.GetAllPaginateAsync(page, 4);

            var mappedDatas = _categoryService.GetMappedDatas(categories);

            var totalPage = await GetPageCountAsync(4);

            Paginate<CategoryCourseVM> response = new(mappedDatas, totalPage, page);

            return View(response);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CategoryCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await _categoryService.ExistAsync(request.Name))
            {
                ModelState.AddModelError("Name", "Category with this name already exists");
                return View();
            }

            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }

            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB");
                return View();
            }

            await _categoryService.CreateAsync(request);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            var category = await _categoryService.GetByIdAsync((int)id);

            if (category is null) return NotFound();

            return View(new CategoryEditVM
            {
                Name = category.Name,
                Description = category.Description,
                Image = category.Image
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id is null) return BadRequest();

            var category = await _categoryService.GetByIdAsync((int)id);

            if (category is null) return NotFound();

            if (!ModelState.IsValid)
            {
                request.Image = category.Image;
                return View(request);
            }

            if (request.Name.Trim().ToLower() != category.Name.Trim().ToLower() && await _categoryService.ExistAsync(request.Name))
            {
                ModelState.AddModelError("Name", "Category with this name already exists");
                request.Image = category.Image;
                return View(request);
            }

            if (request.NewImage is not null)
            {
                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Input can accept only image format");
                    request.Image = category.Image;
                    return View(request);
                }

                if (!request.NewImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("Image", "Image size must be max 200 KB");
                    request.Image = category.Image;
                    return View(request);
                }
            }

            await _categoryService.EditAsync(category, request);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var category = await _categoryService.GetByIdAsync((int)id);

            if (category is null) return NotFound();

            await _categoryService.DeleteAsync(category);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            if (id is null) return BadRequest();

            var category = await _categoryService.GetByIdWithCoursesAsync((int)id);

            if (category is null) return NotFound();

            return View(new CategoryDetailVM
            {
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate.ToString("MM.dd.yyyy"),
                Image = category.Image,
                CourseCount = category.Courses.Count,
                Courses = category.Courses.Select(m => m.Name).ToList()
            });
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            int productCount = await _categoryService.GetCountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }
    }
}
