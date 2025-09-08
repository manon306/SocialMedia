using SocialMedia.BLL.ModelVM.Comment;

namespace SocialMedia.PL.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddComment(AddCommentVm cm)
        {
            cm.CreatedBy = "Menna";
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
