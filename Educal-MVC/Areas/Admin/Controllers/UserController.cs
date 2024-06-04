using Educal_MVC.Models;
using Educal_MVC.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Educal_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();

            List<UserRoleVM> usersWithRoles = new();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRoles = string.Join(", ", roles.ToArray());
                usersWithRoles.Add(new UserRoleVM
                {
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = userRoles
                });
            }

            return View(usersWithRoles);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            ViewBag.users = new SelectList(users, "Id", "UserName");
            ViewBag.roles = new SelectList(roles, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(AddUserRoleVM request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            var role = await _roleManager.FindByIdAsync(request.RoleId);

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                ModelState.AddModelError(string.Empty, "This user is already in this role");
                var users = _userManager.Users.ToList();
                var roles = _roleManager.Roles.ToList();

                ViewBag.users = new SelectList(users, "Id", "UserName");
                ViewBag.roles = new SelectList(roles, "Id", "Name");
                return View();
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            return RedirectToAction(nameof(Index));
        }

    }
}
