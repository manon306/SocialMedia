
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SocialMedia.BLL.ModelVM.Account;
using SocialMedia.DAL.Entity;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.PL.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailService emailService;

        public IActionResult Index()
        {
            return View();
        }
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
        }
        [HttpGet]


        //sign up
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

            //if (result.Succeeded)
            //{
            //    return RedirectToAction("Login");
            //}
            if (result.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var codeBytes = Encoding.UTF8.GetBytes(code);
                var encodedCode = WebEncoders.Base64UrlEncode(codeBytes);

                var callbackUrl = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = encodedCode }, protocol: Request.Scheme);

                var html = $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.";

                await emailService.SendEmailAsync(user.Email, "Confirm your email", html);

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


        //cponfirm email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var decodedBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                return View("ConfirmEmail"); 
            }

            return View("Error");
        }


        //reset password 
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid password reset request.");

            var model = new ResetPasswordVM
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return RedirectToAction("Login", "Account");
            //"Login", "Account"

            //var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));


            var decodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, model.Password);

            //var result = await userManager.ResetPasswordAsync(user, decodedToken, model.Password);
            //if (result.Succeeded)
            //{
            //    await signInManager.SignInAsync(user, isPersistent: false);

            //    return RedirectToAction("Login", "Account");
            //}

            //foreach (var error in result.Errors)
            //    ModelState.AddModelError("", error.Description);

           // var result = await userManager.ResetPasswordAsync(user, decodedToken, model.Password);

            if (result.Succeeded)
            {
                Console.WriteLine("✅ Password reset succeeded for: " + user.Email); 
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("❌ Error: " + error.Description);
                    ModelState.AddModelError("", error.Description);
                }
            }




            return View(model);
        }


        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // forget password 
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { userId = user.Id, token = encodedToken }, protocol: Request.Scheme);


            // ابعت اللينك في الإيميل
            var html = $"Reset your password by <a href='{callbackUrl}'>clicking here</a>.";
            await emailService.SendEmailAsync(model.Email, "Reset Password", html);


            //var token = await userManager.GeneratePasswordResetTokenAsync(user);
            //var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //var callbackUrl = Url.Action("ResetPassword", "Account",
            //    new { userId = user.Id, token = code }, protocol: Request.Scheme);

            //var html = $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.";

            //await emailService.SendEmailAsync(model.Email, "Reset Password", html);

            return RedirectToAction("ForgotPasswordConfirmation");



        }



        //login
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (string.IsNullOrWhiteSpace(loginVM.Email) || string.IsNullOrWhiteSpace(loginVM.Password))
            {
                ModelState.AddModelError("", "Email and Password are required");
                return View(loginVM);
            }

            var user = await userManager.FindByEmailAsync(loginVM.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(
                    user,
                    loginVM.Password,
                    false,
                    false
                );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid Email or Password");
            return View(loginVM);
        }

        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }





        //login by google and eacbook
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

            // if user is here
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded) return LocalRedirect(returnUrl);

            //create account
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
            var user = new User { UserName = email, Email = email };

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
