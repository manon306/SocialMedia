using Microsoft.AspNetCore.Identity;
using SocialMedia.BLL.ModelVM.Comment;
using SocialMedia.DAL.Entity;

namespace SocialMedia.PL.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly UserManager<User> userManager;
        public CommentController(ICommentService commentService , UserManager<User> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(AddCommentVm cm)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            cm.CreatedBy = user.UserName;
            ModelState.Remove("CreatedBy");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Post");
            }
            var result = commentService.AddComment(cm);

            if (!result.Item1)
            {
                ModelState.AddModelError(string.Empty, result.Item2);
                return RedirectToAction("Index", "Post");
            }

            return RedirectToAction("Index", "Post");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentVm c)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "You must login first" });
            }

            // استخدم UserName أو أي بروبرتي تانية بتحدد اليوزر
            c.UpdatedBy = user.UserName;

            ModelState.Remove("UpdatedBy");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = commentService.UpdateComment(c);
            if (!result.Item1)
            {
                return BadRequest(new { message = result.Item2 });
            }

            // بيرجع JSON عادي AJAX يفهمه
            return Json(new { success = true, message = "Comment updated successfully" });
        }

        [HttpPost]
        public IActionResult DeleteComment([FromForm] DeleteCommentVm comment)
        {
            var result = commentService.DeleteComment(comment);
            if (!result.Item1)
            {
                return Json(new { success = false, message = result.Item2 });
            }
            return Json(new { success = true });
        }


        [HttpGet]
        public IActionResult GetAllComments(int id ,int? limit = null )
        {
            var result = commentService.GetAllComment(id);
            if (!result.Item1)
            {
                throw new Exception(result.Item2);
            }
            return Json(result.Item3);

        }
    }
}
