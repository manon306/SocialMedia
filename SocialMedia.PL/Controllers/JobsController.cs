namespace SocialMedia.PL.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobsService _jobsService;

        public JobsController(IJobsService jobsService)
        {
            _jobsService = jobsService;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = await _jobsService.GetAllAsync();
            return View(jobs);
        }

        public async Task<IActionResult> Saved()
        {
            var jobs = await _jobsService.GetSavedAsync();
            return View(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSave(int id)
        {
            await _jobsService.ToggleSaveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var job = await _jobsService.GetByIdAsync(id);
            if (job == null) return NotFound();
            return View(job);
        }
    }
}

