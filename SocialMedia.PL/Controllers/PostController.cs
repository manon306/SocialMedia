using Microsoft.AspNetCore.Mvc;
using SocialMedia.BLL.ModelVM.Post;
using SocialMedia.BLL.Service.Abstraction;

namespace SocialMedia.PL.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IPostService postService;
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPost(CreateVm post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }
            var (isSuccess, errorMessage) = postService.AddPost(post);
            if (isSuccess)
            {
                return RedirectToAction("GetAllPosts");
            }
            ModelState.AddModelError(string.Empty, errorMessage);
            return View();
        }
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var (isSuccess, ErrorMessage ,posts) = postService.GetPosts();
            if (isSuccess)
            {
                return View(posts);
            }
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return View();

        }
        [HttpGet]
        public IActionResult UpdatePost()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeletePost()
        {
            return View();
        }

    }
}
