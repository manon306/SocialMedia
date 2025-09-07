

using Microsoft.AspNetCore.Authorization;

namespace SocialMedia.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }


    }
}
