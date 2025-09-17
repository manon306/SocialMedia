using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DAL.Entity;
using SocialMedia.PL.Models;

namespace SocialMedia.PL.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public ProfileController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: /Profile/Edit
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var vm = new ProfileEditViewModel
            {
                Id = user.Id,
                Address = user.Address,
                Latitude = user.Latitude,
                Longitude = user.Longitude
            };

            ViewBag.GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"];
            return View(vm);
        }

        // POST: /Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"];
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.Address = model.Address;
            user.Latitude = model.Latitude;
            user.Longitude = model.Longitude;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var err in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);

                ViewBag.GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"];
                return View(model);
            }

            return RedirectToAction("Details", new { id = user.Id });
        }

        // GET: /Profile/Details/{id?}
        [AllowAnonymous]
        public async Task<IActionResult> Details(string? id)
        {
            User user;
            if (string.IsNullOrEmpty(id))
            {
                user = await _userManager.GetUserAsync(User);
            }
            else
            {
                user = await _userManager.FindByIdAsync(id);
            }

            if (user == null) return NotFound();

            ViewBag.GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"];
            return View(user); // pass User entity to the view
        }
    }
}