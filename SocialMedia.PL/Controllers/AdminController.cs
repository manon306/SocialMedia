using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SocialMedia.DAL.Entity;

namespace SocialMedia.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.UpdateRole(User.Identity.Name, role);
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Users));
        }
    }
}
