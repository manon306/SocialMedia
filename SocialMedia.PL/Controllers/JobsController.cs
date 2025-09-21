
using SocialMedia.BLL.ModelVM.job;

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

        [HttpPost]
        public async Task<IActionResult> UpdateReview(int id, string? review)
        {
            await _jobsService.UpdateReviewAsync(id, review);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Jobvm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Jobvm jobVm)
        {
            if (!ModelState.IsValid)
            {
                return View(jobVm);
            }

            var job = new Job
            (
                jobVm.Title,
                jobVm.Description,
                jobVm.Company,
                jobVm.Location
            );

            await _jobsService.AddAsync(jobVm);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _jobsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
