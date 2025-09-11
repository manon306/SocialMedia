namespace SocialMedia.PL.Controllers
{
    public class ReactController : Controller
    {
        private readonly IReactService _reactService;
        private readonly UserManager<User> _userManager;

        public ReactController(IReactService reactService, UserManager<User> userManager)
        {
            _reactService = reactService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleReact(int postId, reactType type)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var reactVm = new AddReactVm
            {
                Type = type,
                CreatedBy = user.Id,
                PostID = postId
            };

            var result = _reactService.ToggleReact(reactVm);

            if (result.Item1)
            {
                // الحصول على عدد التفاعلات الجديد
                var countResult = _reactService.GetReactsCount(postId);
                var summaryResult = _reactService.GetReactsSummary(postId);
                var userReactResult = _reactService.GetUserReactType(postId, user.Id);

                return Json(new
                {
                    success = true,
                    message = "React updated successfully",
                    count = countResult.Item3,
                    summary = summaryResult.Item3,
                    userReactType = userReactResult.Item3
                });
            }
            else
            {
                return Json(new { success = false, message = result.Item2 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReactsInfo(int postId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var countResult = _reactService.GetReactsCount(postId);
            var summaryResult = _reactService.GetReactsSummary(postId);
            var userReactResult = _reactService.GetUserReactType(postId, user.Id);

            if (countResult.Item1 && summaryResult.Item1)
            {
                return Json(new
                {
                    success = true,
                    count = countResult.Item3,
                    summary = summaryResult.Item3,
                    userReactType = userReactResult.Item3
                });
            }
            else
            {
                return Json(new { success = false, message = "Error getting reacts info" });
            }
        }
    }
}