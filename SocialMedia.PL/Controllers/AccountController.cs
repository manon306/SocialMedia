using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.BLL.ModelVM.Account;
using SocialMedia.DAL.Entity;
using System.Security.Claims;

namespace SocialMedia.PL.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;


        public IActionResult Index()
        {
            return View();
        }
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM signUpVM)
        {
            var user = new User()
            {
                UserName = signUpVM.UserName,
                Email = signUpVM.Email
            };

            var result = await userManager.CreateAsync(user, signUpVM.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }

            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(SignUpVM signUpVM)
        {
            var user = await userManager.FindByEmailAsync(signUpVM.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(
                    user.UserName,
                    signUpVM.Password,
                    false,
                    false
                );

                if (result.Succeeded)
                    return RedirectToAction("Index", "Post");

            }

            ModelState.AddModelError("", "Invalid Email or Password");
            return View(signUpVM);
        }
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction(nameof(Login));

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded) return LocalRedirect(returnUrl);

            // create account
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            // للتأكد من التفرد
            var userName = fullName.Replace(" ", "") + "_" + info.ProviderKey.Substring(0, 5);

            var user = new User
            {
                UserName = userName, // الاسم الكامل + جزء من الـ provider key لتجنب التكرار
                Email = email
            };


            var createResult = await userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                createResult = await userManager.AddLoginAsync(user, info);
                if (createResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }

            ModelState.AddModelError("", "Error creating user from external login.");
            return RedirectToAction(nameof(Login));

        }

    }

}
