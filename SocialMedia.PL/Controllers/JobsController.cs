using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.BLL.ModelVM.Job;
using SocialMedia.DAL.Entity;
using AutoMapper;

namespace SocialMedia.PL.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobsService _jobsService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public JobsController(IJobsService jobsService, UserManager<User> userManager, IMapper mapper)
        {
            _jobsService = jobsService;
            _userManager = userManager;
            _mapper = mapper;
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

        // Admin-only actions
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != "Admin")
                return Forbid();

            return View(new CreateJobVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobVm model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != "Admin")
                return Forbid();

            if (ModelState.IsValid)
            {
                var job = new Job(model.Title, model.Company, model.Location, model.Description);
                await _jobsService.CreateAsync(job);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != "Admin")
                return Forbid();

            var job = await _jobsService.GetByIdAsync(id);
            if (job == null) return NotFound();

            var model = _mapper.Map<UpdateJobVm>(job);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateJobVm model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != "Admin")
                return Forbid();

            if (ModelState.IsValid)
            {
                var job = await _jobsService.GetByIdAsync(model.Id);
                if (job == null) return NotFound();

                job.Update(model.Title, model.Company, model.Location, model.Description);
                await _jobsService.UpdateAsync(job);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Role != "Admin")
                return Forbid();

            await _jobsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

