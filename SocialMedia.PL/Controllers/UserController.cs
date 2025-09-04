


namespace SocialMedia.PL.Controllers
{
   // [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IUserSerives userServices;

        public UserController(IUserSerives userServices)
        {
            this.userServices = userServices;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProfile model)
        {
            if (ModelState.IsValid)
            {
                var result = userServices.Create(model);

                if (result.Item1) // success
                {
                    TempData["Success"] = "Profile created successfully!";
                    return RedirectToAction("Profiles");
                }

                ModelState.AddModelError("", result.Item2); 
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Profiles()
        {
            var result = userServices.ViewProfile();

            if (!result.Item1) // if failed
            {
                ViewBag.Error = result.Item2;
                return View(new List<ViewProfile>());
            }

            return View(result.Item3);
        }
    }
}
