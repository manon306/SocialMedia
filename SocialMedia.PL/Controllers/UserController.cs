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
                return View("AllUsers", allUsers);
            }

            var result = allUsers
                .Where(u => !string.IsNullOrEmpty(u.Name)
                            && u.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
            {
                ViewBag.Message = "No User Found😢";
            }

            return View("AllUsers", result);
        }
        // GET: User/Suggest/{id}
        public IActionResult Suggest(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID is required");

            (bool success, string message, List<ViewProfileVM> suggestedUsers) = userServices.GetSuggestUsers(id);

            if (!success)
            {
                // في حالة الخطأ أو مفيش يوزر
                return NotFound(new { Message = message });
            }

            // لو عايزة تعمليها View
            return View(suggestedUsers);

            // أو لو API Json
            // return Ok(new { Message = message, Users = suggestedUsers });
        }
    }
}