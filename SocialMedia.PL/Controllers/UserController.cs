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

            var result = string.IsNullOrEmpty(keyword)
                ? allUsers
                : allUsers
                    .Where(u => !string.IsNullOrEmpty(u.Name)
                                && u.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return PartialView("_UsersPartial", result);
        }


    }
}