

using Microsoft.AspNetCore.Mvc;
using SocialMedia.BLL.ModelVM.Profile;
using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.BLL.Service.Implementation;
using System.Security.Claims;

namespace SocialMedia.PL.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _service;

        public UserProfileController(IUserProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProfileVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (success, error) = await _service.CreateProfile(model, userId);

            if (success)
                return RedirectToAction("ViewProfile");

            ViewBag.Error = error;
            return View(model);
        }

        public async Task<IActionResult> ViewProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _service.GetProfile(userId);

            if (profile == null)
                return RedirectToAction("Create");

            return View(profile);
        }
    }
}
