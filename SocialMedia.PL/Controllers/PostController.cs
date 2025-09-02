using Microsoft.AspNetCore.Mvc;
using SocialMedia.BLL.ModelVM.Post;
using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.BLL.Service.Implementation;

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
                var (isSuccess, errorMessage, posts) = postService.GetPosts();
                return View(posts);
            }

            var (isSuccessAdd, errorMessageAdd) = postService.AddPost(post);
            if (!isSuccessAdd)
            {
                ModelState.AddModelError(string.Empty, errorMessageAdd);
            }

            return RedirectToAction("AddPost"); 
        }
        [HttpGet]
        public IActionResult GetAllSavedPosts()
        {
            var (isSuccess, ErrorMessage ,posts) = postService.GetSavedPosts();
            if (isSuccess)
            {
                return View(posts);
            }
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return View();
        }
        [HttpGet]
        public IActionResult GetAllArchivedPosts()
        {
            var (isSuccess, ErrorMessage, posts) = postService.GetArchivedPosts();
            if (isSuccess)
            {
                return View(posts);
            }
            ModelState.AddModelError(string.Empty, ErrorMessage);
            return View();
        }
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var (isSuccess, ErrorMessage, posts) = postService.GetPosts();
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
        [HttpPost]
        public IActionResult DeletePost( int id)
        {
            var deletedBy = "Menna"; 
            var result = postService.DeletePost(id,deletedBy);
            if(result.Item1 == true)
            {
                ViewBag.Message = "Post Deleted Successfully";
                return RedirectToAction("GetAllPosts");
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return RedirectToAction("GetAllPosts");
        }


    }
}
