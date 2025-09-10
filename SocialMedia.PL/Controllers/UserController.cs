


using SocialMedia.BLL.ModelVM.User;

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
        public IActionResult Search(string keyword)
        {
            var allUsers = userServices.GetAll().Item3;

            if (string.IsNullOrEmpty(keyword))
            {
                return View("Index", allUsers);
            }

            var result = allUsers
                .Where(u => !string.IsNullOrEmpty(u.Name)
                            && u.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
            {
                ViewBag.Message = "No User Found😢";
            }

            return View("Index", result);
        }


    }
}