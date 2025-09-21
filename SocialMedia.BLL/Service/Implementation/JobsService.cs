namespace SocialMedia.BLL.Service.Implementation
{
    public class JobsService : IJobsService
    {
        private readonly IJobsRepo _jobsRepo;
        private readonly IMapper _mapper;
        public JobsService(IJobsRepo jobsRepo , IMapper mapper)
        {
            _jobsRepo = jobsRepo;
            _mapper = mapper;
        }

        public async Task<List<Jobvm>> GetAllAsync()
        {
            var jobs = await _jobsRepo.GetAllAsync();
            return _mapper.Map<List<Jobvm>>(jobs);
        }
        public async Task<Jobvm?> GetByIdAsync(int id)
        {
            var job = await _jobsRepo.GetByIdAsync(id);
            return _mapper.Map<Jobvm?>(job);
        }

        public Task ToggleSaveAsync(int id)
        {
            return _jobsRepo.ToggleSaveAsync(id);
        }

        public Task UpdateReviewAsync(int id, string? review)
        {
            return _jobsRepo.UpdateReviewAsync(id, review);
        }

        public async Task<List<Jobvm>> GetSavedAsync()
        {
            var job= await _jobsRepo.GetSavedAsync();
            return _mapper.Map<List<Jobvm>>(job);
        }
        public async Task AddAsync(Jobvm jobVm)
        {
            var job =  _mapper.Map<DAL.Entity.Job>(jobVm); // ✨ هنا بنحول من VM → Entity
            await _jobsRepo.AddAsync(job);
        }

        public Task DeleteAsync(int id)
        {
            return _jobsRepo.DeleteAsync(id);
        }
    }
}