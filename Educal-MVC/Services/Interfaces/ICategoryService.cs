using Educal_MVC.Models;
using Educal_MVC.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Educal_MVC.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync(int? take = null);
        Task<Category> GetByIdAsync(int id);
        Task<Category> GetByIdWithCoursesAsync(int id);
        Task<SelectList> GetAllSelectedAsync();
        IEnumerable<CategoryCourseVM> GetMappedDatas(IEnumerable<Category> categories);
        Task<IEnumerable<Category>> GetAllPaginateAsync(int page, int take);
        Task<int> GetCountAsync();
        Task<bool> ExistAsync(string name);
        Task CreateAsync(CategoryCreateVM request);
        Task EditAsync(Category category, CategoryEditVM request);
        Task DeleteAsync(Category category);
    }
}
