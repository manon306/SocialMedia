namespace SocialMedia.PL.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            var (isSuccess, errorMessage, posts) = postService.GetPosts();
            if (!isSuccess)
            {
                ModelState.AddModelError("", errorMessage);
                posts = new List<PostVm>();
            }

            // رجع الموديل اللي يحتوي على CreateVm + List<PostVm>
            var viewModel = new Tuple<CreateVm, List<PostVm>>(
                new CreateVm(),
                posts
            );

            return View(viewModel);
        }
        private readonly IPostService postService;
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View("Index");
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

            return RedirectToAction("Index"); 
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
        public IActionResult UpdatePost(int id)
        {
            var result = postService.GetById(id);
            if (!result.Item1)
            {
                return NotFound(result.Item2);
            }

            var post = result.Item3;

            // نحول الـ Entity لـ ViewModel
            var model = new updatePostVm
            {
                ID = post.ID,
                Content = post.Content,
                Image = post.Image,
                Videos = post.Videos,
                UpdatedBy = "Menna" 
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult UpdatePost(updatePostVm post)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();

                //return View(post); 
            }
            var result = postService.UpdatePost(post);
            if (result.Item1)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return View(post); 
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
        [HttpPost]
        public IActionResult ToggleSavedPosts(int id)
        {
            var result = postService.toggleSaved(id);
            if (result.Item1 == true)
            {
                ViewBag.Message = "Post Saved Successfully";
                return RedirectToAction("GetAllSavedPosts");
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return RedirectToAction("GetAllSavedPosts");
        }
        [HttpPost]
        public IActionResult ToggleArchivePosts(int id)
        {
            var result = postService.toggleArchive(id);
            if (result.Item1 == true)
            {
                ViewBag.Message = "Post Saved Successfully";
                return RedirectToAction("GetAllArchivedPosts");
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return RedirectToAction("GetAllArchivedPosts");
        }
        
    }
}
