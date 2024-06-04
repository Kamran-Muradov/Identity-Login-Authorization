using Educal_MVC.Helpers;
using Educal_MVC.Helpers.Extensions;
using Educal_MVC.Services.Interfaces;
using Educal_MVC.ViewModels.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Educal_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;

        public CourseController(
            ICourseService courseService,
            ICategoryService categoryService)
        {
            _courseService = courseService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var courses = await _courseService.GetAllPaginateAsync(page, 4);

            var mappedDatas = _courseService.GetMappedDatas(courses);

            int totalPage = await GetPageCountAsync(4);

            Paginate<CourseAdminVM> response = new(mappedDatas, totalPage, page);

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                return View();
            }

            if (request.DiscountPrice is not null)
            {
                if (decimal.Parse(request.Price) <= decimal.Parse(request.DiscountPrice))
                {
                    ModelState.AddModelError("DiscountPrice", "Discount price must be smaller than price");
                    ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                    return View();
                }
            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Images", "Image size can be max 500 Kb");
                    ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be only image");
                    ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                    return View();
                }
            }

            await _courseService.CreateAsync(request);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            var course = await _courseService.GetByIdWithAllDatasAsync((int)id);

            if (course is null) return NotFound();

            ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);

            return View(new CourseEditVM
            {
                Name = course.Name,
                Description = course.Description,
                Price = course.Price.ToString(),
                DiscountPrice = course.DiscountPrice.ToString(),
                CategoryId = course.CategoryId,
                Images = course.CourseImages.Select(m => new CourseImageEditVm
                {
                    Id = m.Id,
                    Name = m.Name,
                    IsMain = m.IsMain,
                    CourseId = m.CourseId

                }).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CourseEditVM request)
        {
            if (id is null) return BadRequest();

            var course = await _courseService.GetByIdWithAllDatasAsync((int)id);

            if (course is null) return NotFound();

            List<CourseImageEditVm> images = course.CourseImages
                .Select(m => new CourseImageEditVm
                {
                    Id = m.Id,
                    Name = m.Name,
                    IsMain = m.IsMain,
                })
                .ToList();

            request.Images = images;

            if (!ModelState.IsValid)
            {
                ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                return View(request);
            }

            if (request.DiscountPrice is not null)
            {
                if (decimal.Parse(request.Price) <= decimal.Parse(request.DiscountPrice))
                {
                    ModelState.AddModelError("DiscountPrice", "Discount price must be smaller than price");
                    ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                    return View(request);
                }
            }

            if (request.NewImages is not null)
            {
                foreach (var item in request.NewImages)
                {
                    if (!item.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Images", "Image size can be max 500 Kb");
                        ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                        return View(request);
                    }

                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Images", "File type must be only image");
                        ViewBag.categories = _categoryService.GetAllSelectedAsync().Result.OrderBy(m => m.Text);
                        return View(request);
                    }
                }
            }

            await _courseService.EditAsync(course, request);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCourseImage(MainAndDeleteImageVM request)
        {
            await _courseService.DeleteCourseImageAsync(request);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetMainImage(MainAndDeleteImageVM request)
        {
            await _courseService.SetMainImageAsync(request);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var course = await _courseService.GetByIdWithAllDatasAsync((int)id);

            if (course is null) return NotFound();

            await _courseService.DeleteAsync(course);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var course = await _courseService.GetByIdWithAllDatasAsync((int)id);

            if (course is null) return NotFound();

            return View(new CourseDetailVM
            {
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                DiscountPrice = course.DiscountPrice,
                Category = course.Category.Name,
                Images = course.CourseImages.Select(i => new CourseImageVM
                {
                    Name = i.Name,
                    IsMain = i.IsMain
                }).ToList()
            });
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            int productCount = await _courseService.GetCountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }
    }
}
