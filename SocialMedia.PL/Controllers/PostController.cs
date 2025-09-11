using AutoMapper;

namespace SocialMedia.PL.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<User> userManager;  
        
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var profile = await service.GetProfile(user.Id.ToString());
            if (profile != null)
            {
                ViewBag.ProfileImage = profile.ProfileImagePath;
            }
            else
            {
                ViewBag.ProfileImage = "default-profile.jpg"; // صورة افتراضية
            }
            var (isSuccess, errorMessage, posts) = postService.GetPosts();
            if (!isSuccess)
            {
                ModelState.AddModelError("", errorMessage);
                posts = new List<PostVm>();
                
            }

            // رجع الموديل اللي يحتوي على CreateVm + List<PostVm>
            var viewModel = (
                new CreateVm(),
                posts,
                profile

            );

            return View(viewModel);
        }
        private readonly IPostService postService;
        private readonly IUserProfileService service;
        public PostController(IPostService postService, UserManager<User> userManager, IUserProfileService service)
        {
            this.postService = postService;
            this.userManager = userManager;
            this.service = service;
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddPost(CreateVm post)
        {
            var user = await userManager.GetUserAsync(User);
            var profile = await service.GetProfile(user.Id.ToString());
            post.UserId = user.Id;
            ModelState.Remove("UserId");
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
        [HttpPost]
        public async Task<IActionResult> SharePost(int postId, string content)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = postService.SharePost(postId, user.Id, content);

            if (!result.Item1)
            {
                TempData["Error"] = result.Item2;
            }
            else
            {
                TempData["Success"] = "Post shared successfully!";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var (isSuccess, errorMessage, posts) = postService.GetPosts();
            var profile = await service.GetProfile(user.Id.ToString());
            ViewBag.ProfileImage = profile.ProfileImagePath;

            if (!isSuccess)
            {
                ModelState.AddModelError("", errorMessage);
                posts = new List<PostVm>();
            }
            ViewData["ProfileImage"] = profile.ProfileImagePath;
            var viewModel = (
                
                posts,
                profile
                , new CreateVm()
            );

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSavedPosts()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var (isSuccess, ErrorMessage, posts) = postService.GetSavedPosts();
            var profile = await service.GetProfile(user.Id.ToString()); 

            if (isSuccess)
            {
                var viewModel = (
                    posts,
                    profile
                );
                return View(viewModel);
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
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Item2);
            return RedirectToAction("Index");
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
