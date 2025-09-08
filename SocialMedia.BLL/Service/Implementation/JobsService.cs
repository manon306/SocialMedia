using SocialMedia.BLL.Service.Abstraction;
using SocialMedia.DAL.Entity;
using SocialMedia.DAL.REPO.Abstraction;

namespace SocialMedia.BLL.Service.Implementation
{
    public class JobsService : IJobsService
    {
        private readonly IJobsRepo _jobsRepo;

        public JobsService(IJobsRepo jobsRepo)
        {
            _jobsRepo = jobsRepo;
        }

        public Task<List<Job>> GetAllAsync()
        {
            return _jobsRepo.GetAllAsync();
        }

        public Task<Job?> GetByIdAsync(int id)
        {
            return _jobsRepo.GetByIdAsync(id);
        }

        public Task ToggleSaveAsync(int id)
        {
            return _jobsRepo.ToggleSaveAsync(id);
        }

        public Task UpdateReviewAsync(int id, string? review)
        {
            return _jobsRepo.UpdateReviewAsync(id, review);
        }

        public Task<List<Job>> GetSavedAsync()
        {
            return _jobsRepo.GetSavedAsync();
        }
    }
}

