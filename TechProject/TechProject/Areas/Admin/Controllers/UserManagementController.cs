using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tech.DataAccess.Repository;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.ViewModels;
using Tech.Utility;


namespace TechProject.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin)]
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index(string roleFilter, string searchTerm)
        {
            var users = await _userManager.Users.ToListAsync();

            // Filter by role
            if (!string.IsNullOrEmpty(roleFilter))
            {
                var roleUsers = await _userManager.GetUsersInRoleAsync(roleFilter);
                users = users.Where(u => roleUsers.Any(r => r.Id == u.Id)).ToList();
            }

            // Search by email or username
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                      || u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }

            ViewData["Roles"] = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewData["SelectedRole"] = roleFilter;
            ViewData["SearchTerm"] = searchTerm;

            return View(users);
        }



        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            var user = _unitOfWork.ApplicationUser.Get(user => user.Id == userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            var viewModel = new UserVM
            {
                User = user,
                Roles = userRoles.ToList(),
                AllRoles = allRoles
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateRoles(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction("Index");
        }

      

    }
}
