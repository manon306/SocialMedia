

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.BLL.ModelVM.Profile;
using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.BLL.Service.Implementation;
using SocialMedia.DAL.Entity;
using System.Security.Claims;

namespace SocialMedia.PL.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService service;
        private readonly UserManager<User> _userManager;
        private readonly IMapper mapper;
        public UserProfileController(IMapper mapper,IUserProfileService service, UserManager<User> _userManager)
        {
            this._userManager = _userManager;
            this.mapper = mapper;
            this.service = service;
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
            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.Error = "⚠️ UserId is NULL Login أو Claims.";
                return View(model);
            }
            var (success, error) = await service.CreateProfile(model, userId);
            //if (string.IsNullOrEmpty(userId))
            //    
            if (success)
                return RedirectToAction("ViewProfile", "UserProfile");
            //else
            //    return RedirectToAction("Login", "Account");
            ViewBag.Error = error;
            return View(model);
        }

        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await service.GetProfile(userId);

            if (profile == null)
                return RedirectToAction("Create");

            return View(profile);
        }

        public async Task<IActionResult> ViewProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await service.GetProfile(userId);

            if (profile == null)
                return RedirectToAction("Create");

            return View(profile);
        }




        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await service.GetProfile(userId);

            if (profile == null)
                return RedirectToAction("Create");

            var model = new EditProfileVM
            {
                Name = profile.Name,
                Headline = profile.Headline,
                Bio = profile.Bio,
                Location = profile.Location,
                Skills = profile.Skills
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (success, error) = await service.EditProfile(model, userId);

            if (success)
                return RedirectToAction("MyProfile");

            ViewBag.Error = error;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (success, error) = await service.DeleteProfile(userId);

            if (success)
                return RedirectToAction("Index", "Home");

            ViewBag.Error = error;
            return RedirectToAction("MyProfile");
        }
    }
}
